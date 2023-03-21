// <copyright file="Clocks.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="Clock" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(Clock))]
    public static class Clocks
    {
        #region Query fields

        private static Clock[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="Clock" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="Clock" /> instances within the collection.</value>
        public static IList<Clock> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(Clocks).GetProperties().
                                          Where(property => property.Name != "All").
                                          Select(property => property.GetValue(null, null) as Clock).
                                          ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="Clock" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="Clock" /> instances that match the specified identifier.</returns>
        public static IList<Clock> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="Clock" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="Clock" /> instances that match the specified name.</returns>
        public static IList<Clock> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static Clock _UTC;

        #endregion

        #region Public static properties

        /// <summary>
        /// UTC.
        /// </summary>
        public static Clock UTC { get { return _UTC ?? (_UTC = new Clock("AEGIS::814001", "UTC", Calendars.GregorianCalendar, Duration.Zero)); } }

        #endregion
    }
}
