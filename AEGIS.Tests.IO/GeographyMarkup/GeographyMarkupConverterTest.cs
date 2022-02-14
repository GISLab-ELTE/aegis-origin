/// <copyright file="GeometryConverterTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Daniel Ballagi</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.IO.GeographyMarkup;
using NUnit.Framework;
using System;
using System.Linq;
using System.Xml.Linq;

namespace ELTE.AEGIS.Tests.IO.GeographyMarkup
{
    /// <summary>
    /// Test fixture for the <see cref="GeographyMarkupConverter" /> class.
    /// </summary>
    [TestFixture]
    public class GeographyMarkupConverterTest
    {
        #region Private fields

        /// <summary>
        /// The geometries.
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
            _factory = new GeometryFactory();

            _geometries = new IGeometry[]
            {
                _factory.CreatePoint(1,1),
                _factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i))),
                _factory.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i * i, i * i)), 
                            new Coordinate[][] { Enumerable.Range(1, 100).Select(i => new Coordinate(i, i)).ToArray(), 
                                                 Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray() }),
                _factory.CreateMultiPoint(Enumerable.Range(1, 4).Select(i => _factory.CreatePoint(i, i))),
                _factory.CreateMultiLineString(new ILineString[] { _factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i))) }),
                _factory.CreateMultiPolygon(new IPolygon[] { _factory.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i * i, i * i)), 
                                                                                    new Coordinate[][] { Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray(), Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray() }) })
            
            };
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the conversion to geography markup.
        /// </summary>
        [Test]
        public void GeometryConverterGeometryMarkupTest()
        {
            foreach (IGeometry geometry in _geometries)
            {
                XElement gml = GeographyMarkupConverter.ToGeographyMarkupInXElement(geometry);
                IGeometry converted = GeographyMarkupConverter.ToGeometry(gml, _factory);

                Assert.AreEqual(0, new GeometryComparer().Compare(geometry, converted));
            }
        }

        /// <summary>
        /// Tests the conversion to string.
        /// </summary>
        [Test]
        public void GeometryConverterStringTest()
        {
            foreach (IGeometry geometry in _geometries)
            {
                String gml = GeographyMarkupConverter.ToGeographyMarkupInString(geometry);
                IGeometry converted = GeographyMarkupConverter.ToGeometry(gml, _factory);

                Assert.AreEqual(0, new GeometryComparer().Compare(geometry, converted));
            }
        }

        #endregion
    }
}
