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
        /// The radiometric resolution is less than 1.;radiometricResolution
        /// or
        /// The radiometric resolution is geater than 64.;radiometricResolution
        /// </exception>
        public static UInt64 RadiometricResolutionMax(Int32 radiometricResolution)
        {
            if (radiometricResolution < 1)
                throw new ArgumentException("The radiometric resolution is less than 1.", "radiometricResolution");
            if (radiometricResolution > 64)
                throw new ArgumentException("The radiometric resolution is geater than 64.", "radiometricResolution");

            if (radiometricResolution == 64)
                return UInt64.MaxValue;

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
    }
}
