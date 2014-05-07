/// <copyright file="ParameterizedOperation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Represents an operation with specified parameters.
    /// </summary>
    public class ParameterizedOperation
    {
        #region Public properties

        /// <summary>
        /// Gets the operation method.
        /// </summary>
        /// <value>The operation method.</value>
        public OperationMethod Method { get; private set; }

        /// <summary>
        /// Gets the parameters of the operation.
        /// </summary>
        /// <value>The parameters of the operation.</value>
        public IDictionary<OperationParameter, Object> Parameters { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterizedOperation" /> class.
        /// </summary>
        /// <param name="method">The operation method.</param>
        /// <param name="parameters">The parameters.</param>
        public ParameterizedOperation(OperationMethod method, IDictionary<OperationParameter, Object> parameters)
        {
            if (method == null)
                throw new ArgumentNullException("method", "The method is null.");

            Method = method;
            Parameters = parameters;
        }

        #endregion
    }
}
