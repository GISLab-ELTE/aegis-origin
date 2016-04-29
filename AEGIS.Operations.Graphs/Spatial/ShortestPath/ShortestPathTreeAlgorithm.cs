/// <copyright file="ShortestPathTreeAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Krisztián Fodor</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spatial.ShortestPath
{
    /// <summary>
    /// Represents an operation computing the shortest path from a source vertex to all other vertices.
    /// </summary>
    public abstract class ShortestPathTreeAlgorithm : Operation<IGeometryGraph, IGeometryGraph>
    {
        #region Protected fields

        /// <summary>
        /// The source vertex parameter.
        /// </summary>
        protected readonly IGraphVertex _sourceVertex;

        /// <summary>
        /// The metric used to compute the weight of edges.
        /// </summary>
        protected readonly Func<IGraphEdge, Double> _weightMetric;

        /// <summary>
        /// The dictionary of parent vertices.
        /// </summary>
        protected Dictionary<IGraphVertex, IGraphVertex> _parent;

        /// <summary>
        /// The dictionary of distances.
        /// </summary>
        protected Dictionary<IGraphVertex, Double> _distance;

        /// <summary>
        /// A value indicating whether the path is valid.
        /// </summary>
        protected Boolean _isValidPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortestPathTreeAlgorithm"/> class.
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
        protected ShortestPathTreeAlgorithm(IGeometryGraph source, IGeometryGraph target, OperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _sourceVertex = ResolveParameter<IGraphVertex>(GraphOperationParameters.SourceVertex);
            _weightMetric = ResolveParameter<Func<IGraphEdge, Double>>(GraphOperationParameters.WeightMetric);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The result object.</returns>
        protected override IGeometryGraph PrepareResult() 
        {
            _parent = new Dictionary<IGraphVertex, IGraphVertex>(Source.VertexComparer);
            _distance = new Dictionary<IGraphVertex, Double>(Source.VertexComparer);
            _isValidPath = true;

            return Source.Factory.CreateGraph(Source.VertexComparer, Source.EdgeComparer);
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override IGeometryGraph FinalizeResult()
        {
            // if the path is not valid, there is no result
            if (!_isValidPath)
                return Result;
            
            // generate a graph from the path
            _parent.Remove(_sourceVertex); // removing the source, we really don't want an edge between a vertex and a null.
            foreach (IGraphVertex vertex in _parent.Keys)
            {
                IGraphVertex sourceVertex = Result.GetVertex(_parent[vertex].Coordinate);
                IGraphVertex targetVertex = Result.GetVertex(vertex.Coordinate);

                if (sourceVertex == null)
                {
                    sourceVertex = Result.AddVertex(_parent[vertex].Coordinate);
                    sourceVertex["Distance"] = _distance[_parent[vertex]];
                }

                if (targetVertex == null)
                {
                    targetVertex = Result.AddVertex(vertex.Coordinate);
                    targetVertex["Distance"] = _distance[vertex];
                }

                Result.AddEdge(sourceVertex, targetVertex);
            }

            return Result;
        }

        #endregion
    }
}
