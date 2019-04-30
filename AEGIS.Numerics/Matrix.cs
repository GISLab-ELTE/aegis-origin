/// <copyright file="Matrix.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELTE.AEGIS.Numerics
{
    /// <summary>
    /// Represents a matrix in Euclidean space.
    /// </summary>
    [Serializable]
    public class Matrix : IEnumerable<Double>
    {
        #region Private types

        /// <summary>
        /// Enumerates the elements of a matrix.
        /// </summary>
        /// <remarks>
        /// The enumerator performs a level order traversal of the specified matrix.
        /// </remarks>
        public class Enumerator : IEnumerator<Double>
        {
            #region Private fields

            /// <summary>
            /// The matrix that is enumerated.
            /// </summary>
            private Matrix _localMatrix;

            /// <summary>
            /// The version of the matrix when the enumerator was cerated.
            /// </summary>
            private Int32 _localVersion;

            /// <summary>
            /// The current row index.
            /// </summary>
            private Int32 _rowIndex;

            /// <summary>
            /// The current column index.
            /// </summary>
            private Int32 _columnIndex;

            /// <summary>
            /// A value indicating whether the enumerator is disposed.
            /// </summary>
            private Boolean _disposed;

            #endregion

            #region IEnumerable properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>The element at the current position of the enumerator.</value>
            public Double Current
            {
                get { return _localMatrix._values[_rowIndex * _localMatrix.NumberOfColumns + _columnIndex]; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>The element at the current position of the enumerator.-</value>
            Object IEnumerator.Current
            {
                get { return _localMatrix._values[_rowIndex * _localMatrix.NumberOfColumns + _columnIndex]; }
            }

            #endregion

            #region Constructors and destructor

            /// <summary>
            /// Initializes a new instance of the <see cref="Heap{TKey, TValue}.Enumerator" /> class.
            /// </summary>
            /// <param name="heap">The heap.</param>
            internal Enumerator(Matrix matrix)
            {
                _localMatrix = matrix;
                _localVersion = matrix._version;

                _rowIndex = -1;
                _columnIndex = _localMatrix.NumberOfColumns - 1;
                _disposed = false;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="Enumerator"/> class.
            /// </summary>
            ~Enumerator()
            {
                Dispose(false);
            }

            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localMatrix._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_rowIndex == _localMatrix.NumberOfRows - 1 && _columnIndex == _localMatrix.NumberOfColumns - 1)
                    return false;

                _columnIndex++;
                if (_columnIndex == _localMatrix.NumberOfColumns)
                {
                    _rowIndex++;
                    _columnIndex = 0;
                }

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localMatrix._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _rowIndex = -1;
                _columnIndex = _localMatrix.NumberOfColumns - 1;
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_disposed)
                    return;

                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Protected methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
            protected virtual void Dispose(Boolean disposing)
            {
                _disposed = true;

                if (disposing)
                {
                    _localMatrix = null;
                }
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The version of the matrix.
        /// </summary>
        private Int32 _version;

        /// <summary>
        /// The values stored in row major order.
        /// </summary>
        private Double[] _values;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of rows.</value>
        public Int32 NumberOfRows { get; private set; }
        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>The number of columns.</value>
        public Int32 NumberOfColumns { get; private set; }

        /// <summary>
        /// Determines whether the matrix is square.
        /// </summary>
        /// <value><c>true</c> if the matrix is square; otherwise, <c>false</c>.</value>
        public Boolean IsSquare { get { return NumberOfRows == NumberOfColumns; } }

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
                for (Int32 rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < NumberOfColumns; columnIndex++)
                        trace += _values[rowIndex * NumberOfColumns + columnIndex];
                return trace;
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
                if (rowIndex < 0 || rowIndex >= NumberOfRows)
                    throw new IndexOutOfRangeException("Row index was outside the bounds of the matrix.");
                if (columnIndex < 0 || columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("Column index was outside the bounds of the matrix.");

                return _values[rowIndex * NumberOfColumns + columnIndex];
            }
            set
            {
                if (rowIndex < 0 || rowIndex >= NumberOfRows)
                    throw new IndexOutOfRangeException("Row index was outside the bounds of the matrix.");
                if (columnIndex < 0 || columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("Column index was outside the bounds of the matrix.");

                if (_values[rowIndex * NumberOfColumns + columnIndex] == value)
                    return;

                _values[rowIndex * NumberOfColumns + columnIndex] = value;
                _version++;
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

            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            _values = new Double[NumberOfRows * NumberOfColumns];
            _version = 0;
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

            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            _values = Enumerable.Repeat(defaultValue, NumberOfRows * NumberOfColumns).ToArray();
            _version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix" />.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public Matrix(Double[,] source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            NumberOfRows = source.GetLength(0);
            NumberOfColumns = source.GetLength(1);
            _values = new Double[NumberOfRows * NumberOfColumns];
            _version = 0;

            for (Int32 rowIndex = 0; rowIndex < source.GetLength(0); rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < source.GetLength(1); columnIndex++)
                {
                    _values[rowIndex * NumberOfColumns + columnIndex] = source[rowIndex, columnIndex];
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix" /> class based on the other matrix.
        /// </summary>
        /// <param name="other">The other matrix.</param>
        /// <exception cref="System.ArgumentNullException">The other matrix is null.</exception>
        public Matrix(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other matrix is null.");

            NumberOfRows = other.NumberOfRows;
            NumberOfColumns = other.NumberOfColumns;
            _values = new Double[NumberOfRows * NumberOfColumns];
            _version = 0;

            Array.Copy(other._values, _values, _values.Length);
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
            if (rowIndex < 0 || rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "Row index was outside the bounds of the matrix.");

            Double[] values = new Double[NumberOfColumns];

            Array.Copy(_values, rowIndex * NumberOfColumns, values, 0, NumberOfColumns);

            return values;
        }


        /// <summary>
        /// Returns the specified column of the matrix.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The array of values in the specified column.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Column index was outside the bounds of the matrix.</exception>
        public Double[] GetColumn(Int32 columnIndex)
        {
            if (columnIndex < 0 || NumberOfRows == 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "Column index was outside the bounds of the matrix.");

            Double[] values = new Double[NumberOfRows];

            for (Int32 rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
                values[rowIndex] = _values[rowIndex * NumberOfColumns + columnIndex];

            return values;
        }


        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<Double> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
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
            for (Int32 rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
            {
                if (rowIndex > 0)
                    builder.Append("; ");
                for (Int32 columnIndex = 0; columnIndex < NumberOfColumns; columnIndex++)
                {
                    if (columnIndex > 0)
                        builder.Append(' ');
                    builder.Append(_values[rowIndex * NumberOfColumns + columnIndex]);
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

            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = first[rowIndex, columnIndex] + second[rowIndex, columnIndex];
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

            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = first[rowIndex, columnIndex] - second[rowIndex, columnIndex];
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
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = -matrix[rowIndex, columnIndex];
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
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = scalar * matrix[rowIndex, columnIndex];
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
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    for (Int32 k = 0; k < first.NumberOfColumns; k++)
                    {
                        result[rowIndex, columnIndex] += first[rowIndex, k] * second[k, columnIndex];
                    }
                }
            return result;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether the specified matrix is a null matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values of the matrix are zeros; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsNull(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            if (matrix._values.Length == 0)
                return true;

            return matrix._values.All(value => value == 0);
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

            if (matrix._values.Length == 0)
                return true;

            return matrix._values.All(value => !Double.IsNaN(value));
        }

        /// <summary>
        /// Determines whether the specified matrix is symmetric.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if the matrix is square and symmetric; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsSymmetric(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            if (matrix.NumberOfRows != matrix.NumberOfColumns)
                return false;

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = rowIndex + 1; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    if (matrix._values[rowIndex * matrix.NumberOfColumns + columnIndex] != matrix._values[rowIndex * matrix.NumberOfColumns + columnIndex])
                        return false;
                }
            return true;
        }

        /// <summary>
        /// Determines whether the specified matrix instances are equal.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns><c>true</c> if the matrices are the same size, and all values are equal; otherwise, <c>false</c>.</returns>
        public static Boolean AreEqual(Matrix first, Matrix second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;
            if (ReferenceEquals(first, second))
                return true;

            if (first.NumberOfRows != second.NumberOfRows || first.NumberOfColumns != second.NumberOfColumns)
                return false;

            return first._values.SequenceEqual(second._values);
        }

        #endregion
    }
}
