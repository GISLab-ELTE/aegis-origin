/// <copyright file="SimilarityTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class SimilarityTransformationTest
    {
        private SimilarityTransformation _transformation;

        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget, Length.FromMetre(-129.549));
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget, Length.FromMetre(-208.185));
            parameters.Add(CoordinateOperationParameters.XAxisRotation, Angle.FromArcSecond(1.56504));
            parameters.Add(CoordinateOperationParameters.ScaleDifference, 1.55);

            _transformation = new SimilarityTransformation("EPSG::9621", "UTM zone 31N", parameters);
        }

        [TestCase]
        public void SimilarityTransformationForward()
        {
            Coordinate coordinate = new Coordinate(300000, 4500000);
            Coordinate expected = new Coordinate(299905.060, 4499796.515);
            Coordinate transformed = _transformation.Forward(coordinate);

            Assert.AreEqual(transformed.X, expected.X, 1000);
            Assert.AreEqual(transformed.Y, expected.Y, 1000);
        }

        [TestCase]
        public void SimilarityTransformationReverse()
        {
            Coordinate expected = new Coordinate(300000, 4500000);
            Coordinate transformed = _transformation.Reverse(_transformation.Forward(expected));

            Assert.AreEqual(transformed.X, expected.X, 1000);
            Assert.AreEqual(transformed.Y, expected.Y, 1000);
        }
    }
}
