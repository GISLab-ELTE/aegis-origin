/// <copyright file="RasterFactory.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Raster
{
    /// <summary>
    /// Represents a factory for producing in-memory <see cref="IRaster" /> instances.
    /// </summary>
    public class RasterFactory : Factory, IRasterFactory
    {
        #region Private constant fields

        /// <summary>
        /// The default radiometric resolution used for integer rasters. This field is constant. 
        /// </summary>
        private const Int32 DefaultRadiometricResolution = 16;

        /// <summary>
        /// The default radiometric resolution used for float rasters. This field is constant. 
        /// </summary>
        private const Int32 DefaultFloatRadiometricResolution = 32;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterFactory" /> class.
        /// </summary>
        public RasterFactory() { }

        #endregion

        #region Factory methods for floating rasters

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        public IFloatRaster CreateFloatRaster(RasterMapper mapper)
        {
            return CreateFloatRaster(0, 0, 0, null, null, mapper);
        }

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        public IFloatRaster CreateFloatRaster(IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            return CreateFloatRaster(0, 0, 0, null, spectralRanges, mapper);
        }

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
        public IFloatRaster CreateFloatRaster(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateFloatRaster(0, numberOfRows, numberOfColumns, null, null, mapper);
        }

        /// <summary>
        /// Creates a raster image containing floating point values.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        public IFloatRaster CreateFloatRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateFloatRaster(spectralResolution, numberOfRows, numberOfColumns, null, null, mapper);
        }

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
        public IFloatRaster CreateFloatRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            if (radiometricResolution < 1)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is less than 1.");
            if (radiometricResolution > 64)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is greater than 64.");

            return CreateFloatRaster(spectralResolution, numberOfRows, numberOfColumns, Enumerable.Repeat(radiometricResolution, spectralResolution).ToArray(), spectralRanges, mapper);
        }

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
        public virtual IFloatRaster CreateFloatRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            if (radiometricResolutions != null && radiometricResolutions.Min() < 1)
                throw new ArgumentOutOfRangeException("radiometricResolutions", "One or more of the radiometric resolutions is less than 1.");
            if (radiometricResolutions != null && radiometricResolutions.Max() > 64)
                throw new ArgumentOutOfRangeException("radiometricResolutions", "One or more of the radiometric resolutions is greater than 64.");

            Int32 baseRadiometricResolution = (radiometricResolutions != null) ? radiometricResolutions.Max() : DefaultRadiometricResolution;

            if (baseRadiometricResolution <= 32)
                return new FloatRaster32(this, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);

            return new FloatRaster64(this, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);
        }

        #endregion

        #region Factory methods for all rasters

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        public IRaster CreateRaster(RasterMapper mapper)
        {
            return CreateRaster(0, 0, 0, null, null, mapper, RasterRepresentation.Integer);
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>The raster image produced by the factory.</returns>
        public IRaster CreateRaster(RasterMapper mapper, RasterRepresentation representation)
        {
            return CreateRaster(0, 0, 0, null, null, mapper, representation);  
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        public IRaster CreateRaster(IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            return CreateRaster(0, 0, 0, null, spectralRanges, mapper, RasterRepresentation.Integer);
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="representation">The representation of the raster.</param>
        /// <returns>The raster image produced by the factory.</returns>
        public IRaster CreateRaster(IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            return CreateRaster(0, 0, 0, null, spectralRanges, mapper, representation);
        }

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
        public IRaster CreateRaster(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateRaster(0, numberOfRows, numberOfColumns, null, null, mapper, RasterRepresentation.Integer);  
        }
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
        public IRaster CreateRaster(Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation)
        {
            return CreateRaster(0, numberOfRows, numberOfColumns, null, null, mapper, representation);
        }
        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The raster image produced by the factory.</returns>
        public IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateRaster(spectralResolution, numberOfRows, numberOfColumns, null, null, mapper, RasterRepresentation.Integer);
        }
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
        public IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper, RasterRepresentation representation)
        {
            return CreateRaster(spectralResolution, numberOfRows, numberOfColumns, null, null, mapper, representation);
        }
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
        public IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            return CreateRaster(spectralResolution, numberOfRows, numberOfColumns, Enumerable.Repeat(radiometricResolution, spectralResolution).ToArray(), spectralRanges, mapper, RasterRepresentation.Integer);
        }
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
        public IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            if (radiometricResolution < 1)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is less than 1.");
            if (radiometricResolution > 64)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is greater than 32.");

            return CreateRaster(spectralResolution, numberOfRows, numberOfColumns, Enumerable.Repeat(radiometricResolution, spectralResolution).ToArray(), spectralRanges, mapper, representation);
        }
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
        public IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            return CreateRaster(spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper, RasterRepresentation.Integer);
        }
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
        public virtual IRaster CreateRaster(Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper, RasterRepresentation representation)
        {
            if (radiometricResolutions != null && radiometricResolutions.Min() < 1)
                throw new ArgumentOutOfRangeException("radiometricResolutions", "One or more of the radiometric resolutions is less than 1.");
            if (radiometricResolutions != null && radiometricResolutions.Max() > 64)
                throw new ArgumentOutOfRangeException("radiometricResolutions", "One or more of the radiometric resolutions is greater than 64.");

            Int32 baseRadiometricResolution = (radiometricResolutions != null) ? radiometricResolutions.Max() : DefaultRadiometricResolution;

            switch (representation)
            {
                case RasterRepresentation.Integer:
                    if (baseRadiometricResolution <= 8)
                        return new Raster8(this, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);

                    if (baseRadiometricResolution <= 16)
                        return new Raster16(this, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);

                    if (baseRadiometricResolution <= 32)
                        return new Raster32(this, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);

                    return new FloatRaster64(this, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);

                case RasterRepresentation.Floating:
                    if (baseRadiometricResolution <= 32)
                        return new FloatRaster32(this, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);

                    return new FloatRaster64(this, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper);
            }
            return null;
        }

        #endregion 
    
        #region Protected Factory methods

        /// <summary>
        /// Gets the product type of the factory.
        /// </summary>
        /// <returns>
        /// The product type of the factory.
        /// </returns>
        protected override Type GetProductType()
        {
            return typeof(IRaster);
        }

        #endregion
    }
}
