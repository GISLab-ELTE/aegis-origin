/// <copyright file="SpectralOperationMethods.Indexing.cs" company="Eötvös Loránd University (ELTE)">
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

        private static SpectralOperationMethod _anthocyaninReflectanceIndexComputation;
        private static SpectralOperationMethod _anthocyaninReflectanceIndex1Computation;
        private static SpectralOperationMethod _anthocyaninReflectanceIndex2Computation;
        private static SpectralOperationMethod _atmosphericallyResistantVegetationIndexComputation;
        private static SpectralOperationMethod _carotenoidReflectanceIndexComputation;
        private static SpectralOperationMethod _carotenoidReflectanceIndex1Computation;
        private static SpectralOperationMethod _carotenoidReflectanceIndex2Computation;
        private static SpectralOperationMethod _celluloseAbsorptionIndexComputation;
        private static SpectralOperationMethod _enhancedVegetationIndexComputation;
        private static SpectralOperationMethod _modifiedRedEdgeNormalizedDifferenceVegetationIndexComputation;
        private static SpectralOperationMethod _modifiedRedEdgeSimpleRatioIndexComputation;
        private static SpectralOperationMethod _moistureStressIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceInfraredIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceLigninIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceNitrogenIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceSoilIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceVegetationIndexComputation;
        private static SpectralOperationMethod _normalizedDifferenceWaterIndexComputation;
        private static SpectralOperationMethod _photochemicalReflectanceIndexComputation;
        private static SpectralOperationMethod _plantSenescenceReflectanceIndexComputation;
        private static SpectralOperationMethod _redEdgeNormalizedDifferenceVegetationIndexComputation;
        private static SpectralOperationMethod _simpleRatioIndexComputation;
        private static SpectralOperationMethod _soilAdjustedVegetationIndexComputation;
        private static SpectralOperationMethod _structureInsensitivePigmentIndexComputation;
        private static SpectralOperationMethod _sumGreenIndexComputation;
        private static SpectralOperationMethod _vogelmannRedEdgeIndexComputation;
        private static SpectralOperationMethod _vogelmannRedEdgeIndex1Computation;
        private static SpectralOperationMethod _vogelmannRedEdgeIndex2Computation;
        private static SpectralOperationMethod _vogelmannRedEdgeIndex3Computation;
        private static SpectralOperationMethod _waterBandIndexComputation;

        #endregion

        #region Public static properties

        /// <summary>
        /// Anthocyanin reflectance index (ARI) computation.
        /// </summary>
        public static SpectralOperationMethod AnthocyaninReflectanceIndexComputation
        {
            get
            {
                return _anthocyaninReflectanceIndexComputation ?? (_anthocyaninReflectanceIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252370", "Anthocyanin reflectance index (ARI) computation",
                                                                         "The anthocyanin reflectance index (ARI) is a reflectance measurement that is sensitive to anthocyanin in plant foliage.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf550nmBand,
                                                                         SpectralOperationParameters.IndexOf700nmBand,
                                                                         SpectralOperationParameters.IndexOf800nmBand));
            }
        }

        /// <summary>
        /// Anthocyanin reflectance index 1 (ARI1) computation.
        /// </summary>
        public static SpectralOperationMethod AnthocyaninReflectanceIndex1Computation
        {
            get
            {
                return _anthocyaninReflectanceIndex1Computation ?? (_anthocyaninReflectanceIndex1Computation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252371", "Anthocyanin reflectance index 1 (ARI1) computation",
                                                                         "The anthocyanin reflectance index 1 (ARI1) is a reflectance measurement that is sensitive to anthocyanin in plant foliage.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf550nmBand,
                                                                         SpectralOperationParameters.IndexOf700nmBand));
            }
        }

        /// <summary>
        /// Anthocyanin reflectance index 2 (ARI2) computation.
        /// </summary>
        public static SpectralOperationMethod AnthocyaninReflectanceIndex2Computation
        {
            get
            {
                return _anthocyaninReflectanceIndex2Computation ?? (_anthocyaninReflectanceIndex2Computation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252372", "Anthocyanin reflectance index 2 (ARI2) computation",
                                                                         "The anthocyanin reflectance index 2 (ARI2) is a reflectance measurement that is sensitive to anthocyanin in plant foliage.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf550nmBand,
                                                                         SpectralOperationParameters.IndexOf700nmBand,
                                                                         SpectralOperationParameters.IndexOf800nmBand));
            }
        }

        /// <summary>
        /// Atmospherically resistant vegetation index (ARVI) computation.
        /// </summary>
        public static SpectralOperationMethod AtmosphericallyResistantVegetationIndexComputation
        {
            get
            {
                return _atmosphericallyResistantVegetationIndexComputation ?? (_atmosphericallyResistantVegetationIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252031", "Atmospherically resistant vegetation index (ARVI) computation",
                                                                         "The atmospherically resistant vegetation index (ARVI) is an enhancement to the NDVI.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfNearInfraredBand,
                                                                         SpectralOperationParameters.IndexOfBlueBand));
            }
        }

        /// <summary>
        /// Carotenoid reflectance index  (CRIx) computation.
        /// </summary>
        public static SpectralOperationMethod CarotenoidReflectanceIndexComputation
        {
            get
            {
                return _carotenoidReflectanceIndexComputation ?? (_carotenoidReflectanceIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252320", "Carotenoid reflectance index (CRIx) computation",
                                                                         "The carotenoid reflectance index (CRIx) is a reflectance measurement that is sensitive to carotenoid pigments in plant foliage.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf510nmBand,
                                                                         SpectralOperationParameters.IndexOf550nmBand,
                                                                         SpectralOperationParameters.IndexOf700nmBand));
            }
        }

        /// <summary>
        /// Carotenoid reflectance index 1 (ARI1) computation.
        /// </summary>
        public static SpectralOperationMethod CarotenoidReflectanceIndex1Computation
        {
            get
            {
                return _carotenoidReflectanceIndex1Computation ?? (_carotenoidReflectanceIndex1Computation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252321", "Carotenoid reflectance index 1 (CRI1) computation",
                                                                         "The carotenoid reflectance index 1 (CRI1) is a reflectance measurement that is sensitive to carotenoid pigments in plant foliage.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf510nmBand,
                                                                         SpectralOperationParameters.IndexOf550nmBand));
            }
        }

        /// <summary>
        /// Carotenoid reflectance index 2 (CRI2) computation.
        /// </summary>
        public static SpectralOperationMethod CarotenoidReflectanceIndex2Computation
        {
            get
            {
                return _carotenoidReflectanceIndex2Computation ?? (_carotenoidReflectanceIndex2Computation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252322", "Carotenoid reflectance index 2 (CRI2) computation",
                                                                         "The carotenoid reflectance index 2 (CRI2) is a reflectance measurement that is sensitive to carotenoid pigments in plant foliage.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf510nmBand,
                                                                         SpectralOperationParameters.IndexOf700nmBand));
            }
        }

        /// <summary>
        /// Cellulose absorption index (CAI) computation.
        /// </summary>
        public static SpectralOperationMethod CelluloseAbsorptionIndexComputation
        {
            get
            {
                return _celluloseAbsorptionIndexComputation ?? (_celluloseAbsorptionIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252231", "Cellulose absorption index (CAI) computation",
                                                                         "The cellulose absorption index (CAI) quantifies exposed surfaces that contain dried plant material.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf2000nmBand,
                                                                         SpectralOperationParameters.IndexOf2100nmBand,
                                                                         SpectralOperationParameters.IndexOf2200nmBand));
            }
        }

        /// <summary>
        /// Enhanced vegetation index (EVI) computation.
        /// </summary>
        public static SpectralOperationMethod EnhancedVegetationIndexComputation
        {
            get
            {
                return _enhancedVegetationIndexComputation ?? (_enhancedVegetationIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252061", "Enhanced vegetation index (EVI) computation",
                                                                         "The enhanced vegetation index (EVI) is an 'optimized' index designed to enhance the vegetation signal.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfNearInfraredBand,
                                                                         SpectralOperationParameters.IndexOfBlueBand));
            }
        }

        /// <summary>
        /// Modified red edge normalized difference vegetation index (mNDVI705) computation.
        /// </summary>
        public static SpectralOperationMethod ModifiedRedEdgeNormalizedDifferenceVegetationIndexComputation
        {
            get
            {
                return _modifiedRedEdgeNormalizedDifferenceVegetationIndexComputation ?? (_modifiedRedEdgeNormalizedDifferenceVegetationIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252751", "Modified red edge normalized difference vegetation index (mNDVI705) computation",
                                                                         "The modified red edge normalized difference vegetation index is a modification of the NDVI705.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf445nmBand,
                                                                         SpectralOperationParameters.IndexOf705nmBand,
                                                                         SpectralOperationParameters.IndexOf750nmBand));
            }
        }

        /// <summary>
        /// Modified red edge simple ratio index (mSR705) computation.
        /// </summary>
        public static SpectralOperationMethod ModifiedRedEdgeSimpleRatioIndexComputation
        {
            get
            {
                return _modifiedRedEdgeSimpleRatioIndexComputation ?? (_modifiedRedEdgeSimpleRatioIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252752", "Modified red edge simple ratio index (mSR705) computation",
                                                                         "The modified red edge simple ratio index is a modification of the SR.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf445nmBand,
                                                                         SpectralOperationParameters.IndexOf705nmBand,
                                                                         SpectralOperationParameters.IndexOf750nmBand));
            }
        }

        /// <summary>
        /// Moisture stress index (MSI) computation.
        /// </summary>
        public static SpectralOperationMethod MoistureStressIndexComputation
        {
            get
            {
                return _moistureStressIndexComputation ?? (_moistureStressIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252350", "Moisture stress index (MSI) computation",
                                                                         "Moisture stress index (MSI) is a reflectance measurement that is sensitive to increases in leaf water content.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf819nmBand,
                                                                         SpectralOperationParameters.IndexOf1599nmBand));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252011", "Normalized difference index (NDxI) computation",
                                                                         "Normalized difference indices (for soil, vegetation and water) are standard ways to model the ratio of spectral bands.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfNearInfraredBand,
                                                                         SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));
            }
        }

        /// <summary>
        /// Normalized difference infrared index (NDII) computation.
        /// </summary>
        public static SpectralOperationMethod NormalizedDifferenceInfraredIndexComputation
        {
            get
            {
                return _normalizedDifferenceInfraredIndexComputation ?? (_normalizedDifferenceInfraredIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252015", "Normalized difference infrared index (NDII) computation",
                                                                         "Normalized difference infrared index (NDII) are standard ways to model the ratio of spectral bands.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf819nmBand,
                                                                         SpectralOperationParameters.IndexOf1649nmBand));
            }
        }

        /// <summary>
        /// Normalized difference lignin index (NDLI) computation.
        /// </summary>
        public static SpectralOperationMethod NormalizedDifferenceLigninIndexComputation
        {
            get
            {
                return _normalizedDifferenceLigninIndexComputation ?? (_normalizedDifferenceLigninIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252022", "Normalized difference lignin index (NDLI) computation",
                                                                         "Normalized difference lignin index is designed to estimate the relative amounts of lignin contained in vegetation canopies.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf1680nmBand,
                                                                         SpectralOperationParameters.IndexOf1754nmBand));
            }
        }

        /// <summary>
        /// Normalized difference nitrogen index (NDNI) computation.
        /// </summary>
        public static SpectralOperationMethod NormalizedDifferenceNitogenIndexComputation
        {
            get
            {
                return _normalizedDifferenceNitrogenIndexComputation ?? (_normalizedDifferenceNitrogenIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252023", "Normalized difference nitrogen index (NDNI) computation",
                                                                         "Normalized difference nitrogen index is used to measure the strong sensitivity to changing nitrogen status when the canopy is green.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf1510nmBand,
                                                                         SpectralOperationParameters.IndexOf1680nmBand));
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252012", "Normalized difference soil index (NDSI) computation",
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252013", "Normalized difference index vegetation (NDVI) computation",
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
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252014", "Normalized difference index water (NDWI) computation",
                                                                         "Normalized difference indices (for soil, vegetation and water) are standard ways to model the ratio of spectral bands.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfShortWavelengthInfraredBand));
            }
        }

        /// <summary>
        /// Photochemical reflectance index (PRI) computation.
        /// </summary>
        public static SpectralOperationMethod PhotochemicalReflectanceIndexComputation
        {
            get
            {
                return _photochemicalReflectanceIndexComputation ?? (_photochemicalReflectanceIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252301", "Photochemical reflectance index (PRI) computation",
                                                                         "The photochemical reflectance index takes advantage of the changes to carotenoid pigments.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf531nmBand,
                                                                         SpectralOperationParameters.IndexOf570nmBand));
            }
        }

        /// <summary>
        /// Plant senescence reflectance index (PSRI) computation.
        /// </summary>
        public static SpectralOperationMethod PlantSenescenceReflectanceIndexComputation
        {
            get
            {
                return _plantSenescenceReflectanceIndexComputation ?? (_plantSenescenceReflectanceIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252302", "Plant senescence reflectance index (PSRI) computation",
                                                                         "The plant senescence reflectance index is designed to maximize sensitivity of the index to the ratio of bulk carotenoids.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf500nmBand,
                                                                         SpectralOperationParameters.IndexOf680nmBand,
                                                                         SpectralOperationParameters.IndexOf750nmBand));
            }
        }

        /// <summary>
        /// Red edge normalized difference vegetation index (NDVI705) computation.
        /// </summary>
        public static SpectralOperationMethod RedEdgeNormalizedDifferenceVegetationIndexComputation
        {
            get
            {
                return _redEdgeNormalizedDifferenceVegetationIndexComputation ?? (_redEdgeNormalizedDifferenceVegetationIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252750", "Red edge normalized difference vegetation index (NDVI705) computation",
                                                                         "The red edge normalized difference vegetation index is a modification of the normalized difference vegetation index.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf705nmBand,
                                                                         SpectralOperationParameters.IndexOf750nmBand));
            }
        }

        /// <summary>
        /// Simple ratio (SR) index computation.
        /// </summary>
        public static SpectralOperationMethod SimpleRatioIndexComputation
        {
            get
            {
                return _simpleRatioIndexComputation ?? (_simpleRatioIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252070", "Simple ratio (SR) index computation",
                                                                         "The simple ratio index is a well known and often used spectral vegetation index.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfNearInfraredBand));
            }
        }

        /// <summary>
        /// Soil adjusted vegetation (SAVI) index computation.
        /// </summary>
        public static SpectralOperationMethod SoilAdjustedVegetationIndexComputation
        {
            get
            {
                return _soilAdjustedVegetationIndexComputation ?? (_soilAdjustedVegetationIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252033", "Soil adjusted vegetation (SAVI) index computation",
                                                                         "The simple ratio index is a well known and often used spectral vegetation index.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOfRedBand,
                                                                         SpectralOperationParameters.IndexOfNearInfraredBand));
            }
        }

        /// <summary>
        /// Structure insensitive pigment (SIPI) index computation.
        /// </summary>
        public static SpectralOperationMethod StructureInsensitivePigmentIndexComputation
        {
            get
            {
                return _structureInsensitivePigmentIndexComputation ?? (_structureInsensitivePigmentIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252308", "Structure insensitive pigment (SIPI) index computation",
                                                                         "The structure insensitive pigment index is a good index to use in areas with high variability in the canopy structure, or leaf area index.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf445nmBand,
                                                                         SpectralOperationParameters.IndexOf680nmBand,
                                                                         SpectralOperationParameters.IndexOf800nmBand));
            }
        }

        /// <summary>
        /// Sum green (SG) index computation.
        /// </summary>
        public static SpectralOperationMethod SumGreenIndexComputation
        {
            get
            {
                return _sumGreenIndexComputation ?? (_sumGreenIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252205", "Sum green (SR) index computation",
                                                                         "The sum green index is used to detect changes in vegetation greenness.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndicesOfBandsBetween500nm600nm));
            }
        }

        /// <summary>
        /// Vogelmann red edge (VOGx) index computation.
        /// </summary>
        public static SpectralOperationMethod VogelmannRedEdgeIndexComputation
        {
            get
            {
                return _vogelmannRedEdgeIndexComputation ?? (_vogelmannRedEdgeIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252710", "Vogelmann red edge (VOGx) index computation",
                                                                         "The Vogelmann red edge (VOGx) index is a narrowband reflectance measurement.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf715nmBand,
                                                                         SpectralOperationParameters.IndexOf720nmBand,
                                                                         SpectralOperationParameters.IndexOf726nmBand,
                                                                         SpectralOperationParameters.IndexOf734nmBand,
                                                                         SpectralOperationParameters.IndexOf740nmBand,
                                                                         SpectralOperationParameters.IndexOf747nmBand));
            }
        }

        /// <summary>
        /// Vogelmann red edge index 1  (VOG1) computation.
        /// </summary>
        public static SpectralOperationMethod VogelmannRedEdgeIndex1Computation
        {
            get
            {
                return _vogelmannRedEdgeIndex1Computation ?? (_vogelmannRedEdgeIndex1Computation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252711", "Vogelmann red edge index 1 (VOG1) computation",
                                                                         "The Vogelmann red edge index 1 is a narrowband reflectance measurement.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf720nmBand,
                                                                         SpectralOperationParameters.IndexOf740nmBand));
            }
        }

        /// <summary>
        /// Vogelmann red edge index 2 (VOG2) computation.
        /// </summary>
        public static SpectralOperationMethod VogelmannRedEdgeIndex2Computation
        {
            get
            {
                return _vogelmannRedEdgeIndex2Computation ?? (_vogelmannRedEdgeIndex2Computation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252712", "Vogelmann red edge index 2 (VOG2) computation",
                                                                         "The Vogelmann red edge index 2 is a narrowband reflectance measurement.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf715nmBand,
                                                                         SpectralOperationParameters.IndexOf726nmBand,
                                                                         SpectralOperationParameters.IndexOf734nmBand,
                                                                         SpectralOperationParameters.IndexOf747nmBand));
            }
        }

        /// <summary>
        /// Vogelmann red edge index 3 (VOG3) computation.
        /// </summary>
        public static SpectralOperationMethod VogelmannRedEdgeIndex3Computation
        {
            get
            {
                return _vogelmannRedEdgeIndex3Computation ?? (_vogelmannRedEdgeIndex3Computation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252713", "Vogelmann red edge index 3 (VOG3) computation",
                                                                         "The Vogelmann red edge index 3 is a narrowband reflectance measurement.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf715nmBand,
                                                                         SpectralOperationParameters.IndexOf720nmBand,
                                                                         SpectralOperationParameters.IndexOf734nmBand,
                                                                         SpectralOperationParameters.IndexOf747nmBand));
            }
        }

        /// <summary>
        /// Water band index (WBI) computation.
        /// </summary>
        public static SpectralOperationMethod WaterBandIndexComputation
        {
            get
            {
                return _waterBandIndexComputation ?? (_waterBandIndexComputation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::252075", "Water band index (WBI) computation",
                                                                         "The water band index (WBI)  is a reflectance measurement that is sensitive to changes in canopy water content.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.Local,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.IndexOf900nmBand,
                                                                         SpectralOperationParameters.IndexOf970nmBand));
            }
        }

        #endregion
    }
}
