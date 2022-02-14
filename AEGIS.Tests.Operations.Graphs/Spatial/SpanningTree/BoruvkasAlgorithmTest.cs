/// <copyright file="BoruvkasAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dávid Kis</author>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELTE.AEGIS.Operations.Spatial.SpanningTree;
using NUnit.Framework;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Geometry;
using Moq;
namespace ELTE.AEGIS.Tests.Operations.Spatial.SpanningTree
{
    /// <summary>
    /// Test fixture for class <see cref="BoruvkasAlgorithm" />.
    /// </summary>
    [TestFixture]
    public class BoruvkasAlgorithmTest
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

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _referenceSystemMock = new Mock<IReferenceSystem>(MockBehavior.Strict);
            IGeometryFactory factory = new GeometryFactory(_referenceSystemMock.Object);

            // source: http://en.wikipedia.org/wiki/Bor%C5%AFvka%27s_algorithm

            // source graph
            _sourceGraph = factory.CreateNetwork();

            IGraphVertex vertex1 = _sourceGraph.AddVertex(new Coordinate(0, 0));
            IGraphVertex vertex2 = _sourceGraph.AddVertex(new Coordinate(1, 0));
            IGraphVertex vertex3 = _sourceGraph.AddVertex(new Coordinate(1, 1));
            IGraphVertex vertex4 = _sourceGraph.AddVertex(new Coordinate(2, 1));
            IGraphVertex vertex5 = _sourceGraph.AddVertex(new Coordinate(1, 2)); 
            IGraphVertex vertex6 = _sourceGraph.AddVertex(new Coordinate(0, 2)); 
            IGraphVertex vertex7 = _sourceGraph.AddVertex(new Coordinate(2, 0));

            _sourceGraph.AddEdge(vertex1, vertex2, CreateWeightMetadata(7));
            _sourceGraph.AddEdge(vertex1, vertex4, CreateWeightMetadata(4));
            _sourceGraph.AddEdge(vertex2, vertex3, CreateWeightMetadata(11));
            _sourceGraph.AddEdge(vertex2, vertex4, CreateWeightMetadata(9));
            _sourceGraph.AddEdge(vertex2, vertex5, CreateWeightMetadata(10));
            _sourceGraph.AddEdge(vertex3, vertex5, CreateWeightMetadata(5));
            _sourceGraph.AddEdge(vertex4, vertex5, CreateWeightMetadata(15));
            _sourceGraph.AddEdge(vertex4, vertex6, CreateWeightMetadata(6));
            _sourceGraph.AddEdge(vertex5, vertex6, CreateWeightMetadata(12));
            _sourceGraph.AddEdge(vertex6, vertex7, CreateWeightMetadata(13));
            _sourceGraph.AddEdge(vertex5, vertex7, CreateWeightMetadata(8));

            _sourceGraph.AddEdge(vertex2, vertex1, CreateWeightMetadata(7));
            _sourceGraph.AddEdge(vertex4, vertex1, CreateWeightMetadata(4));
            _sourceGraph.AddEdge(vertex3, vertex2, CreateWeightMetadata(11));
            _sourceGraph.AddEdge(vertex4, vertex2, CreateWeightMetadata(9));
            _sourceGraph.AddEdge(vertex5, vertex2, CreateWeightMetadata(10));
            _sourceGraph.AddEdge(vertex5, vertex3, CreateWeightMetadata(5));
            _sourceGraph.AddEdge(vertex5, vertex4, CreateWeightMetadata(15));
            _sourceGraph.AddEdge(vertex6, vertex4, CreateWeightMetadata(6));
            _sourceGraph.AddEdge(vertex6, vertex5, CreateWeightMetadata(12));
            _sourceGraph.AddEdge(vertex7, vertex6, CreateWeightMetadata(13));
            _sourceGraph.AddEdge(vertex7, vertex5, CreateWeightMetadata(8));

            // result graph
            _resultGraph = factory.CreateNetwork();

            vertex1 = _resultGraph.AddVertex(new Coordinate(0, 0));
            vertex2 = _resultGraph.AddVertex(new Coordinate(1, 0));
            vertex3 = _resultGraph.AddVertex(new Coordinate(1, 1));
            vertex4 = _resultGraph.AddVertex(new Coordinate(2, 1));
            vertex5 = _resultGraph.AddVertex(new Coordinate(1, 2));
            vertex6 = _resultGraph.AddVertex(new Coordinate(0, 2));
            vertex7 = _resultGraph.AddVertex(new Coordinate(2, 0));

            _resultGraph.AddEdge(vertex1, vertex2, CreateWeightMetadata(7));
            _resultGraph.AddEdge(vertex1, vertex4, CreateWeightMetadata(4));
            _resultGraph.AddEdge(vertex2, vertex5, CreateWeightMetadata(10));
            _resultGraph.AddEdge(vertex3, vertex5, CreateWeightMetadata(5));
            _resultGraph.AddEdge(vertex4, vertex6, CreateWeightMetadata(6));
            _resultGraph.AddEdge(vertex5, vertex7, CreateWeightMetadata(8));

            _resultGraph.AddEdge(vertex2, vertex1, CreateWeightMetadata(7));
            _resultGraph.AddEdge(vertex4, vertex1, CreateWeightMetadata(4));
            _resultGraph.AddEdge(vertex5, vertex2, CreateWeightMetadata(10));
            _resultGraph.AddEdge(vertex5, vertex3, CreateWeightMetadata(5));
            _resultGraph.AddEdge(vertex6, vertex4, CreateWeightMetadata(6));
            _resultGraph.AddEdge(vertex7, vertex5, CreateWeightMetadata(8));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="Execute" /> method.
        /// </summary>
        [Test]
        public void BoruvkasAlgorithmExecuteTest()
        {
            Func<IGraphEdge, Double> weightFunction = edge => Convert.ToDouble(edge["Weight"]);

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters[GraphOperationParameters.WeightMetric] = weightFunction;

            BoruvkasAlgorithm operation = new BoruvkasAlgorithm(_sourceGraph, parameters);

            operation.Execute();

            Assert.AreEqual(_resultGraph.VertexCount, operation.Result.VertexCount);
            Assert.AreEqual(_resultGraph.EdgeCount, operation.Result.EdgeCount);

            foreach (IGraphEdge resultEdge in operation.Result.Edges)
            {
                IGraphEdge edge = _resultGraph.GetEdge(resultEdge.Source.Coordinate, resultEdge.Target.Coordinate);

                Assert.IsNotNull(edge);
                Assert.AreEqual(resultEdge.Source.Coordinate, edge.Source.Coordinate);
                Assert.AreEqual(resultEdge.Target.Coordinate, edge.Target.Coordinate);
                Assert.AreEqual(resultEdge.Metadata, edge.Metadata);
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

        #endregion
    }
}
