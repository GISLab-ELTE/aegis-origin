/// <copyright file="BentleyOttmannAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
/// <author>Máté Cserép</author>

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="BentleyOttmannAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class BentleyOttmannAlgorithmTest
    {
        #region Private fields

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory _factory;

        #endregion

        #region Test setup

        /// <summary>
        /// Test fixture setup.
        /// </summary>
        [TestFixtureSetUp]
        public void FixtureInitialize()
        {
            _factory = Factory.DefaultInstance<IGeometryFactory>();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="Intersection" /> method.
        /// </summary>
        [Test]
        public void BentleyOttmannAlgorithmIntersectionTest()
        {
            // single line, no intersection

            IList<Coordinate> intersections = BentleyOttmannAlgorithm.Intersection(new List<Coordinate>
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 20)
                });

            Assert.IsEmpty(intersections);


            // single linestring, one intersection

            intersections = BentleyOttmannAlgorithm.Intersection(new List<Coordinate>
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 20),
                    new Coordinate(15, 20),
                    new Coordinate(15, 10)
                });

            Assert.AreEqual(new[] { new Coordinate(15, 15) }, intersections);


            // multiple lines, no intersection

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(10, 10),
                            new Coordinate(20, 20)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(0, 0),
                            new Coordinate(10, 0)
                        }
                });

            Assert.IsEmpty(intersections);


            // multiple lines, one intersection
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(10, 10),
                            new Coordinate(20, 20)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(0, 0),
                            new Coordinate(10, 10)
                        }
                });

            Assert.AreEqual(new[] { new Coordinate(10, 10) }, intersections);


            // multiple lines, one intersection

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(10, 10),
                            new Coordinate(20, 20)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(15, 20),
                            new Coordinate(15, 10)
                        }
                });

            Assert.AreEqual(new[] { new Coordinate(15, 15) }, intersections);
        

            // multiple lines, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(-10, 0),
                            new Coordinate(10, 0)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(-10, -10),
                            new Coordinate(10, 10)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(3, 5),
                            new Coordinate(10, 5)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(4, 8),
                            new Coordinate(10, 8)
                        }
                });

            Assert.AreEqual(new[]
            {
                new Coordinate(0, 0),
                new Coordinate(5, 5),
                new Coordinate(8, 8),
            }, intersections);


            // multiple lines, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(-5, 0),
                            new Coordinate(5, 0)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(0, -2),
                            new Coordinate(8, 2)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(1, -3),
                            new Coordinate(3, 3)
                        }
                });

            Assert.AreEqual(intersections.Count, 3);
            Assert.AreEqual(intersections[0].X, 1.6, 0.0001);
            Assert.AreEqual(intersections[0].Y, -1.2, 0.0001);
            Assert.AreEqual(intersections[1].X, 2, 0.0001);
            Assert.AreEqual(intersections[1].Y, 0, 0.0001);
            Assert.AreEqual(intersections[2].X, 4, 0.0001);
            Assert.AreEqual(intersections[2].Y, 0, 0.0001);


            // multiple lines, multiple intersections in the same coordinate
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(-5, 0),
                            new Coordinate(5, 0)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(0, 5),
                            new Coordinate(5, 0)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(4, -1),
                            new Coordinate(5, 0)
                        }
                });

            Assert.AreEqual(new[]
            {
                new Coordinate(5, 0),
                new Coordinate(5, 0),
                new Coordinate(5, 0),
            }, intersections);


            // multiple lines, multiple intersections in the same coordinate
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(10, 0),
                            new Coordinate(10, 10),
                            new Coordinate(0, 10)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(10, 20),
                            new Coordinate(10, 10),
                            new Coordinate(20, 10)
                        }
                });

            Assert.AreEqual(new[]
            {
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
            }, intersections);


            // multiple lines, multiple intersections (even in the same coordinate)

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(0, 0),
                            new Coordinate(10, 0),
                            new Coordinate(10, 10),
                            new Coordinate(0, 10),
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(20, 0),
                            new Coordinate(10, 0),
                            new Coordinate(10, 10),
                            new Coordinate(20, 10),
                        }
                });

            Assert.AreEqual(new[]
            {
                new Coordinate(10, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
            }, intersections);


            // single polygon, no intersection
            
            intersections = BentleyOttmannAlgorithm.Intersection(
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10))
                        .Shell.Coordinates);

            Assert.IsEmpty(intersections);


            // multiple polygons, no intersection
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    _factory.CreatePolygon(
                        _factory.CreatePoint(0, 0),
                        _factory.CreatePoint(10, 0),
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(0, 10))
                            .Shell.Coordinates,
                    _factory.CreatePolygon(
                        _factory.CreatePoint(15, 0),
                        _factory.CreatePoint(20, 0),
                        _factory.CreatePoint(20, 5),
                        _factory.CreatePoint(15, 5))
                            .Shell.Coordinates
                });

            Assert.IsEmpty(intersections);


            // multiple polygons, multiple intersections in the same coordinate
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    _factory.CreatePolygon(
                        _factory.CreatePoint(0, 0),
                        _factory.CreatePoint(10, 0),
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(0, 10))
                            .Shell.Coordinates,
                    _factory.CreatePolygon(
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(20, 10),
                        _factory.CreatePoint(20, 20),
                        _factory.CreatePoint(10, 20))
                            .Shell.Coordinates
                });

            Assert.AreEqual(new[]
            {
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
            }, intersections);


            // multiple polygons, multiple intersections

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    _factory.CreatePolygon(
                        _factory.CreatePoint(0, 0),
                        _factory.CreatePoint(10, 0),
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(0, 10))
                            .Shell.Coordinates,
                    _factory.CreatePolygon(
                        _factory.CreatePoint(5, 5),
                        _factory.CreatePoint(15, 5),
                        _factory.CreatePoint(15, 15),
                        _factory.CreatePoint(5, 15))
                            .Shell.Coordinates
                });

            Assert.AreEqual(new[]
            {
                new Coordinate(5, 10),
                new Coordinate(10, 5),
            }, intersections);


            // multiple polygons, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    _factory.CreatePolygon(
                        _factory.CreatePoint(0, 0),
                        _factory.CreatePoint(10, 0),
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(0, 10))
                            .Shell.Coordinates,
                    _factory.CreatePolygon(
                        _factory.CreatePoint(-10, -5),
                        _factory.CreatePoint(10, -5),
                        _factory.CreatePoint(0, 5))
                            .Shell.Coordinates
                });

            Assert.AreEqual(new[]
            {
                new Coordinate(0, 5),
                new Coordinate(0, 5),
                new Coordinate(5, 0),
            }, intersections);


            // multiple polygons, multiple intersections (even in the same coordinate)

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(8, 0),
                    _factory.CreatePoint(8, 8),
                    _factory.CreatePoint(0, 8))
                    .Shell.Coordinates,
                _factory.CreatePolygon(
                    _factory.CreatePoint(8, 0),
                    _factory.CreatePoint(14, 0),
                    _factory.CreatePoint(14, 8),
                    _factory.CreatePoint(8, 8))
                    .Shell.Coordinates,
            });

            Assert.AreEqual(new[]
            {
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
            }, intersections);


            // the previous scenario with one less coordinate

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(8, 0),
                    _factory.CreatePoint(8, 8),
                    _factory.CreatePoint(0, 8))
                    .Shell.Coordinates,
                _factory.CreatePolygon(
                    _factory.CreatePoint(8, 0),
                    _factory.CreatePoint(14, 0),
                    _factory.CreatePoint(8, 8))
                    .Shell.Coordinates,
            });

            Assert.AreEqual(new[]
            {
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
            }, intersections);


            // the previous scenario with an additional polygon

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(8, 0),
                    _factory.CreatePoint(8, 8),
                    _factory.CreatePoint(0, 8))
                    .Shell.Coordinates,
                _factory.CreatePolygon(
                    _factory.CreatePoint(8, 0),
                    _factory.CreatePoint(14, 0),
                    _factory.CreatePoint(8, 8))
                    .Shell.Coordinates,
                _factory.CreatePolygon(
                    _factory.CreatePoint(-2, 6),
                    _factory.CreatePoint(14, 6),
                    _factory.CreatePoint(4, 14))
                    .Shell.Coordinates
            });

            Assert.AreEqual(new[]
            {
                new Coordinate(0, 6),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 6),
                new Coordinate(8, 6),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(9.5, 6),
            }, intersections);


            // the previous scenario with additonal polygons

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(8, 0),
                    _factory.CreatePoint(8, 8),
                    _factory.CreatePoint(0, 8))
                    .Shell.Coordinates,
                _factory.CreatePolygon(
                    _factory.CreatePoint(8, 0),
                    _factory.CreatePoint(14, 0),
                    _factory.CreatePoint(8, 8))
                    .Shell.Coordinates,
                _factory.CreatePolygon(
                    _factory.CreatePoint(-4, -6),
                    _factory.CreatePoint(4, -6),
                    _factory.CreatePoint(4, 2),
                    _factory.CreatePoint(-4, 2))
                    .Shell.Coordinates,
                _factory.CreatePolygon(
                    _factory.CreatePoint(-2, 6),
                    _factory.CreatePoint(14, 6),
                    _factory.CreatePoint(4, 14))
                    .Shell.Coordinates,
                _factory.CreatePolygon(
                    _factory.CreatePoint(2, 1),
                    _factory.CreatePoint(6, 1),
                    _factory.CreatePoint(6, 13),
                    _factory.CreatePoint(2, 13))
                    .Shell.Coordinates
            });

            Assert.AreEqual(new[]
            {
                new Coordinate(0, 2),
                new Coordinate(0, 6),
                new Coordinate(2, 2),
                new Coordinate(2, 6),
                new Coordinate(2, 8),
                new Coordinate(2, 11.33333),
                new Coordinate(3.25, 13),
                new Coordinate(4, 0),
                new Coordinate(4, 1),
                new Coordinate(5.25, 13),
                new Coordinate(6, 6),
                new Coordinate(6, 8),
                new Coordinate(6, 12.4),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 6),
                new Coordinate(8, 6),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(9.5, 6),
            }, intersections.Select(coordinate => RoundCoordinate(coordinate)));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Rounds the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The rounded coordinate.</returns>
        private Coordinate RoundCoordinate(Coordinate coordinate)
        {
            return new Coordinate(Math.Round(coordinate.X, 5), Math.Round(coordinate.Y, 5), Math.Round(coordinate.Z, 5));
        }

        #endregion
    }
}
