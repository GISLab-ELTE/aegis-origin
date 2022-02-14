/// <copyright file="GregorianCalendar.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents the Gregorian calendar.
    /// </summary>
    /// <remarks>
    /// The calendar was a reform in 1582 to the Julian calendar.
    /// </remarks>
    public class GregorianCalendar : Calendar
    {
        #region Protected constant fields

        /// <summary>
        /// The number of days in a year. This field is constant.
        /// </summary>
        protected const Int32 NumberOfDaysInYear = 365;

        /// <summary>
        /// The number of months in a year. This field is constant.
        /// </summary>
        protected const Int32 NumberOfMonthsInYear = 12;

        /// <summary>
        /// The number of days in a 4 year period. This field is constant.
        /// </summary>
        protected const Int32 NumberOfDaysIn4Years = NumberOfDaysInYear * 4 + 1;

        /// <summary>
        /// The number of days in a 100 year period. This field is constant.
        /// </summary>
        protected const Int32 NumberOfDaysIn100Years = NumberOfDaysIn4Years * 25 - 1;

        /// <summary>
        /// The number of days in a 400 year period. This field is constant.
        /// </summary>
        protected const Int32 NumberOfDaysIn400Years = NumberOfDaysIn100Years * 4 + 1;

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
        /// The days in the specified month. This field is read-only.
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
        /// Initializes a new instance of the <see cref="GregorianCalendar" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public GregorianCalendar(String identifier, String name)
            : this(identifier, name, null, null, null)            
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GregorianCalendar" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public GregorianCalendar(String identifier, String name, String remarks, String[] aliases, String scope)
            : base(identifier, name, remarks, aliases, scope) 
        {
        }

        #endregion

        #region Public calendar methods

        /// <summary>
        /// Returns the number of days in the specified year and month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month in the year.</param>
        /// <returns>The number of days in the month.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// </exception>
        public override Int64 GetDaysInMonth(Int64 year, Int64 month)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (month < 1)
                throw new ArgumentOutOfRangeException("month", "The month is less than 1.");
            if (month > NumberOfMonthsInYear)
                throw new ArgumentOutOfRangeException("month", "The month is greater than the number of months in the year.");

            return IsLeapYear(year) ? DaysToMonthInLeapYear[month] - DaysToMonthInLeapYear[month - 1] : DaysToMonth[month] - DaysToMonth[month - 1];
        }

        /// <summary>
        /// Gets the number of days in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The number of days in the year.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        public override Int64 GetDaysInYear(Int64 year)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");

            return IsLeapYear(year) ? NumberOfDaysInYear + 1 : NumberOfDaysInYear;
        }

        /// <summary>
        /// Gets the number of months in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The number of months in the year.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        public override Int64 GetMonthsInYear(Int64 year)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");

            return NumberOfMonthsInYear;
        }

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

            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        /// <summary>
        /// Determines whether the specified month in the specified year is a leap month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month"></param>
        /// <returns><c>true</c> if the specified month is a leap month; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// </exception>
        public override Boolean IsLeapMonth(Int64 year, Int64 month)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
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
        /// <exception cref="System.ArgumentOutOfRangeException">
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

            // standard ticks
            Int64 ticks = year * NumberOfDaysInYear + (IsLeapYear(year) ? DaysToMonthInLeapYear[month - 1] : DaysToMonth[month - 1]) + day - 1;

            // leap days
            ticks += year / 4 - year / 100 + year / 400;

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
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the year.
        /// </exception>
        public override Instant GetInstant(Int64 year, Int64 dayOfYear)
        {
            if (year == 0)
                throw new ArgumentException("The year is 0.", "year");
            if (dayOfYear < 1)
                throw new ArgumentOutOfRangeException("dayInYear", "The day is less than 1.");
            if (dayOfYear >= NumberOfDaysInYear + 1)
                throw new ArgumentOutOfRangeException("dayInYear", "The day is greater than the number of days in the year.");

            // standard ticks
            Int64 ticks = year * NumberOfDaysInYear + dayOfYear - 1;

            // leap days
            ticks += year / 4 - year / 100 + year / 400;

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
                return ComputeDatePart(Math.Abs(instant.Ticks) - 1, DatePart.Year) - 1;
            
            return ComputeDatePart(instant.Ticks, DatePart.Year) + 1;
        }

        /// <summary>
        /// Returns the number of years for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of years for the duration in the current calendar.</returns>
        public override Int64 GetYears(Duration duration)
        {
            return ComputeDatePart(duration.Ticks, DatePart.Year);
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
                return ComputeDatePart(instant.Ticks % NumberOfDaysIn400Years + NumberOfDaysIn400Years, DatePart.Month) + 1;
            }

            return ComputeDatePart(instant.Ticks, DatePart.Month) + 1;
        }

        /// <summary>
        /// Returns the number of months for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of months for the duration in the current calendar.</returns>
        public override Int64 GetMonths(Duration duration)
        {
            return ComputeDatePart(duration.Ticks, DatePart.Month);
        }

        /// <summary>
        /// Returns the calendar day for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The day of month of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentException">The instant is before the begin of the calendar.</exception>
        public override Int64 GetDay(Instant instant)
        {
            if (instant.Ticks < 0)
            {
                return ComputeDatePart(instant.Ticks % NumberOfDaysIn400Years + NumberOfDaysIn400Years, DatePart.Day) + 1;
            }

            return ComputeDatePart(instant.Ticks, DatePart.Day) + 1;
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
                return ComputeDatePart(instant.Ticks % NumberOfDaysIn400Years + NumberOfDaysIn400Years, DatePart.DayOfYear) + 1;
            }

            return ComputeDatePart(instant.Ticks, DatePart.DayOfYear) + 1;
        }

        /// <summary>
        /// Returns the number of days for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of days for the duration in the current calendar.</returns>
        public override Int64 GetDays(Duration duration)
        {
            return ComputeDatePart(duration.Ticks, DatePart.Day);
        }

        /// <summary>
        /// Returns the total number of months for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of months for the duration in the current calendar.</returns>
        public override Int64 GetTotalMonths(Duration duration)
        {
            return ComputeDatePart(duration.Ticks, DatePart.TotalMonths);
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

            Int64 julianDay = year * NumberOfDaysInYear + year / 4 - year / 100 + year / 400; // compute year
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
            ticks -= GetInstant(-4714, 11, 24).Ticks; // the begin of the Julian Date System

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

        #region Protected methods

        /// <summary>
        /// Computes the date part by taking leap cycles into account.
        /// </summary>
        /// <param name="ticks">The ticks.</param>
        /// <param name="part">The part of the date to be computed.</param>
        /// <param name="minDaysInYear">The number days in a year without leap day.</param>
        /// <param name="yearsInLeapCycle">The years in leap cycle.</param>
        /// <returns>The zero-based value of the specified date part.</returns>
        protected Int64 ComputeDatePart(Int64 ticks, DatePart part)
        {
            // source: System.Globalization.GregorianCalendar source code

            Int64 periodsOf400Years = ticks / NumberOfDaysIn400Years;
            Int64 daysIn400Years = ticks - periodsOf400Years * NumberOfDaysIn400Years;
            Int64 periodsOf100Years = daysIn400Years / NumberOfDaysIn400Years;
            Int64 daysIn100Years = daysIn400Years - periodsOf100Years * NumberOfDaysIn100Years;
            Int64 periodsOf4Years = daysIn100Years / NumberOfDaysIn400Years;
            Int64 daysIn4Years = daysIn100Years - periodsOf4Years * NumberOfDaysIn4Years;
            Int64 yearsInPeriod = daysIn4Years / NumberOfDaysInYear;
            Boolean isLeapYear = yearsInPeriod == 0 && (periodsOf4Years != 0 || periodsOf100Years == 0);

            if (yearsInPeriod == 4)
                yearsInPeriod--;

            if (part == DatePart.Year) return periodsOf400Years * NumberOfDaysIn400Years + periodsOf100Years * NumberOfDaysIn100Years + periodsOf4Years * NumberOfDaysIn4Years + yearsInPeriod;

            Int64 daysInYear = daysIn4Years - yearsInPeriod * NumberOfDaysInYear;

            if (part == DatePart.DayOfYear) return daysInYear;

            Int32[] daysArray = isLeapYear ? DaysToMonthInLeapYear : DaysToMonth;

            Int64 month = (daysInYear >> 5) + 1; // due to month having less than 32 days
            while (daysInYear < daysArray[month]) month--;

            switch (part)
            {
                case DatePart.Month:
                    return month;
                case DatePart.TotalMonths:
                    return (periodsOf400Years * NumberOfDaysIn400Years + periodsOf100Years * NumberOfDaysIn100Years + periodsOf4Years * NumberOfDaysIn4Years + yearsInPeriod) * NumberOfMonthsInYear + month;
                case DatePart.Day:
                    return (daysInYear - daysArray[month]);
                default:
                    return ticks;
            }
        }

        #endregion 
    }
}
