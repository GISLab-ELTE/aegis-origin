/// <copyright file="RasterFloat64.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterFloat64" /> class.
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
        public RasterFloat64(IRasterFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper)
            : base(factory, numberOfBands, numberOfRows, numberOfColumns, radiometricResolution, mapper)
        {
            // generate empty values for all bands
            _values = Enumerable.Repeat<Double[]>(null, numberOfBands).ToArray();

            for (Int32 bandIndex = 0; bandIndex < _values.Length; bandIndex++)
            {
                _values[bandIndex] = new Double[NumberOfRows * NumberOfColumns];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterFloat64" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <param name="dimensions">The dimensions of the raster.</param>
        /// <param name="mapper">The mapper.</param>
        private RasterFloat64(IRasterFactory factory, Double[][] spectralValues, RasterDimensions dimensions, RasterMapper mapper)
            : base(factory, dimensions, mapper)
        {
            // copy values for all bands
            for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
            {
                _values[bandIndex] = new Double[spectralValues[bandIndex].Length];
                Array.Copy(spectralValues[bandIndex], _values[bandIndex], spectralValues[bandIndex].Length);
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
            return new RasterFloat64(Factory, _values, Dimensions, Mapper);
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
            for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
            {
                _values[bandIndex][rowIndex * NumberOfColumns + columnIndex] = (Double)spectralValues[bandIndex];
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
            for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
            {
                _values[bandIndex][rowIndex * NumberOfColumns + columnIndex] = (Double)spectralValues[bandIndex];
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
            UInt32[] values = new UInt32[NumberOfBands];
            for (Int32 bandIndex = 0; bandIndex < values.Length; bandIndex++)
            {
                values[bandIndex] = (UInt32)_values[bandIndex][rowIndex * NumberOfColumns + columnIndex];
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
            for (Int32 bandIndex = 0; bandIndex < values.Length; bandIndex++)
            {
                values[bandIndex] = _values[bandIndex][rowIndex * NumberOfColumns + columnIndex];
            }
            return values;
        }

        #endregion
    }
}
