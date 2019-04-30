/// <copyright file="EhfaPrimitiveType.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS.IO.ErdasImagine.Types
{
    /// <summary>
    /// Represents a primitive type in the HFA file structure.
    /// </summary>
    public class EhfaPrimitiveType : IEhfaObjectType
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EhfaPrimitiveType" /> class.
        /// </summary>
        /// <param name="itemName">The item name.</param>
        /// <param name="itemNumber">The item number.</param>
        /// <param name="itemType">The item type.</param>
        /// <param name="placement">The data placement mode.</param>
        public EhfaPrimitiveType(String itemName, Int32 itemNumber, ItemType itemType, DataPlacement placement)
        {
            ItemType = itemType;
            ItemName = itemName;
            ItemNumber = itemNumber;
            DataPlacement = placement;
            EnumItems = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EhfaPrimitiveType" /> class.
        /// </summary>
        /// <param name="itemName">The item name.</param>
        /// <param name="itemNumber">The item number.</param>
        /// <param name="placement">The data placement mode.</param>
        /// <param name="enumItems">The list of items in the enumeration.</param>
        public EhfaPrimitiveType(String itemName, Int32 itemNumber, DataPlacement placement, IReadOnlyList<String> enumItems)
        {
            ItemType = ItemType.Enum;
            ItemName = itemName;
            ItemNumber = itemNumber;
            DataPlacement = placement;
            EnumItems = enumItems;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the data placement mode.
        /// </summary>
        /// <value>The data placement mode.</value>
        public DataPlacement DataPlacement { get; private set; }

        /// <summary>
        /// Gets the list of items in case the type is an enumeration.
        /// </summary>
        /// <value>The list of items if the type is an enumeration, otherwise, <c>null</c>.</value>
        public IReadOnlyList<String> EnumItems { get; private set; }

        /// <summary>
        /// Gets the item name.
        /// </summary>
        /// <value>The item name.</value>
        public String ItemName { get; private set; }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        /// <value>The item number.</value>
        public Int32 ItemNumber { get; private set; }

        /// <summary>
        /// Gets the item type.
        /// </summary>
        /// <value>The type of the contained object.</value>
        public ItemType ItemType { get; private set; }

        #endregion
    }
}
