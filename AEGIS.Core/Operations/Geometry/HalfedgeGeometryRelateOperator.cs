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
/// <author>Máté Cserép</author>

using ELTE.AEGIS.Topology;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Geometry
{
    /// <summary>
    /// Represents an operator computing spatial relations using the halfedge topology model.
    /// </summary>
    public class HalfedgeGeometryRelateOperator : IGeometryRelateOperator
    {
        // Source: http://edndoc.esri.com/arcsde/9.0/general_topics/understand_spatial_relations.htm

        #region Private fields

        /// <summary>
        /// The halfedge graph to operate on.
        /// </summary>
        private IHalfedgeGraph _graph;

        /// <summary>
        /// Identifiers present in the first operand.
        /// </summary>
        private ISet<Int32> _aIdentifiers;

        /// <summary>
        /// Identifiers present in the second operand.
        /// </summary>
        private ISet<Int32> _bIdentifiers;

        #endregion

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
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Contains(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            return Within(otherGeometry, geometry);
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
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Crosses(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // the dimensions of the geometries must be different
            if (geometry.Dimension == otherGeometry.Dimension)
                return false;

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return false;

            try
            {
                Merge(geometry, otherGeometry);
                if (_graph.Faces.Any(face => IsBoth(face.Identifiers)))
                    return false;
                if (_graph.Edges.Any(edge => IsBoth(edge.Identifiers)))
                    return false;
                if (_graph.Vertices.All(vertex => IsNone(vertex.Identifiers) || IsBoth(vertex.Identifiers)))
                    return false;
                if (_graph.Vertices.Any(vertex => IsFirst(vertex.Identifiers)) && 
                    _graph.Vertices.Any(vertex => IsSecond(vertex.Identifiers)))
                    return true;

                return false;
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
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
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Disjoint(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return true;

            try
            {
                Merge(geometry, otherGeometry);
                if (_graph.Faces.Any(face => IsBoth(face.Identifiers)))
                    return false;
                if (_graph.Edges.Any(edge => IsBoth(edge.Identifiers)))
                    return false;
                if (_graph.Vertices.Any(vertex => IsBoth(vertex.Identifiers)))
                    return false;
                return true;
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
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
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
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
                Merge(geometry, otherGeometry);
                if (_graph.Faces.Any(face => !IsBoth(face.Identifiers)))
                    return false;
                if (_graph.Edges.Any(edge => !IsBoth(edge.Identifiers)))
                    return false;
                if (_graph.Vertices.Any(vertex => !IsBoth(vertex.Identifiers)))
                    return false;
                return true;
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
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
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
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
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Overlaps(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // the dimensions of the geometries must be equal
            if (geometry.Dimension != otherGeometry.Dimension)
                return false;

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return false;

            try
            {
                Merge(geometry, otherGeometry);
                if (_graph.Vertices.All(vertex => IsNone(vertex.Identifiers) || IsBoth(vertex.Identifiers)))
                    return false;
                if (_graph.Vertices.Any(vertex => IsFirst(vertex.Identifiers)) && 
                    _graph.Vertices.Any(vertex => IsSecond(vertex.Identifiers)))
                {
                    // the dimension of the intersecting part must match the dimension of the geometries
                    if (_graph.Faces.Any(face => IsBoth(face.Identifiers)))
                        return geometry.Dimension == 2;
                    if (_graph.Edges.Any(edge => IsBoth(edge.Identifiers)))
                        return geometry.Dimension == 1;

                   return geometry.Dimension == 0;
                }

                return false;
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
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
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
        public Boolean Touches(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // the dimension of least one geometry must be least 2
            if (Math.Max(geometry.Dimension, otherGeometry.Dimension) < 2)
                return false;

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return false;

            try
            {
                Merge(geometry, otherGeometry);
                if (_graph.Faces.Any(face => IsBoth(face.Identifiers)))
                    return false;
                if (_graph.Edges.Any(edge => IsBoth(edge.Identifiers)))
                    return true;
                if (_graph.Vertices.Any(vertex => IsBoth(vertex.Identifiers)))
                    return true;
                return false;
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
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
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
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
                Merge(geometry, otherGeometry);
                if (_graph.Faces.Any(face => IsFirst(face.Identifiers)))
                    return false;
                if (_graph.Edges.Any(edge => IsFirst(edge.Identifiers)))
                    return false;
                if (_graph.Vertices.Any(vertex => IsFirst(vertex.Identifiers)))
                    return false;
                if (_graph.Faces.Any(face => IsSecond(face.Identifiers)))
                    return true;

                return false;
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
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
        /// <returns>The halfedge graph containing the geometries.</returns>
        private void Merge(IGeometry geometry, IGeometry otherGeometry)
        {
            _graph = new HalfedgeGraph(new HalfedgeGraph.FixedIdentifierProvider(1));
            _graph.MergeGeometry(geometry);
            _aIdentifiers = new HashSet<Int32>(_graph.Vertices.SelectMany(v => v.Identifiers));

            HalfedgeGraph otherGraph = new HalfedgeGraph(new HalfedgeGraph.FixedIdentifierProvider(2));
            otherGraph.MergeGeometry(otherGeometry);
            _bIdentifiers = new HashSet<Int32>(otherGraph.Vertices.SelectMany(v => v.Identifiers));

            _graph.MergeGraph(otherGraph);
        }

        /// <summary>
        /// Determines whether a set of identifiers are present in none of the operands.
        /// </summary>
        /// <param name="identifiers"></param>
        /// <returns><c>true</c> if none of the <paramref name="identifiers"/> are present in either of the operands; <c>false</c> otherwise.</returns>
        private Boolean IsNone(ISet<Int32> identifiers)
        {
            return identifiers.All(id => !_aIdentifiers.Contains(id) && !_bIdentifiers.Contains(id));
        }

        /// <summary>
        /// Determines whether a set of identifiers are present in both operands.
        /// </summary>
        /// <param name="identifiers">The identifiers to match.</param>
        /// <returns><c>true</c> if all of the <paramref name="identifiers"/> are present in both of the operands; <c>false</c> otherwise.</returns>
        private Boolean IsBoth(ISet<Int32> identifiers)
        {
            return identifiers.Any(id => _aIdentifiers.Contains(id)) &&
                   identifiers.Any(id => _bIdentifiers.Contains(id));
        }

        /// <summary>
        /// Determines whether a set of identifiers are present in only the first operand.
        /// </summary>
        /// <param name="identifiers">The identifiers to match.</param>
        /// <returns><c>true</c> if all of the <paramref name="identifiers"/> are present in only the first operand; <c>false</c> otherwise.</returns>
        private Boolean IsFirst(ISet<Int32> identifiers)
        {
            return identifiers.All(id => _aIdentifiers.Contains(id) && !_bIdentifiers.Contains(id));
        }

        /// <summary>
        /// Determines whether a set of identiers are present in only the second operand.
        /// </summary>
        /// <param name="identifiers">The identifiers to match.</param>
        /// <returns><c>true</c> if all of the <paramref name="identifiers"/> are present in only the second operand; <c>false</c> otherwise.</returns>
        private Boolean IsSecond(ISet<Int32> identifiers)
        {
            return identifiers.All(id => !_aIdentifiers.Contains(id) && _bIdentifiers.Contains(id));
        }

        #endregion
    }
}
