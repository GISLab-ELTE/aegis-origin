/// <copyright file="RStarTreeTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Tests.Indices.RTrees
{
    /// <summary>
    /// Text fixture for the <see cref="RStarTree" /> class.
    /// </summary>
    [TestFixture]
    public class RStarTreeTest
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
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void RStarTreeConstructorTest()
        {
            // default values

            RStarTree tree = new RStarTree();

            Assert.AreEqual(0, tree.Height);
            Assert.AreEqual(0, tree.NumberOfGeometries);
            Assert.AreEqual(8, tree.MinChildren);
            Assert.AreEqual(12, tree.MaxChildren);


            // min. 2, max. 4 children

            tree = new RStarTree(2, 4);

            Assert.AreEqual(0, tree.Height);
            Assert.AreEqual(0, tree.NumberOfGeometries);
            Assert.AreEqual(2, tree.MinChildren);
            Assert.AreEqual(4, tree.MaxChildren);


            // min. 10, max. 100 children

            tree = new RStarTree(10, 100);

            Assert.AreEqual(0, tree.Height);
            Assert.AreEqual(0, tree.NumberOfGeometries);
            Assert.AreEqual(10, tree.MinChildren);
            Assert.AreEqual(100, tree.MaxChildren);


            // errors

            Assert.Throws<ArgumentOutOfRangeException>(() => new RStarTree(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RStarTree(1, 1));
        }

        /// <summary>
        /// Test case for the <see cref="Add" /> method.
        /// </summary>
        [Test]
        public void RStarTreeAddTest()
        {
            RStarTree tree = new RStarTree(2, 3);

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
            Assert.IsTrue(tree.Height <= Math.Ceiling(Math.Log(_geometries.Count, 2)));


            // add a complete collection

            tree = new RStarTree(2, 3);
            tree.Add(_geometries);

            Assert.AreEqual(_geometries.Count, tree.NumberOfGeometries);


            // errors

            Assert.Throws<ArgumentNullException>(() => tree.Add((IGeometry)null));
            Assert.Throws<ArgumentNullException>(() => tree.Add((IEnumerable<IGeometry>)null));
        }

        #endregion
    }
}

