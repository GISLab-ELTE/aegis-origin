/// <copyright file="RasterBand.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Raster
{
    /// <summary>
    /// Represents a band of a multispectral raster image.
    /// </summary>
    public class RasterBand : IRasterBand
    {
        #region Protected fields

        protected readonly IRaster _raster;
        protected readonly Int32 _bandIndex;

        #endregion

        #region IRasterBand properties

        /// <summary>
        /// Gets the raster where the band is located.
        /// </summary>
        /// <value>The raster where the band is located.</value>
        public IRaster Raster { get { return _raster; } }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        public Int32 NumberOfColumns { get { return _raster.NumberOfColumns; } }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        public Int32 NumberOfRows { get { return _raster.NumberOfRows; } }

        /// <summary>
        /// Gets the radiometric resolution of the band.
        /// </summary>
        /// <value>The number of bits used for storing spectral values.</value>
        public Int32 RadiometricResolution { get { return _raster.RadiometricResolution; } }

        /// <summary>
        /// Gets the histogram values of the band.
        /// </summary>
        /// <value>The histogram values of the band.</value>
        public IReadOnlyList<Int32> HistogramValues { get { return _raster.HistogramValues[_bandIndex]; } }

        /// <summary>
        /// Get a value indicating whether the raster band is mapped to coordinate space.
        /// </summary>
        /// <value><c>true</c> if the raster band is mapped to coordinate space; otherwise, <c>false</c>.</value>
        public Boolean IsMapped { get { return Raster.IsMapped; } }

        /// <summary>
        /// Gets a value indicating whether the raster band is readable.
        /// </summary>
        /// <value><c>true</c> if the raster band is readable; otherwise, <c>false</c>.</value>
        public Boolean IsReadable { get { return Raster.IsReadable; } }

        /// <summary>
        /// Gets a value indicating whether the raster band is writable.
        /// </summary>
        /// <value><c>true</c> if the raster band is writable; otherwise, <c>false</c>.</value>
        public Boolean IsWritable { get { return Raster.IsWritable; } }

        /// <summary>
        /// Gets the format of the raster.
        /// </summary>
        /// <value>The format of the raster.</value>
        public RasterFormat Format { get { return _raster.Format; } }

        /// <summary>
        /// Gets or sets a spectral value at a specified row and column index.
        /// </summary>
        /// <value>The spectral value located at the specified coordinate.</value>
        /// <param name="x">The zero-based column index of the value.</param>
        /// <param name="y">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index.</returns>
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
        public UInt32 this[Int32 rowIndex, Int32 columnIndex] { get { return GetValue(rowIndex, columnIndex); } set { SetValue(rowIndex, columnIndex, value); } }

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
        public UInt32 this[Coordinate coordinate] { get { return GetValue(coordinate); } set { SetValue(coordinate, value); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterBand" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="bandIndex">The zero-based index of the band.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is greater than or equal to the number of bands in the raster.
        /// </exception>
        public RasterBand(IRaster raster, Int32 bandIndex)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bandIndex), "The band index is less than 0.");
            if (bandIndex >= raster.NumberOfBands)
                throw new ArgumentOutOfRangeException(nameof(bandIndex), "The band index is greater than or equal to the number of bands in the raster.");

            _raster = raster;
            _bandIndex = bandIndex;
        }

        #endregion

        #region IRasterBand methods

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
        public void SetValue(Int32 rowIndex, Int32 columnIndex, UInt32 value)
        {
            _raster.SetValue(rowIndex, columnIndex, _bandIndex, value);
        }

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
        public void SetValue(Coordinate coordinate, UInt32 value)
        {
            _raster.SetValue(coordinate, _bandIndex, value);
        }

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
        public void SetFloatValue(Int32 rowIndex, Int32 columnIndex, Double value)
        {
            _raster.SetFloatValue(rowIndex, columnIndex, _bandIndex, value);
        }

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
        public void SetFloatValue(Coordinate coordinate, Double value)
        {
            _raster.SetFloatValue(coordinate, _bandIndex, value);
        }

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
        public UInt32 GetValue(Int32 rowIndex, Int32 columnIndex)
        {
            return _raster.GetValue(rowIndex, columnIndex, _bandIndex);
        }

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
        public UInt32 GetValue(Coordinate coordinate)
        {
            return _raster.GetValue(coordinate, _bandIndex);
        }

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
        public Double GetFloatValue(Int32 rowIndex, Int32 columnIndex)
        {
            return _raster.GetFloatValue(rowIndex, columnIndex, _bandIndex);
        }

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
        public Double GetFloatValue(Coordinate coordinate)
        {
            return _raster.GetFloatValue(coordinate, _bandIndex);
        }

        /// <summary>
        /// Gets the nearest spectral value to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index or in the nearest index if either the row or column indices are out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        public UInt32 GetNearestValue(Int32 rowIndex, Int32 columnIndex)
        {
            return _raster.GetNearestValue(rowIndex, columnIndex, _bandIndex);
        }

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
        public UInt32 GetNearestValue(Coordinate coordinate)
        {
            return _raster.GetNearestValue(coordinate, _bandIndex);
        }

        /// <summary>
        /// Gets the nearest spectral value to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index or in the nearest index if either the row or column indices are out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        public Double GetNearestFloatValue(Int32 rowIndex, Int32 columnIndex)
        {
            return _raster.GetNearestFloatValue(rowIndex, columnIndex, _bandIndex);
        }

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
        public Double GetNearestFloatValue(Coordinate coordinate)
        {
            return _raster.GetNearestFloatValue(coordinate, _bandIndex);
        }
        
        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return "RasterBand [" + NumberOfRows + "x" + NumberOfColumns + "x" + RadiometricResolution + "]";
        }

        #endregion
    }
}
