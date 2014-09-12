/// <copyright file="LUDecomposition.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    /// <summary>
    /// Represents a type perfoming the LU decomposition of <see cref="Matrix" /> instances.
    /// </summary>
    public class LUDecomposition
    {
        #region Private fields

        private Matrix _matrix; // original matrix
        private Int32 _numberOfRows; // the number of rows in the original matrix
        private Int32 _numberOfColumns; // the number of columns in the original matrix
        private Double? _determinant; // the determinant of the original matrix
        private Int32[] _permutationArray; // the permutation array for generating the pivot matrix 
        private Int32 _numberOfPermutations; // permutációk száma a determináns számításához
        private Matrix _p; // the generation pivot permutation matrix
        private Matrix _l; // the generated L matrix
        private Matrix _u; // the generated U matrix

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the L (lower triangular) matrix.
        /// </summary>
        /// <value>The L matrix.</value>
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
        /// Gets the U (upper triangular) matrix.
        /// </summary>
        /// <value>The U matrix.</value>
        public Matrix U 
        { 
            get 
            {
                if (_u == null)
                    Compute();
                return _u; 
            } 
        }

        /// <summary>
        /// Gets the LU (the lower and upper triangles integrated) matrix.
        /// </summary>
        /// <value>The LU matrix.</value>
        public Matrix LU
        {
            get
            {
                if (_l == null || _u == null)
                    Compute();
                return _l * _u;   
            }
        }

        /// <summary>
        /// Gets the P (pivot permutation) matrix.
        /// </summary>
        /// <value>The P matrix.</value>
        public Matrix P
        {
            get 
            {
                if (_p == null)
                    ComputePivotPermutationMatrix();

                return _p;
            }
        }

        /// <summary>
        /// Gets the determinant of the matrix.
        /// </summary>
        /// <value>The determinant of the matrix.</value>
        /// <exception cref="System.InvalidOperationException">The matrix is not square.</exception>
        public Double Determinant
        { 
            get
            {
                if (_numberOfColumns != _numberOfRows)
                    throw new InvalidOperationException("The matrix is not square.");
                if (_determinant == null)
                    ComputeDeterminant();

                return _determinant.Value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LUDecomposition" /> class.
        /// </summary>
        /// <param name="matrix">The matrix of which the decompomposition is computed.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public LUDecomposition(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");
            if (!matrix.IsSquare)
                throw new ArgumentException("The matrix is not square.", "matrix");

            _matrix = matrix;
            _numberOfRows = matrix.NumberOfRows;
            _numberOfColumns = matrix.NumberOfColumns;
            _determinant = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Perform computation.
        /// </summary>
        public void Compute()
        {
            // source: Rosetta Code, http://rosettacode.org/wiki/LU_decomposition

            if (_l != null && _u != null)
                return;

            Matrix pivotMatrix = Pivotize();

            _l = MatrixFactory.CreateIdentity(_numberOfRows, Math.Min(_numberOfRows, _numberOfColumns));
            _u = new Matrix(Math.Min(_numberOfRows, _numberOfColumns), _numberOfColumns);

            for (Int32 i = 0; i < _numberOfRows; i++)
                for (Int32 j = 0; j < _numberOfColumns; j++)
                {
                    Double sum;
                    if (j <= i)
                    {
                        sum = 0;
                        for (Int32 k = 0; k < j; k++)
                        {
                            sum += _l[j, k] * _u[k, i];
                        }
                        _u[j, i] = pivotMatrix[j, i] - sum;
                    }
                    if (j >= i)
                    {
                        sum = 0;
                        for (Int32 k = 0; k < i; k++)
                        {
                            sum += _l[j, k] * _u[k, i];
                        }
                        if (_u[i, i] == 0)
                            _l[j, i] = (pivotMatrix[j, i] - sum);
                        else
                            _l[j, i] = (pivotMatrix[j, i] - sum) / _u[i, i];
                    }
                }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Pivotizes the matrix.
        /// </summary>
        /// <returns>The pivot matrix.</returns>
        private Matrix Pivotize()
        {
            Matrix pivotMatrix = MatrixFactory.CreateIdentity(_numberOfRows, _numberOfColumns);

            ComputePivotPermutationArray();

            for (Int32 i = 0; i < _numberOfRows; i++)
            {
                for (Int32 j = 0; j < _numberOfColumns; j++)
                    pivotMatrix[i, j] = _matrix[_permutationArray[i], j];
            }
            return pivotMatrix;
        }

        /// <summary>
        /// Computes the pivot permutation array for the matrix.
        /// </summary>
        private void ComputePivotPermutationArray()
        {
            if (_permutationArray != null)
                return;

            // first we assume that all rows are in corrent order
            _permutationArray = Enumerable.Range(0, _numberOfRows).ToArray();
            _numberOfPermutations = 0;

            for (Int32 i = 0; i < _numberOfColumns; i++)
            {
                Int32 maxJ = i; // max search in the (partial) row
                for (Int32 j = i; j < _numberOfRows; j++)
                {
                    if (Math.Abs(_matrix[j, i]) > Math.Abs(_matrix[maxJ, i]))
                        maxJ = j;
                }
                if (maxJ != i) // if the row is not in the correct order
                {
                    _numberOfPermutations++;
                    Int32 temp = _permutationArray[i];
                    _permutationArray[i] = _permutationArray[maxJ];
                    _permutationArray[maxJ] = temp;
                }
            }
        }

        /// <summary>
        /// Computes the pivot permutation matrix.
        /// </summary>
        private void ComputePivotPermutationMatrix()
        {
            if (_p != null)
                return;

            ComputePivotPermutationArray();

            _p = new Matrix(_permutationArray.Length, _permutationArray.Length);

            for (Int32 i = 0; i < _numberOfRows; i++)
                _p[i, _permutationArray[i]] = 1;
        }

        /// <summary>
        /// Computes the determinant of the matrix.
        /// </summary>
        private void ComputeDeterminant()
        {
            if (_determinant != null)
                return;
            
            _determinant = (_numberOfPermutations % 2 == 0) ? 1 : (-1);
            for (Int32 i = 0; i < _numberOfRows; i++)
                _determinant *= _u[i, i];
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Decomposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public static Matrix Decompose(Matrix matrix)
        {
            LUDecomposition luDecomposition = new LUDecomposition(matrix);
            luDecomposition.Compute();
            return luDecomposition.LU;
        }


        /// <summary>
        /// Decomposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix to decompose.</param>
        /// <param name="l">The L (lower triangular) matrix.</param>
        /// <param name="u">The U (upper triangular) matrix.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public static void Decompose(Matrix matrix, out Matrix l, out Matrix u)
        {
            LUDecomposition luDecomposition = new LUDecomposition(matrix);
            luDecomposition.Compute();
            l = luDecomposition.L;
            u = luDecomposition.U;
        }

        /// <summary>
        /// Computes the determinant of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The determinant of the specified matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public static Double ComputeDeterminant(Matrix matrix)
        {
            LUDecomposition luDecomposition = new LUDecomposition(matrix);
            luDecomposition.Compute();
            return luDecomposition.Determinant;
        }

        /// <summary>
        /// Inverts the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public static Matrix Invert(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            LUDecomposition luDecomposition = new LUDecomposition(matrix);
            luDecomposition.Compute();

            Matrix inverse = new Matrix(matrix.NumberOfColumns, matrix.NumberOfRows);

            for (int i = 0; i < matrix.NumberOfRows; ++i)
            {
                Vector b = new Vector(matrix.NumberOfColumns, 0);
                b[i] = 1;       // identity vector

                Vector y = SolveEquation(luDecomposition, b);
                
                for (int j = 0; j < inverse.NumberOfRows; ++j)
                {
                    inverse[j, i] = y[j];
                }
            }
            return inverse;
        }

        /// <summary>
        /// Solves a linear equation system.
        /// </summary>
        /// <param name="a">The left side of the equation represented by a matrix.</param>
        /// <param name="b">The right side of the equation represented by a vector.</param>
        /// <returns>The vector containing the unknown variables of the equation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The matrix is null.
        /// or
        /// The vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The matrix is not square.
        /// or
        /// The size of the matrix dow not match the size fo the vector.
        /// </exception>
        public static Vector SolveEquation(Matrix a, Vector b)
        {
            if (a == null)
                throw new ArgumentNullException("a", "The matrix is null.");
            if (b == null)
                throw new ArgumentNullException("b", "The vector is null.");
            if (!a.IsSquare)
                throw new ArgumentException("The matrix is not square.", "a");
            if (a.NumberOfRows != b.Size)
                throw new ArgumentException("The size of the matrix dow not match the size fo the vector.", "b");

            LUDecomposition luDecomposition = new LUDecomposition(a);
            luDecomposition.Compute();
            return SolveEquation(luDecomposition, b);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Solves a linear equation system.
        /// </summary>
        /// <param name="luDecomposition">The LU decomposition.</param>
        /// <param name="b">The vector.</param>
        /// <returns>The vector containing the unknown variables of the equation.</returns>
        private static Vector SolveEquation(LUDecomposition luDecomposition, Vector b)
        {
            Vector pb = new Vector(b.Size);

            // P*b
            for (int i = 0; i < b.Size; ++i)
            {
                double s = 0;
                for (int j = 0; j < luDecomposition.P.NumberOfColumns; ++j)
                {
                    s += luDecomposition.P[i, j] * b[j];
                }
                pb[i] = s;
            }

            // L*y = P*b with forward substitution
            Vector y = new Vector(pb.Size);
            for (int i = 0; i < luDecomposition.L.NumberOfRows; ++i)
            {
                y[i] = pb[i];
                for (int j = 0; j < i; ++j)
                {
                    y[i] -= luDecomposition.L[i, j] * y[j];
                }
                y[i] /= luDecomposition.L[i, i] == 0 ? 1 : luDecomposition.L[i, i];
            }

            // U*x = y with back substitution
            Vector x = new Vector(y.Size);
            for (int i = x.Size - 1; i >= 0; --i)
            {
                x[i] = y[i];
                for (int j = i + 1; j < luDecomposition.U.NumberOfRows; ++j)
                {
                    x[i] -= luDecomposition.U[i, j] * x[j];
                }

                x[i] /= (luDecomposition.U[i, i] == 0 ? 1 : luDecomposition.U[i, i]);
            }

            return x;
        }

        #endregion
    }
}
