/// <copyright file="GeometryModel.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines the supported models for geometry representation
    /// </summary>
    [Flags]
    public enum GeometryModel
    {
        /// <summary>
        /// No spatial or temporal support is specified.
        /// </summary>
        None = 1,

        /// <summary>
        /// Spatial with 2 dimensional coordinate system.
        /// </summary>
        Spatial2D = 2,

        /// <summary>
        /// Spatial with 3 dimensional coordinate system.
        /// </summary>
        Spatial3D = 4,

        /// <summary>
        /// Spatio-temporal with 2 dimensional spatial coordinate system and one dimensional temporal coordinate system.
        /// </summary>
        SpatioTemporal2D = 8,

        /// <summary>
        /// Spatio-temporal with 3 dimensional spatial coordinate system and one dimensional temporal coordinate system.
        /// </summary>
        SpatioTemporal3D = 16,

        /// <summary>
        /// Temporal with one dimensional coordinate system.
        /// </summary>
        Temporal = 32
    }
}
