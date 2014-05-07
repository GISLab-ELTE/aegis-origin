/// <copyright file="IPoint.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines behavior for point geometries.
    /// </summary>
    /// <remarks>
    /// A Point is a 0-dimensional geometric object and represents a single location in coordinate space.
    /// </remarks>
    public interface IPoint : IGeometry
    {
        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        Double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>The Y coordinate.</value>
        Double Y { get; set; }

        /// <summary>
        /// Gets or sets the Z coordinate.
        /// </summary>
        /// <value>The Z coordinate.</value>
        Double Z { get; set; }

        /// <summary>
        /// Gets or sets the coordinate associated with the point.
        /// </summary>
        /// <value>The coordinate associated with the point.</value>
        Coordinate Coordinate { get; set; }
    }
}
