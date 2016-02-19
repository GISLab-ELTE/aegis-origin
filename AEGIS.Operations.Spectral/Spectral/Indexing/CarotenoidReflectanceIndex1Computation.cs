/// <copyright file="CarotenoidReflectanceIndex1Computation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
    /// Represents an operation computing the carotenoid reflectance index 1 (CRI1) of raster geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::252321", "Carotenoid reflectance index 1 (CRI1) computation")]
    public class CarotenoidReflectanceIndex1Computation : SpectralTransformation
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CarotenoidReflectanceIndex1Computation" /> class.
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
        public CarotenoidReflectanceIndex1Computation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CarotenoidReflectanceIndex1Computation" /> class.
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
        public CarotenoidReflectanceIndex1Computation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.CarotenoidReflectanceIndex1Computation, parameters)
        {
            try
            {
                _indexOf510nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf510nmBand, _source.Imaging.SpectralRanges.FindIndex(range => range.WavelengthMinimum <= 510e-9 && range.WavelengthMaximum >= 510e-9)));
                _indexOf550nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf550nmBand, _source.Imaging.SpectralRanges.FindIndex(range => range.WavelengthMinimum <= 550e-9 && range.WavelengthMaximum >= 550e-9)));
            }
            catch
            {
                throw new ArgumentException("The source does not contain required data.", "source");
            }

            if (_indexOf510nmBand < 0 || _indexOf510nmBand >= Source.Raster.NumberOfBands ||
                _indexOf550nmBand < 0 || _indexOf550nmBand >= Source.Raster.NumberOfBands)
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
            Double nm510, nm550;

            switch (Source.Raster.Format)
            {
                case RasterFormat.Integer:
                    nm510 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf510nmBand);
                    nm550 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf550nmBand);
                    break;

                default:
                    nm510 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf510nmBand);
                    nm550 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf550nmBand);
                    break;
            }

            return (nm510 == 0 || nm550 == 0) ? 0 : (1 / nm510 - 1 / nm550);
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
        protected override void PrepareResult()
        {
            _result = Source.Factory.CreateSpectralGeometry(Source,
                                                            PrepareRasterResult(RasterFormat.Floating,
                                                                                1,
                                                                                Source.Raster.NumberOfRows,
                                                                                Source.Raster.NumberOfColumns,
                                                                                new Int32[] { 32 },
                                                                                Source.Raster.Mapper),
                                                            RasterPresentation.CreateGrayscalePresentation(),
                                                            Source.Imaging);
        }

        #endregion
    }
}
