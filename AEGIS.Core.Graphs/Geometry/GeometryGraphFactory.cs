/// <copyright file="GeometryGraphFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a factory for producing <see cref="GeometryGraph" /> instances.
    /// </summary>
    public class GeometryGraphFactory : Factory, IGeometryGraphFactory
    {
        #region Private fields

        /// <summary>
        /// The geometry factory. This field is read-only.
        /// </summary>
        private readonly IGeometryFactory _geometryFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryGraphFactory" /> class.
        /// </summary>
        /// <param name="geometryFactory">The geometry factory.</param>
        /// <exception cref="System.ArgumentNullException">The geometry factory is null.</exception>
        public GeometryGraphFactory(IGeometryFactory geometryFactory) : base(geometryFactory)
        {
            if (geometryFactory == null)
                throw new ArgumentNullException("geometryFactory", "The geometry factory is null.");

            _geometryFactory = geometryFactory;
        }

        #endregion

        #region Factory methods for graphs

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <returns>An empty graph.</returns>
        public IGeometryGraph CreateGraph()
        {
            return new GeometryGraph(this, null);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty graph using the specified comparers.</returns>
        public IGeometryGraph CreateGraph(IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            return new GeometryGraph(this, null, vertexEqualityComparer, edgeEqualityComparer);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty graph with the specified metadata using the specified comparers.</returns>
        public IGeometryGraph CreateGraph(IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata)
        {
            return new GeometryGraph(this, metadata, vertexEqualityComparer, edgeEqualityComparer);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty graph with the specified metadata.</returns>
        public IGeometryGraph CreateGraph(IDictionary<String, Object> metadata)
        {
            return new GeometryGraph(this, metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <returns>A graph containing the specified coordinates as vertices.</returns>
        public IGeometryGraph CreateGraph(IEnumerable<Coordinate> coordinates)
        {
            return CreateGraph(coordinates, null, null, null);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>A graph containing the specified coordinates as vertices using the specified comparers.</returns>
        public IGeometryGraph CreateGraph(IEnumerable<Coordinate> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            return CreateGraph(coordinates, vertexEqualityComparer, edgeEqualityComparer, null);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and the metadata using the specified comparers.</returns>
        public IGeometryGraph CreateGraph(IEnumerable<Coordinate> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata)
        {
            IGeometryGraph graph = new GeometryGraph(this, metadata, vertexEqualityComparer, edgeEqualityComparer);

            if (coordinates != null)
            {
                foreach (Coordinate coordinate in coordinates)
                    graph.AddVertex(coordinate);
            }

            return graph;
        }


        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and the specified metadata.</returns>
        public IGeometryGraph CreateGraph(IEnumerable<Coordinate> coordinates, IDictionary<String, Object> metadata)
        {
            return CreateGraph(coordinates, null, null, metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges.</returns>
        public IGeometryGraph CreateGraph(IEnumerable<IEnumerable<Coordinate>> coordinates)
        {
            return CreateGraph(coordinates, null, null, null);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges using the specified comparers.</returns>
        public IGeometryGraph CreateGraph(IEnumerable<IEnumerable<Coordinate>> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            return CreateGraph(coordinates, vertexEqualityComparer, edgeEqualityComparer, null);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges and the metadata using the specified comparers.</returns>
        public IGeometryGraph CreateGraph(IEnumerable<IEnumerable<Coordinate>> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata)
        {
            IGeometryGraph graph = new GeometryGraph(this, metadata, vertexEqualityComparer, edgeEqualityComparer);
            if (coordinates != null)
            {
                foreach (IEnumerable<Coordinate> coordinateCollection in coordinates)
                {
                    if (coordinateCollection == null)
                        continue;

                    IGraphVertex first, second;

                    IEnumerator<Coordinate> enumerator = coordinateCollection.GetEnumerator();

                    if (enumerator.MoveNext())
                    {
                        first = graph.AddVertex(enumerator.Current);
                        while (enumerator.MoveNext())
                        {
                            second = graph.AddVertex(enumerator.Current);

                            graph.AddEdge(first, second);

                            first = second;
                        }
                    }
                }
            }

            return graph;
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges and the specified metadata.</returns>
        public IGeometryGraph CreateGraph(IEnumerable<IEnumerable<Coordinate>> coordinates, IDictionary<String, Object> metadata)
        {
            return CreateGraph(coordinates, null, null,  metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <returns>A graph that matches <param name="other" />.</returns>
        public IGeometryGraph CreateGraph(IGeometryGraph other)
        {
            IGeometryGraph graph = new GeometryGraph(this, other.Metadata, other.VertexComparer, other.EdgeComparer);

            other.ToGraph(graph); // using the extension method

            return graph;
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="other">The other geometry graph.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>A graph that matches <param name="other" /> using the specified comparers.</returns>
        public IGeometryGraph CreateGraph(IGeometryGraph other, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            IGeometryGraph graph = new GeometryGraph(this, other.Metadata, vertexEqualityComparer, edgeEqualityComparer);

            other.ToGraph(graph); // using the extension method

            return graph;
        }

        #endregion

        #region Factory methods for networks

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <returns>An empty network.</returns>
        public IGeometryGraph CreateNetwork()
        {
            return new GeometryNetwork(this, null);
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty network with the specified metadata.</returns>
        public IGeometryGraph CreateNetwork(IDictionary<String, Object> metadata)
        {
            return new GeometryNetwork(this, metadata);
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <returns>A network containing the specified coordinates as vertices.</returns>
        public IGeometryGraph CreateNetwork(IEnumerable<Coordinate> coordinates)
        {
            return CreateNetwork(coordinates, null);
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A network containing the specified coordinates as vertices and the specified metadata.</returns>
        public IGeometryGraph CreateNetwork(IEnumerable<Coordinate> coordinates, IDictionary<String, Object> metadata)
        {
            IGeometryGraph graph = new GeometryNetwork(this, metadata);

            if (coordinates != null)
            {
                foreach (Coordinate coordinate in coordinates)
                    graph.AddVertex(coordinate);
            }

            return graph;
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <returns>A network containing the specified coordinates as vertices and edges.</returns>
        public IGeometryGraph CreateNetwork(IEnumerable<IEnumerable<Coordinate>> coordinates)
        {
            return CreateNetwork(coordinates, null);
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A network containing the specified coordinates as vertices and edges and the specified metadata.</returns>
        public IGeometryGraph CreateNetwork(IEnumerable<IEnumerable<Coordinate>> coordinates, IDictionary<String, Object> metadata)
        {
            IGeometryGraph graph = new GeometryNetwork(this, metadata);
            if (coordinates != null)
            {
                foreach (IEnumerable<Coordinate> coordinateCollection in coordinates)
                {
                    if (coordinateCollection == null)
                        continue;

                    IGraphVertex first, second;

                    IEnumerator<Coordinate> enumerator = coordinateCollection.GetEnumerator();

                    if (enumerator.MoveNext())
                    {
                        first = graph.AddVertex(enumerator.Current);
                        while (enumerator.MoveNext())
                        {
                            second = graph.AddVertex(enumerator.Current);

                            graph.AddEdge(first, second);

                            first = second;
                        }
                    }
                }
            }

            return graph;
        }

        #endregion
    }
}
