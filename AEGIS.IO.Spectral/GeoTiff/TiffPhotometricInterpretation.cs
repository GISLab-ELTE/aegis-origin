/// <copyright file="TiffCompression.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Defines the TIFF photometric interpretations.
    /// </summary>
    public enum TiffPhotometricInterpretation
    {
        /// <summary>
        /// Monochrome, white is zero.
        /// </summary>
        WhiteIsZero = 0,

        /// <summary>
        /// Monochrome, black is zero.
        /// </summary>
        BlackIsZero = 1,

        /// <summary>
        /// RGB.
        /// </summary>
        RGB = 2,

        /// <summary>
        /// Palette color.
        /// </summary>
        PaletteColor = 3,

        /// <summary>
        /// Transparency mask.
        /// </summary>
        TransparencyMask = 4,

        /// <summary>
        /// CMYK.
        /// </summary>
        CMYK = 5,

        /// <summary>
        /// YCbCr.
        /// </summary>
        YCbCr = 6,

        /// <summary>
        /// CIE L*a*b.
        /// </summary>
        CIELab = 8
    }
}
