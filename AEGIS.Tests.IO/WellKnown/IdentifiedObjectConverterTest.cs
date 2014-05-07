/// <copyright file="IdentifiedObjectConverterTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.WellKnown;
using ELTE.AEGIS.Reference;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.IO.WellKnown
{
    [TestFixture]
    public class IdentifiedObjectConverterTest
    {
        [TestCase]
        public void IdentifiedObjectConverterTextConversionTest()
        {
            String projectedUTMZone33 = "PROJCS[\"UTM Zone 33, Northern Hemisphere\",GEOGCS[\"Geographic Coordinate System\",DATUM[\"WGS84\",SPHEROID[\"WGS84\",6378137,298.257223560493]],PRIMEM[\"Greenwich\",0],UNIT[\"degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",15],PARAMETER[\"scale_factor\",0.9996],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1]]";
            String UTMZone10NAD83 = "PROJCS[\"NAD_1983_UTM_Zone_10N\", GEOGCS[\"GCS_North_American_1983\", DATUM[ \"D_North_American_1983\",ELLIPSOID[\"GRS_1980\",6378137,298.257222101]], PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]], PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0], PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",-123.0], PARAMETER[\"Scale_Factor\",0.9996],PARAMETER[\"Latitude_of_Origin\",0.0], UNIT[\"Meter\",1.0]]";
            String geodeticBase = "GEOGCS[\"Unknown\",DATUM[\"D_Unknown\",SPHEROID[\"Clarke_1866\",6378206.4,294.978698213901]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]";
            String wgs1984 = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295]]";

            IdentifiedObjectConverter.ToWellKnownText(IdentifiedObjectConverter.ToIdentifiedObject(projectedUTMZone33));
            IdentifiedObjectConverter.ToWellKnownText(IdentifiedObjectConverter.ToIdentifiedObject(UTMZone10NAD83));
            IdentifiedObjectConverter.ToWellKnownText(IdentifiedObjectConverter.ToIdentifiedObject(geodeticBase));
            IdentifiedObjectConverter.ToWellKnownText(IdentifiedObjectConverter.ToIdentifiedObject(wgs1984));
        }

        [TestCase]
        public void IdentifiedObjectConverterObjectConversionTest()
        {
            foreach (ProjectedCoordinateReferenceSystem projected in ProjectedCoordinateReferenceSystems.All)
            {
                String wkt = IdentifiedObjectConverter.ToWellKnownText(projected as IReferenceSystem);
                ProjectedCoordinateReferenceSystem converted = IdentifiedObjectConverter.ToIdentifiedObject(wkt) as ProjectedCoordinateReferenceSystem;
                Assert.AreEqual(projected, converted);
            }

            foreach (GeographicCoordinateReferenceSystem geodetic in Geographic2DCoordinateReferenceSystems.All)
            {
                String wkt = IdentifiedObjectConverter.ToWellKnownText(geodetic as IReferenceSystem);
                GeographicCoordinateReferenceSystem converted = IdentifiedObjectConverter.ToIdentifiedObject(wkt) as GeographicCoordinateReferenceSystem;
                Assert.AreEqual(geodetic, converted);
            }
        }
    }
}
