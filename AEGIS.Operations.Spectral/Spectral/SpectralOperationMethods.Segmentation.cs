/// <copyright file="SpectralOperationMethods.Segmentation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
        private static SpectralOperationMethod _isodataClustering;
        private static SpectralOperationMethod _graphBasedMergeSegmentation;
        private static SpectralOperationMethod _kMeansClustering;
        private static SpectralOperationMethod _quadtreeBasedSegmentation;
        private static SpectralOperationMethod _sequentialCouplingSegmentation;
        private static SpectralOperationMethod _splitAndMergeSegmentation;

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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::254101", "Best merge segmentation",
                                                                         "Best merge segmentation chooses any two neighboring segments over the image if their contraction is optimal with respect to the threshold. The algorithm performs multiple iterations, until no merge can be performed, or until the integration number is reached.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace,
                                                                         CommonOperationParameters.NumberOfIterations,
                                                                         SpectralOperationParameters.SegmentMergeThreshold,
                                                                         SpectralOperationParameters.SpectralDistanceAlgorithm,
                                                                         SpectralOperationParameters.SpectralDistanceType));
            }
        }

        /// <summary>
        /// ISODATA clustering.
        /// </summary>
        public static SpectralOperationMethod IsodataClustering
        {
            get
            {
                return _isodataClustering ?? (_isodataClustering =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::254210", "ISODATA clustering",
                                                                         "ISODATA clustering performs classification of spectral vectors in different clusters, by randomly initializing cluster centers, and then aligning these centers based on multispectral space properties. The initial number of cluster centers, and the distance threshold can be specified.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Global,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.NumberOfClusters,
                                                                         SpectralOperationParameters.ClusterCenterDistanceAlgorithm,
                                                                         SpectralOperationParameters.ClusterCenterDistanceType,
                                                                         SpectralOperationParameters.ClusterDistanceThreshold,
                                                                         SpectralOperationParameters.SpectralDistanceAlgorithm,
                                                                         SpectralOperationParameters.SpectralDistanceType));
            }
        }

        /// <summary>
        /// Graph-based merge segmentation.
        /// </summary>
        public static SpectralOperationMethod GraphBasedMergeSegmentation
        {
            get
            {
                return _graphBasedMergeSegmentation ?? (_graphBasedMergeSegmentation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::254104", "Graph-based merge segmentation",
                                                                         "In graph-based merge segmentation, the image is represented in graph form. Edges are taken in the descending order of their weight, and it is decided whether the two segments belonging to the two end nodes can be contracted.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.SegmentMergeThreshold,
                                                                         SpectralOperationParameters.SpectralDistanceAlgorithm,
                                                                         SpectralOperationParameters.SpectralDistanceType));
            }
        }

        /// <summary>
        /// K-means clustering.
        /// </summary>
        public static SpectralOperationMethod KMeansClustering
        {
            get
            {
                return _kMeansClustering ?? (_kMeansClustering =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::254220", "K-means clustering",
                                                                         "K-means clustering aims to partition n observations into k clusters in which each observation belongs to the cluster with the nearest mean, serving as a prototype of the cluster.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Global,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.NumberOfClusters,
                                                                         SpectralOperationParameters.SpectralDistanceAlgorithm,
                                                                         SpectralOperationParameters.SpectralDistanceType,
                                                                         CommonOperationParameters.NumberOfIterations));
            }
        }
        /// <summary>
        /// Quadtree-based segmentation.
        /// </summary>
        public static SpectralOperationMethod QuadTreeBasedSegmentation
        {
            get
            {
                return _quadtreeBasedSegmentation ?? (_quadtreeBasedSegmentation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::254131", "Quadtree-based segmentation",
                                                                         "Quadtree-based segmentation.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace,
                                                                         CommonOperationParameters.NumberOfIterations,
                                                                         SpectralOperationParameters.SegmentSplitThreshold,
                                                                         SpectralOperationParameters.SpectralDistanceAlgorithm,
                                                                         SpectralOperationParameters.SpectralDistanceType));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::254120", "Sequential coupling segmentation",
                                                                         "Sequential linking deals with the statistical homogeneity of segments using an iteration of merging neighboring cells in row-major order.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.SegmentHomogeneityThreshold,
                                                                         SpectralOperationParameters.VarianceThresholdBeforeMerge,
                                                                         SpectralOperationParameters.VarianceThresholdAfterMerge,
                                                                         SpectralOperationParameters.SpectralDistanceAlgorithm,
                                                                         SpectralOperationParameters.SpectralDistanceType));
            }
        }

        /// <summary>
        /// Split-and-merge segmentation.
        /// </summary>
        public static SpectralOperationMethod SplitAndMergeSegmentation
        {
            get
            {
                return _splitAndMergeSegmentation ?? (_splitAndMergeSegmentation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::254133", "Split-and-merge segmentation",
                                                                         "Splitting and merging attempts to divide an image into uniform regions.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Zonal,
                                                                         ExecutionMode.OutPlace,
                                                                         CommonOperationParameters.NumberOfIterations,
                                                                         SpectralOperationParameters.SegmentSplitThreshold,
                                                                         SpectralOperationParameters.SegmentMergeThreshold,
                                                                         SpectralOperationParameters.SpectralDistanceAlgorithm,
                                                                         SpectralOperationParameters.SpectralDistanceType));
            }
        }

        #endregion
    }
}
