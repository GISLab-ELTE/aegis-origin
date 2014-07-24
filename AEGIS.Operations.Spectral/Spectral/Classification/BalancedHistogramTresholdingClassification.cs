/// <copyright file="BalancedHistogramTresholdingClassification.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents a threshold based spectral classification using histogram balancing.
    /// </summary>
    [OperationClass("AEGIS::213124", "Balanced histogram thresholding")]
    public class BalancedHistogramTresholdingClassification : ThresholdingClassification
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BalancedHistogramTresholdingClassification" /> class.
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
        protected BalancedHistogramTresholdingClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.BalancedHistogramThresholdingClassification, parameters)
        {
            if (_sourceBandIndex >= 0)
            {
                _lowerThresholdValues = new Double[] { RasterAlgorithms.ComputeHistogramBalance(_source.Raster[_sourceBandIndex].HistogramValues) };
                _upperThresholdValues = new Double[] { Convert.ToDouble(ResolveParameter(SpectralOperationParameters.UpperThresholdBoundary)) };
            }
            else
            {
                _lowerThresholdValues = new Double[_source.Raster.SpectralResolution];
                _upperThresholdValues = new Double[_source.Raster.SpectralResolution];

                for (Int32 i = 0; i < _lowerThresholdValues.Length; i++)
                {
                    _lowerThresholdValues[i] = RasterAlgorithms.ComputeHistogramBalance(_source.Raster[i].HistogramValues);
                    _upperThresholdValues[i] = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.UpperThresholdBoundary));
                }
            }
        }

        #endregion
    }
}
