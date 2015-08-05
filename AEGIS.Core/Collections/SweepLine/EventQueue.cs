/// <copyright file="EventQueue.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Represents an event queue used by Sweep lines.
    /// </summary>
    public class EventQueue
    {
        #region Private types

        /// <summary>
        /// Represents a heap data structure containing <see cref="Event"/> instances.
        /// </summary>
        private sealed class EventHeap : Heap<Event, Event>
        {
            #region Private fields

            /// <summary>
            /// An inner <see cref="CoordinateComparer" /> instance.
            /// </summary>
            private readonly CoordinateComparer _coordinateComparer = new CoordinateComparer();

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="EventHeap"/> class.
            /// </summary>
            public EventHeap() : base(new EventComparer())
            { }

            #endregion

            #region Public methods

            /// <summary>
            /// Inserts the specified event element into the heap.
            /// </summary>
            /// <param name="element">The event element of the element to insert.</param>
            /// <exception cref="System.ArgumentNullException"><paramref name="element"/> is null.</exception>
            public void Insert(Event element)
            {
                Insert(element, element);
            }

            /// <summary>
            /// Determines whether the <see cref="EventHeap"/> contains any event element with the given coordinate.
            /// </summary>
            /// <param name="position">The coordinate position to locate in the <see cref="EventHeap"/>.</param>
            /// <returns><c>true</c> if the the <see cref="EventHeap"/> contains an event element with the specified position; otherwise <c>false</c>.</returns>
            public Boolean Contains(Coordinate position)
            {
                return this.Any(item => _coordinateComparer.Compare(item.Key.Vertex, position) == 0);
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The coordinate comparer.
        /// </summary>
        private readonly IComparer<Coordinate> _comparer;

        /// <summary>
        /// The event heap.
        /// </summary>
        private readonly EventHeap _eventHeap;

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
            _eventHeap = new EventHeap();

            Int32 compare;
            for (Int32 i = 0; i < source.Count - 1; i++)
            {
                EndPointEvent firstEvent = new EndPointEvent { Edge = i, Vertex = source[i] };
                EndPointEvent secondEvent = new EndPointEvent { Edge = i, Vertex = source[i + 1] };

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

                _eventHeap.Insert(firstEvent);
                _eventHeap.Insert(secondEvent);
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
            _eventHeap = new EventHeap();

            Int32 index = 0, compare;
            foreach (IList<Coordinate> coordinateList in source)
            {
                if (coordinateList == null || coordinateList.Count < 2)
                    continue;

                for (Int32 i = 0; i < coordinateList.Count - 1; i++)
                {
                    EndPointEvent firstEvent = new EndPointEvent { Edge = index, Vertex = coordinateList[i] };
                    EndPointEvent secondEvent = new EndPointEvent { Edge = index, Vertex = coordinateList[i + 1] };

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
                    
                    _eventHeap.Insert(firstEvent);
                    _eventHeap.Insert(secondEvent);

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
        public void Add(IntersectionEvent intersectionEvent)
        {
            if (intersectionEvent == null)
                throw new ArgumentNullException("intersectionEvent", "The intersection event is null.");

            _eventHeap.Insert(intersectionEvent);
        }

        /// <summary>
        /// Determines whether the queue contains an event at the given coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the queue contains an event at <paramref name="coordinate"/>; otherwise <c>false</c>.</returns>
        public Boolean Contains(Coordinate coordinate)
        {
            return _eventHeap.Contains(coordinate);
        }

        /// <summary>
        /// Determines whether the queue contains an intersection event.
        /// </summary>
        /// <param name="intersectionEvent">The intersection event.</param>
        /// <returns><c>true</c> if the queue contains the <paramref name="intersectionEvent"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The instersection event is null.</exception>
        public Boolean Contains(IntersectionEvent intersectionEvent)
        {
            if (intersectionEvent == null)
                throw new ArgumentNullException("intersectionEvent", "The instersection event is null.");

            return _eventHeap.Contains(intersectionEvent);
        }

        #endregion
    }
}
