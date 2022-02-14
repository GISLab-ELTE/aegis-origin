/// <copyright file="GuamProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class GuamProjectionTest
    {
        private GuamProjection _projection;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(13, 28, 20.87887));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(144, 44, 55.50254));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(50000.00));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(50000.00));

            _projection = new GuamProjection("EPSG::9831", "Guam SPCS", parameters, Ellipsoids.Clarke1866, AreasOfUse.World);
        }

        [TestCase]
        public void GuamProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(13, 20, 20.53846), Angle.FromDegree(144, 38, 07.19265));
            Coordinate expected = new Coordinate(37712.48, 35242.00);
            Coordinate transformed = _projection.Forward(coordinate);

            Assert.AreEqual(transformed.X, expected.X, 0.01);
            Assert.AreEqual(transformed.Y, expected.Y, 0.01);
        }

        [TestCase]
        public void GuamProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(13, 20, 20.53846), Angle.FromDegree(144, 38, 07.19265));
            GeoCoordinate transformed = _projection.Reverse(_projection.Forward(expected));

            Assert.AreEqual(transformed.Latitude.BaseValue, expected.Latitude.BaseValue, 0.001);
            Assert.AreEqual(transformed.Longitude.BaseValue, expected.Longitude.BaseValue, 0.001);
        }
    }
}
