/// <copyright file="SpectralResamplingStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using System;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Resampling.Strategy
{
    /// <summary>
    /// Represents a spectral resampling strategy.
    /// </summary>
    public abstract class SpectralResamplingStrategy
    {
        #region Protected fields

        /// <summary>
        /// The raster image.
        /// </summary>
        protected IRaster _raster;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralResamplingStrategy"/> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        protected SpectralResamplingStrategy(IRaster raster)
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
        public abstract UInt32 Compute(Double rowIndex, Double columnIndex, Int32 bandIndex);

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        public abstract UInt32[] Compute(Double rowIndex, Double columnIndex);

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
