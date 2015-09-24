/// <copyright file="MultiRaster.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
    /// Represents a collection of rasters.
    /// </summary>
    public class MultiRaster : IRaster
    {
        #region Private fields

        private IRaster[] _rasters;
        private Dictionary<Int32, Int32> _rasterIndexDictionary;
        private Dictionary<Int32, Int32> _rasterBandIndexDictionary;

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets the factory of the raster.
        /// </summary>
        /// <value>The factory implementation the raster was constructed by.</value>
        public IRasterFactory Factory
        {
            get { return _rasters[0].Factory; }
        }
        
        /// <summary>
        /// Gets the number of spectral values in a row.
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        public Int32 NumberOfColumns
        {
            get { return _rasters[0].NumberOfColumns; }
        }
        
        /// <summary>
        /// Gets the number of spectral values in a column.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        public Int32 NumberOfRows
        {
            get { return _rasters[0].NumberOfRows; }
        }
        
        /// <summary>
        /// Gets the number of bands in the raster.
        /// </summary>
        /// <value>The number of spectral bands contained in the raster.</value>
        public Int32 NumberOfBands { get; private set; }
        
        /// <summary>
        /// Gets the spectral bands of the raster.
        /// </summary>
        /// <value>The read-only list containing the spectral bands in the raster.</value>
        public IList<IRasterBand> Bands
        {
            get 
            {
                List<IRasterBand> bands = new List<IRasterBand>(NumberOfBands);
                foreach (IRaster raster in _rasters)
                    bands.AddRange(raster.Bands);
                return bands.AsReadOnly();
            }
        }
        
        /// <summary>
        /// Gets the radiometric resolutions of the bands in the raster.
        /// </summary>
        /// <value>The read-only list containing the radiometric resolution of each band in the raster.</value>
        public IList<Int32> RadiometricResolutions
        {
            get
            {
                List<Int32> bands = new List<Int32>(NumberOfBands);
                foreach (IRaster raster in _rasters)
                    bands.AddRange(raster.RadiometricResolutions);
                return bands.AsReadOnly();
            }
        }
        
        /// <summary>
        /// Gets the histogram values of the raster.
        /// </summary>
        /// <value>The read-only list containing the histogram values of each band in the raster.</value>
        /// <exception cref="System.NotSupportedException">Histogram query is not supported.</exception>
        public IList<IList<Int32>> HistogramValues
        {
            get
            {
                List<IList<Int32>> bands = new List<IList<Int32>>(NumberOfBands);
                foreach (IRaster raster in _rasters)
                    bands.AddRange(raster.HistogramValues);
                return bands.AsReadOnly();
            }
        }
        
        /// <summary>
        /// Gets the bounding coordinates of the raster.
        /// </summary>
        /// <value>The read-only list containing the bounding coordinates of the raster in counterclockwise order.</value>
        public IList<Coordinate> Coordinates
        {
            get { return _rasters[0].Coordinates; }
        }
        
        /// <summary>
        /// Gets the raster mapper used for mapping raster space to coordinate space.
        /// </summary>
        /// <value>The raster mapper associated with the raster.</value>
        public RasterMapper Mapper
        {
            get { return _rasters[0].Mapper; }
        }

        /// <summary>
        /// Get a value indicating whether the raster is mapped to coordinate space.
        /// </summary>
        /// <value><c>true</c> if the raster is mapped to coordinate spacet; otherwise, <c>false</c>.</value>
        public Boolean IsMapped
        {
            get { return _rasters.All(raster => raster.IsMapped); }
        }
        
        /// <summary>
        /// Gets a value indicating whether the raster is readable.
        /// </summary>
        /// <value><c>true</c> if the raster is readable; otherwise, <c>false</c>.</value>
        public Boolean IsReadable
        {
            get { return _rasters.All(raster => raster.IsReadable); }
        }
        
        /// <summary>
        /// Gets a value indicating whether the raster is writable.
        /// </summary>
        /// <value><c>true</c> if the raster is writable; otherwise, <c>false</c>.</value>
        public Boolean IsWritable
        {
            get { return _rasters.All(raster => raster.IsWritable); }
        }

        /// <summary>
        /// Gets the format of the raster.
        /// </summary>
        /// <value>The format of the raster.</value>
        public RasterFormat Format
        {
            get { return _rasters[0].Format; }
        }

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
                if (index >= NumberOfBands)
                    throw new IndexOutOfRangeException("The index is equal to or greater than the number of bands in the raster.");
                
                return _rasters[_rasterIndexDictionary[index]].GetBand(_rasterBandIndexDictionary[index]);
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
                if (bandIndex >= NumberOfBands)
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
                if (bandIndex >= NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
                    throw new NotSupportedException("The geometry of the raster is not defined.");
                if (bandIndex < 0)
                    throw new IndexOutOfRangeException("The band index is less than 0.");
                if (bandIndex >= NumberOfBands)
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
                    throw new NotSupportedException("The geometry of the raster is not defined.");
                if (bandIndex < 0)
                    throw new IndexOutOfRangeException("The band index is less than 0.");
                if (bandIndex >= NumberOfBands)
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
                if (value.Length != NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
                    throw new NotSupportedException("The geometry of the raster is not defined.");

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
                    throw new NotSupportedException("The geometry of the raster is not defined.");
                if (value == null)
                    throw new InvalidOperationException("The spectral values are not specified.");
                if (value.Length != NumberOfBands)
                    throw new InvalidOperationException("The number of spectral values does not match the number of bands in the raster.");
            
                Int32 rowIndex, columnIndex;
                Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);

                if (rowIndex < 0 || rowIndex >= NumberOfRows || columnIndex < 0 || columnIndex >= NumberOfColumns)
                    throw new IndexOutOfRangeException("The coordinate is not within the raster.");

                ApplySetValues(rowIndex, columnIndex, value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiRaster"/> class.
        /// </summary>
        /// <param name="rasters">The rasters.</param>
        /// <exception cref="System.ArgumentNullException">No rasters are specified.</exception>
        /// <exception cref="System.ArgumentException">
        /// One or more rasters are null.
        /// or
        /// The rasters do not have the same dimension.
        /// or
        /// The rasters do not have the same format.
        /// </exception>
        public MultiRaster(params IRaster[] rasters)
        {
            if (rasters == null || rasters.Length == 0)
                throw new ArgumentNullException("rasters", "No rasters are specified.");
            if (rasters.Any(raster => raster == null))
                throw new ArgumentException("One or more rasters are null.", "rasters");
            if (rasters.Any(raster => raster.NumberOfRows != rasters[0].NumberOfRows) ||
                rasters.Any(raster => raster.NumberOfBands != rasters[0].NumberOfBands))
                throw new ArgumentException("The rasters do not have the same dimension.", "rasters");
            if (rasters.Any(raster => raster.Format != rasters[0].Format))
                throw new ArgumentException("The rasters do not have the same format.", "rasters");

            NumberOfBands = rasters.Sum(raster => raster.NumberOfBands);

            _rasters = rasters;
            _rasterIndexDictionary = new Dictionary<Int32, Int32>(NumberOfBands);
            _rasterBandIndexDictionary = new Dictionary<Int32, Int32>(NumberOfBands);

            Int32 rasterIndex = 0;
            Int32 rasterBandIndex = 0;
            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                _rasterBandIndexDictionary[bandIndex] = rasterBandIndex;
                _rasterIndexDictionary[bandIndex] = rasterIndex;

                rasterBandIndex++;

                if (rasterBandIndex == _rasters[rasterIndex].NumberOfBands)
                {
                    rasterIndex++;
                    rasterBandIndex = 0;
                }
            }
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
            if (index >= NumberOfBands)
                throw new ArgumentOutOfRangeException("index", "The index is equal to or greater than the number of bands in the raster.");

            return _rasters[_rasterIndexDictionary[index]].GetBand(_rasterBandIndexDictionary[index]);
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
            if (bandIndex >= NumberOfBands)
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
        /// The geometry of the raster is not defined.</exception>
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
                throw new NotSupportedException("The geometry of the raster is not defined.");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= NumberOfBands)
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
            if (spectralValues.Length != NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands in the raster.", "spectralValues");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (bandIndex >= NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
            if (bandIndex >= NumberOfBands)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (spectralValues.Length != NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands in the raster.", "spectralValues");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (bandIndex >= NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
            if (bandIndex >= NumberOfBands)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
        /// The geometry of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public UInt32[] GetValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (bandIndex >= NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
            if (bandIndex >= NumberOfBands)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
        /// The geometry of the raster is not defined.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The coordinate is not within the raster.</exception>
        public Double[] GetFloatValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (bandIndex >= NumberOfBands)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            Int32 trueRowIndex = rowIndex >= NumberOfRows ? -(NumberOfRows - rowIndex) : Math.Abs(rowIndex);
            Int32 trueColumnIndex = columnIndex >= NumberOfColumns ? -(NumberOfColumns - columnIndex) : Math.Abs(columnIndex);     

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
        /// The geometry of the raster is not defined.
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
                throw new NotSupportedException("The geometry of the raster is not defined.");
            
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
            Int32 trueColumnIndex = columnIndex >= NumberOfColumns ? -(NumberOfColumns - columnIndex) : Math.Abs(columnIndex);     

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
        /// The geometry of the raster is not defined.
        /// </exception>
        public UInt32[] GetBoxedValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (bandIndex >= NumberOfBands)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            Int32 trueRowIndex = rowIndex >= NumberOfRows ? -(NumberOfRows - rowIndex) : Math.Abs(rowIndex);
            Int32 trueColumnIndex = columnIndex >= NumberOfColumns ? -(NumberOfColumns - columnIndex) : Math.Abs(columnIndex);     

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
        /// The geometry of the raster is not defined.
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
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            Int32 trueColumnIndex = columnIndex >= NumberOfColumns ? -(NumberOfColumns - columnIndex) : Math.Abs(columnIndex);     

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
        /// The geometry of the raster is not defined.
        /// </exception>
        public Double[] GetBoxedFloatValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (bandIndex >= NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
        /// The geometry of the raster is not defined.
        /// </exception>
        public UInt32[] GetNearestValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (bandIndex >= NumberOfBands)
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
        /// The geometry of the raster is not defined.
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
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
        /// The geometry of the raster is not defined.
        /// </exception>
        public Double[] GetNearestFloatValues(Coordinate coordinate)
        {
            if (!IsReadable)
                throw new NotSupportedException("The raster is not readable.");
            if (Mapper == null)
                throw new NotSupportedException("The geometry of the raster is not defined.");

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
            if (!IsReadable || Format == RasterFormat.Floating)
                throw new NotSupportedException("Histogram query is not supported.");

            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is less than 0.");
            if (bandIndex >= NumberOfBands)
                throw new ArgumentOutOfRangeException("bandIndex", "The band index is equal to or greater than the number of bands.");

            return ApplyGetHistogramValues(bandIndex);
        }

        #endregion    
        
        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="Raster" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="Raster" /> instance.</returns>
        public Object Clone()
        {
            return new MultiRaster(_rasters);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        private void ApplySetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
        {
            _rasters[_rasterIndexDictionary[bandIndex]].SetValue(rowIndex, columnIndex, _rasterBandIndexDictionary[bandIndex], spectralValue);
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        private void ApplySetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
                _rasters[_rasterIndexDictionary[bandIndex]].SetValue(rowIndex, columnIndex, _rasterBandIndexDictionary[bandIndex], spectralValues[bandIndex]);
        }

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        private void ApplySetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
        {
            _rasters[_rasterIndexDictionary[bandIndex]].SetFloatValue(rowIndex, columnIndex, _rasterBandIndexDictionary[bandIndex], spectralValue);
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        private void ApplySetFloatValues(Int32 rowIndex, Int32 columnIndex, Double[] spectralValues)
        {
            for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
                _rasters[_rasterIndexDictionary[bandIndex]].SetFloatValue(rowIndex, columnIndex, _rasterBandIndexDictionary[bandIndex], spectralValues[bandIndex]);
        }

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        private UInt32 ApplyGetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return _rasters[_rasterIndexDictionary[bandIndex]].GetValue(rowIndex, columnIndex, _rasterBandIndexDictionary[bandIndex]);
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        private UInt32[] ApplyGetValues(Int32 rowIndex, Int32 columnIndex)
        {
            UInt32[] spectralValues = new UInt32[NumberOfBands];

            for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
                spectralValues[bandIndex] = _rasters[_rasterIndexDictionary[bandIndex]].GetValue(rowIndex, columnIndex, _rasterBandIndexDictionary[bandIndex]);

            return spectralValues;
        }

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        private Double ApplyGetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return _rasters[_rasterIndexDictionary[bandIndex]].GetFloatValue(rowIndex, columnIndex, _rasterBandIndexDictionary[bandIndex]);
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        private Double[] ApplyGetFloatValues(Int32 rowIndex, Int32 columnIndex)
        {
            Double[] spectralValues = new Double[NumberOfBands];

            for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
                spectralValues[bandIndex] = _rasters[_rasterIndexDictionary[bandIndex]].GetFloatValue(rowIndex, columnIndex, _rasterBandIndexDictionary[bandIndex]);

            return spectralValues;
        }

        /// <summary>
        /// Gets the histogram values of a specified band.
        /// </summary>
        /// <param name="bandIndex">The zero-based index of the band.</param>
        /// <returns>The read-only list containing the histogram values for the specified band.<returns>
        private IList<Int32> ApplyGetHistogramValues(Int32 bandIndex) 
        { 
            return _rasters[_rasterIndexDictionary[bandIndex]].GetHistogramValues(_rasterBandIndexDictionary[bandIndex]); 
        }

        #endregion
    }
}
