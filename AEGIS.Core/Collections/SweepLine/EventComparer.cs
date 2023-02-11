// <copyright file="EventComparer.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Represents a comparer for <see cref="Event"/> instances.
    /// </summary>
    /// <author>Máté Cserép</author>
    public sealed class EventComparer : IComparer<Event>
    {
        #region Private fields
        /// <summary>
        /// Stores an inner <see cref="CoordinateComparer" /> instance.
        /// </summary>
        private readonly CoordinateComparer _coordinateComparer = new CoordinateComparer();

        #endregion

        #region IComparer methods

        /// <summary>
        /// Compares two <see cref="Event"/> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <remarks>
        /// Events primarily compared by their vertex coordinate, secondarily by their type.
        /// </remarks>
        /// <param name="x">The first <see cref="Event"/> to compare.</param>
        /// <param name="y">The second <see cref="Event"/> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
        public Int32 Compare(Event x, Event y)
        {
            Int32 result = _coordinateComparer.Compare(x.Vertex, y.Vertex);
            if (result != 0) return result;

            if (x is EndPointEvent && y is EndPointEvent)
            {
                EndPointEvent ex = (EndPointEvent)x;
                EndPointEvent ey = (EndPointEvent)y;

                result = ex.Type.CompareTo(ey.Type);
                if (result == 0)
                    result = ex.Edge.CompareTo(ey.Edge);
            }
            else if (x is IntersectionEvent && y is IntersectionEvent)
            {
                IntersectionEvent ix = (IntersectionEvent)x;
                IntersectionEvent iy = (IntersectionEvent)y;

                if (result == 0) result = _coordinateComparer.Compare(ix.Below.LeftCoordinate, iy.Below.LeftCoordinate);
                if (result == 0) result = _coordinateComparer.Compare(ix.Above.LeftCoordinate, iy.Above.LeftCoordinate);
                if (result == 0) result = _coordinateComparer.Compare(ix.Below.RightCoordinate, iy.Below.RightCoordinate);
                if (result == 0) result = _coordinateComparer.Compare(ix.Above.RightCoordinate, iy.Above.RightCoordinate);
                if (result == 0) result = ix.Below.Edge.CompareTo(iy.Below.Edge);
                if (result == 0) result = ix.Above.Edge.CompareTo(iy.Above.Edge);
                if (result == 0) result = ix.IsClose.CompareTo(iy.IsClose);
            }
            else if (x is EndPointEvent && y is IntersectionEvent)
            {
                result = ((EndPointEvent)x).Type == EventType.Left ? -1 : 1;
            }
            else if (y is EndPointEvent && x is IntersectionEvent)
            {
                result = ((EndPointEvent)y).Type == EventType.Left ? 1 : -1;
            }
            return result;
        }

        #endregion
    }
}