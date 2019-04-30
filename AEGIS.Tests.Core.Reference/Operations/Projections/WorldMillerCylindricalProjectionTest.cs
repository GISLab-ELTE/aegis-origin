/// <copyright file="WorldMillerCylindricalProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Péter Rónai</author>

using ELTE.AEGIS;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using ELTE.AEGIS.Reference.Operations.Projections;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AEGIS.Tests.Reference.Operations.Projections
{
    /// <summary>
    /// Test fixture for the <see cref="WorldMillerCylindricalProjection" /> class.
    /// </summary>
    [TestFixture]
    public class WorldMillerCylindricalProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private WorldMillerCylindricalProjection _projection;

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

            String identifier = CoordinateOperationMethods.WorldMillerCylindricalProjection.Identifier;
            String name = CoordinateOperationMethods.WorldMillerCylindricalProjection.Name;
            _projection = new WorldMillerCylindricalProjection(identifier, name, parameters, Ellipsoids.WGS1984, AreasOfUse.World);
        }

        /// <summary>
        /// Tests the <see cref="Forward" /> method.
        /// </summary>
        [Test]
        public void WorldMillerCylindricalProjectionForwardTest()
        {
            // expected values were generated with OSGeo Proj.4 (https://github.com/OSGeo/proj.4/wiki)
            GeoCoordinate input = new GeoCoordinate(Angle.FromDegree(30), Angle.FromDegree(-110));
            Coordinate expected = new Coordinate(Length.FromMetre(-2226389.81587).Value, Length.FromMetre(3441760.13671).Value);
            Coordinate actual = _projection.Forward(input);

            Assert.AreEqual(expected.X, actual.X, 0.00001);
            Assert.AreEqual(expected.Y, actual.Y, 0.00001);
        }

        /// <summary>
        /// Tests the <see cref="Reverse" /> method.
        /// </summary>
        [Test]
        public void WorldMillerCylindricalProjectionReverseTest()
        {
            // expected values were generated with OSGeo Proj.4 (https://github.com/OSGeo/proj.4/wiki)
            Coordinate input = new Coordinate(Length.FromMetre(600300).Value, Length.FromMetre(-295675).Value);
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-2.65548507092), Angle.FromDegree(-84.6074133494));
            GeoCoordinate actual = _projection.Reverse(input);

            Assert.AreEqual(expected.Latitude.BaseValue, actual.Latitude.BaseValue, 0.00000000001);
            Assert.AreEqual(expected.Longitude.BaseValue, actual.Longitude.BaseValue, 0.00000000001);
        }
    }
}
