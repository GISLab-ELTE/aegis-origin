/// <copyright file="MaximumFlowComputation.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations.Spatial.MaximumFlow
{
    /// <summary>
    /// Represents an operation for computing the maximum flow between two vertices in a graph.
    /// </summary>
    public abstract class MaximumFlowComputation : Operation<IGeometryGraph, IGeometryGraph>
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
        /// The metric used to compute the capacity of edges.
        /// </summary>
        protected readonly Func<IGraphEdge, Int32> _capacityMetric;

        /// <summary>
        /// The dictionary containing the used capacity fot he edges.
        /// </summary>
        protected Dictionary<IGraphEdge, Int32> _usedCapacity;

        /// <summary>
        /// The value of the maximum flow.
        /// </summary>
        protected Int32 _maximumFlow;

        /// <summary>
        /// A value indicating whethet the target vertex is reached.
        /// </summary>
        protected Boolean _isTargetReached;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumFlowComputation"/> class.
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
        /// The parameter value does not satisfy the conditions of the parameter.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        protected MaximumFlowComputation(IGeometryGraph source, IGeometryGraph target, OperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _sourceVertex = ResolveParameter<IGraphVertex>(GraphOperationParameters.SourceVertex);
            _targetVertex = ResolveParameter<IGraphVertex>(GraphOperationParameters.TargetVertex);
            _capacityMetric = ResolveParameter<Func<IGraphEdge, Int32>>(GraphOperationParameters.CapacityMetric);
        }

        #endregion 

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _usedCapacity = new Dictionary<IGraphEdge, Int32>();
            _maximumFlow = 0;
            _isTargetReached = false;
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        protected override void FinalizeResult()
        {
            if (!_isTargetReached)
                return;

            if (_result == null)
                _result = _source.Factory.CreateGraph(_source.VertexComparer, _source.EdgeComparer);

            _result["MaximumFlow"] = _maximumFlow;

            foreach (IGraphEdge edge in _usedCapacity.Keys)
            {
                Dictionary<String, Object> metadata = new Dictionary<String, Object>();
                metadata["ResidualCapacity"] = _capacityMetric(edge) - _usedCapacity[edge];

                _result.AddEdge(edge.Source.Coordinate, edge.Target.Coordinate, metadata);
            }
        }

        #endregion
    }
}
