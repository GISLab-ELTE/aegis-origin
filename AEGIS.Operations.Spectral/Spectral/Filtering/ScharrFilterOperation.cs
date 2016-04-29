/// <copyright file="ScharrFilterTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dóra Papp</author>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represents a gradient filtering operation using Scharr filter.
    /// </summary>
    /// <remarks>
    /// The Scharr edge detection filter is a specialization of the Sobel filter.
    /// Scharr operators result from an optimization minimizing weighted mean squared angular error in Fourier domain. 
    /// </remarks>
    [OperationMethodImplementation("AEGIS::251183", "Scharr filter")]
    public class ScharrFilterOperation : GradientFilterOperation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScharrFilterOperation" /> class.
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
        public ScharrFilterOperation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScharrFilterOperation" /> class.
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
        public ScharrFilterOperation(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.ScharrFilter, parameters)
        {
            AddFilter(FilterFactory.CreateScharrHorizontalFilter());
            AddFilter(FilterFactory.CreateScharrVerticalFilter());
        }

        #endregion

        #region Protected GradientFilterOperation methods

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
