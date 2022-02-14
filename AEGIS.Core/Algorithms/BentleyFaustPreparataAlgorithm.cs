/// <copyright file="BentleyFaustPreparataAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for running the Bentley-Faust-Preparata algorithm.
    /// </summary>
    /// <remarks>
    /// The Bentley-Faust-Preparata algorithm is used for computing the approximate convex hull of a planar coordinate 
    /// set in O(n) runtime. In many applications, an approximate hull suffices, and the gain in speed can be 
    /// significant for very large coordinate sets.
    /// The algorithm assumes that the specified coordinates are valid, distinct and in the same plane.
    /// </remarks>
    public class BentleyFaustPreparataAlgorithm
    {
        #region Private types

        /// <summary>
        /// Represents a range bin.
        /// </summary>
        private struct RangeBin
        {
            /// <summary>
            /// Gets or sets the minimum of the bin.
            /// </summary>
            public Int32? Min { get; set; }

            /// <summary>
            /// Gets or sets the maximum of the bin.
            /// </summary>
            public Int32? Max { get; set; }
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The list of source coordinates.
        /// </summary>
        protected IList<Coordinate> _source;

        /// <summary>
        /// The approximate convex hull of the source.
        /// </summary>
        protected Coordinate[] _result;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        protected Boolean _hasResult;

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
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The list of approximate convex hull coordinates.</value>
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
        /// Initializes a new instance of the <see cref="BentleyFaustPreparataAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyFaustPreparataAlgorithm(IList<Coordinate> source)
            : this(source, PrecisionModel.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyFaustPreparataAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyFaustPreparataAlgorithm(IList<Coordinate> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = source;
            _hasResult = false;
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the approximate convex hull.
        /// </summary>
        public void Compute()
        {
            if (_source.Count < 4)
            {
                _result = _source.Distinct().ToArray();
                _hasResult = true;
                return;
            }

            // source: http://geomalgorithms.com/a11-_hull-2.html

            Int32 minMin = 0, minMax = 0;
            Int32 maxMin = 0, maxMax = 0;
            Double xMin = _source[0].X, xMax = _source[0].X;
            Int32 numberOfContainers = _source.Count / 2;
            Int32 bottomOfStack = 0, topOfStack = -1; // indices for bottom and top of the stack
            Coordinate currentCoordinate; // the current coordinate being considered
            Coordinate[] hullStack = new Coordinate[_source.Count];

            // get the coordinates with min-max X, and min-max Y
            for (Int32 i = 1; i < _source.Count; i++)
            {
                if (_source[i].X <= xMin)
                {
                    if (_source[i].X < xMin)
                    {
                        xMin = _source[i].X;
                        minMin = minMax = i;
                    }
                    else
                    {
                        if (_source[i].Y < _source[minMin].Y)
                            minMin = i;
                        else if (_source[i].Y > _source[minMax].Y)
                            minMax = i;
                    }
                }
                if (_source[i].X >= xMax)
                {
                    if (_source[i].X > xMax)
                    {
                        xMax = _source[i].X;
                        maxMin = maxMax = i;
                    }
                    else
                    {
                        if (_source[i].Y < _source[maxMin].Y)
                            maxMin = i;
                        else if (_source[i].Y > _source[maxMax].Y)
                            maxMax = i;
                    }
                }
            }

            // degenerate case: all x coordinates are equal to xmin
            if (xMin == xMax)
            {
                if (minMax != minMin)
                    _result = new Coordinate[] { _source[minMin], _source[minMax] };
                else
                    _result = new Coordinate[] { _source[minMin] };
            }

            // get the max and min coordinates in the range bins
            RangeBin[] binArray = new RangeBin[numberOfContainers + 2];

            binArray[0].Min = minMin; binArray[0].Max = minMax;
            binArray[numberOfContainers + 1].Min = maxMin; binArray[numberOfContainers + 1].Max = maxMax;
            for (Int32 b = 1; b <= numberOfContainers; b++)
            {
                binArray[b].Min = binArray[b].Max = null;
            }

            for (Int32 b, i = 0; i < _source.Count; i++)
            {
                if (_source[i].X == xMin || _source[i].X == xMax)
                    continue;

                if (Coordinate.Orientation(_source[minMin], _source[maxMin], _source[i], PrecisionModel) == Orientation.Clockwise) // below lower line
                {
                    b = Convert.ToInt32(numberOfContainers * (_source[i].X - xMin) / (xMax - xMin)) + 1;
                    if (binArray[b].Min == null || _source[i].Y < _source[binArray[b].Min.Value].Y)
                        binArray[b].Min = i;
                }
                else if (Coordinate.Orientation(_source[minMin], _source[maxMin], _source[i], PrecisionModel) == Orientation.CounterClockwise) // above upper line
                {
                    b = Convert.ToInt32(numberOfContainers * (_source[i].X - xMin) / (xMax - xMin)) + 1;
                    if (binArray[b].Max == null || _source[i].Y > _source[binArray[b].Max.Value].Y)
                        binArray[b].Max = i;
                }
            }

            // use the chain algorithm to get the lower and upper hulls

            // compute the lower hull on the stack
            for (Int32 i = 0; i <= numberOfContainers + 1; ++i)
            {
                if (binArray[i].Min == null) 
                    continue;

                currentCoordinate = _source[binArray[i].Min.Value];

                while (topOfStack > 0) // there are at least 2 points on the stack
                {
                    if (Coordinate.Orientation(hullStack[topOfStack - 1], hullStack[topOfStack], currentCoordinate, PrecisionModel) == Orientation.CounterClockwise)
                        break;
                    else
                        --topOfStack;
                }
                topOfStack++;
                hullStack[topOfStack] = currentCoordinate;
            }

            // compute the upper hull on the stack above the bottom hull
            if (maxMax != maxMin)
            {
                topOfStack++;
                hullStack[topOfStack] = _source[maxMax];
            }

            bottomOfStack = topOfStack;

            for (Int32 i = numberOfContainers; i >= 0; --i)
            {
                if (binArray[i].Max == null)
                    continue;

                currentCoordinate = _source[binArray[i].Max.Value];

                while (topOfStack > bottomOfStack) // there are at least 2 points on the upper stack
                {
                    if (Coordinate.Orientation(hullStack[topOfStack - 1], hullStack[topOfStack], currentCoordinate, PrecisionModel) == Orientation.CounterClockwise)
                        break;
                    else
                        topOfStack--;
                }

                topOfStack++;
                hullStack[topOfStack] = currentCoordinate;
            }

            // push joining endpoint onto stack
            if (minMax != minMin)
            {
                topOfStack++;
                hullStack[topOfStack] = _source[minMin];
            }

            // generate result from stack
            _result = new Coordinate[topOfStack + 1];
            Array.Copy(hullStack, _result, topOfStack);
            _result[topOfStack] = _result[0];

            _hasResult = true;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        /// <returns>The approximate convex hull of <see cref="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> ApproximateConvexHull(IBasicPolygon source)
        {
            return ApproximateConvexHull(source, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The approximate convex hull of <see cref="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> ApproximateConvexHull(IBasicPolygon source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.Shell == null || source.Shell.Coordinates == null)
                return null;

            return new BentleyFaustPreparataAlgorithm(source.Shell.Coordinates, precisionModel).Result;
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <returns>The approximate convex hull of <see cref="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> ApproximateConvexHull(IList<Coordinate> source)
        {
            return new BentleyFaustPreparataAlgorithm(source).Result;
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The approximate convex hull of <see cref="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> ApproximateConvexHull(IList<Coordinate> source, PrecisionModel precisionModel)
        {
            return new BentleyFaustPreparataAlgorithm(source, precisionModel).Result;
        }

        #endregion
    }
}
