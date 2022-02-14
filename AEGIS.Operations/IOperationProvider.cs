/// <copyright file="IOperationProvider.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations
{
    using IOperation = IOperation<Object, Object>;

    /// <summary>
    /// Defines behavior for operation providers.
    /// </summary>
    /// <remarks>
    /// The operation provider is a source where operations are located, that can be either retrieved to be executed by the current <see cref="IOperationsEngine"/> instance, or directly executed by the provider.
    /// </remarks>
    public interface IOperationProvider : IDisposable
    {
        /// <summary>
        /// Gets the uniform resource identifier (URI) of the provider.
        /// </summary>
        /// <value>The uniform resource identifier of the provider.</value>
        Uri Uri { get; }

        /// <summary>
        /// Returns the operation methods supported by the provider.
        /// </summary>
        /// <returns>The read-only list of operation methods supported by the provider.</returns>
        IList<OperationMethod> GetMethods();

        /// <summary>
        /// Returns all operations provided by the provider.
        /// </summary>
        /// <returns>The read-only dictionary of provided operations.</returns>
        IDictionary<OperationMethod, IList<Type>> GetOperations();

        /// <summary>
        /// Returns the operations for the specified method.
        /// </summary>
        /// <param name="method">The operation method.</param>
        /// <returns>The read-only list of provided operations for the specified method.</returns>
        /// <exception cref="System.ArgumentNullException">The method is null.</exception>
        IList<Type> GetOperations(OperationMethod method);
    }
}
