/// <copyright file="NormalizedDifferenceIndexComputation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Indexing
{
    /// <summary>
    /// Represents an operation computing the normalized differential indices (NDxI) of raster geometries.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::213511", "Normalized difference index (NDxI) computation")]
    public class NormalizedDifferenceIndexComputation : SpectralTransformation
    {
        #region Private fields

        private readonly Int32 _indexOfRedBand;
        private readonly Int32 _indexOfNearInfraredBand;
        private readonly Int32 _indexOfShortWaveInfraredBand;

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
            if (parameters != null && parameters.ContainsKey(SpectralOperationParameters.IndexOfRedBand))
            {
                _indexOfRedBand = Convert.ToInt32(parameters[SpectralOperationParameters.IndexOfRedBand]);
            }
            else
            {
                if (source.Raster.SpectralRanges.Contains(SpectralRanges.Red))
                    _indexOfRedBand = source.Raster.SpectralRanges.IndexOf(SpectralRanges.Red);
                else
                    _indexOfRedBand = 0;
            }

            if (parameters != null && parameters.ContainsKey(SpectralOperationParameters.IndexOfNearInfraredBand))
            {
                _indexOfNearInfraredBand = Convert.ToInt32(parameters[SpectralOperationParameters.IndexOfNearInfraredBand]);
            }
            else
            {
                if (source.Raster.SpectralRanges.Contains(SpectralRanges.NearInfrared))
                    _indexOfNearInfraredBand = source.Raster.SpectralRanges.IndexOf(SpectralRanges.NearInfrared);
                else
                    _indexOfNearInfraredBand = 1;
            }

            if (parameters != null && parameters.ContainsKey(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand))
            {
                _indexOfShortWaveInfraredBand = Convert.ToInt32(parameters[SpectralOperationParameters.IndexOfShortWavelengthInfraredBand]);
            }
            else
            {
                if (source.Raster.SpectralRanges.Contains(SpectralRanges.ShortWavelengthInfrared))
                    _indexOfShortWaveInfraredBand = source.Raster.SpectralRanges.IndexOf(SpectralRanges.ShortWavelengthInfrared);
                else
                    _indexOfShortWaveInfraredBand = 2;
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
            Double swir, nir, red;

            switch (_source.Raster.Representation)
            {
                case RasterRepresentation.Floating:
                    IFloatRaster source = _source.Raster as IFloatRaster;
                    switch (bandIndex)
                    {
                        case 0:
                            swir = source.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                            nir = source.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            return (swir - nir) / (swir + nir);
                        case 1:
                            nir = source.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            red = source.GetValue(rowIndex, columnIndex, _indexOfRedBand);
                            return (nir - red) / (nir + red);
                        case 2:
                            swir = source.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                            red = source.GetValue(rowIndex, columnIndex, _indexOfRedBand);
                            return (swir - red) / (swir + red);
                    }
                    break;
                case RasterRepresentation.Integer:
                    switch (bandIndex)
                    {
                        case 0:
                            swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                            nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            return (swir - nir) / (swir + nir);
                        case 1:
                            nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);
                            return (nir - red) / (nir + red);
                        case 2:
                            swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                            red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);
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

            switch (_source.Raster.Representation)
            {
                case RasterRepresentation.Floating:
                    IFloatRaster source = _source.Raster as IFloatRaster;

                    swir = source.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                    nir = source.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                    red = source.GetValue(rowIndex, columnIndex, _indexOfRedBand);

                    return new Double[] { (swir - nir) / (swir + nir), (nir - red) / (nir + red), (swir - red) / (swir + red) };
                case RasterRepresentation.Integer:

                    swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                    nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                    red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand);

                    return new Double[] { (swir - nir) / (swir + nir), (nir - red) / (nir + red), (swir - red) / (swir + red) };
            }
            return null;
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _result = _source.Factory.CreateSpectralGeometry(PrepareRasterResult(3,
                                                                                 _source.Raster.NumberOfRows,
                                                                                 _source.Raster.NumberOfColumns,
                                                                                 _source.Raster.RadiometricResolutions,
                                                                                 _source.Raster.SpectralRanges,
                                                                                 _source.Raster.Mapper,
                                                                                 RasterRepresentation.Floating), _source);
        }

        #endregion
    }
}
