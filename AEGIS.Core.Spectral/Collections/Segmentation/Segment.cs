/// <copyright file="Segment.cs" company="Eötvös Loránd University (ELTE)">
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
        #region Protected fields

        /// <summary>
        /// The array of mean spectral values for each band.
        /// </summary>
        protected Double[] _mean;

        /// <summary>
        /// The array of square mean spectral values for each band.
        /// </summary>
        protected Double[] _meanSquare;

        /// <summary>
        /// The array of spectral value variance for each band.
        /// </summary>
        protected Double[] _variance;

        /// <summary>
        /// The matrix of comoment values between bands.
        /// </summary>
        protected Double[,] _comoment;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of bands is less than 1.</exception>
        public Segment(Int32 numberOfBands)
            : this(numberOfBands, SpectralStatistics.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="statistics">The statistics computed for the segment.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of bands is less than 1.</exception>
        public Segment(Int32 numberOfBands, SpectralStatistics statistics)
        {
            if (numberOfBands < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfBands), "The number of bands is less than 1.");

            Count = 0;

            // initialize collections
            _mean = new Double[numberOfBands];

            if (statistics.HasFlag(SpectralStatistics.Variance))
            {
                _meanSquare = new Double[numberOfBands];
                _variance = new Double[numberOfBands];
            }

            if (statistics.HasFlag(SpectralStatistics.Comoment))
                _comoment = new Double[numberOfBands, numberOfBands];

            if (statistics.HasFlag(SpectralStatistics.Covariance))
                Covariance = new Double[numberOfBands, numberOfBands];
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="source">The source segment.</param>
        /// <exception cref="System.ArgumentNullException">The source segment is null.</exception>
        public Segment(Segment source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source segment is null.");
            
            Count = source.Count;
            _mean = new Double[source._mean.Length];
            Array.Copy(source._mean, _mean, source._mean.Length);

            if (source._comoment != null)
            {
                _comoment = new Double[source.NumberOfBands, source.NumberOfBands];
                Array.Copy(source._comoment, _comoment, source._comoment.Length);
            }
            if (source._variance != null)
            {
                _meanSquare = new Double[source._meanSquare.Length];
                Array.Copy(source._meanSquare, _meanSquare, source._meanSquare.Length);

                _variance = new Double[source._variance.Length];
                Array.Copy(source._variance, _variance, source._variance.Length);
            }
            if (source.Covariance != null)
            {
                Covariance = new Double[source.NumberOfBands, source.NumberOfBands];
                Array.Copy(source.Covariance, Covariance, source.Covariance.Length);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The array of spectral values is empty.</exception>
        public Segment(UInt32[] spectralValues)
                : this(spectralValues, SpectralStatistics.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <param name="statistics">The statistics computed for the segment.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The array of spectral values is empty.</exception>
        public Segment(UInt32[] spectralValues, SpectralStatistics statistics)
                : this(spectralValues == null || spectralValues.Length == 0 ? 1 : spectralValues.Length, statistics)
        {
            if (spectralValues == null)
                throw new ArgumentNullException(nameof(spectralValues), "The array of spectral values is null.");
            if (spectralValues.Length == 0)
                throw new ArgumentException("The array of spectral values is empty.", "spectralValues");

            AddValues(spectralValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The array of spectral values is empty.</exception>
        public Segment(Double[] spectralValues)
            : this(spectralValues, SpectralStatistics.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="spectralValues">The array of spectral values.</param>
        /// <param name="statistics">The statistics computed for the segment.</param>
        /// <exception cref="System.ArgumentNullException">The array of spectral values is null.</exception>
        /// <exception cref="System.ArgumentException">The array of spectral values is empty.</exception>
        public Segment(Double[] spectralValues, SpectralStatistics statistics)
            : this(spectralValues == null || spectralValues.Length == 0 ? 1 : spectralValues.Length, statistics)
        {
            if (spectralValues == null)
                throw new ArgumentNullException(nameof(spectralValues), "The array of spectral values is null.");
            if (spectralValues.Length == 0)
                throw new ArgumentException("The array of spectral values is empty.", "spectralValues");
            
            AddFloatValues(spectralValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment" /> class.
        /// </summary>
        /// <param name="raster">The raster from which the segment is derived.</param>
        /// <param name="statistics">The statistics computed for the segment.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public Segment(IRaster raster, SpectralStatistics statistics)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");

            Count = raster.NumberOfRows * raster. NumberOfColumns;

            // mean computation
            _mean = new Double[raster.NumberOfBands];

            for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                {
                    for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                        _mean[bandIndex] += raster.GetFloatValue(rowIndex, columnIndex, bandIndex);
                }
            }

            for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
            {
                _mean[bandIndex] /= (raster.NumberOfColumns * raster.NumberOfRows);
            }
            
            // variance computation
            if (statistics.HasFlag(SpectralStatistics.Variance) && raster.NumberOfColumns * raster.NumberOfRows > 1)
            {
                _variance = new Double[raster.NumberOfBands];
                for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                    {
                        for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                           _variance[bandIndex] += Math.Pow(_mean[bandIndex] - raster.GetFloatValue(rowIndex, columnIndex, bandIndex), 2);
                    }
                }

                for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                {
                    _variance[bandIndex] /= (raster.NumberOfColumns * raster.NumberOfRows - 1);
                }
            }
            
            // comoment computation
            if (statistics.HasFlag(SpectralStatistics.Comoment))
            {
                _comoment = new Double[raster.NumberOfBands, raster.NumberOfBands];
                for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                    {
                        for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                            for (Int32 otherBandIndex = 0; otherBandIndex < raster.NumberOfBands; otherBandIndex++)
                                _comoment[bandIndex, otherBandIndex] += (raster.GetFloatValue(rowIndex, columnIndex, bandIndex) - _mean[bandIndex]) * (raster.GetFloatValue(rowIndex, columnIndex, otherBandIndex) - _mean[otherBandIndex]);
                    }
                }
            }

            // covariance computation
            if (statistics.HasFlag(SpectralStatistics.Covariance) && raster.NumberOfColumns * raster.NumberOfRows > 1)
            {
                Covariance = new Double[raster.NumberOfBands, raster.NumberOfBands];
                for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                    {
                        for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                            for (Int32 otherBandIndex = 0; otherBandIndex < raster.NumberOfBands; otherBandIndex++)
                                Covariance[bandIndex, otherBandIndex] += (_mean[bandIndex] - raster.GetFloatValue(rowIndex, columnIndex, bandIndex)) * (_mean[otherBandIndex] - raster.GetFloatValue(rowIndex, columnIndex, otherBandIndex));
                    }
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the number of spectral vectors within the set.
        /// </summary>
        /// <value>The number of spectral vectors within the set.</value>
        public Int32 Count { get; protected set; }

        /// <summary>
        /// Gets the number of bands.
        /// </summary>
        /// <value>The number of bands.</value>
        public Int32 NumberOfBands { get { return _mean.Length; } }

        /// <summary>
        /// Gets the mean of spectral values for each band.
        /// </summary>
        /// <value>The read-only list containing the mean of spectral values for each band.</value>
        public IReadOnlyList<Double> Mean { get { return _mean; } }

        /// <summary>
        /// Gets the estimated variance of spectral values for each band.
        /// </summary>
        /// <value>The read-only list containing the estimated variance of spectral values for each band.</value>
        public IReadOnlyList<Double> Variance { get { return _variance; } }

        /// <summary>
        /// Gets the covariance of spectral values between the bands.
        /// </summary>
        /// <value>The covariance of spectral values between the bands.</value>
        public Double[,] Covariance { get; protected set; }

        /// <summary>
        /// Gets or sets the Morton code of the segment.
        /// </summary>
        /// <value>The Morton code of the segment.</value>
        public Double MortonCode { get; set; }

        /// <summary>
        /// Gets the statistics computed for the segment.
        /// </summary>
        /// <value>The statistics computed for the segment.</value>
        public SpectralStatistics Statistics
        {
            get
            {
                SpectralStatistics statistics = SpectralStatistics.None;

                if (_variance != null)
                    statistics |= SpectralStatistics.Variance;
                if (Covariance != null)
                    statistics |= SpectralStatistics.Covariance;
                if (_comoment != null)
                    statistics |= SpectralStatistics.Comoment;

                return statistics;
            }
        }

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
                throw new ArgumentNullException(nameof(spectralValues), "The array of spectral values is null.");
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands.", "spectralValues");

            Count++;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];

                // incremental mean and variance computation
                _mean[bandIndex] = _mean[bandIndex] + (spectralValues[bandIndex] - _mean[bandIndex]) / Count;

                if (_variance != null && Count > 1)
                {
                    _meanSquare[bandIndex] += (spectralValues[bandIndex] - previousMean) * (spectralValues[bandIndex] - _mean[bandIndex]);
                    _variance[bandIndex] = _meanSquare[bandIndex] / (Count - 1);
                }

                // use comoment for computing covariance
                if (_comoment != null)
                {
                    for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                    {
                        _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] + (spectralValues[bandIndex] - _mean[bandIndex]) * (spectralValues[otherBandIndex] - _mean[otherBandIndex]);

                        if (Covariance != null)
                        {
                            Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                            Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                        }
                    }
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
                throw new ArgumentNullException(nameof(spectralValues), "The array of spectral values is null.");
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands.", "spectralValues");

            Count++;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];

                // incremental mean and variance computation
                _mean[bandIndex] = _mean[bandIndex] + (spectralValues[bandIndex] - _mean[bandIndex]) / Count;

                if (_variance != null && Count > 1)
                {
                    _meanSquare[bandIndex] += (spectralValues[bandIndex] - previousMean) * (spectralValues[bandIndex] - _mean[bandIndex]);
                    _variance[bandIndex] = _meanSquare[bandIndex] / (Count - 1);
                }

                // use comoment for computing covariance
                if (_comoment != null)
                {
                    for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                    {
                        _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] + (spectralValues[bandIndex] - _mean[bandIndex]) * (spectralValues[otherBandIndex] - _mean[otherBandIndex]);

                        if (Covariance != null)
                        {
                            Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                            Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                        }
                    }
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
                throw new ArgumentNullException(nameof(spectralValues), "The array of spectral values is null.");
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands.", "spectralValues");

            Count--;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];

                // incremental mean and variance computation
                _mean[bandIndex] = _mean[bandIndex] - (spectralValues[bandIndex] - _mean[bandIndex]) / Count;

                if (_variance != null && Count > 1)
                {
                    _meanSquare[bandIndex] -= (spectralValues[bandIndex] - previousMean) * (spectralValues[bandIndex] - _mean[bandIndex]);
                    _variance[bandIndex] = _meanSquare[bandIndex] / (Count - 1);
                }

                // use comoment for computing covariance
                if (_comoment != null)
                {
                    for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                    {
                        _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] - (spectralValues[bandIndex] - _mean[bandIndex]) * (spectralValues[otherBandIndex] - _mean[otherBandIndex]);

                        if (Covariance != null)
                        {
                            Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                            Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                        }
                    }
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
                throw new ArgumentNullException(nameof(spectralValues), "The array of spectral values is null.");
            if (spectralValues.Length != NumberOfBands)
                throw new ArgumentException("The number of spectral values does not match the number of bands.", "spectralValues");

            Count--;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];

                // incremental mean and variance computation
                _mean[bandIndex] = _mean[bandIndex] - (spectralValues[bandIndex] - _mean[bandIndex]) / Count;

                if (_variance != null && Count > 1)
                {
                    _meanSquare[bandIndex] -= (spectralValues[bandIndex] - previousMean) * (spectralValues[bandIndex] - _mean[bandIndex]);
                    _variance[bandIndex] = _meanSquare[bandIndex] / (Count - 1);
                }

                // use comoment for computing covariance
                if (_comoment != null)
                {
                    for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                    {
                        _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] - (spectralValues[bandIndex] - _mean[bandIndex]) * (spectralValues[otherBandIndex] - _mean[otherBandIndex]);

                        if (Covariance != null)
                        {
                            Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                            Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                        }
                    }
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

            if (_variance == null)
                return false;

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                if (_variance[bandIndex] / _meanSquare[bandIndex] > homogeneityThreshold)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Merges another segment into the current instance.
        /// </summary>
        /// <param name="other">The other segment.</param>
        /// <exception cref="System.ArgumentNullException">The other segment is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of bands in the other set does not match the current segment.
        /// or
        /// The statistics of the other segment does not match the current segment.
        /// </exception>
        public void Merge(Segment other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other segment is null.");
            if (NumberOfBands != other.NumberOfBands)
                throw new ArgumentException("The number of bands in the other segment does not match the current segment.", "other");
            if (Statistics != other.Statistics)
                throw new ArgumentException("The statistics of the other segment does not match the current segment.", "other");
            
            if (this == other)
                return;
            
            Int32 previousCount = Count;
            Count = Count + other.Count;
            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];
                Double meanDelta = other._mean[bandIndex] - _mean[bandIndex];
                _mean[bandIndex] = previousMean + meanDelta * other.Count / Count;

                if (_variance != null)
                {
                    if (Count == 2)
                        _variance[bandIndex] = Math.Sqrt(Calculator.Pow(_mean[bandIndex] - previousMean, 2) + Calculator.Pow(_mean[bandIndex] - other._mean[bandIndex], 2));
                    else
                    {
                        _variance[bandIndex] = (previousCount - 1) * _variance[bandIndex] + (other.Count - 1) * other._variance[bandIndex];
                        _variance[bandIndex] += Calculator.Pow(previousMean - other._mean[bandIndex], 2) * (previousCount - 1) * (other.Count - 1) / (Count - 2);
                        _variance[bandIndex] /= Count;
                    }
                }

                for (Int32 otherBandIndex = bandIndex + 1; otherBandIndex < NumberOfBands; otherBandIndex++)
                {
                    Double bandMean = _mean[bandIndex] - other._mean[bandIndex];
                    Double otherBandMean = _mean[otherBandIndex] - other._mean[otherBandIndex];

                    if (_comoment != null)
                    {
                        _comoment[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] + other._comoment[bandIndex, otherBandIndex];
                        _comoment[bandIndex, otherBandIndex] += bandMean * otherBandMean * previousCount * other.Count / Count;

                        if (Covariance != null)
                        {
                            Covariance[bandIndex, otherBandIndex] = _comoment[bandIndex, otherBandIndex] / Count;
                            Covariance[otherBandIndex, bandIndex] = Covariance[bandIndex, otherBandIndex];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the merge of two segments is homogeneous.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <param name="homogeneityThreshold">The homogeneity threshold.</param>
        /// <returns><c>true</c> if the result of merging the two segments is homogeneous; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The other segment is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of bands in the other set does not match the current segment.
        /// or
        /// The statistics of the other segment does not match the current segment.
        /// </exception>
        public Boolean IsMergeHomogenous(Segment other, Double homogeneityThreshold)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other segment is null.");
            if (NumberOfBands != other.NumberOfBands)
                throw new ArgumentException("The number of bands in the other segment does not match the current segment.", "other");
            if (Statistics != other.Statistics)
                throw new ArgumentException("The statistics of the other segment does not match the current segment.", "other");

            if (this == other)
                return false;

            if (_variance == null)
                return false;

            Int32 previousCount = Count;
            Int32 count = Count + other.Count;
            Double[] mean = new Double[NumberOfBands];
            Double[] variance = new Double[NumberOfBands];

            for (Int32 bandIndex = 0; bandIndex < NumberOfBands; bandIndex++)
            {
                Double previousMean = _mean[bandIndex];
                Double meanDelta = other._mean[bandIndex] - _mean[bandIndex];
                mean[bandIndex] = previousMean + meanDelta * other.Count / Count;

                if (_variance != null)
                {
                    Double meanSquare = _meanSquare[bandIndex] + other._meanSquare[bandIndex] + meanDelta * meanDelta * previousCount * other.Count / Count;
                    variance[bandIndex] = _meanSquare[bandIndex] / (Count - 1);
                }
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
    }
}
