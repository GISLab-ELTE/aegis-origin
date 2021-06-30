/// <copyright file="GeoJsonReader.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Norbert Vass</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Management;
using ELTE.AEGIS.Reference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ELTE.AEGIS.IO.GeoJSON
{
    /// <summary>
    /// Represents a GeoJSON format reader.
    /// </summary>    
    /// <remarks>
    /// GeoJSON is a geospatial data interchange format based on JavaScript Object Notation (JSON).
    /// You can read about the format specification <a href="http://geojson.org/geojson-spec.html">here</a>.
    /// </remarks>

    [IdentifiedObjectInstance("AEGIS::610102", "GeoJSON file")]
    public class GeoJsonReader : GeometryStreamReader
    {
        #region Private fields

        /// <summary>
        /// The root GeoJson element.
        /// </summary>
        private GeoJsonObject _geo;

        //private bool _isFeatureCollectionReading;

        /// <summary>
        /// The number of elements to be read.
        /// </summary>
        private int _numElements;

        /// <summary>
        /// The number of readed elements. -1 means that the root element (_geo) hasn't read yet.
        /// </summary>
        private int _readedElements = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoJsonReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// </exception>
        public GeoJsonReader(String path) : base(path, GeometryStreamFormats.GeoJson, null)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoJsonReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// </exception>
        public GeoJsonReader(Uri path)
            : base(path, GeometryStreamFormats.GeoJson, null)
        {
        }

        #endregion

        #region GeometryStreamReader protected methods

        /// <summary>
        /// Returns a value indicating whether the end of the stream is reached.
        /// </summary>
        /// <returns><c>true</c> if the end of the stream is reached; otherwise, <c>false</c>.</returns>
        protected override Boolean GetEndOfStream()
        {
            return _readedElements >= _numElements;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (_geo != null)
                _geo = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        /// <exception cref="System.IO.IOException">
        /// Exception occurs when the file format is wrong.
        /// </exception>
        /// /// <exception cref="System.NotSupportedException">
        /// Exception occurs when we read feature which isn't contain a geometry.
        /// </exception>
        protected override IGeometry ApplyReadGeometry()
        {
            try
            {
                if (_readedElements == -1)
                {
                    LoadData();
                    _readedElements++;
                }

                GeometryObject obj;
                Dictionary<string, object> properties = new Dictionary<string, object>();

                if (_geo.Type == "FeatureCollection")
                {
                    Feature feat = (_geo as FeatureCollection).Features[_readedElements];
                    _readedElements++;

                    obj = feat.Geometry;
                    if (obj == null)
                        throw new NotSupportedException("Feature with null geometry not supported.");

                    // if crs property is null, it can inherit from his feature or featurecollection
                    if (obj.Crs == null)
                    {
                        if (feat.Crs != null)
                            obj.Crs = feat.Crs;
                        else
                            obj.Crs = (_geo as FeatureCollection).Crs;
                    }

                    //properties
                    if (feat.Properties != null)
                    {
                        properties = feat.Properties;
                    }

                    if (feat.Id != null)
                        properties.Add("OBJECTID", feat.Id);
                }
                else if (_geo.Type == "Feature")
                {
                    _readedElements++;
                    Feature feat = (_geo as Feature);

                    obj = feat.Geometry;
                    if (obj == null)
                        throw new IOException("Feature with null geometry not supported.");

                    // if crs property is null, it can inherit from his feature or featurecollection
                    if (obj.Crs == null)
                        obj.Crs = feat.Crs;

                    if (feat.Properties != null)
                    {
                        properties = feat.Properties;
                    }

                    if (feat.Id != null)
                        properties.Add("OBJECTID", feat.Id);
                }
                else
                {
                    _readedElements++;
                    obj = (_geo as GeometryObject);
                }

                ResolveFactory(GetReferenceSystem(obj.Crs));
                
                switch (obj.Type)
                {
                    case "Point":
                        if (obj.Coordinates == null)
                            throw new IOException("Geometry coordinates value is null.");
                        double[] coords = obj.Coordinates.Children().Select(r => (double)r).ToArray();
                        return CreatePoint(coords, properties);
                    case "MultiPoint":
                        if (obj.Coordinates == null)
                            throw new IOException("Geometry coordinates value is null.");
                        return CreateMultiPoint(obj.Coordinates, properties);
                    case "LineString":
                        if (obj.Coordinates == null)
                            throw new IOException("Geometry coordinates value is null.");
                        return CreateLineString(obj.Coordinates, properties);
                    case "MultiLineString":
                        if (obj.Coordinates == null)
                            throw new IOException("Geometry coordinates value is null.");
                        return CreateMultiLineString(obj.Coordinates, properties);
                    case "Polygon":
                        if (obj.Coordinates == null)
                            throw new IOException("Geometry coordinates value is null.");
                        return CreatePolygon(obj.Coordinates, properties);
                    case "MultiPolygon":
                        if (obj.Coordinates == null)
                            throw new IOException("Geometry coordinates value is null.");
                        return CreateMultiPolygon(obj.Coordinates, properties);
                    case "GeometryCollection":
                        if (obj.Geometries == null)
                            throw new IOException("GeometryCollection geometries value is null.");
                        return CreateGeometryCollection(obj.Geometries, properties);
                    default: throw new ArgumentException("Not supported geometry type: " + obj.Type + ".");
                }
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentInvalid, ex);
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Determines the ReferenceSystem from the Crs object.
        /// </summary>
        /// <param name="obj">The input crs object.</param>
        /// <returns>The determined ReferenceSystem.</returns>
        private IReferenceSystem GetReferenceSystem(CrsObject obj)
        {
            if (obj == null)
                return GeographicCoordinateReferenceSystems.FromName("WGS84").FirstOrDefault();

            if (obj.Type == "name")
            {
                if (obj.Properties.ContainsKey("name"))
                {
                    IList<IReferenceSystem> systems = GeocentricCoordinateReferenceSystems.FromIdentifier(obj.Properties["name"]).ToList<IReferenceSystem>();
                    if (systems.Count > 0)
                        return systems[0];

                    systems = GeographicCoordinateReferenceSystems.FromIdentifier(obj.Properties["name"]).ToList<IReferenceSystem>();
                    if (systems.Count > 0)
                        return systems[0];

                    systems = GridReferenceSystems.FromIdentifier(obj.Properties["name"]).ToList<IReferenceSystem>();
                    if (systems.Count > 0)
                        return systems[0];

                    systems = ProjectedCoordinateReferenceSystems.FromIdentifier(obj.Properties["name"]).ToList<IReferenceSystem>();
                    if (systems.Count > 0)
                        return systems[0];

                    systems = VerticalCoordinateReferenceSystems.FromIdentifier(obj.Properties["name"]).ToList<IReferenceSystem>();
                    if (systems.Count > 0)
                        return systems[0];

                    throw new NotSupportedException("Not supported Coordinate Reference System: " + obj.Properties["name"]);

                }
                else throw new IOException("Named Crs must have a \"name\" property with string value.");
            }
            else if (obj.Type == "link")
            {
                if (obj.Properties.ContainsKey("href"))
                {
                    if (!obj.Properties.ContainsKey("type") || obj.Properties.ContainsKey("type") && obj.Properties["type"] == "esriwkt")
                        return ReadReferenceSystemFromFile(obj.Properties["href"]);

                    else throw new NotSupportedException("The given Reference System type is not supported.");
                } else throw new InvalidDataException("Linked Crs must have an \"href\" property with string value.");
            }
            else throw new NotSupportedException("Not supported Coordinate Reference System type: " + obj.Type);
        }

        /// <summary>
        /// Determines the ReferenceSystem from file.
        /// </summary>
        /// <param name="path">The path of the file contains the reference system.</param>
        /// <returns>The determined ReferenceSystem.</returns>
        private IReferenceSystem ReadReferenceSystemFromFile(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    StringBuilder builder = new StringBuilder();
                    while (!reader.EndOfStream)
                        builder.Append(reader.ReadLine().Trim());

                    return WellKnown.WellKnownTextConverter.ToReferenceSystem(builder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Unrecognized CRS in linked WKT file.", ex);
            }
        }

        /// <summary>
        /// Reads all content from file and stores them.
        /// </summary>
        private void LoadData()
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(_baseStream))
                {
                    _geo = JsonConvert.DeserializeObject<GeoJsonObject>(streamReader.ReadToEnd(), new GeoJsonConvert());
                    streamReader.Close();
                }

                if (_geo == null || (_geo.Type == "Feature" && (_geo as Feature).Geometry == null))
                    _numElements = 0;
                else if (_geo.Type == "FeatureCollection")
                    _numElements = (_geo as FeatureCollection).Features.Length;
                else
                    _numElements = 1;
            } catch (Exception)
            {
                throw new IOException(MessageContentInvalid);
            }
        }

        /// <summary>
        /// Creates a point with the given coordinates and properties.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="properties">The data to be stored with the point.</param>
        /// <returns>The created point.</returns>
        private IPoint CreatePoint(double[] coordinates, IDictionary<string, object> properties)
        {
            if (coordinates.Length == 2)
                return new Point(coordinates[0], coordinates[1], 0, Factory, properties);
            else if (coordinates.Length == 3)
                return new Point(coordinates[0], coordinates[1], coordinates[2], Factory, properties);
            else
                throw new ArgumentException("Only 2 or 3 dimensional geometries supported.");
        }

        /// <summary>
        /// Creates a MultiPoint with the given coordinates and properties.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="properties">The data to be stored with the MultiPoint.</param>
        /// <returns>The created MultiPoint.</returns>
        private IMultiPoint CreateMultiPoint(JArray coordinates, IDictionary<string, object> properties)
        {
            List<IPoint> points = new List<IPoint>();
            foreach (JToken coord in coordinates.Children())
            {
                double[] coords = coord.Children().Select(r => (double)r).ToArray();
                points.Add(CreatePoint(coords, null));
            }
            return new MultiPoint(points, Factory, properties);
        }

        /// <summary>
        /// Creates a LineString with the given coordinates and properties.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="properties">The data to be stored with the LineString.</param>
        /// <returns>The created LineString.</returns>
        /// <exception cref="System.IO.IOException">
        /// The coordinates' lengh less than 2.
        /// </exception>        
        private ILineString CreateLineString(JArray coordinates, IDictionary<string, object> properties)
        {
            List<IPoint> points = new List<IPoint>();

            foreach (JToken coord in coordinates.Children())
            {
                double[] coords = coord.Children().Select(r => (double)r).ToArray();
                points.Add(CreatePoint(coords, null));
            }

            if (points.Count < 2)
                throw new IOException("LineString must have two or more coordinates.");
            return Factory.CreateLineString(points, properties);
        }

        /// <summary>
        /// Creates a MultiLineString with the given coordinates and properties.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="properties">The data to be stored with the MultiLineString.</param>
        /// <returns>The created MultiLineString.</returns>
        private IMultiLineString CreateMultiLineString(JArray coordinates, IDictionary<string, object> properties)
        {
            List<ILineString> lineStrings = new List<ILineString>();

            foreach (JArray coord in coordinates.Children())
            {
                lineStrings.Add(CreateLineString(coord, null));
            }
            return Factory.CreateMultiLineString(lineStrings, properties);
        }

        /// <summary>
        /// Creates a Polygon with the given coordinates and properties.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="properties">The data to be stored with the Polygon.</param>
        /// <returns>The created Polygon.</returns>
        /// <exception cref="System.IO.IOException">
        /// The polygon's has less than 4 coordinates, or the first and the last coordinates don't match.
        /// </exception>
        private IPolygon CreatePolygon(JArray coordinates, IDictionary<string, object> properties)
        {
            List<Coordinate> points = new List<Coordinate>();
            List<List<Coordinate>> holes = new List<List<Coordinate>>();

            bool first = true;
            foreach (JArray coords in coordinates.Children())
            {
                List<Coordinate> temp = new List<Coordinate>();

                foreach (JArray coord in coords.Children())
                {
                    IPoint point = CreatePoint(coord.Children().Select(r => (double)r).ToArray(), null);
                    temp.Add(new Coordinate(point.X, point.Y, point.Z));
                }

                if (temp.Count < 4)
                    throw new IOException("Polygon must have four or more coordinates.");
                if (temp.First() != temp.Last())
                    throw new IOException("Polygon's first and last coordinate must match.");

                if (!first)
                    holes.Add(temp);
                else
                {
                    points = temp;
                    first = false;
                }
            }

            return Factory.CreatePolygon(points, holes, properties);
        }

        /// <summary>
        /// Creates a MultiPolygon with the given coordinates and properties.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="properties">The data to be stored with the MultiPolygon.</param>
        /// <returns>The created MultiPolygon.</returns>
        private IMultiPolygon CreateMultiPolygon(JArray coordinates, IDictionary<string, object> properties)
        {
            List<IPolygon> polygons = new List<IPolygon>();

            foreach (JArray coord in coordinates.Children())
            {
                polygons.Add(CreatePolygon(coord, null));
            }
            return Factory.CreateMultiPolygon(polygons, properties);
        }

        /// <summary>
        /// Creates a GeometryCollection with the given geometries and properties.
        /// </summary>
        /// <param name="geometries">The geometries.</param>
        /// <param name="properties">The data to be stored with the GeometryCollection.</param>
        /// <returns>The created GeometryCollection.</returns>
        /// <exception cref="System.NullReferenceException">
        /// The coordinates of a geometry is null, or the geometries of a geometrycollection is null.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// One of the geometries' type not supported.
        /// </exception>
        private IGeometryCollection<IGeometry> CreateGeometryCollection(GeometryObject[] geometries,
                                                                        IDictionary<String, object> properties)
        {
            List<IGeometry> geos = new List<IGeometry>();

            try
            {
                foreach (GeometryObject obj in geometries)
                {
                    switch (obj.Type)
                    {
                        case "Point":
                            if (obj.Coordinates == null)
                                throw new NullReferenceException("Geometry coordinates value is null.");
                            double[] coords = obj.Coordinates.Children().Select(r => (double)r).ToArray();
                            geos.Add(CreatePoint(coords, properties)); break;
                        case "MultiPoint":
                            if (obj.Coordinates == null)
                                throw new NullReferenceException("Geometry coordinates value is null.");
                            geos.Add(CreateMultiPoint(obj.Coordinates, properties)); break;
                        case "LineString":
                            if (obj.Coordinates == null)
                                throw new NullReferenceException("Geometry coordinates value is null.");
                            geos.Add(CreateLineString(obj.Coordinates, properties)); break;
                        case "MultiLineString":
                            if (obj.Coordinates == null)
                                throw new NullReferenceException("Geometry coordinates value is null.");
                            geos.Add(CreateMultiLineString(obj.Coordinates, properties)); break;
                        case "Polygon":
                            if (obj.Coordinates == null)
                                throw new NullReferenceException("Geometry coordinates value is null.");
                            geos.Add(CreatePolygon(obj.Coordinates, properties)); break;
                        case "MultiPolygon":
                            if (obj.Coordinates == null)
                                throw new NullReferenceException("Geometry coordinates value is null.");
                            geos.Add(CreateMultiPolygon(obj.Coordinates, properties)); break;
                        case "GeometryCollection":
                            if (obj.Geometries == null)
                                throw new NullReferenceException("GeometryCollection geometries value is null.");
                            geos.Add(CreateGeometryCollection(obj.Geometries, properties)); break;
                        default: throw new IOException("Not supported geometry type: " + obj.Type + ".");
                    }
                }
                return Factory.CreateGeometryCollection(geos, properties);
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentInvalid, ex);
            }
        }

        #endregion
    }
}
