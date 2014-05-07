/// <copyright file="ShamosHoeyAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Algorithms
{
    [TestFixture]
    public class ShamosHoeyAlgorithmTest
    {
        [TestCase]
        public void ShamosHoeyAlgorithmIntersectsTest()
        {
            // test case 1: simple polygon, intersection

            IList<Coordinate> coordinates = new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(1, 0), new Coordinate(0, 1) };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinates));


            // test case 2: simple polygon, no intersection

            coordinates = new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 0), new Coordinate(3, 1) };
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));


            // test case 3: simple line, no intersection

            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n, n)).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));

            
            // test case 4: line string, no intersection

            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n, n % 2)).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));


            // test case 5: line string, no intersection

            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n % 2, n)).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));
        

            // test case 6: random line string, no intersection

            Random random = new Random();

            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(random.Next(1000), n)).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));


            // test case 7: random line string, no intersection
            
            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n, random.Next(1000))).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));
        
            
            // test case 8: random line string, intersection

            coordinates = Enumerable.Range(0, 10000).Select(n => new Coordinate(random.Next(100), random.Next(100))).ToArray();
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinates));
        

            // test case 9: multiple line strings, intersection

            IList<Coordinate[]> coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1) },
                new Coordinate[] { new Coordinate(0, 1), new Coordinate(1, 0) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // test case 10: multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2) },
                new Coordinate[] { new Coordinate(2, 0), new Coordinate(0, 2) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // test case 11: multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, 1), new Coordinate(1, 0) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // test case 11: multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(1, 2), new Coordinate(2, 1) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // test case 11: multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(2, 3), new Coordinate(3, 2) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // test case 11: multiple line strings, no intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, -1), new Coordinate(-1, 0) },
                new Coordinate[] { new Coordinate(-1, -2), new Coordinate(-2, -1) },
                new Coordinate[] { new Coordinate(-2, -3), new Coordinate(-3, -2) }
            };
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // test case 11: multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, -1), new Coordinate(-1, 0) },
                new Coordinate[] { new Coordinate(-1, -2), new Coordinate(-2, -1) },
                new Coordinate[] { new Coordinate(-2, -3), new Coordinate(-3, -2) },
                new Coordinate[] { new Coordinate(1, 2), new Coordinate(2, 1) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // test case 11: multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                Enumerable.Range(1, 1000).Select(n => new Coordinate(n, n)).ToArray(),
                Enumerable.Range(1, 1000).Select(n => new Coordinate(1000 - n, n)).ToArray()
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));
        }
    }
}
