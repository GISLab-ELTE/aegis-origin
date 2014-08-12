/// <copyright file="SparseArrayTest.cs" company="Eötvös Loránd University (ELTE)">
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
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Collections
{
    /// <summary>
    /// Test fixture for the <see cref="SparseArray"/> class.
    /// </summary>
    [TestFixture]
    public class SparseArrayTest
    {
        #region Private fields

        /// <summary>
        /// The array of values that are inserted into the sparse array.
        /// </summary>
        private Int32[] _values;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _values = new Int32[] { 0, 1, 0, 2, 3, 0, 0 };
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void SparseArrayConstructorTest()
        {
            // no parameters

            SparseArray<Int32> array = new SparseArray<Int32>(0);

            Assert.IsFalse(array.IsReadOnly);
            Assert.AreEqual(0, array.Length);
            Assert.AreEqual(0, array.Count);


            // capacity parameter

            array = new SparseArray<Int32>(1000);

            Assert.AreEqual(1000, array.Length);
            Assert.AreEqual(0, array.Count);


            // source parameter without zeros

            array = new SparseArray<Int32>(Enumerable.Range(1, 1000));

            Assert.AreEqual(1000, array.Length);
            Assert.AreEqual(1000, array.Count);

            for (Int32 i = 0; i < array.Length; i++)
                Assert.AreEqual(i + 1, array[i]);


            // source parameter with zeros

            array = new SparseArray<Int32>(_values);

            Assert.IsTrue(_values.Length > array.Count);
            Assert.AreEqual(_values.Length, array.Length);

            for (Int32 i = 0; i < array.Length; i++)
                Assert.AreEqual(_values[i], array[i]);


            // source parameter with only zeros

            array = new SparseArray<Int32>(Enumerable.Repeat(0, 10));

            Assert.AreEqual(10, array.Length);
            Assert.AreEqual(0, array.Count);

            for (Int32 i = 0; i < array.Count; i++)
                Assert.AreEqual(0, array[i]);


            // exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => array = new SparseArray<Int32>(-1));
            Assert.Throws<ArgumentNullException>(() => array = new SparseArray<Int32>(null));
        }

        /// <summary>
        /// Test case for the indexer property.
        /// </summary>
        [Test]
        public void SparseArrayItemTest()
        {
            // get

            SparseArray<Int32> array = new SparseArray<Int32>(_values);

            for (Int32 i = 0; i < array.Count; i++)
                Assert.AreEqual(_values[i], array[i]);


            // set

            array = new SparseArray<Int32>(Enumerable.Repeat(0, _values.Length));

            for (Int32 i = 0; i < _values.Length; i++)
                array[i] = _values[i];

            for (Int32 i = 0; i < array.Count; i++)
                Assert.AreEqual(_values[i], array[i]);

            for (Int32 i = 0; i < _values.Length; i++)
                array[i] = _values[i];

            for (Int32 i = 0; i < array.Count; i++)
                Assert.AreEqual(_values[i], array[i]);


            // exceptions

            Assert.Throws<IndexOutOfRangeException>(() => array[-1] = _values[0]);
            Assert.Throws<IndexOutOfRangeException>(() => _values[0] = array[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => array[_values.Length] = _values[0]);
            Assert.Throws<IndexOutOfRangeException>(() => _values[0] = array[_values.Length]);
        }

        /// <summary>
        /// Test case for the <see cref="Add"/> method.
        /// </summary>
        [Test]
        public void SparseArrayAddTest()
        {
            // empty array

            SparseArray<Int32> array = new SparseArray<Int32>(0);

            for (Int32 i = 0; i < _values.Length; i++)
                array.Add(_values[i]);

            Assert.AreEqual(_values.Length, array.Length);

            for (Int32 i = 0; i < array.Count; i++)
                Assert.AreEqual(_values[i], array[i]);


            // filled array

            array = new SparseArray<Int32>(Enumerable.Repeat(0, 10));

            for (Int32 i = 0; i < _values.Length; i++)
                array.Add(_values[i]);

            Assert.AreEqual(_values.Length + 10, array.Length);

            for (Int32 i = 10; i < array.Count; i++)
                Assert.AreEqual(_values[i - 10], array[i]);
        }

        /// <summary>
        /// Test case for the <see cref="Clear"/> method.
        /// </summary>
        [Test]
        public void SparseArrayClearTest()
        {
            // empty array

            SparseArray<Int32> array = new SparseArray<Int32>(0);
            array.Clear();

            Assert.AreEqual(0, array.Length);


            // filled array

            array = new SparseArray<Int32>(_values);
            array.Clear();

            Assert.AreEqual(0, array.Count);
            Assert.AreEqual(_values.Length, array.Length);
        }

        /// <summary>
        /// Test case for the <see cref="Contains"/> method.
        /// </summary>
        [Test]
        public void SparseArrayContainsTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(_values);

            Assert.IsTrue(array.Contains(0));
            Assert.IsFalse(array.Contains(-1));
            Assert.IsTrue(array.Contains(1));
            Assert.IsTrue(array.Contains(2));
            Assert.IsFalse(array.Contains(4));
        }

        /// <summary>
        /// Test case for the <see cref="CopyTo"/> method.
        /// </summary>
        [Test]
        public void SparseArrayCopyToTest()
        {
            Int32[] result = new Int32[100];

            // zero start index

            SparseArray<Int32> array = new SparseArray<Int32>(_values);
            array.CopyTo(result, 0);

            for (Int32 i = 0; i < array.Count; i++)
                Assert.AreEqual(result[i], array[i]);


            // greater start index

            array.CopyTo(result, 50);

            for (Int32 i = 50; i < array.Count; i++)
                Assert.AreEqual(result[50 + i], array[i]);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => array.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => array.CopyTo(result, -1));
            Assert.Throws<ArgumentException>(() => array.CopyTo(result, 100));
        }

        /// <summary>
        /// Test case for the <see cref="Remove"/> method.
        /// </summary>
        [Test]
        public void SparseArrayRemoveTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(_values);

            Assert.IsFalse(array.Remove(-1));
            Assert.AreEqual(_values.Length, array.Length);

            Assert.IsTrue(array.Remove(0));
            Assert.AreEqual(_values.Length - 1, array.Length);

            Assert.IsTrue(array.Remove(0));
            Assert.AreEqual(_values.Length - 2, array.Length);

            Assert.IsTrue(array.Remove(1));
            Assert.AreEqual(_values.Length - 3, array.Length);

            for (Int32 i = 0; i < array.Count; i++)
                Assert.AreEqual(_values[3 + i], array[i]);
        }

        /// <summary>
        /// Test case for the <see cref="IndexOf"/> method.
        /// </summary>
        [Test]
        public void SparseArrayIndexOfTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(_values);

            Assert.AreEqual(0, array.IndexOf(0));
            Assert.AreEqual(-1, array.IndexOf(-1));
            Assert.AreEqual(1, array.IndexOf(1));
            Assert.AreEqual(3, array.IndexOf(2));
        }

        /// <summary>
        /// Test case for the <see cref="Insert"/> method.
        /// </summary>
        [Test]
        public void SparseArrayInsertTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(_values);
            array.Insert(1, -1);
            array.Insert(3, 0);
            array.Insert(8, -2);

            Assert.AreEqual(_values.Length + 3, array.Length);

            Assert.AreEqual(-1, array[1]);
            Assert.AreEqual(1, array[2]);
            Assert.AreEqual(0, array[3]);
            Assert.AreEqual(0, array[4]);
            Assert.AreEqual(2, array[5]);


            // exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => array.Insert(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => array.Insert(array.Length, 1));
        }

        /// <summary>
        /// Test case for the <see cref="RemoveAt"/> method.
        /// </summary>
        [Test]
        public void SparseArrayRemoveAtTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(_values);
            array.RemoveAt(1);
            array.RemoveAt(1);
            array.RemoveAt(3);

            Assert.AreEqual(_values.Length - 3, array.Length);

            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(0, array[3]);


            // exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => array.RemoveAt(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => array.RemoveAt(array.Length));
        }

        /// <summary>
        /// Test case for the enumerator.
        /// </summary>
        [Test]
        public void SparseArrayEnumeratorTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(_values);

            IEnumerator enumerator = _values.GetEnumerator();
            IEnumerator<Int32> arrayEnumerator = array.GetEnumerator();
            IEnumerator arrayCollectionEnumerator = (array as IEnumerable).GetEnumerator();

            while (enumerator.MoveNext())
            {
                Assert.IsTrue(arrayEnumerator.MoveNext());
                Assert.IsTrue(arrayCollectionEnumerator.MoveNext());

                Assert.AreEqual(enumerator.Current, arrayEnumerator.Current);
                Assert.AreEqual(arrayCollectionEnumerator.Current, arrayEnumerator.Current);
            }


            arrayEnumerator.Reset();
            arrayCollectionEnumerator.Reset();

            while (arrayEnumerator.MoveNext())
            {
                Assert.IsTrue(arrayCollectionEnumerator.MoveNext());
                Assert.AreEqual(arrayCollectionEnumerator.Current, arrayEnumerator.Current);
            }


            // exceptions

            array.Add(0);

            Assert.Throws<InvalidOperationException>(() => arrayEnumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => arrayEnumerator.Reset());

            arrayEnumerator.Dispose();

            Assert.Throws<ObjectDisposedException>(() => arrayEnumerator.MoveNext());

            arrayEnumerator.Dispose();
        }

        #endregion
    }
}
