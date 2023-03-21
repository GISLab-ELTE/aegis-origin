// <copyright file="GraphReferenceTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spatial;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Test.Operations.Spatial
{
    /// <summary>
    /// Test fixture for the <see cref="GraphReferenceTransformation" /> class.
    /// </summary>
    [TestFixture]
    public class GraphReferenceTransformationTest
    {
        #region Test methods

        /// <summary>
        /// Tests operation execution.
        /// </summary>
        [Test]
        public void GraphReferenceTransformationExecuteTest()
        {
            // with metadata

            TestExecuteForReferenceSystems(true);
            TestExecuteForGraphs(true);


            // without metadata

            TestExecuteForReferenceSystems(false);
            TestExecuteForGraphs(false);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Tests execution for multiple reference systems.
        /// </summary>
        /// <param name="metadataPreservation">Indicates whether the metadata should be preserved.</param>
        private void TestExecuteForReferenceSystems(Boolean metadataPreservation)
        {
            Coordinate sourceCoordinate = new Coordinate(10, 10);
            IGeometryGraph sourceGraph = new GeometryFactory(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateGraph(new Coordinate[] { sourceCoordinate });


            // projected to projected

            GeoCoordinate sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Reverse(sourceCoordinate);
            GeoCoordinate expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);
            Coordinate expectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Forward(expectedGeoCoordinate);

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);
            parameters.Add(CommonOperationParameters.MetadataPreservation, metadataPreservation);

            GraphReferenceTransformation transformation = new GraphReferenceTransformation(sourceGraph, parameters);
            transformation.Execute();

            IGeometryGraph resultGraph = transformation.Result as IGeometryGraph;

            Assert.AreEqual(1, resultGraph.VertexCount);
            Assert.AreEqual(expectedCoordinate, resultGraph.Vertices[0].Coordinate);
            Assert.AreEqual(sourceGraph.Metadata, resultGraph.Metadata);


            // geographic to projected

            sourceGeoCoordinate = new GeoCoordinate(Angle.FromDegree(45), Angle.FromDegree(45));
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);
            expectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Forward(expectedGeoCoordinate);

            sourceCoordinate = new Coordinate(45, 45);
            sourceGraph = new GeometryFactory(Geographic2DCoordinateReferenceSystems.HD72).CreateGraph(new Coordinate[] { sourceCoordinate });

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new GraphReferenceTransformation(sourceGraph, parameters);
            transformation.Execute();

            resultGraph = transformation.Result as IGeometryGraph;

            Assert.AreEqual(expectedCoordinate, resultGraph.Vertices[0].Coordinate);


            // projected to geographic

            sourceCoordinate = new Coordinate(10, 10);
            sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Reverse(sourceCoordinate);
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);

            sourceGraph = new GeometryFactory(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateGraph(new Coordinate[] { sourceCoordinate });

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, Geographic2DCoordinateReferenceSystems.WGS84);

            transformation = new GraphReferenceTransformation(sourceGraph, parameters);
            transformation.Execute();

            resultGraph = transformation.Result as IGeometryGraph;

            Assert.AreEqual(expectedGeoCoordinate.Latitude.GetValue(UnitsOfMeasurement.Degree), resultGraph.Vertices[0].Coordinate.X);
            Assert.AreEqual(expectedGeoCoordinate.Longitude.GetValue(UnitsOfMeasurement.Degree), resultGraph.Vertices[0].Coordinate.Y);


            // projected to projected (reverse)

            sourceCoordinate = new Coordinate(10, 10);
            sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Reverse(sourceCoordinate);
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Reverse(sourceGeoCoordinate);
            expectedCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Forward(expectedGeoCoordinate);

            sourceGraph = new GeometryFactory(ProjectedCoordinateReferenceSystems.WGS84_WorldMercator).CreateGraph(new Coordinate[] { sourceCoordinate });

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.HD72_EOV);

            transformation = new GraphReferenceTransformation(sourceGraph, parameters);
            transformation.Execute();

            resultGraph = transformation.Result as IGeometryGraph;

            Assert.AreEqual(expectedCoordinate, resultGraph.Vertices[0].Coordinate);


            // same reference system

            sourceGraph = new GeometryFactory(ProjectedCoordinateReferenceSystems.WGS84_WorldMercator).CreateGraph(new Coordinate[] { sourceCoordinate });

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new GraphReferenceTransformation(sourceGraph, parameters);
            transformation.Execute();

            resultGraph = transformation.Result as IGeometryGraph;

            Assert.AreEqual(sourceGraph.Vertices[0].Coordinate, resultGraph.Vertices[0].Coordinate);


            // no source reference system

            sourceGraph = new GeometryFactory().CreateGraph(new Coordinate[] { sourceCoordinate });

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new GraphReferenceTransformation(sourceGraph, parameters);
            transformation.Execute();

            resultGraph = transformation.Result as IGeometryGraph;

            Assert.AreEqual(sourceGraph.Vertices[0].Coordinate, resultGraph.Vertices[0].Coordinate);


            // no target reference system

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, null);

            Assert.Throws<ArgumentException>(() => transformation = new GraphReferenceTransformation(sourceGraph, parameters));
        }

        /// <summary>
        /// Tests execution for multiple graphs.
        /// </summary>
        /// <param name="metadataPreservation">Indicates whether the metadata should be preserved.</param>
        private void TestExecuteForGraphs(Boolean metadataPreservation)
        {
            // empty graph

            IGeometryGraph sourceGraph = new GeometryFactory(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateGraph();

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);
            parameters.Add(CommonOperationParameters.MetadataPreservation, metadataPreservation);

            GraphReferenceTransformation transformation = new GraphReferenceTransformation(sourceGraph, parameters);
            transformation.Execute();

            IGeometryGraph resultGraph = transformation.Result as IGeometryGraph;

            Assert.AreEqual(0, resultGraph.VertexCount);
            Assert.AreEqual(0, resultGraph.EdgeCount);
            Assert.AreEqual(sourceGraph.Metadata, resultGraph.Metadata);


            // graph with points and edges

            Coordinate[] coordinates = new Coordinate[] { new Coordinate(10, 10), new Coordinate(20, 20), new Coordinate(30, 30) };

            sourceGraph = new GeometryFactory(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateGraph();
            IGraphVertex[] vertices = coordinates.Select(coordinate => sourceGraph.AddVertex(coordinate)).ToArray();

            sourceGraph.AddEdge(vertices[0], vertices[1]);
            sourceGraph.AddEdge(vertices[1], vertices[2]);

            transformation = new GraphReferenceTransformation(sourceGraph, parameters);
            transformation.Execute();

            resultGraph = transformation.Result as IGeometryGraph;

            Assert.AreEqual(3, resultGraph.VertexCount);
            Assert.AreEqual(2, resultGraph.EdgeCount);
            Assert.AreEqual(1, resultGraph.OutEdges(resultGraph.Vertices[0]).Count);
            Assert.AreEqual(resultGraph.Vertices[1], resultGraph.OutEdges(resultGraph.Vertices[0]).First().Target);
            Assert.AreEqual(1, resultGraph.OutEdges(resultGraph.Vertices[1]).Count);
            Assert.AreEqual(resultGraph.Vertices[2], resultGraph.OutEdges(resultGraph.Vertices[1]).First().Target);

            for (Int32 vertexIndex = 0; vertexIndex < resultGraph.VertexCount; vertexIndex++)
            {
                GeoCoordinate sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Reverse(coordinates[vertexIndex]);
                GeoCoordinate expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);
                Coordinate expectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Forward(expectedGeoCoordinate);

                Assert.AreEqual(expectedCoordinate, resultGraph.Vertices[vertexIndex].Coordinate);
            }


            // not supported geometry

            Mock<IGeometry> geometryMock = new Mock<IGeometry>(MockBehavior.Loose);
            geometryMock.Setup(geometry => geometry.Factory).Returns(() => new GeometryFactory(ProjectedCoordinateReferenceSystems.HD72_EOV));
            geometryMock.Setup(geometry => geometry.ReferenceSystem).Returns(() => ProjectedCoordinateReferenceSystems.HD72_EOV);

            transformation = new GraphReferenceTransformation(geometryMock.Object, parameters);
            Assert.Throws<InvalidOperationException>(() => transformation.Execute());
        }

        #endregion
    }
}
