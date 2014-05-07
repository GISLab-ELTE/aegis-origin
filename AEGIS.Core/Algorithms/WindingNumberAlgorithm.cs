/// <copyright file="WindingNumberAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for running the Winding Number algorithm.
    /// </summary>
    /// <remarks>
    /// The Winding Number method counts the number of times the polygon winds around a coordinate. 
    /// The coordinate outside only when this "winding number" is zero; otherwise, the coordinate is inside.
    /// The algorithm assumes that the specified coordinates are valid, ordered, distinct and in the same plane.
    /// </remarks>
    public class WindingNumberAlgorithm
    {
        #region Protected fields

        protected IList<Coordinate> _shell;
        protected Coordinate _coordinate;
        protected Int32 _result;
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
        /// Gets or sets the coordinate for which the Winding Number is computed.
        /// </summary>
        /// <value>The coordinate.</value>
        public Coordinate Coordinate
        {
            get { return _coordinate; }
            set 
            {
                if (_coordinate != value) { _coordinate = value; _hasResult = false; }
            }
        }

        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The Winding Number of the specified coordinate.</value>
        public Int32 Result
        {
            get
            {
                if (!_hasResult)
                    Compute();
                return _result;
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindingNumberAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="coordinate">The coordinate for which the Winding Number is calculated.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public WindingNumberAlgorithm(IList<Coordinate> shell, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            _shell = shell;
            _coordinate = coordinate;
            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the Winding Number.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="coordinate">The coordinate for which the Winding Number is calculated.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public void Compute(IList<Coordinate> shell, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            _shell = shell;
            _coordinate = coordinate;
            _hasResult = false;
            Compute();
        }

        /// <summary>
        /// Computes the Winding Number.
        /// </summary>
        public void Compute()
        {
            // source: http://geomalgorithms.com/a03-_inclusion.html

            _result = 0;

            // loop through all edges of the polygon
            for (Int32 i = 0; i < _shell.Count - 1; i++)
            { 
                if (_shell[i].Y <= _coordinate.Y)
                {
                    if (_shell[i + 1].Y > _coordinate.Y) // an upward crossing
                        if (Coordinate.Orientation(_shell[i], _shell[i + 1], _coordinate) == Orientation.CounterClockwise)
                            ++_result; // have  a valid up intersect
                }
                else
                {
                    if (_shell[i + 1].Y <= _coordinate.Y) // a downward crossing
                        if (Coordinate.Orientation(_shell[i], _shell[i + 1], _coordinate) == Orientation.Clockwise)
                            --_result; // have  a valid down intersect
                }
            }
            _hasResult = true;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsInsidePolygon(IList<Coordinate> shell, Coordinate coordinate)
        {
            return (new WindingNumberAlgorithm(shell, coordinate).Result != 0);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsInsidePolygon(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate)
        {
            if (new WindingNumberAlgorithm(shell, coordinate).Result == 0)
                return false;

            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                {
                    if (new WindingNumberAlgorithm(hole, coordinate).Result != 0)
                        return false;
                }
            }

            return true;
        }

        #endregion
    }
}
