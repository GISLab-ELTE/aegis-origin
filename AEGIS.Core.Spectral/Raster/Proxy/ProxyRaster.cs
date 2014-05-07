/// <copyright file="ProxyRaster.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Raster.Proxy
{
    /// <summary>
    /// Represents a raster image managing values in a entity containg integer values.
    /// </summary>
    public class ProxyRaster : IRaster
    {
        #region Private fields

        private readonly IRasterFactory _factory;
        private readonly ISpectralEntity _entity;
        private readonly RasterMapper _mapper;
        private readonly Int32 _spectralResolution;
        private readonly Int32 _numberOfColumns;
        private readonly Int32 _numberOfRows;
        private readonly Int32[] _radiometricResolutions;
        private readonly SpectralRange[] _spectralRanges;
        private readonly IRasterBand[] _bands;
        private readonly Boolean _isSequentialDataset;
        private Coordinate[] _coordinates;

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets the factory of the raster.
        /// </summary>
        /// <value>The factory implementation the raster was constructed by.</value>
        public IRasterFactory Factory { get { return _factory; } }

        /// <summary>
        /// Gets the number of spectral values in a row.
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        public Int32 NumberOfColumns { get { return _numberOfColumns; } }

        /// <summary>
        /// Gets the number of spectral values in a column.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        public Int32 NumberOfRows { get { return _numberOfRows; } }

        /// <summary>
        /// Gets the spectral resolution of the raster.
        /// </summary>
        /// <value>The number of spectral bands contained in the raster.</value>
        public Int32 SpectralResolution { get { return _spectralResolution; } }

        /// <summary>
        /// Gets the spectral bands of the raster.
        /// </summary>
        /// <value>The read-only list containing the spectral bands in the raster.</value>
        public IList<IRasterBand> Bands { get { return Array.AsReadOnly(_bands); } }

        /// <summary>
        /// Gets the radiometric resolutions of the bands in the raster.
        /// </summary>
        /// <value>The read-only list containing the radiometric resolution of each band in the raster.</value>
        public IList<Int32> RadiometricResolutions { get { return Array.AsReadOnly(_radiometricResolutions); } }

        /// <summary>
        /// Gets the spectral ranges of the raster.
        /// </summary>
        /// <value>The read-only list containing the spectral range of each band in the raster.</value>
        public IList<SpectralRange> SpectralRanges { get { return Array.AsReadOnly(_spectralRanges); } }

        /// <summary>
        /// Gets the bounding coordinates of the raster.
        /// </summary>
        /// <value>The read-only list containing the bounding coordinates of the raster in counterclockwise order.</value>
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        public IList<Coordinate> Coordinates
        {
            get
            {
                if (_coordinates == null)
                {
                    if (_mapper == null)
                        _coordinates = Enumerable.Repeat(Coordinate.Empty, 4).ToArray();
                    else
                    {
                        _coordinates = new Coordinate[4];
                        _coordinates[0] = _mapper.MapCoordinate(0, 0);
                        _coordinates[2] = _mapper.MapCoordinate(_numberOfRows, 0);
                        _coordinates[1] = _mapper.MapCoordinate(_numberOfRows, _numberOfColumns);
                        _coordinates[3] = _mapper.MapCoordinate(0, _numberOfColumns);
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
        /// Get a value indicating whether the raster is mapped to coordinate space.
        /// </summary>
        /// <value><c>true</c> if the raster is mapped to coordinate spacet; otherwise, <c>false</c>.</value>
        public Boolean IsMapped { get { return _mapper != null; } }

        /// <summary>
        /// Gets a value indicating whether the raster is readable.
        /// </summary>
        /// <value><c>true</c> if the raster is readable; otherwise, <c>false</c>.</value>
        public Boolean IsReadable { get { return _entity.IsReadable; } }

        /// <summary>
        /// Gets a value indicating whether the raster is writable.
        /// </summary>
        /// <value><c>true</c> if the raster is writable; otherwise, <c>false</c>.</value>
        public Boolean IsWritable { get { return _entity.IsReadable; } }

        /// <summary>
        /// Gets the representation of the raster.
        /// </summary>
        /// <value>The representation of the raster.</value>
        public RasterRepresentation Representation { get { return RasterRepresentation.Integer; } }

        /// <summary>
        /// Gets a raster band with the specified index.
        /// </summary>
        /// <value>The raster band at the specified index.</value>
        /// <param name="index">The zero-based index of the band.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the spectral resolution of the raster.
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
        /// the column index is equal to or greater than the number of columns.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
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
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The coordinate is not within the raster.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
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
        /// <exception cref="System.InvalidOperationException">The number of spectral values does not match the spectral resolution.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// the column index is equal to or greater than the number of columns.
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
        /// <exception cref="System.NotSupportedException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.InvalidOperationException">The number of spectral values does not match the spectral resolution.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public UInt32[] this[Coordinate coordinate]
        {
            get { return GetValues(coordinate); }
            set { SetValues(coordinate, value); }
        }

        #endregion

        #region IRaster properties (explicit)

        /// <summary>
        /// Gets the histogram values of the raster.
        /// </summary>
        /// <value>The read-only list containing the histogram values of each band in the raster.</value>
        /// <exception cref="System.NotSupportedException">Histogram query is not supported.</exception>
        IList<IList<Int32>> IRaster.HistogramValues { get { throw new NotSupportedException("Histogram query is not supported."); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyRaster" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="spectralRanges">The spectral ranges.</param>
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="System.ArgumentNullException">The entity is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The entity does not contain integer values.
        /// or
        /// The number of spectral ranges does not match the spectral resolution.
        /// </exception>
        public ProxyRaster(IRasterFactory factory, ISpectralEntity entity, IList<SpectralRange> spectralRanges, RasterMapper mapper)
        {
            if (entity == null)
                throw new ArgumentNullException("entity", "The entity is null.");

            if (entity.Representation != RasterRepresentation.Integer)
                throw new ArgumentException("The entity does not contain integer values.", "entity");
            if (spectralRanges != null && entity.SpectralResolution != spectralRanges.Count)
                throw new ArgumentException("The number of spectral ranges does not match the spectral resolution.", "spectralRanges");


            _factory = factory;
            _entity = entity;
            _isSequentialDataset = _entity.SupportedOrders.Contains(SpectralDataOrder.RowColumnBand);
            _mapper = mapper;

            _spectralResolution = _entity.SpectralResolution;
            _numberOfColumns = _entity.NumberOfColumns;
            _numberOfRows = _entity.NumberOfRows;
            _mapper = mapper;

            _bands = Enumerable.Range(0, _spectralResolution).Select(index => new RasterBand(this, index)).ToArray<IRasterBand>();

            _radiometricResolutions = _entity.RadiometricResolutions.ToArray();
            
            if (spectralRanges != null)
                _spectralRanges = spectralRanges.ToArray();
            else
                _spectralRanges = Enumerable.Repeat<SpectralRange>(null, _spectralResolution).ToArray();
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
            if (index >= _spectralResolution)
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
        public void SetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
        {
            _entity.WriteValue(rowIndex, columnIndex, bandIndex, spectralValue);
        }

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
        public void SetValue(Coordinate coordinate, Int32 bandIndex, UInt32 spectralValue)
        {
            if (_mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= _numberOfRows || columnIndex < 0 || columnIndex >= _numberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            SetValue(rowIndex, columnIndex, bandIndex, spectralValue);
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.NotSupportedException">The raster is not writable.</exception>
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
        public void SetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The spectral values are not specified.");
            if (spectralValues.Length != _spectralResolution)
                throw new ArgumentException("The number of spectral values does not match the spectral resolution of the raster.", "spectralValues");

            if (_isSequentialDataset)
                _entity.WriteValueSequence(rowIndex, columnIndex, 0, spectralValues);
            else
            {
                for (Int32 k = 0; k < spectralValues.Length; k++)
                    _entity.WriteValue(rowIndex, columnIndex, k, spectralValues[k]);
            }
        }

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
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the spectral resolution of the raster.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public void SetValues(Coordinate coordinate, UInt32[] spectralValues)
        {
            if (_mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The spectral values are not specified.");
            if (spectralValues.Length != _spectralResolution)
                throw new ArgumentException("The number of spectral values does not match the spectral resolution of the raster.", "spectralValues");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= _numberOfRows || columnIndex < 0 || columnIndex >= _numberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            SetValues(rowIndex, columnIndex, spectralValues);
        }

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
        public UInt32 GetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return _entity.ReadValue(rowIndex, columnIndex, bandIndex);
        }

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
        public UInt32 GetValue(Coordinate coordinate, Int32 bandIndex)
        {
            if (_mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= _numberOfRows || columnIndex < 0 || columnIndex >= _numberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return GetValue(rowIndex, columnIndex, bandIndex);
        }

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
        public UInt32[] GetValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (_isSequentialDataset)
                return _entity.ReadValueSequence(rowIndex, columnIndex, 0, _spectralResolution);
            else
            {
                UInt32[] values = new UInt32[_spectralResolution];
                for (Int32 k = 0; k < _spectralResolution; k++)
                    values[k] = _entity.ReadValue(rowIndex, columnIndex, k);

                return values;
            }
        }

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
        public UInt32[] GetValues(Coordinate coordinate)
        {
            if (_mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= _numberOfRows || columnIndex < 0 || columnIndex >= _numberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return GetValues(rowIndex, columnIndex);
        }

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        public UInt32 GetNearestValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Int32 trueRowIndex = Math.Min(Math.Max(rowIndex, 0), _numberOfRows - 1);
            Int32 trueColumnIndex = Math.Min(Math.Max(columnIndex, 0), _numberOfColumns - 1);

            return GetValue(trueRowIndex, trueColumnIndex, bandIndex);
        }

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
        public UInt32 GetNearestValue(Coordinate coordinate, Int32 bandIndex)
        {
            if (_mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetNearestValue(rowIndex, columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the nearest index if either row or column is out odf range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        public UInt32[] GetNearestValues(Int32 rowIndex, Int32 columnIndex)
        {
            Int32 trueRowIndex = Math.Min(Math.Max(rowIndex, 0), _numberOfRows - 1);
            Int32 trueColumnIndex = Math.Min(Math.Max(columnIndex, 0), _numberOfColumns - 1);

            if (_isSequentialDataset)
                return _entity.ReadValueSequence(trueRowIndex, trueColumnIndex, 0, _spectralResolution);
            else
            {
                UInt32[] values = new UInt32[_spectralResolution];
                for (Int32 k = 0; k < _spectralResolution; k++)
                    values[k] = _entity.ReadValue(trueRowIndex, trueColumnIndex, k);

                return values;
            }
        }

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
        public UInt32[] GetNearestValues(Coordinate coordinate)
        {
            if (_mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetNearestValues(rowIndex, columnIndex);
        }

        #endregion

        #region IRaster methods (explicit)

        /// <summary>
        /// Gets the histogram values of a specified band.
        /// </summary>
        /// <param name="bandIndex">The zero-based index of the band.</param>
        /// <returns>The read-only list containing the histogram values for the specified band.<returns>
        /// <exception cref="System.NotSupportedException">Histogram query is not supported.</exception>
        IList<Int32> IRaster.GetHistogramValues(Int32 bandIndex) { throw new NotSupportedException("Histogram query is not supported."); }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="ProxyRaster" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="ProxyRaster" /> instance.</returns>
        public object Clone()
        {
            return new ProxyRaster(_factory, _entity, _spectralRanges, _mapper);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return "Raster [" + _numberOfRows + "x" + _numberOfColumns + "x" + _spectralResolution + "] (on dataaset " + _entity.ToString() + ")";
        }

        #endregion
    }
}
