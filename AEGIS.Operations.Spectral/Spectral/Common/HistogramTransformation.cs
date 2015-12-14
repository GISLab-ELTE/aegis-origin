/// <copyright file="HistogramTransformation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spectral.Common
{
    /// <summary>
    ///  Represent an operation performing histogram transformation of <see cref="ISpectralGeometry"/> instances.
    /// </summary>
    public abstract class HistogramTransformation : PerBandSpectralTransformation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramTransformation" /> class.
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
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// </exception>
        protected HistogramTransformation(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
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
            return Compute(bandIndex, _source.Raster.GetValue(rowIndex, columnIndex, bandIndex));
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the specified value.
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The transformed value.</returns>
        protected abstract UInt32 Compute(Int32 bandIndex, UInt32 value);

        #endregion
    }
}
