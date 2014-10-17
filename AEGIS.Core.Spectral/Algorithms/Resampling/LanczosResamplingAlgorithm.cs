/// <copyright file="LanczosResamplingAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;
using System;

namespace ELTE.AEGIS.Algorithms.Resampling
{
    /// <summary>
    /// Represents a type performing resapling of raster images using Lanczos interpolation.
    /// </summary>
    public class LanczosResamplingAlgorithm : RasterResamplingAlgorithm
    {
        #region Public properties

        /// <summary>
        /// Gets the radius of values.
        /// </summary>
        /// <value>The radius of values around the coordinate. The raius is usually between 2 and 5.</value>
        public Int32 Radius { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LanczosResamplingAlgorithm" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public LanczosResamplingAlgorithm(IRaster raster)
            : this(raster, 3)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanczosResamplingAlgorithm" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="radius">The radius of the Lanczos kernel.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The radius is less than 1.</exception>
        public LanczosResamplingAlgorithm(IRaster raster, Int32 radius) : base(raster)
        {
            // source: http://pixinsight.com/doc/docs/InterpolationAlgorithms/InterpolationAlgorithms.html

            if (radius < 2)
                throw new ArgumentOutOfRangeException("radius", "The radius is less than 1.");

            Radius = radius;
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
            Double value = 0;
            Double weight = 0;

            for (Int32 i = -Radius + 1; i <= Radius; i++)
                for (Int32 j = -Radius + 1; j <= Radius; j++)
                {
                    Double lanczosValue = Lanczos(i - Calculator.Fraction(rowIndex)) * Lanczos(j - Calculator.Fraction(columnIndex));

                    value += _raster.GetBoxedFloatValue((Int32)Math.Floor(rowIndex) + i, (Int32)Math.Floor(columnIndex) + j, bandIndex) * lanczosValue;

                    weight += lanczosValue;
                }

            return value / weight;
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
            Double weight = 0;

            for (Int32 i = -Radius + 1; i <= Radius; i++)
                for (Int32 j = -Radius + 1; j <= Radius; j++)
                {
                    Double lanczosValue = Lanczos(i - Calculator.Fraction(rowIndex)) * Lanczos(j - Calculator.Fraction(columnIndex));

                    for (Int32 bandIndex = 0; bandIndex < _raster.NumberOfBands; bandIndex++)
                    {
                        values[bandIndex] += _raster.GetBoxedFloatValue((Int32)Math.Floor(rowIndex) + i, (Int32)Math.Floor(columnIndex) + j, bandIndex) * lanczosValue;
                    }

                    weight += lanczosValue;
                }

            for (Int32 bandIndex = 0; bandIndex < _raster.NumberOfBands; bandIndex++)
            {
                values[bandIndex] /= weight;
            }

            return values;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns the Lanczos kernel of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The Lanczos kernel of <paramref name="value"/>.</returns>
        private Double Lanczos(Double value)
        {
            if (value == 0)
                return 1;

            if (value < 0)
                value = -value;

            if (value > Radius)
                return 0;

            return Calculator.Sinc(value / Radius);
        }

        #endregion
    }
}
