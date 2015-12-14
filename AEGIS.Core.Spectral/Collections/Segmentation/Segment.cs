/// <copyright file="Segment.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Collections.Segmentation
{
    /// <summary>
    /// Represents a spectral segment within a raster image.
    /// </summary>
    public class Segment
    {
        #region Private fields

        /// <summary>
        /// The array of mean spectral values for each band.
        /// </summary>
        private Double[] _mean;

        /// <summary>
        /// The array of spectral value variance for each band.
        /// </summary>
        private Double[] _variance;

        /// <summary>
        /// The matrix of comoment values between bands.
        /// </summary>
        private Double[,] _comoment;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of bands is less than 1.</exception>
        public Segment(Int32 numberOfBands)
        {
            if (numberOfBands < 1)
                throw new ArgumentOutOfRangeException("numberOfBands", "The number of bands is less than 1.");

            Count = 0;

            // initialize collections
            _mean = new Double[numberOfBands];
            _variance = new Double[numberOfBands];
            _comoment = new Double[numberOfBands, numberOfBands];
            Covariance = new Double[numberOfBands, numberOfBands];

            // initialize read-only wrapper collections
            Mean = _mean.AsReadOnly();
            Variance = _variance.AsReadOnly();
            MortonCode = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The array of spectral values is empty.</exception>
        public Segment(UInt32[] spectralValues)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The array of spectral values is null.");
            if (spectralValues.Length == 0)
                throw new ArgumentException("The array of spectral values is empty.", "spectralValues");

            Count = 0;

            // initialize collections
            _mean = new Double[spectralValues.Length];
            _variance = new Double[spectralValues.Length];
            _comoment = new Double[spectralValues.Length, spectralValues.Length];
            Covariance = new Double[spectralValues.Length, spectralValues.Length];

            // initialize read-only wrapper collections
            Mean = _mean.AsReadOnly();
            Variance = _variance.AsReadOnly();

            AddValues(spectralValues);
            MortonCode = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The array of spectral values is empty.</exception>
        public Segment(Double[] spectralValues)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The array of spectral values is null.");
            if (spectralValues.Length == 0)
                throw new ArgumentException("The array of spectral values is empty.", "spectralValues");

            Count = 0;

            // initialize collections
            _mean = new Double[spectralValues.Length];
            _variance = new Double[spectralValues.Length];
            _comoment = new Double[spectralValues.Length, spectralValues.Length];
            Covariance = new Double[spectralValues.Length, spectralValues.Length];

            // initialize read-only wrapper collections
            Mean = _mean.AsReadOnly();
            Variance = _variance.AsReadOnly();

            AddFloatValues(spectralValues);
            MortonCode = 0.0;
        }

        #endregion

        #region Public properties

        public Double MortonCode { get; set; }

        /// <summary>
        /// Gets the number of spectral vectors within the set.
        /// </summary>
        /// <value>The number of spectral vectors within the set.</value>
        public Int32 Count { get; private set; }

        /// <summary>
        /// Gets the number of bands.
        /// </summary>
        /// <value>The number of bands.</value>
        public Int32 NumberOfBands { get { return _mean.Length; } }

        /// <summary>
        /// Gets the mean of spectral values for each band.
        /// </summary>
        /// <value>The read-only list containing the mean of spectral values for each band.</value>
        public IList<Double> Mean { get; private set; }

        /// <summary>
        /// Gets the estimated variance of spectral values for each band.
        /// </summary>
        /// <value>The read-only list containing the estimated variance of spectral values for each band.</value>
        public IList<Double> Variance { get; private set; }

        /// <summary>
        /// Gets the covariance of spectral values between the bands.
        /// </summary>
        /// <value>The covariance of spectral values between the bands.</value>
        public Double[,] Covariance { get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified spectral values to the segment.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands.</exception>
        public void AddValues(UInt32[] spectralValues)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The array of spectral values is null.");
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands.", "spectralValues");

            Count++;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];

                // incremental mean and variance computation
                _mean[bandIndex] = _mean[bandIndex] + (spectralValues[bandIndex] - _mean[bandIndex]) / Count;

                if (Count > 1)
                    _variance[bandIndex] = (Count - 2) * _variance[bandIndex] * (Count - 2) / (Count - 1) + (spectralValues[bandIndex] - previousMean) * (spectralValues[bandIndex] - previousMean) / Count;

                // use comoment for computing covariance
                for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                {
                    _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] + (spectralValues[bandIndex] - _mean[bandIndex]) * (spectralValues[otherBandIndex] - _mean[otherBandIndex]);
                    Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                    Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                }
            }
        }

        /// <summary>
        /// Adds the specified spectral values to the segment.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands.</exception>
        public void AddFloatValues(Double[] spectralValues)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The array of spectral values is null.");
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands.", "spectralValues");

            Count++;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];

                // incremental mean and variance computation
                _mean[bandIndex] = _mean[bandIndex] + (spectralValues[bandIndex] - _mean[bandIndex]) / Count;

                if (Count > 1)
                    _variance[bandIndex] = (Count - 2) * _variance[bandIndex] * (Count - 2) / (Count - 1) + (spectralValues[bandIndex] - previousMean) * (spectralValues[bandIndex] - previousMean) / Count;

                // use comoment for computing covariance
                for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                {
                    _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] + (spectralValues[bandIndex] - _mean[bandIndex]) * (spectralValues[otherBandIndex] - _mean[otherBandIndex]);
                    Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                    Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                }
            }
        }

        /// <summary>
        /// Removes the spectral values located at the specified index to the segment.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands.</exception>
        public void RemoveValues(UInt32[] spectralValues)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The array of spectral values is null.");
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands.", "spectralValues");

            Count--;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];

                // incremental mean and variance computation
                _mean[bandIndex] = _mean[bandIndex] - (spectralValues[bandIndex] - _mean[bandIndex]) / Count;
                _variance[bandIndex] = ((Count + 1) * _variance[bandIndex] - (spectralValues[bandIndex] - previousMean) * (spectralValues[bandIndex] - _mean[bandIndex])) / Count;

                // use comoment for computing covariance
                for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                {
                    _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] - (spectralValues[bandIndex] - _mean[bandIndex]) * (spectralValues[otherBandIndex] - _mean[otherBandIndex]);
                    Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                    Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                }
            }
        }

        /// <summary>
        /// Removes the spectral values located at the specified index to the segment.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The number of spectral values does not match the number of bands.</exception>
        public void RemoveFloatValues(Double[] spectralValues)
        {
            if (spectralValues == null)
                throw new ArgumentNullException("spectralValues", "The array of spectral values is null.");
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands.", "spectralValues");

            Count--;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];

                // incremental mean and variance computation
                _mean[bandIndex] = _mean[bandIndex] - (spectralValues[bandIndex] - _mean[bandIndex]) / Count;
                _variance[bandIndex] = ((Count + 1) * _variance[bandIndex] - (spectralValues[bandIndex] - previousMean) * (spectralValues[bandIndex] - _mean[bandIndex])) / Count;

                // use comoment for computing covariance
                for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                {
                    _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] - (spectralValues[bandIndex] - _mean[bandIndex]) * (spectralValues[otherBandIndex] - _mean[otherBandIndex]);
                    Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                    Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                }
            }
        }

        /// <summary>
        /// Returns a value indicating whether the segment is homogeneous.
        /// </summary>
        /// <param name="homogeneityThreshold">The upper threshold for homogeneity.</param>
        /// <returns><c>true</c> if the segment is homogeneous; otherwise <c>false</c>.</returns>
        public Boolean IsHomogeneous(Double homogeneityThreshold)
        {
            if (Count <= 1)
                return true;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                if (_variance[bandIndex] / (_mean[bandIndex] * _mean[bandIndex]) > homogeneityThreshold)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Merges another segment into the current instance.
        /// </summary>
        /// <param name="other">The other segment.</param>
        /// <exception cref="System.ArgumentNullException">The other segment is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bands in the other set does not match the current segment.</exception>
        public void Merge(Segment other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other segment is null.");
            if (NumberOfBands != other.NumberOfBands)
                throw new ArgumentException("The number of bands in the other segment does not match the current segment.", "other");

            if (this == other)
                return;


            Int32 previousCount = Count;
            Count = Count + other.Count;
            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];
                _mean[bandIndex] = (previousCount * previousMean + other.Count * other._mean[bandIndex]) / Count;
                _variance[bandIndex] = (previousCount - 1) * _variance[bandIndex] + (other.Count - 1) * other._variance[bandIndex];
                _variance[bandIndex] += Calculator.Pow(previousMean - other._mean[bandIndex], 2) * (previousCount - 1) * (other.Count - 1) / (Count - 2);
                _variance[bandIndex] /= Count;

                for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                {
                    Double bandMean = _mean[bandIndex] - other._mean[bandIndex];
                    Double otherBandMean = _mean[otherBandIndex] - other._mean[otherBandIndex];

                    _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] + other._comoment[bandIndex, otherBandIndex];
                    _comoment[bandIndex, otherBandIndex] += bandMean * otherBandMean * previousCount * other.Count / Count;

                    Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                    Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                }
            }
        }

        /// <summary>
        /// Determines whether the merge of two segments is homogeneous.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <param name="homogeneityThreshold">The homogeneity threshold.</param>
        /// <returns><c>true</c> if the result of merging the two segments is homogeneous; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other segment is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bands in the other segment does not match the current segment.;other</exception>
        public Boolean IsMergeHomogenous(Segment other, Double homogeneityThreshold)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other segment is null.");
            if (NumberOfBands != other.NumberOfBands)
                throw new ArgumentException("The number of bands in the other segment does not match the current segment.", "other");

            if (this == other)
                return false;

            Int32 previousCount = Count;
            Int32 count = Count + other.Count;
            Double[] mean = new Double[NumberOfBands];
            Double[] variance = new Double[NumberOfBands];
            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];
                mean[bandIndex] = (previousCount * previousMean + other.Count * other._mean[bandIndex]) / count;
                variance[bandIndex] = (previousCount - 1) * _variance[bandIndex] + (other.Count - 1) * other._variance[bandIndex];
                variance[bandIndex] += Calculator.Pow(previousMean - other._mean[bandIndex], 2) * (previousCount - 1) * (other.Count - 1) / (count - 2);
                variance[bandIndex] /= count;
            }

            if (count <= 1)
                return true;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                if (Double.IsNaN(variance[bandIndex]) || variance[bandIndex] / (mean[bandIndex] * mean[bandIndex]) > homogeneityThreshold)
                    return false;
            }

            return true;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Sums the specified segments.
        /// </summary>
        /// <param name="first">The first segment.</param>
        /// <param name="second">The second segment.</param>
        /// <returns>The summed segment of <paramref name="first"/> and <paramref name="second"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first segment is null.
        /// or
        /// The second segment is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bands of the two segments does not match.</exception>
        public static Segment operator +(Segment first, Segment second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first segment is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second segment is null.");

            if (first.NumberOfBands != second.NumberOfBands)
                throw new ArgumentException("The number of bands of the two segments does not match.", "second");

            Segment result = new Segment(first.NumberOfBands);

            // based on the sum of squares for the data

            result.Count = first.Count + second.Count;
            for (Int32 bandIndex = 0; bandIndex < result.NumberOfBands; bandIndex++)
            {
                result._mean[bandIndex] = (first.Count * first._mean[bandIndex] + second.Count * second._mean[bandIndex]) / result.Count;

                Double firstValue = first.Count * (first._variance[bandIndex] + first._mean[bandIndex] * first._mean[bandIndex]);
                Double secondValue = second.Count * (second._variance[bandIndex] + second._mean[bandIndex] * second._mean[bandIndex]);

                result._variance[bandIndex] = (firstValue + secondValue) / result.Count - result._mean[bandIndex] * result._mean[bandIndex];

                for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < result.NumberOfBands; otherBandIndex++)
                {
                    Double bandMean = first._mean[bandIndex] - second._mean[bandIndex];
                    Double otherBandMean = first._mean[otherBandIndex] - second._mean[otherBandIndex];

                    result._comoment[bandIndex, otherBandIndex] = first._comoment[bandIndex, otherBandIndex] + second._comoment[bandIndex, otherBandIndex];
                    result._comoment[bandIndex, otherBandIndex] += bandMean * otherBandMean * first.Count * second.Count / result.Count;

                    result.Covariance[bandIndex, otherBandIndex] = result._comoment[bandIndex, otherBandIndex] / result.Count;
                    result.Covariance[otherBandIndex, bandIndex] = result.Covariance[bandIndex, otherBandIndex];
                }
            }

            return result;
        }

        #endregion
    }
}
