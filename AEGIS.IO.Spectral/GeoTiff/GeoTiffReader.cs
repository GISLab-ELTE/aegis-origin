/// <copyright file="GeoTiffReader.cs" company="Eötvös Loránd University (ELTE)">
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
            /// Geographic.
            /// </summary>
            Geographic = 2,

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
        private Dictionary<Int16, Object> _currentGeoKeys;

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
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public GeoTiffReader(String path)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, null)
        {
            try
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsGeoTiffFilePath);
            }
            catch { }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="metafileReader">The metafile reader.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public GeoTiffReader(String path, GeoTiffMetafileReader metafileReader)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, null)
        {
            _metafileReader = metafileReader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="metafileReader">The metafile reader.</param>
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public GeoTiffReader(String path, GeoTiffMetafileReader metafileReader, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
            _metafileReader = metafileReader;
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public GeoTiffReader(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
            try
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsGeoTiffFilePath);
            }
            catch { }
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public GeoTiffReader(Uri path)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, null)
        {
            try
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsGeoTiffFilePath);
            }
            catch { }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="metafileReader">The metafile reader.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public GeoTiffReader(Uri path, GeoTiffMetafileReader metafileReader)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, null)
        {
            _metafileReader = metafileReader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="metafileReader">The metafile reader.</param>
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public GeoTiffReader(Uri path, GeoTiffMetafileReader metafileReader, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
            _metafileReader = metafileReader;
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
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public GeoTiffReader(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
            try
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsGeoTiffFilePath);
            }
            catch { }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
        public GeoTiffReader(Stream stream)
            : base(stream, SpectralGeometryStreamFormats.GeoTiff, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="metafileReader">The metafile reader.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
        public GeoTiffReader(Stream stream, GeoTiffMetafileReader metafileReader)
            : base(stream, SpectralGeometryStreamFormats.GeoTiff, null)
        {
            _metafileReader = metafileReader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffReader" /> class.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="metafileReader">The metafile reader.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
        public GeoTiffReader(Stream stream, GeoTiffMetafileReader metafileReader, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, SpectralGeometryStreamFormats.GeoTiff, null)
        {
            _metafileReader = metafileReader;
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
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
        public GeoTiffReader(Stream stream, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, SpectralGeometryStreamFormats.GeoTiff, null)
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

            if (_currentGeoKeys == null)
                return metadata;

            if (_currentGeoKeys.ContainsKey(1026))
                metadata["GeoCitation"] = _currentGeoKeys[1026];
            if (_currentGeoKeys.ContainsKey(2049))
                metadata["GeodeticCoordinateReferenceSystemCitation"] = _currentGeoKeys[2049];
            if (_currentGeoKeys.ContainsKey(3073))
                metadata["ProjectedCoordinateReferenceSystemCitation"] = _currentGeoKeys[3073];

            return metadata;
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
            if (!_currentGeoKeys.ContainsKey(1025))
                return null;

            RasterMapMode mode = Convert.ToInt16(_currentGeoKeys[1025]) == 1 ? RasterMapMode.ValueIsArea : RasterMapMode.ValueIsCoordinate;

            Double[] modelTiePointsArray = null;
            Double[] modelPixelScaleArray = null;
            Double[] modelTransformationArray = null;

            // gather information from tags
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(33922))
            {
                modelTiePointsArray = _imageFileDirectories[_currentImageIndex][33922].Select(value => Convert.ToDouble(value)).ToArray();
            }
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(33550))
            {
                modelPixelScaleArray = _imageFileDirectories[_currentImageIndex][33550].Select(value => Convert.ToDouble(value)).ToArray();
            }
            if (_imageFileDirectories[_currentImageIndex].ContainsKey(34264))
            {
                modelTransformationArray = _imageFileDirectories[_currentImageIndex][34264].Select(value => Convert.ToDouble(value)).ToArray();
            }
            // for GeoTIFF 0.2, IntergraphMatrixTag (33920) may contain the transformation values
            if (modelTransformationArray == null && _imageFileDirectories[_currentImageIndex].ContainsKey(33920))
            {
                modelTransformationArray = _imageFileDirectories[_currentImageIndex][33920].Select(value => Convert.ToDouble(value)).ToArray();
            }

            // compute with model tie points
            if (modelTiePointsArray != null && (modelTiePointsArray.Length > 6 || (modelTiePointsArray.Length == 6 && modelPixelScaleArray.Length == 3)))
            {
                if (modelTiePointsArray.Length > 6) // transformation is specified by tiepoints
                {
                    RasterCoordinate[] coordinates = new RasterCoordinate[modelTiePointsArray.Length / 6];

                    for (Int32 i = 0; i < modelTiePointsArray.Length; i += 6)
                    {
                        coordinates[i / 6] = new RasterCoordinate(Convert.ToInt32(modelTiePointsArray[i]), Convert.ToInt32(modelTiePointsArray[i + 1]), new Coordinate(modelTiePointsArray[i + 3], modelTiePointsArray[i + 4], modelTiePointsArray[i + 5]));
                    }

                    return RasterMapper.FromCoordinates(mode, coordinates);
                }
                else // transformation is specified by single tiepoint and scale
                {
                    Coordinate rasterSpaceCoordinate = new Coordinate(modelTiePointsArray[0], modelTiePointsArray[1], modelTiePointsArray[2]);
                    Coordinate modelSpaceCoordinate = new Coordinate(modelTiePointsArray[3], modelTiePointsArray[4], modelTiePointsArray[5]);
                    Double scaleX = modelPixelScaleArray[0];
                    Double scaleY = modelPixelScaleArray[1];
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
            if (_currentGeoKeys.ContainsKey(1024))
            {
                // reference system information is in the file
                try
                {
                    // read from GeoTIFF geokeys
                    if (_currentGeoKeys != null && _currentGeoKeys.ContainsKey(1024))
                    {
                        switch ((ReferenceSystemType)Convert.ToInt32(_currentGeoKeys[1024]))
                        {
                            case ReferenceSystemType.Projected:
                                return ComputeProjectedCoordinateReferenceSystem();
                            case ReferenceSystemType.Geographic:
                                return ComputeGeographicCoordinateReferenceSystem();
                            case ReferenceSystemType.Geocentric:
                                // currenty not used
                                return null;
                            case ReferenceSystemType.UserDefined:
                                // currenty not used
                                return null;
                        }
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
        /// Computes the geo keys for the current raster.
        /// </summary>
        /// <exception cref="System.IO.InvalidDataException">Geo key data is in an invalid format.</exception>
        private void ComputeGeoKeys()
        {
            _currentGeoKeys = new Dictionary<Int16, Object>();

            // there are no geo keys in the file
            if (!_imageFileDirectories[_currentImageIndex].ContainsKey(34735))
            {
                return;
            }

            try
            {
                _geoTiffFormatVersion = _imageFileDirectories[_currentImageIndex][34735][0] + "." + _imageFileDirectories[_currentImageIndex][34735][1] + "." + _imageFileDirectories[_currentImageIndex][34735][2];
                Int32 offset, count;

                for (Int16 i = 4; i < _imageFileDirectories[_currentImageIndex][34735].Length; i += 4)
                {
                    switch (Convert.ToInt32(_imageFileDirectories[_currentImageIndex][34735][1 + i]))
                    {
                        case 0:
                            _currentGeoKeys.Add(Convert.ToInt16(_imageFileDirectories[_currentImageIndex][34735][i]), Convert.ToInt16(_imageFileDirectories[_currentImageIndex][34735][3 + i]));
                            break;
                        case 34736:
                            offset = Convert.ToInt32(_imageFileDirectories[_currentImageIndex][34735][3 + i]);
                            Double doubleValue = Convert.ToDouble(_imageFileDirectories[_currentImageIndex][34736][offset]);
                            _currentGeoKeys.Add(Convert.ToInt16(_imageFileDirectories[_currentImageIndex][34735][i]), doubleValue);
                            break;
                        case 34737:
                            count = Convert.ToInt32(_imageFileDirectories[_currentImageIndex][34735][2 + i]);
                            offset = Convert.ToInt32(_imageFileDirectories[_currentImageIndex][34735][3 + i]);
                            // strings are concatenated to a single value using the | (pipe) character, so they are split, and the ending character is removed
                            _currentGeoKeys.Add(Convert.ToInt16(_imageFileDirectories[_currentImageIndex][34735][i]), Convert.ToString(_imageFileDirectories[_currentImageIndex][34737][0]).Substring(offset, count - 1));
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
        /// Computes the geographic coordinate reference system.
        /// </summary>
        /// <returns>The geographic coordinate reference system.</returns>
        /// <exception cref="System.IO.InvalidDataException">Geographic coordinate reference system code is invalid.</exception>
        private GeographicCoordinateReferenceSystem ComputeGeographicCoordinateReferenceSystem()
        {
            Int32 code = Convert.ToInt32(_currentGeoKeys[2048]);
            // EPSG geodetic coordinate reference system codes
            if (code >= 400 && code <= 4999)
                return GeographicCoordinateReferenceSystems.FromIdentifier("EPSG::" + code).FirstOrDefault();

            // user-defined geodetic coordinate reference system
            if (code == 32767)
            {
                String citation = _currentGeoKeys[2049].ToString();
                GeodeticDatum datum = ComputeGeodeticDatum();
                UnitOfMeasurement angleUnit = ComputeAngularUnit();

                CoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystem.UserDefinedIdentifier, CoordinateSystem.UserDefinedName, 
                                                                         CoordinateSystemType.Ellipsoidal,
                                                                         CoordinateSystemAxisFactory.GeodeticLatitude(angleUnit),
                                                                         CoordinateSystemAxisFactory.GeodeticLongitude(angleUnit));

                return new GeographicCoordinateReferenceSystem(GeographicCoordinateReferenceSystem.UserDefinedIdentifier, GeographicCoordinateReferenceSystem.UserDefinedName, 
                                                               citation, null, null, coordinateSystem, datum, null);
            }

            throw new InvalidDataException("Geographic coordinate reference system code is invalid.");
        }

        /// <summary>
        /// Computes the geodetic datum.
        /// </summary>
        /// <returns>The geodetic datum.</returns>
        /// <exception cref="System.IO.InvalidDataException">Geodetic Datum code is invalid.</exception>
        private GeodeticDatum ComputeGeodeticDatum()
        {
            Int32 code = Convert.ToInt32(_currentGeoKeys[2050]);
            // EPSG geodetic datum codes
            if (code >= 6000 && code <= 6999)
                return GeodeticDatums.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined geodetic datum
            if (code == 32767)
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
            Int32 code = Convert.ToInt32(_currentGeoKeys[2056]);
            // EPSG ellipsoid codes
            if (code >= 8000 && code <= 8999)
                return Ellipsoids.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined ellipsoid
            if (code == 32767)
            {
                Double semiMajorAxis = Convert.ToDouble(_currentGeoKeys[2057]);

                // either semi-minor axis or the inverse flattening is defined
                if (_currentGeoKeys.ContainsKey(2058))
                {
                    Double semiMinorAxis = Convert.ToDouble(_currentGeoKeys[2058]);
                    return Ellipsoid.FromSemiMinorAxis(Ellipsoid.UserDefinedIdentifier, Ellipsoid.UserDefinedName, semiMajorAxis, semiMinorAxis);
                }
                else
                {
                    Double inverseFlattening = Convert.ToDouble(_currentGeoKeys[2059]);
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
            if (!_currentGeoKeys.ContainsKey(2051))
                return Meridians.Greenwich;

            Int32 code = Convert.ToInt32(_currentGeoKeys[2051]);
            // EPSG prime meridian codes
            if (code >= 8000 && code <= 8999)
                return Meridians.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined prime meridian
            if (code == 32767)
            {
                Double longitude = Convert.ToDouble(_currentGeoKeys[2061]);
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
            if (!_currentGeoKeys.ContainsKey(2052))
                return UnitsOfMeasurement.Metre;

            Int32 code = Convert.ToInt32(_currentGeoKeys[2052]);
            // EPSG unit of measurement codes
            if (code >= 9000 && code <= 9099)
                return UnitsOfMeasurement.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined unit of measurement
            if (code == 32767)
            {
                Double unitSize = Convert.ToDouble(_currentGeoKeys[2053]);

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
            if (!_currentGeoKeys.ContainsKey(2054))
                return UnitsOfMeasurement.Degree;

            Int32 code = Convert.ToInt32(_currentGeoKeys[2054]);
            // EPSG unit of measurement codes
            if (code >= 9100 && code <= 9199)
                return UnitsOfMeasurement.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined unit of measurement
            if (code == 32767)
            {
                Double unitSize = Convert.ToDouble(_currentGeoKeys[2055]);

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
            if (!_currentGeoKeys.ContainsKey(2060))
                return UnitsOfMeasurement.Degree;

            Int32 code = Convert.ToInt32(_currentGeoKeys[2060]);
            // EPSG unit of measurement codes
            if (code >= 9100 && code <= 9199)
                return UnitsOfMeasurement.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined unit of measurement
            if (code == 32767)
            {
                Double unitSize = Convert.ToDouble(_currentGeoKeys[2055]);

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
            Int32 code = Convert.ToInt32(_currentGeoKeys[3072]);
            // EPSG Projected Coordinate Reference System codes
            if (code >= 20000 && code <= 32760)
                return ProjectedCoordinateReferenceSystems.FromIdentifier("EPSG::" + code).FirstOrDefault();
            // user-defined Projected Coordinate Reference System
            if (code == 32767)
            {
                GeographicCoordinateReferenceSystem baseReferenceSystem = ComputeGeographicCoordinateReferenceSystem();
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
            Int32 code = Convert.ToInt32(_currentGeoKeys[3075]);

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
                    return null;
                case 12:
                    return CoordinateOperationMethods.LambertAzimuthalEqualAreaProjection;
                case 13: // CT_EquidistantConic
                    return null;
                case 14: // CT_Stereographic 
                    return null;
                case 15:
                    return CoordinateOperationMethods.PolarStereographicAProjection; // TODO: parameter check
                case 16:
                    return CoordinateOperationMethods.ObliqueStereographicProjection;
                case 17: // CT_Equirectangular 
                    return null;
                case 18:
                    return CoordinateOperationMethods.CassiniSoldnerProjection;
                case 19: // CT_Gnomonic
                    return null;
                case 20: // CT_MillerCylindrical 
                    return null;
                case 21: // CT_Orthographic 
                    return null;
                case 22: // CT_Polyconic 
                    return null;
                case 23: // CT_Robinson 
                    return null;
                case 24: // CT_Sinusoidal 
                    return null;
                case 25: // CT_VanDerGrinten 
                    return null;
                case 26: // CT_NewZealandMapGrid 
                    return null;
                case 27:
                    return CoordinateOperationMethods.TransverseMercatorSouthProjection;
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

            if (_currentGeoKeys.ContainsKey(3078))
            {
                parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, new Angle(Convert.ToDouble(_currentGeoKeys[3078]), angularUnit));
                parameters.Add(CoordinateOperationParameters.LatitudeOfStandardParallel, new Angle(Convert.ToDouble(_currentGeoKeys[3078]), angularUnit));
            }
            if (_currentGeoKeys.ContainsKey(3079))
                parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, new Angle(Convert.ToDouble(_currentGeoKeys[3079]), angularUnit));
            if (_currentGeoKeys.ContainsKey(3080))
            {
                parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[3080]), angularUnit));
                parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[3080]), angularUnit));
            }
            if (_currentGeoKeys.ContainsKey(3081))
                parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[3081]), angularUnit));
            if (_currentGeoKeys.ContainsKey(3082))
                parameters.Add(CoordinateOperationParameters.FalseEasting, new Length(Convert.ToDouble(_currentGeoKeys[3082]), linearUnit));
            if (_currentGeoKeys.ContainsKey(3083))
                parameters.Add(CoordinateOperationParameters.FalseNorthing, new Length(Convert.ToDouble(_currentGeoKeys[3083]), linearUnit));
            if (_currentGeoKeys.ContainsKey(3084))
                parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[3084]), angularUnit));
            if (_currentGeoKeys.ContainsKey(3085))
                parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, new Angle(Convert.ToDouble(_currentGeoKeys[3085]), angularUnit));
            if (_currentGeoKeys.ContainsKey(3086))
                parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, new Length(Convert.ToDouble(_currentGeoKeys[3086]), linearUnit));
            if (_currentGeoKeys.ContainsKey(3087))
                parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, new Length(Convert.ToDouble(_currentGeoKeys[3087]), linearUnit));
            if (_currentGeoKeys.ContainsKey(3088))
                parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, new Angle(Convert.ToDouble(_currentGeoKeys[3088]), angularUnit));
            if (_currentGeoKeys.ContainsKey(3089))
                parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, new Angle(Convert.ToDouble(_currentGeoKeys[3089]), angularUnit));
            if (_currentGeoKeys.ContainsKey(3090))
                parameters.Add(CoordinateOperationParameters.EastingAtProjectionCentre, new Length(Convert.ToDouble(_currentGeoKeys[3090]), linearUnit));
            if (_currentGeoKeys.ContainsKey(3091))
                parameters.Add(CoordinateOperationParameters.NorthingAtProjectionCentre, new Length(Convert.ToDouble(_currentGeoKeys[3091]), linearUnit));
            if (_currentGeoKeys.ContainsKey(3092))
                parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, Convert.ToDouble(_currentGeoKeys[3092]));
            if (_currentGeoKeys.ContainsKey(3093))
                parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, Convert.ToDouble(_currentGeoKeys[3093]));
            if (_currentGeoKeys.ContainsKey(3094))
                parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, new Angle(Convert.ToDouble(_currentGeoKeys[3094]), ComputeAzimuthUnit()));

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
            Int32 code = Convert.ToInt32(_currentGeoKeys[3074]);

            if (code >= 10000 && code <= 19999)
                return CoordinateProjectionFactory.FromIdentifier("EPSG::" + code, ellipsoid).FirstOrDefault();

            if (code == 32767)
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
