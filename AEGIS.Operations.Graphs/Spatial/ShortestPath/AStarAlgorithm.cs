/// <copyright file="AStarAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections;
using ELTE.AEGIS.Management;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spatial.ShortestPath
{
    /// <summary>
    /// Represents an operation performing A* algorithm on a graph.
    /// </summary>
    [OperationMethodImplementation("AEGIS::221910", "A* algorithm")]
    public class AStarAlgorithm : ShortestPathAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The heuristic metric.
        /// </summary>
        private readonly Func<IGraphVertex, IGraphVertex, Double> _heuristicMetric;

        /// <summary>
        /// The heuristic limit multiplier.
        /// </summary>
        private readonly Double _heuristicLimitMultiplier;

        /// <summary>
        /// The priority queue used for ordering vertices.
        /// </summary>
        private Heap<Double, IGraphVertex> _priorityQueue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AStarAlgorithm" /> class.
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
        public AStarAlgorithm(IGeometryGraph source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AStarAlgorithm" /> class.
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
        public AStarAlgorithm(IGeometryGraph source, IGeometryGraph target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, GraphOperationMethods.AStarAlgorithm, parameters)
        {
            _heuristicMetric = ResolveParameter<Func<IGraphVertex, IGraphVertex, Double>>(GraphOperationParameters.HeuristicMetric);
            _heuristicLimitMultiplier = Convert.ToDouble(ResolveParameter(GraphOperationParameters.HeuristicLimitMultiplier));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            _priorityQueue = new Heap<Double, IGraphVertex>();
            _distance = new Dictionary<IGraphVertex, Double>();
            _priorityQueue.Insert(0, _sourceVertex);
            _distance.Add(_sourceVertex, 0);
            _parent.Add(_sourceVertex, null);

            Double limit = _heuristicMetric(_sourceVertex, _targetVertex) * _heuristicLimitMultiplier; // the direct distance used for search space limitation

            while (_priorityQueue.Count > 0 && !_isTargetReached)
            {
                IGraphVertex currentVertex = _priorityQueue.RemovePeek();

                _finished.Add(currentVertex);

                if (Source.VertexComparer.Equals(currentVertex, _targetVertex)) // the target vertex is found
                {
                    _isTargetReached = true;
                    return;
                }

                foreach (IGraphEdge edge in Source.OutEdges(currentVertex))
                {
                    if (_finished.Contains(edge.Target))
                        continue;

                    if (!_distance.ContainsKey(edge.Target))
                    {
                        if (_distance[currentVertex] < limit * _heuristicLimitMultiplier)
                        {
                            _distance[edge.Target] = _distance[currentVertex] + _weightMetric(edge);
                            _parent[edge.Target] = currentVertex;
                            _priorityQueue.Insert(_distance[edge.Target] + _heuristicMetric(edge.Target, _targetVertex), edge.Target);
                        }
                    }
                    else if (_distance[edge.Target] > _distance[currentVertex] + _weightMetric(edge))
                    {
                        _distance[edge.Target] = _distance[currentVertex] + _weightMetric(edge);
                        _parent[edge.Target] = currentVertex;
                        _priorityQueue.Insert(_distance[edge.Target] + _heuristicMetric(edge.Target, _targetVertex), edge.Target);
                    }
                }
            }
        }

        #endregion
    }
}
