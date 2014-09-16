/// <copyright file="ISpectralGeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for factories producing <see cref="ISpectralGeometry" /> instances.
    /// </summary>
    public interface ISpectralGeometryFactory : IFactory
    {
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);
       
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, IRasterService service, RasterMapper mapper);

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
        /// The geometry is null.
        /// or
        /// The raster service is null.
        /// </exception>
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, IRasterService service, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, IRaster raster);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="raster">The raster.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The raster is null.
        /// </exception>
        ISpectralGeometry CreateSpectralGeometry(IGeometry geometry, IRaster raster, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="other">The other raster image.</param>
        /// <returns>The produced spectral geometry matching <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        ISpectralGeometry CreateSpectralGeometry(ISpectralGeometry other);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(IRaster raster);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(IRaster raster, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IRaster raster);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IRaster raster, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IRaster raster, RasterPresentation presentation, RasterImaging imaging);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IRaster raster, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging);

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
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster);

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
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster image.</param>
        /// <param name="presentation">The raster presentation data.</param>
        /// <param name="imaging">The raster imaging data.</param>
        /// <returns>The produced spectral polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, RasterPresentation presentation, RasterImaging imaging);

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
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, RasterPresentation presentation, RasterImaging imaging, IDictionary<String, Object> metadata);

        #endregion
    }
}
