/// <copyright file="CoordinateProjectionFactoryTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AEGIS.Tests.Reference.Operations
{
    [TestFixture]
    public class CoordinateProjectionFactoryTest
    {
        [TestCase]
        public void CoordinateProjectionFactoryFromMethodIdentifierTest()
        {
            CoordinateProjection expected = CoordinateProjectionFactory.BritishNationalGrid(Ellipsoids.WGS1984);

            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(49));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-2));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(400000), Ellipsoids.WGS1984.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(-100000), Ellipsoids.WGS1984.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9996012717);

            CoordinateProjection actual = CoordinateProjectionFactory.FromMethodIdentifier(expected.Method.Identifier, parameters, Ellipsoids.WGS1984, AreasOfUse.GreatBritainMan).FirstOrDefault();

            Assert.AreEqual(expected.Method, actual.Method);
        }

        [TestCase]
        public void CoordinateProjectionFactoryFromMethodNameTest()
        {
            CoordinateProjection expected = CoordinateProjectionFactory.BritishNationalGrid(Ellipsoids.WGS1984);

            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(49));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-2));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(400000), Ellipsoids.WGS1984.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(-100000), Ellipsoids.WGS1984.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9996012717);

            CoordinateProjection actual = CoordinateProjectionFactory.FromMethodName(expected.Method.Name, parameters, Ellipsoids.WGS1984, AreasOfUse.GreatBritainMan).FirstOrDefault();

            Assert.AreEqual(expected.Method, actual.Method);
        }
    }
}
