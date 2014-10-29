/// <copyright file="NormalizedDifferenceSoilIndexComputation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Indexing
{
    /// <summary>
    /// Represents an operation computing the normalized difference soil index (NDSI) of raster geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213512", "Normalized difference soil index (NDSI) computation")]
    public class NormalizedDifferenceSoilIndexComputation : SpectralTransformation
    {
        #region Private fields

        private readonly Int32 _indexOfNearInfraredBand;
        private readonly Int32 _indexOfShortWaveInfraredBand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedDifferenceSoilIndexComputation" /> class.
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
        public NormalizedDifferenceSoilIndexComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedDifferenceSoilIndexComputation" /> class.
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
        public NormalizedDifferenceSoilIndexComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.NormalizedDifferenceSoilIndexComputation, parameters)
        {
            if (IsProvidedParameter(SpectralOperationParameters.IndexOfNearInfraredBand))
            {
                _indexOfNearInfraredBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfNearInfraredBand));
            }
            else if (_source.Imaging != null && _source.Imaging.SpectralDomains.Contains(SpectralDomain.NearInfrared))
            {
                _indexOfNearInfraredBand = _source.Imaging.SpectralDomains.IndexOf(SpectralDomain.NearInfrared);
            }
            else
                _indexOfNearInfraredBand = 1;

            if (IsProvidedParameter(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand))
            {
                _indexOfShortWaveInfraredBand = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));
            }
            else if (_source.Imaging != null && _source.Imaging.SpectralDomains.Contains(SpectralDomain.ShortWavelengthInfrared))
            {
                _indexOfShortWaveInfraredBand = _source.Imaging.SpectralDomains.IndexOf(SpectralDomain.ShortWavelengthInfrared);
            }
            else
                _indexOfShortWaveInfraredBand = 2;
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
            Double swir, nir;

            switch (_source.Raster.Format)
            {
                case RasterFormat.Integer:
                    swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                    nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                    break;

                default:
                    swir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                    nir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                    break;
            }

            return (swir - nir) / (swir + nir);
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
            _result = _source.Factory.CreateSpectralGeometry(_source,
                                                             PrepareRasterResult(RasterFormat.Floating,
                                                                                 1,
                                                                                 _source.Raster.NumberOfRows,
                                                                                 _source.Raster.NumberOfColumns,
                                                                                 new Int32[] { 64 },
                                                                                 _source.Raster.Mapper));
        }

        #endregion
    }
}
