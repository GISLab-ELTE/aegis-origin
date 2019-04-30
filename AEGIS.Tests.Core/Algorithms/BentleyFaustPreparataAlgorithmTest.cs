/// <copyright file="BentleyFaustPreparataAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="BentleyFaustPreparataAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class BentleyFaustPreparataAlgorithmTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="ComputeConvexHull" /> method.
        /// </summary>
        [Test]
        public void BentleyFaustPreparataAlgorithmComputeConvexHullTest()
        {
            // test case 1: convex polygon

            List<Coordinate> shell = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10), 
                new Coordinate(0, 0)
            };

            IList<Coordinate> convexHull = BentleyFaustPreparataAlgorithm.ApproximateConvexHull(shell);

            Assert.AreEqual(shell.Count, convexHull.Count);

            for (Int32 i = 0; i < shell.Count; i++)
                Assert.AreEqual(shell[i], convexHull[i]);


            // test case 2: concave polygon

            shell = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(2, 1), new Coordinate(8, 1), 
                new Coordinate(10, 0), new Coordinate(9, 2), new Coordinate(9, 8), 
                new Coordinate(10, 10), new Coordinate(8, 9), new Coordinate(2, 9), 
                new Coordinate(0, 10), new Coordinate(1, 8), new Coordinate(1, 2),
                new Coordinate(0, 0)
            };

            List<Coordinate> expected = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10), 
                new Coordinate(0, 0)
            };

            convexHull = BentleyFaustPreparataAlgorithm.ApproximateConvexHull(shell);

            Assert.AreEqual(expected.Count, convexHull.Count);

            for (Int32 i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], convexHull[i]);
        }

        #endregion
    }
}
