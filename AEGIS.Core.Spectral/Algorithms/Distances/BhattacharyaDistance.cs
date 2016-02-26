/// <copyright file="BhattacharyaDistance.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Algorithms.Distances
{
    /// <summary>
    /// Represents a type measuring Bhattacharya distance of spectral values.
    /// </summary>
    public class BhattacharyaDistance : SpectralDistance
    {
        #region Public properties

        /// <summary>
        /// Gets the statistics required by the spectral distance.
        /// </summary>
        /// <value>The statistics required by the spectral distance.</value>
        public override SpectralStatistics Statistics { get { return SpectralStatistics.Covariance; } }

        #endregion

        #region Protected SpectralDistance methods

        /// <summary>
        /// Computes the spectral distance between two segments.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="otherSegment">The other segment.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(Segment segment, Segment otherSegment)
        {
            Matrix covarianceA = new Matrix(segment.Covariance);
            Matrix covarianceB = new Matrix(otherSegment.Covariance);
            Matrix v = new Matrix(1, segment.NumberOfBands);

            for (Int32 bandIndex = 0; bandIndex < segment.NumberOfBands; bandIndex++)
                v[0, bandIndex] = segment.Mean[bandIndex] - otherSegment.Mean[bandIndex];

            Matrix meanMatrix = (covarianceA + covarianceB) * 0.5;

            Double detMean = meanMatrix.ComputeDeterminant();
            Double detA = covarianceA.ComputeDeterminant();
            Double detB = covarianceB.ComputeDeterminant();

            Double distanceBase = (v * meanMatrix.Invert() * v.Transpone())[0, 0] / 8;

            // if any determinants are zero 
            if (detMean == 0 || detA == 0 || detB == 0)
                return distanceBase + 0.5;

            return distanceBase + 0.5 * Math.Log(Math.Abs(detMean / Math.Sqrt(Math.Abs(detA * detB))));
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IEnumerable<UInt32> values, IEnumerable<UInt32> otherValues)
        {
            return 0;
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IEnumerable<Double> values, IEnumerable<UInt32> otherValues)
        {
            return 0;
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IEnumerable<Double> values, IEnumerable<Double> otherValues)
        {
            return 0;
        }

        #endregion
    }
}
