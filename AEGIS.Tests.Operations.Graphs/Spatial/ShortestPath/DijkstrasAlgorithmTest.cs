// <copyright file="DijkstrasAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Operations.Spatial.ShortestPath;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spatial.ShortestPath
{
    /// <summary>
    /// Test fixture for class <see cref="DijkstrasAlgorithm" />.
    /// </summary>
    [TestFixture]
    public class DijkstrasAlgorithmTest
    {
        #region Private fields

        /// <summary>
        /// The source graph.
        /// </summary>
        private IGeometryGraph _sourceGraph;

        /// <summary>
        /// The result graph.
        /// </summary>
        private IGeometryGraph _resultGraph;

        /// <summary>
        /// The source vertex.
        /// </summary>
        private IGraphVertex _sourceVertex;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // source: http://en.wikipedia.org/wiki/Dijkstra%27s_algorithm

            IGeometryFactory factory = new GeometryFactory();

            // source graph
            _sourceGraph = factory.CreateNetwork();

            IGraphVertex vertex1 = _sourceGraph.AddVertex(new Coordinate(0, 0));
            IGraphVertex vertex2 = _sourceGraph.AddVertex(new Coordinate(1, 0));
            IGraphVertex vertex3 = _sourceGraph.AddVertex(new Coordinate(1, 1));
            IGraphVertex vertex4 = _sourceGraph.AddVertex(new Coordinate(2, 1));
            IGraphVertex vertex5 = _sourceGraph.AddVertex(new Coordinate(1, 2));
            IGraphVertex vertex6 = _sourceGraph.AddVertex(new Coordinate(0, 2));

            _sourceGraph.AddEdge(vertex1, vertex2, CreateWeightMetadata(7));
            _sourceGraph.AddEdge(vertex1, vertex3, CreateWeightMetadata(9));
            _sourceGraph.AddEdge(vertex1, vertex6, CreateWeightMetadata(14));
            _sourceGraph.AddEdge(vertex2, vertex3, CreateWeightMetadata(10));
            _sourceGraph.AddEdge(vertex2, vertex4, CreateWeightMetadata(15));
            _sourceGraph.AddEdge(vertex3, vertex4, CreateWeightMetadata(11));
            _sourceGraph.AddEdge(vertex3, vertex6, CreateWeightMetadata(2));
            _sourceGraph.AddEdge(vertex4, vertex5, CreateWeightMetadata(6));
            _sourceGraph.AddEdge(vertex5, vertex6, CreateWeightMetadata(9));

            _sourceGraph.AddEdge(vertex2, vertex1, CreateWeightMetadata(7));
            _sourceGraph.AddEdge(vertex3, vertex1, CreateWeightMetadata(9));
            _sourceGraph.AddEdge(vertex6, vertex1, CreateWeightMetadata(14));
            _sourceGraph.AddEdge(vertex3, vertex2, CreateWeightMetadata(10));
            _sourceGraph.AddEdge(vertex4, vertex2, CreateWeightMetadata(15));
            _sourceGraph.AddEdge(vertex4, vertex3, CreateWeightMetadata(11));
            _sourceGraph.AddEdge(vertex6, vertex3, CreateWeightMetadata(2));
            _sourceGraph.AddEdge(vertex5, vertex4, CreateWeightMetadata(6));
            _sourceGraph.AddEdge(vertex6, vertex5, CreateWeightMetadata(9));

            // source vertex
            _sourceVertex = vertex1;

            // result graph
            _resultGraph = factory.CreateNetwork();

            vertex1 = _resultGraph.AddVertex(new Coordinate(0, 0), CreateDistanceMetadata(0));
            vertex2 = _resultGraph.AddVertex(new Coordinate(1, 0), CreateDistanceMetadata(7));
            vertex3 = _resultGraph.AddVertex(new Coordinate(1, 1), CreateDistanceMetadata(9));
            vertex4 = _resultGraph.AddVertex(new Coordinate(2, 1), CreateDistanceMetadata(20));
            vertex5 = _resultGraph.AddVertex(new Coordinate(1, 2), CreateDistanceMetadata(20));
            vertex6 = _resultGraph.AddVertex(new Coordinate(0, 2), CreateDistanceMetadata(11));

            _resultGraph.AddEdge(vertex1, vertex2);
            _resultGraph.AddEdge(vertex1, vertex3);
            _resultGraph.AddEdge(vertex3, vertex4);
            _resultGraph.AddEdge(vertex3, vertex6);
            _resultGraph.AddEdge(vertex6, vertex5);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests operation execution.
        /// </summary>
        [Test]
        public void DijkstrasSinglePathAlgorithmExecuteTest()
        {
            Func<IGraphEdge, Double> weightFunction = edge => Convert.ToDouble(edge["Weight"]);

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters[GraphOperationParameters.SourceVertex] = _sourceVertex;
            parameters[GraphOperationParameters.WeightMetric] = weightFunction;

            DijkstrasAlgorithm operation = new DijkstrasAlgorithm(_sourceGraph, parameters);

            operation.Execute();

            Assert.AreEqual(_resultGraph.VertexCount, operation.Result.VertexCount);
            Assert.AreEqual(_resultGraph.EdgeCount, operation.Result.EdgeCount);

            foreach (IGraphVertex resultVertex in operation.Result.Vertices)
            {
                IGraphVertex vertex = _resultGraph.GetVertex(resultVertex.Coordinate);

                Assert.IsNotNull(vertex);
                Assert.AreEqual(vertex["Distance"], resultVertex["Distance"]);

                Assert.IsTrue(operation.Result.OutEdges(resultVertex).All(edge => _resultGraph.GetAllEdges(edge.Source.Coordinate, edge.Target.Coordinate).Count == 1));
            }
        }

        #endregion

        #region  Private methods

        /// <summary>
        /// Creates metadata containing a weight value.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <returns>The produced metadata.</returns>
        private IDictionary<String, Object> CreateWeightMetadata(Double weight)
        {
            IDictionary<String, Object> metadata = new Dictionary<String, Object>();
            metadata["Weight"] = weight;

            return metadata;
        }

        /// <summary>
        /// Creates metadata containing a distance value.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <returns>The produced metadata.</returns>
        private IDictionary<String, Object> CreateDistanceMetadata(Double distance)
        {
            IDictionary<String, Object> metadata = new Dictionary<String, Object>();
            metadata["Distance"] = distance;

            return metadata;
        }

        #endregion
    }
}
