/// <copyright file="SpectralResampling.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms.Resampling;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Resampling
{
    /// <summary>
    /// Represents an operation resampling spectral content of geometries. 
    /// </summary>
    [OperationMethodImplementation("AEGIS::213301", "Spectral resampling")]
    public class SpectralResampling : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The number of columns taken from the source image.
        /// </summary>
        private Double _sourceColumnCount;

        /// <summary>
        /// The column offset (shift) within the source image.
        /// </summary>
        private Double _sourceColumnOffset;

        /// <summary>
        /// The number of rows taken from the source image.
        /// </summary>
        private Double _sourceRowCount;

        /// <summary>
        /// The row offset (shift) within the source image.
        /// </summary>
        private Double _sourceRowOffset;

        /// <summary>
        /// The number of columns in the target image.
        /// </summary>
        private Int32 _targetNumberOfColumns;

        /// <summary>
        /// The number of rows in the target image.
        /// </summary>
        private Int32 _targetNumberOfRows;

        /// <summary>
        /// The resampling strategy.
        /// </summary>
        private RasterResamplingAlgorithm _resamplingAlgorithm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralResampling"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// or
        /// The source geometry does not contain raster data.
        /// </exception>
        public SpectralResampling(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralResampling"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// or
        /// The source geometry does not contain raster data.
        /// </exception>
        public SpectralResampling(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.SpectralResampling, parameters)
        {
            _sourceColumnCount = IsProvidedParameter(SpectralOperationParameters.SourceColumnCount) ? Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SourceColumnCount)) : Source.Raster.NumberOfColumns;
            _sourceColumnOffset = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SourceColumnOffset));
            _sourceRowCount = IsProvidedParameter(SpectralOperationParameters.SourceRowCount) ? Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SourceRowCount)) : Source.Raster.NumberOfRows;
            _sourceRowOffset = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SourceRowOffset));

            _targetNumberOfRows = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.NumberOfRows));
            _targetNumberOfColumns = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.NumberOfColumns));

            // check whether the strategy is provided
            if (IsProvidedParameter(SpectralOperationParameters.RasterResamplingAlgorithm))
                _resamplingAlgorithm = ResolveParameter<RasterResamplingAlgorithm>(SpectralOperationParameters.RasterResamplingAlgorithm);

            // check the strategy type (that has default value)
            if (_resamplingAlgorithm == null)
            {
                try
                {
                    _resamplingAlgorithm = (RasterResamplingAlgorithm)Activator.CreateInstance(ResolveParameter<Type>(SpectralOperationParameters.RasterResamplingAlgorithmType), new Object[] { Source.Raster });
                }
                catch
                {
                    _resamplingAlgorithm = (RasterResamplingAlgorithm)Activator.CreateInstance((Type)SpectralOperationParameters.RasterResamplingAlgorithmType.DefaultValue, new Object[] { Source.Raster });
                }
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            // compute the mapper
            RasterMapper mapper = null;

            if (Source.Raster.IsMapped)
                mapper = RasterMapper.FromMapper(Source.Raster.Mapper, 
                                                 (Coordinate)(Source.Raster.Mapper.MapCoordinate(_sourceRowOffset, _sourceColumnOffset) - Source.Raster.Mapper.MapCoordinate(0, 0)), 
                                                 new CoordinateVector(_sourceRowCount / _targetNumberOfRows, _sourceColumnCount / _targetNumberOfColumns));

            IPolygon polygon;

            if (mapper.Mode == RasterMapMode.ValueIsArea)
            {
                polygon = Source.Factory.CreatePolygon(mapper.MapCoordinate(0, 0), mapper.MapCoordinate(_targetNumberOfRows - 1, 0), mapper.MapCoordinate(_targetNumberOfRows - 1, _targetNumberOfColumns - 1), mapper.MapCoordinate(0, _targetNumberOfColumns - 1));
            }
            else
            {
                polygon = Source.Factory.CreatePolygon(mapper.MapCoordinate(0, 0), mapper.MapCoordinate(_targetNumberOfRows, 0), mapper.MapCoordinate(_targetNumberOfRows, _targetNumberOfColumns), mapper.MapCoordinate(0, _targetNumberOfColumns));
            }

            _result = _source.Factory.CreateSpectralGeometry(polygon,
                                                             PrepareRasterResult(Source.Raster.Format,
                                                                                 Source.Raster.NumberOfBands,
                                                                                 _targetNumberOfRows,
                                                                                 _targetNumberOfColumns,
                                                                                 Source.Raster.RadiometricResolutions,
                                                                                 mapper),
                                                             Source.Presentation,
                                                             Source.Imaging);
        }

        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Double originalRowIndex = _sourceRowOffset + (rowIndex + 0.5) / _targetNumberOfRows * _sourceRowCount - 0.5;
            Double originalColumnIndex = _sourceColumnOffset + (columnIndex + 0.5) / _targetNumberOfColumns * _sourceColumnCount - 0.5;

            return _resamplingAlgorithm.Compute(originalRowIndex, originalColumnIndex, bandIndex);
        }

        /// <summary>
        /// Computes the specified floating spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] Compute(Int32 rowIndex, Int32 columnIndex)
        {
            Double originalRowIndex = _sourceRowOffset + (rowIndex + 0.5) / _targetNumberOfRows * _sourceRowCount - 0.5;
            Double originalColumnIndex = _sourceColumnOffset + (columnIndex + 0.5) / _targetNumberOfColumns * _sourceColumnCount - 0.5;

            return _resamplingAlgorithm.Compute(originalRowIndex, originalColumnIndex);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Double originalRowIndex = _sourceRowOffset + (rowIndex + 0.5) / _targetNumberOfRows * _sourceRowCount - 0.5;
            Double originalColumnIndex = _sourceColumnOffset + (columnIndex + 0.5) / _targetNumberOfColumns * _sourceColumnCount - 0.5;

            return _resamplingAlgorithm.ComputeFloat(originalRowIndex, originalColumnIndex, bandIndex);
        }


        /// <summary>
        /// Computes the specified floating spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            Double originalRowIndex = _sourceRowOffset + (rowIndex + 0.5) / _targetNumberOfRows * _sourceRowCount - 0.5;
            Double originalColumnIndex = _sourceColumnOffset + (columnIndex + 0.5) / _targetNumberOfColumns * _sourceColumnCount - 0.5;

            return _resamplingAlgorithm.ComputeFloat(originalRowIndex, originalColumnIndex);
        }

        #endregion
    }
}
