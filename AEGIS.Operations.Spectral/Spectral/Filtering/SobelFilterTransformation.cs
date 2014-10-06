/// <copyright file="SobelFilterTransformation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represents a Sobel filter transformation.
    /// </summary>
    /// <remarks>
    /// The Sobel operator is a discrete differentiation operator, computing an approximation of the gradient of the image intensity function. 
    /// At each point in the image, the result of the Prewitt operator is either the corresponding gradient vector or the norm of this vector.
    /// </remarks>
    [OperationClass("AEGIS::213265", "Sobel filter")]
    public class SobelFilterTransformation : MultiFilterTransformation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SobelFilterTransformation" /> class.
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
        /// </exception>
        public SobelFilterTransformation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SobelFilterTransformation" /> class.
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
        public SobelFilterTransformation(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.SobelFilter, parameters)
        {
            _filters = new Filter[2];
            _filters[0] = FilterFactory.CreateSobelHorizontalFilter();
            _filters[1] = FilterFactory.CreateSobelVerticalFilter();
        }

        #endregion

        #region Protected MultiFilterTransformation methods

        /// <summary>
        /// Combines the specified filtered values.
        /// </summary>
        /// <param name="values">The array of filtered values.</param>
        /// <returns>The combination of the values for the specified filter.</returns>
        protected override Double CombineValues(Double[] values)
        {
            return Math.Sqrt(values[0] * values[0] + values[1] * values[1]);
        }

        #endregion
    }
}
