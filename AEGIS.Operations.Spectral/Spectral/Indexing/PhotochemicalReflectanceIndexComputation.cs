/// <copyright file="PhotochemicalReflectanceIndexComputation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an operation computing the photochemical reflectance index (PRI) of raster geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::252301", "Photochemical reflectance index (PRI) computation")]
    public class PhotochemicalReflectanceIndexComputation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The index of the 531nm band.
        /// </summary>
        private readonly Int32 _indexOf531nmBand;

        /// <summary>
        /// The index of the 570nm band.
        /// </summary>
        private readonly Int32 _indexOf570nmBand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotochemicalReflectanceIndexComputation" /> class.
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
        public PhotochemicalReflectanceIndexComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotochemicalReflectanceIndexComputation" /> class.
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
        public PhotochemicalReflectanceIndexComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.PhotochemicalReflectanceIndexComputation, parameters)
        {
            try
            {
                _indexOf531nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf531nmBand, _source.Imaging.SpectralRanges.FindIndex(range => range.WavelengthMinimum <= 531e-9 && range.WavelengthMaximum >= 531e-9)));
                _indexOf570nmBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOf570nmBand, _source.Imaging.SpectralRanges.FindIndex(range => range.WavelengthMinimum <= 570e-9 && range.WavelengthMaximum >= 570e-9)));
            }
            catch
            {
                throw new ArgumentException("The source does not contain required data.", "source");
            }

            if (_indexOf531nmBand < 0 || _indexOf531nmBand >= Source.Raster.NumberOfBands ||
                _indexOf570nmBand < 0 || _indexOf570nmBand >= Source.Raster.NumberOfBands)
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
            Double nm531, nm570;

            switch (Source.Raster.Format)
            {
                case RasterFormat.Integer:
                    nm531 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf531nmBand);
                    nm570 = Source.Raster.GetValue(rowIndex, columnIndex, _indexOf570nmBand);
                    break;
                default:
                    nm531 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf531nmBand);
                    nm570 = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOf570nmBand);
                    break;
            }

            return (nm531 + nm570 == 0) ? 0 : ((nm531 - nm570) / (nm531 + nm570));

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
                                                                                32,
                                                                                Source.Raster.Mapper),
                                                            RasterPresentation.CreateGrayscalePresentation(),
                                                            Source.Imaging);
        }

        #endregion

    }
}
