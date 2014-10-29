/// <copyright file="BellmanFordAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Krisztián Fodor</author>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spatial.ShortestPath
{
    /// <summary>
    /// Represents an operation performing the Bellmann-Ford algorithm on graphs.
    /// </summary>
    [OperationMethodImplementation("AEGIS::212314", "Bellman-Ford algorithm")]
    public class BellmanFordAlgorithm : ShortestPathTreeAlgorithm
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BellmanFordAlgorithm"/> class.
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
        public BellmanFordAlgorithm(IGeometryGraph source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, GraphOperationMethods.BellmannFordAlgorithm, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BellmanFordAlgorithm"/> class.
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
        public BellmanFordAlgorithm(IGeometryGraph source, IGeometryGraph target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, GraphOperationMethods.BellmannFordAlgorithm, parameters)
        {
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            _distance.Add(_sourceVertex, 0);
            _parent.Add(_sourceVertex, null);

            Boolean hasChanged = false;

            // relax edges
            for (int i = 0; i < _source.VertexCount; i++)
            {
                hasChanged = false;

                foreach (IGraphEdge edge in _source.Edges)
                {
                    if (!_distance.ContainsKey(edge.Target))
                    {
                        if (!_distance.ContainsKey(edge.Source))
                        {
                            continue;
                        }

                        _distance[edge.Target] = _distance[edge.Source] + _weightMetric(edge);
                        _parent[edge.Target] = edge.Source;
                        hasChanged = true;
                    }
                    else if (_distance.ContainsKey(edge.Source) && _distance[edge.Target] > _distance[edge.Source] + _weightMetric(edge))
                    {
                        _distance[edge.Target] = _distance[edge.Source] + _weightMetric(edge);
                        _parent[edge.Target] = edge.Source;
                        hasChanged = true;
                    }
                }

                if (!hasChanged)
                {
                    break;
                }
            }

            // check whether graph has a negative cycle
            _isValidPath = !hasChanged;
        }

        #endregion
    }
}
