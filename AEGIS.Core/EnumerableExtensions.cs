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

using ELTE.AEGIS.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Provides extensions to enumerations.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Public static extension methods

        /// <summary>
        /// Converts a geometry enumeration to geometry collection.
        /// </summary>
        /// <typeparam name="T">The type of the geometry.</typeparam>
        /// <param name="collection">The collection of geometries.</param>
        /// <returns>The geometry collection containing the geometries.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        /// <exception cref="System.ArgumentException">The factory of the geometries in the collection is not the same.</exception>
        public static IGeometryCollection<T> ToGeometryCollection<T>(this IEnumerable<T> collection) where T : IGeometry
        {
            if (collection == null)
                throw new ArgumentNullException("collection", "The collection is null.");

            // in case of empty collection the default factory is used
            if (!collection.Any())
                return Factory.DefaultInstance<IGeometryFactory>().CreateGeometryCollection<T>(collection);

            // the factory of the 
            if (collection.Any(geometry => !geometry.Factory.Equals(collection.First().Factory)))
                throw new ArgumentException("The factory of the geometries in the collection is not the same.", "collection");

            return collection.First().Factory.CreateGeometryCollection<T>(collection);
        }

        /// <summary>
        /// Returns a read-only list wrapper for the current collection.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The list.</param>
        /// <returns>A <see cref="ReadOnlySet{T}"/> that acts as a read-only wrapper around the current <see cref="IList{T}"/>.</returns>
        public static IList<T> AsReadOnly<T>(this IList<T> collection)
        {
            return new ReadOnlyCollection<T>(collection);
        }

        /// <summary>
        /// Returns a read-only dictionary wrapper for the current collection.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="collection">The dictionary.</param>
        /// <returns>A <see cref="ReadOnlyDictionary{TKey, TValue}"/> that acts as a read-only wrapper around the current <see cref="IDictionary{TKey, TValue}"/>.</returns>
        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection", "The collection is null.");

            return new ReadOnlyDictionary<TKey, TValue>(collection);
        }

        /// <summary>
        /// Returns a read-only set wrapper for the current collection.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The set.</param>
        /// <returns>A <see cref="ReadOnlySet{T}"/> that acts as a read-only wrapper around the current <see cref="ISet{T}"/>.</returns>
        public static ISet<T> AsReadOnly<T>(this ISet<T> collection)
        {
            return new ReadOnlySet<T>(collection);
        }

        #endregion
    }
}
