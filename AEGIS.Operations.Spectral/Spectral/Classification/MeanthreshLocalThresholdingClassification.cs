// <copyright file="MeanthreshLocalThresholdingClassification.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represent a local thresholding classification using the Meanthresh algorithm.
    /// </summary>
    /// <remarks>
    /// The Meanthresh algorithm calculates the mean value in a window and if the pixel's intensity is above the mean
    /// the pixel is set to white color, otherwise the pixel is set to black color.
    /// To increase the resistance to noise the threshold value can be shifted by a thresholding constant.
    /// <author>Gábor Balázs Butkay</author>
    /// </remarks>
    [OperationMethodImplementation("AEGIS::253305", "Meanthresh local thresholding")]
    public class MeanthreshLocalThresholdingClassification : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The thresholding constant.
        /// </summary>
        private Double _thresholdingConstant;

        /// <summary>
        /// The array of mean values for the specified bands.
        /// </summary>
        private Double[] _meanValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of <see cref="MeanthreshLocalThresholdingClassification" /> class.
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
        public MeanthreshLocalThresholdingClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="MeanthreshLocalThresholdingClassification" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
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
        public MeanthreshLocalThresholdingClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, SpectralOperationMethods.MeanthreshLocalThresholdingClassification, parameters)
        {
            _thresholdingConstant = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.MeanthreshThresholdingConstant));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            _meanValue = new Double[Source.Raster.NumberOfBands];

            for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
            {
                Double result = 0;
                for (Int32 rowIndex = 0; rowIndex < Source.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < Source.Raster.NumberOfColumns; columnIndex++)
                        result += Source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex);

                _meanValue[bandIndex] = result / (Source.Raster.NumberOfColumns * Source.Raster.NumberOfRows);
            }

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
            return Source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex) >= _meanValue[bandIndex] - _thresholdingConstant ? Byte.MaxValue : Byte.MinValue;
        }

        #endregion
    }
}
