/// <copyright file="EdmondsKarpAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Marcell Lipp</author>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spatial.MaximumFlow
{
    /// <summary>
    /// Represents an operation computing the maximum flow between two vertices in a graph using the Edmonds-Karp algorithm.
    /// </summary>
    [OperationMethodImplementation("AEGIS::212710", "Edmonds-Karp algorithm")]
    public class EdmondsKarpAlgorithm : MaximumFlowComputation
    {
        // TODO: use multikey dictionary for reverse edges that are not available

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmondsKarpAlgorithm"/> class.
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
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public EdmondsKarpAlgorithm(IGeometryGraph source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, GraphOperationMethods.EdmondsKarpAlgorithm, parameters)
        {         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmondsKarpAlgorithm"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="capacity">The capacity.</param>
        /// /// <exception cref="System.ArgumentNullException">
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
        public EdmondsKarpAlgorithm(IGeometryGraph source, IGeometryGraph target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, GraphOperationMethods.EdmondsKarpAlgorithm, parameters)
        {
            
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            // source: http://en.wikipedia.org/wiki/Edmonds%E2%80%93Karp_algorithm

            Int32 capacity;
            Dictionary<IGraphVertex, IGraphVertex> parent;

            while (BreadthFirstSearch(out capacity, out parent))
            {
                _maximumFlow += capacity;

                // backtrack seach
                IGraphVertex currentVertex = _targetVertex;

                while (!_source.VertexComparer.Equals(currentVertex, _sourceVertex))
                {
                    IGraphVertex parentVertex = parent[currentVertex];

                    IGraphEdge forwardEdge = _source.GetEdge(parentVertex, currentVertex);
                    IGraphEdge reverseEdge = _source.GetEdge(currentVertex, parentVertex);

                    // modify used capacity
                    if (!_usedCapacity.ContainsKey(forwardEdge))
                        _usedCapacity.Add(forwardEdge, 0);

                    _usedCapacity[forwardEdge] += capacity;

                    if (reverseEdge != null)
                    {
                        if (!_usedCapacity.ContainsKey(reverseEdge))
                            _usedCapacity.Add(reverseEdge, 0);

                        _usedCapacity[reverseEdge] -= capacity;
                    }

                    currentVertex = parentVertex;
                }

                _isTargetReached = true;
            }
        }

        #endregion

        #region Protected methods 

        /// <summary>
        /// Executes a breath first-search on the graph.
        /// </summary>
        protected Boolean BreadthFirstSearch(out Int32 capacity, out Dictionary<IGraphVertex, IGraphVertex> parent)
        {
            capacity = 0;
            parent = new Dictionary<IGraphVertex, IGraphVertex>(_source.VertexComparer);

            Dictionary<IGraphVertex, Int32> pathCapacity = new Dictionary<IGraphVertex, Int32>(_source.VertexComparer);
            Queue<IGraphVertex> vertexQueue = new Queue<IGraphVertex>();
            vertexQueue.Enqueue(_sourceVertex);

            while (vertexQueue.Count > 0)
            {
                IGraphVertex currentVertex = vertexQueue.Dequeue();

                foreach (IGraphEdge edge in _source.OutEdges(currentVertex))
                {
                    if (!_usedCapacity.ContainsKey(edge))
                        _usedCapacity.Add(edge, 0);

                    // if there is available capacity, and target is not seen before in search
                    if (_capacityMetric(edge) - _usedCapacity[edge] > 0 && !parent.ContainsKey(edge.Target))
                    {
                        parent.Add(edge.Target, currentVertex);
                       
                        if (pathCapacity.ContainsKey(currentVertex))
                            pathCapacity.Add(edge.Target, Math.Min(pathCapacity[currentVertex], _capacityMetric(edge) - _usedCapacity[edge]));
                        else
                            pathCapacity.Add(edge.Target, _capacityMetric(edge) - _usedCapacity[edge]);

                        if (_source.VertexComparer.Equals(_targetVertex, edge.Target))
                        {
                            capacity = pathCapacity[edge.Target];
                            return true;
                        }
                        else
                        {
                            vertexQueue.Enqueue(edge.Target);
                        }
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
