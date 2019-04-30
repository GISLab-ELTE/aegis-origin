/// <copyright file="JulianDateSystem.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
    /// Represents a coordinate system of Julian Dates.
    /// </summary>
    /// <remarks>
    /// The Julian Date of any instant is the Julian day number days plus the fraction of the day since the beginning of the Julian Period used primarily by astronomers.
    /// The day number 0 is assigned to the day starting at noon on November 24, 4714 BC in the Gregorian calendar. 
    /// </remarks>
    public class JulianDateSystem : TemporalCoordinateSystem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianDateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public JulianDateSystem(String identifier, String name)
            : this(identifier, name, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianDateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public JulianDateSystem(String identifier, String name, String remarks, String[] aliases, String scope)
            : base(identifier, name, remarks, aliases, scope, new DateAndTime(Calendars.GregorianCalendar.GetInstant(-4713, 11, 24), Clocks.UTC.GetInstant(12, 0, 0), CompoundTemporalReferenceSystems.GregorianCalendarUTCClock, null), UnitsOfMeasurement.Second)
        {             
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="JulianDateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="origin">The origin.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// interval;The interval is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The origin is not expressed in the Gregorian calendar and UTC clock.;origin
        /// or
        /// The interval is not a time measure.;interval
        /// </exception>
        protected JulianDateSystem(String identifier, String name, String remarks, String[] aliases, String scope, DateAndTime origin)
            : base(identifier, name, remarks, aliases, scope, origin, UnitsOfMeasurement.Second)
        {
        }

        #endregion

        #region Public TemporalCoordinateSystem methods

        /// <summary>
        /// Transforms a coordinate to date/time.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>A date/time representation of the specified coordinate in the Gregorian calendar and UTC clock.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The coordinate is not referenced in the current coordinate system.
        /// or
        /// The coordinate is before the origin of the current coordinate system.
        /// </exception>
        public override DateAndTime TransformToDateAndTime(TemporalCoordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate", "The coordinate is null.");
            if (!coordinate.ReferenceSystem.Equals(this))
                throw new ArgumentException("The coordinate is not referenced in the current coordinate system.", "coordinate");
            if (coordinate.Instant.Ticks < 0)
                throw new ArgumentException("The coordinate is before the origin of the current coordinate system.", "coordinate");

            // compute the components
            Int64 date = Convert.ToInt64((coordinate as JulianDate).Date) + Origin.Date.Instant.Ticks;
            Int64 time = Convert.ToInt64((coordinate as JulianDate).Time / Clocks.UTC.SecondsPerDay) * Clocks.UTC.TicksPerDay;

            // correct the half day
            if (time > Clocks.UTC.TicksPerDay / 2)
            {
                time -= Clocks.UTC.TicksPerDay / 2;
            }
            else
            {
                date--;
                time += Clocks.UTC.TicksPerDay / 2;
            }

            return new DateAndTime(new Instant(date), new Instant(time), CompoundTemporalReferenceSystems.GregorianCalendarUTCClock, null);
        }

        /// <summary>
        /// Transforms a date/time to a coordinate.
        /// </summary>
        /// <param name="dateTime">A date/time in the Gregorian calendar and UTC clock.</param>
        /// <returns>A coordinate representation og the specified date/time.</returns>
        /// <exception cref="System.ArgumentNullException">The date/time is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The date/time is not referenced in the Gregorian calendar and UTC Clock.
        /// or
        /// The date/time is before the origin of the current coordinate system.
        /// </exception>
        public override TemporalCoordinate TransformFromDateAndTime(DateAndTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime", "The date/time is null.");
            if (!dateTime.ReferenceSystem.Equals(CompoundTemporalReferenceSystems.GregorianCalendarUTCClock))
                throw new ArgumentException("The date/time is not referenced in the Gregorian calendar and UTC Clock.", "dateTime");
            if (dateTime.Instant < Origin.Instant)
                throw new ArgumentException("The date/time is before the origin of the current coordinate system.", "dateTime");

            // compute the components
            Int64 date = dateTime.Date.Instant.Ticks - Origin.Instant.Ticks;
            Decimal time = Convert.ToDecimal(dateTime.TimeOfDay.Instant.Ticks) / Clocks.UTC.TicksPerDay;

            // correct the half day
            if (time < Clocks.UTC.TicksPerDay / 2)
            {
                time += Clocks.UTC.TicksPerDay / 2;
            }
            else
            {
                date++;
                time -= Clocks.UTC.TicksPerDay / 2;
            }

            return new JulianDate(new Instant(Convert.ToInt64(date + time) * Clocks.UTC.SecondsPerDay), this, null);
        }

        #endregion
    }
}
