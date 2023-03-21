// <copyright file="FilterFactory.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using System;

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represents a factory for creating common filters.
    /// </summary>
    public static class FilterFactory
    {
        #region Public static methods

        /// <summary>
        /// Creates a box filter.
        /// </summary>
        /// <param name="radius">The size of the filter.</param>
        /// <returns>The produced box filter.</returns>
        public static Filter CreateBoxFilter(Int32 radius)
        {
            if (radius < 1)
                throw new ArgumentOutOfRangeException("radius", "The radius is not positive.");

            Int32 size = radius * 2 + 1;

            return new Filter(new Matrix(size, size, 1), size * size, 0);
        }

        /// <summary>
        /// Creates a Gabor filter.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="sigma">The sigma.</param>
        /// <param name="waveLength">The wavelength.</param>
        /// <param name="orientation">The orientation (in degrees).</param>
        /// <param name="phaseOffset">The phase offset (in degrees).</param>
        /// <param name="spatialAspectRatio">The spatial aspect ratio.</param>
        /// <returns>The produced Gabor filter.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The radius is not positive.
        /// or
        /// Standard deviation is less than or equal to 0.
        /// or
        /// Wavelength is less than of equal to 2.
        /// or
        /// The wavelength is 2 and the phase offset is -90 which results the Gabor filter sampled in it's zero crossings.
        /// or
        /// The wavelength is 2 and the phase offset is 90 which results the Gabor filter sampled in it's zero crossings.
        /// or
        /// Orientation is less than 0.
        /// or
        /// Orientation is greater than 360.
        /// or
        /// Phase offset is less than -180.
        /// or
        /// Phase offset is greater than 180.
        /// </exception>
        public static Filter CreateGaborFilter(Int32 radius, Double sigma, Double waveLength, Double orientation, Double phaseOffset, Double spatialAspectRatio)
        {
            if (radius < 1)
                throw new ArgumentOutOfRangeException("radius", "The radius is not positive.");
            if (sigma <= 0)
                throw new ArgumentOutOfRangeException("sigma", "Standard deviation is less than or equal to 0.");
            if (waveLength < 2.0)
                throw new ArgumentOutOfRangeException("wavelength", "Wavelength is less than of equal to 2.");
            if (waveLength.Equals(2.0) && phaseOffset == -90)
                throw new ArgumentOutOfRangeException("phaseOffset", "The wavelength is 2 and the phase offset is -90 which results the Gabor filter sampled in it's zero crossings.");
            if (waveLength.Equals(2.0) && phaseOffset == 90)
                throw new ArgumentOutOfRangeException("phaseOffset", "The wavelength is 2 and the phase offset is 90 which results the Gabor filter sampled in it's zero crossings.");
            if (orientation < 0)
                throw new ArgumentOutOfRangeException("orientation", "Orientation is less than 0.");
            if (orientation > 360)
                throw new ArgumentOutOfRangeException("orientation", "Orientation is greater than 360.");
            if (phaseOffset < -180)
                throw new ArgumentOutOfRangeException("phaseOffset", "Phase offset is less than -180.");
            if (phaseOffset > 180)
                throw new ArgumentOutOfRangeException("phaseOffset", "Phase offset is greater than 180.");

            Matrix matrix = new Matrix(radius * 2 + 1, radius * 2 + 1);

            Double sum = 0;

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    double xTheta = columnIndex * Math.Cos(orientation) + rowIndex * Math.Sin(orientation);
                    double yTheta = -columnIndex * Math.Sin(orientation) + rowIndex * Math.Cos(orientation);
                    double harmonicFunction = CalculateHarmonicFunction(xTheta, yTheta, spatialAspectRatio, sigma);
                    double gaussianFunction = CalculateGaussianFunction(xTheta, waveLength, phaseOffset);
                    matrix[rowIndex, columnIndex] = Math.Exp(harmonicFunction) * Math.Cos(gaussianFunction);

                    sum += matrix[rowIndex, columnIndex];
                }

            return new Filter(matrix, sum, 0);
        }

        /// <summary>
        /// Creates a Gaussian filter.
        /// </summary>
        /// <param name="radius">The radius of the filter.</param>
        /// <returns>The produced Gaussian filter.</returns>
        public static Filter CreateGaussianFilter(Int32 radius)
        {
            return CreateGaussianFilter(radius, 1);
        }

        /// <summary>
        /// Creates a Gaussian filter.
        /// </summary>
        /// <param name="radius">The radius of the filter.</param>
        /// <param name="sigma">The standard deviation of the Gaussian distribution.</param>
        /// <returns>The produced Gaussian filter.</returns>
        public static Filter CreateGaussianFilter(Int32 radius, Double sigma)
        {
            // source: http://en.wikipedia.org/wiki/Gaussian_blur

            if (radius < 1)
                throw new ArgumentOutOfRangeException("radius", "The radius is not positive.");

            Matrix matrix = new Matrix(radius * 2 + 1, radius * 2 + 1);

            Double sum = 0;

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    matrix[rowIndex, columnIndex] = Gaussian(rowIndex - radius, sigma) * Gaussian(columnIndex - radius, sigma);

                    sum += matrix[rowIndex, columnIndex];
                }

            return new Filter(matrix, sum, 0);
        }

        /// <summary>
        /// Creates the discrete laplace filter.
        /// </summary>
        /// <returns>The created laplace filter.</returns>
        public static Filter CreateDiscreteLaplaceFilter()
        {
            Matrix matrix = new Matrix(3, 3, 1f);
            matrix[0, 0] = matrix[2, 2] = matrix[2, 0] = matrix[0, 2] = 0.5f;
            matrix[1, 1] = -6;

            return new Filter(matrix, 1, 0);
        }

        /// <summary>
        /// Creates a mean removal filter.
        /// </summary>
        /// <param name="weight">The weight of the mean value.</param>
        /// <returns>The produced mean removal filter.</returns>
        public static Filter CreateMeanRemovalFilter(Double weight)
        {
            Matrix matrix = new Matrix(3, 3, -1);
            matrix[1, 1] = weight;

            return new Filter(matrix, weight - 8, 0);
        }

        /// <summary>
        /// Creates a horizontal Prewitt filter.
        /// </summary>
        /// <returns>The produced Prewitt filter.</returns>
        public static Filter CreatePrewittHorizontalFilter()
        {
            Matrix matrix = new Matrix(3, 3);
            matrix[0, 0] = matrix[1, 0] = matrix[2, 0] = -1;
            matrix[0, 2] = matrix[1, 2] = matrix[2, 2] = 1;

            return new Filter(matrix, 1, 0);
        }

        /// <summary>
        /// Creates a vertical Prewitt filter.
        /// </summary>
        /// <returns>The produced Prewitt filter.</returns>
        public static Filter CreatePrewittVerticalFilter()
        {
            Matrix matrix = new Matrix(3, 3);
            matrix[0, 0] = matrix[0, 1] = matrix[0, 2] = -1;
            matrix[2, 0] = matrix[2, 1] = matrix[2, 2] = 1;

            return new Filter(matrix, 1, 0);
        }


        /// <summary>
        /// Creates a horizontal Roberts operator filter.
        /// </summary>
        /// <returns>The produced Roberts filter operator.</returns>
        public static Filter CreateRobertsHorizontalFilter()
        {
            Matrix matrix = new Matrix(3, 3, 0);
            matrix[0, 0] = 1;
            matrix[2, 2] = -1;

            return new Filter(matrix, 1, 0);
        }

        /// <summary>
        /// Creates a vertical Roberts Operator Filter.
        /// </summary>
        /// <returns>The produced Roberts filter operator.</returns>
        public static Filter CreateRobertsVerticalFilter()
        {
            Matrix matrix = new Matrix(3, 3, 0);
            matrix[0, 2] = 1;
            matrix[2, 0] = -1;

            return new Filter(matrix, 1, 0);
        }

        /// <summary>
        /// Creates a horizontal Scharr filter.
        /// </summary>
        /// <returns>The produced Scharr filter.</returns>
        public static Filter CreateScharrHorizontalFilter()
        {
            Matrix matrix = new Matrix(3, 3);
            matrix[0, 0] = matrix[2, 0] = 3;
            matrix[1, 0] = 10;
            matrix[0, 2] = matrix[2, 2] = -3;
            matrix[1, 2] = -10;

            return new Filter(matrix, 1, 0);
        }

        /// <summary>
        /// Creates a vertical Scharr filter.
        /// </summary>
        /// <returns>The produced Scharr filter.</returns>
        public static Filter CreateScharrVerticalFilter()
        {
            Matrix matrix = new Matrix(3, 3);
            matrix[0, 0] = matrix[0, 2] = 3;
            matrix[0, 1] = 10;
            matrix[2, 0] = matrix[2, 2] = -3;
            matrix[2, 1] = -10;

            return new Filter(matrix, 1, 0);
        }

        /// <summary>
        /// Creates a horizontal Sobel filter.
        /// </summary>
        /// <returns>The produced Sobel filter.</returns>
        public static Filter CreateSobelHorizontalFilter()
        {
            Matrix matrix = new Matrix(3, 3);
            matrix[0, 0] = matrix[2, 0] = -1;
            matrix[1, 0] = -2;
            matrix[0, 2] = matrix[2, 2] = 1;
            matrix[1, 2] = 2;

            return new Filter(matrix, 1, 0);
        }

        /// <summary>
        /// Creates a vertical Sobel filter.
        /// </summary>
        /// <returns>The produced Sobel filter.</returns>
        public static Filter CreateSobelVerticalFilter()
        {
            Matrix matrix = new Matrix(3, 3);
            matrix[0, 0] = matrix[0, 2] = 1;
            matrix[0, 1] = 2;
            matrix[2, 0] = matrix[2, 2] = -1;
            matrix[2, 1] = -2;

            return new Filter(matrix, 1, 0);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Returns the Gaussian function value for the variable.
        /// </summary>
        /// <param name="x">The variable.</param>
        /// <param name="sigma">The standard deviation of the Gaussian distribution.</param>
        /// <returns>The Gaussian value for the variable.</returns>
        private static Double Gaussian(Double x, Double sigma)
        {
            return 1 / Math.Sqrt(2 * Math.PI) / sigma * Math.Exp(-x * x / (2 * sigma * sigma));
        }

        /// <summary>
        /// Calculate the harmonic function component of the convolution.
        /// </summary>
        /// <returns>The produced harmonic function.</returns>
        private static Double CalculateHarmonicFunction(Double xTheta, Double yTheta, Double spatialAspectRatio, Double standardDeviation)
        {
            return -(Math.Pow(xTheta, 2) + Math.Pow(spatialAspectRatio, 2) * Math.Pow(yTheta, 2)) / (2 * Math.Pow(standardDeviation, 2));
        }

        /// <summary>
        /// Calculate the Gaussian function component of the convolution.
        /// </summary>
        /// <returns>The produced Gaussian function.</returns>
        private static Double CalculateGaussianFunction(Double xTheta, Double waveLength, Double phaseOffset)
        {
            return 2 * Math.PI * (xTheta / waveLength) + phaseOffset;
        }

        #endregion
    }
}
