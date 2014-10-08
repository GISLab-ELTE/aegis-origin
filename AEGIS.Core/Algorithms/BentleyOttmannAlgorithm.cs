/// <copyright file="BentleyOttmannAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Collections.SweepLine;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents the Bentley-Ottmann Algorithm for determining intersection of line strings.
    /// </summary>
    /// <remarks>
    /// The Bentley–Ottmann algorithm is a sweep line algorithm for listing all crossings in a set of line segments.
    /// It extends the Shamos–Hoey algorithm, a similar previous algorithm for testing whether or not a set of line segments has any crossings.
    /// For an input consisting of n line segments with k crossings, the Bentley–Ottmann algorithm takes time O((n + k) log n).
    /// In cases where k = o(n^2 / log n), this is an improvement on a naïve algorithm that tests every pair of segments, which takes O(n^2).
    /// </remarks>
    /// <seealso cref="ShamosHoeyAlgorithm"/>
    public class BentleyOttmannAlgorithm
    {
        #region Protected fields

        protected EventQueue _eventQueue;
        protected SweepLine _sweepLine;
        protected Boolean _hasResult;
        protected List<Coordinate> _result;
        protected List<Tuple<Int32, Int32>> _edges;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The <see cref="IList{Coordinate}"/> containing the intersection coordinates.</value>
        public IList<Coordinate> Result { get { if (!_hasResult) Compute(); return _result; } }

        /// <summary>
        /// Gets the edge details of the result.
        /// </summary>
        /// <value>The <see cref="IList{Tuple}"/> containing the edge indices intersecting at the coordinates in <see cref="Result"/>.</value>
        public IList<Tuple<Int32, Int32>> Edges { get { if (!_hasResult) Compute(); return _edges; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Algorithms.Spatial.BentleyOttmannAlgorithm"/> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        public BentleyOttmannAlgorithm(IList<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new EventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Algorithms.Spatial.BentleyOttmannAlgorithm"/> class.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
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
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        public void Compute(IList<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new EventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
            Compute();
        }
        /// <summary>
        /// Computes the intersection of one or more line strings.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        public void Compute(IEnumerable<IList<Coordinate>> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new EventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
            Compute();
        }

        /// <summary>
        /// Computes the intersection of one or more line strings.
        /// </summary>
        public void Compute()
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            _result = new List<Coordinate>();
            _edges = new List<Tuple<Int32, Int32>>();
            Event currentEvent = _eventQueue.Next();
            SweepLineSegment segment;
            IList<Coordinate> intersections;

            while (currentEvent != null)
            {
                if (currentEvent is EndPointEvent)
                {
                    var endPointEvent = (EndPointEvent) currentEvent;
                    switch (endPointEvent.Type)
                    {
                        case EventType.Left:
                            segment = _sweepLine.Add(endPointEvent);
                            if (segment.Above != null)
                            {
                                intersections = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate,
                                                                            segment.Above.LeftCoordinate, segment.Above.RightCoordinate);
                                if (intersections.Count > 0)
                                {
                                    var intersectionEvent = new IntersectionEvent
                                        {
                                            Vertex = intersections[0],
                                            Below = segment,
                                            Above = segment.Above
                                        };
                                    _eventQueue.Add(intersectionEvent);
                                }
                            }
                            if (segment.Below != null)
                            {
                                intersections = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate,
                                                                            segment.Below.LeftCoordinate, segment.Below.RightCoordinate);
                                if (intersections.Count > 0)
                                {
                                    var intersectionEvent = new IntersectionEvent
                                        {
                                            Vertex = intersections[0],
                                            Below = segment.Below,
                                            Above = segment
                                        };
                                    _eventQueue.Add(intersectionEvent);
                                }
                            }
                            break;
                        case EventType.Right:
                            segment = _sweepLine.Search(endPointEvent);
                            if (segment != null)
                            {
                                if (segment.Above != null && segment.Below != null)
                                {
                                    intersections = LineAlgorithms.Intersection(segment.Above.LeftCoordinate,
                                                                                segment.Above.RightCoordinate,
                                                                                segment.Below.LeftCoordinate,
                                                                                segment.Below.RightCoordinate);
                                    if (intersections.Count > 0)
                                    {
                                        var intersectionEvent = new IntersectionEvent
                                            {
                                                Vertex = intersections[0],
                                                Below = segment.Below,
                                                Above = segment.Above
                                            };
                                        if (!_eventQueue.Contains(intersectionEvent))
                                            _eventQueue.Add(intersectionEvent);
                                    }
                                }
                                _sweepLine.Remove(segment);
                            }
                            break;
                    }
                }
                else if (currentEvent is IntersectionEvent)
                {
                    var intersectionEvent = (IntersectionEvent) currentEvent;
                    /*
                     * Segment order before intersection: segmentBelow <-> segmentAbove <-> segment <-> segmentAboveAbove
                     * Segment order after intersection:  segmentBelow <-> segment <-> segmentAbove <-> segmentAboveAbove
                     */
                    segment = intersectionEvent.Above;
                    SweepLineSegment segmentAbove = intersectionEvent.Below;

                    if (_sweepLine.Intersect(segment, segmentAbove))
                    {
                        if (!_sweepLine.IsAdjacent(segment.Edge, segmentAbove.Edge))
                        {
                            _result.Add(currentEvent.Vertex);
                            _edges.Add(Tuple.Create(Math.Min(segment.Edge, segmentAbove.Edge),
                                                      Math.Max(segment.Edge, segmentAbove.Edge)));
                        }

                        if (segmentAbove.Above != null)
                        {
                            intersections = LineAlgorithms.Intersection(segmentAbove.LeftCoordinate, segmentAbove.RightCoordinate,
                                                                        segmentAbove.Above.LeftCoordinate,
                                                                        segmentAbove.Above.RightCoordinate);
                            if (intersections.Count > 0 && intersections[0].X >= intersectionEvent.Vertex.X)
                            {
                                var newIntersectionEvent = new IntersectionEvent
                                    {
                                        Vertex = intersections[0],
                                        Below = segmentAbove,
                                        Above = segmentAbove.Above
                                    };
                                if (!_eventQueue.Contains(newIntersectionEvent))
                                    _eventQueue.Add(newIntersectionEvent);
                            }
                        }

                        if (segment.Below != null)
                        {
                            intersections = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate,
                                                                        segment.Below.LeftCoordinate, segment.Below.RightCoordinate);
                            if (intersections.Count > 0 && intersections[0].X >= intersectionEvent.Vertex.X)
                            {
                                var newIntersectionEvent = new IntersectionEvent
                                    {
                                        Vertex = intersections[0],
                                        Below = segment.Below,
                                        Above = segment
                                    };
                                if (!_eventQueue.Contains(newIntersectionEvent))
                                    _eventQueue.Add(newIntersectionEvent);
                            }
                        }
                    }
                }

                currentEvent = _eventQueue.Next();
            }
            _hasResult = true;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Computes the intersection coordinates of a line string.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <returns>The <see cref="T:System.Collections.Generic.IList{Coordinate}"/> containing the intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        public static IList<Coordinate> Intersection(IList<Coordinate> source)
        {
            return new BentleyOttmannAlgorithm(source).Result;
        }
        /// <summary>
        /// Computes the intersection coordinates of multiple line strings.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <returns>The <see cref="T:System.Collections.Generic.IList{Coordinate}"/> containing the intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        public static IList<Coordinate> Intersection(IEnumerable<IList<Coordinate>> source)
        {
            return new BentleyOttmannAlgorithm(source).Result;
        }

        #endregion
    }
}
