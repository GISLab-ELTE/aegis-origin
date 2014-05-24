/// <copyright file="SpectralGeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Raster;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a factory for producing <see cref="ISpectralGeometry" /> instances.
    /// </summary>
    /// <remarks>This implementation of <see cref="ISpectralGeometryFactory" /> produces spectral geometries in coordinate space.</remarks>
    public class SpectralGeometryFactory : Factory, ISpectralGeometryFactory
    {
        #region Private fields

        private IGeometryFactory _geometryFactory;
        private IRasterFactory _rasterFactory;

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

        #region Factory methods for raster geometries

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
        public virtual ISpectralGeometry CreateSpectralGeometry(IRaster raster, IGeometry source)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            if (source is IPolygon)
            {
                IPolygon sourcePolygon = source as IPolygon;
                return new SpectralPolygon(this, raster, sourcePolygon.Shell, sourcePolygon.Holes, sourcePolygon.Metadata);
            }

            return null;
        }

        #endregion

        #region Factory methods for spectral polygons

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IRaster raster)
        {
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The raster mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }


        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IRaster raster, IDictionary<String, Object> metadata)
        {
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, raster.Coordinates, null, metadata);
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
        public virtual ISpectralPolygon CreateSpectralPolygon(IRaster raster, IEnumerable<Coordinate> shell)
        {
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, null, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, null);
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
        public virtual ISpectralPolygon CreateSpectralPolygon(IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns,radiometricResolutions, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, holes, null);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, null);
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
        public virtual ISpectralPolygon CreateSpectralPolygon(IRaster raster, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, null, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, null, metadata);
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
        public virtual ISpectralPolygon CreateSpectralPolygon(IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(mapper);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions,spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, shell, holes, metadata);
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
        public virtual ISpectralPolygon CreateSpectralPolygon(IRaster raster, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(mapper);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(mapper, representation);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, mapper, representation);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            IRaster raster = _rasterFactory.CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation);
            return new SpectralPolygon(this, raster, source.Shell, source.Holes, source.Metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A spectral polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other spectral polygon is null.</exception>
        public virtual ISpectralPolygon CreateSpectralPolygon(ISpectralPolygon other)
        {
            if (other == null)
                throw new ArgumentNullException("source", "The source is null.");

            return new SpectralPolygon(this, other.Raster.Clone() as IRaster, other.Shell, other.Holes, other.Metadata);
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
