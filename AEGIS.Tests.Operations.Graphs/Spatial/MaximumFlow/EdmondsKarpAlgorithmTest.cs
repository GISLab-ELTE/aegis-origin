/// <copyright file="EdmondsKarpAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
using ELTE.AEGIS.Operations.Spatial;
using ELTE.AEGIS.Operations.Spatial.MaximumFlow;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spatial.MaximumFlow
{
    /// <summary>
    /// Test fixture for class <see cref="EdmondsKarpAlgorithm" />.
    /// </summary>
    [TestFixture]
    public class EdmondsKarpAlgorithmTest
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

        /// <summary>
        /// The target vertex.
        /// </summary>
        private IGraphVertex _targetVertex;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // source: http://en.wikipedia.org/wiki/Edmonds%E2%80%93Karp_algorithm

            IGeometryFactory factory = new GeometryFactory();

            // source graph
            _sourceGraph = factory.CreateNetwork();

            // vertices
            IGraphVertex vertexA = _sourceGraph.AddVertex(new Coordinate(0, 2));
            IGraphVertex vertexB = _sourceGraph.AddVertex(new Coordinate(0, 0));
            IGraphVertex vertexC = _sourceGraph.AddVertex(new Coordinate(1, 1));
            IGraphVertex vertexD = _sourceGraph.AddVertex(new Coordinate(2, 2));
            IGraphVertex vertexE = _sourceGraph.AddVertex(new Coordinate(2, 0));
            IGraphVertex vertexF = _sourceGraph.AddVertex(new Coordinate(3, 2));
            IGraphVertex vertexG = _sourceGraph.AddVertex(new Coordinate(3, 0));

            // forward edges
            _sourceGraph.AddEdge(vertexA, vertexB, CreateCapacityMetadata(3));
            _sourceGraph.AddEdge(vertexA, vertexD, CreateCapacityMetadata(3));
            _sourceGraph.AddEdge(vertexB, vertexC, CreateCapacityMetadata(4));
            _sourceGraph.AddEdge(vertexC, vertexA, CreateCapacityMetadata(3));
            _sourceGraph.AddEdge(vertexC, vertexD, CreateCapacityMetadata(1));
            _sourceGraph.AddEdge(vertexC, vertexE, CreateCapacityMetadata(2));
            _sourceGraph.AddEdge(vertexD, vertexE, CreateCapacityMetadata(2));
            _sourceGraph.AddEdge(vertexD, vertexF, CreateCapacityMetadata(6));
            _sourceGraph.AddEdge(vertexE, vertexB, CreateCapacityMetadata(1));
            _sourceGraph.AddEdge(vertexE, vertexG, CreateCapacityMetadata(1));
            _sourceGraph.AddEdge(vertexF, vertexG, CreateCapacityMetadata(9));

            // reverse edges
            _sourceGraph.AddEdge(vertexB, vertexA, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexD, vertexA, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexC, vertexB, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexA, vertexC, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexD, vertexC, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexE, vertexC, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexE, vertexD, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexF, vertexD, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexB, vertexE, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexG, vertexE, CreateCapacityMetadata(0));
            _sourceGraph.AddEdge(vertexG, vertexF, CreateCapacityMetadata(0));

            // source and target vertices
            _sourceVertex = vertexA;
            _targetVertex = vertexG;

            // result graph
            _resultGraph = factory.CreateNetwork();

            // vertices
            vertexA = _resultGraph.AddVertex(new Coordinate(0, 2));
            vertexB = _resultGraph.AddVertex(new Coordinate(0, 0));
            vertexC = _resultGraph.AddVertex(new Coordinate(1, 1));
            vertexD = _resultGraph.AddVertex(new Coordinate(2, 2));
            vertexE = _resultGraph.AddVertex(new Coordinate(2, 0));
            vertexF = _resultGraph.AddVertex(new Coordinate(3, 2));
            vertexG = _resultGraph.AddVertex(new Coordinate(3, 0));

            // edges
            _resultGraph.AddEdge(vertexA, vertexB, CreateResidualCapacityMetadata(1));
            _resultGraph.AddEdge(vertexA, vertexD, CreateResidualCapacityMetadata(0));
            _resultGraph.AddEdge(vertexB, vertexC, CreateResidualCapacityMetadata(2));
            _resultGraph.AddEdge(vertexC, vertexA, CreateResidualCapacityMetadata(3));
            _resultGraph.AddEdge(vertexC, vertexD, CreateResidualCapacityMetadata(0));
            _resultGraph.AddEdge(vertexC, vertexE, CreateResidualCapacityMetadata(1));
            _resultGraph.AddEdge(vertexD, vertexE, CreateResidualCapacityMetadata(2));
            _resultGraph.AddEdge(vertexD, vertexF, CreateResidualCapacityMetadata(2));
            _resultGraph.AddEdge(vertexE, vertexB, CreateResidualCapacityMetadata(1));
            _resultGraph.AddEdge(vertexE, vertexG, CreateResidualCapacityMetadata(0));
            _resultGraph.AddEdge(vertexF, vertexG, CreateResidualCapacityMetadata(5));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests operation execution.
        /// </summary>
        [Test]
        public void EdmondsKarpAlgorithmExecuteTest()
        {
            Func<IGraphEdge, Int32> capacityFunction = edge => (Int32)(edge["Capacity"]);

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters[GraphOperationParameters.SourceVertex] = _sourceVertex;
            parameters[GraphOperationParameters.TargetVertex] = _targetVertex;
            parameters[GraphOperationParameters.CapacityMetric] = capacityFunction;

            EdmondsKarpAlgorithm operation = new EdmondsKarpAlgorithm(_sourceGraph, parameters);

            operation.Execute();

            Assert.AreEqual(_resultGraph.VertexCount, operation.Result.VertexCount);
            Assert.AreEqual(5, operation.Result["MaximumFlow"]);

            foreach (IGraphEdge edge in _resultGraph.Edges)
            {
                IGraphEdge resultEdge = operation.Result.Edges.FirstOrDefault(e => e.Source.Coordinate.Equals(edge.Source.Coordinate) && e.Target.Coordinate.Equals(edge.Target.Coordinate));

                Assert.IsNotNull(resultEdge);
                Assert.AreEqual(edge["ResidualCapacity"], resultEdge["ResidualCapacity"]);
            }
        }

        #endregion

        #region  Private methods

        /// <summary>
        /// Creates metadata containing a capacity value.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns>The produced metadata.</returns>
        private IDictionary<String, Object> CreateCapacityMetadata(Int32 capacity)
        {
            IDictionary<String, Object> metadata = new Dictionary<String, Object>();
            metadata["Capacity"] = capacity;

            return metadata;
        }

        /// <summary>
        /// Creates metadata containing a residual capacity value.
        /// </summary>
        /// <param name="capacity">The residual capacity.</param>
        /// <returns>The produced metadata.</returns>
        private IDictionary<String, Object> CreateResidualCapacityMetadata(Int32 capacity)
        {
            IDictionary<String, Object> metadata = new Dictionary<String, Object>();
            metadata["ResidualCapacity"] = capacity;

            return metadata;
        }

        #endregion
    }
}
