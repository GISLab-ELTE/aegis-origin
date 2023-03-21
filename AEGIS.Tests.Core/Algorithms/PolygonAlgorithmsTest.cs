// <copyright file="PolygonAlgorithmsTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Test fixture for the <see cref="PolygonAlgorithms" /> class.
    /// </summary>
    [TestFixture]
    public class PolygonAlgorithmsTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="Area" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsAreaTest()
        {
            // area of a rectangle in counterclockwise orientation
            List<Coordinate> shell = new List<Coordinate>
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(5, 100),
                new Coordinate(5, 5)
            };
            Assert.AreEqual(11875, PolygonAlgorithms.Area(shell));

            // area of a rectangle in counterclockwise orientation and double precision coordinates
            shell = new List<Coordinate>
            {
                new Coordinate(5.5, 5.5), new Coordinate(130.5, 5.5),
                new Coordinate(130.5, 100.5), new Coordinate(5.5, 100.5)
            };
            IBasicPolygon polygon = new BasicPolygon(shell, null);
            Assert.AreEqual(11875, PolygonAlgorithms.Area(polygon));

            // area of a square with a hole
            shell = new List<Coordinate>
            {
                new Coordinate(-5, 5), new Coordinate(5, 5),
                new Coordinate(5, 15), new Coordinate(-5, 15)
            };
            List<Coordinate> hole = new List<Coordinate>
            {
                new Coordinate(0, 0), new Coordinate(0, 2),
                new Coordinate(2, 2), new Coordinate(2, 0),
            };

            polygon = new BasicPolygon(shell, new[] { hole });
            Assert.AreEqual(96, PolygonAlgorithms.Area(polygon));

            // area of a regular hexagon in counterclockwise orientation
            shell = new List<Coordinate>
            {
                new Coordinate(-3, -5), new Coordinate(3, -5),
                new Coordinate(6, 0), new Coordinate(3, 5),
                new Coordinate(-3, 5), new Coordinate(-6, 0)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.AreEqual(90, PolygonAlgorithms.Area(polygon));

            // area of a concave polygon
            shell = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(1, 3),
                new Coordinate(5, 3), new Coordinate(7, 5),
                new Coordinate(7, 1)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.AreEqual(14, PolygonAlgorithms.Area(polygon));

            // area of a non-simple polygon
            shell = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(1, 3),
                new Coordinate(5, 3), new Coordinate(7, 5),
                new Coordinate(7, 3), new Coordinate(5, 3),
                new Coordinate(5, 1)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.AreEqual(10, PolygonAlgorithms.Area(polygon));

            // exceptions
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.Area((IBasicPolygon)null));
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.Area((List<Coordinate>)null));
        }

        /// <summary>
        /// Tests the <see cref="IsConvex" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsIsConvexTest()
        {
            // rectangle with counterclockwise orientation
            List<Coordinate> shell = new List<Coordinate>
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(5, 100)
            };
            IBasicPolygon polygon = new BasicPolygon(shell, null);
            Assert.IsTrue(PolygonAlgorithms.IsConvex(polygon));

            // rectangle with a hole
            shell = new List<Coordinate>
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(5, 100)
            };
            List<Coordinate> hole = new List<Coordinate>
            {
                new Coordinate(0, 0), new Coordinate(0, 2),
                new Coordinate(2, 2), new Coordinate(2, 0),
            };
            polygon = new BasicPolygon(shell, new[] { hole });
            Assert.IsFalse(PolygonAlgorithms.IsConvex(polygon));

            // non-convex polygon with counterclockwise orientation
            shell = new List<Coordinate>
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(70, 70),
                new Coordinate(5, 100)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsFalse(PolygonAlgorithms.IsConvex(polygon));

            // convex polygon with counterclockwise orientation
            shell = new List<Coordinate>
            {
                new Coordinate(6, 7), new Coordinate(7, 6),
                new Coordinate(9, 6), new Coordinate(9, 8),
                new Coordinate(8, 10), new Coordinate(7, 9)
            };
            Assert.IsTrue(PolygonAlgorithms.IsConvex(shell));

            // convex polygon with clockwise orientation
            shell = new List<Coordinate>
            {
                new Coordinate(0, 2), new Coordinate(2, 0),
                new Coordinate(3, -2), new Coordinate(1, -4),
                new Coordinate(-1, -4), new Coordinate(-3, -2),
                new Coordinate(-2, 0)
            };
            Assert.IsTrue(PolygonAlgorithms.IsConvex(shell));

            // convex polygon that contains collinear edges
            shell = new List<Coordinate>
            {
                new Coordinate(0, 2), new Coordinate(2, 0),
                new Coordinate(3, -2), new Coordinate(1, -4),
                new Coordinate(0, -4),
                new Coordinate(-1, -4), new Coordinate(-3, -2),
                new Coordinate(-2, 0)
            };
            Assert.IsTrue(PolygonAlgorithms.IsConvex(shell));

            // non-simple polygon
            shell = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(1, 3),
                new Coordinate(5, 3), new Coordinate(7, 5),
                new Coordinate(7, 3), new Coordinate(5, 3),
                new Coordinate(5, 1)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsFalse(PolygonAlgorithms.IsConvex(polygon));

            // exceptions
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.IsConvex((IBasicPolygon)null));
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.IsConvex((List<Coordinate>)null));
        }

        /// <summary>
        /// Tests the <see cref="IsSimple" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsIsSimpleTest()
        {
            // simple convex polygon without holes in clockwise orientation, containing collinear edges
            List<Coordinate> shell = new List<Coordinate>
            {
                new Coordinate(0, 2), new Coordinate(2, 0),
                new Coordinate(3, -2), new Coordinate(1, -4),
                new Coordinate(0, -4), new Coordinate(-1, -4), 
                new Coordinate(-3, -2), new Coordinate(-2, 0)
            };
            IBasicPolygon polygon = new BasicPolygon(shell, null);
            Assert.IsTrue(PolygonAlgorithms.IsSimple(polygon));

            // simple concave polygon without holes in clockwise orientation
            shell = new List<Coordinate>
            {
                new Coordinate(2, 2), new Coordinate(2, 5),
                new Coordinate(4, 5), new Coordinate(3, 3),
                new Coordinate(6, 3), new Coordinate(5, 5), 
                new Coordinate(7, 5), new Coordinate(7, 2)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsTrue(PolygonAlgorithms.IsSimple(polygon));

            // non-simple polygon without holes, with two intersecting edges
            shell = new List<Coordinate>
            {
                new Coordinate(2, 2), new Coordinate(7, 5),
                new Coordinate(2, 5), new Coordinate(7, 2)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsFalse(PolygonAlgorithms.IsSimple(polygon));

            // non-simple polygon with a hole
            shell = new List<Coordinate>
            {
                new Coordinate(2, 2), new Coordinate(7, 5),
                new Coordinate(2, 5), new Coordinate(7, 2)
            };
            List<Coordinate> hole = new List<Coordinate>
            {
                new Coordinate(4, 2), new Coordinate(5, 2),
                new Coordinate(5, 3), new Coordinate(4, 3)
            };
            polygon = new BasicPolygon(shell, new[] { hole });
            Assert.IsFalse(PolygonAlgorithms.IsSimple(polygon));

            // convex polygon with a hole
            shell = new List<Coordinate>
            {
                new Coordinate(10, 5), new Coordinate(11, 4),
                new Coordinate(16, 4), new Coordinate(17, 5),
                new Coordinate(17, 8), new Coordinate(16, 9),
                new Coordinate(11, 9), new Coordinate(10, 8)
            };
            hole = new List<Coordinate>
            {
                new Coordinate(13, 7), new Coordinate(13, 8),
                new Coordinate(14, 8), new Coordinate(14, 7)
            };
            polygon = new BasicPolygon(shell, new[] { hole });
            Assert.IsFalse(PolygonAlgorithms.IsSimple(polygon));

            // non-simple polygon with two intersecting edges
            shell = new List<Coordinate>
            {
                new Coordinate(5, 5), new Coordinate(3, 2), 
                new Coordinate(6, 9), new Coordinate(8, 2),
                new Coordinate(10, 5)              
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsFalse(PolygonAlgorithms.IsSimple(polygon));

            // non- simple polygon with three intersecting edges
            shell = new List<Coordinate>
            {
                new Coordinate(4, 2), new Coordinate(7, 1), 
                new Coordinate(4, 6), new Coordinate(2, 4),
                new Coordinate(8, 5), new Coordinate(9, 4),
                new Coordinate(4, 2)
            };
            Assert.IsFalse(PolygonAlgorithms.IsSimple(shell));

            // exceptions
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.IsSimple((IBasicPolygon)null));
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.IsSimple((List<Coordinate>)null));
        }

        /// <summary>
        /// Tests the <see cref="IsValid" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsIsValidTest()
        {
            // simple convex polygon without holes in clockwise orientation, containing collinear edges
            List<Coordinate> shell = new List<Coordinate>
            {
                new Coordinate(0, 2), new Coordinate(2, 0),
                new Coordinate(3, -2), new Coordinate(1, -4),
                new Coordinate(0, -4), new Coordinate(-1, -4), 
                new Coordinate(-3, -2), new Coordinate(-2, 0)
            };
            IBasicPolygon polygon = new BasicPolygon(shell, null);
            Assert.IsTrue(PolygonAlgorithms.IsValid(polygon));

            // rectangle in counterclockwise orientation
            shell = new List<Coordinate>
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(5, 100),
                new Coordinate(5, 5)
            };
            Assert.IsTrue(PolygonAlgorithms.IsValid(shell));

            // square with a hole
            shell = new List<Coordinate>
            {
                new Coordinate(-5, 5), new Coordinate(5, 5),
                new Coordinate(5, 15), new Coordinate(-5, 15)
            };
            List<Coordinate> hole = new List<Coordinate>
            {
                new Coordinate(0, 0), new Coordinate(0, 2),
                new Coordinate(2, 2), new Coordinate(2, 0),
            };
            polygon = new BasicPolygon(shell, new[] { hole });
            Assert.IsTrue(PolygonAlgorithms.IsValid(polygon));

            // two coordinates
            shell = new List<Coordinate>
            {
                new Coordinate(-5, 5), new Coordinate(5, 5)
            };
            Assert.IsFalse(PolygonAlgorithms.IsValid(shell));

            // simple concave polygon in counterclockwise orientation
            shell = new List<Coordinate>
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(70, 70),
                new Coordinate(5, 100)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsTrue(PolygonAlgorithms.IsValid(polygon));

            // simple concave polygon in clockwise orientation
            shell = new List<Coordinate>
            {
                new Coordinate(2, 2), new Coordinate(2, 5),
                new Coordinate(4, 5), new Coordinate(3, 3),
                new Coordinate(6, 3), new Coordinate(5, 5), 
                new Coordinate(7, 5), new Coordinate(7, 2)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsTrue(PolygonAlgorithms.IsValid(polygon));

            // triangle with a line inside
            shell = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(6, 1),
                new Coordinate(2, 6)
            };
            hole = new List<Coordinate>
            {
                new Coordinate(2, 2), new Coordinate(4, 2)
            };
            polygon = new BasicPolygon(shell, new[] { hole });
            Assert.IsFalse(PolygonAlgorithms.IsValid(polygon));

            // non-simple polygon
            shell = new List<Coordinate>
            {
                new Coordinate(2, 2), new Coordinate(7, 5),
                new Coordinate(2, 5), new Coordinate(7, 2)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsFalse(PolygonAlgorithms.IsValid(polygon));

            // coordinate present more than once
            shell = new List<Coordinate>
            {
                new Coordinate(2, 2), new Coordinate(2, 5),
                new Coordinate(4, 5), new Coordinate(3, 3),
                new Coordinate(6, 3), new Coordinate(5, 5), 
                new Coordinate(7, 5), new Coordinate(7, 2),
                new Coordinate(7, 2)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsFalse(PolygonAlgorithms.IsValid(polygon));

            // not ordered coordinates
            shell = new List<Coordinate>
            {
                new Coordinate(2, 2), new Coordinate(2, 5),
                new Coordinate(3, 3), new Coordinate(7, 5),
                new Coordinate(6, 3), new Coordinate(5, 5), 
                new Coordinate(7, 2), new Coordinate(4, 5),
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsFalse(PolygonAlgorithms.IsValid(polygon));

            // different Z coordinates
            shell = new List<Coordinate>
            {
                new Coordinate(5, 5, 1), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(70, 70),
                new Coordinate(5, 100)
            };
            polygon = new BasicPolygon(shell, null);
            Assert.IsFalse(PolygonAlgorithms.IsValid(polygon));

            // exceptions
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.IsValid((IBasicPolygon)null));
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.IsValid((List<Coordinate>)null));
        }

        /// <summary>
        /// Tests the <see cref="Orientation" /> method.
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
        /// Tests the <see cref="SignedArea" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsSingedAreaTest()
        {
            // signed area of a rectangle with coordinates given in counterclockwise order
            List<Coordinate> shell = new List<Coordinate>
            {
                new Coordinate(5.5, 5.5), new Coordinate(130.5, 5.5),
                new Coordinate(130.5, 100.5), new Coordinate(5.5, 100.5)
            };
            BasicPolygon polygon = new BasicPolygon(shell, null);
            Assert.AreEqual(-11875, PolygonAlgorithms.SignedArea(polygon));

            // signed area of a triangle with coordinates given in clockwise order
            shell = new List<Coordinate>
            {
                new Coordinate(3, 3), new Coordinate(6, 1),
                new Coordinate(2, 1), new Coordinate(3, 3)
            };
            Assert.AreEqual(Math.Round(3.98), Math.Round(PolygonAlgorithms.SignedArea(shell)));

            // exceptions
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.SignedArea((List<Coordinate>)null));
            Assert.Throws<ArgumentNullException>(() => PolygonAlgorithms.SignedArea((IBasicPolygon)null));
        }

        #endregion
    }
}
