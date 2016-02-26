/// <copyright file="IRasterBand.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for raster image bands.
    /// </summary>
    public interface IRasterBand
    {
        #region Properties

        /// <summary>
        /// Gets the raster where the band is located.
        /// </summary>
        /// <value>The raster where the band is located.</value>
        IRaster Raster { get; }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        Int32 NumberOfColumns { get; }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        Int32 NumberOfRows { get; }

        /// <summary>
        /// Gets the radiometric resolution of the band.
        /// </summary>
        /// <value>The number of bits used for storing spectral values.</value>
        Int32 RadiometricResolution { get; }

        /// <summary>
        /// Gets the histogram values of the band.
        /// </summary>
        /// <value>The histogram values of the band.</value>
        IList<Int32> HistogramValues { get; }

        /// <summary>
        /// Get a value indicating whether the raster band is mapped to coordinate space.
        /// </summary>
        /// <value><c>true</c> if the raster band is mapped to coordinate space; otherwise, <c>false</c>.</value>
        Boolean IsMapped { get; }

        /// <summary>
        /// Gets a value indicating whether the raster band is readable.
        /// </summary>
        /// <value><c>true</c> if the raster band is readable; otherwise, <c>false</c>.</value>
        Boolean IsReadable { get; }

        /// <summary>
        /// Gets a value indicating whether the raster band is writable.
        /// </summary>
        /// <value><c>true</c> if the raster band is writable; otherwise, <c>false</c>.</value>
        Boolean IsWritable { get; }

        /// <summary>
        /// Gets the format of the raster.
        /// </summary>
        /// <value>The format of the raster.</value>
        RasterFormat Format { get; }
        
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
        UInt32 this[Int32 rowIndex, Int32 columnIndex] { get; set; }

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
        /// The geometry of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        UInt32 this[Coordinate coordinate] { get; set; }

        #endregion

        #region Methods

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
        void SetValue(Int32 rowIndex, Int32 columnIndex, UInt32 value);

        /// <summary>
        /// Sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="value">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not writable.
        /// or
        /// The geometry of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        void SetValue(Coordinate coordinate, UInt32 value);

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
        void SetFloatValue(Int32 rowIndex, Int32 columnIndex, Double value);

        /// <summary>
        /// Sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="value">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not writable.
        /// or
        /// The geometry of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        void SetFloatValue(Coordinate coordinate, Double value);

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
        UInt32 GetValue(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Gets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The geometry of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        UInt32 GetValue(Coordinate coordinate);

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
        Double GetFloatValue(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Gets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The geometry of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        Double GetFloatValue(Coordinate coordinate);

        /// <summary>
        /// Gets the nearest spectral value to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index or in the nearest index if either the row or column indices are out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        UInt32 GetNearestValue(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Gets the nearest spectral value to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate or at the nearest index if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The geometry of the raster is not defined.
        /// </exception>
        UInt32 GetNearestValue(Coordinate coordinate);

        /// <summary>
        /// Gets the nearest spectral value to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index or in the nearest index if either the row or column indices are out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        Double GetNearestFloatValue(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Gets the nearest spectral value to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate or at the nearest index if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The geometry of the raster is not defined.
        /// </exception>
        Double GetNearestFloatValue(Coordinate coordinate);

        #endregion
    }
}
