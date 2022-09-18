using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ELTE.AEGIS.LiDAR.Indexes;
using ELTE.AEGIS.IO.Lasfile;

namespace ELTE.AEGIS.Tests.LiDAR.Indexes
{
    [TestFixture]
    public class PointOctreeTest
    {
        private PointOctree<LasPointBase> tree;

        [SetUp]
        public void SetUp()
        {
            tree = new PointOctree<LasPointBase>(new Envelope(0, 100, 0, 100, 0, 100), 1);
        }

        [Test]
        public void OctreeConstructorTest()
        {
            Envelope e = new Envelope(0, 100, 0, 100, 0, 100);
            PointOctree<LasPointBase> tree = new PointOctree<LasPointBase>(e, 1);

            Assert.AreEqual(0, tree.NumberOfPoints);
            Assert.AreEqual(1, tree.NumberOfNodes);
            Assert.AreEqual(false, tree.IsReadOnly);
            Assert.AreEqual(e, tree.Envelope);
        }

        [Test]
        public void OctreeAddTest()
        {
            Assert.AreEqual(0, tree.NumberOfPoints);

            for (int i = 0; i < 20; i++)
            {
                tree.Add(new LasPointFormat0(i, i, i), new Coordinate(i, i, i));
                Assert.AreEqual(i + 1, tree.NumberOfPoints);
            }

            //rebuilds the tree, because the point is outside the original envelope
            tree.Add(new LasPointFormat0(200, 200, 200), new Coordinate(200, 200, 200));

            Assert.AreEqual(21, tree.NumberOfPoints);
            Assert.IsTrue(tree.Envelope.Equals(new Envelope(0, 200, 0, 200, 0, 200)));
        }

        [Test]
        public void OctreeGetAllTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, value)));
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            Assert.AreEqual(tree.NumberOfPoints, tree.GetAll().Count);
            Assert.AreEqual(tree.NumberOfPoints, tree.GetAllWithCoords().Count);
        }

        [Test]
        public void OctreeSearchTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, value)));
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            Assert.AreEqual(0, tree.Search(new Envelope(0.5, 0.6, 0.5, 0.6, 0.5, 0.6)).Count);
            Assert.AreEqual(0, tree.SearchWithCoords(new Envelope(0.5, 0.6, 0.5, 0.6, 0.5, 0.6)).Count);
            Assert.AreEqual(0, tree.Search(new Envelope(100, 100, 100, 100, 100, 100)).Count);
            Assert.AreEqual(0, tree.SearchWithCoords(new Envelope(100, 100, 100, 100, 100, 100)).Count);
            Assert.AreEqual(0, tree.Search(new Envelope(200, 210, 200, 210, 200, 210)).Count);
            Assert.AreEqual(0, tree.SearchWithCoords(new Envelope(200, 210, 200, 210, 200, 210)).Count);

            Assert.AreEqual(tree.NumberOfPoints, tree.Search(Envelope.Infinity).Count);
            Assert.AreEqual(tree.NumberOfPoints, tree.SearchWithCoords(Envelope.Infinity).Count);
            Assert.AreEqual(tree.NumberOfPoints, tree.Search(Envelope.FromCoordinates(points.Select(x => x.Coordinate))).Count);
            Assert.AreEqual(tree.NumberOfPoints, tree.SearchWithCoords(Envelope.FromCoordinates(points.Select(x => x.Coordinate))).Count);

            Assert.AreEqual(5, tree.Search(new Envelope(0, 5, 0, 5, 0, 5)).Count);
            Assert.AreEqual(5, tree.SearchWithCoords(new Envelope(0, 5, 0, 5, 0, 5)).Count);
            Assert.AreEqual(26, tree.Search(new Envelope(5, 30, 5, 45, 5, 45)).Count);
            Assert.AreEqual(26, tree.SearchWithCoords(new Envelope(5, 30, 5, 45, 5, 45)).Count);

            Assert.Throws<ArgumentNullException>(() => tree.Search(null));
            Assert.Throws<ArgumentNullException>(() => tree.SearchWithCoords(null));
        }

        [Test]
        public void OctreeClearTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, value)));
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            tree.Clear();

            Assert.AreEqual(0, tree.NumberOfPoints);
            Assert.AreEqual(1, tree.NumberOfNodes);
        }

        [Test]
        public void OctreeRemoveTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, value)));
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

            foreach (var point in treeObjs)
                tree.Remove(point);

            Assert.AreEqual(0, tree.NumberOfPoints);

            //envelope remove
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            Assert.IsTrue(tree.Remove(new Envelope(0, 49, 0, 49, 0, 49)));
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
        public void OctreeRebuildTest()
        {
            List<LasPointFormat0> points = new List<LasPointFormat0>(Enumerable.Range(1, 99).Select(value => new LasPointFormat0(value, value, value)));
            foreach (var point in points)
                tree.Add(point, point.Coordinate);

            for (int i = 40; i < points.Count; i++)
            {
                tree.Remove(points[i], points[i].Coordinate);
            }

            tree.Rebuild();

            Assert.AreEqual(40, tree.NumberOfPoints);
            Assert.IsTrue(tree.Envelope.Equals(new Envelope(1, 40, 1, 40, 1, 40)));
        }

        /*/// <summary>
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void OctreeConstructorTest()
        {
            Octree tree = new Octree(new Envelope(0, 100, 0, 100, 0, 100));

            tree.NumberOfGeometries.ShouldBe(0);
            tree.IsReadOnly.ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="Octree.Add" /> method.
        /// </summary>
        [Test]
        public void OctreeAddTest()
        {
            // Should add different types of a single geometry within the tree's bound
            this.tree.Add(this.factory.CreatePoint(5, 5, 5));
            this.tree.NumberOfGeometries.ShouldBe(1);

            Coordinate[] polygonCoordinates = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 10, 10),
                new Coordinate(10, 10, 10),
                new Coordinate(10, 0, 15),
                new Coordinate(0, 0, 0),
            };
            this.tree.Add(this.factory.CreatePolygon(polygonCoordinates));
            this.tree.NumberOfGeometries.ShouldBe(2);

            // Should add a single geometry from outside the tree's bound
            IPoint pointOutSideBound = this.factory.CreatePoint(110, 110, 50);
            this.tree.Add(pointOutSideBound);
            this.tree.NumberOfGeometries.ShouldBe(3);

            // Should add all geometries from geometry list
            List<IBasicGeometry> geometries = new List<IBasicGeometry>
            {
                this.factory.CreatePoint(65, 65),
                this.factory.CreatePolygon(
                    new Coordinate[]
                    {
                        new Coordinate(0, 0, 0),
                        new Coordinate(0, 15, 10),
                        new Coordinate(15, 15, 0),
                        new Coordinate(15, 0, 8),
                        new Coordinate(0, 0, 0),
                    }),
            };
            this.tree.Add(geometries);
            this.tree.NumberOfGeometries.ShouldBe(5);

            // errors
            Should.Throw<ArgumentNullException>(() => this.tree.Add((IGeometry)null));
            Should.Throw<ArgumentNullException>(() => this.tree.Add((IEnumerable<IGeometry>)null));
        }

        /// <summary>
        /// Tests the <see cref="Octree.Contains" /> method.
        /// </summary>
        [Test]
        public void OctreeContainsTest()
        {
            List<IPoint> points = new List<IPoint>(Enumerable.Range(1, 99).Select(value => this.factory.CreatePoint(value, value, value)));
            this.tree.Add(points);

            this.tree.Contains(null).ShouldBeFalse();
            this.tree.Contains(this.factory.CreatePoint(1000, 1000, 1000)).ShouldBeFalse();

            foreach (IPoint point in points)
                this.tree.Contains(point).ShouldBeTrue();
        }

        /// <summary>
        /// Tests the <see cref="Octree.Search" /> method.
        /// </summary>
        [Test]
        public void OctreeSearchTest()
        {
            List<IPoint> points = new List<IPoint>(Enumerable.Range(1, 99).Select(value => this.factory.CreatePoint(value, value, value)));
            this.tree.Add(points);

            // Should be empty for envelopes that do not contain any of the geometries
            this.tree.Search(new Envelope(0.5, 0.6, 0.5, 0.6, 0.5, 0.6));
            this.tree.Search(new Envelope(100, 100, 100, 100, 100, 100));
            this.tree.Search(new Envelope(200, 210, 200, 210, 200, 210));

            // Should find all geometries as results for these searches
            this.tree.Search(Envelope.Infinity).Count().ShouldBe(this.tree.NumberOfGeometries);
            this.tree.Search(Envelope.FromEnvelopes(points.Select(geometry => geometry.Envelope))).Count().ShouldBe(this.tree.NumberOfGeometries);

            // Should find correct results for some concrete examples
            Octree tree = new Octree(new Envelope(0, 100, 0, 100, 0, 100));

            Coordinate[] firstShell = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 10, 10),
                new Coordinate(10, 10, 10),
                new Coordinate(10, 0, 0),
                new Coordinate(0, 0, 0),
            };

            Coordinate[] secondShell = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 15, 15),
                new Coordinate(15, 15, 15),
                new Coordinate(15, 0, 0),
                new Coordinate(0, 0, 0),
            };

            Coordinate[] thirdShell = new Coordinate[]
            {
                new Coordinate(30, 30, 0),
                new Coordinate(30, 40, 40),
                new Coordinate(40, 40, 40),
                new Coordinate(40, 30, 50),
                new Coordinate(30, 30, 0),
            };

            tree.Add(this.factory.CreatePolygon(firstShell));
            tree.Add(this.factory.CreatePolygon(secondShell));
            tree.Add(this.factory.CreatePolygon(thirdShell));

            tree.Search(Envelope.FromCoordinates(firstShell)).Count().ShouldBe(1);
            tree.Search(Envelope.FromCoordinates(secondShell)).Count().ShouldBe(2);
            tree.Search(Envelope.FromCoordinates(thirdShell)).Count().ShouldBe(1);
            tree.Search(Envelope.FromCoordinates(secondShell.Union(thirdShell))).Count().ShouldBe(3);

            // Should throw exception for null
            Should.Throw<ArgumentNullException>(() => this.tree.Search(null));
        }

        /// <summary>
        /// Tests the <see cref="Octree.Remove" /> method.
        /// </summary>
        [Test]
        public void OctreeRemoveTest()
        {
            List<IPoint> points = new List<IPoint>(Enumerable.Range(1, 99).Select(value => this.factory.CreatePoint(value, value, value)));
            this.tree.Add(points);

            // Should not remove geometries that are not in the tree
            this.tree.Remove(this.factory.CreatePoint(1000, 1000, 1000)).ShouldBeFalse();
            this.tree.Remove(this.factory.CreatePoint(1.5, 1.5, 0)).ShouldBeFalse();

            // Should remove geometries that are in the tree
            foreach (IPoint point in points)
                this.tree.Remove(point).ShouldBeTrue();

            this.tree.NumberOfGeometries.ShouldBe(0);

            // Should remove correctly based on envelope
            this.tree.Add(points);

            this.tree.Remove(new Envelope(0, 49, 0, 49, 0, 49)).ShouldBeTrue();
            this.tree.NumberOfGeometries.ShouldBe(50);
            this.tree.Remove(Envelope.Infinity).ShouldBeTrue();
            this.tree.NumberOfGeometries.ShouldBe(0);

            // Should remove based on envelope with results
            this.tree.Remove(Envelope.Infinity, out List<IBasicGeometry> geometries).ShouldBeFalse();
            geometries.Count.ShouldBe(0);

            this.tree.Add(points);

            this.tree.Remove(Envelope.Infinity, out geometries).ShouldBeTrue();
            geometries.Count.ShouldBe(points.Count);

            // Should throw exception when removing with null
            Should.Throw<ArgumentNullException>(() => this.tree.Remove((Envelope)null));
            Should.Throw<ArgumentNullException>(() => this.tree.Remove((IGeometry)null));
        }

        /// <summary>
        /// Tests the <see cref="Octree.Clear" /> method.
        /// </summary>
        [Test]
        public void OctreeClearTest()
        {
            List<IPoint> points = new List<IPoint>(Enumerable.Range(1, 99).Select(value => this.factory.CreatePoint(value, value, value)));
            this.tree.Add(points);

            this.tree.Clear();

            this.tree.NumberOfGeometries.ShouldBe(0);
        }*/
    }
}
