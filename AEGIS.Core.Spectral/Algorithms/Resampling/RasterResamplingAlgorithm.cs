// <copyright file="RasterResamplingAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Algorithms;
using System;
using System.Linq;

namespace ELTE.AEGIS.Algorithms.Resampling
{
    /// <summary>
    /// Represents a type performing resampling of raster images.
    /// </summary>
    public abstract class RasterResamplingAlgorithm
    {
        #region Protected fields

        /// <summary>
        /// The raster image.
        /// </summary>
        protected IRaster _raster;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterResamplingAlgorithm" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        protected RasterResamplingAlgorithm(IRaster raster)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");

            _raster = raster;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        public virtual UInt32 Compute(Double rowIndex, Double columnIndex, Int32 bandIndex)
        {
            return RasterAlgorithms.Restrict(ComputeFloat(rowIndex, columnIndex, bandIndex), _raster.RadiometricResolution);
        }

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public virtual UInt32[] Compute(Double rowIndex, Double columnIndex)
        {
            Double[] resultFloat = ComputeFloat(rowIndex, columnIndex);

            UInt32[] result = new UInt32[_raster.NumberOfBands];
            for (Int32 bandIndex = 0; bandIndex < _raster.NumberOfBands; bandIndex++)
            {
                result[bandIndex] = RasterAlgorithms.Restrict(resultFloat[bandIndex], _raster.RadiometricResolution);
            }
            return result;
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        public abstract Double ComputeFloat(Double rowIndex, Double columnIndex, Int32 bandIndex);

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public abstract Double[] ComputeFloat(Double rowIndex, Double columnIndex);

        #endregion
    }
}
