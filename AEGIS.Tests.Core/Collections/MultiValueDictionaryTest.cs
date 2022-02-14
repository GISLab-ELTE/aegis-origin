/// <copyright file="MultiValueDictionaryTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Daniel Ballagi</author>

using ELTE.AEGIS.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Collections
{
    /// <summary>
    /// Test fixture for the <see cref="MultiValueDictionary" /> class.
    /// </summary>
    [TestFixture]
    class MultiValueDictionaryTest
    {
        #region Private fields

        /// <summary>
        /// The array of values that are inserted into the dictionary.
        /// </summary>
        private KeyValuePair<Int32, List<String>>[] _items;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _items = Enumerable.Range(1, 10).Select(value => new KeyValuePair<Int32, List<String>>(value, new List<String>() 
            { 
                value.ToString(),
                value.ToString() + value.ToString()
            })).ToArray();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the constructors.
        /// </summary>
        [Test]
        public void MultiValueDictionaryConstructorTest()
        {
            // No parameters

            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();
            Assert.AreEqual(0, dictionary.Count());
            Assert.AreEqual(false, dictionary.IsReadOnly);

            // Comparer parameter

            dictionary = new MultiValueDictionary<Int32, String>(EqualityComparer<Int32>.Default);
            Assert.AreEqual(0, dictionary.Count());
            Assert.AreEqual(false, dictionary.IsReadOnly);

            // Copy constructor

            dictionary.Add(1, "one");
            MultiValueDictionary<Int32, String> secondDictionary = new MultiValueDictionary<Int32, String>(dictionary);
            Assert.AreEqual(1, secondDictionary.Count);
            Assert.AreEqual(new List<String> { "one" }, secondDictionary[1]);
            Assert.AreEqual(false, dictionary.IsReadOnly);

            // Copy constructor with comparer parameter

            secondDictionary = new MultiValueDictionary<Int32, String>(dictionary, EqualityComparer<Int32>.Default);
            Assert.AreEqual(1, secondDictionary.Count);
            Assert.AreEqual(new List<String> { "one" }, secondDictionary[1]);
            Assert.AreEqual(false, dictionary.IsReadOnly);
        }

        /// <summary>
        /// Tests the <see cref="Add" /> method.
        /// </summary>
        [Test]
        public void MultiValueDictionaryAddTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();
            dictionary.Add(1, "1");
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(new List<String> { "1" }, dictionary[1]);

            dictionary.Add(1, "11");
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(new List<String> { "1", "11" }, dictionary[1]);

            dictionary.Add(2, new List<String> { "2", "22" });
            Assert.AreEqual(2, dictionary.Count);
            Assert.AreEqual(new List<String> { "2", "22" }, dictionary[2]);

            dictionary.Add(2, new List<String> { "222", "2222" });
            Assert.AreEqual(2, dictionary.Count);
            Assert.AreEqual(new List<String> { "2", "22", "222", "2222" }, dictionary[2]);

            (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).Add(new KeyValuePair<Int32, ICollection<String>>(3, new List<String> { "3" } as ICollection<String>));
            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual(new List<String> { "3" }, dictionary[3]);

            dictionary[4] = new List<String> { "4" };
            Assert.AreEqual(4, dictionary.Count);
            Assert.AreEqual(new List<String> { "4" }, dictionary[4]);
        }

        /// <summary>
        /// Tests the <see cref="Remove" /> method.
        /// </summary>
        [Test]
        public void MultiValueDictionaryRemoveTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> item in _items)
                dictionary.Add(item.Key, item.Value);

            Assert.AreEqual(false, dictionary.Remove(0, "0"));
            Assert.AreEqual(true, dictionary.Remove(1, "1"));
            Assert.AreEqual(false, dictionary.Remove(1, "1"));
            Assert.AreEqual(true, dictionary.Remove(1, "11"));
            Assert.AreEqual(false, dictionary.Remove(1, "11"));

            dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> item in _items)
                dictionary.Add(item.Key, item.Value);

            Assert.AreEqual(false, dictionary.Remove(0));
            Assert.AreEqual(true, dictionary.Remove(1));
            Assert.AreEqual(false, dictionary.Remove(1));

            (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).Add(new KeyValuePair<Int32, ICollection<String>>(3, new List<String> { "3" } as ICollection<String>));
            Assert.AreEqual(true, (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).Remove(new KeyValuePair<Int32, ICollection<String>>(3, new List<String> { "3" } as ICollection<String>)));
            Assert.AreEqual(false, (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).Remove(new KeyValuePair<Int32, ICollection<String>>(3, new List<String> { "3" } as ICollection<String>)));
        }

        /// <summary>
        /// Tests the <see cref="Clear" /> method.
        /// </summary>
        [Test]
        public void MultiValueDictionaryClearTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> item in _items)
                dictionary.Add(item.Key, item.Value);

            Assert.AreEqual(10, dictionary.Count());

            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count());
        }

        /// <summary>
        /// Tests the <see cref="Contains" /> method.
        /// </summary>
        [Test]
        public void MultiValueDictionaryContainsTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> item in _items)
                dictionary.Add(item.Key, item.Value);

            Assert.AreEqual(true, dictionary.ContainsKey(1));
            Assert.AreEqual(false, dictionary.ContainsKey(0));

            Assert.AreEqual(true, dictionary.Contains(new KeyValuePair<Int32, ICollection<String>>(1, new List<String> { "1", "11" } as ICollection<String>)));
            Assert.AreEqual(false, dictionary.Contains(new KeyValuePair<Int32, ICollection<String>>(1, new List<String> { "0", "11" } as ICollection<String>)));
        }

        /// <summary>
        /// Tests the <see cref="CopyTo" /> method.
        /// </summary>
        [Test]
        public void MultiValueDictionaryCopyToTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> item in _items)
                dictionary.Add(item.Key, item.Value);

            KeyValuePair<Int32, ICollection<String>>[] actual = new KeyValuePair<Int32,ICollection<String>>[10];
            (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).CopyTo(actual, 0);

            IEnumerable<KeyValuePair<Int32, ICollection<String>>> expected = _items.Select(value => new KeyValuePair<Int32, ICollection<String>>(value.Key, value.Value as ICollection<String>));

            Assert.AreEqual(expected.Select(v => v.Key), actual.Select(v => v.Key));
            Assert.AreEqual(expected.Select(v => v.Value), actual.Select(v => v.Value));
        }

        /// <summary>
        /// Tests the <see cref="Keys" /> property.
        /// </summary>
        [Test]
        public void MultiValueDictionaryKeysTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> item in _items)
                dictionary.Add(item.Key, item.Value);

            Assert.IsTrue(dictionary.Keys.SequenceEqual(Enumerable.Range(1, 10)));
        }

        /// <summary>
        /// Tests the <see cref="Values" /> property.
        /// </summary>
        [Test]
        public void MultiValueDictionaryValuesTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> item in _items)
                dictionary.Add(item.Key, item.Value);

            Assert.AreEqual(_items.Length, dictionary.Values.Count);

            foreach (KeyValuePair<Int32, List<String>> item in _items)
                Assert.IsTrue(dictionary.Values.Any(value => value.SequenceEqual(item.Value)));
        }

        #endregion
    }
}
