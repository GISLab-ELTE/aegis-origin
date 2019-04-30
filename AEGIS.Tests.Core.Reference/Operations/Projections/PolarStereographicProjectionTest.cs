/// <copyright file="PolarStereographicProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class PolarStereographicProjectionTest
    {
        private PolarStereographicAProjection _projectionA;
        private PolarStereographicBProjection _projectionB;
        private PolarStereographicCProjection _projectionC;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(90));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.994);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(2000000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(2000000));

            _projectionA = new PolarStereographicAProjection("EPSG::16061", "Universal Polar Stereographic North", parameters, Ellipsoids.WGS1984, AreasOfUse.World_60NTo90N);

            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfStandardParallel, Angle.FromDegree(-71));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(70));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(6000000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(6000000));

            _projectionB = new PolarStereographicBProjection("EPSG::19993", "Australian Antarctic Polar Stereographic", parameters, Ellipsoids.WGS1984, AreasOfUse.World);

            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfStandardParallel, Angle.FromDegree(-67));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(140));
            parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(300000));
            parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(200000));

            _projectionC = new PolarStereographicCProjection("EPSG::19983", "Terre Adelie Polar Stereographic ", parameters, Ellipsoids.International1924, AreasOfUse.World);
        }

        [TestCase]
        public void PolarStereographicAProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(73), Angle.FromDegree(44));
            Coordinate expected = new Coordinate(3320416.75, 632668.43);
            Coordinate transformed = _projectionA.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void PolarStereographicAProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(73), Angle.FromDegree(44));
            GeoCoordinate transformed = _projectionA.Reverse(_projectionA.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void PolarStereographicBProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(-75), Angle.FromDegree(120));
            Coordinate expected = new Coordinate(7255380.79, 7053389.56);
            Coordinate transformed = _projectionB.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void PolarStereographicBProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-75), Angle.FromDegree(120));
            GeoCoordinate transformed = _projectionB.Reverse(_projectionB.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void PolarStereographicCProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(-66, 36, 18.820), Angle.FromDegree(140, 04, 17.04));
            Coordinate expected = new Coordinate(303169.52, 244055.72);
            Coordinate transformed = _projectionC.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void PolarStereographicCProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-66, 36, 18.820), Angle.FromDegree(140, 04, 17.04));
            GeoCoordinate transformed = _projectionC.Reverse(_projectionC.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
