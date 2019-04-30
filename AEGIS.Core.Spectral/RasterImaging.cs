/// <copyright file="RasterImaging.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a type contains imaging data of raster images.
    /// </summary>
    public class RasterImaging
    {
        #region Private fields

        /// <summary>
        /// The array of band data.
        /// </summary>
        private List<RasterImagingBand> _bands;

        /// <summary>
        /// The list of spectral domains for each band.
        /// </summary>
        private SpectralDomain[] _spectralDomains;
        
        /// <summary>
        /// The list of spectral ranges for each band.
        /// </summary>
        private SpectralRange[] _spectralRanges;

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
        public IReadOnlyList<GeoCoordinate> ImageLocation { get; private set; }

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
        /// <value>The read-only array containing the band data.</value>
        public IReadOnlyList<RasterImagingBand> Bands { get { return _bands; } }

        /// <summary>
        /// Gets the spectral domains of the bands.
        /// </summary>
        /// <value>The read-only list containing the spectral domain of each band.</value>
        public IReadOnlyList<SpectralDomain> SpectralDomains
        {
            get
            {
                if (_spectralDomains == null)
                    _spectralDomains = _bands.Select(bandData => bandData.SpectralDomain).ToArray();

                return _spectralDomains;
            }
        }

        /// <summary>
        /// Gets the spectral ranges of the bands.
        /// </summary>
        /// <value>The read-only list containing the spectral range of each band.</value>
        public IReadOnlyList<SpectralRange> SpectralRanges 
        { 
            get
            {
                if (_spectralRanges == null)
                    _spectralRanges = _bands.Select(bandData => bandData.SpectralRange).ToArray();

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
        public RasterImaging(ImagingDevice device, DateTime imagingTime, GeoCoordinate deviceLocation, IEnumerable<GeoCoordinate> imageLocation, 
                             Double incidenceAngle, Double viewingAngle, Double sunAzimuth, Double sunElevation, params RasterImagingBand[] bandData)
        {
            if (imageLocation != null && imageLocation.Count() != 4)
                throw new ArgumentException("The number of coordinates in the image location is not equal to 4.", nameof(imageLocation));

            Device = device;
            Time = imagingTime;
            DeviceLocation = deviceLocation;
            ImageLocation = imageLocation.ToArray();
            IncidenceAngle = incidenceAngle;
            ViewingAngle = viewingAngle;
            SunAzimuth = sunAzimuth;
            SunElevation = sunElevation;
            _bands = bandData.ToList();
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
        public RasterImaging(ImagingDevice device, DateTime imagingTime, GeoCoordinate deviceLocation, IEnumerable<GeoCoordinate> imageLocation, 
                             Double incidenceAngle, Double viewingAngle, Double sunAzimuth, Double sunElevation, IEnumerable<RasterImagingBand> bandData)
        {
            if (imageLocation != null && imageLocation.Count() != 4)
                throw new ArgumentException("The number of coordinates in the image location is not equal to 4.", nameof(imageLocation));

            Device = device;
            Time = imagingTime;
            DeviceLocation = deviceLocation;
            ImageLocation = imageLocation.ToArray();
            IncidenceAngle = incidenceAngle;
            ViewingAngle = viewingAngle;
            SunAzimuth = sunAzimuth;
            SunElevation = sunElevation;
            _bands = bandData.ToList();
            _additionalParameters = new Dictionary<String, Object>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Inserts a band at the specified index.
        /// </summary>
        /// <param name="index">The band index.</param>
        /// <param name="band">The raster imaging band.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The band index is out of range.</exception>
        public void InsertBand(Int32 index, RasterImagingBand band)
        {
            if (index < 0 || index > _bands.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "The band index is out of range.");

            if (index == _bands.Count)
                _bands.Add(band);
            else
                _bands.Insert(index, band);

            _spectralDomains = null;
            _spectralRanges = null;
        }

        /// <summary>
        /// Removes a band at the specified index.
        /// </summary>
        /// <param name="index">The band index.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The band index is out of range.</exception>
        public void RemoveBand(Int32 index)
        {
            if (index < 0 || index >= _bands.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "The band index is out of range.");

            _bands.RemoveAt(index);
            _spectralDomains = null;
            _spectralRanges = null;
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Filters raster imaging data for the specified bands.
        /// </summary>
        /// <param name="imaging">The raster imaging.</param>
        /// <param name="bandIndices">The band indices.</param>
        /// <returns>The filtered raster imaging data.</returns>
        public static RasterImaging Filter(RasterImaging imaging, params Int32[] bandIndices)
        {
            if (imaging == null)
                throw new ArgumentNullException(nameof(imaging), "The raster imaging is null.");
            if (bandIndices == null)
                throw new ArgumentNullException(nameof(bandIndices), "The band index array is null.");
            if (bandIndices.Length == 0)
                throw new ArgumentException("No bands specified.", nameof(bandIndices));
            if (bandIndices.Any(bandIndex => bandIndex < 0 ||bandIndex >= imaging.Bands.Count))
                throw new ArgumentException("One or more bands are out of range.", nameof(bandIndices));

            return new RasterImaging(imaging.Device, imaging.Time, imaging.DeviceLocation, imaging.ImageLocation, imaging.IncidenceAngle, imaging.ViewingAngle, imaging.SunAzimuth, imaging.SunElevation, bandIndices.Select(bandIndex => imaging._bands[bandIndex]));
        }


        #endregion
    }
}
