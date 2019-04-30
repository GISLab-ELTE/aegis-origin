/// <copyright file="CompoundReferenceSystems.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of known <see cref="CompoundReferenceSystem" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(CompoundReferenceSystem))]
    public static class CompoundReferenceSystems
    {
        #region Query fields

        private static CompoundReferenceSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="CompoundReferenceSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="CompoundReferenceSystem" /> instances within the collection.</value>
        public static IList<CompoundReferenceSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(CompoundReferenceSystems).GetProperties().
                                                                      Where(property => property.Name != "All").
                                                                      Select(property => property.GetValue(null, null) as CompoundReferenceSystem).
                                                                      ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="CompoundReferenceSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="CompoundReferenceSystem" /> instances that match the specified identifier.</returns>
        public static IList<CompoundReferenceSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="CompoundReferenceSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="CompoundReferenceSystem" /> instances that match the specified name.</returns>
        public static IList<CompoundReferenceSystem> FromName(String name)
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

        private static CompoundReferenceSystem _NAD83NSR2007wNAV88;

        #endregion

        #region Public static properties

        /// <summary>
        /// NAD83(NSRS2007) + NAVD88 height.
        /// </summary>
        public static CompoundReferenceSystem NAD83NSR2007wNAV88
        {
            get
            {
                if (_NAD83NSR2007wNAV88 == null)
                    _NAD83NSR2007wNAV88 = new CompoundReferenceSystem("EPSG::5500", "NAD83(NSRS2007) + NAVD88 height", 
                                                                      AreasOfUse.USACONUSOnshore,
                                                                      Geographic3DCoordinateReferenceSystems.NAD83NSR2007,
                                                                      VerticalCoordinateReferenceSystems.NAVD88);
                return _NAD83NSR2007wNAV88;
            }
        }

        #endregion
    }
}
