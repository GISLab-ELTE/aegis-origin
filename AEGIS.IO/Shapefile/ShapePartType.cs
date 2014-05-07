/// <copyright file="ShapePartType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.IO
{
    /// <summary>
    /// Defines the types of the shape parts.
    /// </summary>
    enum ShapePartType
    {
        /// <summary>
        /// Triangle strip.
        /// </summary>
        TriangleStrip = 0,

        /// <summary>
        /// Triangle fan.
        /// </summary>
        TrianlgeFan = 1,

        /// <summary>
        /// Outer ring.
        /// </summary>
        OuterRing = 2,

        /// <summary>
        /// Inner ring.
        /// </summary>
        InnerRing = 3,

        /// <summary>
        /// First ring.
        /// </summary>
        FirstRing = 4,

        /// <summary>
        /// Ring.
        /// </summary>
        Ring = 5
    }
}
