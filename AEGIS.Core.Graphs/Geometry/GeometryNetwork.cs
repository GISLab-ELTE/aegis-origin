/// <copyright file="GeometryNetwork.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a graph form of geometry in spatial coordinate space, where every vertex has unique coordinates.
    /// </summary>
    public class GeometryNetwork : GeometryGraph
    {
        #region Protected types

        /// <summary>
        /// Represents an equality comparer for vertices based on coordinates.
        /// </summary>
        protected class VertexCoordinateEqualityComparer : IEqualityComparer<IGraphVertex>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="GraphVertex" /> to compare.</param>
            /// <param name="y">The second object of type <see cref="GraphVertex" /> to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public Boolean Equals(IGraphVertex x, IGraphVertex y)
            {
                return x.Coordinate.Equals(y.Coordinate);
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="GraphVertex" /> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            /// <exception cref="System.ArgumentNullException">obj;The type of obj is a reference type and obj is null.</exception>
            public Int32 GetHashCode(IGraphVertex obj)
            {
                if (obj == null)
                    throw new ArgumentNullException("The type of obj is a reference type and obj is null.");

                return obj.Coordinate.GetHashCode();
            }
        }

        /// <summary>
        /// Represents an equality comparer for edges based on coordinates.
        /// </summary>
        protected class EdgeCoordinateEqualityComparer : IEqualityComparer<IGraphEdge>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="GraphEdge" /> to compare.</param>
            /// <param name="y">The second object of type <see cref="GraphEdge" /> to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public Boolean Equals(IGraphEdge x, IGraphEdge y)
            {
                return (x.Source.Coordinate.Equals(y.Source.Coordinate) && x.Target.Coordinate.Equals(y.Target.Coordinate));
        
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="GraphEdge" /> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            /// <exception cref="System.ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
            public Int32 GetHashCode(IGraphEdge obj)
            {
                if (obj == null)
                    throw new ArgumentNullException("The type of obj is a reference type and obj is null.");

                return (obj.Source.Coordinate.GetHashCode() >> 2) ^ obj.Target.Coordinate.GetHashCode() ^ 721602071;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryNetwork" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public GeometryNetwork(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(new VertexCoordinateEqualityComparer(), new EdgeCoordinateEqualityComparer(), precisionModel, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryNetwork" /> class.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public GeometryNetwork(IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(new VertexCoordinateEqualityComparer(), new EdgeCoordinateEqualityComparer(), factory, metadata)
        {
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="GeometryNetwork" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="GeometryNetwork" /> instance.</returns>
        public override Object Clone()
        {
            GeometryNetwork result = new GeometryNetwork(Factory, Metadata);
            CloneToGraph(this, result);
            return result;
        }

        #endregion
    }
}
