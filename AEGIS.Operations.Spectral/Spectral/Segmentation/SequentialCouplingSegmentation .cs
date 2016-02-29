/// <copyright file="SequentialCouplingSegmentation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    /// <summary>
    /// Represents an operation performing segmentation on spectral geometries using sequential coupling.
    /// </summary>
    [OperationMethodImplementation("AEGIS::254120", "Sequential coupling segmentation")]
    public class SequentialCouplingSegmentation : SpectralSegmentation
    {
        #region Private types

        /// <summary>
        /// Defines the possible directions for segment merging.
        /// </summary>
        private enum SegmentMergeDirection { Left, Up, Right1, Right2, None }

        #endregion

        #region Private Fields

        /// <summary>
        /// The segment homogeneity threshold.
        /// </summary>
        private Double _segmentHomogeneityThreshold;

        /// <summary>
        /// Threshold value for segment variance before merge.
        /// </summary>
        private Double _varianceThresholdBeforeMerge;

        /// <summary>
        /// Threshold value for segment variance after merge.
        /// </summary>
        private Double _varianceThresholdAfterMerge;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialCouplingSegmentation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The source geometry does not contain raster data.
        /// or
        /// The raster format of the source is not supported by the method.
        /// </exception>
        public SequentialCouplingSegmentation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialCouplingSegmentation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// or
        /// The source geometry does not contain raster data.
        /// or
        /// The raster format of the source is not supported by the method.
        /// </exception>
        public SequentialCouplingSegmentation(ISpectralGeometry source, SegmentCollection target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.SequentialCouplingSegmentation, parameters)
        {
            _segmentHomogeneityThreshold = Convert.ToDouble(parameters[SpectralOperationParameters.SegmentHomogeneityThreshold]);
            _varianceThresholdBeforeMerge = Convert.ToDouble(parameters[SpectralOperationParameters.VarianceThresholdBeforeMerge]);
            _varianceThresholdAfterMerge = Convert.ToDouble(parameters[SpectralOperationParameters.VarianceThresholdAfterMerge]);

            _varianceThresholdAfterMerge = Calculator.Pow(_varianceThresholdAfterMerge, 2); 
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            if (_result == null)
                _result = new SegmentCollection(_source.Raster, _distance.Statistics | SpectralStatistics.Variance);
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            for (Int32 rowIndex = 0; rowIndex < _source.Raster.NumberOfRows; rowIndex += 2)
            {
                // initialize the segments of the next rows with 2x2 sized segments
                for (Int32 columnIndex = 0; columnIndex < _source.Raster.NumberOfColumns; columnIndex += 2)
                {
                    if (columnIndex < _source.Raster.NumberOfColumns - 1)
                    {
                        _result.MergeSegments(rowIndex, columnIndex, rowIndex, columnIndex + 1);
                    }
                    if (rowIndex < _source.Raster.NumberOfRows - 1)
                    {
                        _result.MergeSegments(rowIndex, columnIndex, rowIndex + 1, columnIndex);
                        if (columnIndex < _source.Raster.NumberOfColumns - 1)
                            _result.MergeSegments(rowIndex, columnIndex, rowIndex + 1, columnIndex + 1);
                    }
                }

                // connect segments
                for (Int32 columnIndex = 0; columnIndex < _source.Raster.NumberOfColumns; columnIndex += 2)
                {
                    SegmentMergeDirection direction = SegmentMergeDirection.None;
                    Double currentDistance = Double.MaxValue;

                    Segment currentSegment = _result[rowIndex, columnIndex];

                    // only connect if the cell is homogeneous
                    if (currentSegment.IsHomogeneous(_segmentHomogeneityThreshold))
                    {
                        // select the best merge direction based on the ANOVA criteria and the euclidean distance

                        // left
                        if (CanMergeSegments(currentSegment, rowIndex, columnIndex - 2))
                        {
                            currentDistance = _distance.Distance(currentSegment, _result[rowIndex, columnIndex - 2]);
                            direction = SegmentMergeDirection.Left;
                        }

                        // up
                        if (CanMergeSegments(currentSegment, rowIndex - 2, columnIndex))
                        {
                            Double distance = _distance.Distance(currentSegment, _result[rowIndex - 2, columnIndex]);
                            if (distance < currentDistance)
                                direction = SegmentMergeDirection.Up;
                        }

                        // right 1
                        if (CanMergeSegments(rowIndex - 2, columnIndex + 2, rowIndex, columnIndex + 2))
                        {
                            Segment mergedSegment = _result.MergeSegments(_result[rowIndex - 2, columnIndex + 2], _result[rowIndex, columnIndex + 2]);

                            if (CanMergeSegments(mergedSegment, currentSegment))
                            {
                                Double distance = _distance.Distance(mergedSegment, currentSegment);
                                if (distance < currentDistance)
                                    direction = SegmentMergeDirection.Right1;                                
                            }
                        }

                        // right 2
                        /*if (direction == SegmentMergeDirection.Right1 && CanMergeSegments(rowIndex - 2, columnIndex + 4, rowIndex, columnIndex + 4))
                        {
                            Segment upperSegment = _result[rowIndex - 2, columnIndex + 4];
                            Segment lowerSegment = _result[rowIndex, columnIndex + 4];
                            Segment mergedSegment = upperSegment + lowerSegment;

                            if (CanMergeSegments(mergedSegment, currentSegment))
                            {
                                Double distance = SpectralDistances.EuclideanDistance(mergedSegment, currentSegment);
                                if (distance < currentDistance)
                                    direction = SegmentMergeDirection.Right2;
                            }
                        }*/

                        // apply merge
                        switch (direction)
                        {
                            case SegmentMergeDirection.Left:
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex, columnIndex - 2);
                                break;
                            case SegmentMergeDirection.Up:
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex - 2, columnIndex);
                                break;
                            case SegmentMergeDirection.Right1:
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex, columnIndex + 2);
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex - 2, columnIndex + 2);
                                break;
                            case SegmentMergeDirection.Right2:
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex, columnIndex + 2);
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex - 2, columnIndex + 2);
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex, columnIndex + 4);
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex - 2, columnIndex + 4);
                                break;
                        }
                    }
                }
            }

            // remove original segments that are not homogeneous
            for (Int32 rowIndex = 0; rowIndex < _source.Raster.NumberOfRows; rowIndex += 2)
            {
                for (Int32 columnIndex = 0; columnIndex < _source.Raster.NumberOfColumns; columnIndex += 2)
                {
                    if (_result[rowIndex, columnIndex].Count <= 4 && !_result[rowIndex, columnIndex].IsHomogeneous(_segmentHomogeneityThreshold))
                        _result.SplitSegment(rowIndex, columnIndex);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Determines whether the specified segments can be merged.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="otherSegment">The other segment.</param>
        /// <returns><c>true</c> if the segments can be merged, otherwise <c>false</c>.</returns>
        private Boolean CanMergeSegments(Segment segment, Segment otherSegment)
        {
            if (segment == otherSegment)
                return false;

            if (!otherSegment.IsHomogeneous(_segmentHomogeneityThreshold))
                return false;

            for (Int32 bandIndex = 0; bandIndex < _source.Raster.NumberOfBands; bandIndex++)
                if (!AnovaCriteria(segment, otherSegment, bandIndex))
                    return false;

            return true;
        }

        /// <summary>
        /// Determines whether the specified segments can be merged.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="otherRowIndex">The row index of the other segment.</param>
        /// <param name="otherColumnIndex">The column index of he other segment.</param>
        /// <returns><c>true</c> if the segments can be merged, otherwise <c>false</c>.</returns>
        private Boolean CanMergeSegments(Segment segment, Int32 rowIndex, Int32 columnIndex)
        {
            if (rowIndex < 0 || rowIndex >= _source.Raster.NumberOfRows || columnIndex < 0 || columnIndex >= _source.Raster.NumberOfColumns)
                return false;

            Segment otherSegment = _result[rowIndex, columnIndex];
            return CanMergeSegments(segment, otherSegment);
        }

        /// <summary>
        /// Determines whether the specified segments can be merged.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="otherRowIndex">The row index of the other segment.</param>
        /// <param name="otherColumnIndex">The column index of he other segment.</param>
        /// <returns><c>true</c> if the segments can be merged, otherwise <c>false</c>.</returns>
        private Boolean CanMergeSegments(Int32 rowIndex, Int32 columnIndex, Int32 otherRowIndex, Int32 otherColumnIndex)
        {
            if (rowIndex < 0 || rowIndex >= _source.Raster.NumberOfRows || columnIndex < 0 || columnIndex >= _source.Raster.NumberOfColumns)
                return false;
            if (otherRowIndex < 0 || otherRowIndex >= _source.Raster.NumberOfRows || otherColumnIndex < 0 || otherColumnIndex >= _source.Raster.NumberOfColumns)
                return false;

            Segment segment = _result[rowIndex, columnIndex];
            Segment otherSegment = _result[otherRowIndex, otherColumnIndex];

            return CanMergeSegments(segment, otherSegment);
        }

        /// <summary>
        /// Returns whether the ANOVA criteria hold for the specified band of the two segments.
        /// </summary>
        /// <param name="first">The first segment.</param>
        /// <param name="second">The second segment.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns><c>true</c> if the ANOVA criteria are satisfied, otherwise <c>false</c>.</returns>
        private Boolean AnovaCriteria(Segment first, Segment second, Int32 bandIndex)
        {
            Double aX = first.Variance[bandIndex] * (first.Count - 1);
            Double aY = second.Variance[bandIndex] * (second.Count - 1);
            Int32 m = first.Count;
            Int32 n = second.Count;

            if (aX + aY == 0)
                return true;

            Double denominator = Math.Pow((aX + aY) / (n + m), (n + m - 2));

            Double secondCriteriaValue = Math.Pow(aX / m, m - 1) * Math.Pow(aY / n, n - 1) / denominator;
            if (!Double.IsInfinity(denominator) && secondCriteriaValue < _varianceThresholdBeforeMerge)
                return false;

            Double zMean = (first.Mean[bandIndex] * m + second.Mean[bandIndex] * n) / (m + n);

            Double bX = 0, bY = 0;

            foreach (UInt32 value in _result.GetFloatValues(first, bandIndex))
            {
                bX += (value - zMean) * (value - zMean);
            }

            foreach (UInt32 value in _result.GetFloatValues(second, bandIndex))
            {
                bY += (value - zMean) * (value - zMean);
            }

            if (bX + bY == 0)
                return false;

            Double firstCriteriaValue = Math.Pow((aX + aY) / (bX + bY), (m + n) / 2.0);

            if (firstCriteriaValue < _varianceThresholdAfterMerge)
                return false;

            return true;
        }

        #endregion
    }
}
