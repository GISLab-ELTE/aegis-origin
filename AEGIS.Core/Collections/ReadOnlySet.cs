/// <copyright file="ReadOnlySet.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Collections
{
    /// <summary>
    /// Represents a read-only set of values.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public class ReadOnlySet<T> : ISet<T>
    {
        #region Private fields

        /// <summary>
        /// The underlying set. This field is read-only.
        /// </summary>
        private readonly ISet<T> _set; 

        #endregion

        #region ICollection properties

        /// <summary>
        /// Gets the number of elements contained in the set.
        /// </summary>
        /// <value>The number of elements contained in the set.</value>
        public Int32 Count
        {
            get { return _set.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <returns><c>true</c> if the collection is read-only; otherwise, <c>false</c>.</returns>
        public Boolean IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySet{T}" /> class.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <exception cref="System.ArgumentNullException">The set is null.</exception>
        public ReadOnlySet(ISet<T> set)
        {
            if (set == null)
                throw new ArgumentNullException("set", "The set is null.");

            _set = set;
        }

        #endregion

        #region ISet methods

        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// <returns><c>true</c> if the current set is a subset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public Boolean IsSubsetOf(IEnumerable<T> other)
        {
            return _set.IsSubsetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <returns><c>true</c> if the current set is a superset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public Boolean IsSupersetOf(IEnumerable<T> other)
        {
            return _set.IsSupersetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a correct superset of a specified collection.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="T:System.Collections.Generic.ISet`1"/> object is a correct superset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        /// <param name="other">The collection to compare to the current set. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public Boolean IsProperSupersetOf(IEnumerable<T> other)
        {
            return _set.IsProperSupersetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a property (strict) subset of a specified collection.
        /// </summary>
        /// <returns><c>true</c> if the current set is a correct subset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public Boolean IsProperSubsetOf(IEnumerable<T> other)
        {
            return _set.IsProperSubsetOf(other);
        }

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <returns><c>true</c> if the current set and <paramref name="other"/> share at least one common element; otherwise, <c>false</c>.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public Boolean Overlaps(IEnumerable<T> other)
        {
            return _set.Overlaps(other);
        }

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <returns><c>true</c> if the current set is equal to <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        public Boolean SetEquals(IEnumerable<T> other)
        {
            return _set.SetEquals(other);
        }

        #endregion

        #region ISet methods (explicit)

        /// <summary>
        /// Modifies the current set so that it contains all elements that are present in both the current set and in the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="System.NotSupportedException">The set is read-only.</exception>
        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("The set is read-only.");
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are also in a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="System.NotSupportedException">The set is read-only.</exception>
        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("The set is read-only.");
        }

        /// <summary>
        /// Removes all elements in the specified collection from the current set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the set.</param>
        /// <exception cref="System.NotSupportedException">The set is read-only.</exception>
        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("The set is read-only.");
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="System.NotSupportedException">The set is read-only.</exception>
        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("The set is read-only.");
        }

        /// <summary>
        /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns><c>true</c> if the element is added to the set; <c>false</c> if the element is already in the set.</returns>
        /// <exception cref="System.NotSupportedException">The set is read-only.</exception>
        Boolean ISet<T>.Add(T item)
        {
            throw new NotSupportedException("The set is read-only.");
        }

        #endregion

        #region ICollection methods

        /// <summary>
        /// Determines whether the set contains a specific value.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="item"/> is found in the set; otherwise, <c>false</c>.</returns>
        /// <param name="item">The object to locate in the set.</param>
        public Boolean Contains(T item)
        {
            return _set.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the set to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from set. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="array"/> is multidimensional.
        /// or
        /// The number of elements in the source set is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// or
        /// Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            _set.CopyTo(array, arrayIndex);
        }

        #endregion

        #region ICollection methods (explicit)

        /// <summary>
        /// Adds an item to the set.
        /// </summary>
        /// <param name="item">The object to add to the set.</param>
        /// <exception cref="System.NotSupportedException">The set is read-only.</exception>
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException("The set is read-only.");
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref="System.NotSupportedException">The set is read-only.</exception>
        void ICollection<T>.Clear()
        {
            throw new NotSupportedException("The set is read-only.");
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the set.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="item"/> was successfully removed from the set; otherwise, <c>false</c>.</returns>
        /// <param name="item">The object to remove from the set.</param>
        Boolean ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException("The set is read-only.");
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        #endregion
    }
}
