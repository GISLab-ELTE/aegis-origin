/// <copyright file="SpectralOperationMethods.Segmentation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="SpectralOperationMethod" /> instances.
    /// </summary>
    public static partial class SpectralOperationMethods
    {
        #region Private static fields

        private static SpectralOperationMethod _bestMergeBasedSegmentation;
        private static SpectralOperationMethod _graphBasedSegmentation;
        private static SpectralOperationMethod _saturationThresholdingSegmentation;
        private static SpectralOperationMethod _sequentialCouplingSegmentation;
        private static SpectralOperationMethod _splitBasedSegmentation;

        #endregion

        #region Public static properties

        /// <summary>
        /// Best merge segmentation.
        /// </summary>
        public static SpectralOperationMethod BestMergeBasedSegmentation
        {
            get
            {
                return _bestMergeBasedSegmentation ?? (_bestMergeBasedSegmentation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::000000", "Best merge segmentation",
                                                                         "", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.NumberOfIterations,
                                                                         SpectralOperationParameters.SegmentMergeThreshold));
            }
        }

        /// <summary>
        /// Graph based merge segmentation.
        /// </summary>
        public static SpectralOperationMethod GraphBasedSegmentation
        {
            get
            {
                return _graphBasedSegmentation ?? (_graphBasedSegmentation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::000000", "Graph based merge segmentation",
                                                                         "", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.SegmentMergeThreshold));
            }
        }

        /// <summary>
        /// Saturation thresholding segmentation.
        /// </summary>
        public static SpectralOperationMethod SaturationThresholdingSegmentation
        {
            get
            {
                return _saturationThresholdingSegmentation ?? (_saturationThresholdingSegmentation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::000000", "Saturation thresholding segmentation",
                                                                         "", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace
                                                                         ));
            }
        }

        /// <summary>
        /// Sequential coupling segmentation.
        /// </summary>
        public static SpectralOperationMethod SequentialCouplingSegmentation
        {
            get
            {
                return _sequentialCouplingSegmentation ?? (_sequentialCouplingSegmentation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::000000", "Sequential coupling segmentation ",
                                                                         "", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.SegmentHomogeneityThreshold,
                                                                         SpectralOperationParameters.VarianceThresholdBeforeMerge,
                                                                         SpectralOperationParameters.VarianceThresholdAfterMerge));
            }
        }

        /// <summary>
        /// Split based segmentation.
        /// </summary>
        public static SpectralOperationMethod SplitBasedSegmentation
        {
            get
            {
                return _splitBasedSegmentation ?? (_splitBasedSegmentation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::000000", "Split based segmentation",
                                                                         "", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Global,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.SegmentHomogeneityThreshold));
            }
        }

        #endregion
    }
}
