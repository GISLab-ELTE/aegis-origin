/// <copyright file="TiffSampleFormat.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines the TIFF tag types.
    /// </summary>
    public enum TiffTagType
    {
        /// <summary>
        /// Unsigned byte.
        /// </summary>
        Byte = 1,

        /// <summary>
        /// ASCII character (8 bits).
        /// </summary>
        ASCII = 2,

        /// <summary>
        /// Unsigned short integer (16 bits).
        /// </summary>
        Short = 3,

        /// <summary>
        /// Unsigned long integer (32 bits).
        /// </summary>
        Long = 4,

        /// <summary>
        /// Unsigned rational (64 bits).
        /// </summary>
        Rational = 5,

        /// <summary>
        /// Signed byte.
        /// </summary>
        SByte = 6,

        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined = 7,

        /// <summary>
        /// Signed short integer (16 bits).
        /// </summary>
        SShort = 8,

        /// <summary>
        /// Signed long integer (32 bits).
        /// </summary>
        SLong = 9,

        /// <summary>
        /// Signed rational (64 bits).
        /// </summary>
        SRational = 10,
        
        /// <summary>
        /// Floating point number (32 bits).
        /// </summary>
        Float = 11,

        /// <summary>
        /// Double precision floating point number (64 bits).
        /// </summary>
        Double = 12
    }
}
