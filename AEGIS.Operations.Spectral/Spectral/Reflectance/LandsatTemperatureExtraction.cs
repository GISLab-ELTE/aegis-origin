/// <copyright file="LandsatTemperatureComputation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Robeto Giachetta. Licensed under the
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
/// <author>Gréta Bereczki</author>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Reflectance
{
    /// <summary>
    /// Represents an operation computing the surface temperature (in Kelvin) for Landsat images.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213467", "Temperature extraction")]
    public class LandsatTemperatureExtraction : SpectralTransformation
    {
        #region Constants

        #endregion

        #region Private fields

        /// <summary>
        /// The index of the thermal band.
        /// </summary>
        private readonly Int32 _indexOfThermalBand;

        /// <summary>
        /// The array of calibration constant 1 values.
        /// </summary>
        private readonly Double _calibrationConstant1;

        /// <summary>
        /// The array of calibration constant 2 values.
        /// </summary>
        private readonly Double _calibrationConstant2;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LandsatTemperatureExtraction"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentException">
        /// The source does not contain required data.
        /// or
        /// The source does not contain required data.
        /// or
        /// The number of bands in the source is less than required.
        /// </exception>
        public LandsatTemperatureExtraction(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandsatTemperatureExtraction"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="result">The result.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentException">
        /// The source does not contain required data.
        /// or
        /// The source does not contain required data.
        /// or
        /// The number of bands in the source is less than required.
        /// </exception>
        public LandsatTemperatureExtraction(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.TemperatureExtraction, parameters)
        {
            if (Source.Imaging == null)
                throw new ArgumentException("The source does not contain required data.", "source");
            if (Source.Imaging.Device.Mission != "Landsat" || (Source.Imaging.Device.MissionNumber != 7 && Source.Imaging.Device.MissionNumber != 8))
                throw new ArgumentException("The source does not contain required data.", "source");

            if (IsProvidedParameter(SpectralOperationParameters.IndexOfLongWavelengthInfraredBand))
            {
                _indexOfThermalBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfLongWavelengthInfraredBand));
            }
            else
            {
                RasterImagingBand thermalBand = Source.Imaging.Bands.FirstOrDefault(band => band.SpectralDomain == SpectralDomain.LongWavelengthInfrared);
                _indexOfThermalBand = Source.Imaging.Bands.IndexOf(thermalBand);
            }

            if (Source.Raster.NumberOfBands <= _indexOfThermalBand)
                throw new ArgumentException("The number of bands in the source is less than required.", "source");

            switch (Source.Imaging.Device.MissionNumber)
            {
                case 5:
                    _calibrationConstant1 = 607.76;
                    _calibrationConstant2 = 1260.56;
                    break;

                case 7:
                    _calibrationConstant1 = 666.09;
                    _calibrationConstant2 = 1282.71;
                    break;

                case 8:
                    _calibrationConstant1 = Convert.ToDouble(Source.Imaging["K1_CONSTANT_BAND_11"], CultureInfo.InvariantCulture);
                    _calibrationConstant2 = Convert.ToDouble(Source.Imaging["K2_CONSTANT_BAND_11"], CultureInfo.InvariantCulture);
                    break;
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override sealed void PrepareResult()
        {
            _result = _source.Factory.CreateSpectralGeometry(_source,
                                                             PrepareRasterResult(RasterFormat.Floating,
                                                                                 1,
                                                                                 _source.Raster.NumberOfRows,
                                                                                 _source.Raster.NumberOfColumns,
                                                                                 new Int32[] { 32 },
                                                                                 _source.Raster.Mapper),
                                                            _source.Presentation,
                                                             _source.Imaging);
        }

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>
        /// The spectral value at the specified index.
        /// </returns>
        /// <exception cref="System.ArgumentException">The band index is not suitable.</exception>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Double solarRadiance = Source.Imaging.Bands[_indexOfThermalBand].PhysicalGain * Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfThermalBand) + Source.Imaging.Bands[_indexOfThermalBand].PhysicalBias;

            return _calibrationConstant2 / Math.Log(_calibrationConstant1 / solarRadiance + 1);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>
        /// The array containing the spectral values for each band at the specified index.
        /// </returns>
        protected override Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            return new Double[] { ComputeFloat(rowIndex, columnIndex, 0) };
        }

        #endregion
    }
}
