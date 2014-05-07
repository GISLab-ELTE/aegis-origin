/// <copyright file="ICurve.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for curve geometries.
    /// </summary>
    /// <remarks>
    /// A curve is a one-dimensional geometric object usually stored as a sequence of points, with the subtype of the curve specifying the form of the interpolation between the points.
    /// </remarks>
    public interface ICurve : IGeometry, IEnumerable<Coordinate>
    {
        /// <summary>
        /// Gets a value indicating whether the curve is closed.
        /// </summary>
        /// <value><c>true</c> if the starting and ending coordinates are equal; otherwise, <c>false</c>.</value>
        Boolean IsClosed { get; }

        /// <summary>
        /// Gets a value indicating whether the curve is a ring.
        /// </summary>
        /// <value><c>true</c> if the curve is simple and closed; otherwise, <c>false</c>.</value>
        Boolean IsRing { get; }

        /// <summary>
        /// Gets the number of coordinates in the curve.
        /// </summary>
        /// <value>The number of coordinates in the curve.</value>
        Int32 Count { get; }

        /// <summary>
        /// Gets the length of the curve.
        /// </summary>
        /// <value>The length of the curve.</value>
        Double Length { get; }

        /// <summary>
        /// Gets the list of coordinates in the curve.
        /// </summary>
        /// <value>The coordinates of the curve.</value>
        IList<Coordinate> Coordinates { get; }

        /// <summary>
        /// Gets the staring coordinate.
        /// </summary>
        /// <value>The first coordinate of the curve.</value>
        Coordinate StartCoordinate { get; }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The last coordinate of the curve.</value>
        Coordinate EndCoordinate { get; }

        /// <summary>
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to get.</param>
        /// <returns>The coordinate located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        Coordinate GetCoordinate(Int32 index);

        /// <summary>
        /// Sets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to set.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        void SetCoordinate(Int32 index, Coordinate coordinate);
    }
}
