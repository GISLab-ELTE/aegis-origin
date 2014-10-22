/// <copyright file="GaussianRandomGenerator.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>István Tiboldi</author>

using System;

namespace ELTE.AEGIS.Numerics.Statistics
{
    /// <summary>
    /// Represents a random number generator using a Gaussian distribution.
    /// </summary>
    public class GaussianRandomGenerator
    {
        #region Private fields

        /// <summary>
        /// A value indicating whether the next number is available.
        /// </summary>
        private Boolean _available;

        /// <summary>
        /// The next generated number.
        /// </summary>
        private Double _nextGauss;

        /// <summary>
        /// The random generator.
        /// </summary>
        private Random _generator;

        /// <summary>
        /// The number of generated values.
        /// </summary>
        private Int32 _counter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianRandomGenerator" /> class.
        /// </summary>
        public GaussianRandomGenerator()
        {
            _generator = new Random();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>The generated number.</returns>
        public Double NextDouble()
        {
            _counter++;
            if (_available)
            {
                _available = false;
                return _nextGauss;
            }

            Double u1 = _generator.NextDouble();
            Double u2 = _generator.NextDouble();
            Double temp1 = Math.Sqrt(-2.0 * Math.Log(u1));
            Double temp2 = 2.0 * Math.PI * u2;

            _nextGauss = temp1 * Math.Sin(temp2);
            _available = true;
            return temp1 * Math.Cos(temp2);
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <param name="median">The median.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        /// <returns>The generated number.</returns>
        public Double NextDouble(Double median, Double standardDeviation)
        {
            return median + standardDeviation * NextDouble();
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <param name="standardDeviation">The standard deviation.</param>
        /// <returns>The generated number.</returns>
        public Double NextDouble(Double standardDeviation)
        {
            return standardDeviation * NextDouble();
        }

        #endregion
    }
}
