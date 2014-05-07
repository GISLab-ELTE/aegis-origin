/// <copyright file="Matrix.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Text;
using ELTE.AEGIS.Numerics.LinearAlgebra;

namespace ELTE.AEGIS.Numerics
{
    /// <summary>
    /// Represents a matrix in Euclidean space.
    /// </summary>
    [Serializable]
    public class Matrix
    {
        #region Private fields

        private Double[][] _values; // the values stored by rows

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of rows.</value>
        public Int32 NumberOfRows { get { return _values.Length; } }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>The number of columns.</value>
        public Int32 NumberOfColumns { get { return _values.Length == 0 ? 0 : _values[0].Length; } }

        /// <summary>
        /// Determines whether the matrix is square.
        /// </summary>
        /// <value><c>true</c> if the matrix is square; otherwise, <c>false</c>.</value>
        public Boolean IsSquare { get { return _values.Length == 0 || _values.Length == _values[0].Length; } }

        /// <summary>
        /// Determines whether the matrix is symmetric.
        /// </summary>
        /// <value><c>true</c> if the matrix is square and symmetric; otherwise, <c>false</c>.</value>
        public Boolean IsSymmetric
        {
            get
            {
                if (_values.Length != 0 && _values.Length != _values[0].Length)
                    return false;

                for (Int32 i = 0; i < _values.Length; i++)
                    for (Int32 j = i + 1; j < _values[i].Length; j++)
                    {
                        if (_values[i][j] != _values[j][i])
                            return false;
                    }
                return true;
            }
        }

        /// <summary>
        /// Gets the trace of the matrix.
        /// </summary>
        /// <value>The sum of elements on the main diagonal of the matrix.</value>
        /// <exception cref="System.InvalidOperationException">The matrix must be square to have a trace.</exception>
        public Double Trace 
        {
            get
            {
                if (!IsSquare)
                    throw new InvalidOperationException("The matrix must be square to have a trace.");

                Double trace = 1;
                for (Int32 i = 0; i < _values.Length; i++)
                    for (Int32 j = 0; j < _values[i].Length; j++)
                        trace += _values[i][j];
                return trace;
            }
        }

        /// <summary>
        /// Gets the row located at the specified index.
        /// </summary>
        /// <value>The <see cref="Double[]" /> row values located at the index.</value>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>The <see cref="Double[]" /> row values located at the index.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Row index was outside the bounds of the matrix.</exception>
        public Double[] this[Int32 rowIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex >= _values.Length)
                    throw new IndexOutOfRangeException("Row index was outside the bounds of the matrix.");

                return _values[rowIndex];
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Double" /> value located at the specified row and column indices.
        /// </summary>
        /// <value>The <see cref="Double" /> value located at the specified row and column indices.</value>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The <see cref="Double" /> value at the specified index.</returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Row index was outside the bounds of the matrix.
        /// or
        /// Column index was outside the bounds of the matrix.
        /// </exception>
        public Double this[Int32 rowIndex, Int32 columnIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex >= _values.Length)
                    throw new IndexOutOfRangeException("Row index was outside the bounds of the matrix.");
                if (columnIndex < 0 || columnIndex >= _values[0].Length)
                    throw new IndexOutOfRangeException("Column index was outside the bounds of the matrix.");

                return _values[rowIndex][columnIndex];
            }
            set
            {
                if (rowIndex < 0 || rowIndex >= _values.Length)
                    throw new IndexOutOfRangeException("Row index was outside the bounds of the matrix.");
                if (columnIndex < 0 || columnIndex >= _values[0].Length)
                    throw new IndexOutOfRangeException("Column index was outside the bounds of the matrix.");

                _values[rowIndex][columnIndex] = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public Matrix(Int32 numberOfRows, Int32 numberOfColumns)
        {
            if (numberOfRows < 0)
                throw new ArgumentOutOfRangeException("numberOfRows", "The number of rows is less than 0.");
            if (numberOfColumns < 0)
                throw new ArgumentOutOfRangeException("numberOfColumns", "The number of columns is less than 0.");

            _values = new Double[numberOfRows][];
            for (Int32 i = 0; i < _values.Length; i++)
                _values[i] = new Double[numberOfColumns];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public Matrix(Int32 numberOfRows, Int32 numberOfColumns, Double defaultValue)
        {
            if (numberOfRows < 0)
                throw new ArgumentOutOfRangeException("numberOfRows", "The number of rows is less than 0.");
            if (numberOfColumns < 0)
                throw new ArgumentOutOfRangeException("numberOfColumns", "The number of columns is less than 0.");

            _values = new Double[numberOfRows][];
            for (Int32 i = 0; i < _values.Length; i++)
                _values[i] = Enumerable.Repeat(defaultValue, numberOfColumns).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix" /> class based on the other matrix.
        /// </summary>
        /// <param name="other">The other matrix.</param>
        public Matrix(Matrix other)
        {
            _values = new Double[other.NumberOfRows][];
            for (Int32 i = 0; i < _values.Length; i++)
            {
                _values[i] = new Double[other._values[i].Length];
                Array.Copy(other._values[i], _values[i], _values[i].Length);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the specified row of the matrix.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>The array of values in the specified row.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Row index was outside the bounds of the matrix.</exception>
        public Double[] GetRow(Int32 rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _values.Length)
                throw new ArgumentOutOfRangeException("rowIndex", "Row index was outside the bounds of the matrix.");

            return _values[rowIndex];
        }


        /// <summary>
        /// Returns the specified column of the matrix.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The array of values in the specified column.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Column index was outside the bounds of the matrix.</exception>
        public Double[] GetColumn(Int32 columnIndex)
        {
            if (columnIndex < 0 || _values.Length == 0 || columnIndex >= _values[0].Length)
                throw new ArgumentOutOfRangeException("columnIndex", "Column index was outside the bounds of the matrix.");

            Double[] values = new Double[_values[0].Length];

            for (Int32 i = 0; i < _values[0].Length; i++)
                values[i] = _values[i][columnIndex];

            return values;
        }


        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the values of the instance.</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(NumberOfColumns * NumberOfRows * 8);
            builder.Append("(");
            for (Int32 i = 0; i < _values.Length; i++)
            {
                if (i > 0)
                    builder.Append("; ");
                for (Int32 j = 0; j < _values[i].Length; j++)
                {
                    if (j > 0)
                        builder.Append(' ');
                    builder.Append(_values[i][j]);
                }
            }
            builder.Append(")");

            return builder.ToString();
        }

        #endregion

        #region Operators
       
        /// <summary>
        /// Sums the specified <see cref="Matrix" /> instances.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns>The sum of the two <see cref="Matrix" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first matrix is null.
        /// or
        /// The second matrix is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of rows does not match.
        /// or
        /// The number of columns does not match.
        /// </exception>
        public static Matrix operator +(Matrix first, Matrix second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first matrix is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second matrix is null.");

            if (first.NumberOfRows != second.NumberOfRows)
                throw new ArgumentException("The number of rows does not match.", "second");
            if (first.NumberOfColumns != second.NumberOfColumns)
                throw new ArgumentException("The number of columns does not match.", "second");

            Matrix result = new Matrix(first.NumberOfRows, first.NumberOfColumns);

            for (Int32 i = 0; i < result.NumberOfRows; i++)
                for (Int32 j = 0; j < result.NumberOfColumns; j++)
                {
                    result[i, j] = first[i, j] + second[i, j];
                }

            return result;
        }

        /// <summary>
        /// Extracts the specified <see cref="Matrix" /> instances.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns>The extract of the two <see cref="Matrix" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first matrix is null.
        /// or
        /// The second matrix is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of rows does not match.
        /// or
        /// The number of columns does not match.
        /// </exception>
        public static Matrix operator -(Matrix first, Matrix second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first matrix is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second matrix is null.");

            if (first.NumberOfRows != second.NumberOfRows)
                throw new ArgumentException("The number of rows does not match.", "second");
            if (first.NumberOfColumns != second.NumberOfColumns)
                throw new ArgumentException("The number of columns does not match.", "second");

            Matrix result = new Matrix(first.NumberOfRows, first.NumberOfColumns);

            for (Int32 i = 0; i < result.NumberOfRows; i++)
                for (Int32 j = 0; j < result.NumberOfColumns; j++)
                {
                    result[i, j] = first[i, j] - second[i, j];
                }

            return result;
        }

        /// <summary>
        /// Inverts all values of the specified <see cref="Matrix" />.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The <see cref="Matrix" /> with the inverted values.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Matrix operator -(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            Matrix result = new Matrix(matrix.NumberOfRows, matrix.NumberOfColumns);
            for (Int32 i = 0; i < result.NumberOfRows; i++)
                for (Int32 j = 0; j < result.NumberOfColumns; j++)
                {
                    result[i, j] = -matrix[i, j];
                }

            return result;
        }

        /// <summary>
        /// Multiplies the <see cref="System.Double" /> scalar with the specified <see cref="Matrix" />.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The scalar multiplication of the <see cref="Matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Matrix operator *(Double scalar, Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            Matrix result = new Matrix(matrix.NumberOfRows, matrix.NumberOfColumns);
            for (Int32 i = 0; i < result.NumberOfRows; i++)
                for (Int32 j = 0; j < result.NumberOfColumns; j++)
                {
                    result[i, j] = scalar * matrix[i, j];
                }

            return result;
        }

        /// <summary>
        /// Multiplies the specified <see cref="Matrix" /> with the <see cref="System.Double" /> scalar.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The scalar multiplication of the <see cref="Matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Matrix operator *(Matrix matrix, Double scalar)
        {
            return scalar * matrix; // the operation is commutative
        }

        /// <summary>
        /// Multiplies the specified <see cref="Matrix" /> instances.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns>The product of the specified <see cref="Matrix" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first matrix is null.
        /// or
        /// The second matrix is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of columns in the first matrix does not match the number of rows in the second matrix.</exception>
        public static Matrix operator *(Matrix first, Matrix second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first matrix is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second matrix is null.");

            if (first.NumberOfColumns != second.NumberOfRows)
                throw new ArgumentException("The number of columns in the first matrix does not match the number of rows in the second matrix.", "second");

            Matrix result = new Matrix(first.NumberOfRows, second.NumberOfColumns);
            for (Int32 i = 0; i < result.NumberOfRows; i++)
                for (Int32 j = 0; j < result.NumberOfColumns; j++)
                {
                    for (Int32 k = 0; k < first.NumberOfColumns; k++)
                    {
                        result[i, j] += first[i, k] * second[k, j];
                    }
                }
            return result;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether the specified matrix is a nullmatrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values of the matrix are zeros; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsNull(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            return matrix._values.All(array => array.All(value => value == 0));
        }

        /// <summary>
        /// Determines whether the specified matrix is valid.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values of the matrix are numbers; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsValid(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            return matrix._values.All(array => array.All(value => !Double.IsNaN(value)));
        }

        /// <summary>
        /// Determines whether the specified matrix instances are equal.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns><c>true</c> if the matrixes are the same size, and all values are equal; otherwise, <c>false</c>.</returns>
        public static Boolean AreEqual(Matrix first, Matrix second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;
            if (ReferenceEquals(first, second))
                return true;

            if (first._values.Length != second._values.Length)
                return false;

            for (Int32 i = 0; i < first._values.Length; i++)
            {
                if (first._values[i].Length != second._values[i].Length)
                    return false;
                if (!first._values[i].SequenceEqual(second._values[i]))
                    return false;
            }

            return true;
        }

        #endregion
    }
}
