using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Collections.Spectral.Mask
{
    /// <summary>
    /// Represents a masked band of a multispectral raster image.
    /// </summary>
    public class MaskedRasterBand : IRasterBand
    {
        #region Protected fields

        protected IRasterBand _source;

        protected Int32[] _histogramValues;
        protected Int32 _rowStart;
        protected Int32 _rowCount;
        protected Int32 _columnCount;
        protected Int32 _columnStart;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the source band of the mask
        /// </summary>
        /// <value>The source band of the mask.</value>
        public IRasterBand Band { get { return _source; } }

        #endregion



        #region IRasterBand properties

        /// <summary>
        /// Gets the raster where the band is located.
        /// </summary>
        /// <value>The raster where the band is located.</value>
        public IRaster Raster { get { return _source.Raster; } }
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
        /// Gets the radiometric resolution of the band.
        /// </summary>
        /// <value>The number of bits used for storing spectral values.</value>
        public Int32 RadiometricResolution { get { return _source.RadiometricResolution; } }
        /// <summary>
        /// Gets the histogram values of the band.
        /// </summary>
        /// <value>The histogram values of the band.</value>
        public IList<Int32> HistogramValues
        {
            get
            {
                if (_histogramValues == null)
                {
                    _histogramValues = new Int32[1UL << _source.RadiometricResolution];
                    for (Int32 i = _rowStart; i < _rowStart + _rowCount; i++)
                        for (Int32 j = _columnStart; j < _columnStart + _columnCount; j++)
                        {
                            _histogramValues[_source.GetValue(i, j)]++;
                        }
                }
                return Array.AsReadOnly(_histogramValues);
            }
        }
        /// <summary>
        /// Gets the spectral range of the band.
        /// </summary>
        /// <value>The spectral range of the band.</value>
        public SpectralRange SpectralRange { get { return _source.SpectralRange; } }
        /// <summary>
        /// Gets a value indicating whether the raster is read-only.
        /// </summary>
        /// <value><c>true</c> if the raster is read-only; otherwise <c>false</c>.</value>
        public Boolean IsReadOnly { get { return _source.IsReadOnly; } }
        /// <summary>
        /// Gets the representation of the raster.
        /// </summary>
        /// <value>The representation of the raster.</value>
        public RasterRepresentation Representation { get { return _source.Representation; } }
        /// <summary>
        /// Gets or sets a spectral value at a specified row and column index.
        /// </summary>
        /// <value>The spectral value located at the specified coordinate.</value>
        /// <param name="x">The zero-based column index of the value.</param>
        /// <param name="y">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// rowIndex;The row index is less than 0.
        /// or
        /// rowIndex;The row index is equal to or greater than the number of rows.
        /// or
        /// columnIndex;The column index is less than 0.
        /// or
        /// columnIndex;the column index is equal to or greater than the number of columns.
        /// </exception>
        public UInt32 this[Int32 rowIndex, Int32 columnIndex] { get { return GetValue(rowIndex, columnIndex); } set { SetValue(rowIndex, columnIndex, value); } }
        /// <summary>
        /// Gets or sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="x">The zero-based column index of the pixel.</param>
        /// <param name="y">The zero-based row index of the pixel.</param>
        /// <value>The spectral value located at the specified coordinate.</value>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">coordinate;The coordinate is not within the raster.</exception>
        public UInt32 this[Coordinate coordinate] { get { return GetValue(coordinate); } set { SetValue(coordinate, value); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Collections.Spectral.MaskedRasterBand" /> class.
        /// </summary>
        /// <param name="source">The source raster band.</param>
        /// <param name="rowStart">The starting row index of a mask.</param>
        /// <param name="rowCount">The number of rows in the mask.</param>
        /// <param name="columnStart">The starting column index of a mask.</param>
        /// <param name="columnCount">The number of columns in the mask.</param>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// rowStart;The starting row index is less than 0.
        /// or
        /// rowStart;The starting row index is equal to or greater than the number of rows in the source.
        /// or
        /// columnIndex;The starting column index is less than 0.
        /// or
        /// columnIndex;the starting column index is equal to or greater than the number of columns in the source.
        /// or
        /// rowCount;The starting row index and the row count is greater than the number of rows in the source.
        /// or
        /// columnCount;The starting columns index and the column count is greater than the number of rows in the source.
        /// </exception>
        public MaskedRasterBand(IRasterBand source, Int32 rowStart, Int32 rowCount, Int32 columnStart, Int32 columnCount)
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
                throw new ArgumentOutOfRangeException("columnIndex", "The starting column index is equal to or greater than the number of columns in the source.");
            if (rowStart + rowCount > source.NumberOfRows)
                throw new ArgumentOutOfRangeException("rowCount", "The starting row index and the row count is greater than the number of rows in the source.");
            if (columnStart + columnCount > source.NumberOfRows)
                throw new ArgumentOutOfRangeException("columnCount", "The starting columns index and the column count is greater than the number of rows in the source.");

            _source = source;
            _rowStart = rowStart;
            _rowCount = rowCount;
            _columnStart = columnStart;
            _columnCount = columnCount;
        }

        #endregion

        #region IRasterBand methods

        /// <summary>
        /// Sets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// rowIndex;The row index is less than 0.
        /// or
        /// rowIndex;The row index is equal to or greater than the number of rows.
        /// or
        /// columnIndex;The column index is less than 0.
        /// or
        /// columnIndex;the column index is equal to or greater than the number of columns.
        /// </exception>
        public void SetValue(Int32 rowIndex, Int32 columnIndex, UInt32 spectralValue)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _rowCount)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("columnIndex", "the column index is equal to or greater than the number of columns.");

            _source.SetValue(_rowStart + rowIndex, _columnStart + columnIndex, spectralValue);
        }
        /// <summary>
        /// Sets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">coordinate;The coordinate is not within the raster.</exception>
        public void SetValue(Coordinate coordinate, UInt32 spectralValue)
        {
            _source.SetValue(coordinate, spectralValue);
        }
        /// <summary>
        /// Gets a spectral value at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// rowIndex;The row index is less than 0.
        /// or
        /// rowIndex;The row index is equal to or greater than the number of rows.
        /// or
        /// columnIndex;The column index is less than 0.
        /// or
        /// columnIndex;the column index is equal to or greater than the number of columns.
        /// </exception>
        public UInt32 GetValue(Int32 rowIndex, Int32 columnIndex)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is less than 0.");
            if (rowIndex >= _rowCount)
                throw new ArgumentOutOfRangeException("rowIndex", "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "The column index is less than 0.");
            if (columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("columnIndex", "the column index is equal to or greater than the number of columns.");

            return _source.GetValue(_rowStart + rowIndex, _columnStart + columnIndex);
        }
        /// <summary>
        /// Gets a spectral value at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">coordinate;The coordinate is not within the raster.</exception>
        public UInt32 GetValue(Coordinate coordinate)
        {
            if (_source.Raster.Mapper == null)
                throw new InvalidOperationException("The mapping of the raster is not defined.");

            Int32 rowIndex, columnIndex;
            _source.Raster.Mapper.MapRaster(coordinate, out rowIndex, out columnIndex);
            if (rowIndex < _rowStart || rowIndex >= _rowStart + _rowCount || columnIndex < _columnStart || columnIndex >= _columnCount)
                throw new ArgumentOutOfRangeException("coordinate", "The coordinate is not within the raster.");

            return _source.GetValue(_rowStart + rowIndex, _columnStart + columnIndex);
        }
        /// <summary>
        /// Gets the nearest spectral value to a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        /// <returns>The spectral value located at the specified row and column index or in the nearest index if either the row or column indices are out of range.</returns>
        public UInt32 GetNearestValue(Int32 rowIndex, Int32 columnIndex)
        {
            Int32 trueRowIndex = _rowStart + Math.Min(Math.Max(rowIndex, 0), _rowCount - 1);
            Int32 trueColumnIndex = _columnStart + Math.Min(Math.Max(columnIndex, 0), _columnCount - 1);
            return _source.GetValue(trueRowIndex, trueColumnIndex);
        }
        /// <summary>
        /// Gets the nearest spectral value to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The spectral value located at the specified coordinate or at the nearest index if the coordinate is out of range.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        public UInt32 GetNearestValue(Coordinate coordinate)
        {
            if (_source.Raster.Mapper == null)
                throw new InvalidOperationException("The mapping of the raster is not defined.");

            return _source.GetValue(coordinate);
        }

        #endregion        
        
        #region Object methods

        /// <summary>
        /// Returns the <see cref="T:System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="T:System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return "RasterBand [" + NumberOfRows + "x" + NumberOfColumns + "x" + RadiometricResolution + "]";
        }

        #endregion
    }
}
