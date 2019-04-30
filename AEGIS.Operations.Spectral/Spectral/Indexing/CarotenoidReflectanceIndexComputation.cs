/// <copyright file="CarotenoidReflectanceIndexComputation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Greta Bereczki</author>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Indexing
{
    /// <summary>
    /// Represents an operation computing the carotenoid reflectance indices (CRIx) of raster geometries.
    /// </summary>
    /// <remarks>
    /// The operation computes carotenoid reflectance index 1 (CRI1) and the carotenoid reflectance index 2 (CRI2) in the specified order.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::252320", "Carotenoid reflectance index (CRIx) computation")]
    public class CarotenoidReflectanceIndexComputation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The index of the 510nm band.
        /// </summary>
        private readonly Int32 _indexOf510nmBand;

        /// <summary>
        /// The index of the 550nm band.
        /// </summary>
        private readonly Int32 _indexOf550nmBand;

        /// <summary>
        /// The index of the 700nm band.
        /// </summary>
        private readonly Int32 _indexOf700nmBand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CarotenoidReflectanceIndexComputation" /> class.
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
        public CarotenoidReflectanceIndexComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CarotenoidReflectanceIndexComputation" /> class.
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
        public CarotenoidReflectanceIndexComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.CarotenoidReflectanceIndexComputation, parameters)
        {
            try
            {
                _indexOf510nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf510nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 510e-9 && range.WavelengthMaximum >= 510e-9)));
                _indexOf550nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf550nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 550e-9 && range.WavelengthMaximum >= 550e-9)));
                _indexOf700nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf700nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 700e-9 && range.WavelengthMaximum >= 700e-9)));
            }
            catch
            {
                throw new ArgumentException("The source does not contain required data.", "source");
            }

            if (_indexOf510nmBand < 0 || _indexOf510nmBand >= Source.Raster.NumberOfBands ||
                _indexOf550nmBand < 0 || _indexOf550nmBand >= Source.Raster.NumberOfBands ||
                _indexOf700nmBand < 0 || _indexOf700nmBand >= Source.Raster.NumberOfBands)
            {
                throw new ArgumentException("The source contains invalid data.", "source");
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
            Double nm510, nm550, nm700;

            switch (Source.Raster.Format)
            {
                case RasterFormat.Integer:
                    switch (bandIndex)
                    {
                        case 0: //CRI1
                            nm510 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf510nmBand);
                            nm550 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf550nmBand);

                            return (nm510 == 0 || nm550 == 0) ? 0 : (1 / nm510 - 1 / nm550);

                        case 1: //CRI2
                            nm510 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf510nmBand);
                            nm700 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf700nmBand);

                            return (nm510 == 0 || nm700 == 0) ? 0 : (1 / nm510 - 1 / nm700);
                    }
                    break;
                case RasterFormat.Floating:
                    switch (bandIndex)
                    {
                        case 0: //CRI1
                            nm510 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf510nmBand);
                            nm550 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf550nmBand);

                            return (nm510 == 0 || nm550 == 0) ? 0 : (1 / nm510 - 1 / nm550);

                        case 1: //CRI2
                            nm510 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf510nmBand);
                            nm700 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf700nmBand);

                            return (nm510 == 0 || nm700 == 0) ? 0 : (1 / nm510 - 1 / nm700);
                    }
                    break;
            }

            return 0;
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            Double[] result = new Double[2];
            for (Int32 bandIndex = 0; bandIndex < 2; bandIndex++)
                result[bandIndex] = ComputeFloat(rowIndex, columnIndex, bandIndex);

            return result;
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            SetResultProperties(RasterFormat.Floating, 2, 32, RasterPresentation.CreateGrayscalePresentation());

            return base.PrepareResult();
        }

        #endregion
    }
}
