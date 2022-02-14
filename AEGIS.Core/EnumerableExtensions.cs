/// <copyright file="EnumerableExtensions.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections;
using ELTE.AEGIS.Geometry;
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
        #region Extension methods for transformation

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
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

            // in case of empty collection the default factory is used
            if (!collection.Any())
                return new GeometryFactory().CreateGeometryCollection<T>(collection);

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
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

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

        #region Extension methods for search

        /// <summary>
        /// Determines the index of a specific item in the collection.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="match">The condition of the element to search for.</param>
        /// <returns>The zero-based index of item if found in the collection; otherwise, -1.</returns>
        public static Int32 IndexOf<T>(this IEnumerable<T> collection, T item)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

            IList<T> collectionList = collection as IList<T>;
            if (collectionList != null)
                return collectionList.IndexOf(item);

            Int32 index = 0;
            foreach(T currentItem in collection)
            {
                if (currentItem.Equals(item))
                    return index;

                index++;
            }

            return -1;
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of the element that matches the defined predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="match">The condition of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of the element that matches the defined predicate if found in the collection; otherwise, -1.</returns>
        public static Int32 IndexOf<T>(this IEnumerable<T> collection, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

            Int32 index = 0;
            foreach (T item in collection)
            {
                if (match(item))
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Returns the zero-based indices of the elements that match the defined predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="match">The condition of the element to search for.</param>
        /// <returns>The zero-based indices of the elements that match the defined predicate if found in the collection; otherwise, -1.</returns>
        public static IList<Int32> IndicesOf<T>(this IEnumerable<T> collection, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

            IList<Int32> indices = new List<Int32>();

            Int32 index = 0;
            foreach (T item in collection)
            {
                if (match(item))
                    indices.Add(index);
                index++;
            }

            return indices;
        }

        /// <summary>
        /// Returns the index of the largest item within the collection.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The index of the largest item within the collection.</returns>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        /// <exception cref="ArgumentException">The collection is empty.</exception>
        public static Int32 MaxIndex<T>(this IEnumerable<T> collection) where T : IComparable<T>
        {
            return MaxIndex(collection, value => value);
        }

        /// <summary>
        /// Returns the index of the largest item within the collection specified by the selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector function.</param>
        /// <returns>The index of the largest item within the collection specified by the selector.</returns>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        /// <exception cref="ArgumentException">The collection is empty.</exception>
        public static Int32 MaxIndex<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> selector) where TResult : IComparable<TResult>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

            IEnumerator<TSource> enumerator = collection.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new ArgumentException(nameof(collection), "The collection is empty.");

            Int32 index = 0, maxIndex = 0;
            TResult max = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                TResult currentResult = selector(enumerator.Current);
                if (max.CompareTo(currentResult) < 0)
                {
                    max = currentResult;
                    maxIndex = index;
                }
                index++;
            }

            return maxIndex;
        }

        /// <summary>
        /// Returns the index of the smallest item within the collection.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The index of the smallest item within the collection.</returns>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        /// <exception cref="ArgumentException">The collection is empty.</exception>
        public static Int32 MinIndex<T>(this IEnumerable<T> collection) where T : IComparable<T>
        {
            return MinIndex(collection, value => value);
        }

        /// <summary>
        /// Returns the index of the smallest item within the collection specified by the selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector function.</param>
        /// <returns>The index of the smallest item within the collection specified by the selector.</returns>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        /// <exception cref="ArgumentException">The collection is empty.</exception>
        public static Int32 MinIndex<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> selector) where TResult : IComparable<TResult>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

            IEnumerator<TSource> enumerator = collection.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new ArgumentException(nameof(collection), "The collection is empty.");

            Int32 index = 0, minIndex = 0;
            TResult min = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                TResult currentResult = selector(enumerator.Current);
                if (min.CompareTo(currentResult) > 0)
                {
                    min = currentResult;
                    minIndex = index;
                }
                index++;
            }

            return minIndex;
        }

        /// <summary>
        /// Returns the largest item within the collection or the default value if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The largest item within the collection or the default value if the collection is empty.</returns>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        public static T MaxOrDefault<T>(this IEnumerable<T> collection) where T : IComparable<T>
        {
            return MaxOrDefault(collection, value => value);
        }

        /// <summary>
        /// Returns the largest item within the collection specified by the selector or the default value if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The largest item within the collection specified by the selector or the default value if the collection is empty.</returns>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> selector) where TResult : IComparable<TResult>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

            IEnumerator<TSource> enumerator = collection.GetEnumerator();

            if (!enumerator.MoveNext())
                return default(TResult);

            TResult max = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                TResult currentResult = selector(enumerator.Current);
                if (max.CompareTo(currentResult) < 0)
                {
                    max = currentResult;
                }
            }

            return max;
        }

        /// <summary>
        /// Returns the smallest item within the collection or the default value if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The smallest item within the collection or the default value if the collection is empty.</returns>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        public static T MinOrDefault<T>(this IEnumerable<T> collection) where T : IComparable<T>
        {
            return MinOrDefault(collection, value => value);
        }

        /// <summary>
        /// Returns the smallest item within the collection specified by the selector or the default value if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The smallest item within the collection specified by the selector or the default value if the collection is empty.</returns>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> selector) where TResult : IComparable<TResult>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "The collection is null.");

            IEnumerator<TSource> enumerator = collection.GetEnumerator();

            if (!enumerator.MoveNext())
                return default(TResult);
            
            TResult min = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                TResult currentResult = selector(enumerator.Current);
                if (min.CompareTo(currentResult) > 0)
                {
                    min = currentResult;
                }
            }

            return min;
        }

        #endregion
    }
}
