/// <copyright file="RTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Indices.RTrees;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Indices.RTrees
{
    /// <summary>
    /// Test fixture for the <see cref="RTree" /> class.
    /// </summary>
    [TestFixture]
    public class RTreeTest
    {
        #region Private fields

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory _factory;

        /// <summary>
        /// The list of geometries.
        /// </summary>
        private List<IPoint> _geometries;

        /// <summary>
        /// The R-tree.
        /// </summary>
        private RTree _tree;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Random random = new Random();

            _factory = new GeometryFactory();
            _geometries = new List<IPoint>();

            for (Int32 i = 0; i < 1000; i++)
                _geometries.Add(_factory.CreatePoint(random.Next(1000), random.Next(1000), random.Next(1000)));

            _tree = new RTree();
            _tree.Add(_geometries);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void RTreeConstructorTest()
        {
            // default values

            RTree tree = new RTree();

            Assert.AreEqual(0, tree.Height);
            Assert.AreEqual(0, tree.NumberOfGeometries);
            Assert.AreEqual(8, tree.MinChildren);
            Assert.AreEqual(12, tree.MaxChildren);
            Assert.IsFalse(tree.IsReadOnly);


            // min. 2, max. 4 children

            tree = new RTree(2, 4);

            Assert.AreEqual(0, tree.Height);
            Assert.AreEqual(0, tree.NumberOfGeometries);
            Assert.AreEqual(2, tree.MinChildren);
            Assert.AreEqual(4, tree.MaxChildren);
            Assert.IsFalse(tree.IsReadOnly);


            // min. 10, max. 100 children

            tree = new RTree(10, 100);

            Assert.AreEqual(0, tree.Height);
            Assert.AreEqual(0, tree.NumberOfGeometries);
            Assert.AreEqual(10, tree.MinChildren);
            Assert.AreEqual(100, tree.MaxChildren);
            Assert.IsFalse(tree.IsReadOnly);


            // errors

            Assert.Throws<ArgumentOutOfRangeException>(() => new RStarTree(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RStarTree(1, 1));
        }

        /// <summary>
        /// Test case for the <see cref="Add" /> method.
        /// </summary>
        [Test]
        public void RTreeAddTest()
        {
            RTree tree = new RTree(2, 3);

            // add a single geometry

            tree.Add(_geometries[0]);
            Assert.AreEqual(1, tree.Height);
            Assert.AreEqual(1, tree.NumberOfGeometries);

            tree.Add(_geometries[1]);
            Assert.AreEqual(1, tree.Height);
            Assert.AreEqual(2, tree.NumberOfGeometries);

            tree.Add(_geometries[2]);
            Assert.AreEqual(1, tree.Height);
            Assert.AreEqual(3, tree.NumberOfGeometries);

            tree.Add(_geometries[3]);
            Assert.AreEqual(2, tree.Height);
            Assert.AreEqual(4, tree.NumberOfGeometries);

            for (Int32 geometryIndex = 4; geometryIndex < _geometries.Count; geometryIndex++)
                tree.Add(_geometries[geometryIndex]);

            Assert.AreEqual(_geometries.Count, tree.NumberOfGeometries);
            Assert.IsTrue(tree.Height >= Math.Floor(Math.Log(_geometries.Count, 3)));
            Assert.IsTrue(tree.Height <= _geometries.Count);


            // add a complete collection

            tree = new RTree(2, 12);
            tree.Add(_geometries);

            Assert.AreEqual(_geometries.Count, tree.NumberOfGeometries);


            // errors

            Assert.Throws<ArgumentNullException>(() => tree.Add((IGeometry)null));
            Assert.Throws<ArgumentNullException>(() => tree.Add((IEnumerable<IGeometry>)null));
        }

        /// <summary>
        /// Test case for the <see cref="Search" /> method.
        /// </summary>
        [Test]
        public void RTreeSearchTest()
        {
            // empty results

            Assert.IsEmpty(_tree.Search(Envelope.Undefined));

            for (Int32 i = 0; i < 1000; i++)
                Assert.IsEmpty(_tree.Search(new Envelope(i, i, i, i, i, i)));


            // all results

            Assert.AreEqual(_tree.NumberOfGeometries, _tree.Search(Envelope.Infinity).Count);
            Assert.AreEqual(_tree.NumberOfGeometries, _tree.Search(Envelope.FromEnvelopes(_geometries.Select(geometry => geometry.Envelope))).Count);



            // exact results

            RTree tree = new RTree();

            Coordinate[] firstShell = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 10, 0),
                new Coordinate(10, 10, 0),
                new Coordinate(10, 0, 0),
                new Coordinate(0, 0, 0)
            };

            Coordinate[] secondShell = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 15, 0),
                new Coordinate(15, 15, 0),
                new Coordinate(15, 0, 0),
                new Coordinate(0, 0, 0)
            };

            Coordinate[] thirdShell = new Coordinate[]
            {
                new Coordinate(30, 30, 0),
                new Coordinate(30, 40, 0),
                new Coordinate(40, 40, 0),
                new Coordinate(40, 30, 0),
                new Coordinate(30, 30, 0)
            };

            tree.Add(_factory.CreatePolygon(firstShell));
            tree.Add(_factory.CreatePolygon(secondShell));
            tree.Add(_factory.CreatePolygon(thirdShell));

            Assert.AreEqual(1, tree.Search(Envelope.FromCoordinates(firstShell)).Count);
            Assert.AreEqual(2, tree.Search(Envelope.FromCoordinates(secondShell)).Count);
            Assert.AreEqual(1, tree.Search(Envelope.FromCoordinates(thirdShell)).Count);
            Assert.AreEqual(3, tree.Search(Envelope.FromCoordinates(secondShell.Union(thirdShell))).Count);


            // errors

            Assert.Throws<ArgumentNullException>(() => _tree.Search(null));
        }

        /// <summary>
        /// Test case for the <see cref="Contains" /> method.
        /// </summary>
        [Test]
        public void RTreeContainsTest()
        {
            Assert.IsFalse(_tree.Contains(null));
            Assert.IsFalse(_tree.Contains(_factory.CreatePoint(1000, 1000, 1000)));
            Assert.IsFalse(_tree.Contains(_factory.CreatePoint(Coordinate.Undefined)));

            foreach (IPoint geometry in _geometries)
                Assert.IsTrue(_tree.Contains(geometry));
        }

        /// <summary>
        /// Test case for the <see cref="Remove" /> method.
        /// </summary>
        [Test]
        public void RTreeRemoveTest()
        {
            // remove geometry

            Assert.IsFalse(_tree.Remove(_factory.CreatePoint(1000, 1000, 1000)));
            Assert.IsFalse(_tree.Remove(_factory.CreatePoint(Coordinate.Undefined)));

            foreach (IPoint geometry in _geometries)
                Assert.IsTrue(_tree.Remove(geometry));


            // remove envelope

            _tree.Add(_geometries);

            Assert.IsTrue(_tree.Remove(Envelope.Infinity));


            // remove envelope with results

            IList<IGeometry> geometries = new List<IGeometry>();

            Assert.IsFalse(_tree.Remove(Envelope.Infinity, out geometries));
            Assert.AreEqual(0, geometries.Count);

            _tree.Add(_geometries);

            Assert.IsTrue(_tree.Remove(Envelope.Infinity, out geometries));
            Assert.AreEqual(_geometries.Count, geometries.Count);


            // errors

            Assert.Throws<ArgumentNullException>(() => _tree.Remove((Envelope)null));
            Assert.Throws<ArgumentNullException>(() => _tree.Remove((IGeometry)null));
        }

        /// <summary>
        /// Test case for the <see cref="Clear" /> method.
        /// </summary>
        [Test]
        public void RTreeClearTest()
        {
            _tree.Clear();

            Assert.AreEqual(0, _tree.Height);
            Assert.AreEqual(0, _tree.NumberOfGeometries);
        }

        #endregion
    }

}

