// <copyright file="ModifiedJulianDateSystem.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a coordinate system of modified Julian Dates.
    /// </summary>
    /// <value>
    /// The Modified Julian Date was introduced by the Smithsonian Astrophysical Observatory in 1957 to record the orbit of Sputnik via an IBM 704 (36-bit machine) and using only 18 bits until August 7, 2576. 
    /// Modified Julian Date is the epoch of OpenVMS, using 63-bit date/time postponing the next Y2K campaign to July 31, 31086 02:48:05.47.
    /// Modified Julian Date is defined relative to midnight, rather than noon.
    /// </value>
    public class ModifiedJulianDateSystem : JulianDateSystem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReducedJulianDateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ModifiedJulianDateSystem(String identifier, String name)
            : this(identifier, name, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReducedJulianDateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ModifiedJulianDateSystem(String identifier, String name, String remarks, String[] aliases, String scope)
            : base(identifier, name, remarks, aliases, scope, new DateAndTime(Calendars.GregorianCalendar.GetInstant(1858, 11, 16), Clocks.UTC.GetInstant(0, 0, 0), CompoundTemporalReferenceSystems.GregorianCalendarUTCClock, null))
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

            JulianDate julianDate = new JulianDate(new Instant((coordinate as JulianDate).Instant.Ticks + 2400000 * Clocks.UTC.SecondsPerDay), TemporalCoordinateSystems.JulianDateSystem, null);

            return base.TransformToDateAndTime(julianDate);
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
            JulianDate julianDate = base.TransformFromDateAndTime(dateTime) as JulianDate;

            return new JulianDate(new Instant(julianDate.Instant.Ticks - 2400000 * Clocks.UTC.SecondsPerDay), this, null);
        }

        #endregion
    }
}
