// <copyright file="ShamosHoeyAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="ShamosHoeyAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class ShamosHoeyAlgorithmTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="Intersects" /> method.
        /// </summary>
        [Test]
        public void ShamosHoeyAlgorithmIntersectsTest()
        {
            // simple polygon, intersection

            IList<Coordinate> coordinates = new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(1, 0), new Coordinate(0, 1) };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinates));


            // simple polygon, no intersection

            coordinates = new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 0), new Coordinate(3, 1) };
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));


            // simple line, no intersection

            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n, n)).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));

            
            // line string, no intersection

            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n, n % 2)).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));


            // line string, no intersection

            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n % 2, n)).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));
        

            // random line string, no intersection

            Random random = new Random();

            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(random.Next(1000), n)).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));


            // random line string, no intersection
            
            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n, random.Next(1000))).ToArray();
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinates));
        
            
            // random line string, intersection

            coordinates = Enumerable.Range(0, 10000).Select(n => new Coordinate(random.Next(100), random.Next(100))).ToArray();
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinates));
        

            // multiple line strings, intersection

            IList<Coordinate[]> coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1) },
                new Coordinate[] { new Coordinate(0, 1), new Coordinate(1, 0) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2) },
                new Coordinate[] { new Coordinate(2, 0), new Coordinate(0, 2) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, 1), new Coordinate(1, 0) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(1, 2), new Coordinate(2, 1) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(2, 3), new Coordinate(3, 2) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // multiple line strings, no intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, -1), new Coordinate(-1, 0) },
                new Coordinate[] { new Coordinate(-1, -2), new Coordinate(-2, -1) },
                new Coordinate[] { new Coordinate(-2, -3), new Coordinate(-3, -2) }
            };
            Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, -1), new Coordinate(-1, 0) },
                new Coordinate[] { new Coordinate(-1, -2), new Coordinate(-2, -1) },
                new Coordinate[] { new Coordinate(-2, -3), new Coordinate(-3, -2) },
                new Coordinate[] { new Coordinate(1, 2), new Coordinate(2, 1) }
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));


            // multiple line strings, intersection

            coordinateLists = new List<Coordinate[]>
            {                
                Enumerable.Range(1, 1000).Select(n => new Coordinate(n, n)).ToArray(),
                Enumerable.Range(1, 1000).Select(n => new Coordinate(1000 - n, n)).ToArray()
            };
            Assert.IsTrue(ShamosHoeyAlgorithm.Intersects(coordinateLists));
        }

        #endregion
    }
}
