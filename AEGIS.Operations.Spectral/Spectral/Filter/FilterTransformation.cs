﻿/// <copyright file="FilterTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Filter
{
    /// <summary>
    /// Represents a filter transformation.
    /// </summary>
    public abstract class FilterTransformation : PerBandSpectralTransformation
    {
        #region Private fields

        private UInt64[] _peeks;

        #endregion

        #region Protected fields

        protected Int32 _filterSize;
        protected Double _filterFactor;
        protected Matrix _filterKernel;
        protected Double _filterOffset;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTransformation" /> class.
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
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        protected FilterTransformation(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            if (_sourceBandIndex >= 0)
            {
                _peeks = new UInt64[] { RasterAlgorithms.RadiometricResolutionMax(_source.Raster.RadiometricResolutions[_sourceBandIndex]) };
            }
            else
            {
                _peeks = new UInt64[_source.Raster.SpectralResolution];

                for (Int32 i = 0; i < _peeks.Length; i++)
                {
                    _peeks[i] = RasterAlgorithms.RadiometricResolutionMax(_source.Raster.RadiometricResolutions[i]);
                }
            }
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
            Double filteredValue = 0;
            Int32 rowBase = rowIndex - _filterSize / 2;
            Int32 columnBase = columnIndex - _filterSize / 2;
            for (Int32 k = 0; k < _filterSize; k++)
                for (Int32 l = 0; l < _filterSize; l++)
                {
                    filteredValue += _source.Raster.GetNearestValue(rowBase + k, columnBase + l, bandIndex) * _filterKernel[k, l];
                }
            filteredValue = filteredValue / _filterFactor + _filterOffset;

            return (UInt32)(Math.Min(Math.Max(0, filteredValue), _peeks[bandIndex]));
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
            Double filteredValue = 0;
            Int32 rowBase = rowIndex - _filterSize / 2;
            Int32 columnBase = columnIndex - _filterSize / 2;
            for (Int32 k = 0; k < _filterSize; k++)
                for (Int32 l = 0; l < _filterSize; l++)
                {
                    filteredValue += (_source.Raster as IFloatRaster).GetNearestValue(rowBase + k, columnBase + l, bandIndex) * _filterKernel[k, l];
                }
            filteredValue = filteredValue / _filterFactor + _filterOffset;

            return filteredValue;
        }

        #endregion
    }
}
