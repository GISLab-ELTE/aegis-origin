/// <copyright file="GeometryGraphTest.cs" company="Eötvös Loránd University (ELTE)">
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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Geometry
{
    [TestFixture]
    public class GeometryGraphTest
    {
        [TestFixture]
        public class BreathFirstEnumeratorTest
        { 
            [TestCase]
            public void GeometryGraphBreathFirstEnumeratorMoveNextTest()
            {
                // test case 1: empty graph

                GeometryGraph graph = new GeometryGraph((IReferenceSystem)null, null);
                IEnumerator<IGraphVertex> enumerator = graph.GetEnumerator(EnumerationStrategy.BreathFirst);

                Assert.IsNull(enumerator.Current);
                Assert.IsFalse(enumerator.MoveNext());
                Assert.IsNull(enumerator.Current);

                // test case 2: weekly connected graph

                graph = new GeometryGraph((IReferenceSystem)null, null);
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


                // test case 3: strongly connected graph

                graph = new GeometryGraph((IReferenceSystem)null, null);
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


                // test case 4: disconnected graph

                graph = new GeometryGraph((IReferenceSystem)null, null);
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


                // test case 5: modified collection

                graph.AddVertex(new Coordinate(20, 20));

                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }

            [TestCase]
            public void GeometryGraphBreathFirstEnumeratorResetTest()
            {
                // test case 1: simple reset

                GeometryGraph graph = new GeometryGraph((IReferenceSystem)null, null);

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


                // test case 2: modified collection

                graph.AddVertex(new Coordinate(20, 20));

                Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
            }
        }

        [TestFixture]
        public class DepthFirstEnumeratorTest
        {
            [TestCase]
            public void GeometryGraphDepthFirstEnumeratorMoveNextTest()
            {
                // test case 1: empty graph

                GeometryGraph graph = new GeometryGraph((IReferenceSystem)null, null);
                IEnumerator<IGraphVertex> enumerator = graph.GetEnumerator(EnumerationStrategy.DepthFirst);

                Assert.IsNull(enumerator.Current);
                Assert.IsFalse(enumerator.MoveNext());
                Assert.IsNull(enumerator.Current);

                // test case 2: weekly connected graph

                graph = new GeometryGraph((IReferenceSystem)null, null);
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


                // test case 3: strongly connected graph

                graph = new GeometryGraph((IReferenceSystem)null, null);
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


                // test case 4: disconnected graph

                graph = new GeometryGraph((IReferenceSystem)null, null);
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


                // test case 5: modified collection

                graph.AddVertex(new Coordinate(20, 20));

                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }

            [TestCase]
            public void GeometryGraphDepthFirstEnumeratorResetTest()
            {
                // test case 1: simple reset

                GeometryGraph graph = new GeometryGraph((IReferenceSystem)null, null);

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


                // test case 2: modified collection

                graph.AddVertex(new Coordinate(20, 20));

                Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
            }
        }       
    }
}
