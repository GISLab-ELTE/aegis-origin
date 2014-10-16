/// <copyright file="HouseholderTransformation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
    /// Represents a type perfoming the Householder Transformation of <see cref="Vector" /> instances.
    /// </summary>
    public class HouseholderTransformation
    {
        #region Private fields

        private Double[] _vector; // original vector
        private Int32 _size; // the size of the vector
        private Matrix _h; // the Householder transformed matrix

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the Householder transform.
        /// </summary>
        /// <value>The Householder transform matrix.</value>
        public Matrix H 
        {
            get 
            {
                if (_h == null)
                    Compute();
                return _h;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HouseholderTransformation" /> class.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public HouseholderTransformation(Double[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");

            _vector = vector;
            _size = vector.Length;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HouseholderTransformation" /> class.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public HouseholderTransformation(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");

            _vector = vector.Values;
            _size = vector.Size;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Perform computation.
        /// </summary>
        public void Compute()
        {
            // source: Rosetta Code, http://rosettacode.org/wiki/QR_decomposition

            Vector u = new Vector(_vector);
            u[0] -= Math.Sign(_vector[0]) * Norm.ComputePNorm(_vector, 2);
            Vector v = u / Norm.ComputePNorm(u, 2);
            Matrix vTranspone = v.Transpone();

            _h = MatrixFactory.CreateIdentity(_size) - 2 / (vTranspone * v)[0] * (v * vTranspone);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Transforms the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Householder matrix of the <paramref name="vector" />.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Matrix Transform(Vector vector)
        {
            HouseholderTransformation transformation = new HouseholderTransformation(vector);
            transformation.Compute();
            return transformation.H;
        }

        /// <summary>
        /// Transforms the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Householder matrix of the <paramref name="vector" />.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Matrix Transform(Double[] vector)
        {
            HouseholderTransformation transformation = new HouseholderTransformation(vector);
            transformation.Compute();
            return transformation.H;
        }

        /// <summary>
        /// Tridiagonalizes the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The tridiagonalization of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The matrix is not square.
        /// or
        /// The matrix is not symmetric.
        /// </exception>
        public static Matrix Tridiagonalize(Matrix matrix)
        {
            // source: Wikipedia, http://en.wikipedia.org/wiki/Householder_transformation

            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");
            if (!matrix.IsSquare)
                throw new ArgumentException("The matrix is not square.", "matrix");
            if (!Matrix.IsSymmetric(matrix))
                throw new ArgumentException("The matrix is not symmetric.", "matrix");

            Matrix tridiagonalMatrix = new Matrix(matrix);

            for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns - 2; columnIndex++)
            {
                Double[] column = tridiagonalMatrix.GetColumn(columnIndex);

                Double sum = 0;
                for (Int32 j = columnIndex + 1; j < column.Length; j++)
                    sum += column[j] * column[j];

                Double alpha = -Math.Sign(column[columnIndex + 1]) * Math.Sqrt(sum);
                Double r = Math.Sqrt(0.5 * (alpha * alpha - column[columnIndex + 1] * alpha));

                Vector v = new Vector(column.Length);
                v[columnIndex + 1] = (column[columnIndex + 1] - alpha) / 2 / r;
                for (Int32 j = columnIndex + 2; j < column.Length; j++)
                    v[j] = column[j] / 2 / r;

                Matrix p = MatrixFactory.CreateIdentity(column.Length) - 2 * v * v.Transpone();

                tridiagonalMatrix = p * tridiagonalMatrix * p;
            }

            return tridiagonalMatrix;
        }

        #endregion
    }
}
