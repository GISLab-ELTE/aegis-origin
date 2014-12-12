/// <copyright file="PolygonCentroidAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents an algorithm for computing polygon centroid.
    /// </summary>
    /// <remarks>
    /// The algorithm assumes that the source polygon is valid. 
    /// </remarks>
    public class PolygonCentroidAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The coordinates of the polygon shell.
        /// </summary>
        private IList<Coordinate> _shell;

        /// <summary>
        /// The collection of coordinates representing the polygon holes.
        /// </summary>
        private IEnumerable<IList<Coordinate>> _holes;

        /// <summary>
        /// The centroid of the polygon.
        /// </summary>
        private Coordinate _result;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        private Boolean _hasResult;

        /// <summary>
        /// The area of the polygon.
        /// </summary>
        private Double _area;

        /// <summary>
        /// The base coordinate.
        /// </summary>
        private Coordinate _baseCoordinate;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the coordinates of the polygon shell.
        /// </summary>
        /// <value>The read-only list of coordinates representing the polygon shell.</value>
        public IList<Coordinate> Shell
        {
            get 
            {
                if (_shell.IsReadOnly)
                    return _shell;
                else
                    return _shell.AsReadOnly(); 
            }
        }

        /// <summary>
        /// Gets collection of coordinates representing the polygon holes.
        /// </summary>
        /// <value>The collection of coordinates representing the polygon holes.</value>
        public IEnumerable<IList<Coordinate>> Holes
        {
            get 
            { 
                return _holes.Select(hole => hole.IsReadOnly ? hole : hole.AsReadOnly()); 
            }
        }

        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The centroid of the polygon.</value>
        public Coordinate Result 
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
        /// Initializes a new instance of the <see cref="PolygonCentroidAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        public PolygonCentroidAlgorithm(IBasicPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _shell = source.Shell.Coordinates;
            _holes = source.Holes.Select(hole => hole.Coordinates);
            _result = Coordinate.Undefined;
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCentroidAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        public PolygonCentroidAlgorithm(IList<Coordinate> shell) 
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            _shell = shell;
            _holes = null;
            _result = Coordinate.Undefined;
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCentroidAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        public PolygonCentroidAlgorithm(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes) 
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            _shell = shell;
            _holes = holes;
            _result = Coordinate.Undefined;
            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the centroid of the polygon.
        /// </summary>
        public void Compute()
        {
            if (_hasResult)
                return;

            if (_holes == null)
            {
                // in case there are no holes, a simplified algorithm may be used
                ComputeForShell();
            }
            else
            {
                // with holes triangulation must be performed
                ComputeForShellAndHoles();                
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the centroid based on the shell.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The number of coordinates in less than 3.
        /// or
        /// The coordinates do not represent a surface.
        /// </exception>
        protected void ComputeForShell()
        {
            Double resultX = 0, resultY = 0;
            _area = 0;

            if (_shell.Count < 3) // if there are not enough coordinates
            {
                _hasResult = true;
                return;
            }

            for (Int32 i = 0; i < _shell.Count - 1; i++)
            {
                if (_shell[i].Z != _shell[0].Z) // the coordinates are not in one plane
                {
                    _hasResult = true;
                    return;
                }

                resultX += (_shell[i].X + _shell[i + 1].X) * (_shell[i].X * _shell[i + 1].Y - _shell[i + 1].X * _shell[i].Y);
                resultY += (_shell[i].Y + _shell[i + 1].Y) * (_shell[i].X * _shell[i + 1].Y - _shell[i + 1].X * _shell[i].Y);
                _area += _shell[i].X * _shell[i + 1].Y - _shell[i + 1].X * _shell[i].Y;
            }

            resultX /= (6 * _area);
            resultY /= (6 * _area);

            _result = new Coordinate(resultX, resultY, _shell[0].Z);
            _hasResult = true;
        }

        /// <summary>
        /// Computes the centroid based on both the shell and the holes.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The number of coordinates in less than 3.
        /// or
        /// The coordinates do not represent a surface.
        /// </exception>
        protected void ComputeForShellAndHoles()
        {
            Double resultX = 0, resultY = 0, resultZ = 0;
            _area = 0;

            _baseCoordinate = _shell[0];
            resultZ = _shell[0].Z;

            AddCoordinates(_shell, 1, ref resultX, ref resultY);

            if (_hasResult)
                return;

            foreach (IList<Coordinate> hole in _holes)
            {
                if (_hasResult)
                    return;

                AddCoordinates(hole, -1, ref resultX, ref resultY);            
            }

            if (_hasResult)
                return;

            resultX = _result.X / 3 / _area;
            resultY = _result.Y / 3 / _area;

            _result = new Coordinate(resultX, resultY, resultZ);
            _hasResult = true;
        }

        /// <summary>
        /// Add a list of coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="sign">The sign of the coordinates (positive for shell, negative for hole).</param>
        /// <param name="resultX">The X coordinate of the result.</param>
        /// <param name="resultY">The Y coordinate of the result.</param>
        /// <exception cref="System.InvalidOperationException">
        /// The number of coordinates in less than 3.
        /// or
        /// The coordinates do not represent a surface.
        /// </exception>
        protected void AddCoordinates(IList<Coordinate> coordinates, Int32 sign, ref Double resultX, ref Double resultY)
        {
            if (coordinates.Count < 3) // if there are not enough coordinates
                _hasResult = true;

            for (Int32 i = 0; i < coordinates.Count - 1; i++)
            {
                if (coordinates[i].Z != coordinates[0].Z)
                    _hasResult = true;

                AddTriangle(_baseCoordinate, coordinates[i], coordinates[i + 1], sign, ref resultX, ref resultY);
            }

            if (coordinates[coordinates.Count - 1].Z != coordinates[0].Z)
                _hasResult = true;
        }

        /// <summary>
        /// Adds a triangle.
        /// </summary>
        /// <param name="first">The first coordinate of the triangle.</param>
        /// <param name="second">The second coordinate of the triangle.</param>
        /// <param name="third">The third coordinate of the triangle.</param>
        /// <param name="sign">The sign of the triangle (positive for shell, negative for hole).</param>
        /// <param name="resultX">The X coordinate of the result.</param>
        /// <param name="resultY">The Y coordinate of the result.</param>
        protected void AddTriangle(Coordinate first, Coordinate second, Coordinate third, Int32 sign, ref Double resultX, ref Double resultY)
        {
            Double area = (second.X - first.X) * (third.Y - first.Y) - (third.X - first.X) * (second.Y - first.Y);
            _area += sign * area;

            resultX += sign * area * (first.X + second.X + third.X);
            resultY += sign * area * (first.Y + second.Y + third.Y);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IBasicPolygon source)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(source);
            algorithm.Compute();

            return algorithm.Result;
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IList<Coordinate> shell)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(shell);
            algorithm.Compute();

            return algorithm.Result;
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The collection of coordinates representing the poolygon holes.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(shell, holes);
            algorithm.Compute();

            return algorithm.Result;
        }

        #endregion
    }
}
