﻿/// <copyright file="OperationAspect.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Defines the ellipsoidal application aspects of operation.
    /// </summary>
    public enum OperationAspect
    {
        /// <summary>
        /// North polar case.
        /// </summary>
        NorthPolar,

        /// <summary>
        /// South polar case.
        /// </summary>
        SouthPolar,

        /// <summary>
        /// Oblique case.
        /// </summary>
        Oblique,

        /// <summary>
        /// Equatorial case.
        /// </summary>
        Equatorial,

        /// <summary>
        /// Undefined case.
        /// </summary>
        Undefined
    }
}
