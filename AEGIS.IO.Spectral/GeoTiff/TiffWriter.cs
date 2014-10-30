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
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.GeoTiff
{
    using System.Globalization;
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

        #endregion

        #region Private fields

        private Boolean _fileHeaderWritten;
        private Int32 _currentImageFileDirectoryStartPosition;
        private Int32 _currentImageFileDirectoryEndPosition;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter"/> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream opening.</exception>
        public TiffWriter(String path)
            : base(path, SpectralGeometryStreamFormats.Tiff, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter"/> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream opening.</exception>
        public TiffWriter(Uri path)
            : base(path, SpectralGeometryStreamFormats.Tiff, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter"/> class.
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
            if ((geometry as ISpectralGeometry).Raster == null)
                return;

            WriteHeader();

            UInt32 startPosition = 0, endPosition = 0;

            TiffCompression compression = TiffCompression.None; // TODO: support other compressions
            TiffSampleFormat sampleFormat = (geometry as ISpectralGeometry).Raster.Format == RasterFormat.Floating ? TiffSampleFormat.Floating : TiffSampleFormat.UnsignedInteger;

            // perform writing based on representation
            WriteRasterContentToStrip((geometry as ISpectralGeometry).Raster, compression, sampleFormat, out startPosition, out endPosition);

            TiffImageFileDirectory imageFileDirectory = ComputeImageFileDirectory(geometry as ISpectralGeometry, compression, sampleFormat, startPosition, endPosition);
            WriteImageFileDirectory(imageFileDirectory);
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
        protected virtual TiffImageFileDirectory ComputeImageFileDirectory(ISpectralGeometry geometry, TiffCompression compression, TiffSampleFormat format, UInt32 startPosition, UInt32 endPosition)
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
            imageFileDirectory.Add(273, new Object[] { startPosition }); // strip offsets
            imageFileDirectory.Add(279, new Object[] { (UInt32)(endPosition - startPosition) }); // strip byte counts
            imageFileDirectory.Add(258, geometry.Raster.RadiometricResolutions.Select(resolution => (UInt16)resolution).Cast<Object>().ToArray() ); // bits per sample
            imageFileDirectory.Add(277, new Object[] { (UInt32)geometry.Raster.NumberOfBands }); // samples per pixel
            imageFileDirectory.Add(339, new Object[] { (UInt16)format }); // sample format

            // add metadata
            imageFileDirectory.Add(305, new Object[] { "AEGIS Spatio-Temporal Framework" }); // software
            imageFileDirectory.Add(306, new Object[] { DateTime.UtcNow.ToString(CultureInfo.InvariantCulture.DateTimeFormat) }); // date time

            if (geometry.Metadata != null)
            {
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
            }

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

            Byte[] bytes = new Byte[4];

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

            EndianBitConverter.CopyBytes((UInt16)42, bytes, 2); // TIFF identifier
            _baseStream.Write(bytes, 0, bytes.Length);
            _baseStream.Seek(8, SeekOrigin.Begin);

            _currentImageFileDirectoryEndPosition = 4;

            _fileHeaderWritten = true;
        }

        /// <summary>
        /// Writes an image file directory to the stream.
        /// </summary>
        /// <param name="imageFileDirectory">The image file directory.</param>
        private void WriteImageFileDirectory(TiffImageFileDirectory imageFileDirectory)
        {
            _currentImageFileDirectoryStartPosition = (Int32)_baseStream.Position;

            // write the position of the image file descriptor
            _baseStream.Seek(_currentImageFileDirectoryEndPosition, SeekOrigin.Begin);
            _baseStream.Write(EndianBitConverter.GetBytes(_currentImageFileDirectoryStartPosition), 0, 4);

            // size of IFD
            Int32 imageFileDirectorySize = 2 + 12 * imageFileDirectory.Count + 4;
            _currentImageFileDirectoryEndPosition = _currentImageFileDirectoryStartPosition + 2 + 12 * imageFileDirectory.Count;

            // position after the IFD to write exceeding values
            _baseStream.Seek(_currentImageFileDirectoryStartPosition + imageFileDirectorySize, SeekOrigin.Begin);

            // the IFD should be written in one operation
            Byte[] bytes = new Byte[imageFileDirectorySize];
            EndianBitConverter.CopyBytes((UInt16)imageFileDirectory.Count, bytes, 0); // number of entries
            Int32 byteIndex = 2;

            TiffTagType entryType;
            UInt16 dataSize;
            UInt32 dataCount;

            foreach (UInt16 entryTag in imageFileDirectory.Keys)
            {
                entryType = GetTagType(imageFileDirectory[entryTag][0]);
                dataSize = GetTagSize(entryType);

                if (entryType == TiffTagType.ASCII)
                {
                    dataCount = 0;
                    for (Int32 i = 0; i < imageFileDirectory[entryTag].Length; i++)
                    {
                        dataCount += (UInt32)(imageFileDirectory[entryTag][i] as String).Length;
                    }
                }
                else
                {
                    dataCount = (UInt32)imageFileDirectory[entryTag].Length;
                }

                EndianBitConverter.CopyBytes(entryTag, bytes, byteIndex);
                EndianBitConverter.CopyBytes((UInt16)entryType, bytes, byteIndex + 2);
                EndianBitConverter.CopyBytes(dataCount, bytes, byteIndex + 4);

                // values exceeding 4 bytes must be written to another position
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
                    UInt32 valuePosition = (UInt32)_baseStream.Position;
                    _baseStream.Write(dataBytes, 0, dataBytes.Length);

                    EndianBitConverter.CopyBytes(valuePosition, bytes, byteIndex + 8);
                }
                byteIndex += 12;
            }

            UInt32 finalPosition = (UInt32)_baseStream.Position;

            // write the IFD
            _baseStream.Seek(_currentImageFileDirectoryStartPosition, SeekOrigin.Begin);
            _baseStream.Write(bytes, 0, bytes.Length);

            // position after all values
            _baseStream.Seek(finalPosition, SeekOrigin.Begin);
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
        /// <param name="startPosition">The starting position of the strip.</param>
        /// <param name="endPosition">The ending position of the strip.</param>
        private void WriteRasterContentToStrip(IRaster raster, TiffCompression compression, TiffSampleFormat format, out UInt32 startPosition, out UInt32 endPosition)
        {
            // mark the starting position of the strip
            startPosition = (UInt32)_baseStream.Position;

            Int32 numberOfBytes = (Int32)Math.Ceiling(raster.RadiometricResolutions.Max() / 8.0) * raster.NumberOfBands * raster.NumberOfRows * raster.NumberOfColumns;
            Int32 numberOfBytesLeft = numberOfBytes;

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

                            numberOfBytesLeft -= byteIndex;
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

            // mark the ending position of the strip
            endPosition = (UInt32)_baseStream.Position;
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
            }
            return startIndex + dataSize;
        }

        #endregion
    }
}
