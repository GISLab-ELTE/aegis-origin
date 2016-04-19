/// <copyright file="PolygonCentroidAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

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
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        public PolygonCentroidAlgorithm(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes) 
            : this(shell, holes, PrecisionModel.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCentroidAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        public PolygonCentroidAlgorithm(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, PrecisionModel precisionModel)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            _shell = shell;
            _holes = holes;
            _result = Coordinate.Undefined;
            _hasResult = false;
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the centroid of the polygon.
        /// </summary>
        public void Compute()
        {
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
                resultX += (_shell[i].X + _shell[i + 1].X) * (_shell[i].X * _shell[i + 1].Y - _shell[i + 1].X * _shell[i].Y);
                resultY += (_shell[i].Y + _shell[i + 1].Y) * (_shell[i].X * _shell[i + 1].Y - _shell[i + 1].X * _shell[i].Y);
                _area += _shell[i].X * _shell[i + 1].Y - _shell[i + 1].X * _shell[i].Y;
            }

            _area /= 2;

            resultX /= (6 * _area);
            resultY /= (6 * _area);

            _result = PrecisionModel.MakePrecise(new Coordinate(resultX, resultY, _shell[0].Z));
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

            foreach (IList<Coordinate> hole in _holes)
            {
                if (hole == null)
                    continue;

                AddCoordinates(hole, -1, ref resultX, ref resultY);            
            }

            resultX = resultX / 3 / _area;
            resultY = resultY / 3 / _area;

            _result = PrecisionModel.MakePrecise(new Coordinate(resultX, resultY, resultZ));
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
                return;

            for (Int32 i = 0; i < coordinates.Count - 1; i++)
            {
                AddTriangle(_baseCoordinate, coordinates[i], coordinates[i + 1], sign, ref resultX, ref resultY);
            }
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
            return ComputeCentroid(source, PrecisionModel.Default);
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IBasicPolygon source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.Shell == null || source.Shell.Coordinates == null)
                return Coordinate.Undefined;

            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(source.Shell.Coordinates, source.Holes != null ? source.Holes.Select(hole => hole != null ? hole.Coordinates : null) : null, precisionModel);
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
            return ComputeCentroid(shell, PrecisionModel.Default);
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IList<Coordinate> shell, PrecisionModel precisionModel)
        {
            return ComputeCentroid(shell, null, precisionModel);
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes)
        {
            return ComputeCentroid(shell, holes, PrecisionModel.Default);
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, PrecisionModel precisionModel)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(shell, holes, precisionModel);
            algorithm.Compute();

            return algorithm.Result;
        }

        #endregion
    }
}
