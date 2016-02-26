/// <copyright file="CoordinateConversion.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a coordinate conversion.
    /// </summary>
    /// <typeparam name="SourceType">The type of the source.</typeparam>
    /// <typeparam name="ResultType">The type of the result.</typeparam>
    /// <remarks>
    /// A coordinate operation through which the output coordinates are referenced to the same datum as are the input coordinates. 
    /// The best-known example of a coordinate conversion is a map projection. 
    /// The parameter values describing coordinate conversions are defined rather than empirically derived.
    /// </remarks>
    public abstract class CoordinateConversion<SourceType, ResultType> : CoordinateOperation<SourceType, ResultType>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperation{SourceType, DestinationType}" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// </exception>
        protected CoordinateConversion(String identifier, String name, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, method, parameters)
        {
        }

        #endregion
    }
}
