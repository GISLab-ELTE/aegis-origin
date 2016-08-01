/// <copyright file="SegmentedRaster.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections.Segmentation;
using System;
using System.Linq;

namespace ELTE.AEGIS.Raster
{
    /// <summary>
    /// Represents a raster which was segmented.
    /// </summary>
    public class SegmentedRaster : Raster, ISegmentedRaster
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentedRaster"/> class.
        /// </summary>
        /// <param name="segments">The collection of segments.</param>
        /// <exception cref="System.ArgumentNullException">The segment collection is null.</exception>
        /// <exception cref="System.ArgumentException">The segment collection has no raster.</exception>
        public SegmentedRaster(SegmentCollection segments) : base(segments.Raster.Factory, segments.Raster.Dimensions, segments.Raster.Mapper)
        {
            Segments = segments;
        }

        #endregion

        #region IRaster properties

        /// <summary>
        /// Gets the format of the raster.
        /// </summary>
        /// <value>The format of the raster.</value>
        public override RasterFormat Format
        {
            get
            {
                return Segments.Raster.Format;
            }
        }

        #endregion

        #region ISegmentedRaster properties

        /// <summary>
        /// Gets the collection of segments.
        /// </summary>
        /// <value>The collection of segments.</value>
        public SegmentCollection Segments { get; private set; }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="Raster" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="Raster" /> instance.</returns>
        public override Object Clone()
        {
            return new SegmentedRaster(new SegmentCollection(Segments));
        }

        #endregion

        #region Protected Raster methods

        /// <summary>
        /// Returns the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ApplyGetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return Segments.GetSegment(rowIndex, columnIndex).Mean[bandIndex];
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ApplyGetFloatValues(Int32 rowIndex, Int32 columnIndex)
        {
            return Segments.GetSegment(rowIndex, columnIndex).Mean.ToArray();
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
            return (UInt32)Segments.GetSegment(rowIndex, columnIndex).Mean[bandIndex];
        }

        /// <summary>
        /// Returns all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the values.</param>
        /// <param name="columnIndex">The zero-based column index of the values.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] ApplyGetValues(Int32 rowIndex, Int32 columnIndex)
        {
            return Segments.GetSegment(rowIndex, columnIndex).Mean.Select(value => (UInt32)value).ToArray();
        }

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">The raster is read-only.</exception>
        protected override void ApplySetFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
        {
            throw new NotSupportedException("The raster is read-only.");
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.NotSupportedException">The raster is read-only.</exception>
        protected override void ApplySetFloatValues(Int32 rowIndex, Int32 columnIndex, Double[] spectralValues)
        {
            throw new NotSupportedException("The raster is read-only.");
        }

        /// <summary>
        /// Sets the spectral value at a specified index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <param name="spectralValue">The spectral value.</param>
        /// <exception cref="System.NotSupportedException">The raster is read-only.</exception>
        protected override void ApplySetValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
        {
            throw new NotSupportedException("The raster is read-only.");
        }

        /// <summary>
        /// Sets all spectral values at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="spectralValues">The array containing the spectral values for each band.</param>
        /// <exception cref="System.NotSupportedException">The raster is read-only.</exception>
        protected override void ApplySetValues(Int32 rowIndex, Int32 columnIndex, UInt32[] spectralValues)
        {
            throw new NotSupportedException("The raster is read-only.");
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns the raster factory of the collection.
        /// </summary>
        /// <param name="segments">The segments.</param>
        /// <returns>The raster factory of the collection.</returns>
        /// <exception cref="System.ArgumentNullException">The segment collection is null.</exception>
        /// <exception cref="System.ArgumentException">The segment collection has no raster.</exception>
        private IRasterFactory GetFactory(SegmentCollection segments)
        {
            if (segments == null)
                throw new ArgumentNullException(nameof(segments), "The segment collection is null.");
            if (segments.Raster == null)
                throw new ArgumentException("The segment collection has no raster.", nameof(segments));

            return segments.Raster.Factory;
        }

        #endregion
    }
}
