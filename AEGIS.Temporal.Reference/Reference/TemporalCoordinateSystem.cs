// <copyright file="TemporalCoordinateSystem.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a temporal coordinate system.
    /// </summary>
    public abstract class TemporalCoordinateSystem : TemporalReferenceSystem
    {
        #region Public properties

        /// <summary>
        /// Gets the origin of the coordinate system.
        /// </summary>
        /// <value>The origin of the coordinate system expressed in the Gregorian calendar and UTC clock.</value>
        public DateAndTime Origin { get; private set; }

        /// <summary>
        /// Gets the interval of the coordinate system.
        /// </summary>
        /// <value>The single unit of measure used as the base interval for the scale.</value>
        public UnitOfMeasurement Interval { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporalCoordinateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="origin">The origin of the coordinate system expressed in the Gregorian calendar and UTC clock.</param>
        /// <param name="interval">The interval of the coordinate system.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The interval is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The origin is not expressed in the Gregorian calendar and UTC clock.
        /// or
        /// The interval is not a time measure.
        /// </exception>
        protected TemporalCoordinateSystem(String identifier, String name, String remarks, String[] aliases, String scope, DateAndTime origin, UnitOfMeasurement interval)
            : base(identifier, name, remarks, aliases, scope)
        {
            if (interval == null)
                throw new ArgumentNullException("interval", "The interval is null.");
            if (origin != null && !origin.ReferenceSystem.Equals(CompoundTemporalReferenceSystems.GregorianCalendarUTCClock))
                throw new ArgumentException("The origin is not expressed in the Gregorian calendar and UTC clock.", "origin");
            if (interval.Type != UnitQuantityType.Time)
                throw new ArgumentException("The interval is not a time measure.", "interval");
            Origin = origin;
            Interval = interval;
        }

        #endregion

        #region Public methods

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
        public abstract DateAndTime TransformToDateAndTime(TemporalCoordinate coordinate);

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
        public abstract TemporalCoordinate TransformFromDateAndTime(DateAndTime dateTime);

        #endregion
    }
}
