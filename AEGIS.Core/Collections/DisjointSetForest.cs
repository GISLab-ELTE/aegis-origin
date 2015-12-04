/// <copyright file="DisjointSetForest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dávid Kis</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Collections
{
    /// <summary>
    /// Represents a disjoint-set data structure
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the sets.</typeparam>
    /// <remarks>
    /// In computing, a disjoint-set data structure, also called a union–find data structure or merge–find set,
    /// is a data structure that keeps track of a set of elements partitioned into a number of disjoint (nonoverlapping) subsets.
    /// This implementation of the <see cref="IDisjointSet{TElement}" /> interface is the Disjoint-set forests that are data structures
    /// where each set is represented by a tree data structure, in which each node holds a reference to its parent node.
    /// </remarks>
    public class DisjointSetForest<TElement> : IDisjointSet<TElement>, IEnumerable, IEnumerable<TElement>
    {
        #region Private fields

        /// <summary>
        /// The parent of the element in the tree.
        /// </summary>
        private Dictionary<TElement, TElement> _parent;

        /// <summary>
        /// The rank of the subset containing the element.
        /// </summary>
        private Dictionary<TElement, Int32> _rank;

        #endregion

        #region IDisjointSet properties

        /// <summary>
        /// The number of elements in the disjoint-set forest.
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _parent.Keys.Count;
            }
        }

        /// <summary>
        /// The number of disjoint sets.
        /// </summary>
        public Int32 SetCount { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSetForest{TElement}"/> class that is empty and has the default initial capacity.
        /// </summary>
        public DisjointSetForest()
        {
            _parent = new Dictionary<TElement, TElement>();
            _rank = new Dictionary<TElement, int>();
            SetCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSetForest{TElement}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="DisjointSetForest{TElement}" /> can contain, without resizing.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="capacity" /> is less than 0.</exception>
        public DisjointSetForest(Int32 capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity","Capacity is less than 0.");

            _parent = new Dictionary<TElement, TElement>(capacity);
            _rank = new Dictionary<TElement, int>(capacity);
            SetCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSetForest{TElement}"/> class, that contains singletons(set with one element) from the specified <see cref="IEnumerable{TElement}" />. 
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{TElement}" /> whose elements are gonna be singletons in the new <see cref="DisjointSetForest{TElement}" />.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="source" /> is null.</exception>
        public DisjointSetForest(IEnumerable<TElement> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _parent = new Dictionary<TElement, TElement>();
            _rank = new Dictionary<TElement, int>();
            SetCount = 0;
            foreach (TElement element in source)
            {
                MakeSet(element);
            }
        }

        #endregion

        #region IDisjointSet methods

        /// <summary>
        /// Makes a set containing only the given element.
        /// </summary>
        /// <remarks>
        /// If the element is already in one of the subsets, nothing happens.
        /// </remarks>
        /// <param name="element">The element.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="element" /> is null.</exception>
        public void MakeSet(TElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element", "Element is null.");

            if (!_parent.ContainsKey(element))
            {
                _parent[element] = element;
                _rank[element] = 0;
                SetCount++;
            }
        }

        /// <summary>
        /// Determine which subset a particular element is in.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The representative for the subset containing <paramref name="element" />.</returns>
        /// <exception cref="System.ArgumentNullException">The <paramref name="element" /> is null.</exception>
        /// <exception cref="System.ArgumentException">The <paramref name="element" /> is not present in any set.</exception>
        public TElement Find(TElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element", "Element is null.");

            if (!_parent.ContainsKey(element))
                throw new ArgumentException("The element is not present in any set", "element");

            if (_parent[element].Equals(element))
            {
                return element;
            }
            else
            {
                _parent[element] = Find(_parent[element]);
                return _parent[element];
            }
        }

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
        public void Union(TElement x, TElement y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "Element is null.");
            if (y == null)
                throw new ArgumentNullException("y", "Element is null.");
            if (!_parent.ContainsKey(x))
                throw new ArgumentException("The element is not present in any set","x");
            if (!_parent.ContainsKey(y))
                throw new ArgumentException("The element is not present in any set", "y");

            TElement xRoot = Find(x);
            TElement yRoot = Find(y);
            int xRank = _rank[xRoot];
            int yRank = _rank[yRoot];

            if (xRoot.Equals(yRoot))
                return;

            SetCount--;

            if (xRank < yRank)
            {
                _parent[xRoot] = yRoot;

            }
            else if (yRank < xRank)
            {
                _parent[yRoot] = xRoot;

            }
            else
            {
                _parent[xRoot] = yRoot;
                _rank[xRoot]++;
            }
        }

        /// <summary>
        /// Removes all element from disjoint sets/>.
        /// </summary>
        public void Clear()
        {
            _parent.Clear();
            _rank.Clear();
            SetCount = 0;
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection in subset order.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{TElement}" /> object that can be used to iterate through the collection in subset order.</returns>
        public IEnumerable<TElement> GetOrderedEnumerator()
        {
            foreach (var pair in _parent.ToList().Select(x => new KeyValuePair<TElement, TElement>(x.Key, Find(x.Key))).GroupBy(x => x.Value))
            {
                foreach (var keyValuePair in pair)
                {
                    yield return keyValuePair.Key;
                }
            }
        }
        public IEnumerable<TElement> OrderedEnumerator
        {
            get { return GetOrderedEnumerator(); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{TElement}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            foreach (TElement element in _parent.Keys)
            {
                yield return element;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}