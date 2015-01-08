/// <copyright file="HalfedgeGeometryOverlayOperator.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an operator computing spatial overlays using the halfedge topology model.
    /// </summary>
    public class HalfedgeGeometryOverlayOperator : IGeometryOverlayOperator
    {
        #region IGeometryOverlayOperator methods

        /// <summary>
        /// Computes the difference between the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The difference between the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry Difference(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);
                return geometry.Factory.CreateGeometryCollection(graph.Faces.Where(face => !face.IsHole && face.Tag == Tag.A).Select(face => face.ToGeometry(geometry.Factory)));
            }
            catch (ArgumentException) 
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Computes the intersection of the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The intersection of the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry Intersection(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return null;

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);
                return geometry.Factory.CreateGeometryCollection(graph.Faces.Where(face => !face.IsHole && face.Tag == Tag.Both).Select(face => face.ToGeometry(geometry.Factory)));
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Computes the symmetric difference between the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The symmetric difference between the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry SymmetricDifference(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);
                return geometry.Factory.CreateGeometryCollection(graph.Faces.Where(face => !face.IsHole && (face.Tag == Tag.A || face.Tag == Tag.B)).Select(face => face.ToGeometry(geometry.Factory)));
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Computes the union of the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The union of the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry Union(IGeometry geometry, IGeometry otherGeometry)
        {

            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            try
            {
                HalfedgeGraph graph = Merge(geometry, otherGeometry);
                return graph.ToGeometry(geometry.Factory);
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
