// <copyright file="CalendarDate.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using ELTE.AEGIS.Temporal.Reference;

namespace ELTE.AEGIS.Temporal.Positioning
{
    /// <summary>
    /// Represents a date in a specified calendar.
    /// </summary>
    public class CalendarDate : TemporalPosition
    {
        #region Public properties

        /// <summary>
        /// Gets the calendar era of the date.
        /// </summary>
        /// <value>The era according to the associated calendar.</value>
        public CalendarEra Era { get { return (ReferenceSystem as Calendar).GetEra(_instant); } }

        /// <summary>
        /// Gets the year component of the date.
        /// </summary>
        /// <value>The year according to the associated calendar.</value>
        public Int64 Year { get { return (ReferenceSystem as Calendar).GetYear(_instant); } }

        /// <summary>
        /// Gets the month component of the date.
        /// </summary>
        /// <value>The month according to the associated calendar.</value>
        public Int64 Month { get { return (ReferenceSystem as Calendar).GetMonth(_instant); } }

        /// <summary>
        /// Gets the day component of the date.
        /// </summary>
        /// <value>The day according to the associated calendar.</value>
        public Int64 Day { get { return (ReferenceSystem as Calendar).GetDay(_instant); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarDate" /> class.
        /// </summary>
        /// <param name="instant">The instant of the date.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">referenceSystem;The reference system is null.</exception>
        public CalendarDate(Instant instant, Calendar referenceSystem, IMetadataCollection metadata) :
            base(instant, referenceSystem, metadata)
        {         
        }

        #endregion
    }
}
