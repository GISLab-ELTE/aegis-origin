/// <copyright file="GeoJsonConverter.cs" company="Eötvös Loránd University (ELTE)">
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace ELTE.AEGIS.IO.GeoJSON
{
    public static class GeoJsonConverter
    {
        #region Private static fields

        /// <summary>
        /// Used to write to the output json file.
        /// </summary>
        private static JsonTextWriter _jsonwriter;

        /// <summary>
        /// Coordinate Reference System for FeatureCollection, if exists
        /// </summary>
        private static IReferenceSystem _refSystem;

        #endregion

        #region Public conversion methods from Geometry to GeoJSON

        /// <summary>
        /// Converts Geometry to GeoJSON representation. Computes the FeatureCollection's 
        /// Coordinate Reference System (if needed) for optimal file size, then writes all geometries to file.
        /// </summary>
        /// <param name="path">The path for the output file.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The parameter is null.
        /// </exception>
        /// /// <exception cref="System.IOException">
        /// The path is invalid.
        /// </exception>
        public static void ToGeoJson(this IGeometry geometry, string path)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometries parameter is null");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path", "Path is null or contains only whitespaces");

            try
            {
                StreamWriter writer = new StreamWriter(path, false);
                _jsonwriter = new JsonTextWriter(writer);

                if (geometry is IPoint || geometry is IMultiPoint || geometry is ILineString || geometry is IMultiLineString ||
                    geometry is IPolygon || geometry is IMultiPolygon || geometry is IGeometryCollection<IGeometry>)
                    WriteFeature(geometry);
                else throw new NotSupportedException("Geometry type" + geometry.Name + " not supported.");
            }
            catch (IOException ioex)
            {
                throw new IOException("Wrong file path.", ioex);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Geometry content is invalid", ex);
            }
            finally
            {
                _jsonwriter.Close();
            }
        }

        /// <summary>
        /// Converts Geometry to GeoJSON representation. Computes the FeatureCollection's 
        /// Coordinate Reference System (if needed) for optimal file size, then writes all geometries to file.
        /// </summary>
        /// <param name="path">The path for the output file.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The parameter is null.
        /// </exception>
        /// /// <exception cref="System.IOException">
        /// The path is invalid.
        /// </exception>
        public static void ToGeoJson(this IList<IGeometry> geometries, string path)
        {
            if (geometries == null)
                throw new ArgumentNullException("geometry", "The geometries parameter is null");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path", "Path is null or contains only whitespaces");
            if (geometries.Count == 0)
                return;

            try
            {
                StreamWriter writer = new StreamWriter(path, false);
                _jsonwriter = new JsonTextWriter(writer);

                if (geometries.Count > 1)
                {
                    Dictionary<IReferenceSystem, int> counts = new Dictionary<IReferenceSystem, int>();

                    bool nullValueFound = false;
                    for (int i = 0; i < geometries.Count && !nullValueFound; i++)
                    {
                        if (geometries[i].ReferenceSystem == null)
                            nullValueFound = true;
                        else if (counts.ContainsKey(geometries[i].ReferenceSystem))
                            counts[geometries[i].ReferenceSystem] += 1;
                        else
                            counts.Add(geometries[i].ReferenceSystem, 1);
                    }

                    _jsonwriter.WriteStartObject();
                    _jsonwriter.WritePropertyName("type");
                    _jsonwriter.WriteValue("FeatureCollection");

                    if (!nullValueFound)
                    {
                        int max = 0;
                        foreach (var pair in counts)
                        {
                            if (pair.Value > max)
                            {
                                max = pair.Value;
                                _refSystem = pair.Key;
                            }
                        }
                        
                        WriteCoordinateReferenceSystem(_refSystem);
                    }

                    _jsonwriter.WritePropertyName("features");
                    _jsonwriter.WriteStartArray();

                    for (int i = 0; i < geometries.Count; i++)
                    {
                        WriteFeature(geometries[i]);
                    }

                    _jsonwriter.WriteEndArray();
                    _jsonwriter.WriteEndObject();

                    _jsonwriter.Close();
                }
                else WriteFeature(geometries[0]);
            }
            catch (IOException ioex)
            {
                throw new IOException("Wrong file path.", ioex);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Geometry content is invalid", ex);
            }
            finally
            {
                _jsonwriter.Close();
            }
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Writes the Coordinate Reference System to GeoJSON file.
        /// </summary>
        /// <param name="system">The ReferenceSystem.</param>
        private static void WriteCoordinateReferenceSystem(IReferenceSystem system)
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
        private static void WriteEnvelope(Envelope envelope, bool is2D)
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
        private static void WriteProperties(IMetadataCollection metadata)
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
        /// Writes out a feature to GeoJSON file.
        /// </summary>
        /// <param name="geometry">The feature's geometry.</param>
        private static void WriteFeature(IGeometry geometry)
        {
            _jsonwriter.WriteStartObject();
            _jsonwriter.WritePropertyName("type");
            _jsonwriter.WriteValue("Feature");

            if (geometry.Metadata != null && geometry.Metadata.Count > 0)
            {
                if (geometry.Metadata.ContainsKey("OBJECTID") && geometry.Metadata["OBJECTID"] != null)
                {
                    _jsonwriter.WritePropertyName("id");
                    _jsonwriter.WriteValue(geometry.Metadata["OBJECTID"]);
                }

                // "properties": {}     is also valid
                _jsonwriter.WritePropertyName("properties");
                _jsonwriter.WriteStartObject();
                WriteProperties(geometry.Metadata);
                _jsonwriter.WriteEndObject();
            }

            if ((geometry.ReferenceSystem != null) && (_refSystem == null || _refSystem != null && geometry.ReferenceSystem != _refSystem))
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

            _jsonwriter.WritePropertyName("geometry");
            _jsonwriter.WriteStartObject();

            WriteGeometry(geometry);

            _jsonwriter.WriteEndObject();
            _jsonwriter.WriteEndObject();
        }

        /// <summary>
        /// Writes out a geometry to GeoJSON file.
        /// </summary>
        /// <param name="geometry">The geometry to be written.</param>
        private static void WriteGeometry(IGeometry geometry)
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
        /// Writes out a point to GeoJSON file.
        /// </summary>
        /// <param name="point">The point.</param>
        private static void WritePoint(IPoint point)
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
        private static void WritePoint(Coordinate point, bool is3D)
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
        private static void WriteMultiPoint(IMultiPoint point)
        {
            _jsonwriter.WriteStartArray();

            for (int i = 0; i < point.Count; i++)
            {
                WritePoint(point[i]);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a LineString to GeoJSON file.
        /// </summary>
        /// <param name="lstr">The LineString.</param>
        private static void WriteLineString(ILineString lstr)
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
        private static void WriteMultiLineString(IMultiLineString mlstr)
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
        private static void WritePolygon(IPolygon polygon)
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
        private static void WriteMultiPolygon(IMultiPolygon polygon)
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
        private static void WriteGeometryCollection(IGeometryCollection<IGeometry> collection)
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
