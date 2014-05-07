/// <copyright file="PolygonAlgorithmsTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Core.Algorithms.Spatial
{
    [TestFixture]
    public class PolygonAlgorithmsTest
    {
        [TestCase]
        public void PolygonAlgorithmsOrientationTest()
        {
            // test case 1: counter clockwise orientation
            List<Coordinate> coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10), 
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.CounterClockwise, PolygonAlgorithms.Orientation(coordinates));

            // test case 2: clockwise orientation
            coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(10, 10), new Coordinate(10, 0), 
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.Clockwise, PolygonAlgorithms.Orientation(coordinates));

            // test case 3: collinear orientation
            coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(0, 20), new Coordinate(0, 30),
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.Collinear, PolygonAlgorithms.Orientation(coordinates));

            // test case 4: concave polygon
            coordinates = new List<Coordinate> 
            { 
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(10, 10), new Coordinate(10, 0), 
                new Coordinate(10, 10), new Coordinate(0, 10), 
                new Coordinate(0, 0)
            };

            Assert.AreEqual(Orientation.Clockwise, PolygonAlgorithms.Orientation(coordinates));

            // test case 5: concave polygon
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
    }
}
