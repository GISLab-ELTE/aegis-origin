/// <copyright file="NiblackLocalThresholdingClassification.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
/// <author>Gábor Balázs Butkay</author>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represent a local thresholding classification using Niblack's algorithm.
    /// </summary>
    /// <remarks>
    /// Niblack local thresholding separates white and black pixels given the local mean and standard deviation for the current window.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::253311", "Niblack local thresholding")]
    public class NiblackLocalThresholdingClassification : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The weight of the standard deviation.
        /// </summary>
        private Double _standardDeviationWeight;

        /// <summary>
        /// The window radius.
        /// </summary>
        private Int32 _windowRadius;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NiblackLocalThresholdingClassification" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The source is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public NiblackLocalThresholdingClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NiblackLocalThresholdingClassification" /> class.
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
        public NiblackLocalThresholdingClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, SpectralOperationMethods.NiblackLocalThresholdingClassification, parameters)
        {
            _standardDeviationWeight = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.StandardDeviationWeight));
            _windowRadius = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.ThresholdingWindowRadius));
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
            Double mean = ComputeMean(rowIndex, columnIndex, bandIndex);
            Double standardDeviation = ComputeStandardDeviation(rowIndex, columnIndex, bandIndex, mean);

            return (Source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex) > mean + _standardDeviationWeight * standardDeviation) ? Byte.MaxValue : Byte.MinValue;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Compute the mean value of the window.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The mean value of the window.</returns>
        private Double ComputeMean(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Double sum = 0;
            Int32 numberOfElements = 0;

            Int32 rowStart = Math.Max(rowIndex - _windowRadius, 0);
            Int32 rowEnd = Math.Min(rowIndex + _windowRadius, Source.Raster.NumberOfRows);
            Int32 columnStart = Math.Max(columnIndex - _windowRadius, 0);
            Int32 columnEnd = Math.Min(columnIndex + _windowRadius, Source.Raster.NumberOfColumns);

            for (Int32 localRowIndex = rowStart; localRowIndex < rowEnd; localRowIndex++)
            {
                for (Int32 localColumnIndex = columnStart; localColumnIndex < columnEnd; localColumnIndex++)
                {
                    sum += Source.Raster.GetFloatValue(localRowIndex, localColumnIndex, bandIndex);
                    numberOfElements++;
                }
            }

            return sum / numberOfElements;
        }

        /// <summary>
        /// Compute the standard deviation of the window.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <param name="mean">The mean value of the window.</param>
        /// <returns>The standard deviation of the window.</returns>
        private Double ComputeStandardDeviation(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double mean)
        {
            Double sum = 0;
            Int32 numberOfElements = 0;

            Int32 rowStart = Math.Max(rowIndex - _windowRadius, 0);
            Int32 rowEnd = Math.Min(rowIndex + _windowRadius, Source.Raster.NumberOfRows);
            Int32 columnStart = Math.Max(columnIndex - _windowRadius, 0);
            Int32 columnEnd = Math.Min(columnIndex + _windowRadius, Source.Raster.NumberOfColumns);
            
            for (Int32 localRowIndex = rowStart; localRowIndex < rowEnd; localRowIndex++)
            {
                for (Int32 localColumnIndex = columnStart; localColumnIndex < columnEnd; localColumnIndex++)
                {
                    sum += Math.Pow(Source.Raster.GetFloatValue(localRowIndex, localColumnIndex, bandIndex) - mean, 2);
                    numberOfElements++;
                }
            }

            return Math.Sqrt(sum / numberOfElements);
        }

        #endregion
    }
}
