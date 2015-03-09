/// <copyright file="GraphReferenceTransformation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Operations.Management;
using ELTE.AEGIS.Operations.Spatial.Strategy;
using ELTE.AEGIS.Reference;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spatial
{
    /// <summary>
    /// Represents a reference system transformation.
    /// </summary>
    [OperationMethodImplementation("AEGIS::212901", "Reference system transformation", "1.0.0", typeof(GraphReferenceTransformationCredential))]
    public class GraphReferenceTransformation : Operation<IGeometry, IGeometry>
    {
        #region Private fields

        /// <summary>
        /// The target reference system. This field is read-only.
        /// </summary>
        private readonly IReferenceSystem _targetReferenceSystem;

        /// <summary>
        /// The value indicating whether the geometry metadata is preserved. This field is read-only.
        /// </summary>
        private readonly Boolean _metadataPreservation;

        /// <summary>
        /// The geometry factory used for producing the result.
        /// </summary>
        private IGeometryFactory _factory;

        /// <summary>
        /// The transformation strategy.
        /// </summary>
        private ITransformationStrategy _transformation;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphReferenceTransformation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        public GraphReferenceTransformation(IGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, ReferenceOperationMethods.ReferenceTransformation, parameters)
        {
            _targetReferenceSystem = ResolveParameter<IReferenceSystem>(ReferenceOperationParameters.TargetReferenceSystem);
            _metadataPreservation = ResolveParameter<Boolean>(CommonOperationParameters.MetadataPreservation);
            _factory = ResolveParameter<IGeometryFactory>(CommonOperationParameters.GeometryFactory);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            if (_source.ReferenceSystem != null && _targetReferenceSystem != null && !_source.ReferenceSystem.Equals(_targetReferenceSystem))
            {
                // strategy pattern
                _transformation = TransformationStrategyFactory.CreateStrategy(_source.ReferenceSystem as ReferenceSystem, _targetReferenceSystem as ReferenceSystem);
            }

            if (_factory == null)
                _factory = (IGeometryFactory)FactoryRegistry.GetFactory(FactoryRegistry.GetContract(_source.Factory), _targetReferenceSystem);
        }

        /// <summary>
        /// Computes the result.
        /// </summary>
        protected override void ComputeResult()
        {
            _result = Compute(_source);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the transformation of the source geometry.
        /// </summary>
        /// <param name="source">The source geometry.</param>
        /// <returns>The geometry in the specified reference system.</returns>
        /// <exception cref="System.InvalidOperationException">Transformation of the specified geometry is not supported.</exception>
        private IGeometry Compute(IGeometry source)
        {
            if (source == null)
                return null;

            if (source.ReferenceSystem == null || source.ReferenceSystem.Equals(_targetReferenceSystem))
                return source;

            if (source is IGeometryGraph)
                return Compute(source as IGeometryGraph);
            if (source is IGeometryCollection<IGeometryGraph>)
                return Compute(source as IGeometryCollection<IGeometryGraph>);

            throw new InvalidOperationException("Transformation of the specified geometry is not supported.");
        }

        /// <summary>
        /// Computes the transformation of the source geometry graph.
        /// </summary>
        /// <param name="source">The source geometry graph.</param>
        /// <returns>The geometry graph in the specified reference system.</returns>
        private IGeometryGraph Compute(IGeometryGraph source)
        {
            IGeometryGraph graph = _factory.CreateGraph(source.VertexComparer, source.EdgeComparer, _metadataPreservation ? source.Metadata : null);

            Dictionary<IGraphVertex, IGraphVertex> vertexMapping = new Dictionary<IGraphVertex, IGraphVertex>();

            foreach (IGraphVertex vertex in source.Vertices)
            {
                vertexMapping.Add(vertex, graph.AddVertex(Compute(vertex.Coordinate), _metadataPreservation ? vertex.Metadata : null));
            }

            foreach (IGraphVertex vertex in source.Vertices)
            {
                foreach (IGraphEdge edge in source.OutEdges(vertex))
                    graph.AddEdge(vertexMapping[edge.Source], vertexMapping[edge.Target], _metadataPreservation ? edge.Metadata : null);
            }

            return graph;
        }

        /// <summary>
        /// Computes the transformation of the source geometry.
        /// </summary>
        /// <param name="source">The source geometry.</param>
        /// <returns>The geometry in the specified reference system.</returns>
        private IGeometryCollection<IGeometryGraph> Compute(IGeometryCollection<IGeometryGraph> source)
        {
            return _factory.CreateGeometryCollection(source.Select(geometry => Compute(geometry)),
                                                     _metadataPreservation ? source.Metadata : null);
        }
       
        /// <summary>
        /// Computes the transformation of the coordinate.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <returns>The coordinate in the specified reference system.</returns>
        private Coordinate Compute(Coordinate coordinate)
        {
            return _transformation.Transform(coordinate);
        }

        #endregion
    }
}
