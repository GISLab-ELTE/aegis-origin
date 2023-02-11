// <copyright file="IBasicLineString.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for basic curve.
    /// </summary>
    public interface IBasicLineString : IBasicGeometry, IEnumerable<Coordinate>
    {
        /// <summary>
        /// Gets the number of coordinates in the line string.
        /// </summary>
        /// <value>The number of coordinates in the line string.</value>
        Int32 Count { get; }

        /// <summary>
        /// Gets the coordinates in the line string.
        /// </summary>
        /// <value>The read-only list of coordinates of the line string.</value>
        IList<Coordinate> Coordinates { get; }

        /// <summary>
        /// Gets the staring coordinate.
        /// </summary>
        /// <value>The first coordinate of the line string.</value>
        Coordinate StartCoordinate { get; }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The last coordinate of the line string.</value>
        Coordinate EndCoordinate { get; }

        /// <summary>
        /// Determines whether the line string contains the specified coordinate within its coordinates.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line string contains the specified coordinate within its coordinates; otherwise, <c>false</c>.</returns>
        Boolean Contains(Coordinate coordinate);

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
    }
}
