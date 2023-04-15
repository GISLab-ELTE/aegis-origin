// <copyright file="SinusoidalProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Reference.Operations.Projections;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Reference.Operations.Projections
{
    /// <summary>
    /// Test fixture for the <see cref="SinusoidalProjection" /> class.
    /// </summary>
    /// <author>Péter Rónai</author>
    [TestFixture]
    public class SinusoidalProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private SinusoidalProjection _projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-90));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            String identifier = CoordinateOperationMethods.SinusoidalProjection.Identifier;
            String name = CoordinateOperationMethods.SinusoidalProjection.Name;
            _projection = new SinusoidalProjection(identifier, name, parameters, Ellipsoids.WGS1984, AreasOfUse.World);
        }

        /// <summary>
        /// Tests the <see cref="Forward" /> method.
        /// </summary>
        [Test]
        public void SinusoidalProjectionForwardTest()
        {
            // expected values were generated with OSGeo Proj.4 (https://github.com/OSGeo/proj.4/wiki)
            GeoCoordinate input = new GeoCoordinate(Angle.FromDegree(30), Angle.FromDegree(-110));
            Coordinate expected = new Coordinate(Length.FromMetre(-1929725.60502).Value, Length.FromMetre(3320113.39794).Value);
            Coordinate actual = _projection.Forward(input);

            Assert.AreEqual(expected.X, actual.X, 0.001);
            Assert.AreEqual(expected.Y, actual.Y, 0.001);
        }

        /// <summary>
        /// Tests the <see cref="Reverse" /> method.
        /// </summary>
        [Test]
        public void SinusoidalProjectionReverseTest()
        {
            // expected values were generated with OSGeo Proj.4 (https://github.com/OSGeo/proj.4/wiki)
            Coordinate input = new Coordinate(Length.FromMetre(600300).Value, Length.FromMetre(-295675).Value);
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-2.67397496527), Angle.FromDegree(-84.6015746607));
            GeoCoordinate actual = _projection.Reverse(input);

            Assert.AreEqual(expected.Latitude.BaseValue, actual.Latitude.BaseValue, 0.00000000001);
            Assert.AreEqual(expected.Longitude.BaseValue, actual.Longitude.BaseValue, 0.00000000001);
        }
    }
}
