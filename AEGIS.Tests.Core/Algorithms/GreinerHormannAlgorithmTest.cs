/// <copyright file="GreinerHormannAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Geometry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="GreinerHormannAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class GreinerHormannAlgorithmTest
    {
        #region Private types

        /// <summary>
        /// Defines a default comparer class for <see cref="CoordinateRing"/> instances.
        /// </summary>
        private class CoordinateRingComparer : IEqualityComparer<CoordinateRing>
        {
            #region Static fields

            /// <summary>
            /// A singleton instance of the <see cref="CoordinateRingComparer"/> class.
            /// </summary>
            private static readonly CoordinateRingComparer _instance = new CoordinateRingComparer();

            #endregion

            #region Static properties

            /// <summary>
            /// Gets the singleton instance of the <see cref="CoordinateRingComparer"/> class.
            /// </summary>
            /// <value>The instance.</value>
            public static CoordinateRingComparer Instance { get { return _instance; } }

            #endregion

            #region Constructors

            /// <summary>
            /// Prevents a default instance of the <see cref="CoordinateRingComparer"/> class from being created.
            /// </summary>
            private CoordinateRingComparer() { }

            #endregion

            #region IEqualityComparer methods

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first <see cref="CoordinateRing"/> compare.</param>
            /// <param name="y">The second <see cref="CoordinateRing"/> to compare.</param>
            /// <returns>true if the specified objects are equal; otherwise, false.</returns>
            public Boolean Equals(CoordinateRing x, CoordinateRing y)
            {
                return x.Equals(y);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The <see cref="CoordinateRing"/> for which a hash code is to be returned.</param>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public Int32 GetHashCode(CoordinateRing obj)
            {
                return obj.GetHashCode();
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// Stores a geometry factory.
        /// </summary>
        private GeometryFactory _factory;

        #endregion

        #region Test setup

        /// <summary>
        /// Test fixture setup.
        /// </summary>
        [TestFixtureSetUp]
        public void FixtureInitialize()
        {
            _factory = new GeometryFactory();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test cases for non-intersecting polygon shells without holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmNoIntersectionTest()
        {
            // distinct polygons
            var algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(10, 0),
                        new Coordinate(10, 10),
                        new Coordinate(0, 10),
                        new Coordinate(0, 0)
                    },
                new List<Coordinate>
                    {
                        new Coordinate(20, 0),
                        new Coordinate(30, 0),
                        new Coordinate(30, 10),
                        new Coordinate(20, 10),
                        new Coordinate(20, 0)
                    });

            Assert.IsEmpty(algorithm.InternalPolygons);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(20, 0),
                    new Coordinate(30, 0),
                    new Coordinate(30, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // containing polygons
            algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(30, 0),
                        new Coordinate(30, 30),
                        new Coordinate(0, 30),
                        new Coordinate(0, 0)
                    },
                new List<Coordinate>
                    {
                        new Coordinate(10, 10),
                        new Coordinate(20, 10),
                        new Coordinate(20, 20),
                        new Coordinate(10, 20),
                        new Coordinate(10, 10)
                    });

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 20),
                    new Coordinate(10, 20),
                    new Coordinate(10, 10)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(30, 0),
                    new Coordinate(30, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(10, 10),
                        new Coordinate(20, 10),
                        new Coordinate(20, 20),
                        new Coordinate(10, 20),
                        new Coordinate(10, 10)
                    }
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.IsEmpty(algorithm.ExternalSecondPolygons);
        }

        /// <summary>
        /// Test cases for tangential, but non-intersecting polygon shells without holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmTangentialTest()
        {
            // tangent polygons without any mid-intersection point on edge
            GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(10, 0),
                        new Coordinate(10, 10),
                        new Coordinate(0, 10),
                        new Coordinate(0, 0)
                    },
                new List<Coordinate>
                    {
                        new Coordinate(10, 0),
                        new Coordinate(20, 0),
                        new Coordinate(20, 10),
                        new Coordinate(10, 10),
                        new Coordinate(10, 0)
                    });

            Assert.IsEmpty(algorithm.InternalPolygons);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // tangent polygons with mid-intersection point on edge
            algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(10, 0),
                        new Coordinate(10, 10),
                        new Coordinate(0, 10),
                        new Coordinate(0, 0)
                    },
                new List<Coordinate>
                    {
                        new Coordinate(10, 0),
                        new Coordinate(20, 0),
                        new Coordinate(20, 20),
                        new Coordinate(10, 20),
                        new Coordinate(10, 0)
                    });

            Assert.IsEmpty(algorithm.InternalPolygons);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 20),
                    new Coordinate(10, 20),
                    new Coordinate(10, 10),
                    new Coordinate(10, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // tangent and containing polygons (touch from the inner boundary)
            algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(10, 0),
                        new Coordinate(10, 10),
                        new Coordinate(0, 10),
                        new Coordinate(0, 0)
                    },
                new List<Coordinate>
                    {
                        new Coordinate(6, 6),
                        new Coordinate(10, 6),
                        new Coordinate(10, 10),
                        new Coordinate(6, 10),
                        new Coordinate(6, 6)
                    });

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(6, 6),
                        new Coordinate(10, 6),
                        new Coordinate(10, 10),
                        new Coordinate(6, 10),
                        new Coordinate(6, 6)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 6),
                    new Coordinate(6, 6),
                    new Coordinate(6, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.IsEmpty(algorithm.ExternalSecondPolygons);
        }

        /// <summary>
        /// Tests cases for intersecting convex polygons without holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmConvexTest()
        {
            // single intersection of convex polygons
            var algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(10, 0),
                        new Coordinate(10, 10),
                        new Coordinate(0, 10),
                        new Coordinate(0, 0)
                    },
                new List<Coordinate>
                    {
                        new Coordinate(5, 5),
                        new Coordinate(15, 5),
                        new Coordinate(15, 15),
                        new Coordinate(5, 15),
                        new Coordinate(5, 5)
                    });

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 5),
                    new Coordinate(10, 10),
                    new Coordinate(5, 10),
                    new Coordinate(5, 5),
                    new Coordinate(10, 5)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(5, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 5),
                    new Coordinate(5, 5),
                    new Coordinate(5, 10)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 5),
                    new Coordinate(15, 5),
                    new Coordinate(15, 15),
                    new Coordinate(5, 15),
                    new Coordinate(5, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 5)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));
        }

        /// <summary>
        /// Tests cases for intersecting concave polygons without holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmConcaveTest()
        {
            // intersection of a convex and a concave polygon, single intersection
            var algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(50, 0),
                        new Coordinate(50, 30),
                        new Coordinate(0, 30),
                        new Coordinate(0, 0)
                    },
                new List<Coordinate>
                    {
                        new Coordinate(10, -10),
                        new Coordinate(20, 10),
                        new Coordinate(30, 10),
                        new Coordinate(40, -10),
                        new Coordinate(60, 20),
                        new Coordinate(20, 40),
                        new Coordinate(10, -10)
                    });

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(12, 0),
                    new Coordinate(15, 0),
                    new Coordinate(20, 10),
                    new Coordinate(30, 10),
                    new Coordinate(35, 0),
                    new Coordinate(46.66667, 0),
                    new Coordinate(50, 5),
                    new Coordinate(50, 25),
                    new Coordinate(40, 30),
                    new Coordinate(18, 30),
                    new Coordinate(12, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)
                                           .Select(list => list.Select(coordinate => RoundCoordinate(coordinate)))))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(15, 0),
                    new Coordinate(35, 0),
                    new Coordinate(30, 10),
                    new Coordinate(20, 10),
                    new Coordinate(15, 0)
                },
                new[]
                {
                    new Coordinate(18, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0),
                    new Coordinate(12, 0),
                    new Coordinate(18, 30)
                },
                new[]
                {
                    new Coordinate(46.66667, 0),
                    new Coordinate(50, 0),
                    new Coordinate(50, 5),
                    new Coordinate(46.66667, 0)
                },
                new[]
                {
                    new Coordinate(50, 25),
                    new Coordinate(50, 30),
                    new Coordinate(40, 30),
                    new Coordinate(50, 25)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)
                                           .Select(list => list.Select(coordinate => RoundCoordinate(coordinate)))))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(12, 0),
                    new Coordinate(10, -10),
                    new Coordinate(15, 0),
                    new Coordinate(12, 0)
                },
                new[]
                {
                    new Coordinate(35, 0),
                    new Coordinate(40, -10),
                    new Coordinate(46.66667, 0),
                    new Coordinate(35, 0)
                },
                new[]
                {
                    new Coordinate(40, 30),
                    new Coordinate(20, 40),
                    new Coordinate(18, 30),
                    new Coordinate(40, 30)
                },
                new[]
                {
                    new Coordinate(50, 5),
                    new Coordinate(60, 20),
                    new Coordinate(50, 25),
                    new Coordinate(50, 5)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)
                                           .Select(list => list.Select(coordinate => RoundCoordinate(coordinate)))))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // intersection of a convex and a concave polygon, multiple intersections
            algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(60, 0),
                        new Coordinate(60, 30),
                        new Coordinate(0, 30),
                        new Coordinate(0, 0)
                    },
                new List<Coordinate>
                    {
                        new Coordinate(10, 40),
                        new Coordinate(10, -10),
                        new Coordinate(20, 40),
                        new Coordinate(30, -10),
                        new Coordinate(40, 40),
                        new Coordinate(50, -10),
                        new Coordinate(50, 50),
                        new Coordinate(20, 50),
                        new Coordinate(10, 40)
                    });

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(12, 0),
                    new Coordinate(18, 30),
                    new Coordinate(10, 30),
                    new Coordinate(10, 0)
                },
                new[]
                {
                    new Coordinate(28, 0),
                    new Coordinate(32, 0),
                    new Coordinate(38, 30),
                    new Coordinate(22, 30),
                    new Coordinate(28, 0)
                },
                new[]
                {
                    new Coordinate(48, 0),
                    new Coordinate(50, 0),
                    new Coordinate(50, 30),
                    new Coordinate(42, 30),
                    new Coordinate(48, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 30)
                },
                new[]
                {
                    new Coordinate(12, 0),
                    new Coordinate(28, 0),
                    new Coordinate(22, 30),
                    new Coordinate(18, 30),
                    new Coordinate(12, 0)
                },
                new[]
                {
                    new Coordinate(32, 0),
                    new Coordinate(48, 0),
                    new Coordinate(42, 30),
                    new Coordinate(38, 30),
                    new Coordinate(32, 0)
                },
                new[]
                {
                    new Coordinate(50, 0),
                    new Coordinate(60, 0),
                    new Coordinate(60, 30),
                    new Coordinate(50, 30),
                    new Coordinate(50, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(10, -10),
                    new Coordinate(12, 0),
                    new Coordinate(10, 0)
                },
                new[]
                {
                    new Coordinate(18, 30),
                    new Coordinate(20, 40),
                    new Coordinate(22, 30),
                    new Coordinate(38, 30),
                    new Coordinate(40, 40),
                    new Coordinate(42, 30),
                    new Coordinate(50, 30),
                    new Coordinate(50, 50),
                    new Coordinate(20, 50),
                    new Coordinate(10, 40),
                    new Coordinate(10, 30),
                    new Coordinate(18, 30)
                },
                new[]
                {
                    new Coordinate(28, 0),
                    new Coordinate(30, -10),
                    new Coordinate(32, 0),
                    new Coordinate(28, 0)
                },
                new[]
                {
                    new Coordinate(48, 0),
                    new Coordinate(50, -10),
                    new Coordinate(50, 0),
                    new Coordinate(48, 0)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));
        }


        /// <summary>
        /// Tests for polygon intersection with holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmHoleTest()
        {
            // hole contained in internal part
            List<Coordinate> shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0)
            };

            List<Coordinate>[] holesA = new List<Coordinate>[]
            {
                new List<Coordinate>
                {
                    new Coordinate(5, 5),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 5),
                    new Coordinate(5, 5)
                }
            };

            List<Coordinate> shellB = new List<Coordinate>
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4)
            };

            List<Coordinate>[] holesB;

            var algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(8, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 4),
                    new Coordinate(8, 4)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(5, 5),
                        new Coordinate(5, 7),
                        new Coordinate(7, 7),
                        new Coordinate(7, 5),
                        new Coordinate(5, 5)
                    }
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(4, 4),
                    new Coordinate(4, 8)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(12, 4),
                    new Coordinate(12, 12),
                    new Coordinate(4, 12),
                    new Coordinate(4, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 4)
                },
                new[]
                {
                    new Coordinate(5, 5),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 5),
                    new Coordinate(5, 5)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // hole overlapping internal and external part
            shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0)
            };

            holesA = new List<Coordinate>[]
            {
                new List<Coordinate>
                {
                    new Coordinate(5, 1),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 1),
                    new Coordinate(5, 1)
                },
            };

            shellB = new List<Coordinate>
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4)
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(8, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 4),
                    new Coordinate(5, 4),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 4),
                    new Coordinate(8, 4)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(7, 4),
                    new Coordinate(5, 4),
                    new Coordinate(4, 4),
                    new Coordinate(4, 8)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(5, 1),
                        new Coordinate(5, 4),
                        new Coordinate(7, 4),
                        new Coordinate(7, 1),
                        new Coordinate(5, 1)
                    }
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(12, 4),
                    new Coordinate(12, 12),
                    new Coordinate(4, 12),
                    new Coordinate(4, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 4)
                },
                new[]
                {
                    new Coordinate(5, 4),
                    new Coordinate(7, 4),
                    new Coordinate(7, 7),
                    new Coordinate(5, 7),
                    new Coordinate(5, 4)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // hole contained in internal part touching side of an external part
            shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0)
            };

            holesA = new List<Coordinate>[]
            {
                new List<Coordinate>
                {
                    new Coordinate(5, 4),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 4),
                    new Coordinate(5, 4)
                },
            };

            shellB = new List<Coordinate>
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4)
            };


            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(8, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 4),
                    new Coordinate(5, 4),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 4),
                    new Coordinate(8, 4)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(7, 4),
                    new Coordinate(5, 4),
                    new Coordinate(4, 4),
                    new Coordinate(4, 8)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(12, 4),
                    new Coordinate(12, 12),
                    new Coordinate(4, 12),
                    new Coordinate(4, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 4)
                },
                new[]
                {
                    new Coordinate(5, 4),
                    new Coordinate(7, 4),
                    new Coordinate(7, 7),
                    new Coordinate(5, 7),
                    new Coordinate(5, 4)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));

            // polygon shell intersected with a ring like polygon: shell with shell and shell with hole intersections
            /* TODO: skipped because of flaws in the preision model
            shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(6, 0),
                new Coordinate(9, 4),
                new Coordinate(3, 10),
                new Coordinate(-3, 4),
                new Coordinate(0, 0)
            };

            holesA = new List<Coordinate>[]
            {
                new List<Coordinate>
                {
                    new Coordinate(0, 1),
                    new Coordinate(-2, 4),
                    new Coordinate(3, 9),
                    new Coordinate(8, 4),
                    new Coordinate(6, 1),
                    new Coordinate(0, 1)
                }
            };

            shellB = new List<Coordinate>
            {
                new Coordinate(-5, 2),
                new Coordinate(11, 2),
                new Coordinate(11, 6),
                new Coordinate(3, 6),
                new Coordinate(0, 10),
                new Coordinate(-5, 5),
                new Coordinate(-5, 2)
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null,
                precisionModel: new PrecisionModel(0.0001));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(-3, 4),
                    new Coordinate(-1.5, 2),
                    new Coordinate(-0.6667, 2),
                    new Coordinate(-2, 4),
                    new Coordinate(1.7143, 7.7143),
                    new Coordinate(1.2857, 8.2857),
                    new Coordinate(-3, 4),
                },
                new[]
                {
                    new Coordinate(9, 4),
                    new Coordinate(7, 6),
                    new Coordinate(6, 6),
                    new Coordinate(8, 4),
                    new Coordinate(6.6667, 2),
                    new Coordinate(7.5, 2),
                    new Coordinate(9, 4),
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(6, 0),
                    new Coordinate(7.5, 2),
                    new Coordinate(-1.5, 2),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(3, 6),
                    new Coordinate(7, 6),
                    new Coordinate(3, 10),
                    new Coordinate(1.2857, 8.2857),
                    new Coordinate(3, 6),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(0, 1),
                        new Coordinate(-0.6667, 2),
                        new Coordinate(6.6667, 2),
                        new Coordinate(6, 1),
                        new Coordinate(0, 1),
                    }
                },
                new[]
                {
                    new[]
                    {
                        new Coordinate(3, 6),
                        new Coordinate(1.7143, 7.7143),
                        new Coordinate(3, 9),
                        new Coordinate(6, 6),
                        new Coordinate(3, 6),
                    }
                },
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(-5, 2),
                    new Coordinate(-1.5, 2),
                    new Coordinate(-3, 4),
                    new Coordinate(-2, 4),
                    new Coordinate(1.2857, 8.2857),
                    new Coordinate(0, 10),
                    new Coordinate(-5, 5),
                    new Coordinate(-5, 2),
                },
                new[]
                {
                    new Coordinate(7.5, 2),
                    new Coordinate(11, 2),
                    new Coordinate(11, 6),
                    new Coordinate(7, 6),
                    new Coordinate(9, 4),
                    new Coordinate(7.5, 2),
                },
                new[]
                {
                    new Coordinate(-2, 4),
                    new Coordinate(-0.6667, 2),
                    new Coordinate(6.6667, 2),
                    new Coordinate(8, 4),
                    new Coordinate(6, 6),
                    new Coordinate(3, 6),
                    new Coordinate(1.7143, 7.7143),
                    new Coordinate(-2, 4),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));
            */

            // shell with shell and hole with hole intersections
            shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(12, 0),
                new Coordinate(12, 12),
                new Coordinate(0, 12),
                new Coordinate(0, 0)
            };

            holesA = new List<Coordinate>[]
            {
                new List<Coordinate>
                {
                    new Coordinate(4, 8),
                    new Coordinate(4, 10),
                    new Coordinate(8, 10),
                    new Coordinate(8, 8),
                    new Coordinate(4, 8)
                }
            };

            shellB = new List<Coordinate>
            {
                new Coordinate(2, 6),
                new Coordinate(10, 6),
                new Coordinate(10, 18),
                new Coordinate(2, 18),
                new Coordinate(2, 6)
            };

            holesB = new List<Coordinate>[]
            {
                new List<Coordinate>
                {
                    new Coordinate(3, 9),
                    new Coordinate(3, 11),
                    new Coordinate(9, 11),
                    new Coordinate(9, 9),
                    new Coordinate(3, 9),
                }
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, holesB);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 12),
                    new Coordinate(2, 12),
                    new Coordinate(2, 6),
                    new Coordinate(10, 6),
                    new Coordinate(10, 12)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(3, 9),
                        new Coordinate(3, 11),
                        new Coordinate(9, 11),
                        new Coordinate(9, 9),
                        new Coordinate(8, 9),
                        new Coordinate(4, 9),
                        new Coordinate(3, 9),
                    },
                    new[]
                    {
                        new Coordinate(4, 9),
                        new Coordinate(8, 9),
                        new Coordinate(8, 8),
                        new Coordinate(4, 8),
                        new Coordinate(4, 9)
                    }
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 12),
                    new Coordinate(0, 12),
                    new Coordinate(0, 0),
                    new Coordinate(12, 0),
                    new Coordinate(12, 12),
                    new Coordinate(10, 12),
                    new Coordinate(10, 6),
                    new Coordinate(2, 6),
                    new Coordinate(2, 12)
                },
                new[]
                {
                    new Coordinate(4, 9),
                    new Coordinate(4, 10),
                    new Coordinate(8, 10),
                    new Coordinate(8, 9),
                    new Coordinate(9, 9),
                    new Coordinate(9, 11),
                    new Coordinate(3, 11),
                    new Coordinate(3, 9),
                    new Coordinate(4, 9)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(10, 12),
                    new Coordinate(10, 18),
                    new Coordinate(2, 18),
                    new Coordinate(2, 12),
                    new Coordinate(10, 12)
                },
                new[]
                {
                    new Coordinate(8, 9),
                    new Coordinate(4, 9),
                    new Coordinate(4, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 9)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));

            // two ring like polygon intersects: shell with shell, shell with hole and hole with hole intersections
            shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0)
            };

            holesA = new List<Coordinate>[]
            {
                new List<Coordinate>
                {
                    new Coordinate(1, 1),
                    new Coordinate(1, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 1),
                    new Coordinate(1, 1)
                },
            };

            shellB = new List<Coordinate>
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4)
            };

            holesB = new List<Coordinate>[]
            {
                new List<Coordinate>
                {
                    new Coordinate(5, 5),
                    new Coordinate(5, 11),
                    new Coordinate(11, 11),
                    new Coordinate(11, 5),
                    new Coordinate(5, 5)
                },
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, holesB);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(5, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 7),
                    new Coordinate(5, 7),
                    new Coordinate(5, 8)
                },
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(8, 5),
                    new Coordinate(7, 5),
                    new Coordinate(7, 4),
                    new Coordinate(8, 4)
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(7, 4),
                    new Coordinate(4, 4),
                    new Coordinate(4, 7),
                    new Coordinate(4, 8),
                },
                new[]
                {
                    new Coordinate(8, 5),
                    new Coordinate(8, 8),
                    new Coordinate(5, 8),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 5),
                    new Coordinate(8, 5)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(1, 1),
                        new Coordinate(1, 7),
                        new Coordinate(4, 7),
                        new Coordinate(4, 4),
                        new Coordinate(7, 4),
                        new Coordinate(7, 1),
                        new Coordinate(1, 1)
                    }
                },
                new Coordinate[0][]
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(12, 4),
                    new Coordinate(12, 12),
                    new Coordinate(4, 12),
                    new Coordinate(4, 8),
                    new Coordinate(5, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 5),
                    new Coordinate(8, 4)
                },
                new[]
                {
                    new Coordinate(4, 7),
                    new Coordinate(4, 4),
                    new Coordinate(7, 4),
                    new Coordinate(7, 5),
                    new Coordinate(5, 5),
                    new Coordinate(5, 7),
                    new Coordinate(4, 7)
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(11, 11),
                        new Coordinate(11, 5),
                        new Coordinate(8, 5),
                        new Coordinate(8, 8),
                        new Coordinate(5, 8),
                        new Coordinate(5, 11),
                        new Coordinate(11, 11)
                    }
                },
                new Coordinate[0][]
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));


            // hole is completly filled with the other subject polygon
            IPolygon polygonA = _factory.CreatePolygon(
                new List<Coordinate>
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                },
                new List<Coordinate>[]
                {
                    new List<Coordinate>
                    {
                        new Coordinate(2, 2),
                        new Coordinate(2, 8),
                        new Coordinate(8, 8),
                        new Coordinate(8, 2),
                        new Coordinate(2, 2),
                    },
                });

            IPolygon polygonB = _factory.CreatePolygon(
                new List<Coordinate>
                {
                    new Coordinate(2, 2),
                    new Coordinate(8, 2),
                    new Coordinate(8, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 2),
                });

            Assert.IsTrue(polygonA.IsValid);
            Assert.IsTrue(polygonB.IsValid);

            algorithm = new GreinerHormannAlgorithm(polygonA, polygonB);

            Assert.IsEmpty(algorithm.InternalPolygons);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(2, 2),
                        new Coordinate(2, 8),
                        new Coordinate(8, 8),
                        new Coordinate(8, 2),
                        new Coordinate(2, 2),
                    }
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(8, 2),
                    new Coordinate(8, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 2),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // hole is partially filled with the other subject polygon
            polygonA = _factory.CreatePolygon(
                new List<Coordinate>
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                },
                new List<Coordinate>[]
                {
                    new List<Coordinate>
                    {
                        new Coordinate(2, 2),
                        new Coordinate(2, 8),
                        new Coordinate(8, 8),
                        new Coordinate(8, 2),
                        new Coordinate(2, 2),
                    },
                });

            polygonB = _factory.CreatePolygon(
                new List<Coordinate>
                {
                    new Coordinate(2, 2),
                    new Coordinate(8, 2),
                    new Coordinate(8, 8),
                    new Coordinate(2, 2),
                });

            Assert.IsTrue(polygonA.IsValid);
            Assert.IsTrue(polygonB.IsValid);

            algorithm = new GreinerHormannAlgorithm(polygonA, polygonB);

            Assert.IsEmpty(algorithm.InternalPolygons);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(2, 2),
                        new Coordinate(8, 8),
                        new Coordinate(8, 2),
                        new Coordinate(2, 2),
                    },
                    new[]
                    {
                        new Coordinate(2, 2),
                        new Coordinate(2, 8),
                        new Coordinate(8, 8),
                        new Coordinate(2, 2),
                    }
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(8, 2),
                    new Coordinate(8, 8),
                    new Coordinate(2, 2),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));

            
            // shell of the first polygon contains the second and the holes also contain each other
            polygonA = _factory.CreatePolygon(
                new List<Coordinate>
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                },
                new List<Coordinate>[]
                {
                    new List<Coordinate>
                    {
                        new Coordinate(2, 2),
                        new Coordinate(2, 8),
                        new Coordinate(8, 8),
                        new Coordinate(8, 2),
                        new Coordinate(2, 2),
                    },
                });

            polygonB = _factory.CreatePolygon(
                new List<Coordinate>
                {
                    new Coordinate(-2, -2),
                    new Coordinate(12, -2),
                    new Coordinate(12, 12),
                    new Coordinate(-2, 12),
                    new Coordinate(-2, -2),
                },
                new List<Coordinate>[]
                {
                    new List<Coordinate>
                    {
                        new Coordinate(2, 2),
                        new Coordinate(2, 8),
                        new Coordinate(8, 8),
                        new Coordinate(8, 2),
                        new Coordinate(2, 2),
                    },
                });

            Assert.IsTrue(polygonA.IsValid);
            Assert.IsTrue(polygonB.IsValid);

            algorithm = new GreinerHormannAlgorithm(polygonA, polygonB);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(2, 2),
                        new Coordinate(2, 8),
                        new Coordinate(8, 8),
                        new Coordinate(8, 2),
                        new Coordinate(2, 2),
                    }
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));

            Assert.IsEmpty(algorithm.ExternalFirstPolygons);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(-2, -2),
                    new Coordinate(12, -2),
                    new Coordinate(12, 12),
                    new Coordinate(-2, 12),
                    new Coordinate(-2, -2),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(0, 0),
                        new Coordinate(0, 10),
                        new Coordinate(10, 10),
                        new Coordinate(10, 0),
                        new Coordinate(0, 0),
                    }
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Holes)))
                  .Using(CoordinateRingComparer.Instance));
        }

        /// <summary>
        /// Tests for degenerate intersection of polygons.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmDegenerateTest()
        {
            // degenerate test: common edges and multiple exit points in a row
            var algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(8, 0),
                        new Coordinate(8, 8),
                        new Coordinate(0, 8),
                        new Coordinate(0, 2),
                        new Coordinate(4, 2),
                        new Coordinate(4, 0),
                        new Coordinate(8, 0),
                    },
                new List<Coordinate>
                    {
                        new Coordinate(4, 1),
                        new Coordinate(6, 1),
                        new Coordinate(6, 13),
                        new Coordinate(2, 13),
                        new Coordinate(2, 2),
                        new Coordinate(4, 2),
                        new Coordinate(4, 1),
                    });

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(4, 2),
                    new Coordinate(4, 1),
                    new Coordinate(6, 1),
                    new Coordinate(6, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 2),
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 2),
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                },
                new[]
                {
                    new Coordinate(4, 1),
                    new Coordinate(4, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 1),
                    new Coordinate(4, 1),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(6, 8),
                    new Coordinate(6, 13),
                    new Coordinate(2, 13),
                    new Coordinate(2, 8),
                    new Coordinate(6, 8),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // degenerate test: common edges and multiple entry points in a row
            algorithm = new GreinerHormannAlgorithm(
                new List<Coordinate>
                    {
                        new Coordinate(0, 0),
                        new Coordinate(8, 0),
                        new Coordinate(8, 8),
                        new Coordinate(6, 8),
                        new Coordinate(6, 6),
                        new Coordinate(2, 6),
                        new Coordinate(2, 8),
                        new Coordinate(0, 8),
                        new Coordinate(0, 0),
                    },
                new List<Coordinate>
                    {
                        new Coordinate(-2, 6),
                        new Coordinate(2, 6),
                        new Coordinate(2, 12),
                        new Coordinate(-2, 6),
                    });

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 6),
                    new Coordinate(2, 6),
                    new Coordinate(2, 8),
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(0, 6),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 6),
                    new Coordinate(2, 6),
                    new Coordinate(0, 6),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(2, 12),
                    new Coordinate(-2, 6),
                    new Coordinate(0, 6),
                    new Coordinate(0, 8),
                    new Coordinate(2, 8),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));


            // degenerate test: containment with common edges
            List<Coordinate> shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0),
            };

            List<Coordinate> shellB = new List<Coordinate>
            {
                new Coordinate(2, 0),
                new Coordinate(6, 0),
                new Coordinate(6, 8),
                new Coordinate(2, 8),
                new Coordinate(2, 0),
            };

            algorithm = new GreinerHormannAlgorithm(shellA, shellB);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 0),
                    new Coordinate(6, 0),
                    new Coordinate(6, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 0),
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(2, 0),
                    new Coordinate(2, 8),
                },
                new[]
                {
                    new Coordinate(6, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 0),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalFirstPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalFirstPolygons.SelectMany(polygon => polygon.Holes));

            Assert.IsEmpty(algorithm.ExternalSecondPolygons);


            // the previous test case, only the order of the parameters are swapped
            algorithm = new GreinerHormannAlgorithm(shellB, shellA);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(2, 0),
                    new Coordinate(6, 0),
                    new Coordinate(6, 8),
                    new Coordinate(2, 8),
                }
            }), Is.EqualTo(AsRing(algorithm.InternalPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes));

            Assert.IsEmpty(algorithm.ExternalFirstPolygons);

            Assert.That(AsRing(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(2, 0),
                    new Coordinate(2, 8),
                },
                new[]
                {
                    new Coordinate(6, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 0),
                }
            }), Is.EqualTo(AsRing(algorithm.ExternalSecondPolygons.Select(polygon => polygon.Shell)))
                  .Using(CoordinateRingComparer.Instance));
            Assert.IsEmpty(algorithm.ExternalSecondPolygons.SelectMany(polygon => polygon.Holes));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Converts a coordinate list into a <see cref="CoordinateRing"/>.
        /// </summary>
        /// <param name="value">The input coordinates.</param>
        /// <returns>The coordinate ring.</returns>
        private CoordinateRing AsRing(IEnumerable<Coordinate> value)
        {
            return new CoordinateRing(value);
        }

        /// <summary>
        /// Converts a sequence of coordinate lists into a sequence of <see cref="CoordinateRing"/>.
        /// </summary>
        /// <param name="value">The input coordinates.</param>
        /// <returns>The coordinate rings.</returns>
        private IEnumerable<CoordinateRing> AsRing(IEnumerable<IEnumerable<Coordinate>> value)
        {
            return value.Select(shell => new CoordinateRing(shell));
        }

        /// <summary>
        /// Converts a group of sequence of coordinate lists into a group of sequence of <see cref="CoordinateRing"/>.
        /// </summary>
        /// <param name="value">The input coordinates.</param>
        /// <returns>The coordinate rings.</returns>
        private IEnumerable<IEnumerable<CoordinateRing>> AsRing(IEnumerable<IEnumerable<IEnumerable<Coordinate>>> value)
        {
            return value.Select(group => group.Select(shell => new CoordinateRing(shell)));
        }

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
