/// <copyright file="SpectralOperationMethods.Filtering.cs" company="Eötvös Loránd University (ELTE)">
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

        private static SpectralOperationMethod _boxFilter;
        private static SpectralOperationMethod _customFilter;
        private static SpectralOperationMethod _discreteLaplaceFilter;
        private static SpectralOperationMethod _gaborFilter;
        private static SpectralOperationMethod _gaussianBlurFilter;
        private static SpectralOperationMethod _kuwaharaFilter;
        private static SpectralOperationMethod _maximumFilter;
        private static SpectralOperationMethod _meanRemovalFilter;
        private static SpectralOperationMethod _medianFilter;
        private static SpectralOperationMethod _minimumFilter;
        private static SpectralOperationMethod _prewittFilter;
        private static SpectralOperationMethod _robertsFilter;
        private static SpectralOperationMethod _scharrFilter;
        private static SpectralOperationMethod _sobelFilter;
        private static SpectralOperationMethod _unsharpMasking;
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251101", "Box filter",
                                                                         "Returns the mean of the neighboring values, thus smoothening the image.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251100", "Custom filter",
                                                                         "Returns a linear combination of the neighboring values based on kernel, factor and offset values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterKernel,
                                                                         SpectralOperationParameters.FilterOffset,
                                                                         SpectralOperationParameters.FilterFactor));
            }
        }

        /// <summary>
        /// Discrete Laplace filter.
        /// </summary>
        public static SpectralOperationMethod DiscreteLaplaceFilter
        {
            get
            {
                return _discreteLaplaceFilter ?? (_discreteLaplaceFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251205", "Discrete Laplace filter",
                                                                         "The Laplace filter is primarily used for edge detection and motion estimation.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251104", "Gaussian blur filter",
                                                                         "A Gaussian blur (also known as Gaussian smoothing) is the result of blurring an image by a Gaussian function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius,
                                                                         SpectralOperationParameters.FilterStandardDeviation));
            }
        }

        /// <summary>
        /// Gabor filter.
        /// </summary>
        public static SpectralOperationMethod GaborFilter
        {
            get
            {
                return _gaborFilter ?? (_gaborFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251175", "Gabor filter",
                                                                         "Returns with a real component of Gabor filter impulse answer.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterOrientation,
                                                                         SpectralOperationParameters.FilterPhaseOffset,
                                                                         SpectralOperationParameters.FilterSpatialAspectRatio,
                                                                         SpectralOperationParameters.FilterStandardDeviation,
                                                                         SpectralOperationParameters.FilterWavelength));
            }
        }

        /// <summary>
        /// Kuwahara filter.
        /// </summary>
        public static SpectralOperationMethod KuwaharaFilter
        {
            get
            {
                return _kuwaharaFilter ?? (_kuwaharaFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251168", "Kuwahara filter",
                                                                         "The Kuwahara filter is a non-linear smoothing filter used in image processing for adaptive noise reduction.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251141", "Maximum filter",
                                                                         "Returns the maximum of the neighboring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251142", "Minimum filter",
                                                                         "Returns the minimum of the neighboring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius));
            }
        }

        /// <summary>
        /// Mean removal filter.
        /// </summary>
        public static SpectralOperationMethod MeanRemovalFilter
        {
            get
            {
                return _meanRemovalFilter ?? (_meanRemovalFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251301", "Mean removal filter",
                                                                         "Removes the mean of the neighboring values from the central value. This filter has an opposite effect as the box filter, thus helping image sharpening.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterWeight));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251108", "Median filter",
                                                                         "Returns the median of the neighboring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius));
            }
        }

        /// <summary>
        /// Prewitt filter.
        /// </summary>
        public static SpectralOperationMethod PrewittFilter
        {
            get
            {
                return _prewittFilter ?? (_prewittFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251181", "Prewitt filter",
                                                                         "The Prewitt filter is used for edge detection purposes by computing an approximation of the gradient of the image intensity function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace));
            }
        }

        /// <summary>
        /// Roberts filter.
        /// </summary>
        public static SpectralOperationMethod RobertsFilter
        {
            get
            {
                return _robertsFilter ?? (_robertsFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251185", "Roberts filter",
                                                                         "The Roberts filter is primarily used for edge detection and motion estimation.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace));
            }
        }

        /// <summary>
        /// Scharr filter.
        /// </summary>
        public static SpectralOperationMethod ScharrFilter
        {
            get
            {
                return _scharrFilter ?? (_scharrFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251183", "Scharr filter",
                                                                         "The Scharr edge detection filter is a specialization of the Sobel filter. Scharr operators result from an optimization minimizing weighted mean squared angular error in Fourier domain.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace));
            }
        }

        /// <summary>
        /// Sobel filter.
        /// </summary>
        public static SpectralOperationMethod SobelFilter
        {
            get
            {
                return _sobelFilter ?? (_sobelFilter =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251182", "Sobel filter",
                                                                         "The Sobel filter is used for edge detection purposes by computing an approximation of the gradient of the image intensity function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace));
            }
        }

        /// <summary>
        /// Unsharp masking.
        /// </summary>
        public static SpectralOperationMethod UnsharpMasking
        {
            get
            {
                return _unsharpMasking ?? (_unsharpMasking =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251302", "Unsharp masking",
                                                                         "Unsharp masking uses a blurred positive image to create a mask of the original image, which is then combined with the negative image to create a sharper image. The method uses Gaussian blur technique.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.SharpeningAmount,
                                                                         SpectralOperationParameters.SharpeningRadius,
                                                                         SpectralOperationParameters.SharpeningThreshold));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::251109", "Weighted median filter",
                                                                        "Returns the median of the neighboring values multiplied by the weight kernel.", null, "1.0.0",
                                                                        false, SpectralOperationDomain.BandFocal,
                                                                        ExecutionMode.OutPlace,
                                                                        SpectralOperationParameters.FilterKernel));
            }
        }


        #endregion
    }
}
