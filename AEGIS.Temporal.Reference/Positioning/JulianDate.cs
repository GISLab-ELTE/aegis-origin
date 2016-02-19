/// <copyright file="JulianDate.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
using ELTE.AEGIS.Temporal.Reference;

namespace ELTE.AEGIS.Temporal.Positioning
{
    /// <summary>
    /// Represents a Julian Date.
    /// </summary>
    public class JulianDate : TemporalCoordinate
    {
        #region Public properties

        /// <summary>
        /// Gets the date component of the Julian Date.
        /// </summary>
        /// <value>The preceding noon plus since the beginning of the Julian Period.</value>
        public Decimal Date { get { return _instant.Ticks / Clocks.UTC.SecondsPerDay; } }

        /// <summary>
        /// Gets the time component of the Julian Date.
        /// </summary>
        /// <value>The fraction of the day.</value>
        public Decimal Time { get { return DateAndTime - Date; } }

        /// <summary>
        /// Gets the date/time representation of the Julian Date.
        /// </summary>
        /// <value>The preceding noon plus the fraction of the day since the beginning of the Julian Period.</value>
        public Decimal DateAndTime { get { return Convert.ToDecimal(_instant.Ticks) / Clocks.UTC.SecondsPerDay; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JulianDate" /> class.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public JulianDate(Instant instant, TemporalCoordinateSystem referenceSystem, IMetadataCollection metadata) : base(instant, referenceSystem, metadata) { }

        #endregion
    }
}
