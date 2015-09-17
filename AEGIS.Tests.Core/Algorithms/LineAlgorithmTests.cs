/// <copyright file="LineAlgorithmsTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2015 Roberto Giachetta. Licensed under the
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
/// <authors>Gréta Bereczki, Roberto Giachetta</authors>

namespace ELTE.AEGIS.Tests.Algorithms
{
    using ELTE.AEGIS.Algorithms;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Test fixture for the <see cref="LineAlgorithms" /> class.
    /// </summary>
    [TestFixture]
    public class LineAlgorithmsTest
    {
        /// <summary>
        /// Tests the <see cref="Centroid" /> method.
        /// </summary>
        [Test]
        public void LineAlgorithmsCentroidTest()
        {
            // valid lines
            Assert.AreEqual(new Coordinate(4.5, 6.5), LineAlgorithms.Centroid(new Coordinate(2, 5), new Coordinate(7, 8)));
            Assert.AreEqual(new Coordinate(5, 51), LineAlgorithms.Centroid(new BasicLineString(new[] { new Coordinate(5, 2), new Coordinate(5, 100) })));


            // invalid line
            Assert.IsFalse(LineAlgorithms.Centroid(new Coordinate(Double.NaN, 5), new Coordinate(7, 8)).IsValid);


            // valid line string
            List<Coordinate> lineString = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(1, 3),
                new Coordinate(1, 7)
            };
            Assert.AreEqual(new Coordinate(1, 4), LineAlgorithms.Centroid(lineString));


            // valid lines with fixed precision
            PrecisionModel precision = new PrecisionModel(0.001);

            Assert.AreEqual(new Coordinate(22000, 19000), LineAlgorithms.Centroid(new Coordinate(23000, 16000), new Coordinate(20000, 22000), precision));
            Assert.AreEqual(new Coordinate(434898000, 461826000), LineAlgorithms.Centroid(new Coordinate(864325000, 96041000), new Coordinate(5470000, 827611000), precision));


            // valid lines at given fixed precision
            Assert.AreEqual(new Coordinate(22000, 19000), LineAlgorithms.Centroid(new Coordinate(23654, 16412), new Coordinate(19530, 22009), precision));
            Assert.AreEqual(new Coordinate(434898000, 461826000), LineAlgorithms.Centroid(new Coordinate(864325203, 96041202), new Coordinate(5470432, 827611534), precision));
        }

        /// <summary>
        /// Tests the <see cref="Coincides" /> method.
        /// </summary>
        [Test]
        public void LineAlgorithmsCoincidesTest()
        {
            // coinciding lines
            Coordinate firstCoordinate = new Coordinate(1, 1);
            CoordinateVector firstVector = new CoordinateVector(0, 2);
            Coordinate secondCoordinate = new Coordinate(1, 1);
            CoordinateVector secondVector = new CoordinateVector(0, 4);
            Assert.IsTrue(LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector));

            firstCoordinate = new Coordinate(2, 5);
            firstVector = new CoordinateVector(0, 1);
            secondCoordinate = new Coordinate(2, 4);
            secondVector = new CoordinateVector(0, 4);
            Assert.IsTrue(LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector));

            firstCoordinate = new Coordinate(15, 7);
            firstVector = new CoordinateVector(9, 5);
            secondCoordinate = new Coordinate(15, 7);
            secondVector = new CoordinateVector(9, 5);
            Assert.IsTrue(LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector));

            firstCoordinate = new Coordinate(2, 4);
            firstVector = new CoordinateVector(3, -1);
            secondCoordinate = new Coordinate(14, 0);
            secondVector = new CoordinateVector(3, -1);
            Assert.IsTrue(LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector));


            // not coinciding lines
            firstCoordinate = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondCoordinate = new Coordinate(1, 1);
            secondVector = new CoordinateVector(1, 0);
            Assert.IsFalse(LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector));

            firstCoordinate = new Coordinate(2, 5);
            firstVector = new CoordinateVector(0, 1);
            secondCoordinate = new Coordinate(2, 4);
            secondVector = new CoordinateVector(4, 0);
            Assert.IsFalse(LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector));
        }

        /// <summary>
        /// Tests the <see cref="Contains" /> method.
        /// </summary>
        [Test]
        public void LineAlgorithmsContainsTest()
        {
            // line contains point
            Coordinate lineStart = new Coordinate(0, 1);
            Coordinate lineEnd = new Coordinate(100, 1);
            Coordinate coordinate = new Coordinate(1, 1);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate));

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(2, 2);
            coordinate = new Coordinate(1, 1);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate));

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(-4, 2);
            coordinate = new Coordinate(-2, 1);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate));

            coordinate = new Coordinate(-1, 0.5);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate));


            // line does not contain point
            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(2, 2);
            coordinate = new Coordinate(1, 1.2);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineEnd, coordinate));

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(0, 200);
            coordinate = new Coordinate(0, 201);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineEnd, coordinate));

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(0, 200);
            coordinate = new Coordinate(0, 201);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineEnd, coordinate));


            // infinite line contains point
            lineStart = new Coordinate(2, 5);
            CoordinateVector lineVector = new CoordinateVector(0, 1);
            coordinate = new Coordinate(2, 4);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineVector, coordinate));

            coordinate = new Coordinate(2, 400);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineVector, coordinate));

            lineStart = new Coordinate(2, 4);
            lineVector = new CoordinateVector(-3, 1);
            coordinate = new Coordinate(14, 0);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineVector, coordinate));


            // infinite line does not contain point
            coordinate = new Coordinate(2.001, 400);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineVector, coordinate));

            lineStart = new Coordinate(1, 1);
            lineVector = new CoordinateVector(0, 2);
            coordinate = new Coordinate(0, 101);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineVector, coordinate));


            // finite line with fixed precision
            PrecisionModel precision = new PrecisionModel(0.001);
            
            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(2000, 2000);
            coordinate = new Coordinate(1000, 1000);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision));

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(-4000, 2000);
            coordinate = new Coordinate(-3000, 2000);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision));

            coordinate = new Coordinate(-3000, 1000);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision));


            // finite line at given fixed precision
            lineStart = new Coordinate(12, 201);
            lineEnd = new Coordinate(1870, 2390);
            coordinate = new Coordinate(980, 1110);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision));

            lineStart = new Coordinate(-20, 50);
            lineEnd = new Coordinate(-4102, 1960);
            coordinate = new Coordinate(-2111, 983);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision));

            lineStart = new Coordinate(-20, 50);
            lineEnd = new Coordinate(-4102, 1960);
            coordinate = new Coordinate(-2111, 983);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision));


            // infinite line with fixed precision
            lineStart = new Coordinate(1000, 1000);
            lineVector = new CoordinateVector(0, 2000);
            coordinate = new Coordinate(1000, 0);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision));

            coordinate = new Coordinate(0, 0);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision));

            coordinate = new Coordinate(2000, 0);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision));


            // infinite at given fixed precision
            lineStart = new Coordinate(1260, 1440);
            lineVector = new CoordinateVector(0, 2035);
            coordinate = new Coordinate(1230, 0);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision));

            coordinate = new Coordinate(1759, 0);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision));

            coordinate = new Coordinate(761, 0);
            Assert.IsTrue(LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision));

            coordinate = new Coordinate(759, 0);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision));

            coordinate = new Coordinate(1761, 0);
            Assert.IsFalse(LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision));
        }

        /// <summary>
        /// Tests the <see cref="Distance" /> method.
        /// </summary>
        [Test]
        public void LineAlgorithmsDistanceTest()
        {
            // distance of a line and a coordinate
            Coordinate lineStart = new Coordinate(0, 1);
            Coordinate lineEnd = new Coordinate(100, 1);
            Coordinate coordinate = new Coordinate(1, 1);
            Assert.AreEqual(0, LineAlgorithms.Distance(lineStart, lineEnd, coordinate));

            lineStart = new Coordinate(0, 1);
            lineEnd = new Coordinate(100, 1);
            coordinate = new Coordinate(1, 7);
            Assert.AreEqual(6, LineAlgorithms.Distance(lineStart, lineEnd, coordinate));

            lineStart = new Coordinate(5, 7);
            lineEnd = new Coordinate(5, 70);
            coordinate = new Coordinate(5, 6);
            Assert.AreEqual(1, LineAlgorithms.Distance(lineStart, lineEnd, coordinate));

            lineStart = new Coordinate(5, 7);
            lineEnd = new Coordinate(5, 70);
            coordinate = new Coordinate(4, 7);
            Assert.AreEqual(1, LineAlgorithms.Distance(lineStart, lineEnd, coordinate));

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(2, -3);
            coordinate = new Coordinate(2, 1);
            Assert.AreEqual(2.22, Math.Round(LineAlgorithms.Distance(lineStart, lineEnd, coordinate), 2));


            // distance of two lines
            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(100, 0);
            Coordinate secondLineStart = new Coordinate(0, 0);
            Coordinate secondLineEnd = new Coordinate(0, 100);
            Assert.AreEqual(0, LineAlgorithms.Distance(lineStart, lineEnd, secondLineStart, secondLineEnd));

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(100, 0);
            secondLineStart = new Coordinate(0, 1);
            secondLineEnd = new Coordinate(100, 1);
            Assert.AreEqual(1, LineAlgorithms.Distance(lineStart, lineEnd, secondLineStart, secondLineEnd));

            lineStart = new Coordinate(2, 4);
            lineEnd = new Coordinate(5, 3);
            secondLineStart = new Coordinate(6, 2);
            secondLineEnd = new Coordinate(6, 6);
            Assert.AreEqual(1, LineAlgorithms.Distance(lineStart, lineEnd, secondLineStart, secondLineEnd));

            lineStart = new Coordinate(1, 1);
            lineEnd = new Coordinate(1, 4);
            secondLineStart = new Coordinate(4, 2);
            secondLineEnd = new Coordinate(0, 2);
            Assert.AreEqual(0, LineAlgorithms.Distance(lineStart, lineEnd, secondLineStart, secondLineEnd));


            // distance of an infinite line and a point
            lineStart = new Coordinate(1, 1);
            CoordinateVector coordinateVector = new CoordinateVector(0, 1);
            coordinate = new Coordinate(2, 1500);
            Assert.AreEqual(1, LineAlgorithms.Distance(lineStart, coordinateVector, coordinate));

            lineStart = new Coordinate(1, 1);
            coordinateVector = new CoordinateVector(0, 1);
            coordinate = new Coordinate(1, 1500);
            Assert.AreEqual(0, LineAlgorithms.Distance(lineStart, coordinateVector, coordinate));

            lineStart = new Coordinate(1, 1);
            coordinateVector = new CoordinateVector(0, 1);
            coordinate = new Coordinate(0, -100);
            Assert.AreEqual(1, LineAlgorithms.Distance(lineStart, coordinateVector, coordinate));

            lineStart = new Coordinate(2, 4);
            coordinateVector = new CoordinateVector(-3, 1);
            coordinate = new Coordinate(14, 0);
            Assert.AreEqual(0, LineAlgorithms.Distance(lineStart, coordinateVector, coordinate));

            lineStart = new Coordinate(2, 4);
            coordinateVector = new CoordinateVector(-3, 1);
            coordinate = new Coordinate(5, 3);
            Assert.AreEqual(0, LineAlgorithms.Distance(lineStart, coordinateVector, coordinate));
        }

        /// <summary>
        /// Tests the <see cref="Intersects" /> method.
        /// </summary>
        [Test]
        public void LineAlgorithmsIntersectsTest()
        {
            // coinciding lines
            Coordinate firstLineStart = new Coordinate(1, 1);
            Coordinate firstLineEnd = new Coordinate(4, 1);
            Coordinate secondLineStart = new Coordinate(1, 1);
            Coordinate secondLineEnd = new Coordinate(4, 1);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // parallel lines
            firstLineStart = new Coordinate(1.3, 1.3);
            firstLineEnd = new Coordinate(1.3, 4.3);
            secondLineStart = new Coordinate(2.3, 1.3);
            secondLineEnd = new Coordinate(2.3, 4.3);
            Assert.IsFalse(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // intersecting lines
            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 4);
            secondLineStart = new Coordinate(4, 2);
            secondLineEnd = new Coordinate(0, 2);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(4, 4);
            secondLineStart = new Coordinate(1, 4);
            secondLineEnd = new Coordinate(4, 1);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2, 5);
            secondLineEnd = new Coordinate(2, 10);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2, 1);
            secondLineEnd = new Coordinate(2, 8);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(7, 7);
            firstLineEnd = new Coordinate(7, 7);
            secondLineStart = new Coordinate(7, 7);
            secondLineEnd = new Coordinate(7, 7);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // not intersecting lines
            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2.1, 1);
            secondLineEnd = new Coordinate(10, 8);
            Assert.IsFalse(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(1, 8.001);
            secondLineEnd = new Coordinate(10, 8.001);
            Assert.IsFalse(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // internally intersecting lines
            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(3, 2);
            secondLineEnd = new Coordinate(2, 3);
            Assert.IsTrue(LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(3, 2);
            secondLineEnd = new Coordinate(3, 2);
            Assert.IsTrue(LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(1, 2);
            secondLineEnd = new Coordinate(200, 3);
            Assert.IsTrue(LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(1, 2);
            secondLineEnd = new Coordinate(200, 3);
            Assert.IsTrue(LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // not internally intersecting lines
            firstLineStart = new Coordinate(0, 0);
            firstLineEnd = new Coordinate(0, 1000.33);
            secondLineStart = new Coordinate(1000.33, 200);
            secondLineEnd = new Coordinate(0, 0);
            Assert.IsFalse(LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(100, 1);
            secondLineStart = new Coordinate(1, 4);
            secondLineEnd = new Coordinate(1, 1);
            Assert.IsFalse(LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // intersecting infinite lines
            firstLineStart = new Coordinate(2, 5);
            CoordinateVector firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 4);
            CoordinateVector secondVector = new CoordinateVector(4, 0);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector));

            firstLineStart = new Coordinate(2, 4);
            firstVector = new CoordinateVector(-3, 1);
            secondLineStart = new Coordinate(5, 3);
            secondVector = new CoordinateVector(-3, 1);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector));

            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(1, 1);
            secondVector = new CoordinateVector(1, 0);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector));

            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 2);
            secondVector = new CoordinateVector(0, 1);
            Assert.IsTrue(LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector));
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector));
        }

        /// <summary>
        /// Tests the <see cref="Intersection" /> method.
        /// </summary>
        [Test]
        public void LineAlgorithmsIntersectionTest()
        {
            // coinciding lines
            Coordinate firstLineStart = new Coordinate(1, 1);
            Coordinate firstLineEnd = new Coordinate(4, 1);
            Coordinate secondLineStart = new Coordinate(1, 1);
            Coordinate secondLineEnd = new Coordinate(4, 1);
            IList<Coordinate> expectedResult = new List<Coordinate> { new Coordinate(1, 1), new Coordinate(4, 1) };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // lines intersecting in one point
            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 4);
            secondLineStart = new Coordinate(4, 2);
            secondLineEnd = new Coordinate(0, 2);
            expectedResult = new List<Coordinate> { new Coordinate(1, 2) };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(-1, 6);
            firstLineEnd = new Coordinate(1, 2);
            secondLineStart = new Coordinate(4, 0);
            secondLineEnd = new Coordinate(0, 4);
            expectedResult = new List<Coordinate> { new Coordinate(0, 4) };

            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));
            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 2);
            secondLineStart = new Coordinate(2, 2);
            secondLineEnd = new Coordinate(2, 2);
            expectedResult = new List<Coordinate> { new Coordinate(2, 2) };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // lines intersecting in more than one point
            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2, 5);
            secondLineEnd = new Coordinate(2, 10);
            expectedResult = new List<Coordinate> { new Coordinate(2, 5), new Coordinate(2, 8) };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(4, 1);
            firstLineEnd = new Coordinate(1, 4);
            secondLineStart = new Coordinate(3, 2);
            secondLineEnd = new Coordinate(2, 3);
            expectedResult = new List<Coordinate> { new Coordinate(3, 2), new Coordinate(2, 3) };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // parallel lines
            firstLineStart = new Coordinate(1.3, 1.3);
            firstLineEnd = new Coordinate(1.3, 4.3);
            secondLineStart = new Coordinate(2.3, 1.3);
            secondLineEnd = new Coordinate(2.3, 4.3);
            expectedResult = new List<Coordinate> { };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // not intersecting lines
            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2.1, 1);
            secondLineEnd = new Coordinate(10, 8);
            expectedResult = new List<Coordinate> { };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // coinciding infinite lines
            firstLineStart = new Coordinate(2, 4);
            CoordinateVector firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 5);
            CoordinateVector secondVector = new CoordinateVector(0, 4);
            Assert.AreEqual(new Coordinate(2, 4), LineAlgorithms.Intersection(firstLineStart, firstVector, secondLineStart, secondVector));


            // intersecting infinite lines
            firstLineStart = new Coordinate(1, 2);
            firstVector = new CoordinateVector(1, -2);
            secondLineStart = new Coordinate(4, 0);
            secondVector = new CoordinateVector(1, -1);
            Assert.AreEqual(new Coordinate(0, 4), LineAlgorithms.Intersection(firstLineStart, firstVector, secondLineStart, secondVector));

            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(1, 1);
            secondVector = new CoordinateVector(1, 0);
            Assert.AreEqual(new Coordinate(1, 1), LineAlgorithms.Intersection(firstLineStart, firstVector, secondLineStart, secondVector));


            // not intersecting infinite lines
            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 2);
            secondVector = new CoordinateVector(0, 1);
            Assert.IsFalse(LineAlgorithms.Intersection(firstLineStart, firstVector, secondLineStart, secondVector).IsValid);


            // internally intersecting lines
            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(3, 2);
            secondLineEnd = new Coordinate(2, 3);
            expectedResult = new List<Coordinate> { new Coordinate(3, 2), new Coordinate(2, 3) };
            Assert.AreEqual(expectedResult, LineAlgorithms.InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(1, 2);
            secondLineEnd = new Coordinate(200, 2);
            expectedResult = new List<Coordinate> { new Coordinate(3, 2) };
            Assert.AreEqual(expectedResult, LineAlgorithms.InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(10.58, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(10.58, 4);
            secondLineEnd = new Coordinate(4, 1);
            expectedResult = new List<Coordinate> { new Coordinate(10.58, 4), new Coordinate(4, 1) };
            Assert.AreEqual(expectedResult, LineAlgorithms.InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // not internally intersecting lines
            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(10, 1);
            secondLineStart = new Coordinate(1, 4);
            secondLineEnd = new Coordinate(1, 1);
            expectedResult = new List<Coordinate> { };
            Assert.AreEqual(expectedResult, LineAlgorithms.InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));


            // intersecting lines with fixed precision
            PrecisionModel precision = new PrecisionModel(0.001);

            firstLineStart = new Coordinate(1000, 1000);
            firstLineEnd = new Coordinate(1000, 4000);
            secondLineStart = new Coordinate(4000, 2000);
            secondLineEnd = new Coordinate(-2000, 1000);
            expectedResult = new List<Coordinate> { new Coordinate(1000, 2000) };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, precision));

            secondLineEnd = new Coordinate(-2000, 0);
            expectedResult = new List<Coordinate> { new Coordinate(1000, 1000) };
            Assert.AreEqual(expectedResult, LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, precision));
        }

        /// <summary>
        /// Tests the <see cref="IsCollinear" /> method.
        /// </summary>
        [Test]
        public void LineAlgorithmsIsCollinearTest()
        {
            // collinear lines
            Coordinate firstLineStart = new Coordinate(1, 1);
            Coordinate firstLineEnd = new Coordinate(4, 1);
            Coordinate secondLineStart = new Coordinate(1, 1);
            Coordinate secondLineEnd = new Coordinate(4, 1);
            Assert.IsTrue(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(1, 1);
            secondLineEnd = new Coordinate(15, 1);
            Assert.IsTrue(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(16, 1);
            secondLineStart = new Coordinate(15, 1);
            secondLineEnd = new Coordinate(2, 1);
            Assert.IsTrue(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(15, 1);
            secondLineEnd = new Coordinate(1, 1);
            Assert.IsTrue(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(4, 1);
            secondLineEnd = new Coordinate(-5, 1);
            Assert.IsTrue(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(20, 1);
            firstLineEnd = new Coordinate(250, 1);
            secondLineEnd = new Coordinate(1000, 1);
            secondLineStart = new Coordinate(250, 1);
            Assert.IsTrue(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(20, 1);
            firstLineEnd = new Coordinate(250, 1);
            secondLineStart = new Coordinate(259, 1);
            secondLineEnd = new Coordinate(1000, 1);
            Assert.IsTrue(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            // not collinear lines
            firstLineStart = new Coordinate(20, 1);
            firstLineEnd = new Coordinate(250, 1);
            secondLineStart = new Coordinate(1, 100);
            secondLineEnd = new Coordinate(1, 1200);
            Assert.IsFalse(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(20, 1);
            firstLineEnd = new Coordinate(250, 1);
            secondLineStart = new Coordinate(250, 1);
            secondLineEnd = new Coordinate(1000, 2);
            Assert.IsFalse(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1.2, 1.2);
            firstLineEnd = new Coordinate(1.2, 7.2);
            secondLineStart = new Coordinate(2.2, 8.2);
            secondLineEnd = new Coordinate(2.2, 15.2);
            Assert.IsFalse(LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));
        }

        /// <summary>
        /// Tests the <see cref="IsParallel" /> method.
        /// </summary>
        [Test]
        public void LineAlgorithmsIsParallelTest()
        {
            // parallel lines
            Coordinate firstLineStart = new Coordinate(1, 1);
            Coordinate firstLineEnd = new Coordinate(4, 1);
            Coordinate secondLineStart = new Coordinate(2, 2);
            Coordinate secondLineEnd = new Coordinate(4, 2);
            Assert.IsTrue(LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));
            Assert.IsFalse(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1.2, 1.2);
            firstLineEnd = new Coordinate(1.2, 7.2);
            secondLineStart = new Coordinate(2.2, 8.2);
            secondLineEnd = new Coordinate(2.2, 15.2);
            Assert.IsTrue(LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));
            Assert.IsFalse(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 7);
            secondLineStart = new Coordinate(1, 1);
            secondLineEnd = new Coordinate(1, 7);
            Assert.IsTrue(LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            // non parallel lines
            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 7);
            secondLineStart = new Coordinate(2, 1);
            secondLineEnd = new Coordinate(2.2, 8);
            Assert.IsFalse(LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 7);
            secondLineStart = new Coordinate(0, 2);
            secondLineEnd = new Coordinate(10, 2);
            Assert.IsFalse(LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd));

            // parallel infinite lines
            firstLineStart = new Coordinate(1, 1);
            CoordinateVector firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 2);
            CoordinateVector secondVector = new CoordinateVector(0, 1);
            Assert.IsTrue(LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector));

            firstLineStart = new Coordinate(2, 5);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 4);
            secondVector = new CoordinateVector(0, 4);
            Assert.IsTrue(LineAlgorithms.Coincides(firstLineStart, firstVector, secondLineStart, secondVector));
            Assert.IsTrue(LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector));

            firstLineStart = new Coordinate(2, 4);
            firstVector = new CoordinateVector(-3, 1);
            secondLineStart = new Coordinate(5, 3);
            secondVector = new CoordinateVector(-3, 1);
            Assert.IsTrue(LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector));

            // non parallel infinite lines
            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(1, 1);
            secondVector = new CoordinateVector(1, 0);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector));
            Assert.IsFalse(LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector));

            firstLineStart = new Coordinate(2, 5);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 4);
            secondVector = new CoordinateVector(4, 0);
            Assert.IsTrue(LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector));
            Assert.IsFalse(LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector));
        }
    }
}
