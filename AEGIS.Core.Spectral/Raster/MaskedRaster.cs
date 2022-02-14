/// <copyright file="MaskedRaster.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Raster
{
    /// <summary>
    /// Represents a masked multispectral raster image.
    /// </summary>
    public class MaskedRaster : Raster
    {
        #region Private fields
        
        /// <summary>
        /// The source raster. This field is read-only.
        /// </summary>
        private readonly IRaster _source;

        /// <summary>
        /// The starting row index. This field is read-only.
        /// </summary>
        private readonly Int32 _rowIndex;

        /// <summary>
        /// The starting column index. This field is read-only.
        /// </summary>
        private readonly Int32 _columnIndex;

        /// <summary>
        /// The list of histogram values.
        /// </summary>
        private IReadOnlyList<Int32>[] _histogramValues;

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets the format of the raster.
        /// </summary>
        /// <value>The format of the raster.</value>
        public override RasterFormat Format { get { return _source.Format; } }

        /// <summary>
        /// Gets a value indicating whether the raster is readable.
        /// </summary>
        /// <value><c>true</c> if the raster is readable; otherwise, <c>false</c>.</value>
        public override Boolean IsReadable { get { return _source.IsReadable; } }

        /// <summary>
        /// Gets a value indicating whether the raster is writable.
        /// </summary>
        /// <value><c>true</c> if the raster is writable; otherwise, <c>false</c>.</value>
        public override Boolean IsWritable { get { return _source.IsWritable; } }

        #endregion
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaskedRaster" /> class.
        /// </summary>
        /// <param name="factory">The raster factory.</param>
        /// <param name="source">The source raster.</param>
        /// <param name="rowIndex">The starting row index of a mask.</param>
        /// <param name="columnIndex">The starting column index of a mask.</param>
        /// <param name="numberOfRows">The number of rows in the mask.</param>
        /// <param name="numberOfColumns">The number of columns in the mask.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting row index is less than 0.
        /// or
        /// The starting row index is equal to or greater than the number of rows in the source.
        /// or
        /// The starting column index is less than 0.
        /// or
        /// the starting column index is equal to or greater than the number of columns in the source.
        /// or
        /// The starting row index and the number of rows is greater than the number of rows in the source.
        /// or
        /// The starting columns index and the number of columns is greater than the number of columns in the source.
        /// </exception>
        public MaskedRaster(IRasterFactory factory, IRaster source, Int32 rowIndex, Int32 columnIndex, Int32 numberOfRows, Int32 numberOfColumns)
            : base(factory, GetNumberOfBands(source), numberOfRows, numberOfColumns, GetRadiometricResolution(source), ComputeMapper(source, rowIndex, columnIndex))
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "The starting row index is less than 0.");
            if (rowIndex >= source.NumberOfRows)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "The starting row index is equal to or greater than the number of rows in the source.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "The starting column index is less than 0.");
            if (columnIndex >= source.NumberOfColumns)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "the starting column index is equal to or greater than the number of columns in the source.");
            if (rowIndex + numberOfRows > source.NumberOfRows)
                throw new ArgumentOutOfRangeException(nameof(numberOfRows), "The starting row index and the number of rows is greater than the number of rows in the source.");
            if (columnIndex + numberOfColumns > source.NumberOfColumns)
                throw new ArgumentOutOfRangeException(nameof(numberOfColumns), "The starting columns index and the number of columns is greater than the number of columns in the source.");

            _source = source;
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _histogramValues = Enumerable.Repeat<IReadOnlyList<Int32>>(null, _source.NumberOfBands).ToArray();            
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="MaskedRaster" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="MaskedRaster" /> instance.</returns>
        public override Object Clone()
        {
            return new MaskedRaster(Factory, _source, _rowIndex, NumberOfRows, _columnIndex, NumberOfColumns);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return "Raster [" + NumberOfRows + "x" + NumberOfColumns + "x" + _source.NumberOfBands + "]";
        }

        #endregion

        #region Protected Raster methods

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        protected override void ApplySetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
        {
            _source.SetValue(_rowIndex + rowIndex, _columnIndex + columnIndex, bandIndex, spectralValue);
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        protected override void ApplySetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            _source.SetValues(_rowIndex + rowIndex, _columnIndex + columnIndex, spectralValues);
        }

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        protected override void ApplySetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
        {
            _source.SetFloatValue(_rowIndex + rowIndex, _columnIndex + columnIndex, bandIndex, spectralValue);
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        protected override void ApplySetFloatValues(Int32 rowIndex, Int32 columnIndex, Double[] spectralValues)
        {
            _source.SetFloatValues(_rowIndex + rowIndex, _columnIndex + columnIndex, spectralValues);
        }

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 ApplyGetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return _source.GetValue(_rowIndex + rowIndex, _columnIndex + columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] ApplyGetValues(Int32 rowIndex, Int32 columnIndex)
        {
            return _source.GetValues(_rowIndex + rowIndex, _columnIndex + columnIndex);
        }

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ApplyGetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return _source.GetFloatValue(_rowIndex + rowIndex, _columnIndex + columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ApplyGetFloatValues(Int32 rowIndex, Int32 columnIndex)
        {
            return _source.GetFloatValues(_rowIndex + rowIndex, _columnIndex + columnIndex);
        }

        /// <summary>
        /// Gets the histogram values of a specified band.
        /// </summary>
        /// <param name="bandIndex">The zero-based index of the band.</param>
        /// <returns>The read-only list containing the histogram values for the specified band.<returns>
        protected override IReadOnlyList<Int32> ApplyGetHistogramValues(Int32 bandIndex) 
        {
            if (Format == RasterFormat.Floating)
                return null;

            if (_histogramValues[bandIndex] == null)
            {
                if (RadiometricResolution < 32)
                {
                    Int32[] histogramValues = new Int32[1UL << _source.RadiometricResolution];

                    for (Int32 rowIndex = _rowIndex; rowIndex < _rowIndex + NumberOfRows; rowIndex++)
                        for (Int32 columnIndex = _columnIndex; columnIndex < _columnIndex + NumberOfColumns; columnIndex++)
                        {
                            histogramValues[(Int32)_source.GetValue(rowIndex, columnIndex, bandIndex)]++;
                        }

                    _histogramValues[bandIndex] = histogramValues;
                }

                else
                {
                    SparseArray<Int32> histogramValues = new SparseArray<Int32>(1L << _source.RadiometricResolution);

                    for (Int32 rowIndex = _rowIndex; rowIndex < _rowIndex + NumberOfRows; rowIndex++)
                        for (Int32 columnIndex = _columnIndex; columnIndex < _columnIndex + NumberOfColumns; columnIndex++)
                        {
                            histogramValues[_source.GetValue(rowIndex, columnIndex, bandIndex)]++;
                        }

                    _histogramValues[bandIndex] = histogramValues;
                }

            }

            return _histogramValues[bandIndex];
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Returns the number of bands of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The number of bands.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        private static Int32 GetNumberOfBands(IRaster source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            return source.NumberOfBands;
        }

        /// <summary>
        /// Returns the radiometric resolutions of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The radiometric resolutions of the specified source.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        private static Int32 GetRadiometricResolution(IRaster source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            return source.RadiometricResolution;
        }

        /// <summary>
        /// Computes the raster mapper of the mask.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="rowStart">The starting row index of a mask.</param>
        /// <param name="columnStart">The starting column index of a mask.</param>
        /// <param name="numberOfRows">The number of rows in the mask.</param>
        /// <param name="numberOfColumns">The number of columns in the mask.</param>
        /// <returns>The raster mapper of the specified source.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        private static RasterMapper ComputeMapper(IRaster source, Int32 rowStart, Int32 columnStart)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            if (source.Mapper == null)
                return null;
            else
            {
                // the mask is a translation of the source
                return RasterMapper.FromMapper(source.Mapper, (Coordinate)(source.Mapper.MapCoordinate(rowStart, columnStart) - source.Mapper.MapCoordinate(0, 0)), new CoordinateVector(1, 1, 1));
            }
        }

        #endregion
    }
}
