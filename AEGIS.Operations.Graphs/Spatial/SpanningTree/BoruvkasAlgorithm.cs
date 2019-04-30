/// <copyright file="BoruvkasAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dávid Kis</author>

using ELTE.AEGIS.Collections;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace ELTE.AEGIS.Operations.Spatial.SpanningTree
{
    /// <summary>
    /// Represents an operation performing Borůvka's algorithm(also known as Sollin's algorithm).
    /// </summary>
    /// <remarks>
    /// Borůvka's algorithm is an algorithm for finding a minimum spanning tree in a graph for which all edge weights are distinct.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::225508", "Borůvka's algorithm")]
    public class BoruvkasAlgorithm : MinimumSpanningTreeAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The disjoint-set forest to track the components.
        /// </summary>
        private DisjointSetForest<IGraphVertex> _forest;

        /// <summary>
        /// The set to track the cheapest out-edges.
        /// </summary>
        private HashSet<IGraphEdge> _edgeSet; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoruvkasAlgorithm" /> class.
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
        public BoruvkasAlgorithm(IGeometryGraph source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoruvkasAlgorithm" /> class.
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
        public BoruvkasAlgorithm(IGeometryGraph source, IGeometryGraph target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, GraphOperationMethods.BoruvkasAgorithm, parameters)
        {
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            _edgeSet = new HashSet<IGraphEdge>();
            _forest = new DisjointSetForest<IGraphVertex>(Source.Vertices);

            while (_forest.SetCount > 1)
            {
                IGraphVertex currentRep = null;

                foreach (IGraphVertex vertex in _forest.OrderedEnumerator)
                {
                    if (currentRep != _forest.Find(vertex))
                    {
                        //new component
                        currentRep = _forest.Find(vertex);
                        if (_edgeSet.Count > 0)
                        {
                            _spanningEdges.Add(_edgeSet.MinBy(x => _weightMetric(x)));
                        }
                        _edgeSet.Clear();
                    }

                    if (Source.OutEdges(vertex).Any(x => _forest.Find(x.Target) != currentRep))
                        _edgeSet.Add(
                            Source.OutEdges(vertex)
                                .Where(x => _forest.Find(x.Target) != currentRep)
                                .MinBy(y => _weightMetric(y)));
                }
                
                if (_edgeSet.Count > 0)
                {
                    //on the last element there is no component change
                    _spanningEdges.Add(_edgeSet.MinBy(x => _weightMetric(x)));
                }

                _edgeSet.Clear();

                foreach (IGraphEdge spanningEdge in _spanningEdges)
                {
                    _forest.Union(spanningEdge.Source, spanningEdge.Target);
                }
            }
        }

        #endregion
    }
}
