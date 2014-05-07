﻿/// <copyright file="DijkstraSinglePathAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a shortest path transformation between two vertices using Dijkstra's algorithm.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::212331", "Dijkstra's algorithm (to target)")]
    public class DijkstrasSingleShortestPathAlgorithm : SingleShortestPathAlgorithm
    {
        #region Private fileds

        private Heap<Double, IGraphVertex> _priorityQueue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DijkstrasSingleShortestPathAlgorithm" /> class.
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
        public DijkstrasSingleShortestPathAlgorithm(IGeometryGraph source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DijkstrasSingleShortestPathAlgorithm" /> class.
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
        public DijkstrasSingleShortestPathAlgorithm(IGeometryGraph source, IGeometryGraph target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, GraphOperationMethods.DijkstrasSinlgeShortestPathAlgorithm, parameters)
        {
            _priorityQueue = null;
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            _priorityQueue = new Heap<Double, IGraphVertex>();

            _priorityQueue.Insert(0, _sourceVertex);
            _distance.Add(_sourceVertex, 0);
            _parent.Add(_sourceVertex, null);

            while (_priorityQueue.Count > 0 && !_isTargetFound)
            {
                IGraphVertex v = _priorityQueue.RemovePeek();

                if (_finished.Contains(v))
                    return;

                _finished.Add(v);

                if (_source.VertexComparer.Equals(v, _targetVertex))
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
                        _distance[edge.Target] = _distance[v] + _distanceMetric(edge.Source.Coordinate, edge.Target.Coordinate);
                        _parent[edge.Target] = v;
                        _priorityQueue.Insert(_distance[edge.Target], edge.Target);
                    }
                    else if (_distance[edge.Target] > _distance[v] + _distanceMetric(edge.Source.Coordinate, edge.Target.Coordinate))
                    {
                        _distance[edge.Target] = _distance[v] + _distanceMetric(edge.Source.Coordinate, edge.Target.Coordinate);
                        _parent[edge.Target] = v;
                        _priorityQueue.Insert(_distance[edge.Target], edge.Target);
                    }
                }
            }
        }

        #endregion
    }
}
