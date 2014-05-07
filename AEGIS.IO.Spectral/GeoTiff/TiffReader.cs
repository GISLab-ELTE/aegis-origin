/// <copyright file="TiffReader.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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
    // short expression for the IFD table
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

        protected List<TiffImageFileDirectory> _imageFileDirectories;
        protected Int32 _subImageIndex;

        #endregion

        #region Private fields

        private ByteOrder _byteOrder;
        private UInt32 _imageFileDirectoryOffset;

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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
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
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
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
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
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
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
        protected TiffReader(Stream stream, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, format, parameters)
        {
            ReadHeader();
        }

        #endregion

        #region GeometryStreamReader protected methods

        /// <summary>
        /// Returns a value indicating whether the end of the stream is reached.
        /// </summary>
        /// <returns><c>true</c> if the end of the stream is reached; otherwise <c>false</c>.</returns>
        protected override Boolean GetEndOfStream() { return _subImageIndex == _imageFileDirectories.Count; }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        protected override IGeometry ApplyReadGeometry()
        {
            if (_imageFileDirectories[_subImageIndex][258].Min() != _imageFileDirectories[_subImageIndex][258].Max())
            {
                throw new InvalidDataException("Stream format is not supported.");
            }

            try
            {
                IReferenceSystem referenceSystem = ReadGeometryReferenceSystem();
                IRaster raster = ReadRasterContent(referenceSystem);
                IDictionary<String, Object> metadata = ReadGeometryMetadata();
                
                _subImageIndex++;

                return ResolveFactory(referenceSystem).CreateSpectralPolygon(raster, metadata);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Stream content is invalid.", ex);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads the header.
        /// </summary>
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
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
                if (EndianBitConverter.ToUInt16(bytes, 2, _byteOrder) != 42)
                {
                    throw new InvalidDataException("Stream header content is invalid.");
                }

                _imageFileDirectories = new List<TiffImageFileDirectory>();
                _imageFileDirectories.Add(new TiffImageFileDirectory());

                try
                {
                    ReadImageFileDirectory(EndianBitConverter.ToUInt32(bytes, 4, _byteOrder));
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException("Stream header content is invalid.", ex);
                }

                _baseStream.Seek(4, SeekOrigin.Begin);
                _subImageIndex = 0;
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream reading.", ex);
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
            _imageFileDirectoryOffset = offset;
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
                dataSize = GetTagSize(entryType);

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
                            dataArray[dataIndex] = GetTagValue(entryType, dataBytes, dataIndex * dataSize, _byteOrder);
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
                            dataArray[dataIndex] = GetTagValue(entryType, bytes, entryIndex * 12 + 8 + dataIndex * dataSize, _byteOrder);
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
        /// Reads the raster of the geometry.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The raster of the geometry.</returns>
        /// <exception cref="System.NotSupportedException">Compression is not supported.</exception>
        private IRaster ReadRasterContent(IReferenceSystem referenceSystem)
        {
            // read raster 
            TiffPhotometricInterpretation photometricInterpretation = (TiffPhotometricInterpretation)Convert.ToUInt16(_imageFileDirectories[_subImageIndex][262][0]);
            TiffCompression compression = (TiffCompression)Convert.ToUInt16(_imageFileDirectories[_subImageIndex][259][0]);

            if (compression != TiffCompression.None)
                throw new NotSupportedException("Compression is not supported.");

            Int32 imageLength = Convert.ToInt32(_imageFileDirectories[_subImageIndex][257][0]);
            Int32 imageWidth = Convert.ToInt32(_imageFileDirectories[_subImageIndex][256][0]);

            SpectralRange[] spectralRanges = ReadRasterSpectralRangeData();
            Int32[] radiometricResolutions = ReadRasterRadiometricResolutionData();
            RasterMapper mapper = ReadRasterToModelSpaceMapping();

            // check the sample format
            TiffSampleFormat format = TiffSampleFormat.UnsignedInteger;
            if (_imageFileDirectories[_subImageIndex].ContainsKey(339))
            {
                format = (TiffSampleFormat)Convert.ToUInt16(_imageFileDirectories[_subImageIndex][339][0]);
            }

            IRaster raster = null;

            // create the raster based on the format
            switch (format)
            { 
                case TiffSampleFormat.UnsignedInteger:
                case TiffSampleFormat.Undefined:
                    raster = ResolveFactory(referenceSystem).GetFactory<ISpectralGeometryFactory>().GetFactory<IRasterFactory>().CreateRaster(radiometricResolutions.Length, imageLength, imageWidth, radiometricResolutions, spectralRanges, mapper);
                    break;
                case TiffSampleFormat.SignedInteger:
                case TiffSampleFormat.Floating:
                    raster = ResolveFactory(referenceSystem).GetFactory<ISpectralGeometryFactory>().GetFactory<IRasterFactory>().CreateFloatRaster(radiometricResolutions.Length, imageLength, imageWidth, radiometricResolutions, spectralRanges, mapper);
                    break;
            }

            // determine whether the image is stripped or tiled
            if (_imageFileDirectories[_subImageIndex].ContainsKey(273))
            {
                ReadRasterContentFromStrips(raster, compression, format);
            }

            if (_imageFileDirectories[_subImageIndex].ContainsKey(324))
            {
                ReadRasterContentFromTiles(raster, compression, format);
            }
              
            return raster;
        }

        /// <summary>
        /// Reads the raster of the geometry stored in strips.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="compression">The compression.</param>
        /// <param name="format">The format.</param>
        /// <exception cref="System.NotSupportedException">Compression is not supported.</exception>
        private void ReadRasterContentFromStrips(IRaster raster, TiffCompression compression, TiffSampleFormat format)
        {
            Int32 rowsPerStrip = Convert.ToInt32(_imageFileDirectories[_subImageIndex][278][0]);
            Int32 numberOfStrips = (Int32)Math.Ceiling((Single)raster.NumberOfRows / rowsPerStrip);
            UInt32[] stripOffsets = _imageFileDirectories[_subImageIndex][273].Cast<UInt32>().ToArray();
            UInt32[] stripByteCounts = _imageFileDirectories[_subImageIndex][279].Cast<UInt32>().ToArray();
            
            // fit the maximum number of readable bytes to the radiometric resolution
            UInt32 numberOfReadableBytes = NumberOfReadableBytes;
            while (numberOfReadableBytes * 8 % raster.RadiometricResolutions.Sum() != 0)
                numberOfReadableBytes--;

            // read the stripes
            Int32 rowIndex = 0;
            Int32 columnIndex = 0;
            for (Int32 stripIndex = 0; stripIndex < numberOfStrips; stripIndex++)
            {
                _baseStream.Seek(stripOffsets[stripIndex], SeekOrigin.Begin);

                // perform decompression of stream if compressed
                Stream contentStream = null;
                switch (compression)
                {
                    default:
                        contentStream = _baseStream;
                        break;
                }

                // should not load more bytes into the memory than suggested
                Byte[] stripBytes = new Byte[stripByteCounts[stripIndex] < numberOfReadableBytes ? stripByteCounts[stripIndex] : numberOfReadableBytes];
                Int32 numberOfBytesLeft = (Int32)stripByteCounts[stripIndex];
                Int32 byteIndex = 0, bitIndex = 0;

                contentStream.Read(stripBytes, 0, stripBytes.Length);

                // multiple iterations may be required for large strips
                while (numberOfBytesLeft > 0 && rowIndex < raster.NumberOfRows)
                {
                    // perform reading based on format
                    switch (format)
                    {
                        case TiffSampleFormat.Undefined:
                        case TiffSampleFormat.UnsignedInteger:
                            raster.SetValues(rowIndex, columnIndex, ReadUnsignedIntegerValues(stripBytes, ref byteIndex, ref bitIndex, raster.RadiometricResolutions));
                            break;
                        case TiffSampleFormat.SignedInteger:
                            (raster as IFloatRaster).SetValues(rowIndex, columnIndex, ReadSignedIntegerValues(stripBytes, ref byteIndex, ref bitIndex, raster.RadiometricResolutions));
                            break;
                        case TiffSampleFormat.Floating:
                            (raster as IFloatRaster).SetValues(rowIndex, columnIndex, ReadFloatValues(stripBytes, ref byteIndex, ref bitIndex, raster.RadiometricResolutions));
                            break;
                    }

                    columnIndex++;
                    if (columnIndex == raster.NumberOfColumns)
                    {
                        columnIndex = 0;
                        rowIndex++;

                        if (rowIndex == raster.NumberOfRows)
                            break;
                    }

                    if (byteIndex == stripBytes.Length)
                    {
                        numberOfBytesLeft -= stripBytes.Length;
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
        /// <param name="compression">The compression.</param>
        /// <param name="format">The format.</param>
        /// <exception cref="System.NotSupportedException">Compression is not supported.</exception>
        private void ReadRasterContentFromTiles(IRaster raster, TiffCompression compression, TiffSampleFormat format)
        {
            UInt32[] tileOffsets = _imageFileDirectories[_subImageIndex][324].Cast<UInt32>().ToArray();
            UInt32[] tileByteCounts = _imageFileDirectories[_subImageIndex][325].Cast<UInt32>().ToArray();
            Int32 tileWidth = Convert.ToInt32(_imageFileDirectories[_subImageIndex][322][0]);
            Int32 tileLength = Convert.ToInt32(_imageFileDirectories[_subImageIndex][323][0]);

            Int32 tilesAcross = (raster.NumberOfColumns + tileWidth - 1) / tileWidth;
            Int32 tilesDown = (raster.NumberOfRows + tileLength - 1) / tileLength;

            Int32 rowIndex = 0;
            Int32 columnIndex = 0;

            // reading the tiles
            for (Int32 i = 0; i < tilesDown; i++)
                for (Int32 j = 0; j < tilesAcross; j++)
                {
                    _baseStream.Seek(tileOffsets[i * tilesAcross + j], SeekOrigin.Begin);

                    // perform decompression of stream if compressed
                    Stream contentStream = null;
                    switch (compression)
                    {
                        default:
                            contentStream = _baseStream;
                            break;
                    }

                    // read a single tile
                    Byte[] tileBytes = new Byte[tileByteCounts[i * tilesAcross + j]];
                    Int32 byteIndex = 0, bitIndex = 0;
                    Int32 byteShift = raster.RadiometricResolutions.Sum() / 8;
                    Int32 bitShift = raster.RadiometricResolutions.Sum() % 8;

                    contentStream.Read(tileBytes, 0, tileBytes.Length);

                    for (Int32 k = 0; k < tileLength; k++)
                        for (Int32 l = 0; l < tileWidth; l++)
                        {
                            rowIndex = i * tileLength + k;
                            columnIndex = j * tileWidth + l;

                            if (rowIndex >= raster.NumberOfRows || columnIndex >= raster.NumberOfColumns) // the tile is larger than the image
                            {
                                byteIndex += byteShift; // the values in the array must be skipped
                                bitIndex += bitShift;
                                continue;
                            }
                               
                            switch (format)
                            {
                                case TiffSampleFormat.Undefined:
                                case TiffSampleFormat.UnsignedInteger:
                                    raster.SetValues(rowIndex, columnIndex, ReadUnsignedIntegerValues(tileBytes, ref byteIndex, ref bitIndex, raster.RadiometricResolutions));
                                    break;
                                case TiffSampleFormat.SignedInteger:
                                    (raster as IFloatRaster).SetValues(rowIndex, columnIndex, ReadSignedIntegerValues(tileBytes, ref byteIndex, ref bitIndex, raster.RadiometricResolutions));
                                    break;
                                case TiffSampleFormat.Floating:
                                    (raster as IFloatRaster).SetValues(rowIndex, columnIndex, ReadFloatValues(tileBytes, ref byteIndex, ref bitIndex, raster.RadiometricResolutions));
                                    break;
                            }
                        }
                }
        }


        /// <summary>
        /// Reads the unsigned integer values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <returns>The unsigned integer values representing a pixel.</returns>
        /// <exception cref="System.NotSupportedException">Radiometric resolution is not supported.</exception>
        private UInt32[] ReadUnsignedIntegerValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, IList<Int32> radiometricResolutions)
        {
            UInt32[] values = new UInt32[radiometricResolutions.Count];

            for (Int32 i = 0; i < values.Length; i++)
            {
                if (radiometricResolutions[i] == 8)
                {
                    values[i] = Convert.ToByte(bytes[byteIndex]);
                    byteIndex++;
                }
                else if (radiometricResolutions[i] == 16)
                {
                    values[i] = EndianBitConverter.ToUInt16(bytes, byteIndex, _byteOrder);
                    byteIndex += 2;
                }
                else if (radiometricResolutions[i] == 32)
                {
                    values[i] = EndianBitConverter.ToUInt32(bytes, byteIndex, _byteOrder);
                    byteIndex += 4;
                }
                else if (radiometricResolutions[i] < 8 && 8 % radiometricResolutions[i] == 0)
                {
                    bitIndex -= radiometricResolutions[i];
                    values[i] = Convert.ToByte(bytes[byteIndex] >> bitIndex);

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

            return values;
        }

        /// <summary>
        /// Reads the signed integer values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <returns>The signed integer values representing a pixel.</returns>
        /// <exception cref="System.NotSupportedException">Radiometric resolution is not supported.</exception>
        private Double[] ReadSignedIntegerValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, IList<Int32> radiometricResolutions)
        {
            Double[] values = new Double[radiometricResolutions.Count];

            for (Int32 i = 0; i < values.Length; i++)
            {
                if (radiometricResolutions[i] == 8)
                {
                    values[i] = Convert.ToSByte(bytes[byteIndex]);
                    byteIndex++;
                }
                else if (radiometricResolutions[i] == 16)
                {
                    values[i] = EndianBitConverter.ToInt16(bytes, byteIndex, _byteOrder);
                    byteIndex += 2;
                }
                else if (radiometricResolutions[i] == 32)
                {
                    values[i] = EndianBitConverter.ToInt32(bytes, byteIndex, _byteOrder);
                    byteIndex += 4;
                }
                else if (radiometricResolutions[i] < 8 && 8 % radiometricResolutions[i] == 0)
                {
                    bitIndex -= radiometricResolutions[i];
                    values[i] = Convert.ToSByte(bytes[byteIndex] >> bitIndex);

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

            return values;
        }

        /// <summary>
        /// Reads the floating point values from the array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <returns>The floating point values representing a pixel.</returns>
        /// <exception cref="System.NotSupportedException">Radiometric resolution is not supported.</exception>
        private Double[] ReadFloatValues(Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, IList<Int32> radiometricResolutions)
        {
            Double[] values = new Double[radiometricResolutions.Count];

            for (Int32 i = 0; i < values.Length; i++)
            {
                if (radiometricResolutions[i] == 32)
                {
                    values[i] = EndianBitConverter.ToSingle(bytes, byteIndex, _byteOrder);
                    byteIndex += 2;
                }
                else if (radiometricResolutions[i] == 64)
                {
                    values[i] = EndianBitConverter.ToDouble(bytes, byteIndex, _byteOrder);
                    byteIndex += 4;
                }
                else
                {
                    // TODO: support other resolutions
                    throw new NotSupportedException("Radiometric resolution is not supported.");
                }
            }

            return values;
        }

        /// <summary>
        /// Reads the spectral range data of the raster.
        /// </summary>
        /// <returns>An array containing the sppectral ranges of the raster.</returns>
        private SpectralRange[] ReadRasterSpectralRangeData()
        {
            TiffPhotometricInterpretation photometricInterpretation = (TiffPhotometricInterpretation)Convert.ToUInt16(_imageFileDirectories[_subImageIndex][262][0]);
            UInt16 samplesPerPixel = 1;
            if (_imageFileDirectories[_subImageIndex].ContainsKey(277))
                samplesPerPixel = Convert.ToUInt16(_imageFileDirectories[_subImageIndex][277][0]);

            SpectralRange[] ranges = null;
            switch (photometricInterpretation)
            {
                case TiffPhotometricInterpretation.RGB:
                    if (samplesPerPixel == 3) 
                        // the RGB interpretation is only correct when there are 3 samples per pixel
                        ranges = SpectralRanges.RGB;
                    else
                        ranges = Enumerable.Repeat<SpectralRange>(null, samplesPerPixel).ToArray();
                    break;
                default:
                    ranges = Enumerable.Repeat<SpectralRange>(null, samplesPerPixel).ToArray();
                    break;
            }
            // TODO: process other interpretations

            return ranges;
        }

        /// <summary>
        /// Reads the radiometric resolutions of the raster image.
        /// </summary>
        /// <returns>An array containing the radiometrix resolutions of the raster image.</returns>
        private Int32[] ReadRasterRadiometricResolutionData()
        {
            UInt16 samplesPerPixel = 1, bitsPerSample = 1;

            if (_imageFileDirectories[_subImageIndex].ContainsKey(277))
                samplesPerPixel = Convert.ToUInt16(_imageFileDirectories[_subImageIndex][277][0]);

            if (_imageFileDirectories[_subImageIndex].ContainsKey(258))
            {
                if (_imageFileDirectories[_subImageIndex][258].Length == samplesPerPixel)
                    return _imageFileDirectories[_subImageIndex][258].Select(value => Convert.ToInt32(value)).ToArray();
                else
                    bitsPerSample = Convert.ToUInt16(_imageFileDirectories[_subImageIndex][258][0]);
            }

            return Enumerable.Repeat<Int32>(bitsPerSample, samplesPerPixel).ToArray();
        }

        #endregion 

        #region Protected methods

        /// <summary>
        /// Reads the metadata of the raster geometry.
        /// </summary>
        /// <returns>A dictionary containing the metadata of he raster geometry.</returns>
        protected virtual IDictionary<String, Object> ReadGeometryMetadata()
        {
            Dictionary<String, Object> metadata = new Dictionary<String, Object>();
            if (_imageFileDirectories[_subImageIndex].ContainsKey(269))
                metadata.Add("DocumentName", _imageFileDirectories[_subImageIndex][269][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(270))
                metadata.Add("ImageDescription", _imageFileDirectories[_subImageIndex][270][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(271))
                metadata.Add("Make", _imageFileDirectories[_subImageIndex][271][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(272))
                metadata.Add("Model", _imageFileDirectories[_subImageIndex][272][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(285))
                metadata.Add("PageName", _imageFileDirectories[_subImageIndex][285][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(305))
                metadata.Add("Software", _imageFileDirectories[_subImageIndex][305][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(306))
                metadata.Add("DateTime", _imageFileDirectories[_subImageIndex][306][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(307))
                metadata.Add("Artist", _imageFileDirectories[_subImageIndex][307][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(308))
                metadata.Add("HostComputer", _imageFileDirectories[_subImageIndex][308][0]);
            if (_imageFileDirectories[_subImageIndex].ContainsKey(33432))
                metadata.Add("Copyright", _imageFileDirectories[_subImageIndex][33432][0]);

            return metadata;
        }     
   
        /// <summary>
        /// Reads the mapping from model space to raster space.
        /// </summary>
        /// <returns>The mapping from model space to raster space.</returns>
        protected virtual RasterMapper ReadRasterToModelSpaceMapping()
        {
            return null;
        }

        /// <summary>
        /// Reads the reference system of the raster geometry.
        /// </summary>
        /// <returns>The reference system of the raster geometry.</returns>
        protected virtual IReferenceSystem ReadGeometryReferenceSystem()
        {
            return null;
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
        /// Returns the value of a TIFF tag.
        /// </summary>
        /// <param name="type">The type of the tag.</param>
        /// <param name="array">The byte array.</param>
        /// <param name="startIndex">The starting index of the value in the array.</param>
        /// <param name="byteOrder">The byte order.</param>
        /// <returns>The value of the tag.</returns>
        private static Object GetTagValue(TiffTagType type, Byte[] array, Int32 startIndex, ByteOrder byteOrder)
        {
            switch (type)
            {
                case TiffTagType.Byte:
                    return array[startIndex];
                case TiffTagType.ASCII:
                    return System.Text.Encoding.ASCII.GetChars(array, startIndex, 1)[0];
                case TiffTagType.Short:
                    return EndianBitConverter.ToUInt16(array, startIndex, byteOrder);
                case TiffTagType.Long:
                    return EndianBitConverter.ToUInt32(array, startIndex, byteOrder);
                case TiffTagType.Rational:
                    return EndianBitConverter.ToRational(array, startIndex, byteOrder);
                case TiffTagType.SByte:
                    return Convert.ToSByte(array[startIndex]);
                case TiffTagType.Undefined:
                    return array[startIndex];
                case TiffTagType.SShort:
                    return EndianBitConverter.ToUInt16(array, startIndex, byteOrder);
                case TiffTagType.SLong:
                    return EndianBitConverter.ToUInt32(array, startIndex, byteOrder);
                case TiffTagType.SRational:
                    return EndianBitConverter.ToRational(array, startIndex, byteOrder);
                case TiffTagType.Float:
                    return EndianBitConverter.ToSingle(array, startIndex, byteOrder);
                case TiffTagType.Double:
                    return EndianBitConverter.ToDouble(array, startIndex, byteOrder);
                default:
                    return null;
            }
        }

        #endregion
    }
}
