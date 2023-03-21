// <copyright file="TopoJsonReader.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Management;
using ELTE.AEGIS.Reference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.TopoJSON
{
    /// <summary>
    /// Represents a TopoJSON format reader.
    /// </summary>
    /// <remarks>
    /// TopoJSON is a topological geospatial data interchange format based on GeoJSON.
    /// You can read about the format specification <a href="https://github.com/mbostock/topojson-specification">here</a>.
    /// </remarks>
    /// <author>Norbert Vass</author>
    [IdentifiedObjectInstance("AEGIS::610103", "TopoJSON file")]
    public class TopoJsonReader : GeometryStreamReader
    {
        #region Private fields

        /// <summary>
        /// The root TopoJson element (a topology).
        /// </summary>
        private TopologyObject _topo;

        /// <summary>
        /// The number of readed elements. -1 means that the root element (_topo) hasn't read yet.
        /// </summary>
        private int _readedElements = -1;

        /// <summary>
        /// The topology's "objects" member's keys.
        /// </summary>
        private string[] _keys;

        /// <summary>
        /// Indicates that the topology is quantized or not.
        /// </summary>
        private bool _quantized = false;

        #endregion

        #region Private Properties

        /// <summary>
        /// The number of elements to be read.
        /// </summary>
        private int NumElements
        {
            get
            {
                if (_topo != null && _topo.Objects != null)
                    return _topo.Objects.Count;
                else
                    return 0;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TopoJsonReader" /> class.
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
        public TopoJsonReader(String path)
            : base(path, GeometryStreamFormats.TopoJson, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopoJsonReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// </exception>
        public TopoJsonReader(Uri path)
            : base(path, GeometryStreamFormats.TopoJson, null)
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
            return _readedElements >= NumElements;
        }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        /// <exception cref="System.IO.IOException">
        /// Exception occurs when the file format is wrong.
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

                Dictionary<string, object> properties = new Dictionary<string, object>();
                TopoGeometryObject obj;
                if (_topo.Objects[_keys[_readedElements]] != null)
                    obj = _topo.Objects[_keys[_readedElements]];
                else
                {
                    _readedElements++;
                    throw new IOException("Wrong TopoJSON file format.");
                }

                _readedElements++;

                if (obj.Properties != null)
                {
                    if (obj.Properties.ContainsKey("crs"))
                        obj.Properties.Remove("crs");

                    properties = obj.Properties;
                }

                if (obj.Id != null)
                    properties.Add("OBJECTID", obj.Id);

                switch (obj.Type)
                {
                    case "Point":
                        if (obj.Coordinates == null)
                            throw new IOException("Geometry coordinates value is null.");
                        List<double> coords = obj.Coordinates.Children().Select(r => (double)r).ToList<double>();
                        return CreatePoint(coords, properties, false);
                    case "MultiPoint":
                        if (obj.Coordinates == null)
                            throw new IOException("Geometry coordinates value is null.");
                        return CreateMultiPoint(obj.Coordinates, properties);
                    case "LineString":
                        if (obj.Arcs == null)
                            throw new IOException("Geometry arcs value is null.");
                        return CreateLineString(obj.Arcs, properties);
                    case "MultiLineString":
                        if (obj.Arcs == null)
                            throw new IOException("Geometry arcs value is null.");
                        return CreateMultiLineString(obj.Arcs, properties);
                    case "Polygon":
                        if (obj.Arcs == null)
                            throw new IOException("Geometry arcs value is null.");
                        return CreatePolygon(obj.Arcs, properties);
                    case "MultiPolygon":
                        if (obj.Arcs == null)
                            throw new IOException("Geometry arcs value is null.");
                        return CreateMultiPolygon(obj.Arcs, properties);
                    case "GeometryCollection":
                        if (obj.Geometries == null)
                            throw new IOException("GeometryCollection geometries value is null.");
                        return CreateGeometryCollection(obj.Geometries, properties);
                    default: throw new IOException("Not supported geometry type: " + obj.Type + ".");
                }
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentReadError, ex);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads all content from file and stores them.
        /// </summary>
        private void LoadData()
        {
            using (StreamReader streamReader = new StreamReader(_baseStream))
            {
                _topo = JsonConvert.DeserializeObject<TopologyObject>(streamReader.ReadToEnd(), new TopoJsonConvert());
                streamReader.Close();
            }

            if (_topo == null)
                throw new IOException("No topology readed");
            else if (_topo.Type == "Topology" && _topo.Objects != null && _topo.Objects.Count != 0)
            {
                _keys = _topo.Objects.Keys.ToArray();
            }
            else
                throw new IOException("TopoJSON file must start with a single Topology object.");

            if (_topo.Transform != null)
            {
                if (_topo.Transform.Scale.Length == 2 && _topo.Transform.Translate.Length == 2)
                {
                    _quantized = true;
                }
                else
                    throw new IOException("The Translate and Scale arrays' size must be 2.");
            }

            for (int i = 0; i < _topo.Arcs.Count; i++)
            {
                if (!IsValidArc(_topo.Arcs[i]))
                    throw new ArgumentException("The Arcs property format is invalid.", "Arcs");

                if (_quantized)
                    _topo.Arcs[i] = DecodeArc(_topo.Arcs[i]);
            }

            if (_topo.Objects.Values.Count > 0)
            {
                bool l = true;
                var enumerator = _topo.Objects.Values.GetEnumerator();
                do
                {
                    var element = enumerator.Current;
                    if (element != null && element.Properties != null && element.Properties.ContainsKey("crs"))
                    {
                        l = false;
                        ResolveFactory(ResolveCoordinateReferenceSystem(element.Properties["crs"].ToString()));
                    }
                } while (enumerator.MoveNext() && l);

                if (l)
                    ResolveFactory();
            }
        }

        private IReferenceSystem ResolveCoordinateReferenceSystem(string id)
        {
            if (string.IsNullOrEmpty(id))
                return GeographicCoordinateReferenceSystems.FromName("WGS84").FirstOrDefault();

            IList<IReferenceSystem> systems = GeocentricCoordinateReferenceSystems.FromIdentifier(id).ToList<IReferenceSystem>();
            if (systems.Count > 0)
                return systems[0];

            systems = GeographicCoordinateReferenceSystems.FromIdentifier(id).ToList<IReferenceSystem>();
            if (systems.Count > 0)
                return systems[0];

            systems = GridReferenceSystems.FromIdentifier(id).ToList<IReferenceSystem>();
            if (systems.Count > 0)
                return systems[0];

            systems = ProjectedCoordinateReferenceSystems.FromIdentifier(id).ToList<IReferenceSystem>();
            if (systems.Count > 0)
                return systems[0];

            systems = VerticalCoordinateReferenceSystems.FromIdentifier(id).ToList<IReferenceSystem>();
            if (systems.Count > 0)
                return systems[0];

            return GeographicCoordinateReferenceSystems.FromName("WGS84").FirstOrDefault();
        }

        /// <summary>
        /// Checks the given arc.
        /// </summary>
        /// <param name="arc">The arc to be validate.</param>
        /// <returns>Valid or not.</returns>
        private bool IsValidArc(List<List<double>> arc)
        {
            if (arc.Count < 2)
                return false;

            for (int i = 0; i < arc.Count; i++)
            {
                if (arc[i].Count < 2 || arc[i].Count > 3)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Decodes an arc and returns with it.
        /// </summary>
        /// <param name="arc">The arc to be decoded.</param>
        /// <returns>The decoded arc.</returns>
        private List<List<double>> DecodeArc(List<List<double>> arc)
        {
            if (arc.Count < 2)
                throw new ArgumentException("Each arc's length must be 2 or more.");

            double scale0 = _topo.Transform.Scale[0];
            double scale1 = _topo.Transform.Scale[1];
            double translate0 = _topo.Transform.Translate[0];
            double translate1 = _topo.Transform.Translate[1];

            double x = 0;
            double y = 0;

            for (int i = 0; i < arc.Count; i++ )
            {
                arc[i][0] = /*Math.Round(*/(x += /*(int)*/arc[i][0]) * scale0 + translate0/*)*/;
                arc[i][1] = /*Math.Round(*/(y += /*(int)*/arc[i][1]) * scale1 + translate1/*)*/;
            }

            return arc;
        }

        /// <summary>
        /// Transforms the point with the topology's transform object.
        /// </summary>
        /// <param name="point">The point.</param>
        private void TransformPoint(ref List<double> point)
        {
            point[0] = point[1] * _topo.Transform.Scale[0] + _topo.Transform.Translate[0];
            point[1] = point[1] * _topo.Transform.Scale[1] + _topo.Transform.Translate[1];
        }

        /// <summary>
        /// Creates a point with the given coordinates and properties.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="properties">The data.</param>
        /// <param name="isArc">The point is in a linestring or not (just a single point).</param>
        /// <returns>The created Point.</returns>
        private IPoint CreatePoint(List<double> coordinates, Dictionary<String, object> properties, bool isArc = true)
        {
            if (coordinates.Count < 2 || coordinates.Count > 3)
                throw new ArgumentException("Only 2 or 3 dimensional geometries supported.");

            if (_quantized && !isArc)
                TransformPoint(ref coordinates);

            if (coordinates.Count == 2)
                return new Point(coordinates[0], coordinates[1], 0, Factory, properties);
            else
                return new Point(coordinates[0], coordinates[1], coordinates[2], Factory, properties);
        }

        /// <summary>
        /// Creates a Multipoint with the given coordinates and properties.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="properties">The data.</param>
        /// <returns>The created MultiPoint.</returns>
        private IMultiPoint CreateMultiPoint(JArray coordinates, Dictionary<String, object> properties)
        {
            List<IPoint> points = new List<IPoint>();
            foreach (JToken coord in coordinates.Children())
            {
                List<double> coords = coord.Children().Select(r => (double)r).ToList<double>();
                points.Add(CreatePoint(coords, null, false));
            }
            return new MultiPoint(points, Factory, properties);
        }

        /// <summary>
        /// Creates a LineString with the given arcs and properties.
        /// </summary>
        /// <param name="arcs">The arcs.</param>
        /// <param name="properties">The data.</param>
        /// <returns>The created LineString.</returns>
        /// <exception cref="System.ArgumentException">
        /// The arcs array is invalid.
        /// </exception>
        private ILineString CreateLineString(JArray arcs, Dictionary<String, object> properties)
        {
            List<IPoint> points = new List<IPoint>();

            foreach (JToken arc in arcs.Children())
            {
                int index = (int)arc;
                List<List<double>> coords;

                if (index < 0)
                {
                    coords = _topo.Arcs[index * -1 - 1];
                    coords.Reverse();
                }
                else
                    coords = _topo.Arcs[index];

                if (points.Count != 0)
                {
                    IPoint p = points.Last();
                    if ((Math.Abs(coords[0][0] - p.X) >= 1 && Math.Abs(coords[0][1] - p.Y) >= 1) && ((coords[0].Count == 3 && coords[0][2] != p.Z) || (coords[0].Count != 3 && p.Z != 0.0)))
                    {
                        //System.Diagnostics.Debug.WriteLine(arcs[0].ToString() + " " + arcs[1].ToString() + " " + index);
                        throw new ArgumentException("The arc's last position must match with the next arc's first position.", "arcs");
                    }
                } else
                    points.Add(CreatePoint(coords[0], null));

                for (int i = 1; i < coords.Count; i++)
                {
                    points.Add(CreatePoint(coords[i], null));
                }
            }

            if (points.Count < 2)
                throw new ArgumentException("LineString must have two or more coordinates.", "arcs");
            return Factory.CreateLineString(points, properties);
        }

        /// <summary>
        /// Creates a MultiLineString with the given arcs and properties.
        /// </summary>
        /// <param name="arcs">The arcs.</param>
        /// <param name="properties">The data.</param>
        /// <returns>The created MultiLineString.</returns>
        private IMultiLineString CreateMultiLineString(JArray arcs, Dictionary<String, object> properties)
        {
            List<ILineString> lineStrings = new List<ILineString>();

            foreach (JArray arc in arcs.Children())
            {
                lineStrings.Add(CreateLineString(arc, null));
            }
            return Factory.CreateMultiLineString(lineStrings, properties);
        }

        /// <summary>
        /// Creates a Polygon with the given arcs and properties.
        /// </summary>
        /// <param name="arcs">The arcs.</param>
        /// <param name="properties">The data.</param>
        /// <returns>The created Polygon.</returns>
        private IPolygon CreatePolygon(JArray arcs, Dictionary<String, object> properties)
        {
            ILineString points = null;
            List<ILineString> holes = new List<ILineString>();

            bool first = true;
            foreach (JArray arc in arcs.Children())
            {
                ILineString temp = CreateLineString(arc, null);

                if (temp.Coordinates.Count < 4)
                    throw new InvalidDataException("Polygon must have at least four coordinates.");
                if (Math.Abs(temp.Coordinates.First().X - temp.Coordinates.Last().X) > 1 ||
                    Math.Abs(temp.Coordinates.First().Y - temp.Coordinates.Last().Y) > 1)
                {
                    throw new InvalidDataException("Polygon's first and last coordinate must match.");
                } else
                {
                    List<Coordinate> coordinates = new List<Coordinate>(temp.Coordinates);
                    coordinates[coordinates.Count - 1] = temp.Coordinates.First();
                    temp = Factory.CreateLineString(coordinates);
                }

                if (!first)
                    holes.Add(temp);
                else
                {
                    points = temp;
                    first = false;
                }
            }

            if (points == null)
                throw new ArgumentException("Wrong polygon arcs format.");

            return Factory.CreatePolygon(points.Coordinates, holes, properties);
        }

        /// <summary>
        /// Creates a MultiPolygon with the given arcs and properties.
        /// </summary>
        /// <param name="arcs">The arcs.</param>
        /// <param name="properties">The data.</param>
        /// <returns>The created MultiPolygon.</returns>
        private IMultiPolygon CreateMultiPolygon(JArray arcs, Dictionary<String, object> properties)
        {
            List<IPolygon> polygons = new List<IPolygon>();

            foreach (JArray coord in arcs.Children())
            {
                polygons.Add(CreatePolygon(coord, null));
            }
            return Factory.CreateMultiPolygon(polygons, properties);
        }

        /// <summary>
        /// Creates a GeometryCollection with the given geometries and properties.
        /// </summary>
        /// <param name="geometries">The geometries.</param>
        /// <param name="properties">The data.</param>
        /// <returns>The created GeometryCollection.</returns>
        /// <exception cref="System.IO.IOException">
        /// A geometry is invalid in the geometries array.
        /// </exception>
        private IGeometryCollection<IGeometry> CreateGeometryCollection(TopoGeometryObject[] geometries, Dictionary<String, object> properties)
        {
            List<IGeometry> geos = new List<IGeometry>();

            try
            {
                foreach (TopoGeometryObject obj in geometries)
                {
                    if (obj.Id != null)
                    {
                        if (obj.Properties == null)
                            obj.Properties = new Dictionary<string, object>();
                        obj.Properties.Add("OBJECTID", obj.Id);
                    }
                    switch (obj.Type)
                    {
                        case "Point":
                            if (obj.Coordinates == null)
                                throw new IOException("Geometry coordinates value is null.");
                            List<double> coords = obj.Coordinates.Children().Select(r => (double)r).ToList<double>();
                            geos.Add(CreatePoint(coords, obj.Properties, false)); break;
                        case "MultiPoint":
                            if (obj.Coordinates == null)
                                throw new IOException("Geometry coordinates value is null.");
                            geos.Add(CreateMultiPoint(obj.Coordinates, obj.Properties)); break;
                        case "LineString":
                            if (obj.Arcs == null)
                                throw new IOException("Geometry arcs value is null.");
                            geos.Add(CreateLineString(obj.Arcs, obj.Properties)); break;
                        case "MultiLineString":
                            if (obj.Arcs == null)
                                throw new IOException("Geometry arcs value is null.");
                            geos.Add(CreateMultiLineString(obj.Arcs, obj.Properties)); break;
                        case "Polygon":
                            if (obj.Arcs == null)
                                throw new IOException("Geometry arcs value is null.");
                            geos.Add(CreatePolygon(obj.Arcs, obj.Properties)); break;
                        case "MultiPolygon":
                            if (obj.Arcs == null)
                                throw new IOException("Geometry arcs value is null.");
                            geos.Add(CreateMultiPolygon(obj.Arcs, obj.Properties)); break;
                        case "GeometryCollection":
                            if (obj.Geometries == null)
                                throw new IOException("GeometryCollection geometries value is null.");
                            geos.Add(CreateGeometryCollection(obj.Geometries, obj.Properties)); break;
                        default: throw new IOException("Not supported geometry type: " + obj.Type + ".");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentReadError, ex);
            }
            return Factory.CreateGeometryCollection(geos, properties);
        }

        #endregion
    }
}
