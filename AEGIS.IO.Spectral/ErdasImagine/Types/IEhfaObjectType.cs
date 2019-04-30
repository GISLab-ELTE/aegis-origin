/// <copyright file="IEhfaObjectType.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an object type (a primitive type or a structure type) in the HFA file structure.
    /// </summary>
    public interface IEhfaObjectType
    {
        /// <summary>
        /// Gets the placement mode.
        /// </summary>
        /// <value>The data placement mode.</value>
        DataPlacement DataPlacement { get; }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        String ItemName { get; }

        /// <summary>
        /// Gets the type this object type contains.
        /// </summary>
        /// <value>The type what this object type contains.</value>
        ItemType ItemType { get; }
    }
}
