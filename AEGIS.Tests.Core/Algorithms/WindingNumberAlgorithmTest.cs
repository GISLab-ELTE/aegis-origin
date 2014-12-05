/// <copyright file="WindingNumberAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>
/// <author>Máté Cserép</author>

using System.Collections.Generic;
using ELTE.AEGIS.Algorithms;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="WindingNumberAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class WindingNumberAlgorithmTest
    {
        /// <summary>
        /// Test case for the <see cref="IsInsidePolygon" /> method.
        /// </summary>
        [Test]
        public void WindingNumberAlgorithmIsInsidePolygonTest()
        {
            // simple convex polygon

            Coordinate[] shell = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };

            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(5, 5)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(10, 5)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(15, 5)));


            // simple convex polygon with negative coordinates

            shell = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, -5),
                new Coordinate(10, 15),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };

            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(5, 5)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(5, 0)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(5, 10)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(-5, 0)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(-5, 10)));


            // convex polygon with holes

            shell = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(20, 0),
                new Coordinate(20, 20),
                new Coordinate(0, 20),
                new Coordinate(0, 0),
            };
            Coordinate[] hole1 = new Coordinate[]
            {
                new Coordinate(5, 5),
                new Coordinate(5, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 5),
                new Coordinate(5, 5),
            };
            Coordinate[] hole2 = new Coordinate[]
            {
                new Coordinate(10, 10),
                new Coordinate(5, 15),
                new Coordinate(15, 15),
                new Coordinate(15, 10),
                new Coordinate(10, 10),
            };

            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, new Coordinate(10, 10)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, new Coordinate(18, 8)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, new Coordinate(12, 2)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, new Coordinate(12, 12)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new List<Coordinate[]>() { hole1, hole2 }, new Coordinate(8, 8)));


            // concave polygon

            shell = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(15, 0),
                new Coordinate(15, 5),
                new Coordinate(12, 5),
                new Coordinate(12, 7),
                new Coordinate(10, 7),
                new Coordinate(10, 5),
                new Coordinate(5, 5),
                new Coordinate(5, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };

            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(3, 3)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(3, 5)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(3, 7)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(3, 12)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(11, 3)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(11, 5)));
            Assert.IsTrue(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(11, 6)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(11, 8)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(-5, 3)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(-5, 5)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(-5, 6)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(-5, 7)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(-5, 8)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(-5, 12)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(4, -1)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(5, -1)));
            Assert.IsFalse(WindingNumberAlgorithm.IsInsidePolygon(shell, new Coordinate(20, -1)));
        }

        /// <summary>
        /// Test case for the <see cref="IsOnBoundary" /> property.
        /// </summary>
        public void WindingNumberAlgorithmIsOnBoundaryTest()
        {
            // simple convex polygon

            Coordinate[] shell = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };

            WindingNumberAlgorithm algorithm = new WindingNumberAlgorithm(shell, new Coordinate(0, 5), true);
            algorithm.Compute();
            Assert.IsTrue(algorithm.IsOnBoundary.Value);

            algorithm.Coordinate = new Coordinate(10, 5);
            algorithm.Compute();
            Assert.IsTrue(algorithm.IsOnBoundary.Value);

            algorithm.Coordinate = new Coordinate(5, 0);
            algorithm.Compute();
            Assert.IsTrue(algorithm.IsOnBoundary.Value);

            algorithm.Coordinate = new Coordinate(10, 10);
            algorithm.Compute();
            Assert.IsTrue(algorithm.IsOnBoundary.Value);

            algorithm.Coordinate = new Coordinate(5, 5);
            algorithm.Compute();
            Assert.IsFalse(algorithm.IsOnBoundary.Value);

            algorithm.Coordinate = new Coordinate(15, 5);
            algorithm.Compute();
            Assert.IsFalse(algorithm.IsOnBoundary.Value);


            // simple convex polygon with negative coordinates

            shell = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, -5),
                new Coordinate(10, 15),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };

            algorithm = new WindingNumberAlgorithm(shell, new Coordinate(5, 12.5), true);
            algorithm.Compute();
            Assert.IsTrue(algorithm.IsOnBoundary.Value);

            algorithm.Coordinate = new Coordinate(5, 10);
            algorithm.Compute();
            Assert.IsFalse(algorithm.IsOnBoundary.Value);

            algorithm.Coordinate = new Coordinate(-5, 0);
            algorithm.Compute();
            Assert.IsFalse(algorithm.IsOnBoundary.Value);
        }
    }
}
