/// <copyright file="GeometryConverter.WellKnownText.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.IO.WellKnown
{
    /// <summary>
    /// Represents a converter for Well-known Text (WKT) representation.
    /// </summary>
    public static partial class GeometryConverter
    {
        #region Public conversion methods from geometry to WKT

        /// <summary>
        /// Convert to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The WKT representation of the <paramref name="geometry" />.</returns>
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
        /// Only 2D and 3D spatial conversion is supported for Well-Known Text.;geometryModel
        /// or
        /// Conversion not supported with the specified geometry type.;geometry
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

            throw new ArgumentException("Conversion is not suppported with the specified geometry type.", "geometry");
        }

        #endregion

        #region Public conversion methods from WKT to geometry

        /// <summary>
        /// Convert Well-known Text representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="referenceSystem">The reference system of the geometry.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        public static IGeometry ToGeometry(String source, IReferenceSystem referenceSystem)
        {
            return ToGeometry(source, Factory.GetInstance<IGeometryFactory>(referenceSystem));
        }

        /// <summary>
        /// Convert Well-known Text representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        public static IGeometry ToGeometry(String source, IGeometryFactory factory)
        {
            if (source == null)
                throw new ArgumentNullException("geometryText", "The geometry text is null.");

            if (source == String.Empty)
                throw new ArgumentException("geometryText", "The geometry text cannot be empty.");

            if (factory == null)
                factory = Factory.DefaultInstance<IGeometryFactory>();

            try
            {
                source = Regex.Replace(source.ToUpper().Trim(), @"\s+", " ", RegexOptions.Multiline);

                Int32 geometryContentStartIndex = source.IndexOf("("); // look for parenthesis
                String geometryType, geometryContent;
                if (geometryContentStartIndex > -1)
                {
                    geometryType = source.Substring(0, geometryContentStartIndex).Trim();
                    geometryContent = source.Substring(geometryContentStartIndex + 1, source.Length - geometryContentStartIndex - 2).Trim();
                }
                else // there is no parenthesis if the geometry is empty
                {
                    geometryType = source.Substring(0, source.LastIndexOf(' ')).Trim();
                    geometryContent = source.Substring(source.LastIndexOf(' ')).Trim();
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
            catch
            {
                throw new ArgumentException("The content of the source is invalid.", "source");
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

        #region Private conversion utility methods

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

        #endregion
    }
}
