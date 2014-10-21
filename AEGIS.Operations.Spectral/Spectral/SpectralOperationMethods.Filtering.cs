/// <copyright file="SpectralOperationMethods.Filtering.cs" company="Eötvös Loránd University (ELTE)">
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

        private static SpectralOperationMethod _boxFilter;
        private static SpectralOperationMethod _customFilter;
        private static SpectralOperationMethod _gaussianBlurFilter;
        private static SpectralOperationMethod _laplaceFilter;
        private static SpectralOperationMethod _maximumFilter;
        private static SpectralOperationMethod _meanRemovalFilter;
        private static SpectralOperationMethod _medianFilter;
        private static SpectralOperationMethod _minimumFilter;
        private static SpectralOperationMethod _prewittFilter;
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213202", "Box filter",
                                                                         "Returns the mean of the neighbouring values, thus smothening the image.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius,
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
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.FilterRadius,
                                                                         SpectralOperationParameters.GaussianStandardDeviation));
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
                                                                         SpectralOperationParameters.FilterRadius,
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
                                                                         "Returns the maximum of the neighbouring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius,
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
                                                                         "Returns the minimum of the neighbouring values.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterRadius,
                                                                         SpectralOperationParameters.BandIndex));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213241", "Mean removal filter",
                                                                         "Removes the mean of the neighbouring values from the central value. This filter has an opposite effect as the box filter, thus helping image sharpening.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.FilterWeight,
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
                                                                         SpectralOperationParameters.FilterRadius,
                                                                         SpectralOperationParameters.BandIndex));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213264", "Prewitt filter",
                                                                         "The Prewitt filter is used for edge detection purposes by computing an approximation of the gradient of the image intensity function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.BandIndex));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213265", "Sobel filter",
                                                                         "The Sobel filter is used for edge detection purposes by computing an approximation of the gradient of the image intensity function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.BandIndex));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213242", "Unsharp masking",
                                                                         "Unsharp masking uses a blurred positive image to create a mask of the original image, which is then combined with the negative image to create a sharper image. The method uses Gaussian blur technique.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.BandIndex,
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
