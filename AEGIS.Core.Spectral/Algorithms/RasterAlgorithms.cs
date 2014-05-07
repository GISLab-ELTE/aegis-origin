/// <copyright file="RasterAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using System;
using ELTE.AEGIS.Numerics;
using System.Collections.Generic;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Contains algorithms for computing raster properties.
    /// </summary>
    public static class RasterAlgorithms
    {
        #region Radiometric resolution

        /// <summary>
        /// Computes the maximum value of a radiometric resolution.
        /// </summary>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <returns>The maximum value of the specified radiometric resolution.</returns>
        /// <exception cref="System.ArgumentException">
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is geater than 32.
        /// </exception>
        public static UInt64 RadiometricResolutionMax(Int32 radiometricResolution)
        {
            if (radiometricResolution < 1)
                throw new ArgumentException("The radiometric resolution is less than 1.", "radiometricResolution");
            if (radiometricResolution > 32)
                throw new ArgumentException("The radiometric resolution is geater than 32.", "radiometricResolution");

            return Convert.ToUInt64(1UL << radiometricResolution) - 1;
        }

        #endregion

        #region Threshold

        /// <summary>
        /// Compute the Otsu threshold.
        /// </summary>
        /// <param name="histogramValues">The histogram values of the image.</param>
        /// <returns>The Otsu threshold computed from the histrogram values.</returns>
        /// <exception cref="System.ArgumentNullException">The list of histogram values is null.</exception>
        /// <exception cref="System.ArgumentException">The list of histogram values is empty.</exception>
        public static Double ComputeOtsuThreshold(IList<Int32> histogramValues)
        {
            if (histogramValues == null)
                throw new ArgumentNullException("histogramValues", "The list of histogram values is null.");
            if (histogramValues.Count == 0)
                throw new ArgumentException("The list of histogram values is empty.", "histogramValues");

            // sum the pixel values according to histrogram values with relation to intensity
            Double sum = 0; 
            Int32 valueCount = 0;
            for (Int32 i = 0; i < histogramValues.Count; i++)
            {
                sum += i * histogramValues[i];
                valueCount += histogramValues[i];
            }

            Double varianceMax = 0, varianceBetween, threshold = 0, meanOfBackground, meanOfForeground;
            Int64 sumB = 0, weightOfBackground = 0, weightOfForeground = 0;

            for (Int32 i = 0; i < histogramValues.Count; i++)
            {
                weightOfBackground += histogramValues[i];
                if (weightOfBackground == 0) continue;

                weightOfForeground = valueCount - weightOfBackground;
                if (weightOfForeground == 0) break;

                sumB += i * histogramValues[i];

                meanOfBackground = Convert.ToSingle(sumB) / weightOfBackground;
                meanOfForeground = Convert.ToSingle(sum - sumB) / weightOfForeground;

                // compute the variance between classes
                varianceBetween = (weightOfBackground * weightOfForeground) * (meanOfBackground - meanOfForeground) * (meanOfBackground - meanOfForeground);

                // change the maximum
                if (varianceBetween > varianceMax)
                {
                    varianceMax = varianceBetween;
                    threshold = i;
                }
            }
            return threshold;
        }

        #endregion
    }
}
