/// <copyright file="IPolygon.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for polygon geometries.
    /// </summary>
    /// <remarks>
    /// A polygon is a planar surface defined by 1 exterior boundary and 0 or more interior boundaries. Each interior boundary defines a hole in the polygon.
    /// </remarks>
    public interface IPolygon : ISurface
    {
        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The <see cref="ILinearRing" /> representing the shell of the polygon.</value>
        ILinearRing Shell { get; }

        /// <summary>
        /// Gets the number of holes of the polygon.
        /// </summary>
        /// <value>The number of holes in the <see cref="ILinearRing" />.</value>
        Int32 HoleCount { get; }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The <see cref="IList{ILinearRing}" /> containing the holes of the polygon.</value>
        IList<ILinearRing> Holes { get; }

        /// <summary>
        /// Add a hole to the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.ArgumentNullException">The hole is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the hole does not match the reference system of the polygon.</exception>
        void AddHole(ILinearRing hole);

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">There are no holes in the polygon.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// index;The index is less than 0.
        /// or
        /// index;Index is equal to or greater than the number of coordinates.
        /// </exception>
        ILinearRing GetHole(Int32 index);

        /// <summary>
        /// Removes a hole from the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the polygon contains the <paramref name="hole" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The hole is null.</exception>
        Boolean RemoveHole(ILinearRing hole);

        /// <summary>
        /// Removes the hole at the specified index of the polygon.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is greater than or equal to the number of holes.
        /// </exception>
        void RemoveHoleAt(Int32 index);

        /// <summary>
        /// Removes all holes from the polygon.
        /// </summary>
        void ClearHoles();
    }
}
