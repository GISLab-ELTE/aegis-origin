/// <copyright file="ThresholdingClassification.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents a threshold based spectral classification.
    /// </summary>
    public abstract class ThresholdingClassification : PerBandSpectralTransformation
    {
        #region protected fields

        protected Func<IRaster, Int32, Int32, Int32, Boolean> _selectorFunction;
        protected Double[] _lowerThresholdValues;
        protected Double[] _upperThresholdValues;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ThresholdingClassification" /> class.
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
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the method.
        /// </exception>
        protected ThresholdingClassification(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _lowerThresholdValues = Enumerable.Repeat(Double.MinValue, source.Raster.NumberOfBands).ToArray();
            _upperThresholdValues = Enumerable.Repeat(Double.MaxValue, source.Raster.NumberOfBands).ToArray();
        }

        #endregion

        #region Protected RasterTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override sealed UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            // decide based on selector function
            if (_selectorFunction != null)
                return _selectorFunction(_source.Raster, rowIndex, columnIndex, bandIndex) ? Byte.MaxValue : Byte.MinValue;

            // decide based on constants
            if (_source.Raster.Format == RasterFormat.Floating)
            {
                Double value = _source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex);
                return (value >= _lowerThresholdValues[bandIndex] && value <= _upperThresholdValues[bandIndex]) ? Byte.MaxValue : Byte.MinValue;
            }
            else
            {
                UInt32 value = _source.Raster.GetValue(rowIndex, columnIndex, bandIndex);
                return (value >= _lowerThresholdValues[bandIndex] && value <= _upperThresholdValues[bandIndex]) ? Byte.MaxValue : Byte.MinValue;
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override sealed void PrepareResult()
        {
            // the result will have integer representation in all cases
            if (_sourceBandIndices != null)
            {
                _result = _source.Factory.CreateSpectralGeometry(_source,
                                                                 PrepareRasterResult(RasterFormat.Integer, 
                                                                                     _sourceBandIndices.Length,
                                                                                     _source.Raster.NumberOfRows,
                                                                                     _source.Raster.NumberOfColumns,
                                                                                     Enumerable.Repeat(8, _sourceBandIndices.Length).ToArray(),
                                                                                     _source.Raster.Mapper), 
                                                                 RasterPresentation.CreateGrayscalePresentation(),
                                                                 _source.Imaging);
            }
            else
            {
                _result = _source.Factory.CreateSpectralGeometry(_source,
                                                                 PrepareRasterResult(RasterFormat.Integer, 
                                                                                     _source.Raster.NumberOfBands,
                                                                                     _source.Raster.NumberOfRows,
                                                                                     _source.Raster.NumberOfColumns,
                                                                                     Enumerable.Repeat(8, _source.Raster.NumberOfBands).ToArray(),
                                                                                     _source.Raster.Mapper),
                                                                 _source.Presentation,
                                                                 _source.Imaging);
            }
        }

        #endregion
    }
}
