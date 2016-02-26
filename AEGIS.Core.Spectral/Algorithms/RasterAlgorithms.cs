/// <copyright file="RasterAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
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
        /// The radiometric resolution is greater than 32.
        /// </exception>
        public static UInt32 RadiometricResolutionMax(Int32 radiometricResolution)
        {
            if (radiometricResolution < 1)
                return 0;
            if (radiometricResolution >= 32)
                return UInt32.MaxValue;

            return (1U << radiometricResolution) - 1;
        }

        /// <summary>
        /// Restricts the specified spectral value for the a radiometric resolution.
        /// </summary>
        /// <param name="spectralValue">The spectral value.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <returns></returns>
        public static UInt32 Restrict(UInt32 spectralValue, Int32 radiometricResolution)
        {
            if (radiometricResolution < 1)
                return 0;
            if (radiometricResolution >= 32)
                return spectralValue;

            if (spectralValue > (1U << radiometricResolution) - 1)
                return 1U << radiometricResolution - 1;

            return spectralValue;
        }

        /// <summary>
        /// Restricts the specified spectral value for a radiometric resolution.
        /// </summary>
        /// <param name="spectralValue">The spectral value.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <returns>The value restricted for the specified radiometric resolution.</returns>
        public static UInt32 Restrict(Double spectralValue, Int32 radiometricResolution)
        {
            if (radiometricResolution < 1)
                return 0;
            if (Double.IsNaN(spectralValue) || spectralValue < 0)
                return 0;

            if (radiometricResolution >= 32)
                return Convert.ToUInt32(Math.Min(spectralValue, UInt32.MaxValue));

            return Convert.ToUInt32(Math.Min(spectralValue, (1UL << radiometricResolution) - 1));
        }

        #endregion

        #region Threshold

        /// <summary>
        /// Compute the Otsu threshold.
        /// </summary>
        /// <param name="histogramValues">The histogram values of the image.</param>
        /// <returns>The Otsu threshold computed from the histogram values.</returns>
        /// <exception cref="System.ArgumentNullException">The list of histogram values is null.</exception>
        /// <exception cref="System.ArgumentException">The list of histogram values is empty.</exception>
        public static Double ComputeOtsuThreshold(IList<Int32> histogramValues)
        {
            if (histogramValues == null)
                throw new ArgumentNullException("histogramValues", "The list of histogram values is null.");
            if (histogramValues.Count == 0)
                throw new ArgumentException("The list of histogram values is empty.", "histogramValues");

            // sum the pixel values according to histogram values with relation to intensity
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

        /// <summary>
        /// Computes the balance point of a histogram.
        /// </summary>
        /// <param name="histogramValues">The histogram values of the image.</param>
        /// <returns>The balance point of the histogram values.</returns>
        /// <exception cref="System.ArgumentNullException">The list of histogram values is null.</exception>
        /// <exception cref="System.ArgumentException">The list of histogram values is empty.</exception>
        public static Double ComputeHistogramBalance(IList<Int32> histogramValues)
        {
            if (histogramValues == null)
                throw new ArgumentNullException("histogramValues", "The list of histogram values is null.");
            if (histogramValues.Count == 0)
                throw new ArgumentException("The list of histogram values is empty.", "histogramValues");

            Int32 leftBoundary = 0, rightBoundary = histogramValues.Count - 1;

            // adjust the boundaries
            while (leftBoundary < rightBoundary && histogramValues[leftBoundary] == 0)
                leftBoundary++;

            while (rightBoundary > leftBoundary && histogramValues[rightBoundary] == 0)
                rightBoundary--;

            Int32 balance = (leftBoundary + rightBoundary) / 2;

            // define the starting weights
            Int32 leftWeight = 0, rightWeight = 0;

            for (Int32 i = 0; i < histogramValues.Count; i++)
            {
                if (i <= balance)
                    leftWeight += histogramValues[i];
                if (i >= balance)
                    rightWeight += histogramValues[i];
            }

            while (leftBoundary < rightBoundary)
            {
                if (leftWeight < rightWeight) // right side is heavier
                {
                    rightWeight -= histogramValues[rightBoundary];
                    rightBoundary--;

                    if ((leftBoundary + rightBoundary) / 2 < balance)
                    {
                        rightWeight += histogramValues[balance];
                        leftWeight -= histogramValues[balance];
                        balance--;
                    }
                }
                else // left side is heavier
                {
                    leftWeight -= histogramValues[leftBoundary];
                    leftBoundary++;
                    if ((leftBoundary + rightBoundary) / 2 > balance)
                    {
                        leftWeight += histogramValues[balance];
                        rightWeight -= histogramValues[balance];
                        balance++;
                    }
                }
            }

            return balance;
        }

        #endregion

        #region Dimension matching

        /// <summary>
        /// Determines whether the dimension and location of the specified rasters match.
        /// </summary>
        /// <param name="first">The first raster.</param>
        /// <param name="second">The second raster.</param>
        /// <returns><c>true</c> if the specified rasters have matching dimensions and location; otherwise, <c>false</c>.</returns>
        public static Boolean IsMatching(IRaster first, IRaster second)
        {
            return (!first.IsMapped || !second.IsMapped ||
                     first.Mapper.Equals(second.Mapper)) &&
                    first.NumberOfRows == second.NumberOfRows &&
                    first.NumberOfColumns == second.NumberOfColumns;
        }

        #endregion
    }
}
