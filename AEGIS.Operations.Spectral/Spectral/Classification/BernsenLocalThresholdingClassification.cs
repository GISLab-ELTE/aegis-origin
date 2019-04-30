/// <copyright file="BernsenLocalTresholdingClassification.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gábor Balázs Butkay</author>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represent a local thresholding classification using Bernsen's algorithm.
    /// </summary>
    /// <remarks>
    /// In Bernsen local thresholding, if the local contrast is above or equal to the user provided contrast threshold, the threshold 
    /// is set at the local midgray value (the mean of the minimum and maximum gray values in the local window). 
    /// If the local contrast is below the contrast threshold the neighborhood is considered to consist only of one class and the pixel 
    /// is set to object or background depending on the value of the midgray.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::253302", "Bernsen local thresholding")]
    public class BernsenLocalThresholdingClassification : SpectralTransformation
    {
        #region Constructors

        /// <summary>
        /// Initialize a new instance of <see cref="BernsenLocalThresholdingClassification" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The source is invalid.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public BernsenLocalThresholdingClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="BernsenLocalThresholdingClassification" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The source is invalid.
        /// or
        /// The target is invalid.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public BernsenLocalThresholdingClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, SpectralOperationMethods.BernsenLocalThresholdingClassification, parameters)
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
            Double nMax = GetNeighborMaximum(rowIndex, columnIndex, bandIndex);
            Double nMin = GetNeighborMinimum(rowIndex, columnIndex, bandIndex);

            return (Source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex) < (nMax + nMin) / 2) ? Byte.MaxValue : Byte.MinValue;
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

        #region Private methods

        /// <summary>
        /// Count the minimum value of the neighbor pixel's float value.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The minimum value.</returns>
        private Double GetNeighborMinimum(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Double min = Double.MaxValue;

            for (Int32 i = rowIndex - 1; i <= rowIndex + 1; i++)
            {
                for (Int32 j = columnIndex - 1; j <= columnIndex + 1; j++)
                {
                    Boolean isOwnPixel = i == rowIndex && j == columnIndex;
                    Boolean isInRange = i >= 0 && j >= 0 && i < Source.Raster.NumberOfRows && j < Source.Raster.NumberOfColumns;
                    if (isInRange && !isOwnPixel && min > Source.Raster.GetFloatValue(i, j, bandIndex))
                        min = Source.Raster.GetFloatValue(i, j, bandIndex);
                }
            }

            return min;
        }

        /// <summary>
        /// Count the maximum value of the neighbor pixel's float value.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The maximum value.</returns>
        private Double GetNeighborMaximum(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Double max = -1;

            for (Int32 i = rowIndex - 1; i <= rowIndex + 1; i++)
            {
                for (Int32 j = columnIndex - 1; j <= columnIndex + 1; j++)
                {
                    Boolean isOwnPixel = i == rowIndex && j == columnIndex;
                    Boolean isInRange = i >= 0 && j >= 0 && i < Source.Raster.NumberOfRows && j < Source.Raster.NumberOfColumns;
                    if (isInRange && !isOwnPixel && max < Source.Raster.GetFloatValue(i, j, bandIndex))
                        max = Source.Raster.GetFloatValue(i, j, bandIndex);
                }
            }

            return max;
        }

        #endregion
    }
}
