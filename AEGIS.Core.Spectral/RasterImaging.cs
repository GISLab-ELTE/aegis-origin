/// <copyright file="RasterImaging.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
    /// Represents a type containg imaging data of raster images.
    /// </summary>
    public class RasterImaging
    {
        #region Private fields

        /// <summary>
        /// The array of band data.
        /// </summary>
        private RasterImagingBand[] _bands;

        /// <summary>
        /// The list of spectral domains for each band.
        /// </summary>
        private IList<SpectralDomain> _spectralDomains;
        
        /// <summary>
        /// The list of spectral ranges for each band.
        /// </summary>
        private IList<SpectralRange> _spectralRanges;

        /// <summary>
        /// The dictionary of additional parameters.
        /// </summary>
        private Dictionary<String, Object> _additionalParameters;

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
        public DateTime Time { get; private set; }

        /// <summary>
        /// Gets the location of the imaging device.
        /// </summary>
        /// <value>The geographic coordinate of the imaging device.</value>
        public GeoCoordinate DeviceLocation { get; private set; }

        /// <summary>
        /// Gets the geographic coordinates of the image.
        /// </summary>
        /// <value>The geographic coordinates of the image in clockwise order.</value>
        public GeoCoordinate[] ImageLocation { get; private set; }

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
        public IList<RasterImagingBand> Bands { get { return _bands.AsReadOnly(); } }

        /// <summary>
        /// Gets the spectral domains of the bands.
        /// </summary>
        /// <value>The read-only list containing the spectral domain of each band.</value>
        public IList<SpectralDomain> SpectralDomains
        {
            get
            {
                if (_spectralDomains == null)
                    _spectralDomains = _bands.Select(bandData => bandData.SpectralDomain).ToArray().AsReadOnly();

                return _spectralDomains;
            }
        }

        /// <summary>
        /// Gets the spectral ranges of the bands.
        /// </summary>
        /// <value>The read-only list containing the spectral range of each band.</value>
        public IList<SpectralRange> SpectralRanges 
        { 
            get 
            {
                if (_spectralRanges == null)
                    _spectralRanges = _bands.Select(bandData => bandData.SpectralRange).ToArray().AsReadOnly();

                return _spectralRanges;
            } 
        }

        /// <summary>
        /// Gets or sets additional imaging properties.
        /// </summary>
        /// <param name="key">The key of the property.</param>
        /// <returns>The property value for the specified <paramref name="key" /> if it exists; otherwise, <c>null</c>.</returns>
        public Object this[String key]
        {
            get
            {
                if (key == null)
                    return null;

                Object value;
                if (!_additionalParameters.TryGetValue(key, out value))
                    return null;

                return value;
            }
            set
            {
                if (key == null)
                    return;

                _additionalParameters[key] = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterImaging" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="imagingTime">The imaging time.</param>
        /// <param name="deviceLocation">The location of the imaging device.</param>
        /// <param name="imageLocation">The geographic coordinates of the image.</param>
        /// <param name="incidenceAngle">The incidence angle.</param>
        /// <param name="viewingAngle">The viewing angle.</param>
        /// <param name="sunAzimuth">The sun azimuth.</param>
        /// <param name="sunElevation">The sun elevation.</param>
        /// <param name="bandData">The band data.</param>
        /// <exception cref="System.ArgumentException">The number of coordinates in the image location is not equal to 4.</exception>
        public RasterImaging(ImagingDevice device, DateTime imagingTime, GeoCoordinate deviceLocation, IList<GeoCoordinate> imageLocation, 
                             Double incidenceAngle, Double viewingAngle, Double sunAzimuth, Double sunElevation, params RasterImagingBand[] bandData)
        {
            if (imageLocation != null && imageLocation.Count != 4)
                throw new ArgumentException("The number of coordinates in the image location is not equal to 4.", "imageLocation");

            Device = device;
            Time = imagingTime;
            DeviceLocation = deviceLocation;
            ImageLocation = imageLocation.ToArray();
            IncidenceAngle = incidenceAngle;
            ViewingAngle = viewingAngle;
            SunAzimuth = sunAzimuth;
            SunElevation = sunElevation;
            _bands = bandData.ToArray();
            _additionalParameters = new Dictionary<String, Object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterImaging" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="imagingTime">The imaging time.</param>
        /// <param name="deviceLocation">The location of the imaging device.</param>
        /// <exception cref="System.ArgumentException">The number of coordinates in the image location is not equal to 4.</exception>
        /// <param name="incidenceAngle">The incidence angle.</param>
        /// <param name="viewingAngle">The viewing angle.</param>
        /// <param name="sunAzimuth">The sun azimuth.</param>
        /// <param name="sunElevation">The sun elevation.</param>
        /// <param name="bandData">The band data.</param>
        /// <exception cref="System.ArgumentException">The number of coordinates in the image location is not equal to 4.</exception>
        public RasterImaging(ImagingDevice device, DateTime imagingTime, GeoCoordinate deviceLocation, IList<GeoCoordinate> imageLocation, 
                             Double incidenceAngle, Double viewingAngle, Double sunAzimuth, Double sunElevation, IList<RasterImagingBand> bandData)
        {
            if (imageLocation != null && imageLocation.Count != 4)
                throw new ArgumentException("The number of coordinates in the image location is not equal to 4.", "imageLocation");

            Device = device;
            Time = imagingTime;
            DeviceLocation = deviceLocation;
            ImageLocation = imageLocation.ToArray();
            IncidenceAngle = incidenceAngle;
            ViewingAngle = viewingAngle;
            SunAzimuth = sunAzimuth;
            SunElevation = sunElevation;
            _bands = bandData.ToArray();
            _additionalParameters = new Dictionary<String, Object>();
        }

        #endregion        
    }
}
