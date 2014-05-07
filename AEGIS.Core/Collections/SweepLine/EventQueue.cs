/// <copyright file="EventQueue.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Collections;

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Represents an event queue used by Sweep lines.
    /// </summary>
    public class EventQueue
    {
        #region Private fields

        private readonly IComparer<Coordinate> _comparer;
        private readonly Heap<Coordinate, Event> _eventHeap;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventQueue" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public EventQueue(IList<Coordinate> source)
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _comparer = new CoordinateComparer();
            _eventHeap = new Heap<Coordinate, Event>(new CoordinateComparer());

            Int32 compare;
            for (Int32 i = 0; i < source.Count - 1; i++)
            {
                Event firstEvent = new Event { Edge = i, Vertex = source[i] };
                Event secondEvent = new Event { Edge = i, Vertex = source[i + 1] };

                compare = _comparer.Compare(source[i], source[i + 1]);
                if (compare == 0) continue;

                if (compare < 0)
                {
                    firstEvent.Type = EventType.Left;
                    secondEvent.Type = EventType.Right;
                }
                else
                {
                    firstEvent.Type = EventType.Right;
                    secondEvent.Type = EventType.Left;
                }

                _eventHeap.Insert(firstEvent.Vertex, firstEvent);
                _eventHeap.Insert(firstEvent.Vertex, secondEvent);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventQueue" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public EventQueue(IEnumerable<IList<Coordinate>> source)
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _comparer = new CoordinateComparer();
            _eventHeap = new Heap<Coordinate, Event>(new CoordinateComparer());

            Int32 index = 0, compare;
            foreach (IList<Coordinate> coordinateList in source)
            {
                if (coordinateList == null || coordinateList.Count < 2)
                    continue;

                for (Int32 i = 0; i < coordinateList.Count - 1; i++)
                {
                    Event firstEvent = new Event { Edge = index, Vertex = coordinateList[i] };
                    Event secondEvent = new Event { Edge = index, Vertex = coordinateList[i + 1] };

                    compare = _comparer.Compare(coordinateList[i], coordinateList[i + 1]);
                    if (compare == 0) continue;

                    if (compare < 0)
                    {
                        firstEvent.Type = EventType.Left;
                        secondEvent.Type = EventType.Right;
                    }
                    else
                    {
                        firstEvent.Type = EventType.Right;
                        secondEvent.Type = EventType.Left;
                    }
                    _eventHeap.Insert(firstEvent.Vertex, firstEvent);
                    _eventHeap.Insert(firstEvent.Vertex, secondEvent);

                    index++;
                }
                index++;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Retrieves the next event from the queue.
        /// </summary>
        /// <returns>The next event in the queue.</returns>
        public Event Next()
        {
            return (_eventHeap.Count > 0) ? _eventHeap.RemovePeek() : null;
        }

        /// <summary>
        /// Adds an intersection event to the queue.
        /// </summary>
        /// <param name="intersectionEvent">The intersection event.</param>
        /// <exception cref="System.ArgumentNullException">The instersection event is null.</exception>
        /// <exception cref="System.ArgumentException">The event is not an intersection.</exception>
        public void Add(Event intersectionEvent)
        {
            if (intersectionEvent == null)
                throw new ArgumentNullException("intersectionEvent", "The instersection event is null.");
            if (intersectionEvent.Type != EventType.Intersection)
                throw new ArgumentException("The event is not an intersection.", "intersectionEvent");

            _eventHeap.Insert(intersectionEvent.Vertex, intersectionEvent);
        }

        /// <summary>
        /// Determines whether the queue contains a coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the queue contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Coordinate coordinate)
        {
            return (_eventHeap.Contains(coordinate));
        }

        #endregion
    }
}
