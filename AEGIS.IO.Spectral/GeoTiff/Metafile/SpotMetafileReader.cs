/// <copyright file="SpotMetafileReader.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ELTE.AEGIS.IO.GeoTiff.Metafile
{
    /// <summary>
    /// Represents a type for reading SPOT GeoTIFF metafiles.
    /// </summary>
    /// <remarks>
    /// SPOT metafiles are XML documents usually stored under the name <c>METADATA.DIM</c>. Thus each source file should be located in a separate folder.
    /// The metafile contains information about imaging, location, and satellite path.
    /// </remarks>
    public class SpotMetafileReader : GeoTiffMetafileReader
    {
        #region Private fields

        private XDocument _document;

        #endregion

        #region Protected GeoTiffMetafileReader properties

        /// <summary>
        /// Gets the default extension of the metafile.
        /// </summary>
        protected override String DefaultExtension
        {
            get { return "dim"; }
        }

        /// <summary>
        /// Gets the default file name of the metafile.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override String DefaultFileName
        {
            get { return "metadata.dim"; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffMetafileReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="option">The path option.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">The metafile does not exist.</exception>
        public SpotMetafileReader(String path, GeoTiffMetafilePathOption option)
            : base(path, option)
        {
            _document = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffMetafileReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="option">The path option.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">The metafile does not exist.</exception>
        public SpotMetafileReader(Uri path, GeoTiffMetafilePathOption option)
            : base(path, option)
        {
            _document = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotMetafileReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        public SpotMetafileReader(Stream stream)
            : base(stream)
        {
            _document = null;
        }

        #endregion

        #region Protected GeoTiffMetafileReader methods

        /// <summary>
        /// Reads the device information stored in the metafile stream.
        /// </summary>
        /// <returns>The device data.</returns>
        protected override ImagingDevice ReadDeviceFromStream()
        {
            if (_document == null)
                _document = XDocument.Load(_stream);

            XElement sceneSourceElement = _document.Element("Dimap_Document").Element("Dataset_Sources").Element("Source_Information").Element("Scene_Source");

            // device information
            String mission = sceneSourceElement.Element("MISSION").Value;
            Int32 missionIndex = Int32.Parse(sceneSourceElement.Element("MISSION_INDEX").Value);
            String instrument = sceneSourceElement.Element("INSTRUMENT").Value;
            Int32 instrumentIndex = Int32.Parse(sceneSourceElement.Element("INSTRUMENT_INDEX").Value);

            return ImagingDevices.FromName(mission + missionIndex + " " + instrument + instrumentIndex).FirstOrDefault();
        }

        /// <summary>
        /// Reads the imaging information stored in the metafile stream.
        /// </summary>
        /// <returns>The imaging data.</returns>
        protected override RasterImaging ReadImagingFromStream()
        {
            if (_document == null)
                _document = XDocument.Load(_stream);

            // read the device data.
            ImagingDevice device = ReadDeviceData();

            XElement sceneSourceElement = _document.Element("Dimap_Document").Element("Dataset_Sources").Element("Source_Information").Element("Scene_Source");

            // time
            DateTime imagingTime = DateTime.Parse(sceneSourceElement.Element("IMAGING_DATE").Value + " " + sceneSourceElement.Element("IMAGING_TIME").Value, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);

            // view and location
            Double incidenceAngle = Double.Parse(sceneSourceElement.Element("INCIDENCE_ANGLE").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double viewingAngle = Double.Parse(sceneSourceElement.Element("VIEWING_ANGLE").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double sunAzimuth = Double.Parse(sceneSourceElement.Element("SUN_AZIMUTH").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double sunElevation = Double.Parse(sceneSourceElement.Element("SUN_ELEVATION").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);

            XElement ephemeris = _document.Element("Dimap_Document").Element("Data_Strip").Element("Ephemeris");

            Double altitude = Double.Parse(ephemeris.Element("SATELLITE_ALTITUDE").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double latitude = Double.Parse(ephemeris.Element("NADIR_LAT").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double longitude = Double.Parse(ephemeris.Element("NADIR_LON").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);

            GeoCoordinate location = new GeoCoordinate(Angle.FromDegree(latitude), Angle.FromDegree(longitude), Length.FromMetre(altitude));

            // band parameters
            List<RasterImagingBand> bandData = new List<RasterImagingBand>();

            foreach (XElement bandElement in _document.Element("Dimap_Document").Element("Image_Interpretation").Elements("Spectral_Band_Info"))
            {
                Double physicalGain = Double.Parse(bandElement.Element("PHYSICAL_GAIN").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
                Double physicalBias = Double.Parse(bandElement.Element("PHYSICAL_BIAS").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);

                // match the device band data
                ImagingDeviceBand deviceBand = device.Bands.FirstOrDefault(band =>  band.Description.Contains(bandElement.Element("BAND_DESCRIPTION").Value));

                if (deviceBand != null)
                    bandData.Add(new RasterImagingBand(deviceBand.Description, physicalGain, physicalBias, deviceBand.SpectralDomain, deviceBand.SpectralRange));
                else // if no match is found
                    bandData.Add(new RasterImagingBand(bandElement.Element("BAND_DESCRIPTION").Value, physicalGain, physicalBias, SpectralDomain.Undefined, null));
            }

            return new RasterImaging(device, imagingTime, location, incidenceAngle, viewingAngle, sunAzimuth, sunElevation, bandData);
        }

        #endregion
    }
}
