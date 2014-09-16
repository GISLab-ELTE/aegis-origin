/// <copyright file="RasterBandImaging.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
    /// Represents a type containg imaging data of a raster band.
    /// </summary>
    public class RasterImagingBand
    {
        #region Public properties

        /// <summary>
        /// Gets the band description.
        /// </summary>
        /// <value>The band description.</value>
        public String Description { get; private set; }

        /// <summary>
        /// Gets the physical gain.
        /// </summary>
        /// <value>The physical gain.</value>
        public Double PhysicalGain { get; private set; }

        /// <summary>
        /// Gets the physical bias.
        /// </summary>
        /// <value>The physical bias.</value>
        public Double PhysicalBias { get; private set; }

        /// <summary>
        /// Gets the spectral domain of the band.
        /// </summary>
        /// <value>Tje spectral domain of the band.</value>
        public SpectralDomain SpectralDomain { get; private set; }

        /// <summary>
        /// Gets the spectral range of the band.
        /// </summary>
        /// <value>The spectral range of the band.</value>
        public SpectralRange SpectralRange { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterImagingBand" /> class.
        /// </summary>
        /// <param name="description">The band description.</param>
        /// <param name="physicalGain">The physical gain of the band.</param>
        /// <param name="physicalBias">The physical bias of the band.</param>
        /// <param name="spectralDomain">The spectral domain of the band.</param>
        /// <param name="spectralRange">The spectral range of the band.</param>
        /// <exception cref="System.ArgumentNullException">The description is null.</exception>
        /// <exception cref="System.ArgumentException">The description is empty, or consists of only shitespace characters.</exception>
        public RasterImagingBand(String description, Double physicalGain, Double physicalBias, SpectralDomain spectralDomain, SpectralRange spectralRange)
        {
            if (description == null)
                throw new ArgumentNullException("description", "The description is null.");
            if (String.IsNullOrWhiteSpace(description))
                throw new ArgumentException("The description is empty, or consists of only shitespace characters.", "description");

            Description = description;
            PhysicalGain = physicalGain;
            PhysicalBias = physicalBias;
            SpectralDomain = spectralDomain;
            SpectralRange = spectralRange;
        }

        #endregion
    }
}
