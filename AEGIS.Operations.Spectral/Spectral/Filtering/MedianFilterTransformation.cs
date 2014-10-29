/// <copyright file="MedianFilterTransformation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represents a median filter transformation.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213213", "Median filter")]
    public class MedianFilterTransformation : PerBandSpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The radius of the filter.
        /// </summary>
        private Int32 _filterRadius;

        /// <summary>
        /// The array of filtered values.
        /// </summary>
        private Double[] _filteredValues;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MedianFilterTransformation" /> class.
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
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public MedianFilterTransformation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MedianFilterTransformation" /> class.
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
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public MedianFilterTransformation(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.MedianFilter, parameters)
        {
            _filterRadius = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.FilterRadius));
            _filteredValues = new Double[_filterRadius * _filterRadius];
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
            Int32 rowBase = rowIndex - _filterRadius / 2;
            Int32 columnBase = columnIndex - _filterRadius / 2;
            Int32 index = 0;

            for (Int32 k = 0; k < _filterRadius; k++)
                for (Int32 l = 0; l < _filterRadius; l++)
                {
                    _filteredValues[index] = _source.Raster.GetNearestValue(rowBase + k, columnBase + l, bandIndex);
                    index++;
                }
            Array.Sort(_filteredValues);

            return (UInt32)_filteredValues[_filterRadius * _filterRadius / 2];
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
            Int32 rowBase = rowIndex - _filterRadius / 2;
            Int32 columnBase = columnIndex - _filterRadius / 2;
            Int32 index = 0;

            for (Int32 k = 0; k < _filterRadius; k++)
                for (Int32 l = 0; l < _filterRadius; l++)
                {
                    _filteredValues[index] = _source.Raster.GetNearestFloatValue(rowBase + k, columnBase + l, bandIndex);
                    index++;
                }
            Array.Sort(_filteredValues);

            return _filteredValues[_filterRadius * _filterRadius / 2];
        }

        #endregion
    }
}
