/// <copyright file="CyrusBeckAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Daniel Ballagi</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for performing the Cyrus–Beck algorithm.
    /// </summary>
    /// <remarks>
    /// The Cyrus–Beck algorithm is a computational geometry algorithm used for line clipping using a convex polygon 
    /// clipping window. It was designed to be more efficient than the <see cref="CohenSutherlandAlgorithm"/> which 
    /// uses repetitive clipping.
    /// </remarks>
    public class CyrusBeckAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The collection of source line strings.
        /// </summary>
        private IEnumerable<IList<Coordinate>> _source;

        /// <summary>
        /// The clipping window.
        /// </summary>
        private IList<Coordinate> _window;

        /// <summary>
        /// The list of clipped line strings.
        /// </summary>
        private List<IList<Coordinate>> _result;

        /// <summary>
        /// A values indicating whether the result was already computed.
        /// </summary>
        private Boolean _hasResult;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CyrusBeckAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The list of coordinates to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public CyrusBeckAlgorithm(IList<Coordinate> source, Envelope window)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (window == null)
                throw new ArgumentNullException("window", "The clipping window is null.");

            _source = new List<IList<Coordinate>>();
            (_source as List<IList<Coordinate>>).Add(source);
            _window = new List<Coordinate>()
                {
                    new Coordinate(window.MinX, window.MinY),
                    new Coordinate(window.MinX, window.MaxY),
                    new Coordinate(window.MaxX, window.MaxY),
                    new Coordinate(window.MaxX, window.MinY)
                };
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyrusBeckAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The list of coordinates to clip.</param>
        /// <param name="window">The list of coordinates of the clipping window.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public CyrusBeckAlgorithm(IList<Coordinate> source, IList<Coordinate> window)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (window == null)
                throw new ArgumentNullException("window", "The clipping window is null.");

            _source = new List<IList<Coordinate>>();
            (_source as List<IList<Coordinate>>).Add(source);
            _window = window;
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyrusBeckAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The collection of line strings to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public CyrusBeckAlgorithm(IEnumerable<IList<Coordinate>> source, Envelope window)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (window == null)
                throw new ArgumentNullException("window", "The clipping window is null.");

            _source = source;
            _window = new List<Coordinate>()
                {
                    new Coordinate(window.MinX, window.MinY),
                    new Coordinate(window.MinX, window.MaxY),
                    new Coordinate(window.MaxX, window.MaxY),
                    new Coordinate(window.MaxX, window.MinY)
                };
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyrusBeckAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The collection of line strings to clip.</param>
        /// <param name="window">The list of coordinates of the clipping window.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public CyrusBeckAlgorithm(IEnumerable<IList<Coordinate>> source, IList<Coordinate> window)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (window == null)
                throw new ArgumentNullException("window", "The clipping window is null.");

            _source = source;
            _window = window;
            _hasResult = false;
        }

        #endregion
        
        #region Public properties

        /// <summary>
        /// Gets the collection of source line strings.
        /// </summary>
        public IEnumerable<IList<Coordinate>> Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Gets the clipping window.
        /// </summary>
        public IList<Coordinate> Window
        {
            get { return _window; }
        }

        /// <summary>
        /// Gets the collection of clipped line strings.
        /// </summary>
        public IEnumerable<IList<Coordinate>> Result
        {
            get
            {
                if (!_hasResult)
                    Compute();

                return _result;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the result of the algorithm.
        /// </summary>
        public void Compute()
        {
            _result = new List<IList<Coordinate>>();
            
            List<Coordinate> windowNormals = ComputeNormals(_window);
            
            foreach (IList<Coordinate> line in _source)
            {
                List<Coordinate> retLineString = new List<Coordinate>();

                for (Int32 index = 0; index < line.Count - 1; ++index)
                {
                    Coordinate[] calculatedCoordinates = Clip(line[index], line[index + 1], _window, windowNormals);

                    if (calculatedCoordinates == null)
                        continue;

                    if (retLineString.Count != 0 && retLineString.Last() == calculatedCoordinates.First())
                        retLineString.Add(calculatedCoordinates.Last());
                    else
                    {
                        if (retLineString.Count != 0)
                            _result.Add(retLineString);
                        retLineString = new List<Coordinate>();
                        retLineString.AddRange(calculatedCoordinates);
                    }
                }

                _result.Add(retLineString);
            }

            _hasResult = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Clips a line string.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <param name="window">The clipping window.</param>
        /// <param name="windowNormals">The normal vectors of the clipping window.</param>
        /// <returns>The clipped line string.</returns>
        private static Coordinate[] Clip(Coordinate first, Coordinate second, IList<Coordinate> window, List<Coordinate> windowNormals)
        {
            CoordinateVector P = second - first;

            Double minT = 0, maxT = 1;

            for (int i = 0; i < window.Count(); ++i)
            {
                Coordinate F = window[i];
                Coordinate N = windowNormals[i];
                CoordinateVector Q = first - F;

                Double Pn = CoordinateVector.DotProduct(P.X, P.Y, P.Z, N.X, N.Y, N.Z);
                Double Qn = CoordinateVector.DotProduct(Q.X, Q.Y, Q.Z, N.X, N.Y, N.Z);

                if (Pn < 0)
                    minT = Math.Max(minT, -Qn / Pn);
                else if (Pn > 0)
                    maxT = Math.Min(maxT, -Qn / Pn);
            }

            if (minT < 0 && maxT > 1 || minT > maxT)
                return null;

            Coordinate retC2 = first + maxT * P;
            Coordinate retC1 = first + minT * P;

            return new Coordinate[]
            {
                new Coordinate(retC1.X, retC1.Y),
                new Coordinate(retC2.X, retC2.Y)
            };
        }

        /// <summary>
        /// Computes the normal vectors of the edges.
        /// </summary>
        /// <param name="window">The clipping window.</param>
        /// <returns>The normal vectors.</returns>
        private static List<Coordinate> ComputeNormals(IList<Coordinate> window)
        {
            List<Coordinate> normals = new List<Coordinate>(window.Count);
            CoordinateVector direction;

            for (Int32 index = 0; index < window.Count - 1; ++index) 
            {
                direction = window[index + 1] - window[index];
                direction = direction.Normalize();

                normals.Add(new Coordinate(-direction.Y, direction.X));
            }

            direction = window[0] - window[window.Count() - 1];
            direction = direction.Normalize();

            normals.Add(new Coordinate(-direction.Y, direction.X));

            return normals;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Clips a line string with an envelope.
        /// </summary>
        /// <param name="source">The line string to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IEnumerable<IList<Coordinate>> Clip(IBasicLineString source, Envelope window)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            return new CyrusBeckAlgorithm(source.Coordinates, window).Result;
        }

        /// <summary>
        /// Clips a polygons with an envelope.
        /// </summary>
        /// <param name="source">The polygon to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IEnumerable<IList<Coordinate>> Clip(IBasicPolygon source, Envelope window)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            List<IList<Coordinate>> coordinates = new List<IList<Coordinate>>();

            coordinates.Add(source.Shell.Coordinates);

            foreach (IBasicLineString linestring in source.Holes)
            {
                coordinates.Add(linestring.Coordinates);
            }

            return new CyrusBeckAlgorithm(coordinates, window).Result;
        }

        /// <summary>
        /// Clips a collection of polygons with an envelope.
        /// </summary>
        /// <param name="source">The list of polygons to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IEnumerable<IList<Coordinate>> Clip(IList<IBasicPolygon> source, Envelope window)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            if (window == null)
                throw new ArgumentNullException("window", "The clipping window is null.");

            List<IList<Coordinate>> coordinates = new List<IList<Coordinate>>();

            foreach (IBasicPolygon basicPolygon in source)
            {
                if (basicPolygon == null || basicPolygon.Shell == null)
                    continue;

                coordinates.Add(basicPolygon.Shell.Coordinates);

                foreach (IBasicLineString hole in basicPolygon.Holes)
                {
                    if (hole == null)
                        continue;

                    coordinates.Add(hole.Coordinates);
                }
            }

            return new CyrusBeckAlgorithm(coordinates, window).Result;
        }

        #endregion
    }
}
