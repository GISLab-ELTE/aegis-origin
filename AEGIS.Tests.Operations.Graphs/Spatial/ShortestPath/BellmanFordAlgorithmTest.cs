/// <copyright file="BellmanFordAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spatial.ShortestPath;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.Tests.Operations.Spatial.ShortestPath
{
    /// <summary>
    /// Test fixture for class <see cref="BellmanFordAlgorithm" />.
    /// </summary>
    [TestFixture]
    public class BellmanFordAlgorithmTest
    {
        #region Private fields

        /// <summary>
        /// The mock of the reference system.
        /// </summary>
        private Mock<IReferenceSystem> _referenceSystemMock;

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
            _referenceSystemMock = new Mock<IReferenceSystem>(MockBehavior.Strict);

            // source graph
            _sourceGraph = Factory.GetInstance<GeometryGraphFactory>(_referenceSystemMock.Object).CreateNetwork();

            IGraphVertex vertex1 = _sourceGraph.AddVertex(new Coordinate(0, 1));
            IGraphVertex vertex2 = _sourceGraph.AddVertex(new Coordinate(1, 1));
            IGraphVertex vertex3 = _sourceGraph.AddVertex(new Coordinate(1, 0));
            IGraphVertex vertex4 = _sourceGraph.AddVertex(new Coordinate(2, 0));
            IGraphVertex vertex5 = _sourceGraph.AddVertex(new Coordinate(0, 0));

            _sourceGraph.AddEdge(vertex1, vertex2, CreateWeightMetadata(5));
            _sourceGraph.AddEdge(vertex1, vertex3, CreateWeightMetadata(8));
            _sourceGraph.AddEdge(vertex1, vertex4, CreateWeightMetadata(-4));
            _sourceGraph.AddEdge(vertex2, vertex1, CreateWeightMetadata(-2));
            _sourceGraph.AddEdge(vertex3, vertex2, CreateWeightMetadata(-3));
            _sourceGraph.AddEdge(vertex3, vertex4, CreateWeightMetadata(9));
            _sourceGraph.AddEdge(vertex4, vertex2, CreateWeightMetadata(7));
            _sourceGraph.AddEdge(vertex5, vertex1, CreateWeightMetadata(6));
            _sourceGraph.AddEdge(vertex5, vertex3, CreateWeightMetadata(7));

            // source vertex
            _sourceVertex = vertex5;

            // result graph
            _resultGraph = Factory.GetInstance<GeometryGraphFactory>(_referenceSystemMock.Object).CreateNetwork();

            vertex1 = _resultGraph.AddVertex(new Coordinate(0, 1), CreateDistanceMetadata(2));
            vertex2 = _resultGraph.AddVertex(new Coordinate(1, 1), CreateDistanceMetadata(4));
            vertex3 = _resultGraph.AddVertex(new Coordinate(1, 0), CreateDistanceMetadata(7));
            vertex4 = _resultGraph.AddVertex(new Coordinate(2, 0), CreateDistanceMetadata(-2));
            vertex5 = _resultGraph.AddVertex(new Coordinate(0, 0), CreateDistanceMetadata(0));

            _resultGraph.AddEdge(vertex5, vertex3);
            _resultGraph.AddEdge(vertex3, vertex2);
            _resultGraph.AddEdge(vertex2, vertex1);
            _resultGraph.AddEdge(vertex1, vertex4);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for operation execution.
        /// </summary>
        [Test]
        public void BellmanFordExecuteTest()
        {
            Func<IGraphEdge, Double> weightFunction = edge => Convert.ToDouble(edge["Weight"]);

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters[GraphOperationParameters.SourceVertex] = _sourceVertex;
            parameters[GraphOperationParameters.WeightMetric] = weightFunction;

            BellmanFordAlgorithm operation = new BellmanFordAlgorithm(_sourceGraph, parameters);

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
