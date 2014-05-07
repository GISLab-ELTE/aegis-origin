/// <copyright file="BentleyOttmannAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
using ELTE.AEGIS.Collections.SweepLine;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents the Bentley-Ottmann Algorithm for determining intersection of line strings.
    /// </summary>
    public partial class BentleyOttmannAlgorithm
    {
        #region Protected fields

        protected EventQueue _eventQueue;
        protected SweepLine _sweepLine;
        protected Boolean _hasResult;
        protected List<Coordinate> _result;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The <see cref="IList{Coordinate}" /> containing the intersection coordinates.</value>
        public IList<Coordinate> Result { get { if (!_hasResult) Compute(); return _result; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyOttmannAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyOttmannAlgorithm(IList<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new EventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyOttmannAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyOttmannAlgorithm(IEnumerable<IList<Coordinate>> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new EventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the intersection of one or more line strings.
        /// </summary>
        public void Compute()
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            _result = new List<Coordinate>();
            Event e = _eventQueue.Next();
            SweepLineSegment segment;
            Dictionary<Coordinate, Tuple<SweepLineSegment, SweepLineSegment>> intersectingSegments = new Dictionary<Coordinate,Tuple<SweepLineSegment,SweepLineSegment>>();
            IList<Coordinate> intersections;

            while (e != null)
            {
                switch (e.Type)
                { 
                    case EventType.Left:
                        segment = _sweepLine.Add(e);
                        if (segment.Above != null && Math.Abs(segment.Edge - segment.Above.Edge) != 1)                            
                        {
                            intersections = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate, segment.Above.LeftCoordinate, segment.Above.RightCoordinate);
                            if (intersections.Count > 0)
                            {
                                Event intersectionEvent = new Event { Type = EventType.Intersection, Edge = 0, Vertex = intersections[0] };
                                _eventQueue.Add(intersectionEvent);
                                intersectingSegments.Add(intersections[0], new Tuple<SweepLineSegment,SweepLineSegment>(segment, segment.Above));
                            }
                        }
                        if (segment.Below != null && Math.Abs(segment.Edge - segment.Below.Edge) != 1)
                        {
                            intersections = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate, segment.Below.LeftCoordinate, segment.Below.RightCoordinate);
                            if (intersections.Count > 0){
                                Event intersectionEvent = new Event { Type = EventType.Intersection, Edge = 0, Vertex = intersections[0] };
                                _eventQueue.Add(intersectionEvent);
                                intersectingSegments.Add(intersections[0], new Tuple<SweepLineSegment,SweepLineSegment>(segment.Below, segment));
                            }
                        }
                        break;
                    case EventType.Right:
                        segment = _sweepLine.Search(e);
                        if (segment != null)
                        {
                            if (segment.Above != null && segment.Below != null &&
                                Math.Abs(segment.Below.Edge - segment.Above.Edge) != 1)
                            {
                                intersections = LineAlgorithms.Intersection(segment.Above.LeftCoordinate, segment.Above.RightCoordinate, segment.Below.LeftCoordinate, segment.Below.RightCoordinate);
                                if (intersections.Count > 0 && !_eventQueue.Contains(intersections[0]))
                                {
                                    Event intersectionEvent = new Event { Type = EventType.Intersection, Edge = 0, Vertex = intersections[0] };
                                    _eventQueue.Add(intersectionEvent);
                                    intersectingSegments.Add(intersections[0], new Tuple<SweepLineSegment,SweepLineSegment>(segment.Below, segment.Above));
                                }
                            }
                            _sweepLine.Remove(segment);
                        }
                        break;
                    case EventType.Intersection:
                        _result.Add(e.Vertex);

                        segment = intersectingSegments[e.Vertex].Item1;
                        SweepLineSegment segmentAbove = intersectingSegments[e.Vertex].Item2;

                        intersections = LineAlgorithms.Intersection(segmentAbove.LeftCoordinate, segmentAbove.RightCoordinate, segmentAbove.Above.LeftCoordinate, segmentAbove.Above.RightCoordinate);
                        if (intersections.Count > 0 && !_eventQueue.Contains(intersections[0]))
                        {
                            Event intersectionEvent = new Event { Type = EventType.Intersection, Edge = 0, Vertex = intersections[0] };
                            _eventQueue.Add(intersectionEvent);
                            intersectingSegments.Add(intersections[0], new Tuple<SweepLineSegment,SweepLineSegment>(segment.Below, segment.Above));
                        }

                                        intersections = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate, segment.Below.LeftCoordinate, segment.Below.RightCoordinate);
                        if (intersections.Count > 0 && !_eventQueue.Contains(intersections[0]))
                        {
                            Event intersectionEvent = new Event { Type = EventType.Intersection, Edge = 0, Vertex = intersections[0] };
                            _eventQueue.Add(intersectionEvent);
                            intersectingSegments.Add(intersections[0], new Tuple<SweepLineSegment,SweepLineSegment>(segment.Below, segment.Above));
                        }
                        break;
                }

                e = _eventQueue.Next();
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Computes the intersection coordinates of a line string.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <returns>The <see cref="IList{Coordinate}" /> containing the intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> Intersection(IList<Coordinate> source)
        {
            return new BentleyOttmannAlgorithm(source).Result;
        }
        /// <summary>
        /// Computes the intersection coordinates of multiple line strings.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <returns>The <see cref="IList{Coordinate}" /> containing the intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> Intersection(IEnumerable<IList<Coordinate>> source)
        {
            return new BentleyOttmannAlgorithm(source).Result;
        }

        #endregion
    }
}
