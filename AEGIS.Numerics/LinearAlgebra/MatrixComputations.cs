/// <copyright file="MatrixComputations.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    /// <summary>
    /// Defines additional computation methods for <see cref="Matrix" /> instances.
    /// </summary>
    public static class MatrixComputations
    {
        #region Matrix properties

        /// <summary>
        /// Computes the determinant of the matrix.
        /// </summary>
        /// <value>The determinant of the matrix.</value>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Double ComputeDeterminant(this Matrix matrix)
        {
            return LUDecomposition.ComputeDeterminant(matrix);
        }

        /// <summary>
        /// Computes the definiteness of the matrix.
        /// </summary>
        /// <value>The definiteness of the matrix.</value>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static MatrixDefiniteness ComputeDefiniteness(this Matrix matrix) 
        {
            Double[] eigenValues = QRDecomposition.ComputeEigenvalues(matrix); 
            Int32 posValues = 0;
            Int32 negValues = 0;
            Int32 zeroValues = 0;
            for (Int32 i = 0; i < eigenValues.Length; ++i)
            {
                if (eigenValues[i] > 0)
                    posValues++;
                else if (eigenValues[i] < 0)
                    negValues++;
                else
                    zeroValues++;
            }

            if (posValues > 0 && negValues == 0 && zeroValues == 0)
                return MatrixDefiniteness.PositiveDefinite;
            else if (negValues > 0 && posValues == 0 && zeroValues == 0)
                return MatrixDefiniteness.NegativeDefinite;
            else if (posValues > 0 && zeroValues > 0 && negValues == 0)
                return MatrixDefiniteness.PositiveSemidefinite;
            else if (negValues > 0 && zeroValues > 0 && posValues == 0)
                return MatrixDefiniteness.NegativeSemiDefinite;
            else
                return MatrixDefiniteness.Indefinite;
        }

        /// <summary>
        /// Computes the eigenvalues of the matrix.
        /// </summary>
        /// <value>The eigenvalues of the matrix.</value>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Double[] Eigenvalues(this Matrix matrix)  
        {
            return QRDecomposition.ComputeEigenvalues(matrix); 
        }

        /// <summary>
        /// Computes the eigenvectors of the matrix.
        /// </summary>
        /// <value>The eigenvectors of the matrix.</value>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Vector[] Eigenvectors(this Matrix matrix) 
        {
            return QRDecomposition.ComputeEigenvectors(matrix);
        }

        #endregion

        #region Matrix transformations

        /// <summary>
        /// Transpones the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The transponed matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Matrix Transpone(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            Matrix result = new Matrix(matrix.NumberOfColumns, matrix.NumberOfRows);

            for (Int32 i = 0; i < matrix.NumberOfRows; i++)
                for (Int32 j = 0; j < matrix.NumberOfColumns; j++)
                {
                    result[j, i] = matrix[i, j];
                }

            return result;
        }

        /// <summary>
        /// Inverts the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The inverse matrix.</returns>
        public static Matrix Invert(this Matrix matrix)
        {
            return LUDecomposition.Invert(matrix);
        }

        #endregion
    }
}
