// <copyright file="HalfedgeGraphTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Topology;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Topology
{
    /// <summary>
    /// Test fixture for the <see cref="HalfedgeGraph" /> class.
    /// </summary>
    /// <author>Máté Cserép</author>
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
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _factory = new GeometryFactory();
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
        /// Tests the addition of polygons to a graph.
        /// </summary>
        [Test]
        public void HalfedgeGraphAddTest()
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
            Assert.AreEqual(polygon.Shell.Coordinates.Count - 1, face.Vertices.Count());
            for (Int32 i = 0; i < polygon.Shell.Coordinates.Count - 1; ++i)
            {
                Int32 jNext = (i + 1) % (polygon.Shell.Coordinates.Count - 1);
                Assert.AreEqual(polygon.Shell.Coordinates[jNext], face.Vertices.ElementAt(i).Position);
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

            Assert.AreEqual(0, faces[0].Edges.Count(e => e.FaceA == null || e.FaceB == null));
            Assert.AreEqual(4, faces[0].Vertices.Max(v => v.Vertices.Count()));


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
            Assert.IsFalse(faces[2].IsAdjacent(faces[5]));
        }

        /// <summary>
        /// Tests the removal of vertices and faces from a graph.
        /// </summary>
        [Test]
        public void HalfedgeGraphRemoveTest()
        {
            // removing point vertices

            _graph.AddPoint(_factory.CreatePoint(0, 5));
            _graph.AddPoint(_factory.CreatePoint(-2, 11));
            _graph.AddPoint(_factory.CreatePoint(3, -6));
            _graph.AddPoint(_factory.CreatePoint(7, 3));
            _graph.AddPoint(_factory.CreatePoint(8, 9));

            _graph.VerifyTopology();
            Assert.AreEqual(5, _graph.Vertices.Count());

            _graph.RemoveVertex(new Coordinate(4, 1));
            Assert.AreEqual(5, _graph.Vertices.Count());

            _graph.RemoveVertex(new Coordinate(7, 3));
            Assert.AreEqual(4, _graph.Vertices.Count());

            _graph.RemoveVertex(new Coordinate(0, 5));
            Assert.AreEqual(3, _graph.Vertices.Count());
            _graph.VerifyTopology();


            // adding and removing polygon vertices

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
            Assert.AreEqual(2, _graph.Faces.Count());
            Assert.AreEqual(8, _graph.Edges.Count());
            Assert.AreEqual(16, _graph.Halfedges.Count());
            Assert.AreEqual(7, _graph.Vertices.Count());

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(0, 0), RemoveMode.Clean));
            _graph.VerifyTopology();
            Assert.AreEqual(1, _graph.Faces.Count());
            Assert.AreEqual(5, _graph.Edges.Count());
            Assert.AreEqual(10, _graph.Halfedges.Count());
            Assert.AreEqual(5, _graph.Vertices.Count());

            // re-add face
            _graph.AddPolygon(polygons[0]);
            _graph.VerifyTopology();
            Assert.AreEqual(2, _graph.Faces.Count());
            Assert.AreEqual(8, _graph.Edges.Count());
            Assert.AreEqual(16, _graph.Halfedges.Count());
            Assert.AreEqual(7, _graph.Vertices.Count());

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(15, -5), RemoveMode.Clean));
            _graph.VerifyTopology();
            Assert.AreEqual(1, _graph.Faces.Count());
            Assert.AreEqual(4, _graph.Edges.Count());
            Assert.AreEqual(8, _graph.Halfedges.Count());
            Assert.AreEqual(4, _graph.Vertices.Count());

            // re-add face
            _graph.AddPolygon(polygons[1]);
            _graph.VerifyTopology();
            Assert.AreEqual(2, _graph.Faces.Count());
            Assert.AreEqual(8, _graph.Edges.Count());
            Assert.AreEqual(16, _graph.Halfedges.Count());
            Assert.AreEqual(7, _graph.Vertices.Count());

            // remove non-isolated vertex without force
            try
            {
                _graph.RemoveVertex(new Coordinate(0, 0), RemoveMode.Normal);
                Assert.Fail();
            }
            catch (InvalidOperationException) { }

            // remove non-existent vertex
            Assert.IsFalse(_graph.RemoveVertex(new Coordinate(5, 5), RemoveMode.Clean));


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
            Assert.AreEqual(14, _graph.Faces.Count());
            Assert.AreEqual(28, _graph.Edges.Count());
            Assert.AreEqual(56, _graph.Halfedges.Count());
            Assert.AreEqual(15, _graph.Vertices.Count());

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(0, 0), RemoveMode.Clean));
            _graph.VerifyTopology();
            Assert.AreEqual(8, _graph.Faces.Count());
            Assert.AreEqual(22, _graph.Edges.Count());
            Assert.AreEqual(44, _graph.Halfedges.Count());
            Assert.AreEqual(14, _graph.Vertices.Count());

            // re-add faces
            for (Int32 i = 0; i < 6; ++i)
                _graph.AddPolygon(polygons[i]);
            _graph.VerifyTopology();
            Assert.AreEqual(14, _graph.Faces.Count());
            Assert.AreEqual(28, _graph.Edges.Count());
            Assert.AreEqual(56, _graph.Halfedges.Count());
            Assert.AreEqual(15, _graph.Vertices.Count());


            // adding and removing vertices of single and multiple polygons

            polygons = new IPolygon[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(20, 0),
                    _factory.CreatePoint(20, 20),
                    _factory.CreatePoint(15, 20),
                    _factory.CreatePoint(10, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(20, 0),
                    _factory.CreatePoint(25, 10),
                    _factory.CreatePoint(20, 20)),
            };

            _graph.Clear();
            for (Int32 i = 0; i < polygons.Length; ++i)
                _graph.AddPolygon(polygons[i]);

            _graph.VerifyTopology();
            Assert.AreEqual(3, _graph.Faces.Count());
            Assert.AreEqual(10, _graph.Edges.Count());
            Assert.AreEqual(20, _graph.Halfedges.Count());
            Assert.AreEqual(8, _graph.Vertices.Count());

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(0, 0), RemoveMode.Clean));
            _graph.VerifyTopology();
            Assert.AreEqual(2, _graph.Faces.Count());
            Assert.AreEqual(7, _graph.Edges.Count());
            Assert.AreEqual(14, _graph.Halfedges.Count());
            Assert.AreEqual(6, _graph.Vertices.Count());

            // re-add face
            _graph.AddPolygon(polygons[0]);
            _graph.VerifyTopology();
            Assert.AreEqual(3, _graph.Faces.Count());
            Assert.AreEqual(10, _graph.Edges.Count());
            Assert.AreEqual(20, _graph.Halfedges.Count());
            Assert.AreEqual(8, _graph.Vertices.Count());

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(15, 20), RemoveMode.Clean));
            _graph.VerifyTopology();
            Assert.AreEqual(2, _graph.Faces.Count());
            Assert.AreEqual(7, _graph.Edges.Count());
            Assert.AreEqual(14, _graph.Halfedges.Count());
            Assert.AreEqual(7, _graph.Vertices.Count());

            // re-add face
            _graph.AddPolygon(polygons[1]);
            _graph.VerifyTopology();
            Assert.AreEqual(3, _graph.Faces.Count());
            Assert.AreEqual(10, _graph.Edges.Count());
            Assert.AreEqual(20, _graph.Halfedges.Count());
            Assert.AreEqual(8, _graph.Vertices.Count());

            // remove vertex
            Assert.IsTrue(_graph.RemoveVertex(new Coordinate(20, 20), RemoveMode.Clean));
            _graph.VerifyTopology();
            Assert.AreEqual(1, _graph.Faces.Count());
            Assert.AreEqual(4, _graph.Edges.Count());
            Assert.AreEqual(8, _graph.Halfedges.Count());
            Assert.AreEqual(4, _graph.Vertices.Count());

            // re-add face
            _graph.AddPolygon(polygons[1]);
            _graph.AddPolygon(polygons[2]);
            _graph.VerifyTopology();
            Assert.AreEqual(3, _graph.Faces.Count());
            Assert.AreEqual(10, _graph.Edges.Count());
            Assert.AreEqual(20, _graph.Halfedges.Count());
            Assert.AreEqual(8, _graph.Vertices.Count());
        }

        /// <summary>
        /// Tests the merging of polygons (without holes) into a graph.
        /// </summary>
        [Test]
        public void HalfedgeGraphMergeTest()
        {
            // merging two adjacent polygons

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
            Assert.AreEqual(2, _graph.Faces.Count());
            Assert.AreEqual(8, _graph.Edges.Count());
            Assert.AreEqual(16, _graph.Halfedges.Count());
            Assert.AreEqual(7, _graph.Vertices.Count());


            // adding two adjacent polygons and merging an intersecting one

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
            Assert.AreEqual(4, _graph.Faces.Count());
            Assert.AreEqual(16, _graph.Edges.Count());
            Assert.AreEqual(32, _graph.Halfedges.Count());
            Assert.AreEqual(13, _graph.Vertices.Count());

            var vertices = _graph.Vertices.Select(vertex => vertex.Position).ToArray();
            Assert.Contains(new Coordinate(0, 3), vertices);
            Assert.Contains(new Coordinate(0, 6), vertices);


            // merging two adjacent polygons and one intersecting both

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
            Assert.AreEqual(8, _graph.Faces.Count());
            Assert.AreEqual(24, _graph.Edges.Count());
            Assert.AreEqual(48, _graph.Halfedges.Count());
            Assert.AreEqual(17, _graph.Vertices.Count());

            Assert.AreEqual(5, _graph.Vertices.Count(vertex => vertex.Position.Y == 3));
            Assert.AreEqual(5, _graph.Vertices.Count(vertex => vertex.Position.Y == 6));


            // merging four adjacent polygons

            _graph.Clear();
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(0, 0),
                _factory.CreatePoint(1, 0),
                _factory.CreatePoint(1, 2),
                _factory.CreatePoint(0, 2)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(1, 0),
                _factory.CreatePoint(2, 0),
                _factory.CreatePoint(2, 1),
                _factory.CreatePoint(1, 1)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(2, 0),
                _factory.CreatePoint(3, 0),
                _factory.CreatePoint(3, 2),
                _factory.CreatePoint(2, 2)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(1, 1),
                _factory.CreatePoint(2, 1),
                _factory.CreatePoint(1, 2)));

            _graph.VerifyTopology();
            Assert.AreEqual(4, _graph.Faces.Count());
            Assert.AreEqual(13, _graph.Edges.Count());
            Assert.AreEqual(26, _graph.Halfedges.Count());
            Assert.AreEqual(10, _graph.Vertices.Count());

            Assert.AreEqual(2, _graph.Faces.Count(face => face.Vertices.Count() == 5 && face.Edges.Count() == 5));
            Assert.AreEqual(1, _graph.Faces.Count(face => face.Vertices.Count() == 4 && face.Edges.Count() == 4));
            Assert.AreEqual(1, _graph.Faces.Count(face => face.Vertices.Count() == 3 && face.Edges.Count() == 3));


            // merging two adjacent and two intersecting polygons

            _graph.Clear();
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

            _graph.VerifyTopology();
            Assert.AreEqual(4, _graph.Faces.Count());
            Assert.AreEqual(14, _graph.Edges.Count());
            Assert.AreEqual(28, _graph.Halfedges.Count());
            Assert.AreEqual(11, _graph.Vertices.Count());

            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(2, 1),
                _factory.CreatePoint(6, 1),
                _factory.CreatePoint(6, 13),
                _factory.CreatePoint(2, 13)));

            _graph.VerifyTopology();
            Assert.AreEqual(8, _graph.Faces.Count());
            Assert.AreEqual(26, _graph.Edges.Count());
            Assert.AreEqual(52, _graph.Halfedges.Count());
            Assert.AreEqual(19, _graph.Vertices.Count());


            // merging two adjacent polygons with one intersecting / touching them

            _graph.Clear();
            _graph.AddPolygon(_factory.CreatePolygon(
                _factory.CreatePoint(0, 0),
                _factory.CreatePoint(10, 0),
                _factory.CreatePoint(10, 10),
                _factory.CreatePoint(0, 10)));
            _graph.AddPolygon(_factory.CreatePolygon(
                _factory.CreatePoint(0, 10),
                _factory.CreatePoint(10, 10),
                _factory.CreatePoint(10, 20),
                _factory.CreatePoint(0, 20)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(5, 5),
                _factory.CreatePoint(15, 5),
                _factory.CreatePoint(15, 10),
                _factory.CreatePoint(5, 10)));

            _graph.VerifyTopology();
            Assert.AreEqual(4, _graph.Faces.Count());
            Assert.AreEqual(14, _graph.Edges.Count());
            Assert.AreEqual(28, _graph.Halfedges.Count());
            Assert.AreEqual(11, _graph.Vertices.Count());

            Assert.AreEqual(1, _graph.Faces.Count(face => face.Vertices.Count() == 6 && face.Edges.Count() == 6));
            Assert.AreEqual(1, _graph.Faces.Count(face => face.Vertices.Count() == 5 && face.Edges.Count() == 5));
            Assert.AreEqual(2, _graph.Faces.Count(face => face.Vertices.Count() == 4 && face.Edges.Count() == 4));
        }

        /// <summary>
        /// Tests the merging of topology graphs.
        /// </summary>
        [Test]
        public void HalfedgeGraphMergeGraphTest()
        {
            // graph A: merging two adjacent and one intersecting polygons
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

            _graph.VerifyTopology();
            Assert.AreEqual(4, _graph.Faces.Count());
            Assert.AreEqual(14, _graph.Edges.Count());
            Assert.AreEqual(28, _graph.Halfedges.Count());
            Assert.AreEqual(11, _graph.Vertices.Count());

            // graph B: two intersecting polygons
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

            graphB.VerifyTopology();
            Assert.AreEqual(7, graphB.Faces.Count());
            Assert.AreEqual(19, graphB.Edges.Count());
            Assert.AreEqual(38, graphB.Halfedges.Count());
            Assert.AreEqual(13, graphB.Vertices.Count());

            // graph merge: numerous intersections
            _graph.MergeGraph(graphB);

            _graph.VerifyTopology();
            Assert.AreEqual(17, _graph.Faces.Count());
            Assert.AreEqual(47, _graph.Edges.Count());
            Assert.AreEqual(94, _graph.Halfedges.Count());
            Assert.AreEqual(31, _graph.Vertices.Count());
        }

        /// <summary>
        /// Tests the merging of polygons (with holes) into a graph.
        /// </summary>
        [Test]
        public void HalfedgeGraphMergeHoleTest()
        {
            // filling polygon hole with two other subject polygons

            IPolygon polygon = _factory.CreatePolygon(
                _factory.CreatePoint(0, 0),
                _factory.CreatePoint(10, 0),
                _factory.CreatePoint(10, 10),
                _factory.CreatePoint(0, 10));
            polygon.AddHole(_factory.CreateLinearRing(
                _factory.CreatePoint(2, 2),
                _factory.CreatePoint(2, 8),
                _factory.CreatePoint(8, 8),
                _factory.CreatePoint(8, 2)));
            Assert.IsTrue(polygon.IsValid);
            _graph.MergePolygon(polygon);

            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(2, 2),
                _factory.CreatePoint(8, 2),
                _factory.CreatePoint(8, 8)));
            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(2, 2),
                _factory.CreatePoint(8, 8),
                _factory.CreatePoint(2, 8)));

            _graph.VerifyTopology();
            Assert.AreEqual(3, _graph.Faces.Count());
            Assert.AreEqual(9, _graph.Edges.Count());
            Assert.AreEqual(18, _graph.Halfedges.Count());
            Assert.AreEqual(8, _graph.Vertices.Count());

            Assert.AreEqual(1, _graph.Faces.Count(face => face.Vertices.Count() == 4 && face.Edges.Count() == 4));
            Assert.AreEqual(2, _graph.Faces.Count(face => face.Vertices.Count() == 3 && face.Edges.Count() == 3));
            Assert.AreEqual(2, _graph.Faces.Sum(face => face.Holes.Count()));


            // a polygon hole containing a polygon with a hole is intersected with another subject polygon

            _graph.Clear();
            polygon = _factory.CreatePolygon(
                _factory.CreatePoint(0, 0),
                _factory.CreatePoint(20, 0),
                _factory.CreatePoint(20, 20),
                _factory.CreatePoint(0, 20));
            polygon.AddHole(_factory.CreateLinearRing(
                _factory.CreatePoint(5, 5),
                _factory.CreatePoint(5, 15),
                _factory.CreatePoint(15, 15),
                _factory.CreatePoint(15, 5)));
            Assert.IsTrue(polygon.IsValid);
            _graph.MergePolygon(polygon);

            polygon = _factory.CreatePolygon(
               _factory.CreatePoint(5, 5),
               _factory.CreatePoint(15, 5),
               _factory.CreatePoint(15, 15),
               _factory.CreatePoint(5, 15));
            polygon.AddHole(_factory.CreateLinearRing(
                _factory.CreatePoint(8, 8),
                _factory.CreatePoint(8, 12),
                _factory.CreatePoint(12, 12),
                _factory.CreatePoint(12, 8)));
            Assert.IsTrue(polygon.IsValid);
            _graph.MergePolygon(polygon);

            _graph.VerifyTopology();
            Assert.AreEqual(2,  _graph.Faces.Count());
            Assert.AreEqual(12, _graph.Edges.Count());
            Assert.AreEqual(24, _graph.Halfedges.Count());
            Assert.AreEqual(12, _graph.Vertices.Count());
            Assert.AreEqual(2,  _graph.Faces.Sum(face => face.Holes.Count()));

            _graph.MergePolygon(_factory.CreatePolygon(
                _factory.CreatePoint(9, -5),
                _factory.CreatePoint(11, -5),
                _factory.CreatePoint(11, 25),
                _factory.CreatePoint(9, 25)));

            _graph.VerifyTopology();
            Assert.AreEqual(11, _graph.Faces.Count());
            Assert.AreEqual(40, _graph.Edges.Count());
            Assert.AreEqual(80, _graph.Halfedges.Count());
            Assert.AreEqual(28, _graph.Vertices.Count());
            Assert.AreEqual(0,  _graph.Faces.Sum(face => face.Holes.Count()));
        }

        #endregion
    }
}
