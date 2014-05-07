/// <copyright file="GeometryConverterTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.IO.WellKnown;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.IO.WellKnown
{
    [TestFixture]
    public class GeometryConverterTest
    {
        private IGeometry[] _geometries;
        private IGeometryFactory _factory;

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

        [TestCase]
        public void GeometryConverterWellKnownBinaryTest()
        {
            foreach (IGeometry geometry in _geometries)
            {
                Byte[] wkb = GeometryConverter.ToWellKnownBinary(geometry);
                IGeometry converted = GeometryConverter.ToGeometry(wkb, _factory);

                Assert.AreEqual(0, new GeometryComparer().Compare(geometry, converted));
            }
        }

        [TestCase]
        public void GeometryConverterWellKnownTextTest()
        {
            foreach (IGeometry geometry in _geometries)
            {
                String wkt = GeometryConverter.ToWellKnownText(geometry);
                IGeometry converted = GeometryConverter.ToGeometry(wkt, _factory);

                Assert.AreEqual(0, new GeometryComparer().Compare(geometry, converted));
            }
        }
    }
}
