/// <copyright file="LandsatTopOfAtmosphereReflectanceComputation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
    /// Represents an operation computing the Top of Atmosphere (ToA) reflectance of raster geometries for Landsat TM7 images.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213461", "Top of atmosphere reflectance computation")]
    public class LandsatTopOfAtmosphereReflectanceComputation : TopOfAtmosphereReflectanceComputation
    {
        #region Private fields

        /// <summary>
        /// The ToA reflectance gain.
        /// </summary>
        private Double[] _toarefGain;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LandsatTopOfAtmosphereReflectanceComputation" /> class.
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
        /// or
        /// The source does not contain required data.
        /// or
        /// The source contains invalid data.
        /// </exception>
        public LandsatTopOfAtmosphereReflectanceComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LandsatTopOfAtmosphereReflectanceComputation" /> class.
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
        /// The source contains invalid data.
        /// </exception>
        public LandsatTopOfAtmosphereReflectanceComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, parameters)
        {
            if (Source.Imaging == null)
                throw new ArgumentException("The source does not contain required data.", "source");
            if (Source.Imaging.Device.Mission != "Landsat")
                throw new ArgumentException("The source does not contain required data.", "source");
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
                                                                                Source.Raster.NumberOfBands,
                                                                                Source.Raster.NumberOfRows,
                                                                                Source.Raster.NumberOfColumns,
                                                                                32,
                                                                                Source.Raster.Mapper),
                                                            Source.Presentation,
                                                            Source.Imaging);

            _toarefGain = new Double[Source.Raster.NumberOfBands];

            switch (Source.Imaging.Device.MissionNumber)
            {
                case 7:
                    Double sunZenith = 90 - Source.Imaging.SunElevation, doySolarIrradianceSunZenithRatio;
                    Int32 dayOfYear = Source.Imaging.Time.DayOfYear;

                    for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
                    {
                        doySolarIrradianceSunZenithRatio = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (dayOfYear - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (dayOfYear - 4) * Math.PI / 180))) / (Source.Imaging.Bands[bandIndex].SolarIrradiance * Math.Cos(sunZenith * Constants.PI / 180)));
                        _toarefGain[bandIndex] = doySolarIrradianceSunZenithRatio;
                    }
                    break;
                case 8:
                    for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
                    {
                        _toarefGain[bandIndex] = 1 / (Math.Sin(Source.Imaging.SunElevation * Math.PI / 180));
                    }
                    break;
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
            switch (Source.Imaging.Device.MissionNumber)
            {
                case 7:
                    return (((Convert.ToDouble(Source.Raster.GetValue(rowIndex, columnIndex, bandIndex)) - 1) * Source.Imaging.Bands[bandIndex].PhysicalGain) + Source.Imaging.Bands[bandIndex].PhysicalBias) * _toarefGain[bandIndex] * 100;
                case 8:
                    Double value = (Convert.ToDouble(Source.Raster.GetValue(rowIndex, columnIndex, bandIndex)) * 0.00002 - 0.1) * _toarefGain[bandIndex] * 100;

                    return value > 0 ? value : 0;
            }

            return 0;
        }

        /// <summary>
        /// Computes the specified floating spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override sealed Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            Double[] values = new Double[_result.Raster.NumberOfBands];
            for (Int32 bandIndex = 0; bandIndex < _result.Raster.NumberOfBands; bandIndex++)
                values[bandIndex] = ComputeFloat(rowIndex, columnIndex, bandIndex);

            return values;
        }

        #endregion
    }
}

