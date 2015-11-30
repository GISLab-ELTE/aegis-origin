/// <copyright file="UnsharpMaskingOperation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represents an Unsharp madking filter operation.
    /// </summary>
    /// <remarks>
    /// Unsharp masking (USM) is an image sharpening technique. 
    /// The "unsharp" of the name derives from the fact that the technique uses a blurred, or "unsharp", negative image to create a mask of the original image.
    /// The unsharped mask is then combined with the positive (original) image, creating an image that is less blurry than the original. 
    /// The resulting image, although clearer, may be a less accurate representation of the image's subject. 
    /// </remarks>
    [OperationMethodImplementation("AEGIS::213242", "Unsharp masking filter")]
    public class UnsharpMaskingOperation : PerBandSpectralTransformation
    {
        #region Private fields

        private Double _amount;
        private Double _threshold;
        private Filter _unsharpFilter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsharpMaskingOperation" /> class.
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
        public UnsharpMaskingOperation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsharpMaskingOperation" /> class.
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
        public UnsharpMaskingOperation(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.UnsharpMasking, parameters)
        {
            _amount = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SharpeningAmount));
            _threshold = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SharpeningThreshold));
            _unsharpFilter = FilterFactory.CreateGaussianFilter(Convert.ToInt32(ResolveParameter(SpectralOperationParameters.SharpeningRadius)));
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
            // if threshold is specified, the magnitude of the gradient is computed
            if (_threshold > 0)
            {
                Double gradientRow = -_source.Raster.GetNearestValue(rowIndex - 1, columnIndex, bandIndex) + _source.Raster.GetNearestValue(rowIndex + 1, columnIndex, bandIndex);
                Double gradientColumn = -_source.Raster.GetNearestValue(rowIndex, columnIndex - 1, bandIndex) + _source.Raster.GetNearestValue(rowIndex, columnIndex + 1, bandIndex);

                // if the threshold is not reached, the original value is returned
                if (Math.Sqrt(gradientRow *gradientRow + gradientColumn * gradientColumn) < _threshold)
                    return _source.Raster.GetValue(rowIndex, columnIndex, bandIndex);
            }

            Double filteredValue = 0;
            for (Int32 filterRowIndex = -_unsharpFilter.Radius; filterRowIndex <= _unsharpFilter.Radius; filterRowIndex++)
                for (Int32 filterColumnIndex = -_unsharpFilter.Radius; filterColumnIndex <= _unsharpFilter.Radius; filterColumnIndex++)
                {
                    filteredValue += _source.Raster.GetNearestValue(rowIndex + filterRowIndex, columnIndex + filterColumnIndex, bandIndex) * _unsharpFilter.Kernel[filterRowIndex + _unsharpFilter.Radius, filterColumnIndex + _unsharpFilter.Radius];
                }
            filteredValue = filteredValue / _unsharpFilter.Factor + _unsharpFilter.Offset;

            return RasterAlgorithms.Restrict((1 + _amount) * _source.Raster.GetValue(rowIndex, columnIndex, bandIndex) - _amount * filteredValue, _source.Raster.RadiometricResolutions[bandIndex]);
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
            // if threshold is specified, the magnitude of the gradient is computed
            if (_threshold > 0)
            {
                Double gradientRow = -_source.Raster.GetNearestFloatValue(rowIndex - 1, columnIndex, bandIndex) + _source.Raster.GetNearestFloatValue(rowIndex + 1, columnIndex, bandIndex);
                Double gradientColumn = -_source.Raster.GetNearestFloatValue(rowIndex, columnIndex - 1, bandIndex) + _source.Raster.GetNearestFloatValue(rowIndex, columnIndex + 1, bandIndex);

                // if the threshold is not reached, the original value is returned
                if (Math.Sqrt(gradientRow * gradientRow + gradientColumn * gradientColumn) < _threshold)
                    return _source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex);
            }

            Double filteredValue = 0;
            for (Int32 filterRowIndex = -_unsharpFilter.Radius; filterRowIndex <= _unsharpFilter.Radius; filterRowIndex++)
                for (Int32 filterColumnIndex = -_unsharpFilter.Radius; filterColumnIndex <= _unsharpFilter.Radius; filterColumnIndex++)
                {
                    filteredValue += _source.Raster.GetNearestFloatValue(rowIndex + filterRowIndex, columnIndex + filterColumnIndex, bandIndex) * _unsharpFilter.Kernel[filterRowIndex + _unsharpFilter.Radius, filterColumnIndex + _unsharpFilter.Radius];
                }
            filteredValue = filteredValue / _unsharpFilter.Factor + _unsharpFilter.Offset;

            return RasterAlgorithms.Restrict((1 + _amount) * _source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex) - _amount * filteredValue, _source.Raster.RadiometricResolutions[bandIndex]);
        }

        #endregion
    }
}
