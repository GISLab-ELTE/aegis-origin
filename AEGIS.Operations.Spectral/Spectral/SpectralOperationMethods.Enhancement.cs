/// <copyright file="SpectralOperationMethods.Enhancement.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Robeto Giachetta. Licensed under the
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
    public partial class SpectralOperationMethods
    {
        #region Private static fields

        private static SpectralOperationMethod _morphologicalClosingOperation;
        private static SpectralOperationMethod _morphologicalDilationOperation;
        private static SpectralOperationMethod _morphologicalErosionOperation;
        private static SpectralOperationMethod _morphologicalOpeningOperation;

        #endregion

        #region Public static properties

        /// <summary>
        /// Morphological closing operation.
        /// </summary>
        public static SpectralOperationMethod MorphologicalClosingOperation
        {
            get
            {
                return _morphologicalClosingOperation ?? (_morphologicalClosingOperation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213294", "Morphological closing operation",
                                                                         "Erosion is one of two fundamental operations (the other being dilation) in morphological image processing. In mathematical morphology, the closing of a set by a structuring element is the erosion of the dilation of that set. Closing removes small holes within the image.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.MorphologicalStructuringElement));
            }
        }

        /// <summary>
        /// Morphological dilation operation.
        /// </summary>
        public static SpectralOperationMethod MorphologicalDilationOperation
        {
            get
            {
                return _morphologicalDilationOperation ?? (_morphologicalDilationOperation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213292", "Morphological dilation operation",
                                                                         "Dilation is one of two fundamental operations (the other being erosion) in morphological image processing. In dilation, the value of the output pixel is the minimum value of all the pixels in the input pixel's neighbourhood.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.MorphologicalStructuringElement));
            }
        }

        /// <summary>
        /// Morphological erosion operation.
        /// </summary>
        public static SpectralOperationMethod MorphologicalErosionOperation
        {
            get
            {
                return _morphologicalErosionOperation ?? (_morphologicalErosionOperation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213291", "Morphological erosion operation",
                                                                         "In erosion, the value of the output pixel is the maximum value of all the pixels in the input pixel's neighbourhood.", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.MorphologicalStructuringElement));
            }
        }

        /// <summary>
        /// Morphological opening operation.
        /// </summary>
        public static SpectralOperationMethod MorphologicalOpeningOperation
        {
            get
            {
                return _morphologicalOpeningOperation ?? (_morphologicalOpeningOperation =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::213293", "Morphological opening operation",
                                                                         "In mathematical morphology, opening is the dilation of the erosion of a set by a structuring element. Opening removes small objects within the image. ", null, "1.0.0",
                                                                         false, SpectralOperationDomain.BandFocal,
                                                                         ExecutionMode.OutPlace,
                                                                         SpectralOperationParameters.MorphologicalStructuringElement));
            }
        }

        #endregion
    }
}
