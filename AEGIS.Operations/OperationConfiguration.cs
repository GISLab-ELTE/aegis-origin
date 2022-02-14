/// <copyright file="OperationConfiguration.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
using System.Linq;

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Represents an operation with specified parameters.
    /// </summary>
    public class OperationConfiguration
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
        /// Initializes a new instance of the <see cref="OperationConfiguration" /> class.
        /// </summary>
        /// <param name="method">The operation method.</param>
        /// <param name="parameters">The parameters.</param>
        public OperationConfiguration(OperationMethod method, IDictionary<OperationParameter, Object> parameters)
        {
            if (method == null)
                throw new ArgumentNullException("method", "The method is null.");

            if (parameters == null && method.Parameters != null && method.Parameters.Any(parameter => !parameter.IsOptional))
                throw new ArgumentNullException("parameters", "The method requires parameters which are not specified.");

            if (parameters != null && method.Parameters != null)
            {
                foreach (OperationParameter parameter in method.Parameters)
                {
                    // check parameter existence
                    if (!parameter.IsOptional && (!parameters.ContainsKey(parameter) || parameters[parameter] == null))
                        throw new ArgumentException("The parameters do not contain a required parameter value (" + parameter.Name + ").", "parameters");

                    if (parameters.ContainsKey(parameter) && parameters[parameter] != null)
                    {
                        // check parameter type
                        if (!(parameter.Type.GetInterfaces().Contains(typeof(IConvertible)) && parameters[parameter] is IConvertible) &&
                            !parameter.Type.Equals(parameters[parameter].GetType()) &&
                            !parameters[parameter].GetType().IsSubclassOf(parameter.Type) &&
                            !parameters[parameter].GetType().GetInterfaces().Contains(parameter.Type))
                            throw new ArgumentException("The type of a parameter value (" + parameter.Name + ") does not match the type specified by the method.", "parameters");

                        // check parameter value
                        if (!parameter.IsValid(parameters[parameter]))
                            throw new ArgumentException("The parameter value (" + parameter.Name + ") does not satisfy the conditions of the parameter.", "parameters");
                    }
                }
            }

            Method = method;
            Parameters = parameters;
        }

        #endregion
    }
}
