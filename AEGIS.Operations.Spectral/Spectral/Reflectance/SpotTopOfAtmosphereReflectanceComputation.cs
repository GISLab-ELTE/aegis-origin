/// <copyright file="SpotTopOfAtmosphereReflectanceComputation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spectral.Reflectance
{
    /// <summary>
    /// Represents an operation computing the Top of Atmosphere (ToA) reflectance of raster geometries for SPOT images.
    /// </summary>
    [OperationMethodImplementation("AEGIS::255105", "Top of atmosphere reflectance computation")]
    public class SpotTopOfAtmosphereReflectanceComputation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The index of the green band.
        /// </summary>
        private readonly Int32 _indexOfGreenBand;

        /// <summary>
        /// The index of the red band.
        /// </summary>
        private readonly Int32 _indexOfRedBand;

        /// <summary>
        /// The index of the near infrared band.
        /// </summary>
        private readonly Int32 _indexOfNearInfraredBand;

        /// <summary>
        /// The index of the short-wave infrared band.
        /// </summary>
        private readonly Int32 _indexOfShortWavelengthInfraredBand;

        /// <summary>
        /// The ToA reflectance gain.
        /// </summary>
        private Double[] _toarefGain;

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
        /// or
        /// The source does not contain required data.
        /// or
        /// The source contains invalid data.
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
        /// The source contains invalid data.
        /// </exception>
        public SpotTopOfAtmosphereReflectanceComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.TopOfAtmospehereReflectanceComputation, parameters)
        {
            if (Source.Imaging == null)
                throw new ArgumentException("The source does not contain required data.", "source");
            if (Source.Imaging.Device.Mission != "SPOT")
                throw new ArgumentException("The source does not contain required data.", "source");

            try
            {
                _indexOfGreenBand = ResolveParameter(SpectralOperationParameters.IndexOfGreenBand, Source.Imaging.SpectralDomains.IndexOf(SpectralDomain.Green));
                _indexOfRedBand = ResolveParameter(SpectralOperationParameters.IndexOfRedBand, Source.Imaging.SpectralDomains.IndexOf(SpectralDomain.Red));
                _indexOfNearInfraredBand = ResolveParameter(SpectralOperationParameters.IndexOfNearInfraredBand, Source.Imaging.SpectralDomains.IndexOf(SpectralDomain.NearInfrared));
                _indexOfShortWavelengthInfraredBand = ResolveParameter(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand, Source.Imaging.SpectralDomains.IndexOf(SpectralDomain.ShortWavelengthInfrared));
            }
            catch
            {
                throw new ArgumentException("The source does not contain required data.", "source");
            }

            if (_indexOfGreenBand < 0 || _indexOfGreenBand >= Source.Raster.NumberOfBands ||
                _indexOfRedBand < 0 || _indexOfRedBand >= Source.Raster.NumberOfBands ||
                _indexOfNearInfraredBand < 0 || _indexOfNearInfraredBand >= Source.Raster.NumberOfBands ||
                _indexOfShortWavelengthInfraredBand < 0 || _indexOfShortWavelengthInfraredBand >= Source.Raster.NumberOfBands)
            {
                throw new ArgumentException("The source contains invalid data.", "source");
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            _toarefGain = new Double[Source.Raster.NumberOfBands];

            Double sunZenith = 90 - Source.Imaging.SunElevation, doySolarIrradianceSunZenithRatio;
            Int32 dayOfYear = Source.Imaging.Time.DayOfYear;

            for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
            {
                doySolarIrradianceSunZenithRatio = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (dayOfYear - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (dayOfYear - 4) * Math.PI / 180))) / (Source.Imaging.Bands[bandIndex].SolarIrradiance * Math.Cos(sunZenith * Constants.PI / 180)));
                _toarefGain[bandIndex] = doySolarIrradianceSunZenithRatio / Source.Imaging.Bands[bandIndex].PhysicalGain;
            }

            SetResultProperties(RasterFormat.Floating, 32, RasterPresentation.CreateGrayscalePresentation());

            return base.PrepareResult();
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
                    return Source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand) * _toarefGain[_indexOfGreenBand] * 100;
                case 1:
                    return Source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand) * _toarefGain[_indexOfRedBand] * 100;
                case 2:
                    return Source.Raster.GetValue(rowIndex, columnIndex, _indexOfGreenBand) * _toarefGain[_indexOfNearInfraredBand] * 100;
                case 3:
                    return Source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand) * _toarefGain[_indexOfShortWavelengthInfraredBand] * 100;
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
                Source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand) * _toarefGain[_indexOfGreenBand] * 100,
                Source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand) * _toarefGain[_indexOfRedBand] * 100,
                Source.Raster.GetValue(rowIndex, columnIndex, _indexOfGreenBand) * _toarefGain[_indexOfNearInfraredBand] * 100,
                Source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand) * _toarefGain[_indexOfShortWavelengthInfraredBand] * 100
            };
        }

        #endregion
    }
}
