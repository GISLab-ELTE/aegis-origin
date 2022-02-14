/// <copyright file="ProjectedCoordinateReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Reference.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="ProjectedCoordinateReferenceSystem" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(ProjectedCoordinateReferenceSystem))]
    public static class ProjectedCoordinateReferenceSystems
    {
        #region Query fields

        private static ProjectedCoordinateReferenceSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="T:ELTE.AEGIS.Reference.ProjectedCoordinateReferenceSystem"/> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="T:ELTE.AEGIS.Reference.ProjectedCoordinateReferenceSystem"/> instances within the collection.</value>
        public static IList<ProjectedCoordinateReferenceSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(ProjectedCoordinateReferenceSystems).GetProperties().
                                                                       Where(property => property.Name != "All").
                                                                       Select(property => property.GetValue(null, null) as ProjectedCoordinateReferenceSystem).
                                                                       ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns <see cref="T:ELTE.AEGIS.Reference.ProjectedCoordinateReferenceSystem"/> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="T:ELTE.AEGIS.Reference.ProjectedCoordinateReferenceSystem"/> instances that match the specified identifier.</returns>
        public static IList<ProjectedCoordinateReferenceSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }
        /// <summary>
        /// Returns <see cref="T:ELTE.AEGIS.Reference.ProjectedCoordinateReferenceSystem"/> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="T:ELTE.AEGIS.Reference.ProjectedCoordinateReferenceSystem"/> instances that match the specified name.</returns>
        public static IList<ProjectedCoordinateReferenceSystem> FromName(String name)
        {
            if (name == null)
                return null;

            // name correction
            name = Regex.Escape(name);

            return All.Where(obj => Regex.IsMatch(obj.Name, name, RegexOptions.IgnoreCase) || 
                                    Regex.IsMatch(obj.Name.Replace("/ ", String.Empty), name, RegexOptions.IgnoreCase) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => Regex.IsMatch(alias, name, RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static ProjectedCoordinateReferenceSystem _amersfoort_RDNew;
        private static ProjectedCoordinateReferenceSystem _HD72_EOV;
        private static ProjectedCoordinateReferenceSystem _NAD27_TexasSouthCentral;
        private static ProjectedCoordinateReferenceSystem _NAD83_AlabamaWest;
        private static ProjectedCoordinateReferenceSystem _OSGB36_BritishNationalGrid;
        private static ProjectedCoordinateReferenceSystem _WGS84_UPSN_EN;
        private static ProjectedCoordinateReferenceSystem _WGS84_UPSS_EN;
        private static ProjectedCoordinateReferenceSystem _WGS84_WorldMercator;

        #endregion

        #region Public static properties

        /// <summary>
        /// Amersfoort / RD New. 
        /// </summary>
        public static ProjectedCoordinateReferenceSystem Amersfoort_RDNew
        {
            get
            {
                if (_amersfoort_RDNew == null)
                    _amersfoort_RDNew = new ProjectedCoordinateReferenceSystem("EPSG::28992", "Amersfoort / RD New",
                                                                               "Replaces Amersfoort / RD Old.",
                                                                               null,
                                                                               "Large and medium scale topographic mapping and engineering survey.",
                                                                               Geographic2DCoordinateReferenceSystems.Amersfoort,
                                                                               CoordinateSystems.CartesianENM,
                                                                               AreasOfUse.NetherlandsOnshore,
                                                                               CoordinateProjectionFactory.RDNew((Geographic2DCoordinateReferenceSystems.Amersfoort.Datum as GeodeticDatum).Ellipsoid));
                return _amersfoort_RDNew;
            }
        }

        /// <summary>
        /// HD72 / EOV. 
        /// </summary>
        public static ProjectedCoordinateReferenceSystem HD72_EOV
        {
            get
            {
                if (_HD72_EOV == null)
                    _HD72_EOV = new ProjectedCoordinateReferenceSystem("EPSG::23700", "HD72 / EOV",
                                                                       null,
                                                                       null,
                                                                       "Large and medium scale topographic mapping and engineering survey.",
                                                                       Geographic2DCoordinateReferenceSystems.HD72,
                                                                       CoordinateSystems.CartesianENM,
                                                                       AreasOfUse.Hungary,
                                                                       CoordinateProjectionFactory.EOV((Geographic2DCoordinateReferenceSystems.HD72.Datum as GeodeticDatum).Ellipsoid));
                return _HD72_EOV;
            }
        }

        /// <summary>
        /// NAD27 / Texas South Central. 
        /// </summary>
        public static ProjectedCoordinateReferenceSystem NAD27_TexasSouthCentral
        {
            get
            {
                if (_NAD27_TexasSouthCentral == null)
                    _NAD27_TexasSouthCentral = new ProjectedCoordinateReferenceSystem("EPSG::32040", "NAD27 / Texas South Central",
                                                                                     null,
                                                                                     null,
                                                                                     "Large and medium scale topographic mapping and engineering survey.",
                                                                                     Geographic2DCoordinateReferenceSystems.NAD27,
                                                                                     CoordinateSystems.CartesianENM,
                                                                                     AreasOfUse.USATexasSPCS27SC,
                                                                                     CoordinateProjectionFactory.TexasCS27SouthCentral((Geographic2DCoordinateReferenceSystems.NAD27.Datum as GeodeticDatum).Ellipsoid));
                return _NAD27_TexasSouthCentral;
            }
        }

        /// <summary>
        /// NAD83 / Alabama West . 
        /// </summary>
        public static ProjectedCoordinateReferenceSystem NAD83_AlabamaWest
        {
            get
            {
                if (_NAD83_AlabamaWest == null)
                    _NAD83_AlabamaWest = new ProjectedCoordinateReferenceSystem("EPSG::26930", "NAD83 / Alabama West",
                                                                                null,
                                                                                null,
                                                                                "Large and medium scale topographic mapping and engineering survey.",
                                                                                Geographic2DCoordinateReferenceSystems.NAD83,
                                                                                CoordinateSystems.CartesianENM,
                                                                                AreasOfUse.USAAlabamaSPCSW,
                                                                                CoordinateProjectionFactory.SPCS83AlabamaWestZone((Geographic2DCoordinateReferenceSystems.NAD83.Datum as GeodeticDatum).Ellipsoid));
                return _NAD83_AlabamaWest;
            }
        }

        /// <summary>
        /// OSGB36 / British National Grid. 
        /// </summary>
        public static ProjectedCoordinateReferenceSystem OSGB36_BritishNationalGrid
        {
            get
            {
                if (_OSGB36_BritishNationalGrid == null)
                    _OSGB36_BritishNationalGrid = new ProjectedCoordinateReferenceSystem("EPSG::27700", "OSGB36 / British National Grid",
                                                                                         null,
                                                                                         null,
                                                                                         "Large and medium scale topographic mapping and engineering survey.",
                                                                                         Geographic2DCoordinateReferenceSystems.OSGB36,
                                                                                         CoordinateSystems.CartesianENM,
                                                                                         AreasOfUse.GreatBritainMan,
                                                                                         CoordinateProjectionFactory.BritishNationalGrid((Geographic2DCoordinateReferenceSystems.OSGB36.Datum as GeodeticDatum).Ellipsoid));
                return _OSGB36_BritishNationalGrid;
            }
        }
        /// <summary>
        /// WGS84 / World Mercator. 
        /// </summary>
        public static ProjectedCoordinateReferenceSystem WGS84_WorldMercator
        {
            get
            {
                if (_WGS84_WorldMercator == null)
                    _WGS84_WorldMercator = new ProjectedCoordinateReferenceSystem("EPSG::3395", "WGS84 / World Mercator",
                                                                                  "Euro-centric view of world excluding polar areas.",
                                                                                  null,
                                                                                  "Very small scale mapping.",
                                                                                  Geographic2DCoordinateReferenceSystems.WGS84,
                                                                                  CoordinateSystems.CartesianENM,
                                                                                  AreasOfUse.World_80STo84N,
                                                                                  CoordinateProjectionFactory.WorldMercator((Geographic2DCoordinateReferenceSystems.WGS84.Datum as GeodeticDatum).Ellipsoid));
                return _WGS84_WorldMercator;
            }
        }
        /// <summary>
        /// WGS84 / UPS North (E,N). 
        /// </summary>
        public static ProjectedCoordinateReferenceSystem WGS84_UPSN_EN
        {
            get
            {
                if (_WGS84_UPSN_EN == null)
                    _WGS84_UPSN_EN = new ProjectedCoordinateReferenceSystem("EPSG::5041", "WGS84 / UPS North (E,N)", 
                                                                            null,
                                                                            null,
                                                                            "Military mapping by NATO.",
                                                                            Geographic2DCoordinateReferenceSystems.WGS84,
                                                                            CoordinateSystems.CartesianENM, 
                                                                            AreasOfUse.World_60NTo90N,
                                                                            CoordinateProjectionFactory.UniversalPolarStereographicNorth((Geographic2DCoordinateReferenceSystems.WGS84.Datum as GeodeticDatum).Ellipsoid));
                return _WGS84_UPSN_EN;
            }
        }
        /// <summary>
        /// WGS84 / UPS South (E,N). 
        /// </summary>
        public static ProjectedCoordinateReferenceSystem WGS84_UPSS_EN
        {
            get
            {
                if (_WGS84_UPSS_EN == null)
                    _WGS84_UPSS_EN = new ProjectedCoordinateReferenceSystem("EPSG::5042", "WGS84 / UPS South (E,N)",
                                                                            null,
                                                                            null,
                                                                            "Military mapping by NATO.",
                                                                            Geographic2DCoordinateReferenceSystems.WGS84,
                                                                            CoordinateSystems.CartesianENM,
                                                                            AreasOfUse.World_60STo90S,
                                                                            CoordinateProjectionFactory.UniversalPolarStereographicSouth((Geographic2DCoordinateReferenceSystems.WGS84.Datum as GeodeticDatum).Ellipsoid));
                return _WGS84_UPSS_EN;
            }
        }

        #endregion
    }
}
