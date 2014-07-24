/// <copyright file="ShortestPathAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spatial.ShortestPath
{
    /// <summary>
    /// Represents an operation computing the shortest path between two vertices in a graph.
    /// </summary>
    public abstract class ShortestPathAlgorithm : Operation<IGeometryGraph, IGeometryGraph>
    {
        #region Protected fields

        /// <summary>
        /// The source vertex. This field is read-only.
        /// </summary>
        protected readonly IGraphVertex _sourceVertex;

        /// <summary>
        /// The target vertex. This field is read-only.
        /// </summary>
        protected readonly IGraphVertex _targetVertex;

        /// <summary>
        /// The metric used to compute distances between coordinates.
        /// </summary>
        protected readonly Func<IGraphEdge, Double> _distanceMetric;

        /// <summary>
        /// The dictionary of parent vertices.
        /// </summary>
        protected Dictionary<IGraphVertex, IGraphVertex> _parent;

        /// <summary>
        /// The dictionary of vertex distances.
        /// </summary>
        protected Dictionary<IGraphVertex, Double> _distance;

        /// <summary>
        /// The set of finished vertices.
        /// </summary>
        protected HashSet<IGraphVertex> _finished;

        /// <summary>
        /// A value indicating whethet the target vertex is reached.
        /// </summary>
        protected Boolean _isTargetReached;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortestPathAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method is null.
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
        protected ShortestPathAlgorithm(IGeometryGraph source, IGeometryGraph target, OperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _sourceVertex = ResolveParameter<IGraphVertex>(GraphOperationParameters.SourceVertex);
            _targetVertex = ResolveParameter<IGraphVertex>(GraphOperationParameters.TargetVertex);
            _distanceMetric = ResolveParameter<Func<IGraphEdge, Double>>(GraphOperationParameters.DistanceMetric);
        }

        #endregion 

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _parent = new Dictionary<IGraphVertex, IGraphVertex>();
            _distance = new Dictionary<IGraphVertex, Double>();
            _finished = new HashSet<IGraphVertex>();
            _isTargetReached = false;
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        protected override void FinalizeResult()
        {
            if (!_isTargetReached)
            {
                return;
            }

            // generate a network from the path
            if (_result == null)
                _result = _source.Factory.CreateNetwork();

            IGraphVertex current = _targetVertex;
            _result.AddVertex(current.Coordinate);

            while (_parent.ContainsKey(current) && !_source.VertexComparer.Equals(_sourceVertex, current))
            {
                IMetadataCollection metadata = _result.Factory.CreateMetadata();
                metadata["Distance"] = _distance[current];

                _result.AddVertex(_parent[current].Coordinate);
                _result.AddEdge(_parent[current].Coordinate, current.Coordinate, metadata);

                current = _parent[current];
            }
        }

        #endregion
    }
}
