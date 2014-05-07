/// <copyright file="MercatorProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Reference.Operations
{
    [TestFixture]
    public class MercatorProjectionTest
    {
        private MercatorAProjection _projectionA;
        private MercatorBProjection _projectionB;
        private MercatorSphericalProjection _projectionSpherical;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(110));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(3900000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(900000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.997);

            _projectionA = new MercatorAProjection("EPSG::19905", "Netherlands East Indies Equatorial Zone", parameters, Ellipsoids.Bessel1841, AreasOfUse.World);

            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(42));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(51));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            _projectionB = new MercatorBProjection("EPSG::19884", "Caspian Sea Mercator", parameters, Ellipsoids.Krassowsky1940, AreasOfUse.World);

            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            _projectionSpherical = new MercatorSphericalProjection(String.Empty, "World Spherical Mercator",
                                                                   Ellipsoid.FromSphere(String.Empty, "Sphere", 6371007),
                                                                   parameters, AreasOfUse.World);
        }

        [TestCase]
        public void MercatorAProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(-3), Angle.FromDegree(120));
            Coordinate expected = new Coordinate(5009726.58, 569150.82);
            Coordinate transformed = _projectionA.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 1);
            Assert.AreEqual(expected.Y, transformed.Y, 1);
        }

        [TestCase]
        public void MercatorAProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-3), Angle.FromDegree(120));
            GeoCoordinate transformed = _projectionA.Reverse(_projectionA.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.0001);
        }

        [TestCase]
        public void MercatorBProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53), Angle.FromDegree(53));
            Coordinate expected = new Coordinate(165704.29, 5171848.07);
            Coordinate transformed = _projectionB.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 1);
            Assert.AreEqual(expected.Y, transformed.Y, 1);
        }

        [TestCase]
        public void MercatorBProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(53), Angle.FromDegree(53));
            GeoCoordinate transformed = _projectionB.Reverse(_projectionB.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.0001);
        }

        [TestCase]
        public void MercatorSphericalProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(24, 22, 54.433), Angle.FromDegree(-100, 20, 0));
            Coordinate expected = new Coordinate(-11156569.9, 2796869.94);
            Coordinate transformed = _projectionSpherical.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 100);
            Assert.AreEqual(expected.Y, transformed.Y, 100);
        }

        [TestCase]
        public void MercatorSphericalProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(24, 22, 54.433), Angle.FromDegree(-100, 20, 0));
            GeoCoordinate transformed = _projectionSpherical.Reverse(_projectionSpherical.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.0001);
        }
    }
}
