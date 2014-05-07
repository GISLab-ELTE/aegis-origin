/// <copyright file="ClockTime.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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
    /// Represents a time in a specified clock.
    /// </summary>
    public class ClockTime : TemporalPosition
    {
        #region Public properties

        /// <summary>
        /// Gets the hour component of the time.
        /// </summary>
        /// <value>The hour according to the associated clock.</value>
        public Int32 Hour { get { return (ReferenceSystem as Clock).GetHour(_instant); } }

        /// <summary>
        /// Gets the minute component of the time.
        /// </summary>
        /// <value>The minute according to the associated clock.</value>
        public Int32 Minute { get { return (ReferenceSystem as Clock).GetMinute(_instant); } }

        /// <summary>
        /// Gets the second component of the time.
        /// </summary>
        /// <value>The second according to the associated clock.</value>
        public Int32 Second { get { return (ReferenceSystem as Clock).GetSecond(_instant); } }

        /// <summary>
        /// Gets the millisecond component of the time.
        /// </summary>
        /// <value>The millisecond according to the associated clock.</value>
        public Int32 Millisecond { get { return (ReferenceSystem as Clock).GetMilliseconds(_instant); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClockTime" /> class.
        /// </summary>
        /// <param name="instant">The instant of the time.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">referenceSystem;The reference system is null.</exception>
        public ClockTime(Instant instant, Clock referenceSystem, IMetadataCollection metadata) :
            base(instant, referenceSystem, metadata)
        {
        }

        #endregion
    }
}
