/// <copyright file="CassiniSoldnerProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Geodesy;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Reference.Operations
{
    [TestFixture]
    public class CassiniSoldnerProjectionTest
    {
        private CassiniSoldnerProjection _projection;
        private HyperbolicCassiniSoldnerProjection _hyperbolicProjection;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(10, 26, 30));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-61, 20, 0));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromClarkesLink(430000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromClarkesLink(325000));

            _projection = new CassiniSoldnerProjection("EPSG::19925", "Trinidad Grid", parameters, Ellipsoids.Clarke1858.ReMeasure(UnitsOfMeasurement.ClarkesLink), AreasOfUse.TrinidadAndTobago);


            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(-16, 15, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(179, 20, 00));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromClarkesChain(12513.318), UnitsOfMeasurement.ClarkesFoot));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromClarkesChain(16628.885), UnitsOfMeasurement.ClarkesFoot));

            _hyperbolicProjection = new HyperbolicCassiniSoldnerProjection("EPSG::9833", "Vanua Levu Grid", parameters, Ellipsoids.Clarke1880, AreasOfUse.World);
        }

        [TestCase]
        public void CassiniSoldnerProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(10), Angle.FromDegree(-62));
            Coordinate expected = new Coordinate(Length.FromClarkesLink(66644.94).Value, Length.FromClarkesLink(82536.22).Value);
            Coordinate transformed = _projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void CassiniSoldnerProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(10), Angle.FromDegree(-62));
            GeoCoordinate transformed = _projection.Reverse(_projection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void HyperbolicCassiniSoldnerProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(-16, 50, 29.2435), Angle.FromDegree(179, 59, 39.6115));
            Coordinate expected = new Coordinate(Length.Convert(Length.FromClarkesChain(16015.2890), UnitsOfMeasurement.ClarkesFoot).Value,
                                                 Length.Convert(Length.FromClarkesChain(13369.6601), UnitsOfMeasurement.ClarkesFoot).Value);
            Coordinate transformed = _hyperbolicProjection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void HyperbolicCassiniSoldnerProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-16, 50, 29.2435), Angle.FromDegree(179, 59, 39.6115));
            GeoCoordinate transformed = _hyperbolicProjection.Reverse(_hyperbolicProjection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
