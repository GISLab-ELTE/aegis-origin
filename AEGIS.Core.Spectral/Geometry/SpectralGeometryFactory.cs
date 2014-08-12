/// <copyright file="SpectralGeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Raster;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a factory for producing <see cref="ISpectralGeometry" /> instances.
    /// </summary>
    /// <remarks>This implementation of <see cref="ISpectralGeometryFactory" /> produces spectral geometries in coordinate space.</remarks>
    public class SpectralGeometryFactory : Factory, ISpectralGeometryFactory
    {
        #region Private fields

        /// <summary>
        /// The geometry factory. This field is read-only.
        /// </summary>
        private readonly IGeometryFactory _geometryFactory;

        /// <summary>
        /// The raster factory. This field is read-only.
        /// </summary>
        private readonly IRasterFactory _rasterFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralGeometryFactory"/> class.
        /// </summary>
        /// <param name="geometryFactory">The geometry factory.</param>
        /// <exception cref="System.ArgumentNullException">The geometry factory is null.</exception>
        public SpectralGeometryFactory(IGeometryFactory geometryFactory, IRasterFactory rasterFactory) 
            : base(geometryFactory ?? Factory.DefaultInstance<IGeometryFactory>(), rasterFactory ?? Factory.DefaultInstance<RasterFactory>())
        {
            _geometryFactory = geometryFactory ?? Factory.DefaultInstance<IGeometryFactory>();
            _rasterFactory = rasterFactory ?? Factory.DefaultInstance<IRasterFactory>();
        }

        #endregion

        #region Factory methods for spectral geometries

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), null);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), scene);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), null);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), scene);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene);
        }

        /// <summary>
        /// Creates a raster image for the specified service.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The raster mapper.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The raster service is null.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, IRasterService service, RasterMapper mapper)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(service, mapper), null);
        }

        /// <summary>
        /// Creates a raster image for the specified service.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The raster mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The raster service is null.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, IRasterService service, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralGeometry(geometry, _rasterFactory.CreateRaster(service, mapper), scene);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="raster">The raster.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The raster is null.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, IRaster raster)
        {
            return CreateSpectralGeometry(geometry, raster, null);
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="raster">The raster.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The raster is null.
        /// </exception>
        public ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, IRaster raster, ImagingScene scene)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");

            if (geometry is IPolygon)
            {
                return new SpectralPolygon(this, (geometry as IPolygon).Shell, (geometry as IPolygon).Holes, raster, scene, geometry.Metadata);
            }

            return null;
        }

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="other">The other raster image.</param>
        /// <returns>The produced spectral geometry matching <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        public ISpectralGeometry CreateSpectralGeometry(ISpectralGeometry other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other geometry is null.");

            if (other is IPolygon)
            {
                return new SpectralPolygon(this, (other as IPolygon).Shell, (other as IPolygon).Holes, other.Raster, other.ImagingScene, other.Metadata);
            }

            return null;
        }

        #endregion

        #region Factory methods for spectral polygons

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), null,  null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public ISpectralPolygon CreateSpectralPolygon(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(_rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IRaster raster)
        {
            return CreateSpectralPolygon(raster, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IRaster raster, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(raster, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IRaster raster, ImagingScene scene)
        {
            return CreateSpectralPolygon(raster, scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IRaster raster, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(raster.Coordinates, null, raster, scene, metadata);
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), null, null);
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), scene, metadata);
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, null);
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, metadata);
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
        /// <param name="scene">The imaging scene data.</param>
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, null);
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
        /// <param name="scene">The imaging scene data.</param>
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, metadata);
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), null, null);
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), scene, metadata);
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, null);
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, metadata);
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
        /// <param name="scene">The imaging scene data.</param>
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, null);
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
        /// <param name="scene">The imaging scene data.</param>
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IRaster raster)
        {
            return CreateSpectralPolygon(shell, null, raster, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IRaster raster, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, raster, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IRaster raster, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, null, raster, scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IRaster raster, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, null, raster, scene, metadata);
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), null, null);
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), null, metadata);
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
        /// <param name="scene">The imaging scene data.</param>
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), scene, null);
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
        /// <param name="scene">The imaging scene data.</param>
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, mapper), scene, metadata);
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, null);
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, metadata);
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
        /// <param name="scene">The imaging scene data.</param>
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, null);
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
        /// <param name="scene">The imaging scene data.</param>
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, metadata);
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), null, null);
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), null, metadata);
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
        /// <param name="scene">The imaging scene data.</param>
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), scene, null);
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
        /// <param name="scene">The imaging scene data.</param>
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
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, mapper), scene, metadata);
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, null);
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), null, metadata);
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
        /// <param name="scene">The imaging scene data.</param>
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, null);
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
        /// <param name="scene">The imaging scene data.</param>
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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The number of radiometric resolutions does not match the number of bands.
        /// </exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, _rasterFactory.CreateRaster(representation, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper), scene, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster)
        {
            return CreateSpectralPolygon(shell, holes, raster, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, IDictionary<String, Object> metadata)
        {
            return CreateSpectralPolygon(shell, holes, raster, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, ImagingScene scene)
        {
            return CreateSpectralPolygon(shell, holes, raster, scene, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            return new SpectralPolygon(this, shell, holes, raster, scene, metadata);
        }

        #endregion





        #region Protected Factory methods

        /// <summary>
        /// Gets the product type of the factory.
        /// </summary>
        /// <returns>The product type of the factory.</returns>
        protected override Type GetProductType()
        {
            return typeof(ISpectralGeometry);
        }

        #endregion
    }
}
