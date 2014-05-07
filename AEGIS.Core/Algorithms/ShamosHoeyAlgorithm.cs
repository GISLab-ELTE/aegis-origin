/// <copyright file="ShamosHoeyAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents the Shamos-Hoey Algorithm for determining intersection of line strings.
    /// </summary>
    public class ShamosHoeyAlgorithm
    {
        #region Protected fields

        protected PresortedEventQueue _eventQueue;
        protected SweepLine _sweepLine;
        protected Boolean _hasResult;
        protected Boolean _result;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value><c>true</c> if the specified line strings intersect; otherwise, <c>false</c>.</value>
        public Boolean Result { get { if (!_hasResult) Compute(); return _result; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricShamosHoeyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ShamosHoeyAlgorithm(IList<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new PresortedEventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricShamosHoeyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ShamosHoeyAlgorithm(IEnumerable<IList<Coordinate>> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new PresortedEventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Computes whether one or more line strings specified by coordinates intersects with eachother.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public void Compute(IList<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new PresortedEventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
            Compute();
        }

        /// <summary>
        /// Computes whether one or more line strings specified by coordinates intersects with eachother.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public void Compute(IEnumerable<IList<Coordinate>> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _eventQueue = new PresortedEventQueue(source);
            _sweepLine = new SweepLine(source);
            _hasResult = false;
            Compute();
        }

        /// <summary>
        /// Computes whether one or more line strings specified by coordinates intersects with eachother.
        /// </summary>
        public void Compute()
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            Event e = _eventQueue.Next();
            SweepLineSegment segment;

            while (e != null)
            {
                switch (e.Type)
                { 
                    case EventType.Left:
                        segment = _sweepLine.Add(e);
                        if (segment.Above != null && Math.Abs(segment.Edge - segment.Above.Edge) != 1 && 
                            LineAlgorithms.Intersects(segment.LeftCoordinate, segment.RightCoordinate, segment.Above.LeftCoordinate, segment.Above.RightCoordinate) ||
                            segment.Below != null && Math.Abs(segment.Edge - segment.Below.Edge) != 1 && 
                            LineAlgorithms.Intersects(segment.LeftCoordinate, segment.RightCoordinate, segment.Below.LeftCoordinate, segment.Below.RightCoordinate))
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
                            if (segment.Above != null && segment.Below != null &&
                                Math.Abs(segment.Below.Edge - segment.Above.Edge) != 1 && 
                                LineAlgorithms.Intersects(segment.Above.LeftCoordinate, segment.Above.RightCoordinate, segment.Below.LeftCoordinate, segment.Below.RightCoordinate))
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
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IList<Coordinate> source)
        {
            return new ShamosHoeyAlgorithm(source).Result;
        }

        /// <summary>
        /// Determines whether line strings specified by coordinates intersect.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IEnumerable<IList<Coordinate>> source)
        {
            return new ShamosHoeyAlgorithm(source).Result;
        }

        #endregion
    }
}
