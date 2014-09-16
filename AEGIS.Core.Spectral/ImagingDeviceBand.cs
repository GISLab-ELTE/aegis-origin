/// <copyright file="ImagingDeviceBand.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a band of the spectral imaging device.
    /// </summary>
    public class ImagingDeviceBand
    {
        #region Public properties

        /// <summary>
        /// Gets the number of the band.
        /// </summary>
        /// <value>The number of the band.</value>
        public Int32 Number { get; private set; }

        /// <summary>
        /// Gets the description of the band.
        /// </summary>
        /// <value>The description of the band.</value>
        public String Description { get; private set; }

        /// <summary>
        /// Gets the spatial resolution in range direction.
        /// </summary>
        /// <value>The distance covered by a single spectral value in range direction.</value>
        public Length RangeResolution { get; private set; }

        /// <summary>
        /// Gets the spatial resolution in azimuth direction.
        /// </summary>
        /// <value>The distance covered by a single spectral value in azimuth direction.</value>
        public Length AzimuthResolution { get; private set; }

        /// <summary>
        /// Gets the swath width.
        /// </summary>
        /// <value>The distance covered by a scan.</value>
        public Length Swath { get; private set; }

        /// <summary>
        /// Gets the radiometric resolution.
        /// </summary>
        /// <value>The radiometric resolution.</value>
        public Int32 RadiometricResolution { get; private set; }

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
        /// Initializes a new instance of the <see cref="ImagingDeviceBand" /> class.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="description">The description.</param>
        /// <param name="spatialResolution">The spatial resolution.</param>
        /// <param name="swath">The swath width.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralDomain">The spectral domain of the band.</param>
        /// <param name="spectralRange">The spectral range of the band.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The spatial resolution is equal to or less than 0.
        /// or
        /// The swath width is less than the spatial resolution.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is grater than 64.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">The spectral range is null.</exception>
        public ImagingDeviceBand(Int32 number, String description, Length spatialResoltion, Length swath, Int32 radiometricResolution, SpectralDomain spectralDomain, SpectralRange spectralRange)
        {
            if (spatialResoltion <= Length.Zero)
                throw new ArgumentOutOfRangeException("spatialResoltion", "The spatial resolution is equal to or less than 0.");
            if (swath < spatialResoltion)
                throw new ArgumentOutOfRangeException("swath", "The swath width is less than the spatial resolution.");
            if (radiometricResolution < 1)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is less than 1.");
            if (radiometricResolution > 64)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is grater than 64.");
            if (spectralRange == null)
                throw new ArgumentNullException("spectralRange", "The spectral range is null.");

            Number = number;
            Description = description;
            RangeResolution = spatialResoltion;
            AzimuthResolution = spatialResoltion;
            Swath = swath;
            RadiometricResolution = radiometricResolution;
            SpectralDomain = spectralDomain;
            SpectralRange = spectralRange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagingDeviceBand" /> class.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="description">The description.</param>
        /// <param name="rangeResultion">The range resoltion.</param>
        /// <param name="azimuthResolution">The azimuth resolution.</param>
        /// <param name="swath">The swath width.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="spectralDomain">The spectral domain of the band.</param>
        /// <param name="spectralRange">The spectral range of the band.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The range resolution is equal to or less than 0.
        /// or
        /// The azimuth resolution is equal to or less than 0.
        /// or
        /// The swath width is less than the spatial resolution.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is grater than 64.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">The spectral range is null.</exception>
        public ImagingDeviceBand(Int32 number, String description, Length rangeResultion, Length azimuthResolution, Length swath, Int32 radiometricResolution, SpectralDomain spectralDomain, SpectralRange spectralRange)
        {
            if (rangeResultion <= Length.Zero)
                throw new ArgumentOutOfRangeException("rangeResultion", "The range resolution is equal to or less than 0.");
            if (azimuthResolution <= Length.Zero)
                throw new ArgumentOutOfRangeException("azimuthResolution", "The azimuth resolution is equal to or less than 0.");
            if (swath < rangeResultion)
                throw new ArgumentOutOfRangeException("swath", "The swath width is less than the range resolution.");
            if (radiometricResolution < 1)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is less than 1.");
            if (radiometricResolution > 64)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is grater than 64.");
            if (spectralRange == null)
                throw new ArgumentNullException("spectralRange", "The spectral range is null.");

            Number = number;
            Description = description;
            RangeResolution = rangeResultion;
            AzimuthResolution = azimuthResolution;
            Swath = swath;
            RadiometricResolution = radiometricResolution;
            SpectralDomain = spectralDomain;
            SpectralRange = spectralRange;
        }

        #endregion
    }
}
