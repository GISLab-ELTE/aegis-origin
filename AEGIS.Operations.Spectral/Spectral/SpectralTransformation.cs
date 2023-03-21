// <copyright file="SpectralTransformation.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a transformation that is applied on the spectral data of the geometry.
    /// </summary>
    public abstract class SpectralTransformation : Operation<ISpectralGeometry, ISpectralGeometry>
    {
        #region Private types

        /// <summary>
        /// Represents the raster service of the spectral transformation.
        /// </summary>
        private class SpectralTransformationService : IRasterService
        {
            #region Private constant fields

            /// <summary>
            /// The maximal number of values that can be read. This field is constant.
            /// </summary>
            private const Int32 MaximumNumberOfValues = 2 << 20;

            #endregion

            #region Private fields

            /// <summary>
            /// The operation computing the spectral values of the entity. This field is read-only.
            /// </summary>
            private readonly SpectralTransformation _operation;

            #endregion

            #region IRasterService properties

            /// <summary>
            /// Gets the dimensions of the raster.
            /// </summary>
            /// <value>
            /// The dimensions of the raster.
            /// </value>
            public RasterDimensions Dimensions { get; }

            /// <summary>
            /// Gets the format of the service.
            /// </summary>
            /// <value>The format of the service.</value>
            public RasterFormat Format { get; }

            /// <summary>
            /// Gets a value indicating whether the service is readable.
            /// </summary>
            /// <value><c>true</c> if the service is readable; otherwise, <c>false</c>.</value>
            public Boolean IsReadable { get { return true; } }

            /// <summary>
            /// Gets a value indicating whether the service is writable.
            /// </summary>
            /// <value><c>true</c> if the service is writable; otherwise, <c>false</c>.</value>
            public Boolean IsWritable { get { return false; } }

            /// <summary>
            /// Gets the data order of the service.
            /// </summary>
            /// <value>The data order of the service.</value>
            public RasterDataOrder DataOrder { get { return RasterDataOrder.Unspecified; } }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SpectralTransformationService" /> class.
            /// </summary>
            /// <param name="format">The format.</param>
            /// <param name="operation">The operation.</param>
            /// <param name="numberOfBands">The number of spectral bands.</param>
            /// <param name="numberOfRows">The number of rows.</param>
            /// <param name="numberOfColumns">The number of columns.</param>
            /// <param name="radiometricResolutions">The radiometric resolutions.</param>
            public SpectralTransformationService(SpectralTransformation operation, RasterFormat format, RasterDimensions dimensions)
            {
                _operation = operation;

                Dimensions = dimensions;
                Format = format;
            }

            #endregion

            #region IRasterService methods for reading integer values

            /// <summary>
            /// Reads the specified spectral value from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the value.</param>
            /// <param name="columnIndex">The zero-based column index of the value.</param>
            /// <param name="bandIndex">The zero-based band index of the value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            public UInt32 ReadValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
            {
                return _operation.Compute(rowIndex, columnIndex, bandIndex);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <returns>The array containing the sequence of values in the default order of the service.</returns>
            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                return ReadValueSequence(startIndex, numberOfValues, RasterDataOrder.RowColumnBand);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                // compute the row/column/band indices from the start index
                Int32 columnIndex = 0, rowIndex = 0, bandIndex = 0;
                rowIndex = startIndex / (Dimensions.NumberOfColumns * Dimensions.NumberOfBands);
                columnIndex = (startIndex - rowIndex * Dimensions.NumberOfColumns * Dimensions.NumberOfBands) / Dimensions.NumberOfBands;
                bandIndex = startIndex - rowIndex * Dimensions.NumberOfColumns * Dimensions.NumberOfBands - columnIndex * Dimensions.NumberOfBands;

                return ReadValueSequence(rowIndex, columnIndex, bandIndex, numberOfValues, readOrder);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <returns>The array containing the sequence of values in the default order of the service.</returns>
            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                return ReadValueSequence(rowIndex, columnIndex, bandIndex, numberOfValues, RasterDataOrder.RowColumnBand);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                // there may be not enough values, or the number may be greater than the maximum allowed
                UInt32[] values = new UInt32[Calculator.Min(MaximumNumberOfValues, numberOfValues, (Dimensions.NumberOfRows - rowIndex) * Dimensions.NumberOfColumns * Dimensions.NumberOfBands + (Dimensions.NumberOfColumns - columnIndex) * Dimensions.NumberOfBands + Dimensions.NumberOfBands - bandIndex)];

                Int32 currentIndex = 0;
                while (currentIndex < values.Length)
                { 
                    // read the specified pixel
                    UInt32[] currentValues = _operation.Compute(rowIndex, columnIndex);
                    Array.Copy(currentValues, bandIndex, values, currentIndex, Math.Min(currentValues.Length - bandIndex, values.Length - currentIndex));

                    // change indices for the next pixel
                    bandIndex = 0;
                    columnIndex++;
                    if (columnIndex == Dimensions.NumberOfColumns) { columnIndex = 0; rowIndex++; }

                    currentIndex += currentValues.Length - bandIndex;
                }

                return values;
            }

            #endregion

            #region IRasterService methods for reading floating point values

            /// <summary>
            /// Reads the specified spectral value from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the value.</param>
            /// <param name="columnIndex">The zero-based column index of the value.</param>
            /// <param name="bandIndex">The zero-based band index of the value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            public Double ReadFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
            {
                return _operation.ComputeFloat(rowIndex, columnIndex, bandIndex);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <returns>The array containing the sequence of values in the default order of the service.</returns>
            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                return ReadFloatValueSequence(startIndex, numberOfValues, RasterDataOrder.RowColumnBand);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                // compute the row/column/band indices from the start index
                Int32 columnIndex = 0, rowIndex = 0, bandIndex = 0;
                rowIndex = startIndex / (Dimensions.NumberOfColumns * Dimensions.NumberOfBands);
                columnIndex = (startIndex - rowIndex * Dimensions.NumberOfColumns * Dimensions.NumberOfBands) / Dimensions.NumberOfBands;
                bandIndex = startIndex - rowIndex * Dimensions.NumberOfColumns * Dimensions.NumberOfBands - columnIndex * Dimensions.NumberOfBands;

                return ReadFloatValueSequence(rowIndex, columnIndex, bandIndex, numberOfValues, readOrder);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                return ReadFloatValueSequence(rowIndex, columnIndex, bandIndex, numberOfValues, RasterDataOrder.RowColumnBand);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <returns>The array containing the sequence of values in the default order of the service.</returns>
            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                // there may be not enough values, or the number may be greater than the maximum allowed
                Double[] values = new Double[Calculator.Min(MaximumNumberOfValues, numberOfValues, (Dimensions.NumberOfRows - rowIndex) * Dimensions.NumberOfColumns * Dimensions.NumberOfBands + (Dimensions.NumberOfColumns - columnIndex) * Dimensions.NumberOfBands + Dimensions.NumberOfBands - bandIndex)];

                Int32 currentIndex = 0;
                while (currentIndex < values.Length)
                {
                    // read the specified pixel
                    Double[] currentValues = _operation.ComputeFloat(rowIndex, columnIndex);
                    Array.Copy(currentValues, bandIndex, values, currentIndex, Math.Min(currentValues.Length - bandIndex, values.Length - currentIndex));

                    // change indices for the next pixel
                    bandIndex = 0;
                    columnIndex++;
                    if (columnIndex == Dimensions.NumberOfColumns) { columnIndex = 0; rowIndex++; }

                    currentIndex += currentValues.Length - bandIndex;
                }

                return values;
            }

            #endregion

            #region IRasterService methods for writing integer values (explicit)

            /// <summary>
            /// Writes the specified spectral value to the service.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValueSequence(Int32 startIndex, UInt32[] spectralValues)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValueSequence(Int32 startIndex, UInt32[] spectralValues, RasterDataOrder writeOrder)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues, RasterDataOrder writeOrder)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            #endregion

            #region IRasterService methods for writing float values (explicit)

            /// <summary>
            /// Writes the specified spectral value to the service.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValueSequence(Int32 startIndex, Double[] spectralValues)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValueSequence(Int32 startIndex, Double[] spectralValues, RasterDataOrder writeOrder)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues, RasterDataOrder writeOrder)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// A value indicating whether the properties of the result are set.
        /// </summary>
        private Boolean _resultPropertiesSet;

        /// <summary>
        /// The geometry of the result.
        /// </summary>
        private IGeometry _resultGeometry;

        /// <summary>
        /// The raster format of the result.
        /// </summary>
        private RasterFormat _resultFormat;

        /// <summary>
        /// The raster dimensions of the result.
        /// </summary>
        private RasterDimensions _resultDimensions;

        /// <summary>
        /// The raster mapper of the result.
        /// </summary>
        private RasterMapper _resultMapper;

        /// <summary>
        /// The raster presentation of the result.
        /// </summary>
        private RasterPresentation _resultPresentation;

        /// <summary>
        /// The raster imaging of the result.
        /// </summary>
        private RasterImaging _resultImaging;

        #endregion

        #region Protected properties

        /// <summary>
        /// The geometry factory.
        /// </summary>
        protected IGeometryFactory Factory { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralTransformation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source is invalid.
        /// or
        /// The target is invalid.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        protected SpectralTransformation(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            if (source.Raster == null)
                throw new ArgumentException("The source is invalid.", nameof(source), new InvalidOperationException("The geometry does not contain raster data."));
            if (!method.SupportedFormats.Contains(source.Raster.Format))
                throw new ArgumentException("The source is invalid.", nameof(source), new InvalidOperationException("The raster format is not supported by the method."));
            if (target != null && target.Raster == null)
                throw new ArgumentException("The target is invalid.", nameof(source), new InvalidOperationException("The geometry does not contain raster data."));

            Factory = ResolveParameter<IGeometryFactory>(CommonOperationParameters.GeometryFactory, Source.Factory);

            if (Factory.GetFactory<IRasterFactory>() == null)
                Factory = Source.Factory;

            _resultPropertiesSet = false;
        }

        #endregion
                
        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            SetResultProperties(Source.Raster.Format, Source.Raster.Dimensions, Source.Raster.Mapper, Source.Presentation, Source.Imaging);

            // raster
            IRaster raster;
            if (State == OperationState.Initialized)
            {
                raster = Factory.GetFactory<IRasterFactory>().CreateRaster(new SpectralTransformationService(this, _resultFormat, _resultDimensions), _resultMapper);
            }
            else
            {
                raster = Factory.GetFactory<IRasterFactory>().CreateRaster(_resultFormat, _resultDimensions.NumberOfBands, _resultDimensions.NumberOfRows, _resultDimensions.NumberOfColumns, _resultDimensions.RadiometricResolution, _resultMapper);
            }

            // geometry
            IGeometry geometry;
            if (_resultGeometry != null)
            {
                geometry = _resultGeometry;
            }
            else if (Source.Raster.Dimensions.Equals(_resultDimensions) && (Source.Raster.Mapper != null && _resultMapper != null && Source.Raster.Mapper.Equals(_resultMapper)))
            {
                geometry = Source;
            }
            else
            {
                if (_resultMapper == null)
                {
                    geometry = Source.Factory.CreatePolygon(new Coordinate(0, 0),
                                                           new Coordinate(_resultDimensions.NumberOfRows, 0),
                                                           new Coordinate(_resultDimensions.NumberOfRows, _resultDimensions.NumberOfColumns),
                                                           new Coordinate(0, _resultDimensions.NumberOfColumns));
                }
                else if (_resultMapper.Mode == RasterMapMode.ValueIsArea)
                {
                    geometry = Source.Factory.CreatePolygon(_resultMapper.MapCoordinate(0, 0),
                                                           _resultMapper.MapCoordinate(_resultDimensions.NumberOfRows - 1, 0),
                                                           _resultMapper.MapCoordinate(_resultDimensions.NumberOfRows - 1, _resultDimensions.NumberOfColumns - 1),
                                                           _resultMapper.MapCoordinate(0, _resultDimensions.NumberOfColumns - 1));
                }
                else
                {
                    geometry = Source.Factory.CreatePolygon(_resultMapper.MapCoordinate(0, 0),
                                                           _resultMapper.MapCoordinate(_resultDimensions.NumberOfRows, 0),
                                                           _resultMapper.MapCoordinate(_resultDimensions.NumberOfRows, _resultDimensions.NumberOfColumns),
                                                           _resultMapper.MapCoordinate(0, _resultDimensions.NumberOfColumns));
                }
            }

            // result
            return Factory.CreateSpectralGeometry(geometry, raster, _resultPresentation, _resultImaging);            
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            if ((Method as SpectralOperationMethod).SpectralDomain.HasFlag(SpectralOperationDomain.Band))
            {
                if (Result.Raster.Format == RasterFormat.Floating)
                {
                    for (Int32 bandIndex = 0; bandIndex < Result.Raster.NumberOfBands; bandIndex++)
                        for (Int32 rowIndex = 0; rowIndex < Result.Raster.NumberOfRows; rowIndex++)
                            for (Int32 columnIndex = 0; columnIndex < Result.Raster.NumberOfColumns; columnIndex++)
                                Result.Raster.SetFloatValue(rowIndex, columnIndex, bandIndex, ComputeFloat(rowIndex, columnIndex, bandIndex));
                }
                else
                {
                    for (Int32 bandIndex = 0; bandIndex < Result.Raster.NumberOfBands; bandIndex++)
                        for (Int32 rowIndex = 0; rowIndex < Result.Raster.NumberOfRows; rowIndex++)
                            for (Int32 columnIndex = 0; columnIndex < Result.Raster.NumberOfColumns; columnIndex++)
                                Result.Raster.SetValue(rowIndex, columnIndex, bandIndex, Compute(rowIndex, columnIndex, bandIndex));
                }
            }
            else
            {
                if (Result.Raster.Format == RasterFormat.Floating)
                {
                    for (Int32 i = 0; i < Result.Raster.NumberOfRows; i++)
                        for (Int32 j = 0; j < Result.Raster.NumberOfColumns; j++)
                            Result.Raster.SetFloatValues(i, j, ComputeFloat(i, j));
                }
                else
                {
                    for (Int32 i = 0; i < Result.Raster.NumberOfRows; i++)
                        for (Int32 j = 0; j < Result.Raster.NumberOfColumns; j++)
                            Result.Raster.SetValues(i, j, Compute(i, j));
                }
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="dimensions">The raster dimensions.</param>
        protected void SetResultProperties(RasterDimensions dimensions)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = Source.Raster.Format;
            _resultDimensions = dimensions ?? Source.Raster.Dimensions;
            _resultMapper = Source.Raster.Mapper;
            _resultPresentation = Source.Presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="dimensions">The raster dimensions.</param>
        protected void SetResultProperties(RasterFormat format, RasterDimensions dimensions)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = dimensions ?? Source.Raster.Dimensions;
            _resultMapper = Source.Raster.Mapper;
            _resultPresentation = Source.Presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="dimensions">The raster dimensions.</param>
        /// <param name="mapper">The mapper.</param>
        protected void SetResultProperties(RasterFormat format, RasterDimensions dimensions, RasterMapper mapper)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = dimensions ?? Source.Raster.Dimensions;
            _resultMapper = mapper;
            _resultPresentation = Source.Presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="dimensions">The raster dimensions.</param>
        /// <param name="presentation">The raster presentation.</param>
        protected void SetResultProperties(RasterFormat format, RasterDimensions dimensions, RasterPresentation presentation)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = dimensions ?? Source.Raster.Dimensions;
            _resultMapper = Source.Raster.Mapper;
            _resultPresentation = presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="dimensions">The raster dimensions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation.</param>
        protected void SetResultProperties(RasterFormat format, RasterDimensions dimensions, RasterMapper mapper, RasterPresentation presentation)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = dimensions ?? Source.Raster.Dimensions;
            _resultMapper = mapper;
            _resultPresentation = presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="dimensions">The raster dimensions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation.</param>
        /// <param name="imaging">The raster imaging.</param>
        protected void SetResultProperties(RasterFormat format, RasterDimensions dimensions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = dimensions ?? Source.Raster.Dimensions;
            _resultMapper = mapper;
            _resultPresentation = presentation;
            _resultImaging = imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        protected void SetResultProperties(RasterFormat format, Int32 radiometricResolution)
        {
            SetResultProperties(format, Source.Raster.NumberOfBands, Source.Raster.NumberOfRows, Source.Raster.NumberOfColumns, radiometricResolution);
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="presentation">The raster presentation.</param>
        protected void SetResultProperties(RasterFormat format, Int32 radiometricResolution, RasterPresentation rasterPresentation)
        {
            SetResultProperties(format, Source.Raster.NumberOfBands, Source.Raster.NumberOfRows, Source.Raster.NumberOfColumns, radiometricResolution, rasterPresentation);
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="presentation">The raster presentation.</param>
        protected void SetResultProperties(RasterFormat format, Int32 numberOfBands, Int32 radiometricResolution, RasterPresentation presentation)
        {
            SetResultProperties(format, numberOfBands, Source.Raster.NumberOfRows, Source.Raster.NumberOfColumns, radiometricResolution, presentation);
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        protected void SetResultProperties(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = Source.Raster.Format;
            _resultDimensions = new RasterDimensions(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution);
            _resultMapper = Source.Raster.Mapper;
            _resultPresentation = Source.Presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        protected void SetResultProperties(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = new RasterDimensions(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution);
            _resultMapper = Source.Raster.Mapper;
            _resultPresentation = Source.Presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }
        
        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        protected void SetResultProperties(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = new RasterDimensions(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution);
            _resultMapper = mapper;
            _resultPresentation = Source.Presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="presentation">The raster presentation.</param>
        protected void SetResultProperties(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterPresentation presentation)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = new RasterDimensions(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution);
            _resultMapper = Source.Raster.Mapper;
            _resultPresentation = presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation.</param>
        protected void SetResultProperties(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = new RasterDimensions(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution);
            _resultMapper = mapper;
            _resultPresentation = presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }

        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="format">The raster format.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation.</param>
        /// <param name="imaging">The raster imaging.</param>
        protected void SetResultProperties(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (_resultPropertiesSet)
                return;

            _resultFormat = format;
            _resultDimensions = new RasterDimensions(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution);
            _resultMapper = mapper;
            _resultPresentation = presentation;
            _resultImaging = imaging;

            _resultPropertiesSet = true;
        }


        /// <summary>
        /// Sets the properties of the result.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The raster format.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="presentation">The raster presentation.</param>
        protected void SetResultProperties(ISpectralGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 radiometricResolution, RasterPresentation presentation)
        {
            if (_resultPropertiesSet)
                return;

            _resultGeometry = geometry;
            _resultFormat = format;
            _resultDimensions = new RasterDimensions(numberOfBands, Source.Raster.NumberOfRows, Source.Raster.NumberOfColumns, radiometricResolution);
            _resultMapper = Source.Raster.Mapper;
            _resultPresentation = presentation;
            _resultImaging = Source.Imaging;

            _resultPropertiesSet = true;
        }


        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected virtual UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex) 
        {
            throw new NotSupportedException("The specified execution is not supported.");
        }

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected virtual UInt32[] Compute(Int32 rowIndex, Int32 columnIndex)
        {
            UInt32[] values = new UInt32[Result.Raster.NumberOfBands];
            for (Int32 bandIndex = 0; bandIndex < Result.Raster.NumberOfBands; bandIndex++)
                values[bandIndex] = Compute(rowIndex, columnIndex, bandIndex);

            return values;
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected virtual Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            throw new NotSupportedException("The specified execution is not supported.");
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected virtual Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            Double[] values = new Double[Result.Raster.NumberOfBands];
            for (Int32 bandIndex = 0; bandIndex < Result.Raster.NumberOfBands; bandIndex++)
                values[bandIndex] = ComputeFloat(rowIndex, columnIndex, bandIndex);

            return values;
        }
        
        #endregion
    }
}
