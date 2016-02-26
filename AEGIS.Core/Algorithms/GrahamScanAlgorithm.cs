/// <copyright file="GrahamScanAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
    /// Represents a type for running the Graham scan algorithm.
    /// </summary>
    /// <remarks>
    /// The Graham scan algorithm is used to compute the convex hull of a planar polygon in O(n log n) runtime.
    /// The algorithms assumes the specified coordinates are valid, in counterclockwise order and in the same plane.
    /// </remarks>
    public class GrahamScanAlgorithm
    {
        #region Private types

        /// <summary>
        /// Represents a comparer used by the Graham scan algorithm.
        /// </summary>
        private class GrahamComparer : IComparer<Coordinate>
        {
            #region Private fields

            /// <summary>
            /// The origin coordinate.
            /// </summary>
            private Coordinate _origin;

            /// <summary>
            /// The precision model.
            /// </summary>
            private PrecisionModel _precision;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="GrahamComparer" /> class.
            /// </summary>
            /// <param name="origin">The origin.</param>
            public GrahamComparer(Coordinate origin, PrecisionModel precision)
            {
                _origin = origin;
                _precision = precision;
            }

            #endregion

            #region IComparer methods

            /// <summary>
            /// Compares the specified coordinates.
            /// </summary>
            /// <param name="x">The first coordinate to compare.</param>
            /// <param name="y">The second coordinate to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
            public Int32 Compare(Coordinate x, Coordinate y)
            {
                if (x.Equals(y))
                    return 0;

                Orientation orientation = Coordinate.Orientation(_origin, x, y, _precision);

                switch (orientation)
                {
                    case Orientation.CounterClockwise:
                        return -1;
                    case Orientation.Clockwise:
                        return 1;
                    default:
                        return Coordinate.Distance(_origin, x) < Coordinate.Distance(_origin, y) ? -1 : 1;
                }
            }

            #endregion
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
        /// Initializes a new instance of the <see cref="GrahamScanAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        protected GrahamScanAlgorithm(IList<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = source;
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrahamScanAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        protected GrahamScanAlgorithm(IList<Coordinate> source, PrecisionModel precisionModel) 
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
        /// Computes the convex hull.
        /// </summary>
        public void Compute()
        {
            // source: http://en.wikipedia.org/wiki/Graham_scan

            Coordinate[] coordinates = new Coordinate[_source.Count];
            _source.CopyTo(coordinates, 0);

            // search for the minimal coordinate

            Int32 min = 0;
            for (Int32 i = 1; i < coordinates.Length; i++)
            {
                if ((coordinates[i].Y < coordinates[min].Y) || ((coordinates[i].Y == coordinates[min].Y) && (coordinates[i].X < coordinates[min].X)))
                {
                    min = i;
                }
            }

            Coordinate temp = coordinates[min];
            coordinates[min] = coordinates[0];
            coordinates[0] = temp;

            // sort coordinates
            Array.Sort(coordinates, 1, coordinates.Length - 2, new GrahamComparer(coordinates[0], PrecisionModel));

            // select convex hull candidate coordinates
            Coordinate[] candidateCoordiantes = new Coordinate[coordinates.Length];

            // first 3 elements are definitely candidates
            Int32 convexHullCount = 3;
            Array.Copy(coordinates, candidateCoordiantes, 3);

            for (Int32 i = 3; i < coordinates.Length; i++)
            {
                // further coordinates should be checked and removed if not counter clockwise
                while (convexHullCount > 2 && Coordinate.Orientation(candidateCoordiantes[convexHullCount - 2], candidateCoordiantes[convexHullCount - 1], coordinates[i], PrecisionModel) != Orientation.CounterClockwise)
                {
                    convexHullCount--;
                }
                candidateCoordiantes[convexHullCount] = coordinates[i];
                convexHullCount++;
            }

            // copy to result array
            _result = new Coordinate[convexHullCount];
            Array.Copy(candidateCoordiantes, _result, convexHullCount);

            _hasResult = true;
        }

        #endregion
       
        #region Public static methods

        /// <summary>
        /// Computes the convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        /// <returns>The convex hull of <see cref="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> ComputeConvexHull(IBasicPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.Shell == null || source.Shell.Coordinates == null)
                return null;

            return new GrahamScanAlgorithm(source.Shell.Coordinates).Result;
        }

        /// <summary>
        /// Computes the convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The convex hull of <see cref="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> ComputeConvexHull(IBasicPolygon source, PrecisionModel precision)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.Shell == null || source.Shell.Coordinates == null)
                return null;

            return new GrahamScanAlgorithm(source.Shell.Coordinates, precision).Result;
        }

        /// <summary>
        /// Computes the convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <returns>The convex hull of <see cref="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> ComputeConvexHull(IList<Coordinate> source)
        {
            return new GrahamScanAlgorithm(source).Result;
        }

        /// <summary>
        /// Computes the convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The convex hull of <see cref="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IList<Coordinate> ComputeConvexHull(IList<Coordinate> source, PrecisionModel precision)
        {
            return new GrahamScanAlgorithm(source, precision).Result;
        }

        #endregion
    }
}
