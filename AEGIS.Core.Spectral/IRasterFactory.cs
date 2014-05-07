/// <copyright file="IRasterFactory.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines behavior for factories producing <see cref="IRaster" /> instances.
    /// </summary>
    public interface IRasterFactory : IFactory
    {
        #region Factory methods for floating rasters

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IFloatRaster CreateFloatRaster(RasterMapper mapper);

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IFloatRaster CreateFloatRaster(IList<SpectralRange> spectralRanges, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        IFloatRaster CreateFloatRaster(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IFloatRaster CreateFloatRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The spectral resolution is less than 0.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of spectral ranges does not match the spectral resolution.
        /// </exception>
        IFloatRaster CreateFloatRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The spectral resolution is less than 0.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more of the radiometric resolutions is less than 1.
        /// or
        /// One or more of the radiometric resolutions is greater than 64.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of radiometric resolutions does not match the spectral resolution.
        /// or
        /// The number of spectral ranges does not match the spectral resolution.
        /// </exception>
        IFloatRaster CreateFloatRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper);

        #endregion

        #region Factory methods for all rasters

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IRaster CreateRaster(RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IRaster CreateRaster(RasterMapper mapper, RasterRepresentation representation);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IRaster CreateRaster(IList<SpectralRange> spectralRanges, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IRaster CreateRaster(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        IRaster CreateRaster(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        IRaster CreateRaster(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The spectral resolution is less than 0.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The spectral resolution is less than 0.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of spectral ranges does not match the spectral resolution.
        /// </exception>
        IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The spectral resolution is less than 0.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more of the radiometric resolutions is less than 1.
        /// or
        /// One or more of the radiometric resolutions is greater than 64.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of radiometric resolutions does not match the spectral resolution.
        /// or
        /// The number of spectral ranges does not match the spectral resolution.
        /// </exception>
        IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">Thre representation of the raster.</param>
        /// <returns>The raster image produced by the factory.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The spectral resolution is less than 0.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more of the radiometric resolutions is less than 1.
        /// or
        /// One or more of the radiometric resolutions is greater than 64.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of radiometric resolutions does not match the spectral resolution.
        /// or
        /// The number of spectral ranges does not match the spectral resolution.
        /// </exception>
        IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation);

        #endregion
    }
}
