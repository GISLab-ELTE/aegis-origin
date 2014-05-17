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
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation);


        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper);


        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation);


        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata);


        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata);


        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata);


        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A spectral polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IEnumerable<Coordinate> shell);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell);


        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A spectral polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell);


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
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell);


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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);


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
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);


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
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);


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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the metadata.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);


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
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);


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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the metadata.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);
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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);
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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);
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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);


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
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);


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
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);


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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);
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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);
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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, IPolygon source);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(RasterMapper mapper, RasterRepresentation representation, IPolygon source);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source);

        /// <summary>
        /// Creates a spectral polygon.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">The source polygon is null.</exception>
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IPolygon source);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IPolygon source);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, IPolygon source);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation, IPolygon source);


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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, IPolygon source);

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
        ISpectralPolygon CreateSpectralPolygon(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation, IPolygon source);

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
