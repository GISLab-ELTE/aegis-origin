/// <copyright file="ISpectralGeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines behavior for factories producing <see cref="ISpectralGeometry" /> instances.
    /// </summary>
    public interface ISpectralGeometryFactory : IFactory
    {
        #region Factory methods for raster geometries

        /// <summary>
        /// Creates a spectral geometry to match a specified geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="raster">The raster.</param>
        /// <returns>The spectral geometry that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The raster is null.
        /// </exception>
        ISpectralGeometry CreateSpectralGeometry(IRaster raster, IGeometry source);

        #endregion

        #region Factory methods for spectral polygons

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(IRaster raster);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, IEnumerable<Coordinate> shell);

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
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, IPolygon source);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A spectral polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other spectral polygon is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(ISpectralPolygon other);

        #endregion
    }
}
