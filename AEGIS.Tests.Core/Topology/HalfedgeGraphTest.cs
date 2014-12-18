/// <copyright file="HalfedgeGraphTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
/// <author>Máté Cserép</author>

using ELTE.AEGIS.Topology;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Topology
{
    /// <summary>
    /// Test fixture for the <see cref="HalfedgeGraph" /> class.
    /// </summary>
    [TestFixture]
    public class HalfedgeGraphTest
    {
        #region Private fields

        /// <summary>
        /// The halfedge graph.
        /// </summary>
        private HalfedgeGraph _graph;

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory _factory;

        #endregion

        #region Test setup

        /// <summary>
        /// Test fixture setup.
        /// </summary>
        [TestFixtureSetUp]
        public void FixtureInitialize()
        {
            _factory = Factory.DefaultInstance<IGeometryFactory>();
        }

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _graph = new HalfedgeGraph();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="AddPolygon" /> method.
        /// </summary>
        [Test]
        public void HalfedgeGraphAddPolygonTest()
        {
            // single polygon

            IPolygon polygon =
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10));

            IFace face = _graph.AddPolygon(polygon);

            _graph.VerifyTopology();
            Assert.AreEqual(face.Vertices.Count(), polygon.Shell.Coordinates.Count - 1);
            for (Int32 i = 0; i < polygon.Shell.Coordinates.Count - 1; ++i)
            {
                Int32 jNext = (i + 1) % (polygon.Shell.Coordinates.Count - 1);
                Assert.AreEqual(face.Vertices.ElementAt(i).Position, polygon.Shell.Coordinates[jNext]);
            }


            // adjacent polygons

            IPolygon[] polygons = new IPolygon[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(15, -5),
                    _factory.CreatePoint(25, 10),
                    _factory.CreatePoint(20, 20),
                    _factory.CreatePoint(10, 10))
            };

            _graph.Clear();
            IFace[] faces = new IFace[polygons.Length];
            for (Int32 i = 0; i < polygons.Length; ++i)
                faces[i] = _graph.AddPolygon(polygons[i]);

            _graph.VerifyTopology();
            Assert.IsTrue(faces[0].IsAdjacent(faces[1]));
            Assert.AreSame(faces[0].Edges.Single(e => e.FaceA != null && e.FaceB != null), faces[1].Edges.Single(e => e.FaceA != null && e.FaceB != null));
        

            // multiple adjacent polygons

            polygons = new IPolygon[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(15, -5),
                    _factory.CreatePoint(25, 10),
                    _factory.CreatePoint(20, 20),
                    _factory.CreatePoint(10, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 10),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(20, 20),
                    _factory.CreatePoint(-5, 15)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(-5, 15),
                    _factory.CreatePoint(-10, 5),
                    _factory.CreatePoint(-5, -5),
                    _factory.CreatePoint(5, -5),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(0, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(5, -5),
                    _factory.CreatePoint(15, -5),
                    _factory.CreatePoint(10, 0))
            };

            _graph.Clear();
            faces = new IFace[polygons.Length];
            for (Int32 i = 0; i < polygons.Length; ++i)
                faces[i] = _graph.AddPolygon(polygons[i]);

            _graph.VerifyTopology();
            Assert.IsTrue(faces[0].IsAdjacent(faces[1]));
            Assert.IsTrue(faces[0].IsAdjacent(faces[2]));
            Assert.IsTrue(faces[0].IsAdjacent(faces[3]));
            Assert.IsTrue(faces[1].IsAdjacent(faces[2]));
            Assert.IsTrue(faces[2].IsAdjacent(faces[3]));
            Assert.IsTrue(faces[3].IsAdjacent(faces[4]));

            Assert.IsFalse(faces[0].IsAdjacent(faces[4]));
            Assert.IsFalse(faces[1].IsAdjacent(faces[3]));
            Assert.IsFalse(faces[2].IsAdjacent(faces[4]));

            Assert.AreEqual(faces[0].Edges.Count(e => e.FaceA == null || e.FaceB == null), 0);
            Assert.AreEqual(faces[0].Vertices.Max(v => v.Vertices.Count()), 4);


            // adjacent polygons with one overlapping polygon

            polygons = new IPolygon[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(15, -5),
                    _factory.CreatePoint(25, 10),
                    _factory.CreatePoint(20, 20),
                    _factory.CreatePoint(10, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 10),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(20, 20),
                    _factory.CreatePoint(-5, 15)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(-5, 15),
                    _factory.CreatePoint(-10, 5),
                    _factory.CreatePoint(-5, -5),
                    _factory.CreatePoint(5, -5),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(0, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(5, -5),
                    _factory.CreatePoint(15, -5),
                    _factory.CreatePoint(10, 0)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(-5, -5),
                    _factory.CreatePoint(5, -10),
                    _factory.CreatePoint(15, -5),
                    _factory.CreatePoint(5, -5))
            };

            _graph.Clear();
            faces = new IFace[polygons.Length];
            for (Int32 i = 0; i < polygons.Length; ++i)
                faces[i] = _graph.AddPolygon(polygons[i]);

            _graph.VerifyTopology();
            Assert.IsTrue(faces[3].IsAdjacent(faces[5]));
            Assert.IsTrue(faces[4].IsAdjacent(faces[5]));

            Assert.IsFalse(faces[0].IsAdjacent(faces[5]));
            Assert.IsFalse(faces[1].IsAdjacent(faces[5]));
        }

        /// <summary>
        /// Tests the <see cref="RemoveVertex" /> method.
        /// </summary>
        [Test]
        public void HalfedgeGraphRemoveVertexTest()
        {
            // removing point vertices

            _graph.AddPoint(_factory.CreatePoint(0, 5));
            _graph.AddPoint(_factory.CreatePoint(-2, 11));
            _graph.AddPoint(_factory.CreatePoint(3, -6));
            _graph.AddPoint(_factory.CreatePoint(7, 3));
            _graph.AddPoint(_factory.CreatePoint(8, 9));

            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Vertices.Count(), 5);

            _graph.RemoveVertex(new Coordinate(4, 1));
            Assert.AreEqual(_graph.Vertices.Count(), 5);

            _graph.RemoveVertex(new Coordinate(7, 3));
            Assert.AreEqual(_graph.Vertices.Count(), 4);

            _graph.RemoveVertex(new Coordinate(0, 5));
            Assert.AreEqual(_graph.Vertices.Count(), 3);

            _graph.VerifyTopology();
        

            // adding and removing removing polygon vertices

            IPolygon[] polygons = new IPolygon[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(15, -5),
                    _factory.CreatePoint(25, 10),
                    _factory.CreatePoint(20, 20),
                    _factory.CreatePoint(10, 10))
            };

            _graph.Clear();
            for (Int32 i = 0; i < polygons.Length; ++i)
                _graph.AddPolygon(polygons[i]);

            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 2);
            Assert.AreEqual(_graph.Edges.Count(), 8);
            Assert.AreEqual(_graph.Halfedges.Count(), 16);
            Assert.AreEqual(_graph.Vertices.Count(), 7);

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(0, 0), forced: true));
            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 1);
            Assert.AreEqual(_graph.Edges.Count(), 5);
            Assert.AreEqual(_graph.Halfedges.Count(), 10);
            Assert.AreEqual(_graph.Vertices.Count(), 6);

            // re-add face
            _graph.AddPolygon(polygons[0]);
            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 2);
            Assert.AreEqual(_graph.Edges.Count(), 8);
            Assert.AreEqual(_graph.Halfedges.Count(), 16);
            Assert.AreEqual(_graph.Vertices.Count(), 7);

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(15, -5), forced: true));
            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 1);
            Assert.AreEqual(_graph.Edges.Count(), 4);
            Assert.AreEqual(_graph.Halfedges.Count(), 8);
            Assert.AreEqual(_graph.Vertices.Count(), 6);

            // re-add face
            _graph.AddPolygon(polygons[1]);
            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 2);
            Assert.AreEqual(_graph.Edges.Count(), 8);
            Assert.AreEqual(_graph.Halfedges.Count(), 16);
            Assert.AreEqual(_graph.Vertices.Count(), 7);

            // remove non-isolated vertex without force
            try
            {
                _graph.RemoveVertex(new Coordinate(0, 0), forced: false);
                Assert.Fail();
            }
            catch (InvalidOperationException) { }

            // remove non-existent vertex
            Assert.IsFalse(_graph.RemoveVertex(new Coordinate(5, 5), forced: true));
        

            // adding and removing vertices of multiple polygons

            polygons = new IPolygon[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(6, 0),
                    _factory.CreatePoint(3, 6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(3, 6),
                    _factory.CreatePoint(-3, 6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(-3, 6),
                    _factory.CreatePoint(-6, 0)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(-6, 0),
                    _factory.CreatePoint(-3, -6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(-3, -6),
                    _factory.CreatePoint(3, -6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(3, -6),
                    _factory.CreatePoint(6, 0)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(6, 6),
                    _factory.CreatePoint(3, 6),
                    _factory.CreatePoint(6, 0)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(-6, 6),
                    _factory.CreatePoint(-6, 0),
                    _factory.CreatePoint(-3, 6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(-6, -6),
                    _factory.CreatePoint(-3, -6),
                    _factory.CreatePoint(-6, 0)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(6, -6),
                    _factory.CreatePoint(6, 0),
                    _factory.CreatePoint(3, -6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 12),
                    _factory.CreatePoint(-6, 6),
                    _factory.CreatePoint(-3, 6),
                    _factory.CreatePoint(3, 6),
                    _factory.CreatePoint(6, 6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, -12),
                    _factory.CreatePoint(6, -6),
                    _factory.CreatePoint(3, -6),
                    _factory.CreatePoint(-3, -6),
                    _factory.CreatePoint(-6, -6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(12, 0),
                    _factory.CreatePoint(6, 6),
                    _factory.CreatePoint(6, 0),
                    _factory.CreatePoint(6, -6)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(-12, 0),
                    _factory.CreatePoint(-6, -6),
                    _factory.CreatePoint(-6, 0),
                    _factory.CreatePoint(-6, 6)),
            };

            _graph.Clear();
            for (Int32 i = 0; i < polygons.Length; ++i)
                _graph.AddPolygon(polygons[i]);

            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 14);
            Assert.AreEqual(_graph.Edges.Count(), 28);
            Assert.AreEqual(_graph.Halfedges.Count(), 56);
            Assert.AreEqual(_graph.Vertices.Count(), 15);

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(0, 0), forced: true));
            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 8);
            Assert.AreEqual(_graph.Edges.Count(), 22);
            Assert.AreEqual(_graph.Halfedges.Count(), 44);
            Assert.AreEqual(_graph.Vertices.Count(), 14);

            // re-add faces
            for (Int32 i = 0; i < 6; ++i)
                _graph.AddPolygon(polygons[i]);
            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 14);
            Assert.AreEqual(_graph.Edges.Count(), 28);
            Assert.AreEqual(_graph.Halfedges.Count(), 56);
            Assert.AreEqual(_graph.Vertices.Count(), 15);
        }

        /// <summary>
        /// Tests the <see cref="MergePolygon" /> method.
        /// </summary>
        [Test]
        public void HalfedgeGraphMergePolygonTest()
        {
            // merging two polygons

            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(0, 0),
                _factory.CreatePoint(10, 0),
                _factory.CreatePoint(10, 10),
                _factory.CreatePoint(0, 10)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(10, 0),
                _factory.CreatePoint(15, -5),
                _factory.CreatePoint(25, 10),
                _factory.CreatePoint(20, 20),
                _factory.CreatePoint(10, 10)));

            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 2);
            Assert.AreEqual(_graph.Edges.Count(), 8);
            Assert.AreEqual(_graph.Halfedges.Count(), 16);
            Assert.AreEqual(_graph.Vertices.Count(), 7);
        

            // merging three polygons

            _graph.Clear();
            _graph.AddPolygon(_factory.CreatePolygon(
                _factory.CreatePoint(0, 0),
                _factory.CreatePoint(10, 0),
                _factory.CreatePoint(10, 10),
                _factory.CreatePoint(0, 10)));
            _graph.AddPolygon(_factory.CreatePolygon(
                _factory.CreatePoint(10, 0),
                _factory.CreatePoint(15, -5),
                _factory.CreatePoint(25, 10),
                _factory.CreatePoint(20, 20),
                _factory.CreatePoint(10, 10)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(-3, 3),
                _factory.CreatePoint(3, 3),
                _factory.CreatePoint(3, 6),
                _factory.CreatePoint(-3, 6)));

            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 4);
            Assert.AreEqual(_graph.Edges.Count(), 16);
            Assert.AreEqual(_graph.Halfedges.Count(), 32);
            Assert.AreEqual(_graph.Vertices.Count(), 13);

            var vertices = _graph.Vertices.Select(vertex => vertex.Position).ToArray();
            Assert.Contains(new Coordinate(0, 3), vertices);
            Assert.Contains(new Coordinate(0, 6), vertices);
        

            // merging three polygons

            _graph.Clear();
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(0, 0),
                _factory.CreatePoint(10, 0),
                _factory.CreatePoint(10, 10),
                _factory.CreatePoint(0, 10)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(10, 0),
                _factory.CreatePoint(15, -5),
                _factory.CreatePoint(25, 10),
                _factory.CreatePoint(20, 20),
                _factory.CreatePoint(10, 10)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(-3, 3),
                _factory.CreatePoint(30, 3),
                _factory.CreatePoint(30, 6),
                _factory.CreatePoint(-3, 6)));

            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 8);
            Assert.AreEqual(_graph.Edges.Count(), 24);
            Assert.AreEqual(_graph.Halfedges.Count(), 48);
            Assert.AreEqual(_graph.Vertices.Count(), 17);

            Assert.AreEqual(_graph.Vertices.Count(vertex => vertex.Position.Y == 3), 5);
            Assert.AreEqual(_graph.Vertices.Count(vertex => vertex.Position.Y == 6), 5);
        }

        /// <summary>
        /// Tests the <see cref="MergeGraph" /> method.
        /// </summary>
        [Test]
        public void HalfedgeGraphMergeGraphTest()
        {
            // Creation of graph A
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(0, 0),
                _factory.CreatePoint(8, 0),
                _factory.CreatePoint(8, 8),
                _factory.CreatePoint(0, 8)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(8, 0),
                _factory.CreatePoint(14, 0),
                _factory.CreatePoint(8, 8)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(-4, -6),
                _factory.CreatePoint(4, -6),
                _factory.CreatePoint(4, 2),
                _factory.CreatePoint(-4, 2)));

            // Verification of graph A
            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 4);
            Assert.AreEqual(_graph.Edges.Count(), 14);
            Assert.AreEqual(_graph.Halfedges.Count(), 28);
            Assert.AreEqual(_graph.Vertices.Count(), 11);

            // Creation of graph B
            HalfedgeGraph graphB = new HalfedgeGraph();
            graphB.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(-2, 6),
                _factory.CreatePoint(14, 6),
                _factory.CreatePoint(4, 14)));
            graphB.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(2, 1),
                _factory.CreatePoint(6, 1),
                _factory.CreatePoint(6, 13),
                _factory.CreatePoint(2, 13)));

            // Verification of graph B
            graphB.VerifyTopology();
            Assert.AreEqual(graphB.Faces.Count(), 7);
            Assert.AreEqual(graphB.Edges.Count(), 19);
            Assert.AreEqual(graphB.Halfedges.Count(), 38);
            Assert.AreEqual(graphB.Vertices.Count(), 13);
      
            // Graph merge
            _graph.MergeGraph(graphB);

            // Verification of the result
            _graph.VerifyTopology();
            Assert.AreEqual(_graph.Faces.Count(), 17);
            Assert.AreEqual(_graph.Edges.Count(), 45);
            Assert.AreEqual(_graph.Halfedges.Count(), 90);
            Assert.AreEqual(_graph.Vertices.Count(), 31);
        }

        #endregion
    }
}
