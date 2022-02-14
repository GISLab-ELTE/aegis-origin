/// <copyright file="ISpectralIndex.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS.Indices
{
    /// <summary>
    /// Defines behavior of spectral indices.
    /// </summary>
    public interface ISpectralIndex
    {
        /// <summary>
        /// Adds a raster to the index.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Add(IRaster raster);

        /// <summary>
        /// Adds a raster to the index.
        /// </summary>
        /// <param name="collection">The raster collection.</param>
        /// <exception cref="System.ArgumentNullException">The raster collection is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Add(IEnumerable<IRaster> collection);

        /// <summary>
        /// Queries the index for any raster data within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The raster representing the content within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        IRaster Query(Envelope envelope);

        /// <summary>
        /// Determines whether the raster contains the raster.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <returns><c>true</c> if the index contains the raster; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        Boolean Contains(IRaster raster);

        /// <summary>
        /// Determines whether the raster contains data in the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any raster data is within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        Boolean Contains(Envelope envelope);

        /// <summary>
        /// Removes the specified raster from the index.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <returns><c>true</c> if the index contains the raster; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(IRaster raster);

        /// <summary>
        /// Removes the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any raster data is within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Envelope envelope);

        /// <summary>
        /// Removes the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="raster">The raster removed from the index.</param>
        /// <returns><c>true</c> if any raster data is within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Envelope envelope, out IRaster raster);

        /// <summary>
        /// Clears all content from the index.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Clear();
    }
}
