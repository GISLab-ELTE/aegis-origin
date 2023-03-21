// <copyright file="CalendarEra.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a calendar era.
    /// </summary>
    public class CalendarEra : IdentifiedObject
    {
        #region Public properties

        /// <summary>
        /// Gets the calendar associated with the era.
        /// </summary>
        /// <value>The calendar associated with the era.</value>
        public Calendar Calendar { get; private set; }

        /// <summary>
        /// Gets the reference event.
        /// </summary>
        /// <value>The mythical or historic event which fixes the position of the base scale of the calendar era.</value>
        public String ReferenceEvent { get; private set; }

        /// <summary>
        /// Gets the reference date.
        /// </summary>
        /// <value>The date of the reference event expressed as a date in the given calendar.</value>
        public CalendarDate ReferenceDate { get; private set; }

        /// <summary>
        /// Gets the reference Julian Date.
        /// </summary>
        /// <value>The date of the reference event expressed as a Julian Date.</value>
        public JulianDate JulianReferenceDate { get { return Calendar.TranformToJulianDate(ReferenceDate); } }
        
        /// <summary>
        /// Gets the beginning of the period for which the calendar era was used.
        /// </summary
        /// <value>The beginning date of the period for which the calendar era was used expressed as a date in the given calendar.</value>
        public CalendarDate BeginOfUse { get; private set; }
        
        /// <summary>
        /// Gets the end of the period for which the calendar era was used.
        /// </summary
        /// <value>The ending date of the period for which the calendar era was used expressed as a date in the given calendar.</value>
        public CalendarDate EndOfUse { get; private set; }
        
        /// <summary>
        /// Gets the period for which the calendar era was used.
        /// </summary>
        /// <value>The period for which the calendar era was used.</value>
        public PeriodDuration PeriodOfUse { get { return new PeriodDuration(EndOfUse.Instant - BeginOfUse.Instant, Duration.Zero, Calendar, Calendar.TimeBasis, null); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEra" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="calendar">The calendar.</param>
        /// <param name="referenceEvent">The reference event.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <param name="beginOfUse">The beginning date of usage.</param>
        /// <param name="endOfUse">The ending date of usage.</param>
        /// <exception cref="System.ArgumentNullException">The calendar is null.</exception>
        public CalendarEra(String identifier, String name, Calendar calendar, String referenceEvent, Instant referenceDate, Instant beginOfUse, Instant endOfUse)
            : this(identifier, name, null, null, calendar, referenceEvent, referenceDate, beginOfUse, endOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEra" />class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="calendar">The calendar.</param>
        /// <param name="referenceEvent">The reference event.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <param name="beginOfUse">The beginning date of usage.</param>
        /// <param name="endOfUse">The ending date of usage.</param>
        /// <exception cref="System.ArgumentNullException">The calendar is null.</exception>
        public CalendarEra(String identifier, String name, String remarks, String[] aliases, Calendar calendar, String referenceEvent, Instant referenceDate, Instant beginOfUse, Instant endOfUse)
            : base(identifier, name, remarks, aliases)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar", "The calendar is null.");

            Calendar = calendar;
            ReferenceEvent = referenceEvent;
            ReferenceDate = new CalendarDate(referenceDate, calendar, null);
            BeginOfUse = new CalendarDate(beginOfUse, calendar, null);
            EndOfUse = new CalendarDate(endOfUse, calendar, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEra" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="calendar">The calendar.</param>
        /// <param name="referenceEvent">The reference event.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <param name="beginOfUse">The beginning date of usage.</param>
        /// <param name="endOfUse">The ending date of usage.</param>
        /// <exception cref="System.ArgumentNullException">The calendar is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The reference date is not in the specified calendar.
        /// or
        /// The beginning date of usage is not in the specified calendar.
        /// or
        /// The ending date of usage is not in the specified calendar.
        /// </exception>
        protected CalendarEra(String identifier, String name, Calendar calendar, String referenceEvent, CalendarDate referenceDate, CalendarDate beginOfUse, CalendarDate endOfUse) :
            this(identifier, name, null, null, calendar, referenceEvent, referenceDate, beginOfUse, endOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEra" />class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="calendar">The calendar.</param>
        /// <param name="referenceEvent">The reference event.</param>
        /// <param name="referenceDate">The reference date.</param>
        /// <param name="beginOfUse">The beginning date of usage.</param>
        /// <param name="endOfUse">The ending date of usage.</param>
        /// <exception cref="System.ArgumentNullException">The calendar is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The reference date is not in the specified calendar.
        /// or
        /// The beginning date of usage is not in the specified calendar.
        /// or
        /// The ending date of usage is not in the specified calendar.
        /// </exception>
        protected CalendarEra(String identifier, String name, String remarks, String[] aliases, Calendar calendar, String referenceEvent, CalendarDate referenceDate, CalendarDate beginOfUse, CalendarDate endOfUse)
            : base(identifier, name, remarks, aliases)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar", "The calendar is null.");
            if (referenceDate != null && !calendar.Equals(referenceDate.ReferenceSystem))
                throw new ArgumentException("The reference date is not in the specified calendar.", "referenceDate");
            if (beginOfUse != null && !calendar.Equals(beginOfUse.ReferenceSystem))
                throw new ArgumentException("The beginning date of usage is not in the specified calendar.", "beginOfUse");
            if (endOfUse != null && !calendar.Equals(endOfUse.ReferenceSystem))
                throw new ArgumentException("The ending date of usage is not in the specified calendar.", "endOfUse");

            Calendar = calendar;
            ReferenceEvent = referenceEvent;
            ReferenceDate = referenceDate;
            BeginOfUse = beginOfUse;
            EndOfUse = endOfUse;
        }

        #endregion
    }
}
