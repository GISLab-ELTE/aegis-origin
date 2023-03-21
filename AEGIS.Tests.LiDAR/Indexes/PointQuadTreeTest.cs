// <copyright file="PointQuadTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.LiDAR.Indexes
{
    [TestFixture]
    public class PointQuadTreeTest
    {
        private PointQuadTree<LasPointBase> tree;

        [SetUp]
        public void SetUp()
        {
            tree = new PointQuadTree<LasPointBase>(new Envelope(0, 100, 0, 100, 0, 0), 1);
        }

        [Test]
        public void QuadTreeConstructorTest()
        {
            Envelope e = new Envelope(0, 100, 0, 100, 0, 0);
            PointQuadTree<LasPointBase> tree = new PointQuadTree<LasPointBase>(e, 1);

            Assert.AreEqual(0, tree.NumberOfPoints);
            Assert.AreEqual(1, tree.NumberOfNodes);
            Assert.AreEqual(false, tree.IsReadOnly);
            Assert.AreEqual(e, tree.Envelope);
        }

        [Test]
        public void QuadTreeAddTest()
        {
            Assert.AreEqual(0, tree.NumberOfPoints);

            for (int i = 0; i < 20; i++)
            {
                tree.Add(new LasPointFormat0(i, i, 0), new Coordinate(i,i));
                Assert.AreEqual(i+1, tree.NumberOfPoints);
            }

            //rebuilds the tree, because the point is outside the original envelope
            tree.Add(new LasPointFormat0(200, 200, 0), new Coordinate(200, 200));

            Assert.AreEqual(21, tree.NumberOfPoints);
            Assert.IsTrue(tree.Envelope.Equals(new Envelope(0, 200, 0, 200, 0, 0)));
        }

        [Test]
        public void QuadTreeGetAllTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, 0)));
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            Assert.AreEqual(tree.NumberOfPoints, tree.GetAll().Count);
            Assert.AreEqual(tree.NumberOfPoints, tree.GetAllWithCoords().Count);
        }

        [Test]
        public void QuadTreeSearchTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, 0)));
            foreach(var point in points)
                tree.Add(point, point.Coordinate);

            Assert.AreEqual(0, tree.Search(new Envelope(0.5, 0.6, 0.5, 0.6, 0, 0)).Count);
            Assert.AreEqual(0, tree.SearchWithCoords(new Envelope(0.5, 0.6, 0.5, 0.6, 0, 0)).Count);
            Assert.AreEqual(0, tree.Search(new Envelope(100, 100, 100, 100, 0, 0)).Count);
            Assert.AreEqual(0, tree.SearchWithCoords(new Envelope(100, 100, 100, 100, 0, 0)).Count);
            Assert.AreEqual(0, tree.Search(new Envelope(200, 210, 200, 210, 0, 0)).Count);
            Assert.AreEqual(0, tree.SearchWithCoords(new Envelope(200, 210, 200, 210, 0, 0)).Count);

            Assert.AreEqual(tree.NumberOfPoints, tree.Search(Envelope.Infinity).Count);
            Assert.AreEqual(tree.NumberOfPoints, tree.SearchWithCoords(Envelope.Infinity).Count);
            Assert.AreEqual(tree.NumberOfPoints, tree.Search(Envelope.FromCoordinates(points.Select(x => x.Coordinate))).Count);
            Assert.AreEqual(tree.NumberOfPoints, tree.SearchWithCoords(Envelope.FromCoordinates(points.Select(x => x.Coordinate))).Count);

            Assert.AreEqual(5, tree.Search(new Envelope(0, 5, 0, 5, 0, 0)).Count);
            Assert.AreEqual(5, tree.SearchWithCoords(new Envelope(0, 5, 0, 5, 0, 0)).Count);
            Assert.AreEqual(26, tree.Search(new Envelope(5, 30, 5, 45, 0, 0)).Count);
            Assert.AreEqual(26, tree.SearchWithCoords(new Envelope(5, 30, 5, 45, 0, 0)).Count);

            Assert.Throws<ArgumentNullException>(() => tree.Search(null));
            Assert.Throws<ArgumentNullException>(() => tree.SearchWithCoords(null));
        }

        [Test]
        public void QuadTreeClearTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, 0)));
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            tree.Clear();

            Assert.AreEqual(0, tree.NumberOfPoints);
            Assert.AreEqual(1, tree.NumberOfNodes);
        }

        [Test]
        public void QuadTreeRemoveTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, 0)));
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            //single point remove
            Assert.IsFalse(tree.Remove(points[0], points[1].Coordinate));

            foreach (var point in points)
                Assert.IsTrue(tree.Remove(point, point.Coordinate));

            Assert.AreEqual(0, tree.NumberOfPoints);

            //single treeObject remove
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            var treeObjs = tree.GetAllWithCoords();

            foreach(var point in treeObjs)
                tree.Remove(point);

            Assert.AreEqual(0, tree.NumberOfPoints);

            //envelope remove
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            Assert.IsTrue(tree.Remove(new Envelope(0, 49, 0, 49, 0, 0)));
            Assert.AreEqual(50, tree.NumberOfPoints);
            Assert.IsTrue(tree.Remove(Envelope.Infinity));
            Assert.AreEqual(0, tree.NumberOfPoints);

            // envelope remove with results
            Assert.IsFalse(tree.Remove(Envelope.Infinity, out List<LasPointBase> removed));
            Assert.AreEqual(0, tree.NumberOfPoints);

            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            Assert.IsTrue(tree.Remove(Envelope.Infinity, out removed));
            Assert.AreEqual(0, tree.NumberOfPoints);
            Assert.AreEqual(points.Count, removed.Count);

            //check for null envelope
            Assert.Throws<ArgumentNullException>(() => tree.Remove((Envelope)null));
        }

        [Test]
        public void QuadTreeRebuildTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, 0)));
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            for (int i = 40; i < points.Count; i++)
            {
                tree.Remove(points[i], points[i].Coordinate);
            }

            tree.Rebuild();

            Assert.AreEqual(40, tree.NumberOfPoints);
            Assert.IsTrue(tree.Envelope.Equals(new Envelope(1, 40, 1, 40, 0, 0)));
        }
    }
}
