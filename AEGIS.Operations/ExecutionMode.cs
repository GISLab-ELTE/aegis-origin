// <copyright file="ExecutionMode.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Defines the modes of operation execution.
    /// </summary>
    [Flags]
    public enum ExecutionMode
    {
        /// <summary>
        /// The operation places all results within the source.
        /// </summary>
        InPlace = 1,

        /// <summary>
        /// The operation creates all results in a new object and does not modify the source.
        /// </summary>
        OutPlace = 2,

        /// <summary>
        /// The operation can place all results in either the source or in a new object.
        /// </summary>
        Any
    }
}
