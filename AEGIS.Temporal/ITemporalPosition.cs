// <copyright file="ITemporalPosition.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Temporal
{
    /// <summary>
    /// Defines general behavior of temporal positions.
    /// </summary>
    public interface ITemporalPosition : IMetadataProvider
    {
        /// <summary>
        /// Gets the instant associated with the temporal position.
        /// </summary>
        /// <value>The instant associated with the temporal position.</value>
        Instant Instant { get; set; }

        /// <summary>
        /// Gets the reference system of the <see cref="Positioning.TemporalPosition" />.
        /// </summary>
        /// <value>The reference system of the temporal position.</value>
        IReferenceSystem ReferenceSystem { get; }

        // <summary>
        /// Occurs when the temporal position is changed.
        /// </summary>
        event EventHandler PositionChanged;
    }
}
