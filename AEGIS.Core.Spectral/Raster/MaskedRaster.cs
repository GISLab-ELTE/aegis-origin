/// <copyright file="MaskedRaster.cs" company="Eötvös Loránd University (ELTE)">
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
        private List<Int32[]> _histogramValues;

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets the representation of the raster.
        /// </summary>
        /// <value>The representation of the raster.</value>
        public override RasterRepresentation Representation { get { return _source.Representation; } }

        #endregion

        #region Protected Raster properties

        /// <summary>
        /// Gets the maximum radiometric resolution.
        /// </summary>
        /// <value>The maximum radiometric resolution.</value>
        protected override Int32 MaxRadiometricResolution { get { return _source.RadiometricResolutions.Max(); } }

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
            : base(factory, GetNumberOfBands(source), numberOfRows, numberOfColumns, GetRadiometricResolutions(source), GetMapper(source, rowIndex, columnIndex, numberOfRows, numberOfColumns))
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The starting row index is less than 0.");
            if (rowIndex >= source.NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The starting row index is equal to or greater than the number of rows in the source.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The starting column index is less than 0.");
            if (columnIndex >= source.NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "the starting column index is equal to or greater than the number of columns in the source.");
            if (rowIndex + numberOfRows > source.NumberOfRows)
                throw new ArgumentOutOfRangeException("numberOfRows", "The starting row index and the number of rows is greater than the number of rows in the source.");
            if (columnIndex + numberOfColumns > source.NumberOfColumns)
                throw new ArgumentOutOfRangeException("numberOfColumns", "The starting columns index and the number of columns is greater than the number of columns in the source.");

            _source = source;
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _histogramValues = Enumerable.Repeat<Int32[]>(null, _source.NumberOfBands).ToList();
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
        protected override IList<Int32> ApplyGetHistogramValues(Int32 bandIndex) 
        {
            if (_histogramValues[bandIndex] == null)
            {
                _histogramValues[bandIndex] = new Int32[1UL << _source.RadiometricResolutions[bandIndex]];
                for (Int32 rowIndex = _rowIndex; rowIndex < _rowIndex + NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = _columnIndex; columnIndex < _columnIndex + NumberOfColumns; columnIndex++)
                    {
                        _histogramValues[bandIndex][_source.GetValue(rowIndex, columnIndex, bandIndex)]++;
                    }
            }

            return Array.AsReadOnly(_histogramValues[bandIndex]);
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
        private static IList<Int32> GetRadiometricResolutions(IRaster source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            return source.RadiometricResolutions;
        }

        /// <summary>
        /// Returns the raster mapper of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="rowStart">The starting row index of a mask.</param>
        /// <param name="columnStart">The starting column index of a mask.</param>
        /// <param name="numberOfRows">The number of rows in the mask.</param>
        /// <param name="numberOfColumns">The number of columns in the mask.</param>
        /// <returns>The raster mapper of the specified source.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        private static RasterMapper GetMapper(IRaster source, Int32 rowStart, Int32 columnStart, Int32 numberOfRows, Int32 numberOfColumns)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            if (source.Mapper == null)
                return null;
            else
            {
                RasterMapping[] mappings = new RasterMapping[]
                {
                    new RasterMapping(rowStart, columnStart, source.Mapper.MapCoordinate(rowStart, columnStart)),
                    new RasterMapping(rowStart, columnStart + numberOfColumns, source.Mapper.MapCoordinate(rowStart, columnStart + numberOfColumns)),
                    new RasterMapping(rowStart + numberOfRows, columnStart + numberOfColumns, source.Mapper.MapCoordinate(rowStart + numberOfRows, columnStart + numberOfColumns)),
                    new RasterMapping(rowStart + numberOfRows, columnStart, source.Mapper.MapCoordinate(rowStart + numberOfRows, columnStart))
                };

                return RasterMapper.FromMappings(mappings, source.Mapper.Mode);
            }
        }

        #endregion
    }
}
