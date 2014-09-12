/// <copyright file="RasterInterpretations.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a collection of known <see cref="RasterInterpretation"/> instances.
    /// </summary>
    public class RasterInterpretations
    {
        #region Private fields

        private static RasterInterpretation _grayscale;
        private static RasterInterpretation _invertedGrayscale;
        private static RasterInterpretation _RGB;
        private static RasterInterpretation _sRGB;
        private static RasterInterpretation _HSV;
        private static RasterInterpretation _HSL;
        private static RasterInterpretation _CMYK;
        private static RasterInterpretation _YCbCr;
        private static RasterInterpretation _CIELab;
        private static RasterInterpretation _transparent;

        #endregion

        #region Public properties

        /// <summary>
        /// Single sample grayscale interpretation with increasing spectral intensity.
        /// </summary>
        public static RasterInterpretation Grayscale
        {
            get
            {
                return _grayscale ?? (_grayscale = new RasterInterpretation(RasterColorSpace.Grayscale));
            }
        }

        /// <summary>
        /// Single sample grayscale interpretation with descreasing spectral intensity.
        /// </summary>
        public static RasterInterpretation InvertedGrayscale
        {
            get
            {
                return _invertedGrayscale ?? (_invertedGrayscale = new RasterInterpretation(RasterColorSpace.InvertedGrayscale));
            }
        }

        /// <summary>
        /// RGB interpretation.
        /// </summary>
        public static RasterInterpretation RGB
        {
            get
            {
                return _RGB ?? (_RGB = new RasterInterpretation(RasterColorSpace.RGB, RasterColorSpaceChannel.Red, RasterColorSpaceChannel.Green, RasterColorSpaceChannel.Blue));
            }
        }

        /// <summary>
        /// sRGB interpretation.
        /// </summary>
        public static RasterInterpretation SRGB
        {
            get
            {
                return _sRGB ?? (_sRGB = new RasterInterpretation(RasterColorSpace.SRGB, RasterColorSpaceChannel.Red, RasterColorSpaceChannel.Green, RasterColorSpaceChannel.Blue));
            }
        }

        /// <summary>
        /// HSV (hue-saturation-value) interpretation.
        /// </summary>
        public static RasterInterpretation HSV
        {
            get
            {
                return _HSV ?? (_HSV = new RasterInterpretation(RasterColorSpace.HSV, RasterColorSpaceChannel.Hue, RasterColorSpaceChannel.Saturation, RasterColorSpaceChannel.Value));
            }
        }

        /// <summary>
        /// HSV (hue-saturation-lightness) interpretation.
        /// </summary>
        public static RasterInterpretation HSL
        {
            get
            {
                return _HSL ?? (_HSL = new RasterInterpretation(RasterColorSpace.HSL, RasterColorSpaceChannel.Hue, RasterColorSpaceChannel.Saturation, RasterColorSpaceChannel.Lightness));
            }
        }

        /// <summary>
        /// CMYK (cyan-magenta-yellow-black) interpretation.
        /// </summary>
        public static RasterInterpretation CMYK
        {
            get
            {
                return _CMYK ?? (_CMYK = new RasterInterpretation(RasterColorSpace.CMYK, RasterColorSpaceChannel.Cyan, RasterColorSpaceChannel.Magenta, RasterColorSpaceChannel.Yellow, RasterColorSpaceChannel.Black));
            }
        }

        /// <summary>
        /// YCbCr interpretation.
        /// </summary>
        public static RasterInterpretation YCbCr
        {
            get
            {
                return _YCbCr ?? (_YCbCr = new RasterInterpretation(RasterColorSpace.YCbCr, RasterColorSpaceChannel.Luma, RasterColorSpaceChannel.BlueDifferenceChroma, RasterColorSpaceChannel.RedDifferenceChroma));
            }
        }

        /// <summary>
        /// CIE L*a*b interpretation.
        /// </summary>
        public static RasterInterpretation CIELab
        {
            get
            {
                return _CIELab ?? (_CIELab = new RasterInterpretation(RasterColorSpace.CIELab, RasterColorSpaceChannel.Lightness, RasterColorSpaceChannel.A, RasterColorSpaceChannel.B));
            }
        }

        /// <summary>
        /// Transparency values.
        /// </summary>
        public static RasterInterpretation Transparent
        {
            get
            {
                return _transparent ?? (_transparent = new RasterInterpretation(RasterColorSpace.Undefined, RasterColorSpaceChannel.Transparent));
            }
        }

        #endregion
    }
}
