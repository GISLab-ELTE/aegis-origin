/// <copyright file="Raster16.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Raster
{
    /// <summary>
    /// Represents a multispectral raster image with maximum 16 bits radiometric resolution per band.
    /// </summary>
    public class Raster16 : Raster
    {
        #region Private fields

        private readonly UInt16[][] _values;
        private readonly Int32[][] _histogramValues;

        #endregion

        #region Protected Raster properties

        /// <summary>
        /// Gets the maximum radiometric resolution.
        /// </summary>
        /// <value>The maximum radiometric resolution.</value>
        protected override Int32 MaxRadiometricResolution { get { return 16; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster16" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="spectralResolution">The spectral resolution.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The spectral resolution is less than 0.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// Not all radiometric resolution values fall within the predefined range.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of radiometric resolutions does not match the spectral resolution.
        /// or
        /// The number of spectral ranges does not match the spectral resolution.
        /// </exception>
        public Raster16(IRasterFactory factory, Int32 spectralResolution, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper)
            : base(factory, spectralResolution, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper)
        {
            // generate empty values for all bands
            _values = Enumerable.Repeat<UInt16[]>(null, spectralResolution).ToArray();
            _histogramValues = Enumerable.Repeat<Int32[]>(null, spectralResolution).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster16" /> class.
        /// </summary>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Not all radiometric resolution values fall within the predefined range.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of radiometric resolutions does not match the number of bands.
        /// or
        /// The number of spectral ranges does not match the number of bands.
        /// or
        /// The number of spectral values within the arrays does not match the column and row values.
        /// or
        /// Not all matrices within the list of spectral values have the same dimension.
        /// </exception>
        public Raster16(IRasterFactory factory, IList<UInt16[]> spectralValues, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, IList<SpectralRange> spectralRanges, RasterMapper mapper)
            : base(factory, spectralValues != null ? spectralValues.Count : 0, numberOfRows, numberOfColumns, radiometricResolutions, spectralRanges, mapper)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The spectral values are not specified.");

            // copy values for all bands
            for (Int32 i = 0; i < spectralValues.Count; i++)
            {
                _values[i] = new UInt16[spectralValues[i].Length];
                Array.Copy(spectralValues[i], _values[i], spectralValues[i].Length);
            }

            _histogramValues = Enumerable.Repeat<Int32[]>(null, spectralValues.Count).ToArray();
        }

        #endregion

        #region IRaster methods

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
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
        public override void SetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _numberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _numberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _spectralResolution)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            // modify the histrogram values if they are already calculated
            if (_histogramValues[bandIndex] != null)
            {
                _histogramValues[bandIndex][_values[bandIndex][rowIndex * _numberOfRows + columnIndex]]--;
                _histogramValues[bandIndex][(UInt16)spectralValue]++;
            }

            // create the spectral values if the they don't exist
            if (_values[bandIndex] == null)
            {
                if (spectralValue == 0)
                    return;

                _values[bandIndex] = new UInt16[_numberOfRows * _numberOfColumns];
            }

            _values[bandIndex][rowIndex * _numberOfColumns + columnIndex] = (UInt16)spectralValue;
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.ArgumentNullException">The spectral values are not specified.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the spectral resolution of the raster.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        public override void SetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The spectral values are not specified.");
            if (spectralValues.Length != _spectralResolution)
                throw new ArgumentException("The number of spectral values does not match the spectral resolution of the raster.", "spectralValues");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _numberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _numberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");

            for (Int32 k = 0; k < spectralValues.Length; k++)
            {
                // modify the histrogram values if they are already calculated
                if (_histogramValues[k] != null)
                {
                    _histogramValues[k][_values[k][rowIndex * _numberOfRows + columnIndex]]--;
                    _histogramValues[k][(UInt16)spectralValues[k]]++;
                }

                // create the spectral values if the they don't exist
                if (_values[k] == null)
                {
                    if (spectralValues[k] == 0)
                        continue;

                    _values[k] = new UInt16[_numberOfRows * _numberOfColumns];
                }

                _values[k][rowIndex * _numberOfColumns + columnIndex] = (UInt16)spectralValues[k];
            }
        }
        
        /// <summary>
        /// Returns the spectral value at a specified index.
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
        public override UInt32 GetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _numberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _numberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _spectralResolution)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            if (_values[bandIndex] == null)
                return 0;

            return _values[bandIndex][rowIndex * _numberOfColumns + columnIndex];
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        public override UInt32[] GetValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");

            UInt32[] values = new UInt32[_spectralResolution];
            for (Int32 k = 0; k < values.Length; k++)
            {
                values[k] = (_values[k] == null) ? (UInt16)0 : _values[k][rowIndex * _numberOfColumns + columnIndex];
            }
            return values;
        }

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the nearest index if either row or column is out of range.</returns>
        public override UInt32 GetNearestValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Int32 trueRowIndex = Math.Min(Math.Max(rowIndex, 0), _numberOfRows - 1);
            Int32 trueColumnIndex = Math.Min(Math.Max(columnIndex, 0), _numberOfColumns - 1);

            return (_values[bandIndex] == null) ? (UInt16)0 : _values[bandIndex][trueRowIndex * _numberOfColumns + trueColumnIndex];
        }

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="k">The zero-based row index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the nearest index if either row or column is out odf range.</returns>
        public override UInt32[] GetNearestValues(Int32 rowIndex, Int32 columnIndex)
        {
            UInt32[] values = new UInt32[_spectralResolution];
            Int32 trueRowIndex = Math.Min(Math.Max(rowIndex, 0), _numberOfRows - 1);
            Int32 trueColumnIndex = Math.Min(Math.Max(columnIndex, 0), _numberOfColumns - 1);

            for (Int32 k = 0; k < values.Length; k++)
            {
                values[k] = (_values[k] == null) ? (UInt16)0 : _values[k][rowIndex * _numberOfColumns + columnIndex];
            }
            return values;
        }

        /// <summary>
        /// Gets the histogram values of a specified band.
        /// </summary>
        /// <param name="bandIndex">The zero-based index of the band.</param>
        /// <returns>The read-only list containing the histogram values for the specified band.<returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public override IList<Int32> GetHistogramValues(Int32 bandIndex)
        {
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _spectralResolution)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            if (_histogramValues[bandIndex] == null)
            {
                _histogramValues[bandIndex] = new Int32[1UL << _radiometricResolutions[bandIndex]];
                if (_values[bandIndex] == null)
                    _histogramValues[bandIndex][0] = _numberOfColumns * _numberOfRows;
                else
                {
                    for (Int32 l = 0; l < _values[bandIndex].Length; l++)
                    {
                        _histogramValues[bandIndex][_values[bandIndex][l]]++;
                    }
                }
            }

            return Array.AsReadOnly(_histogramValues[bandIndex]);
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="Raster16" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="Raster16" /> instance.</returns>
        public override Object Clone()
        {
            return new Raster16(_factory, _values, _numberOfRows, _numberOfColumns, _radiometricResolutions, _spectralRanges, _mapper);
        }

        #endregion
    }
}
