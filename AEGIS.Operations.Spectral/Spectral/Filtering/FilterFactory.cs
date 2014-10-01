/// <copyright file="FilterFactory.cs" company="Eötvös Loránd University (ELTE)">
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
                throw new ArgumentOutOfRangeException("size", "The size is not positive.");

            Int32 size = radius * 2 + 1;

            return new Filter(new Matrix(size, size, 1), size * size, 0);
        }

        /// <summary>
        /// Creates a gaussian filter.
        /// </summary>
        /// <param name="radius">The radius of the filter.</param>
        /// <returns>The produced gaussian filter.</returns>
        public static Filter CreateGaussianFilter(Int32 radius)
        {
            return CreateGaussianFilter(radius, 1);
        }

        /// <summary>
        /// Creates a gaussian filter.
        /// </summary>
        /// <param name="radius">The radius of the filter.</param>
        /// <param name="sigma">The standard deviation of the Gaussian distribution.</param>
        /// <returns>The produced gaussian filter.</returns>
        public static Filter CreateGaussianFilter(Int32 radius, Double sigma)
        {
            // source: http://en.wikipedia.org/wiki/Gaussian_blur

            if (radius < 1)
                throw new ArgumentOutOfRangeException("size", "The size is not positive.");

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
            return 1 / Math.Sqrt(2 * Math.PI * sigma * sigma) * Math.Exp(-x * x / (2 * sigma * sigma));
        }

        #endregion
    }
}
