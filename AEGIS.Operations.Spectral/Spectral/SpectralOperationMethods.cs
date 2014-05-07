﻿/// <copyright file="SpectralOperationMethods.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="SpectralOperationMethod" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(OperationMethod))]
    public static class SpectralOperationMethods
    {
        #region Query fields

        private static OperationMethod[] _all;
        
        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="SpectralOperationMethod" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="SpectralOperationMethod" /> instances within the collection.</value>
        public static IList<OperationMethod> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(SpectralOperationMethods).GetProperties().
                                                            Where(property => property.Name != "All").
                                                            Select(property => property.GetValue(null, null) as OperationMethod).
                                                            ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="SpectralOperationMethod" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="SpectralOperationMethod" /> instances that match the specified identifier.</returns>
        public static IList<OperationMethod> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList();
        }

        /// <summary>
        /// Returns all <see cref="SpectralOperationMethod" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="SpectralOperationMethod" /> instances that match the specified name.</returns>
        public static IList<OperationMethod> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList();
        }

        #endregion

        #region Private static instances

        private static SpectralOperationMethod _boxFilter;
        private static SpectralOperationMethod _contrastLimitedAdaptingHistogramEqualization;
        private static SpectralOperationMethod _densitySlicing;
        private static SpectralOperationMethod _customFilter;
        private static SpectralOperationMethod _customThresholdClassification;
        private static SpectralOperationMethod _gammaCorrection;
        private static SpectralOperationMethod _gaussianBlurFilter;
        private static SpectralOperationMethod _histogramEqualization;
        private static SpectralOperationMethod _histogramSpecification;
        private static SpectralOperationMethod _histogramMatching;
        private static SpectralOperationMethod _inverseGammaCorrection;
        private static SpectralOperationMethod _laplaceFilter;
        private static SpectralOperationMethod _maximumFilter;
        private static SpectralOperationMethod _medianFilter;
        private static SpectralOperationMethod _minimumFilter;
        private static SpectralOperationMethod _normalizedDifferenceIndexComputation;
        private static SpectralOperationMethod _otsuThresholdClassification;
        private static SpectralOperationMethod _saturatingContrastEnhancement;
        private static SpectralOperationMethod _spectralInversion;
        private static SpectralOperationMethod _spectralTranslation;
        private static SpectralOperationMethod _topOfAthmospehereReflectanceComputation;
        private static SpectralOperationMethod _waterloggingClassification;
        private static SpectralOperationMethod _weightedMedianFilter;

        #endregion

        #region Public static properties

        /// <summary>
        /// Box filter.
        /// </summary>
        public static SpectralOperationMethod BoxFilter
        {
            get
            {
                return _boxFilter ?? (_boxFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213202", "Box filter",
                                                                         "Returns the average of the neighbouring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterSize,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Contrast limited adapting histogram equalization.
        /// </summary>
        public static SpectralOperationMethod ContrastLimitedAdaptingHistogramEqualization
        {
            get
            {
                return _contrastLimitedAdaptingHistogramEqualization ?? (_contrastLimitedAdaptingHistogramEqualization =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213133", "Contrast limited adapting histogram equalization",
                                                                         "Contrast limited adapting histogram equalization (CLAHE) differs from ordinary adaptive histogram equalization in its contrast limiting.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Custom filter.
        /// </summary>
        public static SpectralOperationMethod CustomFilter
        {
            get
            {
                return _customFilter ?? (_customFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213200", "Custom filter",
                                                                         "Returns a linear combination of the neighbouring values based on kernel, factor and offset values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterKernel,
                                                                         SpectralOperationParameters.FilterOffset,
                                                                         SpectralOperationParameters.FilterFactor,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Custom threshold.
        /// </summary>
        public static SpectralOperationMethod CustomThresholdClassification
        {
            get
            {
                return _customThresholdClassification ?? (_customThresholdClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213120", "Custom threshold",
                                                                         "Creates a monochrome raster by applying a threshold value on all intensity values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.LowerThresholdValue,
                                                                         SpectralOperationParameters.UpperThresholdValue,
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213145", "Density slicing",
                                                                         "For density slicing the range of grayscale levels is divided into intervals, with each interval assigned to one of a few discrete colors – this is in contrast to pseudo color, which uses a continuous color scale.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.DensitySlicingThresholds));
            }
        }

        /// <summary>
        /// Gamma correction.
        /// </summary>
        public static SpectralOperationMethod GammaCorrection
        {
            get
            {
                return _gammaCorrection ?? (_gammaCorrection =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213180", "Gamma correction",
                                                                         "Gamma encoding of images is required to compensate for properties of human vision, hence to maximize the use of the bits or bandwidth relative to how humans perceive light and color.", null, "1.0.0",
                                                                         true, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.GammaValue));
            }
        }

        /// <summary>
        /// Gaussian blur filter.
        /// </summary>
        public static SpectralOperationMethod GaussianBlurFilter
        {
            get
            {
                return _gaussianBlurFilter ?? (_gaussianBlurFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213204", "Gaussian blur filter",
                                                                         "A Gaussian blur (also known as Gaussian smoothing) is the result of blurring an image by a Gaussian function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterSize,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Histogram equalization.
        /// </summary>
        public static SpectralOperationMethod HistogramEqualization
        {
            get
            {
                return _histogramEqualization ?? (_histogramEqualization =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213131", "Histogram equalization",
                                                                         "Histogram equalization is a method in image processing of contrast adjustment using the image's histogram.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Histogram matching.
        /// </summary>
        public static SpectralOperationMethod HistogramMatching
        {
            get
            {
                return _histogramMatching ?? (_histogramMatching =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213138", "Histogram matching",
                                                                         "Histogram matching is the adjustment of raster histogram to the histogram of another raster.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.HistogramMatchValues));
            }
        }

        /// <summary>
        /// Histogram specification.
        /// </summary>
        public static SpectralOperationMethod HistogramSpecification
        {
            get
            {
                return _histogramSpecification ?? (_histogramSpecification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213139", "Histogram specification",
                                                                         "Histogram specification transforms the band histograms to match the shapes of specific function, rather than simply equalizing them.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.HistogramMatchFunction));
            }
        }

        /// <summary>
        /// Inverse gamma correction.
        /// </summary>
        public static SpectralOperationMethod InverseGammaCorrection
        {
            get
            {
                return _inverseGammaCorrection ?? (_inverseGammaCorrection =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213181", "Inverse gamma correction",
                                                                         "Gamma encoding of images is required to compensate for properties of human vision, hence to maximize the use of the bits or bandwidth relative to how humans perceive light and color. This method is an inversion of the original transformation.", null, "1.0.0",
                                                                         true, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.GammaValue));
            }
        }

        /// <summary>
        /// Laplace filter.
        /// </summary>
        public static SpectralOperationMethod LaplaceFilter
        {
            get
            {
                return _laplaceFilter ?? (_laplaceFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213205", "Laplace filter",
                                                                         "The Laplace filter is primarily used for edge detection and motion estimation.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterSize,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Maximum filter.
        /// </summary>
        public static SpectralOperationMethod MaximumFilter
        {
            get
            {
                return _maximumFilter ?? (_maximumFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213215", "Maximum filter",
                                                                         "Returns the maximum of the the neighbouring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterSize,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Minimum filter.
        /// </summary>
        public static SpectralOperationMethod MinimumFilter
        {
            get
            {
                return _minimumFilter ?? (_minimumFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213216", "Minimum filter",
                                                                         "Returns the minimum of the the neighbouring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterSize,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Median filter.
        /// </summary>
        public static SpectralOperationMethod MedianFilter
        {
            get
            {
                return _medianFilter ?? (_medianFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213213", "Median filter",
                                                                         "Returns the median of the the neighbouring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterSize,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Normalized difference index (NDxI) computation.
        /// </summary>
        public static SpectralOperationMethod NormalizedDifferenceIndexComputation
        {
            get
            {
                return _normalizedDifferenceIndexComputation ?? (_normalizedDifferenceIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213511", "Normalized difference index (NDxI) computation",
                                                                         "Normalized difference indices (for soil, vegetation and water) are standard ways to model the ratio of spectral bands.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfNearInfraredBand,
                                                                         SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));
            }
        }

        /// <summary>
        /// Otsu threshold.
        /// </summary>
        public static SpectralOperationMethod OtsuThresholdClassification
        {
            get
            {
                return _otsuThresholdClassification ?? (_otsuThresholdClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213121", "Otsu threshold",
                                                                         "Performes shape-based raster thresholding using Otsu's method.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandGlobal,
                                                                         RasterRepresentation.Integer,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Saturating contrast enhancement.
        /// </summary>
        public static SpectralOperationMethod SaturatingContrastEnhancement
        {
            get
            {
                return _saturatingContrastEnhancement ?? (_saturatingContrastEnhancement =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213130", "Saturating contrast enhancement",
                                                                         "Saturating contrast enhancement is a linear transformation method, where the raster histogram is pulled to fill the entire spectrum.", null, "1.0.0",
                                                                         true, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Spectral inversion.
        /// </summary>
        public static SpectralOperationMethod SpectralInversion
        {
            get
            {
                return _spectralInversion ?? (_spectralInversion =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213104", "Spectral inversion",
                                                                         "Creates a raster with opposite intensity values.", null, "1.0.0",
                                                                         true, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Spectral translation.
        /// </summary>
        public static SpectralOperationMethod SpectralTranslation
        {
            get
            {
                return _spectralTranslation ?? (_spectralTranslation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213102", "Spectral translation",
                                                                         "The affine translation of spectral values.", null, "1.0.0",
                                                                         true, SpectralOperationDomain.Local,
                                                                         SpectralOperationParameters.SpectralOffset,
                                                                         SpectralOperationParameters.SpectralFactor,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Top of atmosphere reflectance computation.
        /// </summary>
        public static SpectralOperationMethod TopOfAthmospehereReflectanceComputation
        {
            get
            {
                return _topOfAthmospehereReflectanceComputation ?? (_topOfAthmospehereReflectanceComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213461", "Top of atmosphere reflectance computation",
                                                                         ".", null, "1.0.0",
                                                                         true, SpectralOperationDomain.Local,
                                                                         SpectralOperationParameters.SpectralOffset,
                                                                         SpectralOperationParameters.SpectralFactor,
                                                                         SpectralOperationParameters.BandIndex));
            }
        }

        /// <summary>
        /// Waterlogging classification.
        /// </summary>
        public static SpectralOperationMethod WaterloggingClassification
        {
            get
            {
                return _waterloggingClassification ?? (_waterloggingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::000000", "Waterlogging classification",
                                                                        "", null, "1.0.0",
                                                                        false, SpectralOperationDomain.BandLocal,
                                                                        ExecutionMode.OutPlace | ExecutionMode.InPlace,
                                                                        SpectralOperationParameters.IndexOfRedBand,
                                                                        SpectralOperationParameters.IndexOfNearInfraredBand,
                                                                        SpectralOperationParameters.IndexOfShortWavelengthInfraredBand,
                                                                        SpectralOperationParameters.ConvertResultToRGB));
            }
        }

        /// <summary>
        /// Weighted median filter.
        /// </summary>
        public static SpectralOperationMethod WeightedMedianFilter
        {
            get
            {
                return _weightedMedianFilter ?? (_weightedMedianFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213214", "Weighted median filter",
                                                                        "Returns the median of the the neighbouring values multiplied by the weight kernel.", null, "1.0.0",
                                                                        false, SpectralOperationDomain.BandFocal,
                                                                        ExecutionMode.OutPlace,
                                                                        SpectralOperationParameters.FilterKernel,
                                                                        SpectralOperationParameters.BandIndex));
            }
        }

        #endregion
    }
}
