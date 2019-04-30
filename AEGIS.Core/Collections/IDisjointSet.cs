/// <copyright file="IDisjointSet.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
/// <author>Dávid Kis</author>

using System;
using System.Collections;
using System.Collections.Generic;

namespace ELTE.AEGIS.Collections
{
    /// <summary>
    /// Defines behavior of a disjoint-set data structure
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the sets.</typeparam>
    /// <remarks>
    /// In computing, a disjoint-set data structure, also called a union–find data structure or merge–find set,
    /// is a data structure that keeps track of a set of elements partitioned into a number of disjoint (non-overlapping) subsets.
    /// </remarks>
    public interface IDisjointSet<TElement> : IEnumerable<TElement>, IEnumerable
    {
        /// <summary>
        /// The number of elements in the disjoint-set.
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// The number of disjoint sets.
        /// </summary>
        Int32 SetCount { get; }

        /// <summary>
        /// Makes a set containing only the given element.
        /// </summary>
        /// <remarks>
        /// If the element is already in one of the subsets, nothing happens.
        /// </remarks>
        /// <param name="element">The element.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="element" /> is null.</exception>
        void MakeSet(TElement element);

        /// <summary>
        /// Determine which subset a particular element is in.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The representative for the subset containing <paramref name="element" />.</returns>
        /// <exception cref="System.ArgumentNullException">The <paramref name="element" /> is null.</exception>
        /// <exception cref="System.ArgumentException">The <paramref name="element" /> is not present in any set.</exception>
        TElement Find(TElement element);

        /// <summary>
        /// Joins two subsets into a single subset.
        /// </summary>
        /// <param name="x">Element from the first subset.</param>
        /// <param name="y">Element from the second subset.</param>
        /// <exception cref="System.ArgumentException">
        /// Element <paramref name="x" /> is not present in any set.
        /// or
        /// Element <paramref name="y" /> is not present in any set.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Element <paramref name="x" /> is null.
        /// or
        /// Element <paramref name="y" /> is null.
        /// </exception>
        void Union(TElement x, TElement y);

        /// <summary>
        /// Removes all element from the disjoint-set.
        /// </summary>
        void Clear();
    }
}