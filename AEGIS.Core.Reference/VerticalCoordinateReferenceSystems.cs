/// <copyright file="VerticalCoordinateReferenceSystems.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="VerticalCoordinateReferenceSystem" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(VerticalCoordinateReferenceSystem))]
    public static class VerticalCoordinateReferenceSystems
    {
        #region Query fields

        private static VerticalCoordinateReferenceSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="VerticalCoordinateReferenceSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="VerticalCoordinateReferenceSystem" /> instances within the collection.</value>
        public static IList<VerticalCoordinateReferenceSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(VerticalCoordinateReferenceSystems).GetProperties().
                                                                      Where(property => property.Name != "All").
                                                                      Select(property => property.GetValue(null, null) as VerticalCoordinateReferenceSystem).
                                                                      ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns <see cref="VerticalCoordinateReferenceSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="VerticalCoordinateReferenceSystem" /> instances that match the specified identifier.</returns>
        public static IList<VerticalCoordinateReferenceSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns <see cref="VerticalCoordinateReferenceSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="VerticalCoordinateReferenceSystem" /> instances that match the specified name.</returns>
        public static IList<VerticalCoordinateReferenceSystem> FromName(String name)
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

        private static VerticalCoordinateReferenceSystem _NAVD88;
        private static VerticalCoordinateReferenceSystem _EGM2008;
        private static VerticalCoordinateReferenceSystem _EVRF2000;
        private static VerticalCoordinateReferenceSystem _EGM96;
        private static VerticalCoordinateReferenceSystem _EOMA80;
        private static VerticalCoordinateReferenceSystem _EGM84;

        #endregion

        #region Public static properties

        /// <summary>
        /// EGM2008 geoid height.
        /// </summary>
        public static VerticalCoordinateReferenceSystem EGM2008
        {
            get
            {
                return _EGM2008 ?? (_EGM2008 = 
                    new VerticalCoordinateReferenceSystem("EPSG::3855", "EGM2008 geoid height", 
                                                          CoordinateSystems.VerticalHeightM, 
                                                          VerticalDatums.EGM2008, 
                                                          AreasOfUse.World));
            }
        }

        /// <summary>
        /// EGM84 geoid height.
        /// </summary>
        public static VerticalCoordinateReferenceSystem EGM84
        {
            get
            {
                return _EGM84 ?? (_EGM84 =
                    new VerticalCoordinateReferenceSystem("EPSG::5798", "EGM84 geoid height",
                                                          CoordinateSystems.VerticalHeightM,
                                                          VerticalDatums.EGM84,
                                                          AreasOfUse.World));
            }
        }     

        /// <summary>
        /// EGM96 geoid height.
        /// </summary>
        public static VerticalCoordinateReferenceSystem EGM96
        {
            get
            {
                return _EGM96 ?? (_EGM96 = 
                    new VerticalCoordinateReferenceSystem("EPSG::5773", "EGM96 geoid height", 
                                                          CoordinateSystems.VerticalHeightM, 
                                                          VerticalDatums.EGM96, 
                                                          AreasOfUse.World));
            }
        }

        /// <summary>
        /// EOMA 1980 height.
        /// </summary>
        public static VerticalCoordinateReferenceSystem EOMA80
        {
            get
            {
                return _EOMA80 ?? (_EOMA80 = 
                    new VerticalCoordinateReferenceSystem("EPSG::5787", "EOMA 1980 height", 
                                                          CoordinateSystems.VerticalHeightM, 
                                                          VerticalDatums.Baltic80, 
                                                          AreasOfUse.Hungary));
            }
        }

        /// <summary>
        /// EVRF2000 height.
        /// </summary>
        public static VerticalCoordinateReferenceSystem EVRF2000
        {
            get
            {
                return _EVRF2000 ?? (_EVRF2000 =
                    new VerticalCoordinateReferenceSystem("EPSG::5730", "EVRF2000 height",
                                                          CoordinateSystems.VerticalHeightM,
                                                          VerticalDatums.EVRF2000,
                                                          AreasOfUse.EuropeEVRF2000));
            }
        }

        /// <summary>
        /// NAVD88 height.
        /// </summary>
        public static VerticalCoordinateReferenceSystem NAVD88
        {
            get
            {
                return _NAVD88 ?? (_NAVD88 =
                    new VerticalCoordinateReferenceSystem("EPSG::5703", "NAVD88 height",
                                                          CoordinateSystems.VerticalHeightM,
                                                          VerticalDatums.NAVD88,
                                                          AreasOfUse.USACONUSEastAlaskaOnshore));
            }
        }

        #endregion
    }
}
