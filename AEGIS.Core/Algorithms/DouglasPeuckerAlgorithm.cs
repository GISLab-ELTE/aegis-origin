/// <copyright file="DouglasPeuckerAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Bence Molnár</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for executing the Douglas-Peucker algorithm.
    /// </summary>
    /// <remarks>
    /// Douglas-Peucker algorithm is used to reduce vertexes in the specified delta tolerance in O(n^2) runtime. 
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
        private List<Coordinate> _result;

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
            Boolean[] isCoordianteMarked = new Boolean[_source.Count]; // Array that tells if the i-th point is marked or not.

            // Marking first and last point.
            isCoordianteMarked[0] = isCoordianteMarked[_source.Count - 1] = true;

            // Calling the recursive method that
            DecimateCoordinates(0, _source.Count - 1, isCoordianteMarked);

            // Creating result list of marked points.

            _result = new List<Coordinate>();
            for (Int32 coordinateIndex = 0; coordinateIndex < _source.Count; coordinateIndex++)
            {
                if (isCoordianteMarked[coordinateIndex])
                    _result.Add(_source.ElementAt(coordinateIndex));
            }

            _hasResult = true;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Decimates the coordinates.
        /// </summary>
        /// <param name="startIndex">Index of the starting coordinate.</param>
        /// <param name="endIndex">Index of the ending coordinate.</param>
        /// <param name="isCoordinateMarkedArray">The array of values indicating whether the coordinate is marked.</param>
        protected void DecimateCoordinates(Int32 startIndex, Int32 endIndex, Boolean[] isCoordinateMarkedArray)
        {
            // Setting initial values.
            Double maxd2 = 0;
            Int32 maxi = startIndex;

            // There's nothing to decimate.
            if (endIndex <= startIndex + 1)
                return;

            // Getting start and endcoordinate of actual segment.
            Coordinate startCoordinate = _source.ElementAt(startIndex);
            Coordinate endCoordinate = _source.ElementAt(endIndex);

            // Counting vector of segment pointing from start to end coordinate.
            CoordinateVector segmentVector = new CoordinateVector(endCoordinate.X - startCoordinate.X,
                endCoordinate.Y - startCoordinate.Y, endCoordinate.Z - startCoordinate.Z);
            Double squaredSegmentLength = CoordinateVector.DotProduct(segmentVector, segmentVector);

            // Iterating through the points beetween the start and end coordinate at the linstring.
            for (Int32 i = startIndex + 1; i < endIndex; i++)
            {
                Double distance;

                // Actual point between the specified start an end coordinate of segment.
                Coordinate vi = _source.ElementAt(i);
                CoordinateVector actualVertexVector = new CoordinateVector(vi.X - startCoordinate.X, vi.Y - startCoordinate.Y,
                                                            vi.Z - startCoordinate.Z);
                Double vectorProduct = CoordinateVector.DotProduct(segmentVector, actualVertexVector);

                if (vectorProduct <= 0)
                    distance = Math.Pow(Coordinate.Distance(startCoordinate, vi), 2);
                else if (squaredSegmentLength <= vectorProduct)
                    distance = Math.Pow(Coordinate.Distance(endCoordinate, vi), 2);
                else
                {
                    Double b = vectorProduct / squaredSegmentLength;
                    Coordinate Pb = startCoordinate + b * segmentVector;
                    distance = Math.Pow(Coordinate.Distance(vi, Pb), 2);
                }

                if (distance <= maxd2)
                    continue;

                maxi = i;
                maxd2 = distance;
            }

            if (maxd2 <= _delta * _delta) 
                return;

            isCoordinateMarkedArray[maxi] = true;
            DecimateCoordinates(startIndex, maxi, isCoordinateMarkedArray);
            DecimateCoordinates(maxi, startIndex, isCoordinateMarkedArray);
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
