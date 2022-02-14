/// <copyright file="SparseArray.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamás Szabó</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Collections
{
    /// <summary>
    /// Represents a sparse array.
    /// </summary>
    /// <typeparam name="T">The type of the value in the sparse array.</typeparam>
    /// <remarks>
    /// A sparse array is an array in which most of the elements have the same value (known as the default value—usually 0 or null). 
    /// The occurrence of zero elements in a large array is inefficient for both computation and storage. 
    /// An array in which there is a large number of zero elements is referred to as being sparse.
    /// </remarks>  
    [Serializable]
    public class SparseArray<T> : IList<T>, IReadOnlyList<T>
    {
        #region Public types

        /// <summary>
        /// Supports a simple iteration over a <see cref="SparseArray{T}"/> collection.
        /// </summary>
        [Serializable]
        public class Enumerator : IEnumerator<T>
        {
            #region Private fields

            /// <summary>
            /// The array that is enumerated.
            /// </summary>
            private SparseArray<T> _localArray;

            /// <summary>
            /// The version at which the enumerator was instantiated.
            /// </summary>
            private Int32 _localVersion;

            /// <summary>
            /// The position of the enumerator.
            /// </summary>
            private Int64 _position;

            /// <summary>
            /// The current item.
            /// </summary>
            private T _current;

            /// <summary>
            /// The inner enumerator used for enumerating the items.
            /// </summary>
            private IEnumerator<KeyValuePair<Int64, T>> _innerEnumerator;

            /// <summary>
            /// A value indicating whether this instance is disposed.
            /// </summary>
            private Boolean _disposed;

            #endregion

            #region IEnumerator properties

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            public T Current { get { return _current; } }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            Object IEnumerator.Current { get { return _current; } }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> class.
            /// </summary>
            /// <param name="array">The array.</param>
            /// <exception cref="System.ArgumentNullException">The array is null.</exception>
            internal Enumerator(SparseArray<T> array)
            {
                if (array == null)
                    throw new ArgumentNullException("array", "The array is null.");

                _localVersion = array._version;
                _localArray = array;
                _position = -1;
                _innerEnumerator = array._items.GetEnumerator();
                _current = default(T);

                _disposed = false;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="Enumerator"/> class.
            /// </summary>
            ~Enumerator()
            {
                Dispose(false);
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_disposed)
                    return;

                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            #region IEnumerable methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
            public Boolean MoveNext()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                if (_localVersion != _localArray._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _position++;

                if (_position >= _localArray._length)
                {
                    _position = _localArray._length;
                    _current = default(T);
                    return false;
                }

                // advance the inner enumerator to the next position if any
                while (_position > _innerEnumerator.Current.Key && _innerEnumerator.MoveNext());

                if (_position == _innerEnumerator.Current.Key)
                    _current = _innerEnumerator.Current.Value;
                else
                    _current = default(T);

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_localVersion != _localArray._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _innerEnumerator.Reset();
                _position = -1;

                _current = default(T);
            }

            #endregion

            #region Protected methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
            protected virtual void Dispose(Boolean disposing)
            {                
                _disposed = true;

                if (disposing)
                {
                    _innerEnumerator.Dispose();
                    _innerEnumerator = null;

                    _localArray = null;

                    _current = default(T);
                }
            }

            #endregion
        }

        #endregion

        #region Protected fields

        /// <summary>
        /// The items of the sparse array.
        /// </summary>
        protected Dictionary<Int64, T> _items;

        #endregion

        #region Private fields

        /// <summary>
        /// The version of the array.
        /// </summary>
        private Int32 _version;

        /// <summary>
        /// The number of elements in the sparse array.
        /// </summary>
        private Int64 _length;

        #endregion

        #region ICollection properties

        /// <summary>
        /// Gets the actual number of elements contained in the array.
        /// </summary>
        /// <returns>The actual number of elements contained in the array.</returns>
        public Int32 Count { get { return _items.Count; } }

        /// <summary>
        /// Gets a value indicating whether the array is read-only.
        /// </summary>
        /// <returns><c>true</c> if the array is read-only; otherwise, false.</returns>
        public Boolean IsReadOnly { get { return false; } }

        #endregion

        #region IList properties

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The value located at <paramref name="index" />.</returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of items in the array.
        /// </exception>
        public T this[Int32 index]
        {
            get
            {
                if (index < 0)
                    throw new IndexOutOfRangeException("The index is less than 0.");
                if (index >= _length)
                    throw new IndexOutOfRangeException("The index is equal to or greater than the number of items in the array.");

                T value;
                if (!_items.TryGetValue(index, out value))
                    return default(T);

                return value;
            }
            set
            {
                if (index < 0)
                    throw new IndexOutOfRangeException("The index is less than 0.");
                if (index >= _length)
                    throw new IndexOutOfRangeException("The index is equal to or greater than the number of items in the array.");

                if (_items.ContainsKey(index))
                    _items[index] = value;
                else
                    _items.Add(index, value);

                _version++;
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a 32-bit integer that represents the total number of elements in all the dimensions of the array.
        /// </summary>
        /// <value>A 32-bit integer that represents the total number of elements in all the dimensions of the array; <c>0</c> if there are no elements in the array.</value>
        /// <exception cref="System.OverflowException">The array contains more elements than the maximum value.</exception>
        public Int32 Length 
        { 
            get 
            {
                if (_length > Int32.MaxValue)
                    throw new OverflowException("The array contains more elements than the maximum value.");

                return (Int32)_length; 
            } 
        }

        /// <summary>
        /// Gets a 64-bit integer that represents the total number of elements in all the dimensions of the array.
        /// </summary>
        /// <value>A 64-bit integer that represents the total number of elements in all the dimensions of the array; <c>0</c> if there are no elements in the array.</value>
        public Int64 LongLength { get { return _length; } }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The value located at <paramref name="index" />.</returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of items in the array.
        /// </exception>
        public T this[Int64 index]
        {
            get
            {
                if (index < 0)
                    throw new IndexOutOfRangeException("The index is less than 0.");
                if (index >= _length)
                    throw new IndexOutOfRangeException("The index is equal to or greater than the number of items in the array.");

                T value;
                if (!_items.TryGetValue(index, out value))
                    return default(T);

                return value;
            }
            set
            {
                if (index < 0)
                    throw new IndexOutOfRangeException("The index is less than 0.");
                if (index >= _length)
                    throw new IndexOutOfRangeException("The index is equal to or greater than the number of items in the array.");

                if (_items.ContainsKey(index))
                    _items[index] = value;
                else
                    _items.Add(index, value);

                _version++;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseArray{T}" /> class.
        /// </summary>
        /// <param name="length">The number of elements that the array can initially store.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The length is less than 0.</exception>
        public SparseArray(Int64 length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "The length is less than 0.");

            _items = new Dictionary<Int64, T>();
            _version = 0;
            _length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseArray{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public SparseArray(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection", "The collection is null.");

            _items = new Dictionary<Int64, T>();
            _version = 0;

            if (collection is IList<T>)
            {
                IList<T> collectionList = collection as IList<T>;

                _length = collectionList.Count;

                for (Int32 i = 0; i < collectionList.Count; i++)
                    if (!AreEqual(collectionList[i], default(T)))
                        _items.Add(i, collectionList[i]);
            }
            else
            {
                Int32 count = 0;

                foreach (T item in collection)
                {
                    if (!AreEqual(item, default(T)))
                        _items.Add(count, item);

                    count++;
                }

                _length = count;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines the index of a specific item in the array.
        /// </summary>
        /// <param name="item">The object to locate in the array.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, <c>-1</c>.</returns>
        public Int64 IndexOf(T item)
        {
            if (AreEqual(item, default(T)))
            {
                var keys = _items.Keys.ToList();
                for (Int32 index = 0; index < keys.Count; index++)
                {
                    if (index != keys[index])
                        return index;
                }
            }
            else
            {
                foreach (KeyValuePair<Int64, T> pair in _items)
                {
                    if (AreEqual(pair.Value, item))
                        return pair.Key;
                }
            }

            return -1;
        }

        /// <summary>
        /// Inserts an item to the array at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the array.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of elements in the array.
        /// </exception>
        public void Insert(Int64 index, T item)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _length)
                throw new ArgumentOutOfRangeException("index", "The index is equal to or greater than the number of elements in the array.");

            UpdateIndices(index, 1);

            if (!AreEqual(item, default(T)))
            {
                _items.Add(index, item);
            }

            _length++;
            _version++;
        }

        /// <summary>
        /// Removes the array item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of elements in the array.
        /// </exception>
        public void RemoveAt(Int64 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _length)
                throw new ArgumentOutOfRangeException("index", "The index is equal to or greater than the number of elements in the array.");

            if (_items.ContainsKey(index))
                _items.Remove(index);

            UpdateIndices(index, -1);

            _length--;
            _version++;
        }

        #endregion

        #region IList methods

        /// <summary>
        /// Determines the index of a specific item in the array.
        /// </summary>
        /// <param name="item">The object to locate in the array.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, <c>-1</c>.</returns>
        Int32 IList<T>.IndexOf(T item)
        {
            if (AreEqual(item, default(T)))
            {
                var keys = _items.Keys.ToList();
                for (Int32 index = 0; index < keys.Count; index++)
                {
                    if (index != keys[index])
                        return index;
                }
            }
            else
            {
                foreach (KeyValuePair<Int64, T> pair in _items)
                {
                    if (AreEqual(pair.Value, item))
                        return (Int32)pair.Key;
                }
            }

            return -1;
        }

        /// <summary>
        /// Inserts an item to the array at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the array.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of elements in the array.
        /// </exception>
        void IList<T>.Insert(Int32 index, T item)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _length)
                throw new ArgumentOutOfRangeException("index", "The index is equal to or greater than the number of elements in the array.");

            UpdateIndices(index, 1);

            if (!AreEqual(item, default(T)))
            {
                _items.Add(index, item);
            }

            _length++;
            _version++;
        }

        /// <summary>
        /// Removes the array item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of elements in the array.
        /// </exception>
        void IList<T>.RemoveAt(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _length)
                throw new ArgumentOutOfRangeException("index", "The index is equal to or greater than the number of elements in the array.");

            if (_items.ContainsKey(index))
                _items.Remove(index);

            UpdateIndices(index, -1);

            _length--;
            _version++;
        }

        #endregion

        #region ICollection methods

        /// <summary>
        /// Adds an item to the array.
        /// </summary>
        /// <param name="item">The object to add to the array.</param>
        public void Add(T item)
        {
            if (!AreEqual(item, default(T)))
                _items.Add(_length, item);

            _length++;
            _version++;
        }

        /// <summary>
        /// Removes all items from the array.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Determines whether the array contains a specific value.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="item"/> is found in the array; otherwise, <c>false</c>.</returns>
        /// <param name="item">The object to locate in the array.</param>
        public Boolean Contains(T item)
        {
            if (AreEqual(item, default(T)))
                return _length > _items.Count;

            return _items.Values.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the current collection to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from the current collection. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The array index is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.</exception>
        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", "The array index is less than 0.");
            if (array.Length - arrayIndex < _length)
                throw new ArgumentException("The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.", "arrayIndex");

            Int64 arrayLongIndex = arrayIndex;

            foreach (T item in this)
            {
                array[arrayLongIndex] = item;
                arrayLongIndex++;
            } 
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the array.
        /// </summary>
        /// <param name="item">The object to remove from the array.</param>
        /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the array; otherwise, <c>false</c>.
        /// </returns>
        public Boolean Remove(T item)
        {
            Int64 index = IndexOf(item);

            if (index != -1)
            {
                if (!AreEqual(item, default(T)))
                    _items.Remove(index);

                UpdateIndices(index, -1);

                _length--;
                _version++;

                return true;
            }

            return false;
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{TValue}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updates the indices of array.
        /// </summary>
        /// <param name="index">The staring index.</param>
        /// <param name="offset">The offset.</param>
        private void UpdateIndices(Int64 index, Int32 offset)
        {
            _items = _items.ToDictionary(pair => pair.Key < index ? pair.Key : pair.Key + offset, pair => pair.Value);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Returns true if two objects are equal. 
        /// </summary>
        private static Boolean AreEqual(T item, T value)
        {
            return EqualityComparer<T>.Default.Equals(item, value);
        }

        #endregion
    }
}
