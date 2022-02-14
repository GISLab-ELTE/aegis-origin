/// <copyright file="Calendar.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Temporal.Positioning;

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a calendar.
    /// </summary>
    public abstract class Calendar : TemporalReferenceSystem
    {
        #region Protected types

        /// <summary>
        /// Defines the parts of a date.
        /// </summary>
        protected enum DatePart { Year, Month, Day, DayOfYear, TotalMonths, TotalDays }

        #endregion

        #region Protected fields

        protected CalendarEra[] _eras;
        protected String[] _monthNames;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the beginning of the period for which the calendar was used.
        /// </summary
        /// <value>The beginning date of the period for which the calendar was used expressed as a date in the given calendar.</value>
        public CalendarDate BeginOfUse { get { if (_eras == null) ComputeEras(); return _eras[0].BeginOfUse; } }

        /// <summary>
        /// Gets the end of the period for which the calendar was used.
        /// </summary
        /// <value>The ending date of the period for which the calendar was used expressed as a date in the given calendar.</value>
        public CalendarDate EndOfUse { get { if (_eras == null) ComputeEras(); return _eras[_eras.Length - 1].EndOfUse; } }

        /// <summary>
        /// Gets the list of eras in the calendar.
        /// </summary>
        /// <value>A read-only list of eras in the calendar.</value>
        public IList<CalendarEra> Eras { get { if (_eras == null) ComputeEras(); return Array.AsReadOnly(_eras); } }

        /// <summary>
        /// Gets the time basis of the calendar.
        /// </summary>
        /// <value>The clock that serves as a basis to the calendar.</value>
        public abstract Clock TimeBasis { get; }
             
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Calendar" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="eras">The eras.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected Calendar(String identifier, String name, String remarks, String[] aliases, String scope)
            : base(identifier, name, remarks, aliases, scope) 
        {
        }

        #endregion

        #region Public methods

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
        /// The year is after the end of the calendar.
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
        public abstract Int64 GetDaysInMonth(Int64 year, Int64 month);

        /// <summary>
        /// Gets the number of days in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The number of days in the year.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The year is after the end of the calendar.
        /// </exception>
        public abstract Int64 GetDaysInYear(Int64 year);

        /// <summary>
        /// Gets the number of months in the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The number of months in the year.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The year is after the end of the calendar.
        /// </exception>
        public abstract Int64 GetMonthsInYear(Int64 year);

        /// <summary>
        /// Determines whether the specified year in the current era is a leap year. 
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns><c>true</c> if the specified year is a leap year; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The year is after the end of the calendar.
        /// </exception>
        public abstract Boolean IsLeapYear(Int64 year);

        /// <summary>
        /// Determines whether the specified month in the specified year is a leap month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="monthInYear">The month in the year.</param>
        /// <returns><c>true</c> if the specified month is a leap month; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The year is after the end of the calendar.
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
        public abstract Boolean IsLeapMonth(Int64 year, Int64 month);

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
        /// The year is after the end of the calendar.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the year.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The specified date is before the begin of use.
        /// or
        /// The specified date is after the end of use.
        /// </exception>
        public abstract Boolean IsLeapDay(Int64 year, Int64 dayInYear);

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
        /// The year is after the end of the calendar.
        /// or
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the month.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The specified date is before the begin of use.
        /// or
        /// The specified date is after the end of use.
        /// </exception>
        public abstract Boolean IsLeapDay(Int64 year, Int64 month, Int64 day);

        /// <summary>
        /// Determines whether the specified instant is valid.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns><c>true</c> if the instant is valid within the calendar; otherwise, <c>false</c>.</returns>
        public Boolean IsValid(Instant instant) { return instant >= BeginOfUse.Instant && instant <= EndOfUse.Instant; }

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
        /// The year is after the end of the calendar.
        /// or
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the month.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The specified date is before the begin of use.
        /// or
        /// The specified date is after the end of use.
        /// </exception>
        public abstract Instant GetInstant(Int64 year, Int64 month, Int64 day);

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
        /// The year is after the end of the calendar.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the year.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The specified date is before the begin of use.
        /// or
        /// The specified date is after the end of use.
        /// </exception>
        public abstract Instant GetInstant(Int64 year, Int64 dayOfYear);

        /// <summary>
        /// Computes a calendar date.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The instant computed from the year, month and day values.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The year is after the end of the calendar.
        /// or
        /// The month is less than 1.
        /// or
        /// The month is greater than the number of months in the year.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the month.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The specified date is before the begin of use.
        /// or
        /// The specified date is after the end of use.
        /// </exception>
        public CalendarDate GetDate(Int64 year, Int64 month, Int64 day)
        {
            return new CalendarDate(GetInstant(year, month, day), this, null);
        }

        /// <summary>
        /// Computes a calendar date.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="dayOfYear">The day of year.</param>
        /// <returns>The instant computed from the year and day of year values.</returns>
        /// <exception cref="System.ArgumentException">The year is 0.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The year is before the begin of the calendar.
        /// or
        /// The year is after the end of the calendar.
        /// or
        /// The day is less than 1.
        /// or
        /// The day is greater than the number of days in the year.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The specified date is before the begin of use.
        /// or
        /// The specified date is after the end of use.
        /// </exception>
        public CalendarDate GetDate(Int64 year, Int64 dayOfYear)
        {
            return new CalendarDate(GetInstant(year, dayOfYear), this, null);
        }

        /// <summary>
        /// Returns the calendar date for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The date of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The instant is before the begin of the calendar.
        /// or
        /// The instant is after the end of the calendar.
        /// </exception>
        public CalendarDate GetDate(Instant instant)
        {
            if (instant < BeginOfUse.Instant)
                throw new ArgumentOutOfRangeException("instant", "The instant is before the begin of the calendar.");
            if (instant > EndOfUse.Instant)
                throw new ArgumentOutOfRangeException("instant", "The instant is after the end of the calendar.");

            return new CalendarDate(instant, this, null);
        }

        /// <summary>
        /// Returns the era for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The era of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The instant is before the begin of the calendar.
        /// or
        /// The instant is after the end of the calendar.
        /// </exception>
        public CalendarEra GetEra(Instant instant)
        {
            if (_eras == null)
                ComputeEras();

            if (instant < _eras[0].BeginOfUse.Instant)
                throw new ArgumentOutOfRangeException("instant", "The instant is before the begin of the calendar.");
            if (instant > _eras[_eras.Length - 1].EndOfUse.Instant)
                throw new ArgumentOutOfRangeException("instant", "The instant is after the end of the calendar.");

            for (Int32 i = 0; i < _eras.Length; i++)
                if (instant >= _eras[i].BeginOfUse.Instant && instant <= _eras[i].EndOfUse.Instant)
                    return _eras[i];

            return null;
        }

        /// <summary>
        /// Returns the calendar year for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The year of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The instant is before the begin of the calendar.
        /// or
        /// The instant is after the end of the calendar.
        /// </exception>
        public abstract Int64 GetYear(Instant instant);

        /// <summary>
        /// Returns the number of years for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of years for the duration in the current calendar.</returns>
        public abstract Int64 GetYears(Duration duration);

        /// <summary>
        /// Returns the calendar month for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The month of year of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The instant is before the begin of the calendar.
        /// or
        /// The instant is after the end of the calendar.
        /// </exception>
        public abstract Int64 GetMonth(Instant instant);

        /// <summary>
        /// Returns the number of months for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of months for the duration in the current calendar.</returns>
        public abstract Int64 GetMonths(Duration duration);

        /// <summary>
        /// Returns the calendar day for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The day of month of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The instant is before the begin of the calendar.
        /// or
        /// The instant is after the end of the calendar.
        /// </exception>
        public abstract Int64 GetDay(Instant instant);

        /// <summary>
        /// Returns the calendar day for an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The day of year of the instant in the current calendar.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The instant is before the begin of the calendar.
        /// or
        /// The instant is after the end of the calendar.
        /// </exception>
        public abstract Int64 GetDayOfYear(Instant instant);

        /// <summary>
        /// Returns the number of days for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of days for the duration in the current calendar.</returns>
        public abstract Int64 GetDays(Duration duration);

        /// <summary>
        /// Returns the total number of months for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of months for the duration in the current calendar.</returns>
        public abstract Int64 GetTotalMonths(Duration duration);

        /// <summary>
        /// Returns the number of days for a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The number of days for the duration in the current calendar.</returns>
        public abstract Int64 GetTotalDays(Duration duration);

        /// <summary>
        /// Returns the Julian Date of a calendar date.
        /// </summary>
        /// <param name="date">The calendar date</param>
        /// <returns>
        /// The Julian Date that matches the specified date.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The date is null.</exception>
        /// <exception cref="System.ArgumentException">The date is not specified in the current calendar.</exception>
        public JulianDate TranformToJulianDate(CalendarDate date)
        {
            if (date == null)
                throw new ArgumentNullException("date", "The date is null.");
            if (!date.ReferenceSystem.Equals(this))
                throw new ArgumentException("The date is not specified in the current calendar.", "date");

            return TranformToJulianDate(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// Returns the Julian Date of a calendar date.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The Julian Date that matches the specified date.</returns>
        public abstract JulianDate TranformToJulianDate(Int64 year, Int64 month, Int64 day);

        /// <summary>
        /// Returns the calendar date for a Julian Date.
        /// </summary>
        /// <param name="date">The Julian Date.</param>
        /// <returns>The calendar date that matches <paramref name="date" />.</returns>
        public abstract CalendarDate TransformFromJulianDate(JulianDate date);

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the eras of the calendar.
        /// </summary>
        protected abstract void ComputeEras();

        #endregion
    }
}
