// <copyright file="PixelType.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

namespace ELTE.AEGIS.IO.ErdasImagine.Objects
{
    /// <summary>
    /// Defines the pixel types.
    /// </summary>
    /// <author>Tamas Nagy</author>
    public enum PixelType
    {
        /// <summary>
        /// Indicates an unsigned integer number with 1 bits.
        /// </summary>
        UInt1,

        /// <summary>
        /// Indicates an unsigned integer number with 2 bits.
        /// </summary>
        UInt2,

        /// <summary>
        /// Indicates an unsigned integer number with 4 bits.
        /// </summary>
        UInt4,

        /// <summary>
        /// Indicates an unsigned integer number with 8 bits.
        /// </summary>
        UInt8,

        /// <summary>
        /// Indicates an unsigned integer number with 16 bits.
        /// </summary>
        UInt16,

        /// <summary>
        /// Indicates an unsigned integer number with 32 bits.
        /// </summary>
        UInt32,

        /// <summary>
        /// Indicates an integer number with 8 bits.
        /// </summary>
        Int8,

        /// <summary>
        /// Indicates an integer number with 16 bits.
        /// </summary>
        Int16,

        /// <summary>
        /// Indicates an integer number with 32 bits.
        /// </summary>
        Int32,

        /// <summary>
        /// Indicates a floating-point number with 32 bits.
        /// </summary>
        Float32,

        /// <summary>
        /// Indicates a floating-point number with 64 bits.
        /// </summary>
        Float64,

        /// <summary>
        /// Indicates a complex number with 64 bits.
        /// </summary>
        Complex64,

        /// <summary>
        /// Indicates a complex number with 128 bits.
        /// </summary>
        Complex128,

        /// <summary>
        /// Indicates that the type is not defined.
        /// </summary>
        Undefined
    }
}
