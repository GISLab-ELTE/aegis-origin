/// <copyright file="PolygonAlgorithmsTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Core.Algorithms.Spatial
{
    /// <summary>
    /// Test fixture for the <see cref="PolygonAlgorithms" /> class.
    /// </summary>
    [TestFixture]
    public class PolygonAlgorithmsTest
    {
        /// <summary>
        /// Test case for the <see cref="Orientation" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsOrientationTest()
        {
            // counter clockwise orientation convex polygon
            List<Coordinate> coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10), 
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.CounterClockwise, PolygonAlgorithms.Orientation(coordinates));

            // clockwise orientation convex polygon
            coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(10, 10), new Coordinate(10, 0), 
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.Clockwise, PolygonAlgorithms.Orientation(coordinates));

            // collinear orientation convex polygon
            coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(0, 20), new Coordinate(0, 30),
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.Collinear, PolygonAlgorithms.Orientation(coordinates));

            // clockwise concave polygon
            coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(10, 10), new Coordinate(10, 0), 
                new Coordinate(10, 10), new Coordinate(0, 10), 
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.Clockwise, PolygonAlgorithms.Orientation(coordinates));

            // counter clockwise concave polygon
            coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(2, 1), new Coordinate(8, 1), 
                new Coordinate(10, 0), new Coordinate(9, 2), new Coordinate(9, 8), 
                new Coordinate(10, 10), new Coordinate(8, 9), new Coordinate(2, 9), 
                new Coordinate(0, 10), new Coordinate(1, 8), new Coordinate(1, 2),
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.CounterClockwise, PolygonAlgorithms.Orientation(coordinates));
        }

        /// <summary>
        /// Test case for the containment methods.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsContainmentTest()
        {
            // simple polygon
            
            Coordinate[] shell = new Coordinate[]
            { 
                new Coordinate(0, 0),
                new Coordinate(20, 0),
                new Coordinate(15, 5),
                new Coordinate(20, 10), 
                new Coordinate(0, 20),
                new Coordinate(0, 0),
            };

            Coordinate coordinate = new Coordinate(-1, 7);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(0, 0);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(0, 5);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(1, 1);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(3, 3);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(5, 8);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(6, 0);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(8, 4);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(8, 22);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(12, -2);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(12, 8);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(15, 5);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(15, 9);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(16, 1);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));

            coordinate = new Coordinate(19, 3);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, coordinate));
        

            // polygon with holes
            
            shell = new Coordinate[]
            { 
                new Coordinate(0, 0),
                new Coordinate(20, 0),
                new Coordinate(15, 5),
                new Coordinate(20, 10), 
                new Coordinate(0, 20),
                new Coordinate(0, 0),
            };

            Coordinate[] hole1 = new Coordinate[]
            {
                new Coordinate(2, 2),
                new Coordinate(2, 4),
                new Coordinate(4, 4),
                new Coordinate(4, 2),
                new Coordinate(2, 2),
            };
            Coordinate[] hole2 = new Coordinate[]
            {
                new Coordinate(10, 7),
                new Coordinate(10, 9),
                new Coordinate(16, 9),
                new Coordinate(16, 7),
                new Coordinate(10, 7),
            };

            coordinate = new Coordinate(-1, 7);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(0, 0);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(0, 5);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(1, 1);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(3, 3);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(5, 8);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(6, 0);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(8, 4);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(8, 22);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(12, -2);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(12, 8);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(15, 5);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(15, 9);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(16, 1);
            Assert.IsTrue(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));

            coordinate = new Coordinate(19, 3);
            Assert.IsFalse(PolygonAlgorithms.InInterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsTrue(PolygonAlgorithms.InExterior(shell, new[] { hole1, hole2 }, coordinate));
            Assert.IsFalse(PolygonAlgorithms.OnBoundary(shell, new[] { hole1, hole2 }, coordinate));
        }
    }
}
