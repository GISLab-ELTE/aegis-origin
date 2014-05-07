/// <copyright file="GrahamScanAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
        #region Protected types

        /// <summary>
        /// Represents a comparer used by the Graham scan algorithm.
        /// </summary>
        protected class GrahamComparer : IComparer<Coordinate>
        {
            #region Private fields

            private Coordinate _origin;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="GrahamComparer" /> class.
            /// </summary>
            /// <param name="origin">The origin.</param>
            public GrahamComparer(Coordinate origin)
            {
                _origin = origin;
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

                Orientation orientation = Coordinate.Orientation(_origin, x, y);

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

        #region Protected fields

        protected IList<Coordinate> _shell;
        protected Coordinate[] _result;
        protected Boolean _hasResult;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the coordinates of the polygon shell.
        /// </summary>
        /// <value>The coordinates of the polygon shell.</value>
        /// <exception cref="System.InvalidOperationException">The value is null.</exception>
        public IList<Coordinate> Shell
        {
            get { return _shell; }
            set
            {
                if (value == null) throw new InvalidOperationException("The value is null.");
                if (_shell != value) { _shell = value; _hasResult = false; }
            }
        }
        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The coordinates of the convex hull in a ring.</value>
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

            if (PolygonAlgorithms.Orientation(source) != Orientation.CounterClockwise)
                throw new ArgumentException("The source coordinates are not in counter clockwise orientation.", "source");

            _shell = source;
            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the convex hull.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell in counter clockwise order.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public void Compute(IList<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            _shell = shell;
            _hasResult = false;
            Compute();
        }
        /// <summary>
        /// Computes the convex hull.
        /// </summary>
        public void Compute()
        {
            // source: http://en.wikipedia.org/wiki/Graham_scan

            Coordinate[] coordinates = new Coordinate[_shell.Count];
            _shell.CopyTo(coordinates, 0);

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
            Array.Sort(coordinates, 1, coordinates.Length - 2, new GrahamComparer(coordinates[0]));

            // select convex hull candidate coordinates
            Coordinate[] candidateCoordiantes = new Coordinate[coordinates.Length];

            // first 3 elements are definately candidates
            Int32 convexHullCount = 3;
            Array.Copy(coordinates, candidateCoordiantes, 3);

            for (Int32 i = 3; i < coordinates.Length; i++)
            {
                // further coordinates should be checked and removed if not counter clockwise
                while (convexHullCount > 2 && Coordinate.Orientation(candidateCoordiantes[convexHullCount - 2], candidateCoordiantes[convexHullCount - 1], coordinates[i]) != Orientation.CounterClockwise)
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
        /// Computes the convex hull of the source coordinates.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell in counter clockwise order.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IList<Coordinate> ComputeConvexHull(IList<Coordinate> shell)
        {
            return new GrahamScanAlgorithm(shell).Result;
        }

        #endregion
    }
}
