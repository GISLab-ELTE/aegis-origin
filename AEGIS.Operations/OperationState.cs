/// <copyright file="OperationState.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Defines the states of operation.
    /// </summary>
    public enum OperationState
    {
        /// <summary>
        /// The operation is ready to be executed.
        /// </summary>
        Initialized,

        /// <summary>
        /// The operations is initializing the resulting object.
        /// </summary>
        Preparing,

        /// <summary>
        /// The operation is computing the result.
        /// </summary>
        Executing,

        /// <summary>
        /// The operation is finalizing the result.
        /// </summary>
        Finalizing,

        /// <summary>
        /// The operation is complete with result.
        /// </summary>
        FinishedWithResult,

        /// <summary>
        /// The operation is complete, but no result is produced.
        /// </summary>
        FinishedWithoutResult,

        /// <summary>
        /// The operation aborted.
        /// </summary>
        Aborted
    }
}
