// <copyright file="DisjointSetForestTest.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Collections;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Collections
{
    /// <summary>
    /// Test fixture for the <see cref="DisjointSetForest" /> class.
    /// </summary>
    /// <author>Dávid Kis</author>
    [TestFixture]
    public class DisjointSetForestTest
    {
        #region Private fields

        /// <summary>
        /// The array of values that are inserted into the DisjointSetForest.
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
            Random random = new Random(0);

            _values = new int[20];
            for (int i = 0; i < 20; i++)
            {
                Int32 x;
                do
                {
                    x = random.Next(-100, 100);
                } while (_values.Contains(x));
                _values[i] = x;
            }
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void DisjointSetForestConstructorTest()
        {
            // no parameters
            DisjointSetForest<Int32> disjointSet = new DisjointSetForest<Int32>();

            Assert.AreEqual(0, disjointSet.Count);
            Assert.AreEqual(0, disjointSet.SetCount);

            // capacity parameter

            disjointSet = new DisjointSetForest<Int32>(100);

            Assert.AreEqual(0, disjointSet.Count);
            Assert.AreEqual(0, disjointSet.SetCount);

            // source parameter

            disjointSet = new DisjointSetForest<Int32>(_values);

            Assert.AreEqual(_values.Length, disjointSet.Count);
            Assert.AreEqual(_values.Length, disjointSet.SetCount);

            // exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => disjointSet = new DisjointSetForest<Int32>(-1));
            Assert.Throws<ArgumentNullException>(() => disjointSet = new DisjointSetForest<Int32>((IEnumerable<Int32>)null));
        }

        /// <summary>
        /// Tests the <see cref="MakeSet"/> method.
        /// </summary>
        [Test]
        public void DisjointSetForestMakeSetTest()
        {
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>();
            DisjointSetForest<String> disjointSetString = new DisjointSetForest<String>();

            foreach (Int32 value in _values)
            {
                disjointSetInt.MakeSet(value);
                disjointSetString.MakeSet(value.ToString());
            }

            // testing if added correctly

            Assert.AreEqual(_values.Length, disjointSetInt.Count);
            Assert.AreEqual(_values.Length, disjointSetInt.SetCount);

            Assert.AreEqual(_values.Length, disjointSetString.Count);
            Assert.AreEqual(_values.Length, disjointSetString.SetCount);

            foreach (Int32 value in _values)
            {
                Assert.AreEqual(disjointSetInt.Find(value), value);
                Assert.AreEqual(disjointSetString.Find(value.ToString()), value.ToString());
            }

            // testing element uniqueness
            for (Int32 i = 0; i < 20; i++)
            {
                disjointSetInt.MakeSet(_values[0]);
                disjointSetString.MakeSet(_values[0].ToString());
            }

            Assert.AreEqual(_values.Length, disjointSetInt.Count);
            Assert.AreEqual(_values.Length, disjointSetInt.SetCount);

            Assert.AreEqual(_values.Length, disjointSetString.Count);
            Assert.AreEqual(_values.Length, disjointSetString.SetCount);

            // exceptions

            Assert.Throws<ArgumentNullException>(() => new DisjointSetForest<String>().MakeSet(null));
        }

        /// <summary>
        /// Tests the <see cref="Find"/> method.
        /// </summary>
        [Test]
        public void DisjointSetForestFindTest()
        {
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>();
            DisjointSetForest<String> disjointSetString = new DisjointSetForest<String>();

            foreach (Int32 value in _values)
            {
                disjointSetInt.MakeSet(value);
                disjointSetString.MakeSet(value.ToString());
            }

            foreach (Int32 value in _values)
            {
                Assert.AreEqual(disjointSetInt.Find(value), value);
                Assert.AreEqual(disjointSetString.Find(value.ToString()), value.ToString());
            }

            // exceptions

            Assert.Throws<ArgumentNullException>(() => disjointSetString.Find(null));
            Assert.Throws<ArgumentException>(() => disjointSetInt.Find(100));
            Assert.Throws<ArgumentException>(() => disjointSetString.Find(100.ToString()));
        }

        /// <summary>
        /// Tests the <see cref="Union"/> method.
        /// </summary>
        [Test]
        public void DisjointSetForestUnionTest()
        {
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>();
            DisjointSetForest<String> disjointSetString = new DisjointSetForest<String>();

            foreach (Int32 value in _values)
            {
                disjointSetInt.MakeSet(value);
                disjointSetString.MakeSet(value.ToString());
            }

            for (Int32 i = 0; i < _values.Length; i = i + 2)
            {
                disjointSetInt.Union(_values[i], _values[i + 1]);
                disjointSetString.Union(_values[i].ToString(), _values[i + 1].ToString());
            }

            // testing representatives

            for (Int32 i = 0; i < _values.Length; i = i + 2)
            {
                Assert.AreEqual(disjointSetInt.Find(_values[i]), disjointSetInt.Find(_values[i + 1]));
                Assert.AreEqual(disjointSetString.Find(_values[i].ToString()), disjointSetString.Find(_values[i + 1].ToString()));
            }

            // testing setCounts

            Assert.AreEqual(disjointSetInt.SetCount,_values.Length / 2);
            Assert.AreEqual(disjointSetString.SetCount, _values.Length / 2);

            Assert.AreEqual(disjointSetInt.Count, _values.Length);
            Assert.AreEqual(disjointSetString.Count, _values.Length);

            // exceptions

            Assert.Throws<ArgumentException>(() => disjointSetInt.Union(_values[0], 100));
            Assert.Throws<ArgumentException>(() => disjointSetInt.Union(100, _values[0]));
            Assert.Throws<ArgumentException>(() => disjointSetInt.Union(100, 101));

            Assert.Throws<ArgumentException>(() => disjointSetString.Union(_values[0].ToString(), 100.ToString()));
            Assert.Throws<ArgumentException>(() => disjointSetString.Union(100.ToString(), _values[0].ToString()));
            Assert.Throws<ArgumentException>(() => disjointSetString.Union(100.ToString(), 101.ToString()));

            Assert.Throws<ArgumentNullException>(() => disjointSetString.Union(_values[0].ToString(), null));
            Assert.Throws<ArgumentNullException>(() => disjointSetString.Union(null, _values[0].ToString()));
            Assert.Throws<ArgumentNullException>(() => disjointSetString.Union(null, null));
        }

        /// <summary>
        /// Tests the <see cref="Clear" /> method.
        /// </summary>
        [Test]
        public void DisjointSetForestClearTest()
        {
            // empty

            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>();

            disjointSetInt.Clear();

            Assert.AreEqual(0, disjointSetInt.Count);
            Assert.AreEqual(0, disjointSetInt.SetCount);


            // filled

            disjointSetInt = new DisjointSetForest<Int32>(_values);

            disjointSetInt.Clear();

            Assert.AreEqual(0, disjointSetInt.Count);
            Assert.AreEqual(0, disjointSetInt.SetCount);
        }

        /// <summary>
        /// Tests the enumerator.
        /// </summary>
        [Test]
        public void DisjointSetForestEnumeratorTest()
        {
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>(_values);
            IEnumerator enumerator = _values.GetEnumerator();
            IEnumerator<Int32> disjointSetEnumerator = disjointSetInt.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Assert.IsTrue(disjointSetEnumerator.MoveNext());
                Assert.AreEqual(enumerator.Current, disjointSetEnumerator.Current);
            }
        }

        #endregion
    }
}
