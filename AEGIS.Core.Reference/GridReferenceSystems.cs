/// <copyright file="GridReferenceSystems.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Reference.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="GridReferenceSystem" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(GridReferenceSystem))]
    public static class GridReferenceSystems
    {
        #region Query fields

        private static GridReferenceSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="GridReferenceSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="GridReferenceSystem" /> instances within the collection.</value>
        public static IList<GridReferenceSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(GridReferenceSystems).GetProperties().
                                                        Where(property => property.Name != "All").
                                                        Select(property => property.GetValue(null, null) as GridReferenceSystem).
                                                        ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GridReferenceSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="GridReferenceSystem" /> instances that match the specified identifier.</returns>
        public static IList<GridReferenceSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="GridReferenceSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="GridReferenceSystem" /> instances that match the specified name.</returns>
        public static IList<GridReferenceSystem> FromName(String name)
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

        private static GridReferenceSystem _geographicGrid;
        private static GridReferenceSystem _militaryGrid;

        #endregion

        #region Public static properties

        /// <summary>
        /// Geographic Grid Reference System (Georef).
        /// </summary>
        public static GridReferenceSystem GeographicGrid
        {
            get
            {
                return _geographicGrid ?? (_geographicGrid =
                    new GridReferenceSystem("AEGIS::745201", "Geographic Grid Reference System",
                                            null,
                                            new String[] { "Georef" },
                                            "Primarily used for air navigation, particularly in military or inter-service applications.",
                                            CoordinateSystems.EllipsoidalLatLonD,
                                            GeodeticDatums.WGS84,
                                            AreasOfUse.World, 
                                            GridProjectionFactory.GeographicGridProjection()));
            }
        }

        /// <summary>
        /// Military Grid Reference System (MGRS).
        /// </summary>
        public static GridReferenceSystem MilitaryGrid
        {
            get
            {
                return _militaryGrid ?? (_militaryGrid =
                    new GridReferenceSystem("AEGIS::745202", "Military Grid Reference System",
                                            null,
                                            new String[] { "MGRS" },
                                            "Used by NATO militaries for locating points on the earth.",
                                            CoordinateSystems.EllipsoidalLatLonD,
                                            GeodeticDatums.WGS84,
                                            AreasOfUse.World,
                                            GridProjectionFactory.MilitaryGridProjection()));
            }
        }

        #endregion
    }
}
