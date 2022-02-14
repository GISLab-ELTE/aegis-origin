/// <copyright file="HotineObliqueMercatorProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Reference.Operations
{
    [TestFixture]
    public class HotineObliqueMercatorProjectionTest
    {
        private HotineObliqueMercatorAProjection _projectionA;
        private HotineObliqueMercatorBProjection _projectionB;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(4));
            parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, Angle.FromDegree(115));
            parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, Angle.FromDegree(53, 18, 56.9537));
            parameters.Add(CoordinateOperationParameters.AngleFromRectifiedToSkewGrid, Angle.FromDegree(53, 7, 48.3685));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, 0.99984);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            _projectionA = new HotineObliqueMercatorAProjection("EPSG::19894", "Borneo RSO", parameters, Ellipsoids.GRS1980, AreasOfUse.World);

            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(4));
            parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, Angle.FromDegree(115));
            parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, Angle.FromDegree(53, 18, 56.9537));
            parameters.Add(CoordinateOperationParameters.AngleFromRectifiedToSkewGrid, Angle.FromDegree(53, 7, 48.3685));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, 0.99984);
            parameters.Add(CoordinateOperationParameters.EastingAtProjectionCentre, Length.FromMetre(590476.87));
            parameters.Add(CoordinateOperationParameters.NorthingAtProjectionCentre, Length.FromMetre(442857.65));

            _projectionB = new HotineObliqueMercatorBProjection("EPSG::19958", "Rectified Skew Orthomorphic Borneo Grid (metres)", parameters, Ellipsoids.Everest1967, AreasOfUse.World);
        }

        [TestCase]
        public void HotineObliqueMercatorAProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(5, 23, 14.1129), Angle.FromDegree(115, 48, 19.8196));
            Coordinate expected = new Coordinate(679245.73, 596562.78);
            Coordinate transformed = _projectionA.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 100);
            Assert.AreEqual(expected.Y, transformed.Y, 100);
        }

        [TestCase]
        public void HotineObliqueMercatorAProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(5, 23, 14.1129), Angle.FromDegree(115, 48, 19.8196));
            GeoCoordinate transformed = _projectionA.Reverse(_projectionA.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void HotineObliqueMercatorBProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(5, 23, 14.1129), Angle.FromDegree(115, 48, 19.8196));
            Coordinate expected = new Coordinate(679245.73, 596562.78);
            Coordinate transformed = _projectionB.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void HotineObliqueMercatorBProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(5, 23, 14.1129), Angle.FromDegree(115, 48, 19.8196));
            GeoCoordinate transformed = _projectionB.Reverse(_projectionB.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
