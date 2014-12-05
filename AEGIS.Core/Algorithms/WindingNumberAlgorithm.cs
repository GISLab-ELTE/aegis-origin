/// <copyright file="WindingNumberAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for performing the Winding Number algorithm.
    /// </summary>
    /// <remarks>
    /// The Winding Number method counts the number of times the polygon winds around a coordinate. 
    /// When the coordinate is outside, the value of the winding number is zero; otherwise, the coordinate is inside.
    /// However, the winding number is not defined for coordinates on the boundary of the polygon, it might be both a non-zero or a zero value.
    /// For an input consisting of n line segments, the Winding Number algorithm has a linear complexity of O(2n).
    /// The algorithm assumes that the specified coordinates are valid, ordered, distinct and in the same plane.
    /// </remarks>
    public class WindingNumberAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The polygon shell.
        /// </summary>
        private IList<Coordinate> _shell;

        /// <summary>
        /// Thge coordinate.
        /// </summary>
        private Coordinate _coordinate;

        /// <summary>
        /// A value indicating whether to verify if the given coordinate is on the shell.
        /// </summary>
        private Boolean _verifyBoundary;

        /// <summary>
        /// A value indicating whether the result is computed.
        /// </summary>
        private Boolean _hasResult;

        /// <summary>
        /// The result of the algorithm.
        /// </summary>
        private Int32 _result;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the coordinates of the polygon shell.
        /// </summary>
        /// <value>The coordinates of the polygon shell.</value>
        /// <exception cref="System.InvalidOperationException">The value is null.</exception>
        public IList<Coordinate> Shell
        {
            get { return _shell.AsReadOnly(); }
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
                if (_coordinate != value)
                { 
                    _coordinate = value; 
                    _hasResult = false; 
                }
            }
        }

        /// <summary>
        /// Gets of sets a values indicating whether to verify if the given coordinate is on the shell.
        /// </summary>
        /// <value><c>true</c> to verify whether <see cref="Coordinate" /> is on the <see cref="Shell" />; otherwise <c>false</c>.</value>
        public Boolean VerifyBoundary
        {
            get { return _verifyBoundary; }
            set
            {
                if (_verifyBoundary != value) 
                { 
                    _verifyBoundary = value; 
                    _hasResult = false; 
                }
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

        /// <summary>
        /// Gets a value indicating whether the given coordinate is on the shell.
        /// </summary>
        /// <remarks>
        /// Requesting the value of this property does not execute the computation of the algorithm.
        /// </remarks>
        /// <value><c>true</c> if the <see cref="Coordinate" /> is <b>certainly</b> on the <see cref="Shell" />, <c>false</c> if <b>certainly</b> not; otherwise <c>null</c>.</value>
        public Boolean? IsOnBoundary { get; protected set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindingNumberAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="coordinate">The coordinate for which the Winding Number is calculated.</param>
        /// <param name="verifyBoundary">A value indicating whether to verify if the coordinate is on the boundary.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The shell must contain at least 3 different coordinates.
        /// or
        /// The first and the last coordinates must be equal.
        /// </exception>
        public WindingNumberAlgorithm(IList<Coordinate> shell, Coordinate coordinate, Boolean verifyBoundary = false)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (shell.Count < 4)
                throw new ArgumentException("The shell must contain at least 3 different coordinates.", "shell");
            if (!shell[0].Equals(shell[shell.Count - 1]))
                throw new ArgumentException("The first and the last coordinates must be equal.", "shell");

            _shell = shell;
            _coordinate = coordinate;
            _verifyBoundary = verifyBoundary;
            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the Winding Number.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="coordinate">The coordinate for which the Winding Number is calculated.</param>
        /// <param name="verifyBoundary">A value indicating whether to verify if the coordinate is on the boundary.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The shell contains less than 3 distinct coordinates.
        /// or
        /// The first and last coordinates of the shell do not match.
        /// </exception>
        public void Compute(IList<Coordinate> shell, Coordinate coordinate, Boolean verifyBoundary = false)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (shell.Count < 4)
                throw new ArgumentException("The shell contains less than 3 distinct coordinates.", "shell");
            if (!shell[0].Equals(shell[shell.Count - 1]))
                throw new ArgumentException("The first and last coordinates of the shell do not match.", "shell");

            _shell = shell;
            _coordinate = coordinate;
            _verifyBoundary = verifyBoundary;
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
            IsOnBoundary = null;

            // Loop through all edges of the polygon.
            for (Int32 i = 0; i < _shell.Count - 1; i++)
            {
                Orientation orientation;

                // An upward crossing.
                if (_shell[i].Y <= _coordinate.Y && _shell[i + 1].Y > _coordinate.Y)
                {
                    orientation = Coordinate.Orientation(_coordinate, _shell[i], _shell[i + 1]);
                    switch (orientation)
                    {
                        case Orientation.CounterClockwise:
                            // Has a valid up intersect.
                            ++_result;
                            break;
                        case Orientation.Collinear:
                            // The winding number is not defined for coordinates on the boundary.
                            IsOnBoundary = true;
                            break;
                    }
                }
                // A downward crossing.
                else if (_shell[i].Y > _coordinate.Y && _shell[i + 1].Y <= _coordinate.Y)
                {
                    orientation = Coordinate.Orientation(_coordinate, _shell[i], _shell[i + 1]);
                    switch (orientation)
                    {
                        case Orientation.Clockwise:
                            // Has a valid down intersect.
                            --_result;
                            break;
                        case Orientation.Collinear:
                            // The winding number is not defined for coordinates on the boundary.
                            IsOnBoundary = true;
                            break;
                    }
                }
            }

            if (!IsOnBoundary.HasValue && _verifyBoundary)
                CheckBoundary();

            _hasResult = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Checks whether the given coordinate is on the shell.
        /// </summary>
        private void CheckBoundary()
        {
            IsOnBoundary = false;
            for (Int32 i = 0; i < _shell.Count - 1; i++)
                if (LineAlgorithms.Contains(_shell[i], _shell[i + 1], _coordinate))
                {
                    IsOnBoundary = true;
                    break;
                }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon.
        /// </remarks>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate" />; otherwise <c>false</c>.</returns>
        public static Boolean IsInsidePolygon(IList<Coordinate> shell, Coordinate coordinate)
        {
            return (new WindingNumberAlgorithm(shell, coordinate).Result != 0);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon or the holes.
        /// </remarks>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate" />; otherwise <c>false</c>.</returns>
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
