/// <copyright file="IGeometryGraph.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines a graph form of geometries in coordinate space.
    /// </summary>
    public interface IGeometryGraph : IGeometry, IEnumerable<IGraphVertex>
    {
        #region Properties

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        /// <value>The number of vertices in the graph.</value>
        Int32 VertexCount { get; }

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        /// <value>The number of edges in the graph.</value>
        Int32 EdgeCount { get; }

        /// <summary>
        /// Gets the list of vertices.
        /// </summary>
        /// <value>The read-only list of vertices.</value>
        IList<IGraphVertex> Vertices { get; }

        /// <summary>
        /// Gets the list of edges.
        /// </summary>
        /// <value>The read-only list of edges.</value>
        IList<IGraphEdge> Edges { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer" /> object used by the graph for comparing vertices.
        /// </summary>
        /// <value>The <see cref="IEqualityComparer" /> object used by the graph for comparing vertices.</value>
        IEqualityComparer<IGraphVertex> VertexComparer { get; }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer" /> object used by the graph for comparing edges.
        /// </summary>
        /// <value>The <see cref="IEqualityComparer" /> object used by the graph for comparing edges.</value>
        IEqualityComparer<IGraphEdge> EdgeComparer { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the outgoing edges of a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The read-only set containing edges with <paramref="vertex"> as source.</returns>
        ISet<IGraphEdge> OutEdges(IGraphVertex vertex);

        /// <summary>
        /// Returns the incoming edges of a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The read-only set containing edges with <paramref="vertex"> as target.</returns>
        ISet<IGraphEdge> InEdges(IGraphVertex vertex);

        /// <summary>
        /// Adds a new vertex to a graph.
        /// </summary>
        /// <param name="coordinate">The coordinate of the vertex.</param>
        /// <returns>The vertex created at the <paramref name="coordinate" />.</returns>
        IGraphVertex AddVertex(Coordinate coordinate);

        /// <summary>
        /// Adds a new vertex to a graph.
        /// </summary>
        /// <param name="coordinate">The coordinate of the vertex.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The vertex created at the <paramref name="coordinate" />.</returns>
        IGraphVertex AddVertex(Coordinate coordinate, IDictionary<String, Object> metadata);

        /// <summary>
        /// Returns a vertex located at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The vertex located at the <paramref name="coordinate" /> if any; otherwise, <c>null</c>.</returns>
        IGraphVertex GetVertex(Coordinate coordinate);

        /// <summary>
        /// Returns all vertices located at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The list of vertices located at the <paramref name="coordinate" />.</returns>
        IList<IGraphVertex> GetAllVertices(Coordinate coordinate);

        /// <summary>
        /// Determines whether the graph contains the specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns><c>true</c> if the graph contains the <paramref name="vertex" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The vertex is null.</exception>
        Boolean Contains(IGraphVertex vertex);

        /// <summary>
        /// Adds a new edge to the graph.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <returns>The edge created between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        IGraphEdge AddEdge(Coordinate source, Coordinate target);

        /// <summary>
        /// Adds a new edge to the graph.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The edge created between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        IGraphEdge AddEdge(Coordinate source, Coordinate target, IDictionary<String, Object> metadata);

        /// <summary>
        /// Adds a new edge to the graph.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <returns>The edge created between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source vertex is not within the graph.
        /// or
        /// The target vertex is not within the graph.
        /// </exception>
        IGraphEdge AddEdge(IGraphVertex source, IGraphVertex target);

        /// <summary>
        /// Adds a new edge to the graph.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The edge created between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source vertex is not within the graph.
        /// or
        /// The target vertex is not within the graph.
        /// </exception>
        IGraphEdge AddEdge(IGraphVertex source, IGraphVertex target, IDictionary<String, Object> metadata);

        /// <summary>
        /// Returns an edge between the specified coordinates.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <returns>The edge between <paramref name="source" /> and <paramref name="target" /> coordinates.</returns>
        IGraphEdge GetEdge(Coordinate source, Coordinate target);

        /// <summary>
        /// Returns an edge between the specified vertices.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <returns>The edge between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source vertex is not within the graph.
        /// or
        /// The target vertex is not within the graph.
        /// </exception>
        IGraphEdge GetEdge(IGraphVertex source, IGraphVertex target);

        /// <summary>
        /// Returns all edges between the specified coordinates.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <returns>The read-only list of edges between <paramref name="source" /> and <paramref name="target" /> coordinates.</returns>
        IList<IGraphEdge> GetAllEdges(Coordinate source, Coordinate target);

        /// <summary>
        /// Returns all edges between the specified vertices.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <returns>The read-only list of edges between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source vertex is not within the graph.
        /// or
        /// The target vertex is not within the graph.
        /// </exception>
        IList<IGraphEdge> GetAllEdges(IGraphVertex source, IGraphVertex target);

        /// <summary>
        /// Returns the nearest vertex to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The nearest vertex to <paramref name="coordinate" /> if the graph is not empty; otherwise, <c>null</c>.</returns>
        IGraphVertex GetNearestVertex(Coordinate coordinate);

        /// <summary>
        /// Returns the nearest vertex to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="accuracy">The accuracy of the localization.</param>
        /// <returns>The first vertex within the specified <paramref name="accuracy" /> to <paramref name="coordinate" /> if any; otherwise, <c>null</c>.</returns>
        IGraphVertex GetNearestVertex(Coordinate coordinate, Double accuracy);

        /// <summary>
        /// Removes all vertices located at the specified vertex from the graph.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if at least one vertex was located and removed from the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        Boolean RemoveVertex(Coordinate coordinate);

        /// <summary>
        /// Removes the specified vertex from the graph.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns><c>true</c> if <paramref name="vertex" /> is within the graph and has been removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The vertex is null.</exception>
        Boolean RemoveVertex(IGraphVertex vertex);

        /// <summary>
        /// Removes the edge from the graph.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns><c>true</c> if the edgee was located and removed from the graph; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The edge is null.</exception>
        Boolean RemoveEdge(IGraphEdge edge);

        /// <summary>
        /// Removes all edges between the source and target coordinates from the graph.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <returns><c>true</c> if at least one edge was located and removed between <paramref name="source" /> and <paramref name="target" />; otherwise, <c>false</c>.</returns>
        Boolean RemoveEdge(Coordinate source, Coordinate target);

        /// <summary>
        /// Removes all edges between the source and target vertices from the graph.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <returns><c>true</c> if at least one edge was located and removed between <paramref name="source" /> and <paramref name="target" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        Boolean RemoveEdge(IGraphVertex source, IGraphVertex target);

        /// <summary>
        /// Returns an enumerator that iterates through the graph.
        /// </summary>
        /// <param name="strategy">The strategy of the enumeration.</param>
        /// <returns>An <see cref="IEnumerator{IGraphVertex}" /> object that can be used to iterate through the graph.</returns>
        IEnumerator<IGraphVertex> GetEnumerator(EnumerationStrategy strategy);

        #endregion
    }
}
