/// <copyright file="MonotoneSubdivisionAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Orsolya Harazin</author>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test class for the <see cref="MonotoneSubdivisionAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class MonotoneSubdivisionAlgorithmTest
    {
        #region Private fields

        /// <summary>
        /// The shell of a polygon.
        /// </summary>
        private List<Coordinate> _shell;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _shell = new List<Coordinate>
            {
                new Coordinate(0, 0), new Coordinate(-10, 5), new Coordinate(10, 10),
                new Coordinate(5, 20), new Coordinate(5, -10), new Coordinate(-10, -10)
            };
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void MonotoneSubdivisionAlgorithmConstructorTest()
        {
            // predefined polygon (which is extended)

            MonotoneSubdivisionAlgorithm algorithm = new MonotoneSubdivisionAlgorithm(_shell);

            Assert.AreEqual(_shell.Count + 1, algorithm.Shell.Count);
            Assert.IsTrue(_shell.SequenceEqual(algorithm.Shell.Take(_shell.Count)));
            Assert.AreEqual(algorithm.Shell[0], algorithm.Shell[algorithm.Shell.Count - 1]);

            // random polygon

            IBasicPolygon polygon = RandomPolygonGenerator.CreateRandomPolygon(100, new Coordinate(10, 10), new Coordinate(50, 50));
            algorithm = new MonotoneSubdivisionAlgorithm(polygon.Shell.Coordinates);

            Assert.IsTrue(polygon.Shell.SequenceEqual(algorithm.Shell));


            // exceptions

            Assert.IsTrue(polygon.Shell.SequenceEqual(algorithm.Shell));

            Assert.Throws<ArgumentNullException>(() => algorithm = new MonotoneSubdivisionAlgorithm(null));
        }

        /// <summary>
        /// Tests the <see cref="Compute" /> method.
        /// </summary>
        [Test]
        public void MonotoneSubdivisionAlgorithmComputeTest()
        {
            MonotoneSubdivisionAlgorithm algorithm = new MonotoneSubdivisionAlgorithm(_shell);

            algorithm.Compute();

            Assert.AreEqual(4, algorithm.Result.Count);
            Assert.IsTrue(algorithm.Result[0].SequenceEqual(new List<Coordinate> { new Coordinate(10, 10), new Coordinate(5, 20), new Coordinate(5, -10) }));
            Assert.IsTrue(algorithm.Result[1].SequenceEqual(new List<Coordinate> { new Coordinate(0, 0), new Coordinate(-10, 5), new Coordinate(10, 10) }));
            Assert.IsTrue(algorithm.Result[2].SequenceEqual(new List<Coordinate> { new Coordinate(0, 0), new Coordinate(10, 10), new Coordinate(5, -10) }));
            Assert.IsTrue(algorithm.Result[3].SequenceEqual(new List<Coordinate> { new Coordinate(0, 0), new Coordinate(5, -10), new Coordinate(-10, -10) }));
        }

        /// <summary>
        /// Tests the <see cref="Triangulate" /> method.
        /// </summary>
        [Test]
        public void MonotoneSubdivisionTriangulateTest()
        {
            for (Int32 polygonNumber = 1; polygonNumber < 10; polygonNumber++)
            {
                IBasicPolygon polygon = RandomPolygonGenerator.CreateRandomPolygon(10 * polygonNumber, new Coordinate(10, 10), new Coordinate(50, 50));
                IList<Coordinate[]> triangles = MonotoneSubdivisionAlgorithm.Triangulate(polygon.Shell.Coordinates);

                Assert.AreEqual(polygon.Shell.Count - 3, triangles.Count);

                foreach (Coordinate[] triangle in triangles)
                {
                    Assert.AreEqual(3, triangle.Length);

                    foreach (Coordinate coordinate in triangle)
                    {
                        Assert.IsTrue(polygon.Shell.Contains(coordinate));
                    }
                }
            }
            
        }

        #endregion
    }
}
