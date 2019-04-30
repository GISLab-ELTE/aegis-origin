/// <copyright file="ReadOnlySetTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Collections
{
    /// <summary>
    /// Test fixture for the <see cref="ReadOnlySet"/> class.
    /// </summary>
    [TestFixture]
    public class ReadOnlySetTest
    {
        #region Private fields

        /// <summary>
        /// The array of inner sets that are wrapped.
        /// </summary>
        private ISet<Int32>[] _innerSets;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Random random = new Random(0);
            Int32[] randomValues = Enumerable.Range(0, 1000).Select(value => random.Next(1, 1000)).ToArray();

            _innerSets = new ISet<Int32>[]
            {
                new SortedSet<Int32>(),
                new SortedSet<Int32>(Enumerable.Range(1, 1000)),
                new SortedSet<Int32>(Enumerable.Repeat(1, 10)),
                new SortedSet<Int32>(randomValues),
                new HashSet<Int32>(),
                new HashSet<Int32>(Enumerable.Range(1, 1000)),
                new HashSet<Int32>(Enumerable.Range(1, 10)),
                new HashSet<Int32>(randomValues)
            };
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void ReadOnlySetConstructorTest()
        {
            foreach(ISet<Int32> set in _innerSets)
            {
                ReadOnlySet<Int32> readOnlySet = new ReadOnlySet<Int32>(set);

                Assert.IsTrue(readOnlySet.IsReadOnly);
                Assert.AreEqual(set.Count, readOnlySet.Count);

            }


            // exceptions

            Assert.Throws<ArgumentNullException>(() => new ReadOnlySet<Int32>(null));
        }

        /// <summary>
        /// Test case for supported interface methods.
        /// </summary>
        [Test]
        public void ReadOnlySetSupportedMethodsTest()
        {
            foreach (ISet<Int32> set in _innerSets)
            {
                ReadOnlySet<Int32> readOnlySet = new ReadOnlySet<Int32>(set);

                Assert.AreEqual(set.IsSubsetOf(_innerSets[0]), readOnlySet.IsSubsetOf(_innerSets[0]));
                Assert.AreEqual(set.IsSupersetOf(_innerSets[0]), readOnlySet.IsSupersetOf(_innerSets[0]));
                Assert.AreEqual(set.IsProperSupersetOf(_innerSets[0]), readOnlySet.IsProperSupersetOf(_innerSets[0]));
                Assert.AreEqual(set.IsProperSubsetOf(_innerSets[0]), readOnlySet.IsProperSubsetOf(_innerSets[0]));
                Assert.AreEqual(set.Overlaps(_innerSets[0]), readOnlySet.Overlaps(_innerSets[0]));
                Assert.AreEqual(set.SetEquals(_innerSets[0]), readOnlySet.SetEquals(_innerSets[0]));

                Assert.AreEqual(set.Contains(0), readOnlySet.Contains(0));

                Int32[] setValues = new Int32[set.Count], readOnlySetValues = new Int32[readOnlySet.Count];
                set.CopyTo(setValues, 0);
                readOnlySet.CopyTo(readOnlySetValues, 0);

                Assert.IsTrue(setValues.SequenceEqual(readOnlySetValues));
            }
        }

        /// <summary>
        /// Test case for not supported interface methods.
        /// </summary>
        [Test]
        public void ReadOnlySetNotSupportedMethodsTest()
        {
            ISet<Int32> readOnlySet = new ReadOnlySet<Int32>(_innerSets[0]);

            Assert.Throws<NotSupportedException>(() => readOnlySet.Add(0));
            Assert.Throws<NotSupportedException>(() => (readOnlySet as ICollection<Int32>).Add(0));
            Assert.Throws<NotSupportedException>(() => readOnlySet.Remove(0));
            Assert.Throws<NotSupportedException>(() => readOnlySet.Clear());
            Assert.Throws<NotSupportedException>(() => readOnlySet.UnionWith(_innerSets[1]));
            Assert.Throws<NotSupportedException>(() => readOnlySet.IntersectWith(_innerSets[1]));
            Assert.Throws<NotSupportedException>(() => readOnlySet.ExceptWith(_innerSets[1]));
            Assert.Throws<NotSupportedException>(() => readOnlySet.SymmetricExceptWith(_innerSets[1]));
            Assert.Throws<NotSupportedException>(() => readOnlySet.UnionWith(_innerSets[1]));
        }
        
        /// <summary>
        /// Test case for the enumerator.
        /// </summary>
        [Test]
        public void ReadOnlySetEnumeratorTest()
        {
            // traversal

            foreach (ISet<Int32> set in _innerSets)
            {
                ReadOnlySet<Int32> readOnlySet = new ReadOnlySet<Int32>(set);

                IEnumerator<Int32> enumerator = set.GetEnumerator();
                IEnumerator<Int32> readOnlyEnumerator = readOnlySet.GetEnumerator();
                IEnumerator readOnlyCollectionEnumerator = (readOnlySet as IEnumerable).GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Assert.IsTrue(readOnlyEnumerator.MoveNext());
                    Assert.IsTrue(readOnlyCollectionEnumerator.MoveNext());

                    Assert.AreEqual(enumerator.Current, readOnlyEnumerator.Current);
                    Assert.AreEqual(readOnlyCollectionEnumerator.Current, readOnlyEnumerator.Current);
                }
            }
        }

        #endregion
    }
}
