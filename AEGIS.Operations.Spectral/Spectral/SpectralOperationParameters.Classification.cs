/// <copyright file="SpectralOperationParameters.Classification.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Operations.Spectral.Segmentation;
using System;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances for spectral operations.
    /// </summary>
    public static partial class SpectralOperationParameters
    {
        #region Private static fields

        private static OperationParameter _classificationSegmentationMethod;
        private static OperationParameter _classificationSegmentationType;
        private static OperationParameter _classificationClusteringMethod;
        private static OperationParameter _classificationClusteringType;
        private static OperationParameter _classificationStudyArea;
        private static OperationParameter _colorPalette;
        private static OperationParameter _densitySlicingThresholds;
        private static OperationParameter _lowerThresholdBoundary;
        private static OperationParameter _spectralPredicate;
        private static OperationParameter _upperThresholdBoundary;

        #endregion

        #region Public static properties

        /// <summary>
        /// Clustering method of the classification.
        /// </summary>
        public static OperationParameter ClassificationClusteringMethod
        {
            get
            {
                return _classificationClusteringMethod ?? (_classificationClusteringMethod =
                    OperationParameter.CreateOptionalParameter<OperationMethod>("AEGIS::213851", "Clustering method of the classification",
                                                                                "The clustering method used for creating the clusters during thematic classification.", null,
                                                                                (OperationMethod)null)
                );
            }
        }

        /// <summary>
        /// Clustering tyoe of the classification operation.
        /// </summary>
        public static OperationParameter ClassificationClusteringType
        {
            get
            {
                return _classificationClusteringType ?? (_classificationClusteringType =
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::213852", "Clustering type of the classification operation",
                                                                     "The clustering operation used for creating the clusters during thematic classification.", null,
                                                                     typeof(IsodataClustering),
                                                                     Conditions.Inherits<SpectralClustering>())
                );
            }
        }

        /// <summary>
        /// Segmentation method of the classification.
        /// </summary>
        public static OperationParameter ClassificationSegmentationMethod
        {
            get
            {
                return _classificationSegmentationMethod ?? (_classificationSegmentationMethod =
                    OperationParameter.CreateOptionalParameter<OperationMethod>("AEGIS::213854", "Segmentation method of the classification",
                                                                                "The segmentation method used for creating the initial segments during thematic classification.", null,
                                                                                (OperationMethod)null)
                );
            }
        }

        /// <summary>
        /// Segmentation type of the classification operation.
        /// </summary>
        public static OperationParameter ClassificationSegmentationType
        {
            get
            {
                return _classificationSegmentationType ?? (_classificationSegmentationType =
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::213855", "Segmentation type of the classification operation",
                                                                     "The segmentation operation used for creating the initial segments during thematic classification.", null,
                                                                     typeof(SequentialCouplingSegmentation),
                                                                     Conditions.Inherits<SpectralSegmentation>())
                );
            }
        }

        /// <summary>
        /// Study area of the classification.
        /// </summary>
        public static OperationParameter ClassificationStudyArea
        {
            get
            {
                return _classificationStudyArea ?? (_classificationStudyArea =
                    OperationParameter.CreateRequiredParameter<ISpectralGeometry>("AEGIS::213850", "Study area of the classification",
                                                                                  "The pre-classified spectral geometry that serves as a study area for classification.", null)
                );
            }
        }

        /// <summary>
        /// Color palette.
        /// </summary>
        public static OperationParameter ColorPalette
        {
            get
            {
                return _colorPalette ?? (_colorPalette =
                    OperationParameter.CreateRequiredParameter<UInt32[][]>("AEGIS::213801", "Color palette",
                                                                           "An array specifying an RGB color palette.", null)
                    );
            }
        }

        /// <summary>
        /// Contrast enhancement value.
        /// </summary>
        public static OperationParameter DensitySlicingThresholds
        {
            get
            {
                return _densitySlicingThresholds ?? (_densitySlicingThresholds =
                    OperationParameter.CreateOptionalParameter<Double[]>("AEGIS::223819", "Density slicing thresholds",
                                                                         "The array of threshold values used for dencity slicing.", null,
                                                                         (Double[])null)
                    );
            }
        }

        /// <summary>
        /// Lower threshold boundary.
        /// </summary>
        public static OperationParameter LowerThresholdBoundary
        {
            get
            {
                return _lowerThresholdBoundary ?? (_lowerThresholdBoundary =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS:223821", "Lower threshold boundary",
                                                                       "The lower threshold boundary value for creating a monochrome image.", null)
                );

            }
        }

        /// <summary>
        /// Spectral predicate.
        /// </summary>
        public static OperationParameter SpectralPredicate
        {
            get
            {
                return _spectralPredicate ?? (_spectralPredicate =
                    OperationParameter.CreateRequiredParameter<Func<IRaster, Int32, Int32, Int32, Boolean>>("AEGIS::223800", "Spectral predicate",
                                                                                                            "Represents a function that defines a set of criteria on the specified raster and determines whether the current value meets those criteria.", null)
                );
            }
        }

        /// <summary>
        /// Upper threshold boundary.
        /// </summary>
        public static OperationParameter UpperThresholdBoundary
        {
            get
            {
                return _upperThresholdBoundary ?? (_upperThresholdBoundary =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS:223822", "Upper threshold boundary",
                                                                       "The upper threshold boundary for creating a monochrome image.", null,
                                                                       Double.PositiveInfinity)
                );

            }
        }

        #endregion
    }
}
