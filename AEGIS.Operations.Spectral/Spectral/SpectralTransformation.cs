/// <copyright file="SpectralTransformation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a transformation that is applied on the spectral data of the geometry.
    /// </summary>
    public abstract class SpectralTransformation : Operation<ISpectralGeometry, ISpectralGeometry>
    {
        #region Private types

        /// <summary>
        /// Represents the raster service of the spectral transformation.
        /// </summary>
        private class SpectralTransformationService : IRasterService
        {
            #region Private constant fields

            /// <summary>
            /// Tha maximal number of values that can be read. This field is constant.
            /// </summary>
            private const Int32 MaximumNumberOfValues = 2 << 20;

            #endregion

            #region Private fields

            /// <summary>
            /// The operation computing the spectral values of the entity. This field is read-only.
            /// </summary>
            private readonly SpectralTransformation _operation;

            /// <summary>
            /// The array of radiometric resolutions. This field is read-only.
            /// </summary>
            private readonly Int32[] _radiometricResolutions;

            #endregion

            #region IRasterService properties

            /// <summary>
            /// Gets the number of columns.
            /// </summary>
            /// <value>The number of spectral values contained in a row.</value>
            public Int32 NumberOfColumns { get; private set; }

            /// <summary>
            /// Gets the number of rows.
            /// </summary>
            /// <value>The number of spectral values contained in a column.</value>
            public Int32 NumberOfRows { get; private set; }

            /// <summary>
            /// Gets the number of spectral bands.
            /// </summary>
            /// <value>The number of spectral bands contained in the raster.</value>
            public Int32 NumberOfBands { get; private set; }

            /// <summary>
            /// Gets the radiometric resolutions of the bands in the raster.
            /// </summary>
            /// <value>The list containing the radiometric resolution of each band in the raster.</value>
            public IList<Int32> RadiometricResolutions { get { return Array.AsReadOnly(_radiometricResolutions); } }

            /// <summary>
            /// Gets a value indicating whether the service is readable.
            /// </summary>
            /// <value><c>true</c> if the service is readable; otherwise, <c>false</c>.</value>
            public Boolean IsReadable { get { return true; } }

            /// <summary>
            /// Gets a value indicating whether the service is writable.
            /// </summary>
            /// <value><c>true</c> if the service is writable; otherwise, <c>false</c>.</value>
            public Boolean IsWritable { get { return false; } }

            /// <summary>
            /// Gets the format of the service.
            /// </summary>
            /// <value>The format of the service.</value>
            public RasterFormat Format { get; private set; }

            /// <summary>
            /// Gets the supported read/write orders.
            /// </summary>
            /// <value>The list of supported read/write orders.</value>
            public IList<RasterDataOrder> SupportedOrders { get { return Array.AsReadOnly(_supportedOrders); } }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SpectralTransformationService" /> class.
            /// </summary>
            /// <param name="format">The format.</param>
            /// <param name="operation">The operation.</param>
            /// <param name="numberOfBands">The number of spectral bands.</param>
            /// <param name="numberOfRows">The number of rows.</param>
            /// <param name="numberOfColumns">The number of columns.</param>
            /// <param name="radiometricResolutions">The radiometric resolutions.</param>
            public SpectralTransformationService(SpectralTransformation operation, RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions)
            {
                _operation = operation;
                _radiometricResolutions = radiometricResolutions.ToArray();

                NumberOfBands = numberOfBands;
                NumberOfColumns = numberOfColumns;
                NumberOfRows = numberOfRows;
                Format = format;

                if (_supportedOrders == null)
                    _supportedOrders = new RasterDataOrder[] { RasterDataOrder.RowColumnBand };
            }

            #endregion

            #region IRasterService methods for reading integer values

            /// <summary>
            /// Reads the specified spectral value from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the value.</param>
            /// <param name="columnIndex">The zero-based column index of the value.</param>
            /// <param name="bandIndex">The zero-based band index of the value.</param>
            /// <returns>The spectral value at the specified index.</returns>
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
            public UInt32 ReadValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
            {
                return _operation.Compute(rowIndex, columnIndex, bandIndex);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <returns>The array containing the sequence of values in the default order of the service.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// or
            /// The number of values is less than 0.
            /// or
            /// The number of values is greater than the maximum number allowed to be read.
            /// </exception>
            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                return ReadValueSequence(startIndex, numberOfValues, RasterDataOrder.RowColumnBand);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            /// <exception cref="System.NotSupportedException">The specified reading order is not supported.</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// or
            /// The number of values is less than 0.
            /// or
            /// The number of values is greater than the maximum number allowed to be read.
            /// </exception>
            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is less than 0.");
                if (startIndex <= NumberOfRows * NumberOfColumns * NumberOfBands)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is equal to or greater than the number of values.");
                if (numberOfValues < 0)
                    throw new ArgumentOutOfRangeException("numberOfValues", "The number of values is less than 0.");
                if (readOrder != RasterDataOrder.RowColumnBand)
                    throw new NotSupportedException("The specified reading order is not supported.");

                // compute the row/column/band indices from the start index
                Int32 columnIndex = 0, rowIndex = 0, bandIndex = 0;
                rowIndex = startIndex / (NumberOfColumns * NumberOfBands);
                columnIndex = (startIndex - rowIndex * NumberOfColumns * NumberOfBands) / NumberOfBands;
                bandIndex = startIndex - rowIndex * NumberOfColumns * NumberOfBands - columnIndex * NumberOfBands;

                return ReadValueSequence(rowIndex, columnIndex, bandIndex, numberOfValues, readOrder);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <returns>The array containing the sequence of values in the default order of the service.</returns>
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
            /// or
            /// The number of values is greater than the maximum number allowed to be read.
            /// </exception>
            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                return ReadValueSequence(rowIndex, columnIndex, bandIndex, numberOfValues, RasterDataOrder.RowColumnBand);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            /// <exception cref="System.NotSupportedException">The specified reading order is not supported.</exception>
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
            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                if (rowIndex < 0)
                    throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
                if (rowIndex >= NumberOfRows)
                    throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
                if (columnIndex >= NumberOfColumns)
                    throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
                if (bandIndex >= NumberOfBands)
                    throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");
                if (numberOfValues < 0)
                    throw new ArgumentOutOfRangeException("numberOfValues", "The number of values is less than 0.");
                if (readOrder != RasterDataOrder.RowColumnBand)
                    throw new NotSupportedException("The specified reading order is not supported.");

                // there may be not enough values, or the number may be greater than the maximum allowed
                UInt32[] values = new UInt32[Calculator.Min(MaximumNumberOfValues, numberOfValues, (NumberOfRows - rowIndex) * NumberOfColumns * NumberOfBands + (NumberOfColumns - columnIndex) * NumberOfBands + NumberOfBands - bandIndex)];

                Int32 currentIndex = 0;
                while (currentIndex < values.Length)
                { 
                    // read the specified pixel
                    UInt32[] currentValues = _operation.Compute(rowIndex, columnIndex);
                    Array.Copy(currentValues, bandIndex, values, columnIndex, Math.Min(currentValues.Length - bandIndex, values.Length - currentIndex));

                    // change indices for the next pixel
                    bandIndex = 0;
                    columnIndex++;
                    if (columnIndex == NumberOfColumns) { columnIndex = 0; rowIndex++; }

                    currentIndex += currentValues.Length - bandIndex;
                }

                return values;
            }

            #endregion

            #region IRasterService methods for reading floating point values

            /// <summary>
            /// Reads the specified spectral value from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the value.</param>
            /// <param name="columnIndex">The zero-based column index of the value.</param>
            /// <param name="bandIndex">The zero-based band index of the value.</param>
            /// <returns>The spectral value at the specified index.</returns>
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
            public Double ReadFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
            {
                return _operation.ComputeFloat(rowIndex, columnIndex, bandIndex);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <returns>The array containing the sequence of values in the default order of the service.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// or
            /// The number of valies is less than 0.
            /// </exception>
            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                return ReadFloatValueSequence(startIndex, numberOfValues, RasterDataOrder.RowColumnBand);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            /// <exception cref="System.NotSupportedException">The specified reading order is not supported.</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// or
            /// The number of values is less than 0.
            /// </exception>
            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is less than 0.");
                if (startIndex <= NumberOfRows * NumberOfColumns * NumberOfBands)
                    throw new ArgumentOutOfRangeException("startIndex", "The start index is equal to or greater than the number of values.");
                if (numberOfValues < 0)
                    throw new ArgumentOutOfRangeException("numberOfValues", "The number of values is less than 0.");
                if (readOrder != RasterDataOrder.RowColumnBand)
                    throw new NotSupportedException("The specified reading order is not supported.");

                // compute the row/column/band indices from the start index
                Int32 columnIndex = 0, rowIndex = 0, bandIndex = 0;
                rowIndex = startIndex / (NumberOfColumns * NumberOfBands);
                columnIndex = (startIndex - rowIndex * NumberOfColumns * NumberOfBands) / NumberOfBands;
                bandIndex = startIndex - rowIndex * NumberOfColumns * NumberOfBands - columnIndex * NumberOfBands;

                return ReadFloatValueSequence(rowIndex, columnIndex, bandIndex, numberOfValues, readOrder);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="startIndex">The zero-based absolute starting index.</param>
            /// <param name="numberOfValues">The number of values to be read.</param>
            /// <param name="readOrder">The reading order.</param>
            /// <returns>The array containing the sequence of values in the specified order.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The start index is less than 0.
            /// or
            /// The start index is equal to or greater than the number of values.
            /// or
            /// The number of values is less than 0.
            /// </exception>
            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                return ReadFloatValueSequence(rowIndex, columnIndex, bandIndex, numberOfValues, RasterDataOrder.RowColumnBand);
            }

            /// <summary>
            /// Reads a sequence of spectral values from the service.
            /// </summary>
            /// <param name="rowIndex">The zero-based row index of the first value.</param>
            /// <param name="columnIndex">The zero-based column index of the first value.</param>
            /// <param name="bandIndex">The zero-based band index of the first value.</param>
            /// <param name="numberOfValues">The number of values.</param>
            /// <returns>The array containing the sequence of values in the default order of the service.</returns>
            /// <exception cref="System.NotSupportedException">The specified reading order is not supported.</exception>
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
            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, RasterDataOrder readOrder)
            {
                if (rowIndex < 0)
                    throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
                if (rowIndex >= NumberOfRows)
                    throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
                if (columnIndex >= NumberOfColumns)
                    throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
                if (bandIndex >= NumberOfBands)
                    throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");
                if (numberOfValues < 0)
                    throw new ArgumentOutOfRangeException("numberOfValues", "The number of values is less than 0.");
                if (readOrder != RasterDataOrder.RowColumnBand)
                    throw new NotSupportedException("The specified reading order is not supported.");

                // there may be not enough values, or the number may be greater than the maximum allowed
                Double[] values = new Double[Calculator.Min(MaximumNumberOfValues, numberOfValues, (NumberOfRows - rowIndex) * NumberOfColumns * NumberOfBands + (NumberOfColumns - columnIndex) * NumberOfBands + NumberOfBands - bandIndex)];

                Int32 currentIndex = 0;
                while (currentIndex < values.Length)
                {
                    // read the specified pixel
                    Double[] currentValues = _operation.ComputeFloat(rowIndex, columnIndex);
                    Array.Copy(currentValues, bandIndex, values, columnIndex, Math.Min(currentValues.Length - bandIndex, values.Length - currentIndex));

                    // change indices for the next pixel
                    bandIndex = 0;
                    columnIndex++;
                    if (columnIndex == NumberOfColumns) { columnIndex = 0; rowIndex++; }

                    currentIndex += currentValues.Length - bandIndex;
                }

                return values;
            }

            #endregion

            #region IRasterService methods for writing integer values (explicit)

            /// <summary>
            /// Writes the specified spectral value to the service.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValueSequence(Int32 startIndex, UInt32[] spectralValues)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValueSequence(Int32 startIndex, UInt32[] spectralValues, RasterDataOrder writeOrder)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues, RasterDataOrder writeOrder)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            #endregion

            #region IRasterService methods for writing float values (explicit)

            /// <summary>
            /// Writes the specified spectral value to the service.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValueSequence(Int32 startIndex, Double[] spectralValues)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes the specified spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValueSequence(Int32 startIndex, Double[] spectralValues, RasterDataOrder writeOrder)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            /// <summary>
            /// Writes a sequence of spectral values to the service in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The service does not support writing.</exception>
            void IRasterService.WriteFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues, RasterDataOrder writeOrder)
            {
                throw new NotSupportedException("The service does not support writing.");
            }

            #endregion

            #region Private static fields

            /// <summary>
            /// The supported spectral orders.
            /// </summary>
            private static RasterDataOrder[] _supportedOrders;

            #endregion
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralTransformation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// or
        /// The source geometry does not contain raster data.
        /// or
        /// The raster format of the source is not supported by the method.
        /// </exception>
        protected SpectralTransformation(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            if (source.Raster == null)
                throw new ArgumentException("The source geometry does not contain raster data.", "source");
            if (!method.SupportedRepresentations.Contains(source.Raster.Format))
                throw new ArgumentException("The raster format of the source is not supported by the method.", "source");
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _result = _source.Factory.CreateSpectralGeometry(_source, 
                                                             PrepareRasterResult(_source.Raster.Format,
                                                                                 _source.Raster.NumberOfBands,
                                                                                 _source.Raster.NumberOfRows,
                                                                                 _source.Raster.NumberOfColumns,
                                                                                 _source.Raster.RadiometricResolutions,
                                                                                 _source.Raster.Mapper),
                                                             _source.Presentation,
                                                             _source.Imaging);
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            if (_result.Raster.Format == RasterFormat.Floating)
            {
                for (Int32 i = 0; i < _result.Raster.NumberOfRows; i++)
                    for (Int32 j = 0; j < _result.Raster.NumberOfColumns; j++)
                        _result.Raster.SetFloatValues(i, j, ComputeFloat(i, j));
            }
            else
            {
                for (Int32 i = 0; i < _result.Raster.NumberOfRows; i++)
                    for (Int32 j = 0; j < _result.Raster.NumberOfColumns; j++)
                        _result.Raster.SetValues(i, j, Compute(i, j));
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Prepares the raster result of the operation.
        /// </summary>
        /// <param name="numberOfBands">The spectral resolution.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="format">The format.</param>
        /// <returns>The resulting raster.</returns>
        protected IRaster PrepareRasterResult(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
        {
            return PrepareRasterResult(format, numberOfBands, numberOfRows, numberOfColumns, Enumerable.Repeat(radiometricResolution, numberOfBands).ToArray(), mapper);
        }

        /// <summary>
        /// Prepares the raster result of the operation.
        /// </summary>
        /// <param name="numberOfBands">The spectral resolution.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="format">The format.</param>
        /// <returns>The resulting raster.</returns>
        protected IRaster PrepareRasterResult(RasterFormat format, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            if (State == OperationState.Initialized)
            {
                return Factory.DefaultInstance<IRasterFactory>().CreateRaster(new SpectralTransformationService(this, format,
                                                                                                                numberOfBands, numberOfRows, numberOfColumns, 
                                                                                                                radiometricResolutions), 
                                                                              mapper);
            }
            else
            {
                return Factory.DefaultInstance<IRasterFactory>().CreateRaster(format, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper);
            }
        }

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected virtual UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex) 
        {
            throw new NotSupportedException("The specified execution is not supported.");
        }

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected virtual UInt32[] Compute(Int32 rowIndex, Int32 columnIndex)
        {
            throw new NotSupportedException("The specified execution is not supported.");
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected virtual Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            throw new NotSupportedException("The specified execution is not supported.");
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected virtual Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            throw new NotSupportedException("The specified execution is not supported.");
        }

        #endregion
    }
}
