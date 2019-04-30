/// <copyright file="SpectralDataOrder.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines possible orders for storing raster data in services.
    /// </summary>
    [Flags]
    public enum RasterDataOrder
    {
        /// <summary>
        /// Represents a service not supporting any order.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents a service with multiple bands of the same pixel following each other (a.k.a. band sequential).
        /// </summary>
        RowColumnBand = 1,

        /// <summary>
        /// Represents a service with the bands of the raster following each other (a.k.a. band interleaved by pixel).
        /// </summary>
        BandRowColumn = 2,

        /// <summary>
        /// Represents a service with multiple bands of the same row following each other (a.k.a. band interleaved by line).
        /// </summary>
        RowBandColumn = 4,

        /// <summary>
        /// Represents a service supporting all possible orders.
        /// </summary>
        Unspecified = 7
    }
}
