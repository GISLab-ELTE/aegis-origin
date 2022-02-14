/// <copyright file="GeocentricToTopocentricConversionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Reference.Operations
{
    [TestFixture]
    public class GeocentricToTopocentricConversionTest
    {
        private GeocentricToTopocentricConversion _conversion;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.GeocenticXOfTopocentricOrigin, Length.FromMetre(3652755.3058));
            parameters.Add(CoordinateOperationParameters.GeocenticYOfTopocentricOrigin, Length.FromMetre(319574.6799));
            parameters.Add(CoordinateOperationParameters.GeocenticZOfTopocentricOrigin, Length.FromMetre(5201547.3536));

            _conversion = new GeocentricToTopocentricConversion(parameters, Ellipsoids.WGS1984);
        }

        [TestCase]
        public void GeocentricToTopocentricConversionForwardTest()
        {
            Coordinate coordinate = new Coordinate(3771793.968, 140253.342, 5124304.349);
            Coordinate expected = new Coordinate(-189013.869, -128642.04, -4220.171);
            Coordinate transformed = _conversion.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.001);
            Assert.AreEqual(expected.Y, transformed.Y, 0.001);
            Assert.AreEqual(expected.Z, transformed.Z, 0.001);
        }

        [TestCase]
        public void GeocentricToTopocentricConversionReverseTest()
        {
            Coordinate expected = new Coordinate(3771793.968, 140253.342, 5124304.349);
            Coordinate transformed = _conversion.Reverse(_conversion.Forward(expected));

            Assert.AreEqual(expected.X, transformed.X, 0.001);
            Assert.AreEqual(expected.Y, transformed.Y, 0.001);
            Assert.AreEqual(expected.Z, transformed.Z, 0.001);
        }
    }
}
