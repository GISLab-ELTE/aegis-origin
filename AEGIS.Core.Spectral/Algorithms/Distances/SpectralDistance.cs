/// <copyright file="SpectralDistance.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections.Segmentation;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Algorithms.Distances
{
    /// <summary>
    /// Represents a type measuring distance of spectral values.
    /// </summary>
    public abstract class SpectralDistance
    {
        #region Public properties

        /// <summary>
        /// Gets the statistics required by the spectral distance.
        /// </summary>
        /// <value>The statistics required by the spectral distance.</value>
        public abstract SpectralStatistics Statistics { get; }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the spectral distance between two segments.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="otherSegment">The other segment.</param>
        /// <returns>The spectral distance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The segment is null.
        /// or
        /// The other segment is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bands is not same in the two segments.</exception>
        public Double Distance(Segment segment, Segment otherSegment)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (otherSegment == null)
                throw new ArgumentNullException("otherSegment", "The other segment is null.");
            if (segment.NumberOfBands != otherSegment.NumberOfBands)
                throw new ArgumentException("otherSegment", "The number of bands is not same in the two segments.");

            return ComputeDistance(segment, otherSegment);
        }

        /// <summary>
        /// Computes the spectral distance between a segment and a spectral vector.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="values">The spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The segment is null.
        /// or
        /// The spectral value vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bands is not the same in the segment and the vector.</exception>
        public Double Distance(Segment segment, IList<UInt32> values)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (values == null)
                throw new ArgumentNullException("values", "The spectral value vector is null.");
            if (segment.NumberOfBands != values.Count)
                throw new ArgumentException("values", "The number of bands is not the same in the segment and the vector.");

            return ComputeDistance(segment, values);
        }

        /// <summary>
        /// Computes the spectral distance between a segment and a spectral vector.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="values">The spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The segment is null.
        /// or
        /// The spectral value vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bands is not the same in the segment and the vector.</exception>
        public Double Distance(Segment segment, IList<Double> values)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (values == null)
                throw new ArgumentNullException("values", "The spectral value vector is null.");
            if (segment.NumberOfBands != values.Count)
                throw new ArgumentException("values", "The number of bands is not the same in the segment and the vector.");

            return ComputeDistance(segment, values);
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The spectral vector.</param>
        /// <param name="otherValues">The other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The spectral value vector is null.
        /// or
        /// The segment is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bands is not the same in the two vectors.</exception>
        public Double Distance(IList<UInt32> values, IList<UInt32> otherValues)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The spectral value vector is null.");
            if (otherValues == null)
                throw new ArgumentNullException("otherValues", "The segment is null.");
            if (values.Count != otherValues.Count)
                throw new ArgumentException("otherValues", "The number of bands is not the same in the two vectors.");

            return ComputeDistance(values, otherValues);
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The spectral vector.</param>
        /// <param name="otherValues">The other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The spectral value vector is null.
        /// or
        /// The segment is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bands is not the same in the two vectors.</exception>
        public Double Distance(IList<Double> values, IList<UInt32> otherValues)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The spectral value vector is null.");
            if (otherValues == null)
                throw new ArgumentNullException("otherValues", "The segment is null.");
            if (values.Count != otherValues.Count)
                throw new ArgumentException("otherValues", "The number of bands is not the same in the two vectors.");

            return ComputeDistance(values, otherValues);
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The spectral vector.</param>
        /// <param name="otherValues">The other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The spectral value vector is null.
        /// or
        /// The segment is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bands is not the same in the two vectors.</exception>
        public Double Distance(IList<Double> values, IList<Double> otherValues)
        {
            if (values == null)
                throw new ArgumentNullException("values", "The spectral value vector is null.");
            if (otherValues == null)
                throw new ArgumentNullException("otherValues", "The segment is null.");
            if (values.Count != otherValues.Count)
                throw new ArgumentException("otherValues", "The number of bands is not the same in the two vectors.");

            return ComputeDistance(values, otherValues);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the spectral distance between two segments.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="otherSegment">The other segment.</param>
        /// <returns>The spectral distance.</returns>
        protected virtual Double ComputeDistance(Segment segment, Segment otherSegment)
        {
            return ComputeDistance(segment.Mean, otherSegment.Mean);
        }

        /// <summary>
        /// Computes the spectral distance between a segment and a spectral vector.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="values">The values of the spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected virtual Double ComputeDistance(Segment segment, IEnumerable<UInt32> values)
        {
            return ComputeDistance(segment.Mean, values);
        }

        /// <summary>
        /// Computes the spectral distance between a segment and a spectral vector.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="values">The values of the spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected virtual Double ComputeDistance(Segment segment, IEnumerable<Double> values)
        {
            return ComputeDistance(segment.Mean, values);
        }

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected abstract Double ComputeDistance(IEnumerable<UInt32> values, IEnumerable<UInt32> otherValues);

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected abstract Double ComputeDistance(IEnumerable<Double> values, IEnumerable<UInt32> otherValues);

        /// <summary>
        /// Computes the spectral distance between spectral vectors.
        /// </summary>
        /// <param name="values">The values of the spectral vector.</param>
        /// <param name="otherValues">The values of the other spectral vector.</param>
        /// <returns>The spectral distance.</returns>
        protected abstract Double ComputeDistance(IEnumerable<Double> values, IEnumerable<Double> otherValues);

        #endregion
    }
}
