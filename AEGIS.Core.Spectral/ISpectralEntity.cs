/// <copyright file="ISpectralEntity.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines methods for reading and writing spectral entity.
    /// </summary>
    public interface ISpectralEntity
    {
        #region Properties

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        Int32 NumberOfColumns { get; }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        Int32 NumberOfRows { get; }

        /// <summary>
        /// Gets the spectral resolution of the entity.
        /// </summary>
        /// <value>The number of spectral bands contained in the entity.</value>
        Int32 SpectralResolution { get; }

        /// <summary>
        /// Gets the radiometric resolutions of the bands in the raster.
        /// </summary>
        /// <value>The list containing the radiometric resolution of each band in the raster.</value>
        IList<Int32> RadiometricResolutions { get; }

        /// <summary>
        /// Gets a value indicating whether the entity is readable.
        /// </summary>
        /// <value><c>true</c> if the entity is readable; otherwise, <c>false</c>.</value>
        Boolean IsReadable { get; }

        /// <summary>
        /// Gets a value indicating whether the entity is writable.
        /// </summary>
        /// <value><c>true</c> if the entity is writable; otherwise, <c>false</c>.</value>
        Boolean IsWritable { get; }

        /// <summary>
        /// Gets the representation of the entity.
        /// </summary>
        /// <value>The representation of the entity.</value>
        RasterRepresentation Representation { get; }

        /// <summary>
        /// Gets the supported read/write orders.
        /// </summary>
        /// <value>The list of supported read/write orders.</value>
        IList<SpectralDataOrder> SupportedOrders { get; }

        #endregion

        #region Methods for reading integer values

        /// <summary>
        /// Reads the specified spectral value from the entity.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The entity is not readable.</exception>
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
        /// Reads a sequence of spectral values from the entity.
        /// </summary>
        /// <param name="startIndex">The zero-based absolute starting index.</param>
        /// <param name="numberOfValues">The number of values to be read.</param>
        /// <returns>The array containing the sequence of values in the default order of the entity.</returns>
        /// <exception cref="System.NotSupportedException">The entity is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// or
        /// The number of valies is less than 0.
        /// </exception>
        UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues);

        /// <summary>
        /// Reads a sequence of spectral values from the entity.
        /// </summary>
        /// <param name="startIndex">The zero-based absolute starting index.</param>
        /// <param name="numberOfValues">The number of values to be read.</param>
        /// <param name="readOrder">The reading order.</param>
        /// <returns>The array containing the sequence of values in the specified order.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The entity is not readable.
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
        UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues, SpectralDataOrder readOrder);

        /// <summary>
        /// Reads a sequence of spectral values from the entity.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the first value.</param>
        /// <param name="columnIndex">The zero-based column index of the first value.</param>
        /// <param name="bandIndex">The zero-based band index of the first value.</param>
        /// <param name="numberOfValues">The number of values.</param>
        /// <returns>The array containing the sequence of values in the default order of the entity.</returns>
        /// <exception cref="System.NotSupportedException">The entity is not readable.</exception>
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
        /// Reads a sequence of spectral values from the entity.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the first value.</param>
        /// <param name="columnIndex">The zero-based column index of the first value.</param>
        /// <param name="bandIndex">The zero-based band index of the first value.</param>
        /// <param name="numberOfValues">The number of values.</param>
        /// <param name="readOrder">The reading order.</param>
        /// <returns>The array containing the sequence of values in the specified order.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The entity is not readable.
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
        UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, SpectralDataOrder readOrder);

        #endregion

        #region Methods for reading floating point values

        /// <summary>
        /// Reads the specified spectral value from the entity.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The entity is not readable.</exception>
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
        /// Reads a sequence of spectral values from the entity.
        /// </summary>
        /// <param name="startIndex">The zero-based absolute starting index.</param>
        /// <param name="numberOfValues">The number of values to be read.</param>
        /// <returns>The array containing the sequence of values in the default order of the entity.</returns>
        /// <exception cref="System.NotSupportedException">The entity is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// or
        /// The number of valies is less than 0.
        /// </exception>
        Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues);

        /// <summary>
        /// Reads a sequence of spectral values from the entity.
        /// </summary>
        /// <param name="startIndex">The zero-based absolute starting index.</param>
        /// <param name="numberOfValues">The number of values to be read.</param>
        /// <param name="readOrder">The reading order.</param>
        /// <returns>The array containing the sequence of values in the specified order.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The entity is not readable.
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
        Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues, SpectralDataOrder readOrder);

        /// <summary>
        /// Reads a sequence of spectral values from the entity.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the first value.</param>
        /// <param name="columnIndex">The zero-based column index of the first value.</param>
        /// <param name="bandIndex">The zero-based band index of the first value.</param>
        /// <param name="numberOfValues">The number of values.</param>
        /// <returns>The array containing the sequence of values in the default order of the entity.</returns>
        /// <exception cref="System.NotSupportedException">The entity is not readable.</exception>
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
        /// Reads a sequence of spectral values from the entity.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the first value.</param>
        /// <param name="columnIndex">The zero-based column index of the first value.</param>
        /// <param name="bandIndex">The zero-based band index of the first value.</param>
        /// <param name="numberOfValues">The number of values.</param>
        /// <param name="readOrder">The reading order.</param>
        /// <returns>The array containing the sequence of values in the specified order.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The entity is not readable.
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
        Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, SpectralDataOrder readOrder);

        #endregion

        #region Methods for writing integer values

        /// <summary>
        /// Writes the specified spectral value to the entity.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The entity is not writable.</exception>
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
        /// Writes the specified spectral values to the entity in the default order.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <exception cref="System.NotSupportedException">The entity is not writable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// </exception>
        void WriteValueSequence(Int32 startIndex, UInt32[] spectralValues);

        /// <summary>
        /// Writes the specified spectral values to the entity in the specified order.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">
        /// The entity is not writable.
        /// or
        /// The writing order is not supported.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// </exception>
        void WriteValueSequence(Int32 startIndex, UInt32[] spectralValues, SpectralDataOrder writeOrder);

        /// <summary>
        /// Writes a sequence of spectral values to the entity in the default order.
        /// </summary>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="bandIndex">The starting band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <exception cref="System.NotSupportedException">The entity is not writable.</exception>
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
        /// Writes a sequence of spectral values to the entity in the specified order.
        /// </summary>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="bandIndex">The starting band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">
        /// The entity is not writable.
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
        void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues, SpectralDataOrder writeOrder);

        #endregion

        #region Methods for writing integer values

        /// <summary>
        /// Writes the specified spectral value to the entity.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The entity is not writable.</exception>
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
        void WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue);

        /// <summary>
        /// Writes the specified spectral values to the entity in the default order.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <exception cref="System.NotSupportedException">The entity is not writable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// </exception>
        void WriteValueSequence(Int32 startIndex, Double[] spectralValues);

        /// <summary>
        /// Writes the specified spectral values to the entity in the specified order.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">
        /// The entity is not writable.
        /// or
        /// The writing order is not supported.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The start index is less than 0.
        /// or
        /// The start index is equal to or greater than the number of values.
        /// </exception>
        void WriteValueSequence(Int32 startIndex, Double[] spectralValues, SpectralDataOrder writeOrder);

        /// <summary>
        /// Writes a sequence of spectral values to the entity in the default order.
        /// </summary>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="bandIndex">The starting band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <exception cref="System.NotSupportedException">The entity is not writable.</exception>
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
        void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues);

        /// <summary>
        /// Writes a sequence of spectral values to the entity in the specified order.
        /// </summary>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="bandIndex">The starting band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">
        /// The entity is not writable.
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
        void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues, SpectralDataOrder writeOrder);

        #endregion
    }
}
