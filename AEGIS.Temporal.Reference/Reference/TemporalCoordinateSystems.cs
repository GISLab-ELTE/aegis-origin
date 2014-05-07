/// <copyright file="TemporalCoordinateSystems.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="TemporalCoordinateSystem" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(TemporalCoordinateSystem))]
    public static class TemporalCoordinateSystems
    {
        #region Query fields

        private static TemporalCoordinateSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="TemporalCoordinateSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="TemporalCoordinateSystem" /> instances within the collection.</value>
        public static IList<TemporalCoordinateSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(TemporalCoordinateSystems).GetProperties().
                                                             Where(property => property.Name != "All").
                                                             Select(property => property.GetValue(null, null) as TemporalCoordinateSystem).
                                                             ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="TemporalCoordinateSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="TemporalCoordinateSystem" /> instances that match the specified identifier.</returns>
        public static IList<TemporalCoordinateSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="TemporalCoordinateSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="TemporalCoordinateSystem" /> instances that match the specified name.</returns>
        public static IList<TemporalCoordinateSystem> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static TemporalCoordinateSystem _julianDateSystem;
        private static TemporalCoordinateSystem _modifiedJulianDateSystem;

        #endregion

        #region Public static properties

        /// <summary>
        /// Julian Date System.
        /// </summary>
        public static TemporalCoordinateSystem JulianDateSystem 
        { 
            get 
            { 
                return _julianDateSystem ?? (_julianDateSystem = 
                    new JulianDateSystem("AEGIS::816101", "Julian Date System")); 
            } 
        }

        /// <summary>
        /// Modified Julian Date System.
        /// </summary>
        public static TemporalCoordinateSystem ModifiedJulianDateSystem 
        { 
            get 
            { 
                return _modifiedJulianDateSystem ?? (_modifiedJulianDateSystem = 
                    new JulianDateSystem("AEGIS::816102", "Modified Julian Date System")); 
            } 
        }

        #endregion
    }
}
