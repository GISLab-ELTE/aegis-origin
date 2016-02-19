/// <copyright file="MinimumSpanningTreeAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spatial.SpanningTree
{
    /// <summary>
    /// Represents an operation computing the minimum spanning tree of the graph
    /// </summary>
    public abstract class MinimumSpanningTreeAlgorithm : Operation<IGeometryGraph, IGeometryGraph>
    {
        #region Protected fields

        /// <summary>
        /// The metric used to compute the weight of edges.
        /// </summary>
        protected readonly Func<IGraphEdge, Double> _weightMetric;

        /// <summary>
        /// The list of edges of the spanning tree.
        /// </summary>
        protected List<IGraphEdge> _spanningEdges;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumSpanningTreeAlgorithm" /> class.
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
        protected MinimumSpanningTreeAlgorithm(IGeometryGraph source, IGeometryGraph target, OperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _weightMetric = ResolveParameter<Func<IGraphEdge, Double>>(GraphOperationParameters.WeightMetric);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _spanningEdges = new List<IGraphEdge>();
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        protected override void FinalizeResult()
        {

            // generate a graph from the spanning tree if target is null
            if (_result == null)
                _result = _source.Factory.CreateGraph(_source.VertexComparer, _source.EdgeComparer);

            foreach (IGraphEdge spanningEdge in _spanningEdges)
            {
                _result.AddEdge(spanningEdge.Source.Coordinate, spanningEdge.Target.Coordinate, spanningEdge.Metadata);
                _result.AddEdge(spanningEdge.Target.Coordinate, spanningEdge.Source.Coordinate, spanningEdge.Metadata);
            }
        }

        #endregion
    }
}

