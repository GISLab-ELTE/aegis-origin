/// <copyright file="Norm.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    /// <summary>
    /// Defines methods for computing norms of vectors and matrices.
    /// </summary>
    public static class Norm
    {
        #region Vector norms

        /// <summary>
        /// Computes the p-norm of the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="p">The power of the norm.</param>
        /// <returns>The p-norm of the <paramref name="vector" />.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        /// <exception cref="System.ArgumentException">The value of p is less than 1.</exception>
        public static Double ComputePNorm(Double[] vector, Int32 p)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");
            if (p < 1)
                throw new ArgumentException("The value of p is less than 1.", "p");

            Double sum = 0;
            for (Int32 i = 0; i < vector.Length; i++)
                sum += Calculator.Pow(Math.Abs(vector[i]), p);
            return Math.Pow(sum, 1.0 / p);
        }

        /// <summary>
        /// Computes the p-norm of the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="p">The power of the norm.</param>
        /// <returns>The p-norm of the <paramref name="vector" />.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        /// <exception cref="System.ArgumentException">The value of p is less than 1.</exception>
        public static Double ComputePNorm(Vector vector, Int32 p)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");
            if (p < 1)
                throw new ArgumentException("The value of p is less than 1.", "p");

            Double sum = 0;
            for (Int32 i = 0; i < vector.Size; i++)
                sum += Calculator.Pow(Math.Abs(vector[i]), p);
            return Math.Pow(sum, 1.0 / p);
        }

        #endregion

        #region Matrix norms

        /// <summary>
        /// Computes the p-norm of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="p">The power of the norm.</param>
        /// <returns>The p-norm of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The value of p is less than 1.</exception>
        public static Double ComputePNorm(Matrix matrix, Int32 p)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");
            if (p < 1)
                throw new ArgumentException("The value of p is less than 1.", "p");

            Double sum = 0;
            for (Int32 i = 0; i < matrix.NumberOfRows; i++)
                for (Int32 j = 0; j < matrix.NumberOfColumns; j++)
                    sum += Calculator.Pow(Math.Abs(matrix[i, j]), p);
            return Math.Pow(sum, 1.0 / p);
        }

        #endregion
    }
}
