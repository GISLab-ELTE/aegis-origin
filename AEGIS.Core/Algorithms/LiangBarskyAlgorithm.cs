// <copyright file="LiangBarskyAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for performing the Liang–Barsky algorithm.
    /// </summary>
    /// <remarks>
    /// The Liang–Barsky algorithm is a computational geometry algorithm used for line clipping using a rectangular
    /// clipping window. The algorithm uses the parametric equation of a line and inequalities describing the range of
    /// the clipping window to determine the intersections between the line and the clipping window. With these
    /// intersections it knows which portion of the line should be drawn. This algorithm is significantly more
    /// efficient than <see cref="CohenSutherlandAlgorithm" />. The idea of the Liang-Barsky clipping algorithm is to
    /// do as much testing as possible before computing line intersections.
    /// </remarks>
    /// <author>Daniel Ballagi</author>
    public class LiangBarskyAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The collection of source line strings.
        /// </summary>
        private IEnumerable<IList<Coordinate>> _source;

        /// <summary>
        /// The clipping window.
        /// </summary>
        private Envelope _window;

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
        /// Initializes a new instance of the <see cref="LiangBarskyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The list of coordinates to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public LiangBarskyAlgorithm(IList<Coordinate> source, Envelope window)
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
        /// Initializes a new instance of the <see cref="LiangBarskyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The list of line strings to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public LiangBarskyAlgorithm(IEnumerable<IList<Coordinate>> source, Envelope window)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (window == null)
                throw new ArgumentNullException("window", "The clipping window is null.");

            _source = new List<IList<Coordinate>>();
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
        public Envelope Window
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

            foreach (IList<Coordinate> line in _source)
            {
                List<Coordinate> retLineString = new List<Coordinate>();

                for (int i = 0; i < line.Count() - 1; ++i)
                {
                    Coordinate[] calculatedCoordinates = Clip(line[i], line[i + 1], _window);

                    if (calculatedCoordinates == null)
                        continue;

                    if (retLineString.Count() != 0 && retLineString.Last() == calculatedCoordinates.First())
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
        /// Clips a line.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="envelope">The clipping window.</param>
        /// <returns>The clipped line.</returns>
        private static Coordinate[] Clip(Coordinate first, Coordinate second, Envelope envelope)
        {
            Double minT = 0, maxT = 1;
            Double deltaX = second.X - first.X;
            Double deltaY = second.Y - first.Y;
            Double p = 0, q = 0, r = 0;

            for (Int32 edge = 0; edge < 4; ++edge)
            {
                if (edge == 0)
                {
                    p = -deltaX;
                    q = first.X - envelope.MinX;
                }
                if (edge == 1)
                {
                    p = deltaX;
                    q = envelope.MaxX - first.X;
                }
                if (edge == 2)
                {
                    p = -deltaY;
                    q = first.Y - envelope.MinY;
                }
                if (edge == 3)
                {
                    p = deltaY;
                    q = envelope.MaxY - first.Y;
                }

                if (p == 0 && q < 0)
                    return null;

                r = q / p;

                if (p < 0)
                {
                    if (r > maxT)
                        return null;
                    else if (r > minT)
                        minT = r;
                }
                else if (p > 0)
                {
                    if (r < minT)
                        return null;
                    else if (r < maxT)
                        maxT = r;
                }
            }

            return new Coordinate[]
            {
                new Coordinate(first.X + minT * deltaX, first.Y + minT * deltaY),
                new Coordinate(first.X + maxT * deltaX, first.Y + maxT * deltaY)
            };
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

            return new LiangBarskyAlgorithm(source.Coordinates, window).Result;
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

            foreach (IBasicLineString hole in source.Holes)
            {
                if (hole == null)
                    continue;

                coordinates.Add(hole.Coordinates);
            }

            return new LiangBarskyAlgorithm(coordinates, window).Result;
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

            return new LiangBarskyAlgorithm(coordinates, window).Result;
        }

        #endregion
    }
}
