/// <copyright file="LambertConicProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Reference.Operations
{
    [TestFixture]
    public class LambertConicProjectionTest
    {
        private LambertConicConformal1SPProjection _projection1SP;
        private LambertConicConformal1SPWestOrientatedProjection _projection1SPWestOrientated;
        private LambertConicConformal2SPProjection _projection2SP;
        private LambertConicConformal2SPBelgiumProjection _projection2SPBelgium;
        private LambertConicNearConformalProjection _projectionLambertConicNearConformal;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(18, 00, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-77, 00, 00));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(250000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(150000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 1.000000);

            _projection1SP = new LambertConicConformal1SPProjection("EPSG::9801", "Jamaica National Grid", parameters, Ellipsoids.Clarke1866, AreasOfUse.World);


            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(18, 00, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(77, 00, 00));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(250000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(150000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 1.000000);

            _projection1SPWestOrientated = new LambertConicConformal1SPWestOrientatedProjection("EPSG::9826", "Jamaica National Grid", parameters, Ellipsoids.Clarke1866, AreasOfUse.World);


            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(27, 50, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(-99, 00, 00));
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(28, 23, 00));
            parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, Angle.FromDegree(30, 17, 00));
            parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.Convert(Length.FromUSSurveyFoot(2000000.00), UnitsOfMeasurement.Metre));
            parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.Convert(Length.FromUSSurveyFoot(0.00), UnitsOfMeasurement.Metre));

            _projection2SP = new LambertConicConformal2SPProjection("EPSG::9802", "Texas South Central", parameters, Ellipsoids.Clarke1866, AreasOfUse.World);


            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(90, 00, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(4, 21, 24.983));
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(49, 50, 00));
            parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, Angle.FromDegree(51, 10, 00));
            parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(150000.01));
            parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(5400088.44));

            _projection2SPBelgium = new LambertConicConformal2SPBelgiumProjection("EPSG::9803", "Belge Lambert 72", parameters, Ellipsoids.International1924, AreasOfUse.World);


            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(34, 39, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(37, 21, 00));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(300000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(300000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.99962560);

            _projectionLambertConicNearConformal = new LambertConicNearConformalProjection("EPSG::9817", "Levant Zone", parameters, Ellipsoids.Clarke1880IGN, AreasOfUse.World);
        }

        [TestCase]
        public void LambertConicConformal1SPProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(17, 55, 55.80), Angle.FromDegree(-76, 56, 37.26));
            Coordinate expected = new Coordinate(255966.58, 142493.51);
            Coordinate transformed = _projection1SP.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void LambertConicConformal1SPProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(17, 55, 55.80), Angle.FromDegree(-76, 56, 37.26));
            GeoCoordinate transformed = _projection1SP.Reverse(_projection1SP.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void LambertConicConformal1SPWestOrientatedProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(17, 55, 55.80), Angle.FromDegree(76, 56, 37.26));
            Coordinate expected = new Coordinate(255966.58, 142493.51);
            Coordinate transformed = _projection1SPWestOrientated.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void LambertConicConformal1SPWestOrientatedProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(17, 55, 55.80), Angle.FromDegree(76, 56, 37.26));
            GeoCoordinate transformed = _projection1SPWestOrientated.Reverse(_projection1SPWestOrientated.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void LambertConicConformal2SPProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(28, 30, 00.00), Angle.FromDegree(-96, 00, 00.00));
            Coordinate expected = new Coordinate(Length.FromUSSurveyFoot(2963503.91).BaseValue, Length.FromUSSurveyFoot(254759.80).BaseValue);
            Coordinate transformed = _projection2SP.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void LambertConicConformal2SPProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(28, 30, 00.00), Angle.FromDegree(-96, 00, 00.00));
            GeoCoordinate transformed = _projection2SP.Reverse(_projection2SP.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void LambertConicConformal2SPBelgiumProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 40, 46.461), Angle.FromDegree(5, 48, 26.533));
            Coordinate expected = new Coordinate(251763.20, 153034.13);
            Coordinate transformed = _projection2SPBelgium.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void LambertConicConformal2SPBelgiumProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(50, 40, 46.461), Angle.FromDegree(5, 48, 26.533));
            GeoCoordinate transformed = _projection2SPBelgium.Reverse(_projection2SPBelgium.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void LambertConicNearConformalProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(37, 31, 17.625), Angle.FromDegree(34, 08, 11.291));
            Coordinate expected = new Coordinate(15707.96, 623165.96);
            Coordinate transformed = _projectionLambertConicNearConformal.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void LambertConicNearConformalProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(37, 31, 17.625), Angle.FromDegree(34, 08, 11.291));
            GeoCoordinate transformed = _projectionLambertConicNearConformal.Reverse(_projectionLambertConicNearConformal.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
