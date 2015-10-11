/// <copyright file="BentleyOttmannAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Collections.SweepLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents the Bentley-Ottmann Algorithm for determining intersection of line strings.
    /// </summary>
    /// <remarks>
    /// The Bentley–Ottmann algorithm is a sweep line algorithm for listing all crossings in a set of line segments.
    /// It extends the Shamos–Hoey algorithm, a similar previous algorithm for testing whether or not a set of line segments has any crossings.
    /// For an input consisting of n line segments with k crossings, the Bentley–Ottmann algorithm takes time O((n + k) log n).
    /// In cases where k = o(n^2 / log n), this is an improvement on a naive algorithm that tests every pair of segments, which takes O(n^2).
    /// </remarks>
    /// <seealso cref="ShamosHoeyAlgorithm"/>
    public class BentleyOttmannAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The list of source coordinates.
        /// </summary>
        private readonly IList<Coordinate> _source;

        /// <summary>
        /// The list of intersection coordinates.
        /// </summary>
        private List<Coordinate> _intersections;

        /// <summary>
        /// The list of intersection edge indices in the source list.
        /// </summary>
        private List<Tuple<Int32, Int32>> _edgeIndices;

        /// <summary>
        /// The event queue.
        /// </summary>
        private readonly EventQueue _eventQueue;

        /// <summary>
        /// The sweep line.
        /// </summary>
        private readonly SweepLine _sweepLine;

        /// <summary>
        /// A values indicating whether the result was already computed.
        /// </summary>
        private Boolean _hasResult;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the source coordinates.
        /// </summary>
        /// <value>The read-only list of source coordinates.</value>
        public IList<Coordinate> Source
        {
            get
            {
                if (_source.IsReadOnly)
                    return _source;
                else
                    return _source.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the intersection coordinates.
        /// </summary>
        /// <value>The list of intersection coordinates.</value>
        public IList<Coordinate> Intersections 
        { 
            get 
            { 
                if (!_hasResult) 
                    Compute(); 
                return _intersections; 
            } 
        }

        /// <summary>
        /// Gets the indices of intersecting edges.
        /// </summary>
        /// <value>The list of intersecting edge indices with respect to <see cref="Source" /> coordinates.</value>
        /// <remarks>
        /// Indices are assigned sequentially to the input edges from zero, skipping a number when starting a new linestring.
        /// </remarks>
        public IList<Tuple<Int32, Int32>> EdgeIndices 
        { 
            get 
            { 
                if (!_hasResult) 
                    Compute(); 
                return _edgeIndices; 
            } 
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyOttmannAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyOttmannAlgorithm(IBasicLineString source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = source.Coordinates;
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _eventQueue = new EventQueue(source.Coordinates);
            _sweepLine = new SweepLine(source.Coordinates, PrecisionModel);
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyOttmannAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyOttmannAlgorithm(IList<Coordinate> source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = source;
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _eventQueue = new EventQueue(source);
            _sweepLine = new SweepLine(source, PrecisionModel);
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyOttmannAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The collection of line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyOttmannAlgorithm(IEnumerable<IBasicLineString> source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = new List<Coordinate>();
            foreach (IBasicLineString linestring in source)
                (_source as List<Coordinate>).AddRange(linestring.Coordinates);

            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _eventQueue = new EventQueue(source.Select(lineString => lineString == null ? null : lineString.Coordinates));
            _sweepLine = new SweepLine(source.Select(lineString => lineString == null ? null : lineString.Coordinates), PrecisionModel);
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyOttmannAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyOttmannAlgorithm(IEnumerable<IList<Coordinate>> source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = new List<Coordinate>();
            foreach (IList<Coordinate> linestring in source)
                (_source as List<Coordinate>).AddRange(linestring);

            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _eventQueue = new EventQueue(source);
            _sweepLine = new SweepLine(source, PrecisionModel);
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

            _intersections = new List<Coordinate>();
            _edgeIndices = new List<Tuple<Int32, Int32>>();
            Event currentEvent = _eventQueue.Next();
            SweepLineSegment segment;
            IList<Coordinate> intersections;

            while (currentEvent != null)
            {
                if (currentEvent is EndPointEvent)
                {
                    EndPointEvent endPointEvent = (EndPointEvent)currentEvent;
                    switch (endPointEvent.Type)
                    {
                        // Left endpoint event: check for possible intersection with below and / or above segments.
                        case EventType.Left:
                            segment = _sweepLine.Add(endPointEvent);
                            if (segment.Above != null)
                            {
                                intersections = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate,
                                                                            segment.Above.LeftCoordinate, segment.Above.RightCoordinate,
                                                                            PrecisionModel);

                                if (intersections.Count > 0)
                                {
                                    IntersectionEvent intersectionEvent = new IntersectionEvent
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
                                                                            segment.Below.LeftCoordinate, segment.Below.RightCoordinate,
                                                                            PrecisionModel);

                                if (intersections.Count > 0)
                                {
                                    IntersectionEvent intersectionEvent = new IntersectionEvent
                                    {
                                        Vertex = intersections[0],
                                        Below = segment.Below,
                                        Above = segment
                                    };
                                    _eventQueue.Add(intersectionEvent);
                                }
                            }
                            break;

                        // Right endpoint event: check for possible intersection of the below and above segments.
                        case EventType.Right:
                            segment = _sweepLine.Search(endPointEvent);
                            if (segment != null)
                            {
                                if (segment.Above != null && segment.Below != null)
                                {
                                    intersections = LineAlgorithms.Intersection(segment.Above.LeftCoordinate,
                                                                                segment.Above.RightCoordinate,
                                                                                segment.Below.LeftCoordinate,
                                                                                segment.Below.RightCoordinate,
                                                                                PrecisionModel);

                                    if (intersections.Count > 0)
                                    {
                                        IntersectionEvent intersectionEvent = new IntersectionEvent
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

                // Intersection point event: switch the two concerned segments and check for possible intersection with their below and above segments.
                else if (currentEvent is IntersectionEvent)
                {
                    IntersectionEvent intersectionEvent = (IntersectionEvent)currentEvent;
                    /*
                     * Segment order before intersection: segmentBelow <-> segmentAbove <-> segment <-> segmentAboveAbove
                     * Segment order after intersection:  segmentBelow <-> segment <-> segmentAbove <-> segmentAboveAbove
                     */
                    segment = intersectionEvent.Above;
                    SweepLineSegment segmentAbove = intersectionEvent.Below;

                    // Handle closing intersection points when segments (partially) overlap each other.
                    if (intersectionEvent.IsClose)
                    {
                        if (!_sweepLine.IsAdjacent(segment.Edge, segmentAbove.Edge))
                        {
                            _intersections.Add(currentEvent.Vertex);
                            _edgeIndices.Add(Tuple.Create(Math.Min(segment.Edge, segmentAbove.Edge),
                                                          Math.Max(segment.Edge, segmentAbove.Edge)));
                        }
                    }

                    // It is possible that the previously detected intersection point is not a real intersection, because a new segment started between them,
                    // therefore a repeated check is necessary to carry out.
                    else if (_sweepLine.Add(intersectionEvent))
                    {
                        if (!_sweepLine.IsAdjacent(segment.Edge, segmentAbove.Edge))
                        {
                            _intersections.Add(currentEvent.Vertex);
                            _edgeIndices.Add(Tuple.Create(Math.Min(segment.Edge, segmentAbove.Edge),
                                                      Math.Max(segment.Edge, segmentAbove.Edge)));

                            intersections = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate,
                                                                        segmentAbove.LeftCoordinate, segmentAbove.RightCoordinate,
                                                                        PrecisionModel);

                            if (intersections.Count > 1 && !intersections[1].Equals(intersections[0]))
                            {
                                IntersectionEvent newIntersectionEvent = new IntersectionEvent
                                {
                                    Vertex = intersections[1],
                                    Below = segment,
                                    Above = segmentAbove,
                                    IsClose = true,
                                };
                                _eventQueue.Add(newIntersectionEvent);
                            }
                        }

                        if (segmentAbove.Above != null)
                        {
                            intersections = LineAlgorithms.Intersection(segmentAbove.LeftCoordinate, segmentAbove.RightCoordinate,
                                                                        segmentAbove.Above.LeftCoordinate, segmentAbove.Above.RightCoordinate,
                                                                        PrecisionModel);

                            if (intersections.Count > 0 && intersections[0].X >= intersectionEvent.Vertex.X)
                            {
                                IntersectionEvent newIntersectionEvent = new IntersectionEvent
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
                                                                        segment.Below.LeftCoordinate, segment.Below.RightCoordinate,
                                                                        PrecisionModel);

                            if (intersections.Count > 0 && intersections[0].X >= intersectionEvent.Vertex.X)
                            {
                                IntersectionEvent newIntersectionEvent = new IntersectionEvent
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
        /// <param name="source">The line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The list of intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> Intersection(IBasicLineString source, PrecisionModel precisionModel = null)
        {
            return new BentleyOttmannAlgorithm(source, precisionModel).Intersections;
        }

        /// <summary>
        /// Computes the intersection coordinates of a line string.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The list of intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> Intersection(IList<Coordinate> source, PrecisionModel precisionModel = null)
        {
            return new BentleyOttmannAlgorithm(source, precisionModel).Intersections;
        }

        /// <summary>
        /// Computes the intersection coordinates of multiple line strings.
        /// </summary>
        /// <param name="source">The collection of line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The list of intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> Intersection(IEnumerable<IBasicLineString> source, PrecisionModel precisionModel = null)
        {
            return new BentleyOttmannAlgorithm(source, precisionModel).Intersections;
        }

        /// <summary>
        /// Computes the intersection coordinates of multiple line strings.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel"></param>
        /// <returns>The list of intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> Intersection(IEnumerable<IList<Coordinate>> source, PrecisionModel precisionModel = null)
        {
            return new BentleyOttmannAlgorithm(source, precisionModel).Intersections;
        }

        #endregion
    }
}
