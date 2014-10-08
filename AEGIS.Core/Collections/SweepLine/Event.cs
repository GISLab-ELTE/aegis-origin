/// <copyright file="Event.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Defines the event types.
    /// </summary>
    public enum EventType { Left, Right }

    /// <summary>
    /// Represents an event.
    /// </summary>
    public class Event : IComparable<Event>
    {
        #region Public fields

        /// <summary>
        /// Gets or sets the polygon vertex associated with the event.
        /// </summary>
        public Coordinate Vertex { get; set; }

        #endregion

        #region Protected fields

        /// <summary>
        /// Stores an inner <see cref="CoordinateComparer"/> instance.
        /// </summary>
        protected readonly CoordinateComparer _coordinateComparer = new CoordinateComparer();

        #endregion

        #region IComparable methods

        /// <summary>
        /// Compares the current <see cref="T:ELTE.AEGIS.Collections.SweepLine.Event"/> with another <see cref="T:ELTE.AEGIS.Collections.SweepLine.Event"/>.
        /// </summary>
        /// <param name="other">An event to compare with this event.</param>
        /// <returns>A value that indicates the relative order of the events being compared.</returns>
        public Int32 CompareTo(Event other)
        {
            return _coordinateComparer.Compare(Vertex, other.Vertex);
        }

        #endregion
    }

    /// <summary>
    /// Represents an end point event.
    /// </summary>
    public class EndPointEvent : Event, IComparable<EndPointEvent>
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

        #region IComparable methods

        /// <summary>
        /// Compares the current <see cref="EndPointEvent"/> with another <see cref="EndPointEvent"/>.
        /// </summary>
        /// <param name="other">An endpoint event to compare with this event.</param>
        /// <returns>A value that indicates the relative order of the events being compared.</returns>
        public Int32 CompareTo(EndPointEvent other)
        {
            Int32 result = base.CompareTo(other);
            if (result == 0)
                result = Type.CompareTo(other.Type);
            if (result == 0)
                result = Edge.CompareTo(other.Edge);
            return result;
        }

        #endregion
    }

    /// <summary>
    /// Represents an intersection point event.
    /// </summary>
    public class IntersectionEvent : Event
    {
        #region Public fields

        /// <summary>
        /// Gets or sets the below <see cref="SweepLineSegment"/> instance intersecting at this event.
        /// </summary>
        public SweepLineSegment Below { get; set; }

        /// <summary>
        /// Gets or sets the above <see cref="SweepLineSegment"/> instance intersecting at this event.
        /// </summary>
        public SweepLineSegment Above { get; set; }

        #endregion

        #region IComparable methods

        /// <summary>
        /// Compares the current <see cref="IntersectionEvent"/> with another <see cref="IntersectionEvent"/>.
        /// </summary>
        /// <param name="other">An intersection event to compare with this event.</param>
        /// <returns>A value that indicates the relative order of the events being compared.</returns>
        public Int32 CompareTo(IntersectionEvent other)
        {
            Int32 result = base.CompareTo(other);
            if (result == 0 && Below.Equals(Above))
                return result;
            
            if (result == 0) result = _coordinateComparer.Compare(Below.LeftCoordinate, other.Below.LeftCoordinate);
            if (result == 0) result = _coordinateComparer.Compare(Above.LeftCoordinate, other.Above.LeftCoordinate);
            if (result == 0) result = _coordinateComparer.Compare(Below.RightCoordinate, other.Below.RightCoordinate);
            if (result == 0) result = _coordinateComparer.Compare(Above.RightCoordinate, other.Above.RightCoordinate);
            return result;
        }

        #endregion
    }
}
