/// <copyright file="SpectralOperationParameters.Classification.cs" company="Eötvös Loránd University (ELTE)">
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

        private static OperationParameter _classificationClusteringMethod;
        private static OperationParameter _classificationClusteringType;
        private static OperationParameter _classificationReferenceGeometry;
        private static OperationParameter _classificationSegmentationMethod;
        private static OperationParameter _classificationSegmentationType;
        private static OperationParameter _classificationValidationGeometry;
        private static OperationParameter _colorPalette;
        private static OperationParameter _densitySlicingThresholds;
        private static OperationParameter _lowerThresholdBoundary;
        private static OperationParameter _meanthreshThresholdingConstant;
        private static OperationParameter _spectralPredicate;
        private static OperationParameter _standardDeviationRange;
        private static OperationParameter _standardDeviationWeight;
        private static OperationParameter _thresholdingWindowRadius;
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
                    OperationParameter.CreateOptionalParameter<OperationMethod>("AEGIS::354121", "Clustering method of the classification",
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
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::354122", "Clustering type of the classification operation",
                                                                     "The clustering operation used for creating the clusters during thematic classification.", null,
                                                                     typeof(IsodataClustering),
                                                                     Conditions.Inherits<SpectralClustering>())
                );
            }
        }

        /// <summary>
        /// Reference spectral geometry of the classification.
        /// </summary>
        public static OperationParameter ClassificationReferenceGeometry
        {
            get
            {
                return _classificationReferenceGeometry ?? (_classificationReferenceGeometry =
                    OperationParameter.CreateRequiredParameter<ISpectralGeometry>("AEGIS::354150", "Reference spectral geometry of the classification",
                                                                                  "The pre-classified spectral geometry that serves as a reference area for classification.", null)
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
                    OperationParameter.CreateOptionalParameter<OperationMethod>("AEGIS::354124", "Segmentation method of the classification",
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
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::354125", "Segmentation type of the classification operation",
                                                                     "The segmentation operation used for creating the initial segments during thematic classification.", null,
                                                                     typeof(SequentialCouplingSegmentation),
                                                                     Conditions.Inherits<SpectralSegmentation>())
                );
            }
        }

        /// <summary>
        /// Validation spectral geometry of the classification.
        /// </summary>
        public static OperationParameter ClassificationValidationGeometry
        {
            get
            {
                return _classificationValidationGeometry ?? (_classificationValidationGeometry =
                    OperationParameter.CreateRequiredParameter<ISpectralGeometry>("AEGIS::354160", "Validation spectral geometry of the classification",
                                                                                  "The pre-classified spectral geometry that serves as validation of classification results.", null)
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
                    OperationParameter.CreateRequiredParameter<UInt16[][]>("AEGIS::213801", "Color palette",
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
                    OperationParameter.CreateOptionalParameter<Double[]>("AEGIS::354920", "Density slicing thresholds",
                                                                         "The array of threshold values used for density slicing.", null,
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
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS:354801", "Lower threshold boundary",
                                                                       "The lower threshold boundary value for creating a monochrome image.", null)
                );

            }
        }

        /// <summary>
        /// Meanthresh thresholding constant.
        /// </summary>
        public static OperationParameter MeanthreshThresholdingConstant
        {
            get
            {
                return _meanthreshThresholdingConstant ?? (_meanthreshThresholdingConstant =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::354826", "Meanthresh thresholding constant",
                                                                       "Defines a constant which increases the resistance to noise of the Meanthresh algorithm.", null,
                                                                       0, Conditions.IsNotNegative()));
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
                    OperationParameter.CreateRequiredParameter<Func<IRaster, Int32, Int32, Int32, Boolean>>("AEGIS::354200", "Spectral predicate",
                                                                                                            "Represents a function that defines a set of criteria on the specified raster and determines whether the current value meets those criteria.", null)
                );
            }
        }

        /// <summary>
        /// Standard deviation range.
        /// </summary>
        public static OperationParameter StandardDeviationRange
        {
            get
            {
                return _standardDeviationRange ?? (_standardDeviationRange =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::354830", "Standard deviation range", "The range of the standard deviation used by Sauvola's algorithm.", null,
                                                                       128,
                                                                       Conditions.IsGreaterThanOrEqualTo(1))
                );
            }
        }

        /// <summary>
        /// Standard deviation weight.
        /// </summary>
        public static OperationParameter StandardDeviationWeight
        {
            get
            {
                return _standardDeviationWeight ?? (_standardDeviationWeight =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::354831", "Standard deviation weight",
                                                                       "Determines the weight of the standard deviation for thresholding.", null,
                                                                       0.5, 
                                                                       Conditions.IsBetween(0, 1))
                );
            }
        }

        /// <summary>
        /// Thresholding window radius.
        /// </summary>
        public static OperationParameter ThresholdingWindowRadius
        {
            get
            {
                return _thresholdingWindowRadius ?? (_thresholdingWindowRadius =
                    OperationParameter.CreateOptionalParameter<Int32>("AEGIS::354820", "Thresholding window radius", 
                                                                      "The window radius of local thresholding operations.", null, 
                                                                      3, Conditions.IsPositive()));
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
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS:354802", "Upper threshold boundary",
                                                                       "The upper threshold boundary for creating a monochrome image.", null,
                                                                       Double.PositiveInfinity)
                );

            }
        }

        #endregion
    }
}
