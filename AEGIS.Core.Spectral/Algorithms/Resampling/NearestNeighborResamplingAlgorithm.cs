/// <copyright file="NearestNeighbourResamplingAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Algorithms.Resampling
{
    /// <summary>
    /// Represents a type performing resampling of raster images using nearest neighbor interpolation.
    /// </summary>
    public class NearestNeighborResamplingAlgorithm : RasterResamplingAlgorithm
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NearestNeighborResamplingAlgorithm" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public NearestNeighborResamplingAlgorithm(IRaster raster)
            : base(raster)
        { }

        #endregion

        #region Public RasterResamplingAlgorithm methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        public override UInt32 Compute(Double rowIndex, Double columnIndex, Int32 bandIndex)
        {
            if (rowIndex < -1 || rowIndex > _raster.NumberOfRows ||
                columnIndex < -1 || columnIndex > _raster.NumberOfColumns)
                return 0;

            return _raster.GetNearestValue((Int32)Math.Floor(rowIndex + 0.5), (Int32)Math.Floor(columnIndex + 0.5), bandIndex);
        }

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public override UInt32[] Compute(Double rowIndex, Double columnIndex)
        {
            if (rowIndex < -1 || rowIndex > _raster.NumberOfRows ||
                columnIndex < -1 || columnIndex > _raster.NumberOfColumns)
                return new UInt32[_raster.NumberOfBands];

            return _raster.GetNearestValues((Int32)Math.Floor(rowIndex + 0.5), (Int32)Math.Floor(columnIndex + 0.5));
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        public override Double ComputeFloat(Double rowIndex, Double columnIndex, Int32 bandIndex)
        {
            if (rowIndex < -1 || rowIndex > _raster.NumberOfRows ||
                columnIndex < -1 || columnIndex > _raster.NumberOfColumns)
                return 0;

            return _raster.GetNearestFloatValue((Int32)Math.Floor(rowIndex + 0.5), (Int32)Math.Floor(columnIndex + 0.5), bandIndex);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public override Double[] ComputeFloat(Double rowIndex, Double columnIndex)
        {
            if (rowIndex < -1 || rowIndex > _raster.NumberOfRows ||
                columnIndex < -1 || columnIndex > _raster.NumberOfColumns)
                return new Double[_raster.NumberOfBands];

            return _raster.GetNearestFloatValues((Int32)Math.Floor(rowIndex + 0.5), (Int32)Math.Floor(columnIndex + 0.5));
        }

        #endregion
    }
}
