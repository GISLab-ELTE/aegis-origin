/// <copyright file="MaskedRaster.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Raster.Mask
{
    /// <summary>
    /// Represents a masked multispectral raster image.
    /// </summary>
    public class MaskedRaster : IRaster
    {
        #region Private fields
        
        private readonly IRaster _source;
        private readonly Int32 _rowStart;
        private readonly Int32 _rowCount;
        private readonly Int32 _columnCount;
        private readonly Int32 _columnStart;
        private readonly RasterMapper _mapper;

        private List<IRasterBand> _bands;
        private Coordinate[] _coordinates;
        private List<Int32[]> _histogramValues;

        #endregion

        #region IRasterBand properties

        /// <summary>
        /// Gets the factory of the raster.
        /// </summary>
        /// <value>The factory implementation the raster was constructed by.</value>
        public IRasterFactory Factory { get { return _source.Factory; } }

        /// <summary>
        /// Gets the number of columns
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        public Int32 NumberOfColumns { get { return _columnCount; } }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        public Int32 NumberOfRows { get { return _rowCount; } }

        /// <summary>
        /// Gets the spectral resolution of the raster.
        /// </summary>
        /// <value>The number of spectral bands contained in the raster.</value>
        public Int32 SpectralResolution { get { return _source.SpectralResolution; } }

        /// <summary>
        /// Gets the spectral bands of the raster.
        /// </summary>
        /// <value>The read-only list containing the spectral bands in the raster.</value>
        public IList<IRasterBand> Bands { get { return _bands.AsReadOnly(); } }

        /// <summary>
        /// Gets the radiometric resolutions of the bands in the raster.
        /// </summary>
        /// <value>The read-only list containing the radiometric resolution of each band in the raster.</value>
        public IList<Int32> RadiometricResolutions { get { return _source.RadiometricResolutions; } }

        /// <summary>
        /// Gets the spectral ranges of the bands in the raster.
        /// </summary>
        /// <value>The read-only list containing the spectral range of each band in the raster.</value>
        public IList<SpectralRange> SpectralRanges { get { return _source.SpectralRanges; } }

        /// <summary>
        /// Gets the histogram values of the raster.
        /// </summary>
        /// <value>The read-only list containing the histogram values of each band in the raster.</value>
        public IList<IList<Int32>> HistogramValues 
        { 
            get
            {
                return Enumerable.Range(0, _source.SpectralResolution).Select(GetHistogramValues).ToList().AsReadOnly();
            }        
        }

        /// <summary>
        /// Gets the bounding coordinates of the raster.
        /// </summary>
        /// <value>The read-only list containing the bounding coordinates of the raster in clockwise order.</value>
        public IList<Coordinate> Coordinates
        {
            get
            {
                if (_coordinates == null)
                {
                    if (_mapper == null)
                        _coordinates = Enumerable.Repeat(Coordinate.Undefined, 4).ToArray();
                    else
                    {
                        _coordinates = new Coordinate[4];
                        _coordinates[0] = _mapper.MapCoordinate(0, 0);
                        _coordinates[3] = _mapper.MapCoordinate(0, _columnCount);
                        _coordinates[1] = _mapper.MapCoordinate(_rowCount, _columnCount);
                        _coordinates[2] = _mapper.MapCoordinate(_rowCount, 0);
                    }
                }
                return Array.AsReadOnly(_coordinates);
            }
        }
                /// <summary>
        /// Gets the raster mapper used for mapping raster space to coordinate space.
        /// </summary>
        /// <value>The raster mapper associated with the raster.</value>
        public RasterMapper Mapper { get { return _mapper; } }

        /// <summary>
        /// Get a value indicating whether the raster band is mapped to coordinate space.
        /// </summary>
        /// <value><c>true</c> if the raster band is mapped to coordinate spacet; otherwise, <c>false</c>.</value>
        public Boolean IsMapped { get { return _source.IsMapped; } }

        /// <summary>
        /// Gets a value indicating whether the raster band is readable.
        /// </summary>
        /// <value><c>true</c> if the raster band is readable; otherwise, <c>false</c>.</value>
        public Boolean IsReadable { get { return _source.IsReadable; } }

        /// <summary>
        /// Gets a value indicating whether the raster band is writable.
        /// </summary>
        /// <value><c>true</c> if the raster band is writable; otherwise, <c>false</c>.</value>
        public Boolean IsWritable { get { return _source.IsWritable; } }

        /// <summary>
        /// Gets the representation of the raster.
        /// </summary>
        /// <value>The representation of the raster.</value>
        public RasterRepresentation Representation { get { return _source.Representation; } }

        /// <summary>
        /// Gets a raster band with the specified index.
        /// </summary>
        /// <value>The raster band at the specified index.</value>
        /// <param name="index">The zero-based index of the band.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// index;The index is less than 0.
        /// or
        /// index;The index is equal to or greater than the spectral resolution of the raster.
        /// </exception>
        public IRasterBand this[Int32 index] { get { return GetBand(index); } }

        /// <summary>
        /// Gets or sets a spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="columnIndex">The zero-based row index of the values.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <value>The array containing the spectral values for each band.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// columnIndex;the column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// bandIndex;the band index is equal to or greater than the number of bands.
        /// </exception>        
        public UInt32 this[Int32 rowIndex, Int32 columnIndex, Int32 bandIndex] 
        { 
            get { return GetValue(rowIndex, columnIndex, bandIndex); } 
            set { SetValue(rowIndex, columnIndex, bandIndex, value); } 
        }
        /// <summary>
        /// Gets or sets a spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="columnIndex">The zero-based row index of the values.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <value>The array containing the spectral values for each band.</value>
        /// <exception cref="System.InvalidOperationException">
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The coordinate is not within the raster.
        /// or
        /// The band index is less than 0.
        /// or
        /// bandIndex;the band index is equal to or greater than the number of bands.
        /// </exception>        
        public UInt32 this[Coordinate coordinate, Int32 bandIndex] 
        { 
            get { return GetValue(coordinate, bandIndex); } 
            set { SetValue(coordinate, bandIndex, value); } 
        }
        /// <summary>
        /// Gets or sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="columnIndex">The zero-based row index of the values.</param>
        /// <value>The array containing the spectral values for each band.</value>
        /// <exception cref="System.InvalidOperationException">
        /// The number of spectral values does not match the spectral resolution.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// columnIndex;the column index is equal to or greater than the number of columns.
        /// </exception>
        public UInt32[] this[Int32 rowIndex, Int32 columnIndex]
        {
            get { return GetValues(rowIndex, columnIndex); }
            set { SetValues(rowIndex, columnIndex, value); }
        }
        /// <summary>
        /// Gets or sets all spectral values at a specified coordinate.
        /// </summary>
        /// <value>The array containing the spectral values for each band.</value>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.InvalidOperationException">
        /// The mapping of the raster is not defined.
        /// or
        /// The number of spectral values does not match the spectral resolution.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public UInt32[] this[Coordinate coordinate]
        {
            get { return GetValues(coordinate); }
            set { SetValues(coordinate, value); }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the source raster.
        /// </summary>
        /// <value>The source raster.</value>
        public IRaster Source { get { return _source; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaskedRaster" /> class.
        /// </summary>
        /// <param name="source">The source raster.</param>
        /// <param name="rowStart">The starting row index of a mask.</param>
        /// <param name="rowCount">The number of rows in the mask.</param>
        /// <param name="columnStart">The starting column index of a mask.</param>
        /// <param name="columnCount">The number of columns in the mask.</param>
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
        /// The starting row index and the row count is greater than the number of rows in the source.
        /// or
        /// The starting columns index and the column count is greater than the number of rows in the source.
        /// </exception>
        public MaskedRaster(IRaster source, Int32 rowStart, Int32 rowCount, Int32 columnStart, Int32 columnCount)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (rowStart < 0)
                throw new ArgumentOutOfRangeException("rowStart", "The starting row index is less than 0.");
            if (rowStart >= source.NumberOfRows)
                throw new ArgumentOutOfRangeException("rowStart", "The starting row index is equal to or greater than the number of rows in the source.");
            if (columnStart < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The starting column index is less than 0.");
            if (columnStart >= source.NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "the starting column index is equal to or greater than the number of columns in the source.");
            if (rowStart + rowCount > source.NumberOfRows)
                throw new ArgumentOutOfRangeException("rowCount", "The starting row index and the row count is greater than the number of rows in the source.");
            if (columnStart + columnCount > source.NumberOfRows)
                throw new ArgumentOutOfRangeException("columnCount", "The starting columns index and the column count is greater than the number of rows in the source.");

            _source = source;
            _rowStart = rowStart;
            _rowCount = rowCount;
            _columnStart = columnStart;
            _columnCount = columnCount;

            _bands = Enumerable.Range(0, _source.SpectralResolution).Select(index => new RasterBand(this, index)).ToList<IRasterBand>();
            _histogramValues = Enumerable.Repeat<Int32[]>(null, _source.SpectralResolution).ToList();

            if (_source.Mapper == null)
                _mapper = null;
            else
            {
                RasterMapping[] mappings = new RasterMapping[]
                {
                    new RasterMapping(_rowStart, _columnStart, _source.Mapper.MapCoordinate(_rowStart, _columnStart)),
                    new RasterMapping(_rowStart, _columnStart + _columnCount, _source.Mapper.MapCoordinate(_rowStart, _columnStart + _columnCount)),
                    new RasterMapping(_rowStart + _rowCount, _columnStart + _columnCount, _source.Mapper.MapCoordinate(_rowStart + _rowCount, _columnStart + _columnCount)),
                    new RasterMapping(_rowStart + _rowCount, _columnStart, _source.Mapper.MapCoordinate(_rowStart + _rowCount, _columnStart))
                };

                _mapper = RasterMapper.FromMappings(mappings, _source.Mapper.Mode);
            }
        }

        #endregion

        #region IRaster methods

        /// <summary>
        /// Returns a band at a specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the band.</param>
        /// <returns>The raster band at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the spectral resolution of the raster.
        /// </exception>
        public IRasterBand GetBand(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _bands.Count)
                throw new ArgumentOutOfRangeException("index", "The index is equal to or greater than the spectral resolution of the raster.");

            return _bands[index];
        }

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
        public void SetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _rowCount)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("columnIndex", "the column index is equal to or greater than the number of columns.");

            _source.SetValue(_rowStart + rowIndex, _columnStart + columnIndex, bandIndex, spectralValue);
        }

        /// <summary>
        /// Sets the spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The coordinate is not within the raster.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public void SetValue(Coordinate coordinate, Int32 bandIndex, UInt32 spectralValue)
        {
            if (_source.Mapper == null)
                throw new InvalidOperationException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);
            if (rowIndex < _rowStart || rowIndex >= _rowCount || columnIndex < _columnStart || columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            _source.SetValue(_rowStart + rowIndex, _columnStart + columnIndex, bandIndex, spectralValue);
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.ArgumentNullException">spectralValues;The spectral values are not specified.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the spectral resolution of the raster.;spectralValues</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        public void SetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _rowCount)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("columnIndex", "the column index is equal to or greater than the number of columns.");

            _source.SetValues(_rowStart + rowIndex, _columnStart + columnIndex, spectralValues);
        }

        /// <summary>
        /// Sets all spectral values at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.ArgumentNullException">spectralValues;The spectral values are not specified.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the spectral resolution of the raster.;spectralValues</exception>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public void SetValues(Coordinate coordinate, UInt32[] spectralValues)
        {
            if (_source.Mapper == null)
                throw new InvalidOperationException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);
            if (rowIndex < _rowStart || rowIndex >= _rowCount || columnIndex < _columnStart || columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            _source.SetValues(_rowStart + rowIndex, _columnStart + columnIndex, spectralValues);
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
        public UInt32 GetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        { 
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _rowCount)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("columnIndex", "the column index is equal to or greater than the number of columns.");

            return _source.GetValue(_rowStart + rowIndex, _columnStart + columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified coordinate.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// or
        /// The coordinate is not within the raster.
        /// </exception>
        public UInt32 GetValue(Coordinate coordinate, Int32 bandIndex)
        {
            if (_source.Mapper == null)
                throw new InvalidOperationException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);
            if (rowIndex < _rowStart || rowIndex >= _rowCount || columnIndex < _columnStart || columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return _source.GetValue(_rowStart + rowIndex, _columnStart + columnIndex, bandIndex);
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
        public UInt32[] GetValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _rowCount)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("columnIndex", "the column index is equal to or greater than the number of columns.");

            return _source.GetValues(_rowStart + rowIndex, _columnStart + columnIndex);
        }

        /// <summary>
        /// Returns all spectral values at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public UInt32[] GetValues(Coordinate coordinate)
        {
            if (_source.Mapper == null)
                throw new InvalidOperationException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);
            if (rowIndex < _rowStart || rowIndex >= _rowCount || columnIndex < _columnStart || columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return _source.GetValues(_rowStart + rowIndex, _columnStart + columnIndex);
        }

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the nearest index if either row or column is out of range.</returns>
        public UInt32 GetNearestValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Int32 trueRowIndex = _rowStart + Math.Min(Math.Max(rowIndex, 0), _rowCount - 1);
            Int32 trueColumnIndex = _columnStart + Math.Min(Math.Max(columnIndex, 0), _columnCount - 1);
            return _source.GetNearestValue(trueRowIndex, trueColumnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>Returns the nearest spectral value in a band at the specified coordinate or at the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        public UInt32 GetNearestValue(Coordinate coordinate, Int32 bandIndex)
        {
            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return _source.GetNearestValue(_rowStart + rowIndex, _columnStart + columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="k">The zero-based row index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the nearest index if either row or column is out odf range.</returns>
        public UInt32[] GetNearestValues(Int32 rowIndex, Int32 columnIndex)
        {
            Int32 trueRowIndex = _rowStart + Math.Min(Math.Max(rowIndex, 0), _rowCount - 1);
            Int32 trueColumnIndex = _columnStart + Math.Min(Math.Max(columnIndex, 0), _columnCount - 1);
            return _source.GetNearestValues(trueRowIndex, trueColumnIndex);
        }

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified coordinate or at the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        public UInt32[] GetNearestValues(Coordinate coordinate)
        {
            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return _source.GetNearestValues(_rowStart + rowIndex, _columnStart + columnIndex);
        }

        /// <summary>
        /// Gets the histogram values of a specified band.
        /// </summary>
        /// <param name="bandIndex">The zero-based index of the band.</param>
        /// The read-only list containing the histogram values for the specified band.
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public IList<Int32> GetHistogramValues(Int32 bandIndex)
        {
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Count)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            if (_histogramValues[bandIndex] == null)
            {
                _histogramValues[bandIndex] = new Int32[1UL << _source.RadiometricResolutions[bandIndex]];
                for (Int32 i = _rowStart; i < _rowStart + _rowCount; i++)
                    for (Int32 j = _columnStart; j < _columnStart + _columnCount; j++)
                    {
                        _histogramValues[bandIndex][_source.GetValue(i, j, bandIndex)]++;
                    }
            }

            return Array.AsReadOnly(_histogramValues[bandIndex]);
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="MaskedRaster" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="MaskedRaster" /> instance.</returns>
        public Object Clone()
        {
            return new MaskedRaster(_source, _rowStart, _rowCount, _columnStart, _columnCount);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return "Raster [" + _rowCount + "x" + _columnCount + "x" + _source.SpectralResolution + "]";
        }

        #endregion
    }
}
