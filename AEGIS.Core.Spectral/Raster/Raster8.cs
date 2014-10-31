/// <copyright file="Raster8.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Raster
{
    /// <summary>
    /// Represents a multispectral raster image with maximum 16 bits radiometric resolution per band.
    /// </summary>
    public class Raster8 : Raster
    {
        #region Private fields

        /// <summary>
        /// The array of spectral values. This field is read-only.
        /// </summary>
        private readonly Byte[][] _values;

        /// <summary>
        /// The array of histogram values. This field is read-only.
        /// </summary>
        private readonly Int32[][] _histogramValues;

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets the format of the raster.
        /// </summary>
        /// <value>The format of the raster.</value>
        public override RasterFormat Format { get { return RasterFormat.Integer; } }

        #endregion

        #region Protected Raster properties

        /// <summary>
        /// Gets the maximum radiometric resolution.
        /// </summary>
        /// <value>The maximum radiometric resolution.</value>
        protected override Int32 MaxRadiometricResolution { get { return 8; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster8" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="numberOfBands">The number of spectral bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// Not all radiometric resolution values fall within the predefined range.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        public Raster8(IRasterFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
            : base(factory, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper)
        {
            // generate empty values for all bands
            _values = Enumerable.Repeat<Byte[]>(null, numberOfBands).ToArray();
            _histogramValues = Enumerable.Repeat<Int32[]>(null, numberOfBands).ToArray();

            for (Int32 k = 0; k < _values.Length; k++)
            {
                _values[k] = new Byte[NumberOfRows * NumberOfColumns];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster8" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        private Raster8(IRasterFactory factory, IList<Byte[]> spectralValues, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
            : base(factory, spectralValues != null ? spectralValues.Count : 0, numberOfRows, numberOfColumns, radiometricResolutions, mapper)
        {
            // copy values for all bands
            for (Int32 i = 0; i < spectralValues.Count; i++)
            {
                _values[i] = new Byte[spectralValues[i].Length];
                Array.Copy(spectralValues[i], _values[i], spectralValues[i].Length);
            }

            _histogramValues = Enumerable.Repeat<Int32[]>(null, spectralValues.Count).ToArray();
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="Raster8" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="Raster8" /> instance.</returns>
        public override Object Clone()
        {
            return new Raster8(Factory, _values, NumberOfRows, NumberOfColumns, _radiometricResolutions, Mapper);
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
            // modify the histrogram values if they are already calculated
            if (_histogramValues[bandIndex] != null)
            {
                _histogramValues[bandIndex][_values[bandIndex][rowIndex * NumberOfRows + columnIndex]]--;
                _histogramValues[bandIndex][(Byte)spectralValue]++;
            }

            _values[bandIndex][rowIndex * NumberOfColumns + columnIndex] = (Byte)spectralValue;
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        protected override void ApplySetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            for (Int32 k = 0; k < spectralValues.Length; k++)
            {
                // modify the histrogram values if they are already calculated
                if (_histogramValues[k] != null)
                {
                    _histogramValues[k][_values[k][rowIndex * NumberOfRows + columnIndex]]--;
                    _histogramValues[k][(Byte)spectralValues[k]]++;
                }

                _values[k][rowIndex * NumberOfColumns + columnIndex] = (Byte)spectralValues[k];
            }
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
            // modify the histrogram values if they are already calculated
            if (_histogramValues[bandIndex] != null)
            {
                _histogramValues[bandIndex][_values[bandIndex][rowIndex * NumberOfRows + columnIndex]]--;
                _histogramValues[bandIndex][(Byte)spectralValue]++;
            }

            _values[bandIndex][rowIndex * NumberOfColumns + columnIndex] = (Byte)spectralValue;
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        protected override void ApplySetFloatValues(Int32 rowIndex, Int32 columnIndex, Double[] spectralValues)
        {
            for (Int32 k = 0; k < spectralValues.Length; k++)
            {
                // modify the histrogram values if they are already calculated
                if (_histogramValues[k] != null)
                {
                    _histogramValues[k][_values[k][rowIndex * NumberOfRows + columnIndex]]--;
                    _histogramValues[k][(Byte)spectralValues[k]]++;
                }

                _values[k][rowIndex * NumberOfColumns + columnIndex] = (Byte)spectralValues[k];
            }
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
            return _values[bandIndex][rowIndex * NumberOfColumns + columnIndex];
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] ApplyGetValues(Int32 rowIndex, Int32 columnIndex)
        {
            UInt32[] values = new UInt32[_bands.Length];
            for (Int32 k = 0; k < values.Length; k++)
            {
                values[k] = _values[k][rowIndex * NumberOfColumns + columnIndex];
            }
            return values;
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
            return _values[bandIndex][rowIndex * NumberOfColumns + columnIndex];
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ApplyGetFloatValues(Int32 rowIndex, Int32 columnIndex)
        {
            Double[] values = new Double[_bands.Length];
            for (Int32 k = 0; k < values.Length; k++)
            {
                values[k] = _values[k][rowIndex * NumberOfColumns + columnIndex];
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
        protected override IList<Int32> ApplyGetHistogramValues(Int32 bandIndex)
        {
            if (_histogramValues[bandIndex] == null)
            {
                _histogramValues[bandIndex] = new Int32[1UL << _radiometricResolutions[bandIndex]];
                if (_values[bandIndex] == null)
                    _histogramValues[bandIndex][0] = NumberOfColumns * NumberOfRows;
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
    }
}
