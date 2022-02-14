/// <copyright file="IRasterService.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior of services providing raster image data.
    /// </summary>
    public interface IRasterService
    {
        #region Properties

        /// <summary>
        /// Gets the dimensions of the raster.
        /// </summary>
        /// <value>
        /// The dimensions of the raster.
        /// </value>
        RasterDimensions Dimensions { get; }

        /// <summary>
        /// Gets the format of the service.
        /// </summary>
        /// <value>The format of the service.</value>
        RasterFormat Format { get; }

        /// <summary>
        /// Gets a value indicating whether the service is readable.
        /// </summary>
        /// <value><c>true</c> if the service is readable; otherwise, <c>false</c>.</value>
        Boolean IsReadable { get; }

        /// <summary>
        /// Gets a value indicating whether the service is writable.
        /// </summary>
        /// <value><c>true</c> if the service is writable; otherwise, <c>false</c>.</value>
        Boolean IsWritable { get; }

        /// <summary>
        /// Gets the data order of the service.
        /// </summary>
        /// <value>The data order of the service.</value>
        RasterDataOrder DataOrder { get; }

        #endregion

        #region Methods for reading integer values

        /// <summary>
        /// Reads the specified spectral value from the service.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The service does not support reading.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        UInt32 ReadValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);

        /// <summary>
        /// Reads a sequence of spectral values from the service.
        /// </summary>
        /// <param name="startIndex">The zero-based absolute starting index.</param>
        /// <param name="numberOfValues">The number of values to be read.</param>
        /// <returns>The array containing the sequence of values in the default order of the service.</returns>
        /// <exception cref="System.NotSupportedException">The service does not support reading.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// or
        /// The number of values is less than 0.
        /// </exception>
        UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues);

        /// <summary>
        /// Reads a sequence of spectral values from the service.
        /// </summary>
        /// <param name="startIndex">The zero-based absolute starting index.</param>
        /// <param name="numberOfValues">The number of values to be read.</param>
        /// <param name="readOrder">The reading order.</param>
        /// <returns>The array containing the sequence of values in the specified order.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The service does not support reading.
        /// or
        /// The specified reading order is not supported. 
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// or
        /// The number of values is less than 0.
        /// </exception>
        UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues, RasterDataOrder readOrder);

        /// <summary>
        /// Reads a sequence of spectral values from the service.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the first value.</param>
        /// <param name="columnIndex">The zero-based column index of the first value.</param>
        /// <param name="bandIndex">The zero-based band index of the first value.</param>
        /// <param name="numberOfValues">The number of values.</param>
        /// <returns>The array containing the sequence of values in the default order of the service.</returns>
        /// <exception cref="System.NotSupportedException">The service does not support reading.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The number of values is less than 0.
        /// </exception>
        UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues);

        /// <summary>
        /// Reads a sequence of spectral values from the service.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the first value.</param>
        /// <param name="columnIndex">The zero-based column index of the first value.</param>
        /// <param name="bandIndex">The zero-based band index of the first value.</param>
        /// <param name="numberOfValues">The number of values.</param>
        /// <param name="readOrder">The reading order.</param>
        /// <returns>The array containing the sequence of values in the specified order.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The service does not support reading.
        /// or
        /// The specified reading order is not supported. 
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The number of values is less than 0.
        /// </exception>
        UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, RasterDataOrder readOrder);

        #endregion

        #region Methods for reading floating point values

        /// <summary>
        /// Reads the specified spectral value from the service.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The service does not support reading.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        Double ReadFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);

        /// <summary>
        /// Reads a sequence of spectral values from the service.
        /// </summary>
        /// <param name="startIndex">The zero-based absolute starting index.</param>
        /// <param name="numberOfValues">The number of values to be read.</param>
        /// <returns>The array containing the sequence of values in the default order of the service.</returns>
        /// <exception cref="System.NotSupportedException">The service does not support reading.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// or
        /// The number of values is less than 0.
        /// </exception>
        Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues);

        /// <summary>
        /// Reads a sequence of spectral values from the service.
        /// </summary>
        /// <param name="startIndex">The zero-based absolute starting index.</param>
        /// <param name="numberOfValues">The number of values to be read.</param>
        /// <param name="readOrder">The reading order.</param>
        /// <returns>The array containing the sequence of values in the specified order.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The service does not support reading.
        /// or
        /// The specified reading order is not supported. 
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// or
        /// The number of values is less than 0.
        /// </exception>
        Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues, RasterDataOrder readOrder);

        /// <summary>
        /// Reads a sequence of spectral values from the service.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the first value.</param>
        /// <param name="columnIndex">The zero-based column index of the first value.</param>
        /// <param name="bandIndex">The zero-based band index of the first value.</param>
        /// <param name="numberOfValues">The number of values.</param>
        /// <returns>The array containing the sequence of values in the default order of the service.</returns>
        /// <exception cref="System.NotSupportedException">The service does not support reading.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The number of values is less than 0.
        /// </exception>
        Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues);

        /// <summary>
        /// Reads a sequence of spectral values from the service.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the first value.</param>
        /// <param name="columnIndex">The zero-based column index of the first value.</param>
        /// <param name="bandIndex">The zero-based band index of the first value.</param>
        /// <param name="numberOfValues">The number of values.</param>
        /// <param name="readOrder">The reading order.</param>
        /// <returns>The array containing the sequence of values in the specified order.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The service does not support reading.
        /// or
        /// The specified reading order is not supported. 
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The number of values is less than 0.
        /// </exception>
        Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, RasterDataOrder readOrder);

        #endregion

        #region Methods for writing integer values

        /// <summary>
        /// Writes the specified spectral value to the service.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        void WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue);

        /// <summary>
        /// Writes the specified spectral values to the service in the default order.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// </exception>
        void WriteValueSequence(Int32 startIndex, UInt32[] spectralValues);

        /// <summary>
        /// Writes the specified spectral values to the service in the specified order.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">
        /// The service does not support writing.
        /// or
        /// The writing order is not supported.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// </exception>
        void WriteValueSequence(Int32 startIndex, UInt32[] spectralValues, RasterDataOrder writeOrder);

        /// <summary>
        /// Writes a sequence of spectral values to the service in the default order.
        /// </summary>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="bandIndex">The starting band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues);

        /// <summary>
        /// Writes a sequence of spectral values to the service in the specified order.
        /// </summary>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="bandIndex">The starting band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">
        /// The service does not support writing.
        /// or
        /// The writing order is not supported.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues, RasterDataOrder writeOrder);

        #endregion

        #region Methods for writing floating point values

        /// <summary>
        /// Writes the specified spectral value to the service.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        void WriteFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue);

        /// <summary>
        /// Writes the specified spectral values to the service in the default order.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// </exception>
        void WriteFloatValueSequence(Int32 startIndex, Double[] spectralValues);

        /// <summary>
        /// Writes the specified spectral values to the service in the specified order.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">
        /// The service does not support writing.
        /// or
        /// The writing order is not supported.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// </exception>
        void WriteFloatValueSequence(Int32 startIndex, Double[] spectralValues, RasterDataOrder writeOrder);

        /// <summary>
        /// Writes a sequence of spectral values to the service in the default order.
        /// </summary>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="bandIndex">The starting band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        void WriteFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues);

        /// <summary>
        /// Writes a sequence of spectral values to the service in the specified order.
        /// </summary>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="bandIndex">The starting band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">
        /// The service does not support writing.
        /// or
        /// The writing order is not supported.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        void WriteFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues, RasterDataOrder writeOrder);

        #endregion
    }
}
