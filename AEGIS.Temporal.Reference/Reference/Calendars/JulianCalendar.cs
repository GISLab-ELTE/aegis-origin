// <copyright file="JulianCalendar.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Julian calendar.
    /// </summary>
    /// <remarks>
    /// The Julian calendar was a reform of the Roman calendar introduced by Julius Caesar in 46 BC (708 AUC). 
    /// It took effect in 45 BC (709 AUC). It was the predominant calendar in most of Europe, and in European settlements in the Americas and elsewhere, until it was superseded by the Gregorian calendar.
    /// </remarks>
    public class JulianCalendar : Calendar
    {
        #region Protected constant fields

        /// <summary>
        /// The reference year. This field is constant.
        /// </summary>
        protected const Int32 ReferenceYear = -46;

        /// <summary>
        /// The first calendar year. This field is constant.
        /// </summary>
        protected const Int32 FirstYear = -45;

        /// <summary>
        /// The number of years in a leap year cycle before the common era. This field is constant.
        /// </summary>
        protected const Int32 LeapYearCycleBCE = 3;

        /// <summary>
        /// The number of years in a leap year cycle in the common era. This field is constant.
        /// </summary>
        protected const Int32 LeapYearCycleCE = 4;

        /// <summary>
        /// The number of months in a year. This field is constant.
        /// </summary>
        protected const Int32 NumberOfDaysInYear = 365;

        /// <summary>
        /// The number of months in a year. This field is constant.
        /// </summary>
        protected const Int32 NumberOfMonthsInYear = 12;

        /// <summary>
        /// The number of years before the common era. This field is constant.
        /// </summary>
        protected const Int32 NumberOfYearsBCE = 45;

        /// <summary>
        /// The number of days before the common era. This field is constant.
        /// </summary>
        protected const Int32 NumberOfDaysBCE = NumberOfYearsBCE * NumberOfDaysInYear + 13;

        #endregion

        #region Protected static fields

        /// <summary>
        /// The days of the year until the beginning of the specified month. This field is read-only.
        /// </summary>
        protected static readonly Int32[] DaysToMonth =
        {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 
        };

        /// <summary>
        /// The days of the year until the beginning of the specified month in a leap year. This field is read-only.
        /// </summary>
        protected static readonly Int32[] DaysToMonthInLeapYear =
        {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366
        };

        #endregion

        #region Public Calendar properties

        /// <summary>
        /// Gets the time basis of the calendar.
        /// </summary>
        /// <value>The clock that serves as a basis to the calendar.</value>
        public override Clock TimeBasis
        {
            get { return Clocks.UTC; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianCalendar" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public JulianCalendar(String identifier, String name)
            : this(identifier, name, null, null, null)            
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianCalendar" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public JulianCalendar(String identifier, String name, String remarks, String[] aliases, String scope)
            : base(identifier, name, remarks, aliases, scope) 
        {
        }

        #endregion

        #region Public Calendar methods

        /// <summary>
        /// Returns the number of days in the specified year and month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month in the year.</param>
        /// <returns>The number of days in the month.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The specified date is before the begin of use.
        /// or
        /// The specified date is after the end of use.
        /// </exception>
        public override Int64 GetDaysInMonth(Int64 year, Int64 month)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (year < FirstYear)
                throw new ArgumentOutOfRangeException("year", "The year is before the begin of the calendar.");
            if (month < 1)
                throw new ArgumentOutOfRangeException("month", "The month is less than 1.");
            if (month > NumberOfMonthsInYear)
                throw new ArgumentOutOfRangeException("month", "The month is greater than the number of months in the year.");

            if (IsLeapYear(year))
                return DaysToMonthInLeapYear[month] - DaysToMonthInLeapYear[month - 1]; 

            return DaysToMonth[month] - DaysToMonth[month - 1];
        }

        /// <summary>
        /// Gets the number of days in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The number of days in the year.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The year is before the begin of the calendar.</exception>
        public override Int64 GetDaysInYear(Int64 year)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (year < FirstYear)
                throw new ArgumentOutOfRangeException("year", "The year is before the begin of the calendar.");

            return IsLeapYear(year) ? NumberOfDaysInYear : NumberOfDaysInYear + 1;
        }

        /// <summary>
        /// Gets the number of months in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The number of months in the year.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The year is before the begin of the calendar.</exception>
        public override Int64 GetMonthsInYear(Int64 year)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (year < FirstYear)
                throw new ArgumentOutOfRangeException("year", "The year is before the begin of the calendar.");

            return NumberOfMonthsInYear;
        }

        /// <summary>
        /// Determines whether the specified year in the current era is a leap year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns><c>true</c> if the specified year is a leap year; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The year is before the begin of the calendar.</exception>
        public override Boolean IsLeapYear(Int64 year)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (year < FirstYear)
                throw new ArgumentOutOfRangeException("year", "The year is before the begin of the calendar.");

            if (year <= -9)
            {
                return year % 3 == 0;
            }
            else if (year >= 8)
            {
                return year % 4 == 0;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified month in the specified year is a leap month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month in the year.</param>
        /// <returns><c>true</c> if the specified month is a leap month; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// </exception>
        public override Boolean IsLeapMonth(Int64 year, Int64 month)
        {
            if (month < 1)
                throw new ArgumentOutOfRangeException("month", "The month is less than 1.");
            if (month > NumberOfMonthsInYear)
                throw new ArgumentOutOfRangeException("month", "The month is greater than the number of months in the year.");

            return IsLeapYear(year) && DaysToMonth[month] - DaysToMonth[month - 1] != DaysToMonthInLeapYear[month] - DaysToMonthInLeapYear[month - 1];
        }

        /// <summary>
        /// Determines whether the specified day in the specified year is a leap day.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="dayInYear">The day in year.</param>
        /// <returns><c>true</c> if the specified day is a leap day; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the year.
        /// </exception>
        public override Boolean IsLeapDay(Int64 year, Int64 dayInYear)
        {
            if (dayInYear < 1)
                throw new ArgumentOutOfRangeException("dayInYear", "The day is less than 1.");
            if (dayInYear >= NumberOfDaysInYear + 1)
                throw new ArgumentOutOfRangeException("dayInYear", "The day is greater than the number of days in the year.");

            return IsLeapYear(year) && dayInYear == DaysToMonthInLeapYear[2];
        }

        /// <summary>
        /// Determines whether the specified date is a leap day.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month in year.</param>
        /// <param name="day">The day in month.</param>
        /// <returns><c>true</c> if the specified date is a leap day; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the month.
        /// </exception>
        public override Boolean IsLeapDay(Int64 year, Int64 month, Int64 day)
        {
            if (day < 1)
                throw new ArgumentOutOfRangeException("day", "The day is less than 1.");

            if (!IsLeapMonth(year, month))
                return false;

            if (day >= DaysToMonthInLeapYear[month] - DaysToMonthInLeapYear[month])
                throw new ArgumentOutOfRangeException("day", "The day is greater than the number of days in the month.");

            return day == DaysToMonthInLeapYear[month] - DaysToMonthInLeapYear[month];
        }

        /// <summary>
        /// Computes an instant in the calendar.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The instant computed from the year, month and day values.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the month.
        /// </exception>
        public override Instant GetInstant(Int64 year, Int64 month, Int64 day)
        {
            // source: http://calendopedia.com/julian.htm

            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (year < FirstYear)
                throw new ArgumentOutOfRangeException("year", "The year is before the begin of the calendar.");
            if (month < 1)
                throw new ArgumentOutOfRangeException("month", "The month is less than 1.");
            if (month > NumberOfMonthsInYear)
                throw new ArgumentOutOfRangeException("month", "The month is greater than the number of months in the year.");
            if (day < 1)
                throw new ArgumentOutOfRangeException("day", "The day is less than 1.");
            if (day > (IsLeapYear(year) ? DaysToMonthInLeapYear[month] - DaysToMonthInLeapYear[month - 1] : DaysToMonth[month] - DaysToMonth[month - 1]))
                throw new ArgumentOutOfRangeException("day", "The day is greater than the number of days in the month.");

            // general ticks
            Int64 ticks = year * NumberOfDaysInYear + (IsLeapYear(year) ? DaysToMonthInLeapYear[month - 1] : DaysToMonth[month - 1]) + day - 1;

            // leap days
            if (year <= -9)
            {
                ticks += (year + 6) / LeapYearCycleBCE; 
            }
            if (year > 8)
            {
                ticks += (year - 5) / LeapYearCycleCE;
            }

            // there is no year 0
            if (year > 0)
                ticks -= NumberOfDaysInYear;

            return new Instant(ticks);
        }

        /// <summary>
        /// Computes an instant in the calendar.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="dayOfYear">The day of year.</param>
        /// <returns>The instant computed from the year and day of year values.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the year.
        /// </exception>
        public override Instant GetInstant(Int64 year, Int64 dayOfYear)
        {
            // source: http://calendopedia.com/julian.htm

            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (year < FirstYear)
                throw new ArgumentOutOfRangeException("year", "The year is before the begin of the calendar.");
            if (dayOfYear < 1)
                throw new ArgumentOutOfRangeException("dayInYear", "The day is less than 1.");
            if (dayOfYear >= NumberOfDaysInYear + 1)
                throw new ArgumentOutOfRangeException("dayInYear", "The day is greater than the number of days in the year.");

            // general ticks
            Int64 ticks = year * NumberOfDaysInYear + dayOfYear - 1;

            // leap days
            if (year <= -9)
            {
                ticks += (year + 6) / LeapYearCycleBCE;
            }
            if (year > 8)
            {
                ticks += (year - 5) / LeapYearCycleCE;
            }

            // there is no year 0
            if (year > 0)
                ticks -= NumberOfDaysInYear;

            return new Instant(ticks);
        }

        /// <summary>
        /// Returns the calendar year for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The year of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentException">The instant is before the begin of the calendar.</exception>
        public override Int64 GetYear(Instant instant)
        {
            if (instant.Ticks < -NumberOfDaysBCE)
                throw new ArgumentException("The instant is before the begin of the calendar.", "instant");

            // 3 year leap cycle from 45 BC to 9 BC
            if (instant.Ticks < -8 * NumberOfDaysInYear) 
            {
                return -ComputeDatePartWithLeapCycle(Math.Abs(instant.Ticks + 6 * NumberOfDaysInYear + 1), DatePart.Year, LeapYearCycleBCE) - 7; 
            }
            // no leap years
            if (instant.Ticks < 0)
            {
                return -ComputeDatePart(Math.Abs(instant.Ticks + 1), DatePart.Year) - 1;
            }
            if (instant.Ticks < 7 * NumberOfDaysInYear)
            {
                return ComputeDatePart(instant.Ticks, DatePart.Year) + 1;
            }
            // 4 year leap cycle from 8 AD
            return ComputeDatePartWithLeapCycle(instant.Ticks - 4 * NumberOfDaysInYear, DatePart.Year, LeapYearCycleCE) + 5; 
        }

        /// <summary>
        /// Returns the number of years for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of years for the duration in the current calendar.</returns>
        public override Int64 GetYears(Duration duration)
        {
            return ComputeDatePartWithLeapCycle(duration.Ticks, DatePart.Year, LeapYearCycleCE); 
        }

        /// <summary>
        /// Returns the calendar month for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The month of year of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentException">The instant is before the begin of the calendar.</exception>
        public override Int64 GetMonth(Instant instant) 
        {
            if (instant.Ticks < -NumberOfDaysBCE)
                throw new ArgumentException("The instant is before the begin of the calendar.", "instant");

            // 3 year leap cycle from 45 BC to 9 BC
            if (instant.Ticks < -6 * NumberOfDaysInYear)
            {
                // the value must be positive and the leap year must be the third year
                return ComputeDatePartWithLeapCycle(instant.Ticks + NumberOfDaysBCE + 2 * NumberOfDaysInYear, DatePart.Month, LeapYearCycleBCE) + 1;
            }
            // no leap years
            if (instant.Ticks < 7 * NumberOfDaysInYear)
            {
                // the value must be positive
                return ComputeDatePart(instant.Ticks + 6 * NumberOfDaysInYear, DatePart.Month) + 1;
            }

            // 4 year leap cycle from 8 AD
            return ComputeDatePartWithLeapCycle(instant.Ticks - 4 * NumberOfDaysInYear, DatePart.Month, LeapYearCycleCE) + 1; 
        }

        /// <summary>
        /// Returns the number of months for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of months for the duration in the current calendar.</returns>
        public override Int64 GetMonths(Duration duration)
        {
            return ComputeDatePartWithLeapCycle(duration.Ticks, DatePart.Month, LeapYearCycleCE); 
        }

        /// <summary>
        /// Returns the calendar day for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The day of month of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentException">The instant is before the begin of the calendar.</exception>
        public override Int64 GetDay(Instant instant)
        {
            if (instant.Ticks < -NumberOfDaysBCE)
                throw new ArgumentException("The instant is before the begin of the calendar.", "instant");

            // 3 year leap cycle from 45 BC to 9 BC
            if (instant.Ticks < -6 * NumberOfDaysInYear)
            {
                // the value must be positive and the leap year must be the third year
                return ComputeDatePartWithLeapCycle(instant.Ticks + NumberOfDaysBCE + 2 * NumberOfDaysInYear, DatePart.Day, LeapYearCycleBCE) + 1;

                // return Convert.ToInt32(Math.Floor((NumberOfDaysInLeapCycleBC - 1.0) / NumberOfDaysInLeapCycleBC * (instant.Ticks + 6 * MinNumberOfDaysInYear) / MinNumberOfDaysInYear)) - 6;
            }
            // no leap years
            if (instant.Ticks < 7 * NumberOfDaysInYear)
            {
                // the value must be positive
                return ComputeDatePart(instant.Ticks + 6 * NumberOfDaysInYear, DatePart.Day) + 1;
            }

            // 4 year leap cycle from 8 AD
            return ComputeDatePartWithLeapCycle(instant.Ticks - 4 * NumberOfDaysInYear, DatePart.Day, LeapYearCycleCE) + 1; 
        }

        /// <summary>
        /// Returns the calendar day for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The day of year of the instant in the current calendar.</returns>
        public override Int64 GetDayOfYear(Instant instant)
        {
            // 3 year leap cycle from 45 BC to 9 BC
            if (instant.Ticks < -6 * NumberOfDaysInYear)
            {
                // the value must be positive and the leap year must be the third year
                return ComputeDatePartWithLeapCycle(instant.Ticks + NumberOfDaysBCE + 2 * NumberOfDaysInYear, DatePart.DayOfYear, LeapYearCycleBCE) + 1;

                // return Convert.ToInt32(Math.Floor((NumberOfDaysInLeapCycleBC - 1.0) / NumberOfDaysInLeapCycleBC * (instant.Ticks + 6 * MinNumberOfDaysInYear) / MinNumberOfDaysInYear)) - 6;
            }
            // no leap years
            if (instant.Ticks < 7 * NumberOfDaysInYear)
            {
                // the value must be positive
                return ComputeDatePart(instant.Ticks + 6 * NumberOfDaysInYear, DatePart.DayOfYear) + 1;
            }

            // 4 year leap cycle from 8 AD
            return ComputeDatePartWithLeapCycle(instant.Ticks - 4 * NumberOfDaysInYear, DatePart.DayOfYear, LeapYearCycleCE) + 1; 
        }

        /// <summary>
        /// Returns the number of days for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of days for the duration in the current calendar.</returns>
        public override Int64 GetDays(Duration duration) 
        {
            return ComputeDatePartWithLeapCycle(duration.Ticks, DatePart.Day, LeapYearCycleCE);
        }

        /// <summary>
        /// Returns the total number of months for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of months for the duration in the current calendar.</returns>
        public override Int64 GetTotalMonths(Duration duration)
        {
            return ComputeDatePartWithLeapCycle(duration.Ticks, DatePart.TotalMonths, LeapYearCycleCE);
        }

        /// <summary>
        /// Returns the total number of days for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of days for the duration in the current calendar.</returns>
        public override Int64 GetTotalDays(Duration duration) 
        { 
            return duration.Ticks; 
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
        /// The year is before the begin of the calendar.
        /// or
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
            if (year < FirstYear)
                throw new ArgumentOutOfRangeException("year", "The year is before the begin of the calendar.");
            if (month < 1)
                throw new ArgumentOutOfRangeException("month", "The month is less than 1.");
            if (month > NumberOfMonthsInYear)
                throw new ArgumentOutOfRangeException("month", "The month is greater than the number of months in the year.");
            if (day < 1)
                throw new ArgumentOutOfRangeException("day", "The day is less than 1.");
            if (day > (IsLeapYear(year) ? DaysToMonthInLeapYear[month] - DaysToMonthInLeapYear[month - 1] : DaysToMonth[month] - DaysToMonth[month - 1]))
                throw new ArgumentOutOfRangeException("day", "The day is greater than the number of days in the month.");

            // source: ISO 19108:2002, Geographic Information - Temporal schema, page 44

            Int64 julianDay = 1461 * (year + 4716 - (14 - month) / 12) / 4; // compute year
            julianDay += (153 * ((month + 9) % 12) + 2) / 5; // compute month
            julianDay += day - 1 - 1401; // compute day

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
            // source: ISO 19108:2002, Geographic Information - Temporal schema, page 44

            Int64 julianDay = date.Instant.Ticks / Clocks.UTC.SecondsPerDay;
            Int64 t = ((4 * (julianDay + 1401) + 3) % 1461) / 4;
            Int64 day = ((5 * t + 2) % 153) / 5 + 1;
            Int64 month = (5 * t + 2 / 153 + 2) % 12 + 1;
            Int64 year = julianDay + 3315 + (14 - month) / 12;

            return GetDate(year, month, day);
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
                new CalendarEra("AEGIS::813303", "Julian era", this, "Calendar reform by Julius Caesar.", GetInstant(ReferenceYear, 1, 1), GetInstant(FirstYear, 1, 1), Instant.MaxValue) 
            };
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the date part.
        /// </summary>
        /// <param name="ticks">The ticks.</param>
        /// <param name="part">The part of the date to be computed.</param>
        /// <returns>The zero-based value of the specified date part.</returns>
        protected Int64 ComputeDatePart(Int64 ticks, DatePart part)
        {
            Int64 year = ticks / NumberOfDaysInYear;

            if (part == DatePart.Year) return year;

            Int64 daysInYear = ticks - year * NumberOfDaysInYear;

            if (part == DatePart.DayOfYear) return daysInYear;

            Int64 month = (daysInYear >> 5) + 1; // due to month having less than 32 days
            while (daysInYear < DaysToMonth[month]) month--;

            switch (part)
            {
                case DatePart.Month:
                    return month;
                case DatePart.TotalMonths:
                    return year * NumberOfDaysInYear + month;
                case DatePart.Day:
                    return (daysInYear - DaysToMonth[month]);
                default:
                    return ticks;
            }
        }

        /// <summary>
        /// Computes the date part by taking leap cycles into account.
        /// </summary>
        /// <param name="ticks">The ticks.</param>
        /// <param name="part">The part of the date to be computed.</param>
        /// <param name="minDaysInYear">The number days in a year without leap day.</param>
        /// <param name="yearsInLeapCycle">The years in leap cycle.</param>
        /// <returns>The zero-based value of the specified date part.</returns>
        protected Int64 ComputeDatePartWithLeapCycle(Int64 ticks, DatePart part, Int32 yearsInLeapCycle)
        {
            // source: System.Globalization.JulianCalendar source code

            Int64 daysInLeapCycle = NumberOfDaysInYear * yearsInLeapCycle + 1;
            Int64 yearPeriods = ticks / daysInLeapCycle;
            Int64 daysInPeriod = ticks - yearPeriods * daysInLeapCycle;
            Int64 yearsInPeriod = daysInPeriod / NumberOfDaysInYear;
            Boolean isLeapYear = (yearsInLeapCycle - Math.Abs(yearsInPeriod)) <= 1;

            // check for overflow on the last day of the leap year
            if (yearsInPeriod == yearsInLeapCycle) 
                yearsInPeriod--; 

            if (part == DatePart.Year) return yearPeriods * yearsInLeapCycle + yearsInPeriod;

            Int64 daysInYear = daysInPeriod - yearsInPeriod * NumberOfDaysInYear;

            if (part == DatePart.DayOfYear) return daysInYear;

            Int32[] daysArray = isLeapYear ? DaysToMonthInLeapYear : DaysToMonth;

            Int64 month = (daysInYear >> 5) + 1; // due to month having less than 32 days
            while (daysInYear < daysArray[month]) month--;

            switch (part)
            {
                case DatePart.Month:
                    return month;
                case DatePart.TotalMonths:
                    return (yearPeriods * yearsInLeapCycle + yearsInPeriod) * NumberOfMonthsInYear + month;
                case DatePart.Day:
                    return (daysInYear - daysArray[month]);
                default:
                    return ticks;
            }
        }

        #endregion 
    }
}
