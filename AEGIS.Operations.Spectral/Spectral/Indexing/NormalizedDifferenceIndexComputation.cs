/// <copyright file="NormalizedDifferenceIndexComputation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Indexing
{
    /// <summary>
    /// Represents an operation computing the normalized difference indices (NDxI) of raster geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213511", "Normalized difference index (NDxI) computation")]
    public class NormalizedDifferenceIndexComputation : SpectralTransformation
    {
        #region Private fields

        private readonly Int32 _indexOfRedBand;
        private readonly Int32 _indexOfNearInfraredBand;
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
        /// </exception>
        public NormalizedDifferenceIndexComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.NormalizedDifferenceIndexComputation, parameters)
        {
            if (IsProvidedParameter(SpectralOperationParameters.IndexOfRedBand))
            {
                _indexOfRedBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfRedBand));
            }
            else if (_source.Imaging != null)
            {
                RasterImagingBand imagingBand = _source.Imaging.Bands.FirstOrDefault(band => band.SpectralDomain == SpectralDomain.Red);
                if (imagingBand != null)
                    _indexOfRedBand = _source.Imaging.Bands.IndexOf(imagingBand);
            }
            else
            {
                _indexOfRedBand = 0;
            }

            if (IsProvidedParameter(SpectralOperationParameters.IndexOfNearInfraredBand))
            {
                _indexOfNearInfraredBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfNearInfraredBand));
            }
            else if (_source.Imaging != null)
            {
                RasterImagingBand imagingBand = _source.Imaging.Bands.FirstOrDefault(band => band.SpectralDomain == SpectralDomain.NearInfrared);
                if (imagingBand != null)
                    _indexOfNearInfraredBand = _source.Imaging.Bands.IndexOf(imagingBand);
            }
            else
            {
                _indexOfNearInfraredBand = 1;
            }

            if (IsProvidedParameter(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand))
            {
                _indexOfShortWavelengthInfraredBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));
            }
            else if (_source.Imaging != null)
            {
                RasterImagingBand imagingBand = _source.Imaging.Bands.FirstOrDefault(band => band.SpectralDomain == SpectralDomain.ShortWavelengthInfrared);
                if (imagingBand != null)
                    _indexOfShortWavelengthInfraredBand = _source.Imaging.Bands.IndexOf(imagingBand);
            }
            else
            {
                _indexOfNearInfraredBand = 2;
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _result = _source.Factory.CreateSpectralGeometry(_source,
                                                             PrepareRasterResult(RasterFormat.Floating,
                                                                                 3,
                                                                                 _source.Raster.NumberOfRows,
                                                                                 _source.Raster.NumberOfColumns,
                                                                                 32,
                                                                                 _source.Raster.Mapper),
                                                             RasterPresentation.CreateFalseColorPresentation(0, 1, 2),
                                                             null);
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

            switch (_source.Raster.Format)
            {
                case RasterFormat.Floating:
                    switch (bandIndex)
                    {
                        case 0: // soil
                            swir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                            nir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);

                            if (swir == 0 && nir == 0)
                                return 0;

                            return (swir - nir) / (swir + nir);

                        case 1: // vegetation
                            nir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            red = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand);

                            if (red == 0 && nir == 0)
                                return 0;

                            return (nir - red) / (nir + red);

                        case 2: // water
                            swir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                            red = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand);

                            if (swir == 0 && red == 0)
                                return 0;

                            return (swir - red) / (swir + red);
                    }
                    break;
                case RasterFormat.Integer:
                    switch (bandIndex)
                    {
                        case 0: // soil
                            swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                            nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            if (swir == 0 && nir == 0)
                                return 0;

                            return (swir - nir) / (swir + nir);

                        case 1: // vegetation
                            nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);

                            if (nir == 0 && red == 0)
                                return 0;

                            return (nir - red) / (nir + red);

                        case 2: // water
                            swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                            red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);

                            if (swir == 0 && red == 0)
                                return 0;

                            return (swir - red) / (swir + red);
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

            switch (_source.Raster.Format)
            {
                case RasterFormat.Integer:
                    swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                    nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                    red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);
                    break;

                default:
                    swir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWavelengthInfraredBand);
                    nir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                    red = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand);
                    break;
                    
            }
            return new Double[] { (swir - nir) / (swir + nir), (nir - red) / (nir + red), (swir - red) / (swir + red) };
        }

        #endregion
    }
}
