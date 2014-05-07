/// <copyright file="RasterMapper.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a type mapping raster coordinates to geometry coordinates.
    /// </summary>
    public struct RasterMapping
    {
        #region Private fields

        private readonly Int32 _rowIndex;
        private readonly Int32 _columnIndex;
        private readonly Coordinate _coordinate;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the row index.
        /// </summary>
        /// <value>The zero-based row index.</value>
        public Int32 RowIndex { get { return _rowIndex; } }

        /// <summary>
        /// Gets the column index.
        /// </summary>
        /// <value>The zero-based column index.</value>
        public Int32 ColumnIndex { get { return _columnIndex; } }

        /// <summary>
        /// Gets the coordinate.
        /// </summary>
        /// <value>The coordinate mapped to the specified row and column indices.</value>
        public Coordinate Coordinate { get { return _coordinate; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMapping" /> struct.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="coordinate">The coordinate.</param>
        public RasterMapping(Int32 rowIndex, Int32 columnIndex, Coordinate coordinate)
        {
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _coordinate = coordinate;
        }

        #endregion
    }
}
