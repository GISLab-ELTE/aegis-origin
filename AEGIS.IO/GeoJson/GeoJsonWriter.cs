// <copyright file="GeoJsonWriter.cs" company="Eötvös Loránd University (ELTE)">
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.IO.GeoJSON
{
    /// <summary>
    /// Represents a GeoJSON format writer.
    /// </summary>
    /// <remarks>
    /// GeoJSON is a geospatial data interchange format based on JavaScript Object Notation (JSON).
    /// You can read about the format specification <a href="http://geojson.org/geojson-spec.html">here</a>.
    /// </remarks>
    /// <author>Norbert Vass</author>    
    [IdentifiedObjectInstance("AEGIS::610102", "GeoJSON file")]
    public class GeoJsonWriter : GeometryStreamWriter
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoJsonWriter" /> class.
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
        public GeoJsonWriter(String path) : base(path, GeometryStreamFormats.GeoJson, null)
        {
            try
            {
                _jsonwriter = new JsonTextWriter(new StreamWriter(_baseStream));
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentWriteError, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoJsonWriter" /> class.
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
        public GeoJsonWriter(Uri path) : base(path, GeometryStreamFormats.GeoJson, null)
        {
            try
            {
                _jsonwriter = new JsonTextWriter(new StreamWriter(_baseStream));
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
        /// The specified geometry does not match the the type of the GeoJSON file.;geometry
        /// </exception>
        protected override void ApplyWriteGeometry(IGeometry geometry)
        {
            if (_isFirst)
            {
                _isFirst = false;
                _jsonwriter.WriteStartObject();
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("FeatureCollection");
                _jsonwriter.WritePropertyName("features");
                _jsonwriter.WriteStartArray();
            }

            WriteFeature(geometry);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (!_isFirst)
            {
                _jsonwriter.WriteEndArray();
                _jsonwriter.WriteEndObject();
            }

            _jsonwriter.Close();

            base.Dispose(disposing);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Writes the Coordinate Reference System to GeoJSON file.
        /// </summary>
        /// <param name="system">The ReferenceSystem.</param>
        private void WriteCoordinateReferenceSystem(IReferenceSystem system)
        {
            _jsonwriter.WritePropertyName("crs");
            _jsonwriter.WriteStartObject();
            _jsonwriter.WritePropertyName("type");
            _jsonwriter.WriteValue("name");
            _jsonwriter.WritePropertyName("properties");
            _jsonwriter.WriteStartObject();
            _jsonwriter.WritePropertyName("name");
            _jsonwriter.WriteValue(system.Identifier);
            _jsonwriter.WriteEndObject();
            _jsonwriter.WriteEndObject();
        }

        /// <summary>
        /// Writes the Bounding Box array to GeoJSON file.
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
        /// Writes out the properties of a geometry to GeoJSON file.
        /// </summary>
        /// <param name="metadata">The metadata of a geometry.</param>
        private void WriteProperties(IMetadataCollection metadata)
        {
            if (metadata == null)
                return;

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
        /// Writes out a geometry to GeoJSON file.
        /// </summary>
        /// <param name="geometry">The geometry to be written.</param>
        private void WriteGeometry(IGeometry geometry)
        {
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
                _jsonwriter.WritePropertyName("coordinates");
                WriteLineString(geometry as ILineString);
            }
            else if (geometry is IMultiLineString)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("MultiLineString");
                _jsonwriter.WritePropertyName("coordinates");
                WriteMultiLineString(geometry as IMultiLineString);
            }
            else if (geometry is IPolygon)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("Polygon");
                _jsonwriter.WritePropertyName("coordinates");
                WritePolygon(geometry as IPolygon);
            }
            else if (geometry is IMultiPolygon)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("MultiPolygon");
                _jsonwriter.WritePropertyName("coordinates");
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
                throw new ArgumentException("Geometry type " + geometry.Name + " is not supported by GeoJSON.", "geometry");
        }

        /// <summary>
        /// Writes out a feature to GeoJSON file.
        /// </summary>
        /// <param name="geometry">The feature's geometry.</param>
        private void WriteFeature(IGeometry geometry)
        {
            _jsonwriter.WriteStartObject();
            _jsonwriter.WritePropertyName("type");
            _jsonwriter.WriteValue("Feature");

            if (geometry.ReferenceSystem != null)
            {
                WriteCoordinateReferenceSystem(geometry.ReferenceSystem);
            }

            if (geometry.Envelope != null)
            {
                if (geometry.SpatialDimension == 2)
                    WriteEnvelope(geometry.Envelope, true);
                else if (geometry.SpatialDimension == 3)
                    WriteEnvelope(geometry.Envelope, false);
            }

            if (geometry.Metadata != null && geometry.Metadata.Count > 0)
            {
                if (geometry.Metadata.ContainsKey("OBJECTID") && geometry.Metadata["OBJECTID"] != null)
                {
                    _jsonwriter.WritePropertyName("id");
                    _jsonwriter.WriteValue(geometry.Metadata["OBJECTID"]);
                }

                _jsonwriter.WritePropertyName("properties");
                _jsonwriter.WriteStartObject();
                WriteProperties(geometry.Metadata);
                _jsonwriter.WriteEndObject();
            }

            _jsonwriter.WritePropertyName("geometry");
            _jsonwriter.WriteStartObject();

            WriteGeometry(geometry);

            _jsonwriter.WriteEndObject();
            _jsonwriter.WriteEndObject();
        }

        /// <summary>
        /// Writes out a point to GeoJSON file.
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
        /// Writes out a point to GeoJSON file.
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
        /// Writes out a MultiPoint to GeoJSON file.
        /// </summary>
        /// <param name="point">The MultiPoint.</param>
        private void WriteMultiPoint(IMultiPoint point)
        {
            _jsonwriter.WriteStartArray();

            for (int i = 0; i < point.Count; i++ )
            {
                WritePoint(point[i]);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a LineString to GeoJSON file.
        /// </summary>
        /// <param name="lstr">The LineString.</param>
        private void WriteLineString(ILineString lstr)
        {
            bool is3D = lstr.SpatialDimension == 3;
            _jsonwriter.WriteStartArray();

            foreach (Coordinate coord in lstr)
            {
                WritePoint(coord, is3D);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a MultiLineString to GeoJSON file.
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
        /// Writes out a Polygon to GeoJSON file.
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
        /// Writes out a MultiPolygon to GeoJSON file.
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
        /// Writes out a GeometryCollection to GeoJSON file.
        /// </summary>
        /// <param name="collection">The collection.</param>
        private void WriteGeometryCollection(IGeometryCollection<IGeometry> collection)
        {
            _jsonwriter.WriteStartArray();

            foreach (IGeometry geometry in collection)
            {
                _jsonwriter.WriteStartObject();
                WriteGeometry(geometry);
                _jsonwriter.WriteEndObject();
            }
            _jsonwriter.WriteEndArray();
        }

        #endregion
    }
}
