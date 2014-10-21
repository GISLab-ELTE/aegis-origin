/// <copyright file="SpectralOperationMethods.Indexing.cs" company="Eötvös Loránd University (ELTE)">
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

        private static SpectralOperationMethod _normalizedDifferenceIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceSoilIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceVegetationIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceWaterIndexComputation;

        #endregion

        #region Public static properties

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
        /// Normalized difference index (NDxI) computation.
        /// </summary>
        public static SpectralOperationMethod NormalizedDifferenceSoilIndexComputation
        {
            get
            {
                return _normalizedDifferenceSoilIndexComputation ?? (_normalizedDifferenceSoilIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213512", "Normalized difference soil index (NDSI) computation",
                                                                         "Normalized difference indices (for soil, vegetation and water) are standard ways to model the ratio of spectral bands.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfNearInfraredBand,
                                                                         SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));
            }
        }

        /// <summary>
        /// Normalized difference vegetation index (NDVI) computation.
        /// </summary>
        public static SpectralOperationMethod NormalizedDifferenceVegetationIndexComputation
        {
            get
            {
                return _normalizedDifferenceVegetationIndexComputation ?? (_normalizedDifferenceVegetationIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213513", "Normalized difference index vegetation (NDVI) computation",
                                                                         "Normalized difference indices (for soil, vegetation and water) are standard ways to model the ratio of spectral bands.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfNearInfraredBand));
            }
        }

        /// <summary>
        /// Normalized difference water index (NDWI) computation.
        /// </summary>
        public static SpectralOperationMethod NormalizedDifferenceWaterIndexComputation
        {
            get
            {
                return _normalizedDifferenceWaterIndexComputation ?? (_normalizedDifferenceWaterIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213514", "Normalized difference index water (NDWI) computation",
                                                                         "Normalized difference indices (for soil, vegetation and water) are standard ways to model the ratio of spectral bands.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));
            }
        }

        #endregion
    }
}
