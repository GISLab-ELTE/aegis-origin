// <copyright file="Event.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Defines the endpoint event types.
    /// </summary>
    /// <author>Máté Cserép</author>
    public enum EventType { Left, Right }

    /// <summary>
    /// Represents an event.
    /// </summary>
    /// <author>Máté Cserép</author>
    public class Event
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the polygon vertex associated with the event.
        /// </summary>
        public Coordinate Vertex { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents an endpoint event.
    /// </summary>
    /// <author>Máté Cserép</author>
    public class EndPointEvent : Event
    {
        #region Public fields

        /// <summary>
        /// Gets or sets the polygon edge associated with the event.
        /// </summary>
        public Int32 Edge { get; set; }

        /// <summary>
        /// Gets or sets the event types.
        /// </summary>
        public EventType Type { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents an intersection event.
    /// </summary>
    /// <author>Máté Cserép</author>
    public class IntersectionEvent : Event
    {
        #region Public fields

        /// <summary>
        /// Gets or sets the below <see cref="SweepLineSegment" /> instance intersecting at this event.
        /// </summary>
        public SweepLineSegment Below { get; set; }

        /// <summary>
        /// Gets or sets the above <see cref="SweepLineSegment" /> instance intersecting at this event.
        /// </summary>
        public SweepLineSegment Above { get; set; }

        /// <summary>
        /// Gets or sets whether the event is a close point for the intersection.
        /// </summary>
        public Boolean IsClose { get; set; }

        #endregion
    }
}
