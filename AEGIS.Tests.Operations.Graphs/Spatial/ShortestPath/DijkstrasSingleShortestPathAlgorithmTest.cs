/// <copyright file="DijsktraSingleShortestPathAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spatial;
using ELTE.AEGIS.Operations.Spatial.ShortestPath;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spatial.ShortestPath
{
    [TestFixture]
    public class DijkstrasSingleShortestPathAlgorithmTest
    {
        private Mock<IReferenceSystem> _referenceSystemMock;
        private List<IGeometryGraph> _graphs;

        [SetUp]
        public void SetUp()
        {
            _referenceSystemMock = new Mock<IReferenceSystem>(MockBehavior.Strict);

            _graphs = new List<IGeometryGraph>();

            // connected simple graph
            IGeometryGraph graph = new GeometryNetwork(_referenceSystemMock.Object, null);
            IGraphVertex v1 = graph.AddVertex(new Coordinate(10, 10));
            IGraphVertex v2 = graph.AddVertex(new Coordinate(20, 10));
            IGraphVertex v3 = graph.AddVertex(new Coordinate(20, 20));
            IGraphVertex v4 = graph.AddVertex(new Coordinate(10, 20));

            graph.AddEdge(v1, v2);
            graph.AddEdge(v2, v3);
            graph.AddEdge(v1, v3);
            graph.AddEdge(v3, v4);
            graph.AddEdge(v3, v2);
            graph.AddEdge(v3, v1);
            graph.AddEdge(v2, v3);
            graph.AddEdge(v3, v2);
            graph.AddEdge(v2, v1);
            graph.AddEdge(v4, v3);
            graph.AddEdge(v4, v1);

            _graphs.Add(graph);

            // disconnected simple graph
            graph = new GeometryNetwork(_referenceSystemMock.Object, null);
            v1 = graph.AddVertex(new Coordinate(10, 10));
            v2 = graph.AddVertex(new Coordinate(20, 10));
            v3 = graph.AddVertex(new Coordinate(20, 20));
            v4 = graph.AddVertex(new Coordinate(10, 20));

            graph.AddEdge(v1, v2);
            graph.AddEdge(v2, v3);
            graph.AddEdge(v1, v3);
            graph.AddEdge(v2, v1);

            _graphs.Add(graph);
        }

        [TestCase]
        public void DijkstraSingleShortestPathAlgorithmInitializeTest()
        {
            foreach (IGeometryGraph graph in _graphs)
            {
                IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
                parameters[GraphOperationParameters.SourceVertex] = graph.GetNearestVertex(new Coordinate(10, 10));
                parameters[GraphOperationParameters.TargetVertex] = graph.GetNearestVertex(new Coordinate(10, 20));

                DijkstrasSingleShortestPathAlgorithm dijkstraAlgorithm = new DijkstrasSingleShortestPathAlgorithm(graph, parameters);

                Assert.AreEqual(GraphOperationMethods.DijkstrasSinlgeShortestPathAlgorithm, dijkstraAlgorithm.Method);
                Assert.AreEqual(OperationState.Initialized, dijkstraAlgorithm.State);
                Assert.AreEqual(false, dijkstraAlgorithm.IsReversible);
                Assert.AreEqual(graph, dijkstraAlgorithm.Source);
                Assert.AreEqual(graph.GetNearestVertex(new Coordinate(10, 10)), dijkstraAlgorithm.Parameters[GraphOperationParameters.SourceVertex]);
                Assert.AreEqual(graph.GetNearestVertex(new Coordinate(10, 20)), dijkstraAlgorithm.Parameters[GraphOperationParameters.TargetVertex]);
                Assert.Throws<NotSupportedException>(() => dijkstraAlgorithm.GetReverseOperation());
            }
        }

        [TestCase]
        public void AStarAlgorithmExecuteTest()
        {
            // test case 1: connected simple graph
            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters[GraphOperationParameters.SourceVertex] = _graphs[0].GetNearestVertex(new Coordinate(10, 10));
            parameters[GraphOperationParameters.TargetVertex] = _graphs[0].GetNearestVertex(new Coordinate(10, 20));

            DijkstrasSingleShortestPathAlgorithm dijkstraAlgorithm = new DijkstrasSingleShortestPathAlgorithm(_graphs[0], parameters);

            Assert.AreEqual(OperationState.Initialized, dijkstraAlgorithm.State);

            dijkstraAlgorithm.Execute();

            Assert.AreEqual(OperationState.Finished, dijkstraAlgorithm.State);

            List<Coordinate> expectedCoordinates = new List<Coordinate> { new Coordinate(10, 10), new Coordinate(20, 20), new Coordinate(10, 20) };

            Assert.AreEqual(expectedCoordinates.Count, dijkstraAlgorithm.Result.VertexCount);
            Assert.AreEqual(expectedCoordinates.Count - 1, dijkstraAlgorithm.Result.EdgeCount);

            IGraphVertex vertex = dijkstraAlgorithm.Result.GetVertex(expectedCoordinates[0]);
            Assert.IsNotNull(vertex);

            for (Int32 i = 1; i < expectedCoordinates.Count; i++)
            {
                IGraphVertex nextVertex = dijkstraAlgorithm.Result.GetVertex(expectedCoordinates[i]);

                Assert.IsNotNull(nextVertex);
                Assert.AreEqual(1, dijkstraAlgorithm.Result.OutEdges(vertex).Count());
                Assert.IsTrue(dijkstraAlgorithm.Result.OutEdges(vertex).Any(edge => edge.Target == nextVertex));

                vertex = nextVertex;
            }

            // test case 2: disconnected simple graph

            parameters = new Dictionary<OperationParameter, Object>();
            parameters[GraphOperationParameters.SourceVertex] = _graphs[1].GetNearestVertex(new Coordinate(10, 10));
            parameters[GraphOperationParameters.TargetVertex] = _graphs[1].GetNearestVertex(new Coordinate(10, 20));

            dijkstraAlgorithm = new DijkstrasSingleShortestPathAlgorithm(_graphs[1], parameters);

            Assert.AreEqual(OperationState.Initialized, dijkstraAlgorithm.State);

            dijkstraAlgorithm.Execute();

            Assert.AreEqual(OperationState.Finished, dijkstraAlgorithm.State);

            Assert.IsNull(dijkstraAlgorithm.Result);
        }
    }
}
