/// <copyright file="PrimsAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dávid Kis</author>

using ELTE.AEGIS.Collections;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spatial.SpanningTree
{
    /// <summary>
    /// Represents an operation performing Prim's algorithm
    /// </summary>
    /// <remarks>
    /// Prim's algorithm is a greedy algorithm that finds a minimum spanning tree for a weighted undirected graph.
    /// By running Prim's algorithm for each connected component of the graph, it can also be used to find the minimum spanning forest.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::225501", "Prim's algorithm")]
    public class PrimsAlgorithm : MinimumSpanningTreeAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The source vertex. This field is read-only.
        /// </summary>
        protected readonly IGraphVertex _sourceVertex;

        /// <summary>
        /// The min heap priority queue.
        /// </summary>
        private Heap<Double, IGraphVertex> _minQ;

        /// <summary>
        /// The dictionary of vertices distance of parents.
        /// </summary>
        private Dictionary<IGraphVertex, Double> _distance;

        /// <summary>
        /// The dictionary of parents of vertices.
        /// </summary>
        private Dictionary<IGraphVertex, IGraphVertex> _parent;

        /// <summary>
        /// The set containing the visited vertices.
        /// </summary>
        private HashSet<IGraphVertex> _visitedVertices;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimsAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public PrimsAlgorithm(IGeometryGraph source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimsAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public PrimsAlgorithm(IGeometryGraph source, IGeometryGraph target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, GraphOperationMethods.PrimsAlgorithm, parameters)
        {
            _sourceVertex = ResolveParameter<IGraphVertex>(GraphOperationParameters.SourceVertex);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            _minQ = new Heap<double, IGraphVertex>();
            _distance = new Dictionary<IGraphVertex, double>();
            _parent = new Dictionary<IGraphVertex, IGraphVertex>();
            _visitedVertices = new HashSet<IGraphVertex>();
            foreach (IGraphVertex v in Source.Vertices.Except(new List<IGraphVertex> {_sourceVertex} ))
            {
                _minQ.Insert(Double.MaxValue, v);
                _distance.Add(v, Double.MaxValue);
            }
            _minQ.Insert(0, _sourceVertex);
            _distance.Add(_sourceVertex, 0);

            while (_minQ.Count > 0)
            {
                IGraphVertex u = _minQ.RemovePeek();
                _visitedVertices.Add(u);
                if (_parent.ContainsKey(u))
                {
                    _spanningEdges.Add(Source.GetEdge(_parent[u], u));
                }
                foreach (IGraphVertex v in GetAdjacentVertices(u))
                {
                    if (!_visitedVertices.Contains(v) && _weightMetric(Source.GetEdge(u,v)) < _distance[v])
                    {
                        _distance[v] = _weightMetric(Source.GetEdge(u, v));
                        _minQ.Insert(_distance[v], v);
                        _parent[v] = u;
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the adjacent vertices of the given vertex
        /// </summary>
        /// <param name="vertex">The vertex</param>
        /// <returns>The adjacent vertices</returns>
        List<IGraphVertex> GetAdjacentVertices(IGraphVertex vertex)
        {
            List<IGraphVertex> adjacentVertices = new List<IGraphVertex>();
            foreach (IGraphEdge edge in Source.OutEdges(vertex))
            {
                if (edge.Source == vertex)
                {
                    adjacentVertices.Add(edge.Target);
                }
                else
                {
                    adjacentVertices.Add(edge.Source);
                }
            }
            return adjacentVertices;
        }

        #endregion
    }
}
