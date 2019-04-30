/// <copyright file="GeographicToGeocentricConversionTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;

namespace AEGIS.Tests.Reference.Operations
{
    [TestFixture]
    public class GeographicToGeocentricConversionTest
    {
        private GeographicToGeocentricConversion _conversion;

        [SetUp]
        public void SetUp()
        {
            _conversion = new GeographicToGeocentricConversion(Ellipsoids.WGS1984);
        }

        [TestCase]
        public void GeographicToGeocentricConversionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            Coordinate expected = new Coordinate(3771793.968, 140253.342, 5124304.349);
            Coordinate transformed = _conversion.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.001);
            Assert.AreEqual(expected.Y, transformed.Y, 0.001);
            Assert.AreEqual(expected.Z, transformed.Z, 0.001);
        }

        [TestCase]
        public void GeographicToGeocentricConversionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            GeoCoordinate transformed = _conversion.Reverse(_conversion.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Height.BaseValue, transformed.Height.BaseValue, 0.0001);
        }
    }
}
