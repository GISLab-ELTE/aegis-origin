/// <copyright file="TemporalCoordinate.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a position in a temporal coordinate system.
    /// </summary>
    public class TemporalCoordinate : TemporalPosition
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Positioning.TemporalCoordinate" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public TemporalCoordinate(Instant instant, TemporalCoordinateSystem referenceSystem, IMetadataCollection metadata)
            : base(instant, referenceSystem, metadata)
        {
        }

        #endregion
    }
}
