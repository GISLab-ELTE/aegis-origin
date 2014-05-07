/// <copyright file="PeriodDuration.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Temporal.Reference;

namespace ELTE.AEGIS.Temporal.Positioning
{
    /// <summary>
    /// Represents a duration in time with respect to a specific calendar and clock.
    /// </summary>
    public class PeriodDuration
    {
        #region Protected fields

        protected readonly Calendar _calendar;
        protected readonly Clock _clock;
        protected Duration _date;
        protected Duration _time;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the years component of the duration.
        /// </summary>
        /// <value>The years component of this instance. The return value can be positive or negative.</value>
        public Int64 Years { get { return _calendar.GetYears(_date); } }

        /// <summary>
        /// Gets the months component of the duration.
        /// </summary>
        /// <value>The months component of this instance.</value>
        public Int64 Months { get { return _calendar.GetMonths(_date); } }

        /// <summary>
        /// Gets the days component of the duration.
        /// </summary>
        /// <value>The days component of this instance.</value>
        public Int64 Days { get { return _calendar.GetDays(_date); } }

        /// <summary>
        /// Gets the hours component of the duration.
        /// </summary>
        /// <value>The hours component of this instance.</value>
        public Int32 Hours { get { return _clock.GetHours(_time); } }

        /// <summary>
        /// Gets the minutes component of the duration.
        /// </summary>
        /// <value>The minutes component of this instance.</value>
        public Int32 Minutes { get { return _clock.GetMinutes(_time); } }

        /// <summary>
        /// Gets the seconds component of the duration.
        /// </summary>
        /// <value>The seconds component of this instance.</value>
        public Int32 Seconds { get { return _clock.GetSeconds(_time); } }

        /// <summary>
        /// Gets the milliseconds component of the duration.
        /// </summary>
        /// <value>The milliseconds component of this instance.</value>
        public Int32 Milliseconds { get { return _clock.GetMilliseconds(_time); } }

        /// <summary>
        /// Gets the value of the duration expressed in the number of months.
        /// </summary>
        /// <value>The total number of months represented by this instance.</value>
        public Int64 TotalMonths { get { return _calendar.GetTotalMonths(_date); } }

        /// <summary>
        /// Gets the value of the duration expressed in the number of days.
        /// </summary>
        /// <value>The total number of days represented by this instance.</value>
        public Int64 TotalDays { get { return _calendar.GetTotalDays(_date); } }

        /// <summary>
        /// Gets the value of the duration expressed in the number of hours.
        /// </summary>
        /// <value>The total number of hours represented by this instance.</value>
        public Int32 TotalHours { get { return _clock.GetHours(_time); } }

        /// <summary>
        /// Gets the value of the duration expressed in the number of minutes.
        /// </summary>
        /// <value>The total number of minutes represented by this instance.</value>
        public Int32 TotalMinutes { get { return _clock.GetMinutes(_time); } }

        /// <summary>
        /// Gets the value of the duration expressed in the number of seconds.
        /// </summary>
        /// <value>The total number of seconds represented by this instance.</value>
        public Int32 TotalSeconds { get { return _clock.GetSeconds(_time); } }

        /// <summary>
        /// Gets the value of the duration expressed in the number of milliseconds.
        /// </summary>
        /// <value>The total number of milliseconds represented by this instance.</value>
        public Int32 TotalMilliseconds { get { return _clock.GetMilliseconds(_time); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodDuration" /> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The reference system cannot be null.</exception>
        /// <exception cref="System.ArgumentException">The reference system does not specify the calendar and the clock.</exception>
        public PeriodDuration(Duration duration, IReferenceSystem referenceSystem, IMetadataCollection metadata)
        {
            if (referenceSystem == null)
                throw new ArgumentNullException("referenceSystem", "The reference system is null.");

            if (referenceSystem is Calendar)
            {
                _calendar = referenceSystem as Calendar;
                _clock = (referenceSystem as Calendar).TimeBasis;
            }
            if (referenceSystem is Clock)
            {
                _calendar = (referenceSystem as Clock).CalendarBasis;
                _clock = referenceSystem as Clock;
            }
            if (referenceSystem is CompoundTemporalReferenceSystem)
            {
                _calendar = (referenceSystem as CompoundTemporalReferenceSystem).Calendar;
                _clock = (referenceSystem as CompoundTemporalReferenceSystem).Clock;
            }
            if (_calendar == null) 
            {
                throw new ArgumentException("The reference system does not specify the calendar and the clock.", "referenceSystem");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodDuration" /> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <param name="calendar">The calendar.</param>
        /// <param name="clock">The clock.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The calendar is null.
        /// or
        /// The clock is null.
        /// </exception>
        public PeriodDuration(Duration date, Duration time, Calendar calendar, Clock clock, IMetadataCollection metadata)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar", "The calendar is null.");
            if (clock == null)
                throw new ArgumentNullException("clock", "The clock is null.");

            _date = date;
            _time = time;
            _calendar = calendar;
            _clock = clock;
        }

        #endregion
    }
}
