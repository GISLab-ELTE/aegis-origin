/// <copyright file="SpectralOperationParameters.Segmentation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms.Distances;
using ELTE.AEGIS.Management;
using System;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances for spectral operations.
    /// </summary>
    public static partial class SpectralOperationParameters
    {
        #region Private static fields

        private static OperationParameter _clusterDistanceAlgorithm;
        private static OperationParameter _clusterDistanceThreshold;
        private static OperationParameter _clusterDistanceType;
        private static OperationParameter _clusterSizeThreshold;
        private static OperationParameter _numberOfClusterCenters;
        private static OperationParameter _segmentHomogeneityThreshold;
        private static OperationParameter _segmentMergeThreshold;
        private static OperationParameter _segmentSplitThreshold;
        private static OperationParameter _varianceThresholdBeforeMerge;
        private static OperationParameter _varianceThresholdAfterMerge;

        #endregion

        #region Public static properties

        /// <summary>
        /// Cluster distance algorithm.
        /// </summary>
        public static OperationParameter ClusterDistanceAlgorithm
        {
            get
            {
                return _clusterDistanceAlgorithm ?? (_clusterDistanceAlgorithm =
                    OperationParameter.CreateOptionalParameter<SpectralDistance>("AEGIS::354501", "Cluster distance algorithm",
                                                                                 "The algorithm used for determining the distance of spectral clusters.", null, (SpectralDistance)null)
                );
            }
        }

        /// <summary>
        /// Cluster distance threshold.
        /// </summary>
        public static OperationParameter ClusterDistanceThreshold
        {
            get
            {
                return _clusterDistanceThreshold ?? (_clusterDistanceThreshold =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::354505", "Cluster distance threshold",
                                                                       "The upper threshold of the spectral distance between clusters.", null)
                );
            }
        }

        /// <summary>
        /// Cluster distance type.
        /// </summary>
        public static OperationParameter ClusterDistanceType
        {
            get
            {
                return _clusterDistanceType ?? (_clusterDistanceType =
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::354502", "Cluster distance type",
                                                                     "The type used for determining the distance of spectral clusters.", null, (Type)null)
                );
            }
        }

        /// <summary>
        /// Cluster distance threshold.
        /// </summary>
        public static OperationParameter ClusterSizeThreshold
        {
            get
            {
                return _clusterSizeThreshold ?? (_clusterSizeThreshold =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::354509", "Cluster size threshold",
                                                                       "The lower threshold for the size of the generated clusters.", null, 
                                                                       5.0, 
                                                                       Conditions.IsNotNegative())
                );
            }
        }


        /// <summary>
        /// Number of clusters.
        /// </summary>
        public static OperationParameter NumberOfClusters
        {
            get
            {
                return _numberOfClusterCenters ?? (_numberOfClusterCenters =
                    OperationParameter.CreateOptionalParameter<Int32>("AEGIS::354520", "Number of clusters",
                                                                      "The initial number of clusters for clustering operations.", null, 0)
                );
            }
        }

        /// <summary>
        /// Segment homogeneity threshold.
        /// </summary>
        public static OperationParameter SegmentHomogeneityThreshold
        {
            get
            {
                return _segmentHomogeneityThreshold ?? (_segmentHomogeneityThreshold =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::354621", "Segment homogeneity threshold",
                                                                       "The threshold used for determining whether segments are homogeneous.", null)
                );
            }
        }

        /// <summary>
        /// Segment merge threshold.
        /// </summary>
        public static OperationParameter SegmentMergeThreshold
        {
            get
            {
                return _segmentMergeThreshold ?? (_segmentMergeThreshold =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::354622", "Segment merge threshold",
                                                                       "The threshold used for determining whether segments should be merged.", null)
                );
            }
        }

        /// <summary>
        /// Segment split threshold.
        /// </summary>
        public static OperationParameter SegmentSplitThreshold
        {
            get
            {
                return _segmentSplitThreshold ?? (_segmentSplitThreshold =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::354623", "Segment split threshold",
                                                                       "The threshold used for determining whether segments should be split.", null)
                );
            }
        }

        /// <summary>
        /// Threshold value for segment variance before merge.
        /// </summary>
        public static OperationParameter VarianceThresholdBeforeMerge
        {
            get
            {
                return _varianceThresholdBeforeMerge ?? (_varianceThresholdBeforeMerge =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::354638", "Threshold value for segment variance before merge",
                                                                       "The threshold value which the between segment variance must reach in order to be merged.", null)
                );
            }
        }

        /// <summary>
        /// Threshold value for segment variance after merge.
        /// </summary>
        public static OperationParameter VarianceThresholdAfterMerge
        {
            get
            {
                return _varianceThresholdAfterMerge ?? (_varianceThresholdAfterMerge =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::354639", "Threshold value for segment variance after merge",
                                                                       "The threshold value the merged segment variance must reach in order to be merged.", null)
                );
            }
        }

        #endregion
    }
}
