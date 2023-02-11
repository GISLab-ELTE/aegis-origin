// <copyright file="BasicThresholdingClassification.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents a basic threshold based spectral classification with lower and upper thresholds.
    /// </summary>
    public abstract class BasicThresholdingClassification : SpectralTransformation
    {
        #region protected fields

        protected Func<IRaster, Int32, Int32, Int32, Boolean> _selectorFunction;
        protected Double[] _lowerThresholdValues;
        protected Double[] _upperThresholdValues;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicThresholdingClassification" /> class.
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
        protected BasicThresholdingClassification(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _lowerThresholdValues = Enumerable.Repeat(Double.MinValue, source.Raster.NumberOfBands).ToArray();
            _upperThresholdValues = Enumerable.Repeat(Double.MaxValue, source.Raster.NumberOfBands).ToArray();
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            SetResultProperties(RasterFormat.Integer, 8, RasterPresentation.CreateGrayscalePresentation());

            return base.PrepareResult();
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
                return _selectorFunction(Source.Raster, rowIndex, columnIndex, bandIndex) ? Byte.MaxValue : Byte.MinValue;

            // decide based on constants
            if (Source.Raster.Format == RasterFormat.Floating)
            {
                Double value = Source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex);
                return (value >= _lowerThresholdValues[bandIndex] && value <= _upperThresholdValues[bandIndex]) ? Byte.MaxValue : Byte.MinValue;
            }
            else
            {
                UInt32 value = Source.Raster.GetValue(rowIndex, columnIndex, bandIndex);
                return (value >= _lowerThresholdValues[bandIndex] && value <= _upperThresholdValues[bandIndex]) ? Byte.MaxValue : Byte.MinValue;
            }
        }
        
        #endregion        
    }
}
