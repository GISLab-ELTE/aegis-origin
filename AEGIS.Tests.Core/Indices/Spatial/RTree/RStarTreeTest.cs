/// <copyright file="RStarTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamás Nagy</author>

using ELTE.AEGIS.Indices.Spatial.RTree;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Indices.Spatial.RTree
{
    [TestFixture]
    public class RStarTreeTest
    {
        private IGeometryFactory _factory;
        private List<IPoint> _geometries;

        [SetUp]
        public void SetUp()
        {
            Random r = new Random();

            _factory = Factory.DefaultInstance<IGeometryFactory>();
            _geometries = new List<IPoint>();

            for (Int32 i = 0; i < 1000; i++)
                _geometries.Add(_factory.CreatePoint(r.Next(1000), r.Next(1000), r.Next(1000)));

        }

        [TestCase]
        public void RStarTreeOperationsTest()
        {
            RStarTree tree = new RStarTree(2, 4);

            foreach (IGeometry geometry in _geometries)
                tree.Add(geometry);

            Assert.IsFalse(tree.Remove(_factory.CreatePoint(0, 0, 0)));

            foreach (IGeometry geometry in _geometries)
                Assert.IsTrue(tree.Contains(geometry));

            Assert.IsTrue(tree.Search(Envelope.FromEnvelopes(_geometries.Select(x => x.Envelope))).Count() == _geometries.Count);
            Assert.IsTrue(tree.Search(new Envelope(-1, -1, -1, -1, -1, -1)).Count() == 0);

            foreach (IGeometry geometry in _geometries)
            {
                Assert.IsTrue(tree.Contains(geometry));
                tree.Remove(geometry);
                Assert.IsFalse(tree.Contains(geometry));
            }

            Assert.AreEqual(0, tree.Height);
        }
    }
}

