/// <copyright file="EnumerableExtensions.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Provides extensions to geometry enumerations.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts a geometry enumeration to geometry collection.
        /// </summary>
        /// <typeparam name="T">The type of the geometry.</typeparam>
        /// <param name="collection">The collection of geometries.</param>
        /// <returns>The geometry collection containing the geometries.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The collection is empty.
        /// or
        /// The factory of the geometries in the collection is not the same.
        /// </exception>
        public static IGeometryCollection<T> ToGeometryCollection<T>(this IEnumerable<T> collection) where T : IGeometry
        {
            if (collection == null)
                throw new ArgumentNullException("collection", "The collection is null.");
            if (!collection.Any())
                throw new ArgumentException("The collection is empty.", "collection");
            if (collection.Any(geometry => !geometry.Factory.Equals(collection.First().Factory)))
                throw new ArgumentException("The factory of the geometries in the collection is not the same.", "collection");

            return collection.First().Factory.CreateGeometryCollection<T>(collection);
        }
    }
}
