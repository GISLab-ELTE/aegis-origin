/// <copyright file="TiffWriter.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.GeoTiff
{
    // short expression for the IFD table
    using TiffImageFileDirectory = Dictionary<UInt16, Object[]>;

    /// <summary>
    /// Represents a TIFF file format writer.
    /// </summary>
    /// <remarks>
    /// TIFF (originally standing for Tagged Image File Format) is a tag-based file format for storing and interchanging raster images.
    /// The general TIFF format does not support georeferencing, hence only raster information can be written to the file.
    /// </remarks>
    [IdentifiedObjectInstance("AEGIS::610201", "Tagged Image File Format")]
    public class TiffWriter : GeometryStreamWriter
    {
        #region Private constant fields

        /// <summary>
        /// The maximum number of bytes to be written into the file in a single operation. This field is constant.
        /// </summary>
        private const Int32 NumberOfWritableBytes = 1 << 20;
        
        /// <summary>
        /// The threshold value (in bytes) after which BigTIFF format is applied. This field is constant.
        /// </summary>
        private const Int64 BigTiffThreshold = (Int64)(UInt32.MaxValue * 0.9);

        #endregion

        #region Private fields

        /// <summary>
        /// Indicates whether the file header was already written.
        /// </summary>
        private Boolean _fileHeaderWritten;

        /// <summary>
        /// The starting position of the current image file directory.
        /// </summary>
        private Int64 _currentImageFileDirectoryStartPosition;

        /// <summary>
        /// The ending position of the current image file directory.
        /// </summary>
        private Int64 _currentImageFileDirectoryEndPosition;

        /// <summary>
        /// The starting position of the current image.
        /// </summary>
        private Int64 _currentImageStartPosition;

        /// <summary>
        /// Indicates whether the contnet should be written in BigTIFF format.
        /// </summary>
        private Boolean _isBigTiffFormat;

        /// <summary>
        /// The size of an entry in the image file directory.
        /// </summary>
        private Int32 _imageFileDirectoryEntrySize;

        /// <summary>
        /// The size of a field in the image file directory.
        /// </summary>
        private Int32 _imageFileDirectoryFieldSize;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public TiffWriter(String path)
            : base(path, SpectralGeometryStreamFormats.Tiff, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public TiffWriter(Uri path)
            : base(path, SpectralGeometryStreamFormats.Tiff, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to be written.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        public TiffWriter(Stream stream)
            : base(stream, SpectralGeometryStreamFormats.Tiff, null)
        {
        }

        #endregion

        #region GeometryStreamWriter protected methods

        /// <summary>
        /// Apply the write operation for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentException">Stream does not support the specified geometry.;geometry</exception>
        protected override void ApplyWriteGeometry(IGeometry geometry)
        {            
            IRaster raster = (geometry as ISpectralGeometry).Raster;

            if (raster == null)
                return;

            // TODO: compute the format more exactly
            if (!_isBigTiffFormat)
                _isBigTiffFormat = (Int64)raster.RadiometricResolutions.Sum() / 8 * raster.NumberOfBands * raster.NumberOfColumns * raster.NumberOfRows > BigTiffThreshold;

            _imageFileDirectoryEntrySize = _isBigTiffFormat ? 20 : 12;
            _imageFileDirectoryFieldSize = _isBigTiffFormat ? 8 : 4;

            WriteHeader();

            TiffCompression compression = TiffCompression.None; // TODO: support other compressions
            TiffSampleFormat sampleFormat = (geometry as ISpectralGeometry).Raster.Format == RasterFormat.Floating ? TiffSampleFormat.Floating : TiffSampleFormat.UnsignedInteger;

            TiffImageFileDirectory imageFileDirectory = ComputeImageFileDirectory(geometry as ISpectralGeometry, compression, sampleFormat);

            // compute and update raster content position
            Int64 imageFileDirectorySize = ComputeImageFileDirectorySize(imageFileDirectory);
            Int64 rasterContentStartPosition = _baseStream.Position + imageFileDirectorySize;
            Int64 rasterContentSize = ComputeRasterContentSize(raster);

            if (_isBigTiffFormat) // update strip offset and length
            {
                imageFileDirectory[273][0] = (UInt64)rasterContentStartPosition;
                imageFileDirectory[279][0] = (UInt64)rasterContentSize;
            }
            else
            {
                imageFileDirectory[273][0] = (UInt32)rasterContentStartPosition;
                imageFileDirectory[279][0] = (UInt32)rasterContentSize;
            }

            // write image file directory
            if (_isBigTiffFormat)
                WriteBigImageFileDirectory(imageFileDirectory);
            else
                WriteImageFileDirectory(imageFileDirectory);

            // perform writing based on representation
            WriteRasterContentToStrip((geometry as ISpectralGeometry).Raster, compression, sampleFormat);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            // write the 0 address of next image file directory
            _baseStream.Seek(_currentImageFileDirectoryEndPosition, SeekOrigin.Begin);
            if (_isBigTiffFormat)
            {
                _baseStream.Write(EndianBitConverter.GetBytes((UInt64)0), 0, 8);
            }
            else
            {
                _baseStream.Write(EndianBitConverter.GetBytes((UInt32)0), 0, 4);
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the image file directory of a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="compression">The compression.</param>
        /// <param name="format">The sample format.</param>
        /// <param name="startPosition">The starting position of the raster content within the stream.</param>
        /// <param name="endPosition">The ending position of the raster content within the stream.</param>
        /// <returns>The computed image file directory.</returns>
        protected virtual TiffImageFileDirectory ComputeImageFileDirectory(ISpectralGeometry geometry, TiffCompression compression, TiffSampleFormat format)
        {
            TiffImageFileDirectory imageFileDirectory = new TiffImageFileDirectory();

            // compute photometric interpretation
            TiffPhotometricInterpretation photometricInterpretation = ComputePhotometricInterpretation(geometry);

            // add raster properties
            imageFileDirectory.Add(262, new Object[] { (UInt16)photometricInterpretation }); // photometric interpretation
            imageFileDirectory.Add(259, new Object[] { (UInt16)compression }); // compression
            imageFileDirectory.Add(257, new Object[] { (UInt32)geometry.Raster.NumberOfRows }); // image length
            imageFileDirectory.Add(256, new Object[] { (UInt32)geometry.Raster.NumberOfColumns }); // image width
            imageFileDirectory.Add(296, new Object[] { (UInt16)2 }); // resolution unit
            imageFileDirectory.Add(282, new Object[] { (geometry.Raster.Mapper != null) ? (Rational)geometry.Raster.Mapper.ColumnSize : (Rational)1 }); // x resolution
            imageFileDirectory.Add(283, new Object[] { (geometry.Raster.Mapper != null) ? (Rational)geometry.Raster.Mapper.RowSize : (Rational)1 }); // y resolution
            imageFileDirectory.Add(278, new Object[] { (UInt32)geometry.Raster.NumberOfRows }); // rows per strip
            imageFileDirectory.Add(273, new Object[] { (UInt32)0 }); // strip offsets (will be updated)
            imageFileDirectory.Add(279, new Object[] { (UInt32)0 }); // strip byte counts (will be updated)
            imageFileDirectory.Add(258, geometry.Raster.RadiometricResolutions.Select(resolution => (UInt16)resolution).Cast<Object>().ToArray()); // bits per sample
            imageFileDirectory.Add(277, new Object[] { (UInt32)geometry.Raster.NumberOfBands }); // samples per pixel
            imageFileDirectory.Add(339, new Object[] { (UInt16)format }); // sample format

            // add color palette
            if (photometricInterpretation == TiffPhotometricInterpretation.PaletteColor)
            {
                Object[] colorMap = new Object[3 * Calculator.Pow(2, geometry.Raster.RadiometricResolutions[0])];

                for (Int32 valueIndex = 0; valueIndex < colorMap.Length / 3; valueIndex++)
                {
                    if (!geometry.Presentation.ColorMap.ContainsKey(valueIndex))
                        continue;

                    colorMap[3 * valueIndex] = geometry.Presentation.ColorMap[valueIndex][0];
                    colorMap[3 * valueIndex + 1] = geometry.Presentation.ColorMap[valueIndex][1];
                    colorMap[3 * valueIndex + 2] = geometry.Presentation.ColorMap[valueIndex][2];
                }

                imageFileDirectory.Add(320, colorMap);
            }

            // add metadata
            imageFileDirectory.Add(305, new Object[] { "AEGIS Spatio-Temporal Framework" }); // software
            imageFileDirectory.Add(306, new Object[] { DateTime.UtcNow.ToString(CultureInfo.InvariantCulture.DateTimeFormat) }); // date time

            if (geometry.Metadata.ContainsKey("DocumentName"))
                imageFileDirectory.Add(269, new Object[] { geometry.Metadata["DocumentName"] });
            if (geometry.Metadata.ContainsKey("ImageDescription"))
                imageFileDirectory.Add(270, new Object[] { geometry.Metadata["ImageDescription"] });
            if (geometry.Metadata.ContainsKey("Make"))
                imageFileDirectory.Add(271, new Object[] { geometry.Metadata["Make"] });
            if (geometry.Metadata.ContainsKey("Model"))
                imageFileDirectory.Add(272, new Object[] { geometry.Metadata["Model"] });
            if (geometry.Metadata.ContainsKey("PageName"))
                imageFileDirectory.Add(285, new Object[] { geometry.Metadata["PageName"] });
            if (geometry.Metadata.ContainsKey("Artist"))
                imageFileDirectory.Add(307, new Object[] { geometry.Metadata["Artist"] });
            if (geometry.Metadata.ContainsKey("HostComputer"))
                imageFileDirectory.Add(308, new Object[] { geometry.Metadata["HostComputer"] });
            if (geometry.Metadata.ContainsKey("Copyright"))
                imageFileDirectory.Add(33432, new Object[] { geometry.Metadata["Copyright"] });

            return imageFileDirectory;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Writes the header.
        /// </summary>
        private void WriteHeader()
        {
            if (_fileHeaderWritten)
                return;

            Byte[] bytes = _isBigTiffFormat ? new Byte[8] : new Byte[4];

            _baseStream.Seek(0, SeekOrigin.Begin);
            switch (EndianBitConverter.DefaultByteOrder) // the default endianness should be used for the encoding
            { 
                case ByteOrder.LittleEndian:
                    EndianBitConverter.CopyBytes((UInt16)0x4949, bytes, 0);
                    break;
                case ByteOrder.BigEndian:
                    EndianBitConverter.CopyBytes((UInt16)0x4D4D, bytes, 0);
                    break;
            }

            if (_isBigTiffFormat)
            {
                EndianBitConverter.CopyBytes((UInt16)43, bytes, 2); // BigTIFF identifier
                EndianBitConverter.CopyBytes((UInt16)8, bytes, 4); // BigTIFF field size
                EndianBitConverter.CopyBytes((UInt16)0, bytes, 6);
                _baseStream.Write(bytes, 0, bytes.Length);
                _baseStream.Seek(16, SeekOrigin.Begin);

                _currentImageFileDirectoryEndPosition = 8;
            }
            else
            {
                EndianBitConverter.CopyBytes((UInt16)42, bytes, 2); // TIFF identifier
                _baseStream.Write(bytes, 0, bytes.Length);
                _baseStream.Seek(8, SeekOrigin.Begin);

                _currentImageFileDirectoryEndPosition = 4;
            }
            _fileHeaderWritten = true;
        }

        /// <summary>
        /// Writes an image file directory to the stream (in BigTIFF format).
        /// </summary>
        /// <param name="imageFileDirectory">The image file directory.</param>
        private void WriteBigImageFileDirectory(TiffImageFileDirectory imageFileDirectory)
        {
            _currentImageFileDirectoryStartPosition = _baseStream.Position;

            // write starting position
            _baseStream.Seek(_currentImageFileDirectoryEndPosition, SeekOrigin.Begin);
            _baseStream.Write(EndianBitConverter.GetBytes((UInt64)_currentImageFileDirectoryStartPosition), 0, _imageFileDirectoryFieldSize);

            // compute size of directory (without additional fields)
            Int64 imageFileDirectorySize = 8 + _imageFileDirectoryEntrySize * imageFileDirectory.Count;
            _currentImageFileDirectoryEndPosition = _currentImageFileDirectoryStartPosition + imageFileDirectorySize;

            // position after the IFD to write exceeding values
            _baseStream.Seek(_currentImageFileDirectoryEndPosition + _imageFileDirectoryFieldSize, SeekOrigin.Begin);

            // the IFD should be written in one operation
            Byte[] bytes = new Byte[imageFileDirectorySize];
            EndianBitConverter.CopyBytes((UInt64)imageFileDirectory.Count, bytes, 0); // number of entries
            Int32 byteIndex = 8;

            TiffTagType entryType;
            UInt16 dataSize;
            Int64 dataCount;

            foreach (UInt16 entryTag in imageFileDirectory.Keys)
            {
                entryType = GetTagType(imageFileDirectory[entryTag][0]);
                dataSize = GetTagSize(entryType);

                if (entryType == TiffTagType.ASCII)
                {
                    dataCount = 0;
                    for (Int32 i = 0; i < imageFileDirectory[entryTag].Length; i++)
                    {
                        dataCount += (imageFileDirectory[entryTag][i] as String).Length;
                    }
                }
                else
                {
                    dataCount = imageFileDirectory[entryTag].Length;
                }

                EndianBitConverter.CopyBytes(entryTag, bytes, byteIndex);
                EndianBitConverter.CopyBytes((UInt16)entryType, bytes, byteIndex + 2);
                EndianBitConverter.CopyBytes((UInt64)dataCount, bytes, byteIndex + 4);

                // values exceeding he field size (8) must be written to another position
                Byte[] dataBytes;
                Int32 dataStartIndex;
                if (dataCount * dataSize <= 8)
                {
                    dataBytes = bytes;
                    dataStartIndex = byteIndex + 12;
                }
                else
                {
                    dataBytes = new Byte[dataCount * dataSize + (dataCount * dataSize) % 2];
                    dataStartIndex = 0;
                }

                for (Int32 valueIndex = 0; valueIndex < imageFileDirectory[entryTag].Length; valueIndex++)
                {
                    dataStartIndex = SetTagValue(entryType, imageFileDirectory[entryTag][valueIndex], dataBytes, dataStartIndex);
                }

                if (dataCount * dataSize > 8)
                {
                    Int64 valuePosition = _baseStream.Position;
                    _baseStream.Write(dataBytes, 0, dataBytes.Length);

                    EndianBitConverter.CopyBytes((UInt64)valuePosition, bytes, byteIndex + 12);
                }
                byteIndex += _imageFileDirectoryEntrySize;
            }

            _currentImageStartPosition = _baseStream.Position;

            // write the IFD
            _baseStream.Seek(_currentImageFileDirectoryStartPosition, SeekOrigin.Begin);
            _baseStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes an image file directory to the stream.
        /// </summary>
        /// <param name="imageFileDirectory">The image file directory.</param>
        private void WriteImageFileDirectory(TiffImageFileDirectory imageFileDirectory)
        {
            _currentImageFileDirectoryStartPosition = _baseStream.Position;

            // write starting position
            _baseStream.Seek(_currentImageFileDirectoryEndPosition, SeekOrigin.Begin);
            _baseStream.Write(EndianBitConverter.GetBytes((UInt32)_currentImageFileDirectoryStartPosition), 0, _imageFileDirectoryFieldSize);

            // compute size of directory (without additional fields)
            Int64 imageFileDirectorySize = 2 + _imageFileDirectoryEntrySize * imageFileDirectory.Count;
            _currentImageFileDirectoryEndPosition = _currentImageFileDirectoryStartPosition + imageFileDirectorySize;

            // position after the IFD to write exceeding values
            _baseStream.Seek(_currentImageFileDirectoryEndPosition + _imageFileDirectoryFieldSize, SeekOrigin.Begin);

            // the IFD should be written in one operation
            Byte[] bytes = new Byte[imageFileDirectorySize];
            EndianBitConverter.CopyBytes((UInt16)imageFileDirectory.Count, bytes, 0); // number of entries
            Int32 byteIndex = 2;

            TiffTagType entryType;
            UInt16 dataSize;
            Int64 dataCount;

            foreach (UInt16 entryTag in imageFileDirectory.Keys)
            {
                entryType = GetTagType(imageFileDirectory[entryTag][0]);
                dataSize = GetTagSize(entryType);

                if (entryType == TiffTagType.ASCII)
                {
                    dataCount = 0;
                    for (Int32 i = 0; i < imageFileDirectory[entryTag].Length; i++)
                    {
                        dataCount += (imageFileDirectory[entryTag][i] as String).Length;
                    }
                }
                else
                {
                    dataCount = imageFileDirectory[entryTag].Length;
                }

                EndianBitConverter.CopyBytes(entryTag, bytes, byteIndex);
                EndianBitConverter.CopyBytes((UInt16)entryType, bytes, byteIndex + 2);
                EndianBitConverter.CopyBytes((UInt32)dataCount, bytes, byteIndex + 4);

                // values exceeding he field size (4) must be written to another position
                Byte[] dataBytes;
                Int32 dataStartIndex;
                if (dataCount * dataSize <= 4)
                {
                    dataBytes = bytes;
                    dataStartIndex = byteIndex + 8;
                }
                else
                {
                    dataBytes = new Byte[dataCount * dataSize + (dataCount * dataSize) % 2];
                    dataStartIndex = 0;
                }

                for (Int32 valueIndex = 0; valueIndex < imageFileDirectory[entryTag].Length; valueIndex++)
                {
                    dataStartIndex = SetTagValue(entryType, imageFileDirectory[entryTag][valueIndex], dataBytes, dataStartIndex);
                }

                if (dataCount * dataSize > 4)
                {
                    Int64 valuePosition = _baseStream.Position;
                    _baseStream.Write(dataBytes, 0, dataBytes.Length);

                    EndianBitConverter.CopyBytes((UInt32)valuePosition, bytes, byteIndex + 8);
                }

                byteIndex += _imageFileDirectoryEntrySize;
            }

            _currentImageStartPosition = _baseStream.Position;

            // write the IFD
            _baseStream.Seek(_currentImageFileDirectoryStartPosition, SeekOrigin.Begin);
            _baseStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Computes the size of the image file directory.
        /// </summary>
        /// <param name="imageFileDirectory">The image file directory.</param>
        /// <returns>The size of the image file directory (in bytes).</returns>
        private Int64 ComputeImageFileDirectorySize(TiffImageFileDirectory imageFileDirectory)
        {
            Int64 size = (_isBigTiffFormat ? 8 : 2) // number of entries
                       + _imageFileDirectoryEntrySize * imageFileDirectory.Count  // entries
                       + _imageFileDirectoryFieldSize; // position of next directory

            TiffTagType entryType;
            UInt16 dataSize;
            Int32 dataCount;

            foreach (UInt16 entryTag in imageFileDirectory.Keys)
            {
                entryType = GetTagType(imageFileDirectory[entryTag][0]);
                dataSize = GetTagSize(entryType);

                if (entryType == TiffTagType.ASCII)
                {
                    dataCount = 0;
                    for (Int32 i = 0; i < imageFileDirectory[entryTag].Length; i++)
                    {
                        dataCount += (imageFileDirectory[entryTag][i] as String).Length;
                    }
                }
                else
                {
                    dataCount = imageFileDirectory[entryTag].Length;
                }

                if (dataCount * dataSize > _imageFileDirectoryFieldSize)
                {
                    size += dataCount * dataSize + (dataCount * dataSize) % 2;
                }
            }

            return size;
        }

        /// <summary>
        /// Computes the size of the raster content.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <returns>The size of the raster content (in bytes).</returns>
        private Int64 ComputeRasterContentSize(IRaster raster)
        {
            Int64 rasterContentSize = (Int64)Math.Ceiling(raster.RadiometricResolutions.Max() / 8.0) * raster.NumberOfBands * raster.NumberOfRows * raster.NumberOfColumns;

            if (rasterContentSize % 2 != 0) // correct the number of bytes
                rasterContentSize++;

            return rasterContentSize;
        }

        /// <summary>
        /// Computes the photometric interpretation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The photometric interpretation of the geometry.</returns>
        private TiffPhotometricInterpretation ComputePhotometricInterpretation(ISpectralGeometry geometry)
        { 
            if (geometry.Presentation == null)
            {
                if (geometry.Raster.NumberOfBands == 3)
                    return TiffPhotometricInterpretation.RGB;

                return TiffPhotometricInterpretation.BlackIsZero;
            }

            switch (geometry.Presentation.Model)
            { 
                case RasterPresentationModel.Grayscale:
                    return TiffPhotometricInterpretation.BlackIsZero;
                case RasterPresentationModel.InvertedGrayscale:
                    return TiffPhotometricInterpretation.WhiteIsZero;
                case RasterPresentationModel.Transparency:
                    return TiffPhotometricInterpretation.TransparencyMask;
                case RasterPresentationModel.TrueColor:
                case RasterPresentationModel.FalseColor:
                    switch (geometry.Presentation.ColorSpace)
                    { 
                        case RasterColorSpace.RGB:
                            return TiffPhotometricInterpretation.RGB;
                        case RasterColorSpace.CIELab:
                            return TiffPhotometricInterpretation.CIELab;
                        case RasterColorSpace.CMYK:
                            return TiffPhotometricInterpretation.CMYK;
                        case RasterColorSpace.YCbCr:
                            return TiffPhotometricInterpretation.YCbCr;
                        default:
                            throw new InvalidDataException("The specified presentation is not supported.");
                    }
                case RasterPresentationModel.PseudoColor:
                    return TiffPhotometricInterpretation.PaletteColor;
                default:
                    throw new InvalidDataException("The specified presentation is not supported.");
            }
            // TODO: process other presentations
        }

        /// <summary>
        /// Writes the raster of the geometry into a strip.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="compression">The compression.</param>
        /// <param name="format">The sample format.</param>
        private void WriteRasterContentToStrip(IRaster raster, TiffCompression compression, TiffSampleFormat format)
        {
            _baseStream.Seek(_currentImageStartPosition, SeekOrigin.Begin);

            // mark the starting position of the strip
            UInt32 numberOfBytes = (UInt32)(Math.Ceiling(raster.RadiometricResolutions.Max() / 8.0) * raster.NumberOfBands * raster.NumberOfRows * raster.NumberOfColumns);
            UInt32 numberOfBytesLeft = numberOfBytes;

            if (numberOfBytes % 2 != 0) // correct the number of bytes
                numberOfBytes++;

            Byte[] bytes = new Byte[numberOfBytes < NumberOfWritableBytes ? numberOfBytes : NumberOfWritableBytes];
            Int32 byteIndex = 0;
            Int32 bitIndex = 8;

            for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                {
                    // write the values for each band into the buffer
                    for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                    {
                        switch (format)
                        {
                            case TiffSampleFormat.Undefined:
                            case TiffSampleFormat.UnsignedInteger:
                                WriteUnsignedIntergerValue(raster.GetValue(rowIndex, columnIndex, bandIndex), raster.RadiometricResolutions[bandIndex], bytes, ref byteIndex, ref bitIndex);
                                break;
                            case TiffSampleFormat.SignedInteger:
                            case TiffSampleFormat.Floating:
                                WriteFloatValue(raster.GetFloatValue(rowIndex, columnIndex, bandIndex), raster.RadiometricResolutions[bandIndex], bytes, ref byteIndex, ref bitIndex);
                                break;
                        }

                        if (byteIndex == bytes.Length)
                        {
                            // write the buffer to the file
                            _baseStream.Write(bytes, 0, byteIndex);

                            byteIndex = 0;

                            numberOfBytesLeft -= (UInt32)byteIndex;
                            // the final array of bytes should not be the number of bytes left
                            if (numberOfBytes > NumberOfWritableBytes && numberOfBytesLeft > 0 && numberOfBytesLeft < NumberOfWritableBytes)
                                bytes = new Byte[numberOfBytesLeft % 2 == 0 ? numberOfBytesLeft : numberOfBytesLeft + 1];
                        }
                    }
                }

            // if any values are left
            if (numberOfBytesLeft > 0)
            {
                _baseStream.Write(bytes, 0, byteIndex);
            }
        }

        /// <summary>
        /// Writes the specified unsigned interger value to the array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="radiometricResolution">The radiometric resolution of the value.</param>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the array.</param>
        /// <param name="bitIndex">The zero-based bit index in the array.</param>
        /// <exception cref="System.NotSupportedException">Radiometric resolution is not supported.</exception>
        private void WriteUnsignedIntergerValue(UInt32 value, Int32 radiometricResolution, Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex)
        {
            if (radiometricResolution == 8)
            {
                bytes[byteIndex] = (Byte)value;
                byteIndex++;
            }
            else if (radiometricResolution == 16)
            {
                EndianBitConverter.CopyBytes((UInt16)value, bytes, byteIndex);
                byteIndex += 2;
            }
            else if (radiometricResolution == 32)
            {
                EndianBitConverter.CopyBytes(value, bytes, byteIndex);
                byteIndex += 4;
            }
            else if (radiometricResolution < 8 && 8 % radiometricResolution == 0)
            {
                bitIndex -= radiometricResolution;
                bytes[byteIndex] += (Byte)(value << bitIndex);

                if (bitIndex == 0)
                {
                    bitIndex = 8;
                    byteIndex++;
                }
            }
            else 
            {
                // TODO: support other resolutions
                throw new NotSupportedException("Radiometric resolution is not supported.");
            }
        }

        /// <summary>
        /// Writes the specified floating point value to the array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="radiometricResolution">The radiometric resolution of the value.</param>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the array.</param>
        /// <param name="bitIndex">The zero-based bit index in the array.</param>
        /// <exception cref="System.NotSupportedException">Radiometric resolution is not supported.</exception>
        private void WriteFloatValue(Double value, Int32 radiometricResolution, Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex)
        {
            if (radiometricResolution == 32)
            {
                EndianBitConverter.CopyBytes((Single)value, bytes, byteIndex);
                byteIndex += 4;
            }
            else if (radiometricResolution == 64)
            {
                EndianBitConverter.CopyBytes(value, bytes, byteIndex);
                byteIndex += 8;
            }
            else 
            {
                // TODO: support other resolutions
                throw new NotSupportedException("Radiometric resolution is not supported.");
            }
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Gets the TIFF tag type of a value.
        /// </summary>
        /// <param name="value">The value of the tag.</param>
        private static TiffTagType GetTagType(Object value)
        {
            if (value is Byte)
                return TiffTagType.Byte;
            if (value is Char || value is String)
                return TiffTagType.ASCII;
            if (value is UInt16)
                return TiffTagType.Short;
            if (value is UInt32)
                return TiffTagType.Long;
            if (value is SByte)
                return TiffTagType.SByte;
            if (value is Int16)
                return TiffTagType.SShort;
            if (value is Int32)
                return TiffTagType.SLong;
            if (value is Rational)
                return TiffTagType.SRational; // missing comparision for Rational
            if (value is Single)
                return TiffTagType.Float;
            if (value is Double)
                return TiffTagType.Double;
            if (value is UInt64)
                return TiffTagType.Long8;
            if (value is Int64)
                return TiffTagType.SLong8;

            return TiffTagType.Undefined;
        }
        /// <summary>
        /// Returns the value size for a specified tag type.
        /// </summary>
        /// <param name="type">The type of the tag.</param>
        /// <returns>The size of the value in bytes.</returns>
        private static UInt16 GetTagSize(TiffTagType type)
        {
            switch (type)
            {
                case TiffTagType.Byte:
                case TiffTagType.SByte:
                case TiffTagType.ASCII:
                case TiffTagType.Undefined:
                    return 1;
                case TiffTagType.Short:
                case TiffTagType.SShort:
                    return 2;
                case TiffTagType.Long:
                case TiffTagType.SLong:
                case TiffTagType.Float:
                    return 4;
                case TiffTagType.Double:
                case TiffTagType.Rational:
                case TiffTagType.SRational:
                case TiffTagType.Long8:
                case TiffTagType.SLong8:
                case TiffTagType.LongOffset:
                    return 8;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Sets the value of a TIFF tag.
        /// </summary>
        /// <param name="type">The type of the tag.</param>
        /// <param name="value">The value.</param>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The zero-based start index.</param>
        /// <returns>The index of the array after the operation.</returns>
        private static Int32 SetTagValue(TiffTagType type, Object value, Byte[] array, Int32 startIndex)
        {
            Int32 dataSize = GetTagSize(type);

            switch (type)
            {
                case TiffTagType.Byte:
                    EndianBitConverter.CopyBytes(Convert.ToByte(value), array, startIndex);
                    break;
                case TiffTagType.ASCII:
                    Byte[] asciiValues = System.Text.Encoding.ASCII.GetBytes(value as String);
                    Array.Copy(asciiValues, 0, array, startIndex, asciiValues.Length);
                    return startIndex + asciiValues.Length;
                case TiffTagType.Short:
                    EndianBitConverter.CopyBytes(Convert.ToUInt16(value), array, startIndex);
                    break;
                case TiffTagType.Long:
                    EndianBitConverter.CopyBytes(Convert.ToUInt32(value), array, startIndex);
                    break;
                case TiffTagType.SByte:
                    EndianBitConverter.CopyBytes(Convert.ToSByte(value), array, startIndex);
                    break;
                case TiffTagType.SShort:
                    EndianBitConverter.CopyBytes(Convert.ToInt16(value), array, startIndex);
                    break;
                case TiffTagType.Rational:
                    EndianBitConverter.CopyBytes((Rational)value, array, startIndex);
                    break;
                case TiffTagType.SRational:
                    EndianBitConverter.CopyBytes((Rational)value, array, startIndex);
                    break;
                case TiffTagType.SLong:
                    EndianBitConverter.CopyBytes(Convert.ToInt32(value), array, startIndex);
                    break;
                case TiffTagType.Float:
                    EndianBitConverter.CopyBytes(Convert.ToSingle(value), array, startIndex);
                    break;
                case TiffTagType.Double:
                    EndianBitConverter.CopyBytes(Convert.ToDouble(value), array, startIndex);
                    break;
                case TiffTagType.Long8:
                case TiffTagType.LongOffset:
                    EndianBitConverter.CopyBytes(Convert.ToUInt64(value), array, startIndex);
                    break;
                case TiffTagType.SLong8:
                    EndianBitConverter.CopyBytes(Convert.ToInt64(value), array, startIndex);
                    break;
            }
            return startIndex + dataSize;
        }

        #endregion
    }
}
