/// <copyright file="LambertAzimuthalEqualAreaProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AEGIS.Tests.Reference.Operations.Projections
{
    [TestFixture]
    public class LambertAzimuthalEqualAreaProjectionTest
    {
        private LambertAzimuthalEqualAreaProjection _projection;
        private Coordinate coordinate;
        private GeoCoordinate geoCoordinate;

        public LambertAzimuthalEqualAreaProjectionTest()
        {
            // Location: Berlin
            coordinate = new Coordinate(4553442.949403, 3271908.232428);
            geoCoordinate = new GeoCoordinate(52.5072003, 13.4247528);
        }

        [SetUp]
        public void SetUp()
        {
            var parameters = new Dictionary<CoordinateOperationParameter, object>
            {
                { CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(52) },
                { CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(10) },
                { CoordinateOperationParameters.FalseEasting, Length.FromMetre(4321000) },
                { CoordinateOperationParameters.FalseNorthing, Length.FromMetre(3210000) }
            };

            _projection = new LambertAzimuthalEqualAreaProjection("EPSG::1149", "ETRS89 to WGS 84", parameters, Ellipsoids.GRS1980, AreasOfUse.EuropeETRS89);
        }

        [TestCase]
        public void LambertAzimuthalEqualAreaProjectionForwardTest()
        {           
            var transformed = _projection.Forward(geoCoordinate);

            Assert.AreEqual(coordinate.X, transformed.X, 0.001);
            Assert.AreEqual(coordinate.Y, transformed.Y, 0.001);
        }

        [TestCase]
        public void LambertAzimuthalEqualAreaProjectionReverseTest()
        {            
            var transformed = _projection.Reverse(coordinate);

            Assert.AreEqual(geoCoordinate.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(geoCoordinate.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
