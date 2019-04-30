/// <copyright file="RasterMapMode.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines modes used by the raster mapper.
    /// </summary>
    public enum RasterMapMode
    {
        /// <summary>
        /// Indicates that the geometry coordinate is mapped to the upper left corner of the pixel with the raster coordinate.
        /// </summary>
        ValueIsArea,

        /// <summary>
        /// Indicates that the geometry coordinate is mapped to the center of the pixel with the raster coordinate.
        /// </summary>
        ValueIsCoordinate
    }
}
