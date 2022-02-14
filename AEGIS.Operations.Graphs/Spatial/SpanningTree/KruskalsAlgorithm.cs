/// <copyright file="KruskalsAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spatial.SpanningTree
{
    /// <summary>
    /// Represents an operation performing Kruskal's algorithm
    /// </summary>
    /// <remarks>
    /// Kruskal's algorithm is a greedy algorithm that finds a minimum spanning tree or forest for a weighted undirected graph.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::225502", "Kruskal's algorithm")]
    public class KruskalsAlgorithm : MinimumSpanningTreeAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The disjoint-set forest to track the components
        /// </summary>
        private DisjointSetForest<IGraphVertex> _forest; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KruskalsAlgorithm" /> class.
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
        public KruskalsAlgorithm(IGeometryGraph source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KruskalsAlgorithm" /> class.
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
        public KruskalsAlgorithm(IGeometryGraph source, IGeometryGraph target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, GraphOperationMethods.KruskalsAlgorithm, parameters)
        {
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            _forest = new DisjointSetForest<IGraphVertex>(Source.Vertices);
            foreach (IGraphEdge edge in Source.Edges.OrderBy(x => _weightMetric(x)))
            {
                if (_forest.Find(edge.Source) != _forest.Find(edge.Target))
                {
                    _spanningEdges.Add(edge);
                    _forest.Union(edge.Source, edge.Target);
                }
            }
        }

        #endregion
    }
}
