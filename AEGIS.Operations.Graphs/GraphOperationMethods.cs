/// <copyright file="GraphOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationMethod" /> instances for graph operations.
    /// </summary>
    [OperationMethodCollection]
    public static class GraphOperationMethods
    {
        #region Query fields

        private static OperationMethod[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OperationMethod" /> instances in the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OperationMethod" /> instances in the collection.</value>
        public static IList<OperationMethod> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(GraphOperationMethods).GetProperties().
                                                         Where(property => property.Name != "All").
                                                         Select(property => property.GetValue(null, null) as OperationMethod).
                                                         ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="OperationMethod" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="OperationMethod" /> instances that match the specified identifier.</returns>
        public static IList<OperationMethod> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList();
        }

        /// <summary>
        /// Returns all <see cref="OperationMethod" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="OperationMethod" /> instances that match the specified name.</returns>
        public static IList<OperationMethod> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList();
        }

        #endregion

        #region Private static fields

        private static OperationMethod _aStarAlgorithm;
        private static OperationMethod _bellmannFordAlgorithm;
        private static OperationMethod _boruvkasAlgorithm;
        private static OperationMethod _breathFirstSearch;
        private static OperationMethod _depthFirstSearch;
        private static OperationMethod _dijkstrasAlgorithm;
        private static OperationMethod _dijkstrasSinglePathAlgorithm;
        private static OperationMethod _edmondsKarpAlgorithm;
        private static OperationMethod _floydWarshallAlgorithmMinimalPath;
        private static OperationMethod _floydWarshallAlgorithmTransitiveClosure;
        private static OperationMethod _geometryPolygonization;
        private static OperationMethod _geometryToGraphConversion;
        private static OperationMethod _geometryToNetworkConversion;
        private static OperationMethod _graphToGeometryConversion;
        private static OperationMethod _kruskalsAlgorithm;
        private static OperationMethod _primsAlgorithm;
        private static OperationMethod _reverseDeleteAlgorithm;

        #endregion

        #region Public static properties

        /// <summary>
        /// A* star algorithm.
        /// </summary>
        public static OperationMethod AStarAlgorithm
        {
            get
            {
                return _aStarAlgorithm ?? (_aStarAlgorithm =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::221910", "A* star algorithm",
                        "A* is a computer algorithm that is widely used in pathfinding and graph traversal, the process of plotting an efficiently traversable path between vertices.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,                          
                        GraphOperationParameters.SourceVertex, 
                        GraphOperationParameters.TargetVertex,
                        GraphOperationParameters.WeightMetric,
                        GraphOperationParameters.HeuristicMetric,                                        
                        GraphOperationParameters.HeuristicLimitMultiplier
                    ));
            }
        }

        /// <summary>
        /// Bellmann-Ford algorithm.
        /// </summary>
        public static OperationMethod BellmannFordAlgorithm
        {
            get
            {
                return _bellmannFordAlgorithm ?? (_bellmannFordAlgorithm =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::221614", "Bellmann-Ford algorithm",
                        "Computes shortest paths from a single source vertex to all of the other vertices in a weighted directed graph.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.SourceVertex,
                        GraphOperationParameters.WeightMetric
                    ));
            }
        }

        /// <summary>
        /// Borůvka's algorithm.
        /// </summary>
        public static OperationMethod BoruvkasAgorithm
        {
            get
            {
                return _boruvkasAlgorithm ?? (_boruvkasAlgorithm =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::225508", "Borůvka's algorithm.",
                        "Algorithm for finding a minimum spanning tree in a graph for which all edge weights are distinct.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.WeightMetric
                    ));
            }
        }

        /// <summary>
        /// Breath-first search.
        /// </summary>
        public static OperationMethod BreathFirstSearch
        {
            get
            {
                return _breathFirstSearch ?? (_breathFirstSearch =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::221301", "Breath-first search",
                        "Traverses the graph by beginning at the root vertex and exploring all the neighboring vertices. Then for each of those nearest vertices, it explores their unexplored neighbor vertices, and so on.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.SourceVertex
                    ));
            }
        }

        /// <summary>
        /// Depth-first search.
        /// </summary>
        public static OperationMethod DepthFirstSearch
        {
            get
            {
                return _depthFirstSearch ?? (_depthFirstSearch =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::221302", "Depth-first search",
                        "Traverses the graph by exploring as far as possible along each branch before backtracking.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.SourceVertex
                    ));
            }
        }

        /// <summary>
        /// Dijkstra's algorithm.
        /// </summary>
        public static OperationMethod DijkstrasAlgorithm
        {
            get
            {
                return _dijkstrasAlgorithm ?? (_dijkstrasAlgorithm =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::221610", "Dijkstra's algorithm",
                        "Dijkstra's algorithm is a graph search algorithm that solves the single-source shortest path problem for a graph to non-negative edge path costs, producing a shortest path tree.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.SourceVertex,
                        GraphOperationParameters.WeightMetric
                    ));
            }
        }

        /// <summary>
        /// Dijkstra's algorithm (single path).
        /// </summary>
        public static OperationMethod DijkstrasSinglePathAlgorithm
        {
            get
            {
                return _dijkstrasSinglePathAlgorithm ?? (_dijkstrasSinglePathAlgorithm =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::221611", "Dijkstra's algorithm (single path)",
                        "Dijkstra's algorithm is a graph search algorithm that solves the single-source shortest path problem for a graph to non-negative edge path costs, producing a shortest path tree. This version of the algorithm computes the shortest single path between two vertices.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.SourceVertex,
                        GraphOperationParameters.TargetVertex,
                        GraphOperationParameters.WeightMetric
                    ));
            }
        }

        /// <summary>
        /// Edmonds-Karp algorithm.
        /// </summary>
        public static OperationMethod EdmondsKarpAlgorithm
        {
            get
            {
                return _edmondsKarpAlgorithm ?? (_edmondsKarpAlgorithm =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::225710", "Edmonds-Karp algorithm",
                        "The Edmonds–Karp algorithm is an implementation of the Ford–Fulkerson method for computing the maximum flow in a graph.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.SourceVertex,
                        GraphOperationParameters.TargetVertex,
                        GraphOperationParameters.CapacityMetric
                    ));
            }
        }

        /// <summary>
        /// Floyd–Warshall algorithm (for minimal path).
        /// </summary>
        public static OperationMethod FloydWarshallAlgorithmMinimalPath
        {
            get
            {
                return _floydWarshallAlgorithmMinimalPath ?? (_floydWarshallAlgorithmMinimalPath =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::221617", "Floyd–Warshall algorithm (for minimal path)",
                        "Floyd–Warshall algorithm is a graph analysis algorithm for finding shortest paths in a weighted graph to positive or negative edge weights (but to no negative cycles).",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.WeightMetric
                    ));
            }
        }
        
        /// <summary>
        /// Floyd–Warshall algorithm (for transitive closure).
        /// </summary>
        public static OperationMethod FloydWarshallAlgorithmTransitiveClosure
        {
            get
            {
                return _floydWarshallAlgorithmTransitiveClosure ?? (_floydWarshallAlgorithmTransitiveClosure =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::223617", "Floyd–Warshall algorithm (for transitive closure)",
                        "Floyd–Warshall algorithm is a graph analysis algorithm for computing the transitive closure of a weighted graph to positive or negative edge weights (but to no negative cycles).",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace
                    ));
            }
        }

        /// <summary>
        /// Geometry polygonization.
        /// </summary>
        public static OperationMethod GeometryPolygonization
        {
            get
            {
                return _geometryPolygonization ?? (_geometryPolygonization =
                    OperationMethod.CreateMethod<IGeometry, IGeometryGraph>(
                        "AEGIS::220205", "Geometry polygonization",
                        "Converts geometries of any kind to polygonal representation.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        CommonOperationParameters.GeometryFactory,
                        CommonOperationParameters.MetadataPreservation
                    ));
            }
        }

        /// <summary>
        /// Geometry to graph conversion.
        /// </summary>
        public static OperationMethod GeometryToGraphConversion
        {
            get
            {
                return _geometryToGraphConversion ?? (_geometryToGraphConversion =
                    OperationMethod.CreateMethod<IGeometry, IGeometryGraph>(
                        "AEGIS::220100", "Geometry to graph conversion",
                        "Converts geometries to a single graph representation. The resulting graph can match the original directions of the geometry lines, os can be bidirectional.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.BidirectionalConversion,
                        CommonOperationParameters.MetadataPreservation
                    ));
            }
        }

        /// <summary>
        /// Geometry to network conversion.
        /// </summary>
        public static OperationMethod GeometryToNetworkConversion
        {
            get
            {
                return _geometryToNetworkConversion ?? (_geometryToNetworkConversion =
                     OperationMethod.CreateMethod<IGeometry, IGeometryGraph>(
                        "AEGIS::220101", "Geometry to network conversion",
                        "Converts geometries to a single network representation. The resulting network can match the original directions of the geometry lines, os can be bidirectional.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.BidirectionalConversion,
                        CommonOperationParameters.MetadataPreservation
                    ));
            }
        }

        /// <summary>
        /// Graph to geometry conversion.
        /// </summary>
        public static OperationMethod GraphToGeometryConversion
        {
            get
            {
                return _graphToGeometryConversion ?? (_graphToGeometryConversion =
                     OperationMethod.CreateMethod<IGeometryGraph, IGeometry>(
                        "AEGIS::220110", "Graph to geometry conversion",
                        "Converts a graph to geometry representation. The dimension of the extracted geometries can be limited to points (0), curves (1) and surfaces (2). The factory of the resulting geometries can also be specified.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.GeometryDimension,
                        CommonOperationParameters.GeometryFactory,
                        CommonOperationParameters.MetadataPreservation
                    ));
            }
        }


        /// <summary>
        /// Kruskal's algorithm.
        /// </summary>
        public static OperationMethod KruskalsAlgorithm
        {
            get
            {
                return _kruskalsAlgorithm ?? (_kruskalsAlgorithm =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::225502", "Kruskal's algorithm",
                        "A greedy algorithm that finds a minimum spanning tree or forest for a weighted undirected graph.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.WeightMetric
                    ));
            }
        }


        /// <summary>
        /// Prim's algorithm.
        /// </summary>
        public static OperationMethod PrimsAlgorithm
        {
            get
            {
                return _primsAlgorithm ?? (_primsAlgorithm =
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::225501", "Prim's algorithm",
                        "Prim's algorithm is a greedy algorithm that finds a minimum spanning tree for a connected weighted undirected graph.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.SourceVertex,
                        GraphOperationParameters.WeightMetric
                    ));
            }
        }

        /// <summary>
        /// Reverse-delete algorithm.
        /// </summary>
        public static OperationMethod ReverseDeleteAlgorithm
        {
            get
            {
                return _reverseDeleteAlgorithm ?? (_reverseDeleteAlgorithm = 
                    OperationMethod.CreateMethod<IGeometryGraph, IGeometryGraph>(
                        "AEGIS::225510", "Reverse-delete algorithm",
                        "Reverse-delete is a greedy algorithm that finds a minimum spanning tree or forest for a weighted undirected graph.",
                        false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.OutPlace,
                        GraphOperationParameters.SourceVertex,
                        GraphOperationParameters.WeightMetric
                    ));
            }
        }

        #endregion
    }
}
