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
using ELTE.AEGIS.Reference;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.IO.WellKnown
{
    /// <summary>
    /// Test fixture for the <see cref="WellKnownTextConverter" /> class.
    /// </summary>
    [TestFixture]
    public class WellKnownTextConverterTest
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
        /// Tests the conversion for geometries.
        /// </summary>
        [Test]
        public void WellKnownTextConverterGeometryTest()
        {
            foreach (IGeometry geometry in _geometries)
            {
                String wkt = WellKnownTextConverter.ToWellKnownText(geometry);
                IGeometry converted = WellKnownTextConverter.ToGeometry(wkt, _factory);

                Assert.AreEqual(0, new GeometryComparer().Compare(geometry, converted));
            }
        }

        /// <summary>
        /// Tests the conversion of identified objects.
        /// </summary>
        [Test]
        public void WellKnownTextConverterIdentifiedObjectTest()
        {
            // conversion from string instance

            String projectedUTMZone33 = "PROJCS[\"UTM Zone 33, Northern Hemisphere\",GEOGCS[\"Geographic Coordinate System\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137,298.257223560493]],PRIMEM[\"Greenwich\",0],UNIT[\"degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",15],PARAMETER[\"scale_factor\",0.9996],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1]]";
            String UTMZone10NAD83 = "PROJCS[\"NAD_1983_UTM_Zone_10N\", GEOGCS[\"GCS_North_American_1983\", DATUM[ \"D_North_American_1983\",ELLIPSOID[\"GRS_1980\",6378137,298.257222101]], PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]], PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0], PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",-123.0], PARAMETER[\"Scale_Factor\",0.9996],PARAMETER[\"Latitude_of_Origin\",0.0], UNIT[\"Meter\",1.0]]";
            String geodeticBase = "GEOGCS[\"Unknown\",DATUM[\"D_Unknown\",SPHEROID[\"Clarke_1866\",6378206.4,294.978698213901]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]";
            String wgs1984 = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295]]";

            Assert.IsNotNullOrEmpty(WellKnownTextConverter.ToWellKnownText(WellKnownTextConverter.ToIdentifiedObject(projectedUTMZone33)));
            Assert.IsNotNullOrEmpty(WellKnownTextConverter.ToWellKnownText(WellKnownTextConverter.ToIdentifiedObject(UTMZone10NAD83)));
            Assert.IsNotNullOrEmpty(WellKnownTextConverter.ToWellKnownText(WellKnownTextConverter.ToIdentifiedObject(geodeticBase)));
            Assert.IsNotNullOrEmpty(WellKnownTextConverter.ToWellKnownText(WellKnownTextConverter.ToIdentifiedObject(wgs1984)));

            // conversion from identified object instance

            foreach (ProjectedCoordinateReferenceSystem projected in ProjectedCoordinateReferenceSystems.All)
            {
                String wkt = WellKnownTextConverter.ToWellKnownText(projected as IReferenceSystem);
                ProjectedCoordinateReferenceSystem converted = WellKnownTextConverter.ToIdentifiedObject(wkt) as ProjectedCoordinateReferenceSystem;
                Assert.AreEqual(projected, converted);
            }

            foreach (GeographicCoordinateReferenceSystem geodetic in Geographic2DCoordinateReferenceSystems.All)
            {
                String wkt = WellKnownTextConverter.ToWellKnownText(geodetic as IReferenceSystem);
                GeographicCoordinateReferenceSystem converted = WellKnownTextConverter.ToIdentifiedObject(wkt) as GeographicCoordinateReferenceSystem;
                Assert.AreEqual(geodetic, converted);
            }
        }

        #endregion
    }
}
