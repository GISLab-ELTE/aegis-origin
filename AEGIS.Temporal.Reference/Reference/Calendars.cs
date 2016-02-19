/// <copyright file="Calendars.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of known <see cref="Calendar" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(Calendar))]
    public static class Calendars
    {
        #region Query fields

        private static Calendar[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="Calendar" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="Calendar" /> instances within the collection.</value>
        public static IList<Calendar> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(Calendars).GetProperties().
                                             Where(property => property.Name != "All").
                                             Select(property => property.GetValue(null, null) as Calendar).
                                             ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="Calendar" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="Calendar" /> instances that match the specified identifier.</returns>
        public static IList<Calendar> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="Calendar" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="Calendar" /> instances that match the specified name.</returns>
        public static IList<Calendar> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static Calendar _gregorianCalendar;
        private static Calendar _julianCalendar;
        private static Calendar _prolepticJulianCalendar;

        #endregion

        #region Public static properties

        /// <summary>
        /// Gregorian calendar.
        /// </summary>
        public static Calendar GregorianCalendar
        {
            get
            {
                return _gregorianCalendar ?? (_gregorianCalendar = new GregorianCalendar("AEGIS::813001", "Gregorian calendar"));
            }
        }

        /// <summary>
        /// Julian calendar.
        /// </summary>
        public static Calendar JulianCalendar
        {
            get
            {
                return _julianCalendar ?? (_julianCalendar = new JulianCalendar("AEGIS::813002", "Julian calendar", "The Julian calendar was a reform of the Roman calendar introduced by Julius Caesar in 46 BC (708 AUC).", null, "Civil calendar."));
            }
        }

        /// <summary>
        /// Proleptic Julian calendar.
        /// </summary>
        public static Calendar ProlepticJulianCalendar
        {
            get
            {
                return _prolepticJulianCalendar ?? (_prolepticJulianCalendar = new ProlepticJulianCalendar("AEGIS::813003", "Proleptic Julian calendar", "This calendar follows the leap year rule strictly, even for dates before 8 CE, where leap years were actually irregular.", null, "Civil calendar."));
            }
        }

        #endregion
    }
}
