/// <copyright file="IBasicPolygon.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines behavior for basic polygon geometries.
    /// </summary>
    public interface IBasicPolygon : IBasicGeometry
    {
        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The <see cref="IBasicLineString" /> representing the shell of the polygon.</value>
        IBasicLineString Shell { get; }

        /// <summary>
        /// Gets the number of holes of the polygon.
        /// </summary>
        /// <value>The number of holes in the polygon.</value>
        Int32 HoleCount { get; }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The <see cref="IList{IBasicCurve}" /> containing the holes of the polygon.</value>
        IList<IBasicLineString> Holes { get; }

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">There are no holes in the polygon.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of holes.
        /// </exception>
        IBasicLineString GetHole(Int32 index);
    }
}
