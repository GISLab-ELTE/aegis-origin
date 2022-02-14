/// <copyright file="JulianCalendarTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Temporal;
using ELTE.AEGIS.Temporal.Reference;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Core.Temporal.Reference
{
    [TestFixture]
    public class JulianCalendarTest
    {
        private Calendar _calendar;
        private Int32[] _daysInMonth;
        private Int32[] _daysInMonthInLeapYear;

        [SetUp]
        public void SetUp()
        {
            _calendar = Calendars.JulianCalendar;
            _daysInMonth = new Int32[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            _daysInMonthInLeapYear = new Int32[] { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        }

        [TestCase]
        public void JulianCalendarInstantTest()
        {
            Int32 ticks = -1;
            Int32 year = -1, month = 12, dayOfYear = 365, dayOfMonth = 31;

            do
            {
                Instant instant = new Instant(ticks);

                Assert.AreEqual(year, _calendar.GetYear(instant));
                Assert.AreEqual(month, _calendar.GetMonth(instant));
                Assert.AreEqual(dayOfYear, _calendar.GetDayOfYear(instant));
                Assert.AreEqual(dayOfMonth, _calendar.GetDay(instant));
                Assert.AreEqual(instant, _calendar.GetInstant(year, month, dayOfMonth));

                ticks--;
                dayOfYear--;
                dayOfMonth--;

                if (dayOfYear == 0)
                {
                    year--;
                    month = 12;
                    dayOfYear = (year < -6 && year % 3 == 0) ? 366 : 365;
                    dayOfMonth = (year < -6 && year % 3 == 0) ? _daysInMonthInLeapYear[month] : _daysInMonth[month];
                }
                else if (dayOfMonth == 0)
                {
                    month--;
                    dayOfMonth = (year < -6 && year % 3 == 0) ? _daysInMonthInLeapYear[month] : _daysInMonth[month];
                }

            } while (year >= -45);


            ticks = 0;
            year = 1; 
            month = 1; 
            dayOfYear = 1; 
            dayOfMonth = 1;

            do
            {
                Instant instant = new Instant(ticks);

                Assert.AreEqual(year, _calendar.GetYear(instant));
                Assert.AreEqual(month, _calendar.GetMonth(instant));
                Assert.AreEqual(dayOfYear, _calendar.GetDayOfYear(instant));
                Assert.AreEqual(dayOfMonth, _calendar.GetDay(instant));
                Assert.AreEqual(instant, _calendar.GetInstant(year, month, dayOfMonth));

                ticks++;
                dayOfYear++;
                dayOfMonth++;

                if (dayOfYear == ((year > 4 && year % 4 == 0) ? 367 : 366))
                {
                    year++;
                    month = 1;
                    dayOfYear = 1;
                    dayOfMonth = 1;
                }
                else if (dayOfMonth == ((year > 4 && year % 4 == 0) ? _daysInMonthInLeapYear[month] : _daysInMonth[month]) + 1)
                {
                    month++;
                    dayOfMonth =1;
                }
            } while (year < 100);
        }
    }
}
