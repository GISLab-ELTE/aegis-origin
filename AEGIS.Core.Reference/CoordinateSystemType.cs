/// <copyright file="CoordinateSystemType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Defines the types of coordinate systems.
    /// </summary>
    public enum CoordinateSystemType 
    { 
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown, 

        /// <summary>
        /// Affine.
        /// </summary>
        Affine, 

        /// <summary>
        /// Cartesian.
        /// </summary>
        Cartesian, 

        /// <summary>
        /// Cylindrical.
        /// </summary>
        Cylindrical, 

        /// <summary>
        /// Ellipsoidal.
        /// </summary>
        Ellipsoidal, 

        /// <summary>
        /// Linear.
        /// </summary>
        Linear, 

        /// <summary>
        /// Polar.
        /// </summary>
        Polar, 

        /// <summary>
        /// Spherical.
        /// </summary>
        Spherical, 

        /// <summary>
        /// UserDefined.
        /// </summary>
        UserDefined, 

        /// <summary>
        /// Vertical.
        /// </summary>
        Vertical, 

        /// <summary>
        /// Temporal.
        /// </summary>
        Temporal 
    }
}
