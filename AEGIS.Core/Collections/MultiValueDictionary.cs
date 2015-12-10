/// <copyright file="MultiValueDictionary.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
/// <author>Daniel Ballagi</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Collections
{
    /// <summary>
    /// Represents a multi-value dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class MultiValueDictionary<TKey, TValue> : IDictionary<TKey, ICollection<TValue>>
    {
        #region Private fields

        /// <summary>
        /// Underlying dictionary.
        /// </summary>
        private Dictionary<TKey, List<TValue>> _dictionary;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public MultiValueDictionary()
        {
            _dictionary = new Dictionary<TKey, List<TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="IComparer{TKey}" />.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        public MultiValueDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, List<TValue>>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains elements copied from the specified <see cref="MultiValueDictionary{TKey, TValue}" /> and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="other">The <see cref="MultiValueDictionary{TKey, TValue}" /> whose elements are copied to the new <see cref="MultiValueDictionary{TKey, TValue}" />.</param>
        public MultiValueDictionary(MultiValueDictionary<TKey, TValue> other)
        {
            _dictionary = new Dictionary<TKey,List<TValue>>(other._dictionary);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains elements copied from the specified <see cref="MultiValueDictionary{TKey, TValue}" /> and uses the specified <see cref="IComparer{TKey}" />.
        /// </summary>
        /// <param name="other">The <see cref="MultiValueDictionary{TKey, TValue}" /> whose elements are copied to the new <see cref="MultiValueDictionary{TKey, TValue}" />.</param>
        /// /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        public MultiValueDictionary(MultiValueDictionary<TKey, TValue> other, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, List<TValue>>(other._dictionary, comparer);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public void Add(TKey key, TValue value)
        {
            if (!_dictionary.ContainsKey(key))
                _dictionary[key] = new List<TValue>();

            _dictionary[key].Add(value);
        }

        /// <summary>
        /// Removes the value with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>. This method returns <c>false</c> if key is not found.</returns>
        public Boolean Remove(TKey key, TValue item)
        {
            if (!ContainsKey(key))
                return false;

            if (!_dictionary[key].Contains(item))
                return false;

            _dictionary[key].Remove(item);

            if (_dictionary[key].Count() == 0)
                _dictionary.Remove(key);

            return true;
        }

        #endregion

        #region ICollection properties

        /// <summary>
        /// Gets the number of elements contained in the dictionary.
        /// </summary>
        public Int32 Count
        {
            get { return _dictionary.Count(); }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IDictionary properties

        /// <summary>
        /// Gets the keys currently present in the dictionary.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

        /// <summary>
        /// Gets the values currently present in the dictionary.
        /// </summary>
        public ICollection<ICollection<TValue>> Values
        {
            get { return _dictionary.Values.ToList<ICollection<TValue>>(); }
        }

        /// <summary>
        /// Gets or sets the element at the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value located at <paramref name="key" />.</returns>
        public ICollection<TValue> this[TKey key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value.ToList<TValue>(); }
        }

        #endregion

        #region ICollection methods

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Adds a KeyValuePair to the dictionary.
        /// </summary>
        /// <param name="item">The object to add to the dictionary.</param>
        void ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Add(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Determines whether the dictionary contains a collection of values.
        /// </summary>
        /// <returns><c>true</c> if all values of <paramref name="item"/> are found in the dictionary; otherwise, <c>false</c>.</returns>
        /// <param name="item">The collection of values to locate in the dictionary.</param>
        Boolean ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Contains(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            if (!ContainsKey(item.Key))
                return false;

            foreach (TValue it in item.Value)
            {
                if (!_dictionary[item.Key].Contains(it))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Copies the elements of the dictionary to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from dictionary. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        void ICollection<KeyValuePair<TKey, ICollection<TValue>>>.CopyTo(KeyValuePair<TKey, ICollection<TValue>>[] array, int index)
        {
            if (index < 0)
                return;

            KeyValuePair<TKey, List<TValue>>[] tempArray = new KeyValuePair<TKey,List<TValue>>[_dictionary.Count()];
            (_dictionary as IDictionary<TKey, List<TValue>>).CopyTo(tempArray, 0);

            for (int i = index; i < tempArray.Length; ++i)
            {
                array[i - index] = new KeyValuePair<TKey, ICollection<TValue>>(tempArray[i].Key, tempArray[i].Value as ICollection<TValue>);
            }
        }

        /// <summary>
        /// Removes a specific KeyValuePair from the dictionary.
        /// </summary>
        /// <returns><c>true</c> if all values of <paramref name="item"/> were successfully removed from the dictionary; otherwise, <c>false</c>.</returns>
        Boolean ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Remove(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            if (!ContainsKey(item.Key))
                return false;

            foreach (TValue it in item.Value)
            {
                if (!this[item.Key].Contains(it))
                    return false;
            }

            Remove(item.Key);

            return true;
        }

        #endregion

        #region IDictionary methods

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public void Add(TKey key, ICollection<TValue> value)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key].AddRange(value);
            }
            else
            {
                _dictionary[key] = value.ToList<TValue>();
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns><c>true</c> if <paramref name="key"/> is found in the dictionary; otherwise, <c>false</c>.</returns>
        public Boolean ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the value with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>. This method returns <c>false</c> if key is not found.</returns>
        public Boolean Remove(TKey key) 
        {
            return _dictionary.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the dictionary contains an element with the specified key; otherwise, <c>false</c>.</returns>
        public Boolean TryGetValue(TKey key, out ICollection<TValue> value)
        {
            List<TValue> outValue;
            Boolean retValue = _dictionary.TryGetValue(key, out outValue);

            value = outValue;

            return retValue;
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{KeyValuePair{TKey, TValue}}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, ICollection<TValue>> item in _dictionary as IDictionary<TKey, ICollection<TValue>>)
            {
                yield return item;
            }
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
    }
}
