/// <copyright file="GeometryGraphTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Geometry
{
    /// <summary>
    /// Test fixture for the <see cref="GeometryGraph" /> class.
    /// </summary>
    [TestFixture]
    public class GeometryGraphTest
    {
        /// <summary>
        /// Test fixture for the <see cref="GeometryGraph.BreathFirstEnumerator" /> class.
        /// </summary>
        [TestFixture]
        public class BreathFirstEnumeratorTest
        {
            #region Test methods

            /// <summary>
            /// Tests the <see cref="MoveNext" /> method of the breath-first enumerator.
            /// </summary>
            [Test]
            public void GeometryGraphBreathFirstEnumeratorMoveNextTest()
            {
                // empty graph

                GeometryGraph graph = new GeometryGraph(PrecisionModel.Default, null, null);
                IEnumerator<IGraphVertex> enumerator = graph.GetEnumerator(EnumerationStrategy.BreathFirst);

                Assert.IsNull(enumerator.Current);
                Assert.IsFalse(enumerator.MoveNext());
                Assert.IsNull(enumerator.Current);

                // weekly connected graph

                graph = new GeometryGraph(PrecisionModel.Default, null, null);
                IGraphVertex v1 = graph.AddVertex(new Coordinate(10, 10));
                IGraphVertex v2 = graph.AddVertex(new Coordinate(0, 0));
                graph.AddEdge(v1, v2);
                enumerator = graph.GetEnumerator(EnumerationStrategy.BreathFirst);

                List<IGraphVertex> list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));


                // strongly connected graph

                graph = new GeometryGraph(PrecisionModel.Default, null, null);
                v1 = graph.AddVertex(new Coordinate(10, 10));
                v2 = graph.AddVertex(new Coordinate(0, 0));
                graph.AddEdge(v1, v2);
                graph.AddEdge(v2, v1);
                enumerator = graph.GetEnumerator(EnumerationStrategy.BreathFirst);

                list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));


                // disconnected graph

                graph = new GeometryGraph(PrecisionModel.Default, null, null);
                v1 = graph.AddVertex(new Coordinate(10, 10));
                v2 = graph.AddVertex(new Coordinate(0, 0));
                IGraphVertex v3 = graph.AddVertex(new Coordinate(5, 5));
                IGraphVertex v4 = graph.AddVertex(new Coordinate(15, 15));
                graph.AddEdge(v1, v2);
                graph.AddEdge(v2, v1);
                graph.AddEdge(v3, v4);
                enumerator = graph.GetEnumerator(EnumerationStrategy.BreathFirst);

                list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));


                // modified collection

                graph.AddVertex(new Coordinate(20, 20));

                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }

            /// <summary>
            /// Tests the <see cref="Reset" /> method of the depth-first enumerator.
            /// </summary>
            [Test]
            public void GeometryGraphBreathFirstEnumeratorResetTest()
            {
                // simple reset

                GeometryGraph graph = new GeometryGraph(PrecisionModel.Default, null, null);

                IGraphVertex v1 = graph.AddVertex(new Coordinate(10, 10));
                IGraphVertex v2 = graph.AddVertex(new Coordinate(0, 0));
                IGraphVertex v3 = graph.AddVertex(new Coordinate(5, 5));
                IGraphVertex v4 = graph.AddVertex(new Coordinate(15, 15));
                graph.AddEdge(v1, v2);
                graph.AddEdge(v2, v1);
                graph.AddEdge(v3, v4);
                IEnumerator<IGraphVertex> enumerator = graph.GetEnumerator(EnumerationStrategy.BreathFirst);

                List<IGraphVertex> list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));

                enumerator.Reset();

                list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));


                // modified collection

                graph.AddVertex(new Coordinate(20, 20));

                Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
            }

            #endregion
        }

        /// <summary>
        /// Test fixture for the <see cref="GeometryGraph.DepthFirstEnumerator" /> class.
        /// </summary>
        [TestFixture]
        public class DepthFirstEnumeratorTest
        {
            #region Test methods

            /// <summary>
            /// Tests the <see cref="MoveNext" /> method of the depth-first enumerator.
            /// </summary>
            [Test]
            public void GeometryGraphDepthFirstEnumeratorMoveNextTest()
            {
                // empty graph

                GeometryGraph graph = new GeometryGraph(PrecisionModel.Default, null, null);
                IEnumerator<IGraphVertex> enumerator = graph.GetEnumerator(EnumerationStrategy.DepthFirst);

                Assert.IsNull(enumerator.Current);
                Assert.IsFalse(enumerator.MoveNext());
                Assert.IsNull(enumerator.Current);

                // weekly connected graph

                graph = new GeometryGraph(PrecisionModel.Default, null, null);
                IGraphVertex v1 = graph.AddVertex(new Coordinate(10, 10));
                IGraphVertex v2 = graph.AddVertex(new Coordinate(0, 0));
                graph.AddEdge(v1, v2);
                enumerator = graph.GetEnumerator(EnumerationStrategy.DepthFirst);

                List<IGraphVertex> list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));


                // strongly connected graph

                graph = new GeometryGraph(PrecisionModel.Default, null, null);
                v1 = graph.AddVertex(new Coordinate(10, 10));
                v2 = graph.AddVertex(new Coordinate(0, 0));
                graph.AddEdge(v1, v2);
                graph.AddEdge(v2, v1);
                enumerator = graph.GetEnumerator(EnumerationStrategy.DepthFirst);

                list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));


                // disconnected graph

                graph = new GeometryGraph(PrecisionModel.Default, null, null);
                v1 = graph.AddVertex(new Coordinate(10, 10));
                v2 = graph.AddVertex(new Coordinate(0, 0));
                IGraphVertex v3 = graph.AddVertex(new Coordinate(5, 5));
                IGraphVertex v4 = graph.AddVertex(new Coordinate(15, 15));
                graph.AddEdge(v1, v2);
                graph.AddEdge(v2, v1);
                graph.AddEdge(v3, v4);
                enumerator = graph.GetEnumerator(EnumerationStrategy.DepthFirst);

                list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));


                // modified collection

                graph.AddVertex(new Coordinate(20, 20));

                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }

            /// <summary>
            /// Tests the <see cref="Reset" /> method of the depth-first enumerator.
            /// </summary>
            [Test]
            public void GeometryGraphDepthFirstEnumeratorResetTest()
            {
                // simple reset

                GeometryGraph graph = new GeometryGraph(PrecisionModel.Default, null, null);

                IGraphVertex v1 = graph.AddVertex(new Coordinate(10, 10));
                IGraphVertex v2 = graph.AddVertex(new Coordinate(0, 0));
                IGraphVertex v3 = graph.AddVertex(new Coordinate(5, 5));
                IGraphVertex v4 = graph.AddVertex(new Coordinate(15, 15));
                graph.AddEdge(v1, v2);
                graph.AddEdge(v2, v1);
                graph.AddEdge(v3, v4);
                IEnumerator<IGraphVertex> enumerator = graph.GetEnumerator(EnumerationStrategy.DepthFirst);

                List<IGraphVertex> list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));

                enumerator.Reset();

                list = new List<IGraphVertex>();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }

                Assert.AreEqual(list.Count, graph.VertexCount);
                Assert.IsTrue(list.All(vertex => graph.Vertices.Contains(vertex)));


                // modified collection

                graph.AddVertex(new Coordinate(20, 20));

                Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
            }

            #endregion
        }       
    }
}
