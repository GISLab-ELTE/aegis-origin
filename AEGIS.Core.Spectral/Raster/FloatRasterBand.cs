/// <copyright file="FloatRasterBand.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Raster
{
    /// <summary>
    /// Represents a band of a multispectral raster image containing floating point values.
    /// </summary>
    public class FloatRasterBand : IFloatRasterBand
    {
        #region Protected fields

        protected readonly IFloatRaster _raster;
        protected readonly Int32 _bandIndex;

        #endregion

        #region IRasterBand properties

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
        public Int32 RadiometricResolution { get { return _raster.RadiometricResolutions[_bandIndex]; } }

        /// <summary>
        /// Gets the histogram values of the band.
        /// </summary>
        /// <value>The histogram values of the band.</value>
        public IList<Int32> HistogramValues { get { return _raster.HistogramValues[_bandIndex]; } }

        /// <summary>
        /// Gets the spectral range of the band.
        /// </summary>
        /// <value>The spectral range of the band.</value>
        public SpectralRange SpectralRange { get { return _raster.SpectralRanges[_bandIndex]; } }

        /// <summary>
        /// Get a value indicating whether the raster band is mapped to coordinate space.
        /// </summary>
        /// <value><c>true</c> if the raster band is mapped to coordinate spacet; otherwise, <c>false</c>.</value>
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
        /// Gets the representation of the raster.
        /// </summary>
        /// <value>The representation of the raster.</value>
        public RasterRepresentation Representation { get { return _raster.Representation; } }

        #endregion

        #region IRasterBand properties (explicit)

        /// <summary>
        /// Gets the raster where the band is located.
        /// </summary>
        /// <value>The raster where the band is located.</value>
        IRaster IRasterBand.Raster { get { return _raster; } }
        /// <summary>
        /// Gets or sets a spectral value at a specified row and column index.
        /// </summary>
        /// <value>The spectral value located at the specified coordinate.</value>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        UInt32 IRasterBand.this[Int32 rowIndex, Int32 columnIndex]
        {
            get { return (UInt32)GetValue(rowIndex, columnIndex); }
            set { SetValue(rowIndex, columnIndex, (UInt32)value); }
        }
        /// <summary>
        /// Gets or sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The zero-based index of the pixel.</param>
        /// <value>The spectral value located at the specified coordinate.</value>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        UInt32 IRasterBand.this[Coordinate coordinate]
        {
            get { return (UInt32)GetValue(coordinate); }
            set { SetValue(coordinate, (UInt32)value); }
        }

        #endregion

        #region IFloatRasterBand properties

        /// <summary>
        /// Gets the raster where the band is located.
        /// </summary>
        /// <value>The raster where the band is located.</value>
        public IFloatRaster Raster { get { return _raster; } }

        /// <summary>
        /// Gets or sets a spectral value at a specified row and column index.
        /// </summary>
        /// <value>The spectral value located at the specified coordinate.</value>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        public Double this[Int32 rowIndex, Int32 columnIndex]
        {
            get { return GetValue(rowIndex, columnIndex); }
            set { SetValue(rowIndex, columnIndex, value); }
        }

        /// <summary>
        /// Gets or sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The zero-based index of the pixel.</param>
        /// <value>The spectral value located at the specified coordinate.</value>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public Double this[Coordinate coordinate]
        {
            get { return GetValue(coordinate); }
            set { SetValue(coordinate, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Raster.RasterBand" /> class.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralRange">The spectral range.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of rows is less than 0.;numberOfRows
        /// or
        /// The number of columns is less than 0.;numberOfColumns
        /// or
        /// The radiometric resolution is less than 1.;radiometricResolution
        /// or
        /// The radiometric resolution is greater than 16.;radiometricResolution
        /// </exception>
        public FloatRasterBand(IFloatRaster raster, Int32 bandIndex)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");

            _raster = raster;
            _bandIndex = bandIndex;
        }

        #endregion

        #region IRasterBand methods (explicit)

        /// <summary>
        /// Sets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <param name="value">The spectral value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        void IRasterBand.SetValue(Int32 rowIndex, Int32 columnIndex, UInt32 value)
        {
            _raster.SetValue(rowIndex, columnIndex, _bandIndex, (Double)value);
        }
        /// <summary>
        /// Sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="value">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        void IRasterBand.SetValue(Coordinate coordinate, UInt32 value)
        {
            _raster.SetValue(coordinate, _bandIndex, (Double)value);
        }
        /// <summary>
        /// Gets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        UInt32 IRasterBand.GetValue(Int32 rowIndex, Int32 columnIndex)
        {
            return (UInt32)_raster.GetValue(rowIndex, columnIndex, _bandIndex);
        }
        /// <summary>
        /// Gets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate.</returns>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        UInt32 IRasterBand.GetValue(Coordinate coordinate)
        {
            return (UInt32)_raster.GetValue(coordinate, _bandIndex);
        }
        /// <summary>
        /// Gets the nearest spectral value to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index or in the nearest index if either the row or column indices are out of range.</returns>
        UInt32 IRasterBand.GetNearestValue(Int32 rowIndex, Int32 columnIndex)
        {
            return (UInt32)_raster.GetNearestValue(rowIndex, columnIndex, _bandIndex);
        }
        /// <summary>
        /// Gets the nearest spectral value to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate or at the nearest index if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        UInt32 IRasterBand.GetNearestValue(Coordinate coordinate)
        {
            return (UInt32)_raster.GetNearestValue(coordinate, _bandIndex);
        }

        #endregion

        #region IFloatRasterBand methods

        /// <summary>
        /// Sets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <param name="value">The spectral value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        public void SetValue(Int32 rowIndex, Int32 columnIndex, Double value)
        {
            _raster.SetValue(rowIndex, columnIndex, _bandIndex, value);
        }
        /// <summary>
        /// Sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="value">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public void SetValue(Coordinate coordinate, Double value)
        {
            _raster.SetValue(coordinate, _bandIndex, value);
        }
        /// <summary>
        /// Gets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        public Double GetValue(Int32 rowIndex, Int32 columnIndex)
        {
            return _raster.GetValue(rowIndex, columnIndex, _bandIndex);
        }
        /// <summary>
        /// Gets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate.</returns>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public Double GetValue(Coordinate coordinate)
        {
            return _raster.GetValue(coordinate, _bandIndex);
        }
        /// <summary>
        /// Gets the nearest spectral value to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index or in the nearest index if either the row or column indices are out of range.</returns>
        public Double GetNearestValue(Int32 rowIndex, Int32 columnIndex)
        {
            return _raster.GetNearestValue(rowIndex, columnIndex, _bandIndex);
        }
        /// <summary>
        /// Gets the nearest spectral value to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate or at the nearest index if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        public Double GetNearestValue(Coordinate coordinate)
        {
            return _raster.GetNearestValue(coordinate, _bandIndex);
        }
        
        #endregion        
    }
}
