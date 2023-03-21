// <copyright file="AvlTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class AvlTreeTest
    {
        [TestCase]
        public void AvlTreeOperationsTest()
        {
            // test case 1: integer sequence

            AvlTree<Int32, String> avl = new AvlTree<Int32, String>();

            for (Int32 i = 0; i < 1000; i++)
            {
                avl.Insert(i, i.ToString());
            }

            for (Int32 i = 0; i < 1000; i++)
            {
                Assert.AreEqual(i.ToString(), avl.Search(i));
            }

            for (Int32 i = 0; i < 1000; i++)
            {
                Assert.IsTrue(avl.Remove(i));
            }

            Assert.AreEqual(0, avl.Count);


            // test case 2: random numbers

            avl = new AvlTree<Int32, String>();

            List<Int32> keyList = new List<Int32>();

            Random random = new Random();
            for (Int32 i = 0; i < 10000; i++)
            {
                Int32 key = random.Next(1, 100000);
                if (!avl.Contains(key))
                {
                    avl.Insert(key, key.ToString());
                    keyList.Add(key);
                }
            }

            Assert.AreEqual(keyList.Count, avl.Count);

            for (Int32 i = 0; i < keyList.Count; i++)
            {
                Assert.AreEqual(keyList[i].ToString(), avl.Search(keyList[i]));
            }

            for (Int32 i = 0; i < keyList.Count; i++)
            {
                Assert.IsTrue(avl.Remove(keyList[i]));
            }

            Assert.AreEqual(0, avl.Count);
        }

        [TestCase]
        public void AvlTreeBalanceTest()
        {
            // test case 1: ++, +

            AvlTree<Int32, String> avl = new AvlTree<Int32, String>();
            avl.Insert(1, "1");
            avl.Insert(2, "2");
            avl.Insert(3, "3");

            Assert.IsTrue(avl.Select(element => element.Key).SequenceEqual(Enumerable.Range(1, 3)));
            Assert.AreEqual(1, avl.Height);
        

            // test case 2: --, -

            avl = new AvlTree<Int32, String>();
            avl.Insert(3, "1");
            avl.Insert(2, "2");
            avl.Insert(1, "3");

            Assert.IsTrue(avl.Select(element => element.Key).SequenceEqual(Enumerable.Range(1, 3)));
            Assert.AreEqual(1, avl.Height);
        
            // test case 3: ++, -

            avl = new AvlTree<Int32, String>();
            avl.Insert(4, "1");
            avl.Insert(2, "2");
            avl.Insert(9, "3");
            avl.Insert(6, "4");
            avl.Insert(3, "5");
            avl.Insert(10, "6");
            avl.Insert(8, "7");
            avl.Insert(11, "8");
            avl.Insert(1, "9");
            avl.Insert(5, "10");
            avl.Insert(7, "11");

            Assert.IsTrue(avl.Select(element => element.Key).SequenceEqual(Enumerable.Range(1, 11)));
            Assert.AreEqual(3, avl.Height);


            // test case 3: --, +

            avl = new AvlTree<Int32, String>();
            avl.Insert(8, "1");
            avl.Insert(10, "2");
            avl.Insert(3, "3");
            avl.Insert(5, "4");
            avl.Insert(11, "5");
            avl.Insert(9, "6");
            avl.Insert(1, "7");
            avl.Insert(4, "8");
            avl.Insert(2, "9");
            avl.Insert(6, "10");
            avl.Insert(7, "11");

            Assert.IsTrue(avl.Select(element => element.Key).SequenceEqual(Enumerable.Range(1, 11)));
            Assert.AreEqual(3, avl.Height);
      

            // test case 4: ++, =

            avl = new AvlTree<Int32, String>();
            avl.Insert(4, "1");
            avl.Insert(2, "2");
            avl.Insert(5, "3");
            avl.Insert(1, "4");
            avl.Insert(3, "5");
            avl.Remove(5);

            Assert.IsTrue(avl.Select(element => element.Key).SequenceEqual(Enumerable.Range(1, 4)));
            Assert.AreEqual(2, avl.Height);
        

            // test case 5: --, =

            avl = new AvlTree<Int32, String>();
            avl.Insert(2, "1");
            avl.Insert(1, "2");
            avl.Insert(4, "3");
            avl.Insert(5, "4");
            avl.Insert(3, "5");
            avl.Remove(1);

            Assert.IsTrue(avl.Select(element => element.Key).SequenceEqual(Enumerable.Range(2, 4)));
            Assert.AreEqual(2, avl.Height);
       

            // test case 7: random tests

            avl = new AvlTree<Int32, String>();

            Random random = new Random();
            for (Int32 i = 0; i < 10000; i++)
            {
                Int32 key = random.Next(1, 100000);
                if (!avl.Contains(key))
                    avl.Insert(key, i.ToString());
            }
            Int32 prevKey = 0;
            Int32 count = 0;
            foreach (KeyValuePair<Int32, String> element in avl)
            {
                Assert.IsTrue(element.Key > prevKey);
                prevKey = element.Key;
                count++;
            }

            for (Int32 i = 0; i < 50000; i++)
            {
                Int32 key = random.Next(1, 100000);
                if (avl.Contains(key))
                    avl.Remove(key);
            }

            prevKey = 0;
            foreach (KeyValuePair<Int32, String> element in avl)
            {
                Assert.IsTrue(element.Key > prevKey);
                prevKey = element.Key;
            }
        }

        [TestCase]
        public void BinarySearchTreeEnumeratorTest()
        {
            // test case 1

            AvlTree<Int32, String> avl = new AvlTree<Int32, String>();

            Random random = new Random();
            for (Int32 i = 0; i < 1000; i++)
            {
                Int32 key = random.Next(1, 1000);
                if (!avl.Contains(key))
                    avl.Insert(key, i.ToString());
            }
            Int32 prevKey = 0;
            Int32 count = 0;
            foreach (KeyValuePair<Int32, String> element in avl)
            {
                Assert.IsTrue(element.Key > prevKey);
                prevKey = element.Key;
                count++;
            }
            Assert.AreEqual(avl.Count, count);
       

            // test case 2

            avl = new AvlTree<Int32, String>();

            random = new Random();
            for (Int32 i = 0; i < 1000; i++)
            {
                Int32 key = random.Next(1, 1000);
                if (!avl.Contains(key))
                    avl.Insert(key, i.ToString());
            }

            ISearchTreeEnumerator<Int32, String> enumerator = avl.GetTreeEnumerator();

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
