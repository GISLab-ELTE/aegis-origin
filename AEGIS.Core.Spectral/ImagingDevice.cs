// <copyright file="ImagingDevice.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a device used for spectral imaging.
    /// </summary>
    public class ImagingDevice : IdentifiedObject
    {
        #region Private fields

        /// <summary>
        /// The array of bands.
        /// </summary>
        private ImagingDeviceBand[] _bands;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the mission.
        /// </summary>
        /// <value>The mission.</value>
        public String Mission { get; private set; }

        /// <summary>
        /// Gets the index of the mission.
        /// </summary>
        /// <value>The index of the mission.</value>
        public Int32 MissionNumber { get; private set; }

        /// <summary>
        /// Gets the recording sensor.
        /// </summary>
        /// <value>The recording sensor.</value>
        public String Sensor { get; private set; }

        /// <summary>
        /// The orbit description.
        /// </summary>
        /// <value>The description of device orbit.</value>
        public String Orbit { get; private set; }

        /// <summary>
        /// Gets the altitude.
        /// </summary>
        /// <value>The general altitude of the mission.</value>
        public Length Altitude { get; private set; }

        /// <summary>
        /// Gets the temporal resolution.
        /// </summary>
        /// <value>The time taken between imaging of the same geographic location.</value>
        public TimeSpan TemporalResolution { get; private set; }

        /// <summary>
        /// Gets the swath width.
        /// </summary>
        /// <value>The distance covered by a scan.</value>
        public Length Swath { get; private set; }

        /// <summary>
        /// Gets the band information.
        /// </summary>
        /// <value>The read-only list containing the band information.</value>
        public IList<ImagingDeviceBand> Bands { get { return Array.AsReadOnly(_bands); } }

        /// <summary>
        /// Gets the radiometric resolutions.
        /// </summary>
        /// <value>The read-only list containing the radiometric resolution for each band.</value>
        public IList<Int32> RadiometricResolutions { get { return Array.AsReadOnly(_bands.Select(band => band.RadiometricResolution).ToArray()); } }

        /// <summary>
        /// Gets the spectral ranges.
        /// </summary>
        /// <value>The read-only list containing the spectral range for each band.</value>
        public IList<SpectralRange> SpectralRanges { get { return Array.AsReadOnly(_bands.Select(band => band.SpectralRange).ToArray()); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagingDevice" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="mission">The mission.</param>
        /// <param name="missionNumber">The mission number.</param>
        /// <param name="sensor">The sensor.</param>
        /// <param name="orbit">The orbit description.</param>
        /// <param name="altitude">The altitude.</param>
        /// <param name="temporalResolution">The temporal resolution.</param>
        /// <param name="swath">The swath width.</param>
        /// <param name="bands">The bands.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The mission is null.
        /// or
        /// The sensor is null.
        /// or
        /// No bands are specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The mission is empty, or consists of only whitespace characters.
        /// or
        /// The sensor is empty, or consists of only whitespace characters.
        /// </exception>
        public ImagingDevice(String identifier, String mission, String sensor, String orbit, Length altitude, TimeSpan temporalResolution, Length swath, params ImagingDeviceBand[] bands)
            : this(identifier, mission, 0, sensor, null, null, orbit, altitude, temporalResolution, swath, bands)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagingDevice" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="mission">The mission.</param>
        /// <param name="missionNumber">The mission number.</param>
        /// <param name="sensor">The sensor.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="orbit">The orbit description.</param>
        /// <param name="altitude">The altitude.</param>
        /// <param name="temporalResolution">The temporal resolution.</param>
        /// <param name="swath">The swath width.</param>
        /// <param name="bands">The bands.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The mission is null.
        /// or
        /// The sensor is null.
        /// or
        /// No bands are specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The mission is empty, or consists of only whitespace characters.
        /// or
        /// The sensor is empty, or consists of only whitespace characters.
        /// </exception>
        public ImagingDevice(String identifier, String mission, Int32 missionNumber, String sensor, String remarks, String[] aliases, String orbit, Length altitude, TimeSpan temporalResolution, Length swath, params ImagingDeviceBand[] bands)
            : base(identifier, null, remarks, aliases)
        {
            if (mission == null)
                throw new ArgumentNullException("mission", "The mission is null.");
            if (sensor == null)
                throw new ArgumentNullException("sensor", "The sensor is null.");
            if (bands == null)
                throw new ArgumentNullException("bands", "No bands are specified.");

            if (String.IsNullOrWhiteSpace(mission))
                throw new ArgumentException("The mission is empty, or consists of only whitespace characters.", "mission");
            if (String.IsNullOrWhiteSpace(sensor))
                throw new ArgumentException("The sensor is empty, or consists of only whitespace characters.", "sensor");

            Name = mission + missionNumber + " " + sensor;
            Mission = mission;
            MissionNumber = missionNumber;
            Sensor = sensor;
            Orbit = orbit;
            Altitude = altitude;
            TemporalResolution = temporalResolution;
            Swath = swath;
            _bands = bands;
        }

        #endregion
    }
}
