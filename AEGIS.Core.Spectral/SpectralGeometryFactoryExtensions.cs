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
    /// Provides extensions to <see cref="IGeometryFactory"/> for producing <see cref="ISpectralGeometry"/> instances.
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, RasterRepresentation representation)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, representation);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, representation);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, representation);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation
)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, representation);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, representation, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, representation, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, representation, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows,numberOfColumns, mapper, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows,numberOfColumns,mapper,representation, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows,numberOfColumns, radiometricResolution, spectralRanges, mapper, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, shell);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, representation,shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, shell);
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
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation, shell);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, representation, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, representation, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, representation, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, representation, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation, shell, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, representation, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, representation, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, representation, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, representation, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, shell, holes, metadata);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation, shell, holes, metadata);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, source);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(mapper, representation, source);
        }

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralRanges, mapper, representation, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(numberOfRows, numberOfColumns, mapper, representation, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, mapper, representation, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, representation, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, source);
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
        public static ISpectralPolygon CreateSpectralPolygon(this IGeometryFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source)
        {
            EnsureFactory(factory);

            return factory.GetFactory<ISpectralGeometryFactory>().CreateSpectralPolygon(spectralResolution, numberOfRows, numberOfColumns, radiometricResolution, spectralRanges, mapper, representation, source);
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
