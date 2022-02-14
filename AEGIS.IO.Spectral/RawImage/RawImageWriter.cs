/// <copyright file="RawImageWriter.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gréta Bereczki</author>

using ELTE.AEGIS.IO.RawImage;
using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.Spectral.RawImage
{
    /// <summary>
    /// Represents a general raw image format writer.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::610210", "Generic Raw Image")]
    public class RawImageWriter : GeometryStreamWriter
    {
        #region Private types

        /// <summary>
        /// Represents the service of writing.
        /// </summary>
        public class RawImageWriterService : IRasterService
        {
            #region Private fields

            /// <summary>
            /// The raw image writer.
            /// </summary>
            private RawImageWriter _rawImageWriter;

            #endregion

            #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="RawImageWriterService" /> class.
            /// </summary>
            /// <param name="rawImageWriter">The raw image writer.</param>
            public RawImageWriterService(RawImageWriter rawImageWriter)
            {
                _rawImageWriter = rawImageWriter;

                Dimensions = new RasterDimensions(_rawImageWriter._numberOfBands, _rawImageWriter._numberOfRows, _rawImageWriter._numberOfColumns, _rawImageWriter._radiometricResolution);
                Format = _rawImageWriter._format;

                switch (rawImageWriter._layout)
                {
                    case RawImageLayout.BandInterlevedByLine:
                        DataOrder = RasterDataOrder.RowBandColumn;
                        break;
                    case RawImageLayout.BandInterlevedByPixel:
                        DataOrder = RasterDataOrder.BandRowColumn;
                        break;
                    case RawImageLayout.BandSequential:
                        DataOrder = RasterDataOrder.RowColumnBand;
                        break;
                    default:
                        break;
                }
            }

            #endregion

            #region IRasterService properties

            /// <summary>
            /// Gets the dimensions of the raster.
            /// </summary>
            /// <value>
            /// The dimensions of the raster.
            /// </value>
            public RasterDimensions Dimensions { get; private set; }

            /// <summary>
            /// Gets a value indicating whether the dataset is readable.
            /// </summary>
            /// <value><c>true</c> if the dataset is readable; otherwise, <c>false</c>.</value>
            public Boolean IsReadable { get { return false; } }

            /// <summary>
            /// Gets a value indicating whether the dataset is writable.
            /// </summary>
            /// <value><c>true</c> if the dataset is writable; otherwise, <c>false</c>.</value>
            public Boolean IsWritable { get { return true; } }

            /// <summary>
            /// Gets the format of the dataset.
            /// </summary>
            /// <value>The format of the dataset.</value>
            public RasterFormat Format { get; private set; }

            /// <summary>
            /// Gets the data order of the service.
            /// </summary>
            /// <value>The data order of the service.</value>
            public RasterDataOrder DataOrder { get; private set; }

            #endregion

            #region IRasterService methods for reading integer value

            /// <summary>
            /// Reads the specified spectral value from the dataset.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the value.</param>
            /// <param name="columnIndex">The zero-based column index of the value.</param>
            /// <param name="bandIndex">The zero-based band index of the value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public UInt32 ReadValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            /// <summary>
            /// Reads a sequence of spectral values from the dataset.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <returns>The array containing the sequence of values in the default order of the dataset.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            /// <summary>
            /// Reads a sequence of spectral values from the dataset.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            /// <summary>
            /// Reads a sequence of spectral values from the dataset.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <returns>The array containing the sequence of values in the default order of the dataset.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            /// <summary>
            /// Reads a sequence of spectral values from the dataset.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            #endregion

            #region IRasterService methods for reading float value

            /// <summary>
            /// Reads the specified spectral value from the dataset.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the value.</param>
            /// <param name="columnIndex">The zero-based column index of the value.</param>
            /// <param name="bandIndex">The zero-based band index of the value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public Double ReadFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            /// <summary>
            /// Reads a sequence of spectral values from the dataset.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <returns>The array containing the sequence of values in the default order of the dataset.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            /// <summary>
            /// Reads a sequence of spectral values from the dataset.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            /// <summary>
            /// Reads a sequence of spectral values from the dataset.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            /// <summary>
            /// Reads a sequence of spectral values from the dataset.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <returns>The array containing the sequence of values in the default order of the dataset.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not readable.</exception>
            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                throw new NotSupportedException("The dataset is not readable.");
            }

            #endregion

            #region IRasterService methods for writing integer values

            /// <summary>
            /// Writes the specified spectral value to the service.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The service is not writable.</exception>
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
            public void WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
            {
                if (rowIndex < 0)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is less than 0.");
                if (rowIndex >= Dimensions.NumberOfRows)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is less than 0.");
                if (columnIndex >= Dimensions.NumberOfColumns)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is less than 0.");
                if (bandIndex >= Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is equal to or greater than the number of bands.");

                _rawImageWriter.WriteValue(rowIndex, columnIndex, bandIndex, spectralValue);
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service is not writable.</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// </exception>
            public void WriteValueSequence(Int32 startIndex, UInt32[] spectralValues)
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is less than 0.");
                if (startIndex >= Dimensions.NumberOfRows * Dimensions.NumberOfColumns * Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is equal to or greater than the number of values.");

                _rawImageWriter.WriteValueSequence(startIndex, spectralValues);
            }

            /// <summary>
            /// <summary>
            /// Writes the specified spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">
            /// The service is not writable.
            /// or
            /// The writing order is not supported.
            /// </exception>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// </exception>
            public void WriteValueSequence(Int32 startIndex, UInt32[] spectralValues, RasterDataOrder writeOrder)
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is less than 0.");
                if (startIndex >= Dimensions.NumberOfRows * Dimensions.NumberOfColumns * Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is equal to or greater than the number of values.");
                if (writeOrder != DataOrder)
                    throw new NotSupportedException("The specified reading order is not supported.");

                _rawImageWriter.WriteValueSequence(startIndex, spectralValues, writeOrder);
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service is not writable.</exception>
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
            public void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues)
            {
                if (rowIndex < 0)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is less than 0.");
                if (rowIndex >= Dimensions.NumberOfRows)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is less than 0.");
                if (columnIndex >= Dimensions.NumberOfColumns)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is less than 0.");
                if (bandIndex >= Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is equal to or greater than the number of bands.");

                _rawImageWriter.WriteValueSequence(rowIndex, columnIndex, bandIndex, spectralValues);
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">
            /// The service is not writable.
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
            public void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues, RasterDataOrder writeOrder)
            {
                if (rowIndex < 0)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is less than 0.");
                if (rowIndex >= Dimensions.NumberOfRows)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is less than 0.");
                if (columnIndex >= Dimensions.NumberOfColumns)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is less than 0.");
                if (bandIndex >= Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is equal to or greater than the number of bands.");
                if (writeOrder != DataOrder)
                    new NotSupportedException("The specified reading order is not supported.");

                _rawImageWriter.WriteValueSequence(rowIndex, columnIndex, bandIndex, spectralValues, writeOrder);
            }

            #endregion

            #region IRasterService methods for writing floating values

            /// <summary>
            /// Writes the specified spectral value to the service.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The service is not writable.</exception>
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
            public void WriteFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
            {
                if (rowIndex < 0)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is less than 0.");
                if (rowIndex >= Dimensions.NumberOfRows)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is less than 0.");
                if (columnIndex >= Dimensions.NumberOfColumns)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is less than 0.");
                if (bandIndex >= Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is equal to or greater than the number of bands.");

                _rawImageWriter.WriteValue(rowIndex, columnIndex, bandIndex, spectralValue);
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service is not writable.</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// </exception>
            public void WriteFloatValueSequence(Int32 startIndex, Double[] spectralValues)
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is less than 0.");
                if (startIndex >= Dimensions.NumberOfRows * Dimensions.NumberOfColumns * Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is equal to or greater than the number of values.");

                _rawImageWriter.WriteValueSequence(startIndex, spectralValues);
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">
            /// The service is not writable.
            /// or
            /// The writing order is not supported.
            /// </exception>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// </exception>
            void IRasterService.WriteFloatValueSequence(Int32 startIndex, Double[] spectralValues, RasterDataOrder writeOrder)
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is less than 0.");
                if (startIndex >= Dimensions.NumberOfRows * Dimensions.NumberOfColumns * Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is equal to or greater than the number of values.");
                if (writeOrder != DataOrder)
                    throw new NotSupportedException("The specified reading order is not supported.");

                _rawImageWriter.WriteValueSequence(startIndex, spectralValues, writeOrder);
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service is not writable.</exception>
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
            public void WriteFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues)
            {
                if (rowIndex < 0)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is less than 0.");
                if (rowIndex >= Dimensions.NumberOfRows)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is less than 0.");
                if (columnIndex >= Dimensions.NumberOfColumns)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is less than 0.");
                if (bandIndex >= Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is equal to or greater than the number of bands.");

                _rawImageWriter.WriteValueSequence(rowIndex, columnIndex, bandIndex, spectralValues);
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">
            /// The service is not writable.
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
            public void WriteFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues, RasterDataOrder writeOrder)
            {
                if (rowIndex < 0)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is less than 0.");
                if (rowIndex >= Dimensions.NumberOfRows)
                    throw new ArgumentOutOfRangeException("rowIndex", "Row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is less than 0.");
                if (columnIndex >= Dimensions.NumberOfColumns)
                    throw new ArgumentOutOfRangeException("columnIndex", "Column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is less than 0.");
                if (bandIndex >= Dimensions.NumberOfBands)
                    throw new ArgumentOutOfRangeException("bandIndex", "Band index is equal to or greater than the number of bands.");
                if (writeOrder != DataOrder)
                    new NotSupportedException("The specified reading order is not supported.");

                _rawImageWriter.WriteValueSequence(rowIndex, columnIndex, bandIndex, spectralValues, writeOrder);
            }

            #endregion
        }

        #endregion

        #region Constant fields

        /// <summary>
        /// The maximum number of bytes to be written into the file in a single operation. This field is constant.
        /// </summary>
        private const Int32 MaxWritableByteCount = 1 << 24;

        #endregion

        #region Protected fields

        /// <summary>
        /// The byte order of the file.
        /// </summary>
        protected ByteOrder _byteOrder;

        /// <summary>
        /// The layout of the file.
        /// </summary>
        protected RawImageLayout _layout;

        /// <summary>
        /// The raster format.
        /// </summary>
        protected RasterFormat _format;

        /// <summary>
        /// The number of bands.
        /// </summary>
        protected Int32 _numberOfBands;

        /// <summary>
        /// The number of rows.
        /// </summary>
        protected Int32 _numberOfRows;

        /// <summary>
        /// The number of columns.
        /// </summary>
        protected Int32 _numberOfColumns;

        /// <summary>
        /// The radiometric resolution of the raster.
        /// </summary>
        protected Int32 _radiometricResolution;

        /// <summary>
        /// The number of bytes gap per band.
        /// </summary>
        protected Int32 _bytesGapPerBand;

        /// <summary>
        /// The number of bytes per band row.
        /// </summary>
        protected Int32 _bytesPerBandRow;

        /// <summary>
        /// The  number of bytes per row.
        /// </summary>
        protected Int32 _bytesPerRow;

        /// <summary>
        /// The number of bytes skipped.
        /// </summary>
        protected Int32 _bytesSkipped;

        /// <summary>
        /// The raster mapper.
        /// </summary>
        protected RasterMapper _mapper;

        /// <summary>
        /// The X coordinate of the upper left pixel.
        /// </summary>
        protected Double _upperLeftX;

        /// <summary>
        /// The Y coordinate of the upper left pixel.
        /// </summary>
        protected Double _upperLeftY;

        /// <summary>
        /// The X vector.
        /// </summary>
        protected Double _vectorX;

        /// <summary>
        /// The Y vector.
        /// </summary>
        protected Double _vectorY;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public RawImageWriter(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GenericRawImage, parameters)
        {
            SetParameters(parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public RawImageWriter(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GenericRawImage, parameters)
        {
            SetParameters(parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageWriter" /> class.
        /// </summary>
        /// <param name="path">The stream to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The stream is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public RawImageWriter(Stream stream, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, SpectralGeometryStreamFormats.GenericRawImage, parameters)
        {
            SetParameters(parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <param name="format">The format of the stream reader.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        protected RawImageWriter(String path, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, format, parameters)
        {
            if (parameters != null)
            {
                SetParameters(parameters);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="format">The format of the stream reader.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        protected RawImageWriter(Uri path, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, format, parameters)
        {
            if (parameters != null)
            {
                SetParameters(parameters);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawImageWriter" /> class.
        /// </summary>
        /// <param name="path">The stream to be read.</param>
        /// <param name="format">The format of the stream reader.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The stream is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream reading.</exception>
        protected RawImageWriter(Stream stream, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, format, parameters)
        {
            if (parameters != null)
            {
                SetParameters(parameters);
            }
        }


        #endregion

        #region Protected GeometryStreamWriter methods

        /// <summary>
        /// Apply the write operation for a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        protected override void ApplyWriteGeometry(IGeometry geometry)
        {
            try
            {
                WriteGeometryReferenceSystem(geometry.ReferenceSystem);
                IRaster raster = (geometry as ISpectralGeometry).Raster;
                _numberOfColumns = raster.NumberOfColumns;
                _numberOfRows = raster.NumberOfRows;
                _numberOfBands = raster.NumberOfBands;
                _radiometricResolution = raster.RadiometricResolution;
                _mapper = raster.Mapper;
                _upperLeftX = _mapper.Translation.X;
                _upperLeftY = _mapper.Translation.Y;
                _vectorX = _mapper.RowSize;
                _vectorY = _mapper.ColumnSize;
                _format = raster.Format;

                WriteRasterContent(geometry.ReferenceSystem, raster);
                WriteGeometryMetadata(geometry.Metadata);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Stream content is invalid.", ex);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the parameters of the format.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        private void SetParameters(IDictionary<GeometryStreamParameter, Object> parameters)
        {
            _layout = ResolveParameter<RawImageLayout>(SpectralGeometryStreamParameters.Layout);
            _byteOrder = ResolveParameter<ByteOrder>(SpectralGeometryStreamParameters.ByteOrder);
            _bytesPerBandRow = Convert.ToInt32(Math.Ceiling(_numberOfColumns * _radiometricResolution / 8.0));
            switch (_layout)
            {
                case RawImageLayout.BandInterlevedByLine:
                    _bytesPerRow = _numberOfBands * _bytesPerBandRow;
                    break;
                case RawImageLayout.BandInterlevedByPixel:
                    _bytesPerRow = (Int32)Math.Ceiling(_numberOfColumns * _numberOfBands * _radiometricResolution / 8.0);
                    break;
                case RawImageLayout.BandSequential:
                    _bytesPerRow = (Int32)Math.Ceiling(_numberOfColumns * _radiometricResolution / 8.0);
                    break;
            }
        }

        /// <summary>
        /// Writes the content of the raster.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="raster">The raster of the geometry.</param>
        private void WriteRasterContent(IReferenceSystem referenceSystem, IRaster raster)
        {
            Int32 numberOfStrips;
            switch (_layout)
            {
                case RawImageLayout.BandInterlevedByLine:
                case RawImageLayout.BandInterlevedByPixel:
                    // prevent from writing too much bytes into the memory at once
                    numberOfStrips = _bytesPerBandRow / MaxWritableByteCount;

                    for (Int32 i = 0; i < numberOfStrips; i++)
                    {
                        WriteStrip(MaxWritableByteCount, raster);
                    }
                    WriteStrip(_bytesPerRow - numberOfStrips * MaxWritableByteCount, raster);
                    break;
                case RawImageLayout.BandSequential:
                    // prevent from writing too much bytes into the memory at once
                    numberOfStrips = _bytesPerRow / MaxWritableByteCount;

                    for (Int32 i = 0; i < numberOfStrips; i++)
                    {
                        WriteStrip(MaxWritableByteCount, raster);
                    }
                    WriteStrip(_bytesPerRow - numberOfStrips * MaxWritableByteCount, raster);
                    break;
            }
        }

        /// <summary>
        /// Write a strip to the file.
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes in the strip.</param>
        /// <param name="raster">The raster.</param>
        private void WriteStrip(Int32 numberOfBytes, IRaster raster)
        {
            Byte[] stripBytes = new Byte[numberOfBytes];

            Int32 numberOfBytesLeft = _numberOfRows * _numberOfColumns * _radiometricResolution * _numberOfBands, byteIndex = 0, bitIndex = 8;

            switch (_layout)
            {
                case RawImageLayout.BandInterlevedByLine:
                    for (Int32 rowIndex = 0; rowIndex < _numberOfRows; rowIndex++)
                    {
                        for (Int32 bandIndex = 0; bandIndex < _numberOfBands; bandIndex++)
                        {
                            for (Int32 columnIndex = 0; columnIndex < _numberOfColumns; columnIndex++)
                            {
                                WriteValue(ref stripBytes, ref byteIndex, ref bitIndex, raster, rowIndex, columnIndex, bandIndex);

                                if (byteIndex == stripBytes.Length)
                                {
                                    _baseStream.Write(stripBytes, 0, stripBytes.Length);
                                    byteIndex = 0;
                                }
                            }
                        }
                    }
                    break;
                case RawImageLayout.BandInterlevedByPixel:
                    for (Int32 rowIndex = 0; rowIndex < _numberOfRows; rowIndex++)
                    {
                        for (Int32 columnIndex = 0; columnIndex < _numberOfColumns; columnIndex++)
                        {
                            for (Int32 bandIndex = 0; bandIndex < _numberOfBands; bandIndex++)
                            {
                                WriteValue(ref stripBytes, ref byteIndex, ref bitIndex, raster, rowIndex, columnIndex, bandIndex);

                                if (byteIndex == stripBytes.Length)
                                {
                                    _baseStream.Write(stripBytes, 0, stripBytes.Length);
                                    byteIndex = 0;
                                }
                            }
                        }
                    }
                    break;
                case RawImageLayout.BandSequential:
                    for (Int32 bandIndex = 0; bandIndex < _numberOfBands; bandIndex++)
                    {
                        for (Int32 rowIndex = 0; rowIndex < _numberOfRows; rowIndex++)
                        {
                            for (Int32 columnIndex = 0; columnIndex < _numberOfColumns; columnIndex++)
                            {
                                WriteValue(ref stripBytes, ref byteIndex, ref bitIndex, raster, rowIndex, columnIndex, bandIndex);

                                if (byteIndex == stripBytes.Length)
                                {
                                    _baseStream.Write(stripBytes, 0, stripBytes.Length);
                                    byteIndex = 0;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Writes the value from the raster to the array.
        /// </summary>
        /// <param name="stripBytes">The strip bytes.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="raster">The raster.</param>
        /// <param name="rowIndex">The zero-based row index in the raster.</param>
        /// <param name="columnIndex">The zero-based column index in the raster.</param>
        /// <param name="bandIndex">The zero-based band index in the raster.</param>
        private void WriteValue(ref Byte[] stripBytes, ref Int32 byteIndex, ref Int32 bitIndex, IRaster raster, Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (_radiometricResolution < 8 && 8 % _radiometricResolution == 0)
            {
                UInt32 value = raster.GetValue(rowIndex, columnIndex, bandIndex);
                bitIndex -= _radiometricResolution;
                stripBytes[byteIndex] += (Byte)(value << bitIndex);
                if (bitIndex == 0)
                {
                    bitIndex = 8;
                    byteIndex++;
                }
            }
            else if (_radiometricResolution == 8)
            {
                Byte[] bytes = EndianBitConverter.GetBytes(raster.GetValue(rowIndex, columnIndex, bandIndex));
                Array.Copy(bytes, 0, stripBytes, byteIndex, sizeof(Byte));
                byteIndex++;
            }
            else if (_radiometricResolution == 16)
            {
                Byte[] bytes = EndianBitConverter.GetBytes(raster.GetValue(rowIndex, columnIndex, bandIndex));
                Array.Copy(bytes, 0, stripBytes, byteIndex, sizeof(UInt16));
                byteIndex += 2;
            }
            else if (_radiometricResolution == 32)
            {
                Byte[] bytes = EndianBitConverter.GetBytes(raster.GetValue(rowIndex, columnIndex, bandIndex));
                Array.Copy(bytes, 0, stripBytes, byteIndex, sizeof(UInt32));
                byteIndex += 4;
            }
        }

        /// <summary>
        /// Writes the spectral value to the array.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="spectralValue">The spectral value.</param>
        private void WriteBytes(ref Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, UInt32 spectralValue)
        {
            if (_radiometricResolution < 8 && 8 % _radiometricResolution == 0)
            {
                bitIndex -= _radiometricResolution;
                bytes[byteIndex] += (Byte)(spectralValue << bitIndex);
                if (bitIndex == 0)
                {
                    bitIndex = 8;
                    byteIndex++;
                }
            }
            else if (_radiometricResolution == 8)
            {
                Byte[] convertedBytes = EndianBitConverter.GetBytes(spectralValue);
                Array.Copy(convertedBytes, 0, bytes, byteIndex, sizeof(Byte));
                byteIndex++;
            }
            else if (_radiometricResolution == 16)
            {
                Byte[] convertedBytes = EndianBitConverter.GetBytes(spectralValue);
                Array.Copy(convertedBytes, 0, bytes, byteIndex, sizeof(UInt16));
                byteIndex += 2;
            }
            else if (_radiometricResolution == 32)
            {
                Byte[] convertedBytes = EndianBitConverter.GetBytes(spectralValue);
                Array.Copy(convertedBytes, 0, bytes, byteIndex, sizeof(UInt32));
                byteIndex += 4;
            }
        }

        /// <summary>
        /// Writes the spectral value to the array.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="byteIndex">The zero-based byte index in the strip array.</param>
        /// <param name="bitIndex">The zero-based bit index in the strip array.</param>
        /// <param name="spectralValue">The spectral value.</param>
        private void WriteFloatBytes(ref Byte[] bytes, ref Int32 byteIndex, ref Int32 bitIndex, Double spectralValue)
        {
            if (_radiometricResolution <= 32)
            {
                Byte[] convertedBytes = EndianBitConverter.GetBytes(spectralValue);
                Array.Copy(convertedBytes, 0, bytes, byteIndex, sizeof(Single));
                byteIndex += 2;
            }
            else if (_radiometricResolution == 64)
            {
                Byte[] convertedBytes = EndianBitConverter.GetBytes(spectralValue);
                Array.Copy(convertedBytes, 0, bytes, byteIndex, sizeof(Double));
                byteIndex += 4;
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Writes the reference system of the raster geometry.
        /// </summary>
        protected virtual void WriteGeometryReferenceSystem(IReferenceSystem referenceSystem)
        {
        }

        /// <summary>
        /// Writes the metadata of the raster geometry.
        /// </summary>
        protected virtual void WriteGeometryMetadata(IDictionary<string, object> metadata)
        {
        }

        #region Writing methods for integer representation

        /// <summary>
        /// Writes the specified spectral value to the stream.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="bandIndex">The zero-based band index.</param>
        /// <param name="spectralValue">The spectral value.</param>
        protected void WriteValue(int rowIndex, int columnIndex, int bandIndex, UInt32 spectralValue)
        {
            Byte[] bytes = (_radiometricResolution < 8) ? new Byte[1] : new Byte[(_radiometricResolution / 8)];

            Int32 index = 0;
            Int32 bitIndex = 8;
            switch (_layout)
            {
                case (RawImageLayout.BandInterlevedByLine):
                    {
                        if (_radiometricResolution < 8)
                            index = (_numberOfBands * _numberOfColumns * rowIndex + bandIndex * _numberOfColumns + columnIndex) * _radiometricResolution / 8;
                        else
                            index = (_numberOfBands * _numberOfColumns * rowIndex + bandIndex * _numberOfColumns + columnIndex) * (_radiometricResolution / 8);

                        bitIndex = 8 - columnIndex * _radiometricResolution % 8;
                    }
                    break;
                case (RawImageLayout.BandInterlevedByPixel):
                    {
                        if (_radiometricResolution < 8)
                            index = ((rowIndex * _numberOfColumns + columnIndex) * _numberOfBands + bandIndex) * _radiometricResolution / 8;
                        else
                            index = ((rowIndex * _numberOfColumns + columnIndex) * _numberOfBands + bandIndex) * (_radiometricResolution / 8);

                        bitIndex = 8 - bandIndex * _radiometricResolution % 8;

                    }
                    break;
                case (RawImageLayout.BandSequential):
                    {
                        if (_radiometricResolution < 8)
                            index = (bandIndex * _numberOfRows * _numberOfColumns + rowIndex * _numberOfColumns + columnIndex) * _radiometricResolution / 8;
                        else
                            index = (bandIndex * _numberOfRows * _numberOfColumns + rowIndex * _numberOfColumns + columnIndex) * (_radiometricResolution / 8);

                        bitIndex = 8 - columnIndex * _radiometricResolution / 8;
                    }
                    break;
                default:
                    break;
            }

            Int32 byteIndex = 0;
            WriteBytes(ref bytes, ref byteIndex, ref bitIndex, spectralValue);
            _baseStream.Seek(index, SeekOrigin.Begin);
            _baseStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes a sequence of spectral values to the stream in the default order.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="bandIndex">The zero-based band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        protected void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues)
        {
            Int32 index = 0;
            switch (_layout)
            {
                case (RawImageLayout.BandInterlevedByLine):
                    {
                        index = _numberOfBands * _numberOfColumns * rowIndex + bandIndex * _numberOfColumns + columnIndex;
                    }
                    break;
                case (RawImageLayout.BandInterlevedByPixel):
                    {
                        index = (rowIndex * _numberOfColumns + columnIndex) * _numberOfBands + bandIndex;
                    }
                    break;
                case (RawImageLayout.BandSequential):
                    {
                        index = bandIndex * _numberOfRows * _numberOfColumns + rowIndex * _numberOfColumns + columnIndex;
                    }
                    break;
                default:
                    break;
            }

            WriteValueSequence(index, spectralValues);
        }

        /// <summary>
        /// Writes the specified spectral values to the stream in the default order.
        /// </summary>
        /// <param name="startIndex">The zero-based start index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        protected void WriteValueSequence(Int32 startIndex, UInt32[] spectralValues)
        {
            Byte[] bytes = (_radiometricResolution < 8) ? new Byte[spectralValues.Length] : new Byte[spectralValues.Length * (_radiometricResolution / 8)];
            Int32 index;
            if (_radiometricResolution < 8)
                index = startIndex * _radiometricResolution / 8;
            else
                index = startIndex * (_radiometricResolution / 8);

            Int32 byteIndex = 0;
            Int32 bitIndex = 8;
            UInt32[] list = new UInt32[spectralValues.Length];
            for (Int32 i = 0; i < list.Length; i++)
            {
                if (byteIndex < bytes.Length)
                {
                    WriteBytes(ref bytes, ref byteIndex, ref bitIndex, spectralValues[i]);
                }
            }
            _baseStream.Seek(index, SeekOrigin.Begin);
            _baseStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the specified spectral values to the stream in the specified order.
        /// </summary>
        /// <param name="startIndex">The zero-based start index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">The writing order is not supported.</exception>
        protected void WriteValueSequence(Int32 startIndex, UInt32[] spectralValues, RasterDataOrder writeOrder)
        {
            if (!((_layout == RawImageLayout.BandInterlevedByLine && writeOrder == RasterDataOrder.RowBandColumn)
               || (_layout == RawImageLayout.BandSequential && writeOrder == RasterDataOrder.RowColumnBand)
               || (_layout == RawImageLayout.BandInterlevedByPixel && writeOrder == RasterDataOrder.BandRowColumn)))
                WriteValueSequence(startIndex, spectralValues);
            else
                throw new NotSupportedException("The specified writing order is not supported.");
        }

        /// <summary>
        /// Writes a sequence of spectral values to the stream in the specified order.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="bandIndex">The zero-based band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">The writing order is not supported.</exception>
        protected void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues, RasterDataOrder writeOrder)
        {
            if (!((_layout == RawImageLayout.BandInterlevedByLine && writeOrder == RasterDataOrder.RowBandColumn)
              || (_layout == RawImageLayout.BandSequential && writeOrder == RasterDataOrder.RowColumnBand)
              || (_layout == RawImageLayout.BandInterlevedByPixel && writeOrder == RasterDataOrder.BandRowColumn)))
                WriteValueSequence(rowIndex, columnIndex, bandIndex, spectralValues);
            else
                throw new NotSupportedException("The specified writing order is not supported.");
        }

        #endregion

        #region Writing methods for floating representation

        /// <summary>
        /// Writes the specified spectral value to the stream.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="bandIndex">The zero-based band index.</param>
        /// <param name="spectralValue">The spectral value.</param>
        protected void WriteValue(int rowIndex, int columnIndex, int bandIndex, Double spectralValue)
        {
            Byte[] bytes = (_radiometricResolution < 8) ? new Byte[1] : new Byte[(_radiometricResolution / 8)];
            Int32 index = 0;
            Int32 bitIndex = 8;
            switch (_layout)
            {
                case (RawImageLayout.BandInterlevedByLine):
                    {
                        if (_radiometricResolution < 8)
                            index = (_numberOfBands * _numberOfColumns * rowIndex + bandIndex * _numberOfColumns + columnIndex) * _radiometricResolution / 8;
                        else
                            index = (_numberOfBands * _numberOfColumns * rowIndex + bandIndex * _numberOfColumns + columnIndex) * (_radiometricResolution / 8);

                        bitIndex = 8 - columnIndex * _radiometricResolution % 8;
                    }
                    break;
                case (RawImageLayout.BandInterlevedByPixel):
                    {
                        if (_radiometricResolution < 8)
                            index = ((rowIndex * _numberOfColumns + columnIndex) * _numberOfBands + bandIndex) * _radiometricResolution / 8;
                        else
                            index = ((rowIndex * _numberOfColumns + columnIndex) * _numberOfBands + bandIndex) * (_radiometricResolution / 8);

                        bitIndex = 8 - bandIndex * _radiometricResolution % 8;
                    }
                    break;
                case (RawImageLayout.BandSequential):
                    {
                        if (_radiometricResolution < 8)
                            index = (bandIndex * _numberOfRows * _numberOfColumns + rowIndex * _numberOfColumns + columnIndex) * _radiometricResolution / 8;
                        else
                            index = (bandIndex * _numberOfRows * _numberOfColumns + rowIndex * _numberOfColumns + columnIndex) * (_radiometricResolution / 8);

                        bitIndex = 8 - bandIndex * _radiometricResolution % 8;
                    }
                    break;
                default:
                    break;
            }

            Int32 byteIndex = 0;
            WriteFloatBytes(ref bytes, ref byteIndex, ref bitIndex, spectralValue);

            _baseStream.Seek(index, SeekOrigin.Begin);
            _baseStream.Write(bytes, 0, bytes.Length);

        }

        /// <summary>
        /// Writes the specified spectral values to the stream in the default order.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="bandIndex">The zero-based band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        protected void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues)
        {
            Byte[] bytes = (_radiometricResolution < 8) ? new Byte[spectralValues.Length * _numberOfBands] : new Byte[spectralValues.Length * (_radiometricResolution / 8) * _numberOfBands];
            Int32 index = 0;
            switch (_layout)
            {
                case (RawImageLayout.BandInterlevedByLine):
                    {
                        index = _numberOfBands * _numberOfColumns * rowIndex + bandIndex * _numberOfColumns + columnIndex;
                    }
                    break;
                case (RawImageLayout.BandInterlevedByPixel):
                    {
                        index = (rowIndex * _numberOfColumns + columnIndex) * _numberOfBands + bandIndex;
                    }
                    break;
                case (RawImageLayout.BandSequential):
                    {
                        index = bandIndex * _numberOfRows * _numberOfColumns + rowIndex * _numberOfColumns + columnIndex;
                    }
                    break;
                default:
                    break;
            }

            WriteValueSequence(index, spectralValues);
        }

        /// <summary>
        /// Writes the specified spectral values to the stream in the default order.
        /// </summary>
        /// <param name="startIndex">The zero-based start index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        protected void WriteValueSequence(Int32 startIndex, Double[] spectralValues)
        {
            Byte[] bytes = (_radiometricResolution < 8) ? new Byte[spectralValues.Length] : new Byte[spectralValues.Length * (_radiometricResolution / 8)];

            Int32 index;
            if (_radiometricResolution < 8)
                index = startIndex * _radiometricResolution / 8;
            else
                index = startIndex * (_radiometricResolution / 8);

            Int32 byteIndex = 0;
            Int32 bitIndex = 8;
            Double[] list = new Double[spectralValues.Length];
            for (Int32 i = 0; i < list.Length; i++)
            {
                if (byteIndex < bytes.Length)
                {
                    WriteFloatBytes(ref bytes, ref byteIndex, ref bitIndex, spectralValues[i]);
                }
            }
            _baseStream.Seek(index, SeekOrigin.Begin);
            _baseStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the specified spectral values to the stream in the specified order.
        /// </summary>
        /// <param name="startIndex">The zero-based start index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The writing order.</param>
        /// <exception cref="System.NotSupportedException">The writing order is not supported.</exception>
        protected void WriteValueSequence(Int32 startIndex, Double[] spectralValues, RasterDataOrder writeOrder)
        {
            if (!((_layout == RawImageLayout.BandInterlevedByLine && writeOrder == RasterDataOrder.RowBandColumn)
              || (_layout == RawImageLayout.BandSequential && writeOrder == RasterDataOrder.RowColumnBand)
              || (_layout == RawImageLayout.BandInterlevedByPixel && writeOrder == RasterDataOrder.BandRowColumn)))
                WriteValueSequence(startIndex, spectralValues);
            else
                throw new NotSupportedException("The specified writing order is not supported.");
        }

        /// <summary>
        /// Writes a sequence of spectral values to the stream in the default order.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="bandIndex">The zero-based band index.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="writeOrder">The write order.</param>
        /// <exception cref="System.NotSupportedException">The writing order is not supported.</exception>
        protected void WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues, RasterDataOrder writeOrder)
        {
            if (!((_layout == RawImageLayout.BandInterlevedByLine && writeOrder == RasterDataOrder.RowBandColumn)
                || (_layout == RawImageLayout.BandSequential && writeOrder == RasterDataOrder.RowColumnBand)
                || (_layout == RawImageLayout.BandInterlevedByPixel && writeOrder == RasterDataOrder.BandRowColumn)))
                WriteValueSequence(rowIndex, columnIndex, bandIndex, spectralValues);
            else
                throw new NotSupportedException("The specified writing order is not supported.");
        }

        #endregion

        #endregion
    }
}
