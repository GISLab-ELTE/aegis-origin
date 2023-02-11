// <copyright file="ItemType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.IO.ErdasImagine.Types
{
    /// <summary>
    /// Defines item types in the MIF data dictionary.
    /// </summary>
    /// <author>Tamas Nagy</author>
    public enum ItemType
    {
        /// <summary>
        /// Unsigned 1-bit integer (0 - 1).
        /// </summary>
        /// <remarks>
        /// When the data are read from a MIF file, they are automatically expanded to give one value per byte in memory.
        /// When they are written to the file, they are automatically compressed to place eight values into one output byte.
        /// </remarks>
        U1 = '1',

        /// <summary>
        /// Unsigned 2-bit integer (0 - 3).
        /// </summary>
        /// <remarks>
        /// When the data are read from a MIF file they are automatically expanded to give one value per byte in memory.
        /// When they are written to the file, they are automatically compressed to place four values into one output byte.
        /// </remarks>
        U2 = '2',

        /// <summary>
        /// Unsigned 4-bit integer (0 - 15).
        /// </summary>
        /// <remarks>
        /// When these data are read from a MIF file, they are automatically expanded to give one value per byte.
        /// When they are written to the file they are automatically compressed to place two values into one output byte.
        /// </remarks>
        U4 = '4',

        /// <summary>
        /// 8-bit unsigned integer.
        /// </summary>
        UChar = 'c',

        /// <summary>
        /// 8-bit signed integer.
        /// </summary>
        Char = 'C',

        /// <summary>
        /// Enumerated data type as a 16-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// The least significant byte is stored first.
        /// The list of strings associated with the type are defined in the data dictionary.
        /// </remarks>
        Enum = 'e',

        /// <summary>
        /// 16-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// The least significant byte is stored first.
        /// </remarks>
        UShort = 's',

        /// <summary>
        /// 16-bit two's-complement integer.
        /// </summary>
        /// <remarks>
        /// The least significant byte is stored first.
        /// </remarks>
        Short = 'S',

        /// <summary>
        /// 32-bit unsigned integer, which represents the number of seconds since 00:00:00 1 JAN 1970.
        /// </summary>
        /// <remarks>
        /// The least significant byte is stored first.
        /// </remarks>
        Time = 't',

        /// <summary>
        /// 32-bit unsigned integer.
        /// </summary>
        /// <remarks>
        /// The least significant byte is stored first.
        /// </remarks>
        ULong = 'l',

        /// <summary>
        /// 32-bit two's-complement integer value.
        /// </summary>
        /// <remarks>
        /// The least significant byte is stored first.
        /// </remarks>
        Long = 'L',

        /// <summary>
        /// Single precision floating point number.
        /// </summary>
        Float = 'f',

        /// <summary>
        /// Double precision floating point number.
        /// </summary>
        Double = 'd',

        /// <summary>
        /// Single precision complex number.
        /// </summary>
        Complex = 'm',

        /// <summary>
        /// Double precision floating point data.
        /// </summary>
        DComplex = 'M',

        /// <summary>
        /// A base-data is a generic two dimensional array of values.
        /// </summary>
        /// <remarks>
        /// It can store any of the types of data used by Erdas Imagine.
        /// It is a variable length object whose size is determined by the data type, the number of rows, and the number of columns.
        /// </remarks>
        BaseData = 'b',

        /// <summary>
        /// Previously defined object.
        /// </summary>
        /// <remarks>
        /// Indicates that the description of the following data has been previously defined.
        /// </summary>
        PreviouslyDefined = 'o',

        /// <summary>
        /// Defined object for this entry.
        /// </summary>
        /// <remarks>
        /// Indicates that the description of the following data follows.
        /// It is similar to using a structure definition within a structure definition.
        /// </summary>
        FollowedDefined = 'x',

        /// <summary>
        /// Structure type.
        /// </summary>
        Structure,

        /// <summary>
        /// Undefined type.
        /// </summary>
        Undefined
    }
}
