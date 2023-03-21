// <copyright file="ISpectralGeometry.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines general behavior of geometry containing spectral data.
    /// </summary>
    public interface ISpectralGeometry : IGeometry
    {
        /// <summary>
        /// Gets the raster image contained within the geometry.
        /// </summary>
        /// <value>The raster image contained within the geometry.</value>
        IRaster Raster { get; }

        /// <summary>
        /// Gets the presentation data.
        /// </summary>
        /// <value>The presentation data of the raster image.</value>
        RasterPresentation Presentation { get; }

        /// <summary>
        /// Gets the imaging data.
        /// </summary>
        /// <value>The imaging data of the raster image.</value>
        RasterImaging Imaging { get; }
    }
}
