/// <copyright file="BilinearResamplingAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using System;

namespace ELTE.AEGIS.Algorithms.Resampling
{
    /// <summary>
    /// Represents a type performing resampling of raster images using bilinear interpolation.
    /// </summary>
    public class BilinearResamplingAlgorithm : RasterResamplingAlgorithm
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BilinearResamplingAlgorithm" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public BilinearResamplingAlgorithm(IRaster raster)
            : base(raster)
        { }

        #endregion

        #region Public RasterResamplingAlgorithm methods

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        public override Double ComputeFloat(Double rowIndex, Double columnIndex, Int32 bandIndex)
        {
            Int32 columnFloor = (Int32)Math.Floor(columnIndex);
            Int32 rowFloor = (Int32)Math.Floor(rowIndex);

            Double firstColumnValue = (1 - rowIndex + rowFloor) * _raster.GetBoxedFloatValue(rowFloor, columnFloor, bandIndex) + (rowIndex - rowFloor) * _raster.GetBoxedFloatValue(rowFloor + 1, columnFloor, bandIndex);
            Double secondColumnValue = (1 - rowIndex + rowFloor) * _raster.GetBoxedFloatValue(rowFloor, columnFloor + 1, bandIndex) + (rowIndex - rowFloor) * _raster.GetBoxedFloatValue(rowFloor + 1, columnFloor + 1, bandIndex);

            return (1 - columnIndex + columnFloor) * firstColumnValue + (columnIndex - columnFloor) * secondColumnValue;
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public override Double[] ComputeFloat(Double rowIndex, Double columnIndex)
        {
            Int32 columnFloor = (Int32)Math.Floor(columnIndex);
            Int32 rowFloor = (Int32)Math.Floor(rowIndex);

            Double firstColumnValue, secondColumnValue;

            Double[] result = new Double[_raster.NumberOfBands];
            for (Int32 bandIndex = 0; bandIndex < _raster.NumberOfBands; bandIndex++)
            {
                firstColumnValue = (1 - rowIndex + rowFloor) * _raster.GetBoxedFloatValue(rowFloor, columnFloor, bandIndex) + (rowIndex - rowFloor) * _raster.GetBoxedFloatValue(rowFloor + 1, columnFloor, bandIndex);
                secondColumnValue = (1 - rowIndex + rowFloor) * _raster.GetBoxedFloatValue(rowFloor, columnFloor + 1, bandIndex) + (rowIndex - rowFloor) * _raster.GetBoxedFloatValue(rowFloor + 1, columnFloor + 1, bandIndex);

                result[bandIndex] = (1 - columnIndex + columnFloor) * firstColumnValue + (columnIndex - columnFloor) * secondColumnValue;
            }

            return result;
        }

        #endregion
    }
}
