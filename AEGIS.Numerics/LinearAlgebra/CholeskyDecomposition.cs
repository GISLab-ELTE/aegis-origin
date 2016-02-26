/// <copyright file="CholeskyDecomposition.cs" company="Eötvös Loránd University (ELTE)">
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
/// <authors>Bence Tomcsik, Roberto Giachetta</authors>

using System;

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    /// <summary>
    /// Represents a type for Cholesky decomposition of <see cref="Matrix" /> instances.
    /// </summary>
    public class CholeskyDecomposition
    {
        #region Private fields

        private Matrix _matrix; // the original matrix
        private Matrix _l; // the generated L matrix
        private Matrix _transposedL; // the generated transposed L matrix

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the L (lower triangular) matrix.
        /// </summary>
        public Matrix L
        {
            get
            {
                if (_l == null)
                    Compute();
                return _l;
            }
        }

        /// <summary>
        /// Gets the transposed L (lower triangular) matrix.
        /// </summary>
        public Matrix LT
        {
            get
            {
                if (_transposedL == null)
                    Compute();
                return _transposedL;
            }
        }

        /// <summary>
        /// Gets the LLt matrix.
        /// </summary>
        public Matrix LLT
        {
            get
            {
                if (_l == null || _transposedL == null)
                    Compute();
                return _l * _transposedL;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CholeskyDecomposition" /> class.
        /// </summary>
        /// <param name="matrix">The matrix of which the decomposition is computed.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public CholeskyDecomposition(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");
            if (!Matrix.IsSymmetric(matrix))
                throw new ArgumentException("matrix", "The matrix is not symmetric");

            _matrix = matrix;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Perform computation.
        /// </summary>
        public void Compute()
        {
            if (_l != null && _transposedL != null)
                return;

            Compute(_matrix, out _l, out _transposedL);
        }

        /// <summary>
        /// Perform computation.
        /// </summary>
        /// <param name="numberOfIterations">The number of iterations.</param>
        public void Compute(Int32 numberOfIterations)
        {
            Matrix matrix = _matrix;
            for (Int32 i = 0; i < numberOfIterations; i++)
            {
                Compute(matrix, out _l, out _transposedL);
                matrix = _l * _transposedL;
            }
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Computes the Cholesky decomposition.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="l">The L matrix.</param>
        /// <param name="lt">The L transposed matrix.</param>
        private static void Compute(Matrix matrix, out Matrix l, out Matrix lt)
        {
            l = new Matrix(matrix.NumberOfRows, matrix.NumberOfRows);
            Double sum;

            for (Int32 i = 0; i < matrix.NumberOfRows; i++)
            {
                for (Int32 j = 0; j <= i; j++)
                {
                    sum = 0;

                    for (Int32 k = 0; k < j; k++)
                    {
                        sum += (l[i, k] * l[j, k]);
                    }

                    if (i == j)
                    {
                        l[i, i] = Math.Sqrt(matrix[i, i] - sum);
                    }
                    else
                    {
                        l[i, j] = (1 / l[j, j]) * (matrix[i, j] - sum);
                    }
                }
            }

            lt = l.Transpone();
        }

        #endregion
    }
}
