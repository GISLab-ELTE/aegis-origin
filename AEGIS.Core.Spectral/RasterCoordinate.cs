/// <copyright file="RasterCoordinate.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a model space coordinate of a specified raster space location.
    /// </summary>
    public class RasterCoordinate
    {
        #region Public properties

        /// <summary>
        /// Gets the row index.
        /// </summary>
        /// <value>The zero-based row index.</value>
        public Int32 RowIndex { get; private set; }

        /// <summary>
        /// Gets the column index.
        /// </summary>
        /// <value>The zero-based column index.</value>
        public Int32 ColumnIndex { get; private set; }

        /// <summary>
        /// Gets the model coordinate.
        /// </summary>
        /// <value>The model coordinate mapped to the specified row and column indices.</value>
        public Coordinate Coordinate { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterCoordinate" /> struct.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="x">The X component of the model coordinate.</param>
        /// <param name="y">The Y component of the model coordinate.</param>
        public RasterCoordinate(Int32 rowIndex, Int32 columnIndex, Double x, Double y)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Coordinate = new Coordinate(x, y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterCoordinate" /> struct.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="x">The X component of the model coordinate.</param>
        /// <param name="y">The Y component of the model coordinate.</param>
        /// <param name="z">The Z component of the model coordinate.</param>
        public RasterCoordinate(Int32 rowIndex, Int32 columnIndex, Double x, Double y, Double z)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Coordinate = new Coordinate(x, y, z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterCoordinate" /> struct.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <param name="coordinate">The model coordinate.</param>
        public RasterCoordinate(Int32 rowIndex, Int32 columnIndex, Coordinate coordinate)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Coordinate = coordinate;
        }

        #endregion
    }
}
