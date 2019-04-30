/// <copyright file="ModifiedAzimuthalEquidistantProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class ModifiedAzimuthalEquidistantProjectionTest
    {
        private ModifiedAzimuthalEquidistantProjection _projection;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(9, 32, 48.15));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(138, 10, 07.48));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(40000.00));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(60000.00));

            _projection = new ModifiedAzimuthalEquidistantProjection("EPSG::9832", "Yap Islands", parameters, Ellipsoids.Clarke1866, AreasOfUse.World);
        }

        [TestCase]
        public void ModifiedAzimuthalEquidistantProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(9, 35, 47.493), Angle.FromDegree(138, 11, 34.908));
            Coordinate expected = new Coordinate(42665.90, 65509.82);
            Coordinate transformed = _projection.Forward(coordinate);

            Assert.AreEqual(transformed.X, expected.X, 0.01);
            Assert.AreEqual(transformed.Y, expected.Y, 0.01);
        }

        [TestCase]
        public void ModifiedAzimuthalEquidistantProjectionProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(9, 35, 47.493), Angle.FromDegree(138, 11, 34.908));
            GeoCoordinate transformed = _projection.Reverse(_projection.Forward(expected));

            Assert.AreEqual(transformed.Latitude.BaseValue, expected.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(transformed.Longitude.BaseValue, expected.Longitude.BaseValue, 0.0001);
        }
    }
}
