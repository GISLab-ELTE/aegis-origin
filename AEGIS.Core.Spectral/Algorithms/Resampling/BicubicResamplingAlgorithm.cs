/// <copyright file="BicubicInterpolationAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
/// <author>Gréta Bereczki</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.Algorithms.Resampling
{
    /// <summary>
    /// Represents an extension of cubic interpolation for interpolating data points on a two dimensional regular grid.
    /// </summary>
    public class BicubicResamplingAlgorithm : RasterResamplingAlgorithm
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BicubicResamplingAlgorithm" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public BicubicResamplingAlgorithm(IRaster raster)
            : base(raster)
        {
        }

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
            if (rowIndex < -2 || rowIndex > _raster.NumberOfRows + 2 ||
                columnIndex < -2 || columnIndex > _raster.NumberOfColumns)
                return 0;

            Int32 columnFloor = (Int32)Math.Floor(columnIndex);
            Int32 rowFloor = (Int32)Math.Floor(rowIndex);

            Double[][] values = new Double[4][];
            for (Int32 row = 0; row < 4; row++)
            {
                values[row] = new Double[4];
                for (Int32 column = 0; column < 4; column++)
                {
                    values[row][column] = _raster.GetBoxedFloatValue(rowFloor + row - 1, columnFloor + column - 1, bandIndex);
                }
            }

            Double[] intermediateResult = new Double[4]; 
            intermediateResult[0] = ComputeInterpolation(values[0], rowIndex - rowFloor);
            intermediateResult[1] = ComputeInterpolation(values[1], rowIndex - rowFloor);
            intermediateResult[2] = ComputeInterpolation(values[2], rowIndex - rowFloor);
            intermediateResult[3] = ComputeInterpolation(values[3], rowIndex - rowFloor);

            return ComputeInterpolation(intermediateResult, columnIndex - columnFloor);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public override Double[] ComputeFloat(Double rowIndex, Double columnIndex)
        {
            Double[] values = new Double[_raster.NumberOfBands];
            
            for (Int32 bandIndex = 0; bandIndex < _raster.NumberOfBands; bandIndex++)
                values[bandIndex] = ComputeFloat(rowIndex, columnIndex, bandIndex);

            return values;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the cubic interpolation.
        /// </summary>
        /// <param name="values">The array of the values.</param>
        /// <param name="fraction">The fraction.</param>
        /// <returns>The cubic interpolation of the values.</returns>
        private Double ComputeInterpolation(Double[] values, Double fraction)
        {
            return values[1] + fraction * (values[2] - values[0] + fraction * (2.0 * values[0] - 2.0 * values[1] + values[2] - values[3] + fraction * (values[1] - values[2] + values[3] - values[0])));
        }

        #endregion
    }
}
