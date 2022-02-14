/// <copyright file="TimeZone.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Reference;

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a time zone.
    /// </summary>
    public class TimeZone : IdentifiedObject
    {
        #region Public properties

        /// <summary>
        /// Gets the daylight saving identifier.
        /// </summary>
        /// <value>An identifier used during daylight saving time.</value>
        public String DaylightIdentifier { get; private set; }

        /// <summary>
        /// Gets the daylight saving name.
        /// </summary>
        /// <value>The primary name used during daylight saving time.</value>
        public String DaylightName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the time zone supports daylight saving time.
        /// </summary>
        /// <value><c>true</c> if the time zone supports daylight saving time; otherwise, <c>false</c>.</value>
        public Boolean SupportsDaylightSavingTime { get; private set; }

        /// <summary>
        /// Gets the area of use where the time zone is applicable.
        /// </summary>
        /// <value>The area of use where the time zone is applicable.</value>
        public AreaOfUse AreaOfUse { get; private set; }

        /// <summary>
        /// Gets the offset from the UTC time zone.
        /// </summary>
        /// <value>The offset from the UTC time zone.</value>
        public Duration Offset { get; private set; }

        /// <summary>
        /// Gets the daylight saving offset from the UTC time zone.
        /// </summary>
        /// <value>The offset from the UTC time zone during daylight saving time.</value>
        public Duration DaylightOffset { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZone" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="offset">The offset from the UTC time zone.</param>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        public TimeZone(String identifier, String name, AreaOfUse areaOfUse, Duration offset)
            : this(identifier, name, false, null, null, areaOfUse, offset, offset)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZone" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="supportsDaylightSavingTime">The supports daylight saving time.</param>
        /// <param name="daylightIdentifier">The daylight identifier.</param>
        /// <param name="daylightName">The daylight name.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="offset">The offset from the UTC time zone.</param>
        /// <param name="daylightOffset">The offset from the UTC time zone during daylight saving time.</param>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        public TimeZone(String identifier, String name, Boolean supportsDaylightSavingTime, String daylightIdentifier, String daylightName, AreaOfUse areaOfUse, Duration offset, Duration daylightOffset)
            : base(identifier, name)
        {
            if (areaOfUse == null)
                throw new ArgumentNullException("areaOfUse", "The area of use is null.");

            SupportsDaylightSavingTime = supportsDaylightSavingTime;
            if (SupportsDaylightSavingTime)
            {
                DaylightIdentifier = daylightIdentifier ?? Identifier;
                DaylightName = daylightName ?? Name;
            }

            AreaOfUse = areaOfUse;
            Offset = offset;
            DaylightOffset = daylightOffset;
        }

        #endregion

        #region Static factory methods

        /// <summary>
        /// Creates a time zone from the UTC offset.
        /// </summary>
        /// <param name="offset">The UTC offset.</param>
        /// <returns>The time zone created from the specified UTC offset.</returns>
        public static TimeZone FromOffset(Duration offset)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
