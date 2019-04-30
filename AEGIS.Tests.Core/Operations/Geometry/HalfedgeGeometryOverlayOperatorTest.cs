/// <copyright file="HalfedgeGeometryOverlayOperatorTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Máté Cserép</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations.Geometry;
using NUnit.Framework;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Geometry
{
    /// <summary>
    /// Test fixture for the <see cref="HalfedgeGeometryOverlayOperator" /> class.
    /// </summary>
    [TestFixture]
    public class HalfedgeGeometryOverlayOperatorTest
    {
        #region Private fields

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory _factory;

        /// <summary>
        /// The geometry overlay operator.
        /// </summary>
        private IGeometryOverlayOperator _operator;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _factory = new GeometryFactory();
            _operator = new HalfedgeGeometryOverlayOperator(_factory);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the various overlay methods.
        /// </summary>
        [Test]
        public void HalfedgeGeometryOverlayTest()
        {
            IMultiPolygon first = _factory.CreateMultiPolygon(new[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(20, 0),
                    _factory.CreatePoint(20, 10),
                    _factory.CreatePoint(10, 10)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 10),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(10, 20),
                    _factory.CreatePoint(0, 20)),
            });

            IMultiPolygon second = _factory.CreateMultiPolygon(new[]
            {
                _factory.CreatePolygon(
                    _factory.CreatePoint(-5, -5),
                    _factory.CreatePoint(5, -5),
                    _factory.CreatePoint(5, 5),
                    _factory.CreatePoint(-5, 5)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(5, -5),
                    _factory.CreatePoint(15, -5),
                    _factory.CreatePoint(15, 5),
                    _factory.CreatePoint(5, 5)),
                _factory.CreatePolygon(
                    _factory.CreatePoint(-5, -15),
                    _factory.CreatePoint(5, -15),
                    _factory.CreatePoint(5, -5),
                    _factory.CreatePoint(-5, -5)),
            });

            Coordinate[][] expected = new[]
            {
                // ExternalFirst
                new[]
                {
                    new Coordinate(0, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 20),
                    new Coordinate(0, 20),
                    new Coordinate(0, 10)
                },
                new[]
                {
                    new Coordinate(0, 5),
                    new Coordinate(5, 5),
                    new Coordinate(10, 5),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 5)
                },
                new[]
                {
                    new Coordinate(10, 5),
                    new Coordinate(15, 5),
                    new Coordinate(15, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 5)
                },
                // Internal
                new[]
                {
                    new Coordinate(5, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 5),
                    new Coordinate(5, 5),
                    new Coordinate(5, 0)
                },
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(15, 0),
                    new Coordinate(15, 5),
                    new Coordinate(10, 5),
                    new Coordinate(10, 0)
                },
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(5, 0),
                    new Coordinate(5, 5),
                    new Coordinate(0, 5),
                    new Coordinate(0, 0)
                },
                // ExternalSecond
                new[]
                {
                    new Coordinate(-5, -15),
                    new Coordinate(5, -15),
                    new Coordinate(5, -5),
                    new Coordinate(-5, -5),
                    new Coordinate(-5, -15)
                },
                new[]
                {
                    new Coordinate(-5, -5),
                    new Coordinate(5, -5),
                    new Coordinate(5, 0),
                    new Coordinate(0, 0),
                    new Coordinate(0, 5),
                    new Coordinate(-5, 5),
                    new Coordinate(-5, -5)
                },
                new[]
                {
                    new Coordinate(5, -5),
                    new Coordinate(15, -5),
                    new Coordinate(15, 0),
                    new Coordinate(10, 0),
                    new Coordinate(5, 0),
                    new Coordinate(5, -5)
                }
            };

            // Difference
            IGeometry result = _operator.Difference(first, second);
            Assert.IsInstanceOf<IGeometryCollection<IPolygon>>(result);

            IGeometryCollection<IPolygon> collection = (IGeometryCollection<IPolygon>) result;
            Assert.AreEqual(new[]
            {
                expected[0],
                expected[1],
                expected[2],
            }.ToRingSet(), collection.Select(polygon => polygon.Shell).ToRingSet());

            // Intersection
            result = _operator.Intersection(first, second);
            Assert.IsInstanceOf<IGeometryCollection<IPolygon>>(result);

            collection = (IGeometryCollection<IPolygon>)result;
            Assert.AreEqual(new[]
            {
                expected[3],
                expected[4],
                expected[5],
            }.ToRingSet(), collection.Select(polygon => polygon.Shell).ToRingSet());

            // Symmetric difference
            result = _operator.SymmetricDifference(first, second);
            Assert.IsInstanceOf<IGeometryCollection<IPolygon>>(result);

            collection = (IGeometryCollection<IPolygon>)result;
            Assert.AreEqual(new[]
            {
                expected[0],
                expected[1],
                expected[2],
                expected[6],
                expected[7],
                expected[8],
            }.ToRingSet(), collection.Select(polygon => polygon.Shell).ToRingSet());

            // Union
            result = _operator.Union(first, second);
            Assert.IsInstanceOf<IGeometryCollection<IPolygon>>(result);

            collection = (IGeometryCollection<IPolygon>)result;
            Assert.AreEqual(expected.ToRingSet(), collection.Select(polygon => polygon.Shell).ToRingSet());
        }

        /// <summary>
        /// Tests commutative containment.
        /// </summary>
        [Test]
        public void HalfedgeGeometryContainmentTest()
        {
            // Case 1
            ILinearRing first = _factory.CreateLinearRing(new Coordinate[]
            {
                new Coordinate(2, 2), 
                new Coordinate(8, 2), 
                new Coordinate(8, 8), 
                new Coordinate(2, 8), 
                new Coordinate(2, 2),
            });

            ILinearRing second = _factory.CreateLinearRing(new Coordinate[]
            {
                new Coordinate(0, 0), 
                new Coordinate(10, 0), 
                new Coordinate(10, 10), 
                new Coordinate(0, 10), 
                new Coordinate(0, 0),
            });

            // Intersection
            IGeometry result = _operator.Intersection(first, second);
            Assert.IsInstanceOf<IPolygon>(result);
            IPolygon polygon = (IPolygon)result;
            Assert.AreEqual(polygon.Shell.ToRing(), first.ToRing());

            result = _operator.Intersection(second, first);
            Assert.IsInstanceOf<IPolygon>(result);
            polygon = (IPolygon)result;
            Assert.AreEqual(polygon.Shell.ToRing(), first.ToRing());


            // Case 2 (similar to 1, only for testing a concrete issue)
            first = _factory.CreateLinearRing(new Coordinate[]
            {
                new Coordinate(308660.021186441, 4920043.37146893),
                new Coordinate(311001.021186441, 4920043.37146893),
                new Coordinate(311001.021186441, 4922922.37146893),
                new Coordinate(308660.021186441, 4922922.37146893),
                new Coordinate(308660.021186441, 4920043.37146893),
            });

            second = _factory.CreateLinearRing(new Coordinate[]
            { 
                new Coordinate(308333.75, 4920001.75),
                new Coordinate(311833.75, 4920001.75),
                new Coordinate(311833.75, 4923501.75),
                new Coordinate(308333.75, 4923501.75),
                new Coordinate(308333.75, 4920001.75),
            });

            // Intersection
            result = _operator.Intersection(first, second);
            Assert.IsInstanceOf<IPolygon>(result);
            polygon = (IPolygon)result;
            Assert.AreEqual(polygon.Shell.ToRing(), first.ToRing());

            result = _operator.Intersection(second, first);
            Assert.IsInstanceOf<IPolygon>(result);
            polygon = (IPolygon)result;
            Assert.AreEqual(polygon.Shell.ToRing(), first.ToRing());
        }

        #endregion
    }
}
