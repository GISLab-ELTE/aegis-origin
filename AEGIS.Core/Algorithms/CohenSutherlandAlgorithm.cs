/// <copyright file="CohenSutherlandAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Daniel Ballagi</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for performing the Cohen–Sutherland algorithm.
    /// </summary>
    /// <remarks>
    /// The Cohen–Sutherland algorithm is a computational geometry algorithm used for line clipping using a rectangular
    /// clipping window. The algorithm divides a two-dimensional space into 9 regions, and then efficiently determines 
    /// the lines and portions of lines that are visible in the center region of interest (the viewport).
    /// </remarks>
    public class CohenSutherlandAlgorithm
    {
        #region Private types

        /// <summary>
        /// The bits represent the location of the point in relation to the viewport.
        /// </summary>
        private enum OutCode
        {
            /// <summary>
            /// Indicates that the coordinate is inside the envelope.
            /// </summary>
            Inside = 0,

            /// <summary>
            /// Indicates that the coordinate is left to the envelope.
            /// </summary>
            Left = 1,

            /// <summary>
            /// Indicates that the coordinate is right to the envelope.
            /// </summary>
            Right = 2,

            /// <summary>
            /// Indicates that the coordinate is below the envelope.
            /// </summary>
            Bottom = 4,

            /// <summary>
            /// Indicates that the coordinate is above the envelope.
            /// </summary>
            Top = 8
        }

        #endregion

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
        /// Initializes a new instance of the <see cref="CohenSutherlandAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The list of coordinates to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public CohenSutherlandAlgorithm(IList<Coordinate> source, Envelope window)
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
        /// Initializes a new instance of the <see cref="CohenSutherlandAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The list of line strings to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public CohenSutherlandAlgorithm(IEnumerable<IList<Coordinate>> source, Envelope window)
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
        public Envelope Window
        {
            get { return _window; }
        }

        /// <summary>
        /// Gets the collection of clipped line strings.
        /// </summary>
        public IList<IList<Coordinate>> Result
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

            foreach (IList<Coordinate> lineString in _source)
            {
                List<Coordinate> clippedLineString = new List<Coordinate>();

                for (Int32 index = 0; index < lineString.Count - 1; ++index)
                {
                    Coordinate[] calculatedCoordinates = Clip(lineString[index], lineString[index + 1], _window);

                    if (calculatedCoordinates == null)
                        continue;

                    if (clippedLineString.Count != 0 && clippedLineString.Last() == calculatedCoordinates.First())
                        clippedLineString.Add(calculatedCoordinates.Last());
                    else
                    {
                        if (clippedLineString.Count != 0)
                            _result.Add(clippedLineString);

                        clippedLineString = new List<Coordinate>(calculatedCoordinates);
                    }
                }

                _result.Add(clippedLineString);
            }
            
            _hasResult = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Compute the bit code of a coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The computed bitcode.</returns>
        private static OutCode ComputeOutCode(Coordinate coordinate, Envelope window) 
        { 
            return ComputeOutCode(coordinate.X, coordinate.Y, window); 
        }

        /// <summary>
        /// Compute the bit code of a coordinate.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The computed bitcode.</returns>
        private static OutCode ComputeOutCode(Double x, Double y, Envelope window)
        {
            OutCode code = OutCode.Inside;

            if (x < window.MinX)
                code |= OutCode.Left;
            if (x > window.MaxX)
                code |= OutCode.Right;
            if (y < window.MinY) 
                code |= OutCode.Top;
            if (y > window.MaxY) 
                code |= OutCode.Bottom;

            return code;
        }

        /// <summary>
        /// Clips a line.
        /// </summary>
        /// <param name="first">The first coordinate of the line.</param>
        /// <param name="second">The second coordinate of the line.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The clipped line.</returns>
        private static Coordinate[] Clip(Coordinate first, Coordinate second, Envelope window)
        {
            Boolean accept = false;

            OutCode outCodeFirst = ComputeOutCode(first, window);
            OutCode outCodeSecond = ComputeOutCode(second, window);

            while (true)
            {
                if ((outCodeFirst | outCodeSecond) == OutCode.Inside)
                {
                    accept = true;
                    break;
                }

                if ((outCodeFirst & outCodeSecond) != 0)
                    break;

                OutCode outCode = outCodeFirst != OutCode.Inside ? outCodeFirst : outCodeSecond;

                Coordinate intersectionCoordinate = ComputeIntersection(first, second, window, outCode);

                if (outCode == outCodeFirst)
                {
                    first = intersectionCoordinate;
                    outCodeFirst = ComputeOutCode(first, window);
                }

                if (outCode == outCodeSecond)
                {
                    second = intersectionCoordinate;
                    outCodeSecond = ComputeOutCode(second, window);
                }
            }

            if (accept)
                return new Coordinate[] { first, second };
            
            return null;
        }

        /// <summary>
        /// Computes the intersection of a line.
        /// </summary>
        /// <param name="first">The first coordinate of the line.</param>
        /// <param name="second">The second coordinate of the line.</param>
        /// <param name="window">The clipping window.</param>
        /// <param name="clipTo">Location of first coordinate in relation to the region of interest.</param>
        /// <returns>The intersection coordinate.</returns>
        /// <exception cref="System.ArgumentException">The code is invalid.</exception>
        private static Coordinate ComputeIntersection(Coordinate first, Coordinate second, Envelope window, OutCode clipTo)
        {
            Double dx = (second.X - first.X);
            Double dy = (second.Y - first.Y);

            Double slopeY = dx / dy;
            Double slopeX = dy / dx;

            if (clipTo.HasFlag(OutCode.Top))
            {
                return new Coordinate(
                    first.X + slopeY * (window.MaxY - first.Y),
                    window.MaxY
                );
            }

            if (clipTo.HasFlag(OutCode.Bottom))
            {
                return new Coordinate(
                    first.X + slopeY * (window.MinY - first.Y),
                    window.MinY
                );
            }

            if (clipTo.HasFlag(OutCode.Right))
            {
                return new Coordinate(
                    window.MaxX,
                    first.Y + slopeX * (window.MaxX - first.X)
                );
            }

            if (clipTo.HasFlag(OutCode.Left))
            {
                return new Coordinate(
                    window.MinX,
                    first.Y + slopeX * (window.MinX - first.X)
                );
            }

            throw new ArgumentException("The code is invalid.", "clipTo");
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

            return new CohenSutherlandAlgorithm(source.Coordinates, window).Result;
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

            return new CohenSutherlandAlgorithm(coordinates, window).Result;
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

            return new CohenSutherlandAlgorithm(coordinates, window).Result;
        }

        #endregion
    }
}
