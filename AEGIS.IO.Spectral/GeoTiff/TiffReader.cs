/// <copyright file="TiffReader.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
using Ionic.Zlib;
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
    /// Represents a TIFF file format reader.
    /// </summary>
    /// <remarks>
    /// TIFF (originally standing for Tagged Image File Format) is a tag-based file format for storing and interchanging raster images.
    /// The general TIFF format does not support georeferencing, hence only raster information can be read from the file.
    /// </remarks>
    [IdentifiedObjectInstance("AEGIS::610201", "Tagged Image File Format")]
    public class TiffReader : GeometryStreamReader
    {
        #region Private constant fields

        /// <summary>
        /// The maximum number of bytes to be read from the file in a single operation. This field is constant.
        /// </summary>
        private const Int32 NumberOfReadableBytes = 1 << 20;

        #endregion

        #region Protected fields

        /// <summary>
        /// The list of image file directories.
        /// </summary>
        protected List<TiffImageFileDirectory> _imageFileDirectories;

        /// <summary>
        /// The index of the current image.
        /// </summary>
        protected Int32 _currentImageIndex;

        #endregion

        #region Private fields

        /// <summary>
        /// The byte order of the file.
        /// </summary>
        private ByteOrder _byteOrder;

        /// <summary>
        /// The sample format of the file.
        /// </summary>
        private TiffSampleFormat _sampleFormat;

        /// <summary>
        /// The compression of the file.
        /// </summary>
        private TiffCompression _compression;

        /// <summary>
        /// A value indicating whether the file uses horizontal differencing.
        /// </summary>
        private Boolean _horizontalDifferencing;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        public TiffReader(String path)
            : base(path, SpectralGeometryStreamFormats.Tiff, null)
        {
            ReadHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        public TiffReader(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.Tiff, parameters)
        {
            ReadHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        public TiffReader(Uri path)
            : base(path, SpectralGeometryStreamFormats.Tiff, null)
        {
            ReadHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        public TiffReader(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.Tiff, parameters)
        {
            ReadHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream reading.</exception>
        public TiffReader(Stream stream)
            : base(stream, SpectralGeometryStreamFormats.Tiff, null)
        {
            ReadHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream reading.</exception>
        public TiffReader(Stream stream, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, SpectralGeometryStreamFormats.Tiff, parameters)
        {
            ReadHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="format">The format of the stream reader.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        protected TiffReader(String path, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, format, parameters)
        {
            ReadHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="format">The format of the stream reader.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        protected TiffReader(Uri path, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, format, parameters)
        {
            ReadHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffReader" /> class.
        /// </summary>
        /// <param name="path">The stream to be read.</param>
        /// <param name="format">The format of the stream reader.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The stream is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream reading.</exception>
        protected TiffReader(Stream stream, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, format, parameters)
        {
            ReadHeader();
        }

        #endregion

        #region Protected GeometryStreamReader methods

        /// <summary>
        /// Returns a value indicating whether the end of the stream is reached.
        /// </summary>
        /// <returns><c>true</c> if the end of the stream is reached; otherwise <c>false</c>.</returns>
        protected override Boolean GetEndOfStream() { return _currentImageIndex == _imageFileDirectories.Count; }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        /// <exception cref="System.NotSupportedException">
        /// Stream format is not supported.
        /// or
        /// Compression is not supported.
        /// </exception>
        protected override IGeometry ApplyReadGeometry()
        {
            if (_imageFileDirectories[_currentImageIndex][TiffTag.BitsPerSample].Min() != _imageFileDirectories[_currentImageIndex][TiffTag.BitsPerSample].Max())
                throw new NotSupportedException("Stream format is not supported.");

            // create the geometry
            ISpectralGeometry spectralGeometry = null;

            // read additional geometry data
            IReferenceSystem referenceSystem = ComputeReferenceSystem();
            IDictionary<String, Object> metadata = ComputeMetadata();
            RasterPresentation presentation = ComputeRasterPresentation();
            RasterImaging imaging = ComputeRasterImaging();

            // read additional raster data
            Int32[] radiometricResolutions = ComputeRadiometricResolutionData();
            RasterMapper mapper = ComputeRasterMapper();

            // compute the raster dimensions
            Int32 imageLength = Convert.ToInt32(_imageFileDirectories[_currentImageIndex][TiffTag.ImageLength][0]);
            Int32 imageWidth = Convert.ToInt32(_imageFileDirectories[_currentImageIndex][TiffTag.ImageWidth][0]);

            // create the raster based on the format
            switch (_sampleFormat)
            {
                case TiffSampleFormat.UnsignedInteger:
                case TiffSampleFormat.Undefined:
                    spectralGeometry = ResolveFactory(referenceSystem).CreateSpectralPolygon(RasterFormat.Integer, radiometricResolutions.Length, imageLength, imageWidth, radiometricResolutions.Max(), mapper, presentation, imaging, metadata);
                    break;
                case TiffSampleFormat.SignedInteger:
                case TiffSampleFormat.Floating:
                    spectralGeometry = ResolveFactory(referenceSystem).CreateSpectralPolygon(RasterFormat.Floating, radiometricResolutions.Length, imageLength, imageWidth, radiometricResolutions.Max(), mapper, presentation, imaging, metadata);
                    break;
            }

            // read content based on the layout
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.StripOffsets))
            {
                // strip layout
                ReadRasterContentFromStrips(spectralGeometry.Raster, radiometricResolutions);
            }
            else if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.TileOffsets))
            {
                // tiled layout
                ReadRasterContentFromTiles(spectralGeometry.Raster, radiometricResolutions);
            }
            else
            {
                // unknown layout
                throw new NotSupportedException("Stream format is not supported.");
            }

            _currentImageIndex++;

            return spectralGeometry;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads the header.
        /// </summary>
        /// <exception cref="System.IO.IOException">Exception occurred during stream reading.</exception>
        private void ReadHeader()
        {
            try
            {
                Byte[] bytes = new Byte[8];
                _baseStream.Read(bytes, 0, bytes.Length);

                // check the byte-order
                UInt16 byteOrder = EndianBitConverter.ToUInt16(bytes, 0);
                if (byteOrder != 0x4949 && byteOrder != 0x4D4D)
                {
                    throw new InvalidDataException("Stream header content is invalid.");
                }
                _byteOrder = (byteOrder == 0x4949) ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

                // check the TIFF identifier
                Int32 identifier = EndianBitConverter.ToUInt16(bytes, 2, _byteOrder);

                _imageFileDirectories = new List<TiffImageFileDirectory>();
                _imageFileDirectories.Add(new TiffImageFileDirectory());
                _currentImageIndex = 0;

                switch (identifier)
                {
                    case 42: // regular TIFF
                        try
                        {
                            ReadImageFileDirectory(EndianBitConverter.ToUInt32(bytes, 4, _byteOrder));
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidDataException("Stream header content is invalid.", ex);
                        }
                        break;
                    case 43: // BigTIFF
                        try
                        {
                            _baseStream.Read(bytes, 0, bytes.Length);
                            ReadBigImageFileDirectory(EndianBitConverter.ToUInt64(bytes, 0, _byteOrder));
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidDataException("Stream header content is invalid.", ex);
                        }
                        break;
                    default:
                        throw new InvalidDataException("Stream header content is invalid.");
                }


                // check the sample format
                if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.SampleFormat))
                    _sampleFormat = (TiffSampleFormat)Convert.ToUInt16(_imageFileDirectories[_currentImageIndex][339][0]);
                else
                    _sampleFormat = TiffSampleFormat.UnsignedInteger;


                // check whether the content is compressed
                if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.Compression))
                    _compression = (TiffCompression)Convert.ToUInt16(_imageFileDirectories[_currentImageIndex][259][0]);
                else
                    _compression = TiffCompression.None;

                if (_compression != TiffCompression.None && _compression != TiffCompression.Deflate)
                    throw new NotSupportedException("Compression is not supported.");

                _horizontalDifferencing = _imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.HorizontalDifferencing) && Convert.ToInt32(_imageFileDirectories[_currentImageIndex][TiffTag.HorizontalDifferencing][0]) == 2;
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentReadError, ex);
            }
        }

        /// <summary>
        /// Reads an image file directory (in BigTIFF format).
        /// </summary>
        /// <param name="offset">The zero-based starting offset of the IFD table.</param>
        private void ReadBigImageFileDirectory(UInt64 offset)
        {
            if (offset == 0)
                return;

            _baseStream.Seek((Int64)offset, SeekOrigin.Begin);

            Byte[] bytes = new Byte[8];

            _baseStream.Read(bytes, 0, 8);

            Int32 numberOfEntries = (Int32)(EndianBitConverter.ToUInt64(bytes, 0, _byteOrder));

            bytes = new Byte[numberOfEntries * 20 + 8];
            _baseStream.Read(bytes, 0, bytes.Length);
            Int64 streamPosition = _baseStream.Position;

            TiffTagType entryType;
            UInt16 entryTag, dataSize;
            Int64 dataCount;
            Object[] dataArray = null;
            String dataString = null;

            for (Int32 entryIndex = 0; entryIndex < numberOfEntries; entryIndex++)
            {
                entryTag = EndianBitConverter.ToUInt16(bytes, entryIndex * 20, _byteOrder);
                entryType = (TiffTagType)EndianBitConverter.ToUInt16(bytes, entryIndex * 20 + 2, _byteOrder);
                dataSize = TiffTag.GetSize(entryType);

                dataCount = (Int64)(EndianBitConverter.ToUInt64(bytes, entryIndex * 20 + 4, _byteOrder));
                if (entryType != TiffTagType.ASCII)
                    dataArray = new Object[dataCount];
                else
                    dataString = String.Empty;

                if (dataSize * dataCount > 8)
                {
                    Byte[] dataBytes = new Byte[dataCount * dataSize];

                    // if the value does not fit into the field move to the position of the value
                    _baseStream.Seek(EndianBitConverter.ToUInt32(bytes, entryIndex * 20 + 12, _byteOrder), SeekOrigin.Begin);
                    _baseStream.Read(dataBytes, 0, dataBytes.Length); // read the value

                    for (Int32 dataIndex = 0; dataIndex < dataCount; dataIndex++)
                    {
                        if (entryType == TiffTagType.ASCII)
                        {
                            dataString += Convert.ToChar(dataBytes[dataIndex]);
                        }
                        else
                        {
                            dataArray[dataIndex] = TiffTag.GetValue(entryType, dataBytes, dataIndex * dataSize, _byteOrder);
                        }
                    }
                }
                else
                {
                    // is the value fits into the field
                    for (Int32 dataIndex = 0; dataIndex < dataCount; dataIndex++)
                    {
                        if (entryType == TiffTagType.ASCII)
                        {
                            dataString += Convert.ToChar(bytes[12 + dataIndex]);
                        }
                        else
                        {
                            dataArray[dataIndex] = TiffTag.GetValue(entryType, bytes, entryIndex * 20 + 12 + dataIndex * dataSize, _byteOrder);
                        }
                    }
                }

                // add the entry to the IFD table
                if (entryType == TiffTagType.ASCII)
                {
                    _imageFileDirectories[_imageFileDirectories.Count - 1].Add(entryTag, new Object[] { dataString });
                }
                else
                {
                    _imageFileDirectories[_imageFileDirectories.Count - 1].Add(entryTag, dataArray);
                }
            }

            offset = EndianBitConverter.ToUInt64(bytes, numberOfEntries * 20, _byteOrder);
            if (offset != 0) // another IFD table exists
            {
                _imageFileDirectories.Add(new TiffImageFileDirectory());
                ReadBigImageFileDirectory(offset); // read the next IFD table    
            }
        }

        /// <summary>
        /// Reads an image file directory.
        /// </summary>
        /// <param name="offset">The zero-based starting offset of the IFD table.</param>
        private void ReadImageFileDirectory(UInt32 offset)
        {
            if (offset == 0)
                return;

            _baseStream.Seek(offset, SeekOrigin.Begin);

            Byte[] bytes = new Byte[2];

            _baseStream.Read(bytes, 0, 2);

            UInt16 numberOfEntries = EndianBitConverter.ToUInt16(bytes, 0, _byteOrder);

            bytes = new Byte[numberOfEntries * 12 + 4];
            _baseStream.Read(bytes, 0, bytes.Length);
            Int64 streamPosition = _baseStream.Position;

            TiffTagType entryType;
            UInt16 entryTag, dataSize;
            UInt32 dataCount;
            Object[] dataArray = null;
            String dataString = null;

            for (Int32 entryIndex = 0; entryIndex < numberOfEntries; entryIndex++)
            {
                entryTag = EndianBitConverter.ToUInt16(bytes, entryIndex * 12 + 0, _byteOrder);
                entryType = (TiffTagType)EndianBitConverter.ToUInt16(bytes, entryIndex * 12 + 2, _byteOrder);
                dataSize = TiffTag.GetSize(entryType);

                dataCount = EndianBitConverter.ToUInt32(bytes, entryIndex * 12 + 4, _byteOrder);
                if (entryType != TiffTagType.ASCII)
                    dataArray = new Object[dataCount];
                else
                    dataString = String.Empty;

                if (dataSize * dataCount > 4)
                {
                    Byte[] dataBytes = new Byte[dataCount * dataSize];

                    // if the value does not fit into the field move to the position of the value
                    _baseStream.Seek(EndianBitConverter.ToUInt32(bytes, entryIndex * 12 + 8, _byteOrder), SeekOrigin.Begin);
                    _baseStream.Read(dataBytes, 0, dataBytes.Length); // read the value

                    for (Int32 dataIndex = 0; dataIndex < dataCount; dataIndex++)
                    {
                        if (entryType == TiffTagType.ASCII)
                        {
                            dataString += Convert.ToChar(dataBytes[dataIndex]);
                        }
                        else
                        {
                            dataArray[dataIndex] = TiffTag.GetValue(entryType, dataBytes, dataIndex * dataSize, _byteOrder);
                        }
                    }
                }
                else
                {
                    // is the value fits into the field
                    for (Int32 dataIndex = 0; dataIndex < dataCount; dataIndex++)
                    {
                        if (entryType == TiffTagType.ASCII)
                        {
                            dataString += Convert.ToChar(bytes[8 + dataIndex]);
                        }
                        else
                        {
                            dataArray[dataIndex] = TiffTag.GetValue(entryType, bytes, entryIndex * 12 + 8 + dataIndex * dataSize, _byteOrder);
                        }
                    }
                }

                // add the entry to the IFD table
                if (entryType == TiffTagType.ASCII)
                {
                    _imageFileDirectories[_imageFileDirectories.Count - 1].Add(entryTag, new Object[] { dataString });
                }
                else
                {
                    _imageFileDirectories[_imageFileDirectories.Count - 1].Add(entryTag, dataArray);
                }
            } 

            offset = EndianBitConverter.ToUInt32(bytes, numberOfEntries * 12, _byteOrder);
            if (offset != 0) // another IFD table exists
            {
                _imageFileDirectories.Add(new TiffImageFileDirectory());
                ReadImageFileDirectory(offset); // read the next IFD table    
            }
        }

        /// <summary>
        /// Reads the raster of the geometry stored in strips.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="radiometricResolutions">The array of radiometric resolutions.</param>
        private void ReadRasterContentFromStrips(IRaster raster, Int32[] radiometricResolutions)
        {
            Int32 rowsPerStrip;
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.RowsPerStrip))
                rowsPerStrip = Convert.ToInt32(_imageFileDirectories[_currentImageIndex][TiffTag.RowsPerStrip][0]);
            else
                rowsPerStrip = raster.NumberOfRows;

            Int32 numberOfStrips = (Int32)Math.Ceiling((Single)raster.NumberOfRows / rowsPerStrip);
            Int64[] stripOffsets = _imageFileDirectories[_currentImageIndex][TiffTag.StripOffsets].Select(value => Convert.ToInt64(value)).ToArray();
            Int64[] stripByteCounts = _imageFileDirectories[_currentImageIndex][TiffTag.StripByteCounts].Select(value => Convert.ToInt64(value)).ToArray();
            
            // fit the maximum number of readable bytes to the radiometric resolution
            UInt32 numberOfReadableBytes = NumberOfReadableBytes;
            while (numberOfReadableBytes * 8 % radiometricResolutions.Sum() != 0)
                numberOfReadableBytes--;

            // read the stripes
            Int32 rowIndex = 0;
            Int32 columnIndex = 0;

            Int64 lastValue = 0;
            Double lastFloatValue = 0;

            for (Int32 stripIndex = 0; stripIndex < numberOfStrips; stripIndex++)
            {
                _baseStream.Seek(stripOffsets[stripIndex], SeekOrigin.Begin);

                // perform decompression of stream if compressed
                Stream contentStream = null;
                switch (_compression)
                {
                    case TiffCompression.Deflate:
                        contentStream = new ZlibStream(_baseStream, CompressionMode.Decompress);
                        break;
                    default:
                        contentStream = _baseStream;
                        break;
                }

                // should not load more bytes into the memory than suggested
                Byte[] stripBytes = new Byte[stripByteCounts[stripIndex] < numberOfReadableBytes ? stripByteCounts[stripIndex] : numberOfReadableBytes];
                Int64 numberOfBytesLeft = stripByteCounts[stripIndex];
                Int32 byteIndex = 0, bitIndex = 8;

                contentStream.Read(stripBytes, 0, stripBytes.Length);

                // multiple iterations may be required for large strips
                while (numberOfBytesLeft > 0 && rowIndex < raster.NumberOfRows)
                {
                    // perform reading based on format
                    
                            if (_horizontalDifferencing)
                            {
                                switch (_sampleFormat)
                                {
                                    case TiffSampleFormat.Undefined:
                                    case TiffSampleFormat.UnsignedInteger:
                                        raster.SetValues(rowIndex, columnIndex, ReadUnsignedIntegerDifferenceValues(stripBytes, ref byteIndex, ref bitIndex, radiometricResolutions, ref lastValue));
                                        break;
                                    case TiffSampleFormat.SignedInteger:
                                        raster.SetFloatValues(rowIndex, columnIndex, ReadSignedIntegerDifferenceValues(stripBytes, ref byteIndex, ref bitIndex, radiometricResolutions, ref lastValue));
                                        break;
                                    case TiffSampleFormat.Floating:
                                        raster.SetFloatValues(rowIndex, columnIndex, ReadFloatDifferenceValues(stripBytes, ref byteIndex, ref bitIndex, radiometricResolutions, ref lastFloatValue));
                                        break;
                                }
                            }
                            else
                            {
                                switch (_sampleFormat)
                                {
                                    case TiffSampleFormat.Undefined:
                                    case TiffSampleFormat.UnsignedInteger:
                                        raster.SetValues(rowIndex, columnIndex, ReadUnsignedIntegerValues(stripBytes, ref byteIndex, ref bitIndex, radiometricResolutions));
                                        break;
                                    case TiffSampleFormat.SignedInteger:
                                        raster.SetFloatValues(rowIndex, columnIndex, ReadSignedIntegerValues(stripBytes, ref byteIndex, ref bitIndex, radiometricResolutions));
                                        break;
                                    case TiffSampleFormat.Floating:
                                        raster.SetFloatValues(rowIndex, columnIndex, ReadFloatValues(stripBytes, ref byteIndex, ref bitIndex, radiometricResolutions));
                                        break;
                                }
                            }

                    columnIndex++;
                    if (columnIndex == raster.NumberOfColumns)
                    {
                        columnIndex = 0;
                        rowIndex++;

                        lastValue = 0;
                        lastFloatValue = 0;

                        if (rowIndex == raster.NumberOfRows)
                            break;
                    }

                    if (byteIndex == stripBytes.Length)
                    {
                        numberOfBytesLeft = numberOfBytesLeft - stripBytes.Length;
                        byteIndex = 0;

                        // the final array of bytes should not be the number of bytes left
                        if (stripByteCounts[stripIndex] > numberOfReadableBytes && numberOfBytesLeft > 0 && numberOfBytesLeft < numberOfReadableBytes)
                            stripBytes = new Byte[numberOfBytesLeft];

                        _baseStream.Read(stripBytes, 0, stripBytes.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Reads the raster of the geometry stored in tiles.
        /// </summary>
        /// <param name="raster">The raster.</param>
        private void ReadRasterContentFromTiles(IRaster raster, Int32[] radiometricResolutions)
        {
            Int64[] tileOffsets = _imageFileDirectories[_currentImageIndex][TiffTag.TileOffsets].Select(value => Convert.ToInt64(value)).ToArray();
            Int64[] tileByteCounts = _imageFileDirectories[_currentImageIndex][TiffTag.TileByteCounts].Select(value => Convert.ToInt64(value)).ToArray();
            Int32 tileWidth = Convert.ToInt32(_imageFileDirectories[_currentImageIndex][TiffTag.TileWidth][0]);
            Int32 tileLength = Convert.ToInt32(_imageFileDirectories[_currentImageIndex][TiffTag.TileLength][0]);

            Int32 tilesAcross = (raster.NumberOfColumns + tileWidth - 1) / tileWidth;
            Int32 tilesDown = (raster.NumberOfRows + tileLength - 1) / tileLength;

            Int32 rowIndex = 0;
            Int32 columnIndex = 0;

            // reading the tiles
            for (Int32 tileVerticalIndex = 0; tileVerticalIndex < tilesDown; tileVerticalIndex++)
                for (Int32 tileHorizontalIndex = 0; tileHorizontalIndex < tilesAcross; tileHorizontalIndex++)
                {
                    _baseStream.Seek(tileOffsets[tileVerticalIndex * tilesAcross + tileHorizontalIndex], SeekOrigin.Begin);

                    // perform decompression of stream if compressed
                    Stream contentStream = null;
                    switch (_compression)
                    {
                        case TiffCompression.Deflate:
                            contentStream = new ZlibStream(_baseStream, CompressionMode.Decompress);
                            break;
                        default:
                            contentStream = _baseStream;
                            break;
                    }

                    // read a single tile
                    Int32 byteIndex = 0, bitIndex = 0;
                    Int32 byteShift = radiometricResolutions.Sum() / 8;
                    Int32 bitShift = radiometricResolutions.Sum() % 8;
                    Byte[] tileBytes = new Byte[tileWidth * tileLength * byteShift];

                    contentStream.Read(tileBytes, 0, tileBytes.Length);

                    Int64 lastValue = 0;
                    Double lastFloatValue = 0;

                    for (Int32 tileRowIndex = 0; tileRowIndex < tileLength; tileRowIndex++)
                    {
                        rowIndex = tileVerticalIndex * tileLength + tileRowIndex;

                        for (Int32 tileColumnIndex = 0; tileColumnIndex < tileWidth; tileColumnIndex++)
                        {
                            lastValue = 0;
                            lastFloatValue = 0;

                            columnIndex = tileHorizontalIndex * tileWidth + tileColumnIndex;

                            if (rowIndex >= raster.NumberOfRows || columnIndex >= raster.NumberOfColumns) // the tile is larger than the image
                            {
                                byteIndex += byteShift; // the values in the array must be skipped
                                bitIndex += bitShift;
                                continue;
                            }

                            if (_horizontalDifferencing)
                            {
                                switch (_sampleFormat)
                                {
                                    case TiffSampleFormat.Undefined:
                                    case TiffSampleFormat.UnsignedInteger:
                                        raster.SetValues(rowIndex, columnIndex, ReadUnsignedIntegerDifferenceValues(tileBytes, ref byteIndex, ref bitIndex, radiometricResolutions, ref lastValue));
                                        break;
                                    case TiffSampleFormat.SignedInteger:
                                        raster.SetFloatValues(rowIndex, columnIndex, ReadSignedIntegerDifferenceValues(tileBytes, ref byteIndex, ref bitIndex, radiometricResolutions, ref lastValue));
                                        break;
                                    case TiffSampleFormat.Floating:
                                        raster.SetFloatValues(rowIndex, columnIndex, ReadFloatDifferenceValues(tileBytes, ref byteIndex, ref bitIndex, radiometricResolutions, ref lastFloatValue));
                                        break;
                                }
                            }
                            else
                            {
                                switch (_sampleFormat)
                                {
                                    case TiffSampleFormat.Undefined:
                                    case TiffSampleFormat.UnsignedInteger:
                                        raster.SetValues(rowIndex, columnIndex, ReadUnsignedIntegerValues(tileBytes, ref byteIndex, ref bitIndex, radiometricResolutions));
                                        break;
                                    case TiffSampleFormat.SignedInteger:
                                        raster.SetFloatValues(rowIndex, columnIndex, ReadSignedIntegerValues(tileBytes, ref byteIndex, ref bitIndex, radiometricResolutions));
                                        break;
                                    case TiffSampleFormat.Floating:
                                        raster.SetFloatValues(rowIndex, columnIndex, ReadFloatValues(tileBytes, ref byteIndex, ref bitIndex, radiometricResolutions));
                                        break;
                                }
                            }
                        }
                    }
                }
        }


        /// <summary>
        /// Reads the unsigned integer values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the array.</param>
        /// <param name="bitIndex">The zero-based bit index in the array.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="radiometricResolutions">The array of radiometric resolutions.</param>
        /// <returns>The unsigned integer values representing a pixel.</returns>
        private UInt32[] ReadUnsignedIntegerValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, Int32[] radiometricResolutions)
        {
            UInt32[] values = new UInt32[radiometricResolutions.Length];

            for (Int32 bandIndex = 0; bandIndex < radiometricResolutions.Length; bandIndex++)
            {
                switch (radiometricResolutions[bandIndex])
                {
                    case 1:
                    case 2:
                    case 4:
                        bitIndex -= radiometricResolutions[bandIndex];
                        values[bandIndex] = Convert.ToByte((bytes[byteIndex] >> bitIndex) & 1);

                        if (bitIndex == 0)
                        {
                            bitIndex = 8;
                            byteIndex++;
                        }
                        break;
                    case 8:
                        values[bandIndex] = Convert.ToByte(bytes[byteIndex]);
                        byteIndex++;
                        break;
                    case 16:
                        values[bandIndex] = EndianBitConverter.ToUInt16(bytes, byteIndex, _byteOrder);
                        byteIndex += 2;
                        break;
                    case 32:
                        values[bandIndex] = EndianBitConverter.ToUInt32(bytes, byteIndex, _byteOrder);
                        byteIndex += 4;
                        break;
                }
                // TODO: support other radiometric resolutions
            }

            return values;
        }

        /// <summary>
        /// Reads the signed integer values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="radiometricResolutions">The array of radiometric resolutions.</param>
        /// <param name="lastValue">The last value read previously.</param>
        /// <returns>The signed integer values representing a pixel.</returns>
        private UInt32[] ReadUnsignedIntegerDifferenceValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, Int32[] radiometricResolutions, ref Int64 lastValue)
        {
            Int32 currentValue;
            UInt32[] values = new UInt32[radiometricResolutions.Length];
            
            for (Int32 bandIndex = 0; bandIndex < radiometricResolutions.Length; bandIndex++)
            {
                switch (radiometricResolutions[bandIndex])
                {
                    case 1:
                    case 2:
                    case 4:
                        bitIndex -= radiometricResolutions[bandIndex];
                        currentValue = Convert.ToSByte((bytes[byteIndex] >> bitIndex) & 1);

                        if (bitIndex == 0)
                        {
                            bitIndex = 8;
                            byteIndex++;
                        }

                        lastValue += currentValue;
                        values[bandIndex] = (Byte)lastValue;
                        break;
                    case 8:
                        currentValue = Convert.ToSByte(bytes[byteIndex]);
                        byteIndex++;

                        lastValue += currentValue;
                        values[bandIndex] = (Byte)lastValue;
                        break;
                    case 16:
                        currentValue = EndianBitConverter.ToInt16(bytes, byteIndex, _byteOrder);
                        byteIndex += 2;

                        lastValue += currentValue;
                        values[bandIndex] = (UInt16)lastValue;
                        break;
                    case 32:
                        currentValue = EndianBitConverter.ToInt32(bytes, byteIndex, _byteOrder);
                        byteIndex += 4;

                        lastValue += currentValue;
                        values[bandIndex] = (UInt32)lastValue;
                        break;
                }
                // TODO: support other radiometric resolutions
            }

            return values;
        }

        /// <summary>
        /// Reads the signed integer values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="radiometricResolutions">The array of radiometric resolutions.</param>
        /// <returns>The signed integer values representing a pixel.</returns>
        private Double[] ReadSignedIntegerValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, Int32[] radiometricResolutions)
        {
            Double[] values = new Double[radiometricResolutions.Length];

            for (Int32 bandIndex = 0; bandIndex < radiometricResolutions.Length; bandIndex++)
            {
                switch (radiometricResolutions[bandIndex])
                {
                    case 1:
                    case 2:
                    case 4:
                        bitIndex -= radiometricResolutions[bandIndex];
                        values[bandIndex] = Convert.ToByte((bytes[byteIndex] >> bitIndex) & 1);

                        if (bitIndex == 0)
                        {
                            bitIndex = 8;
                            byteIndex++;
                        }
                        break;
                    case 8:
                        values[bandIndex] = Convert.ToByte(bytes[byteIndex]);
                        byteIndex++;
                        break;
                    case 16:
                        values[bandIndex] = EndianBitConverter.ToUInt16(bytes, byteIndex, _byteOrder);
                        byteIndex += 2;
                        break;
                    case 32:
                        values[bandIndex] = EndianBitConverter.ToUInt32(bytes, byteIndex, _byteOrder);
                        byteIndex += 4;
                        break;
                }
                // TODO: support other radiometric resolutions
            }

            return values;
        }

        /// <summary>
        /// Reads the signed integer values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="radiometricResolutions">The array of radiometric resolutions.</param>
        /// <param name="lastValue">The last value read previously.</param>
        /// <returns>The signed integer values representing a pixel.</returns>
        private Double[] ReadSignedIntegerDifferenceValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, Int32[] radiometricResolutions, ref Int64 lastValue)
        {
            Int32 currentValue;
            Double[] values = new Double[radiometricResolutions.Length];

            for (Int32 bandIndex = 0; bandIndex < radiometricResolutions.Length; bandIndex++)
            {
                switch (radiometricResolutions[bandIndex])
                {
                    case 1:
                    case 2:
                    case 4:
                        bitIndex -= radiometricResolutions[bandIndex];
                        currentValue = Convert.ToSByte((bytes[byteIndex] >> bitIndex) & 1);

                        if (bitIndex == 0)
                        {
                            bitIndex = 8;
                            byteIndex++;
                        }

                        lastValue += currentValue;
                        values[bandIndex] = (Byte)lastValue;
                        break;
                    case 8:
                        currentValue = Convert.ToSByte(bytes[byteIndex]);
                        byteIndex++;

                        lastValue += currentValue;
                        values[bandIndex] = (Byte)lastValue;
                        break;
                    case 16:
                        currentValue = EndianBitConverter.ToInt16(bytes, byteIndex, _byteOrder);
                        byteIndex += 2;

                        lastValue += currentValue;
                        values[bandIndex] = (UInt16)lastValue;
                        break;
                    case 32:
                        currentValue = EndianBitConverter.ToInt32(bytes, byteIndex, _byteOrder);
                        byteIndex += 4;

                        lastValue += currentValue;
                        values[bandIndex] = (UInt32)lastValue;
                        break;
                }
                // TODO: support other radiometric resolutions
            }

            return values;
        }

        /// <summary>
        /// Reads the floating point values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="radiometricResolutions">The array of radiometric resolutions.</param>
        /// <returns>The floating point values representing a pixel.</returns>
        private Double[] ReadFloatValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, Int32[] radiometricResolutions)
        {
            Double[] values = new Double[radiometricResolutions.Length];

            for (Int32 bandIndex = 0; bandIndex < radiometricResolutions.Length; bandIndex++)
            {
                switch (radiometricResolutions[bandIndex])
                {
                    case 32:
                        values[bandIndex] = EndianBitConverter.ToSingle(bytes, byteIndex, _byteOrder);
                        byteIndex += 4;
                        break;
                    case 64:
                        values[bandIndex] = EndianBitConverter.ToDouble(bytes, byteIndex, _byteOrder);
                        byteIndex += 8;
                        break;
                }
                // TODO: support other radiometric resolutions
            }
            
            return values;
        }

        /// <summary>
        /// Reads the floating point values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="radiometricResolutions">The array of radiometric resolutions.</param>
        /// <param name="lastValue">The last value read previously.</param>
        /// <returns>The floating point values representing a pixel.</returns>
        private Double[] ReadFloatDifferenceValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, Int32[] radiometricResolutions, ref Double lastValue)
        {
            Double currentValue;
            Double[] values = new Double[radiometricResolutions.Length];

            for (Int32 bandIndex = 0; bandIndex < radiometricResolutions.Length; bandIndex++)
            {
                switch (radiometricResolutions[bandIndex])
                {
                    case 32:
                        currentValue = EndianBitConverter.ToSingle(bytes, byteIndex, _byteOrder);
                        byteIndex += 4;

                        lastValue += currentValue;
                        values[bandIndex] = (Single)lastValue;
                        break;
                    case 64:
                        currentValue = EndianBitConverter.ToDouble(bytes, byteIndex, _byteOrder);
                        byteIndex += 8;

                        lastValue += currentValue;
                        values[bandIndex] = lastValue;
                        break;
                }
                // TODO: support other radiometric resolutions
            }
            
            return values;
        }

        /// <summary>
        /// Computes the radiometric resolutions.
        /// </summary>
        /// <returns>An array containing the radiometric resolutions of the raster.</returns>
        private Int32[] ComputeRadiometricResolutionData()
        {
            UInt16 samplesPerPixel = 1, bitsPerSample = 8;

            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.SamplesPerPixel))
                samplesPerPixel = Convert.ToUInt16(_imageFileDirectories[_currentImageIndex][TiffTag.SamplesPerPixel][0]);

            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.BitsPerSample))
            {
                if (_imageFileDirectories[_currentImageIndex][TiffTag.BitsPerSample].Length == samplesPerPixel)
                    return _imageFileDirectories[_currentImageIndex][TiffTag.BitsPerSample].Select(value => Convert.ToInt32(value)).ToArray();
                else
                    bitsPerSample = Convert.ToUInt16(_imageFileDirectories[_currentImageIndex][TiffTag.BitsPerSample][0]);
            }

            return Enumerable.Repeat<Int32>(bitsPerSample, samplesPerPixel).ToArray();
        }

        #endregion 

        #region Protected methods

        /// <summary>
        /// Computes the presentation of the raster image.
        /// </summary>
        /// <returns>The presentation of the raster image.</returns>
        protected virtual RasterPresentation ComputeRasterPresentation()
        {
            switch ((TiffPhotometricInterpretation)Convert.ToInt32(_imageFileDirectories[_currentImageIndex][TiffTag.PhotometricInterpretation][0]))
            {
                case TiffPhotometricInterpretation.BlackIsZero:
                    return RasterPresentation.CreateGrayscalePresentation();
                case TiffPhotometricInterpretation.WhiteIsZero:
                    return RasterPresentation.CreateInvertedGrayscalePresentation();
                case TiffPhotometricInterpretation.RGB:
                    return RasterPresentation.CreateTrueColorPresentation();
                case TiffPhotometricInterpretation.TransparencyMask:
                    return RasterPresentation.CreateTransparencyPresentation();
                case TiffPhotometricInterpretation.CMYK:
                    return RasterPresentation.CreateTrueColorPresentation(RasterColorSpace.CMYK);
                case TiffPhotometricInterpretation.CIELab:
                    return RasterPresentation.CreateTrueColorPresentation(RasterColorSpace.CIELab);
                case TiffPhotometricInterpretation.YCbCr:
                    return RasterPresentation.CreateTrueColorPresentation(RasterColorSpace.YCbCr);
                case TiffPhotometricInterpretation.PaletteColor:
                    Dictionary<Int32, UInt16[]> colorPalette = new Dictionary<Int32, UInt16[]>();
                    Int32 valueCount = _imageFileDirectories[_currentImageIndex][TiffTag.ColorMap].Length / 3;
                    for (Int32 valueIndex = 0; valueIndex < valueCount; valueIndex++)
                    {
                        colorPalette.Add(valueIndex, new UInt16[] {
                            Convert.ToUInt16(_imageFileDirectories[_currentImageIndex][TiffTag.ColorMap][valueIndex]),
                            Convert.ToUInt16(_imageFileDirectories[_currentImageIndex][TiffTag.ColorMap][valueCount + valueIndex]),
                            Convert.ToUInt16(_imageFileDirectories[_currentImageIndex][TiffTag.ColorMap][2 * valueCount + valueIndex])
                        });
                    }
                    return RasterPresentation.CreatePresudoColorPresentation(colorPalette);
                default:
                    return null; // TODO: support other interpretations
            }
        }

        /// <summary>
        /// Computes the imaging data of the raster image.
        /// </summary>
        /// <returns>The imaging data of the raster image.</returns>
        protected virtual RasterImaging ComputeRasterImaging()
        {
            return null;
        }

        /// <summary>
        /// Computes the metadata of the geometry.
        /// </summary>
        /// <returns>A dictionary containing the metadata of the geometry.</returns>
        protected virtual IDictionary<String, Object> ComputeMetadata()
        {
            Dictionary<String, Object> metadata = new Dictionary<String, Object>();
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.DocumentName))
                metadata.Add("GeoTIFF::DocumentName", _imageFileDirectories[_currentImageIndex][TiffTag.DocumentName][0]);
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.ImageDescription))
                metadata.Add("GeoTIFF::ImageDescription", _imageFileDirectories[_currentImageIndex][TiffTag.ImageDescription][0]);
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.Make))
                metadata.Add("GeoTIFF::Make", _imageFileDirectories[_currentImageIndex][TiffTag.Make][0]);
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.Model))
                metadata.Add("GeoTIFF::Model", _imageFileDirectories[_currentImageIndex][TiffTag.Model][0]);
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.PageName))
                metadata.Add("GeoTIFF::PageName", _imageFileDirectories[_currentImageIndex][285][0]);
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.Software))
                metadata.Add("GeoTIFF::Software", _imageFileDirectories[_currentImageIndex][TiffTag.Software][0]);
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.DateTime))
                metadata.Add("GeoTIFF::DateTime", DateTime.Parse(_imageFileDirectories[_currentImageIndex][TiffTag.DateTime][0].ToString(), CultureInfo.InvariantCulture.DateTimeFormat));
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.Artist))
                metadata.Add("GeoTIFF::Artist", _imageFileDirectories[_currentImageIndex][TiffTag.Artist][0]);
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.HostComputer))
                metadata.Add("GeoTIFF::HostComputer", _imageFileDirectories[_currentImageIndex][TiffTag.HostComputer][0]);
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.Copyright))
                metadata.Add("GeoTIFF::Copyright", _imageFileDirectories[_currentImageIndex][TiffTag.Copyright][0]);

            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.AegisAttributes))
            {
                Object attributes = JsonConvert.DeserializeObject(_imageFileDirectories[_currentImageIndex][TiffTag.AegisAttributes][0].ToString(), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                foreach (KeyValuePair<String, Object> attribute in attributes as IDictionary<String, Object>)
                {
                    metadata[attribute.Key] = attribute.Value;
                }
            }

            return metadata;
        }

        /// <summary>
        /// Computes the raster mapper.
        /// </summary>
        /// <returns>The raster mapper.</returns>
        protected virtual RasterMapper ComputeRasterMapper()
        {
            return null;
        }

        /// <summary>
        /// Computes the reference system of the geometry.
        /// </summary>
        /// <returns>The reference system of the geometry.</returns>
        protected virtual IReferenceSystem ComputeReferenceSystem()
        {
            return null;
        }

        #endregion
    }
}
