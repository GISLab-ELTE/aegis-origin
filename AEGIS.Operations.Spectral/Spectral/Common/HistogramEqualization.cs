/// <copyright file="HistogramEqualization.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Common
{
    /// <summary>
    /// Represent an operation performing histogram equalization of <see cref="ISpectralGeometry"/> instances.
    /// </summary>
    [OperationMethodImplementation("AEGIS::250207", "Histogram equalization")]
    public class HistogramEqualization : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The cumulative distribution values for the histogram of each band.
        /// </summary>
        private Double[][] _cumulativeDistributionValues;

        /// <summary>
        /// The cumulative distribution value for the histogram minimum of each band.
        /// </summary>
        private Double[] _cumulativeDistributionMinimums;

        /// <summary>
        /// The cumulative distribution value for the histogram maximum of each band.
        /// </summary>
        private Double[] _cumulativeDistributionMaximums;

        /// <summary>
        /// The radiometric exponent value of each band.
        /// </summary>
        private Double[] _radiometricResolutionExponents;

        /// <summary>
        /// The radiometric limit value of each band.
        /// </summary>
        private UInt32[] _radiometricValueLimits;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramEqualization" /> class.
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
        public HistogramEqualization(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramEqualization" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
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
        public HistogramEqualization(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.HistogramEqualization, parameters)
        {
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            _cumulativeDistributionValues = new Double[Source.Raster.NumberOfBands][];
            _cumulativeDistributionMinimums = new Double[Source.Raster.NumberOfBands];
            _cumulativeDistributionMaximums = new Double[Source.Raster.NumberOfBands];
            _radiometricResolutionExponents = new Double[Source.Raster.NumberOfBands];
            _radiometricValueLimits = new UInt32[Source.Raster.NumberOfBands];

            for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
                ComputeParameters(bandIndex);

            return base.PrepareResult();
        }

        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return Math.Min((UInt32)((_cumulativeDistributionValues[bandIndex][Source.Raster.GetValue(rowIndex, columnIndex, bandIndex)] - _cumulativeDistributionMinimums[bandIndex]) / (_cumulativeDistributionMaximums[bandIndex] - _cumulativeDistributionMinimums[bandIndex]) * _radiometricResolutionExponents[bandIndex]), _radiometricValueLimits[bandIndex]);
        }
        
        #endregion

        #region Private methods

        /// <summary>
        /// Computes the parameters of the specified band.
        /// </summary>
        /// <param name="bandIndex">The band index.</param>
        private void ComputeParameters(Int32 bandIndex)
        {
            // source: http://en.wikipedia.org/wiki/Histogram_equalization

            IReadOnlyList<Int32> histogram = Source.Raster.HistogramValues[bandIndex];

            _cumulativeDistributionValues[bandIndex] = new Double[histogram.Count];

            // setting values
            _cumulativeDistributionValues[bandIndex][0] = Convert.ToDouble(histogram[0]) / (Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns);
            for (Int32 i = 1; i < histogram.Count; i++)
            {
                _cumulativeDistributionValues[bandIndex][i] = _cumulativeDistributionValues[bandIndex][i - 1] + Convert.ToDouble(histogram[i]) / (Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns);
            }

            // setting minimum
            Int32 minIndex = 0;
            for (Int32 i = 0; i < histogram.Count; i++)
                if (histogram[i] != 0)
                {
                    minIndex = i;
                    break;
                }

            _cumulativeDistributionMinimums[bandIndex] = _cumulativeDistributionValues[bandIndex][minIndex];

            // setting maximum
            Int32 maxIndex = histogram.Count - 1;
            for (Int32 i = histogram.Count - 1; i >= 0; i--)
                if (histogram[i] != 0)
                {
                    maxIndex = i;
                    break;
                }

            _cumulativeDistributionMaximums[bandIndex] = _cumulativeDistributionValues[bandIndex][maxIndex];

            // exponent
            _radiometricResolutionExponents[bandIndex] = Calculator.Pow(2, Source.Raster.RadiometricResolution);

            // radiometric value limit
            _radiometricValueLimits[bandIndex] = RasterAlgorithms.RadiometricResolutionMax(Source.Raster.RadiometricResolution);
        }

        #endregion
    }
}
