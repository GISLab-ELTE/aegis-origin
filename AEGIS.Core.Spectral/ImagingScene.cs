/// <copyright file="SpectralImagingData.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a type containg imaging information of spectral data.
    /// </summary>
    public class ImagingScene
    {
        #region Private fields

        /// <summary>
        /// The band data.
        /// </summary>
        private List<ImagingSceneBand> _bandData;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the imaging device.
        /// </summary>
        /// <value>The imaging device.</value>
        public ImagingDevice Device { get; private set; }

        /// <summary>
        /// Gets the time of imaging.
        /// </summary>
        /// <value>The time of imaging.</value>
        public DateTime ImagingTime { get; private set; }

        /// <summary>
        /// Gets the location of imaging.
        /// </summary>
        /// <value>The geographic coordinate of imaging.</value>
        public GeoCoordinate Location { get; private set; }

        /// <summary>
        /// Gets the incidence angle.
        /// </summary>
        /// <value>The incidence angle.</value>
        public Double IncidenceAngle { get; private set; }

        /// <summary>
        /// Gets the viewing angle.
        /// </summary>
        /// <value>The viewing angle.</value>
        public Double ViewingAngle { get; private set; }

        /// <summary>
        /// Gets azimuth of the sun.
        /// </summary>
        /// <value>The azimuth of the sun.</value>
        public Double SunAzimuth { get; private set; }

        /// <summary>
        /// Gets the elevation the sun.
        /// </summary>
        /// <value>The elevation of the sun.</value>
        public Double SunElevation { get; private set; }

        /// <summary>
        /// Gets the band data.
        /// </summary>
        /// <value>The read-only array contaning the band data.</value>
        public IList<ImagingSceneBand> BandData { get { return _bandData.AsReadOnly(); } }

        /// <summary>
        /// Gets the spectral ranges of the bands.
        /// </summary>
        /// <value>The read-only list containing the spectral range of each band.</value>
        public IList<SpectralRange> SpectralRanges { get { return Array.AsReadOnly(_bandData.Select(bandData => bandData.SpectralRange).ToArray()); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagingScene" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="imagingTime">The imaging time.</param>
        /// <param name="location">The location.</param>
        /// <param name="incidenceAngle">The incidence angle.</param>
        /// <param name="viewingAngle">The viewing angle.</param>
        /// <param name="sunAzimuth">The sun azimuth.</param>
        /// <param name="sunElevation">The sun elevation.</param>
        /// <param name="bandData">The band data.</param>
        public ImagingScene(ImagingDevice device, DateTime imagingTime, GeoCoordinate location, Double incidenceAngle, Double viewingAngle, Double sunAzimuth, Double sunElevation, params ImagingSceneBand[] bandData)
        {
            Device = device;
            ImagingTime = imagingTime;
            Location = location;
            IncidenceAngle = incidenceAngle;
            ViewingAngle = viewingAngle;
            SunAzimuth = sunAzimuth;
            SunElevation = sunElevation;
            _bandData = new List<ImagingSceneBand>(bandData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagingScene" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="imagingTime">The imaging time.</param>
        /// <param name="location">The location.</param>
        /// <param name="incidenceAngle">The incidence angle.</param>
        /// <param name="viewingAngle">The viewing angle.</param>
        /// <param name="sunAzimuth">The sun azimuth.</param>
        /// <param name="sunElevation">The sun elevation.</param>
        /// <param name="bandData">The band data.</param>
        public ImagingScene(ImagingDevice device, DateTime imagingTime, GeoCoordinate location, Double incidenceAngle, Double viewingAngle, Double sunAzimuth, Double sunElevation, IList<ImagingSceneBand> bandData)
        {
            Device = device;
            ImagingTime = imagingTime;
            Location = location;
            IncidenceAngle = incidenceAngle;
            ViewingAngle = viewingAngle;
            SunAzimuth = sunAzimuth;
            SunElevation = sunElevation;
            _bandData = new List<ImagingSceneBand>(bandData);
        }

        #endregion
    }
}
