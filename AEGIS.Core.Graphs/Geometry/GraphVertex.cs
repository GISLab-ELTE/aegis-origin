/// <copyright file="GraphVertex.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a graph vertex.
    /// </summary>
    public class GraphVertex : IGraphVertex
    {
        #region Protected fields

        protected readonly IGeometryGraph _graph;
        protected readonly Coordinate _coordinate;
        protected IMetadataCollection _metadata;

        #endregion

        #region IVertex properties

        /// <summary>
        /// Gets the graph of the vertex.
        /// </summary>
        /// <value>The graph the vertex belongs to.</value>
        public IGeometryGraph Graph { get { return _graph; } }

        /// <summary>
        /// Gets the coordinate of the vertex.
        /// </summary>
        public Coordinate Coordinate { get { return _coordinate; }  }

        #endregion

        #region IMetadataProvider properties

        /// <summary>
        /// Gets the metadata container.
        /// </summary>
        public IMetadataCollection Metadata { get { return _metadata; } }

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
        /// Initializes a new instance of the <see cref="GraphVertex" /> class.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        public GraphVertex(IGeometryGraph graph, Coordinate coordinate, IDictionary<String, Object> metadata)
        {
            _graph = graph;
            _coordinate = coordinate;
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
            return "<" + _coordinate.ToString() + ">";
        }

        #endregion
    }
}
