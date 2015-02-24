/// <copyright file="DouglasPeuckerAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
/// <author>Bence Molnár</author>
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for executing the Douglas-Peucker algorithm.
    /// </summary>
    /// <remarks>
    /// Douglas-Peucker (or Ramer-Douglas-Peucker) algorithm is used to reduce vertices in a line string, resulting in a similar line string in O(n^2) runtime.
    /// The algorithm assumes, that the source is a simple line string without circle.
    /// </remarks>
    public class DouglasPeuckerAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The source line string.
        /// </summary>
        private IList<Coordinate> _source;

        /// <summary>
        /// The tolerance.
        /// </summary>
        private Double _delta;

        /// <summary>
        /// The simplified line string.
        /// </summary>
        private Coordinate[] _result;

        /// <summary>
        /// The marks on the coordinates.
        /// </summary>
        private Boolean[] _marks;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        private Boolean _hasResult;

        #endregion

        #region Public properties

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
        /// Gets the result.
        /// </summary>
        /// <value>The list of coordinates of the simplified line string.</value>
        public IList<Coordinate> Result
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
        /// Initializes a new instance of the <see cref="DouglasPeuckerAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="delta">The tolarance.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public DouglasPeuckerAlgorithm(IList<Coordinate> source, Double delta)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (delta <= 0)
                throw new ArgumentOutOfRangeException("delta", "The delta is less than or equal to 0.");

            _source = source;
            _delta = delta;
            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Runs the reduction algorithm.
        /// </summary>
        public void Compute()
        {
            _marks = new Boolean[_source.Count];

            // mark the first and last coordinates
            _marks[0] = _marks[_source.Count - 1] = true;

            SimplifySegment(0, _source.Count - 1); // recursive simplification of the source

            // create the result based on the marked coordinates
            _result = new Coordinate[_marks.Count(value => value)];
            Int32 resultIndex = 0;
            for (Int32 sourceIndex = 0; sourceIndex < _source.Count; sourceIndex++)
            {
                if (_marks[sourceIndex])
                {
                    _result[resultIndex] = _source[sourceIndex];
                    resultIndex++;
                }
            }

            _hasResult = true;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Simplifies a segment of the line string.
        /// </summary>
        /// <param name="startIndex">The strating index of the segment.</param>
        /// <param name="endIndex">The ending index of the segment.</param>
        protected void SimplifySegment(Int32 startIndex, Int32 endIndex)
        {
            if (endIndex <= startIndex + 1) // the segment is a line
                return;

            Double maxDistance = 0;
            Int32 maxIndex = startIndex;

            // find the the most distant coordinate from the line between the starting and ending coordinates
            for (Int32 coordinateIndex = startIndex + 1; coordinateIndex < endIndex; coordinateIndex++)
            {
                Double distance = LineAlgorithms.Distance(_source[startIndex], _source[endIndex], _source[coordinateIndex]);

                if (distance > maxDistance)
                {
                    maxIndex = coordinateIndex;
                    maxDistance = distance;
                }
            }

            if (maxDistance <= _delta) // the distance is smaller than the delta, the all coordinates should be removed
                return;

            // recursively simplify both segments
            _marks[maxIndex] = true;
            SimplifySegment(startIndex, maxIndex);
            SimplifySegment(maxIndex, startIndex);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Simplifies the specified line string.
        /// </summary>
        /// <param name="source">The line string.</param>
        /// <param name="delta">The tolerance.</param>
        /// <returns>The simplified line string.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IBasicLineString Simplify(IBasicLineString source, Double delta)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            DouglasPeuckerAlgorithm algorithm = new DouglasPeuckerAlgorithm(source.Coordinates, delta);
            algorithm.Compute();
            return new BasicLineString(algorithm.Result);
        }

        /// <summary>
        /// Simplifies the specified polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <param name="delta">The tolerance.</param>
        /// <returns>The simplified polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IBasicPolygon Simplify(IBasicPolygon source, Double delta)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            DouglasPeuckerAlgorithm algorithm = new DouglasPeuckerAlgorithm(source.Shell.Coordinates, delta);
            algorithm.Compute();
            IList<Coordinate> shell = algorithm.Result;
            List<IList<Coordinate>> holes = new List<IList<Coordinate>>();

            foreach (IBasicLineString hole in source.Holes)
            {
                algorithm = new DouglasPeuckerAlgorithm(hole.Coordinates, delta);
                algorithm.Compute();
                holes.Add(algorithm.Result);
            }

            return new BasicPolygon(shell, holes);
        }

        /// <summary>
        /// Simplifies the specified line string.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="delta">The tolerance.</param>
        /// <returns>The list of coordinates of the simplified line string.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IList<Coordinate> Simplify(IList<Coordinate> source, Double delta)
        {
            DouglasPeuckerAlgorithm algorithm = new DouglasPeuckerAlgorithm(source, delta);
            algorithm.Compute();
            return algorithm.Result;
        }

        #endregion
    }
}
