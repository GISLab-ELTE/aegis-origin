/// <copyright file="LambertCylindricalEqualAreaProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
/// <author>Tamás Szabó</author>

using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Reference.Operations.Projections
{
    /// <summary>
    /// Test fixture for the Lambert Cylindrical Equal Area projection.
    /// </summary>
    [TestFixture]
    public class LambertCylindricalEqualAreaProjectionTest
    {
        #region Private fields

        /// <summary>
        /// Lambert Cylindrical Equal Area (ellipsoidal case).
        /// </summary>
        private LambertCylindricalEqualAreaEllipsoidalProjection _projectionEllipsoidal;

        /// <summary>
        /// Lambert Cylindrical Equal Area (spherical case).
        /// </summary>
        private LambertCylindricalEqualAreaSphericalProjection _projectionSpherical;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(5));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-75));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            _projectionEllipsoidal = new LambertCylindricalEqualAreaEllipsoidalProjection(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, parameters, Ellipsoids.Clarke1866, AreasOfUse.World);

            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(30));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-75));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            _projectionSpherical = new LambertCylindricalEqualAreaSphericalProjection(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, parameters, Ellipsoids.Clarke1866AS, AreasOfUse.World);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for the ellipsoidal projection <see cref="Forward"/> method.
        /// </summary>
        [Test]
        public void LambertCylindricalEqualAreaEllipsoidalProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(5), Angle.FromDegree(-78));
            Coordinate expected = new Coordinate(-332699.83, 554248.45);
            Coordinate transformed = _projectionEllipsoidal.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        /// <summary>
        /// Test case for the ellipsoidal projection <see cref="Reverse"/> method.
        /// </summary>
        [Test]
        public void LambertCylindricalEqualAreaEllipsoidalProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(35), Angle.FromDegree(80));
            GeoCoordinate transformed = _projectionSpherical.Reverse(_projectionSpherical.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.0001);
        }

        /// <summary>
        /// Test case for the spherical projection <see cref="Forward"/> method.
        /// </summary>
        [Test]
        public void LambertCylindricalEqualAreaSphericalProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(35), Angle.FromDegree(80));
            Coordinate expected = new Coordinate(14926125.81, 4219568.78);
            Coordinate transformed = _projectionSpherical.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        /// <summary>
        /// Test case for the spherical projection <see cref="Reverse"/> method.
        /// </summary>
        [Test]
        public void LambertCylindricalEqualAreaSphericalProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(5), Angle.FromDegree(-78));
            GeoCoordinate transformed = _projectionEllipsoidal.Reverse(_projectionEllipsoidal.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.001);
        }

        #endregion
    }
}