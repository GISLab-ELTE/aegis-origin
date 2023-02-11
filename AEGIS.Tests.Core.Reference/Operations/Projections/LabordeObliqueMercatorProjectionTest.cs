// <copyright file="LabordeObliqueMercatorProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AEGIS.Tests.Reference.Operations.Projections
{
    [TestFixture]
    public class LabordeObliqueMercatorProjectionTest
    {
        private LabordeObliqueMercatorProjection _projection;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromGrad(-21));
            parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, Angle.FromGrad(49));
            parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, Angle.FromGrad(21));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, 0.9995);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(400000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(800000));

            _projection = new LabordeObliqueMercatorProjection("EPSG::19861", "Laborde Grid", parameters, Ellipsoids.International1924, AreasOfUse.World);
        }

        [TestCase]
        public void LabordeObliqueMercatorProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromGrad(-17.988666667), Angle.FromGrad(46.800381173));
            Coordinate expected = new Coordinate(188333.848, 1098841.091);
            Coordinate transformed = _projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.001);
            Assert.AreEqual(expected.Y, transformed.Y, 0.001);
        }

        [TestCase]
        public void LabordeObliqueMercatorProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromGrad(-17.988666667), Angle.FromGrad(46.800381173));
            GeoCoordinate transformed = _projection.Reverse(_projection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
