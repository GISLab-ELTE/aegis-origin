/// <copyright file="SpectralOperationMethods.Classification.cs" company="Eötvös Loránd University (ELTE)">
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

        private static SpectralOperationMethod _densitySlicing;
        private static SpectralOperationMethod _balancedHistogramThresholdingClassification;
        private static SpectralOperationMethod _constantBasedThresholdingClassification;
        private static SpectralOperationMethod _functionBasedThresholdingClassification;
        private static SpectralOperationMethod _otsuThresholdingClassification;
        private static SpectralOperationMethod _paletteColorClassification;
        private static SpectralOperationMethod _randomColorClassification;
        private static SpectralOperationMethod _segmentClassification;
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213826", "Balanced histogram thresholding",
                                                                         "Creates a monochrome raster by separating values based on histogram balancing.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         RasterFormat.Integer,
                                                                         SpectralOperationParameters.BandIndex));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213820", "Constant based spectral thresholding",
                                                                         "Creates a monochrome raster by separating values located within the specified boundaries.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.LowerThresholdBoundary,
                                                                         SpectralOperationParameters.UpperThresholdBoundary,
                                                                         SpectralOperationParameters.BandIndex));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213819", "Density slicing",
                                                                         "For density slicing the range of grayscale levels is divided into intervals, with each interval assigned to one of a few discrete colors – this is in contrast to pseudo color, which uses a continuous color scale.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex,
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213821", "Function based spectral thresholding",
                                                                         "Creates a monochrome raster by separating values based on the specified selector function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.SpectralPredicate,
                                                                         SpectralOperationParameters.BandIndex));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213828", "Otsu thresholding",
                                                                         "Performes shape-based raster thresholding using Otsu's method.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandGlobal,
                                                                         RasterFormat.Integer,
                                                                         SpectralOperationParameters.BandIndex));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213801", "Palette color classification",
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213802", "Random color classification",
                                                                         "This classification method relies on using random RGB colors for creating the classified image based on a collection of segments and/or clusters. The method garantees that each individual class will have a different color.", null, "1.0.0",
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213850", "Reference matching classification",
                                                                         "In reference matching classification, a pre-classified reference raster is aligned with the current raster, with which the process matches the categories. The also process on the collection of clusters.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.SegmentCollection,
                                                                         SpectralOperationParameters.ClassificationReferenceGeometry));
            }
        }

        /// <summary>
        /// Segment classification.
        /// </summary>
        public static SpectralOperationMethod SegmentClassification
        {
            get
            {
                return _segmentClassification ?? (_segmentClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213818", "Segment classification",
                                                                         "The classification of spectral imagery based on a collection of segments. The result data contains the segment indices as values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         SpectralOperationParameters.SegmentCollection));
            }
        }

        #endregion
    }
}
