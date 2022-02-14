/// <copyright file="IOperation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// <summary>
    /// Defines general behavior of operations.
    /// </summary>
    /// <typeparam name="SourceType">The type of the source.</typeparam>
    /// <typeparam name="ResultType">The type of the result.</typeparam>
    public interface IOperation<out SourceType, out ResultType>
    {
        /// <summary>
        /// Gets the method associated with the operation.
        /// </summary>
        /// <value>The associated operation method.</value>
        OperationMethod Method { get; }

        /// <summary>
        /// Gets the parameters of the operation.
        /// </summary>
        /// <value>The parameters of the operation stored as key/value pairs.</value>
        IDictionary<OperationParameter, Object> Parameters { get; }

        /// <summary>
        /// Gets a value indicating whether the operation is reversible.
        /// </summary>
        /// <value><c>true</c> if the operation is reversible; otherwise, <c>false</c>.</value>
        Boolean IsReversible { get; }

        /// <summary>
        /// Gets the operation state.
        /// </summary>
        /// <value>The current state of the operation.</value>
        OperationState State { get; }

        /// <summary>
        /// Gets the source of the operation.
        /// </summary>
        /// <value>The source of the operation.</value>
        SourceType Source { get; }

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        /// <value>The result of the operation.</value>
        ResultType Result { get; }

        /// <summary>
        /// Executes the operation.
        /// </summary>
        void Execute();

        /// <summary>
        /// Returns the reverse operation.
        /// </summary>
        /// <returns>The reverse operation.</returns>
        IOperation<ResultType, SourceType> GetReverseOperation();
    }
}
