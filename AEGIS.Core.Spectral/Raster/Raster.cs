﻿/// <copyright file="Raster.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Provides a base class for multispectral raster images.
    /// </summary>
    public abstract class Raster : IRaster
    {
        #region Private fields

        /// <summary>
        /// The array of coordinates.
        /// </summary>
        private Coordinate[] _coordinates;

        #endregion

        #region Protected fields

        /// <summary>
        /// The array of radiometric resolutions. This field is read-only.
        /// </summary>
        protected readonly Int32[] _radiometricResolutions;

        /// <summary>
        /// The array of bands. This field is read-only.
        /// </summary>
        protected readonly IRasterBand[] _bands;

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets the factory of the raster.
        /// </summary>
        /// <value>The factory implementation the raster was constructed by.</value>
        public IRasterFactory Factory { get; private set; }

        /// <summary>
        /// Gets the number of spectral values in a row.
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        public Int32 NumberOfColumns { get; private set; }

        /// <summary>
        /// Gets the number of spectral values in a column.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        public Int32 NumberOfRows { get; private set; }

        /// <summary>
        /// Gets the number of bands in the raster.
        /// </summary>
        /// <value>The number of spectral bands contained in the raster.</value>
        public Int32 NumberOfBands { get; private set; }

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
        /// Gets the histogram values of the raster.
        /// </summary>
        /// <value>The read-only list containing the histogram values of each band in the raster.</value>
        /// <exception cref="System.NotSupportedException">Histogram query is not supported.</exception>
        public IList<IList<Int32>> HistogramValues
        {
            get
            {
                return Enumerable.Range(0, _bands.Length).Select(ApplyGetHistogramValues).ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the bounding coordinates of the raster.
        /// </summary>
        /// <value>The read-only list containing the bounding coordinates of the raster in counterclockwise order.</value>
        public IList<Coordinate> Coordinates
        {
            get
            {
                if (_coordinates == null)
                {
                    if (Mapper == null)
                        _coordinates = Enumerable.Repeat(Coordinate.Undefined, 4).ToArray();
                    else
                    {
                        _coordinates = new Coordinate[4];

                        _coordinates[0] = Mapper.MapCoordinate(0, 0);
                        _coordinates[2] = Mapper.MapCoordinate(NumberOfRows, 0);
                        _coordinates[1] = Mapper.MapCoordinate(NumberOfRows, NumberOfColumns);
                        _coordinates[3] = Mapper.MapCoordinate(0, NumberOfColumns);
                    }
                }
                return Array.AsReadOnly(_coordinates);
            }
        }

        /// <summary>
        /// Gets the raster mapper used for mapping raster space to coordinate space.
        /// </summary>
        /// <value>The raster mapper associated with the raster.</value>
        public RasterMapper Mapper { get; private set; }

        /// <summary>
        /// Get a value indicating whether the raster is mapped to coordinate space.
        /// </summary>
        /// <value><c>true</c> if the raster is mapped to coordinate spacet; otherwise, <c>false</c>.</value>
        public Boolean IsMapped { get { return Mapper != null; } }

        /// <summary>
        /// Gets a value indicating whether the raster is readable.
        /// </summary>
        /// <value><c>true</c> if the raster is readable; otherwise, <c>false</c>.</value>
        public virtual Boolean IsReadable { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether the raster is writable.
        /// </summary>
        /// <value><c>true</c> if the raster is writable; otherwise, <c>false</c>.</value>
        public virtual Boolean IsWritable { get { return true; } }

        /// <summary>
        /// Gets the representation of the raster.
        /// </summary>
        /// <value>The representation of the raster.</value>
        public abstract RasterRepresentation Representation { get; }

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
        public IRasterBand this[Int32 index]
        {
            get
            {
                if (index < 0)
                    throw new IndexOutOfRangeException("The index is less than 0.");
                if (index >= _bands.Length)
                    throw new IndexOutOfRangeException("The index is equal to or greater than the number of bands in the raster.");

                return _bands[index];
            }
        }

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
        public UInt32 this[Int32 rowIndex, Int32 columnIndex, Int32 bandIndex]
        {
            get             
            {
                if (!IsReadable)
                    throw new NotSupportedException("The raster is not readable.");
                if (rowIndex < 0)
                    throw new IndexOutOfRangeException("The row index is less than 0.");
                if (rowIndex >= NumberOfRows)
                    throw new IndexOutOfRangeException("The row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new IndexOutOfRangeException("The column index is less than 0.");
                if (columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new IndexOutOfRangeException("The band index is less than 0.");
                if (bandIndex >= _bands.Length)
                    throw new IndexOutOfRangeException("The band index is equal to or greater than the number of bands.");

                return ApplyGetValue(rowIndex, columnIndex, bandIndex); 
            }
            set 
            {
                if (!IsWritable)
                    throw new NotSupportedException("The raster is not writable.");
                if (rowIndex < 0)
                    throw new IndexOutOfRangeException("The row index is less than 0.");
                if (rowIndex >= NumberOfRows)
                    throw new IndexOutOfRangeException("The row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new IndexOutOfRangeException("The column index is less than 0.");
                if (columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The column index is equal to or greater than the number of columns.");
                if (bandIndex < 0)
                    throw new IndexOutOfRangeException("The band index is less than 0.");
                if (bandIndex >= _bands.Length)
                    throw new IndexOutOfRangeException("The band index is equal to or greater than the number of bands.");

                ApplySetValue(rowIndex, columnIndex, bandIndex, value); 
            }
        }

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
        public UInt32 this[Coordinate coordinate, Int32 bandIndex]
        {
            get
            {
                if (!IsReadable)
                    throw new NotSupportedException("The raster is not readable.");
                if (Mapper == null)
                    throw new NotSupportedException("The mapping of the raster is not defined.");
                if (bandIndex < 0)
                    throw new IndexOutOfRangeException("The band index is less than 0.");
                if (bandIndex >= _bands.Length)
                    throw new IndexOutOfRangeException("The band index is equal to or greater than the number of bands.");

                Int32 rowIndex, columnIndex;
                Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

                if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The coordinate is not within the raster.");

                return ApplyGetValue(rowIndex, columnIndex, bandIndex);
            }
            set
            {
                if (!IsWritable)
                    throw new NotSupportedException("The raster is not writable.");
                if (Mapper == null)
                    throw new NotSupportedException("The mapping of the raster is not defined.");
                if (bandIndex < 0)
                    throw new IndexOutOfRangeException("The band index is less than 0.");
                if (bandIndex >= _bands.Length)
                    throw new IndexOutOfRangeException("The band index is equal to or greater than the number of bands.");

                Int32 rowIndex, columnIndex;
                Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

                if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The coordinate is not within the raster.");

                ApplySetValue(rowIndex, columnIndex, bandIndex, value);
            }
        }

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
        public UInt32[] this[Int32 rowIndex, Int32 columnIndex]
        {
            get
            {
                if (!IsReadable)
                    throw new NotSupportedException("The raster is not readable.");
                if (rowIndex < 0)
                    throw new IndexOutOfRangeException("The row index is less than 0.");
                if (rowIndex >= NumberOfRows)
                    throw new IndexOutOfRangeException("The row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new IndexOutOfRangeException("The column index is less than 0.");
                if (columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The column index is equal to or greater than the number of columns.");

                return ApplyGetValues(rowIndex, columnIndex);
            }
            set
            {
                if (!IsWritable)
                    throw new NotSupportedException("The raster is not writable.");
                if (rowIndex < 0)
                    throw new IndexOutOfRangeException("The row index is less than 0.");
                if (rowIndex >= NumberOfRows)
                    throw new IndexOutOfRangeException("The row index is equal to or greater than the number of rows.");
                if (columnIndex < 0)
                    throw new IndexOutOfRangeException("The column index is less than 0.");
                if (columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The column index is equal to or greater than the number of columns.");
                if (value == null)
                    throw new InvalidOperationException("The spectral values are not specified.");
                if (value.Length != _bands.Length)
                    throw new InvalidOperationException("The number of spectral values does not match the number of bands in the raster.");
            
                ApplySetValues(rowIndex, columnIndex, value);
            }
        }

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
        public UInt32[] this[Coordinate coordinate]
        {
            get
            {
                if (!IsReadable)
                    throw new NotSupportedException("The raster is not readable.");
                if (Mapper == null)
                    throw new NotSupportedException("The mapping of the raster is not defined.");

                Int32 rowIndex, columnIndex;
                Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

                if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The coordinate is not within the raster.");

                return ApplyGetValues(rowIndex, columnIndex);
            }
            set
            {
                if (!IsWritable)
                    throw new NotSupportedException("The raster is not writable.");
                if (Mapper == null)
                    throw new NotSupportedException("The mapping of the raster is not defined.");
                if (value == null)
                    throw new InvalidOperationException("The spectral values are not specified.");
                if (value.Length != _bands.Length)
                    throw new InvalidOperationException("The number of spectral values does not match the number of bands in the raster.");
            
                Int32 rowIndex, columnIndex;
                Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

                if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The coordinate is not within the raster.");

                ApplySetValues(rowIndex, columnIndex, value);
            }
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// Gets the maximum radiometric resolution.
        /// </summary>
        /// <value>The maximum radiometric resolution.</value>
        protected abstract Int32 MaxRadiometricResolution { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Raster" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The radiometric resolutions.</param>
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
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the spectral resolution.</exception>
        protected Raster(IRasterFactory factory, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper)
        {
            if (numberOfBands < 0)
                throw new ArgumentOutOfRangeException("numberOfBands", "The number of bands is less than 1.");
            if (numberOfRows < 0)
                throw new ArgumentOutOfRangeException("numberOfRows", "The number of rows is less than 0.");
            if (numberOfColumns < 0)
                throw new ArgumentOutOfRangeException("numberOfColumns", "The number of columns is less than 0.");
            if (radiometricResolutions != null && numberOfBands != radiometricResolutions.Count)
                throw new ArgumentException("The number of radiometric resolutions does not match the spectral resolution.", "radiometricResolutions");
            if (radiometricResolutions != null && radiometricResolutions.Count > 0 && (radiometricResolutions.Min() < 1 || radiometricResolutions.Max() > MaxRadiometricResolution))
                throw new ArgumentOutOfRangeException("radiometricResolutions", "Not all radiometric resolution values fall within the predefined range (1.." + MaxRadiometricResolution + ").");

            Factory = factory ?? AEGIS.Factory.DefaultInstance<RasterFactory>();

            NumberOfBands = numberOfBands;
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            Mapper = mapper;

            _bands = Enumerable.Range(0, NumberOfBands).Select(index => new RasterBand(this, index)).ToArray<IRasterBand>();

            if (radiometricResolutions != null)
                _radiometricResolutions = radiometricResolutions.ToArray();
            else
                _radiometricResolutions = Enumerable.Repeat(MaxRadiometricResolution, numberOfBands).ToArray();
        }

        #endregion

        #region IRaster methods

        /// <summary>
        /// Returns a band at a specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the band.</param>
        /// <returns>The raster band at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not writable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of bands in the raster.
        /// </exception>
        public IRasterBand GetBand(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _bands.Length)
                throw new ArgumentOutOfRangeException("index", "The index is equal to or greater than the number of bands in the raster.");

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
            if (!IsWritable)
                throw new NotSupportedException("The raster is not writable.");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            ApplySetValue(rowIndex, columnIndex, bandIndex, spectralValue);
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
        /// The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The coordinate is not within the raster.
        /// or
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public void SetValue(Coordinate coordinate, Int32 bandIndex, UInt32 spectralValue)
        {
            if (!IsWritable)
                throw new NotSupportedException("The raster is not writable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            ApplySetValue(rowIndex, columnIndex, bandIndex, spectralValue);
        }

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
        public void SetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            if (!IsWritable)
                throw new NotSupportedException("The raster is not writable.");
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The spectral values are not specified.");
            if (spectralValues.Length != _bands.Length)
                throw new ArgumentException("The number of spectral values does not match the number of bands in the raster.", "spectralValues");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");

            ApplySetValues(rowIndex, columnIndex, spectralValues);
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
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands in the raster.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public void SetValues(Coordinate coordinate, UInt32[] spectralValues)
        {
            if (!IsWritable)
                throw new NotSupportedException("The raster is not writable.");
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The spectral values are not specified.");
            if (spectralValues.Length != _bands.Length)
                throw new ArgumentException("The number of spectral values does not match the number of bands in the raster.", "spectralValues");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            ApplySetValues(rowIndex, columnIndex, spectralValues);
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
        public void SetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
        {
            if (!IsWritable)
                throw new NotSupportedException("The raster is not writable.");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            ApplySetFloatValue(rowIndex, columnIndex, bandIndex, spectralValue);
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
        public virtual void SetFloatValue(Coordinate coordinate, Int32 bandIndex, Double spectralValue)
        {
            if (!IsWritable)
                throw new NotSupportedException("The raster is not writable.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            ApplySetFloatValue(rowIndex, columnIndex, bandIndex, spectralValue);
        }

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
        public void SetFloatValues(Int32 rowIndex, Int32 columnIndex, Double[] spectralValues)
        {
            if (!IsWritable)
                throw new NotSupportedException("The raster is not writable.");
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The spectral values are not specified.");
            if (spectralValues.Length != _bands.Length)
                throw new ArgumentException("The number of spectral values does not match the number of bands in the raster.", "spectralValues");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");

            ApplySetFloatValues(rowIndex, columnIndex, spectralValues);
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
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands in the raster.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public void SetFloatValues(Coordinate coordinate, Double[] spectralValues)
        {
            if (!IsWritable)
                throw new NotSupportedException("The raster is not writable.");
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The spectral values are not specified.");
            if (spectralValues.Length != _bands.Length)
                throw new ArgumentException("The number of spectral values does not match the number of bands in the raster.", "spectralValues");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            ApplySetFloatValues(rowIndex, columnIndex, spectralValues);
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
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            return ApplyGetValue(rowIndex, columnIndex, bandIndex);
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
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return ApplyGetValue(rowIndex, columnIndex, bandIndex);
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
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");

            return ApplyGetValues(rowIndex, columnIndex);
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
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return ApplyGetValues(rowIndex, columnIndex);
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
        public Double GetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            return ApplyGetFloatValue(rowIndex, columnIndex, bandIndex);
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
        public Double GetFloatValue(Coordinate coordinate, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return ApplyGetFloatValue(rowIndex, columnIndex, bandIndex);
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
        public Double[] GetFloatValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= NumberOfRows)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is equal to or greater than the number of columns.");

            return ApplyGetFloatValues(rowIndex, columnIndex);
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
        public Double[] GetFloatValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return ApplyGetFloatValues(rowIndex, columnIndex);
        }

        /// <summary>
        /// Returns the boxed spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the equidistant index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public UInt32 GetBoxedValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            Int32 trueRowIndex = rowIndex >= NumberOfRows ? -(NumberOfRows - rowIndex) : Math.Abs(rowIndex);
            Int32 trueColumnIndex = columnIndex >= NumberOfRows ? -(NumberOfColumns - columnIndex) : Math.Abs(columnIndex);     

            return ApplyGetValue(trueRowIndex, trueColumnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the boxed spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>Returns the boxed spectral value in a band at the specified coordinate or at the equidistant coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public UInt32 GetBoxedValue(Coordinate coordinate, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");
            
            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetBoxedValue(rowIndex, columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the boxed spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="k">The zero-based row index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the equidistant index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        public UInt32[] GetBoxedValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");

            Int32 trueRowIndex = rowIndex >= NumberOfRows ? -(NumberOfRows - rowIndex) : Math.Abs(rowIndex);
            Int32 trueColumnIndex = columnIndex >= NumberOfRows ? -(NumberOfColumns - columnIndex) : Math.Abs(columnIndex);     

            return ApplyGetValues(trueRowIndex, trueColumnIndex);
        }

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
        public UInt32[] GetBoxedValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex; 
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetBoxedValues(rowIndex, columnIndex);
        }

        /// <summary>
        /// Returns the boxed spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the equidistant index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public Double GetBoxedFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            Int32 trueRowIndex = rowIndex >= NumberOfRows ? -(NumberOfRows - rowIndex) : Math.Abs(rowIndex);
            Int32 trueColumnIndex = columnIndex >= NumberOfRows ? -(NumberOfColumns - columnIndex) : Math.Abs(columnIndex);     

            return ApplyGetFloatValue(trueRowIndex, trueColumnIndex, bandIndex);
        }
        
        /// <summary>
        /// Returns the boxed spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>Returns the boxed spectral value in a band at the specified coordinate or at the equidistant coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public Double GetBoxedFloatValue(Coordinate coordinate, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetBoxedFloatValue(rowIndex, columnIndex, bandIndex);
        }
        
        /// <summary>
        /// Returns the boxed spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="k">The zero-based row index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the equidistant index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        public Double[] GetBoxedFloatValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");

            Int32 trueRowIndex = rowIndex >= NumberOfRows ? -(NumberOfRows - rowIndex) : Math.Abs(rowIndex);
            Int32 trueColumnIndex = columnIndex >= NumberOfRows ? -(NumberOfColumns - columnIndex) : Math.Abs(columnIndex);     

            return ApplyGetFloatValues(trueRowIndex, trueColumnIndex);
        }
        
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
        public Double[] GetBoxedFloatValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetBoxedFloatValues(rowIndex, columnIndex);
        }

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public UInt32 GetNearestValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            Int32 trueRowIndex = Math.Min(Math.Max(rowIndex, 0), NumberOfRows - 1);
            Int32 trueColumnIndex = Math.Min(Math.Max(columnIndex, 0), NumberOfColumns - 1);

            return ApplyGetValue(trueRowIndex, trueColumnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>Returns the nearest spectral value in a band at the specified coordinate or the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public UInt32 GetNearestValue(Coordinate coordinate, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetNearestValue(rowIndex, columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="k">The zero-based row index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        public UInt32[] GetNearestValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");

            Int32 trueRowIndex = Math.Min(Math.Max(rowIndex, 0), NumberOfRows - 1);
            Int32 trueColumnIndex = Math.Min(Math.Max(columnIndex, 0), NumberOfColumns - 1);

            return ApplyGetValues(trueRowIndex, trueColumnIndex);
        }

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified coordinate or the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        public UInt32[] GetNearestValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetNearestValues(rowIndex, columnIndex);
        }

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral values for the band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public Double GetNearestFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            Int32 trueRowIndex = Math.Min(Math.Max(rowIndex, 0), NumberOfRows - 1);
            Int32 trueColumnIndex = Math.Min(Math.Max(columnIndex, 0), NumberOfColumns - 1);

            return ApplyGetFloatValue(trueRowIndex, trueColumnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the nearest spectral value in a band to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>Returns the nearest spectral value in a band at the specified coordinate or the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public Double GetNearestFloatValue(Coordinate coordinate, Int32 bandIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetNearestFloatValue(rowIndex, columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the values.</param>
        /// <param name="k">The zero-based row index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index or the nearest index if either row or column is out of range.</returns>
        /// <exception cref="System.NotSupportedException">The raster is not readable.</exception>
        public Double[] GetNearestFloatValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");

            Int32 trueRowIndex = Math.Min(Math.Max(rowIndex, 0), NumberOfRows - 1);
            Int32 trueColumnIndex = Math.Min(Math.Max(columnIndex, 0), NumberOfColumns - 1);

            return ApplyGetFloatValues(trueRowIndex, trueColumnIndex);
        }

        /// <summary>
        /// Returns the nearest spectral values in all bands to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The array containing the spectral values for each band at the specified coordinate or the nearest coordinate if the coordinate is out of range.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The raster is not readable.
        /// or
        /// The mapping of the raster is not defined.
        /// </exception>
        public Double[] GetNearestFloatValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

            return GetNearestFloatValues(rowIndex, columnIndex);
        }

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
        public IList<Int32> GetHistogramValues(Int32 bandIndex)
        {
            if (!IsReadable || Representation == RasterRepresentation.Floating)
                throw new NotSupportedException("Histogram query is not supported.");

            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= _bands.Length)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            return ApplyGetHistogramValues(bandIndex);
        }

        #endregion        

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="Raster" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="Raster" /> instance.</returns>
        public abstract Object Clone();

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return "Raster [" + NumberOfRows + "x" + NumberOfColumns + "x" + _bands.Length + "]";
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        protected abstract void ApplySetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue);

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        protected abstract void ApplySetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues);
        
        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        protected abstract void ApplySetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue);

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        protected abstract void ApplySetFloatValues(Int32 rowIndex, Int32 columnIndex, Double[] spectralValues);

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected abstract UInt32 ApplyGetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected abstract UInt32[] ApplyGetValues(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected abstract Double ApplyGetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex);

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected abstract Double[] ApplyGetFloatValues(Int32 rowIndex, Int32 columnIndex);

        /// <summary>
        /// Gets the histogram values of a specified band.
        /// </summary>
        /// <param name="bandIndex">The zero-based index of the band.</param>
        /// <returns>The read-only list containing the histogram values for the specified band.<returns>
        protected virtual IList<Int32> ApplyGetHistogramValues(Int32 bandIndex) { return null; }

        #endregion
    }
}
