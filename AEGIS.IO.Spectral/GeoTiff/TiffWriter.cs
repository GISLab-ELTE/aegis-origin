/// <copyright file="TiffWriter.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
    using Newtonsoft.Json;    // short expression for the IFD table
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
        /// The TIFF structure.
        /// </summary>
        private TiffStructure _structure;

        /// <summary>
        /// The number of images written into the stream.
        /// </summary>
        private Int32 _imageCount;

        /// <summary>
        /// The maximum number of images that can be written into the stream.
        /// </summary>
        private Int32 _maxImageCount;

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
            : this(path, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public TiffWriter(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.Tiff, parameters)
        {
            _maxImageCount = Convert.ToInt32(ResolveParameter(SpectralGeometryStreamParameters.MaxNumberOfRasters));
            InitializeStream(ResolveParameter<TiffStructure>(SpectralGeometryStreamParameters.TiffStructure));
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
            : this(path, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public TiffWriter(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.Tiff, parameters)
        {
            _maxImageCount = Convert.ToInt32(ResolveParameter(SpectralGeometryStreamParameters.MaxNumberOfRasters));
            InitializeStream(ResolveParameter<TiffStructure>(SpectralGeometryStreamParameters.TiffStructure));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to be written.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        public TiffWriter(Stream stream)
            : this(stream, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.TiffWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to be written.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        public TiffWriter(Stream stream, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, SpectralGeometryStreamFormats.Tiff, parameters)
        {
            _maxImageCount = Convert.ToInt32(ResolveParameter(SpectralGeometryStreamParameters.MaxNumberOfRasters));
            InitializeStream(ResolveParameter<TiffStructure>(SpectralGeometryStreamParameters.TiffStructure));
        }

        #endregion

        #region GeometryStreamWriter protected methods

        /// <summary>
        /// Apply the write operation for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentException">The number of images in the stream has reached the maximum.</exception>
        protected override void ApplyWriteGeometry(IGeometry geometry)
        {
            if (_imageCount == _maxImageCount)
                throw new InvalidOperationException("The number of images in the stream has reached the maximum.");

            IRaster raster = (geometry as ISpectralGeometry).Raster;

            if (raster == null)
                return;

            // TODO: compute the format more exactly
            Int64 contentSize = (Int64)raster.RadiometricResolution / 8 * raster.NumberOfBands * raster.NumberOfColumns * raster.NumberOfRows;

            InitializeStream(contentSize > BigTiffThreshold ? TiffStructure.BigTiff : TiffStructure.RegularTiff);
            
            TiffCompression compression = TiffCompression.None; // TODO: support other compressions
            TiffSampleFormat sampleFormat = (geometry as ISpectralGeometry).Raster.Format == RasterFormat.Floating ? TiffSampleFormat.Floating : TiffSampleFormat.UnsignedInteger;

            TiffImageFileDirectory imageFileDirectory = ComputeImageFileDirectory(geometry as ISpectralGeometry, compression, sampleFormat);

            // compute and update raster content position
            Int64 imageFileDirectorySize = ComputeImageFileDirectorySize(imageFileDirectory);
            Int64 rasterContentStartPosition = _baseStream.Position + imageFileDirectorySize;
            Int64 rasterContentSize = ComputeRasterContentSize(raster);

            // strip offset and length
            switch (_structure)
            {
                case TiffStructure.RegularTiff:
                    imageFileDirectory[TiffTag.StripOffsets][0] = (UInt32)rasterContentStartPosition;
                    imageFileDirectory[TiffTag.StripByteCounts][0] = (UInt32)rasterContentSize;

                    WriteImageFileDirectory(imageFileDirectory);
                    break;
                case TiffStructure.BigTiff:
                    imageFileDirectory[TiffTag.StripOffsets][0] = (UInt64)rasterContentStartPosition;
                    imageFileDirectory[TiffTag.StripByteCounts][0] = (UInt64)rasterContentSize;

                    WriteBigImageFileDirectory(imageFileDirectory);
                    break;
            }

            // perform writing based on representation
            WriteRasterContentToStrip((geometry as ISpectralGeometry).Raster, compression, sampleFormat);

            _imageCount++;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (_baseStream != null && _imageCount < _maxImageCount)
            {
                // write the 0 address of next image file directory
                _baseStream.Seek(_currentImageFileDirectoryEndPosition, SeekOrigin.Begin);

                switch (_structure)
                {
                    case TiffStructure.RegularTiff:
                        _baseStream.Write(EndianBitConverter.GetBytes((UInt32)0), 0, 4);
                        break;
                    case TiffStructure.BigTiff:
                        _baseStream.Write(EndianBitConverter.GetBytes((UInt64)0), 0, 8);
                        break;
                }
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
            AddImageTag(imageFileDirectory, TiffTag.PhotometricInterpretation, (UInt16)photometricInterpretation);
            AddImageTag(imageFileDirectory, TiffTag.Compression, (UInt16)compression);
            AddImageTag(imageFileDirectory, TiffTag.ImageLength, (UInt32)geometry.Raster.NumberOfRows);
            AddImageTag(imageFileDirectory, TiffTag.ImageWidth, (UInt32)geometry.Raster.NumberOfColumns);
            AddImageTag(imageFileDirectory, TiffTag.ResolutionUnit, (UInt16)2);
            AddImageTag(imageFileDirectory, TiffTag.XResolution, (geometry.Raster.Mapper != null) ? (Rational)geometry.Raster.Mapper.ColumnSize : (Rational)1);
            AddImageTag(imageFileDirectory, TiffTag.YResolution, (geometry.Raster.Mapper != null) ? (Rational)geometry.Raster.Mapper.RowSize : (Rational)1);
            AddImageTag(imageFileDirectory, TiffTag.RowsPerStrip, (UInt32)geometry.Raster.NumberOfRows);
            AddImageTag(imageFileDirectory, TiffTag.StripOffsets, (UInt32)0);
            AddImageTag(imageFileDirectory, TiffTag.StripByteCounts, (UInt32)0);
            AddImageTag(imageFileDirectory, TiffTag.BitsPerSample, Enumerable.Repeat((UInt16)geometry.Raster.RadiometricResolution, geometry.Raster.NumberOfBands).Cast<Object>().ToArray());
            AddImageTag(imageFileDirectory, TiffTag.SamplesPerPixel, (UInt32)geometry.Raster.NumberOfBands);
            AddImageTag(imageFileDirectory, TiffTag.SampleFormat, (UInt16)format);

            // add color palette
            if (photometricInterpretation == TiffPhotometricInterpretation.PaletteColor)
            {
                imageFileDirectory.Add(TiffTag.ColorMap, ComputeColorMap(geometry));
            }

            // add metadata
            AddImageTag(imageFileDirectory, TiffTag.DocumentName, geometry.Metadata, "GeoTIFF::DocumentName");
            AddImageTag(imageFileDirectory, TiffTag.ImageDescription, geometry.Metadata, "GeoTIFF::ImageDescription");
            AddImageTag(imageFileDirectory, TiffTag.DocumentName, geometry.Metadata, "GeoTIFF::Make");
            AddImageTag(imageFileDirectory, TiffTag.Make, geometry.Metadata, "GeoTIFF::DocumentName");
            AddImageTag(imageFileDirectory, TiffTag.Model, geometry.Metadata, "GeoTIFF::Model");
            AddImageTag(imageFileDirectory, TiffTag.PageName, geometry.Metadata, "GeoTIFF::PageName");
            AddImageTag(imageFileDirectory, TiffTag.Artist, geometry.Metadata, "GeoTIFF::Artist");
            AddImageTag(imageFileDirectory, TiffTag.HostComputer, geometry.Metadata, "GeoTIFF::HostComputer");
            AddImageTag(imageFileDirectory, TiffTag.Copyright, geometry.Metadata, "GeoTIFF::Copyright");
            AddImageTag(imageFileDirectory, TiffTag.Software, "AEGIS Geospatial Framework");
            AddImageTag(imageFileDirectory, TiffTag.DateTime, DateTime.Now.ToString(CultureInfo.InvariantCulture.DateTimeFormat));

            Dictionary<String, Object> attributes = geometry.Metadata.Where(attribute => !attribute.Key.Contains("GeoTIFF")).ToDictionary(attribute => attribute.Key, attribute => attribute.Value);

            if (geometry.Metadata.Count(attribute => !attribute.Key.Contains("GeoTIFF")) > 0)
                imageFileDirectory.Add(TiffTag.AegisAttributes, new Object[] { JsonConvert.SerializeObject(attributes, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) });

            return imageFileDirectory;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the stream.
        /// </summary>
        /// <param name="structure">The TIFF structure.</param>
        private void InitializeStream(TiffStructure structure)
        {
            if (structure == TiffStructure.Undefined || _structure != TiffStructure.Undefined)
                return;

            _structure = structure;
            _imageCount = 0;

            Byte[] headerBytes = null;

            switch (structure)
            {
                case TiffStructure.RegularTiff:
                    headerBytes = new Byte[4];
                    _imageFileDirectoryEntrySize = 12;
                    _imageFileDirectoryFieldSize = 4;

                    break;
                case TiffStructure.BigTiff:
                    headerBytes = new Byte[8];
                    _imageFileDirectoryEntrySize = 20;
                    _imageFileDirectoryFieldSize = 8;

                    break;
            }

            _baseStream.Seek(0, SeekOrigin.Begin);
            switch (EndianBitConverter.DefaultByteOrder) // the default endianness should be used for the encoding
            {
                case ByteOrder.LittleEndian:
                    EndianBitConverter.CopyBytes((UInt16)0x4949, headerBytes, 0);
                    break;
                case ByteOrder.BigEndian:
                    EndianBitConverter.CopyBytes((UInt16)0x4D4D, headerBytes, 0);
                    break;
            }

            switch (structure)
            {
                case TiffStructure.RegularTiff:
                    EndianBitConverter.CopyBytes((UInt16)42, headerBytes, 2); // TIFF identifier
                    _baseStream.Write(headerBytes, 0, headerBytes.Length);
                    _baseStream.Seek(8, SeekOrigin.Begin);

                    _currentImageFileDirectoryEndPosition = 4;
                    break;
                case TiffStructure.BigTiff:
                    EndianBitConverter.CopyBytes((UInt16)43, headerBytes, 2); // BigTIFF identifier
                    EndianBitConverter.CopyBytes((UInt16)8, headerBytes, 4); // BigTIFF field size
                    EndianBitConverter.CopyBytes((UInt16)0, headerBytes, 6);
                    _baseStream.Write(headerBytes, 0, headerBytes.Length);
                    _baseStream.Seek(16, SeekOrigin.Begin);

                    _currentImageFileDirectoryEndPosition = 8;
                    break;
            }
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
            Byte[] bytes;
            if (_imageCount == _maxImageCount - 1)
                bytes = new Byte[imageFileDirectorySize + _imageFileDirectoryFieldSize];
            else
                bytes = new Byte[imageFileDirectorySize];

            EndianBitConverter.CopyBytes((UInt64)imageFileDirectory.Count, bytes, 0); // number of entries
            Int32 byteIndex = 8;

            TiffTagType entryType;
            UInt16 dataSize;
            Int64 dataCount;

            foreach (UInt16 entryTag in imageFileDirectory.Keys)
            {
                entryType = TiffTag.GetType(imageFileDirectory[entryTag][0]);
                dataSize = TiffTag.GetSize(entryType);

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
                    dataStartIndex = TiffTag.SetValue(entryType, imageFileDirectory[entryTag][valueIndex], dataBytes, dataStartIndex);
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
            Byte[] bytes;
            if (_imageCount == _maxImageCount - 1)
                bytes = new Byte[imageFileDirectorySize + _imageFileDirectoryFieldSize];
            else
                bytes = new Byte[imageFileDirectorySize];

            EndianBitConverter.CopyBytes((UInt16)imageFileDirectory.Count, bytes, 0); // number of entries
            Int32 byteIndex = 2;

            TiffTagType entryType;
            UInt16 dataSize;
            Int64 dataCount;

            foreach (UInt16 entryTag in imageFileDirectory.Keys)
            {
                entryType = TiffTag.GetType(imageFileDirectory[entryTag][0]);
                dataSize = TiffTag.GetSize(entryType);

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
                    dataStartIndex = TiffTag.SetValue(entryType, imageFileDirectory[entryTag][valueIndex], dataBytes, dataStartIndex);
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
            Int64 size = (_structure == TiffStructure.BigTiff ? 8 : 2) // number of entries
                       + _imageFileDirectoryEntrySize * imageFileDirectory.Count  // entries
                       + _imageFileDirectoryFieldSize; // position of next directory

            TiffTagType entryType;
            UInt16 dataSize;
            Int32 dataCount;

            foreach (UInt16 entryTag in imageFileDirectory.Keys)
            {
                entryType = TiffTag.GetType(imageFileDirectory[entryTag][0]);
                dataSize = TiffTag.GetSize(entryType);

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
            Int64 rasterContentSize = (Int64)Math.Ceiling(raster.RadiometricResolution / 8.0) * raster.NumberOfBands * raster.NumberOfRows * raster.NumberOfColumns;

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
        /// Computes the color map.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The computed color map.</returns>
        private Object[] ComputeColorMap(ISpectralGeometry geometry)
        {
            if (geometry.Presentation.ColorPalette == null)
                return null;

            Int32 valueCount = (Int32)Math.Pow(2, geometry.Raster.RadiometricResolution);

            Object[] colorMap = Enumerable.Repeat((UInt16)0, 3 * valueCount).Cast<Object>().ToArray();

            foreach (Int32 entryIndex in geometry.Presentation.ColorPalette.Keys)
            {
                colorMap[entryIndex] = (UInt16)geometry.Presentation.ColorPalette[entryIndex][0];
                colorMap[valueCount + entryIndex] = (UInt16)geometry.Presentation.ColorPalette[entryIndex][1];
                colorMap[2 * valueCount + entryIndex] = (UInt16)geometry.Presentation.ColorPalette[entryIndex][2];
            }

            return colorMap;
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
            UInt32 numberOfBytes = (UInt32)(Math.Ceiling(raster.RadiometricResolution / 8.0) * raster.NumberOfBands * raster.NumberOfRows * raster.NumberOfColumns);
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
                                WriteUnsignedIntergerValue(raster.GetValue(rowIndex, columnIndex, bandIndex), raster.RadiometricResolution, bytes, ref byteIndex, ref bitIndex);
                                break;
                            case TiffSampleFormat.SignedInteger:
                            case TiffSampleFormat.Floating:
                                WriteFloatValue(raster.GetFloatValue(rowIndex, columnIndex, bandIndex), raster.RadiometricResolution, bytes, ref byteIndex, ref bitIndex);
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
        /// Writes the specified unsigned integer value to the array.
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

        /// <summary>
        /// Adds the specified tag to the Image File Directory.
        /// </summary>
        /// <param name="directory">The Image File Directory.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="value">The value.</param>
        protected void AddImageTag(TiffImageFileDirectory directory, UInt16 tag, Object value)
        {
            directory[tag] = new Object[] { value };
        }

        /// <summary>
        /// Adds the specified tag to the Image File Directory.
        /// </summary>
        /// <param name="directory">The Image File Directory.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="value">The array of values.</param>
        protected void AddImageTag(TiffImageFileDirectory directory, UInt16 tag, Object[] value)
        {
            directory[tag] = value;
        }

        /// <summary>
        /// Adds the specified tag to the Image File Directory.
        /// </summary>
        /// <param name="directory">The Image File Directory.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="metadata">The geometry metadata.</param>
        /// <param name="tagName">The name of the tag.</param>
        protected void AddImageTag(TiffImageFileDirectory directory, UInt16 tag, IDictionary<String, Object> metadata, String tagName)
        {
            if (metadata.ContainsKey(tagName))
            {
                directory[tag] = new Object[] { metadata[tagName] };
                metadata.Remove(tagName);
            }
        }

        #endregion
    }
}
