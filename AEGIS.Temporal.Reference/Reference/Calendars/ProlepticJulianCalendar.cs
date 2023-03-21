// <copyright file="ProlepticJulianCalendar.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a pure proleptic Julian calendar.
    /// </summary>
    /// <remarks>
    /// This calendar follows the leap year rule strictly, even for dates before 8 CE, where leap years were actually irregular.
    /// Although the Julian calendar did not exist before 45 BCE, this calendar assumes it did, thus it is proleptic.
    /// </remarks>
    public class ProlepticJulianCalendar : JulianCalendar
    {
        #region Protected constant fields

        /// <summary>
        /// The number of days in a leap cycle. This field is constant.
        /// </summary>
        protected const Int64 NumberOfDaysInLeapCycle = LeapYearCycleCE * NumberOfDaysInYear + 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProlepticJulianCalendar" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ProlepticJulianCalendar(String identifier, String name)
            : base(identifier, name)            
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProlepticJulianCalendar" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ProlepticJulianCalendar(String identifier, String name, String remarks, String[] aliases, String scope)
            : base(identifier, name, remarks, aliases, scope) 
        {
        }

        #endregion

        #region Public Calendar methods

        /// <summary>
        /// Determines whether the specified year in the current era is a leap year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns><c>true</c> if the specified year is a leap year; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        public override Boolean IsLeapYear(Int64 year)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");

            if (year < 0)
                return (year + 1) % 4 == 0;

            return year % 4 == 0;
        }

        /// <summary>
        /// Computes an instant in the calendar.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The instant computed from the year, month and day values.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        public override Instant GetInstant(Int64 year, Int64 month, Int64 day)
        {
            // source: http://calendopedia.com/julian.htm

            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");

            // general ticks
            Int64 ticks = year * NumberOfDaysInYear + (IsLeapYear(year) ? DaysToMonthInLeapYear[month - 1] : DaysToMonth[month - 1]) + day - 1;

            // leap days
            if (year < 0)
            {
                ticks += (year + 1) / LeapYearCycleCE;
            }
            else
            {
                ticks += (year - 1) / LeapYearCycleCE;
                ticks -= NumberOfDaysInYear; // there is no year 0
            }

            return new Instant(ticks);
        }

        /// <summary>
        /// Computes an instant in the calendar.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="dayOfYear">The day of year.</param>
        /// <returns>The instant computed from the year and day of year values.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        public override Instant GetInstant(Int64 year, Int64 dayInYear)
        {
            // source: http://calendopedia.com/julian.htm

            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");

            // general ticks
            Int64 ticks = year * NumberOfDaysInYear + dayInYear - 1;

            // leap days
            if (year < 0)
            {
                ticks += (year + 1) / LeapYearCycleCE;
            }
            else
            {
                ticks += (year - 1) / LeapYearCycleCE;
                ticks -= NumberOfDaysInYear; // there is no year 0
            }

            if (ticks < NumberOfDaysBCE)
                throw new InvalidOperationException("The specified date is before the begin of use.");

            return new Instant(ticks);
        }

        /// <summary>
        /// Returns the calendar year for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The year of the instant in the current calendar.</returns>
        public override Int64 GetYear(Instant instant)
        {
            if (instant.Ticks < 0)
            {
                return -ComputeDatePartWithLeapCycle(Math.Abs(instant.Ticks - 3 * NumberOfDaysInYear + 1), DatePart.Year, LeapYearCycleCE) + 2;
            }

            return ComputeDatePartWithLeapCycle(instant.Ticks, DatePart.Year, LeapYearCycleCE) + 1;
        }

        /// <summary>
        /// Returns the calendar month for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The month of year of the instant in the current calendar.</returns>
        public override Int64 GetMonth(Instant instant)
        {
            if (instant.Ticks < 0)
            {
                return ComputeDatePartWithLeapCycle((instant.Ticks % NumberOfDaysInLeapCycle) + NumberOfDaysInLeapCycle + NumberOfDaysInYear, DatePart.Month, LeapYearCycleCE) + 1;
            }
            return ComputeDatePartWithLeapCycle(instant.Ticks, DatePart.Month, LeapYearCycleCE) + 1;
        }

        /// <summary>
        /// Returns the calendar day for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The day of month of the instant in the current calendar.</returns>
        public override Int64 GetDay(Instant instant)
        {
            if (instant.Ticks < 0)
            {
                return ComputeDatePartWithLeapCycle((instant.Ticks % NumberOfDaysInLeapCycle) + NumberOfDaysInLeapCycle + NumberOfDaysInYear, DatePart.Day, LeapYearCycleCE) + 1;
            }
            return ComputeDatePartWithLeapCycle(instant.Ticks, DatePart.Day, LeapYearCycleCE) + 1;
        }

        /// <summary>
        /// Returns the calendar day for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The day of year of the instant in the current calendar.</returns>
        public override Int64 GetDayOfYear(Instant instant)
        {
            if (instant.Ticks < 0)
            {
                return ComputeDatePartWithLeapCycle((instant.Ticks % NumberOfDaysInLeapCycle) + NumberOfDaysInLeapCycle + NumberOfDaysInYear, DatePart.DayOfYear, LeapYearCycleCE) + 1;
            }
            return ComputeDatePartWithLeapCycle(instant.Ticks, DatePart.DayOfYear, LeapYearCycleCE) + 1;
        }

        /// <summary>
        /// Returns the Julian Date of a calendar date.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The Julian Date that matches the specified date.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the month.
        /// </exception>
        public override JulianDate TranformToJulianDate(Int64 year, Int64 month, Int64 day)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (month < 1)
                throw new ArgumentOutOfRangeException("month", "The month is less than 1.");
            if (month > NumberOfMonthsInYear)
                throw new ArgumentOutOfRangeException("month", "The month is greater than the number of months in the year.");
            if (day < 1)
                throw new ArgumentOutOfRangeException("day", "The day is less than 1.");
            if (day > (IsLeapYear(year) ? DaysToMonthInLeapYear[month] - DaysToMonthInLeapYear[month - 1] : DaysToMonth[month] - DaysToMonth[month - 1]))
                throw new ArgumentOutOfRangeException("day", "The day is greater than the number of days in the month.");

            year += 4713;

            Int64 julianDay = year * NumberOfDaysInYear + year / LeapYearCycleCE; // compute year
            julianDay += (IsLeapYear(year) ? DaysToMonthInLeapYear[month - 1] : DaysToMonth[month - 1]) + day - 1; // compute month and day

            return new JulianDate(new Instant(julianDay * Clocks.UTC.SecondsPerDay), TemporalCoordinateSystems.JulianDateSystem, null);
        }

        /// <summary>
        /// Returns the calendar date for a Julian Date.
        /// </summary>
        /// <param name="date">The Julian Date.</param>
        /// <returns>The calendar date that matches <paramref name="date" />.</returns>
        /// <exception cref="System.ArgumentNullException">The date is null.</exception>
        public override CalendarDate TransformFromJulianDate(JulianDate date)
        {
            if (date == null)
                throw new ArgumentNullException("date", "The date is null.");

            Int64 ticks = date.Instant.Ticks / Clocks.UTC.SecondsPerDay; // number of days
            ticks -= GetInstant(-4713, 1, 1).Ticks; // the begin of the Julian Date System

            return GetDate(new Instant(ticks));
        }

        #endregion

        #region Protected Calendar methods

        /// <summary>
        /// Computes the eras of the calendar.
        /// </summary>
        protected override void ComputeEras()
        {
            _eras = new CalendarEra[]
            {
                new CalendarEra("AEGIS::813301", "Before Common Era (BCE)", this, null, Instant.MinValue, Instant.MinValue, new Instant(-1)),
                new CalendarEra("AEGIS::813302", "Common Era (CE)", this, null, Instant.MinValue, new Instant(0), Instant.MaxValue)
            };
        }

        #endregion
    }
}
