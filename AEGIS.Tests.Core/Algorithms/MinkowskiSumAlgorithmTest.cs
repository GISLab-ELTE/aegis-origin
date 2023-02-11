// <copyright file="MinkowskiSumAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Algorithms
{
    using ELTE.AEGIS.Algorithms;
    using ELTE.AEGIS.Geometry;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Test fixture for the <see cref="MinkowskiSumAlgorithm" /> class.
    /// </summary>
    /// <author>Gréta Bereczki</author>
    [TestFixture]
    public class MinkowskiSumAlgorithmTest
    {
        #region Private fields

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory _factory;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _factory = new GeometryFactory();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="Buffer" /> method.
        /// </summary>
        [Test]
        public void MinkowskiSumAlgorithmBufferTest()
        {
            // two triangles
            IBasicPolygon buffer = new BasicPolygon(
                new Coordinate[] { new Coordinate(5, 5), new Coordinate(5, 7), new Coordinate(4, 6) });

            List<Coordinate> sourceShellCoordinates = new List<Coordinate>
            {
                new Coordinate(2, 1), new Coordinate(4, 1),
                new Coordinate(3, 3)
            };

            List<Coordinate> expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(6, 7), new Coordinate(7, 6),
                new Coordinate(9, 6),  new Coordinate(9, 8),
                new Coordinate(8, 10), new Coordinate(7, 9),
            };

            IBasicPolygon expectedResult = new BasicPolygon(expectedCoordinates);
            IBasicPolygon actualResult = MinkowskiSumAlgorithm.Buffer(sourceShellCoordinates, buffer);

            Assert.IsTrue(PolygonAlgorithms.IsConvex(actualResult));
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
                Assert.AreEqual(expectedResult.Shell.Coordinates[i], actualResult.Shell.Coordinates[i]);

            // triangle and a square
            buffer = new BasicPolygon(
                new Coordinate[] { new Coordinate(8, 6), new Coordinate(9, 6), new Coordinate(9, 7), new Coordinate(8, 7) });

            sourceShellCoordinates = new List<Coordinate>
            {
                new Coordinate(3, 4), new Coordinate(5, 4),
                new Coordinate(4, 6)
            };

            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(11, 11), new Coordinate(11, 10),
                new Coordinate(13, 10), new Coordinate(14, 10),
                new Coordinate(14, 11), new Coordinate(13, 13),
                new Coordinate(12, 13), new Coordinate(11, 11)
            };

            expectedResult = new BasicPolygon(expectedCoordinates);
            actualResult = MinkowskiSumAlgorithm.Buffer(sourceShellCoordinates, buffer);

            Assert.IsTrue(PolygonAlgorithms.IsConvex(actualResult));
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
                Assert.AreEqual(expectedResult.Shell.Coordinates[i], actualResult.Shell.Coordinates[i]);

            // square and circle of 4 points (square)
            buffer = new BasicPolygon(
                new Coordinate[] { new Coordinate(8, 6), new Coordinate(9, 6), new Coordinate(9, 7), new Coordinate(8, 7) });

            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(6, 6), new Coordinate(8, 4),
                new Coordinate(9, 4),  new Coordinate(11, 6),
                new Coordinate(11, 7), new Coordinate(9, 9),
                new Coordinate(8, 9), new Coordinate(6, 7),
                new Coordinate(6, 6)
            };

            expectedResult = new BasicPolygon(expectedCoordinates);
            actualResult = MinkowskiSumAlgorithm.Buffer(buffer, 2, 4);

            Assert.IsTrue(PolygonAlgorithms.IsConvex(actualResult));
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
            {
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].X, Math.Round(actualResult.Shell.Coordinates[i].X, 2));
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].Y, Math.Round(actualResult.Shell.Coordinates[i].Y, 2));
            }

            // point and circle (of 4 points)
            IBasicPoint point = _factory.CreatePoint(1, 1, 0);
            actualResult = MinkowskiSumAlgorithm.Buffer(point, 2, 4);

            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(3, 1), new Coordinate(1, 3),
                new Coordinate(-1, 1), new Coordinate(1, -1)
            };

            expectedResult = new BasicPolygon(expectedCoordinates);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
            {
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].X, Math.Round(actualResult.Shell.Coordinates[i].X, 2));
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].Y, Math.Round(actualResult.Shell.Coordinates[i].Y, 2));
            }

            // square with a square hole and circle (of 8 points)
            sourceShellCoordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(9, 1),
                new Coordinate(9, 9),
                new Coordinate(1, 9),
                new Coordinate(1, 1)
            };
            List<List<Coordinate>> holes = new List<List<Coordinate>>();
            holes.Add(new List<Coordinate>());
            holes[0] = new List<Coordinate>();
            holes[0].Add(new Coordinate(3, 3));
            holes[0].Add(new Coordinate(3, 7));
            holes[0].Add(new Coordinate(7, 7));
            holes[0].Add(new Coordinate(7, 3));
            holes[0].Add(new Coordinate(3, 3));

            BasicPolygon source = new BasicPolygon(sourceShellCoordinates, holes);
            actualResult = MinkowskiSumAlgorithm.Buffer(source, 1, 8);

            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(0, 1), new Coordinate(0.3, 0.3),
                new Coordinate(1, 0), new Coordinate(9, 0),
                new Coordinate(9.7, 0.3), new Coordinate(10, 1),
                new Coordinate(10, 9), new Coordinate(9.7, 9.7),
                new Coordinate(9, 10), new Coordinate(1, 10),
                new Coordinate(0.3, 9.7), new Coordinate(0, 9)
            };
            List<List<Coordinate>> expectedHoleCoordinates = new List<List<Coordinate>>();
            expectedHoleCoordinates.Add(new List<Coordinate>()
            {
                new Coordinate(4, 3), new Coordinate(3.7, 3.7),
                new Coordinate(3, 4), new Coordinate(3, 6),
                new Coordinate(3.7, 6.3), new Coordinate(4, 7),
                new Coordinate(6, 7), new Coordinate(6.3, 6.3),
                new Coordinate(7, 6), new Coordinate(7, 4),
                new Coordinate(6.3, 3.7), new Coordinate(6, 3)
            });

            expectedResult = new BasicPolygon(expectedCoordinates);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
            {
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].X, Math.Round(actualResult.Shell.Coordinates[i].X, 1));
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].Y, Math.Round(actualResult.Shell.Coordinates[i].Y, 1));
            }

            Assert.AreEqual(expectedHoleCoordinates.Count, actualResult.HoleCount);
            for (Int32 i = 0; i < expectedHoleCoordinates.Count; i++)
                for (Int32 j = 0; j < expectedHoleCoordinates[i].Count; j++)
                {
                    Assert.AreEqual(expectedHoleCoordinates[i][j].X, Math.Round(actualResult.Holes[i].Coordinates[j].X, 1));
                    Assert.AreEqual(expectedHoleCoordinates[i][j].Y, Math.Round(actualResult.Holes[i].Coordinates[j].Y, 1));
                }

            // line string (2 points) and square
            buffer = new BasicPolygon(
               new Coordinate[] { new Coordinate(0, 2), new Coordinate(0, 0), new Coordinate(2, 0), new Coordinate(2, 2) });

            actualResult = MinkowskiSumAlgorithm.Buffer(new BasicLineString(new List<Coordinate>() { new Coordinate(0, 4), new Coordinate(2, 4) }), buffer);
            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(0, 6), new Coordinate(0, 4),
                new Coordinate(2, 4), new Coordinate(4, 4),
                new Coordinate(4, 6), new Coordinate(2, 6)
            };
            expectedResult = new BasicPolygon(expectedCoordinates);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
                Assert.AreEqual(expectedResult.Shell.Coordinates[i], actualResult.Shell.Coordinates[i]);

            // line string (4 points) and square
            buffer = new BasicPolygon(
              new Coordinate[] { new Coordinate(0, 2), new Coordinate(0, 0), new Coordinate(2, 0), new Coordinate(2, 2) });

            actualResult = MinkowskiSumAlgorithm.Buffer(new BasicLineString(new List<Coordinate>() { new Coordinate(1, 1), new Coordinate(3, 3), new Coordinate(5, 6), new Coordinate(3, 8) }), buffer);
            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(1, 3), new Coordinate(1, 1),
                new Coordinate(3, 1), new Coordinate(5, 3),
                new Coordinate(7, 6), new Coordinate(7, 8),
                new Coordinate(5, 10), new Coordinate(3, 10)
            };
            expectedResult = new BasicPolygon(expectedCoordinates);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
                Assert.AreEqual(expectedResult.Shell.Coordinates[i], actualResult.Shell.Coordinates[i]);

            // concave and convex with no hole in the result
            buffer = new BasicPolygon(
                new Coordinate[] { new Coordinate(13, 2), new Coordinate(14, 3), new Coordinate(13, 4), new Coordinate(12, 3) });

            sourceShellCoordinates = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(11, 1),
                new Coordinate(11, 6), new Coordinate(8, 6),
                new Coordinate(9, 3), new Coordinate(3, 3),
                new Coordinate(4, 6), new Coordinate(1, 6)
            };

            actualResult = MinkowskiSumAlgorithm.Buffer(sourceShellCoordinates, buffer);

            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(13, 4), new Coordinate(14, 3),
                new Coordinate(24, 3), new Coordinate(25, 4),
                new Coordinate(25, 9), new Coordinate(24, 10),
                new Coordinate(21, 10), new Coordinate(20, 9),
                new Coordinate(20.67, 7), new Coordinate(17.33, 7),
                new Coordinate(18, 9), new Coordinate(17, 10),
                new Coordinate(14, 10), new Coordinate(13, 9)
            };

            expectedResult = new BasicPolygon(expectedCoordinates);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
            {
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].X, Math.Round(actualResult.Shell.Coordinates[i].X, 2));
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].Y, Math.Round(actualResult.Shell.Coordinates[i].Y, 2));
            }

            // convex and concave with no hole in the result
            buffer = new BasicPolygon(
                new Coordinate[] { new Coordinate(0, 3), new Coordinate(-3, 0), new Coordinate(0, -3), new Coordinate(3, 0) });

            sourceShellCoordinates = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(11, 1),
                new Coordinate(11, 6), new Coordinate(8, 6),
                new Coordinate(9, 3), new Coordinate(3, 3),
                new Coordinate(4, 6), new Coordinate(1, 6)
            };

            actualResult = MinkowskiSumAlgorithm.Buffer(sourceShellCoordinates, buffer);

            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(-2, 1), new Coordinate(1, -2),
                new Coordinate(11, -2), new Coordinate(14, 1),
                new Coordinate(14, 6), new Coordinate(11, 9),
                new Coordinate(8, 9), new Coordinate(6, 7),
                new Coordinate(4, 9), new Coordinate(1, 9),
                new Coordinate(-2, 6)
            };

            expectedResult = new BasicPolygon(expectedCoordinates);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
            {
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].X, Math.Round(actualResult.Shell.Coordinates[i].X, 2));
                Assert.AreEqual(expectedResult.Shell.Coordinates[i].Y, Math.Round(actualResult.Shell.Coordinates[i].Y, 2));
            }

            // convex and concave with hole in the result
            buffer = new BasicPolygon(
                new Coordinate[] { new Coordinate(13, 4), new Coordinate(12, 3), new Coordinate(13, 2), new Coordinate(14, 3) });

            sourceShellCoordinates = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(11, 1),
                new Coordinate(11, 6), new Coordinate(7, 6),
                new Coordinate(9, 3), new Coordinate(4, 3),
                new Coordinate(6, 6), new Coordinate(1, 6)
            };

            actualResult = MinkowskiSumAlgorithm.Buffer(sourceShellCoordinates, buffer);
            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(13, 4), new Coordinate(14, 3),
                new Coordinate(24, 3), new Coordinate(25, 4),
                new Coordinate(25, 9), new Coordinate(24, 10),
                new Coordinate(20, 10), new Coordinate(19.5, 9.5),
                new Coordinate(19, 10), new Coordinate(14, 10),
                new Coordinate(13, 9)
            };

            expectedHoleCoordinates = new List<List<Coordinate>>();
            expectedHoleCoordinates.Add(new List<Coordinate>()
            {
                new Coordinate(19.5, 8.25), new Coordinate(20.3, 7),
                new Coordinate(18.6, 7)
            });

            expectedResult = new BasicPolygon(expectedCoordinates);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
                Assert.AreEqual(expectedResult.Shell.Coordinates[i], actualResult.Shell.Coordinates[i]);

            Assert.AreEqual(expectedHoleCoordinates.Count, actualResult.HoleCount);
            for (Int32 i = 0; i < expectedHoleCoordinates.Count; i++)
            {
                for (Int32 j = 0; j < expectedHoleCoordinates[i].Count; j++)
                {
                    Assert.AreEqual(expectedHoleCoordinates[i][j].X, Math.Round(actualResult.Holes[i].Coordinates[j].X), 2);
                    Assert.AreEqual(expectedHoleCoordinates[i][j].Y, Math.Round(actualResult.Holes[i].Coordinates[j].Y), 2);
                }
            }

            // convex source polygon with hole and a rhombus
            buffer = _factory.CreatePolygon(
               _factory.CreatePoint(10, 2),
               _factory.CreatePoint(11, 3),
               _factory.CreatePoint(10, 4),
               _factory.CreatePoint(9, 3));

            sourceShellCoordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(9, 1),
                new Coordinate(9, 9),
                new Coordinate(1, 9),
                new Coordinate(1, 1)
            };

            holes = new List<List<Coordinate>>();
            holes.Add(new List<Coordinate>());
            holes[0] = new List<Coordinate>();
            holes[0].Add(new Coordinate(3, 3));
            holes[0].Add(new Coordinate(3, 7));
            holes[0].Add(new Coordinate(7, 7));
            holes[0].Add(new Coordinate(7, 3));
            holes[0].Add(new Coordinate(3, 3));

            source = new BasicPolygon(sourceShellCoordinates, holes);
            actualResult = MinkowskiSumAlgorithm.Buffer(source, buffer);

            expectedCoordinates = new List<Coordinate>
            {
                new Coordinate(10, 4), new Coordinate(11, 3),
                new Coordinate(19, 3), new Coordinate(20, 4),
                new Coordinate(20, 12), new Coordinate(19, 13),
                new Coordinate(11, 13), new Coordinate(10, 12)
            };

            expectedHoleCoordinates = new List<List<Coordinate>>();
            expectedHoleCoordinates.Add(new List<Coordinate>()
            {
                new Coordinate(14, 6), new Coordinate(13, 7),
                new Coordinate(13, 9), new Coordinate(14, 10),
                new Coordinate(16, 10), new Coordinate(17, 9),
                new Coordinate(17, 7), new Coordinate(16, 6)
            });

            expectedResult = new BasicPolygon(expectedCoordinates);
            Assert.AreEqual(expectedResult.Shell.Count, actualResult.Shell.Count);
            for (Int32 i = 0; i < expectedResult.Shell.Count; i++)
                Assert.AreEqual(expectedResult.Shell.Coordinates[i], actualResult.Shell.Coordinates[i]);

            Assert.AreEqual(expectedHoleCoordinates.Count, actualResult.HoleCount);
            for (Int32 i = 0; i < expectedHoleCoordinates.Count; i++)
                for (Int32 j = 0; j < expectedHoleCoordinates[i].Count; j++)
                    Assert.AreEqual(expectedHoleCoordinates[i][j], actualResult.Holes[i].Coordinates[j]);

            // ArgumentNullException: null source
            Assert.Throws<ArgumentNullException>(() => MinkowskiSumAlgorithm.Buffer((IBasicPolygon)null, buffer));

            // ArgumentNullException: null buffer polygon
            Assert.Throws<ArgumentNullException>(() => MinkowskiSumAlgorithm.Buffer(source, null));

            // ArgumentException: source polygon with clockwise orientation
            sourceShellCoordinates = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(1, 9),
                new Coordinate(9, 9),
                new Coordinate(9, 1),
                new Coordinate(1, 1)
            };
            source = new BasicPolygon(sourceShellCoordinates);
            Assert.Throws<ArgumentException>(() => MinkowskiSumAlgorithm.Buffer(source, buffer));
        }
        #endregion
    }
}
