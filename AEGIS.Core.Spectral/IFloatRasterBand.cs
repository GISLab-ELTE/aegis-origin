/// <copyright file="IFloatRasterBand.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for raster image bands containing floating point values.
    /// </summary>
    public interface IFloatRasterBand : IRasterBand
    {
        /// <summary>
        /// Gets or sets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the pixel.</param>
        /// <param name="columnIndex">The zero-based row index of the pixel.</param>
        /// <value>The spectral value located at the specified row and column index.</value>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The raster is not writable.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        new Double this[Int32 rowIndex, Int32 columnIndex] { get; set; }

        /// <summary>
        /// Gets or sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="x">The zero-based column index of the pixel.</param>
        /// <param name="y">The zero-based row index of the pixel.</param>
        /// <value>The spectral value located at the specified coordinate.</value>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The raster is not writable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        new Double this[Coordinate coordinate] { get; set; }

        /// <summary>
        /// Sets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <param name="value">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">The raster is not writable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        void SetValue(Int32 rowIndex, Int32 columnIndex, Double value);

        /// <summary>
        /// Sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="value">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not writable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        void SetValue(Coordinate coordinate, Double value);

        /// <summary>
        /// Gets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        new Double GetValue(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Gets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        new Double GetValue(Coordinate coordinate);

        /// <summary>
        /// Gets the nearest spectral value to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index or in the nearest index if either the row or column indices are out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        new Double GetNearestValue(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Gets the nearest spectral value to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate or at the nearest index if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        new Double GetNearestValue(Coordinate coordinate);
    }
}
