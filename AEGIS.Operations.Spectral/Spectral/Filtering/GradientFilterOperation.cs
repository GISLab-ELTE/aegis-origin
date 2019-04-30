/// <copyright file="GradientFilterOperation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Algorithms;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represents a gradient filter operation utilizing one or more filters.
    /// </summary>
    /// <remarks>
    /// Gradient filtering is a technique for modifying or enhancing raster imagery by applying a convolution between the image and the filter. 
    /// The filter consists of a kernel (also known as convolution matrix or mask), a factor and an offset scalar. Depending on the element values, a kernel can cause a wide range of effects.
    /// This variant of filtering uses multiple kernels which are applied separately on the image. The initial results are then combined to form the final result.
    /// Filtering is a focal operation that modifies the central spectral value under the kernel.
    /// </remarks>
    public abstract class GradientFilterOperation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The list of filters.
        /// </summary>
        private List<Filter> _filters;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientFilterOperation" /> class.
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
        protected GradientFilterOperation(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _filters = new List<Filter>();
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
            if (_filters.Count == 0)
            {
                return Source.Raster.GetValue(rowIndex, columnIndex, bandIndex);
            }

            if (_filters.Count == 1)
            {
                Double filteredValue = 0;
                for (Int32 filterRowIndex = -_filters[0].Radius; filterRowIndex <= _filters[0].Radius; filterRowIndex++)
                    for (Int32 filterColumnIndex = -_filters[0].Radius; filterColumnIndex <= _filters[0].Radius; filterColumnIndex++)
                    {
                        filteredValue += Source.Raster.GetNearestValue(rowIndex + filterRowIndex, columnIndex + filterColumnIndex, bandIndex) * _filters[0].Kernel[filterRowIndex + _filters[0].Radius, filterColumnIndex + _filters[0].Radius];
                    }

                return RasterAlgorithms.Restrict(filteredValue / _filters[0].Factor + _filters[0].Offset, Source.Raster.RadiometricResolution);
            }

            Double[] filteredValues = new Double[_filters.Count];

            for (Int32 filterIndex = 0; filterIndex < _filters.Count; filterIndex++)
            {
                for (Int32 filterRowIndex = -_filters[filterIndex].Radius; filterRowIndex <= _filters[filterIndex].Radius; filterRowIndex++)
                    for (Int32 filterColumnIndex = -_filters[filterIndex].Radius; filterColumnIndex <= _filters[filterIndex].Radius; filterColumnIndex++)
                    {
                        filteredValues[filterIndex] += Source.Raster.GetNearestValue(rowIndex + filterRowIndex, columnIndex + filterColumnIndex, bandIndex) * _filters[filterIndex].Kernel[filterRowIndex + _filters[filterIndex].Radius, filterColumnIndex + _filters[filterIndex].Radius];
                    }
                filteredValues[filterIndex] = filteredValues[filterIndex] / _filters[filterIndex].Factor + _filters[filterIndex].Offset;
            }

            return RasterAlgorithms.Restrict(CombineValues(filteredValues), Source.Raster.RadiometricResolution);
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
            if (_filters.Count == 0)
            {
                return Source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex);
            }

            if (_filters.Count == 1)
            {
                Double filteredValue = 0;
                for (Int32 filterRowIndex = -_filters[0].Radius; filterRowIndex <= _filters[0].Radius; filterRowIndex++)
                    for (Int32 filterColumnIndex = -_filters[0].Radius; filterColumnIndex <= _filters[0].Radius; filterColumnIndex++)
                    {
                        filteredValue += Source.Raster.GetNearestFloatValue(rowIndex + filterRowIndex, columnIndex + filterColumnIndex, bandIndex) * _filters[0].Kernel[filterRowIndex + _filters[0].Radius, filterColumnIndex + _filters[0].Radius];
                    }

                return filteredValue / _filters[0].Factor + _filters[0].Offset;
            }

            Double[] filteredValues = new Double[_filters.Count];
            for (Int32 filterIndex = 0; filterIndex < _filters.Count; filterIndex++)
            {
                for (Int32 filterRowIndex = -_filters[filterIndex].Radius; filterRowIndex <= _filters[filterIndex].Radius; filterRowIndex++)
                    for (Int32 filterColumnIndex = -_filters[filterIndex].Radius; filterColumnIndex <= _filters[filterIndex].Radius; filterColumnIndex++)
                    {
                        filteredValues[filterIndex] += Source.Raster.GetNearestFloatValue(rowIndex + filterRowIndex, columnIndex + filterColumnIndex, bandIndex) * _filters[filterIndex].Kernel[filterRowIndex + _filters[filterIndex].Radius, filterColumnIndex + _filters[filterIndex].Radius];
                    }
                filteredValues[filterIndex] = filteredValues[filterIndex] / _filters[filterIndex].Factor + _filters[filterIndex].Offset;
            }

            return CombineValues(filteredValues);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Combines the specified filtered values.
        /// </summary>
        /// <param name="values">The array of filtered values.</param>
        /// <returns>The combination of the values for the specified filter.</returns>
        protected virtual Double CombineValues(Double[] values)
        {
            return 0;
        }

        /// <summary>
        /// Adds a filter to the collection of filters.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        protected void AddFilter(Filter filter)
        {
            if (filter != null)
                _filters.Add(filter);
        }

        #endregion
    }
}
