/// <copyright file="Vector.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELTE.AEGIS.Numerics
{
    /// <summary>
    /// Represents a vector in Euclidean space.
    /// </summary>
    [Serializable]
    public class Vector
    {
        #region Private fields

        private Double[] _values;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the size of the vector.
        /// </summary>
        /// <value>The size of the vector.</value>
        public Int32 Size { get { return _values.Length; } }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <value>The length of the vector.</value>
        public Double Length { get { return Math.Sqrt(_values.Sum(value => Calculator.Square(value))); } }

        /// <summary>
        /// Gets the values of the vector.
        /// </summary>
        /// <value>The values of the vector.</value>
        public Double[] Values { get { return _values; } }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <value>The value.</value>
        /// <param name="index">The index.</param>
        /// <returns>The <see cref="Double" /> value at the specified index.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Index is outside the bounds of the matrix.</exception>
        public Double this[Int32 index]
        {
            get
            {
                if (index < 0 || index >= Size)
                    throw new IndexOutOfRangeException("Index is outside the bounds of the matrix.");
 
                return _values[index];
            }
            set
            {
                if (index < 0 || index >= Size)
                    throw new IndexOutOfRangeException("Index is outside the bounds of the matrix.");
 
                _values[index] = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The size is less than 0.</exception>
        public Vector(Int32 size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size", "The size is less than 0.");

            _values = new Double[size];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        /// <param name="defaultValue">The default value for all dimensions.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The size is less than 0.</exception>
        public Vector(Int32 size, Double defaultValue)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size", "The size is less than 0.");

            _values = Enumerable.Repeat(defaultValue, size).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="values">The values of the vector.</param>
        /// <exception cref="System.ArgumentNullException">The values are not specified.</exception>
        public Vector(params Double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The values are not specified.");

            _values = values.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="values">The values of the vector.</param>
        /// <exception cref="System.ArgumentNullException">The values are not specified.</exception>
        public Vector(IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The values are not specified.");

            _values = values.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <exception cref="System.ArgumentNullException">The other vector are not specified.</exception>
        public Vector(Vector other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other vector are not specified.");

            _values = new Double[other._values.Length];
            Array.Copy(other._values, _values, _values.Length);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Transpose the vector.
        /// </summary>
        /// <returns>The transposed vector as a <see cref="Matrix" />.</returns>
        public Matrix Transpone()
        {
            Matrix matrix = new Matrix(1, Size);
            for (Int32 i = 0; i <Size; i++)
            {
                matrix[0, i] = _values[i];
            }

            return matrix;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the values of the instance.</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(_values.Length * 8);
            builder.Append("(");
            for (Int32 i = 0; i < _values.Length; i++)
            {
                if (i > 0)
                    builder.Append(' ');
                builder.Append(_values[i]);
            }
            builder.Append(")");

            return builder.ToString();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Inverts all values of the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The vector with the inverted values.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Vector operator -(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");

            Vector result = new Vector(vector.Size);
            for (Int32 i = 0; i < vector.Size; i++)
            {
                result._values[i] = -vector._values[i];
            }
            return result;
        }

        /// <summary>
        /// Sums the specified vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum of the two vector instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Vector operator +(Vector first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first vector is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second vector is null.");

            if (first.Size != second.Size)
                throw new ArgumentException("The dimensions of the two vectors are different.");

            Vector result = new Vector(first.Size);
            for (Int32 i = 0; i < first.Size; i++)
            {
                result._values[i] = first._values[i] + second._values[i];
            }
            return result;
        }

        /// <summary>
        /// Sums the specified vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum of the two vector instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Vector operator +(Vector first, Double[] second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first vector is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second vector is null.");

            if (first.Size != second.Length)
                throw new ArgumentException("The dimensions of the two vectors are different.");

            Vector result = new Vector(first.Size);
            for (Int32 i = 0; i < first.Size; i++)
            {
                result._values[i] = first._values[i] + second[i];
            }
            return result;
        }

        /// <summary>
        /// Sums the specified vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum of the two vector instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Vector operator +(Double[] first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first vector is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second vector is null.");

            if (first.Length != second.Size)
                throw new ArgumentException("The dimensions of the two vectors are different.");

            Vector result = new Vector(first.Length);
            for (Int32 i = 0; i < first.Length; i++)
            {
                result._values[i] = first[i] + second._values[i];
            }
            return result;
        }

        /// <summary>
        /// Extracts the specified vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The extract of the two vector instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Vector operator -(Vector first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first vector is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second vector is null.");

            if (first.Size != second.Size)
                throw new ArgumentException("The dimensions of the two vectors are different.");

            Vector result = new Vector(first.Size);
            for (Int32 i = 0; i < first.Size; i++)
            {
                result._values[i] = first._values[i] - second._values[i];
            }
            return result;
        }

        /// <summary>
        /// Multiplies the <see cref="System.Double" /> scalar with a vector.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The scalar multiplication of the vector.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Vector operator *(Double scalar, Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");

            Vector result = new Vector(vector.Size);
            for (Int32 i = 0; i < result.Size; i++)
            {
                result._values[i] = scalar * vector._values[i];
            }
            return result;
        }

        /// <summary>
        /// Multiplies the vector with a <see cref="System.Double" /> scalar.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The scalar multiplication of the vector.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Vector operator *(Vector vector, Double scalar)
        {            
            return scalar * vector; // the operation is commutative
        }

        /// <summary>
        /// Calculate the scalar product of two vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The <see cref="System.Double" /> product of the two vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are not equal.</exception>
        public static Double operator *(Vector first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first vector is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second vector is null.");

            if (first.Size != second.Size)
                throw new ArgumentException("second", "The dimensions of the two vectors are not equal.");

            Double result = 0;
            for (Int32 i = 0; i < first.Size; i++)
            {
                result += first._values[i] * second._values[i];
            }
            return result;
        }

        /// <summary>
        /// Multiplies the vector with a <see cref="Matrix" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The product of the vector and the matrix.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The vector is null.
        /// or
        /// The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The number of rows in the matrix must be one.</exception>
        public static Matrix operator *(Vector vector, Matrix matrix)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            if (matrix.NumberOfRows != 1)
                throw new ArgumentException("The number of rows in the matrix must be one.", "matrix");

            Matrix result = new Matrix(vector.Size, matrix.NumberOfColumns);
            for (Int32 i = 0; i < result.NumberOfRows; i++)
            {
                for (Int32 j = 0; j < result.NumberOfColumns; j++)
                {
                    result[i, j] = vector._values[i] * matrix[0, j];
                }
            }
            return result;
        }

        /// <summary>
        /// Multiplies the <see cref="Matrix" /> with a vector.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The product of the vector and the matrix.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The vector is null.
        /// or
        /// The matrix is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of columns in the matrix does not match the dimension of the vector.</exception>
        public static Vector operator *(Matrix matrix, Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");
            if (matrix == null)
                throw new ArgumentNullException("matrix", "The matrix is null.");

            if (vector.Size != matrix.NumberOfColumns)
                throw new ArgumentException("The dimension of the vector does not match the number of rows in the matrix.", "vector");

            Vector result = new Vector(matrix.NumberOfRows);
            for (Int32 i = 0; i < matrix.NumberOfRows; i++)
            {
                for (Int32 j = 0; j < matrix.NumberOfColumns; j++)
                {
                    result._values[i] += matrix[i, j] * vector._values[j];
                }
            }
            return result;
        }

        /// <summary>
        /// Divides the vector with a <see cref="System.Double" /> scalar.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The quotient of the vector and the scalar.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Vector operator /(Vector vector, Double scalar)
        {
            return (1 / scalar) * vector;
        }

        /// <summary>
        /// Converts the specified vector to a <see cref="Matrix" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The matrix equivalent of the vector.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static explicit operator Matrix(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");

            Matrix matrix = new Matrix(vector.Size, 1);
            for (Int32 i = 0; i < vector.Size; i++)
            {
                matrix[i, 0] = vector[i];
            }
            return matrix;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether the specified vector is a null vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns><c>true</c> if all values of the vector are zeros; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Boolean IsNull(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");

            return vector._values.All(value => value == 0);
        }

        /// <summary>
        /// Determines whether the specified vector is valid.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns><c>true</c> if all values of the vector are numbers; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Boolean IsValid(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException("vector", "The vector is null.");

            return vector._values.All(value => !Double.IsNaN(value));
        }

        /// <summary>
        /// Determines whether the specified vector instances are equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the vectors are the same size, and all values are equal; otherwise, <c>false</c>.</returns>
        public static Boolean AreEqual(Vector first, Vector second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;
            if (ReferenceEquals(first, second))
                return true;

            if (first._values.Length != second._values.Length)
                return false;

            return first._values.SequenceEqual(second._values);
        }

        #endregion
    }
}
