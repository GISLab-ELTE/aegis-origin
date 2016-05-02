/// <copyright file="StructureInsensitivePigmentIndexComputation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an operation computing the structure insensitive pigment index (SIPI) of raster geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::252308", "Structure insensitive pigment index (SIPI) computation")]
    public class StructureInsensitivePigmentIndexComputation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The index of the 445nm band.
        /// </summary>
        private readonly Int32 _indexOf445nmBand;

        /// <summary>
        /// The index of the 680nm band.
        /// </summary>
        private readonly Int32 _indexOf680nmBand;

        /// <summary>
        /// The index of the 800nm band.
        /// </summary>
        private readonly Int32 _indexOf800nmBand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureInsensitivePigmentIndexComputation" /> class.
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
        public StructureInsensitivePigmentIndexComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureInsensitivePigmentIndexComputation" /> class.
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
        public StructureInsensitivePigmentIndexComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.StructureInsensitivePigmentIndexComputation, parameters)
        {
            try
            {
                _indexOf445nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf445nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 445e-9 && range.WavelengthMaximum >= 445e-9)));
                _indexOf680nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf680nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 680e-9 && range.WavelengthMaximum >= 680e-9)));
                _indexOf800nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf800nmBand, Source.Imaging.SpectralRanges.IndexOf(range => range.WavelengthMinimum <= 800e-9 && range.WavelengthMaximum >= 800e-9)));
            }
            catch
            {
                throw new ArgumentException("The source does not contain required data.", "source");
            }

            if (_indexOf445nmBand < 0 || _indexOf445nmBand >= Source.Raster.NumberOfBands ||
                _indexOf680nmBand < 0 || _indexOf680nmBand >= Source.Raster.NumberOfBands ||
                _indexOf800nmBand < 0 || _indexOf800nmBand >= Source.Raster.NumberOfBands)
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
            Double nm445, nm680, nm800;

            switch (Source.Raster.Format)
            {
                case RasterFormat.Integer:
                    nm445 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf445nmBand);
                    nm680 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf680nmBand);
                    nm800 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf800nmBand);
                    break;
                default:
                    nm445 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf445nmBand);
                    nm680 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf680nmBand);
                    nm800 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf800nmBand);
                    break;
            }

            return (nm800 - nm680 == 0) ? 0 : ((nm800 - nm445) / (nm800 - nm680));

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
