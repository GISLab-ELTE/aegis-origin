// <copyright file="DimapMetafileReader.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Reference;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ELTE.AEGIS.IO.GeoTiff.Metafile
{
    /// <summary>
    /// Represents a type for reading Digital Image Map (Dimap) metafiles.
    /// </summary>
    /// <remarks>
    /// Dimap (Digital Image Map) is mainly a metadata format designed by Spot Image, Satellus and CNES (the French National Space Agency) to document digital imagemaps. It was designed taking into account the early experiences of GIS-Geospot and GIS-Image.
    /// Dimap metadata part allows to properly describe a dataset which contains geographic information representing a digital map. Namely, metadata describes in details, in a standard way, all the characteristics of the dataset. 
    /// The Dimap set of metadata is specifically tailored to image (raster) description, it also contains a few tags for vector types of data.
    /// See: http://www.spotimage.fr/dimap/spec/dimap.htm
    /// </remarks>
    public class DimapMetafileReader : GeoTiffMetafileReader
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
        protected override String DefaultFileName
        {
            get { return "metadata.dim"; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DimapMetafileReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
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
        public DimapMetafileReader(String path)
            : base(path)
        {
            _document = XDocument.Load(_stream);

            if (_document.Element("Dimap_Document") == null)
                throw new InvalidDataException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimapMetafileReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
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
        public DimapMetafileReader(Uri path)
            : base(path)
        {
            _document = XDocument.Load(_stream);

            if (_document.Element("Dimap_Document") == null)
                throw new InvalidDataException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DimapMetafileReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        public DimapMetafileReader(Stream stream)
            : base(stream)
        {
            _document = XDocument.Load(_stream);

            if (_document.Element("Dimap_Document") == null)
                throw new InvalidDataException();
        }

        #endregion

        #region Protected GeoTiffMetafileReader methods

        /// <summary>
        /// Reads the device information stored in the metafile stream.
        /// </summary>
        /// <returns>The device data.</returns>
        protected override ImagingDevice ReadDeviceInternal()
        {
            XElement sceneSourceElement = _document.Element("Dimap_Document").Element("Dataset_Sources").Element("Source_Information").Element("Scene_Source");

            // device information
            String mission = sceneSourceElement.Element("MISSION").Value;
            Int32 missionNumber = Int32.Parse(sceneSourceElement.Element("MISSION_INDEX").Value);
            String instrument = sceneSourceElement.Element("INSTRUMENT").Value;

            return ImagingDevices.FromName(mission + missionNumber + " " + instrument).FirstOrDefault();
        }

        /// <summary>
        /// Reads the imaging information stored in the metafile stream.
        /// </summary>
        /// <returns>The imaging data.</returns>
        protected override RasterImaging ReadImagingInternal()
        {
            // read the device data.
            ImagingDevice device = ReadDeviceData();

            XElement sceneSourceElement = _document.Element("Dimap_Document").Element("Dataset_Sources").Element("Source_Information").Element("Scene_Source");

            // time
            DateTime imagingTime = DateTime.Parse(sceneSourceElement.Element("IMAGING_DATE").Value + " " + sceneSourceElement.Element("IMAGING_TIME").Value, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);

            // view
            Double incidenceAngle = Double.Parse(sceneSourceElement.Element("INCIDENCE_ANGLE").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double viewingAngle = Double.Parse(sceneSourceElement.Element("VIEWING_ANGLE").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double sunAzimuth = Double.Parse(sceneSourceElement.Element("SUN_AZIMUTH").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double sunElevation = Double.Parse(sceneSourceElement.Element("SUN_ELEVATION").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);

            // device location
            XElement ephemeris = _document.Element("Dimap_Document").Element("Data_Strip").Element("Ephemeris");

            Double altitude = Double.Parse(ephemeris.Element("SATELLITE_ALTITUDE").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double latitude = Double.Parse(ephemeris.Element("NADIR_LAT").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
            Double longitude = Double.Parse(ephemeris.Element("NADIR_LON").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);

            GeoCoordinate deviceLocation = new GeoCoordinate(Angle.FromDegree(latitude), Angle.FromDegree(longitude), Length.FromMetre(altitude));

            // image location
            List<GeoCoordinate> imageLocation = new List<GeoCoordinate>(4);

            foreach (XElement vertexElement in _document.Element("Dimap_Document").Element("Dataset_Frame").Elements("Vertex"))
            {
                GeoCoordinate coordinate = new GeoCoordinate(Double.Parse(vertexElement.Element("FRAME_LAT").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat),
                                                             Double.Parse(vertexElement.Element("FRAME_LON").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat));
                imageLocation.Add(coordinate);
            }

            // band parameters
            List<RasterImagingBand> bandData = new List<RasterImagingBand>();

            foreach (XElement bandElement in _document.Element("Dimap_Document").Element("Image_Interpretation").Elements("Spectral_Band_Info"))
            {
                String bandIndex = bandElement.Element("BAND_INDEX").Value;
                Double physicalGain = Double.Parse(bandElement.Element("PHYSICAL_GAIN").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
                Double physicalBias = Double.Parse(bandElement.Element("PHYSICAL_BIAS").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);

                // read the solar irradiance
                XElement solarIrradianceElement = _document.Element("Dimap_Document").Element("Data_Strip").Element("Sensor_Calibration").Element("Solar_Irradiance").Elements("Band_Solar_Irradiance").FirstOrDefault(elements => elements.Element("BAND_INDEX").Value.Equals(bandIndex));

                Double solarIrradiance = 0; 

                if (solarIrradianceElement != null)
                    solarIrradiance = Double.Parse(solarIrradianceElement.Element("SOLAR_IRRADIANCE_VALUE").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);

                // match the device band data
                ImagingDeviceBand deviceBand = null;
                if (device != null)
                    deviceBand = device.Bands.FirstOrDefault(band => band.Description.Contains(bandElement.Element("BAND_DESCRIPTION").Value));

                if (deviceBand != null)
                    bandData.Add(new RasterImagingBand(deviceBand.Description, physicalGain, physicalBias, solarIrradiance, deviceBand.SpectralDomain, deviceBand.SpectralRange));
                else // if no match is found
                    bandData.Add(new RasterImagingBand(bandElement.Element("BAND_DESCRIPTION").Value, physicalGain, physicalBias, solarIrradiance, SpectralDomain.Undefined, null));
            }

            return new RasterImaging(device, imagingTime, deviceLocation, imageLocation, incidenceAngle, viewingAngle, sunAzimuth, sunElevation, bandData);
        }

        /// <summary>
        /// Reads the raster mapping stored in the metafile stream.
        /// </summary>
        /// <returns>The raster mapping.</returns>
        protected override RasterMapper ReadMappingInternal()
        {
            List<RasterCoordinate> coordinates = new List<RasterCoordinate>(4);

            foreach (XElement vertexElement in _document.Element("Dimap_Document").Element("Dataset_Frame").Elements("Vertex"))
            {
                RasterCoordinate coordinate = new RasterCoordinate(
                    Int32.Parse(vertexElement.Element("FRAME_ROW").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat),
                    Int32.Parse(vertexElement.Element("FRAME_COL").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat),
                    Double.Parse(vertexElement.Element("FRAME_LAT").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat),
                    Double.Parse(vertexElement.Element("FRAME_LON").Value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat)
                );

                coordinates.Add(coordinate);
            }

            return RasterMapper.FromCoordinates(RasterMapMode.ValueIsArea, coordinates);
        }

        /// <summary>
        /// Reads the reference system stored in the metafile stream.
        /// </summary>
        /// <returns>The reference system.</returns>
        protected override IReferenceSystem ReadReferenceSystemInternal()
        {
            IReferenceSystem referenceSystem = null;
            XElement referenceSystemElement = _document.Element("Dimap_Document").Element("Coordinate_Reference_System");

            if (referenceSystemElement == null)
                return null;

            // horizontal reference system based on type
            XElement codeElement = referenceSystemElement.Element("Horizontal_CS").Element("HORIZONTAL_CS_CODE");
            XElement nameElement = referenceSystemElement.Element("Horizontal_CS").Element("HORIZONTAL_CS_NAME");

            switch (referenceSystemElement.Element("Horizontal_CS").Element("HORIZONTAL_CS_TYPE").Value)
            {
                case "GEOGRAPHIC":
                    if (codeElement == null)
                        codeElement = referenceSystemElement.Element("Horizontal_CS").Element("Geographic_CS").Element("GEOGRAPHIC_CS_CODE");
                    if (nameElement == null)
                        nameElement = referenceSystemElement.Element("Horizontal_CS").Element("Geographic_CS").Element("GEOGRAPHIC_CS_NAME");

                    if (codeElement != null)
                        referenceSystem = Geographic2DCoordinateReferenceSystems.FromIdentifier(codeElement.Value).FirstOrDefault();
                    if (referenceSystem == null && nameElement != null)
                        referenceSystem = Geographic2DCoordinateReferenceSystems.FromName(nameElement.Value).FirstOrDefault();

                    // TODO: process custom datums
                    break;

                case "PROJECTED":
                    if (codeElement != null)
                        referenceSystem = ProjectedCoordinateReferenceSystems.FromIdentifier(codeElement.Value).FirstOrDefault();
                    if (referenceSystem == null && nameElement != null)
                        referenceSystem = ProjectedCoordinateReferenceSystems.FromName(nameElement.Value).FirstOrDefault();

                    // TODO: process custom projections
                    break;
                // TODO: process other horizontal reference system information
            }

            // TODO: process vertical reference system information

            return referenceSystem;
        }

        #endregion
    }
}
