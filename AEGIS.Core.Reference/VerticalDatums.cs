/// <copyright file="VerticalDatums.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
    /// Represents a collection of the known <see cref="VerticalDatum" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(VerticalDatum))]
    public static class VerticalDatums
    {
        #region Query fields

        private static VerticalDatum[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="VerticalDatum" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="VerticalDatum" /> instances within the collection.</value>
        public static IList<VerticalDatum> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(VerticalDatums).GetProperties().
                                                  Where(property => property.Name != "All").
                                                  Select(property => property.GetValue(null, null) as VerticalDatum).
                                                  ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="VerticalDatum" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="VerticalDatum" /> instances that match the specified identifier.</returns>
        public static IList<VerticalDatum> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="VerticalDatum" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="VerticalDatum" /> instances that match the specified name.</returns>
        public static IList<VerticalDatum> FromName(String name)
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

        private static VerticalDatum _baltic80;
        private static VerticalDatum _EGM2008;
        private static VerticalDatum _EGM84;
        private static VerticalDatum _EGM96;
        private static VerticalDatum _EVRF2000;
        private static VerticalDatum _NAVD88;

        #endregion

        #region Public static properties

        /// <summary>
        /// Baltic 1980.
        /// </summary>
        public static VerticalDatum Baltic80
        {
            get
            {
                return _baltic80 ?? (_baltic80 = 
                    new VerticalDatum("EPSG::5187", "Baltic 1980", 
                                      String.Empty, "1988", 
                                      AreasOfUse.Hungary));
            }
        }

        /// <summary>
        /// Earth Gravity Model 2008 (EGM2008).
        /// </summary>
        public static VerticalDatum EGM2008
        {
            get
            {
                return _EGM2008 ?? (_EGM2008 = 
                    new VerticalDatum("EPSG::1027", "Earth Gravity Model 2008 (EGM2008)", 
                                      "WGS84 ellipsoid.", "2008", 
                                      AreasOfUse.World));
            }
        }

        /// <summary>
        /// Earth Gravity Model 1984 (EGM84).
        /// </summary>
        public static VerticalDatum EGM84
        {
            get
            {
                return _EGM84 ?? (_EGM84 = 
                    new VerticalDatum("EPSG::5203", "Earth Gravity Model 1984 (EGM84)",
                                      "WGS84 ellipsoid.", "1984",
                                      AreasOfUse.World));
            }
        }

        /// <summary>
        /// Earth Gravity Model 1996 (EGM96).
        /// </summary>
        public static VerticalDatum EGM96
        {
            get
            {
                return _EGM96 ?? (_EGM96 =
                    new VerticalDatum("EPSG::5171", "Earth Gravity Model 1996 (EGM96)",
                                      "WGS84 ellipsoid.", "1996",
                                      AreasOfUse.World));
            }
        }

        /// <summary>
        /// European Vertical Reference Frame 2000 (EVRF2000).
        /// </summary>
        public static VerticalDatum EVRF2000
        {
            get
            {
                return _EVRF2000 ?? (_EVRF2000 = 
                    new VerticalDatum("EPSG::5129", "European Vertical Reference Frame 2000 (EVRF2000)",
                                      "Height at Normaal Amsterdams Peil (NAP) is zero, defined through height at UELN bench mark 13600 (52°22'53\"N 4°54'34\"E) of 0.71599m. Datum at NAP is mean high tide in 1684.", "2000",
                                      AreasOfUse.EuropeEVRF2000));
            }
        }

        /// <summary>
        /// North American Vertical Datum 1988.
        /// </summary>
        public static VerticalDatum NAVD88
        {
            get
            {
                return _NAVD88 ?? (_NAVD88 = new 
                    VerticalDatum("EPSG::5103", "North American Vertical Datum 1988 (NAVD88)", 
                                  "Father's Point, Rimouski, Quebec.", "1988", 
                                  AreasOfUse.USACONUSEastAlaskaOnshore));
            }
        }

        #endregion
    }
}
