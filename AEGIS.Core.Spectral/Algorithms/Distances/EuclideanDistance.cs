/// <copyright file="EuclideanDistance.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Algorithms.Distances
{
    /// <summary>
    /// Represents a type measuring Euclidean distance of spectral values.
    /// </summary>
    public class EuclideanDistance : SpectralDistance
    {
        #region Proctected SpectralDistance methods

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The spectral vector.</param>
        /// <param name="otherValues">The other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IList<UInt32> values, IList<UInt32> otherValues)
        {
            Double distance = 0.0;

            for (Int32 i = 0; i < values.Count; i++)
                distance += (values[i] - otherValues[i]) * (values[i] - otherValues[i]);

            return Math.Sqrt(distance);
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The spectral vector.</param>
        /// <param name="otherValues">The other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IList<Double> values, IList<UInt32> otherValues)
        {
            Double distance = 0.0;

            for (Int32 i = 0; i < values.Count; i++)
                distance += (values[i] - otherValues[i]) * (values[i] - otherValues[i]);

            return Math.Sqrt(distance);
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The spectral vector.</param>
        /// <param name="otherValues">The other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected override Double ComputeDistance(IList<Double> values, IList<Double> otherValues)
        {
            Double distance = 0.0;

            for (Int32 i = 0; i < values.Count; i++)
                distance += (values[i] - otherValues[i]) * (values[i] - otherValues[i]);

            return Math.Sqrt(distance);
        }

        #endregion
    }
}
