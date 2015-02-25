/// <copyright file="WellKnownTextConverter.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.IO.WellKnown
{
    /// <summary>
    /// Represents a converter for Well-known Text (WKT) representation.
    /// </summary>
    public static class WellKnownTextConverter
    {
        #region Private types

        /// <summary>
        /// Represents a delimiter within the Well-Known Text.
        /// </summary>
        private class Delimiter
        {
            #region Private Fields

            private readonly Char _left;
            private readonly Char _right;

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets the left delimiter.
            /// </summary>
            /// <value>The left delimiter character.</value>
            public Char Left { get { return _left; } }

            /// <summary>
            /// Gets the right delimiter.
            /// </summary>
            /// <value>The right delimiter character.</value>
            public Char Right { get { return _right; } }

            #endregion

            #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="Delimiter" /> class.
            /// </summary>
            /// <param name="leftDelimiter">The left delimiter character.</param>
            /// <exception cref="System.ArgumentException">The delimiter is invalid.</exception>
            public Delimiter(Char leftDelimiter)
            {
                if (leftDelimiter != '[' && leftDelimiter != '(')
                    throw new ArgumentException("The delimiter is invalid.", "leftDelimiter");

                if (leftDelimiter == '[')
                {
                    _left = '[';
                    _right = ']';
                }
                else if (leftDelimiter == '(')
                {
                    _left = '(';
                    _right = ')';
                }
            }

            #endregion
        }

        #endregion

        #region Public conversion methods from geometry to WKT

        /// <summary>
        /// Convert to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">Conversion not supported with the specified geometry type.</exception>
        public static String ToWellKnownText(this IGeometry geometry)
        {
            return ToWellKnownText(geometry, GeometryModel.Spatial3D);
        }

        /// <summary>
        /// Convert to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// Only 2D and 3D spatial conversion is supported for Well-Known Text.
        /// or
        /// Conversion not supported with the specified geometry type.
        /// </exception>
        public static String ToWellKnownText(this IGeometry geometry, GeometryModel geometryModel)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            if (geometryModel != GeometryModel.Spatial2D && geometryModel != GeometryModel.Spatial3D)
                throw new ArgumentException("Only 2D and 3D spatial conversion is supported for Well-Known Text.", "geometryModel");

            if (geometry is IPoint)
                return ComputeWellKnownText(geometry as IPoint, geometryModel);
            if (geometry is ILineString)
                return ComputeWellKnownText(geometry as ILineString, geometryModel);
            if (geometry is IPolygon)
                return ComputeWellKnownText(geometry as IPolygon, geometryModel);
            if (geometry is IMultiPoint)
                return ComputeWellKnownText(geometry as IMultiPoint, geometryModel);
            if (geometry is IMultiLineString)
                return ComputeWellKnownText(geometry as IMultiLineString, geometryModel);
            if (geometry is IMultiPolygon)
                return ComputeWellKnownText(geometry as IMultiPolygon, geometryModel);

            throw new ArgumentException("Conversion is not supported with the specified geometry type.", "geometry");
        }

        #endregion

        #region Public conversion methods from WKT to geometry

        /// <summary>
        /// Convert Well-known Text representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="text">The source text.</param>
        /// <param name="referenceSystem">The reference system of the geometry.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The text is null.</exception>
        /// <exception cref="System.ArgumentException">The text is empty.</exception>
        /// <exception cref="System.IO.InvalidDataException">The content of the text is invalid.</exception>
        public static IGeometry ToGeometry(String text, IReferenceSystem referenceSystem)
        {
            return ToGeometry(text, Factory.GetInstance<IGeometryFactory>(referenceSystem));
        }

        /// <summary>
        /// Convert Well-known Text representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="text">The source text.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The text is null.</exception>
        /// <exception cref="System.ArgumentException">The text is empty.</exception>
        /// <exception cref="System.IO.InvalidDataException">The content of the text is invalid.</exception>
        public static IGeometry ToGeometry(String text, IGeometryFactory factory)
        {
            if (text == null)
                throw new ArgumentNullException("text", "The text is null.");

            if (text == String.Empty)
                throw new ArgumentException("text", "The text is empty.");

            if (factory == null)
                factory = Factory.DefaultInstance<IGeometryFactory>();

            try
            {
                text = Regex.Replace(text.ToUpper().Trim(), @"\s+", " ", RegexOptions.Multiline);

                Int32 geometryContentStartIndex = text.IndexOf("("); // look for parenthesis
                String geometryType, geometryContent;
                if (geometryContentStartIndex > -1)
                {
                    geometryType = text.Substring(0, geometryContentStartIndex).Trim();
                    geometryContent = text.Substring(geometryContentStartIndex + 1, text.Length - geometryContentStartIndex - 2).Trim();
                }
                else // there is no parenthesis if the geometry is empty
                {
                    geometryType = text.Substring(0, text.LastIndexOf(' ')).Trim();
                    geometryContent = text.Substring(text.LastIndexOf(' ')).Trim();
                }

                IGeometry resultGeometry = null;
                switch (geometryType)
                {
                    case "POINT":
                    case "POINT M":
                        resultGeometry = ComputePoint(geometryContent, GeometryModel.Spatial2D, factory);
                        break;
                    case "POINT Z":
                    case "POINT ZM":
                        resultGeometry = ComputePoint(geometryContent, GeometryModel.Spatial3D, factory);
                        break;
                    case "LINESTRING":
                    case "LINESTRING M":
                        resultGeometry = ComputeLineString(geometryContent, GeometryModel.Spatial2D, factory);
                        break;
                    case "LINESTRING Z":
                    case "LINESTRING ZM":
                        resultGeometry = ComputeLineString(geometryContent, GeometryModel.Spatial3D, factory);
                        break;
                    case "POLYGON":
                    case "POLYGON M":
                        resultGeometry = ComputePolygon(geometryContent, GeometryModel.Spatial2D, factory);
                        break;
                    case "POLYGON Z":
                    case "POLYGON ZM":
                        resultGeometry = ComputePolygon(geometryContent, GeometryModel.Spatial3D, factory);
                        break;
                    case "MULTIPOINT":
                    case "MULTIPOINT M":
                        resultGeometry = ComputeMultiPoint(geometryContent, GeometryModel.Spatial2D, factory);
                        break;
                    case "MULTIPOINT Z":
                    case "MULTIPOINT ZM":
                        resultGeometry = ComputeMultiPoint(geometryContent, GeometryModel.Spatial3D, factory);
                        break;
                    case "MULTILINESTRING":
                    case "MULTILINESTRING M":
                        resultGeometry = ComputeMultiLineString(geometryContent, GeometryModel.Spatial2D, factory);
                        break;
                    case "MULTILINESTRING Z":
                    case "MULTILINESTRING ZM":
                        resultGeometry = ComputeMultiLineString(geometryContent, GeometryModel.Spatial3D, factory);
                        break;
                    case "MULTIPOLYGON":
                    case "MULTIPOLYGON M":
                        resultGeometry = ComputeMultiPolygon(geometryContent, GeometryModel.Spatial2D, factory);
                        break;
                    case "MULTIPOLYGON Z":
                    case "MULTIPOLYGON ZM":
                        resultGeometry = ComputeMultiPolygon(geometryContent, GeometryModel.Spatial3D, factory);
                        break;
                }

                return resultGeometry;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("The content of the text is invalid.", ex);
            }
        }

        #endregion

        #region Public conversion methods from identified object to WKT

        /// <summary>
        /// Converts an identified object to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="identifiedObject">The identified object.</param>
        /// <returns>The WKT representation of the identified object.</returns>
        /// <exception cref="System.ArgumentNullException">The identified object is null.</exception>
        /// <exception cref="System.ArgumentException">Conversion is not supported with the specified identified object type.</exception>
        public static String ToWellKnownText(IdentifiedObject identifiedObject)
        {
            if (identifiedObject == null)
                throw new ArgumentNullException("identifiedObject", "The identified object is null.");

            if (identifiedObject is ProjectedCoordinateReferenceSystem)
                return ComputeText(identifiedObject as ProjectedCoordinateReferenceSystem);
            if (identifiedObject is GeographicCoordinateReferenceSystem)
                return ComputeText(identifiedObject as GeographicCoordinateReferenceSystem);
            if (identifiedObject is UnitOfMeasurement)
                return ComputeText(identifiedObject as UnitOfMeasurement);
            if (identifiedObject is GeodeticDatum)
                return ComputeText(identifiedObject as GeodeticDatum);
            if (identifiedObject is Ellipsoid)
                return ComputeText(identifiedObject as Ellipsoid);
            if (identifiedObject is Meridian)
                return ComputeText(identifiedObject as Meridian);

            throw new ArgumentException("Conversion is not supported with the specified identified object type.", "identifiedObject");
        }

        /// <summary>
        /// Converts a reference system to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The WKT representation of the reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The reference system is null.</exception>
        /// <exception cref="System.ArgumentException">Conversion is not supported with the specified refeence system type.</exception>
        public static String ToWellKnownText(IReferenceSystem referenceSystem)
        {
            if (referenceSystem == null)
                throw new ArgumentNullException("referenceSystem", "The reference system is null.");

            if (referenceSystem is ProjectedCoordinateReferenceSystem)
                return ComputeText(referenceSystem as ProjectedCoordinateReferenceSystem);
            if (referenceSystem is GeographicCoordinateReferenceSystem)
                return ComputeText(referenceSystem as GeographicCoordinateReferenceSystem);

            throw new ArgumentException("Conversion is not supported with the specified refeence system type.", "referenceSystem");
        }

        #endregion

        #region Public conversion methods from WKT to identified object

        /// <summary>
        /// Converts the specified Well-known Text to identified object.
        /// </summary>
        /// <param name="text">The text representation of the object.</param>
        /// <returns>The identified object instance.</returns>
        /// <exception cref="System.ArgumentNullException">The text is null.</exception>
        /// <exception cref="System.ArgumentException">The text is empty.</exception>
        /// <exception cref="System.IO.InvalidDataException">The text is invalid.</exception>
        /// <exception cref="System.NotSupportedException">Geocentric coordinate reference systems are not supported.</exception>
        public static IdentifiedObject ToIdentifiedObject(String text)
        {
            Delimiter delim;

            if (text == null)
                throw new ArgumentNullException("text", "The text is null.");
            if (String.IsNullOrEmpty(text))
                throw new ArgumentException("The text is empty.", "text");

            text = Regex.Replace(text.Trim(), @"\s+", "", RegexOptions.Multiline);

            // determining the delimiter used by the WKT representation
            Int32 contextStartIndex = text.IndexOf("[");
            if (contextStartIndex == -1)
            {
                contextStartIndex = text.IndexOf("(");
                if (contextStartIndex == -1)
                    throw new InvalidDataException("The text is invalid.");
                else
                    delim = new Delimiter('(');
            }
            else
                delim = new Delimiter('[');

            String type = text.Substring(0, contextStartIndex);

            try
            {
                String content = text.Substring(contextStartIndex + 1, text.Length - contextStartIndex - 2);

                switch (type)
                {
                    case "PROJCS":
                        return ComputeProjectedReferenceSystem(content, delim);
                    case "GEOGCS":
                        return ComputeGeographicCoordinateReferenceSystem(content, delim);
                    case "DATUM":
                        return ComputeGeodeticDatum(content, delim);
                    case "SPHEROID":
                    case "ELLIPSOID":
                        return ComputeEllipsoid(content);
                    case "PRIMEM":
                        return ComputePrimeMeridian(content);
                    case "GEOCCS":
                        throw new NotSupportedException("Geocentric coordinate reference systems are not supported.");
                    default:
                        throw new InvalidDataException("The text is invalid.");
                }
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("The text is invalid.", ex);
            }
        }

        /// <summary>
        /// Converts the specified Well-known Text to reference system.
        /// </summary>
        /// <param name="text">The text representation of the reference system.</param>
        /// <returns>The reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The text is null.</exception>
        /// <exception cref="System.ArgumentException">The text is empty.</exception>
        /// <exception cref="System.IO.InvalidDataException">The text is invalid.</exception>
        /// <exception cref="System.NotSupportedException">Geocentric coordinate reference systems are not supported.</exception>
        public static IReferenceSystem ToReferenceSystem(String text)
        {
            Delimiter delim;

            if (text == null)
                throw new ArgumentNullException("text", "The text is null.");

            if (String.IsNullOrEmpty(text))
                throw new ArgumentException("The text is empty.", "text");

            text = Regex.Replace(text.Trim(), @"\s+", "", RegexOptions.Multiline);

            // determining the delimiter used by the WKT representation
            Int32 contextStartIndex = text.IndexOf("[");
            if (contextStartIndex == -1)
            {
                contextStartIndex = text.IndexOf("(");
                if (contextStartIndex == -1)
                    throw new InvalidDataException("The text is invalid.");
                else
                    delim = new Delimiter('(');
            }
            else
                delim = new Delimiter('[');

            String type = text.Substring(0, contextStartIndex);

            try
            {
                String content = text.Substring(contextStartIndex + 1, text.Length - contextStartIndex - 2);

                switch (type)
                {
                    case "PROJCS":
                        return ComputeProjectedReferenceSystem(content, delim);
                    case "GEOGCS":
                        return ComputeGeographicCoordinateReferenceSystem(content, delim);
                    case "GEOCCS":
                        throw new NotSupportedException("Geocentric coordinate reference systems are not supported.");
                    default:
                        throw new InvalidDataException("The text is invalid.");
                }
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("The text is invalid.", ex);
            }
        }

        #endregion

        #region Private conversion methods from geometry to WKT

        /// <summary>
        /// Computes the Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
        private static String ComputeWellKnownText(IPoint geometry, GeometryModel geometryModel)
        {
            if (geometryModel == GeometryModel.Spatial3D)
            {
                return String.Format("POINT Z ({0} {1} {2})", geometry.X.ToString("G", CultureInfo.InvariantCulture), geometry.Y.ToString("G", CultureInfo.InvariantCulture), geometry.Z.ToString("G", CultureInfo.InvariantCulture));
            }
            else
            {
                return String.Format("POINT ({0} {1})", geometry.X.ToString("G", CultureInfo.InvariantCulture), geometry.Y.ToString("G", CultureInfo.InvariantCulture));
            }
        }
        /// <summary>
        /// Computes the Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
        private static String ComputeWellKnownText(ILineString geometry, GeometryModel geometryModel)
        {
            StringBuilder builder = new StringBuilder(20 + (geometryModel == GeometryModel.Spatial2D ? 20 : 30) * geometry.Count);
            if (geometryModel == GeometryModel.Spatial3D)
                builder.Append("LINESTRING Z(");
            else
                builder.Append("LINESTRING (");

            ComputeCoordinateList(builder, geometry.Coordinates, geometryModel);
            builder.Append(")");

            return builder.ToString();
        }
        /// <summary>
        /// Computes the Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
        private static String ComputeWellKnownText(IPolygon geometry, GeometryModel geometryModel)
        {
            StringBuilder builder = new StringBuilder(15 + (geometryModel == GeometryModel.Spatial2D ? 20 : 30) * (geometry.Shell.Count + (geometry.Holes != null ? geometry.Holes.Sum(hole => hole.Count) : 0)));
            if (geometryModel == GeometryModel.Spatial3D)
                builder.Append("POLYGON Z((");
            else
                builder.Append("POLYGON ((");

            ComputeCoordinateList(builder, geometry.Shell.Coordinates, geometryModel);
            builder.Append(")");

            for (Int32 j = 0; j < geometry.HoleCount; j++)
            {
                builder.Append(", (");
                ComputeCoordinateList(builder, geometry.Holes[j].Coordinates, geometryModel);
                builder.Append(")");
            }
            builder.Append(")");
            return builder.ToString();        
        }
        /// <summary>
        /// Computes the Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
        private static String ComputeWellKnownText(IMultiPoint geometry, GeometryModel geometryModel)
        {
            StringBuilder builder = new StringBuilder(15 + (geometryModel == GeometryModel.Spatial2D ? 20 : 30) * geometry.Count);
            if (geometryModel == GeometryModel.Spatial3D)
                builder.Append("MULTIPOINT Z(");
            else
                builder.Append("MULTIPOINT (");

            if (geometryModel == GeometryModel.Spatial3D)
            {
                for (Int32 i = 0; i < geometry.Count - 1; i++)
                {
                    builder.Append(String.Format("{0} {1} {2}, ",
                                   geometry[i].X.ToString("G", CultureInfo.InvariantCulture),
                                   geometry[i].Y.ToString("G", CultureInfo.InvariantCulture),
                                   geometry[i].Z.ToString("G", CultureInfo.InvariantCulture)));
                }
                builder.Append(String.Format("{0} {1} {2}",
                               geometry[geometry.Count - 1].X.ToString("G", CultureInfo.InvariantCulture),
                               geometry[geometry.Count - 1].Y.ToString("G", CultureInfo.InvariantCulture),
                               geometry[geometry.Count - 1].Z.ToString("G", CultureInfo.InvariantCulture)));
            }
            else
            {
                for (Int32 i = 0; i < geometry.Count - 1; i++)
                {
                    builder.Append(String.Format("{0} {1}, ",
                                   geometry[i].X.ToString("G", CultureInfo.InvariantCulture),
                                   geometry[i].Y.ToString("G", CultureInfo.InvariantCulture)));
                }
                builder.Append(String.Format("{0} {1}",
                               geometry[geometry.Count - 1].X.ToString("G", CultureInfo.InvariantCulture),
                               geometry[geometry.Count - 1].Y.ToString("G", CultureInfo.InvariantCulture)));
            }

            builder.Append(")");

            return builder.ToString();
        }
        /// <summary>
        /// Computes the Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
        private static String ComputeWellKnownText(IMultiLineString geometry, GeometryModel geometryModel)
        {
            StringBuilder builder = new StringBuilder(20 + (geometryModel == GeometryModel.Spatial2D ? 20 : 30) * geometry.Sum(lineString => lineString.Count));
            if (geometryModel == GeometryModel.Spatial3D)
                builder.Append("MULTILINESTRING Z(");
            else
                builder.Append("MULTILINESTRING (");

            for (Int32 i = 0; i < geometry.Count; i++)
            {
                if (i > 0)
                    builder.Append(", ");
                builder.Append("(");
                ComputeCoordinateList(builder, geometry[i].Coordinates, geometryModel);
                builder.Append(")");
            }
            builder.Append(")");

            return builder.ToString();
        }
        /// <summary>
        /// Computes the Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
        private static String ComputeWellKnownText(IMultiPolygon geometry, GeometryModel geometryModel)
        {
            StringBuilder builder = new StringBuilder(20 + (geometryModel == GeometryModel.Spatial2D ? 20 : 30) * geometry.Sum(polygon => (polygon.Shell.Count + (polygon.Holes != null ? polygon.Holes.Sum(hole => hole.Count) : 0))));
            if (geometryModel == GeometryModel.Spatial3D)
                builder.Append("MULTIPOLYGON Z(");
            else
                builder.Append("MULTIPOLYGON (");

            for (Int32 i = 0; i < geometry.Count; i++)
            {
                if (i > 0)
                    builder.Append(", ");
                builder.Append("((");
                ComputeCoordinateList(builder, geometry[i].Shell.Coordinates, geometryModel);
                builder.Append(")");

                for (Int32 j = 0; j < geometry[i].HoleCount; j++)
                {
                    builder.Append(", (");
                    ComputeCoordinateList(builder, geometry[i].Holes[j].Coordinates, geometryModel);
                    builder.Append(")");
                }

                builder.Append(")");
            }
            builder.Append(")");

            return builder.ToString();
        }

        #endregion

        #region Private conversion methods from WKT to geometry

        /// <summary>
        /// Computes the <see cref="IPoint" /> representation of the WKT.
        /// </summary>
        /// <param name="geometryText">The WKT representation of the geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IPoint" /> representation of the geometry.</returns>
        private static IPoint ComputePoint(String geometryText, GeometryModel geometryModel, IGeometryFactory factory)
        {
            if (geometryText == "EMPTY")
                return factory.CreatePoint(0, 0, 0);
            else
            {
                String[] values = geometryText.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (geometryModel == GeometryModel.Spatial3D)
                {
                    return factory.CreatePoint(Double.Parse(values[0], CultureInfo.InvariantCulture),
                                               Double.Parse(values[1], CultureInfo.InvariantCulture),
                                               Double.Parse(values[2], CultureInfo.InvariantCulture));
                }
                else
                {
                    return factory.CreatePoint(Double.Parse(values[0], CultureInfo.InvariantCulture),
                                               Double.Parse(values[1], CultureInfo.InvariantCulture),
                                               0);
                }
            }
        }
        /// <summary>
        /// Computes the <see cref="ILineString" /> representation of the WKT.
        /// </summary>
        /// <param name="geometryText">The WKT representation of the geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="ILineString" /> representation of the geometry.</returns>
        private static ILineString ComputeLineString(String geometryText, GeometryModel geometryModel, IGeometryFactory factory)
        {
            Coordinate[] coordinates = null;

            if (geometryModel == GeometryModel.Spatial3D)
            {
                coordinates = geometryText.Split(',')
                                          .Select(text => text.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                                          .Select(array => new Coordinate(Double.Parse(array[0], CultureInfo.InvariantCulture), 
                                                                          Double.Parse(array[1], CultureInfo.InvariantCulture), 
                                                                          Double.Parse(array[2], CultureInfo.InvariantCulture))).ToArray();
            }
            else
            {
                coordinates = geometryText.Split(',')
                                          .Select(text => text.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                                          .Select(array => new Coordinate(Double.Parse(array[0], CultureInfo.InvariantCulture),
                                                                          Double.Parse(array[1], CultureInfo.InvariantCulture))).ToArray();
            }

            return factory.CreateLineString(coordinates);
        }
        /// <summary>
        /// Computes the <see cref="IPolygon" /> representation of the WKT.
        /// </summary>
        /// <param name="geometryText">The WKT representation of the geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IPolygon" /> representation of the geometry.</returns>
        private static IPolygon ComputePolygon(String geometryText, GeometryModel geometryModel, IGeometryFactory factory)
        {
            String[] partTexts = geometryText.Trim('(', ')').Split(new String[] { "),(", "), (" }, StringSplitOptions.RemoveEmptyEntries);

            Coordinate[] shellCoordinates = null;

            if (geometryModel == GeometryModel.Spatial3D)
            {
                shellCoordinates = partTexts[0].Split(',')
                                               .Select(text => text.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                                               .Select(array => new Coordinate(Double.Parse(array[0], CultureInfo.InvariantCulture),
                                                                               Double.Parse(array[1], CultureInfo.InvariantCulture),
                                                                               Double.Parse(array[2], CultureInfo.InvariantCulture))).ToArray();
            }
            else
            {
                shellCoordinates = partTexts[0].Split(',')
                                               .Select(text => text.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                                               .Select(array => new Coordinate(Double.Parse(array[0], CultureInfo.InvariantCulture),
                                                                               Double.Parse(array[1], CultureInfo.InvariantCulture))).ToArray();
            }
            Coordinate[][] holeCoordinates = new Coordinate[partTexts.Length - 1][];

            for (Int32 i = 1; i < partTexts.Length; i++)
            {
                if (geometryModel == GeometryModel.Spatial3D)
                {
                    holeCoordinates[i - 1] = partTexts[i].Split(',')
                                                         .Select(text => text.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                                                         .Select(array => new Coordinate(Double.Parse(array[0], CultureInfo.InvariantCulture),
                                                                                         Double.Parse(array[1], CultureInfo.InvariantCulture),
                                                                                         Double.Parse(array[2], CultureInfo.InvariantCulture))).ToArray();
                }
                else
                {
                    holeCoordinates[i - 1] = partTexts[i].Split(',')
                                                         .Select(text => text.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                                                         .Select(array => new Coordinate(Double.Parse(array[0], CultureInfo.InvariantCulture),
                                                                                         Double.Parse(array[1], CultureInfo.InvariantCulture))).ToArray();
                }
            }
            return factory.CreatePolygon(shellCoordinates, holeCoordinates);
        }
        /// <summary>
        /// Computes the <see cref="IMultiPoint" /> representation of the WKT.
        /// </summary>
        /// <param name="geometryText">The WKT representation of the geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiPoint" /> representation of the geometry.</returns>
        private static IMultiPoint ComputeMultiPoint(String geometryText, GeometryModel geometryModel, IGeometryFactory factory)
        {
            String[] partTexts = geometryText.Trim('(', ')').Split(new String[] { "),(", "), (", "," }, StringSplitOptions.RemoveEmptyEntries);

            return factory.CreateMultiPoint(partTexts.Select(partText => ComputePoint(partText, geometryModel, factory)));
        }
        /// <summary>
        /// Computes the <see cref="IMultiLineString" /> representation of the WKT.
        /// </summary>
        /// <param name="geometryText">The WKT representation of the geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiLineString" /> representation of the geometry.</returns>
        private static IMultiLineString ComputeMultiLineString(String geometryText, GeometryModel geometryModel, IGeometryFactory factory)
        {
            String[] partTexts = geometryText.Trim('(', ')').Split(new String[] { "),(", "), (" }, StringSplitOptions.RemoveEmptyEntries);

            return factory.CreateMultiLineString(partTexts.Select(partText => ComputeLineString(partText, geometryModel, factory)));
        }
        /// <summary>
        /// Computes the <see cref="IMultiPolygon" /> representation of the WKT.
        /// </summary>
        /// <param name="geometryText">The WKT representation of the geometry.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiPolygon" /> representation of the geometry.</returns>
        private static IMultiPolygon ComputeMultiPolygon(String geometryText, GeometryModel geometryModel, IGeometryFactory factory)
        {
            String[] partTexts = geometryText.Trim('(', ')').Split(new String[] { ")),((", ")), ((" }, StringSplitOptions.RemoveEmptyEntries);

            return factory.CreateMultiPolygon(partTexts.Select(partText => ComputePolygon(partText, geometryModel, factory)));
        }

        #endregion

        #region Private conversion methods from identified object to WKT

        /// <summary>
        /// Computes the WKT representation of the specified <see cref="ProjectedCoordinateReferenceSystem" />
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The WKT representation of the reference system.</returns>
        /// <exception cref="System.IO.InvalidDataException">The Linear Unit was not specified in the Projected Coordinate Reference System.</exception>
        private static String ComputeText(ProjectedCoordinateReferenceSystem referenceSystem)
        {
            StringBuilder result = new StringBuilder();
            result.Append("PROJCS[\"" + Regex.Replace(referenceSystem.Name, @"\s+", "_") + "\",");

            result.Append(ComputeText(referenceSystem.Base) + ",");

            result.Append("PROJECTION[\"" + Regex.Replace(referenceSystem.Projection.Name, @"\s+", "_") + "\"]" + ",");

            foreach (KeyValuePair<CoordinateOperationParameter, Object> param in referenceSystem.Projection.Parameters)
            {
                Double value;
                switch (param.Key.UnitType)
                {
                    case UnitQuantityType.Angle:
                        value = ((Angle)param.Value).Value;
                        break;
                    case UnitQuantityType.Length:
                        value = ((Length)param.Value).Value;
                        break;
                    default:
                        value = ((Double)param.Value);
                        break;
                }

                result.Append("PARAMETER[\"" + Regex.Replace(param.Key.Name, @"\s+", "_") + "\"," + value.ToString(CultureInfo.InvariantCulture) + "]");
            }

            Boolean foundLinearUnit = false;
            for (Int32 i = 0; i < referenceSystem.CoordinateSystem.Dimension && !foundLinearUnit; i++)
            {
                if (referenceSystem.CoordinateSystem[i].Unit.Type == UnitQuantityType.Length)
                {
                    result.Append(ComputeText(referenceSystem.CoordinateSystem[i].Unit));
                    foundLinearUnit = true;
                }
            }

            if (!foundLinearUnit)
                throw new InvalidDataException("The Linear Unit was not specified in the Projected Coordinate Reference System.");

            result.Append("]");

            return result.ToString();
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="GeographicCoordinateReferenceSystem" />.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The WKT representation of the reference system.</returns>
        /// <exception cref="System.IO.InvalidDataException">The angular unit is not specified in the Geodetic Coordinate Reference System.</exception>
        private static String ComputeText(GeographicCoordinateReferenceSystem referenceSystem)
        {
            StringBuilder result = new StringBuilder("GEOGCS[");

            result.Append("\"" + Regex.Replace(referenceSystem.Name, @"\s+", "_") + "\",");

            result.Append(ComputeText(referenceSystem.Datum as GeodeticDatum) + ",");
            result.Append(ComputeText((referenceSystem.Datum as GeodeticDatum).PrimeMeridian) + ",");

            UnitOfMeasurement linearUnit = null;
            UnitOfMeasurement angularUnit = null;

            for (Int32 i = 0; i < referenceSystem.CoordinateSystem.Dimension; i++)
            {
                if (referenceSystem.CoordinateSystem[i].Unit.Type == UnitQuantityType.Angle)
                    angularUnit = referenceSystem.CoordinateSystem[i].Unit;
                else if (referenceSystem.CoordinateSystem[i].Unit.Type == UnitQuantityType.Length)
                    linearUnit = referenceSystem.CoordinateSystem[i].Unit;
            }

            if (angularUnit == null)
                throw new InvalidDataException("The angular unit is not specified in the Geodetic Coordinate Reference System.");

            result.Append(ComputeText(angularUnit));

            if (linearUnit != null)
                result.Append("," + ComputeText(linearUnit));

            result.Append("]");
            return result.ToString();
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="UnitOfMeasurement" />.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The WKT representation of the unit of measurement.</returns>
        private static String ComputeText(UnitOfMeasurement unit)
        {
            return "UNIT[\"" + Regex.Replace(unit.Name, @"\s+", "_") + "\"," + unit.BaseMultiple.ToString(CultureInfo.InvariantCulture) + "]";
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="GeodeticDatum" />
        /// </summary>
        /// <param name="datum">The geodetic datum.</param>
        /// <returns>The WKT representation of the geodetic datum.</returns>
        private static String ComputeText(GeodeticDatum datum)
        {
            return "DATUM[\"" + Regex.Replace(datum.Name, @"\s+", "_") + "\"," + ComputeText(datum.Ellipsoid) + "]";
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="Ellipsoid" />.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The WKT representation of the ellipsoid.</returns>
        private static String ComputeText(Ellipsoid ellipsoid)
        {
            return "SPHEROID[\"" + Regex.Replace(ellipsoid.Name, @"\s+", "_") + "\"," +
                   ellipsoid.SemiMajorAxis.Value.ToString(CultureInfo.InvariantCulture) + "," +
                   ellipsoid.InverseFattening.ToString(CultureInfo.InvariantCulture) + "]";
        }
        /// <summary>
        /// Computes the WKT representation of the specified <see cref="Meridian" />.
        /// </summary>
        /// <param name="meridian">The meridian.</param>
        /// <returns>The WKT representation of the meridian.</returns>
        private static String ComputeText(Meridian meridian)
        {
            return "PRIMEM[\"" + Regex.Replace(meridian.Name, @"\s+", "_") + "\"," + meridian.Longitude.Value.ToString(CultureInfo.InvariantCulture) + "]";
        }

        #endregion

        #region Private conversion methods from WKT to identified object

        /// <summary>
        /// Gets the Geodetic Coordinate Reference System from the given WKT Text
        /// </summary>
        /// <param name="text">Well-Known Text representation of the Geodetic Coordinate Reference System.</param>
        /// <param name="delim">The used delimiter in the Well-Known Text.</param>
        /// <returns>The Geodetic Coordinate Reference System.</returns>
        /// <exception cref="System.IO.InvalidDataException">The geographic coordinate reference system text is invalid.</exception>
        private static GeographicCoordinateReferenceSystem ComputeGeographicCoordinateReferenceSystem(String text, Delimiter delim)
        {
            try
            {
                // getting the name of the reference system
                Int32 nameEndIndex = text.IndexOf("\"", 1);
                if (nameEndIndex == -1)
                    throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                Int32 nameStartIndex = (nameEndIndex > 4 && text.Substring(1, 4) == "GCS_") ? 5 : 1;

                String referenceSystemName = Regex.Replace(text.Substring(nameStartIndex, nameEndIndex - nameStartIndex), "_", " ");

                text = text.Substring(nameEndIndex + 1);

                // computing the datum
                if (text.Substring(0, 6) != ",DATUM" || text[6] != delim.Left)
                    throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                Int32 datumClosingBracketIndex = FindClosingBracket(text, 6, delim);
                String datumText = text.Substring(7, datumClosingBracketIndex - 7);

                GeodeticDatum datum = ComputeGeodeticDatum(datumText, delim);
                text = text.Substring(datumClosingBracketIndex + 1);

                // computing the prime meridian
                if (text.Substring(0, 7) != ",PRIMEM" || text[7] != delim.Left)
                    throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                Int32 meridianTextLength = FindClosingBracket(text, 7, delim) - 8;

                // if the datum is not a known geodetic datum, then it should be extended with the prime meridian
                if (GeodeticDatums.FromIdentifier(datum.Identifier).FirstOrDefault() == null)
                {
                    String primeMeridianText = text.Substring(8, meridianTextLength);

                    Meridian primeMeridian = ComputePrimeMeridian(primeMeridianText);

                    GeodeticDatum tmp = datum;
                    datum = new GeodeticDatum(datum.Identifier, datum.Name,
                                              datum.AnchorPoint, datum.RealizationEpoch,
                                              datum.AreaOfUse, datum.Ellipsoid, primeMeridian);
                }

                // computing the angular unit
                text = text.Substring(9 + meridianTextLength);
                if (text.Substring(0, 5) != ",UNIT" || text[5] != delim.Left)
                    throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                Int32 angularUnitClosingBracketIndex = FindClosingBracket(text, 5, delim);
                String angularUnitText = text.Substring(6, angularUnitClosingBracketIndex - 6);

                UnitOfMeasurement angularUnit = ComputeAngularUnit(angularUnitText);

                text = text.Substring(angularUnitClosingBracketIndex + 1);

                // computing the coordinate system
                CoordinateSystem coordinateSystem;

                if (!String.IsNullOrEmpty(text))
                {
                    // three-dimensional coordinate system 

                    if (!String.IsNullOrWhiteSpace(referenceSystemName))
                    {
                        // if it matches one of the known reference systems, it can be returned
                        GeographicCoordinateReferenceSystem referenceSystem = Geographic3DCoordinateReferenceSystems.FromName(referenceSystemName).FirstOrDefault();
                        if (referenceSystem != null)
                            return referenceSystem;
                    }

                    if (text.Substring(0, 5) != ",UNIT" || text[5] != delim.Left || text[text.Length] != delim.Right)
                        throw new InvalidDataException("The geographic coordinate reference system text is invalid.");

                    // computing the optional linear unit
                    String linearUnitText = text.Substring(6, text.Length - 7);
                    UnitOfMeasurement linearUnit = ComputeLinearUnit(linearUnitText);

                    coordinateSystem = new CoordinateSystem(CoordinateSystem.UserDefinedIdentifier, CoordinateSystem.UserDefinedName,
                                                            CoordinateSystemType.Ellipsoidal,
                                                            CoordinateSystemAxisFactory.GeodeticLatitude(angularUnit),
                                                            CoordinateSystemAxisFactory.GeodeticLongitude(angularUnit),
                                                            CoordinateSystemAxisFactory.EllipsoidalHeight(linearUnit));

                }
                else
                {
                    // two-dimensional coordinate system

                    if (!String.IsNullOrWhiteSpace(referenceSystemName))
                    {
                        // if it matches one of the known reference systems, it can be returned
                        GeographicCoordinateReferenceSystem referenceSystem = Geographic2DCoordinateReferenceSystems.FromName(referenceSystemName).FirstOrDefault();
                        if (referenceSystem != null)
                            return referenceSystem;
                    }

                    coordinateSystem = new CoordinateSystem(CoordinateSystem.UserDefinedIdentifier, CoordinateSystem.UserDefinedName,
                                                            CoordinateSystemType.Ellipsoidal,
                                                            CoordinateSystemAxisFactory.GeodeticLatitude(angularUnit),
                                                            CoordinateSystemAxisFactory.GeodeticLongitude(angularUnit));
                }

                return new GeographicCoordinateReferenceSystem(GeographicCoordinateReferenceSystem.UserDefinedIdentifier, referenceSystemName, coordinateSystem, datum, AreasOfUse.World);
            }
            catch
            {
                throw new InvalidDataException("The geographic coordinate reference system text is invalid.");
            }
        }

        /// <summary>
        /// Gets the Projected Reference System from the given WKT Text
        /// </summary>
        /// <param name="referenceSystemText">Well-Known Text representation of a Projected Coordinate Reference System.</param>
        /// <param name="delim">The Used delimiter in the Well-Known Text.</param>
        /// <returns>The Projected Coordinate Reference System.</returns>
        /// <exception cref="System.IO.InvalidDataException">The projected coordinate reference system text is invalid.</exception>
        private static ProjectedCoordinateReferenceSystem ComputeProjectedReferenceSystem(String referenceSystemText, Delimiter delim)
        {
            try
            {
                // getting the name of the reference system
                Int32 nameEndIndex = referenceSystemText.IndexOf('"', 1);
                if (nameEndIndex == -1)
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                String referenceSystemName = Regex.Replace(referenceSystemText.Substring(1, nameEndIndex - 1), "_", " ");
                referenceSystemText = referenceSystemText.Substring(nameEndIndex + 1);

                if (!String.IsNullOrWhiteSpace(referenceSystemName))
                {
                    // if it matches one of the known reference systems, it can be returned
                    ProjectedCoordinateReferenceSystem referenceSystem = ProjectedCoordinateReferenceSystems.FromName(referenceSystemName).FirstOrDefault();
                    if (referenceSystem != null)
                        return referenceSystem;
                }

                // computing the geodetic coordinate reference system
                if (referenceSystemText.Substring(0, 7) != ",GEOGCS" || referenceSystemText[7] != delim.Left)
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                Int32 geographicClosingBracketIndex = FindClosingBracket(referenceSystemText, 7, delim);
                String geographicText = referenceSystemText.Substring(8, geographicClosingBracketIndex - 8);

                GeographicCoordinateReferenceSystem geographicReferenceSystem = ComputeGeographicCoordinateReferenceSystem(geographicText, delim);
                referenceSystemText = referenceSystemText.Substring(geographicClosingBracketIndex + 1);

                // computing the projection
                if (referenceSystemText.Substring(0, 11) != ",PROJECTION" || referenceSystemText[11] != delim.Left)
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                Int32 projectionNameEndIndex = referenceSystemText.IndexOf('"', 13);
                String projectionName = Regex.Replace(referenceSystemText.Substring(13, projectionNameEndIndex - 13), "_", " ");

                if (String.IsNullOrWhiteSpace(projectionName))
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                referenceSystemText = referenceSystemText.Substring(projectionNameEndIndex + 2);

                // computing the projection parameters
                Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
                while (referenceSystemText.Substring(0, 10) == ",PARAMETER" && referenceSystemText[10] == delim.Left)
                {
                    Int32 closingBracketIndex = FindClosingBracket(referenceSystemText, 10, delim);

                    Int32 parameterNameEndIndex = referenceSystemText.IndexOf('"', 12);
                    String parameterName = Regex.Replace(referenceSystemText.Substring(12, parameterNameEndIndex - 12), "_", " ");

                    List<CoordinateOperationParameter> parameterList = CoordinateOperationParameters.FromName(parameterName).ToList();
                    foreach (CoordinateOperationParameter parameter in parameterList)
                    {
                        Double parameterValue = Double.Parse(referenceSystemText.Substring(parameterNameEndIndex + 2,
                                                closingBracketIndex - parameterNameEndIndex - 2), CultureInfo.InvariantCulture);

                        switch (parameter.UnitType)
                        {
                            case UnitQuantityType.Angle:
                                parameters.Add(parameter, Angle.FromRadian(parameterValue));
                                break;
                            case UnitQuantityType.Length:
                                parameters.Add(parameter, Length.FromMetre(parameterValue));
                                break;
                            default:
                                parameters.Add(parameter, parameterValue);
                                break;
                        }
                    }

                    referenceSystemText = referenceSystemText.Substring(closingBracketIndex + 1);
                }

                CoordinateProjection projection = CoordinateProjectionFactory.FromMethodName(projectionName, parameters, (geographicReferenceSystem.Datum as GeodeticDatum).Ellipsoid, AreasOfUse.World).FirstOrDefault();

                if (projection == null)
                    throw new InvalidDataException("The projection is unknown.");

                // computing the coordinate system
                if (referenceSystemText.Substring(0, 5) != ",UNIT" || referenceSystemText[5] != delim.Left)
                    throw new InvalidDataException("The projected coordinate reference system text is invalid.");

                Int32 unitBracketCloseIndex = FindClosingBracket(referenceSystemText, 5, delim);
                UnitOfMeasurement linearUnit = ComputeLinearUnit(referenceSystemText.Substring(6, unitBracketCloseIndex - 6));

                CoordinateSystem coordinateSystem = new CoordinateSystem(CoordinateSystem.UserDefinedIdentifier, CoordinateSystem.UserDefinedName,
                                                                         CoordinateSystemType.Cartesian,
                                                                         CoordinateSystemAxisFactory.Easting(linearUnit),
                                                                         CoordinateSystemAxisFactory.Northing(linearUnit));

                return new ProjectedCoordinateReferenceSystem(ProjectedCoordinateReferenceSystem.UserDefinedIdentifier, referenceSystemName, geographicReferenceSystem,
                                                              coordinateSystem, AreasOfUse.World, projection);
            }
            catch
            {
                throw new InvalidDataException("The projected coordinate reference system text is invalid.");
            }
        }

        /// <summary>
        /// Computes the linear unit of measurement from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the linear unit.</param>
        /// <returns>The linear unit of measurement.</returns>
        private static UnitOfMeasurement ComputeLinearUnit(String text)
        {
            return ComputeUnitOfMeasurement(text, UnitQuantityType.Length);
        }
        /// <summary>
        /// Computes the angular unit of measurement from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the angular unit.</param>
        /// <returns>The angular unit of measurement.</returns>
        private static UnitOfMeasurement ComputeAngularUnit(String text)
        {
            return ComputeUnitOfMeasurement(text, UnitQuantityType.Angle);
        }
        /// <summary>
        /// Computes the unit of measurement from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the unit.</param>
        /// <param name="unitQuantityType">Type of the unit, linear or angular.</param>
        /// <returns>The unit of measurement.</returns>
        /// <exception cref="System.IO.InvalidDataException">The unit of measurement text is invalid.</exception>
        private static UnitOfMeasurement ComputeUnitOfMeasurement(String text, UnitQuantityType unitQuantityType)
        {
            try
            {
                String[] splitText = text.Split(',');

                String unitName = Regex.Replace(splitText[0].Trim('"'), "_", " ");
                Double baseMultiple = Double.Parse(splitText[1], CultureInfo.InvariantCulture);

                return new UnitOfMeasurement(UnitOfMeasurement.UserDefinedIdentifier, unitName, null, baseMultiple, unitQuantityType);
            }
            catch
            {
                throw new InvalidDataException("The unit of measurement text is invalid.");
            }
        }

        /// <summary>
        /// Computes the prime meridian from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the prime meridian.</param>
        /// <returns>The prime meridian.</returns>
        private static Meridian ComputePrimeMeridian(String text)
        {
            try
            {
                Int32 meridianNameEndIndex = text.IndexOf('"', 1);
                String meridianName = Regex.Replace(text.Substring(1, meridianNameEndIndex - 1), "_", " ");

                // search for known meridian
                if (!String.IsNullOrWhiteSpace(meridianName))
                {
                    Meridian meridian = Meridians.FromName(meridianName).FirstOrDefault();
                    if (meridian != null)
                        return meridian;
                }

                String longitudeString = text.Substring(meridianNameEndIndex + 2,
                    text.Length - meridianNameEndIndex - 2);

                Double longitude = Double.Parse(longitudeString, CultureInfo.InvariantCulture);

                return new Meridian(Meridian.UserDefinedIdentifier, meridianName, Angle.FromRadian(longitude));
            }
            catch
            {
                throw new InvalidDataException("The prime meridian text is invalid.");
            }
        }

        /// <summary>
        /// Computes the geodetic datum from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the geodetic datum.</param>
        /// <param name="delim">Used Delimiter.</param>
        /// <returns>The geodetic datum.</returns>
        /// <exception cref="System.IO.InvalidDataException">
        /// Invalid geodetic datum text.
        /// or
        /// The geodetic datum text is invalid.
        /// </exception>
        private static GeodeticDatum ComputeGeodeticDatum(String text, Delimiter delim)
        {
            try
            {
                Int32 nameEndIndex = text.IndexOf("\"", 1);
                Int32 nameStartIndex = (nameEndIndex > 2 && text.Substring(1, 2) == "D_") ? 3 : 1;

                String datumName = Regex.Replace(text.Substring(nameStartIndex, nameEndIndex - nameStartIndex), "_", " ");

                // seach for known geodetic datum
                if (!String.IsNullOrWhiteSpace(datumName))
                {
                    GeodeticDatum datum = GeodeticDatums.FromName(datumName).FirstOrDefault();
                    if (datum != null)
                        return datum;
                }

                String ellipsoidText;
                if (text.Substring(nameEndIndex + 1, 9) == ",SPHEROID")
                    ellipsoidText = text.Substring(nameEndIndex + 11, text.Length - (nameEndIndex + 11) - 1);
                else if (text.Substring(nameEndIndex + 1, 10) == ",ELLIPSOID")
                    ellipsoidText = text.Substring(nameEndIndex + 12, text.Length - (nameEndIndex + 12) - 1);
                else
                    throw new InvalidDataException("Invalid geodetic datum text.");

                return new GeodeticDatum(GeodeticDatum.UserDefinedIdentifier, datumName, null, null, AreasOfUse.World, ComputeEllipsoid(ellipsoidText), null);
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch
            {
                throw new InvalidDataException("The geodetic datum text is invalid.");
            }
        }

        /// <summary>
        /// Computes the ellipsoid from the WKT.
        /// </summary>
        /// <param name="text">The WKT representation of the ellipsoid.</param>
        /// <returns>The ellipsoid.</returns>
        /// <exception cref="System.IO.InvalidDataException">The ellipsoid text is invalid.</exception>
        private static Ellipsoid ComputeEllipsoid(String text)
        {
            try
            {
                Int32 nameEndIndex = text.IndexOf("\"", 1);

                String ellipsoidName = Regex.Replace(text.Substring(1, nameEndIndex - 1), "_", " ");

                if (!String.IsNullOrWhiteSpace(ellipsoidName))
                {
                    Ellipsoid ellipsoid = Ellipsoids.FromName(ellipsoidName).FirstOrDefault();
                    if (ellipsoid != null)
                        return ellipsoid;
                }

                // the first value is the semi-major axis, the second is the inverse flattening
                String[] axisAndFlattening = text.Substring(nameEndIndex + 2, text.Length - nameEndIndex - 2).Split(',').ToArray();
                Double semiMajorAxis, inverseFlattening;

                semiMajorAxis = Double.Parse(axisAndFlattening[0], CultureInfo.InvariantCulture);
                inverseFlattening = Double.Parse(axisAndFlattening[1], CultureInfo.InvariantCulture);

                return Ellipsoid.FromInverseFlattening(String.Empty, ellipsoidName, semiMajorAxis, inverseFlattening);
            }
            catch
            {
                throw new InvalidDataException("The ellipsoid text is invalid.");
            }
        }

        #endregion

        #region Private utility methods

        /// <summary>
        /// Computes the WKT representation of a coordinate list.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="coordinateList">The coordinate list.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        private static void ComputeCoordinateList(StringBuilder builder, IList<Coordinate> coordinateList, GeometryModel geometryModel)
        {
            if (geometryModel == GeometryModel.Spatial3D)
            {
                for (Int32 i = 0; i < coordinateList.Count - 1; i++)
                {
                    builder.Append(String.Format("{0} {1} {2}, ",
                                   coordinateList[i].X.ToString("G", CultureInfo.InvariantCulture),
                                   coordinateList[i].Y.ToString("G", CultureInfo.InvariantCulture),
                                   coordinateList[i].Z.ToString("G", CultureInfo.InvariantCulture)));
                }
                builder.Append(String.Format("{0} {1} {2}",
                               coordinateList[coordinateList.Count - 1].X.ToString("G", CultureInfo.InvariantCulture),
                               coordinateList[coordinateList.Count - 1].Y.ToString("G", CultureInfo.InvariantCulture),
                               coordinateList[coordinateList.Count - 1].Z.ToString("G", CultureInfo.InvariantCulture)));
            }
            else
            {
                for (Int32 i = 0; i < coordinateList.Count - 1; i++)
                {
                    builder.Append(String.Format("{0} {1}, ",
                                   coordinateList[i].X.ToString("G", CultureInfo.InvariantCulture),
                                   coordinateList[i].Y.ToString("G", CultureInfo.InvariantCulture)));
                }
                builder.Append(String.Format("{0} {1}",
                               coordinateList[coordinateList.Count - 1].X.ToString("G", CultureInfo.InvariantCulture),
                               coordinateList[coordinateList.Count - 1].Y.ToString("G", CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        /// Finds the closing bracket for an opening bracket.
        /// </summary>
        /// <param name="text">The examined text.</param>
        /// <param name="openingBracketIndex">The zero-based index of the opening bracket.</param>
        /// <param name="delim">The used delimiter.</param>
        /// <returns>The index of the closing bracket.</returns>
        /// <exception cref="System.IO.InvalidDataException">
        /// There is no opening bracket at the given index position.
        /// or
        /// There is no closing bracket at the given index position.
        /// </exception>
        private static Int32 FindClosingBracket(String text, Int32 openingBracketIndex, Delimiter delim)
        {
            if (text[openingBracketIndex] != delim.Left)
                throw new InvalidDataException("There is no opening bracket at the given index position.");

            Int32 openedBrackets = 1;
            openingBracketIndex++;

            while (openedBrackets > 0 && openingBracketIndex < text.Length)
            {
                if (text[openingBracketIndex] == delim.Left)
                    openedBrackets++;
                else if (text[openingBracketIndex] == delim.Right)
                    openedBrackets--;

                if (openedBrackets == 0)
                    return openingBracketIndex;

                openingBracketIndex++;
            }

            throw new InvalidDataException("There is no closing bracket at the given index position.");
        }
        #endregion
    }
}
