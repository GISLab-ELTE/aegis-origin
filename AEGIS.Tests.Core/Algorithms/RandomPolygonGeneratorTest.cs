/// <copyright file="RandomPolygonGeneratorTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Orsolya Harazin</author>

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="RandomPolygonGenerator" /> class.
    /// </summary>
    [TestFixture]
    public class RandomPolygonGeneratorTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="CreateRandomPolygon" /> method.
        /// </summary>
        [Test]
        public void RandomPolygonGeneratorCreateRandomPolygonTest()
        {
            // check properties of generated polygons

            Int32 coordinateNumber = 10;
            Coordinate minCoordinate = new Coordinate(10, 10);
            Coordinate maxCoordinate = new Coordinate(20, 20);
            Double convexityRatio = 0.1;

            for (Int32 polygonNumber = 1; polygonNumber < 100; polygonNumber++)
            {
                IBasicPolygon randomPolygon = RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber * polygonNumber, minCoordinate, maxCoordinate);

                Assert.AreEqual(coordinateNumber * polygonNumber + 1, randomPolygon.Shell.Count); // number of coordinates
                Assert.AreEqual(Orientation.CounterClockwise, PolygonAlgorithms.Orientation(randomPolygon)); // orientation

                foreach (Coordinate coordinate in randomPolygon.Shell)
                {
                    Assert.True(coordinate.X > minCoordinate.X && coordinate.X < maxCoordinate.X &&
                                coordinate.Y > minCoordinate.Y && coordinate.Y < maxCoordinate.Y); // all coordinates are located in the rectangle
                }

                Assert.IsFalse(ShamosHoeyAlgorithm.Intersects(randomPolygon.Shell)); // no intersection
            }

            // check convexity

            for (Int32 polygonNumber = 1; polygonNumber < 100; polygonNumber++)
            {
                Assert.IsTrue(PolygonAlgorithms.IsConvex(RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber * polygonNumber, minCoordinate, maxCoordinate, 1)));
            }

            for (Int32 polygonNumber = 1; polygonNumber < 100; polygonNumber++)
            {
                Assert.IsFalse(PolygonAlgorithms.IsConvex(RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber * polygonNumber, minCoordinate, maxCoordinate, 0)));
            }

            // check exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => RandomPolygonGenerator.CreateRandomPolygon(1, minCoordinate, maxCoordinate, convexityRatio));
            Assert.Throws<ArgumentOutOfRangeException>(() => RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber, maxCoordinate, minCoordinate, convexityRatio));
            Assert.Throws<ArgumentOutOfRangeException>(() => RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber, minCoordinate, maxCoordinate, -1));
        }

        #endregion
    }
}
