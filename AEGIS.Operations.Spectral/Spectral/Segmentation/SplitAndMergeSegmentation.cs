/// <copyright file="SplitAndMergeSegmentation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Greta Bereczki</author>

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    using ELTE.AEGIS.Collections.Segmentation;
    using ELTE.AEGIS.Operations.Management;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an operation performing split-and-merge segmentation of a raster image.
    /// </summary>
    [OperationMethodImplementation("AEGIS::254133", "Split-and-merge segmentation")]
    public class SplitAndMergeSegmentation : SpectralSegmentation
    {
        #region Private fields

        /// <summary>
        /// The maximum number of iterations.
        /// </summary>
        private Double _numberOfIterations;

        /// <summary>
        /// The split threshold.
        /// </summary>
        private Double _splitHomogeneityThreshold;

        /// <summary>
        /// The merge threshold.
        /// </summary>
        private Double _mergeHomogeneityThreshold;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitAndMergeSegmentation"/> class.
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
        public SplitAndMergeSegmentation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, SpectralOperationMethods.SplitAndMergeSegmentation, parameters)
        {
            _numberOfIterations = Convert.ToDouble(ResolveParameter(CommonOperationParameters.NumberOfIterations));
            _splitHomogeneityThreshold = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SegmentSplitThreshold));
            _mergeHomogeneityThreshold = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SegmentMergeThreshold));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            if (_result == null)
                _result = new RectangularSegmentCollection(_source.Raster);
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            RectangularSegmentCollection result = _result as RectangularSegmentCollection;
            IList<Segment> segmentList = result.GetSegments().ToList();

            // split until all the segments are homogeneous or the threshold is reached
            Boolean isHomogeneous = false;
            Double step = 0;
            while (!isHomogeneous && step < _numberOfIterations)
            {
                isHomogeneous = true;
                foreach (Segment segment in segmentList)
                {
                    if (!segment.IsHomogeneous(_splitHomogeneityThreshold))
                    {
                        isHomogeneous = false;
                        result.SplitIntoFour(segment);
                    }
                }
                step++;

                segmentList = result.GetSegments().ToList();
            }

            // set the Morton code values
            segmentList = segmentList.OrderBy(x => x.MortonCode).ToList();
            Double maxNumberOfDigits = Math.Floor(Math.Log10(segmentList.Last().MortonCode) + 1);
            foreach (Segment segment in segmentList)
            {
                Int32 numberOfDigits = Convert.ToInt32(Math.Floor(Math.Log10(segment.MortonCode) + 1));
                for (Int32 i = 0; i < maxNumberOfDigits - numberOfDigits; i++)
                    segment.MortonCode = segment.MortonCode * 10 + 1;
            }

            // merge the appropriate segments
            for (Int32 i = 0; i < segmentList.Count - 1; i++)
            {
                // determine the category of the segment
                Double firstDigit1 = Math.Abs(segmentList[i].MortonCode);
                Double firstDigit2 = Math.Abs(segmentList[i + 1].MortonCode);

                while (firstDigit1 >= 10)
                {
                    firstDigit1 /= 10;
                    firstDigit2 /= 10;
                }

                // merge if possible
                if (Convert.ToInt32(firstDigit1) != Convert.ToInt32(firstDigit2) && segmentList[i].IsMergeHomogenous(segmentList[i + 1], _mergeHomogeneityThreshold))
                {
                    result.MergeSegments(segmentList[i], segmentList[i + 1]);
                    segmentList = result.GetSegments().ToList();
                    segmentList = segmentList.OrderBy(x => x.MortonCode).ToList();
                }
            }
        }

        #endregion
    }
}
