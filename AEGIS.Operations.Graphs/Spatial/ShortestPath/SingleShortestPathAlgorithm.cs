/// <copyright file="SingleShortestPathAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
using System.Linq;

namespace ELTE.AEGIS.Operations.Spatial.ShortestPath
{
    /// <summary>
    /// Represents a shortest path transformation between two vertices.
    /// </summary>
    public abstract class SingleShortestPathAlgorithm : Operation<IGeometryGraph, IGeometryGraph>
    {
        #region Protected fields

        protected readonly IGraphVertex _sourceVertex; // operation parameters
        protected readonly IGraphVertex _targetVertex;
        protected readonly Func<Coordinate, Coordinate, Double> _distanceMetric;
        protected Dictionary<IGraphVertex, IGraphVertex> _parent; // utility fields
        protected Dictionary<IGraphVertex, Double> _distance;
        protected HashSet<IGraphVertex> _finished;
        protected Boolean _isTargetFound;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleShortestPathAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// method;The method is null.
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
        protected SingleShortestPathAlgorithm(IGeometryGraph source, IGeometryGraph target, OperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _sourceVertex = parameters[GraphOperationParameters.SourceVertex] as IGraphVertex;
            _targetVertex = parameters[GraphOperationParameters.TargetVertex] as IGraphVertex;

            if (parameters.ContainsKey(GraphOperationParameters.DistanceMetric))
            {
                _distanceMetric = parameters[GraphOperationParameters.DistanceMetric] as Func<Coordinate, Coordinate, Double>;
            }
            else
            {
                _distanceMetric = (u, v) => Coordinate.Distance(u, v);
            }

            _parent = null;
            _distance = null;
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
            _isTargetFound = false;
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        protected override void FinalizeResult()
        {
            if (!_isTargetFound)
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
