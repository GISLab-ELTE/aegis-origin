/// <copyright file="ManhattanDistance.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Algorithms.Distances
{
    /// <summary>
    /// Represents a type measuring Manhattan (City block) distance of spectral values.
    /// </summary>
    public class ManhattanDistance : SpectralDistance
    {
        #region Public properties

        /// <summary>
        /// Gets the statistics required by the spectral distance.
        /// </summary>
        /// <value>The statistics required by the spectral distance.</value>
        public override SpectralStatistics Statistics { get { return SpectralStatistics.None; } }

        #endregion

        #region Proctected SpectralDistance methods

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IEnumerable<UInt32> values, IEnumerable<UInt32> otherValues)
        {
            Double distance = 0.0;

            IEnumerator<UInt32> enumerator = values.GetEnumerator();
            IEnumerator<UInt32> otherEnumerator = otherValues.GetEnumerator();

            while (enumerator.MoveNext() && otherEnumerator.MoveNext())
            {
                distance += Math.Abs(enumerator.Current - otherEnumerator.Current);
            }

            return distance;
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IEnumerable<Double> values, IEnumerable<UInt32> otherValues)
        {
            Double distance = 0.0;

            IEnumerator<Double> enumerator = values.GetEnumerator();
            IEnumerator<UInt32> otherEnumerator = otherValues.GetEnumerator();

            while (enumerator.MoveNext() && otherEnumerator.MoveNext())
            {
                distance += Math.Abs(enumerator.Current - otherEnumerator.Current);
            }

            return distance;
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IEnumerable<Double> values, IEnumerable<Double> otherValues)
        {
            Double distance = 0.0;

            IEnumerator<Double> enumerator = values.GetEnumerator();
            IEnumerator<Double> otherEnumerator = otherValues.GetEnumerator();

            while (enumerator.MoveNext() && otherEnumerator.MoveNext())
            {
                distance += Math.Abs(enumerator.Current - otherEnumerator.Current);
            }

            return distance;
        }

        #endregion
    }
}
