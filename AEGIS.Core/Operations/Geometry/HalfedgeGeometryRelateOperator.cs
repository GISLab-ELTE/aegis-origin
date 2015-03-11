/// <copyright file="HalfedgeGeometryRelateOperator.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Topology;
using System;
using System.Linq;

namespace ELTE.AEGIS.Operations.Geometry
{
    /// <summary>
    /// Represents an operator computing spatial relations using the halfedge topology model.
    /// </summary>
    public class HalfedgeGeometryRelateOperator : IGeometryRelateOperator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HalfedgeGeometryRelateOperator" /> class.
        /// </summary>
        public HalfedgeGeometryRelateOperator() { }

        #endregion

        #region IGeometryRelateOperator methods

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially contains another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially contains the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Contains(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            return !Within(otherGeometry, geometry);
        }

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially crosses another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially crosses the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Crosses(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return false;

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);

                if (graph.Vertices.All(vertex => vertex.Tag == Tag.None || vertex.Tag == Tag.Both))
                    return false;
                if (graph.Vertices.Any(vertex => vertex.Tag == Tag.A) && graph.Vertices.Any(vertex => vertex.Tag == Tag.B))
                    return true;

                return false;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance is disjoint from another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry is disjoint from the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Disjoint(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // check whether the envelopes are disjoint
            if (!geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return false;

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);

                if (graph.Faces.Any(face => face.Tag.HasFlag(Tag.A) && face.Tag.HasFlag(Tag.B)))
                    return false;
                if (graph.Edges.Any(edge => edge.Tag.HasFlag(Tag.A) && edge.Tag.HasFlag(Tag.B)))
                    return false;
                if (graph.Vertices.Any(vertex => vertex.Tag.HasFlag(Tag.A) && vertex.Tag.HasFlag(Tag.B)))
                    return false;
                return true;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="IGeometry" /> instances are spatially equal.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the specified <see cref="IGeometry" /> instances are spatially equal; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Equals(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // check whether the envelope are equal
            if (!geometry.Envelope.Equals(otherGeometry.Envelope))
                return false;

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);

                if (graph.Faces.Any(face => !face.Tag.HasFlag(Tag.A) || !face.Tag.HasFlag(Tag.B)))
                    return false;
                if (graph.Edges.Any(edge => !edge.Tag.HasFlag(Tag.A) || !edge.Tag.HasFlag(Tag.B)))
                    return false;
                if (graph.Vertices.Any(vertex => !vertex.Tag.HasFlag(Tag.A) || !vertex.Tag.HasFlag(Tag.B)))
                    return false;
                return true;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially intersects another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially intersects the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Intersects(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            return !Disjoint(geometry, otherGeometry);
        }

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially overlaps another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially overlaps the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Overlaps(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // the dimensions of the geometries must be at least 2, and must be equal
            if (geometry.Dimension < 2 || otherGeometry.Dimension < 2)
                return false;
            if (geometry.Dimension != otherGeometry.Dimension)
                return false;

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return false;

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);

                if (graph.Vertices.All(vertex => vertex.Tag == Tag.None || vertex.Tag == Tag.Both))
                    return false;
                if (graph.Vertices.Any(vertex => vertex.Tag == Tag.A) && graph.Vertices.Any(vertex => vertex.Tag == Tag.B))
                {
                    // the dimension of the intersecting part must match the dimension of the geometries
                    if (graph.Faces.Any(face => face.Tag == Tag.Both))
                        return geometry.Dimension == 3;
                    else
                        return geometry.Dimension == 2;
                }

                return false;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance spatially touches another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry spatially touches the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Touches(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return false;

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);

                if (graph.Faces.Any(face => face.Tag.HasFlag(Tag.A) && face.Tag.HasFlag(Tag.B)))
                    return false;
                if (graph.Edges.Any(edge => edge.Tag.HasFlag(Tag.A) && edge.Tag.HasFlag(Tag.B)))
                    return true;
                if (graph.Vertices.Any(vertex => vertex.Tag.HasFlag(Tag.A) && vertex.Tag.HasFlag(Tag.B)))
                    return true;
                return false;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Determines whether a <see cref="IGeometry" /> instance is spatially within another <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns><c>true</c> if the geometry is spatially within the other geometry; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Within(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return false;

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);

                if (graph.Faces.Any(face => !face.Tag.HasFlag(Tag.A) || face.Tag.HasFlag(Tag.B)))
                    return false;
                if (graph.Edges.Any(edge => !edge.Tag.HasFlag(Tag.A) || edge.Tag.HasFlag(Tag.B)))
                    return false;
                if (graph.Vertices.Any(vertex => !vertex.Tag.HasFlag(Tag.A) || vertex.Tag.HasFlag(Tag.B)))
                    return false;
                if (graph.Faces.Any(face => face.Tag.HasFlag(Tag.B) && !face.Tag.HasFlag(Tag.A)))
                    return true;

                return false;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }

        #endregion

        #region Private methods

        /// <summary>
        /// Merges the specified geometries.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The halfedge graph contatining the geometries.</returns>
        private static HalfedgeGraph Merge(IGeometry geometry, IGeometry otherGeometry)
        {
            HalfedgeGraph graph = new HalfedgeGraph();
            graph.MergeGeometry(geometry);

            HalfedgeGraph otherGraph = new HalfedgeGraph();
            otherGraph.MergeGeometry(otherGeometry);

            graph.MergeGraph(otherGraph);

            return graph;
        }

        #endregion
    }
}
