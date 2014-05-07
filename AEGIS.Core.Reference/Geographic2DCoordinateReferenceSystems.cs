/// <copyright file="Geographic2DCoordinateReferenceSystems.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="GeographicCoordinateReferenceSystem" /> instances for 2D coordinates.
    /// </summary>
    [IdentifiedObjectCollection(typeof(GeographicCoordinateReferenceSystem))]
    public class Geographic2DCoordinateReferenceSystems
    {
        #region Query fields

        private static GeographicCoordinateReferenceSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="GeographicCoordinateReferenceSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="GeographicCoordinateReferenceSystem" /> instances within the collection.</value>
        public static IList<GeographicCoordinateReferenceSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(Geographic2DCoordinateReferenceSystems).GetProperties().
                                                                          Where(property => property.Name != "All").
                                                                          Select(property => property.GetValue(null, null) as GeographicCoordinateReferenceSystem).
                                                                          ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GeographicCoordinateReferenceSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A read-only list containing the <see cref="GeographicCoordinateReferenceSystem" /> instances that match the specified identifier.</returns>
        public static IList<GeographicCoordinateReferenceSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="GeographicCoordinateReferenceSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A read-only list containing the <see cref="GeographicCoordinateReferenceSystem" /> instances that match the specified name.</returns>
        public static IList<GeographicCoordinateReferenceSystem> FromName(String name)
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

        private static GeographicCoordinateReferenceSystem _amersfoort;
        private static GeographicCoordinateReferenceSystem _ETRS89;
        private static GeographicCoordinateReferenceSystem _HD72;
        private static GeographicCoordinateReferenceSystem _NAD27;
        private static GeographicCoordinateReferenceSystem _NAD83;
        private static GeographicCoordinateReferenceSystem _NAD83NSR2007;
        private static GeographicCoordinateReferenceSystem _OSGB36;
        private static GeographicCoordinateReferenceSystem _WGS84;

        #endregion

        #region Public static properties

        /// <summary>
        /// Amersfoort.
        /// </summary>
        public static GeographicCoordinateReferenceSystem Amersfoort
        {
            get
            {
                return _amersfoort ?? (_amersfoort =
                    new GeographicCoordinateReferenceSystem("EPSG::4289", "Amersfoort",
                                                            null,
                                                            null,
                                                            "Geodetic survey.",
                                                            CoordinateSystems.EllipsoidalLatLonD,
                                                            GeodeticDatums.Amersfoort,
                                                            AreasOfUse.NetherlandsOnshore));
            }
        }

        /// <summary>
        /// ETRS89.
        /// </summary>
        public static GeographicCoordinateReferenceSystem ETRS89
        {
            get
            {
                return _ETRS89 ?? (_ETRS89 =
                    new GeographicCoordinateReferenceSystem("EPSG::4258", "ETRS89",
                                                            "The distinction in usage between ETRF89 and ETRS89 is confused: although in principle conceptually different in practice both are used as synonyms.",
                                                            new String[] { "ETRF89", "ETRS89-GRS80h", "ETRS 1989", "ETRF 1989", "European Terrestrial System 1989", "European Terrestrial Reference Frame 1989", "European Terrestrial System 1989 (ETRS89)", "European Terrestrial Reference Frame 1989 (ETRF89)" },
                                                            "Horizontal component of 3D system.",
                                                            CoordinateSystems.EllipsoidalLatLonD,
                                                            GeodeticDatums.ETRS89,
                                                            AreasOfUse.EuropeETRS89));
            }
        }

        /// <summary>
        /// HD72.
        /// </summary>
        public static GeographicCoordinateReferenceSystem HD72
        {
            get
            {
                return _HD72 ?? (_HD72 =
                    new GeographicCoordinateReferenceSystem("EPSG::4237", "HD72",
                                                          "Replaced HD1909.",
                                                          new String[] { "HD 1972", "Hungarian Datum 1972", "Hungarian Datum 1972 (HD72)" },
                                                          "Geodetic survey.",
                                                          CoordinateSystems.EllipsoidalLatLonD,
                                                          GeodeticDatums.HD72,
                                                          AreasOfUse.Hungary));
            }
        }

        /// <summary>
        /// NAD27.
        /// </summary>
        public static GeographicCoordinateReferenceSystem NAD27
        {
            get
            {
                return _NAD27 ?? (_NAD27 =
                    new GeographicCoordinateReferenceSystem("EPSG::4267", "NAD27",
                                                          "Note: this coordinate system includes longitudes which are POSITIVE EAST. Replaced by NAD27(76) in Ontario, CGQ77 in Quebec, Mexican Datum of 1993 in Mexico, NAD83 in Canada (excl. Ontario & Quebec) & USA.",
                                                          new String[] { "NAD 1927", "North American Datum 1927", "North American Datum 1927 (NAD27)" },
                                                          "Geodetic survey.",
                                                          CoordinateSystems.EllipsoidalLatLonD,
                                                          GeodeticDatums.NAD27,
                                                          AreasOfUse.NorthAmericaNAD27));
            }
        }

        /// <summary>
        /// NAD83.
        /// </summary>
        public static GeographicCoordinateReferenceSystem NAD83
        {
            get
            {
                return _NAD83 ?? (_NAD83 =
                    new GeographicCoordinateReferenceSystem("EPSG::4269", "NAD83",
                                                            "This CRS includes longitudes which are POSITIVE EAST. The adjustment included connections to Greenland and Mexico but the system has not been adopted there. Except in Alaska, for applications with an accuracy of better than 1m replaced by NAD83(HARN).",
                                                            new String[] { "NAD 1983", "North American Datum 1983" },
                                                            "Geodetic survey.",
                                                            CoordinateSystems.EllipsoidalLatLonD,
                                                            GeodeticDatums.NAD83,
                                                            AreasOfUse.NorthAmericaNAD83));
            }
        }

        /// <summary>
        /// NAD83 (NSRS2007).
        /// </summary>
        public static GeographicCoordinateReferenceSystem NAD83NSR2007
        {
            get
            {
                return _NAD83NSR2007 ?? (_NAD83NSR2007 =
                    new GeographicCoordinateReferenceSystem("EPSG::4759", "NAD83 (NSRS2007)",
                                                          "Note: this coordinate system includes longitudes which are POSITIVE EAST. In Continental US, replaces NAD83(HARN). In Alaska, for accuracies of better than 1m replaces NAD83.",
                                                          new String[] { "NAD 1983 (NSRS2007)", "North American Datum 1983 (NSRS2007)" },
                                                          "Horizontal component of 3D system.",
                                                          CoordinateSystems.EllipsoidalLatLonD,
                                                          GeodeticDatums.NAD83,
                                                          AreasOfUse.USACONUSAlaskaPRVI));
            }
        }

        /// <summary>
        /// OSGB36.
        /// </summary>
        public static GeographicCoordinateReferenceSystem OSGB36
        {
            get
            {
                return _OSGB36 ?? (_OSGB36 =
                    new GeographicCoordinateReferenceSystem("EPSG::4277", "OSGB36",
                                                          null,
                                                          new String[] { "OSGB 1936", "Ordinance Survey Great Britain 1936", "Ordinance Survey Great Britain 1936 (OSGB36)" },
                                                          "Geodetic survey.",
                                                          CoordinateSystems.EllipsoidalLatLonD,
                                                          GeodeticDatums.OSGB1936,
                                                          AreasOfUse.GreatBritainMan));
            }
        }

        /// <summary>
        /// WGS84.
        /// </summary>
        public static GeographicCoordinateReferenceSystem WGS84
        {
            get
            {
                return _WGS84 ?? (_WGS84 =
                    new GeographicCoordinateReferenceSystem("EPSG::4326", "WGS84",
                                                          null,
                                                          new String[] { "WGS 84", "WGS 1984", "World Geodetic System 1984", "World Geodetic System 1984 (WGS84)" },
                                                          "Horizontal component of 3D system. Used by the GPS satellite navigation system and for NATO military geodetic surveying.",
                                                          CoordinateSystems.EllipsoidalLatLonD,
                                                          GeodeticDatums.WGS84,
                                                          AreasOfUse.World));
            }
        }

        #endregion
    }
}
