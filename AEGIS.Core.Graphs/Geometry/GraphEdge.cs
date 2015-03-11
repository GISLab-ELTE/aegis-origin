/// <copyright file="GraphEdge.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a graph edge.
    /// </summary>
    public class GraphEdge : IGraphEdge
    {
        #region Protected fields

        protected readonly IGeometryGraph _graph;
        protected readonly GraphVertex _source;
        protected readonly GraphVertex _target;
        protected IMetadataCollection _metadata;

        #endregion

        #region IEdge properties

        /// <summary>
        /// Gets the graph of the edge.
        /// </summary>
        /// <value>The graph the edge belongs to.</value>
        public IGeometryGraph Graph { get { return _graph; } }

        /// <summary>
        /// Gets the source vertex.
        /// </summary>
        public IGraphVertex Source { get { return _source; } }

        /// <summary>
        /// Gets the target vertex.
        /// </summary>
        public IGraphVertex Target { get { return _target; } }

        /// <summary>
        /// Gets the length of the edge.
        /// </summary>
        public Double Length { get { return Coordinate.Distance(_source.Coordinate, _target.Coordinate); } }
        
        #endregion

        #region IMetadataProvider properties

        /// <summary>
        /// Gets the metadata container.
        /// </summary>
        public IMetadataCollection Metadata { get { return _metadata; } set { _metadata = value; } }

        /// <summary>
        /// Gets or sets the metadata value for a specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The metadata value with the <paramref name="key" /> if it exists; otherwise, <c>null</c>.</returns>
        public Object this[String key]
        {
            get
            {
                Object value = null;
                if (_metadata != null)
                    _metadata.TryGetValue(key, out value);
                return value;
            }
            set
            {
                if (_metadata == null)
                    _metadata = _graph.Factory.GetFactory<IMetadataFactory>().CreateCollection();
                _metadata[key] = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphEdge" /> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="metadata">The metadata.</param>
        public GraphEdge(IGeometryGraph graph, GraphVertex source, GraphVertex target, IDictionary<String, Object> metadata)
        {
            _graph = graph;
            _source = source; 
            _target = target;
            _metadata = _graph.Factory.GetFactory<IMetadataFactory>().CreateCollection(metadata);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return _source.ToString() + " -> " + _target.ToString();
        }

        #endregion
    }
}
