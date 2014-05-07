/// <copyright file="IGraphEdge.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using System;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents an edge of a graph.
    /// </summary>
    public interface IGraphEdge : IMetadataProvider
    {
        /// <summary>
        /// Gets the graph of the edge.
        /// </summary>
        /// <value>The graph the edge belongs to.</value>
        IGeometryGraph Graph { get; }

        /// <summary>
        /// Gets the source vertex.
        /// </summary>
        /// <value>The source vertex.</value>
        IGraphVertex Source { get; }

        /// <summary>
        /// Gets the target vertex.
        /// </summary>
        /// <value>The target vertex.</value>
        IGraphVertex Target { get; }

        /// <summary>
        /// Gets the length of the edge.
        /// </summary>
        /// <value>The euclid distance of the source and target vertices.</value>
        Double Length { get; }
    }
}
