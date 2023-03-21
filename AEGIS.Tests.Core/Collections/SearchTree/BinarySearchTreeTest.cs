// <copyright file="BinarySearchTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections.SearchTree;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Collections.SearchTree
{
    [TestFixture]
    public class BinarySearchTreeTest
    {
        [TestCase]
        public void BinarySearchTreeOperationsTest()
        {
            // test case 1: integer sequence

            BinarySearchTree<Int32, String> bst = new BinarySearchTree<Int32, String>();

            for (Int32 i = 0; i < 1000; i++)
            {
                bst.Insert(i, i.ToString());
            }

            for (Int32 i = 0; i < 1000; i++)
            {
                Assert.AreEqual(i.ToString(), bst.Search(i));
            }

            for (Int32 i = 0; i < 1000; i++)
            {
                Assert.IsTrue(bst.Remove(i));
            }

            Assert.AreEqual(0, bst.Count);


            // test case 2: random numbers

            bst = new BinarySearchTree<Int32, String>();

            List<Int32> keyList = new List<Int32>();

            Random random = new Random();
            for (Int32 i = 0; i < 1000; i++)
            {
                Int32 key = random.Next(1, 1000);
                if (!bst.Contains(key))
                {
                    bst.Insert(key, key.ToString());
                    keyList.Add(key);
                }
            }

            Assert.AreEqual(keyList.Count, bst.Count);

            for (Int32 i = 0; i < keyList.Count; i++)
            {
                Assert.AreEqual(keyList[i].ToString(), bst.Search(keyList[i]));
            }

            for (Int32 i = 0; i < keyList.Count; i++)
            {
                Assert.IsTrue(bst.Remove(keyList[i]));
            }

            Assert.AreEqual(0, bst.Count);
        }

        [TestCase]
        public void BinarySearchTreeHeightTest()
        {
            // test case 1

            BinarySearchTree<Int32, String> bst = new BinarySearchTree<Int32, String>();

            for (Int32 i = 0; i < 1000; i++)
            {
                bst.Insert(i, i.ToString());
            }

            Assert.AreEqual(999, bst.Height);
            Assert.AreEqual(1000, bst.Count);


            // test case 2

            bst = new BinarySearchTree<Int32, String>();

            bst.Insert(500, "500");
            for (Int32 i = 1; i < 500; i++)
            {
                bst.Insert(i, i.ToString());
                bst.Insert(i + 500, i.ToString());
            }

            Assert.AreEqual(499, bst.Height);
            Assert.AreEqual(999, bst.Count);

        }

        [TestCase]
        public void BinarySearchTreeEnumeratorTest()
        {
            // test case 1

            BinarySearchTree<Int32, String> bst = new BinarySearchTree<Int32, String>();

            Random random = new Random();
            for (Int32 i = 0; i < 1000; i++)
            {
                Int32 key = random.Next(1, 1000);
                if (!bst.Contains(key))
                    bst.Insert(key, i.ToString());
            }
            Int32 prevKey = 0;
            Int32 count = 0;
            foreach (KeyValuePair<Int32, String> element in bst)
            {
                Assert.IsTrue(element.Key > prevKey);
                prevKey = element.Key;
                count++;
            }
            Assert.AreEqual(bst.Count, count);


            // test case 2

            bst = new BinarySearchTree<Int32, String>();

            for (Int32 i = 0; i < 1000; i++)
            {
                Int32 key = random.Next(1, 1000);
                if (!bst.Contains(key))
                    bst.Insert(key, i.ToString());
            }

            ISearchTreeEnumerator<Int32, String> enumerator = bst.GetTreeEnumerator();

            List<Int32> forwardList = new List<Int32>();
            List<Int32> backwardList = new List<Int32>();

            if (enumerator.MoveMin())
            {
                do
                {
                    forwardList.Add(enumerator.Current.Key);
                } while (enumerator.MoveNext());
            }

            if (enumerator.MoveMax())
            {
                do
                {
                    backwardList.Add(enumerator.Current.Key);
                } while (enumerator.MovePrev());
            }

            backwardList.Reverse();
            Assert.IsTrue(forwardList.SequenceEqual(backwardList));
        }
    }
}
