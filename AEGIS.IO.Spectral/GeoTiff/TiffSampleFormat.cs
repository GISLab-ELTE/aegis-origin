/// <copyright file="TiffSampleFormat.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Defines the TIFF sample formats.
    /// </summary>
    public enum TiffSampleFormat
    {
        /// <summary>
        /// Unsigned integer.
        /// </summary>
        UnsignedInteger = 1,

        /// <summary>
        /// Two's complement signed integer.
        /// </summary>
        SignedInteger = 2,

        /// <summary>
        /// IEEE floating point.
        /// </summary>
        Floating = 3,

        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined = 4
    }
}
