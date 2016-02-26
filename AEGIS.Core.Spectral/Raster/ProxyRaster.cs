/// <copyright file="ProxyRaster.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a raster image managing values in a entity contains integer values.
    /// </summary>
    public class ProxyRaster : Raster
    {
        #region Private fields

        /// <summary>
        /// The underlying raster service.
        /// </summary>
        private readonly IRasterService _service;

        /// <summary>
        /// A value indicating whether the service supports sequential order.
        /// </summary>
        private readonly Boolean _isSequentialService;

        /// <summary>
        /// The list of histogram values.
        /// </summary>
        private Int32[][] _histogramValues;

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets a value indicating whether the raster is readable.
        /// </summary>
        /// <value><c>true</c> if the raster is readable; otherwise, <c>false</c>.</value>
        public override Boolean IsReadable { get { return _service.IsReadable; } }

        /// <summary>
        /// Gets a value indicating whether the raster is writable.
        /// </summary>
        /// <value><c>true</c> if the raster is writable; otherwise, <c>false</c>.</value>
        public override Boolean IsWritable { get { return _service.IsWritable; } }

        /// <summary>
        /// Gets the format of the raster.
        /// </summary>
        /// <value>The format of the raster.</value>
        public override RasterFormat Format { get { return _service.Format; } }

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
        /// Initializes a new instance of the <see cref="ProxyRaster" /> class.
        /// </summary>
        /// <param name="factory">The raster factory.</param>
        /// <param name="service">The raster service.</param>
        /// <param name="mapper">The raster mapper.</param>
        /// <exception cref="System.ArgumentNullException">The service is null.</exception>
        public ProxyRaster(IRasterFactory factory, IRasterService service, RasterMapper mapper)
            : base(factory, GetNumberOfBands(service), GetNumberOfRows(service), GetNumberOfColumns(service), GetRadiometricResolutions(service), mapper)
        {
            if (service == null)
                throw new ArgumentNullException("service", "The service is null.");
            
            _service = service;
            _isSequentialService = _service.SupportedOrders.Contains(RasterDataOrder.RowColumnBand);

            _histogramValues = Enumerable.Repeat<Int32[]>(null, NumberOfRows).ToArray();
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="ProxyRaster" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="ProxyRaster" /> instance.</returns>
        public override Object Clone()
        {
            return new ProxyRaster(Factory, _service, Mapper);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return "Raster [" + NumberOfRows + "x" + NumberOfColumns + "x" + NumberOfBands + "] (on service " + _service.ToString() + ")";
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
            _service.WriteValue(rowIndex, columnIndex, bandIndex, spectralValue);
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        protected override void ApplySetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            if (_isSequentialService)
                _service.WriteValueSequence(rowIndex, columnIndex, 0, spectralValues);
            else
            {
                for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
                    _service.WriteValue(rowIndex, columnIndex, bandIndex, spectralValues[bandIndex]);
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
            _service.WriteFloatValue(rowIndex, columnIndex, bandIndex, spectralValue);
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        protected override void ApplySetFloatValues(Int32 rowIndex, Int32 columnIndex, Double[] spectralValues)
        {
            if (_isSequentialService)
                _service.WriteFloatValueSequence(rowIndex, columnIndex, 0, spectralValues);
            else
            {
                for (Int32 bandIndex = 0; bandIndex < spectralValues.Length; bandIndex++)
                    _service.WriteFloatValue(rowIndex, columnIndex, bandIndex, spectralValues[bandIndex]);
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
            return _service.ReadValue(rowIndex, columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] ApplyGetValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (_isSequentialService)
                return _service.ReadValueSequence(rowIndex, columnIndex, 0, NumberOfBands);
            else
            {
                UInt32[] values = new UInt32[NumberOfBands];
                for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
                    values[bandIndex] = _service.ReadValue(rowIndex, columnIndex, bandIndex);

                return values;
            }
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
            return _service.ReadFloatValue(rowIndex, columnIndex, bandIndex);
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ApplyGetFloatValues(Int32 rowIndex, Int32 columnIndex)
        {
            if (_isSequentialService)
                return _service.ReadFloatValueSequence(rowIndex, columnIndex, 0, NumberOfBands);
            else
            {
                Double[] values = new Double[NumberOfBands];
                for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
                    values[bandIndex] = _service.ReadFloatValue(rowIndex, columnIndex, bandIndex);

                return values;
            }
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
                _histogramValues[bandIndex] = new Int32[1UL << _service.RadiometricResolutions[bandIndex]];
                for (Int32 rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < NumberOfColumns; columnIndex++)
                    {
                        _histogramValues[bandIndex][_service.ReadValue(rowIndex, columnIndex, bandIndex)]++;
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
        private static Int32 GetNumberOfBands(IRasterService source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            return source.NumberOfBands;
        }

        /// <summary>
        /// Returns the number of rows of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The number of rows.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        private static Int32 GetNumberOfRows(IRasterService source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            return source.NumberOfRows;
        }

        /// <summary>
        /// Returns the number of columns of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The number of columns.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        private static Int32 GetNumberOfColumns(IRasterService source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            return source.NumberOfColumns;
        }

        /// <summary>
        /// Returns the radiometric resolutions of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The radiometric resolutions of the specified source.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        private static IList<Int32> GetRadiometricResolutions(IRasterService source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            return source.RadiometricResolutions;
        }

        #endregion
    }
}
