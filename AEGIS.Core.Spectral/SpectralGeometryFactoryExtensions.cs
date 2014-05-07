/// <copyright file="SpectralGeometryFactoryExtensions.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Provides extensions to <see cref="IGeometryFactory" /> for producing <see cref="ISpectralGeometry" /> instances.
    /// </summary>
    public static class SpectralGeometryFactoryExtensions
    {
        #region Factory methods for spectral geometries

        /// <summary>
        /// Creates a spectral geometry to match a specified geometry.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="source">The source.</param>
        /// <returns>The spectral geometry that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The raster is null.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IRaster raster, IGeometry source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(raster, source);
        }

        #endregion

        #region Factory methods for spectral polygons

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, shell);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, shell);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, shell, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The raster is null.
        /// or
        /// The source polygon is null.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, source);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A spectral polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other spectral polygon is null.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, ISpectralPolygon other)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(other);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Ensures an internal spectral geometry factory the specified geometry factory.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        private static void EnsureFactory(IGeometryFactory factory)
        {
            // query whether the spectral geometry factory is registerd for the geometry factory
            if (!factory.ContainsFactoryFor<ISpectralGeometry>() || !(factory.GetFactoryFor<ISpectralGeometry>() is ISpectralGeometryFactory))
            {
                if (Factory.HasDefaultInstance<ISpectralGeometryFactory>()) // if it has been registered previously
                {
                    factory.SetFactoryFor<ISpectralGeometry>(Factory.GetInstance<ISpectralGeometryFactory>(factory));
                }
                else // if not the default implemenentation is registered
                {
                    factory.SetFactoryFor<ISpectralGeometry>(Factory.GetInstance<Geometry.SpectralGeometryFactory>(factory));
                }
            }
        }

        #endregion
    }
}
