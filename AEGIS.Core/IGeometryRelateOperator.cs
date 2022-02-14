///<copyright file="IGeometryRelateOperator.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines generalized spatial relation operations for <see cref="IGeometry" /> instances.
    /// </summary>
    public interface IGeometryRelateOperator : IDisposable
    {       
        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially contains another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially contains the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        Boolean Contains(IGeometry geometry, IGeometry otherGeometry);

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially crosses another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially crosses the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        Boolean Crosses(IGeometry geometry, IGeometry otherGeometry);

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance is disjoint from another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry is disjoint from the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        Boolean Disjoint(IGeometry geometry, IGeometry otherGeometry);

        /// <summary>
        /// Determines whether the specified <see cref="IGeometry" /> instances are spatially equal.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the specified <see cref="IGeometry" /> instances are spatially equal; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        Boolean Equals(IGeometry geometry, IGeometry otherGeometry);

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially intersects another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially intersects the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        Boolean Intersects(IGeometry geometry, IGeometry otherGeometry);

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially overlaps another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially overlaps the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        Boolean Overlaps(IGeometry geometry, IGeometry otherGeometry);

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially touches another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially touches the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        Boolean Touches(IGeometry geometry, IGeometry otherGeometry);

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance is spatially within another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry is spatially within the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The reference system of two geometries are different.
        /// or
        /// The operation is not supported with the specified geometry type.
        /// </exception>
        Boolean Within(IGeometry geometry, IGeometry otherGeometry);
    }
}
