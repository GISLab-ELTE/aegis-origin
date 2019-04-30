/// <copyright file="SpectralOperationDomain.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Defines the domains of spectral operations.
    /// </summary>
    public enum SpectralOperationDomain
    {
        /// <summary>
        /// Operates within the raster band.
        /// </summary>
        Band = 1,

        /// <summary>
        /// Operates on distinct spectral values within the raster band.
        /// </summary>
        BandLocal = 3,

        /// <summary>
        /// Operates on distinct spectral values by using neighbor values within the raster band.
        /// </summary>
        BandFocal = 5,

        /// <summary>
        /// Operates on distinct parts of the raster band.
        /// </summary>
        BandZonal = 9,

        /// <summary>
        /// Operates on the entire raster band.
        /// </summary>
        BandGlobal = 17,

        /// <summary>
        /// Operates on distinct spectral values within the raster.
        /// </summary>
        Local = 2,

        /// <summary>
        /// Operates on distinct spectral values by using neighbor values within the raster.
        /// </summary>
        Focal = 4,

        /// <summary>
        /// Operates on distinct parts of the raster.
        /// </summary>
        Zonal = 8,

        /// <summary>
        /// Operates on the entire raster.
        /// </summary>
        Global = 16
    }
}
