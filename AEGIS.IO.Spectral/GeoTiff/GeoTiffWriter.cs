// <copyright file="GeoTiffWriter.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.GeoTiff
{
    using GeoKeyDirectory = Dictionary<Int16, Object>;
    using TiffImageFileDirectory = Dictionary<UInt16, Object[]>;

    /// <summary>
    /// Represents a GeoTIFF file format writer.
    /// </summary>
    /// <remarks>
    /// GeoTIFF is a generic TIFF based interchange format for georeferenced raster imagery.
    /// </remarks>
    /// <author>Roberto Giachetta, Krisztián Fodor</author>
    [IdentifiedObjectInstance("AEGIS::610202", "Geotagged Image File Format")]
    public class GeoTiffWriter : TiffWriter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.GeoTiffWriter"/> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public GeoTiffWriter(String path)
            : base(path)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.GeoTiffWriter"/> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public GeoTiffWriter(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.GeoTiffWriter"/> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public GeoTiffWriter(Uri path)
            : base(path)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.GeoTiffWriter"/> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public GeoTiffWriter(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.GeoTiffWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream to be written.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        public GeoTiffWriter(Stream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiff.GeoTiffWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream to be written.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        public GeoTiffWriter(Stream stream, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, parameters)
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the Image File Directory of a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="compression">The compression.</param>
        /// <param name="format">The sample format.</param>
        /// <param name="startPosition">The starting position of the raster content within the stream.</param>
        /// <param name="endPosition">The ending position of the raster content within the stream.</param>
        /// <returns>The computed Image File Directory.</returns>
        protected override TiffImageFileDirectory ComputeImageFileDirectory(ISpectralGeometry geometry, TiffCompression compression, TiffSampleFormat format)
        {
            TiffImageFileDirectory imageFileDirectory = base.ComputeImageFileDirectory(geometry, compression, format);

            CoordinateReferenceSystem referenceSystem = geometry.ReferenceSystem as CoordinateReferenceSystem;

            GeoKeyDirectory geoKeyDirectory = new GeoKeyDirectory();

            AddGeoKey(geoKeyDirectory, GeoKey.Citation, geometry.Metadata, "GeoTIFF::GeoCitation");
            AddGeoKey(geoKeyDirectory, GeoKey.GeodeticCoordinateReferenceSystemCitation, geometry.Metadata, "GeoTIFF::GeodeticCoordinateReferenceSystemCitation");
            AddGeoKey(geoKeyDirectory, GeoKey.ProjectedCoordinateReferenceSystemCitation, geometry.Metadata, "GeoTIFF::ProjectedCoordinateReferenceSystemCitation");

            if (geometry.Raster.Mapper != null) // if mapper is available
            {
                geoKeyDirectory.Add(GeoKey.RasterType, (Int16)((geometry.Raster.Mapper.Mode == RasterMapMode.ValueIsArea) ? 1 : 2));

                imageFileDirectory.Add(TiffTag.ModelTiepointTag, new Object[] { 0.0, 0.0, 0.0, geometry.Raster.Mapper.Translation.X, geometry.Raster.Mapper.Translation.Y, 0.0 });
                imageFileDirectory.Add(TiffTag.ModelPixelScaleTag, new Object[] { geometry.Raster.Mapper.ColumnSize, geometry.Raster.Mapper.RowSize, 1.0 });
            }

            if (referenceSystem != null) // if reference system is available (and supported)
            {
                switch (referenceSystem.Type)
                {
                    case ReferenceSystemType.Projected:
                        ComputeProjectedCoordinateReferenceSystem(geoKeyDirectory, referenceSystem as ProjectedCoordinateReferenceSystem);
                        break;
                    case ReferenceSystemType.Geographic2D:
                    case ReferenceSystemType.Geographic3D:
                        ComputeGeodeticCoordinateReferenceSystem(geoKeyDirectory, referenceSystem as GeographicCoordinateReferenceSystem);
                        break;
                    default: // other reference systems are not supported
                        return imageFileDirectory;
                }
            }

            WriteGeoKeyDirectory(imageFileDirectory, geoKeyDirectory);

            if (geometry.Imaging != null) // add imaging data
            {
                imageFileDirectory.Add(57410, new Object[] { geometry.Imaging.Device.Name });
                imageFileDirectory.Add(57411, new Object[] { geometry.Imaging.Time.ToString(CultureInfo.InvariantCulture.DateTimeFormat) });
                imageFileDirectory.Add(57412, new Object[] { geometry.Imaging.DeviceLocation.Latitude.BaseValue, geometry.Imaging.DeviceLocation.Longitude.BaseValue, geometry.Imaging.DeviceLocation.Height.BaseValue });
                imageFileDirectory.Add(57413, new Object[] { geometry.Imaging.IncidenceAngle, geometry.Imaging.ViewingAngle, geometry.Imaging.SunAzimuth, geometry.Imaging.SunElevation });
                imageFileDirectory.Add(57417, geometry.Imaging.Bands.Select(band => band.PhysicalGain).Cast<Object>().ToArray());
                imageFileDirectory.Add(57418, geometry.Imaging.Bands.Select(band => band.PhysicalBias).Cast<Object>().ToArray());
                imageFileDirectory.Add(57419, geometry.Imaging.Bands.Select(band => band.SolarIrradiance).Cast<Object>().ToArray());

                Object[] imageLocation = new Object[12];
                for (Int32 coordinateIndex = 0; coordinateIndex < geometry.Imaging.ImageLocation.Count; coordinateIndex++)
                {
                    imageLocation[3 * coordinateIndex] = geometry.Imaging.ImageLocation[coordinateIndex].Latitude.BaseValue;
                    imageLocation[3 * coordinateIndex + 1] = geometry.Imaging.ImageLocation[coordinateIndex].Longitude.BaseValue;
                    imageLocation[3 * coordinateIndex + 2] = geometry.Imaging.ImageLocation[coordinateIndex].Height.BaseValue;
                }
                imageFileDirectory.Add(57420, imageLocation);
            }

            return imageFileDirectory;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds the specified geo-key to the directory.
        /// </summary>
        /// <param name="directory">The geo-key directory.</param>
        /// <param name="key">The key.</param>
        /// <param name="metadata">The geometry metadata.</param>
        /// <param name="tagName">The name of the tag.</param>
        private void AddGeoKey(GeoKeyDirectory directory, Int16 key, IDictionary<String, Object> metadata, String tagName)
        {
            if (metadata.ContainsKey(tagName))
            {
                directory.Add(key, new Object[] { metadata[tagName] });
                metadata.Remove(tagName);
            }
        }

        /// <summary>
        /// Computes the projected coordinate reference system for the geo-key directory.
        /// </summary>
        /// <param name="geoKeyDirectory">The geo-key directory.</param>
        /// <param name="referenceSystem">The reference system.</param>
        private void ComputeProjectedCoordinateReferenceSystem(GeoKeyDirectory geoKeyDirectory, ProjectedCoordinateReferenceSystem referenceSystem)
        {
            geoKeyDirectory.Add(GeoKey.ModelType, (Int16)1);

            if (referenceSystem.Identifier != IdentifiedObject.UserDefinedIdentifier)
            {
                geoKeyDirectory.Add(GeoKey.ProjectedCoordinateReferenceSystemType, (Int16)referenceSystem.Code); // ProjectedCSTypeGeoKey
                return;
            }

            // user-defined reference system
            geoKeyDirectory.Add(GeoKey.ProjectedCoordinateReferenceSystemType, Int16.MaxValue);

            ComputeGeodeticCoordinateReferenceSystem(geoKeyDirectory, referenceSystem.Base);
            ComputeCoordinateProjection(geoKeyDirectory, referenceSystem.Projection);
        }

        /// <summary>
        /// Computes the Geodetic coordinate reference system for the geo-key directory.
        /// </summary>
        /// <param name="geoKeyDirectory">The geo-key directory.</param>
        /// <param name="referenceSystem">The reference system.</param>
        private void ComputeGeodeticCoordinateReferenceSystem(GeoKeyDirectory geoKeyDirectory, GeographicCoordinateReferenceSystem referenceSystem)
        {
            if (!geoKeyDirectory.ContainsKey(GeoKey.ModelType))
                geoKeyDirectory.Add(GeoKey.ModelType, (Int16)2);

            if (referenceSystem.Identifier != IdentifiedObject.UserDefinedIdentifier)
            {
                geoKeyDirectory.Add(GeoKey.GeodeticCoordinateReferenceSystemType, (Int16)referenceSystem.Code);
                return;
            }

            // user-defined reference system
            geoKeyDirectory.Add(GeoKey.GeodeticCoordinateReferenceSystemType, Int16.MaxValue);

            // TODO: process user-defined reference system
        }

        /// <summary>
        /// Computes the coordinate projection for the geo-key directory.
        /// </summary>
        /// <param name="geoKeyDirectory">The geo-key directory.</param>
        /// <param name="projection">The coordinate projection.</param>
        private void ComputeCoordinateProjection(GeoKeyDirectory geoKeyDirectory, CoordinateProjection projection)
        {
            if (projection.Code >= 10000 && projection.Code <= 19999)
            {
                geoKeyDirectory.Add(GeoKey.Projection, (Int16)projection.Code);
                return;
            }

            // user-defined projection
            geoKeyDirectory.Add(GeoKey.Projection, Int16.MaxValue);
            geoKeyDirectory.Add(GeoKey.ProjectionCoordinateTransformation, (Int16)projection.Method.Code);

            // TODO: process parameters
        }

        /// <summary>
        /// Writes the geo-key directory into the Image File Directory.
        /// </summary>
        /// <param name="imageFileDirectory">The Image File Directory.</param>
        /// <param name="geoKeyDirectory">The geo-key directory.</param>
        private void WriteGeoKeyDirectory(TiffImageFileDirectory imageFileDirectory, GeoKeyDirectory geoKeyDirectory)
        {
            if (geoKeyDirectory.Count == 0)
                return;

            Object[] geoKeyDirectoryTag = new Object[(geoKeyDirectory.Count + 1) * 4];
            List<Object> geoDoubleParamsTag = new List<Object>();
            List<Object> geoAsciiParamsTag = new List<Object>();

            // header data (version, revision, count)
            geoKeyDirectoryTag[0] = (Int16)1;
            geoKeyDirectoryTag[1] = (Int16)0;
            geoKeyDirectoryTag[2] = (Int16)0;
            geoKeyDirectoryTag[3] = (Int16)geoKeyDirectory.Count;

            // content
            Int32 byteIndex = 4;
            foreach (KeyValuePair<Int16, Object> geoData in geoKeyDirectory)
            {
                geoKeyDirectoryTag[byteIndex] = geoData.Key;
                geoKeyDirectoryTag[byteIndex + 1] = (UInt16)0;
                geoKeyDirectoryTag[byteIndex + 2] = (UInt16)1;

                if (geoData.Value is Int16)
                    geoKeyDirectoryTag[byteIndex + 3] = geoData.Value;

                if (geoData.Value is Double)
                    geoDoubleParamsTag.Add(geoData.Value);

                if (geoData.Value is String)
                    geoAsciiParamsTag.Add(geoData.Value);

                byteIndex += 4;
            }

            imageFileDirectory.Add(TiffTag.GeoKeyDirectoryTag, geoKeyDirectoryTag);

            if (geoDoubleParamsTag.Count > 0)
                imageFileDirectory.Add(TiffTag.GeoDoubleParamsTag, geoDoubleParamsTag.ToArray());
            if (geoAsciiParamsTag.Count > 0)
                imageFileDirectory.Add(TiffTag.GeoAsciiParamsTag, geoAsciiParamsTag.ToArray());
        }

        #endregion
    }
}
