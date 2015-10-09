/// <copyright file="SpectralOperationMethods.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="SpectralOperationMethod" /> instances.
    /// </summary>
    [OperationMethodCollection]
    public static partial class SpectralOperationMethods
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

        #region Private static fields

        private static SpectralOperationMethod _contrastLimitedAdaptingHistogramEqualization;
        private static SpectralOperationMethod _gammaCorrection;
        private static SpectralOperationMethod _histogramEqualization;
        private static SpectralOperationMethod _histogramSpecification;
        private static SpectralOperationMethod _histogramMatching;
        private static SpectralOperationMethod _inverseGammaCorrection;
        private static SpectralOperationMethod _saturatingContrastEnhancement;
        private static SpectralOperationMethod _spectralInversion;
        private static SpectralOperationMethod _spectralResampling;
        private static SpectralOperationMethod _spectralTranslation;
        private static SpectralOperationMethod _temperatureExtraction;
        private static SpectralOperationMethod _topOfAtmospehereReflectanceComputation;

        #endregion

        #region Public static properties

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
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.BandIndices));
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
                                                                         SpectralOperationParameters.BandIndices,
                                                                         SpectralOperationParameters.GammaValue));
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
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.BandIndices));
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
                                                                         SpectralOperationParameters.BandIndices,
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
                                                                         SpectralOperationParameters.BandIndices,
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
                                                                         SpectralOperationParameters.BandIndices,
                                                                         SpectralOperationParameters.GammaValue));
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
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.BandIndices));
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
                                                                         "Inverts all spectral values to the opposite.", null, "1.0.0",
                                                                         true, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.BandIndices));
            }
        }

        /// <summary>
        /// Spectral resampling.
        /// </summary>
        public static SpectralOperationMethod SpectralResampling
        {
            get
            {
                return _spectralResampling ?? (_spectralResampling =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213301", "Spectral resampling",
                                                                         "Resamples spectral values to specified resolution. Resampling of th raster is performed using a predefined algorithm.", null, "1.0.0",
                                                                         true, SpectralOperationDomain.BandFocal,
                                                                         SpectralOperationParameters.NumberOfRows,
                                                                         SpectralOperationParameters.NumberOfColumns,
                                                                         SpectralOperationParameters.RasterResamplingAlgorithm,
                                                                         SpectralOperationParameters.RasterResamplingAlgorithmType));
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
                                                                         SpectralOperationParameters.BandIndex,
                                                                         SpectralOperationParameters.BandIndices));
            }
        }

        /// <summary>
        /// Temperature extraction.
        /// </summary>
        public static SpectralOperationMethod TemperatureExtraction
        {
            get
            {
                return _temperatureExtraction ?? (_temperatureExtraction =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213467", "Temperature extraction",
                                                                         "Determines surface temperature (in Kelvin) on imagery based on long wavelength infrared (thermal) values.", null, "1.0.0",
                                                                         true, SpectralOperationDomain.Local, 
                                                                         SpectralOperationParameters.IndexOfLongWavelengthInfraredBand));
            }
        }

        /// <summary>
        /// Top of atmosphere reflectance computation.
        /// </summary>
        public static SpectralOperationMethod TopOfAtmospehereReflectanceComputation
        {
            get
            {
                return _topOfAtmospehereReflectanceComputation ?? (_topOfAtmospehereReflectanceComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213461", "Top of atmosphere reflectance computation",
                                                                         "Top-of-atmosphere reflectance (or TOA reflectance) is the reflectance measured by a space-based sensor flying higher than the earth's atmosphere.", 
                                                                         new String[] { "ToA reflectance computation", "ToAref computation" }, "1.0.0",
                                                                         true, SpectralOperationDomain.Local));
            }
        }

        #endregion
    }
}
