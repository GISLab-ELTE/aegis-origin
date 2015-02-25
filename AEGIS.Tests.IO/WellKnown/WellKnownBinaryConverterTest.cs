/// <copyright file="WellKnownTextConverterTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.WellKnown;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.IO.WellKnown
{
    /// <summary>
    /// Test fixture for the <see cref="WellKnownBinaryConverter" /> class.
    /// </summary>
    [TestFixture]
    public class WellKnownBinaryConverterTest
    {
        #region Private fields

        /// <summary>
        /// The array of geometries.
        /// </summary>
        private IGeometry[] _geometries;

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
        public void SetUp()
        {
            _factory = Factory.DefaultInstance<IGeometryFactory>();

            _geometries = new IGeometry[]
            {
                _factory.CreatePoint(1,1,1),
                _factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i, i))),
                _factory.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i * i, i * i, i * i)), 
                            new Coordinate[][] { Enumerable.Range(1, 100).Select(i => new Coordinate(i, i, i)).ToArray(), 
                                                 Enumerable.Range(1, 4).Select(i => new Coordinate(i, i, i)).ToArray() }),
                _factory.CreateMultiPoint(Enumerable.Range(1, 4).Select(i => _factory.CreatePoint(i, i, i))),
                _factory.CreateMultiLineString(new ILineString[] { _factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i, i))) }),
                _factory.CreateMultiPolygon(new IPolygon[] { _factory.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i * i, i * i, i * i)), 
                                                                                    new Coordinate[][] { Enumerable.Range(1, 4).Select(i => new Coordinate(i, i, i)).ToArray(), Enumerable.Range(1, 4).Select(i => new Coordinate(i, i, i)).ToArray() }) })
            };
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the conversion for geometries
        /// </summary>
        [Test]
        public void WellKnownBinaryConverterGeometryTest()
        {
            foreach (IGeometry geometry in _geometries)
            {
                Byte[] wkb = WellKnownBinaryConverter.ToWellKnownBinary(geometry);
                IGeometry converted = WellKnownBinaryConverter.ToGeometry(wkb, _factory);

                Assert.AreEqual(0, new GeometryComparer().Compare(geometry, converted));
            }
        }

        #endregion
    }
}
