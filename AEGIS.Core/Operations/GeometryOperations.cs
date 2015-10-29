///<copyright file="GeometryOperations.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Geometry;
using System;

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Represents a type performing spatial operations on <see cref="IGeometry"/> instances.
    /// </summary>
    public static class GeometryOperations
    {
        #region Buffer methods

        /// <summary>
        /// Computes the buffer of the specified <see cref="IGeometry" /> instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <returns>The buffer of the specified <see cref="IGeometry" /> instance.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public static IGeometry Buffer(this IGeometry geometry, Double distance)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            using (IGeometryBufferOperator op = GetBufferOperator(geometry))
            {
                return op.Buffer(geometry, distance);
            }
        }

        #endregion

        #region Convex hull methods

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

            using (IGeometryConvexHullOperator op = GetConvexHullOperator(geometry))
            {
                return op.ConvexHull(geometry);
            }
        }

        #endregion

        #region Measurement methods

        /// <summary>
        /// Computes the distance between the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The distance between the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Double Distance(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryMeasureOperator op = GetMeasureOperator(geometry))
            {
                return op.Distance(geometry, otherGeometry);
            }
        }

        #endregion

        #region Overlay methods

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

            using (IGeometryOverlayOperator op = GetOverlayOperator(geometry))
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

            using (IGeometryOverlayOperator op = GetOverlayOperator(geometry))
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

            using (IGeometryOverlayOperator op = GetOverlayOperator(geometry))
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

            using (IGeometryOverlayOperator op = GetOverlayOperator(geometry))
            {
                return op.Union(geometry, otherGeometry);
            }
        }

        #endregion

        #region Relation methods

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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Boolean Contains(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryRelateOperator op = GetRelateOperator(geometry))
            {
                return op.Contains(geometry, otherGeometry);
            }
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Boolean Crosses(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryRelateOperator op = GetRelateOperator(geometry))
            {
                return op.Crosses(geometry, otherGeometry);
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Boolean Disjoint(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryRelateOperator op = GetRelateOperator(geometry))
            {
                return op.Disjoint(geometry, otherGeometry);
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Boolean Equals(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryRelateOperator op = GetRelateOperator(geometry))
            {
                return op.Equals(geometry, otherGeometry);
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Boolean Intersects(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryRelateOperator op = GetRelateOperator(geometry))
            {
                return op.Intersects(geometry, otherGeometry);
            }
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Boolean Overlaps(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryRelateOperator op = GetRelateOperator(geometry))
            {
                return op.Overlaps(geometry, otherGeometry);
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
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Boolean Touches(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryRelateOperator op = GetRelateOperator(geometry))
            {
                return op.Touches(geometry, otherGeometry);
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
        /// <exception cref="System.ArgumentException">
        /// The reference system of two geometries are different.
        /// or
        /// The operation is not supported with the specified geometry type.
        /// </exception>
        public static Boolean Within(this IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            using (IGeometryRelateOperator op = GetRelateOperator(geometry))
            {
                return op.Within(geometry, otherGeometry);
            }
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Ensures an underlying geometry operator factory for the specified geometry factory.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The factory with the appropriate geometry operations factory.</returns>
        private static void EnsureFactory(IGeometryFactory factory)
        {
            if (factory.ContainsFactory<IGeometryOperatorFactory>())
                return;

            factory.EnsureFactory<IGeometryOperatorFactory>(new GeometryOperatorFactory(factory));
        }

        /// <summary>
        /// Returns the buffer operator for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The buffer operator of the specified geometry, if any; otherwise, the default buffer operator.</returns>
        private static IGeometryBufferOperator GetBufferOperator(IGeometry geometry)
        {
            EnsureFactory(geometry.Factory);

            return geometry.Factory.GetFactory<IGeometryOperatorFactory>().Buffer;
        }

        /// <summary>
        /// Returns the convex hull operator for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The convex hull operator of the specified geometry, if any; otherwise, the default convex hull operator.</returns>
        private static IGeometryConvexHullOperator GetConvexHullOperator(IGeometry geometry)
        {
            EnsureFactory(geometry.Factory);

            return geometry.Factory.GetFactory<IGeometryOperatorFactory>().ConvexHull;
        }

        /// <summary>
        /// Returns the measure operator for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The measure operator of the specified geometry, if any; otherwise, the default measure operator.</returns>
        private static IGeometryMeasureOperator GetMeasureOperator(IGeometry geometry)
        {
            EnsureFactory(geometry.Factory);

            return geometry.Factory.GetFactory<IGeometryOperatorFactory>().Measure;
        }

        /// <summary>
        /// Returns the overlay operator for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The overlay operator of the specified geometry, if any; otherwise, the default overlay operator.</returns>
        private static IGeometryOverlayOperator GetOverlayOperator(IGeometry geometry)
        {
            EnsureFactory(geometry.Factory);

            return geometry.Factory.GetFactory<IGeometryOperatorFactory>().Overlay;
        }

        /// <summary>
        /// Returns the relate operator for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The relate operator of the specified geometry, if any; otherwise, the default relate operator.</returns>
        private static IGeometryRelateOperator GetRelateOperator(IGeometry geometry)
        {
            EnsureFactory(geometry.Factory);

            return geometry.Factory.GetFactory<IGeometryOperatorFactory>().Relate;
        }

        #endregion
    }
}
