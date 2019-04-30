/// <copyright file="PlantSenescenceReflectanceIndexComputation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an operation computing the plant senescence reflectance index (PSRI) of raster geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::252302", "Plant senescence reflectance index (PSRI) computation")]
    public class PlantSenescenceReflectanceIndexComputation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The index of the 500nm band.
        /// </summary>
        private readonly Int32 _indexOf500nmBand;

        /// <summary>
        /// The index of the 680nm band.
        /// </summary>
        private readonly Int32 _indexOf680nmBand;

        /// <summary>
        /// The index of the 750nm band.
        /// </summary>
        private readonly Int32 _indexOf750nmBand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlantSenescenceReflectanceIndexComputation" /> class.
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
        public PlantSenescenceReflectanceIndexComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlantSenescenceReflectanceIndexComputation" /> class.
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
        public PlantSenescenceReflectanceIndexComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.PlantSenescenceReflectanceIndexComputation, parameters)
        {
            try
            {
                _indexOf500nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf500nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 500e-9 && range.WavelengthMaximum >= 500e-9)));
                _indexOf680nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf680nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 680e-9 && range.WavelengthMaximum >= 680e-9)));
                _indexOf750nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf750nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 750e-9 && range.WavelengthMaximum >= 750e-9)));
            }
            catch
            {
                throw new ArgumentException("The source does not contain required data.", "source");
            }

            if (_indexOf500nmBand < 0 || _indexOf500nmBand >= Source.Raster.NumberOfBands ||
                _indexOf680nmBand < 0 || _indexOf680nmBand >= Source.Raster.NumberOfBands ||
                _indexOf750nmBand < 0 || _indexOf750nmBand >= Source.Raster.NumberOfBands)
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
            Double nm500, nm680, nm750;

            switch (Source.Raster.Format)
            {
                case RasterFormat.Integer:
                    nm500 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf500nmBand);
                    nm680 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf680nmBand);
                    nm750 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf750nmBand);
                    break;

                default:
                    nm500 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf500nmBand);
                    nm680 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf680nmBand);
                    nm750 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf750nmBand);
                    break;
            }

            return (nm750 == 0) ? 0 : (nm680 - nm500) / nm750;
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            return new Double[] { ComputeFloat(rowIndex, columnIndex, 0) };
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            SetResultProperties(RasterFormat.Floating, 1, 32, RasterPresentation.CreateGrayscalePresentation());

            return base.PrepareResult();
        }

        #endregion
    }
}
