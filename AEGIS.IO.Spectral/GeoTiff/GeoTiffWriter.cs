/// <copyright file="GeoTiffWriter.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
/// <author>Krisztián Fodor</author>

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
    using GeoKeyDirectory = Dictionary<Int16, Int16>; // short expression for the geo-key directory
    using TiffImageFileDirectory = Dictionary<UInt16, Object[]>; // short expression for the IFD table

    /// <summary>
    /// Represents a GeoTIFF file format writer.
    /// </summary>
    /// <remarks>
    /// GeoTIFF is a generic TIFF based interchange format for georeferenced raster imagery.
    /// </remarks>
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
        /// Computes the image file directory of a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="compression">The compression.</param>
        /// <param name="format">The sample format.</param>
        /// <param name="startPosition">The starting position of the raster content within the stream.</param>
        /// <param name="endPosition">The ending position of the raster content within the stream.</param>
        /// <returns>The computed image file directory.</returns>
        protected override TiffImageFileDirectory ComputeImageFileDirectory(ISpectralGeometry geometry, TiffCompression compression, TiffSampleFormat format)
        {
            TiffImageFileDirectory imageFileDirectory = base.ComputeImageFileDirectory(geometry, compression, format);

            CoordinateReferenceSystem referenceSystem = geometry.ReferenceSystem as CoordinateReferenceSystem;

            GeoKeyDirectory geoKeyDirectory = new GeoKeyDirectory();

            if (geometry.Raster.Mapper != null) // if mapper is available
            {
                geoKeyDirectory.Add(1025, (Int16)((geometry.Raster.Mapper.Mode == RasterMapMode.ValueIsArea) ? 1 : 2)); // GTRasterTypeGeoKey 

                imageFileDirectory.Add(33922, new Object[] { 0.0, 0.0, 0.0, geometry.Raster.Mapper.Translation.X, geometry.Raster.Mapper.Translation.Y, 0.0 }); // ModelTiepointTag
                imageFileDirectory.Add(33550, new Object[] { geometry.Raster.Mapper.ColumnSize, geometry.Raster.Mapper.RowSize, 1.0 }); // ModelPixelScaleTag 
            }

            if (referenceSystem != null) // if reference system is avaible (and supported)
            {
                switch (referenceSystem.Type)
                {
                    case ReferenceSystemType.Projected:
                        ComputeProjectedCoordinateReferenceSystem(geoKeyDirectory, referenceSystem as ProjectedCoordinateReferenceSystem);
                        break;
                    case ReferenceSystemType.Geographic2D:
                    case ReferenceSystemType.Geographic3D:
                        ComputeGeographicCoordinateReferenceSystem(geoKeyDirectory, referenceSystem as GeographicCoordinateReferenceSystem);
                        break;
                    default: // other reference systems are not supported
                        return imageFileDirectory;
                }
            }

            if (geoKeyDirectory.Count > 0) // if any geokeys have been written
                imageFileDirectory.Add(34735, ComputeWritableGeoData(geoKeyDirectory));
            
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
                for (Int32 coordinateIndex = 0; coordinateIndex < geometry.Imaging.ImageLocation.Length; coordinateIndex++)
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
        /// Computes the projected coordinate reference system for the geo-key directory.
        /// </summary>
        /// <param name="geoKeyDirectory">The geo-key directory.</param>
        /// <param name="referenceSystem">The reference system.</param>
        private void ComputeProjectedCoordinateReferenceSystem(GeoKeyDirectory geoKeyDirectory, ProjectedCoordinateReferenceSystem referenceSystem)
        {
            geoKeyDirectory.Add(1024, 1); // RasterTypeGeoKey

            if (referenceSystem.Identifier != IdentifiedObject.UserDefinedIdentifier)
            {
                geoKeyDirectory.Add(3072, (Int16)referenceSystem.Code); // ProjectedCSTypeGeoKey
                return;
            }

            // user-defined reference system
            geoKeyDirectory.Add(3072, Int16.MaxValue); 

            ComputeGeographicCoordinateReferenceSystem(geoKeyDirectory, referenceSystem.Base);
            ComputeCoordinateProjection(geoKeyDirectory, referenceSystem.Projection);
        }

        /// <summary>
        /// Computes the geographic coordinate reference system for the geo-key directory.
        /// </summary>
        /// <param name="geoKeyDirectory">The geo-key directory.</param>
        /// <param name="referenceSystem">The reference system.</param>
        private void ComputeGeographicCoordinateReferenceSystem(GeoKeyDirectory geoKeyDirectory, GeographicCoordinateReferenceSystem referenceSystem)
        {
            if (!geoKeyDirectory.ContainsKey(1024))
                geoKeyDirectory.Add(1024, 2); // RasterTypeGeoKey
            if (referenceSystem.Identifier != IdentifiedObject.UserDefinedIdentifier)
            {
                geoKeyDirectory.Add(2048, (Int16)referenceSystem.Code); // GeographicTypeGeoKey
                return;
            }

            // user-defined reference system
            geoKeyDirectory.Add(2048, Int16.MaxValue);

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
                geoKeyDirectory.Add(3074, (Int16)projection.Code);
                return;
            }

            // user-defined projection
            geoKeyDirectory.Add(3074, Int16.MaxValue);
            geoKeyDirectory.Add(3075, (Int16)projection.Method.Code);

            // TODO: process parameters
        }

        /// <summary>
        /// Constructs a short array form a GeoKeyDirectory.
        /// </summary>
        /// <param name="geoKeyDirectory">GeoKeyDirectory data</param>
        /// <returns>
        /// A GeoKeyDirectory in array format.
        /// </returns>
        private Object[] ComputeWritableGeoData(GeoKeyDirectory geoKeyDirectory)
        {
            Object[] result = new Object[(geoKeyDirectory.Count + 1) * 4];

            // Header data, first 3 is the revision
            result[0] = (Int16)1;
            result[1] = (Int16)0;
            result[2] = (Int16)0;
            result[3] = (Int16)geoKeyDirectory.Count;

            Int32 i = 4;
            foreach (KeyValuePair<Int16, Int16> geoData in geoKeyDirectory)
            {
                /* KeyEntry = { KeyID, TIFFTagLocation, Count, Value_Offset }

                "KeyID" gives the key-ID value of the Key (identical in function
                to TIFF tag ID, but completely independent of TIFF tag-space),
     
                "TIFFTagLocation" indicates which TIFF tag contains the value(s)
                of the Key: if TIFFTagLocation is 0, then the value is SHORT,
                and is contained in the "Value_Offset" entry. Otherwise, the type
                (format) of the value is implied by the TIFF-Type of the tag
                containing the value.
  
                "Count" indicates the number of values in this key.
   
                "Value_Offset" Value_Offset indicates the index-
                offset *into* the TagArray indicated by TIFFTagLocation, if
                it is nonzero. If TIFFTagLocation=0, then Value_Offset
                contains the actual (SHORT) value of the Key, and
                Count=1 is implied. Note that the offset is not a byte-offset,
                but rather an index based on the natural data type of the
                specified tag array.
                 */
                result[i] = geoData.Key;
                result[i + 1] = (UInt16)0;
                result[i + 2] = (UInt16)1;
                result[i + 3] = geoData.Value;

                i += 4;
            }

            return result;
        }

        #endregion
    }
}
