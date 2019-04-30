/// <copyright file="RasterColorSpace.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines color spaces os raster images.
    /// </summary>
    public enum RasterColorSpace
    {
        /// <summary>
        /// RGB color space.
        /// </summary>
        RGB,

        /// <summary>
        /// HSV (hue-saturation-value) color space.
        /// </summary>
        HSV,

        /// <summary>
        /// HSV (hue-saturation-lightness) color space.
        /// </summary>
        HSL,

        /// <summary>
        /// CMYK (cyan-magenta-yellow-black) color space.
        /// </summary>
        CMYK,

        /// <summary>
        /// YCbCr color space.
        /// </summary>
        YCbCr,

        /// <summary>
        /// CIE L*a*b color space.
        /// </summary>
        CIELab,

        /// <summary>
        /// User-defined color space.
        /// </summary>
        UserDefined,

        /// <summary>
        /// No color space.
        /// </summary>
        None
    }
}
