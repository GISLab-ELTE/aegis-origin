/// <copyright file="AStarAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections;
using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spatial.ShortestPath
{
    /// <summary>
    /// Represents a shortest path transformation between two vertices using A* algorithm.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::212334", "A* algorithm")]
    public class AStarAlgorithm : SingleShortestPathAlgorithm
    {
        #region Private fields

        private readonly Func<Coordinate, Coordinate, Double> _heuristicMetric;
        private readonly Double _heuristicLimitMultiplier;
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
            if (parameters.ContainsKey(GraphOperationParameters.HeuristicMetric))
            {
                _heuristicMetric = parameters[GraphOperationParameters.HeuristicMetric] as Func<Coordinate, Coordinate, Double>;
            }
            else
            {
                _heuristicMetric = (u, v) => Coordinate.Distance(u, v);
            }

            _heuristicLimitMultiplier = parameters.ContainsKey(GraphOperationParameters.HeuristicMetric) ? Convert.ToDouble(parameters[GraphOperationParameters.HeuristicMetric]) : Convert.ToDouble(GraphOperationParameters.HeuristicLimitMultiplier.DefaultValue);

            _priorityQueue = null;
            _distance = null;
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

            Double limit = _distanceMetric(_sourceVertex.Coordinate, _targetVertex.Coordinate); // the direct distance used for search space limitation

            while (_priorityQueue.Count > 0 && !_isTargetFound)
            {
                IGraphVertex v = _priorityQueue.RemovePeek();

                if (_finished.Contains(v))
                    return;

                _finished.Add(v);

                if (_source.VertexComparer.Equals(v, _targetVertex)) // the target node is found
                {
                    _isTargetFound = true;
                    return;
                }

                foreach (IGraphEdge edge in _source.OutEdges(v))
                {
                    if (_finished.Contains(edge.Target))
                        continue;

                    if (!_distance.ContainsKey(edge.Target))
                    {
                        if (_distance[v] < limit * _heuristicLimitMultiplier)
                        {
                            _distance[edge.Target] = _distance[v] + _distanceMetric(edge.Source.Coordinate, edge.Target.Coordinate);
                            _parent[edge.Target] = v;
                            _priorityQueue.Insert(_distance[edge.Target] + _heuristicMetric(edge.Target.Coordinate, _targetVertex.Coordinate), edge.Target);
                        }
                    }
                    else if (_distance[edge.Target] > _distance[v] + _distanceMetric(edge.Source.Coordinate, edge.Target.Coordinate))
                    {
                        _distance[edge.Target] = _distance[v] + _distanceMetric(edge.Source.Coordinate, edge.Target.Coordinate);
                        _parent[edge.Target] = v;
                        _priorityQueue.Insert(_distance[edge.Target] + _heuristicMetric(edge.Target.Coordinate, _targetVertex.Coordinate), edge.Target);
                    }
                }
            }
        }

        #endregion
    }
}
