/// <copyright file="ObliqueStereographicProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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
    public class ObliqueStereographicProjectionTest
    {
        private ObliqueStereographicProjection _projection;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(52, 9, 22.178));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(5, 23, 15.5));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(155000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(463000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9999079);

            _projection = new ObliqueStereographicProjection("EPSG::19914", "RD New", parameters, Ellipsoids.Bessel1841, AreasOfUse.NetherlandsOnshore);
        }

        [TestCase]
        public void ObliqueStereographicProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53), Angle.FromDegree(6));
            Coordinate expected = new Coordinate(196105.283, 557057.739);
            Coordinate transformed = _projection.Forward(coordinate);

            Assert.AreEqual(transformed.X, expected.X, 0.01);
            Assert.AreEqual(transformed.Y, expected.Y, 0.01);
        }

        [TestCase]
        public void ObliqueStereographicProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(53), Angle.FromDegree(6));
            GeoCoordinate transformed = _projection.Reverse(_projection.Forward(expected));

            Assert.AreEqual(transformed.Latitude.BaseValue, expected.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(transformed.Longitude.BaseValue, expected.Longitude.BaseValue, 0.0001);
        }
    }
}
