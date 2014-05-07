/// <copyright file="MatrixFactory.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    /// <summary>
    /// Represents a factory type for the production of <see cref="Matrix" /> instances.
    /// </summary>
    public static class MatrixFactory
    {
        #region Identity matrix

        /// <summary>
        /// Creates an identity matrix.
        /// </summary>
        /// <param name="size">The size of the matrix.</param>
        /// <returns>The produced identity matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The size is less than 0.</exception>
        public static Matrix CreateIdentity(Int32 size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size", "The size is less than 0.");

            return CreateIdentity(size, size);
        }

        /// <summary>
        /// Creates an identity matrix.
        /// </summary>
        /// <param name="numberOrRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <returns>The produced identity matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static Matrix CreateIdentity(Int32 numberOrRows, Int32 numberOfColumns)
        {
            Matrix id = new Matrix(numberOrRows, numberOfColumns);
            for (Int32 i = 0; i < numberOrRows && i < numberOfColumns; i++)
                id[i, i] = 1;

            return id;
        }

        #endregion

        #region Diagonal matrix

        /// <summary>
        /// Creates a diagonal matrix.
        /// </summary>
        /// <param name="values">The values to be inserted in the diagonal of the matrix.</param>
        /// <returns>The produced diagonal matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The values are not specified.</exception>
        public static Matrix CreateDiagonal(IList<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The values are not specified.");

            Matrix diagonal = new Matrix(values.Count, values.Count);
            for (Int32 i = 0; i < values.Count; i++)
                diagonal[i, i] = values[i];

            return diagonal;
        }

        /// <summary>
        /// Creates a diagonal matrix.
        /// </summary>
        /// <param name="values">The values to be inserted in the diagonal of the matrix.</param>
        /// <returns>The produced diagonal matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The values are not specified.</exception>
        public static Matrix CreateDiagonal(params Double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The values are not specified.");

            Matrix diagonal = new Matrix(values.Length, values.Length);
            for (Int32 i = 0; i < values.Length; i++)
                diagonal[i, i] = values[i];

            return diagonal;
        }

        /// <summary>
        /// Creates a diagonal matrix.
        /// </summary>
        /// <param name="values">The values to be inserted in the diagonal of the matrix.</param>
        /// <returns>The produced diagonal matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The values are not specified.</exception>
        public static Matrix CreateDiagonal(IList<Int32> values)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The values are not specified.");

            Matrix diagonal = new Matrix(values.Count, values.Count);
            for (Int32 i = 0; i < values.Count; i++)
                diagonal[i, i] = values[i];

            return diagonal;
        }

        /// <summary>
        /// Creates a diagonal matrix.
        /// </summary>
        /// <param name="values">The values to be inserted in the diagonal of the matrix.</param>
        /// <returns>The produced diagonal matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The values are not specified.</exception>
        public static Matrix CreateDiagonal(params Int32[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The values are not specified.");

            Matrix diagonal = new Matrix(values.Length, values.Length);
            for (Int32 i = 0; i < values.Length; i++)
                diagonal[i, i] = values[i];

            return diagonal;
        }

        #endregion

        #region Square matrix

        /// <summary>
        /// Creates a square matrix.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The produced square matrix.</returns>
        /// <exception cref="System.ArgumentException">The number of values is not a square number.</exception>
        public static Matrix CreateSquare(params Double[] values)
        {
            if (Math.Sqrt(values.Length) != Math.Floor(Math.Sqrt(values.Length)))
                throw new ArgumentException("values", "The number of values is not a square number.");

            Int32 size = Convert.ToInt32(Math.Sqrt(values.Length));
            Matrix matrix = new Matrix(size, size);
            for (Int32 k = 0; k < values.Length; k++)
            {
                matrix[k / size, k % size] = values[k];
            }
            return matrix;
        }

        #endregion
    }
}
