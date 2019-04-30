/// <copyright file="ShamosHoeyAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
    /// Represents the Shamos-Hoey Algorithm for determining intersection of line strings.
    /// </summary>
    /// <seealso cref="BentleyOttmannAlgorithm"/>
    public class ShamosHoeyAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The event queue.
        /// </summary>
        private PresortedEventQueue _eventQueue;

        /// <summary>
        /// The sweep line.
        /// </summary>
        private SweepLine _sweepLine;
        

        /// <summary>
        /// A value indicating whether the result is computed.
        /// </summary>
        private Boolean _hasResult;

        /// <summary>
        /// The result of the algorithm.
        /// </summary>
        private Boolean _result;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</value>
        public Boolean Result 
        { 
            get 
            { 
                if (!_hasResult) 
                    Compute();
                return _result; 
            } 
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShamosHoeyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ShamosHoeyAlgorithm(IBasicLineString source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _eventQueue = new PresortedEventQueue(source.Coordinates);
            _sweepLine = new SweepLine(source.Coordinates, precisionModel);
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShamosHoeyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ShamosHoeyAlgorithm(IList<Coordinate> source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _eventQueue = new PresortedEventQueue(source);
            _sweepLine = new SweepLine(source, precisionModel);
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShamosHoeyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ShamosHoeyAlgorithm(IEnumerable<IBasicLineString> source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _eventQueue = new PresortedEventQueue(source.Select(lineString => lineString == null ? null : lineString.Coordinates));
            _sweepLine = new SweepLine(source.Select(hole => hole.Coordinates), precisionModel);
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShamosHoeyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ShamosHoeyAlgorithm(IEnumerable<IList<Coordinate>> source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _eventQueue = new PresortedEventQueue(source);
            _sweepLine = new SweepLine(source, precisionModel);
            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes whether one or more line strings specified by coordinates intersects with each other.
        /// </summary>
        public void Compute()
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            EndPointEvent e = _eventQueue.Next();
            SweepLineSegment segment;

            while (e != null)
            {
                switch (e.Type)
                {
                    case EventType.Left:
                        segment = _sweepLine.Add(e);
                        if (segment.Above != null && _sweepLine.IsAdjacent(segment.Edge, segment.Above.Edge) && segment.LeftCoordinate == segment.Above.LeftCoordinate)
                            _sweepLine.Intersect(segment, segment.Above);
                        else if (segment.Below != null && _sweepLine.IsAdjacent(segment.Edge, segment.Below.Edge) && segment.LeftCoordinate == segment.Below.LeftCoordinate)
                            _sweepLine.Intersect(segment, segment.Below);

                        if (segment.Above != null && !_sweepLine.IsAdjacent(segment.Edge, segment.Above.Edge) &&
                            LineAlgorithms.Intersects(segment.LeftCoordinate, segment.RightCoordinate,
                                                      segment.Above.LeftCoordinate, segment.Above.RightCoordinate,
                                                      PrecisionModel))
                        {
                            _hasResult = true;
                            _result = true;
                            return;
                        }
                        if (segment.Below != null && !_sweepLine.IsAdjacent(segment.Edge, segment.Below.Edge) &&
                            LineAlgorithms.Intersects(segment.LeftCoordinate, segment.RightCoordinate, 
                                                      segment.Below.LeftCoordinate, segment.Below.RightCoordinate,
                                                      PrecisionModel))
                        {
                            _hasResult = true;
                            _result = true;
                            return;
                        }
                        break;
                    case EventType.Right:
                        segment = _sweepLine.Search(e);
                        if (segment != null)
                        {
                            if (segment.Above != null && segment.Below != null && !_sweepLine.IsAdjacent(segment.Below.Edge, segment.Above.Edge) &&
                                LineAlgorithms.Intersects(segment.Above.LeftCoordinate, segment.Above.RightCoordinate,
                                                          segment.Below.LeftCoordinate, segment.Below.RightCoordinate,
                                                          PrecisionModel))
                            {
                                _hasResult = true;
                                _result = true;
                                return;
                            }
                            _sweepLine.Remove(segment);
                        }
                        break;
                }

                e = _eventQueue.Next();
            }

            _hasResult = true;
            _result = false;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether a line string specified by coordinates intersects with itself.
        /// </summary>
        /// <param name="source">The line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IBasicLineString source, PrecisionModel precisionModel = null)
        {
            return new ShamosHoeyAlgorithm(source, precisionModel).Result;
        }

        /// <summary>
        /// Determines whether a line string specified by coordinates intersects with itself.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IList<Coordinate> source, PrecisionModel precisionModel = null)
        {
            return new ShamosHoeyAlgorithm(source, precisionModel).Result;
        }

        /// <summary>
        /// Determines whether line strings specified by coordinates intersect.
        /// </summary>
        /// <param name="source">The line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IEnumerable<IBasicLineString> source, PrecisionModel precisionModel = null)
        {
            return new ShamosHoeyAlgorithm(source, precisionModel).Result;
        }

        /// <summary>
        /// Determines whether line strings specified by coordinates intersect.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IEnumerable<IList<Coordinate>> source, PrecisionModel precisionModel = null)
        {
            return new ShamosHoeyAlgorithm(source, precisionModel).Result;
        }

        #endregion
    }
}
