/// <copyright file="CompoundTemporalReferenceSystems.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="CompoundTemporalReferenceSystem" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(CompoundTemporalReferenceSystem))]
    public static class CompoundTemporalReferenceSystems
    {
        #region Query fields

        private static CompoundTemporalReferenceSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="CompoundTemporalReferenceSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="CompoundTemporalReferenceSystem" /> instances within the collection.</value>
        public static IList<CompoundTemporalReferenceSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(CompoundTemporalReferenceSystems).GetProperties().
                                                                    Where(property => property.Name != "All").
                                                                    Select(property => property.GetValue(null, null) as CompoundTemporalReferenceSystem).
                                                                    ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="CompoundTemporalReferenceSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="CompoundTemporalReferenceSystem" /> instances that match the specified identifier.</returns>
        public static IList<CompoundTemporalReferenceSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="CompoundTemporalReferenceSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="CompoundTemporalReferenceSystem" /> instances that match the specified name.</returns>
        public static IList<CompoundTemporalReferenceSystem> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static CompoundTemporalReferenceSystem _gregorianCalendarUTCClock;

        #endregion

        #region Public static properties

        /// <summary>
        /// Gregorian calendar + UTC clock.
        /// </summary>
        public static CompoundTemporalReferenceSystem GregorianCalendarUTCClock 
        { 
            get 
            { 
                return _gregorianCalendarUTCClock ?? (_gregorianCalendarUTCClock = 
                    new CompoundTemporalReferenceSystem("AEGIS::813501", "Gregorian calendar + UTC clock", Calendars.GregorianCalendar, Clocks.UTC)); 
            } 
        }

        #endregion
    }
}
