// <copyright file="CompoundTemporalReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using ELTE.AEGIS.Temporal.Positioning;

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a compound temporal reference system of a calendar and a clock.
    /// </summary>
    public class CompoundTemporalReferenceSystem : TemporalReferenceSystem
    {
        #region Private fields

        private Calendar _calendar;
        private Clock _clock;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the calendar.
        /// </summary>
        /// <value>The calendar component of the reference system.</value>
        public Calendar Calendar { get { return _calendar; } }

        /// <summary>
        /// Gets the clock.
        /// </summary>
        /// <value>The clock component of the reference system.</value>
        public Clock Clock { get { return _clock; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundTemporalReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="calendar">The calendar.</param>
        /// <param name="clock">The clock.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The calendar is null.
        /// or
        /// The clock is null.
        /// </exception>
        public CompoundTemporalReferenceSystem(String identifier, String name, Calendar calendar, Clock clock)
            : this(identifier, name, null, null, null, calendar, clock)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar", "The calendar is null.");
            if (clock == null)
                throw new ArgumentNullException("clock", "The clock is null.");

            _calendar = calendar;
            _clock = clock;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundTemporalReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="calendar">The calendar.</param>
        /// <param name="clock">The clock.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The calendar is null.
        /// or
        /// The clock is null.
        /// </exception>
        public CompoundTemporalReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, Calendar calendar, Clock clock)
            : base(identifier, name, remarks, aliases, scope) 
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar", "The calendar is null.");
            if (clock == null)
                throw new ArgumentNullException("clock", "The clock is null.");

            _calendar = calendar;
            _clock = clock;
        }

        #endregion
    }
}
