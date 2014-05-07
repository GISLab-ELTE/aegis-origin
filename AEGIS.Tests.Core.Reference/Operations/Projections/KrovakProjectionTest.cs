/// <copyright file="KrovakProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class KrovakProjectionTest
    {
        private KrovakProjection _krovakProjection;
        private KrovakNorthOrientedProjection _krovakNorthOrientedProjection;
        private KrovakModifiedProjection _krovakModifiedProjection;
        private KrovakModifiedNorthOrientedProjection _krovakModifiedNorthOrientedProjection;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(49, 30, 0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(24, 50, 0));
            parameters.Add(CoordinateOperationParameters.CoLatitudeOfConeAxis, Angle.FromDegree(30, 17, 17.303));
            parameters.Add(CoordinateOperationParameters.LatitudeOfPseudoStandardParallel, Angle.FromDegree(78, 30, 0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel, 0.9999);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            _krovakProjection = new KrovakProjection("EPSG::2065", "S-JTSK (Ferro) / Krovak", parameters, Ellipsoids.Bessel1841, AreasOfUse.World);
            
            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(49, 30, 0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(24, 50, 0));
            parameters.Add(CoordinateOperationParameters.CoLatitudeOfConeAxis, Angle.FromDegree(30, 17, 17.303));
            parameters.Add(CoordinateOperationParameters.LatitudeOfPseudoStandardParallel, Angle.FromDegree(78, 30, 0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel, 0.9999);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            _krovakNorthOrientedProjection = new KrovakNorthOrientedProjection("ESPG::5225", "S-JTSK/05 (Ferro) / Modified Krovak East North", parameters, Ellipsoids.Bessel1841, AreasOfUse.World);
            
            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(49, 30, 0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(24, 50, 0));
            parameters.Add(CoordinateOperationParameters.CoLatitudeOfConeAxis, Angle.FromDegree(30, 17, 17.3031));
            parameters.Add(CoordinateOperationParameters.LatitudeOfPseudoStandardParallel, Angle.FromDegree(78, 30, 0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel, 0.9999);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(5000000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(5000000));
            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPoint, Length.FromMetre(1089000));
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPoint, Length.FromMetre(654000));
            parameters.Add(CoordinateOperationParameters.C1, 2.946529277 * Math.Pow(10, -2));
            parameters.Add(CoordinateOperationParameters.C2, 2.515965696 * Math.Pow(10, -2));
            parameters.Add(CoordinateOperationParameters.C3, 1.193845912 * Math.Pow(10, -7));
            parameters.Add(CoordinateOperationParameters.C4, -4.668270147 * Math.Pow(10, -7));
            parameters.Add(CoordinateOperationParameters.C5, 9.233980362 * Math.Pow(10, -12));
            parameters.Add(CoordinateOperationParameters.C6, 1.523735715 * Math.Pow(10, -12));
            parameters.Add(CoordinateOperationParameters.C7, 1.696780024 * Math.Pow(10, -18));
            parameters.Add(CoordinateOperationParameters.C8, 4.408314235 * Math.Pow(10, -18));
            parameters.Add(CoordinateOperationParameters.C9, -8.331083518 * Math.Pow(10, -24));
            parameters.Add(CoordinateOperationParameters.C10, -3.689471323 * Math.Pow(10, -24));

            _krovakModifiedProjection = new KrovakModifiedProjection("ESPG::1042", "Czech Republic and Slovakia", parameters, Ellipsoids.Bessel1841, AreasOfUse.World);
            
            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(49, 30, 0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(24, 50, 0));
            parameters.Add(CoordinateOperationParameters.CoLatitudeOfConeAxis, Angle.FromDegree(30, 17, 17.3031));
            parameters.Add(CoordinateOperationParameters.LatitudeOfPseudoStandardParallel, Angle.FromDegree(78, 30, 0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel, 0.9999);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(5000000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(5000000));
            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPoint, Length.FromMetre(1089000));
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPoint, Length.FromMetre(654000));
            parameters.Add(CoordinateOperationParameters.C1, 2.946529277 * Math.Pow(10, -2));
            parameters.Add(CoordinateOperationParameters.C2, 2.515965696 * Math.Pow(10, -2));
            parameters.Add(CoordinateOperationParameters.C3, 1.193845912 * Math.Pow(10, -7));
            parameters.Add(CoordinateOperationParameters.C4, -4.668270147 * Math.Pow(10, -7));
            parameters.Add(CoordinateOperationParameters.C5, 9.233980362 * Math.Pow(10, -12));
            parameters.Add(CoordinateOperationParameters.C6, 1.523735715 * Math.Pow(10, -12));
            parameters.Add(CoordinateOperationParameters.C7, 1.696780024 * Math.Pow(10, -18));
            parameters.Add(CoordinateOperationParameters.C8, 4.408314235 * Math.Pow(10, -18));
            parameters.Add(CoordinateOperationParameters.C9, -8.331083518 * Math.Pow(10, -24));
            parameters.Add(CoordinateOperationParameters.C10, -3.689471323 * Math.Pow(10, -24));

            _krovakModifiedNorthOrientedProjection = new KrovakModifiedNorthOrientedProjection("ESPG::1043", "Czech Republic and Slovakia", parameters, Ellipsoids.Bessel1841, AreasOfUse.World);
        }

        [TestCase]
        public void KrovakProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442), Angle.FromDegree(16, 50, 59.179));
            Coordinate expected = new Coordinate(568991, 1050538.63);
            Coordinate transformed = _krovakProjection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void KrovakProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442), Angle.FromDegree(16, 50, 59.179));
            GeoCoordinate transformer = _krovakProjection.Reverse(_krovakProjection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformer.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformer.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void KrovakNorthOrientedProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442), Angle.FromDegree(16, 50, 59.179));
            Coordinate expected = new Coordinate(-568991, -1050538.63);
            Coordinate transformed = _krovakNorthOrientedProjection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void KrovakNorthOrientedProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442), Angle.FromDegree(16, 50, 59.179));
            GeoCoordinate transformer = _krovakNorthOrientedProjection.Reverse(_krovakNorthOrientedProjection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformer.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformer.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void KrovakModifiedProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442).BaseValue, Angle.FromDegree(34, 30, 59.179).BaseValue + Meridians.Ferro.Longitude.BaseValue);
            Coordinate expected = new Coordinate(Length.FromMetre(5568990.91).BaseValue, Length.FromMetre(6050538.71).BaseValue);
            Coordinate transformed = _krovakModifiedProjection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 1);
            Assert.AreEqual(expected.Y, transformed.Y, 1);
        }

        [TestCase]
        public void KrovakModifiedProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromRadian(0.876312568), Angle.FromRadian(0.602425500));
            GeoCoordinate transformer = _krovakModifiedProjection.Reverse(_krovakModifiedProjection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformer.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformer.Longitude.BaseValue, 0.00000001);
        }

        [TestCase]
        public void KrovakModifiedNorthOrientedProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442).BaseValue, Angle.FromDegree(34, 30, 59.179).BaseValue + Meridians.Ferro.Longitude.BaseValue);
            Coordinate expected = new Coordinate(Length.FromMetre(-5568990.91).BaseValue, Length.FromMetre(-6050538.71).BaseValue);
            Coordinate transformed = _krovakModifiedNorthOrientedProjection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 1);
            Assert.AreEqual(expected.Y, transformed.Y, 1);
        }

        [TestCase]
        public void KrovakModifiedNorthOrientedProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromRadian(0.876312568), Angle.FromRadian(0.602425500));
            GeoCoordinate transformer = _krovakModifiedNorthOrientedProjection.Reverse(_krovakModifiedNorthOrientedProjection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformer.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformer.Longitude.BaseValue, 0.00000001);
        }
    }
}
