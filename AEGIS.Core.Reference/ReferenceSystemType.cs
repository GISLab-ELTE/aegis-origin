// <copyright file="ReferenceSystemType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Defines the types of coordinate reference systems.
    /// </summary>
    public enum ReferenceSystemType 
    { 
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown, 

        /// <summary>
        /// Compound.
        /// </summary>
        Compound, 

        /// <summary>
        /// Geocentric.
        /// </summary>
        Geocentric,

        /// <summary>
        /// Geographic 2D.
        /// </summary>
        Geographic2D,

        /// <summary>
        /// Geographic 3D.
        /// </summary>
        Geographic3D,

        /// <summary>
        /// Grid.
        /// </summary>
        Grid,

        /// <summary>
        /// Projected.
        /// </summary>
        Projected, 

        /// <summary>
        /// Temporal.
        /// </summary>
        Temporal,

        /// <summary>
        /// User defined.
        /// </summary>
        UserDefined, 

        /// <summary>
        /// Vertical.
        /// </summary>
        Vertical
    }
}