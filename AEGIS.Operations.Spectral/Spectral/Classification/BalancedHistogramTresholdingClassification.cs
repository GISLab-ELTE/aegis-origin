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
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents a threshold based spectral classification using histogram balancing.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213826", "Balanced histogram thresholding")]
    public class BalancedHistogramTresholdingClassification : ThresholdingClassification
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BalancedHistogramTresholdingClassification" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
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
        public BalancedHistogramTresholdingClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {          
        }

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
        public BalancedHistogramTresholdingClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.BalancedHistogramThresholdingClassification, parameters)
        {
            if (_sourceBandIndices != null)
            {
                for (Int32 k = 0; k < _sourceBandIndices.Length; k++)
                {
                    _lowerThresholdValues[k] = RasterAlgorithms.ComputeHistogramBalance(_source.Raster[_sourceBandIndices[k]].HistogramValues);
                }
            }
            else
            {
                for (Int32 i = 0; i < _source.Raster.NumberOfBands; i++)
                {
                    _lowerThresholdValues[i] = RasterAlgorithms.ComputeHistogramBalance(_source.Raster[i].HistogramValues);
                }
            }
        }

        #endregion
    }
}
