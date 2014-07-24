/// <copyright file="ShortestPathTreeAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
        /// The distance metric parameter.
        /// </summary>
        protected readonly Func<IGraphEdge, Double> _distanceMetric;

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
            _distanceMetric = ResolveParameter<Func<IGraphEdge, Double>>(GraphOperationParameters.DistanceMetric);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult() 
        {
            _parent = new Dictionary<IGraphVertex, IGraphVertex>();
            _distance = new Dictionary<IGraphVertex, Double>();
            _isValidPath = true;
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        protected override void FinalizeResult()
        {
            // if the path is not valid, there is no result
            if (!_isValidPath)
            {
                _result = null;
                return;
            }
            
            // generate a network from the path
            if (_result == null)
                _result = _source.Factory.CreateNetwork();

            _parent.Remove(_sourceVertex); // removing the source, we really don't want an edge between a vertex and a null.
            foreach (KeyValuePair<IGraphVertex, IGraphVertex> vertexPair in _parent)
            {
                IMetadataCollection metadata = _result.Factory.CreateMetadata();
                metadata["Distance"] = _distance[vertexPair.Key];

                if (!_result.Contains(vertexPair.Key)) // child
                {
                    _result.AddVertex(vertexPair.Key.Coordinate);
                }
                if (!_result.Contains(vertexPair.Value)) // parent
                {
                    _result.AddVertex(vertexPair.Value.Coordinate);
                }

                _result.AddEdge(vertexPair.Value, vertexPair.Key, metadata);
            }
        }

        #endregion
    }
}
