///<copyright file="GeometryRelate.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a type peforming spatial relation operations on <see cref="IGeometry"/> instances.
    /// </summary>
    public static class GeometryRelate
    {
        #region Public static (extension) methods

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

            using (IGeometryRelateOperator op = GetOperator(geometry))
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

            using (IGeometryRelateOperator op = GetOperator(geometry))
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

            using (IGeometryRelateOperator op = GetOperator(geometry))
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

            using (IGeometryRelateOperator op = GetOperator(geometry))
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

            using (IGeometryRelateOperator op = GetOperator(geometry))
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

            using (IGeometryRelateOperator op = GetOperator(geometry))
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

            using (IGeometryRelateOperator op = GetOperator(geometry))
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

            using (IGeometryRelateOperator op = GetOperator(geometry))
            {
                return op.Within(geometry, otherGeometry);
            }
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Returns the relate operator for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The relate operator of the specified geometry, if any; otherwise, the default relate operator.</returns>
        private static IGeometryRelateOperator GetOperator(IGeometry geometry)
        {
            IGeometryOperatorFactory factory = geometry.Factory.GetFactory<IGeometryOperatorFactory>();

            if (factory == null)
                return Factory.DefaultInstance<IGeometryOperatorFactory>().Relate;

            return factory.Relate;
        }

        #endregion
    }
}
