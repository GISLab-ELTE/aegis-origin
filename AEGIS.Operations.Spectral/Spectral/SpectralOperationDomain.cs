/// <copyright file="SpectralOperationDomain.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Defines the domains of spectral operations.
    /// </summary>
    public enum SpectralOperationDomain
    {
        /// <summary>
        /// Operates on distinct spectral values within the raster band.
        /// </summary>
        BandLocal,

        /// <summary>
        /// Operates on distinct spectral values by using neightbour values within the raster band.
        /// </summary>
        BandFocal,

        /// <summary>
        /// Operates on distinct parts of the raster band.
        /// </summary>
        BandZonal,

        /// <summary>
        /// Operates on the entire raster band.
        /// </summary>
        BandGlobal,

        /// <summary>
        /// Operates on distinct spectral values within the raster.
        /// </summary>
        Local,

        /// <summary>
        /// Operates on distinct spectral values by using neightbour values within the raster.
        /// </summary>
        Focal,

        /// <summary>
        /// Operates on distinct parts of the raster.
        /// </summary>
        Zonal,

        /// <summary>
        /// Operates on the entire raster.
        /// </summary>
        Global
    }
}
