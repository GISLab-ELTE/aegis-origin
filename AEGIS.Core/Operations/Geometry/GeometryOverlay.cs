///<copyright file="GeometryOverlay.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations.Geometry
{
    /// <summary>
    /// Represents a type performing spatial overlay operations on <see cref="IGeometry" /> instances.
    /// </summary>
    public static class GeometryOverlay
    {
        #region Public static (extension) methods

        /// <summary>
        /// Computes the buffer of the specified <see cref="IGeometry" /> instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <returns>The buffer of the specified <see cref="IGeometry" /> instance.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The distance is negative.</exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public static IGeometry Buffer(this IGeometry geometry, Double distance)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            
            using (IGeometryOverlayOperator op = GetOperator(geometry))
            {
                return op.Buffer(geometry, distance);
            }
        }

        /// <summary>
        /// Computes the convex hull of the specified <see cref="IGeometry" /> instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The convex hull of the <see cref="IGeometry" /> instance.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public static IGeometry ConvexHull(this IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            
            using (IGeometryOverlayOperator op = GetOperator(geometry))
            {
                return op.ConvexHull(geometry);
            }
        }

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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static IGeometry Difference(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryOverlayOperator op = GetOperator(geometry))
            {
                return op.Difference(geometry, otherGeometry);
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static IGeometry Intersection(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryOverlayOperator op = GetOperator(geometry))
            {
                return op.Intersection(geometry, otherGeometry);
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static IGeometry SymmetricDifference(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryOverlayOperator op = GetOperator(geometry))
            {
                return op.SymmetricDifference(geometry, otherGeometry);
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static IGeometry Union(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryOverlayOperator op = GetOperator(geometry))
            {
                return op.Union(geometry, otherGeometry);
            }
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Returns the overlay operator for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The overlay operator of the specified geometry, if any; otherwise, the default overlay operator.</returns>
        private static IGeometryOverlayOperator GetOperator(IGeometry geometry)
        {
            IGeometryOperatorFactory factory = geometry.Factory.GetFactory<IGeometryOperatorFactory>();

            if (factory == null)
                return Factory.DefaultInstance<IGeometryOperatorFactory>().Overlay;

            return factory.Overlay;
        }

        #endregion
    }
}
