/// <copyright file="QuadTreeBasedSegmentation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Greta Bereczki</author>

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    using ELTE.AEGIS.Collections.Segmentation;
    using ELTE.AEGIS.Operations.Management;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an operation performing quadtree-based segmentation of a raster image.
    /// </summary>
    [OperationMethodImplementation("AEGIS::254131", "Quadtree-based segmentation")]
    public class QuadtreeBasedSegmentation : SpectralSegmentation
    {
        #region Private fields

        /// <summary>
        /// The maximum number of iterations.
        /// </summary>
        private Double _numberOfIterations;

        /// <summary>
        /// The split homogeneity threshold.
        /// </summary>
        private Double _splitHomogeneityThreshold;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadtreeBasedSegmentation"/> class.
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
        public QuadtreeBasedSegmentation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, SpectralOperationMethods.QuadTreeBasedSegmentation, parameters)
        {
            _numberOfIterations = Convert.ToDouble(ResolveParameter(CommonOperationParameters.NumberOfIterations));
            _splitHomogeneityThreshold = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SegmentSplitThreshold));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            QuadSegmentCollection result = ResultSegments as QuadSegmentCollection;
            IList<Segment> segmentList = result.GetSegments().ToList();

            // split until all the segments are homogeneous
            Boolean isHomogeneous = false;
            Double step = 0;
            while (!isHomogeneous && step < _numberOfIterations)
            {
                isHomogeneous = true;
                foreach (Segment segment in segmentList)
                {
                    if (!segment.IsHomogeneous(_splitHomogeneityThreshold) && segment.Count != 1)
                    {
                        isHomogeneous = false;
                        result.SplitQuad(segment);
                    }
                }
                step++;
                segmentList = result.GetSegments().ToList();
            }
        }

        #endregion

        #region Protected SpectralSegmentation methods

        /// <summary>
        /// Prepares the segment collection.
        /// </summary>
        /// <returns>
        /// The resulting segment collection.
        /// </returns>
        protected override SegmentCollection PrepareSegmentCollection()
        {
            return new QuadSegmentCollection(Source.Raster, _distance.Statistics | SpectralStatistics.Variance);
        }

        #endregion
    }
}
