/// <copyright file="IGeometryGraphFactory.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for factories producing <see cref="IGeometryGraph" /> instances.
    /// </summary>
    public interface IGeometryGraphFactory : IFactory
    {
        #region Factory methods for graphs

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <returns>An empty graph.</returns>
        IGeometryGraph CreateGraph();

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty graph using the specified comparers.</returns>
        IGeometryGraph CreateGraph(IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty graph with the specified metadata using the specified comparers.</returns>
        IGeometryGraph CreateGraph(IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty graph with the specified metadata.</returns>
        IGeometryGraph CreateGraph(IDictionary<String, Object> metadata);
        
        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <returns>A graph containing the specified coordinates as vertices.</returns>
        IGeometryGraph CreateGraph(IEnumerable<Coordinate> coordinates);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>A graph containing the specified coordinates as vertices using the specified comparers.</returns>
        IGeometryGraph CreateGraph(IEnumerable<Coordinate> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and the metadata using the specified comparers.</returns>
        IGeometryGraph CreateGraph(IEnumerable<Coordinate> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and the metadata.</returns>
        IGeometryGraph CreateGraph(IEnumerable<Coordinate> coordinates, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges.</returns>
        IGeometryGraph CreateGraph(IEnumerable<IEnumerable<Coordinate>> coordinates);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges using the specified comparers.</returns>
        IGeometryGraph CreateGraph(IEnumerable<IEnumerable<Coordinate>> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges and the metadata using the specified comparers.</returns>
        IGeometryGraph CreateGraph(IEnumerable<IEnumerable<Coordinate>> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges and the metadata.</returns>
        IGeometryGraph CreateGraph(IEnumerable<IEnumerable<Coordinate>> coordinates, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="other">The other geometry graph.</param>
        /// <returns>A graph that matches <param name="other" />.</returns>
        IGeometryGraph CreateGraph(IGeometryGraph other);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="other">The other geometry graph.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>A graph that matches <param name="other" /> using the specified comparers.</returns>
        IGeometryGraph CreateGraph(IGeometryGraph other, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer);

        #endregion

        #region Factory methods for networks

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <returns>An empty network.</returns>
        IGeometryGraph CreateNetwork();

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty network with the specified metadata.</returns>
        IGeometryGraph CreateNetwork(IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <returns>A network containing the specified coordinates as vertices.</returns>
        IGeometryGraph CreateNetwork(IEnumerable<Coordinate> coordinates);

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A network containing the specified coordinates as vertices and the specified metadata.</returns>
        IGeometryGraph CreateNetwork(IEnumerable<Coordinate> coordinates, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <returns>A network containing the specified coordinates as vertices and edges.</returns>
        IGeometryGraph CreateNetwork(IEnumerable<IEnumerable<Coordinate>> coordinates);

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A network containing the specified coordinates as vertices and edges and the specified metadata.</returns>
        IGeometryGraph CreateNetwork(IEnumerable<IEnumerable<Coordinate>> coordinates, IDictionary<String, Object> metadata);

        #endregion
    }
}
