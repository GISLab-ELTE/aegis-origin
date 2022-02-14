/// <copyright file="GeoTiffReader.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
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

    /// <summary>
    /// Represents a GeoTIFF file format reader.
    /// </summary>
    /// <remarks>
    /// GeoTIFF is a generic TIFF based interchange format for georeferenced raster imagery.
    /// </remarks>
    [IdentifiedObjectInstance("AEGIS::610202", "Geotagged Image File Format")]
    public class GeoTiffReader : TiffReader
    {
        #region Private types

        /// <summary>
        /// Defines the types of reference systems.
        /// </summary>
        private enum ReferenceSystemType
        {
            /// <summary>
            /// Projected.
            /// </summary>
            Projected = 1,

            /// <summary>
            /// Geodetic.
            /// </summary>
            Geodetic = 2,

            /// <summary>
            /// Geocentric.
            /// </summary>
            Geocentric = 3,

            /// <summary>
            /// User defined.
            /// </summary>
            UserDefined = 32767
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The GeoTIFF metafile reader.
        /// </summary>
        private GeoTiffMetafileReader _metafileReader;

        /// <summary>
        /// The GeoTIFF version.
        /// </summary>
        private String _geoTiffFormatVersion;

        /// <summary>
        /// The list of geokeys in the current image.
        /// </summary>
        private GeoKeyDirectory _currentGeoKeys;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        public GeoTiffReader(String path)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, null)
        {
            Boolean includeMetadata = ResolveParameter<Boolean>(SpectralGeometryStreamParameters.IncludeMetadata);

            if (includeMetadata)
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsGeoTiffFilePath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        public GeoTiffReader(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
            Boolean includeMetadata = ResolveParameter<Boolean>(SpectralGeometryStreamParameters.IncludeMetadata);

            if (includeMetadata)
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsGeoTiffFilePath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        public GeoTiffReader(Uri path)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, null)
        {
            Boolean includeMetadata = ResolveParameter<Boolean>(SpectralGeometryStreamParameters.IncludeMetadata);

            if (includeMetadata)
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsGeoTiffFilePath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// or
        /// Exception occurred during stream reading.
        /// </exception>
        public GeoTiffReader(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
            Boolean includeMetadata = ResolveParameter<Boolean>(SpectralGeometryStreamParameters.IncludeMetadata);

            if (includeMetadata)
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsGeoTiffFilePath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream reading.</exception>
        public GeoTiffReader(Stream stream)
            : base(stream, SpectralGeometryStreamFormats.GeoTiff, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream reading.</exception>
        public GeoTiffReader(Stream stream, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
        }

        #endregion

        #region Protected GeometryStreamReader methods

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        protected override IGeometry ApplyReadGeometry()
        {
            ComputeGeoKeys();

            return base.ApplyReadGeometry();
        }

        #endregion

        #region Protected TiffReader methods

        /// <summary>
        /// Computes the spectral imaging scene data of the geometry.
        /// </summary>
        /// <returns>The spectral imaging scene data of the geometry.</returns>
        protected override RasterImaging ComputeRasterImaging()
        {
            // DEPRECATED AEGIS SOLUTION
            // the imaging data may be contained within the tags
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(57410) &&
                _imageFileDirectories[_currentImageIndex].ContainsKey(57411) &&
                _imageFileDirectories[_currentImageIndex].ContainsKey(57412) &&
                _imageFileDirectories[_currentImageIndex].ContainsKey(57413) &&
                _imageFileDirectories[_currentImageIndex].ContainsKey(57417) &&
                _imageFileDirectories[_currentImageIndex].ContainsKey(57418) &&
                _imageFileDirectories[_currentImageIndex].ContainsKey(57419))
            {
                ImagingDevice device = ImagingDevices.FromName(_imageFileDirectories[_currentImageIndex][57410][0].ToString()).FirstOrDefault();

                if (device == null)
                    return null;

                DateTime imagingTime = DateTime.Parse(_imageFileDirectories[_currentImageIndex][57411][0].ToString(), CultureInfo.InvariantCulture.DateTimeFormat);
                GeoCoordinate deviceLocation = new GeoCoordinate(Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57412][0]), Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57412][1]), Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57412][2]));

                Double incidenceAngle = Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57413][0]);
                Double viewingAngle = Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57413][1]);
                Double sunAzimuth = Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57413][2]);
                Double sunElevation = Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57413][3]);

                GeoCoordinate[] imageLocation;
                if (_imageFileDirectories[_currentImageIndex].ContainsKey(57420) && _imageFileDirectories[_currentImageIndex][57420].Length == 12)
                {
                    Double[] imageLocationValues = _imageFileDirectories[_currentImageIndex][57420].Select(value => Convert.ToDouble(value)).ToArray();
                    imageLocation = new GeoCoordinate[] 
                    {
                        new GeoCoordinate(imageLocationValues[0], imageLocationValues[1], imageLocationValues[2]),
                        new GeoCoordinate(imageLocationValues[3], imageLocationValues[4], imageLocationValues[5]),
                        new GeoCoordinate(imageLocationValues[6], imageLocationValues[7], imageLocationValues[8]),
                        new GeoCoordinate(imageLocationValues[9], imageLocationValues[10], imageLocationValues[11])
                    };
                }
                else
                {
                    imageLocation = Enumerable.Repeat(GeoCoordinate.Undefined, 4).ToArray();
                }

                List<RasterImagingBand> bands = new List<RasterImagingBand>();

                for (Int32 bandIndex = 0; bandIndex < device.Bands.Count &&
                                          bandIndex < _imageFileDirectories[_currentImageIndex][57417].Length && 
                                          bandIndex < _imageFileDirectories[_currentImageIndex][57418].Length && 
                                          bandIndex < _imageFileDirectories[_currentImageIndex][57419].Length; bandIndex++)
                {
                    bands.Add(new RasterImagingBand(device.Bands[bandIndex].Description,
                                                    Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57417][bandIndex]),
                                                    Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57418][bandIndex]),
                                                    Convert.ToDouble(_imageFileDirectories[_currentImageIndex][57419][bandIndex]),
                                                    device.Bands[bandIndex].SpectralDomain,
                                                    device.Bands[bandIndex].SpectralRange));
                }

                if (device != null)
                {
                    return new RasterImaging(device, imagingTime, deviceLocation, imageLocation, incidenceAngle, viewingAngle, sunAzimuth, sunElevation, bands);
                }
            }

            // the imaging data may be contained in a metafile
            if (_metafileReader != null)
            {
                try
                {
                    return _metafileReader.ReadImaging();
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// Reads the metadata of the raster image.
        /// </summary>
        /// <returns>A dictionary containing the metadata of he raster image.</returns>
        protected override IDictionary<String, Object> ComputeMetadata()
        {
            IDictionary<String, Object> metadata = base.ComputeMetadata();

            AddMetadata(_currentGeoKeys, GeoKey.Citation, metadata, "GeoTIFF::GeoCitation");
            AddMetadata(_currentGeoKeys, GeoKey.GeodeticCoordinateReferenceSystemCitation, metadata, "GeoTIFF::GeodeticCoordinateReferenceSystemCitation");
            AddMetadata(_currentGeoKeys, GeoKey.ProjectedCoordinateReferenceSystemCitation, metadata, "GeoTIFF::ProjectedCoordinateReferenceSystemCitation");
            
            return metadata;
        }

        /// <summary>
        /// Adds the specified geo-key to the metadata.
        /// </summary>
        /// <param name="directory">The geo-key directory.</param>
        /// <param name="geoKey">The geo-key.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="metadataKey">The metadata key.</param>
        protected void AddMetadata(GeoKeyDirectory directory, Int16 geoKey, IDictionary<String, Object> metadata, String metadataKey)
        {
            if (directory == null || !metadata.ContainsKey(metadataKey))
                return;

            metadata[metadataKey] = directory[geoKey];
        }


        /// <summary>
        /// Computes the raster mapper.
        /// </summary>
        /// <returns>The raster mapper.</returns>
        protected override RasterMapper ComputeRasterMapper()
        {
            RasterMapper mapper = null;

            // try to read from file content
            try
            {
                mapper = ComputeRasterToModelSpaceMapping();
            }
            catch { }

            if (mapper != null)
                return mapper;

            // try to read from metafile
            if (_metafileReader != null)
            {
                try
                {
                    mapper = _metafileReader.ReadMapping();
                }
                catch { }
            }
            

            if (mapper != null)
                return mapper;

            // use default mapping
            return RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, Coordinate.Empty, new CoordinateVector(1, 1, 0));
        }
            
        /// <summary>
        /// Reads the mapping from model space to raster space.
        /// </summary>
        /// <returns>The mapping from model space to raster space.</returns>
        protected RasterMapper ComputeRasterToModelSpaceMapping()
        {
            if (!_currentGeoKeys.ContainsKey(GeoKey.RasterType))
                return null;

            RasterMapMode mode = Convert.ToInt16(_currentGeoKeys[GeoKey.RasterType]) == 1 ? RasterMapMode.ValueIsArea : RasterMapMode.ValueIsCoordinate;

            Double[] modelTiePointsArray = null;
            Double[] modelPixelScaleArray = null;
            Double[] modelTransformationArray = null;

            // gather information from tags
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.ModelTiepointTag))
            {
                modelTiePointsArray = _imageFileDirectories[_currentImageIndex][TiffTag.ModelTiepointTag].Select(value => Convert.ToDouble(value)).ToArray();
            }
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.ModelPixelScaleTag))
            {
                modelPixelScaleArray = _imageFileDirectories[_currentImageIndex][TiffTag.ModelPixelScaleTag].Select(value => Convert.ToDouble(value)).ToArray();
            }
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.ModelTransformationTag))
            {
                modelTransformationArray = _imageFileDirectories[_currentImageIndex][TiffTag.ModelTransformationTag].Select(value => Convert.ToDouble(value)).ToArray();
            }

            // for GeoTIFF 0.2, IntergraphMatrixTag (33920) may contain the transformation values
            if (modelTransformationArray == null && _imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.IntergraphMatrixTag))
            {
                modelTransformationArray = _imageFileDirectories[_currentImageIndex][TiffTag.IntergraphMatrixTag].Select(value => Convert.ToDouble(value)).ToArray();
            }

            // compute with model tie points
            if (modelTiePointsArray != null && (modelTiePointsArray.Length > 6 || (modelTiePointsArray.Length == 6 && modelPixelScaleArray.Length == 3)))
            {
                if (modelTiePointsArray.Length > 6) // transformation is specified by tie points
                {
                    RasterCoordinate[] coordinates = new RasterCoordinate[modelTiePointsArray.Length / 6];

                    for (Int32 i = 0; i < modelTiePointsArray.Length; i += 6)
                    {
                        coordinates[i / 6] = new RasterCoordinate(Convert.ToInt32(modelTiePointsArray[i]), Convert.ToInt32(modelTiePointsArray[i + 1]), new Coordinate(modelTiePointsArray[i + 3], modelTiePointsArray[i + 4], modelTiePointsArray[i + 5]));
                    }

                    return RasterMapper.FromCoordinates(mode, coordinates);
                }
                else // transformation is specified by single tie point and scale
                {
                    Coordinate rasterSpaceCoordinate = new Coordinate(modelTiePointsArray[0], modelTiePointsArray[1], modelTiePointsArray[2]);
                    Coordinate modelSpaceCoordinate = new Coordinate(modelTiePointsArray[3], modelTiePointsArray[4], modelTiePointsArray[5]);
                    Double scaleX = modelPixelScaleArray[0];
                    Double scaleY = -modelPixelScaleArray[1];
                    Double scaleZ = modelPixelScaleArray[2];

                    return RasterMapper.FromTransformation(mode, 
                                                           modelSpaceCoordinate.X - rasterSpaceCoordinate.X * scaleX,
                                                           modelSpaceCoordinate.Y + rasterSpaceCoordinate.Y * scaleY,
                                                           modelSpaceCoordinate.Z - rasterSpaceCoordinate.Z * scaleZ,
                                                           scaleX, scaleY, scaleZ);
                }
            }

            // compute with transformation values
            if (modelTransformationArray != null)
            {
                Matrix transformation = new Matrix(4, 4);
                for (Int32 i = 0; i < 4; i++)
                    for (Int32 j = 0; j < 4; j++)
                    {
                        transformation[i, j] = modelTransformationArray[i * 4 + j];
                    }
                return RasterMapper.FromTransformation(mode, transformation);
            }

            // nothing is available
            return null;
        }

        /// <summary>
        /// Reads the reference system of the raster image.
        /// </summary>
        /// <returns>The reference system of the raster image.</returns>
        protected override IReferenceSystem ComputeReferenceSystem()
        {
            if (_currentGeoKeys != null && _currentGeoKeys.ContainsKey(GeoKey.ModelType))
            {
                // reference system information is in the file
                try
                {
                    switch ((ReferenceSystemType)Convert.ToInt32(_currentGeoKeys[GeoKey.ModelType]))
                    {
                        case ReferenceSystemType.Projected:
                            return ComputeProjectedCoordinateReferenceSystem();
                        case ReferenceSystemType.Geodetic:
                            return ComputeGeodeticCoordinateReferenceSystem();
                        case ReferenceSystemType.Geocentric:
                            // currently not used
                            return null;
                        case ReferenceSystemType.UserDefined:
                            // currently not used
                            return null;
                    }
                }
                catch { }
            }

            // try to read from metafile
            if (_metafileReader != null)
            {
                try
                {
                    return _metafileReader.ReadReferenceSystem();
                }
                catch { }
            }

            return null;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the geokeys for the current raster.
        /// </summary>
        /// <exception cref="System.IO.InvalidDataException">Geo key data is in an invalid format.</exception>
        private void ComputeGeoKeys()
        {
            _currentGeoKeys = new GeoKeyDirectory();

            // there are no geokeys in the file
            if (!_imageFileDirectories[_currentImageIndex].ContainsKey(TiffTag.GeoKeyDirectoryTag))
                return;

            try
            {
                Object[] geokeyDirectory = _imageFileDirectories[_currentImageIndex][TiffTag.GeoKeyDirectoryTag];

                _geoTiffFormatVersion = geokeyDirectory[0] + "." + geokeyDirectory[1] + "." + geokeyDirectory[2];
                Int32 offset, count;

                for (Int16 i = 4; i < geokeyDirectory.Length; i += 4)
                {
                    switch (Convert.ToInt32(geokeyDirectory[1 + i]))
                    {
                        case 0:
                            _currentGeoKeys.Add(Convert.ToInt16(geokeyDirectory[i]), Convert.ToInt16(geokeyDirectory[3 + i]));
                            break;
                        case TiffTag.GeoDoubleParamsTag:
                            offset = Convert.ToInt32(geokeyDirectory[3 + i]);
                            Double doubleValue = Convert.ToDouble(_imageFileDirectories[_currentImageIndex][TiffTag.GeoDoubleParamsTag][offset]);
                            _currentGeoKeys.Add(Convert.ToInt16(geokeyDirectory[i]), doubleValue);
                            break;
                        case TiffTag.GeoAsciiParamsTag:
                            count = Convert.ToInt32(geokeyDirectory[2 + i]);
                            offset = Convert.ToInt32(geokeyDirectory[3 + i]);
                            // strings are concatenated to a single value using the | (pipe) character, so they are split, and the ending character is removed
                            _currentGeoKeys.Add(Convert.ToInt16(geokeyDirectory[i]), Convert.ToString(_imageFileDirectories[_currentImageIndex][TiffTag.GeoAsciiParamsTag][0]).Substring(offset, count - 1));
                            break;
                    }
                }
            }
            catch
            {
                throw new InvalidDataException("Geo key data is in an invalid format.");
            }
        }

        /// <summary>
        /// Computes the Geodetic coordinate reference system.
        /// </summary>
        /// <returns>The Geodetic coordinate reference system.</returns>
        /// <exception cref="System.IO.InvalidDataException">Geodetic coordinate reference system code is invalid.</exception>
        private GeographicCoordinateReferenceSystem ComputeGeodeticCoordinateReferenceSystem()
        {
            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.GeodeticCoordinateReferenceSystemType]);
            // EPSG geodetic coordinate reference system codes
            if (code < 32767)
            {
                GeographicCoordinateReferenceSystem referenceSystem = GeographicCoordinateReferenceSystems.FromIdentifier("EPSG::" + code).FirstOrDefault();

                if (referenceSystem == null)
                    return new GeographicCoordinateReferenceSystem("EPSG::" + code, "undefined", CoordinateSystems.EllipsoidalLatLonD, GeodeticDatums.WGS84, AreasOfUse.World);

                return referenceSystem;
            }

            // user-defined geodetic coordinate reference system
            if (code == Int16.MaxValue)
            {
                String citation = _currentGeoKeys[GeoKey.GeodeticCoordinateReferenceSystemCitation].ToString();
                GeodeticDatum datum = ComputeGeodeticDatum();
                UnitOfMeasurement angleUnit = ComputeAngularUnit();

                CoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystem.UserDefinedIdentifier, CoordinateSystem.UserDefinedName, 
                                                                         CoordinateSystemType.Ellipsoidal,
                                                                         CoordinateSystemAxisFactory.GeodeticLatitude(angleUnit),
                                                                         CoordinateSystemAxisFactory.GeodeticLongitude(angleUnit));

                return new GeographicCoordinateReferenceSystem(GeographicCoordinateReferenceSystem.UserDefinedIdentifier, GeographicCoordinateReferenceSystem.UserDefinedName, 
                                                               citation, null, null, coordinateSystem, datum, null);
            }

            throw new InvalidDataException("Geodetic coordinate reference system code is invalid.");
        }

        /// <summary>
        /// Computes the geodetic datum.
        /// </summary>
        /// <returns>The geodetic datum.</returns>
        /// <exception cref="System.IO.InvalidDataException">Geodetic Datum code is invalid.</exception>
        private GeodeticDatum ComputeGeodeticDatum()
        {
            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.GeodeticDatum]);
            // EPSG geodetic datum codes
            if (code >= 6000 && code <= 6999)
                return GeodeticDatums.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined geodetic datum
            if (code == Int16.MaxValue)
            {
                Ellipsoid ellipsoid = ComputeEllipsoid();
                Meridian primeMeridian = ComputePrimeMeridian();

                return new GeodeticDatum(GeodeticDatum.UserDefinedIdentifier, GeodeticDatum.UserDefinedName, null, null, null, ellipsoid, primeMeridian);
            }

            throw new InvalidDataException("Geodetic datum code is invalid.");
        }

        /// <summary>
        /// Computes the ellipsoid.
        /// </summary>
        /// <returns>The ellipsoid.</returns>
        /// <exception cref="System.IO.InvalidDataException">Prime meridian code is invalid.</exception>
        private Ellipsoid ComputeEllipsoid()
        {
            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.GeodeticEllipsoid]);
            // EPSG ellipsoid codes
            if (code >= 8000 && code <= 8999)
                return Ellipsoids.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined ellipsoid
            if (code == Int16.MaxValue)
            {
                Double semiMajorAxis = Convert.ToDouble(_currentGeoKeys[GeoKey.GeodeticSemiMajorAxis]);

                // either semi-minor axis or the inverse flattening is defined
                if (_currentGeoKeys.ContainsKey(GeoKey.GeodeticSemiMinorAxis))
                {
                    Double semiMinorAxis = Convert.ToDouble(_currentGeoKeys[2058]);
                    return Ellipsoid.FromSemiMinorAxis(Ellipsoid.UserDefinedIdentifier, Ellipsoid.UserDefinedName, semiMajorAxis, semiMinorAxis);
                }
                else
                {
                    Double inverseFlattening = Convert.ToDouble(_currentGeoKeys[GeoKey.GeodeticInverseFlattening]);
                    return Ellipsoid.FromInverseFlattening(Ellipsoid.UserDefinedIdentifier, Ellipsoid.UserDefinedName, semiMajorAxis, inverseFlattening);
                }
            }

            throw new InvalidDataException("Prime meridian code is invalid.");
        }

        /// <summary>
        /// Computes the prime meridian.
        /// </summary>
        /// <returns>The prime meridian.</returns>
        /// <exception cref="System.IO.InvalidDataException">Prime meridian code is invalid.</exception>
        private Meridian ComputePrimeMeridian()
        {
            if (!_currentGeoKeys.ContainsKey(GeoKey.GeodeticPrimeMeridian))
                return Meridians.Greenwich;

            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.GeodeticPrimeMeridian]);
            // EPSG prime meridian codes
            if (code >= 8000 && code <= 8999)
                return Meridians.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined prime meridian
            if (code == Int16.MaxValue)
            {
                Double longitude = Convert.ToDouble(_currentGeoKeys[GeoKey.GeodeticPrimeMeridianLongitude]);
                UnitOfMeasurement angleUnit = ComputeAngularUnit();

                return new Meridian(Meridian.UserDefinedIdentifier, Meridian.UserDefinedName, new Angle(longitude, angleUnit));
            }

            throw new InvalidDataException("Prime meridian code is invalid.");
        }

        /// <summary>
        /// Computes the linear unit.
        /// </summary>
        /// <returns>The linear unit of measurement.</returns>
        /// <exception cref="System.IO.InvalidDataException">Linear unit code is invalid.</exception>
        private UnitOfMeasurement ComputeLinearUnit()
        {
            if (!_currentGeoKeys.ContainsKey(GeoKey.GeodeticLinearUnits))
                return UnitsOfMeasurement.Metre;

            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.GeodeticLinearUnits]);
            // EPSG unit of measurement codes
            if (code >= 9000 && code <= 9099)
                return UnitsOfMeasurement.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined unit of measurement
            if (code == Int16.MaxValue)
            {
                Double unitSize = Convert.ToDouble(_currentGeoKeys[GeoKey.GeodeticLinearUnitSize]);

                return new UnitOfMeasurement(UnitOfMeasurement.UserDefinedIdentifier, UnitOfMeasurement.UserDefinedName, String.Empty, unitSize, UnitQuantityType.Length);
            }

            throw new InvalidDataException("Linear unit code is invalid.");
        }

        /// <summary>
        /// Computes the angular unit.
        /// </summary>
        /// <returns>The angular unit of measurement.</returns>
        /// <exception cref="System.IO.InvalidDataException">Angular unit code is invalid.</exception>
        private UnitOfMeasurement ComputeAngularUnit()
        {
            if (!_currentGeoKeys.ContainsKey(GeoKey.GeodeticAngularUnits))
                return UnitsOfMeasurement.Degree;

            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.GeodeticAngularUnits]);
            // EPSG unit of measurement codes
            if (code >= 9100 && code <= 9199)
                return UnitsOfMeasurement.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined unit of measurement
            if (code == Int16.MaxValue)
            {
                Double unitSize = Convert.ToDouble(_currentGeoKeys[GeoKey.GeodeticAngularUnitSize]);

                return new UnitOfMeasurement(UnitOfMeasurement.UserDefinedIdentifier, UnitOfMeasurement.UserDefinedName, String.Empty, unitSize, UnitQuantityType.Angle);
            }

            throw new InvalidDataException("Angular unit code is invalid.");
        }

        /// <summary>
        /// Computes the azimuth unit.
        /// </summary>
        /// <returns>The azimuth unit of measurement.</returns>
        /// <exception cref="System.IO.InvalidDataException">Azimuth unit code is invalid.</exception>
        private UnitOfMeasurement ComputeAzimuthUnit()
        {
            if (!_currentGeoKeys.ContainsKey(GeoKey.GeodeticAzimuthUnits))
                return UnitsOfMeasurement.Degree;

            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.GeodeticAzimuthUnits]);
            // EPSG unit of measurement codes
            if (code >= 9100 && code <= 9199)
                return UnitsOfMeasurement.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined unit of measurement
            if (code == Int16.MaxValue)
            {
                Double unitSize = Convert.ToDouble(_currentGeoKeys[GeoKey.GeodeticAngularUnitSize]);

                return new UnitOfMeasurement(UnitOfMeasurement.UserDefinedIdentifier, UnitOfMeasurement.UserDefinedName, String.Empty, unitSize, UnitQuantityType.Angle);
            }

            throw new InvalidDataException("Azimuth unit code is invalid.");
        }
      
        /// <summary>
        /// Computes the projected coordinate reference system.
        /// </summary>
        /// <returns>The projected coordinate reference system.</returns>
        /// <exception cref="System.IO.InvalidDataException">Projected coordinate reference system code is invalid.</exception>
        private ProjectedCoordinateReferenceSystem ComputeProjectedCoordinateReferenceSystem()
        {
            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.ProjectedCoordinateReferenceSystemType]);
            // EPSG Projected Coordinate Reference System codes
            if (code < 32767)
            {
                ProjectedCoordinateReferenceSystem referenceSystem = ProjectedCoordinateReferenceSystems.FromIdentifier("EPSG::" + code).FirstOrDefault();

                if (referenceSystem == null)
                    return new ProjectedCoordinateReferenceSystem("EPSG::" + code, "undefined", Geographic2DCoordinateReferenceSystems.WGS84, CoordinateSystems.CartesianENM, AreasOfUse.World, null);

                return referenceSystem;
            }
            // user-defined Projected Coordinate Reference System
            if (code == Int16.MaxValue)
            {
                GeographicCoordinateReferenceSystem baseReferenceSystem = ComputeGeodeticCoordinateReferenceSystem();
                CoordinateProjection projection = ComputeProjection(baseReferenceSystem.Datum.Ellipsoid);

                return new ProjectedCoordinateReferenceSystem(ProjectedCoordinateReferenceSystem.UserDefinedIdentifier, ProjectedCoordinateReferenceSystem.UserDefinedName, 
                                                              baseReferenceSystem, CoordinateSystems.CartesianENM, baseReferenceSystem.AreaOfUse, projection);
            }

            throw new InvalidDataException("Projected coordinate reference system code is invalid.");
        }

        /// <summary>
        /// Computes the coordinate operation method.
        /// </summary>
        /// <returns>The coordinate operation method.</returns>
        private CoordinateOperationMethod ComputeCoordinateOperationMethod()
        {
            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.ProjectionCoordinateTransformation]);

            switch (code)
            {
                case 1:
                    return CoordinateOperationMethods.TransverseMercatorProjection;
                case 2: // CT_TransvMercator_Modified_Alaska 
                    return null;
                case 3: // CT_ObliqueMercator 
                    return CoordinateOperationMethods.HotineObliqueMercatorAProjection; // TODO: parameter check
                case 4:
                    return CoordinateOperationMethods.LabordeObliqueMercatorProjection;
                case 5: // CT_SwissObliqueCylindrical 
                    return null;
                case 6: // CT_ObliqueMercator_Spherical 
                    return null;
                case 7:
                    return CoordinateOperationMethods.MercatorAProjection; // TODO: parameter check
                case 8:
                    return CoordinateOperationMethods.LambertConicConformal2SPProjection;
                case 9:
                    return CoordinateOperationMethods.LambertConicConformal1SPProjection;
                case 10:
                    return CoordinateOperationMethods.LambertAzimuthalEqualAreaProjection;
                case 11: // CT_AlbersEqualArea 
                    return CoordinateOperationMethods.AlbersEqualAreaProjection;
                case 12:
                    return CoordinateOperationMethods.LambertAzimuthalEqualAreaProjection;
                case 13: // CT_EquidistantConic
                    return null;
                case 14: // CT_Stereographic 
                    return CoordinateOperationMethods.ObliqueStereographicProjection;
                case 15:
                    return CoordinateOperationMethods.PolarStereographicAProjection; // TODO: parameter check
                case 16:
                    return CoordinateOperationMethods.ObliqueStereographicProjection;
                case 17: // CT_Equirectangular 
                    return null;
                case 18:
                    return CoordinateOperationMethods.CassiniSoldnerProjection;
                case 19:
                    return CoordinateOperationMethods.GnomonicProjection;
                case 20:
                    return CoordinateOperationMethods.WorldMillerCylindricalProjection;
                case 21: // CT_Orthographic 
                    return null;
                case 22: // CT_Polyconic 
                    return null;
                case 23: // CT_Robinson 
                    return null;
                case 24:
                    return CoordinateOperationMethods.SinusoidalProjection;
                case 25: // CT_VanDerGrinten 
                    return null;
                case 26: // CT_NewZealandMapGrid 
                    return null;
                case 27:
                    return CoordinateOperationMethods.TransverseMercatorSouthProjection;
                case 28:
                    return CoordinateOperationMethods.LambertCylindricalEqualAreaEllipsoidalProjection;
                case 32767:
                    return new CoordinateOperationMethod(CoordinateOperationMethod.UserDefinedIdentifier, CoordinateOperationMethod.UserDefinedName, 
                                                         false, ComputeCoordinateOperationParameters().Keys.ToArray());
            }

            throw new InvalidDataException("Coordinate operation method code is invalid.");
        }

        /// <summary>
        /// Computes the coordinate operation parameters.
        /// </summary>
        /// <returns>The coordinate operation parameters.</returns>
        private Dictionary<CoordinateOperationParameter, Object> ComputeCoordinateOperationParameters()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();

            UnitOfMeasurement angularUnit = ComputeAngularUnit();
            UnitOfMeasurement linearUnit = ComputeLinearUnit();

            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionStdParallel1))
            {
                parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionStdParallel1]), angularUnit));
                parameters.Add(CoordinateOperationParameters.LatitudeOfStandardParallel, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionStdParallel1]), angularUnit));
            }
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionStdParallel2))
                parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionStdParallel2]), angularUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionNaturalOriginLongitude))
            {
                parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionNaturalOriginLongitude]), angularUnit));
                parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionNaturalOriginLongitude]), angularUnit));
            }
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionNaturalOriginLatitude))
                parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionNaturalOriginLatitude]), angularUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionFalseEasting))
                parameters.Add(CoordinateOperationParameters.FalseEasting, new Length(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionFalseEasting]), linearUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionFalseNorthing))
                parameters.Add(CoordinateOperationParameters.FalseNorthing, new Length(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionFalseNorthing]), linearUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionFalseOriginLongitude))
                parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionFalseOriginLongitude]), angularUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionFalseOriginLatitude))
                parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionFalseOriginLatitude]), angularUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionFalseOriginEasting))
                parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, new Length(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionFalseOriginEasting]), linearUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionFalseOriginNorthing))
                parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, new Length(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionFalseOriginNorthing]), linearUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionCenterLongitude))
                parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionCenterLongitude]), angularUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionCenterLatitude))
                parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionCenterLatitude]), angularUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionCenterEasting))
                parameters.Add(CoordinateOperationParameters.EastingAtProjectionCentre, new Length(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionCenterEasting]), linearUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionCenterNorthing))
                parameters.Add(CoordinateOperationParameters.NorthingAtProjectionCentre, new Length(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionCenterNorthing]), linearUnit));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionScaleAtNaturalOrigin))
                parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionScaleAtNaturalOrigin]));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionScaleAtCenter))
                parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionScaleAtCenter]));
            if (_currentGeoKeys.ContainsKey(GeoKey.ProjectionAzimuthAngle))
                parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, new Angle(Convert.ToDouble(_currentGeoKeys[GeoKey.ProjectionAzimuthAngle]), ComputeAzimuthUnit()));

            return parameters;
        }

        /// <summary>
        /// Computes the coordinate projection.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection.</returns>
        /// <exception cref="System.IO.InvalidDataException">Projection code is invalid.</exception>
        private CoordinateProjection ComputeProjection(Ellipsoid ellipsoid)
        {
            Int32 code = Convert.ToInt32(_currentGeoKeys[GeoKey.Projection]);

            if (code >= 10000 && code <= 19999)
                return CoordinateProjectionFactory.FromIdentifier("EPSG::" + code, ellipsoid).FirstOrDefault();

            if (code == Int16.MaxValue)
            {
                CoordinateOperationMethod method = ComputeCoordinateOperationMethod();
                Dictionary<CoordinateOperationParameter, Object> parameters = ComputeCoordinateOperationParameters();

                return CoordinateProjectionFactory.FromMethod(method, parameters, ellipsoid, AreasOfUse.World);
            }

            throw new InvalidDataException("Projection code is invalid.");
        }

        #endregion
    }
}
