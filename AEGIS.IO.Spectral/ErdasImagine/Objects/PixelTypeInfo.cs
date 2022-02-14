/// <copyright file="PixelTypeInfo.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamas Nagy</author>

using System;

namespace ELTE.AEGIS.IO.ErdasImagine.Objects
{
    /// <summary>
    /// Defines methods for providing information on <see cref="PixelType" /> instances.
    /// </summary>
    public static class PixelTypeInfo
    {
        #region Public static methods

        /// <summary>
        /// Parses the specified pixel type string.
        /// </summary>
        /// <param name="pixelType">The pixel type string.</param>
        /// <returns>The parsed pixel type.</returns>
        public static PixelType Parse(String pixelType)
        {
            switch (pixelType.ToLowerInvariant())
            {
                case "u1":
                    return PixelType.UInt1;
                case "u2":
                    return PixelType.UInt2;
                case "u4":
                    return PixelType.UInt4;
                case "u8":
                    return PixelType.UInt8;
                case "u16":
                    return PixelType.UInt16;
                case "u32":
                    return PixelType.UInt32;
                case "s8":
                    return PixelType.Int8;
                case "s16":
                    return PixelType.Int16;
                case "s32":
                    return PixelType.Int32;
                case "f32":
                    return PixelType.Float32;
                case "f64":
                    return PixelType.Float64;
                case "c64":
                    return PixelType.Complex64;
                case "c128":
                    return PixelType.Complex128;
                default:
                    return PixelType.Undefined;
            }
        }

        /// <summary>
        /// Gets the size of the pixel.
        /// </summary>
        /// <param name="pixelType">The pixel type.</param>
        /// <returns>The number of bits of the pixel.</returns>
        public static Int32 GetSize(PixelType pixelType)
        {
            switch (pixelType)
            {
                case PixelType.UInt1:
                    return 1;
                case PixelType.UInt2:
                    return 2;
                case PixelType.UInt4:
                    return 4;
                case PixelType.UInt8:
                    return 8;
                case PixelType.Int8:
                    return 8;
                case PixelType.Int16:
                    return 16;
                case PixelType.Int32:
                    return 32;
                case PixelType.Float32:
                    return 32;
                case PixelType.Float64:
                    return 64;
                case PixelType.Complex64:
                    return 64;
                case PixelType.Complex128:
                    return 128;
                default:
                    return 0;
            }
        }

        #endregion
    }
}
