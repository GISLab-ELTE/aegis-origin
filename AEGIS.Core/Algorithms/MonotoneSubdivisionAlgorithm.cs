/// <copyright file="MonotoneSubdivisionAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
    /// converting them to monotone subdivisions, and triangulating each monotone piece. 
    /// The algorithm can only handle simple polygons without holes. When a polygon with holes is specified, the holes will be ignored.
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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonotoneSubdivisionAlgorithm"/> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public MonotoneSubdivisionAlgorithm(IList<Coordinate> shell)
            : this(shell, PrecisionModel.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonotoneSubdivisionAlgorithm"/> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public MonotoneSubdivisionAlgorithm(IList<Coordinate> shell, PrecisionModel precisionModel)
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
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
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
                                                              shell[(shell.Count + coordinateIndex + 1) % coordinateCount], PrecisionModel);
                nextShell.Remove(nextShell[coordinateIndex]);
                if (WindingNumberAlgorithm.IsInsidePolygon(shell, centroid, PrecisionModel) && !ShamosHoeyAlgorithm.Intersects(nextShell, PrecisionModel))
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
        /// <param name="source">The polygon.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IList<Coordinate[]> Triangulate(IBasicPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.Shell == null || source.Shell.Coordinates == null)
                return null;

            return new MonotoneSubdivisionAlgorithm(source.Shell.Coordinates).Result;
        }

        /// <summary>
        /// Determines the triangles of the polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IList<Coordinate[]> Triangulate(IBasicPolygon source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.Shell == null || source.Shell.Coordinates == null)
                return null;

            return new MonotoneSubdivisionAlgorithm(source.Shell.Coordinates, precisionModel).Result;
        }

        /// <summary>
        /// Determines the triangles of the polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IList<Coordinate[]> Triangulate(IList<Coordinate> source)
        {
            return new MonotoneSubdivisionAlgorithm(source).Result;
        }

        /// <summary>
        /// Determines the triangles of the polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IList<Coordinate[]> Triangulate(IList<Coordinate> source, PrecisionModel precisionModel)
        {
            return new MonotoneSubdivisionAlgorithm(source, precisionModel).Result;
        }

        #endregion
    }
}
