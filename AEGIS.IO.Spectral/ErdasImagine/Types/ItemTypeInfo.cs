/// <copyright file="ItemTypeInfo.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamas Nagy</author>

using System;

namespace ELTE.AEGIS.IO.ErdasImagine.Types
{
    /// <summary>
    /// Defines methods for providing information on <see cref="ItemType" /> instances.
    /// </summary>
    public static class ItemTypeInfo
    {
        #region Public static methods

        /// <summary>
        /// Returns the number of bits of the specific item type.
        /// </summary>
        /// <param name="itemType">The item type.</param>
        /// <returns>The number of bits of the specific item type.</returns>
        public static UInt32 TypeBitCount(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.U1:
                    return 1;
                case ItemType.U2:
                    return 2;
                case ItemType.U4:
                    return 4;
                case ItemType.UChar:
                case ItemType.Char:
                    return 8;
                case ItemType.UShort:
                case ItemType.Short:
                    return 16;
                case ItemType.Enum:
                case ItemType.ULong:
                case ItemType.Long:
                case ItemType.Float:
                    return 32;
                case ItemType.Double:
                case ItemType.Complex:
                    return 64;
                case ItemType.DComplex:
                    return 128;
                default:
                    return 0;
            }

        }

        /// <summary>
        /// Returns the number of bytes of the specific item type.
        /// </summary>
        /// <param name="itemType">The item type.</param>
        /// <returns>The number of bytes of the specific item type.</returns>
        public static Int32 TypeByteCount(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.U1:
                case ItemType.U2:
                case ItemType.U4:
                case ItemType.UChar:
                case ItemType.Char:
                    return 1;
                case ItemType.Enum:
                case ItemType.UShort:
                case ItemType.Short:
                    return 2;
                case ItemType.Time:
                case ItemType.ULong:
                case ItemType.Long:
                case ItemType.Float:
                    return 4;
                case ItemType.Double:
                case ItemType.Complex:
                    return 8;
                case ItemType.DComplex:
                    return 16;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Returns the item type based on the item type index.
        /// </summary>
        /// <param name="itemTypeIndex">The item type index.</param>
        /// <returns>The item type.</returns>
        public static ItemType ItemTypeFromIndex(Int32 itemTypeIndex)
        {
            switch (itemTypeIndex)
            {
                case 0: return ItemType.U1;
                case 1: return ItemType.U2;
                case 2: return ItemType.U4;
                case 3: return ItemType.UChar;
                case 4: return ItemType.Char;
                case 5: return ItemType.UShort;
                case 6: return ItemType.Short;
                case 7: return ItemType.ULong;
                case 8: return ItemType.Long;
                case 9: return ItemType.Float;
                case 10: return ItemType.Double;
                case 11: return ItemType.Complex;
                case 12: return ItemType.DComplex;
                default: return ItemType.Undefined;
            }
        }

        #endregion
    }
}
