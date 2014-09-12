/// <copyright file="IRaster.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for multispectral raster images.
    /// </summary>
    /// <remarks>
    /// A multispectral raster image is a collection of spectral bands containing spectral values for different ranges in the electromagnetic spectrum with the same extent. 
    /// Rasters have values defined in the raster space and are mapped to coordinate space (or model space) using a <see cref="ELTE.AEGIS.RasterMapper" /> instance.
    /// </remarks>
    public interface IRaster : ICloneable
    {
        #region Properties

        /// <summary>
        /// Gets the factory of the raster.
        /// </summary>
        /// <value>The factory implementation the raster was constructed by.</value>
        IRasterFactory Factory { get; }

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
        /// Gets the number of spectral bands.
        /// </summary>
        /// <value>The number of spectral bands contained in the raster.</value>
        Int32 NumberOfBands { get; }

        /// <summary>
        /// Gets the spectral bands of the raster.
        /// </summary>
        /// <value>The read-only list containing the spectral bands in the raster.</value>
        IList<IRasterBand> Bands { get; }

        /// <summary>
        /// Gets the radiometric resolutions of the bands in the raster.
        /// </summary>
        /// <value>The list containing the radiometric resolution of each band in the raster.</value>
        IList<Int32> RadiometricResolutions { get; }

        /// <summary>
        /// Gets the histogram values of the raster.
        /// </summary>
        /// <value>The read-only list containing the histogram values of each band in the raster.</value>
        /// <exception cref="System.NotSupportedException">Histogram query is not supported.</exception>
        IList<IList<Int32>> HistogramValues { get; }

        /// <summary>
        /// Gets the bounding coordinates of the raster.
        /// </summary>
        /// <value>The list containing the bounding coordinates of the raster in counterclockwise order.</value>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        IList<Coordinate> Coordinates { get; }

        /// <summary>
        /// Gets the raster mapper used for mapping raster space to coordinate space.
        /// </summary>
        /// <value>The raster mapper associated with the raster.</value>
        RasterMapper Mapper { get; }

        /// <summary>
        /// Get a value indicating whether the raster is mapped to coordinate space.
        /// </summary>
        /// <value><c>true</c> if the raster is mapped to coordinate spacet; otherwise, <c>false</c>.</value>
        Boolean IsMapped { get; }

        /// <summary>
        /// Gets a value indicating whether the raster is readable.
        /// </summary>
        /// <value><c>true</c> if the raster is readable; otherwise, <c>false</c>.</value>
        Boolean IsReadable { get; }

        /// <summary>
        /// Gets a value indicating whether the raster is writable.
        /// </summary>
        /// <value><c>true</c> if the raster is writable; otherwise, <c>false</c>.</value>
        Boolean IsWritable { get; }

        /// <summary>
        /// Gets the representation of the raster.
        /// </summary>
        /// <value>The representation of the raster.</value>
        RasterRepresentation Representation { get; }

        /// <summary>
        /// Gets a raster band with the specified index.
        /// </summary>
        /// <value>The raster band at the specified index.</value>
        /// <param name="index">The zero-based index of the band.</param>
        /// <exception cref="System.IndexOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of bands in the raster.
        /// </exception>
        IRasterBand this[Int32 index] { get; }

        /// <summary>
        /// Gets or sets a spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="columnIndex">The zero-based row index of the values.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <value>The array containing the spectral values for each band.</value>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The raster is not writable.
        /// </exception>
        /// <exception cref="System.IndexOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        UInt32 this[Int32 rowIndex, Int32 columnIndex, Int32 bandIndex] { get; set; }

        /// <summary>
        /// Gets or sets a spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="columnIndex">The zero-based row index of the values.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <value>The array containing the spectral values for each band.</value>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The raster is not writable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.IndexOutOfRangeException">
        /// The coordinate is not within the raster.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        UInt32 this[Coordinate coordinate, Int32 bandIndex] { get; set; }

        /// <summary>
        /// Gets or sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="columnIndex">The zero-based row index of the values.</param>
        /// <value>The array containing the spectral values for each band.</value>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The raster is not writable.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The spectral values are not specified.
        /// or
        /// The number of spectral values does not match the number of bands.
        /// </exception>
        /// <exception cref="System.IndexOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
        /// </exception>
        UInt32[] this[Int32 rowIndex, Int32 columnIndex] { get; set; }

        /// <summary>
        /// Gets or sets all spectral values at a specified coordinate.
        /// </summary>
        /// <value>The array containing the spectral values for each band.</value>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The raster is not writable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The spectral values are not specified.
        /// or
        /// The number of spectral values does not match the number of bands.
        /// </exception>
        /// <exception cref="System.IndexOutOfRangeException">The coordinate is not within the raster.</exception>
        UInt32[] this[Coordinate coordinate] { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a band at a specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the band.</param>
        /// <returns>The raster band at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of bands in the raster.
        /// </exception>
        IRasterBand GetBand(Int32 index);
        
        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">The raster is not writable.</exception>
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
        void SetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue);

        /// <summary>
        /// Sets the spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not writable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The coordinate is not within the raster.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        void SetValue(Coordinate coordinate, Int32 bandIndex, UInt32 spectralValue);
        
        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.NotSupportedException">The raster is not writable.</exception>
        /// <exception cref="System.ArgumentNullException">The spectral values are not specified.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands in the raster.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        void SetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues);
        
        /// <summary>
        /// Sets all spectral values at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not writable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">The spectral values are not specified.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands in the raster.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        void SetValues(Coordinate coordinate, UInt32[] spectralValues);

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">The raster is not writable.</exception>
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
        void SetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue);

        /// <summary>
        /// Sets the spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not writable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The coordinate is not within the raster.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        void SetFloatValue(Coordinate coordinate, Int32 bandIndex, Double spectralValue);

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.NotSupportedException">The raster is not writable.</exception>
        /// <exception cref="System.ArgumentNullException">The spectral values are not specified.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands in the raster.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        void SetFloatValues(Int32 rowIndex, Int32 columnIndex, Double[] spectralValues);

        /// <summary>
        /// Sets all spectral values at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not writable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">The spectral values are not specified.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands in the raster.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        void SetFloatValues(Coordinate coordinate, Double[] spectralValues);

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
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
        UInt32 GetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);
        
        /// <summary>
        /// Returns the spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified coordinate.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The coordinate is not within the raster.
        /// </exception>
        UInt32 GetValue(Coordinate coordinate, Int32 bandIndex);
        
        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        UInt32[] GetValues(Int32 rowIndex, Int32 columnIndex);
        
        /// <summary>
        /// Returns all spectral values at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        UInt32[] GetValues(Coordinate coordinate);

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
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
        Double GetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);

        /// <summary>
        /// Returns the spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified coordinate.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The coordinate is not within the raster.
        /// </exception>
        Double GetFloatValue(Coordinate coordinate, Int32 bandIndex);

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        Double[] GetFloatValues(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Returns all spectral values at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        Double[] GetFloatValues(Coordinate coordinate);
        
        /// <summary>
        /// Returns the boxed spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the equidistant index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        UInt32 GetBoxedValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);
        
        /// <summary>
        /// Returns the boxed spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>Returns the nearest spectral value in a band at the specified coordinate or at the equidistant coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        UInt32 GetBoxedValue(Coordinate coordinate, Int32 bandIndex);
        
        /// <summary>
        /// Returns the boxed spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the equidistant index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        UInt32[] GetBoxedValues(Int32 rowIndex, Int32 columnIndex);
        
        /// <summary>
        /// Returns the boxed spectral values in all bands to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified coordinate or at the equidistant coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        UInt32[] GetBoxedValues(Coordinate coordinate);

        /// <summary>
        /// Returns the boxed spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the equidistant index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        Double GetBoxedFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);

        /// <summary>
        /// Returns the boxed spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>Returns the nearest spectral value in a band at the specified coordinate or at the equidistant coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        Double GetBoxedFloatValue(Coordinate coordinate, Int32 bandIndex);

        /// <summary>
        /// Returns the boxed spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="k">The zero-based row index of the values.</param> 
        /// <returns>The array containing the spectral values for each band at the specified index or the equidistant index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        Double[] GetBoxedFloatValues(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Returns the boxed spectral values in all bands to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified coordinate or at the equidistant coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        Double[] GetBoxedFloatValues(Coordinate coordinate);

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        UInt32 GetNearestValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>Returns the nearest spectral value in a band at the specified coordinate or at the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        UInt32 GetNearestValue(Coordinate coordinate, Int32 bandIndex);

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        UInt32[] GetNearestValues(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified coordinate or at the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        UInt32[] GetNearestValues(Coordinate coordinate);

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        Double GetNearestFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>Returns the nearest spectral value in a band at the specified coordinate or at the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        Double GetNearestFloatValue(Coordinate coordinate, Int32 bandIndex);

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="k">The zero-based row index of the values.</param> 
        /// <returns>The array containing the spectral values for each band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        Double[] GetNearestFloatValues(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified coordinate or at the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        Double[] GetNearestFloatValues(Coordinate coordinate);

        /// <summary>
        /// Gets the histogram values of a specified band.
        /// </summary>
        /// <param name="bandIndex">The zero-based index of the band.</param>
        /// <returns>The read-only list containing the histogram values for the specified band.<returns>
        /// <exception cref="System.NotSupportedException">Histogram query is not supported.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        IList<Int32> GetHistogramValues(Int32 bandIndex);

        #endregion
    }
}
