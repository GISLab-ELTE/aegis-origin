/// <copyright file="ClockTime.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Temporal.Reference;

namespace ELTE.AEGIS.Temporal.Positioning
{
    /// <summary>
    /// Represents a position expressed as date and time.
    /// </summary>
    public class DateAndTime : TemporalPosition
    {
        #region Protected fields

        private Instant _timeInstant;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the date component of this instance.
        /// </summary>
        /// <value>The calendar date component of this instance in the associated calendar.</value>
        public CalendarDate Date { get { return new CalendarDate(_instant, (ReferenceSystem as CompoundTemporalReferenceSystem).Calendar, null); } }

        /// <summary>
        /// Gets the time component of this instance.
        /// </summary>
        /// <value>The clock time component of this instance in the associated clock.</value>
        public ClockTime TimeOfDay { get { return new ClockTime(_timeInstant, (ReferenceSystem as CompoundTemporalReferenceSystem).Clock, null); } }

        /// <summary>
        /// Gets the calendar era of the date.
        /// </summary>
        /// <value>The era according to the associated calendar.</value>
        public CalendarEra Era { get { return (ReferenceSystem as CompoundTemporalReferenceSystem).Calendar.GetEra(_instant); } }

        /// <summary>
        /// Gets the year component of the date.
        /// </summary>
        /// <value>The year according to the associated calendar.</value>
        public Int64 Year { get { return (ReferenceSystem as CompoundTemporalReferenceSystem).Calendar.GetYear(_instant); } }

        /// <summary>
        /// Gets the month component of the date.
        /// </summary>
        /// <value>The month according to the associated calendar.</value>
        public Int64 Month { get { return (ReferenceSystem as CompoundTemporalReferenceSystem).Calendar.GetMonth(_instant); } }

        /// <summary>
        /// Gets the day component of the date.
        /// </summary>
        /// <value>The day according to the associated calendar.</value>
        public Int64 Day { get { return (ReferenceSystem as CompoundTemporalReferenceSystem).Calendar.GetDay(_instant); } }

        /// <summary>
        /// Gets the hour component of the time.
        /// </summary>
        /// <value>The hour according to the associated clock.</value>
        public Int32 Hour { get { return (ReferenceSystem as CompoundTemporalReferenceSystem).Clock.GetHour(_timeInstant); } }

        /// <summary>
        /// Gets the minute component of the time.
        /// </summary>
        /// <value>The minute according to the associated clock.</value>
        public Int32 Minute { get { return (ReferenceSystem as CompoundTemporalReferenceSystem).Clock.GetMinute(_timeInstant); } }

        /// <summary>
        /// Gets the second component of the time.
        /// </summary>
        /// <value>The second according to the associated clock.</value>
        public Int32 Second { get { return (ReferenceSystem as CompoundTemporalReferenceSystem).Clock.GetSecond(_timeInstant); } }

        /// <summary>
        /// Gets the millisecond component of the time.
        /// </summary>
        /// <value>The millisecond according to the associated clock.</value>
        public Int32 Millisecond { get { return (ReferenceSystem as CompoundTemporalReferenceSystem).Clock.GetMilliseconds(_timeInstant); } }
        
        #endregion

        #region ITemporalPosition properties

        /// <summary>
        /// Gets the instant associated with the temporal position.
        /// </summary>
        /// <value>The instant associated with the temporal position.</value>
        public override Instant Instant
        {
            get { return new Instant(_instant.Ticks * ((ReferenceSystem as CompoundTemporalReferenceSystem).Clock.MillisecondsPerDay / 100) + _timeInstant.Ticks / 100); }
            set
            {
                Int64 datePart = value.Ticks / ((ReferenceSystem as CompoundTemporalReferenceSystem).Clock.MillisecondsPerDay / 100);
                Int64 clockPart = (value.Ticks - datePart) * 100;

                if (_instant.Ticks != datePart || _timeInstant.Ticks != clockPart)
                {
                    _instant = new Instant(datePart);
                    _timeInstant = new Instant(clockPart);
                    OnPositionChanged();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAndTime" /> class.
        /// </summary>
        /// <param name="instant">The date and time.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">referenceSystem;The reference system is null.</exception>
        public DateAndTime(Instant instant, CompoundTemporalReferenceSystem referenceSystem, IMetadataCollection metadata)
            : base(referenceSystem, metadata)
        {
            Int64 datePart = instant.Ticks / ((ReferenceSystem as CompoundTemporalReferenceSystem).Clock.MillisecondsPerDay / 100);
            Int64 clockPart = (instant.Ticks - datePart) * 100;
            _instant = new Instant(datePart);
            _timeInstant = new Instant(clockPart);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateAndTime" /> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">referenceSystem;The reference system is null.</exception>
        public DateAndTime(Instant date, Instant time, CompoundTemporalReferenceSystem referenceSystem, IMetadataCollection metadata)
            : base(referenceSystem, metadata)
        {
            _instant = date;
            _timeInstant = time;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts a calendar date to a date/time expression.
        /// </summary>
        /// <param name="date">The calendar date.</param>
        /// <returns>The equivalent date/time of <paramref name="date" />.</returns>
        public static implicit operator DateAndTime(CalendarDate date)
        {
            CompoundTemporalReferenceSystem referenceSystem = null;
            if (date.ReferenceSystem.Equals(Calendars.GregorianCalendar))
                referenceSystem = CompoundTemporalReferenceSystems.GregorianCalendarUTCClock;

            return new DateAndTime(date.Instant, Instant.Zero, referenceSystem, null);
        }

        /// <summary>
        /// Converts a clock time to a date/time expression.
        /// </summary>
        /// <param name="time">The clock time.</param>
        /// <returns>The equivalent date/time of <paramref name="time" />.</returns>
        public static implicit operator DateAndTime(ClockTime time)
        {
            CompoundTemporalReferenceSystem referenceSystem = null;
            if (time.ReferenceSystem.Equals(Clocks.UTC))
                referenceSystem = CompoundTemporalReferenceSystems.GregorianCalendarUTCClock;

            return new DateAndTime(Instant.Zero, time.Instant, referenceSystem, null);
        }

        #endregion
    }
}
