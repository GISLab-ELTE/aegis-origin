/// <copyright file="Heap.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Collections
{
    /// <summary>
    /// Represents a heap data structure containing key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the heap.</typeparam>
    /// <typeparam name="TValue">The type of the values in the heap.</typeparam>
    /// <remarks>
    /// A heap is a specialized tree-based data structure that satisfies the heap property: 
    /// If A is a parent node of B then key(A) is ordered with respect to key(B) with the 
    /// same ordering applying across the heap.
    /// This implementation of the <see cref="IHeap{TKey, TValue}" /> interface uses the default ordering scheme of the key type if not 
    /// specified differently by providing an instance of the  <see cref="IComparer{T}" /> interface. 
    /// For example, the default ordering provides a min heap in case of number types.
    /// The storage of the key/value pairs is array based with O(log n) insertion and removal time.
    /// </remarks>
    [Serializable]
    public class Heap<TKey, TValue> : IHeap<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        #region Public types

        /// <summary>
        /// Enumerates the elements of a heap.
        /// </summary>
        /// <remarks>
        /// The enumerator performs a level order traversal of the specified heap.
        /// </remarks>
        [Serializable]
        public class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
        {
            #region Private fields

            /// <summary>
            /// The heap that is enumerated.
            /// </summary>
            private Heap<TKey, TValue> _localHeap;

            /// <summary>
            /// The version at which the enumerator was instantiated.
            /// </summary>
            private Int32 _localVersion;

            /// <summary>
            /// The position of the enumerator.
            /// </summary>
            private Int32 _position;

            /// <summary>
            /// The current item.
            /// </summary>
            private KeyValuePair<TKey, TValue> _current;

            /// <summary>
            /// A value indicating whether this instance is disposed.
            /// </summary>
            private Boolean _disposed;

            #endregion

            #region IEnumerable properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>The element at the current position of the enumerator.</value>
            public KeyValuePair<TKey, TValue> Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>The element at the current position of the enumerator.-</value>
            Object IEnumerator.Current
            {
                get { return _current; }
            }

            #endregion

            #region Constructors and destructor

            /// <summary>
            /// Initializes a new instance of the <see cref="Heap{TKey, TValue}.Enumerator" /> class.
            /// </summary>
            /// <param name="heap">The heap.</param>
            internal Enumerator(Heap<TKey, TValue> heap)
            {
                _localHeap = heap;
                _localVersion = heap._version;

                _position = -1;
                _current = default(KeyValuePair<TKey, TValue>);
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

            #region IEnumerable methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localHeap._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _position++;

                if (_position >= _localHeap._size)
                {
                    _position = _localHeap._size;
                    _current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }
                else
                {
                    _current = _localHeap._items[_position];
                    return true;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localHeap._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _position = -1;
                _current = default(KeyValuePair<TKey, TValue>);
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
                    _localHeap = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                }
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The default capacity. This field is constant.
        /// </summary>
        private const Int32 DefaultCapacity = 4;

        /// <summary>
        /// The items of the heap stored in an array.
        /// </summary>
        private KeyValuePair<TKey, TValue>[] _items;

        /// <summary>
        /// The number of items stored in the heap.
        /// </summary>
        private Int32 _size;

        /// <summary>
        /// The version of the heap.
        /// </summary>
        private Int32 _version;

        /// <summary>
        /// The comparer that is used to determine order of keys for the heap..
        /// </summary>
        private readonly IComparer<TKey> _comparer;

        /// <summary>
        /// The empty array. This field is read-only.
        /// </summary>
        private static readonly KeyValuePair<TKey, TValue>[] _emptyArray = new KeyValuePair<TKey, TValue>[0];

        #endregion

        #region IHeap properties

        /// <summary>
        /// Gets the number of elements actually contained in the heap.
        /// </summary>
        /// <value>The number of elements actually contained in the heap.</value>
        public Int32 Count { get { return _size; } }

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <value>The number of elements that the heap can contain before resizing is required.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Capacity is set to a value that is less than <see cref="Count" />.</exception>
        public Int32 Capacity { 
            get { return _items.Length; }
            set
            {
                if (value != _items.Length)
                {
                    if (value < _size)
                        throw new InvalidOperationException("Capacity is set to a value that is less than Count.");

                    if (value > 0)
                    {
                        KeyValuePair<TKey, TValue>[] newItems = new KeyValuePair<TKey, TValue>[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the value at the top of the heap without removing it.
        /// </summary>
        /// <value>The value at the beginning of the heap.</value>
        /// <exception cref="System.InvalidOperationException">The heap is empty.</exception>
        public TValue Peek 
        { 
            get 
            {
                if (_size == 0)
                    throw new InvalidOperationException("The heap is empty.");

                return _items[0].Value; 
            } 
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the <see cref="IComparer{T}" /> that is used to determine order of keys for the heap. 
        /// </summary>
        /// <value>The <see cref="IComparer{T}" /> generic interface implementation that is used to determine order of keys for the current heap and to provide hash values for the keys.</value>
        public IComparer<TKey> Comparer { get { return _comparer; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the default comparer for the key type.
        /// </summary>
        public Heap() 
        {
            _items = _emptyArray;
            _size = 0;
            _version = 0;
            _comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the specified initial capacity, and uses the default comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="Heap{TKey, TValue}" /> can contain.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public Heap(Int32 capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("Capacity is less than 0.");

            _items = new KeyValuePair<TKey, TValue>[capacity];
            _size = 0;
            _version = 0;
            _comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="IComparer{T}" />.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        public Heap(IComparer<TKey> comparer)
        {
            _size = 0;
            _version = 0;
            _items = _emptyArray;

            _comparer = comparer ?? Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that contains elements copied from the specified <see cref="Heap{TKey, TValue}" /> and uses the default comparer for the key type.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{KeyValuePair{TKey, TValue}}" /> whose elements are copied to the new <see cref="Heap{TKey, TValue}" />.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public Heap(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _size = 0;
            _version = 0;
            _items = new KeyValuePair<TKey, TValue>[DefaultCapacity];
            _comparer = Comparer<TKey>.Default;

            foreach (KeyValuePair<TKey, TValue> element in source)
            {
                Insert(element.Key, element.Value);
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="IComparer{T}" />.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{KeyValuePair{TKey, TValue}}" /> whose elements are copied to the new <see cref="Heap{TKey, TValue}" />.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public Heap(IEnumerable<KeyValuePair<TKey, TValue>> source, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _items = _emptyArray;
            _size = 0;
            _version = 0;

            _comparer = comparer ?? Comparer<TKey>.Default;

            foreach (KeyValuePair<TKey, TValue> element in source)
            {
                Insert(element.Key, element.Value);
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="IComparer{T}" />.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="Heap{TKey, TValue}" /> can contain.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public Heap(Int32 capacity, IComparer<TKey> comparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "Capacity is less than 0.");

            _items = new KeyValuePair<TKey, TValue>[capacity];
            _size = 0;
            _version = 0;

            _comparer = comparer ?? Comparer<TKey>.Default;
        }

        #endregion

        #region IHeap methods

        /// <summary>
        /// Inserts the specified key and value to the heap.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key", "Key is null.");

            if (_size == _items.Length) EnsureCapacity(_size + 1);

            _items[_size] = new KeyValuePair<TKey,TValue>(key, value);
            _size++;
            _version++;

            Int32 p = _size - 1;
            while (p > 0 && _comparer.Compare(_items[(p - 1) / 2].Key, _items[p].Key) > 0)
            {
                KeyValuePair<TKey, TValue> h = _items[p];
                _items[p] = _items[(p - 1) / 2];
                _items[(p - 1) / 2] = h;

                p = (p - 1) / 2;
            }
        }

        /// <summary>
        /// Removes and returns the value at the top of the heap.
        /// </summary>
        /// <returns>The value that is removed from the top of the heap.</returns>
        /// <exception cref="System.InvalidOperationException">The heap is empty.</exception>
        public TValue RemovePeek()
        {
            if (_size == 0)
                throw new InvalidOperationException("The heap is empty.");

            KeyValuePair<TKey, TValue> result = _items[0];
            Int32 p = 0, p1, p2, pn;

            _items[0] = _items[_size - 1];
            _size--;
            _version++;

            while (p < _size)
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (_size > p1 && _comparer.Compare(_items[p].Key, _items[p1].Key) > 0)
                {
                    p = p1;
                }
                if (_size > p2 && _comparer.Compare(_items[p].Key, _items[p2].Key) > 0)
                {
                    p = p2;
                }
                if (p == pn)
                    break;

                KeyValuePair<TKey, TValue> h = _items[p];
                _items[p] = _items[pn];
                _items[pn] = h;
            }

            return result.Value;
        }

        /// <summary>
        /// Determines whether the heap contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the heap.</param>
        /// <returns><c>true</c> if the heap contains an element with the specified key; otherwise, <c>false</c>.</returns>
        public Boolean Contains(TKey key)
        {
            for (Int32 i = 0; i < _size; i++)
            {
                if (_comparer.Compare(_items[i].Key, key) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all keys and values from the heap.
        /// </summary>
        public void Clear()
        {
            _size = 0;
            _version++;
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{KeyValuePair{TKey, TValue}}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Ensures the capacity of the linestring is at least the given minimum value.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        protected void EnsureCapacity(Int32 min)
        {
            if (_items.Length < min)
            {
                Int32 newCapacity = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;

                if (newCapacity < min)
                    newCapacity = min;

                if (newCapacity > 0)
                {
                    KeyValuePair<TKey, TValue>[] newItems = new KeyValuePair<TKey, TValue>[newCapacity];
                    if (_size > 0)
                    {
                        Array.Copy(_items, 0, newItems, 0, _size);
                    }
                    _items = newItems;
                }
                else
                {
                    _items = _emptyArray;
                }
            }
        }

        #endregion
    }
}