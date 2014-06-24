/// <copyright file="OperationCertificate.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Management
{
    /// <summary>
    /// Represents a certificate indicating whether and when the operation can be performed within the execution environment.
    /// </summary>
    /// <remarks>
    /// The certificate contains validation methods on conditions reguarding the engine, the source and target object and the parameters of the operation.
    /// The operation can only be executed if the all verifications pass. If not, the operation will definately raise exception during runtime.
    /// The certificate also contains a priority indicator. In case multiple operations satisfy the conditions, the operations with the highest priority should be chosen for execution.
    /// </remarks>
    public class OperationCertificate
    {
        #region Properties

        /// <summary>
        /// Gets the priority of the operation.
        /// </summary>
        /// <value>The priority of the operation.</value>
        public virtual Double Priority { get { return 1; } }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the engine.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <returns><c>true</c> if the engine is valid for the operation; otherwise <c>false</c>.</returns>
        public virtual Boolean ValidateEngine(OperationsEngine engine) { return true; }

        /// <summary>
        /// Validates the source object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><c>true</c> if the source object is valid for the operation; otherwise <c>false</c>.</returns>
        public virtual Boolean ValidateSource(Object source) { return true; }

        /// <summary>
        /// Validates the target object.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns><c>true</c> if the target object is valid for the operation; otherwise <c>false</c>.</returns>
        public virtual Boolean ValidateTarget(Object target) { return true; }

        /// <summary>
        /// Validates the target object with respect to the source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns><c>true</c> if the target object is valid for the operation; otherwise <c>false</c>.</returns>
        public virtual Boolean ValidateTarget(Object target, Object source) { return true; }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns><c>true</c> if the parameters are valid for the operation; otherwise <c>false</c>.</returns>
        public virtual Boolean ValidateParameters(IDictionary<OperationParameter, Object> parameters) { return true; }

        /// <summary>
        /// Validates the parameters with respect to source object.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="source">The source.</param>
        /// <returns><c>true</c> if the parameters are valid for the operation; otherwise <c>false</c>.</returns>
        public virtual Boolean ValidateParameters(IDictionary<OperationParameter, Object> parameters, Object source) { return true; }

        /// <summary>
        /// Validates the parameters with respect to source and target objects.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns><c>true</c> if the parameters are valid for the operation; otherwise <c>false</c>.</returns>
        public virtual Boolean ValidateParameters(IDictionary<OperationParameter, Object> parameters, Object source, Object target) { return true; }

        #endregion
    }
}
