/// <copyright file="IPointBasedSpatialIndex.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roland Krisztandl</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.LiDAR.Indexes
{
    /// <summary>
    /// Defines behavior of spatial indexes, which contains a collection of <typeparamref name="T"/> instances.
    /// </summary>
    /// <remarks>
    /// This is similar to ISpatialIndex, but instead of IGeometry, this uses <typeparamref name="T"/> and <seealso cref="Coordinate"/>.
    /// </remarks>
    public interface IPointBasedSpatialIndex<T>
    {
        /// <summary>
        /// Gets a value indicating whether the index is read-only.
        /// </summary>
        /// <value><c>true</c> if the index is read-only; otherwise, <c>false</c>.</value>
        Boolean IsReadOnly { get; }

        /// <summary>
        /// Gets the number of indexed points.
        /// </summary>
        /// <value>The number of indexed points.</value>
        Int32 NumberOfPoints { get; }

        /// <summary>
        /// Adds a new object to the index.
        /// </summary>
        /// <param name="point">The object's coordinate.</param>
        /// <param name="obj">Object to add.</param>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Add(T obj, Coordinate point);

        /// <summary>
        /// Searches the index for any objects contained within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The collection of points located within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        List<T> Search(Envelope envelope);

        /// <summary>
        /// Removes the specified object from the index.
        /// </summary>
        /// <param name="obj">Object to remove.</param>
        /// <param name="point">The object's coordinate.</param>
        /// <returns><c>true</c> if the point is indexed; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The point is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(T obj, Coordinate point);

        /// <summary>
        /// Removes all objects from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any points are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Envelope envelope);

        /// <summary>
        /// Removes all objects from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="points">The list of points within the envelope.</param>
        /// <returns><c>true</c> if any points are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Envelope envelope, out List<T> points);

        /// <summary>
        /// Clears all objects from the index.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Clear();

        /// <summary>
        /// Gets all objects from the index.
        /// </summary>
        /// <returns>Every objects in the index.</returns>
        List<T> GetAll();
    }
}
