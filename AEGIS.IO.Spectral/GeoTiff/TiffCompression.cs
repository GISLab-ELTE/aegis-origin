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
    /// Defines TIFF compressions.
    /// </summary>
    public enum TiffCompression
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 1,

        /// <summary>
        /// CCITT 1 dimensional modified Huffman code.
        /// </summary>
        CCITTModifiedHuffman = 2,

        /// <summary>
        /// CCITT T4 two level comptression.
        /// </summary>
        CCITTT4 = 3,

        /// <summary>
        /// CCITT T6 two level comptression.
        /// </summary>
        CCITTT6 = 4,

        /// <summary>
        /// LZW
        /// </summary>
        LZW = 5,

        /// <summary>
        /// JPEG.
        /// </summary>
        JPEG = 6,

        /// <summary>
        /// Deflate
        /// </summary>
        Deflate = 8,

        /// <summary>
        /// PackBits.
        /// </summary>
        PackBits = 32773
    }
}
