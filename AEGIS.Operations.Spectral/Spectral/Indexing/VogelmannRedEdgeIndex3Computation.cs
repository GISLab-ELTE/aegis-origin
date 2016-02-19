/// <copyright file="VogelmannRedEdgeIndex3Computation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an operation computing the Vogelmann red edge index 3 (VOG3) of raster geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::252713", "Vogelmann red edge index 3 (VOG3) computation")]
    public class VogelmannRedEdgeIndex3Computation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The index of the 715nm band.
        /// </summary>
        private readonly Int32 _indexOf715nmBand;

        /// <summary>
        /// The index of the 720nm band.
        /// </summary>
        private readonly Int32 _indexOf720nmBand;

        /// <summary>
        /// The index of the 734nm band.
        /// </summary>
        private readonly Int32 _indexOf734nmBand;

        /// <summary>
        /// The index of the 747nm band.
        /// </summary>
        private readonly Int32 _indexOf747nmBand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VogelmannRedEdgeIndex3Computation" /> class.
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
        public VogelmannRedEdgeIndex3Computation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VogelmannRedEdgeIndex3Computation" /> class.
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
        public VogelmannRedEdgeIndex3Computation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.VogelmannRedEdgeIndex3Computation, parameters)
        {
            try
            {
                _indexOf715nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf715nmBand, _source.Imaging.SpectralRanges.FindIndex(range => range.WavelengthMinimum <= 715e-9 && range.WavelengthMaximum >= 715e-9)));
                _indexOf720nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf720nmBand, _source.Imaging.SpectralRanges.FindIndex(range => range.WavelengthMinimum <= 720e-9 && range.WavelengthMaximum >= 720e-9)));
                _indexOf734nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf734nmBand, _source.Imaging.SpectralRanges.FindIndex(range => range.WavelengthMinimum <= 734e-9 && range.WavelengthMaximum >= 734e-9)));
                _indexOf747nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf747nmBand, _source.Imaging.SpectralRanges.FindIndex(range => range.WavelengthMinimum <= 747e-9 && range.WavelengthMaximum >= 747e-9)));
            }
            catch
            {
                throw new ArgumentException("The source does not contain required data.", "source");
            }

            if (_indexOf715nmBand < 0 || _indexOf715nmBand >= Source.Raster.NumberOfBands ||
                _indexOf720nmBand < 0 || _indexOf720nmBand >= Source.Raster.NumberOfBands ||
                _indexOf734nmBand < 0 || _indexOf734nmBand >= Source.Raster.NumberOfBands ||
                _indexOf747nmBand < 0 || _indexOf747nmBand >= Source.Raster.NumberOfBands)
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
            Double nm715, nm720, nm734, nm747;

            switch (Source.Raster.Format)
            {
                case RasterFormat.Integer:
                    nm715 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf715nmBand);
                    nm720 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf720nmBand);
                    nm734 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf734nmBand);
                    nm747 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf747nmBand);
                    break;
                default:
                    nm715 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf715nmBand);
                    nm720 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf720nmBand);
                    nm734 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf734nmBand);
                    nm747 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf747nmBand);
                    break;
            }

            return (nm715 + nm720 == 0) ? 0 : (nm734 - nm747) / (nm715 + nm720);

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
