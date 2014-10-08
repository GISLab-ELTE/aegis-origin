/// <copyright file="PolygonCentroidAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents an algorithm for computing polygon centroid.
    /// </summary>
    public class PolygonCentroidAlgorithm
    {
        #region Private fields

        private IEnumerable<Coordinate> _shell;
        private IEnumerable<IEnumerable<Coordinate>> _holes;
        private Coordinate _result;
        private Boolean _hasResult;
        private Double _area;
        private Coordinate _baseCoordinate;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the shell coordinates.
        /// </summary>
        /// <value>The coordinates of the shell in counterclockwise order.</value>
        /// <exception cref="System.InvalidOperationException">The value is null.</exception>
        public IEnumerable<Coordinate> Shell
        {
            get { return _shell; }
            set 
            {
                if (value == null) throw new InvalidOperationException("The value is null.");
                if (_shell != value) { _shell = value; _hasResult = false; }
            }
        }

        /// <summary>
        /// Gets or sets the coordinates of the holes.
        /// </summary>
        /// <value>The coordinates of the holes in clockwise order.</value>
        public IEnumerable<IEnumerable<Coordinate>> Holes
        {
            get { return _holes; }
            set { if (_holes != value) _holes = value; _hasResult = false; }
        }

        /// <summary>
        /// Gets the result of the computation.
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
        /// <param name="shell">The coordinates of the shell in counterclockwise order.</param>
        public PolygonCentroidAlgorithm(IEnumerable<Coordinate> shell) 
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
        /// <param name="shell">The coordinates of the shell in counterclockwise order.</param>
        /// <param name="holes">The sequences of hole coordinates in clockwise order.</param>
        public PolygonCentroidAlgorithm(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes) 
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
        /// Computes the centroid.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The number of coordinates in less than 3.
        /// or
        /// The coordinates do not represent a surface.
        /// </exception>
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
            IList<Coordinate> shellList = _shell as IList<Coordinate>;

            if (shellList != null)
            {
                Double resultX = 0, resultY = 0;
                _area = 0;

                if (shellList.Count < 3) // if there are not enough coordinates
                {
                    throw new InvalidOperationException("The number of coordinates in less than 3.");
                }

                for (Int32 i = 0; i < shellList.Count - 1; i++)
                {
                    if (shellList[i].Z != shellList[0].Z)
                    {
                        throw new InvalidOperationException("The coordinates do not represent a surface.");
                    }

                    resultX += (shellList[i].X + shellList[i + 1].X) * (shellList[i].X * shellList[i + 1].Y - shellList[i + 1].X * shellList[i].Y);
                    resultY += (shellList[i].Y + shellList[i + 1].Y) * (shellList[i].X * shellList[i + 1].Y - shellList[i + 1].X * shellList[i].Y);
                    _area += shellList[i].X * shellList[i + 1].Y - shellList[i + 1].X * shellList[i].Y;
                }

                resultX /= (6 * _area);
                resultY /= (6 * _area);

                _result = new Coordinate(resultX, resultY, shellList[0].Z);
                _hasResult = true;
            }
            else
            {
                using (IEnumerator<Coordinate> enumerator = _shell.GetEnumerator())
                {
                    Double resultX = 0, resultY = 0, resultZ = 0;
                    _area = 0;

                    Int32 number = 0;
                    if (enumerator.MoveNext())
                    {
                        Coordinate first = enumerator.Current, second;
                        number++;
                        resultZ = first.Z;
                        while (enumerator.MoveNext())
                        {
                            second = enumerator.Current;
                            number++;

                            if (second.Z != first.Z)
                                throw new InvalidOperationException("The coordinates do not represent a surface.");

                            resultX += (first.X + second.X) * (first.X * second.Y - second.X * first.Y);
                            resultY += (first.Y + second.Y) * (first.X * second.Y - second.X * first.Y);
                            _area += first.X * second.Y - second.X * first.Y;

                            first = second;
                        }

                        resultX /= (6 * _area);
                        resultY /= (6 * _area);

                        _result = new Coordinate(resultX, resultY, resultZ);
                        _hasResult = true;
                    }

                    if (number < 3) // if there are not enough coordinates
                        throw new InvalidOperationException("The number of coordinates in less than 3.");
                }
            }
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

            IList<Coordinate> shellList = _shell as IList<Coordinate>;

            if (shellList != null)
            {
                _baseCoordinate = shellList[0];
                resultZ = shellList[0].Z;

                AddCoordinates(shellList, 1, ref resultX, ref resultY);
            }
            else
            {
                if (_shell.Count() < 3)
                    throw new InvalidOperationException("The number of coordinates in less than 3.");

                _baseCoordinate = _shell.First();
                resultZ = _baseCoordinate.Z;

                AddCoordinates(_shell, 1, ref resultX, ref resultY);
            }

            foreach (IEnumerable<Coordinate> hole in _holes)
            {
                IList<Coordinate> holeList = hole as IList<Coordinate>;

                if (holeList != null)
                    AddCoordinates(holeList, -1, ref resultX, ref resultY);
                else
                    AddCoordinates(hole, -1, ref resultX, ref resultY);
            }

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
                throw new InvalidOperationException("The number of coordinates in less than 3.");

            for (Int32 i = 0; i < coordinates.Count - 1; i++)
            {
                if (coordinates[i].Z != coordinates[0].Z)
                    throw new InvalidOperationException("The coordinates do not represent a surface.");

                AddTriangle(_baseCoordinate, coordinates[i], coordinates[i + 1], sign, ref resultX, ref resultY);
            }

            if (coordinates[coordinates.Count - 1].Z != coordinates[0].Z)
                throw new InvalidOperationException("The coordinates do not represent a surface.");
        }

        /// <summary>
        /// Add a list of coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="sign">The sign of the coordinates (positive for shell, negative for hole).</param>
        /// <param name="resultX">The X coordinate of the result.</param>
        /// <param name="resultY">The Y coordinate of the result.</param>
        /// <exception cref="System.InvalidOperationException">
        /// The coordinates do not represent a surface.
        /// or
        /// The number of coordinates in less than 3.
        /// </exception>
        protected void AddCoordinates(IEnumerable<Coordinate> coordinates, Int32 sign, ref Double resultX, ref Double resultY)
        {
            using (IEnumerator<Coordinate> enumerator = coordinates.GetEnumerator())
            {
                Int32 number = 0;

                if (enumerator.MoveNext())
                {
                    Coordinate first = enumerator.Current, second;
                    number++;

                    while (enumerator.MoveNext())
                    {
                        second = enumerator.Current;
                        number++;

                        if (second.Z != first.Z)
                            throw new InvalidOperationException("The coordinates do not represent a surface.");

                        AddTriangle(_baseCoordinate, first, second, sign, ref resultX, ref resultY);
                        first = second;
                    }
                }

                if (number < 3) // if there are not enough coordinates
                {
                    throw new InvalidOperationException("The number of coordinates in less than 3.");
                }
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
        /// Compute the centroid of the polygon from the shell coordinates.
        /// </summary>
        /// <param name="shell">The coordinates of the shell in conterclockwise order.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IEnumerable<Coordinate> shell)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(shell);
            algorithm.Compute();

            return algorithm.Result;
        }

        /// <summary>
        /// Compute the centroid of the polygon from the shell and hole coordinates.
        /// </summary>
        /// <param name="shell">The coordinates of the shell in counterclockwise order.</param>
        /// <param name="holes">The sequences of hole coordinates in clockwise order.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(shell, holes);
            algorithm.Compute();

            return algorithm.Result;
        }

        #endregion
    }
}
