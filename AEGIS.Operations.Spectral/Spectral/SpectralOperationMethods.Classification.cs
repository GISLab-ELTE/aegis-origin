/// <copyright file="SpectralOperationMethods.Classification.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using System;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="SpectralOperationMethod" /> instances.
    /// </summary>
    public static partial class SpectralOperationMethods
    {
        #region Private static fields

        private static SpectralOperationMethod _densitySlicing;
        private static SpectralOperationMethod _bernsenLocalThresholdingClassification;
        private static OperationMethod _classificationAccuracyValidation;
        private static OperationMethod _classificationConfusionValidation;
        private static SpectralOperationMethod _classificationMapValidation;
        private static SpectralOperationMethod _meanthreshLocalThresholdingClassification;
        private static SpectralOperationMethod _niblackLocalThresholdingClassification;
        private static SpectralOperationMethod _balancedHistogramThresholdingClassification;
        private static SpectralOperationMethod _constantBasedThresholdingClassification;
        private static SpectralOperationMethod _functionBasedThresholdingClassification;
        private static SpectralOperationMethod _otsuThresholdingClassification;
        private static SpectralOperationMethod _paletteColorClassification;
        private static SpectralOperationMethod _randomColorClassification;
        private static SpectralOperationMethod _sauvolaLocalThresholdingClassification;
        private static SpectralOperationMethod _segmentBasedClassification;
        private static SpectralOperationMethod _referenceMatchingClassification;

        #endregion

        #region Public static properties

        /// <summary>
        /// Balanced histogram thresholding.
        /// </summary>
        public static SpectralOperationMethod BalancedHistogramThresholdingClassification
        {
            get
            {
                return _balancedHistogramThresholdingClassification ?? (_balancedHistogramThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253126", "Balanced histogram thresholding",
                                                                         "Creates a monochrome raster by separating values based on histogram balancing.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandGlobal,
                                                                         RasterFormat.Integer));
            }
        }

        /// <summary>
        /// Bernsen local thresholding.
        /// </summary>
        public static SpectralOperationMethod BernsenLocalThresholdingClassification
        {
            get
            {
                return _bernsenLocalThresholdingClassification ?? (_bernsenLocalThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253302", "Bernsen local thresholding",
                                                                         "Performs raster thresholding based on local mean value.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         RasterFormat.Integer));
            }
        }

        /// <summary>
        /// Classification validation by accuracy.
        /// </summary>
        public static OperationMethod ClassificationAccuracyValidation
        {
            get
            {
                return _classificationAccuracyValidation ?? (_classificationAccuracyValidation =
                    OperationMethod.CreateMethod<ISpectralGeometry, Double>("AEGIS::253921", "Classification validation by accuracy",
                                                                            "The validation of classification based on a reference geometry that results in the percentage of correctly classified pixels.", null, "1.0.0",
                                                                            false, GeometryModel.Spatial,
                                                                            ExecutionMode.OutPlace,
                                                                            SpectralOperationParameters.ClassificationValidationGeometry));
            }
        }

        /// <summary>
        /// Classification validation by confusion matrix.
        /// </summary>
        public static OperationMethod ClassificationConfusionValidation
        {
            get
            {
                return _classificationConfusionValidation ?? (_classificationConfusionValidation =
                    OperationMethod.CreateMethod<ISpectralGeometry, Matrix>("AEGIS::253922", "Classification validation using confusion matrix",
                                                                            "The validation of classification based on a reference geometry that results in a confusion matrix.", null, "1.0.0",
                                                                            false, GeometryModel.Spatial,
                                                                            ExecutionMode.OutPlace,
                                                                            SpectralOperationParameters.ClassificationValidationGeometry));
            }
        }

        /// <summary>
        /// Classification validation by image map.
        /// </summary>
        public static SpectralOperationMethod ClassificationMapValidation
        {
            get
            {
                return _classificationMapValidation ?? (_classificationMapValidation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253925", "Classification validation using image map",
                                                                         "The  validation of classification based on a reference geometry that results in an image map of correctly classified pixels.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.ClassificationValidationGeometry));
            }
        }

        /// <summary>
        /// Constant based spectral thresholding.
        /// </summary>
        public static SpectralOperationMethod ConstantBasedThresholdClassification
        {
            get
            {
                return _constantBasedThresholdingClassification ?? (_constantBasedThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253120", "Constant based spectral thresholding",
                                                                         "Creates a monochrome raster by separating values located within the specified boundaries.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.LowerThresholdBoundary,
                                                                         SpectralOperationParameters.UpperThresholdBoundary));
            }
        }

        /// <summary>
        /// Density slicing.
        /// </summary>
        public static SpectralOperationMethod DensitySlicing
        {
            get
            {
                return _densitySlicing ?? (_densitySlicing =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253405", "Density slicing",
                                                                         "For density slicing the range of grayscale levels is divided into intervals, with each interval assigned to one of a few discrete colors – this is in contrast to pseudo color, which uses a continuous color scale.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.DensitySlicingThresholds));
            }
        }

        /// <summary>
        /// Function based spectral thresholding.
        /// </summary>
        public static SpectralOperationMethod FunctionBasedThresholdClassification
        {
            get
            {
                return _functionBasedThresholdingClassification ?? (_functionBasedThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253121", "Function based spectral thresholding",
                                                                         "Creates a monochrome raster by separating values based on the specified selector function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.SpectralPredicate));
            }
        }

        /// <summary>
        /// Meanthresh local thresholding.
        /// </summary>
        public static SpectralOperationMethod MeanthreshLocalThresholdingClassification
        {
            get
            {
                return _meanthreshLocalThresholdingClassification ?? (_meanthreshLocalThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253305", "Meanthresh local thresholding",
                                                                         "Performs raster thresholding based on band mean value.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         RasterFormat.Integer,
                                                                         SpectralOperationParameters.MeanthreshThresholdingConstant));
            }
        }

        /// <summary>
        /// Niblack local thresholding.
        /// </summary>
        public static SpectralOperationMethod NiblackLocalThresholdingClassification
        {
            get
            {
                return _niblackLocalThresholdingClassification ?? (_niblackLocalThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253311", "Niblack local thresholding",
                                                                         "Performs raster thresholding based on local mean and standard deviation value.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         RasterFormat.Integer,
                                                                         SpectralOperationParameters.StandardDeviationWeight,
                                                                         SpectralOperationParameters.ThresholdingWindowRadius));
            }
        }

        /// <summary>
        /// Otsu thresholding.
        /// </summary>
        public static SpectralOperationMethod OtsuThresholdingClassification
        {
            get
            {
                return _otsuThresholdingClassification ?? (_otsuThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253205", "Otsu thresholding",
                                                                         "Performs shape-based raster thresholding using Otsu's method.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandGlobal,
                                                                         RasterFormat.Integer));
            }
        }

        /// <summary>
        /// Palette color classification.
        /// </summary>
        public static SpectralOperationMethod PaletteColorClassification
        {
            get
            {
                return _paletteColorClassification ?? (_paletteColorClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253401", "Palette color classification",
                                                                         "This classification method relies on using palette RGB colors for creating the classified image based on an image with multiple classes.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         SpectralOperationParameters.ColorPalette));
            }
        }

        /// <summary>
        /// Random color classification.
        /// </summary>
        public static SpectralOperationMethod RandomColorClassification
        {
            get
            {
                return _randomColorClassification ?? (_randomColorClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253402", "Random color classification",
                                                                         "This classification method relies on using random RGB colors for creating the classified image based on a collection of segments and/or clusters. The method guarantees that each individual class will have a different color.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         RasterFormat.Integer));
            }
        }

        /// <summary>
        /// Reference matching classification.
        /// </summary>
        public static SpectralOperationMethod ReferenceMatchingClassification
        {
            get
            {
                return _referenceMatchingClassification ?? (_referenceMatchingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253010", "Reference matching classification",
                                                                         "In reference matching classification, a pre-classified reference raster is aligned with the current raster, with which the process matches the categories. The also process on the collection of clusters.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.SegmentCollection,
                                                                         SpectralOperationParameters.ClassificationReferenceGeometry));
            }
        }

        /// <summary>
        /// Sauvola local thresholding.
        /// </summary>
        public static SpectralOperationMethod SauvolaLocalThresholdingClassification
        {
            get
            {
                return _sauvolaLocalThresholdingClassification ?? (_sauvolaLocalThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253312", "Sauvola local thresholding",
                                                                         "Local adaptive thresholding based on The Niblack algorithm.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         RasterFormat.Integer,
                                                                         SpectralOperationParameters.StandardDeviationRange,
                                                                         SpectralOperationParameters.StandardDeviationWeight,
                                                                         SpectralOperationParameters.ThresholdingWindowRadius));
            }
        }

        /// <summary>
        /// Segment based classification.
        /// </summary>
        public static SpectralOperationMethod SegmentBasedClassification
        {
            get
            {
                return _segmentBasedClassification ?? (_segmentBasedClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253018", "Segment based classification",
                                                                         "The classification of spectral imagery based on a collection of segments. The result data contains the segment indices as values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         SpectralOperationParameters.SegmentCollection));
            }
        }

        #endregion
    }
}
