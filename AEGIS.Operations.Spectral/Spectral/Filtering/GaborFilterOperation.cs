// <copyright file="GaborFilterOperation.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using ELTE.AEGIS.Operations.Management;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represent a Gabor filter.
    /// <author>Gábor Balázs Butkay</author>
    /// </summary>
    [OperationMethodImplementation("AEGIS::251175", "Gabor filter")]
    public class GaborFilterOperation : GradientFilterOperation
    {
        #region Constructors

        /// <summary>
        /// Initialize a new instance of Gabor filter.
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
        public GaborFilterOperation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        { }

        /// <summary>
        /// Initialize a new instance of Gabor filter.
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
        public GaborFilterOperation(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.GaborFilter, parameters)
        {
            AddFilter(FilterFactory.CreateGaborFilter(Convert.ToInt32(ResolveParameter(SpectralOperationParameters.FilterRadius)),
                                                      Convert.ToDouble(ResolveParameter(SpectralOperationParameters.FilterStandardDeviation)),
                                                      Convert.ToDouble(ResolveParameter(SpectralOperationParameters.FilterWavelength)),
                                                      Convert.ToDouble(ResolveParameter(SpectralOperationParameters.FilterOrientation)),
                                                      Convert.ToDouble(ResolveParameter(SpectralOperationParameters.FilterPhaseOffset)),
                                                      Convert.ToDouble(ResolveParameter(SpectralOperationParameters.FilterSpatialAspectRatio))));
        }

        #endregion
    }
}
