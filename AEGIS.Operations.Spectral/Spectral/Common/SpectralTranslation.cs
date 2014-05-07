﻿/// <copyright file="SpectralTranslation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spectral.Common
{
    /// <summary>
    /// Represents a spectral translation.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::213102", "Spectral translation")]
    public class SpectralTranslation : PerBandSpectralTransformation
    {
        #region Private fields

        private Double _offset;
        private Double _factor;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralTranslation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
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
        /// The value of a parameter is not within the expected range.
        /// </exception>
        public SpectralTranslation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralTranslation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="result">The result.</param>
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
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public SpectralTranslation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.SpectralTranslation, parameters)
        {
            _offset = parameters.ContainsKey(SpectralOperationParameters.SpectralOffset) ? (Double)parameters[SpectralOperationParameters.SpectralOffset] : 0;
            _factor = parameters.ContainsKey(SpectralOperationParameters.SpectralFactor) ? (Double)parameters[SpectralOperationParameters.SpectralFactor] : 1;
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
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return (UInt32)(_source.Raster.GetValue(rowIndex, columnIndex, bandIndex) * _factor + _offset); 
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
            return (_source.Raster as IFloatRaster).GetValue(rowIndex, columnIndex, bandIndex) * _factor + _offset; 
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the reverse operation.
        /// </summary>
        /// <returns>The reverse operation.</returns>
        protected override IOperation<ISpectralGeometry, ISpectralGeometry> ComputeReverseOperation()
        {
            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.SpectralOffset, -_offset);
            parameters.Add(SpectralOperationParameters.SpectralFactor, 1 / _factor);

            return new SpectralTranslation(_result, _source, _parameters);
        }

        #endregion
    }
}
