/// <copyright file="GeometryGraphConversion.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Provides extensions to <see cref="IGeometry" /> for producing <see cref="IGeometryGraph" /> instances.
    /// </summary>
    public static class GeometryGraphExtensions
    {
        #region Conversion methods for geometries

        /// <summary>
        /// Converts the specified geometry to a graph.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The equivalent graph representation of the geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public static IGeometryGraph ToGraph(this IGeometry geometry)
        {
            return ToGraph(geometry, false, false);
        }

        /// <summary>
        /// Converts the specified geometry to a graph.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="isBidirectional">Indicates whether the graph should be bidirectional.</param>
        /// <param name="preserveMetadata">Indicates whether the metadata should be preserved.</param>
        /// <returns>The equivalent graph representation of the geometry.</returns>
        /// <exception cref="System.ArgumentNullException">he geometry is null.</exception>
        public static IGeometryGraph ToGraph(this IGeometry geometry, Boolean isBidirectional, Boolean preserveMetadata)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            IGeometryGraph graph = geometry.Factory.CreateGraph();
            ConvertToGraph(geometry, graph, isBidirectional, preserveMetadata);

            return graph;
        }

        /// <summary>
        /// Converts the specified geometry instance into an existing graph instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="graph">The graph.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The graph is null.
        /// </exception>
        public static void ToGraph(this IGeometry geometry, IGeometryGraph graph)
        {
            ToGraph(geometry, graph, false, false);
        }

        /// <summary>
        /// Converts the specified geometry instance into an existing graph instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="isBidirectional">Indicates whether the conversion should be performed bidirectionally.</param>
        /// <param name="preserveMetadata">Indicates whether the metadata should be preserved.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The graph is null.
        /// </exception>
        public static void ToGraph(this IGeometry geometry, IGeometryGraph graph, Boolean isBidirectional, Boolean preserveMetadata)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (graph == null)
                throw new ArgumentNullException("graph", "The graph is null.");

            ConvertToGraph(geometry, graph, isBidirectional, preserveMetadata);
        }

        #endregion

        #region Private static methods

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

            IGraphVertex source = graph.AddVertex(coordinates[0], preserveMetadata ? metadata : null);
            IGraphVertex first = source;
            IGraphVertex target;

            for (Int32 i = 1; i < coordinates.Count - 1; ++i)
            {
                target = graph.AddVertex(coordinates[i], preserveMetadata ? metadata : null);
                graph.AddEdge(source, target, preserveMetadata ? metadata : null);
                if (isBidirectional)
                {
                    graph.AddEdge(target, source, preserveMetadata ? metadata : null);
                }
                source = target;
            }

            if (isCircular)
            {
                graph.AddEdge(source, first, preserveMetadata ? metadata : null);
                if (isBidirectional)
                {
                    graph.AddEdge(first, source, preserveMetadata ? metadata : null);
                }
            }
            else
            {
                target = graph.AddVertex(coordinates[coordinates.Count - 1], preserveMetadata ? metadata : null);
                graph.AddEdge(source, target, preserveMetadata ? metadata : null);
                if (isBidirectional)
                {
                    graph.AddEdge(target, source, preserveMetadata ? metadata : null);
                }
            }
        }

        #endregion
    }
}
