/// <copyright file="ReferenceTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Reference;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Operations.Reference
{
    [TestFixture]
    public class ReferenceTransformationTest
    {
        [TestCase]
        public void ReferenceTransformationExecutePointTest()
        {
            // test case 1: projected to projected

            Coordinate sourceCoordinate = new Coordinate(10, 10);
            GeoCoordinate sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Reverse(sourceCoordinate);
            GeoCoordinate expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);
            Coordinate expectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Forward(expectedGeoCoordinate);

            IPoint sourcePoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreatePoint(10, 10);

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            ReferenceTransformation transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            IPoint resultPoint = transformation.Result as IPoint;

            // test case 2: geographic to projected

            Assert.AreEqual(expectedCoordinate, resultPoint.Coordinate);

            sourceGeoCoordinate = new GeoCoordinate(Angle.FromDegree(45), Angle.FromDegree(45));
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);
            expectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Forward(expectedGeoCoordinate);

            sourcePoint = Factory.GetInstance<IGeometryFactory>(Geographic2DCoordinateReferenceSystems.HD72).CreatePoint(45, 45);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(expectedCoordinate, resultPoint.Coordinate);

            // test case 3: projected to geographic

            sourceCoordinate = new Coordinate(10, 10);
            sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Reverse(sourceCoordinate);
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);

            sourcePoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreatePoint(sourceCoordinate);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, Geographic2DCoordinateReferenceSystems.WGS84);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(expectedGeoCoordinate.Latitude.GetValue(UnitsOfMeasurement.Degree), resultPoint.Coordinate.X);
            Assert.AreEqual(expectedGeoCoordinate.Longitude.GetValue(UnitsOfMeasurement.Degree), resultPoint.Coordinate.Y);

            // test case 4: projected to projected (reverse)

            sourceCoordinate = new Coordinate(10, 10);
            sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Reverse(sourceCoordinate);
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Reverse(sourceGeoCoordinate);
            expectedCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Forward(expectedGeoCoordinate);

            sourcePoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.WGS84_WorldMercator).CreatePoint(10, 10);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.HD72_EOV);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(expectedCoordinate, resultPoint.Coordinate);

            // test case 5: same reference system

            sourcePoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.WGS84_WorldMercator).CreatePoint(10, 10);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(sourcePoint.Coordinate, resultPoint.Coordinate);

            // test case 6: no source reference system

            sourcePoint = Factory.DefaultInstance<IGeometryFactory>().CreatePoint(10, 10);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(sourcePoint.Coordinate, resultPoint.Coordinate);

            // test case 7: no target reference system

            try
            {
                parameters = new Dictionary<OperationParameter, Object>();
                parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, null);

                transformation = new ReferenceTransformation(sourcePoint, parameters);

                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        [TestCase]
        public void ReferenceTransformationExecuteGeometryTest()
        {
            // TODO: test all geometry types for conversion success
        }
    }
}
