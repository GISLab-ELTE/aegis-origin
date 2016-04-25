/// <copyright file="SaturatingContrastEnhancement.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Common
{
    /// <summary>
    /// Represents an inversion transformation.
    /// </summary>
    [OperationMethodImplementation("AEGIS::250204", "Saturating contrast enhancement")]
    public class SaturatingContrastEnhancement : PerBandSpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The array of offset values.
        /// </summary>
        private Int32[] _offset;

        /// <summary>
        /// The array of factor values.
        /// </summary>
        private Double[] _factor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SaturatingContrastEnhancement" /> class.
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
        public SaturatingContrastEnhancement(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaturatingContrastEnhancement" /> class.
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
        public SaturatingContrastEnhancement(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.SaturatingContrastEnhancement, parameters)
        {
            _offset = new Int32[SourceBandIndices.Length];
            _factor = new Double[SourceBandIndices.Length];

            for (Int32 bandIndex = 0; bandIndex < SourceBandIndices.Length; bandIndex++)
            {
                Int32 minIntensity = 0, maxIntensity = _source.Raster.HistogramValues[SourceBandIndices[bandIndex]].Count - 1;

                while (minIntensity < _source.Raster.HistogramValues[SourceBandIndices[bandIndex]].Count && _source.Raster.HistogramValues[SourceBandIndices[bandIndex]][minIntensity] == 0)
                    minIntensity++;
                while (maxIntensity >= 0 && _source.Raster.HistogramValues[SourceBandIndices[bandIndex]][maxIntensity] == 0)
                    maxIntensity--;

                if (minIntensity == maxIntensity)
                {
                    _offset[bandIndex] = 0;
                    _factor[bandIndex] = 1;
                }
                else
                {
                    _offset[bandIndex] = -minIntensity;
                    _factor[bandIndex] = (Double)RasterAlgorithms.RadiometricResolutionMax(_source.Raster.RadiometricResolution) / (maxIntensity - minIntensity);
                }
            }
        }

        #endregion

        #region Protected RasterTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return RasterAlgorithms.Restrict(_source.Raster.GetValue(rowIndex, columnIndex, bandIndex) * _factor[bandIndex] + _offset[bandIndex], _source.Raster.RadiometricResolution);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return _source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex) * _factor[bandIndex] + _offset[bandIndex];
        }

        #endregion        
    }
}
