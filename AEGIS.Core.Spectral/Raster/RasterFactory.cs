/// <copyright file="RasterFactory.cs" company="Eötvös Loránd University (ELTE)">
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

        #region Factory methods for rasters

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public IRaster CreateRaster(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateRaster(RasterFormat.Any, numberOfBands, numberOfRows, numberOfColumns, mapper);
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
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
        public IRaster CreateRaster(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return CreateRaster(RasterFormat.Any, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
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
        public IRaster CreateRaster(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            return CreateRaster(RasterFormat.Any, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper);
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public IRaster CreateRaster(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper)
        {
            return CreateRaster(format, numberOfBands, numberOfRows, numberOfColumns, format == RasterFormat.Floating ? DefaultFloatRadiometricResolution : DefaultRadiometricResolution, mapper);
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
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
        public IRaster CreateRaster(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            if (numberOfBands < 1)
                throw new ArgumentOutOfRangeException("numberOfBands", "The number of bands is less than 1.");

            if (radiometricResolution < 1)
                throw new ArgumentOutOfRangeException("radiometricResolutions", "The radiometric resolution is less than 1.");
            if (radiometricResolution > 64)
                throw new ArgumentOutOfRangeException("radiometricResolutions", "The radiometric resolution is greater than the maximum allowed.");

            return CreateRaster(format, numberOfBands, numberOfRows, numberOfColumns, Enumerable.Repeat(radiometricResolution, numberOfBands).ToArray(), mapper);
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="format">The format of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
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
        public IRaster CreateRaster(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            if (numberOfBands < 1)
                throw new ArgumentOutOfRangeException("numberOfBands", "The number of bands is less than 1.");
            if (numberOfRows < 0)
                throw new ArgumentOutOfRangeException("numberOfRows", "The number of rows is less than 0.");
            if (numberOfColumns < 0)
                throw new ArgumentOutOfRangeException("numberOfColumns", "The number of columns is less than 0.");
            if (radiometricResolutions != null && numberOfBands != radiometricResolutions.Count)
                throw new ArgumentException("The number of radiometric resolutions does not match the number of bands.", "radiometricResolutions");

            if (radiometricResolutions != null && radiometricResolutions.Min() < 1)
                throw new ArgumentOutOfRangeException("radiometricResolutions", "One or more radiometric resolutions are less than 1.");
            if (radiometricResolutions != null && radiometricResolutions.Max() > 64)
                throw new ArgumentOutOfRangeException("radiometricResolutions", "One or more radiometric resolutions are greater than the maximum allowed.");

           Int32 maxRadiometricResolution;

            switch (format)
            {
                case RasterFormat.Any:
                case RasterFormat.Integer:
                    maxRadiometricResolution = (radiometricResolutions != null) ? radiometricResolutions.Max() : DefaultRadiometricResolution;

                    if (maxRadiometricResolution <= 8)
                        return new Raster8(this, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper);

                    if (maxRadiometricResolution <= 16)
                        return new Raster16(this, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper);

                    if (maxRadiometricResolution <= 32)
                        return new Raster32(this, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper);

                    return new RasterFloat64(this, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper);

                case RasterFormat.Floating:
                     maxRadiometricResolution = (radiometricResolutions != null) ? radiometricResolutions.Max() : DefaultFloatRadiometricResolution;

                    if (maxRadiometricResolution <= 32)
                        return new RasterFloat32(this, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper);

                    return new RasterFloat64(this, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper);
            }

            return null;
        }

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="other">The other raster image.</param>
        /// <returns>The produced raster image matching <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">The other raster is null.</exception>
        public IRaster CreateRaster(IRaster other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other raster is null.");

            IRaster raster = CreateRaster(other.Format, other.NumberOfBands, other.NumberOfRows, other.NumberOfColumns, other.RadiometricResolutions, other.Mapper);

            switch (raster.Format)
            {
                case RasterFormat.Integer:
                    for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                        for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                            for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                            {
                                raster.SetValue(rowIndex, columnIndex, bandIndex, raster.GetValue(rowIndex, columnIndex, bandIndex));
                            }
                    break;
                case RasterFormat.Floating:
                    for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                        for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                            for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                            {
                                raster.SetFloatValue(rowIndex, columnIndex, bandIndex, raster.GetFloatValue(rowIndex, columnIndex, bandIndex));
                            }
                    break;
            }

            return raster;
        }

        #endregion

        #region Factory methods for raster services

        /// <summary>
        /// Creates a raster image for the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The raster mapper.</param>
        /// <returns>The raster mapper created for the specified service.</returns>
        public IRaster CreateRaster(IRasterService service, RasterMapper mapper)
        {
            return new ProxyRaster(this, service, mapper);
        }

        #endregion

        #region Factory methods for raster masks

        /// <summary>
        /// Creates a mask on a raster.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <returns>The raster masked by the specified region.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting row index is less than 0.
        /// or
        /// The starting row index is equal to or greater than the number of rows in the source raster.
        /// or
        /// The starting column index is less than 0.
        /// or
        /// The starting column index is equal to or greater than the number of columns in the source raster.
        /// or
        /// The number of rows is less than 0.
        /// or  
        /// The starting row index and the number of rows is greater than the number of rows in the source raster.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The starting columns index and the number of columns is greater than the number of rows in the source raster.
        /// </exception>
        public IRaster CreateMask(IRaster raster, Int32 rowIndex, Int32 columnIndex, Int32 numberOfRows, Int32 numberOfColumns)
        {
            return new MaskedRaster(this, raster, rowIndex, columnIndex, numberOfRows, numberOfColumns);
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
