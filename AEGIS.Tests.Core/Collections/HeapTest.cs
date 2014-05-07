/// <copyright file="HeapTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Collections;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Collections
{
    [TestFixture]
    public class HeapTest
    {
        [TestCase]
        public void HeapOperationsTest()
        {
            // test case 1: integer sequence

            Heap<Int32, String> stringHeap = new Heap<Int32, String>();
            stringHeap.Insert(1,"1");

            Assert.AreEqual(stringHeap.Count, 1);
            Assert.AreEqual(stringHeap.Peek, "1");

            stringHeap.Insert(3, "3");
            stringHeap.Insert(2, "2");
            stringHeap.Insert(2, "2");

            Assert.AreEqual(stringHeap.Count, 4);
            Assert.AreEqual(stringHeap.Peek, "1");

            stringHeap.Insert(0, "0");

            Assert.AreEqual(stringHeap.Count, 5);
            Assert.AreEqual(stringHeap.Peek, "0");

            Assert.AreEqual(stringHeap.RemovePeek(), "0");
            Assert.AreEqual(stringHeap.RemovePeek(), "1");
            Assert.AreEqual(stringHeap.RemovePeek(), "2");
            Assert.AreEqual(stringHeap.RemovePeek(), "2");
            Assert.AreEqual(stringHeap.RemovePeek(), "3");

            Assert.AreEqual(stringHeap.Count, 0);
        

            // test case 2: random numbers

            Random random = new Random();
            Int32[] values = Enumerable.Range(0, 10000).Select(x => random.Next(0, 10000)).ToArray();

            Heap<Int32, Int32> integerHeap = new Heap<Int32, Int32>();

            for (Int32 i = 0; i < values.Length; i++)
            {
                integerHeap.Insert(values[i], values[i]);
            }

            Int32[] expected = values.OrderBy(x => x).ToArray();

            for (Int32 i = 0; i < values.Length; i++)
            {
                Int32 actual = integerHeap.RemovePeek();

                Assert.AreEqual(expected[i], actual);
            }
        }
    }
}
