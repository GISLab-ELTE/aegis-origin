/// <copyright file="BestMergeBasedSegmentation.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    /// <summary>
    /// Represents an operation performing segmentation on spectral geometries using best merge.
    /// </summary>
    [OperationMethodImplementation("AEGIS::254101", "Best merge segmentation")]
    public class BestMergeBasedSegmentation : SpectralSegmentation
    {
        #region Private types

        /// <summary>
        /// Defines the possible directions for segment merging.
        /// </summary>
        private enum SegmentMergeDirection { Left, Up, Right, Down }

        #endregion

        #region Private Fields
        
        private Int32 _numberOfIterations;
        private Double _mergeThreshold;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BestMergeBasedSegmentation" /> class.
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
        public BestMergeBasedSegmentation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, SpectralOperationMethods.BestMergeBasedSegmentation, parameters)
        {
            _numberOfIterations = Convert.ToInt32(ResolveParameter(CommonOperationParameters.NumberOfIterations));
            _mergeThreshold = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SegmentMergeThreshold));            
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            if (_result == null)
                _result = new SegmentCollection(_source.Raster, _distance.Statistics);
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            Boolean hasMerged = true;
            Double[] distances = new Double[4];

            for (Int32 iterationIndex = 0; iterationIndex < _numberOfIterations && hasMerged; iterationIndex++)
            {
                hasMerged = false;

                for (Int32 rowIndex = 0; rowIndex < _source.Raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < _source.Raster.NumberOfColumns; columnIndex++)
                    {
                        // select best merge direction

                        distances[0] = ComputeDistance(rowIndex, columnIndex, rowIndex, columnIndex - 1); // left
                        distances[1] = ComputeDistance(rowIndex, columnIndex, rowIndex - 1, columnIndex); // up
                        distances[2] = ComputeDistance(rowIndex, columnIndex, rowIndex, columnIndex + 1); // right
                        distances[3] = ComputeDistance(rowIndex, columnIndex, rowIndex + 1, columnIndex); // down

                        Double minDistance = distances.Min();

                        if (minDistance >= _mergeThreshold)
                            continue;

                        // apply merge

                        switch ((SegmentMergeDirection)Array.IndexOf(distances, minDistance))
                        {
                            case SegmentMergeDirection.Left:
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex, columnIndex - 1);
                                break;
                            case SegmentMergeDirection.Up:
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex - 1, columnIndex);
                                break;
                            case SegmentMergeDirection.Right:
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex, columnIndex + 1);
                                break;
                            case SegmentMergeDirection.Down:
                                _result.MergeSegments(rowIndex, columnIndex, rowIndex + 1, columnIndex);
                                break;
                        }

                        hasMerged = true;
                    }
                }
            }
        }
        
        #endregion

        #region Private methods

        /// <summary>
        /// Computes the distance between the specified segments.
        /// </summary>
        /// <param name="firstRowIndex">The row index of the first segment.</param>
        /// <param name="firstColumnIndex">The column index of the first segment.</param>
        /// <param name="secondRowIndex">The row index of the second segment.</param>
        /// <param name="secondColumnIndex">The column index of the second segment.</param>
        /// <returns></returns>
        private Double ComputeDistance(Int32 firstRowIndex, Int32 firstColumnIndex, Int32 secondRowIndex, Int32 secondColumnIndex)
        {
            if (firstRowIndex < 0 || firstColumnIndex < 0 || firstRowIndex >= _source.Raster.NumberOfRows || firstColumnIndex >= _source.Raster.NumberOfColumns)
                return Double.MaxValue;

            if (secondRowIndex < 0 || secondColumnIndex < 0 || secondRowIndex >= _source.Raster.NumberOfRows || secondColumnIndex >= _source.Raster.NumberOfColumns)
                return Double.MaxValue;

            Segment firstSegment = _result[firstRowIndex, firstColumnIndex];
            Segment secondSegment = _result[secondRowIndex, secondColumnIndex];

            if (firstSegment == secondSegment)
                return Double.MaxValue;

            return _distance.Distance(firstSegment, secondSegment);
        }

        #endregion
    }
}
