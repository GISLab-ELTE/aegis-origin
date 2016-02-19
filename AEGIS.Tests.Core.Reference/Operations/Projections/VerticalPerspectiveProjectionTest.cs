/// <copyright file="VerticalPerspectiveProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
/// <author>András Fábián</author>

using ELTE.AEGIS;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AEGIS.Tests.Reference.Operations.Projections
{
    [TestFixture]
    public class VerticalPerspectiveProjectionTest
    {
        private VerticalPerspectiveProjection _projection;
        private VerticalPerspectiveOrthographicProjection _orthographicProjection;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfTopocentricOrigin, Angle.FromDegree(55));
            parameters.Add(CoordinateOperationParameters.LongitudeOfTopocentricOrigin, Angle.FromDegree(5));
            parameters.Add(CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin, Length.FromMetre(200));
            parameters.Add(CoordinateOperationParameters.ViewPointHeight, Length.FromMetre(5900000));

            _projection = new VerticalPerspectiveProjection(parameters, Ellipsoids.WGS1984);

            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfTopocentricOrigin, Angle.FromDegree(55));
            parameters.Add(CoordinateOperationParameters.LongitudeOfTopocentricOrigin, Angle.FromDegree(5));
            parameters.Add(CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin, Length.FromMetre(200));

            _orthographicProjection = new VerticalPerspectiveOrthographicProjection(parameters, Ellipsoids.WGS1984);
        }

        [TestCase]
        public void VerticalPerspectiveProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            Coordinate expected = new Coordinate(-188878.767, -128550.09);
            Coordinate transformed = _projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void VerticalPerspectiveOrthographicProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            Coordinate expected = new Coordinate(-189013.869, -128642.04);
            Coordinate transformed = _orthographicProjection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }
    }
}
