/// <copyright file="GeometryCompatibility.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Compatibility
{
    /// <summary>
    /// Provides compatibility methods for <see cref="IGeometry" /> instances.
    /// </summary>
    /// <remarks>
    /// These extensions provides additional methods for <see cref="IGeometry" /> and descendant types 
    /// to enable full compliance with the OGC Simple Feature Access standard.
    /// </remarks>
    public static class GeometryCompatibility
    {
        #region IGeometry

        /// <summary>
        /// Returns the name of the instantiatable subtype of geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The name of the instantiatable subtype of geometry.</returns>
        public static String GeometryType(this IGeometry geometry)
        {
            return geometry.Name;
        }

        /// <summary>
        /// Returns the spatial reference system identifier for the <see cref="IGeometry" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The spatial reference system identifier for the geometry, or <c>null</c> if no reference system is specified.</returns>
        public static String SRID(this IGeometry geometry)
        {
            return (geometry.ReferenceSystem != null) ? geometry.ReferenceSystem.Identifier : null;
        }

        /// <summary>
        /// Returns whether the <see cref="IGeometry" /> has Z coordinate values.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the geometry has Z coordinate values; otherwise, <c>false</c>.</returns>
        public static Boolean Is3D(this IGeometry geometry)
        {
            return geometry.SpatialDimension == 3;
        }

        /// <summary>
        /// Returns whether the <see cref="IGeometry" /> has M coordinate values.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the geometry has M coordinate values; otherwise, <c>false</c>.</returns>
        public static Boolean IsMeasured(this IGeometry geometry)
        {
            return false;
        }

        /// <summary>
        /// Returns a derived geometry collection value that matches the specified m coordinate value.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="mValue">The M coordinate value.</param>
        /// <returns>The derived geometry collection value that matches <paramref name="mValue" />.</returns>
        public static IGeometry LocateAlong(this IGeometry geometry, Double mValue)
        {
            return null;
        }

        /// <summary>
        /// Returns a derived geometry collection value that matches the specified range of m coordinate values inclusively.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="mValue">The M coordinate value.</param>
        /// <returns>The derived geometry collection value that matches the range between <paramref name="mStart" /> and <paramref name="mEnd" />.</returns>
        public static IGeometry LocateBetween(this IGeometry geometry, Double mStart, Double mEnd)
        {
            return null;
        }

        #endregion

        #region IGeometryCollection

        /// <summary>
        /// Returns the number of geometries in the <see cref="IGeometryCollection{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the geometry.</typeparam>
        /// <param name="geometry">The geometry collection.</param>
        /// <returns>The number of geometries in the <see cref="IGeometryCollection{T}" />.</returns>
        public static Int32 NumGeometries<T>(this IGeometryCollection<T> geometry) where T : IGeometry
        {
            return geometry.Count;
        }

        /// <summary>
        /// Returns an <see cref="IGeometry" /> at the specified index.
        /// </summary>
        /// <typeparam name="T">The type of the geometry.</typeparam>
        /// <param name="geometry">The geometry collection.</param>
        /// <param name="index">The zero-based index of the geometry to return.</param>
        /// <returns>The geometry at the specified index.</returns>
        public static T GeometryN<T>(this IGeometryCollection<T> geometry, Int32 index) where T : IGeometry
        {
            return geometry[index];
        }

        #endregion

        #region ILineString

        /// <summary>
        /// Gets the number of points in the <see cref="ILineString" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The number of coordinates in the <see cref="ILineString" />.</returns>
        public static Int32 NumPoints(this ILineString geometry)
        {
            return geometry.Count;
        }

        /// <summary>
        /// Returns a <see cref="IPoint" /> at the specified index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="index">The zero-based index of the point to return.</param>
        /// <returns>The <see cref="Point" /> at the specified index.</returns>
        public static IPoint PointN(this ILineString geometry, Int32 index)
        {
            return geometry.Factory.CreatePoint(geometry.Coordinates[index].X, geometry.Coordinates[index].Y, geometry.Coordinates[index].Z);
        }

        #endregion

        #region IPolygon

        /// <summary>
        /// Returns the exterior ring of the <see cref="IPolygon" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The exterior ring of the <see cref="IPolygon" />.</returns>
        public static ILinearRing ExteriorRing(this IPolygon geometry)
        {
            return geometry.Shell;
        }

        /// <summary>
        /// Returns the number of interior rings in the <see cref="IPolygon" />.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The number of interior rings in the <see cref="IPolygon" />.</returns>
        public static Int32 NumInteriorRing(this IPolygon geometry)
        {
            return geometry.HoleCount;
        }

        /// <summary>
        /// Returns the interior ring at the specified index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="index">The zero-based index of the interior ring to return.</param>
        /// <returns>The interior ring at the specified index.</returns>
        public static ILinearRing InteriorRingN(this IPolygon geometry, Int32 index)
        {
            return geometry.GetHole(index);
        }

        #endregion
    }
}
