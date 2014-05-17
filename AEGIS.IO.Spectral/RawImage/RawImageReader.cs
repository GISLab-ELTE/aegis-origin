/// <copyright file="RawImageReader.cs" company="Eötvös Loránd University (ELTE)">
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
using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.IO.RawImage
{
    /// <summary>
    /// Represents a general raw image format reader.
    /// </summary>
    /// <remarks>
    /// Raw image formats are not in themselfs image format, but are schemes for storing the actual pixel values of an image in a file. 
    /// Band interleaved by line (BIL), band interleaved by pixel (BIP), and band sequential (BSQ) are common methods of organizing image data for multiband images. 
    /// Therefore, no image metadata (width, height, radiometric resolution, etc.) is actually stored in the file, hence these values must be supplied as parameters. 
    /// Also, raw images do not contain reference system information, this must be supplied in addition.
    /// </remarks>
    [IdentifiedObjectInstance("AEGIS::610210", "Generic Raw Image")]
    public class RawImageReader : GeometryStreamReader
    {
        private class RawImageSpectralEntity : ISpectralEntity
        {

            #region Private fields

            private readonly Int32 _spectralResolution;
            private readonly Int32 _numberOfColumns;
            private readonly Int32 _numberOfRows;
            private readonly Int32[] _radiometricResolutions;
            private readonly RasterRepresentation _representation;

            #endregion

            #region ISpectralEntity properties

            /// <summary>
            /// Gets the number of columns.
            /// </summary>
            /// <value>The number of spectral values contained in a row.</value>
            public Int32 NumberOfColumns { get { return _spectralResolution; } }

            /// <summary>
            /// Gets the number of rows.
            /// </summary>
            /// <value>The number of spectral values contained in a column.</value>
            public Int32 NumberOfRows { get { return _numberOfRows; } }

            /// <summary>
            /// Gets the spectral resolution of the dataset.
            /// </summary>
            /// <value>The number of spectral bands contained in the dataset.</value>
            public Int32 SpectralResolution { get { return _spectralResolution; } }

            /// <summary>
            /// Gets the radiometric resolutions of the bands in the raster.
            /// </summary>
            /// <value>The list containing the radiometric resolution of each band in the raster.</value>
            public IList<Int32> RadiometricResolutions { get { return _radiometricResolutions; } }

            /// <summary>
            /// Gets a value indicating whether the dataset is readable.
            /// </summary>
            /// <value><c>true</c> if the dataset is readable; otherwise, <c>false</c>.</value>
            public Boolean IsReadable { get { return true; } }

            /// <summary>
            /// Gets a value indicating whether the dataset is writable.
            /// </summary>
            /// <value><c>true</c> if the dataset is writable; otherwise, <c>false</c>.</value>
            public Boolean IsWritable { get { return false; } }

            /// <summary>
            /// Gets the representation of the dataset.
            /// </summary>
            /// <value>The representation of the dataset.</value>
            public RasterRepresentation Representation { get { return _representation; } }

            /// <summary>
            /// Gets the supported read/write orders.
            /// </summary>
            /// <value>The list of supported read/write orders.</value>
            public IList<SpectralDataOrder> SupportedOrders { get { return Array.AsReadOnly(_supportedOrders); } }

            #endregion

            #region Constructor

            public RawImageSpectralEntity(RawImageReader rawImageReader)
            {
                _spectralResolution = rawImageReader._spectralResolution;
                _numberOfColumns = rawImageReader._numberOfColumns;
                _numberOfRows = rawImageReader._numberOfRows;
            }

            #endregion

            #region ISpectralEntity methods for reading integer value
            public UInt32 ReadValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
            {
                return 0;
            }
            
            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                return null;
            }

            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues, SpectralDataOrder readOrder)
            {
                return null;
            }

            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                return null;
            }

            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, SpectralDataOrder readOrder)
            {
                return null;
            }


            #endregion

            #region ISpectralEntity methods for reading float value

            public Double ReadFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex) 
            {
                return 0;
            }

            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                return null;
            }

            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues, SpectralDataOrder readOrder)
            {
                return null;
            }

            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                return null;
            }

            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, SpectralDataOrder readOrder)
            {
                return null;
            }




            #endregion

            #region ISpectralEntity methods for writing integer values (explicit)

            /// <summary>
            /// Writes the specified spectral value to the dataset.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            /// <summary>
            /// Writes the specified spectral values to the dataset in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 startIndex, UInt32[] spectralValues)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            /// <summary>
            /// Writes the specified spectral values to the dataset in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 startIndex, UInt32[] spectralValues, SpectralDataOrder writeOrder)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the dataset in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the dataset in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues, SpectralDataOrder writeOrder)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            #endregion

            #region ISpectralEntity methods for writing integer values (explicit)

            /// <summary>
            /// Writes the specified spectral value to the dataset.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            /// <summary>
            /// Writes the specified spectral values to the dataset in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 startIndex, Double[] spectralValues)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            /// <summary>
            /// Writes the specified spectral values to the dataset in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 startIndex, Double[] spectralValues, SpectralDataOrder writeOrder)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the dataset in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the dataset in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues, SpectralDataOrder writeOrder)
            {
                throw new NotSupportedException("The dataset is not writable.");
            }

            #endregion

            private static SpectralDataOrder[] _supportedOrders;
        }

        #region Private constant fields

        /// <summary>
        /// The maximum number of bytes to be read from the file in a single operation. This field is constant.
        /// </summary>
        private const Int32 MaxReadableByteCount = 1 << 24;

        #endregion

        #region Private fields

        protected ByteOrder _byteOrder;
        protected RawImageLayout _layout;
        protected Int32 _numberOfRows;
        protected Int32 _numberOfColumns;
        protected Int32 _spectralResolution;
        protected Int32 _radiometricResolution;
        protected Int32 _bytesGapPerBand;
        protected Int32 _bytesPerBandRow;
        protected Int32 _bytesPerRow;
        protected Int32 _bytesSkipped;
        protected RasterMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
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
        /// <exception cref="System.IO.IOException">Exception occured during stream opening.</exception>
        public RawImageReader(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GenericRawImage, parameters)
        {
            SetParameters(parameters);   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
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
        /// <exception cref="System.IO.IOException">Exception occured during stream opening.</exception>
        public RawImageReader(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GenericRawImage, parameters)
        {
            SetParameters(parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageReader" /> class.
        /// </summary>
        /// <param name="path">The stream to be read.</param>
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
        public RawImageReader(Stream stream, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, SpectralGeometryStreamFormats.GenericRawImage, parameters)
        {
            SetParameters(parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="factory">The factory used for geometry production.</param>
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
        protected RawImageReader(String path, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, format, parameters)
        {
            if (parameters != null)
            {
                SetParameters(parameters);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageReader" /> class.
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
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        protected RawImageReader(Uri path, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, format, parameters)
        {
            if (parameters != null)
            {
                SetParameters(parameters);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageReader" /> class.
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
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
        protected RawImageReader(Stream stream, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, format, parameters)
        {
            if (parameters != null)
            {
                SetParameters(parameters);
            }
        }

        #endregion

        #region Protected GeometryStreamReader methods

        /// <summary>
        /// Returns a value indicating whether the end of the stream is reached.
        /// </summary>
        /// <returns><c>true</c> if the end of the stream is reached; otherwise, <c>false</c>.</returns>
        protected override Boolean GetEndOfStream() { return _baseStream.Position == _baseStream.Length; }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        protected override IGeometry ApplyReadGeometry()
        {
            try
            {
                IReferenceSystem referenceSystem = ReadGeometryReferenceSystem();
                IRaster raster = ReadRasterContent(referenceSystem);
                IDictionary<String, Object> metadata = ReadGeometryMetadata();

                return ResolveFactory(referenceSystem).CreateSpectralPolygon(raster.Mapper, metadata); //!!!
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Stream content is invalid.", ex);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the parameters of the format.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        private void SetParameters(IDictionary<GeometryStreamParameter, Object> parameters)
        {
            _layout = ResolveParameter<RawImageLayout>(SpectralGeometryStreamParameters.Layout);
            _numberOfRows = ResolveParameter<Int32>(SpectralGeometryStreamParameters.NumerOfRows);
            _numberOfColumns = ResolveParameter<Int32>(SpectralGeometryStreamParameters.NumberOfColumns);
            _spectralResolution = ResolveParameter<Int32>(SpectralGeometryStreamParameters.SpectralResolution);
            _radiometricResolution = ResolveParameter<Int32>(SpectralGeometryStreamParameters.RadiometricResolution);
            _byteOrder = ResolveParameter<ByteOrder>(SpectralGeometryStreamParameters.SpectralResolution);
            _bytesGapPerBand = ResolveParameter<Int32>(SpectralGeometryStreamParameters.BytesGapPerBand);
            _bytesPerBandRow = IsProvidedParameter(SpectralGeometryStreamParameters.BytesPerBandRow) ? 
                               ResolveParameter<Int32>(SpectralGeometryStreamParameters.BytesPerBandRow) : 
                               Convert.ToInt32(Math.Ceiling(_numberOfColumns * _radiometricResolution / 8.0));
            _bytesPerRow = IsProvidedParameter(SpectralGeometryStreamParameters.BytesPerRow) ? 
                           ResolveParameter<Int32>(SpectralGeometryStreamParameters.BytesPerRow) : 
                           (_layout == RawImageLayout.BandInterlevedByLine ? _spectralResolution * _bytesGapPerBand : (Int32)Math.Ceiling(_numberOfRows * _numberOfColumns * _radiometricResolution / 8.0));
            _bytesSkipped = ResolveParameter<Int32>(SpectralGeometryStreamParameters.BytesSkipped);

            // set raster mapping if all parameters are available
            if (parameters.ContainsKey(SpectralGeometryStreamParameters.TieCoordinate) && parameters.ContainsKey(SpectralGeometryStreamParameters.ColumnDimension) && parameters.ContainsKey(SpectralGeometryStreamParameters.RowDimension))
                _mapper = RasterMapper.FromTransformation(ResolveParameter<Coordinate>(SpectralGeometryStreamParameters.TieCoordinate).X,
                                                          ResolveParameter<Coordinate>(SpectralGeometryStreamParameters.TieCoordinate).Y,
                                                          0,
                                                          ResolveParameter<Double>(SpectralGeometryStreamParameters.ColumnDimension),
                                                          ResolveParameter<Double>(SpectralGeometryStreamParameters.RowDimension),
                                                          1,
                                                          RasterMapMode.ValueIsCoordinate);
        }

        /// <summary>
        /// Reads the raster of the geometry.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The raster of the geometry.</returns>
        private IRaster ReadRasterContent(IReferenceSystem referenceSystem)
        {
            IRaster raster = ResolveFactory(referenceSystem).GetFactory<ISpectralGeometryFactory>().GetFactory<IRasterFactory>().CreateRaster(_spectralResolution, _numberOfRows, _numberOfColumns, _radiometricResolution, null, _mapper);

            _baseStream.Seek(_bytesSkipped, SeekOrigin.Begin);

            Int32 numberOfStrips;

            switch (_layout)
            {
                case RawImageLayout.BandInterlevedByLine:
                case RawImageLayout.BandInterlevedByPixel:
                    // prevent from reading too much bytes into the memory at once
                    numberOfStrips = _bytesPerBandRow / MaxReadableByteCount;

                    for (Int32 i = 0; i < numberOfStrips; i++)
                    {
                        ReadStrip(MaxReadableByteCount, raster);
                    }
                    ReadStrip(_bytesPerRow - numberOfStrips * MaxReadableByteCount, raster);
                    break;
                case RawImageLayout.BandSequential:
                    // prevent from reading too much bytes into the memory at once
                    numberOfStrips = _bytesPerRow / MaxReadableByteCount;

                    for (Int32 i = 0; i < numberOfStrips; i++)
                    {
                        ReadStrip(MaxReadableByteCount, raster);
                    }
                    ReadStrip(_bytesPerRow - numberOfStrips * MaxReadableByteCount, raster);
                    break;
            }

            return raster;
        }
        /// <summary>
        /// Reads a strip from the file.
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes in the strip.</param>
        /// <param name="raster">The raster.</param>
        private void ReadStrip(Int32 numberOfBytes, IRaster raster)
        {
            Byte[] stripBytes = new Byte[numberOfBytes];

            _baseStream.Read(stripBytes, 0, stripBytes.Length);
            Int32 numberOfBytesLeft = _numberOfRows * _numberOfColumns * _radiometricResolution * _spectralResolution, byteIndex = 0, bitIndex = 8;

            switch (_layout)
            {
                case RawImageLayout.BandInterlevedByLine:
                    for (Int32 rowIndex = 0; rowIndex < _numberOfRows; rowIndex++)
                    {
                        for (Int32 bandIndex = 0; bandIndex < _spectralResolution; bandIndex++)
                        {
                            for (Int32 columnIndex = 0; columnIndex < _numberOfColumns; columnIndex++)
                            {
                                ReadValue(stripBytes, ref byteIndex, ref bitIndex, raster, rowIndex, columnIndex, bandIndex);

                                if (byteIndex == stripBytes.Length)
                                {
                                    _baseStream.Read(stripBytes, 0, stripBytes.Length);
                                    byteIndex = 0;
                                }
                            }
                        }
                    }
                    break;
                case RawImageLayout.BandInterlevedByPixel:
                    for (Int32 rowIndex = 0; rowIndex < _numberOfRows; rowIndex++)
                    {
                        for (Int32 columnIndex = 0; columnIndex < _numberOfColumns; columnIndex++)
                        {
                            for (Int32 bandIndex = 0; bandIndex < _spectralResolution; bandIndex++)
                            {
                                ReadValue(stripBytes, ref byteIndex, ref bitIndex, raster, rowIndex, columnIndex, bandIndex);

                                if (byteIndex == stripBytes.Length)
                                {
                                    _baseStream.Read(stripBytes, 0, stripBytes.Length);
                                    byteIndex = 0;
                                }
                            }
                        }
                    }
                    break;
                case RawImageLayout.BandSequential:
                    for (Int32 bandIndex = 0; bandIndex < _spectralResolution; bandIndex++)
                    {
                        for (Int32 rowIndex = 0; rowIndex < _numberOfRows; rowIndex++)
                        {
                            for (Int32 columnIndex = 0; columnIndex < _numberOfColumns; columnIndex++)
                            {
                                ReadValue(stripBytes, ref byteIndex, ref bitIndex, raster, rowIndex, columnIndex, bandIndex);

                                if (byteIndex == stripBytes.Length)
                                {
                                    _baseStream.Read(stripBytes, 0, stripBytes.Length);
                                    byteIndex = 0;
                                }
                            }
                        }
                        // skip the bytes between the bands
                        _baseStream.Seek(_bytesGapPerBand, SeekOrigin.Current);
                    }
                    break;
            }
        }
        /// <summary>
        /// Reads the value from the array to a raster.
        /// </summary>
        /// <param name="stripBytes">The strip bytes.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="raster">The raster.</param>
        /// <param name="rowIndex">The zero-based row index in the raster.</param>
        /// <param name="columnIndex">The zero-based column index in the raster.</param>
        /// <param name="bandIndex">The zero-based band index in the raster.</param>
        private void ReadValue(Byte[] stripBytes, ref Int32 byteIndex, ref Int32 bitIndex, IRaster raster, Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (_radiometricResolution < 8 && 8 % _radiometricResolution == 0)
            {
                bitIndex -= _radiometricResolution;
                raster.SetValue(rowIndex, columnIndex, bandIndex, Convert.ToByte(stripBytes[byteIndex] >> bitIndex));

                if (bitIndex == 0)
                {
                    bitIndex = 8;
                    byteIndex++;
                }
            }
            else if (_radiometricResolution == 8)
            {
                raster.SetValue(rowIndex, columnIndex, bandIndex, Convert.ToByte(stripBytes[byteIndex]));
                byteIndex++;
            }
            else if (_radiometricResolution == 16)
            {
                raster.SetValue(rowIndex, columnIndex, bandIndex, EndianBitConverter.ToUInt16(stripBytes, byteIndex, _byteOrder));
                byteIndex += 2;
            }
            else if (_radiometricResolution == 32)
            {
                raster.SetValue(rowIndex, columnIndex, bandIndex, EndianBitConverter.ToUInt32(stripBytes, byteIndex, _byteOrder));
                byteIndex += 4;
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Reads the reference system of the raster geometry.
        /// </summary>
        /// <returns>The reference system of the raster geometry.</returns>
        protected virtual IReferenceSystem ReadGeometryReferenceSystem()
        {
            return null;
        }
        /// <summary>
        /// Reads the metadata of the raster geometry.
        /// </summary>
        /// <returns>A dictionary containing the metadata of he raster geometry.</returns>
        protected virtual IDictionary<String, Object> ReadGeometryMetadata()
        {
            return null;
        }


        #endregion
    }
}
