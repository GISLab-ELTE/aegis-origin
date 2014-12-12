/// <copyright file="PolygonCentroidAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Algorithms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="PolygonCentroidAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class PolygonCentroidAlgorithmTest
    {
        #region Private fields

        /// <summary>
        /// The list of polygons.
        /// </summary>
        private List<IBasicPolygon> _polygons;

        /// <summary>
        /// The list of expected coordinates.
        /// </summary>
        private List<Coordinate> _expected;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _polygons = new List<IBasicPolygon>();
            _expected = new List<Coordinate>();

            Coordinate[] shell = new Coordinate[] 
            {
                new Coordinate(0, 0),
                new Coordinate(0, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 0),
                new Coordinate(0, 0)
            };

            _polygons.Add(new BasicPolygon(shell, null));

            shell = new Coordinate[] 
            {
                new Coordinate(-123.222653196, 49.1529676585),
                new Coordinate(-89.4726531957, 49.3823707987),
                new Coordinate(-81.0351531957, 44.0875828344),
                new Coordinate(-71.1914031957, 44.3395630636),
                new Coordinate(-62.0507781957, 48.4583498573),
                new Coordinate(-60.2929656957, 45.0890334085),
                new Coordinate(-78.9257781957, 37.4399716272),
                new Coordinate(-82.0898406957, 31.3536343332),
                new Coordinate(-81.3867156957, 26.4312253295),
                new Coordinate(-91.9335906957, 29.8406412505),
                new Coordinate(-98.2617156957, 26.4312253295),
                new Coordinate(-107.753903196, 32.2499718728),
                new Coordinate(-82.0898406957, 31.3536343332),
                new Coordinate(-116.894528196, 33.1375486348),
                new Coordinate(-122.519528196, 36.0313293064),
                new Coordinate(-126.035153196, 42.2935619329),
                new Coordinate(-123.222653196, 49.1529676585)
            };
            _polygons.Add(new BasicPolygon(shell, null));
            _polygons.Add(new BasicPolygon(shell.Reverse().ToList(), null));

            _expected.Add(new Coordinate(5, 5));
            _expected.Add(new Coordinate(-98.3413124738, 39.8526108305));
            _expected.Add(new Coordinate(-98.3413124738, 39.8526108305));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="ComputeCentroid" /> method.
        /// </summary>
        [Test]
        public void PolygonCentroidAlgorithmComputeCentroidTest()
        {
            // specified polygons

            for (Int32 polygonIndex = 0; polygonIndex < _polygons.Count; polygonIndex++)
            {
                Coordinate result = PolygonCentroidAlgorithm.ComputeCentroid(_polygons[polygonIndex]);

                Assert.AreEqual(_expected[polygonIndex].X, _expected[polygonIndex].X, 0.0000001);
                Assert.AreEqual(_expected[polygonIndex].Y, _expected[polygonIndex].Y, 0.0000001);
            }

            // exceptions

            Assert.Throws<ArgumentNullException>(() => PolygonCentroidAlgorithm.ComputeCentroid((IBasicPolygon)null));
            Assert.Throws<ArgumentNullException>(() => PolygonCentroidAlgorithm.ComputeCentroid((IList<Coordinate>)null));
            Assert.Throws<ArgumentNullException>(() => PolygonCentroidAlgorithm.ComputeCentroid((IList<Coordinate>)null, null));
        }

        #endregion
    }
}
