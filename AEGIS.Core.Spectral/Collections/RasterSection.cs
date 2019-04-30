/// <copyright file="RasterSection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;

namespace ELTE.AEGIS.Collections
{
    /// <summary>
    /// Represents a raster tile.
    /// </summary>
    public class RasterSection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSection" /> class.
        /// </summary>
        public RasterSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSection" /> class.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        public RasterSection(Int32 rowIndex, Int32 columnIndex, Int32 numberOfRows, Int32 numberOfColumns)
            : this (0, rowIndex, columnIndex, numberOfRows, numberOfColumns)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSection" /> class.
        /// </summary>
        /// <param name="id">The part identifier.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        public RasterSection(Int32 id, Int32 rowIndex, Int32 columnIndex, Int32 numberOfRows, Int32 numberOfColumns)
        {
            Id = id;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the tile identifier.
        /// </summary>
        /// <value>
        /// The tile identifier.
        /// </value>
        public Int32 Id { get; set; }

        /// <summary>
        /// Gets thw index of the first row within the section.
        /// </summary>
        /// <value>The index of the first row within the section.</value>
        public Int32 RowIndex { get; set; }

        /// <summary>
        /// Gets thw index of the first row after the section.
        /// </summary>
        /// <value>The index of the first row after the section.</value>
        public Int32 RowEndIndex { get { return RowIndex + NumberOfRows; } }

        /// <summary>
        /// Gets the index of the first column within the section.
        /// </summary>
        /// <value>The index of the first column within the section.</value>
        public Int32 ColumnIndex { get; set; }

        /// <summary>
        /// Gets the index of the first column after the section.
        /// </summary>
        /// <value>The index of the first column after the section.</value>
        public Int32 ColumnEndIndex { get { return ColumnIndex + NumberOfColumns; } }

        /// <summary>
        /// Gets the number of rows in the section.
        /// </summary>
        /// <value>
        /// The number of rows in the section.
        /// </value>
        public Int32 NumberOfRows { get; set; }

        /// <summary>
        /// Gets the number of columns in the section.
        /// </summary>
        /// <value>The number of columns in the section.</value>
        public Int32 NumberOfColumns { get; set; }

        /// <summary>
        /// Gets the area of the raster.
        /// </summary>
        /// <value>The number of spectral values within the raster.</value>
        public Int32 Area { get { return NumberOfRows * NumberOfColumns; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether this section matches another.
        /// </summary>
        /// <param name="other">The other section.</param>
        /// <returns><c>true</c> if this instance has the same dimensions as <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        public Boolean IsMatching(RasterSection other)
        {
            return RowIndex == other.RowIndex &&
                   ColumnIndex == other.ColumnIndex &&
                   NumberOfRows == other.NumberOfRows &&
                   NumberOfColumns == other.NumberOfColumns;
        }

        /// <summary>
        /// Determines whether this section contains another.
        /// </summary>
        /// <param name="other">The other section.</param>
        /// <returns><c>true</c> if this instance contains <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        public Boolean Contains(RasterSection other)
        {
            return RowIndex <= other.RowIndex &&
                   ColumnIndex <= other.ColumnIndex &&
                   RowIndex + NumberOfRows >= other.RowIndex + other.NumberOfRows &&
                   ColumnIndex + NumberOfColumns >= other.ColumnIndex + other.NumberOfColumns;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns a <see cref="String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="String" /> that represents this instance.</returns>
        public override String ToString()
        {
            return String.Format("{0},{1},{2},{3},{4}", Id, RowIndex, ColumnIndex, NumberOfRows, NumberOfColumns);
        }

        #endregion
    }
}
