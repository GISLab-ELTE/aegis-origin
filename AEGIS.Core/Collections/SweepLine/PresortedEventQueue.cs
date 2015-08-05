/// <copyright file="PresortedEventQueue.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a presorted event queue used by Sweep lines.
    /// </summary>
    public sealed class PresortedEventQueue
    {
        #region Private fields

        /// <summary>
        /// The coordinate comparer.
        /// </summary>
        private readonly IComparer<Coordinate> _comparer;

        /// <summary>
        /// The list of endpoint events.
        /// </summary>
        private readonly List<EndPointEvent> _eventList;

        /// <summary>
        /// The current event index.
        /// </summary>
        private Int32 _eventIndex;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PresortedEventQueue" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public PresortedEventQueue(IList<Coordinate> source)
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _comparer = new CoordinateComparer();
            _eventList = new List<EndPointEvent>(2 * source.Count);
            _eventIndex = 0;

            Int32 compare;
            for (Int32 i = 0; i < source.Count - 1; i++)
            {
                var firstEvent = new EndPointEvent { Edge = i, Vertex = source[i] };
                var secondEvent = new EndPointEvent { Edge = i, Vertex = source[i + 1] };

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

                _eventList.Add(firstEvent);
                _eventList.Add(secondEvent);
            }
            _eventList.Sort(new EventComparer());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresortedEventQueue" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public PresortedEventQueue(IEnumerable<IList<Coordinate>> source)
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _comparer = new CoordinateComparer();
            _eventIndex = 0;
            _eventList = new List<EndPointEvent>();

            Int32 index = 0, compare;
            foreach (IList<Coordinate> coordinateList in source)
            {
                if (coordinateList == null || coordinateList.Count < 2)
                    continue;

                for (Int32 i = 0; i < coordinateList.Count - 1; i++)
                {
                    var firstEvent = new EndPointEvent { Edge = index, Vertex = coordinateList[i] };
                    var secondEvent = new EndPointEvent { Edge = index, Vertex = coordinateList[i + 1] };

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
                    _eventList.Add(firstEvent);
                    _eventList.Add(secondEvent);

                    index++;
                }
                index++;
            }
            _eventList.Sort(new EventComparer());
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Retrieves the next event from the queue.
        /// </summary>
        /// <returns>The next event in the queue.</returns>
        public EndPointEvent Next()
        {
            return (_eventIndex < _eventList.Count) ? _eventList[_eventIndex++] : null;
        }

        #endregion
    }
}
