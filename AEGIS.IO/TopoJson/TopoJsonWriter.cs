/// <copyright file="TopoJsonWriter.cs" company="Eötvös Loránd University (ELTE)">
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.TopoJSON
{
    /// <summary>
    /// Represents a TopoJSON format writer.
    /// </summary>    
    /// <remarks>
    /// TopoJSON is a topological geospatial data interchange format based on GeoJSON.
    /// You can read about the format specification <a href="https://github.com/mbostock/topojson-specification">here</a>.
    /// </remarks>

    [IdentifiedObjectInstance("AEGIS::610103", "TopoJSON file")]
    public class TopoJsonWriter : GeometryStreamWriter
    {
        #region Private fields

        /// <summary>
        /// Used to write to the output json file.
        /// </summary>
        private JsonTextWriter _jsonwriter;

        /// <summary>
        /// True, if we haven't write any geometry yet.
        /// </summary>
        private bool _isFirst = true;

        /// <summary>
        /// If a geometry has no name, the object's name containing it will be "object" + objectNum.ToString()
        /// </summary>
        private int _objectNum = 1;

        /// <summary>
        /// The arcs stored during writing. Will be written out after we wrote all geometries.
        /// </summary>
        private List<List<List<double>>> _arcs;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TopoJsonWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// </exception>
        public TopoJsonWriter(String path) : base(path, GeometryStreamFormats.TopoJson, null)
        {
            try
            {
                _jsonwriter = new JsonTextWriter(new StreamWriter(_baseStream));

                _arcs = new List<List<List<double>>>();
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentWriteError, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopoJsonWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occurred during stream opening.
        /// </exception>
        public TopoJsonWriter(Uri path) : base(path, GeometryStreamFormats.TopoJson, null)
        {
            try
            {
                _jsonwriter = new JsonTextWriter(new StreamWriter(_baseStream));

                _arcs = new List<List<List<double>>>();
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentWriteError, ex);
            }
        }

        #endregion

        #region GeometryStreamWriter protected methods

        /// <summary>
        /// Apply the write operation for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentException">
        /// The model of the specified geometry is not supported.;geometry
        /// or
        /// The specified geometry does not match the the type of the TopoJSON file.;geometry
        /// </exception>
        protected override void ApplyWriteGeometry(IGeometry geometry)
        { //This writing method doesn't quantize and simplify the elements. Also doesn't compute the bbox member.
            if (_isFirst)
            {
                _isFirst = false;
                _jsonwriter.WriteStartObject();
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("Topology");
                _jsonwriter.WritePropertyName("objects");
                _jsonwriter.WriteStartObject();
            }

            if (geometry.Metadata != null && geometry.Metadata.ContainsKey("NAME"))
                WriteObject(geometry, geometry.Metadata["NAME"].ToString());
            else
            {
                WriteObject(geometry, "object" + _objectNum.ToString());
                _objectNum++;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (!_isFirst)
            {
                _jsonwriter.WriteEndObject();

                _jsonwriter.WritePropertyName("arcs");
                _jsonwriter.WriteStartArray();
                if (_arcs.Count > 0)
                {
                    WriteArcs();
                }
                
                _jsonwriter.WriteEndArray();

                _jsonwriter.WriteEndObject();
            }

            _jsonwriter.Close();

            base.Dispose(disposing);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Writes out arcs array of the topology.
        /// </summary>
        private void WriteArcs()
        {
            for (int i = 0; i < _arcs.Count; i++)
            {
                _jsonwriter.WriteStartArray();
                for (int j = 0; j < _arcs[i].Count; j++)
                {
                    _jsonwriter.WriteStartArray();
                    for (int k = 0; k < _arcs[i][j].Count; k++)
                    {
                        _jsonwriter.WriteValue(_arcs[i][j][k]);
                    }
                    _jsonwriter.WriteEndArray();
                }
                _jsonwriter.WriteEndArray();
            }
        }

        /// <summary>
        /// Writes the Bounding Box array to TopoJSON file.
        /// </summary>
        /// <param name="envelope">The geometry's envelope.</param>
        /// <param name="is2D">The given envelope is 2 dimensional or not.</param>
        private void WriteEnvelope(Envelope envelope, bool is2D)
        {
            _jsonwriter.WritePropertyName("bbox");
            _jsonwriter.WriteStartArray();
            if (is2D)
            {
                _jsonwriter.WriteValue(envelope.MinX); _jsonwriter.WriteValue(envelope.MinY);
                _jsonwriter.WriteValue(envelope.MaxX); _jsonwriter.WriteValue(envelope.MaxY);
            }
            else
            {
                _jsonwriter.WriteValue(envelope.MinX); _jsonwriter.WriteValue(envelope.MinY);
                _jsonwriter.WriteValue(envelope.MinZ); _jsonwriter.WriteValue(envelope.MaxX);
                _jsonwriter.WriteValue(envelope.MaxY); _jsonwriter.WriteValue(envelope.MaxZ);
            }
            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out the properties of a geometry to TopoJSON file.
        /// </summary>
        /// <param name="metadata">The metadata of a geometry.</param>
        private void WriteProperties(IMetadataCollection metadata)
        {
            JsonSerializer serializer = new JsonSerializer();            

            foreach (KeyValuePair<string, object> k in metadata)
            {
                if (k.Key != "OBJECTID")
                {
                    _jsonwriter.WritePropertyName(k.Key);
                    serializer.Serialize(_jsonwriter, k.Value);
                }
            }
        }

        /// <summary>
        /// Writes out a geometry to TopoJSON file.
        /// </summary>
        /// <param name="geometry">The geometry to be written.</param>
        private void WriteGeometry(IGeometry geometry)
        {
            _jsonwriter.WriteStartObject();
            if (geometry is IPoint)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("Point");
                _jsonwriter.WritePropertyName("coordinates");
                WritePoint(geometry as IPoint);
            }
            else if (geometry is IMultiPoint)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("MultiPoint");
                _jsonwriter.WritePropertyName("coordinates");
                WriteMultiPoint(geometry as IMultiPoint);
            }
            else if (geometry is ILineString)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("LineString");
                _jsonwriter.WritePropertyName("arcs");
                WriteLineString(geometry as ILineString);
            }
            else if (geometry is IMultiLineString)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("MultiLineString");
                _jsonwriter.WritePropertyName("arcs");
                WriteMultiLineString(geometry as IMultiLineString);
            }
            else if (geometry is IPolygon)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("Polygon");
                _jsonwriter.WritePropertyName("arcs");
                WritePolygon(geometry as IPolygon);
            }
            else if (geometry is IMultiPolygon)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("MultiPolygon");
                _jsonwriter.WritePropertyName("arcs");
                WriteMultiPolygon(geometry as IMultiPolygon);
            }
            else if (geometry is IGeometryCollection<IGeometry>)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("GeometryCollection");
                _jsonwriter.WritePropertyName("geometries");
                WriteGeometryCollection(geometry as IGeometryCollection<IGeometry>);
            }
            else
                throw new ArgumentException("Geometry type " + geometry.Name + " is not supported by TopoJSON.", "geometry");

            if (geometry.Envelope != null)
            {
                if (geometry.SpatialDimension == 2)
                {
                    WriteEnvelope(geometry.Envelope, true);
                }
                else if (geometry.SpatialDimension == 3)
                {
                    WriteEnvelope(geometry.Envelope, false);
                }
            }

            bool l1 = geometry.Metadata != null && geometry.Metadata.Count > 0;
            bool l2 = geometry.ReferenceSystem != null;
            if (l1 || l2)
            {
                bool l3 = false;
                if (l1)
                {
                    int a = 0;
                    if (geometry.Metadata.ContainsKey("OBJECTID") && geometry.Metadata["OBJECTID"] != null)
                    {
                        a = 1;
                        _jsonwriter.WritePropertyName("id");
                        _jsonwriter.WriteValue(geometry.Metadata["OBJECTID"]);
                    }

                    l3 = geometry.Metadata.Count > 0 + a;
                }
                
                if (l2 || l3)
                {
                    _jsonwriter.WritePropertyName("properties");
                    _jsonwriter.WriteStartObject();

                    WriteProperties(geometry.Metadata);

                    if (l2)
                    {
                        _jsonwriter.WritePropertyName("crs");
                        _jsonwriter.WriteValue(geometry.ReferenceSystem.Identifier);
                    }

                    _jsonwriter.WriteEndObject();
                }
            }

            _jsonwriter.WriteEndObject();
        }

        /// <summary>
        /// Writes out an object of the topology to TopoJSON file.
        /// </summary>
        /// <param name="geometry">The object's geometry.</param>
        /// <param name="objectname">The object's name.</param>
        private void WriteObject(IGeometry geometry, string objectname)
        {
            _jsonwriter.WritePropertyName(objectname);

            WriteGeometry(geometry);
        }

        /// <summary>
        /// Writes out a point to TopoJSON file.
        /// </summary>
        /// <param name="point">The point.</param>
        private void WritePoint(IPoint point)
        {
            _jsonwriter.WriteStartArray();
            _jsonwriter.WriteValue(point.X);
            _jsonwriter.WriteValue(point.Y);

            if (point.SpatialDimension == 3)
                _jsonwriter.WriteValue(point.Z);

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a point to TopoJSON file.
        /// </summary>
        /// <param name="point">The coordinates of the point.</param>
        /// <param name="is3D">The given point is 3 dimensional or not.</param>
        private void WritePoint(Coordinate point, bool is3D)
        {
            _jsonwriter.WriteStartArray();
            _jsonwriter.WriteValue(point.X);
            _jsonwriter.WriteValue(point.Y);

            if (is3D)
                _jsonwriter.WriteValue(point.Z);

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a MultiPoint to TopoJSON file.
        /// </summary>
        /// <param name="point">The MultiPoint.</param>
        private void WriteMultiPoint(IMultiPoint point)
        {
            _jsonwriter.WriteStartArray();

            for (int i = 0; i < point.Count; i++)
            {
                WritePoint(point[i]);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a LineString to TopoJSON file.
        /// </summary>
        /// <param name="lstr">The LineString.</param>
        private void WriteLineString(ILineString lstr)
        {
            bool is3D = lstr.SpatialDimension == 3;
            IList<int> indexes = ComputeIndexes(lstr.Coordinates, is3D);
            _jsonwriter.WriteStartArray();

            for (int i = 0; i < indexes.Count; i++ )
            {
                _jsonwriter.WriteValue(indexes[i]);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a MultiLineString to TopoJSON file.
        /// </summary>
        /// <param name="mlstr">The MultiLineString.</param>
        private void WriteMultiLineString(IMultiLineString mlstr)
        {
            bool is3D = mlstr.SpatialDimension == 3;
            _jsonwriter.WriteStartArray();

            foreach (ILineString l in mlstr)
            {
                WriteLineString(l);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a Polygon to TopoJSON file.
        /// </summary>
        /// <param name="polygon">The Polygon.</param>
        private void WritePolygon(IPolygon polygon)
        {
            bool is3D = polygon.SpatialDimension == 3;
            _jsonwriter.WriteStartArray();

            WriteLineString(polygon.Shell);

            foreach (ILinearRing lr in polygon.Holes)
            {
                WriteLineString(lr);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a MultiPolygon to TopoJSON file.
        /// </summary>
        /// <param name="polygon">The MultiPolygon.</param>
        private void WriteMultiPolygon(IMultiPolygon polygon)
        {
            bool is3D = polygon.SpatialDimension == 3;
            _jsonwriter.WriteStartArray();

            foreach (IPolygon lr in polygon)
            {
                WritePolygon(lr);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a GeometryCollection to TopoJSON file.
        /// </summary>
        /// <param name="collection">The collection.</param>
        private void WriteGeometryCollection(IGeometryCollection<IGeometry> collection)
        {
            _jsonwriter.WriteStartArray();

            foreach (IGeometry geometry in collection)
            {
                WriteGeometry(geometry);
            }
            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Computes the and creates the arc index list of a LineString (indexes from the topology's arcs).
        /// </summary>
        /// <param name="coords">The coordinates.</param>
        /// <param name="is3D">The given coordinates are 3 dimensionals or not.</param>
        /// <returns></returns>
        private IList<int> ComputeIndexes(IList<Coordinate> coords, bool is3D)
        {
            List<int> indexes = new List<int>();

            bool l = false;
            int startIndex = 0;
            for (int i = 0; i < _arcs.Count && !l; i++)
            {
                int prev = startIndex;
                if (IsMatchingArcs(coords, _arcs[i], is3D, ref startIndex))
                {
                    indexes.Add(i);
                    if (startIndex == coords.Count)
                        l = true;
                    else
                        startIndex--;
                }
                else
                {
                    startIndex = prev;
                    if (IsMatchingArcs(coords, _arcs[i].AsEnumerable().Reverse().ToList(), is3D, ref startIndex))
                    {
                        indexes.Add(i * -1 - 1);
                        if (startIndex == coords.Count)
                            l = true;
                        else
                            startIndex--;
                    }
                    else
                        startIndex = prev;
                }
            }

            if (!l)
            {
                List<List<double>> arc = new List<List<double>>();
                for (int i = startIndex; i < coords.Count; i++)
                {
                    List<double> coord = new List<double>() { coords[i].X, coords[i].Y };
                    if (is3D)
                        coord.Add(coords[i].Z);

                    arc.Add(coord);
                }

                _arcs.Add(arc);

                indexes.Add(_arcs.Count - 1);
            }

            return indexes;
        }

        /// <summary>
        /// Determines whether two arcs are the same or not.
        /// </summary>
        /// <param name="coords">The first arc.</param>
        /// <param name="arc">The second arc.</param>
        /// <param name="is3D">The given arcs are 3 dimensionals or not.</param>
        /// <param name="startIndex">The starting index from where we want to start the check in the coords list.</param>
        /// <returns></returns>
        private bool IsMatchingArcs(IList<Coordinate> coords, List<List<double>> arc, bool is3D, ref int startIndex)
        {
            for (int j = 0; j < arc.Count; j++)
            {
                if (coords.Count > startIndex)
                {
                    List<double> coord = new List<double>() { coords[startIndex].X, coords[startIndex].Y };
                    if (is3D)
                        coord.Add(coords[startIndex].Z);

                    if (coord.SequenceEqual(arc[j]))
                        startIndex++;
                    else
                    {
                        startIndex = startIndex - j + 1;
                        return false;
                    }
                }
                else
                {
                    startIndex = startIndex - j + 1;
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
