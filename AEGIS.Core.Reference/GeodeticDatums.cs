/// <copyright file="GeodeticDatums.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="GeodeticDatum" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(GeodeticDatum))]
    public static class GeodeticDatums
    {
        #region Query fields

        private static GeodeticDatum[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="GeodeticDatum" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="GeodeticDatum" /> instances within the collection.</value>
        public static IList<GeodeticDatum> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(GeodeticDatums).GetProperties().
                                                  Where(property => property.Name != "All").
                                                  Select(property => property.GetValue(null, null) as GeodeticDatum).
                                                  ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GeodeticDatum" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="GeodeticDatum" /> instances that match the specified identifier.</returns>
        public static IList<GeodeticDatum> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="GeodeticDatum" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="GeodeticDatum" /> instances that match the specified name.</returns>
        public static IList<GeodeticDatum> FromName(String name)
        {
            if (name == null)
                return null;

            // name correction
            name = Regex.Escape(name);

            return All.Where(obj => Regex.IsMatch(obj.Name, name, RegexOptions.IgnoreCase) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => Regex.IsMatch(alias, name, RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static GeodeticDatum _HD1909;
        private static GeodeticDatum _GGRS87;
        private static GeodeticDatum _Carthage;
        private static GeodeticDatum _HD72;
        private static GeodeticDatum _ETRS89;
        private static GeodeticDatum _NZGD49;
        private static GeodeticDatum _NAD27;
        private static GeodeticDatum _NAD83;
        private static GeodeticDatum _OSGB1936;
        private static GeodeticDatum _Amersfoort;
        private static GeodeticDatum _WGS84;
        private static GeodeticDatum _PD83;
        private static GeodeticDatum _Voirol1879Paris;

        #endregion

        #region Public static properties

        /// <summary>
        /// Hungarian Datum 1909 (HD1909).
        /// </summary>
        public static GeodeticDatum HD1909
        {
            get
            {
                if (_HD1909 == null)
                    _HD1909 = new GeodeticDatum("EPSG::1024", "Hungarian Datum 1909 (HD1909)",
                                                "Replaced earlier HD1863 adjustment also on Bessel ellipsoid. Both HD1863 and HD1909 were originally on Ferro Prime Meridian but subsequently converted to Greenwich. Replaced by HD72.",
                                                null,
                                                "Fundamental point not given in information source, but presumably Szolohegy which is origin of later HD72.", 
                                                "1909", "Topographic mapping.",
                                                AreasOfUse.Hungary, Ellipsoids.Bessel1841, Meridians.Greenwich);
                return _HD1909;
            }
        }

        /// <summary>
        /// Greek Geodetic Reference System Datum 1987 (GGRS87).
        /// </summary>
        public static GeodeticDatum GGRS87
        {
            get
            {
                if (_GGRS87 == null)
                    _GGRS87 = new GeodeticDatum("EPSG::6121", "Greek Geodetic Reference System Datum 1987 (GGRS87)",
                                                "Replaced (old) Greek datum. Oil industry work based on ED50.",
                                                null,
                                                "Fundamental point: Dionysos. Latitude 38°04'33.8\"N, longitude 23°55'51.0\"E of Greenwich; geoid height 7.0 m.", 
                                                "1987", "Topographic mapping.",
                                                AreasOfUse.GreeceOnshore, Ellipsoids.GRS1980, Meridians.Greenwich);
                return _GGRS87;
            }
        }

        /// <summary>
        /// Carthage.
        /// </summary>
        public static GeodeticDatum Carthage
        {
            get
            {
                if (_Carthage == null)
                    _Carthage = new GeodeticDatum("EPSG::6223", "Carthage",
                                                  "Fundamental point astronomic coordinates determined in 1878.",
                                                  null,
                                                  "Fundamental point: Carthage. Latitude: 40.9464506g = 36°51'06.50\"N, longitude: 8.8724368g E of Paris = 10°19'20.72\"E (of Greenwich).", 
                                                  "1925", "Topographic mapping.",
                                                  AreasOfUse.Tunisia, Ellipsoids.Clarke1880IGN, Meridians.Greenwich);
                return _Carthage;
            }
        }

        /// <summary>
        /// Hungarian Datum 1972 (HD72).
        /// </summary>
        public static GeodeticDatum HD72
        {
            get
            {
                if (_HD72 == null)
                    _HD72 = new GeodeticDatum("EPSG::6237", "Hungarian Datum 1972 (HD72)",
                                              "Replaced Hungarian Datum 1909.",
                                              null,
                                              "Fundamental point: Szőlőhegy. Latitude: 47°17'32,6156\"N, longitude 19°36'09.9865\"E (of Greenwich); geoid height 6.56m.", "1972",
                                              "Topographic mapping.",
                                              AreasOfUse.Hungary,
                                              Ellipsoids.GRS1967, Meridians.Greenwich);
                return _HD72;
            }
        }

        /// <summary>
        /// European Terrestrial System 1989 (ETRS89).
        /// </summary>
        public static GeodeticDatum ETRS89
        {
            get
            {
                if (_ETRS89 == null)
                    _ETRS89 = new GeodeticDatum("EPSG::6258", "European Terrestrial System 1989 (ETRS89)",
                                                "The distinction in usage between ETRF89 and ETRS89 is confused: although in principle conceptually different in practice both are used for the realization.",
                                                new String[] { "European Terrestrial Reference Frame 1989 (ETRF89)" },
                                                "Fixed to the stable part of the Eurasian continental plate and consistent with ITRS at the epoch 1989.0.", 
                                                "1989", "Geodetic survey.",
                                                AreasOfUse.EuropeETRS89, Ellipsoids.GRS1980, Meridians.Greenwich);
                return _ETRS89;
            }
        }

        /// <summary>
        /// New Zealand Geodetic Datum 1949 (NZGD49).
        /// </summary>
        public static GeodeticDatum NZGD49
        {
            get
            {
                if (_NZGD49 == null)
                    _NZGD49 = new GeodeticDatum("EPSG::6272", "New Zealand Geodetic Datum 1949 (NZGD49)",
                                                "Replaced by New Zealand Geodetic Datum 2000 from March 2000.",
                                                new String[] { "GD49" },
                                                "Fundamental point: Papatahi. Latitude: 41°19' 8.900\"S, longitude: 175°02'51.000\"E (of Greenwich).", 
                                                "1949", "Geodetic survey, cadastre, topographic mapping, engineering survey.",
                                                AreasOfUse.NewZealandOnshoreNearShore, Ellipsoids.International1924, Meridians.Greenwich);
                return _NZGD49;
            }
        }

        /// <summary>
        /// North American Datum 1927 (NAD27).
        /// </summary>
        public static GeodeticDatum NAD27
        {
            get
            {
                if (_NAD27 == null)
                    _NAD27 = new GeodeticDatum("EPSG::6267", "North American Datum 1927 (NAD27)",
                                               "In United States (USA) and Canada, replaced by North American Datum 1983 (NAD83); in Mexico, replaced by Mexican Datum of 1993.",
                                               null,
                                               "Fundamental point: Meade's Ranch. Latitude: 39°13'26.686\"N, longitude: 98°32'30.506\"W (of Greenwich).", 
                                               "1927", "Topographic mapping.",
                                               AreasOfUse.NorthAmericaNAD27, Ellipsoids.Clarke1866, Meridians.Greenwich);
                return _NAD27;
            }
        }

        /// <summary>
        /// North American Datum 1983 (NAD83).
        /// </summary>
        public static GeodeticDatum NAD83
        {
            get
            {
                if (_NAD83 == null)
                    _NAD83 = new GeodeticDatum("EPSG::6269", "North American Datum 1983 (NAD83)",
                                               "Although the 1986 adjustment included connections to Greenland and Mexico, it has not been adopted there. In Canada and US, replaced NAD27.",
                                               null,
                                               "Origin at geocentre.", 
                                               "1986", "Topographic mapping.",
                                               AreasOfUse.NorthAmericaNAD83, Ellipsoids.GRS1980, Meridians.Greenwich);
                return _NAD83;
            }
        }

        /// <summary>
        /// Ordnance Survey Great Britain 1936 (OSGB 1936).
        /// </summary>
        public static GeodeticDatum OSGB1936
        {
            get
            {
                if (_OSGB1936 == null)
                    _OSGB1936 = new GeodeticDatum("EPSG::6277", "Ordnance Survey Great Britain 1936 (OSGB 1936)",
                                                  "The average accuracy of OSTN02 compared to the old triangulation network (down to 3rd order) is 0.1m. ",
                                                  null,
                                                  "From April 2002 the datum is defined through the application of the OSTN02 transformation (tfm code 1039) to ETRS89. Prior to 2002, fundamental point: Herstmonceux, Latitude: 50°51'55.271\"N, longitude: 0°20'45.882\"E (of Greenwich).", 
                                                  "1936", "Topographic mapping.",
                                                  AreasOfUse.GreatBritainMan, Ellipsoids.Airy1830, Meridians.Greenwich);
                return _OSGB1936;
            }
        }

        /// <summary>
        /// Amersfoort.
        /// </summary>
        public static GeodeticDatum Amersfoort
        {
            get
            {
                if (_Amersfoort == null)
                    _Amersfoort = new GeodeticDatum("EPSG::6289", "Amersfoort",
                                                    null,
                                                    null,
                                                    "Fundamental point: Amersfoort. Latitude: 52°09'22.178\"N, longitude: 5°23'15.478\"E (of Greenwich).", 
                                                    null, "Geodetic survey, cadastre, topographic mapping, engineering survey.",
                                                    AreasOfUse.NetherlandsOnshore, Ellipsoids.Bessel1841, Meridians.Greenwich);
                return _Amersfoort;
            }
        }

        /// <summary>
        /// World Geodetic System 1984 (WGS84).
        /// </summary>
        public static GeodeticDatum WGS84
        {
            get
            {
                if (_WGS84 == null)
                    _WGS84 = new GeodeticDatum("EPSG::6326", "World Geodetic System 1984 (WGS84)",
                                               " distinction is made between the original WGS 84 frame, WGS 84 (G730), WGS 84 (G873) and WGS 84 (G1150). Since 1997, WGS 84 has been maintained within 10cm of the then current ITRF.",
                                               null,
                                               "Defined through a consistent set of station coordinates. These have changed with time: by 0.7m on 29/6/1994 [WGS 84 (G730)], a further 0.2m on 29/1/1997 [WGS 84 (G873)] and a further 0.06m on 20/1/2002 [WGS 84 (G1150)].", 
                                               "1984", "Satellite navigation.",
                                               AreasOfUse.World, Ellipsoids.WGS1984, Meridians.Greenwich);
                return _WGS84;
            }
        }

        /// <summary>
        /// Potsdam Datum 1983 (PD83).
        /// </summary>
        public static GeodeticDatum PD83
        {
            get
            {
                if (_PD83 == null)
                    _PD83 = new GeodeticDatum("EPSG::6746", "Potsdam Datum 1983 (PD83)",
                                              "PD83 is the realization of DHDN in Thuringen. It is the resultant of applying a transformation derived at 13 points on the border between East and West Germany to Pulkovo 1942/83 points in Thuringen.",
                                              null,
                                              "Fundamental point: Rauenberg. Latitude: 52°27'12.021\"N, longitude: 13°22'04.928\"E (of Greenwich). This station was destroyed in 1910 and the station at Potsdam substituted as the fundamental point.",
                                              "1990", "Geodetic survey, cadastre, topographic mapping, engineering survey.", 
                                              AreasOfUse.GermanyThuringen, Ellipsoids.Bessel1841, Meridians.Greenwich);
                return _PD83;
            }
        }

        /// <summary>
        /// Voirol 1879 (Paris).
        /// </summary>
        public static GeodeticDatum Voirol1879Paris
        {
            get
            {
                if (_Voirol1879Paris == null)
                    _Voirol1879Paris = new GeodeticDatum("EPSG::6821", "Voirol 1879 (Paris)",
                                                         "Replaces Voirol 1875 (Paris).",
                                                         null,
                                                         "Fundamental point: Voirol. Latitude: 40.835864 grads N, longitude: 0.788735 grads E (of Paris).",
                                                         "1879", "Topographic mapping.",
                                                         AreasOfUse.AlgeriaN32, Ellipsoids.Clarke1880IGN, Meridians.Paris);
                return _Voirol1879Paris;
            }
        }

        #endregion
    }
}
