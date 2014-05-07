﻿/// <copyright file="CustomThresholdClassification.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents a threshold transformation with custom threshold value.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::213120", "Custom threshold")]
    public class CustomThresholdClassification : ThresholdClassification
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomThresholdClassification" /> class.
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
        public CustomThresholdClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomThresholdClassification" /> class.
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
        public CustomThresholdClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.CustomThresholdClassification, parameters)
        {
            if (_sourceBandIndex >= 0)
            {
                _lowerThresholdValues = new Double[] { Convert.ToDouble(GetParameter(SpectralOperationParameters.LowerThresholdValue)) };
                _upperThresholdValues = new Double[] { Convert.ToDouble(GetParameter(SpectralOperationParameters.UpperThresholdValue)) };
            }
            else
            {
                _lowerThresholdValues = new Double[_source.Raster.SpectralResolution];
                _upperThresholdValues = new Double[_source.Raster.SpectralResolution];

                for (Int32 i = 0; i < _lowerThresholdValues.Length; i++)
                {
                    _lowerThresholdValues[i] = Convert.ToDouble(GetParameter(SpectralOperationParameters.LowerThresholdValue));
                    _upperThresholdValues[i] = Convert.ToDouble(GetParameter(SpectralOperationParameters.UpperThresholdValue));
                }
            }
        }

        #endregion
    }
}
