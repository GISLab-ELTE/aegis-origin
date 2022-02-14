/// <copyright file="SpectralGeometryFactoryExtensions.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Raster;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Provides extensions to <see cref="IGeometryFactory" /> for producing <see cref="ISpectralGeometry" /> instances.
    /// </summary>
    public static class SpectralGeometryFactoryExtensions
    {
        #region Factory methods for raster geometries

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="raster">The raster.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// or
        /// The raster is null.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, IRaster raster)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, raster);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="raster">The raster.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// or
        /// The raster is null.
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, IRaster raster, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, raster, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging);
        }
        
        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, format, numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, format, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging);
        }
        
        /// <summary>
        /// Creates a raster image for the specified service.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The raster mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// or
        /// The raster service is null.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, IRasterService service, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, service, mapper);
        }

        /// <summary>
        /// Creates a raster image for the specified service.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The raster mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The geometry is null.
        /// or
        /// The raster service is null.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, IGeometry geometry, IRasterService service, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(geometry, service, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="other">The other raster image.</param>
        /// <returns>The produced spectral geometry matching <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        public static ISpectralGeometry CreateSpectralGeometry(this IGeometryFactory factory, ISpectralGeometry other)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralGeometry(other);
        }

        #endregion

        #region Factory methods for spectral polygons

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfBands, numberOfRows, numberOfColumns, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging, metadata);
        }
        
        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(format, numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(format, numberOfBands, numberOfRows, numberOfColumns, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(format, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(format, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging, metadata);
        }
        
        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <returns>The produced spectral polygon.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IRaster raster, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(raster, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, numberOfBands, numberOfRows, numberOfColumns, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging, metadata);
        }
        
        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, format, numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, format, numberOfBands, numberOfRows, numberOfColumns, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, format, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, format, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging, metadata);
        }
        
        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IRaster raster)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, raster);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IRaster raster, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, raster, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IRaster raster, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, raster, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IRaster raster, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, raster, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, numberOfBands, numberOfRows, numberOfColumns, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging, metadata);
        }
        
        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, format, numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, format, numberOfBands, numberOfRows, numberOfColumns, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, format, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, format, numberOfBands, numberOfRows, numberOfColumns, mapper, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper, presentation, imaging, metadata);
        }
        
        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, raster);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, raster, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, RasterPresentation presentation, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, raster, presentation, imaging);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(shell, holes, raster, presentation, imaging, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="others">The other spectral polygons.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">No polygons are specified.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<ISpectralPolygon> others)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(others);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="others">The other spectral polygons.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">No polygons are specified.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IEnumerable<ISpectralPolygon> others, RasterImaging imaging)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(others, imaging);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Ensures an underlying geometry graph factory the specified geometry factory.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <
        private static void EnsureFactory(IGeometryFactory factory)
        {
            if (factory.ContainsFactory<ISpectralGeometryFactory>())
                return;

            factory.EnsureFactory<ISpectralGeometryFactory>(new SpectralGeometryFactory(factory, new RasterFactory()));
        }

        #endregion
    }
}
