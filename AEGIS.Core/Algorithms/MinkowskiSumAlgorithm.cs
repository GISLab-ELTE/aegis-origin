/// <copyright file="MinkowskiSumAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gréta Bereczki</author>

namespace ELTE.AEGIS.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the Minkowski addition algorithm for computing the buffer of a geometry.
    /// </summary>
    /// <remarks>
    /// The Minkowski sum algorithm is used for computing the buffer of a geometry.
    /// The algorithms assumes that the specified coordinates are valid, in counterclockwise order (in case of shell coordinates) or clockwise order (in case of hole coordinates) and in the same plane.
    /// </remarks>
    public class MinkowskiSumAlgorithm
    {
        #region Constants

        /// <summary>
        /// The number of points to create a circle of.
        /// </summary>
        private const Int32 NumberOfPoints = 128;

        #endregion

        #region Private fields

        /// <summary>
        /// The list of source shell coordinates.
        /// </summary>
        private readonly IList<Coordinate> _sourceShellCoordinates;

        /// <summary>
        /// The list of source hole coordinates.
        /// </summary>
        private readonly IList<IList<Coordinate>> _sourceHoleCoordinates;

        /// <summary>
        /// The list of buffer coordinates.
        /// </summary>
        private readonly IList<Coordinate> _bufferCoordinates;

        /// <summary>
        /// A values indicating whether the result was already computed.
        /// </summary>
        private Boolean _hasResult;

        /// <summary>
        /// The result shell coordinates.
        /// </summary>
        private IList<Coordinate> _resultShellCoordinates;

        /// <summary>
        /// The result hole coordinates.
        /// </summary>
        private IList<IList<Coordinate>> _resultHoleCoordinates;

        /// <summary>
        /// The result polygon.
        /// </summary>
        private IBasicPolygon _result;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        public IBasicPolygon Result
        {
            get
            {
                if (!_hasResult)
                {
                    Compute();
                }
                return _result;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MinkowskiSumAlgorithm"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentNullException">The buffer is null.</exception>
        /// <exception cref="System.ArgumentException">The orientation of the source is not suitable.</exception>
        /// <exception cref="System.ArgumentException">The orientation of the buffer is not suitable.</exception>
        public MinkowskiSumAlgorithm(IBasicPoint source, IBasicPolygon buffer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "The source is null.");
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), "The buffer is null.");

            _sourceShellCoordinates = new List<Coordinate>();
            _sourceShellCoordinates.Add(source.Coordinate);

            _bufferCoordinates = new List<Coordinate>();
            foreach (Coordinate coordinate in buffer.Shell.Coordinates)
                _bufferCoordinates.Add(coordinate);

            if (PolygonAlgorithms.Orientation(_bufferCoordinates) != Orientation.CounterClockwise)
                throw new ArgumentException("The orientation of the buffer is not suitable.", nameof(buffer));

            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinkowskiSumAlgorithm"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentNullException">The buffer is null.</exception>
        /// <exception cref="System.ArgumentException">The orientation of the source is not suitable.</exception>
        /// <exception cref="System.ArgumentException">The orientation of the buffer is not suitable.</exception>
        public MinkowskiSumAlgorithm(IBasicLineString source, IBasicPolygon buffer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "The source is null.");
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), "The buffer is null.");

            _sourceShellCoordinates = new List<Coordinate>();
            foreach (Coordinate coordinate in source.Coordinates)
                _sourceShellCoordinates.Add(coordinate);
            if (_sourceShellCoordinates.Count > 1 && _sourceShellCoordinates[_sourceShellCoordinates.Count - 1] != _sourceShellCoordinates[0])
                _sourceShellCoordinates.Add(_sourceShellCoordinates[0]);

            if (_sourceShellCoordinates.Count > 3 && PolygonAlgorithms.Orientation(_sourceShellCoordinates) != Orientation.CounterClockwise)
                throw new ArgumentException("The orientation of the source is not suitable.", nameof(source));

            _bufferCoordinates = new List<Coordinate>();
            foreach (Coordinate coordinate in buffer.Shell.Coordinates)
                _bufferCoordinates.Add(coordinate);

            if (PolygonAlgorithms.Orientation(_bufferCoordinates) != Orientation.CounterClockwise)
                throw new ArgumentException("The orientation of the buffer is not suitable.", nameof(buffer));

            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinkowskiSumAlgorithm"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentNullException">The buffer is null.</exception>
        /// <exception cref="System.ArgumentException">The orientation of the source is not suitable.</exception>
        /// <exception cref="System.ArgumentException">The orientation of the buffer is not suitable.</exception>
        public MinkowskiSumAlgorithm(IBasicPolygon source, IBasicPolygon buffer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "The source is null.");
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), "The buffer is null.");

            _sourceShellCoordinates = new List<Coordinate>();
            foreach (Coordinate coordinate in source.Shell.Coordinates)
                _sourceShellCoordinates.Add(coordinate);

            if (PolygonAlgorithms.Orientation(_sourceShellCoordinates) != Orientation.CounterClockwise)
                throw new ArgumentException("The orientation of the source is not suitable.", nameof(source));

            _sourceHoleCoordinates = new List<IList<Coordinate>>();
            for (Int32 holeIndex = 0; holeIndex < source.HoleCount; holeIndex++)
            {
                _sourceHoleCoordinates.Add(new List<Coordinate>());
                foreach (Coordinate coordinate in source.Holes[holeIndex].Coordinates)
                    _sourceHoleCoordinates[holeIndex].Add(coordinate);
                if (PolygonAlgorithms.Orientation(_sourceHoleCoordinates[holeIndex]) != Orientation.Clockwise)
                    throw new ArgumentException("The orientation of the source is not suitable.", nameof(source));
            }

            _bufferCoordinates = new List<Coordinate>();
            foreach (Coordinate coordinate in buffer.Shell.Coordinates)
                _bufferCoordinates.Add(coordinate);

            if (PolygonAlgorithms.Orientation(_bufferCoordinates) != Orientation.CounterClockwise)
                throw new ArgumentException("The orientation of the buffer is not suitable.", nameof(buffer));

            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinkowskiSumAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentNullException">The buffer is null.</exception>
        /// <exception cref="System.ArgumentException">The orientation of the source is not suitable.</exception>
        /// <exception cref="System.ArgumentException">The orientation of the buffer is not suitable.</exception>
        public MinkowskiSumAlgorithm(IList<Coordinate> source, IBasicPolygon buffer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "The source is null.");
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), "The buffer is null.");

            _sourceShellCoordinates = source;
            if (_sourceShellCoordinates.Count > 1 && _sourceShellCoordinates[_sourceShellCoordinates.Count - 1] != _sourceShellCoordinates[0])
                _sourceShellCoordinates.Add(_sourceShellCoordinates[0]);

            if (_sourceShellCoordinates.Count > 3 && PolygonAlgorithms.Orientation(_sourceShellCoordinates) != Orientation.CounterClockwise)
                throw new ArgumentException("The orientation of the source is not suitable.", nameof(source));

            _bufferCoordinates = new List<Coordinate>();
            foreach (Coordinate coordinate in buffer.Shell.Coordinates)
                _bufferCoordinates.Add(coordinate);

            if (PolygonAlgorithms.Orientation(_bufferCoordinates) != Orientation.CounterClockwise)
                throw new ArgumentException("The orientation of the buffer is not suitable.", nameof(buffer));

            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        public void Compute()
        {
            _resultShellCoordinates = new List<Coordinate>();
            _resultHoleCoordinates = new List<IList<Coordinate>>();

            if (_sourceShellCoordinates.Count == 1)
                ComputeMinkowskiSumOfPoint(_sourceShellCoordinates);
            else
            {
                ComputeMinkowskiSumOfCoordinateList(_sourceShellCoordinates, false);

                if (_sourceHoleCoordinates != null)
                    for (Int32 holeIndex = 0; holeIndex < _sourceHoleCoordinates.Count; holeIndex++)
                        ComputeMinkowskiSumOfCoordinateList(_sourceHoleCoordinates[holeIndex], true);
            }

            _result = new BasicPolygon(_resultShellCoordinates, _resultHoleCoordinates);
            _hasResult = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the Minkowski sum of a point.
        /// </summary>
        /// <param name="sourceCoordinates">The source coordinates.</param>
        private void ComputeMinkowskiSumOfPoint(IList<Coordinate> sourceCoordinates)
        {
            for (Int32 bufferIndex = 0; bufferIndex < _bufferCoordinates.Count - 1; bufferIndex++)
                _resultShellCoordinates.Add(new Coordinate(_bufferCoordinates[bufferIndex].X + sourceCoordinates[0].X, _bufferCoordinates[bufferIndex].Y + sourceCoordinates[0].Y));
        }

        /// <summary>
        /// Computes the Minkowski sum of a polygon.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="isHole">if set to <c>true</c> [is hole].</param>
        private void ComputeMinkowskiSumOfCoordinateList(IList<Coordinate> source, Boolean isHole)
        {
            List<Coordinate> resultCoordinates = new List<Coordinate>();
            for (Int32 sourceIndex = 0; sourceIndex < source.Count - 1; sourceIndex++)
            {
                // find the buffer polygon edges that lie between the corresponding source polygon edges
                List<Tuple<Coordinate, Coordinate>> edges = new List<Tuple<Coordinate, Coordinate>>();
                edges = GetConvolutionEdges(source, sourceIndex);

                // sort edges in counterclockwise order and add them to the result distinctly 
                edges = SortEdgesInCounterclockwiseOrder(edges);
                for (Int32 edgeindex = 0; edgeindex < edges.Count; edgeindex++)
                    resultCoordinates.Add(new Coordinate(edges[edgeindex].Item1.X, edges[edgeindex].Item1.Y));
                if (edges.Count != 0)
                    resultCoordinates.Add(new Coordinate(edges[edges.Count - 1].Item2.X, edges[edges.Count - 1].Item2.Y));
                else if (resultCoordinates.Count >= 2)
                {
                    Int32 count = resultCoordinates.Count - 1;
                    Coordinate coordinate = new Coordinate(source[sourceIndex].X + resultCoordinates[count].X - resultCoordinates[count - 1].X,
                        source[sourceIndex].Y + resultCoordinates[count].Y - resultCoordinates[count - 1].Y);
                    resultCoordinates.Add(coordinate);
                }
            }

            // compute the resulting polygon
            if (resultCoordinates.Count != 0)
            {
                resultCoordinates.Add(resultCoordinates[0]);
                ComputeResultPolygon(resultCoordinates, isHole);
            }
        }

        /// <summary>
        /// Creates the result polygon with separated shell and hole coordinates.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="isHole">if set to <c>true</c> [is hole].</param>
        private void ComputeResultPolygon(IList<Coordinate> coordinates, Boolean isHole)
        {
            BentleyOttmannAlgorithm algorithm = new BentleyOttmannAlgorithm(coordinates);
            List<Coordinate> intersections = new List<Coordinate>(algorithm.Intersections);
            List<Tuple<Int32, Int32>> edgeIndices = new List<Tuple<Int32, Int32>>(algorithm.EdgeIndices);

            // add edge intersection coordinates to the list of coordinates in counterclockwise order
            List<Coordinate> coordinatesWithIntersections = new List<Coordinate>();
            coordinatesWithIntersections = GetCoordinatesWithIntersections(coordinates, intersections, edgeIndices);

            // remove unnecessary internal coordinates and create holes
            List<List<Coordinate>> internalPolygons = new List<List<Coordinate>>();
            List<List<Coordinate>> holes = new List<List<Coordinate>>();
            List<Int32> nonShellIndexes = new List<Int32>();
            Int32 internalPolygonIndex = -1;
            for (Int32 coordinateIndex = 0; coordinateIndex < coordinatesWithIntersections.Count; coordinateIndex++)
            {
                if (internalPolygonIndex != -1)
                {
                    internalPolygons[internalPolygonIndex].Add(coordinatesWithIntersections[coordinateIndex]);
                    nonShellIndexes.Add(coordinateIndex);
                }

                if (intersections.Any(x => x.Equals(coordinatesWithIntersections[coordinateIndex])))
                {
                    Int32 matchingPolygonIndex = internalPolygons.FindIndex(x => x[0].Equals(coordinatesWithIntersections[coordinateIndex]));
                    if (internalPolygonIndex != -1 && internalPolygonIndex < internalPolygons.Count && matchingPolygonIndex != -1)
                    {
                        if (PolygonAlgorithms.Orientation(internalPolygons[matchingPolygonIndex]) == Orientation.Clockwise)
                            holes.Add(internalPolygons[matchingPolygonIndex]);

                        for (Int32 polygonIndex = internalPolygons.Count - 1; polygonIndex >= matchingPolygonIndex; polygonIndex--)
                            internalPolygons.RemoveAt(polygonIndex);

                        internalPolygonIndex = matchingPolygonIndex - 1;
                    }
                    else
                    {
                        internalPolygonIndex++;
                        internalPolygons.Add(new List<Coordinate>() { coordinatesWithIntersections[coordinateIndex] });
                    }
                }
            }

            for (Int32 i = 0; i < nonShellIndexes.Count; i++)
                coordinatesWithIntersections.RemoveAt(nonShellIndexes[nonShellIndexes.Count - 1 - i]);

            // add shell and hole coordinates to the result
            if (isHole)
                _resultHoleCoordinates.Add(new List<Coordinate>(coordinatesWithIntersections));
            else
            {
                _resultShellCoordinates = coordinatesWithIntersections;
                foreach (List<Coordinate> hole in holes)
                    _resultHoleCoordinates.Add(hole);
            }
        }

        /// <summary>
        /// Gets the buffer edges that lie between the corresponding source edges.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="sourceIndex">Index of the source coordinate.</param>
        /// <returns>
        /// The list of edges.
        /// </returns>
        private List<Tuple<Coordinate, Coordinate>> GetConvolutionEdges(IList<Coordinate> coordinates, Int32 sourceIndex)
        {
            List<Tuple<Coordinate, Coordinate>> edges = new List<Tuple<Coordinate, Coordinate>>();
            Double previousElementX = sourceIndex != 0 ? coordinates[sourceIndex - 1].X : coordinates[coordinates.Count - 2].X;
            Double previousElementY = sourceIndex != 0 ? coordinates[sourceIndex - 1].Y : coordinates[coordinates.Count - 2].Y;

            Double firstAngle = Math.Atan2(coordinates[sourceIndex].Y - previousElementY, coordinates[sourceIndex].X - previousElementX);
            Double secondAngle = Math.Atan2(coordinates[sourceIndex + 1].Y - coordinates[sourceIndex].Y, coordinates[sourceIndex + 1].X - coordinates[sourceIndex].X);
            if (firstAngle < 0 && secondAngle == 0)
                secondAngle = 2 * Math.PI;
            if (firstAngle < 0)
                firstAngle = 2 * Math.PI + firstAngle;
            if (secondAngle < 0)
                secondAngle = 2 * Math.PI + secondAngle;

            for (Int32 bufferIndex = 0; bufferIndex < _bufferCoordinates.Count - 1; bufferIndex++)
            {
                Double middleAngle = Math.Atan2(_bufferCoordinates[bufferIndex + 1].Y - _bufferCoordinates[bufferIndex].Y, _bufferCoordinates[bufferIndex + 1].X - _bufferCoordinates[bufferIndex].X);
                if (middleAngle < 0)
                    middleAngle = 2 * Math.PI + middleAngle;

                Boolean addCondition = false;
                if (PolygonAlgorithms.Orientation(coordinates) == Orientation.Clockwise)
                {
                    if (firstAngle >= secondAngle)
                        addCondition = (middleAngle <= firstAngle) && (middleAngle >= secondAngle);
                    else
                        addCondition = middleAngle <= firstAngle || middleAngle >= secondAngle;
                }
                else
                {
                    if (firstAngle <= secondAngle)
                        addCondition = middleAngle >= firstAngle && middleAngle <= secondAngle;
                    else
                        addCondition = (middleAngle > firstAngle) || (middleAngle < secondAngle);
                }

                if (addCondition)
                {
                    edges.Add(
                        Tuple.Create(
                            new Coordinate(
                                coordinates[sourceIndex].X + _bufferCoordinates[bufferIndex].X,
                                coordinates[sourceIndex].Y + _bufferCoordinates[bufferIndex].Y),
                            new Coordinate(
                                coordinates[sourceIndex].X + _bufferCoordinates[bufferIndex + 1].X,
                                coordinates[sourceIndex].Y + _bufferCoordinates[bufferIndex + 1].Y)));
                }
            }
            return edges;
        }

        /// <summary>
        /// Gets the list of coordinates with intersections in counterclockwise order.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="intersections">The intersections.</param>
        /// <param name="edgeIndices">The edge indices.</param>
        /// <returns>
        /// The list of coordinates with intersections.
        /// </returns>
        private List<Coordinate> GetCoordinatesWithIntersections(IList<Coordinate> coordinates, List<Coordinate> intersections, List<Tuple<Int32, Int32>> edgeIndices)
        {
            List<Coordinate> coordinatesWithIntersections = new List<Coordinate>();
            // add intersection coordinates to the list of coordinates in the right order
            for (Int32 coordinateIndex = 0; coordinateIndex < coordinates.Count - 1; coordinateIndex++)
            {
                List<Coordinate> intersectionCoordinates = new List<Coordinate>();
                coordinatesWithIntersections.Add(coordinates[coordinateIndex]);

                for (Int32 edgeIndex = 0; edgeIndex < edgeIndices.Count; edgeIndex++)
                    if (edgeIndices[edgeIndex].Item2.Equals(coordinateIndex))
                        intersectionCoordinates.Add(intersections[edgeIndex]);

                for (Int32 edgeIndex = 0; edgeIndex < edgeIndices.Count; edgeIndex++)
                    if (edgeIndices[edgeIndex].Item1.Equals(coordinateIndex))
                        intersectionCoordinates.Add(intersections[edgeIndex]);

                intersectionCoordinates = SortCoordinatesOnALine(intersectionCoordinates, coordinates[coordinateIndex], coordinates[coordinateIndex + 1]);

                for (Int32 j = 0; j < intersectionCoordinates.Count; j++)
                    coordinatesWithIntersections.Add(intersectionCoordinates[j]);
            }
            coordinatesWithIntersections.Add(coordinates[coordinates.Count - 1]);

            // remove duplicate coordinates
            List<Int32> indexesToRemove = new List<Int32>();

            for (Int32 coordinateIndex = 0; coordinateIndex < coordinatesWithIntersections.Count - 1; coordinateIndex++)
                if (coordinatesWithIntersections[coordinateIndex] == coordinatesWithIntersections[coordinateIndex + 1])
                    indexesToRemove.Add(coordinateIndex);

            for (Int32 i = indexesToRemove.Count - 1; i >= 0; i--)
                coordinatesWithIntersections.RemoveAt(indexesToRemove[i]);

            return coordinatesWithIntersections;
        }

        /// <summary>
        /// Sorts the edges in counterclockwise order.
        /// </summary>
        /// <param name="edges">The edges.</param>
        /// <returns>The sorted edges.</returns>
        private List<Tuple<Coordinate, Coordinate>> SortEdgesInCounterclockwiseOrder(List<Tuple<Coordinate, Coordinate>> edges)
        {
            List<Tuple<Coordinate, Coordinate>> sortedEdges = new List<Tuple<Coordinate, Coordinate>>();
            Int32 startEdgeIndex = edges.FindIndex(x => !edges.Any(y => x.Item1.Equals(y.Item2)));
            if (startEdgeIndex != -1)
            {
                sortedEdges.Add(edges[startEdgeIndex]);
                for (Int32 edgeIndex = 0; edgeIndex < edges.Count; edgeIndex++)
                {
                    Int32 nextEdgeIndex = edges.FindIndex(x => x.Item1.Equals(sortedEdges[edgeIndex].Item2));
                    if (nextEdgeIndex != -1)
                        sortedEdges.Add(edges[nextEdgeIndex]);
                    else
                        break;
                }
            }

            return sortedEdges;
        }

        /// <summary>
        /// Sorts coordinates laying on a given line.
        /// </summary>
        /// <param name="coordinates">The coordinates to sort.</param>
        /// <param name="lineStart">The line start.</param>
        /// <param name="lineEnd">The line end.</param>
        /// <returns>The sorted coordinates.</returns>
        private List<Coordinate> SortCoordinatesOnALine(List<Coordinate> coordinates, Coordinate lineStart, Coordinate lineEnd)
        {
            Boolean descendingX = lineStart.X >= lineEnd.X ? true : false;
            Boolean descendingY = lineStart.Y >= lineEnd.Y ? true : false;

            if (descendingX && descendingY)
                coordinates = coordinates.OrderByDescending(p => p.X).ThenByDescending(p => p.Y).ToList();
            else if (descendingX && !descendingY)
                coordinates = coordinates.OrderByDescending(p => p.X).ThenBy(p => p.Y).ToList();
            else if (!descendingX && descendingY)
                coordinates = coordinates.OrderBy(p => p.X).ThenByDescending(p => p.Y).ToList();
            else
                coordinates = coordinates.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            return coordinates;
        }

        /// <summary>
        /// Creates a circle as a polygon with the given radius and number of points.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="numberOfPoints">The number of points.</param>
        /// <returns>The coordinate list of the circle.</returns>
        private static IList<Coordinate> CreateCircle(Double radius, Int32 numberOfPoints)
        {
            IList<Coordinate> coordinates = new List<Coordinate>();
            for (Int32 i = 0; i < numberOfPoints; i++)
            {
                Double angle = (Math.PI / 180) * i * (360 / numberOfPoints);
                Double x = radius * Math.Cos(angle);
                Double y = radius * Math.Sin(angle);
                coordinates.Add(new Coordinate(x, y));
            }

            return coordinates;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicPoint source, IBasicPolygon buffer)
        {
            return new MinkowskiSumAlgorithm(source, buffer).Result;
        }

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicLineString source, IBasicPolygon buffer)
        {
            return new MinkowskiSumAlgorithm(source, buffer).Result;
        }

        /// <summary>
        ///  Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicPolygon source, IBasicPolygon buffer)
        {
            return new MinkowskiSumAlgorithm(source, buffer).Result;
        }

        /// <summary>
        ///  Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IList<Coordinate> source, IBasicPolygon buffer)
        {
            return new MinkowskiSumAlgorithm(source, buffer).Result;
        }

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicPoint source, Double radius)
        {
            return new MinkowskiSumAlgorithm(source, new BasicPolygon(CreateCircle(radius, NumberOfPoints))).Result;
        }

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicLineString source, Double radius)
        {
            return new MinkowskiSumAlgorithm(source, new BasicPolygon(CreateCircle(radius, NumberOfPoints))).Result;
        }

        /// <summary>
        ///  Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicPolygon source, Double radius)
        {
            return new MinkowskiSumAlgorithm(source, new BasicPolygon(CreateCircle(radius, NumberOfPoints))).Result;
        }

        /// <summary>
        ///  Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IList<Coordinate> source, Double radius)
        {
            return new MinkowskiSumAlgorithm(source, new BasicPolygon(CreateCircle(radius, NumberOfPoints))).Result;
        }

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="numberOfPoints">The number of points.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicPoint source, Double radius, Int32 numberOfPoints)
        {
            return new MinkowskiSumAlgorithm(source, new BasicPolygon(CreateCircle(radius, numberOfPoints))).Result;
        }

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="numberOfPoints">The number of points.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicLineString source, Double radius, Int32 numberOfPoints)
        {
            return new MinkowskiSumAlgorithm(source, new BasicPolygon(CreateCircle(radius, numberOfPoints))).Result;
        }

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="numberOfPoints">The number of points.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IBasicPolygon source, Double radius, Int32 numberOfPoints)
        {
            return new MinkowskiSumAlgorithm(source, new BasicPolygon(CreateCircle(radius, numberOfPoints))).Result;
        }

        /// <summary>
        /// Computes the buffer of the source geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="numberOfPoints">The number of points.</param>
        /// <returns>The result polygon.</returns>
        public static IBasicPolygon Buffer(IList<Coordinate> source, Double radius, Int32 numberOfPoints)
        {
            return new MinkowskiSumAlgorithm(source, new BasicPolygon(CreateCircle(radius, numberOfPoints))).Result;
        }

        #endregion
    }
}
