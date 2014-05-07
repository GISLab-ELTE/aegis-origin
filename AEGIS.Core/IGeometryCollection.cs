/// <copyright file="IGeometryCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines behavior for generic geometry collections in coordinate space.
    /// </summary>
    public interface IGeometryCollection : IGeometryCollection<IGeometry>
    { 
    }

    /// <summary>
    /// Defines behavior for generic geometry collections in coordinate space.
    /// </summary>
    /// <typeparam name="T">The type of geometry.</typeparam>
    public interface IGeometryCollection<out T> : IGeometry, IEnumerable<T> where T : IGeometry
    {
        /// <summary>
        /// Gets the number of geometries contained in the <see cref="IGeometryCollection{T}" />.
        /// </summary>
        /// <value>
        /// The number of geometries contained in the <see cref="IGeometryCollection{T}" />.
        /// </value>
        Int32 Count { get; }

        /// <summary>
        /// Gets a geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to get.</param>
        /// <returns>The geometry at the specified index.</returns>
        T this[Int32 index] { get; }
    }
}
