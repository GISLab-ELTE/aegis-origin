/// <copyright file="SpotTopOfAtmosphereReflectanceComputation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
/// <author>Tamás Kovács</author>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Reflectance
{
    /// <summary>
    /// Represents an operation computing the Top of Atmosphere (ToA) reflectance of raster geometries for SPOT images.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213461", "Top of atmosphere reflectance computation")]
    public class SpotTopOfAtmosphereReflectanceComputation : TopOfAtmosphereReflectanceComputation
    {
        #region Private fields

        private readonly Int32 _indexOfGreenBand;
        private readonly Int32 _indexOfRedBand;
        private readonly Int32 _indexOfNearInfraredBand;
        private readonly Int32 _indexOfShortWavelengthInfraredBand;
        private Double[] _esunPhysicalGainRatio;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotTopOfAtmosphereReflectanceComputation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// </exception>
        public SpotTopOfAtmosphereReflectanceComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SpotTopOfAtmosphereReflectanceComputation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="result">The result.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// or
        /// The source does not contain required data.
        /// or
        /// The number of bands in the source is less than required.
        /// or
        /// The source contains invalid data.
        /// </exception>
        public SpotTopOfAtmosphereReflectanceComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, parameters)
        {
            if (Source.Imaging == null)
                throw new ArgumentException("The source does not contain required data.", "source");
            if (Source.Imaging.Device.Mission != "SPOT")
                throw new ArgumentException("The source does not contain required data.", "source");
            if (Source.Raster.NumberOfBands < 4)
                throw new ArgumentException("The number of bands in the source is less than required.", "source");

            if (IsProvidedParameter(SpectralOperationParameters.IndexOfGreenBand)) // the parameters are provided
            {
                _indexOfGreenBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfGreenBand));

                if (_indexOfGreenBand < 0 || _indexOfGreenBand >= Source.Raster.NumberOfBands)
                    throw new ArgumentException("The value of a parameter is not within the expected range.", "parameters");
            }
            else // the imaging metadata is provided
            {
                RasterImagingBand imagingBand = Source.Imaging.Bands.FirstOrDefault(band => band.SpectralDomain == SpectralDomain.Green);
                if (imagingBand != null)
                {
                    _indexOfGreenBand = Source.Imaging.Bands.IndexOf(imagingBand);

                    if (_indexOfGreenBand < 0 || _indexOfGreenBand >= Source.Raster.NumberOfBands)
                        throw new ArgumentException("The source contains invalid data.", "source");
                }
                else // nothing is provided, default values are used
                    _indexOfGreenBand = 0;
            }

           

            if (IsProvidedParameter(SpectralOperationParameters.IndexOfRedBand))
            {
                _indexOfRedBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfRedBand));

                if (_indexOfRedBand < 0 || _indexOfRedBand >= Source.Raster.NumberOfBands)
                    throw new ArgumentException("The value of a parameter is not within the expected range.", "parameters");
            }
            else
            {
                RasterImagingBand imagingBand = Source.Imaging.Bands.FirstOrDefault(band => band.SpectralDomain == SpectralDomain.Red);
                if (imagingBand != null)
                {
                    _indexOfRedBand = Source.Imaging.Bands.IndexOf(imagingBand);

                    if (_indexOfRedBand < 0 || _indexOfRedBand >= Source.Raster.NumberOfBands)
                        throw new ArgumentException("The source contains invalid data.", "source");
                }
                else
                    _indexOfRedBand = 1;
            }
           
            if (IsProvidedParameter(SpectralOperationParameters.IndexOfNearInfraredBand))
            {
                _indexOfNearInfraredBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfNearInfraredBand));

                if (_indexOfNearInfraredBand < 0 || _indexOfNearInfraredBand >= Source.Raster.NumberOfBands)
                    throw new ArgumentException("The value of a parameter is not within the expected range.", "parameters");
            }
            else
            {
                RasterImagingBand imagingBand = Source.Imaging.Bands.FirstOrDefault(band => band.SpectralDomain == SpectralDomain.NearInfrared);
                if (imagingBand != null)
                {
                    _indexOfNearInfraredBand = Source.Imaging.Bands.IndexOf(imagingBand);

                    if (_indexOfNearInfraredBand < 0 || _indexOfNearInfraredBand >= Source.Raster.NumberOfBands)
                        throw new ArgumentException("The source contains invalid data.", "source");
                }
                else
                    _indexOfNearInfraredBand = 2;
            }
            
            if (IsProvidedParameter(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand))
            {
                _indexOfShortWavelengthInfraredBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));

                if (_indexOfShortWavelengthInfraredBand < 0 || _indexOfShortWavelengthInfraredBand >= Source.Raster.NumberOfBands)
                    throw new ArgumentException("The value of a parameter is not within the expected range.", "parameters");
            }
            else if (Source.Imaging != null)
            {
                RasterImagingBand imagingBand = Source.Imaging.Bands.FirstOrDefault(band => band.SpectralDomain == SpectralDomain.ShortWavelengthInfrared);
                if (imagingBand != null)
                {
                    _indexOfShortWavelengthInfraredBand = Source.Imaging.Bands.IndexOf(imagingBand);

                    if (_indexOfShortWavelengthInfraredBand < 0 || _indexOfShortWavelengthInfraredBand >= Source.Raster.NumberOfBands)
                        throw new ArgumentException("The source contains invalid data.", "source");
                }
                else
                    _indexOfNearInfraredBand = 3;
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _result = Source.Factory.CreateSpectralGeometry(Source,
                                                            PrepareRasterResult(RasterFormat.Floating,
                                                                                4,
                                                                                Source.Raster.NumberOfRows,
                                                                                Source.Raster.NumberOfColumns,
                                                                                32,
                                                                                Source.Raster.Mapper),
                                                            Source.Presentation,
                                                            Source.Imaging);

            Double sunElevation = 90 - Source.Imaging.SunElevation, eSun;
            Int32 dayOfYear = Source.Imaging.Time.DayOfYear;

            _esunPhysicalGainRatio = new Double[4];

            for (Int32 bandIndex = 0; bandIndex < 4; bandIndex++)
            {
                eSun = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (dayOfYear - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (dayOfYear - 4) * Math.PI / 180))) / (Source.Imaging.Bands[bandIndex].SolarIrradiance * Math.Cos(sunElevation * Constants.PI / 180)));
                _esunPhysicalGainRatio[bandIndex] = eSun * 100 / Source.Imaging.Bands[bandIndex].PhysicalGain;
            }
        }

        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            switch (bandIndex)
            {
                case 0:
                    return Source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand) * _esunPhysicalGainRatio[_indexOfGreenBand];
                case 1:
                    return Source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand) * _esunPhysicalGainRatio[_indexOfRedBand];
                case 2:
                    return Source.Raster.GetValue(rowIndex, columnIndex, _indexOfGreenBand) * _esunPhysicalGainRatio[_indexOfNearInfraredBand];
                case 3:
                    return Source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand) * _esunPhysicalGainRatio[_indexOfShortWavelengthInfraredBand];
            }

            return 0;
        }

        /// <summary>
        /// Computes the specified floating spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            return new Double[]
            {
                Source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand) * _esunPhysicalGainRatio[_indexOfGreenBand],
                Source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand) * _esunPhysicalGainRatio[_indexOfRedBand],
                Source.Raster.GetValue(rowIndex, columnIndex, _indexOfGreenBand) * _esunPhysicalGainRatio[_indexOfNearInfraredBand],
                Source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand) * _esunPhysicalGainRatio[_indexOfShortWavelengthInfraredBand]
            };
        }

        #endregion
    }
}
