/// <copyright file="RasterColorSpaceBand.cs" company="Eötvös Loránd University (ELTE)">
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


namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines raster color space bands.
    /// </summary>
    public enum RasterColorSpaceBand
    {
        /// <summary>
        /// Red.
        /// </summary>
        Red,

        /// <summary>
        /// Green.
        /// </summary>
        Green,

        /// <summary>
        /// Blue.
        /// </summary>
        Blue,

        /// <summary>
        /// Hue.
        /// </summary>
        Hue,

        /// <summary>
        /// Saturation.
        /// </summary>
        Saturation,

        /// <summary>
        /// Value.
        /// </summary>
        Value,

        /// <summary>
        /// Lightness.
        /// </summary>
        Lightness,

        /// <summary>
        /// Cyan.
        /// </summary>
        Cyan,

        /// <summary>
        /// Magenta.
        /// </summary>
        Magenta,

        /// <summary>
        /// Yellow.
        /// </summary>
        Yellow,

        /// <summary>
        /// Black.
        /// </summary>
        Black,

        /// <summary>
        /// Luma (Y).
        /// </summary>
        Luma,

        /// <summary>
        /// Blue difference chroma (Cb).
        /// </summary>
        BlueDifferenceChroma,

        /// <summary>
        /// Red difference chroma (Cr).
        /// </summary>
        RedDifferenceChroma,

        /// <summary>
        /// a*.
        /// </summary>
        A,

        /// <summary>
        /// b*.
        /// </summary>
        B,

        /// <summary>
        /// Transparent.
        /// </summary>
        Transparent,

        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined
    }
}
