/// <copyright file="WeilerAthertonAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
    /// Test fixture for the <see cref="WeilerAthertonAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class WeilerAthertonAlgorithmTest
    {
        #region Test methods

        /// <summary>
        /// Test case for distinct polygons.
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmDistinctTest()
        {
            // Data
            var algorithm = new WeilerAthertonAlgorithm(
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

            // Verification
            Assert.IsEmpty(algorithm.Internal);

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                }
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(20, 0),
                    new Coordinate(30, 0),
                    new Coordinate(30, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 0)
                }
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for polygon cxontainign the other polygon.
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmContainTest()
        {
            // Data
            var algorithm = new WeilerAthertonAlgorithm(
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

            // Verification
            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 20),
                    new Coordinate(10, 20),
                    new Coordinate(10, 10)
                }
            }, algorithm.Internal.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.Internal.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(30, 0),
                    new Coordinate(30, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0)
                }
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.IsEmpty(algorithm.ExternalB);
        }

        /// <summary>
        /// Test case for tangent polygons.
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmTangentTest()
        {
            // Data
            WeilerAthertonAlgorithm algorithm = new WeilerAthertonAlgorithm(
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

            // Verification
            Assert.IsEmpty(algorithm.Internal);

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                }
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 0)
                }
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        

            // Data
            algorithm = new WeilerAthertonAlgorithm(
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

            // Verification
            Assert.IsEmpty(algorithm.Internal);

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                }
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for convex polygons.
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmConvexTest()
        {
            // Data
            var algorithm = new WeilerAthertonAlgorithm(
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

            // Verification
            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(10, 5),
                    new Coordinate(10, 10),
                    new Coordinate(5, 10),
                    new Coordinate(5, 5),
                    new Coordinate(10, 5)
                }
            }, algorithm.Internal.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.Internal.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for concave polygons.
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmConcaveTest()
        {
            // Data
            var algorithm = new WeilerAthertonAlgorithm(
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

            // Verification
            Assert.AreEqual(new[]
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
            }, algorithm.Internal.Select(clip => clip.Shell).Select(list => list.Select(coordinate => RoundCoordinate(coordinate))));
            Assert.IsEmpty(algorithm.Internal.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalA.Select(clip => clip.Shell).Select(list => list.Select(coordinate => RoundCoordinate(coordinate))));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalB.Select(clip => clip.Shell).Select(list => list.Select(coordinate => RoundCoordinate(coordinate))));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for multiple clips.
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmMultipleClipTest()
        {
            // Data
            var algorithm = new WeilerAthertonAlgorithm(
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

            // Verification
            Assert.AreEqual(new[]
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
            }, algorithm.Internal.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.Internal.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for polygon containing another polygon (with holes).
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmHoleContainTest()
        {
            // Data
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

            // Verification

            var algorithm = new WeilerAthertonAlgorithm(shellA, holesA, shellB, null);

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(8, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 4),
                    new Coordinate(8, 4)
                }
            }, algorithm.Internal.Select(clip => clip.Shell));
            Assert.AreEqual(new[]
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
            }, algorithm.Internal.Select(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for overlapping polygons (with holes).
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmHoleOverlapTest()
        {
            // Data
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
                    new Coordinate(5, 1),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 1),
                    new Coordinate(5, 1)
                },
            };

            List<Coordinate> shellB = new List<Coordinate>
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4)
            };

            WeilerAthertonAlgorithm algorithm = new WeilerAthertonAlgorithm(shellA, holesA, shellB, null);

            Assert.AreEqual(new[]
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
            }, algorithm.Internal.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.Internal.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(7, 4),
                    new Coordinate(7, 1),
                    new Coordinate(5, 1),
                    new Coordinate(5, 4),
                    new Coordinate(4, 4),
                    new Coordinate(4, 8)
                }
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for tangnet polygons (with holes).
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmHoleTangentTest()
        {
            // Data
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
                    new Coordinate(5, 4),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 4),
                    new Coordinate(5, 4)
                },
            };

            List<Coordinate> shellB = new List<Coordinate>
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4)
            };

            // Verification
            WeilerAthertonAlgorithm algorithm = new WeilerAthertonAlgorithm(shellA, holesA, shellB, null);

            Assert.AreEqual(new[]
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
            }, algorithm.Internal.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.Internal.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for polygons with holes.
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmHoleFrameTest()
        {
            // Data
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
                    new Coordinate(1, 1),
                    new Coordinate(1, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 1),
                    new Coordinate(1, 1)
                },
            };

            List<Coordinate> shellB = new List<Coordinate>
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4)
            };

            List<Coordinate>[] holesB = new List<Coordinate>[]
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

            // Verification
            WeilerAthertonAlgorithm algorithm = new WeilerAthertonAlgorithm(shellA, holesA, shellB, holesB);

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(8, 5),
                    new Coordinate(7, 5),
                    new Coordinate(7, 4),
                    new Coordinate(8, 4)
                },
                new[]
                {
                    new Coordinate(5, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 7),
                    new Coordinate(5, 7),
                    new Coordinate(5, 8)
                }
            }, algorithm.Internal.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.Internal.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(7, 4),
                    new Coordinate(7, 1),
                    new Coordinate(1, 1),
                    new Coordinate(1, 7),
                    new Coordinate(4, 7),
                    new Coordinate(4, 8)
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
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(12, 4),
                    new Coordinate(12, 12),
                    new Coordinate(4, 12),
                    new Coordinate(4, 8),
                    new Coordinate(5, 8),
                    new Coordinate(5, 11),
                    new Coordinate(11, 11),
                    new Coordinate(11, 5),
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
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for clips of polygons (with holes).
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmHoleClipTest()
        {
            // Data
            List<Coordinate> shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(6, 0),
                new Coordinate(9, 4),
                new Coordinate(3, 10),
                new Coordinate(-3, 4),
                new Coordinate(0, 0)
            };

            List<Coordinate>[] holesA = new List<Coordinate>[]
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

            List<Coordinate> shellB = new List<Coordinate>
            {
                new Coordinate(-5, 2),
                new Coordinate(11, 2),
                new Coordinate(11, 6),
                new Coordinate(3, 6),
                new Coordinate(0, 10),
                new Coordinate(-5, 5),
                new Coordinate(-5, 2)
            };

            // Verification
            WeilerAthertonAlgorithm algorithm = new WeilerAthertonAlgorithm(shellA, holesA, shellB, null);

            var internals = algorithm.Internal.Select(clip => clip.Shell).ToList();
            Assert.AreEqual(internals.Count, 2);
            Assert.AreEqual(internals[0].Count, 7);
            Assert.AreEqual(internals[1].Count, 7);
            Assert.IsEmpty(algorithm.Internal.SelectMany(clip => clip.Holes));

            var externalA = algorithm.ExternalA.Select(clip => clip.Shell).ToList();
            Assert.AreEqual(externalA.Count, 2);
            Assert.AreEqual(externalA[0].Count, 9);
            Assert.AreEqual(externalA[1].Count, 7);
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            var externalB = algorithm.ExternalB.Select(clip => clip.Shell).ToList();
            Assert.AreEqual(externalB.Count, 3);
            Assert.AreEqual(externalB[0].Count, 7);
            Assert.AreEqual(externalB[1].Count, 6);
            Assert.AreEqual(externalB[2].Count, 8);
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
        }

        /// <summary>
        /// Test case for intersection polygon holes.
        /// </summary>
        [Test]
        public void WeilerAthertonAlgorithmHoleIntersectTest()
        {
            List<Coordinate> shellA = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(12, 0),
                new Coordinate(12, 12),
                new Coordinate(0, 12),
                new Coordinate(0, 0)
            };

            List<Coordinate>[] holesA = new List<Coordinate>[]
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

            List<Coordinate> shellB = new List<Coordinate>
            {
                new Coordinate(2, 6),
                new Coordinate(10, 6),
                new Coordinate(10, 18),
                new Coordinate(2, 18),
                new Coordinate(2, 6)
            };

            List<Coordinate>[] holesB = new List<Coordinate>[]
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

            // Verification
            WeilerAthertonAlgorithm algorithm = new WeilerAthertonAlgorithm(shellA, holesA, shellB, holesB);

            Assert.AreEqual(new[]
            {
                new[]
                {
                    new Coordinate(10, 12),
                    new Coordinate(2, 12),
                    new Coordinate(2, 6),
                    new Coordinate(10, 6),
                    new Coordinate(10, 12)
                }
            }, algorithm.Internal.Select(clip => clip.Shell));
            Assert.AreEqual(new[]
            {
                new[]
                {
                    new[]
                    {
                        new Coordinate(8, 9),
                        new Coordinate(8, 10),
                        new Coordinate(4, 10),
                        new Coordinate(4, 9),
                        new Coordinate(8, 9)
                    }
                }
            }, algorithm.Internal.Select(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalA.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalA.SelectMany(clip => clip.Holes));

            Assert.AreEqual(new[]
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
            }, algorithm.ExternalB.Select(clip => clip.Shell));
            Assert.IsEmpty(algorithm.ExternalB.SelectMany(clip => clip.Holes));
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
