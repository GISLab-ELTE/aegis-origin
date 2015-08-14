/// <copyright file="HalfedgeGeometryRelateOperatorTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations.Geometry;
using NUnit.Framework;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Geometry
{
    /// <summary>
    /// Test fixture for the <see cref="HalfedgeGeometryRelateOperator" /> class.
    /// </summary>
    [TestFixture]
    public class HalfedgeGeometryRelateOperatorTest
    {
        #region Private fields

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory _factory;

        /// <summary>
        /// The geometry overlay operator.
        /// </summary>
        private IGeometryRelateOperator _operator;

        #endregion

        #region Test setup

        /// <summary>
        /// Test fixture setup.
        /// </summary>
        [TestFixtureSetUp]
        public void FixtureInitialize()
        {
            _factory = new GeometryFactory();
            _operator = new HalfedgeGeometryRelateOperator();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the various spatial relate methods.
        /// </summary>
        [Test]
        public void HalfedgeGeometryRelateTest()
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

            IPolygon third = _factory.CreatePolygon(
                _factory.CreatePoint(3, 5),
                _factory.CreatePoint(7, 5),
                _factory.CreatePoint(7, 15),
                _factory.CreatePoint(3, 15));

            Assert.IsFalse(_operator.Contains(first, first));  // equals is not contains
            Assert.IsFalse(_operator.Contains(first, second));
            Assert.IsTrue(_operator.Contains(first, third));
            Assert.IsFalse(_operator.Contains(second, third));

            Assert.IsFalse(_operator.Within(first, first));
            Assert.IsFalse(_operator.Within(first, second));
            Assert.IsFalse(_operator.Within(first, third));
            Assert.IsFalse(_operator.Within(second, third));

            Assert.IsTrue(_operator.Intersects(first, first));
            Assert.IsTrue(_operator.Intersects(first, second));
            Assert.IsTrue(_operator.Intersects(first, third));
            Assert.IsTrue(_operator.Intersects(second, third));

            Assert.IsFalse(_operator.Disjoint(first, first));
            Assert.IsFalse(_operator.Disjoint(first, second));
            Assert.IsFalse(_operator.Disjoint(first, third));
            Assert.IsFalse(_operator.Disjoint(second, third));

            // for operands with the same dimension crosses is always false
            Assert.IsFalse(_operator.Crosses(first, first));
            Assert.IsFalse(_operator.Crosses(first, second));
            Assert.IsFalse(_operator.Crosses(first, third));
            Assert.IsFalse(_operator.Crosses(second, third));

            Assert.IsTrue(_operator.Equals(first, first));
            Assert.IsFalse(_operator.Equals(first, second));
            Assert.IsFalse(_operator.Equals(first, third));
            Assert.IsFalse(_operator.Equals(second, third));

            Assert.IsFalse(_operator.Touches(first, first));
            Assert.IsFalse(_operator.Touches(first, second));
            Assert.IsFalse(_operator.Touches(first, third));
            Assert.IsTrue(_operator.Touches(second, third));

            Assert.IsFalse(_operator.Overlaps(first, first));
            Assert.IsTrue(_operator.Overlaps(first, second));
            Assert.IsFalse(_operator.Overlaps(first, third));  // contains is not overlaps
            Assert.IsFalse(_operator.Overlaps(second, third)); // the result has not the same dimension  
        }

        #endregion
    }
}
