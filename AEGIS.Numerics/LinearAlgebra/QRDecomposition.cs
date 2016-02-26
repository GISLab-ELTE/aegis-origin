/// <copyright file="QRDecomposition.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    /// <summary>
    /// Represents a type for QR decomposition of <see cref="Matrix" /> instances.
    /// </summary>
    public class QRDecomposition
    {
        #region Private fields

        private Matrix _matrix; // original matrix
        private Int32 _numberOfRows; // the number of rows in the original matrix
        private Int32 _numberOfColumns; // the number of columns in the original matrix
        private Matrix _q; // the generated Q matrix
        private Matrix _r; // the generated R matrix

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the Q (orthogonal) matrix.
        /// </summary>
        /// <value>The Q matrix.</value>
        public Matrix Q
        {
            get
            {
                if (_q == null)
                    Compute();
                return _q;
            }
        }

        /// <summary>
        /// Gets the R (upper triangular) matrix.
        /// </summary>
        /// <value>The R matrix.</value>
        public Matrix R
        {
            get
            {
                if (_r == null)
                    Compute();
                return _r;
            }
        }

        /// <summary>
        /// Gets the QR matrix.
        /// </summary>
        /// <value>The QR matrix.</value>
        public Matrix QR
        {
            get
            {
                if (_q == null || _r == null)
                    Compute();
                return _q * _r;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QRDecomposition" /> class.
        /// </summary>
        /// <param name="matrix">The matrix of which the decomposition is computed.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public QRDecomposition(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");
            _matrix = matrix;
            _numberOfRows = matrix.NumberOfRows;
            _numberOfColumns = matrix.NumberOfColumns;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Perform computation.
        /// </summary>
        public void Compute()
        {
            if (_q != null && _r != null)
                return;

            Compute(_matrix, out _q, out _r);
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
                Compute(matrix, out _q, out _r);
                matrix = _q * _r;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the QR decomposition.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="q">The Q matrix.</param>
        /// <param name="r">The R matrix.</param>
        private static void Compute(Matrix matrix, out Matrix q, out Matrix r)
        {
            // source: Rosetta Code, http://rosettacode.org/wiki/QR_decomposition

            q = MatrixFactory.CreateIdentity(matrix.NumberOfRows);
            r = new Matrix(matrix);

            for (Int32 i = 0; i < matrix.NumberOfRows - 1 && i < matrix.NumberOfColumns; i++)
            {
                Vector v = new Vector(matrix.NumberOfRows - i);

                for (Int32 j = i; j < matrix.NumberOfRows; j++)
                    v[j - i] = r[j, i];

                Matrix h = HouseholderTransformation.Transform(v);

                Matrix hFull = MatrixFactory.CreateIdentity(matrix.NumberOfRows);
                for (Int32 j = i; j < matrix.NumberOfRows; j++)
                    for (Int32 k = i; k < matrix.NumberOfColumns; k++)
                        hFull[j, k] = h[j - i, k - i];

                q = q * hFull;
                r = hFull * r;
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Computes the eigenvalues of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The array containing the eigenvalues of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Double[] ComputeEigenvalues(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            Double[] eigenValues = new Double[matrix.NumberOfRows];

            QRDecomposition decomposition = new QRDecomposition(matrix);
            decomposition.Compute(10);

            for (int i = 0; i < matrix.NumberOfRows; ++i)
                eigenValues[i] = decomposition.QR[i, i];
            return eigenValues;
        }

        /// <summary>
        /// Computes the eigenvectors of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The array containing the eigenvectors of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Vector[] ComputeEigenvectors(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            Vector[] eigenVectors = new Vector[matrix.NumberOfRows];

            QRDecomposition decomposition = new QRDecomposition(matrix);
            decomposition.Compute(2);

            for (int i = 0; i < matrix.NumberOfRows; ++i)
            {
                eigenVectors[i] = new Vector(matrix.NumberOfColumns);
                for (int j = 0; j < matrix.NumberOfColumns; ++j)
                    eigenVectors[i][j] = decomposition.Q[i, j];
            }

            return eigenVectors;
        }

        #endregion
    }
}
