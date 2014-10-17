/// <copyright file="NearestNeighbourResamplingAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Algorithms.Resampling
{
    /// <summary>
    /// Represents a type performing resapling of raster images using nearest neighbour interpolation.
    /// </summary>
    public class NearestNeighbourResamplingAlgorithm : RasterResamplingAlgorithm
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NearestNeighbourResamplingAlgorithm" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public NearestNeighbourResamplingAlgorithm(IRaster raster)
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
            return _raster.GetNearestValue(Convert.ToInt32(Math.Round(rowIndex)), Convert.ToInt32(Math.Round(columnIndex)), bandIndex);
        }

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public override UInt32[] Compute(Double rowIndex, Double columnIndex)
        {
            return _raster.GetNearestValues(Convert.ToInt32(Math.Round(rowIndex)), Convert.ToInt32(Math.Round(columnIndex)));
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
            return _raster.GetNearestFloatValue(Convert.ToInt32(Math.Round(rowIndex)), Convert.ToInt32(Math.Round(columnIndex)), bandIndex);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public override Double[] ComputeFloat(Double rowIndex, Double columnIndex)
        {
            return _raster.GetNearestFloatValues(Convert.ToInt32(Math.Round(rowIndex)), Convert.ToInt32(Math.Round(columnIndex)));
        }

        #endregion
    }
}
