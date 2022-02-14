/// <copyright file="Clock.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Temporal.Positioning;

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a clock.
    /// </summary>
    public class Clock : TemporalReferenceSystem
    {
        #region Public properties

        /// <summary>
        /// Gets the reference event.
        /// </summary>
        /// <value>The name or description of an event, which fixes the position of the base scale of the clock.</value>
        public String ReferenceEvent { get; private set; }

        /// <summary>
        /// Gets the reference time.
        /// </summary>
        /// <value>The time of day associated with the reference event expressed as a time of day in the given clock.</value>
        public ClockTime ReferenceTime { get; private set; }

        /// <summary>
        /// Gets the UTC reference time.
        /// </summary>
        /// <value>The time of day associated with the reference event expressed as a time of day in the UTC clock.</value>
        public ClockTime UTCReferenceTime { get { return new ClockTime(ReferenceTime.Instant - Offset, Clocks.UTC, null); } }

        /// <summary>
        /// Gets the offset from the UTC clock.
        /// </summary>
        /// <value>The offset from the UTC clock.</value>
        public Duration Offset { get; private set; }

        /// <summary>
        /// Gets the number of hours per day.
        /// </summary>
        /// <value>The number of hours per day.</value>
        public Int32 HoursPerDay { get { return 24; } }

        /// <summary>
        /// Gets the number of minutes per hour.
        /// </summary>
        /// <value>The number of minutes per hour.</value>
        public Int32 MinutesPerHour { get { return 60; } }

        /// <summary>
        /// Gets the number of minutes per day.
        /// </summary>
        /// <value>The number of minutes per day.</value>
        public Int32 MinutesPerDay { get { return MinutesPerHour * HoursPerDay; } }

        /// <summary>
        /// Gets the number of seconds per minute.
        /// </summary>
        /// <value>The number of seconds per minute.</value>
        public Int32 SecondsPerMinute { get { return 60; } }

        /// <summary>
        /// Gets the number of seconds per hour.
        /// </summary>
        /// <value>The number of seconds per hour.</value>
        public Int32 SecondsPerHour { get { return SecondsPerMinute * MinutesPerHour; } }

        /// <summary>
        /// Gets the number of seconds per day.
        /// </summary>
        /// <value>The number of seconds per day.</value>
        public Int32 SecondsPerDay { get { return SecondsPerMinute * MinutesPerHour * HoursPerDay; } }

        /// <summary>
        /// Gets the number of milliseconds per second.
        /// </summary>
        /// <value>The number milliseconds per second.</value>
        public Int32 MillisecondsPerSecond { get { return 1000; } }

        /// <summary>
        /// Gets the number of milliseconds per minute.
        /// </summary>
        /// <value>The number milliseconds per minute.</value>
        public Int32 MillisecondsPerMinute { get { return MillisecondsPerSecond * SecondsPerMinute; } }

        /// <summary>
        /// Gets the number of milliseconds per hour.
        /// </summary>
        /// <value>The number milliseconds per hour.</value>
        public Int32 MillisecondsPerHour { get { return MillisecondsPerSecond * SecondsPerMinute * MinutesPerHour; } }

        /// <summary>
        /// Gets the number of milliseconds per day.
        /// </summary>
        /// <value>The number milliseconds per day.</value>
        public Int32 MillisecondsPerDay { get { return MillisecondsPerSecond * SecondsPerMinute * MinutesPerHour * HoursPerDay; } }

        /// <summary>
        /// Gets the number of ticks per millisecond.
        /// </summary>
        /// <value>The number of ticks per millisecond.</value>
        public Int64 TicksPerMillisecond { get { return 1000000; } }

        /// <summary>
        /// Gets the number of ticks per second.
        /// </summary>
        /// <value>The number of ticks per second.</value>
        public Int64 TicksPerSecond { get { return TicksPerMillisecond * MillisecondsPerSecond; } }

        /// <summary>
        /// Gets the number of ticks per minute.
        /// </summary>
        /// <value>The number of ticks per minute.</value>
        public Int64 TicksPerMinute { get { return TicksPerMillisecond * MillisecondsPerSecond * SecondsPerMinute; } }

        /// <summary>
        /// Gets the number of ticks per hour.
        /// </summary>
        /// <value>The number of ticks per hour.</value>
        public Int64 TicksPerHour { get { return TicksPerMillisecond * MillisecondsPerSecond * SecondsPerMinute * MinutesPerHour; } }

        /// <summary>
        /// Gets the number of ticks per day.
        /// </summary>
        /// <value>The number of ticks per day.</value>
        public Int64 TicksPerDay { get { return TicksPerMillisecond * MillisecondsPerSecond * SecondsPerMinute * MinutesPerHour * HoursPerDay; } }

        /// <summary>
        /// Gets the calendar basis of the clock.
        /// </summary>
        /// <value>The calendar that serves as a basis for the clock.</value>
        public Calendar CalendarBasis { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Clock" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="basis">The basis.</param>
        /// <param name="offset">The offset.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The calendar basis is null.
        /// </exception>
        public Clock(String identifier, String name, Calendar basis, Duration offset) : this(identifier, name, null, null, null, basis, null, Instant.Zero, offset) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Clock" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="basis">The basis.</param>
        /// <param name="referenceEvent">The reference event.</param>
        /// <param name="referenceInstant">The reference instant.</param>
        /// <param name="offset">The offset.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The calendar basis is null.
        /// </exception>
        public Clock(String identifier, String name, String remarks, String[] aliases, String scope, Calendar basis, String referenceEvent, Instant referenceInstant, Duration offset)
            : base(identifier, name, remarks, aliases, scope) 
        {
            if (basis == null)
                throw new ArgumentNullException("basis", "The calendar basis is null.");

            CalendarBasis = basis;
            ReferenceEvent = referenceEvent;
            ReferenceTime = new ClockTime(referenceInstant, this, null);
            Offset = offset;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the instant for the specified time.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns>The instant representing the specified time.</returns>
        public Instant GetInstant(Int32 hours, Int32 minutes, Int32 seconds)
        {
            return GetInstant(hours, minutes, seconds, 0);
        }

        /// <summary>
        /// Returns the instant for the specified time.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>The instant representing the specified time.</returns>
        public Instant GetInstant(Int32 hours, Int32 minutes, Int32 seconds, Int32 milliseconds)
        {
            return new Instant(hours * TicksPerHour + minutes * TicksPerMinute + seconds * TicksPerSecond + milliseconds * TicksPerMillisecond); 
        }

        /// <summary>
        /// Returns the clock time for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The clock time for the specified instant.</returns>
        public ClockTime GetTime(Instant instant) { return new ClockTime(instant, this, null); }

        /// <summary>
        /// Returns the hours for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The hours for <paramref name="instant" />.</returns>
        public Int32 GetHour(Instant instant) { return Convert.ToInt32(instant.Ticks / TicksPerHour); }

        /// <summary>
        /// Returns the hours for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The hours for <paramref name="duration" />.</returns>
        public Int32 GetHours(Duration duration) { return Convert.ToInt32(duration.Ticks / TicksPerHour); }

        /// <summary>
        /// Returns the minutes within an hour for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The minutes within an hour for <paramref name="instant" />.</returns>
        public Int32 GetMinute(Instant instant) { return Convert.ToInt32(instant.Ticks / TicksPerMinute) % MinutesPerHour; }
        
        /// <summary>
        /// Returns the minutes within an hour for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The minutes within an hour for <paramref name="duration" />.</returns>
        public Int32 GetMinutes(Duration duration) { return Convert.ToInt32(duration.Ticks / TicksPerMinute) % MinutesPerHour; }
        
        /// <summary>
        /// Returns the seconds within a minute for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The seconds within a minute for <paramref name="instant" />.</returns>
        public Int32 GetSecond(Instant instant) { return Convert.ToInt32(instant.Ticks / TicksPerSecond) % SecondsPerMinute; }
        
        /// <summary>
        /// Returns the seconds within a minute for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The seconds within a minute for <paramref name="duration" />.</returns>
        public Int32 GetSeconds(Duration duration) { return Convert.ToInt32(duration.Ticks / TicksPerSecond) % SecondsPerMinute; }
        
        /// <summary>
        /// Returns the milliseconds within a second for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The milliseconds within a second for <paramref name="instant" />.</returns>
        public Int32 GetMilliseconds(Instant instant) { return Convert.ToInt32(instant.Ticks / TicksPerMillisecond) % MillisecondsPerSecond; }
        
        /// <summary>
        /// Returns the milliseconds within a second for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The milliseconds within a second for <paramref name="duration" />.</returns>
        public Int32 GetMilliseconds(Duration duration) { return Convert.ToInt32(duration.Ticks / TicksPerMillisecond) % MillisecondsPerSecond; }
        
        /// <summary>
        /// Returns the total number of minutes for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The total number of minutes for <paramref name="duration" />.</returns>
        public Int32 GetTotalMinutes(Instant instant) { return Convert.ToInt32(instant.Ticks / TicksPerSecond); }
        
        /// <summary>
        /// Returns the total number of minutes for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The total number of minutes for <paramref name="duration" />.</returns>
        public Int32 GetTotalMinutes(Duration duration) { return Convert.ToInt32(duration.Ticks / TicksPerSecond); }
        
        /// <summary>
        /// Returns the total number of seconds for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The total number of seconds for <paramref name="duration" />.</returns>
        public Int32 GetTotalSeconds(Instant instant) { return Convert.ToInt32(instant.Ticks / TicksPerMinute); }
        
        /// <summary>
        /// Returns the total number of seconds for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The total number of seconds for <paramref name="duration" />.</returns>
        public Int32 GetTotalSeconds(Duration duration) { return Convert.ToInt32(duration.Ticks / TicksPerMinute); }
        
        /// <summary>
        /// Returns the total number of milliseconds for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The total number of milliseconds for <paramref name="duration" />.</returns>
        public Int32 GetTotalMilliseconds(Instant instant) { return Convert.ToInt32(instant.Ticks / TicksPerMillisecond); }
        
        /// <summary>
        /// Returns the total number of milliseconds for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The total number of milliseconds for <paramref name="duration" />.</returns>
        public Int32 GetTotalMilliseconds(Duration duration) { return Convert.ToInt32(duration.Ticks / TicksPerMillisecond); }
        
        /// <summary>
        /// Returns the clock time for an UTC time.
        /// </summary>
        /// <param name="utcTime">The UTC time.</param>
        /// <returns>The clock time for the specified UTC time.</returns>
        /// <exception cref="System.ArgumentNullException">The UTC time is null.</exception>
        /// <exception cref="System.ArgumentException">The specified time is not an UTC time.</exception>
        public ClockTime TransformToClockTime(ClockTime utcTime) 
        {
            if (utcTime == null)
                throw new ArgumentNullException("utcTime", "The UTC time is null.");
            if (!utcTime.ReferenceSystem.Equals(Clocks.UTC))
                throw new ArgumentException("The specified time is not an UTC time.", "utcTime");
            return new ClockTime(utcTime.Instant + Offset, this, null);
        }

        /// <summary>
        /// Returns the UTC time for a clock time.
        /// </summary>
        /// <param name="clockTime">The clock time.</param>
        /// <returns>The UTC time for the specified clock time.</returns>
        /// <exception cref="System.ArgumentNullException">The clock time is null.</exception>
        /// <exception cref="System.ArgumentException">The specified time is not referenced in the current clock.</exception>
        public ClockTime TransformToUTCTime(ClockTime clockTime)
        {
            if (clockTime == null)
                throw new ArgumentNullException("clockTime", "The clock time is null.");
            if (!clockTime.ReferenceSystem.Equals(this))
                throw new ArgumentException("The specified time is not referenced in the current clock.", "clockTime");
            return new ClockTime(clockTime.Instant - Offset, this, null);
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Create a clock from a time zone.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="isDaylightSaving">The value indicating whether the clock should be a daylight saving clock.</param>
        /// <returns>The clock created from the specified time zone.</returns>
        /// <exception cref="System.ArgumentNullException">The time zone is null.</exception>
        public static Clock FromTimeZone(TimeZone timeZone, Boolean isDaylightSaving)
        {
            if (timeZone == null)
                throw new ArgumentNullException("timeZone", "The time zone is null.");

            if (isDaylightSaving && timeZone.SupportsDaylightSavingTime)
                return new Clock(TransformTimeZoneIdentifier(timeZone.DaylightIdentifier), timeZone.DaylightName, Calendars.GregorianCalendar, timeZone.DaylightOffset);
            else
                return new Clock(TransformTimeZoneIdentifier(timeZone.Identifier), timeZone.Name, Calendars.GregorianCalendar, timeZone.Offset);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Transforms the time zone identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The clock identifier matching the time zone.</returns>
        private static String TransformTimeZoneIdentifier(String identifier)
        {
            if (identifier == null)
                return null;
            return "AEGIS::" + (Int32.Parse(identifier.Substring(7, 6)) - 1000);
        }

        #endregion
    }
}
