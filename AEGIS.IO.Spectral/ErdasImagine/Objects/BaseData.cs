// <copyright file="BaseData.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using ELTE.AEGIS.IO.ErdasImagine.Types;

namespace ELTE.AEGIS.IO.ErdasImagine.Objects
{
    /// <summary>
    /// Represents a base-data HFA object. 
    /// </summary>
    /// <remarks>
    /// The base-data is represented as a generic two dimensional array of values. 
    /// It can store any data defined by <see cref="BaseData.Type" />.
    /// </remarks>
    /// <author>Tamas Nagy</author>
    public class BaseData
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseData" /> class.
        /// </summary>
        /// <param name="itemType">The item type.</param>
        /// <param name="values">The two-dimensional array of values.</param>
        public BaseData(ItemType itemType, Object[,] values)
        {
            Type = itemType;
            Values = values;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the type of the items contained by the <see cref="BaseData" />.
        /// </summary>
        /// <value>The type of the items contained by the <see cref="BaseData" />.</value>
        public ItemType Type { get; private set; }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The row count.</value>
        public Int32 RowCount { get { return Values.GetLength(0); } }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>The column count.</value>
        public Int32 ColumnCount { get { return Values.GetLength(1); } }

        /// <summary>
        /// Gets the two-dimensional array of values of the <see cref="BaseData" />.
        /// </summary>
        /// <value>The two-dimensional array of values of the <see cref="BaseData" />.</value>
        public Object[,] Values { get; private set; }

        #endregion

    }
}
