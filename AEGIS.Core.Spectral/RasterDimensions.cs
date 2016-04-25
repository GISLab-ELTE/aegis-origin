/// <copyright file="RasterDimensions.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
    /// Represents a type specifying the dimensions of a raster.
    /// </summary>
    public class RasterDimensions : IEquatable<RasterDimensions>
    {
        /// <summary>
        /// Gets the number of spectral bands.
        /// </summary>
        /// <value>The number of spectral bands contained in the raster.</value>
        public Int32 NumberOfBands { get; private set; }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        public Int32 NumberOfRows { get; private set; }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        public Int32 NumberOfColumns { get; private set; }

        /// <summary>
        /// Gets the radiometric resolution of the bands in the raster.
        /// </summary>
        /// <value>The radiometric resolution of the bands in the raster.</value>
        public Int32 RadiometricResolution { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterDimensions" /> class.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public RasterDimensions(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution)
        {
            if (numberOfBands < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBands), "The number of bands is less than 1.");
            if (numberOfRows < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfRows), "The number of rows is less than 0.");
            if (numberOfColumns < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfColumns), "The number of columns is less than 0.");
            if (radiometricResolution < 1)
                throw new ArgumentOutOfRangeException(nameof(radiometricResolution), "The radiometric resolution is less than 1.");
            if (radiometricResolution > 64)
                throw new ArgumentOutOfRangeException(nameof(radiometricResolution), "The radiometric resolution is greater than 64.");

            NumberOfBands = numberOfBands;
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            RadiometricResolution = radiometricResolution;
        }

        /// <summary>
        /// Determines whether the specified <see cref="RasterDimensions"/> instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="RasterDimensions"/> instance to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public Boolean Equals(RasterDimensions other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;

            return NumberOfBands == other.NumberOfBands && NumberOfRows == other.NumberOfRows && NumberOfColumns == other.NumberOfColumns && RadiometricResolution == other.RadiometricResolution;
        }
        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object  is equal to the current object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(obj, this))
                return true;

            RasterDimensions other = obj as RasterDimensions;

            return Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            return (NumberOfBands ^ 160482013) << 3 ^ (NumberOfRows ^ 160482197) << 2 ^ (NumberOfColumns ^ 160482359) << 1 ^ RadiometricResolution;
        }
    }
}
