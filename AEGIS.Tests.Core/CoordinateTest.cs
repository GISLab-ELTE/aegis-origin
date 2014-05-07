/// <copyright file="CoordianteTest.cs" company="Eötvös Loránd University (ELTE)">
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

using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests
{
    [TestFixture]
    public class CoordinateTest
    {
        [TestCase]
        public void CoordinateAngleTest()
        {
            // test case 1: 90°
            Coordinate origin = new Coordinate(0, 0);
            Coordinate first = new Coordinate(1, 0);
            Coordinate second = new Coordinate(0, 1);

            Assert.AreEqual(Math.PI / 2, Coordinate.Angle(origin, first, second), 0.00001);

            // test case 2: 180°
            origin = new Coordinate(0, 0);
            first = new Coordinate(1, 0);
            second = new Coordinate(-1, 0);

            Assert.AreEqual(Math.PI, Coordinate.Angle(origin, first, second), 0.00001);

            // test case 3: -180°
            origin = new Coordinate(0, 0);
            first = new Coordinate(-1, 0);
            second = new Coordinate(1, 0);

            Assert.AreEqual(Math.PI, Coordinate.Angle(origin, first, second), 0.00001);

            // test case 4: 45°
            origin = new Coordinate(0, 0);
            first = new Coordinate(1, 0);
            second = new Coordinate(1, 1);

            Assert.AreEqual(Math.PI / 4, Coordinate.Angle(origin, first, second), 0.00001);

            // test case 5: 60°
            origin = new Coordinate(0, 0);
            first = new Coordinate(1, 0);
            second = new Coordinate(1, Math.Sqrt(3));

            Assert.AreEqual(Math.PI / 3, Coordinate.Angle(origin, first, second), 0.00001);
        }
    }
}
