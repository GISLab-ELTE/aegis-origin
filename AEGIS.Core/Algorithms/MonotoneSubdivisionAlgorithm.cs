/// <copyright file="MonotoneSubdivisionAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Orsolya Harazin</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for executing the Monotone Subdivision algorithm.
    /// </summary>
    /// <remarks>
    /// Monotone subdivision is an algorithm for creating the triangulation of the polygon by partitioning to trapezoids, 
    /// converting them to monotone subdivisions, and triangulating each montone piece.
    /// </remarks>
    public class MonotoneSubdivisionAlgorithm
    {
        #region Protected fields

        /// <summary>
        /// The coordinates of the polygon shell.
        /// </summary>
        protected IList<Coordinate> _shell;

        /// <summary>
        /// The coordinates of the resulting triangles.
        /// </summary>
        protected IList<Coordinate[]> _result;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        protected Boolean _hasResult;

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
        /// Gets the result.
        /// </summary>
        /// <value>The list of coordinates of the simplified line string.</value>
        public IList<Coordinate[]> Result
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
        /// Initializes a new instance of the <see cref="MonotoneSubdivisionAlgorithm"/> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public MonotoneSubdivisionAlgorithm(IList<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            if (shell[0].Equals(shell[shell.Count - 1]))
            {
                _shell = shell;
            }
            else
            {
                _shell = new List<Coordinate>(shell);
                _shell.Add(shell[0]);
            }

            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Partitions a polygon by triangles.
        /// </summary>
        public void Compute()
        {
            // source: http://www.codeproject.com/Articles/8238/Polygon-Triangulation-in-C

            _result = new List<Coordinate[]>();

            IList<Coordinate> shell = new List<Coordinate>(_shell);
            IList<Coordinate> nextShell = new List<Coordinate>(shell);            
            Coordinate[] triangle;

            Int32 coordinateCount = shell.Count - 1;
            Int32 coordinateIndex = 1;
            while (shell.Count != 4)
            {
                Coordinate centroid = LineAlgorithms.Centroid(shell[(shell.Count + coordinateIndex - 1) % coordinateCount],
                                                              shell[(shell.Count + coordinateIndex + 1) % coordinateCount]);
                nextShell.Remove(nextShell[coordinateIndex]);
                if (WindingNumberAlgorithm.IsInsidePolygon(shell, centroid) && !ShamosHoeyAlgorithm.Intersects(nextShell))
                {
                    triangle = new Coordinate[3]
                    {
                        shell[(coordinateCount + coordinateIndex - 1) % coordinateCount],
                        shell[coordinateIndex],
                        shell[(coordinateCount + coordinateIndex + 1) % coordinateCount]
                    };
                    _result.Add(triangle);
                    shell.Remove(shell[coordinateIndex]);
                    coordinateIndex = 1;
                    coordinateCount--;
                }
                else
                {
                    coordinateIndex = (coordinateIndex + 1) % (shell.Count - 1);
                    nextShell = new List<Coordinate>(shell);
                }
            }
            triangle = new Coordinate[3] { shell[0], shell[1], shell[2] };
            _result.Add(triangle);
            _hasResult = true;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines the triangles of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IList<Coordinate[]> Triangulate(IList<Coordinate> shell)
        {
            return new MonotoneSubdivisionAlgorithm(shell).Result;
        }

        #endregion
    }
}
