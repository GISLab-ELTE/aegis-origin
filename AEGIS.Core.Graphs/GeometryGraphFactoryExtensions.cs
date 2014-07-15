/// <copyright file="GeometryGraphFactory.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Provides extensions to <see cref="IGeometryFactory" /> for producing <see cref="IGeometryGraph" /> instances.
    /// </summary>
    public static class GeometryGraphFactoryExtensions
    {
        #region Factory methods for graphs

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>An empty graph.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph();
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>An empty graph using the specified comparers.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(vertexEqualityComparer, edgeEqualityComparer);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty graph with the specified metadata using the specified comparers.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(vertexEqualityComparer, edgeEqualityComparer, metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty graph with the specified metadata.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <returns>A graph containing the specified coordinates as vertices.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEnumerable<Coordinate> coordinates)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(coordinates);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <returns>A graph containing the specified coordinates as vertices using the specified comparers.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEnumerable<Coordinate> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(coordinates, vertexEqualityComparer, edgeEqualityComparer);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and the metadata using the specified comparers.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEnumerable<Coordinate> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(coordinates, vertexEqualityComparer, edgeEqualityComparer, metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and the metadata.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEnumerable<Coordinate> coordinates, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(coordinates, metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEnumerable<IEnumerable<Coordinate>> coordinates)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(coordinates);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges using the specified comparers.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEnumerable<IEnumerable<Coordinate>> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(coordinates, vertexEqualityComparer, edgeEqualityComparer);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges using the specified comparers.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEnumerable<IEnumerable<Coordinate>> coordinates, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(coordinates, vertexEqualityComparer, edgeEqualityComparer, metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A graph containing the specified coordinates as vertices and edges.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IEnumerable<IEnumerable<Coordinate>> coordinates, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(coordinates, metadata);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>A graph that matches <param name="other" />.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IGeometryGraph other)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(other);
        }

        /// <summary>
        /// Creates a graph.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="vertexEqualityComparer">The vertex comparer.</param>
        /// <param name="edgeEqualityComparer">The edge comparer.</param>
        /// <returns>A graph that matches <param name="other" /> using the specified comparers.</returns>
        public static IGeometryGraph CreateGraph(this IGeometryFactory factory, IGeometryGraph other, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateGraph(other, vertexEqualityComparer, edgeEqualityComparer);
        }

        #endregion

        #region Factory methods for networks

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>An empty network.</returns>
        public static IGeometryGraph CreateNetwork(this IGeometryFactory factory)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateNetwork();
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty network with the specified metadata.</returns>
        public static IGeometryGraph CreateNetwork(this IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateNetwork(metadata);
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <returns>A network containing the specified coordinates as vertices.</returns>
        public static IGeometryGraph CreateNetwork(this IGeometryFactory factory, IEnumerable<Coordinate> coordinates)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateNetwork(coordinates);
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of coodinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A network containing the specified coordinates as vertices and the specified metadata.</returns>
        public static IGeometryGraph CreateNetwork(this IGeometryFactory factory, IEnumerable<Coordinate> coordinates, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateNetwork(coordinates, metadata);
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <returns>A network containing the specified coordinates as vertices and edges.</returns>
        public static IGeometryGraph CreateNetwork(this IGeometryFactory factory, IEnumerable<IEnumerable<Coordinate>> coordinates)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateNetwork(coordinates);
        }

        /// <summary>
        /// Creates a network.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="coordinates">The sequence of edge sequences.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A network containing the specified coordinates as vertices and edges.</returns>
        public static IGeometryGraph CreateNetwork(this IGeometryFactory factory, IEnumerable<IEnumerable<Coordinate>> coordinates, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<IGeometryGraphFactory>().CreateNetwork(coordinates, metadata);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Ensures an internal geometry graph factory the specified geometry factory.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        private static void EnsureFactory(IGeometryFactory factory)
        {
            // query whether the geometry graph factory is registerd for the geometry factory
            if (!factory.ContainsFactoryFor<IGeometryGraph>() || !(factory.GetFactoryFor<IGeometryGraph>() is IGeometryGraphFactory))
            {
                if (Factory.HasDefaultInstance<IGeometryGraphFactory>()) // if it has been registered previously
                {
                    factory.SetFactoryFor<IGeometryGraph>(Factory.GetInstance<IGeometryGraphFactory>(factory));
                }
                else // if not the default implemenentation is registered
                {
                    factory.SetFactoryFor<IGeometryGraph>(Factory.GetInstance<Geometry.GeometryGraphFactory>(factory));
                }
            }
        }

        /// <summary>
        /// Converts the specified geometry to an existing graph instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="isBidirectional">Indicates whether the conversion should be performed bidirectionally.</param>
        /// <param name="preserveMetadata">Indicates whether the metadata should be preserved.</param>
        private static void ConvertToGraph(IGeometry geometry, IGeometryGraph graph, Boolean isBidirectional, Boolean preserveMetadata)
        {
            if (geometry is IPoint)
                ConvertToGraph(geometry as IPoint, graph, isBidirectional, preserveMetadata);
            if (geometry is ILineString)
                ConvertToGraph(geometry as ILineString, graph, isBidirectional, preserveMetadata);
            if (geometry is IPolygon)
                ConvertToGraph(geometry as IPolygon, graph, isBidirectional, preserveMetadata);
            if (geometry is IEnumerable<IGeometry>)
                ConvertToGraph(geometry as IEnumerable<IGeometry>, graph, isBidirectional, preserveMetadata);
            if (geometry is IGeometryGraph)
                ConvertToGraph(geometry as IGeometryGraph, graph, isBidirectional, preserveMetadata);

            throw new ArgumentException("geometry", "Conversion of the specified geometry is not supported.");
        }

        /// <summary>
        /// Converts the specified point to an existing graph instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="isBidirectional">Indicates whether the conversion should be performed bidirectionally.</param>
        /// <param name="preserveMetadata">Indicates whether the metadata should be preserved.</param>
        private static void ConvertToGraph(IPoint geometry, IGeometryGraph graph, Boolean isBidirectional, Boolean preserveMetadata)
        {
            graph.AddVertex(geometry.Coordinate, preserveMetadata ? geometry.Metadata : null);
        }

        /// <summary>
        /// Converts the specified line string to an existing graph instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="isBidirectional">Indicates whether the conversion should be performed bidirectionally.</param>
        /// <param name="preserveMetadata">Indicates whether the metadata should be preserved.</param>
        private static void ConvertToGraph(ILineString geometry, IGeometryGraph graph, Boolean isBidirectional, Boolean preserveMetadata)
        {
            ConvertToGraph(geometry.Coordinates, geometry.Metadata, graph, geometry.IsRing, isBidirectional, preserveMetadata);
        }

        /// <summary>
        /// Converts the specified polygon to an existing graph instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="isBidirectional">Indicates whether the conversion should be performed bidirectionally.</param>
        /// <param name="preserveMetadata">Indicates whether the metadata should be preserved.</param>
        private static void ConvertToGraph(IPolygon geometry, IGeometryGraph graph, Boolean isBidirectional, Boolean preserveMetadata)
        {
            ConvertToGraph(geometry.Shell.Coordinates, geometry.Shell.Metadata, graph, geometry.Shell.IsRing, isBidirectional, preserveMetadata);
            if (geometry.HoleCount > 0)
            {
                foreach (ILinearRing linearRing in geometry.Holes)
                {
                    ConvertToGraph(linearRing.Coordinates, linearRing.Metadata, graph, linearRing.IsRing, isBidirectional, preserveMetadata);
                }
            }
        }

        /// <summary>
        /// Converts the specified geometry collection to an existing graph instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="isBidirectional">Indicates whether the conversion should be performed bidirectionally.</param>
        /// <param name="preserveMetadata">Indicates whether the metadata should be preserved.</param>
        private static void ConvertToGraph(IEnumerable<IGeometry> geometry, IGeometryGraph graph, Boolean isBidirectional, Boolean preserveMetadata)
        {
            foreach (IGeometry item in geometry)
            {
                ConvertToGraph(item, graph, isBidirectional, preserveMetadata);
            }
        }

        /// <summary>
        /// Converts the specified coordinate list to an existing graph instance.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="isCircular">Indicates whether the source is circular.</param>
        /// <param name="isBidirectional">Indicates whether the conversion should be performed bidirectionally.</param>
        /// <param name="preserveMetadata">Indicates whether the metadata should be preserved.</param>
        private static void ConvertToGraph(IList<Coordinate> coordinates, IMetadataCollection metadata, IGeometryGraph graph, Boolean isCircular, Boolean isBidirectional, Boolean preserveMetadata)
        {
            if (graph == null || coordinates == null || coordinates.Count <= 0) return;

            IGraphVertex source = graph.AddVertex(coordinates[0], preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
            IGraphVertex first = source;
            IGraphVertex target;

            for (Int32 i = 1; i < coordinates.Count - 1; ++i)
            {
                target = graph.AddVertex(coordinates[i], preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
                graph.AddEdge(source, target, preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
                if (isBidirectional)
                {
                    graph.AddEdge(target, source, preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
                }
                source = target;
            }

            if (isCircular)
            {
                graph.AddEdge(source, first, preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
                if (isBidirectional)
                {
                    graph.AddEdge(first, source, preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
                }
            }
            else
            {
                target = graph.AddVertex(coordinates[coordinates.Count - 1], preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
                graph.AddEdge(source, target, preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
                if (isBidirectional)
                {
                    graph.AddEdge(target, source, preserveMetadata ? graph.Factory.CreateMetadata(metadata) : null);
                }
            }
        }

        #endregion
    }
}
