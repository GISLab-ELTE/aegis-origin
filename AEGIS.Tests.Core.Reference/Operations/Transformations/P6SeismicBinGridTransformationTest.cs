/// <copyright file="P6SeismicBinGridTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Reference
{
    [TestFixture]
    public class P6SeismicBinGridTransformationTest
    {
        private P6LeftHandedSeismicBinGridTransformation _leftHandedTransformation;
        private P6RightHandedSeismicBinGridTransformation _Transformation;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.BinGridOriginI, 5000);
            parameters.Add(CoordinateOperationParameters.BinGridOriginJ, 0);
            parameters.Add(CoordinateOperationParameters.BinGridOriginEasting, Length.FromUSSurveyFoot(871200));
            parameters.Add(CoordinateOperationParameters.BinGridOriginNorthing, Length.FromUSSurveyFoot(10280160));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOfBinGrid, 1);
            parameters.Add(CoordinateOperationParameters.MapGridBearingOfBinGridJAxis, Angle.FromDegree(340));
            parameters.Add(CoordinateOperationParameters.BinWidthOnIAxis, Length.FromUSSurveyFoot(82.5));
            parameters.Add(CoordinateOperationParameters.BinWidthOnJAxis, Length.FromUSSurveyFoot(41.25));
            parameters.Add(CoordinateOperationParameters.BinNodeIncrementOnIAxis, 1);
            parameters.Add(CoordinateOperationParameters.BinNodeIncrementOnJAxis, 1);

            _leftHandedTransformation = new P6LeftHandedSeismicBinGridTransformation("ESPG::1049", "Left Handed Seismic Bin Grid Transformation", parameters);

            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.BinGridOriginI, 1);
            parameters.Add(CoordinateOperationParameters.BinGridOriginJ, 1);
            parameters.Add(CoordinateOperationParameters.BinGridOriginEasting, Length.FromMetre(456781));
            parameters.Add(CoordinateOperationParameters.BinGridOriginNorthing, Length.FromMetre(5836723));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOfBinGrid, 0.99984);
            parameters.Add(CoordinateOperationParameters.BinWidthOnIAxis, Length.FromMetre(25));
            parameters.Add(CoordinateOperationParameters.BinWidthOnJAxis, Length.FromMetre(12.5));
            parameters.Add(CoordinateOperationParameters.MapGridBearingOfBinGridJAxis, Angle.FromDegree(20));
            parameters.Add(CoordinateOperationParameters.BinNodeIncrementOnIAxis, 1);
            parameters.Add(CoordinateOperationParameters.BinNodeIncrementOnJAxis, 1);

            _Transformation = new P6RightHandedSeismicBinGridTransformation("ESPG::9666", "Right Handed Seismic Bin Grid Transformation", parameters);
        }

        [TestCase]
        public void P6LeftHandedSeismicBinGridTransformationForwardTest()
        {
            Coordinate coordinate = new Coordinate(4700, 247);
            Coordinate expected = new Coordinate(Length.FromUSSurveyFoot(890972.63).BaseValue, Length.FromUSSurveyFoot(10298199.29).BaseValue);
            Coordinate transformed = _leftHandedTransformation.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void P6LeftHandedSeismicBinGridTransformationReverseTest()
        {
            Coordinate expected = new Coordinate(4700, 247);
            Coordinate transformed = _leftHandedTransformation.Reverse(_leftHandedTransformation.Forward(expected));

            Assert.AreEqual(expected.X, transformed.X, 0.0001);
            Assert.AreEqual(expected.Y, transformed.Y, 0.0001);
        }

        [TestCase]
        public void P6RightHandedSeismicBinGridTransformationForwardTest()
        {
            Coordinate coordinate = new Coordinate(300, 247);
            Coordinate expected = new Coordinate(464855.62, 5837055.90);
            Coordinate transformed = _Transformation.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        [TestCase]
        public void P6RightHandedSeismicBinGridTransformationReverseTest()
        {
            Coordinate expected = new Coordinate(300, 247);
            Coordinate transformed = _Transformation.Reverse(_Transformation.Forward(expected));

            Assert.AreEqual(expected.X, transformed.X, 0.0001);
            Assert.AreEqual(expected.Y, transformed.Y, 0.0001);
        }
    }
}
