/// <copyright file="RasterPresentationMode.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines the supported raster presentation models.
    /// </summary>
    public enum RasterPresentationModel
    {
        /// <summary>
        /// Specifies natural color presentation for red, green and blue bands.
        /// </summary>
        TrueColor,

        /// <summary>
        /// Specifies custom color presentation for multiple bands.
        /// </summary>
        FalseColor,

        /// <summary>
        /// Specifies grayscale presentation of a single band.
        /// </summary>
        Grayscale,

        /// <summary>
        /// Specifies inverted grayscale presentation of a single band.
        /// </summary>
        InvertedGrayscale,

        /// <summary>
        /// Specifies continuous colored presentation of a single band using a specified color map.
        /// </summary>
        PseudoColor,

        /// <summary>
        /// Specifies discrete colored presentation of a single band using a specified color map.
        /// </summary>
        DensitySlicing,

        /// <summary>
        /// Specifies a transparent representation
        /// </summary>
        Transparency
    }
}
