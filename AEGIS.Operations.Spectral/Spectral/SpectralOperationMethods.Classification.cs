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
        private static SpectralOperationMethod _waterloggingClassification;

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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213124", "Balanced histogram thresholding",
                                                                         "Creates a monochrome raster by separating values based on histogram balancing.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         RasterFormat.Integer,
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
        /// Function based spectral thresholding.
        /// </summary>
        public static SpectralOperationMethod FunctionBasedThresholdClassification
        {
            get
            {
                return _functionBasedThresholdingClassification ?? (_functionBasedThresholdingClassification =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213122", "Function based spectral thresholding",
                                                                         "Creates a monochrome raster by separating values based on the specified selector function.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandLocal,
                                                                         SpectralOperationParameters.SpectralSelectorFunction,
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::214624", "Waterlogging classification",
                                                                        "", null, "1.0.0",
                                                                        false, SpectralOperationDomain.BandLocal,
                                                                        ExecutionMode.OutPlace | ExecutionMode.InPlace,
                                                                        SpectralOperationParameters.IndexOfRedBand,
                                                                        SpectralOperationParameters.IndexOfNearInfraredBand,
                                                                        SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));
            }
        }

        #endregion
    }
}
