/// <copyright file="Raster8.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster8" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public Raster8(IRasterFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
            : base(factory, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper)
        {
            // generate empty values for all bands
            _values = Enumerable.Repeat<Byte[]>(null, numberOfBands).ToArray();
            _histogramValues = Enumerable.Repeat<Int32[]>(null, numberOfBands).ToArray();

            for (Int32 bandIndex = 0; bandIndex < _values.Length; bandIndex++)
            {
                _values[bandIndex] = new Byte[NumberOfRows * NumberOfColumns];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster8" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <param name="dimensions">The dimensions of the raster.</param>
        /// <param name="mapper">The mapper.</param>
        private Raster8(IRasterFactory factory, Byte[][] spectralValues, RasterDimensions dimensions, RasterMapper mapper)
            : base(factory, dimensions, mapper)
        {
            // copy values for all bands
            for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
            {
                _values[bandIndex] = new Byte[spectralValues[bandIndex].Length];
                Array.Copy(spectralValues[bandIndex], _values[bandIndex], spectralValues[bandIndex].Length);
            }

            _histogramValues = Enumerable.Repeat<Int32[]>(null, spectralValues.Length).ToArray();
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="Raster8" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="Raster8" /> instance.</returns>
        public override Object Clone()
        {
            return new Raster8(Factory, _values, Dimensions, Mapper);
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
            // modify the histogram values if they are already calculated
            if (_histogramValues[bandIndex] != null)
            {
                _histogramValues[bandIndex][_values[bandIndex][rowIndex * NumberOfColumns + columnIndex]]--;
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
                // modify the histogram values if they are already calculated
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
            // modify the histogram values if they are already calculated
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
                // modify the histogram values if they are already calculated
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
            UInt32[] values = new UInt32[NumberOfBands];
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
            Double[] values = new Double[NumberOfBands];
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
        protected override IReadOnlyList<Int32> ApplyGetHistogramValues(Int32 bandIndex)
        {
            if (_histogramValues[bandIndex] == null)
            {
                _histogramValues[bandIndex] = new Int32[1UL << RadiometricResolution];
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

            return _histogramValues[bandIndex];
        }

        #endregion
    }
}
