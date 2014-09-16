/// <copyright file="RasterFloat64.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a multispectral raster image containing floating point values with maximum 64 bits radiometric resolution per band.
    /// </summary>
    public class RasterFloat64 : Raster
    {
        #region Private fields

        /// <summary>
        /// The array of spectral values. This field is read-only.
        /// </summary>
        private readonly Double[][] _values;

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets the format of the raster.
        /// </summary>
        /// <value>The format of the raster.</value>
        public override RasterFormat Format { get { return RasterFormat.Floating; } }

        #endregion

        #region Protected Raster properties

        /// <summary>
        /// Gets the maximum radiometric resolution.
        /// </summary>
        /// <value>The maximum radiometric resolution.</value>
        protected override Int32 MaxRadiometricResolution { get { return 64; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterFloat64" /> class.
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
        public RasterFloat64(IRasterFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
            : base(factory, numberOfBands, numberOfRows, numberOfColumns, radiometricResolutions, mapper)
        {
            // generate empty values for all bands
            _values = Enumerable.Repeat<Double[]>(null, numberOfBands).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterFloat64" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="spectralValues">The spectral values.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        private RasterFloat64(IRasterFactory factory, IList<Double[]> spectralValues, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
            : base(factory, spectralValues != null ? spectralValues.Count : 0, numberOfRows, numberOfColumns, radiometricResolutions, mapper)
        {
            // copy values for all bands
            for (Int32 i = 0; i < spectralValues.Count; i++)
            {
                _values[i] = new Double[spectralValues[i].Length];
                Array.Copy(spectralValues[i], _values[i], spectralValues[i].Length);
            }
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="RasterFloat64" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="RasterFloat64" /> instance.</returns>
        public override Object Clone()
        {
            return new RasterFloat64(Factory, _values, NumberOfRows, NumberOfColumns, _radiometricResolutions, Mapper);
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
            // create the spectral values if the they don't exist
            if (_values[bandIndex] == null)
            {
                if (spectralValue == 0)
                    return;

                _values[bandIndex] = new Double[NumberOfRows * NumberOfColumns];
            }

            _values[bandIndex][rowIndex * NumberOfColumns + columnIndex] = (Double)spectralValue;
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
                // create the spectral values if the they don't exist
                if (_values[k] == null)
                {
                    if (spectralValues[k] == 0)
                        continue;

                    _values[k] = new Double[NumberOfRows * NumberOfColumns];
                }

                _values[k][rowIndex * NumberOfColumns + columnIndex] = (Double)spectralValues[k];
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
            // create the spectral values if the they don't exist
            if (_values[bandIndex] == null)
            {
                if (spectralValue == 0)
                    return;

                _values[bandIndex] = new Double[NumberOfRows * NumberOfColumns];
            }

            _values[bandIndex][rowIndex * NumberOfColumns + columnIndex] = (Double)spectralValue;
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
                // create the spectral values if the they don't exist
                if (_values[k] == null)
                {
                    if (spectralValues[k] == 0)
                        continue;

                    _values[k] = new Double[NumberOfRows * NumberOfColumns];
                }

                _values[k][rowIndex * NumberOfColumns + columnIndex] = (Double)spectralValues[k];
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
            if (_values[bandIndex] == null)
                return 0;

            return (UInt32)_values[bandIndex][rowIndex * NumberOfColumns + columnIndex];
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
                values[k] = (_values[k] == null) ? 0U : (UInt32)_values[k][rowIndex * NumberOfColumns + columnIndex];
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
            if (_values[bandIndex] == null)
                return 0;

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
                values[k] = (_values[k] == null) ? 0 : _values[k][rowIndex * NumberOfColumns + columnIndex];
            }
            return values;
        }

        #endregion
    }
}
