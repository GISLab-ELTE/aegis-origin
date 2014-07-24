/// <copyright file="HeapTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Collections
{
    /// <summary>
    /// Test fixture for the <see cref="Heap"/> class.
    /// </summary>
    [TestFixture]
    public class HeapTest
    {
        #region Private fields

        /// <summary>
        /// The array of values that are inserted into the heap.
        /// </summary>
        private KeyValuePair<Int32, String>[] _values;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        { 
            Randomizer randomizer = new Randomizer();

            _values = randomizer.GetInts(-100, 100, 20).Select(value => new KeyValuePair<Int32, String>(value, value.ToString())).ToArray();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void HeapConstructorTest()
        {
            // no parameters

            Heap<Int32, String> heap = new Heap<Int32, String>();

            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(0, heap.Capacity);
            Assert.AreEqual(Comparer<Int32>.Default, heap.Comparer);


            // comparer parameter


            heap = new Heap<Int32, String>(Comparer<Int32>.Default);

            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(0, heap.Capacity);
            Assert.AreEqual(Comparer<Int32>.Default, heap.Comparer);

            heap = new Heap<Int32, String>((IComparer<Int32>)null);

            Assert.AreEqual(Comparer<Int32>.Default, heap.Comparer);


            // capacity parameter

            heap = new Heap<Int32, String>(100);

            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(100, heap.Capacity);


            // capacity and comparer parameter

            heap = new Heap<Int32, String>(100, Comparer<Int32>.Default);

            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(100, heap.Capacity);
            Assert.AreEqual(Comparer<Int32>.Default, heap.Comparer);


            heap = new Heap<Int32, String>(100, (IComparer<Int32>)null);

            Assert.AreEqual(Comparer<Int32>.Default, heap.Comparer);


            // source parameter

            heap = new Heap<Int32, String>(_values);

            Assert.AreEqual(_values.Length, heap.Count);
            Assert.AreEqual(32, heap.Capacity);
            Assert.AreEqual(Comparer<Int32>.Default, heap.Comparer);


            // source and comparer parameter

            heap = new Heap<Int32, String>(_values, Comparer<Int32>.Default);

            Assert.AreEqual(_values.Length, heap.Count);
            Assert.AreEqual(32, heap.Capacity);
            Assert.AreEqual(Comparer<Int32>.Default, heap.Comparer);

            heap = new Heap<Int32, String>(_values, (IComparer<Int32>)null);

            Assert.AreEqual(Comparer<Int32>.Default, heap.Comparer);


            // exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => heap = new Heap<Int32, String>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => heap = new Heap<Int32, String>(-1, null));
            Assert.Throws<ArgumentNullException>(() => heap = new Heap<Int32, String>((IEnumerable<KeyValuePair<Int32, String>>)null));
            Assert.Throws<ArgumentNullException>(() => heap = new Heap<Int32, String>((IEnumerable<KeyValuePair<Int32, String>>)null, Comparer<Int32>.Default));
        }

        /// <summary>
        /// Test case for the <see cref="Capacity" /> property.
        /// </summary>
        [Test]
        public void HeapCapacityTest()
        {
            // empty heap

            Heap<Int32, String> heap = new Heap<Int32, String>();

            heap.Capacity = 100;

            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(100, heap.Capacity);

            heap.Capacity = 10;

            Assert.AreEqual(10, heap.Capacity);


            heap.Capacity = 0;

            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(0, heap.Capacity);


            // filled heap

            heap.Capacity = _values.Length;

            foreach(KeyValuePair<Int32, String> keyValuePair in _values)
                heap.Insert(keyValuePair.Key, keyValuePair.Value);

            Assert.AreEqual(_values.Length, heap.Count);
            Assert.AreEqual(_values.Length, heap.Capacity);

            heap.Capacity = _values.Length * 10;

            Assert.AreEqual(_values.Length, heap.Count);
            Assert.AreEqual(_values.Length * 10, heap.Capacity);

            heap.Capacity = _values.Length;

            Assert.AreEqual(_values.Length, heap.Count);
            Assert.AreEqual(_values.Length, heap.Capacity);


            // exceptions

            Assert.Throws<InvalidOperationException>(() => heap.Capacity = 0);
        }

        /// <summary>
        /// Test case for the <see cref="Peek" /> property.
        /// </summary>
        [Test]
        public void HeapPeekTest()
        {
            // small heap

            Heap<Int32, String> heap = new Heap<Int32, String>();

            heap.Insert(0, "0");
            Assert.AreEqual("0", heap.Peek);

            heap.Insert(1, "1");
            Assert.AreEqual("0", heap.Peek);

            heap.Insert(-1, "-1");
            Assert.AreEqual("-1", heap.Peek);


            // large heap

            heap = new Heap<Int32, String>(_values.Length);

            for (Int32 i = 0; i < _values.Length; i++)
            {
                heap.Insert(_values[i].Key, _values[i].Value);

                Assert.AreEqual(heap.Min(item => item.Key).ToString(), heap.Peek);
                Assert.AreEqual(_values.Take(i + 1).Min(item => item.Key).ToString(), heap.Peek);
            }


            // exceptions

            heap = new Heap<Int32, String>();
            String peek;
            Assert.Throws<InvalidOperationException>(() => peek = heap.Peek);
        }

        /// <summary>
        /// Test case for the <see cref="Insert" /> method.
        /// </summary>
        [Test]
        public void HeapInsertTest()
        {
            Heap<Int32, String> heap = new Heap<Int32, String>();

            heap.Insert(_values[0].Key, _values[0].Value);

            Assert.AreEqual(_values[0].Value, heap.Peek);
            Assert.AreEqual(4, heap.Capacity);


            heap.Insert(_values[1].Key, _values[1].Value);
            heap.Insert(_values[2].Key, _values[2].Value);
            heap.Insert(_values[3].Key, _values[3].Value);

            Assert.AreEqual(4, heap.Capacity);

            heap.Insert(_values[4].Key, _values[4].Value);
            Assert.AreEqual(8, heap.Capacity);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => new Heap<String, String>().Insert(null, null));
        }

        /// <summary>
        /// Test case for the <see cref="RemovePeek" /> method.
        /// </summary>
        [Test]
        public void HeapRemovePeekTest()
        {
            Heap<Int32, String> heap = new Heap<Int32, String>();

            for (Int32 i = 0; i < _values.Length; i++)
            {
                heap.Insert(_values[i].Key, _values[i].Value);

                Assert.AreEqual(_values.Take(i + 1).Select(item => item.Key).Min().ToString(), heap.Peek);
            }

            while (heap.Count > 0)
            {                
                String peek;
                Assert.AreEqual(heap.Peek, peek = heap.RemovePeek());
                Assert.IsTrue(_values.Select(item => item.Value).Contains(peek));
            }


            // exceptions

            Assert.Throws<InvalidOperationException>(() => new Heap<String, String>().RemovePeek());
        }


        /// <summary>
        /// Test case for the <see cref="Contains" /> method.
        /// </summary>
        [Test]
        public void HeapContainsTest()
        {
            Heap<Int32, String> heap = new Heap<Int32, String>(_values);

            for (Int32 i = -100; i <= 100; i++)
            {
                Assert.AreEqual(_values.Contains(new KeyValuePair<Int32, String>(i, i.ToString())), heap.Contains(i));
            }
        }

        /// <summary>
        /// Test case for the <see cref="Clear" /> method.
        /// </summary>
        [Test]
        public void HeapClearTest()
        {
            // empty heap

            Heap<Int32, String> heap = new Heap<Int32, String>();

            heap.Clear();

            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(0, heap.Capacity);


            // filled heap

            heap = new Heap<Int32, String>(_values);

            Int32 capacity = heap.Capacity;

            heap.Clear();

            Assert.AreEqual(0, heap.Count);
            Assert.AreEqual(capacity, heap.Capacity);
        }

        /// <summary>
        /// Test case for the enumerator.
        /// </summary>
        [Test]
        public void HeapEnumeratorTest()
        {
            Heap<Int32, String> heap = new Heap<Int32, String>(_values.OrderBy(value => value.Key));

            IEnumerator enumerator = _values.OrderBy(value => value.Key).GetEnumerator();
            IEnumerator<KeyValuePair<Int32, String>> heapEnumerator = heap.GetEnumerator();
            IEnumerator heapCollectionEnumerator = (heap as IEnumerable).GetEnumerator();

            while (enumerator.MoveNext())
            {
                Assert.IsTrue(heapEnumerator.MoveNext());
                Assert.IsTrue(heapCollectionEnumerator.MoveNext());

                Assert.AreEqual(enumerator.Current, heapEnumerator.Current);
                Assert.AreEqual(heapCollectionEnumerator.Current, heapEnumerator.Current);
            }


            heapEnumerator.Reset();
            heapCollectionEnumerator.Reset();

            while (heapEnumerator.MoveNext())
            {
                Assert.IsTrue(heapCollectionEnumerator.MoveNext());
                Assert.AreEqual(heapCollectionEnumerator.Current, heapEnumerator.Current);
            }


            // exceptions

            heap.Insert(0, "0");

            Assert.Throws<InvalidOperationException>(() => heapEnumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => heapEnumerator.Reset());

            heapEnumerator.Dispose();

            Assert.Throws<ObjectDisposedException>(() => heapEnumerator.MoveNext());

            heapEnumerator.Dispose();
        }

        #endregion
    }
}
