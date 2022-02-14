/// <copyright file="UnitsOfMeasurement.cs" company="Eötvös Loránd University (ELTE)">
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Temporal
{
    /// <summary>
    /// Represents a collection of known <see cref="UnitOfMeasurement" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(UnitOfMeasurement))]
    public static class UnitsOfMeasurement
    {
        #region Query fields

        private static UnitOfMeasurement[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="UnitOfMeasurement" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="UnitOfMeasurement" /> instances within the collection.</value>
        public static IList<UnitOfMeasurement> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(UnitsOfMeasurement).GetProperties().
                                                      Where(property => property.Name != "All").
                                                      Select(property => property.GetValue(null, null) as UnitOfMeasurement).
                                                      ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="UnitOfMeasurement" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="UnitOfMeasurement" /> instances that match the specified identifier.</returns>
        public static IList<UnitOfMeasurement> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }
        /// <summary>
        /// Returns all <see cref="UnitOfMeasurement" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="UnitOfMeasurement" /> instances that match the specified name.</returns>
        public static IList<UnitOfMeasurement> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static UnitOfMeasurement _millisecond;
        private static UnitOfMeasurement _second;
        private static UnitOfMeasurement _minute;
        private static UnitOfMeasurement _hour;
        private static UnitOfMeasurement _day;

        #endregion

        #region Public static properties

        /// <summary>
        /// Millisecond.
        /// </summary>
        public static UnitOfMeasurement Millisecond
        {
            get { return _millisecond ?? (_millisecond = new UnitOfMeasurement("AEGIS::800102", "Millisecond", "ms", 0.001, UnitQuantityType.Time)); }
        }

        /// <summary>
        /// Second.
        /// </summary>
        public static UnitOfMeasurement Second
        {
            get { return _second ?? (_second = new UnitOfMeasurement("AEGIS::800101", "Second", "s", 1, UnitQuantityType.Time)); }
        }

        /// <summary>
        /// Minute.
        /// </summary>
        public static UnitOfMeasurement Minute
        {
            get { return _minute ?? (_minute = new UnitOfMeasurement("AEGIS::8001013", "Minute", "m", 60, UnitQuantityType.Time)); }
        }

        /// <summary>
        /// Hour.
        /// </summary>
        public static UnitOfMeasurement Hour
        {
            get { return _hour ?? (_hour = new UnitOfMeasurement("AEGIS::800104", "Hour", "h", 3600, UnitQuantityType.Time)); }
        }

        /// <summary>
        /// Day.
        /// </summary>
        public static UnitOfMeasurement Day 
        {
            get { return _day ?? (_day = new UnitOfMeasurement("AEGIS::800105", "Day", "", 86400, UnitQuantityType.Time)); }
        }

        #endregion
    }
}
