/// <copyright file="RasterMasking.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Raster.Mask
{
    /// <summary>
    /// Defines extension methods for creating raster masks.
    /// </summary>
    public static class RasterMasking
    {
        /// <summary>
        /// Apply a mask to the raster.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <returns>The raster masked by the specified region.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting row index is less than 0.
        /// or
        /// The starting row index is equal to or greater than the number of rows in the source.
        /// or
        /// The starting column index is less than 0.
        /// or
        /// The starting column index is equal to or greater than the number of columns in the source.
        /// or
        /// The starting row index and the number of rows is greater than the number of rows in the source.
        /// or
        /// The starting columns index and the number of columns is greater than the number of rows in the source.
        /// </exception>
        public static IRaster ApplyMask(this IRaster raster, Int32 rowIndex, Int32 columnIndex, Int32 numberOfRows, Int32 numberOfColumns)
        {
            return new MaskedRaster(raster, rowIndex, columnIndex, numberOfRows, numberOfColumns);
        }
    }
}
