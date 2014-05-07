/// <copyright file="Event.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Defines the event types.
    /// </summary>
    public enum EventType { Intersection, Left, Right }

    /// <summary>
    /// Represents an event.
    /// </summary>
    public class Event : IComparable<Event>
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

        /// <summary>
        /// Gets or sets the polygon vertex associated with the event.
        /// </summary>
        public Coordinate Vertex { get; set; }

        #endregion

        #region IComparable methods

        /// <summary>
        /// Compares the current <see cref="Event" /> with another <see cref="Event" />.
        /// </summary>
        /// <param name="other">An event to compare with this event.</param>
        /// <returns>A value that indicates the relative order of the events being compared.</returns>
        public Int32 CompareTo(Event other)
        {
            if (Vertex.X > other.Vertex.X) return 1;
            if (Vertex.X < other.Vertex.X) return (-1);
            if (Vertex.Y > other.Vertex.Y) return 1;
            if (Vertex.Y < other.Vertex.Y) return (-1);
            return 0;
        }

        #endregion
    }
}
