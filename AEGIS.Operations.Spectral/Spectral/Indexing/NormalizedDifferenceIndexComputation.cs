// <copyright file="NormalizedDifferenceIndexComputation.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Indexing
{
    /// <summary>
    /// Represents an operation computing the normalized difference indices (NDxI) of raster geometries.
    /// </summary>
    /// <remarks>
    /// The operation computes normalized difference indices vegetation (NDVI), soil (NDSI) and water (NDWI) in the specified order.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::252011", "Normalized difference index (NDxI) computation")]
    public class NormalizedDifferenceIndexComputation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The index of the red band.
        /// </summary>
        private readonly Int32 _indexOfRedBand;

        /// <summary>
        /// The index of the near infrared band.
        /// </summary>
        private readonly Int32 _indexOfNearInfraredBand;

        /// <summary>
        /// The index of the short-wavelength infrared band.
        /// </summary>
        private readonly Int32 _indexOfShortWavelengthInfraredBand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedDifferenceIndexComputation" /> class.
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
        public NormalizedDifferenceIndexComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedDifferenceIndexComputation" /> class.
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
        public NormalizedDifferenceIndexComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.NormalizedDifferenceIndexComputation, parameters)
        {
            try
            {
                _indexOfRedBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfRedBand, Source.Imaging.SpectralDomains.IndexOf(SpectralDomain.Red)));
                _indexOfNearInfraredBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfNearInfraredBand, Source.Imaging.SpectralDomains.IndexOf(SpectralDomain.NearInfrared)));
                _indexOfShortWavelengthInfraredBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand, Source.Imaging.SpectralDomains.IndexOf(SpectralDomain.ShortWavelengthInfrared)));
            }
            catch
            {
                throw new ArgumentException("The source does not contain required data.", "source");
            }

            if (_indexOfRedBand < 0 || _indexOfRedBand >= Source.Raster.NumberOfBands ||
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
            SetResultProperties(RasterFormat.Floating, 3, 32, RasterPresentation.CreateFalseColorPresentation(0, 1, 2));

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
            Double swir, nir, red;

            switch (Source.Raster.Format)
            {
                case RasterFormat.Floating:
                    switch (bandIndex)
                    {
                        case 0: // vegetation
                            nir = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            red = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand);

                            return (nir + red == 0) ? 0 : (nir - red) / (nir + red);

                        case 1: // soil
                            swir = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                            nir = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);

                            return (swir + nir == 0) ? 0 : (swir - nir) / (swir + nir);

                        case 2: // water
                            swir = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                            red = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand);

                            return (swir + red == 0) ? 0 : (swir - red) / (swir + red);
                    }
                    break;
                case RasterFormat.Integer:
                    switch (bandIndex)
                    {
                        case 0: // vegetation
                            nir = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            red = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);
                            
                            return (nir + red == 0) ? 0 : (nir - red) / (nir + red);

                        case 1: // soil
                            swir = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                            nir = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            
                            return (swir + nir == 0) ? 0 : (swir - nir) / (swir + nir);

                        case 2: // water
                            swir = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                            red = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);

                            return (swir + red == 0) ? 0 : (swir - red) / (swir + red);
                    }
                    break;
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
            Double swir, nir, red;

            switch (Source.Raster.Format)
            {
                case RasterFormat.Integer:
                    swir = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                    nir = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                    red = Source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);
                    break; 
                
                default:
                    swir = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                    nir = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                    red = Source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand);
                    break;
            }

            return new Double[] { (nir + red == 0) ? 0 : (nir - red) / (nir + red), 
                                  (swir + nir == 0) ? 0 : (swir - nir) / (swir + nir),
                                  (swir + red == 0) ? 0 : (swir - red) / (swir + red) };
        }

        #endregion
    }
}
