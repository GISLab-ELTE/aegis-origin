/// <copyright file="GeometryConverter.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Daniel Ballagi</author>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ELTE.AEGIS.IO.GeographyMarkup
{
    /// <summary>
    /// Represents a converter for GM (Geography Markup) representation.
    /// </summary>
    public static class GeographyMarkupConverter
    {
        #region Private constants

        /// <summary>
        /// The namespace.
        /// </summary>
        private static readonly XNamespace _nameSpace = "http://www.opengis.net/gml";

        #endregion

        #region Public conversion methods from geometry to GM

        /// <summary>
        /// Converts to Geography Markup (GM) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation in string of the <paramref name="geometry" />.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">Conversion not supported with the specified geometry type.</exception>
        public static String ToGeographyMarkupInString(this IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            try
            {
                if (geometry is IPoint)
                    return CreateGMString(geometry as IPoint, true);
                if (geometry is ILineString)
                    return CreateGMString(geometry as ILineString, true);
                if (geometry is ILinearRing)
                    return CreateGMString(geometry as ILinearRing, true);
                if (geometry is IPolygon)
                    return CreateGMString(geometry as IPolygon, true);
                if (geometry is IMultiPoint)
                    return CreateGMString(geometry as IMultiPoint, true);
                if (geometry is IMultiLineString)
                    return CreateGMString(geometry as IMultiLineString, true);
                if (geometry is IMultiPolygon)
                    return CreateGMString(geometry as IMultiPolygon, true);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Failed to convert Geometry to String.", "geometry", ex);
            }

            throw new ArgumentException("Conversion is not supported with the specified geometry type.", "geometry");
        }

        /// <summary>
        /// Converts to Geography Markup (GM) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation in XElement of the <paramref name="geometry" />.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">Conversion not supported with the specified geometry type.</exception>
        public static XElement ToGeographyMarkupInXElement(this IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("The geometry is null.", "geometry");

            try
            {
                if (geometry is IPoint)
                    return CreateGMXElement(geometry as IPoint);
                if (geometry is ILineString)
                    return CreateGMXElement(geometry as ILineString);
                if (geometry is ILinearRing)
                    return CreateGMXElement(geometry as ILinearRing);
                if (geometry is IPolygon)
                    return CreateGMXElement(geometry as IPolygon);
                if (geometry is IMultiPoint)
                    return CreateGMXElement(geometry as IMultiPoint);
                if (geometry is IMultiLineString)
                    return CreateGMXElement(geometry as IMultiLineString);
                if (geometry is IMultiPolygon)
                    return CreateGMXElement(geometry as IMultiPolygon);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("geometry", "Failed to convert Geometry to XElement.", ex);
            }

            throw new ArgumentException("Conversion is not supported with the specified geometry type.", "geometry");
        }

        #endregion

        #region Public conversion methods from GM to geometry

        /// <summary>
        /// Convert Geography Markup representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="referenceSystem">The reference system of the geometry.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        public static IGeometry ToGeometry(XElement node, IReferenceSystem referenceSystem)
        {
            return ToGeometry(node, Factory.GetInstance<IGeometryFactory>(referenceSystem));
        }

        /// <summary>
        /// Convert Geography Markup representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source XElement node.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        public static IGeometry ToGeometry(XElement node, IGeometryFactory factory)
        {
            if (node == null)
                throw new ArgumentNullException("The geometry node is null.", "node");

            if (factory == null)
                factory = Factory.DefaultInstance<IGeometryFactory>();

            IGeometry resultGeometry = null;

            try
            {
                switch (node.Name.LocalName)
                {
                    case "Point":
                        resultGeometry = ReadPoint(node.Elements(), factory);
                        break;
                    case "LineString":
                        resultGeometry = ReadLineString(node.Elements(), factory);
                        break;
                    case "LinearRing":
                        resultGeometry = ReadLinearRing(node.Elements(), factory);
                        break;
                    case "Polygon":
                        resultGeometry = ReadPolygon(node.Elements(), factory);
                        break;
                    case "MultiPoint":
                        resultGeometry = ReadMultiPoint(node.Elements(), factory);
                        break;
                    case "MultiLineString":
                        resultGeometry = ReadMultiLineString(node.Elements(), factory);
                        break;
                    case "MultiPolygon":
                        resultGeometry = ReadMultiPolygon(node.Elements(), factory);
                        break;
                    case "GeomCollection":
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("node", "Failed to convert XElement to Geometry.", ex);
            }

            return resultGeometry;
        }

        /// <summary>
        /// Convert Geography Markup representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="referenceSystem">The reference system of the geometry.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        public static IGeometry ToGeometry(String source, IReferenceSystem referenceSystem)
        {
            return ToGeometry(source, Factory.GetInstance<IGeometryFactory>(referenceSystem));
        }

        /// <summary>
        /// Convert Geography Markup representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        public static IGeometry ToGeometry(String source, IGeometryFactory factory)
        {
            if (source == null)
                throw new ArgumentNullException("The geometry text is null.", "source");

            if (factory == null)
                factory = Factory.DefaultInstance<IGeometryFactory>();

            StringReader sr = new StringReader(source);
            XDocument doc = XDocument.Load(sr);

            XElement firstElement = doc.Elements().FirstOrDefault();

            if (firstElement != null)
                return ToGeometry(firstElement, factory);
            else
                throw new ArgumentException("The source text contains no elements.", "source");
        }

        #endregion

        #region Private conversion methods from geometry to GM in string

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in string.</returns>
        private static String CreateGMString(IPoint geometry, Boolean isRoot = false)
        {
            StringBuilder builder = new StringBuilder();

            if (isRoot)
                builder.Append(@"<gml:Point xmlns:gml=""http://www.opengis.net/gml"">");
            else
                builder.Append("<gml:Point>");
            builder.Append("<gml:coord><gml:X>" + geometry.X + "</gml:X><gml:Y>" + geometry.Y + "</gml:Y></gml:coord>"); // !!!
            builder.Append("</gml:Point>");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in string.</returns>
        private static String CreateGMString(ILineString geometry, Boolean isRoot = false)
        {
            StringBuilder builder = new StringBuilder();

            if (isRoot)
                builder.Append(@"<gml:LineString xmlns:gml=""http://www.opengis.net/gml"">");
            else
                builder.Append("<gml:LineString>");
            foreach(Coordinate coord in geometry.Coordinates)
            {
                builder.Append("<gml:coord><gml:X>" + coord.X + "</gml:X><gml:Y>" + coord.Y + "</gml:Y></gml:coord>");
            }
            builder.Append("</gml:LineString>");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in string.</returns>
        private static String CreateGMString(ILinearRing geometry, Boolean isRoot = false)
        {
            StringBuilder builder = new StringBuilder();

            if (isRoot)
                builder.Append(@"<gml:LinearRing xmlns:gml=""http://www.opengis.net/gml"">");
            else
                builder.Append("<gml:LinearRing>");
            foreach (Coordinate coord in geometry.Coordinates)
            {
                builder.Append("<gml:coord><gml:X>" + coord.X + "</gml:X><gml:Y>" + coord.Y + "</gml:Y></gml:coord>");
            }
            builder.Append("</gml:LinearRing>");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in string.</returns>
        private static String CreateGMString(IPolygon geometry, Boolean isRoot = false)
        {
            StringBuilder builder = new StringBuilder();

            if (isRoot)
                builder.Append(@"<gml:Polygon xmlns:gml=""http://www.opengis.net/gml"">");
            else
                builder.Append("<gml:Polygon>");
            builder.Append("<gml:outerBoundaryIs>");
            builder.Append(CreateGMString(geometry.Shell));
            builder.Append("</gml:outerBoundaryIs>");
            foreach (ILinearRing boundary in geometry.Holes)
            {
                builder.Append("<gml:innerBoundaryIs>");
                builder.Append(CreateGMString(boundary));
                builder.Append("</gml:innerBoundaryIs>");
            }
            builder.Append("</gml:Polygon>");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in string.</returns>
        private static String CreateGMString(IMultiPoint geometry, Boolean isRoot = false)
        {
            StringBuilder builder = new StringBuilder();

            if (isRoot)
                builder.Append(@"<gml:MultiPoint xmlns:gml=""http://www.opengis.net/gml"">");
            else
                builder.Append("<gml:MultiPoint>");
            for (int i = 0; i < geometry.Count(); ++i)
            {
                builder.Append("<coord><X>" + geometry[i].X + "</X><Y>" + geometry[i].Y + "</Y></coord>");
            }
            builder.Append("</gml:MultiPoint>");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in string.</returns>
        private static String CreateGMString(IMultiLineString geometry, Boolean isRoot = false)
        {
            StringBuilder builder = new StringBuilder();

            if (isRoot)
                builder.Append(@"<gml:MultiLineString xmlns:gml=""http://www.opengis.net/gml"">");
            else
                builder.Append("<gml:MultiLineString>");
            for (int i = 0; i < geometry.Count(); ++i)
            {
                builder.Append("<gml:LineStringMember>");
                builder.Append(CreateGMString(geometry[i] as ILineString));
                builder.Append("</gml:LineStringMember>");
            }
            builder.Append("</gml:MultiLineString>");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in string.</returns>
        private static String CreateGMString(IMultiPolygon geometry, Boolean isRoot = false)
        {
            StringBuilder builder = new StringBuilder();

            if (isRoot)
                builder.Append(@"<gml:MultiPolygon xmlns:gml=""http://www.opengis.net/gml"">");
            else
                builder.Append("<gml:MultiPolygon>");
            for (int i = 0; i < geometry.Count(); ++i)
            {
                builder.Append("<gml:PolygonMember>");
                builder.Append(CreateGMString(geometry[i] as IPolygon));
                builder.Append("</gml:PolygonMember>");
            }
            builder.Append("</gml:MultiPolygon>");

            return builder.ToString();
        }

        #endregion

        #region Private conversion methods from geometry to GM in XElement

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in XElement.</returns>
        private static XElement CreateGMXElement(IPoint geometry)
        {
            XElement xElement = new XElement(_nameSpace + "X", geometry.X);
            XElement yElement = new XElement(_nameSpace + "Y", geometry.Y);
            XElement coordElement = new XElement(_nameSpace + "coord");
            XElement pointElement = new XElement(_nameSpace + "Point", new XAttribute(XNamespace.Xmlns + "gml", _nameSpace));

            coordElement.Add(new object[] { xElement, yElement });
            pointElement.Add(coordElement);

            return pointElement;
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in XElement.</returns>
        private static XElement CreateGMXElement(ILineString geometry)
        {
            XElement xElement;
            XElement yElement;
            XElement lineStringElement = new XElement(_nameSpace + "LineString", new XAttribute(XNamespace.Xmlns + "gml", _nameSpace));

            foreach (Coordinate coord in geometry.Coordinates)
            {
                XElement coordElement = new XElement(_nameSpace + "coord");
                xElement = new XElement(_nameSpace + "X", coord.X);
                yElement = new XElement(_nameSpace + "Y", coord.Y);
                coordElement.Add(new object[] { xElement, yElement });
                lineStringElement.Add(coordElement);
            }

            return lineStringElement;
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in XElement.</returns>
        private static XElement CreateGMXElement(ILinearRing geometry)
        {
            XElement xElement;
            XElement yElement;
            XElement linearRingElement = new XElement(_nameSpace + "LinearRing", new XAttribute(XNamespace.Xmlns + "gml", _nameSpace));

            foreach (Coordinate coord in geometry.Coordinates)
            {
                XElement coordElement = new XElement(_nameSpace + "coord");
                xElement = new XElement(_nameSpace + "X", coord.X);
                yElement = new XElement(_nameSpace + "Y", coord.Y);
                coordElement.Add(new object[] { xElement, yElement });
                linearRingElement.Add(coordElement);
            }

            return linearRingElement;
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in XElement.</returns>
        private static XElement CreateGMXElement(IPolygon geometry)
        {
            XElement outerBoundaryElement = new XElement(_nameSpace + "outerBoundaryIs");
            XElement polygonElement = new XElement(_nameSpace + "Polygon", new XAttribute(XNamespace.Xmlns + "gml", _nameSpace));

            outerBoundaryElement.Add(CreateGMXElement(geometry.Shell));
            polygonElement.Add(outerBoundaryElement);
            foreach (ILinearRing boundary in geometry.Holes)
            {
                XElement innerBoundaryElement = new XElement(_nameSpace + "innerBoundaryIs");
                innerBoundaryElement.Add(CreateGMXElement(boundary));
                polygonElement.Add(innerBoundaryElement);
            }

            return polygonElement;
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in XElement.</returns>
        private static XElement CreateGMXElement(IMultiPoint geometry)
        {
            XElement xElement;
            XElement yElement;
            XElement multiPointElement = new XElement(_nameSpace + "MultiPoint", new XAttribute(XNamespace.Xmlns + "gml", _nameSpace));

            for (int i = 0; i < geometry.Count(); ++i)
            {
                XElement coordElement = new XElement(_nameSpace + "coord");
                xElement = new XElement(_nameSpace + "X", geometry[i].X);
                yElement = new XElement(_nameSpace + "Y", geometry[i].Y);
                coordElement.Add(new object[] { xElement, yElement });
                multiPointElement.Add(coordElement);
            }

            return multiPointElement;
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in XElement.</returns>
        private static XElement CreateGMXElement(IMultiLineString geometry)
        {
            XElement multiLineStringElement = new XElement(_nameSpace + "MultiLineString", new XAttribute(XNamespace.Xmlns + "gml", _nameSpace));

            for (int i = 0; i < geometry.Count(); ++i)
            {
                XElement lineStringMemberElement = new XElement(_nameSpace + "LineStringMember");
                lineStringMemberElement.Add(CreateGMXElement(geometry[i]));
                multiLineStringElement.Add(lineStringMemberElement);
            }

            return multiLineStringElement;
        }

        /// <summary>
        /// Converts the Geography Markup representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The GM representation of the <paramref name="geometry" /> in XElement.</returns>
        private static XElement CreateGMXElement(IMultiPolygon geometry)
        {
            XElement polygonElement = new XElement(_nameSpace + "MultiPolygon", new XAttribute(XNamespace.Xmlns + "gml", _nameSpace));

            for (int i = 0; i < geometry.Count(); ++i)
            {
                XElement polygonMemberElement = new XElement(_nameSpace + "PolygonMember");
                polygonMemberElement.Add(CreateGMXElement(geometry[i]));
                polygonElement.Add(polygonMemberElement);
            }

            return polygonElement;
        }

        #endregion

        #region Private conversion methods from GM in XElement to geometry

        /// <summary>
        /// Reads the <see cref="IPoint" /> representation of the GM.
        /// </summary>
        /// <param name="node">The GM representation of the geometry.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IPoint" /> representation of the geometry.</returns>
        private static IPoint ReadPoint(IEnumerable<XElement> node, IGeometryFactory factory)
        {
            if (node == null || node.Count() == 0)
                return factory.CreatePoint(0, 0);

            if (node.Any(r => r.Name.LocalName == "coordinates"))
            {
                XElement first = node.FirstOrDefault(r => r.Name.LocalName == "coordinates");
                String[] coordinates;

                if (first == null)
                    throw new ArgumentException("The point geometry has no coordinates.", "node");
                else
                    coordinates = first.Value.Split(',');

                if (coordinates == null)
                    throw new ArgumentException("The point geometry has no coordinates.", "node");

                return factory.CreatePoint(Double.Parse(coordinates[0], System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                                           Double.Parse(coordinates[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            }
            else
            {
                XElement coordNode = node.FirstOrDefault(r => r.Name.LocalName == "coord");

                if (coordNode == null)
                    throw new ArgumentException("The point geometry has no coordinates.", "node");

                return factory.CreatePoint(ReadCoord(coordNode));
            }
        }

        /// <summary>
        /// Reads the <see cref="ILineString" /> representation of the GM.
        /// </summary>
        /// <param name="node">The GM representation of the geometry.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="ILineString" /> representation of the geometry.</returns>
        private static ILineString ReadLineString(IEnumerable<XElement> node, IGeometryFactory factory)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            if (node.Any(r => r.Name.LocalName == "coordinates"))
            {
                XElement first = node.FirstOrDefault(n => n.Name.LocalName == "coordinates");

                if (first == null)
                    throw new ArgumentException("The linestring geometry has no coordinates", "node");
                else
                    coordinates = first.Value.Split(' ')
                                  .Select(text => text.Split(','))
                                  .Select(array => new Coordinate(Double.Parse(array[0], System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                                                                  Double.Parse(array[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat))).ToList();

                if (coordinates == null)
                    throw new ArgumentException("The linestring geometry has no coordinates", "node");
            }
            else
            {
                foreach (XElement coordNode in node.Where(n => n.Name.LocalName == "coord"))
                {
                    coordinates.Add(ReadCoord(coordNode));
                }
            }

            return factory.CreateLineString(coordinates);
        }

        /// <summary>
        /// Reads the <see cref="ILinearRing" /> representation of the GM.
        /// </summary>
        /// <param name="node">The GM representation of the geometry.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="ILinearRing" /> representation of the geometry.</returns>
        private static ILinearRing ReadLinearRing(IEnumerable<XElement> node, IGeometryFactory factory)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            if (node.Any(r => r.Name.LocalName == "coordinates"))
            {
                XElement first = node.FirstOrDefault(n => n.Name.LocalName == "coordinates");

                if (first == null)
                    throw new ArgumentException("The linearring geometry has no coordinates", "node");
                else
                    coordinates = first.Value.Split(' ')
                                  .Select(text => text.Split(','))
                                  .Select(array => new Coordinate(Double.Parse(array[0], System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                                                                  Double.Parse(array[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat))).ToList();

                if (coordinates == null)
                    throw new ArgumentException("The linearring geometry has no coordinates", "node");
            }
            else
            {
                foreach (XElement coordNode in node.Where(n => n.Name.LocalName == "coord"))
                {
                    coordinates.Add(ReadCoord(coordNode));
                }
            }

            return factory.CreateLinearRing(coordinates);
        }

        /// <summary>
        /// Reads the <see cref="IPolygon" /> representation of the GM.
        /// </summary>
        /// <param name="node">The GM representation of the geometry.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IPolygon" /> representation of the geometry.</returns>
        private static IPolygon ReadPolygon(IEnumerable<XElement> node, IGeometryFactory factory)
        {
            IList<Coordinate> shellCoordinates;
            List<IList<Coordinate>> holeCoordinates = new List<IList<Coordinate>>();

            XElement shellNode = node.FirstOrDefault(n => n.Name.LocalName == "outerBoundaryIs");

            if (shellNode == null)
                throw new ArgumentException("The polygon geometry has no outer boundary", "node");

            XElement linearRingNode = shellNode.Elements().First();
            shellCoordinates = ReadLinearRing(linearRingNode.Elements(), factory).Coordinates;

            if (node.Any(r => r.Name.LocalName == "innerBoundaryIs"))
            {
                foreach (XElement boundaryNode in node.Where(n => n.Name.LocalName == "innerBoundaryIs"))
                {
                    linearRingNode = boundaryNode.Elements().FirstOrDefault();

                    if (linearRingNode == null)
                        throw new ArgumentException("The polygon geometry's inner boundary has no linearrings", "node");

                    holeCoordinates.Add(ReadLinearRing(linearRingNode.Elements(), factory).Coordinates);
                }
            }

            return factory.CreatePolygon(shellCoordinates, holeCoordinates);
        }

        /// <summary>
        /// Reads the <see cref="IMultiPoint" /> representation of the GM.
        /// </summary>
        /// <param name="node">The GM representation of the geometry.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiPoint" /> representation of the geometry.</returns>
        private static IMultiPoint ReadMultiPoint(IEnumerable<XElement> node, IGeometryFactory factory)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            if (node.Any(r => r.Name.LocalName == "coordinates"))
            {
                XElement first = node.FirstOrDefault(n => n.Name.LocalName == "coordinates");


                if (first == null)
                    throw new ArgumentException("The multipoint geometry has no coordinates.", "node");
                else
                    coordinates = first.Value.Split(' ')
                                  .Select(text => text.Split(','))
                                  .Select(array => new Coordinate(Double.Parse(array[0], System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                                                                  Double.Parse(array[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat))).ToList();

                if (coordinates == null)
                    throw new ArgumentException("The multipoint geometry has no coordinates.", "node");
            }
            else
            {
                foreach (XElement coordNode in node.Where(n => n.Name.LocalName == "coord"))
                {
                    coordinates.Add(ReadCoord(coordNode));
                }
            }

            return factory.CreateMultiPoint(coordinates);
        }

        /// <summary>
        /// Reads the <see cref="IMultiLineString" /> representation of the GM.
        /// </summary>
        /// <param name="node">The GM representation of the geometry.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiLineString" /> representation of the geometry.</returns>
        private static IMultiLineString ReadMultiLineString(IEnumerable<XElement> node, IGeometryFactory factory)
        {
            List<ILineString> lineStrings = new List<ILineString>();

            foreach (XElement lineStringMember in node.Where(n => n.Name.LocalName == "LineStringMember"))
            {
                XElement lineString = lineStringMember.Elements().FirstOrDefault();

                if (lineString == null)
                    throw new ArgumentException("THe multilinestring geometry misses a linestringmember.", "node");

                lineStrings.Add(ReadLineString(lineString.Elements(), factory));
            }

            return factory.CreateMultiLineString(lineStrings);
        }

        /// <summary>
        /// Reads the <see cref="IMultiPolygon" /> representation of the GM.
        /// </summary>
        /// <param name="node">The GM representation of the geometry.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiPolygon" /> representation of the geometry.</returns>
        private static IMultiPolygon ReadMultiPolygon(IEnumerable<XElement> node, IGeometryFactory factory)
        {
            List<IPolygon> polygons = new List<IPolygon>();

            foreach (XElement polygonMember in node.Where(n => n.Name.LocalName == "PolygonMember"))
            {
                XElement polygon = polygonMember.Elements().FirstOrDefault();

                if (polygon == null)
                    throw new ArgumentException("The multipolygon geometry missed a polygonmember.", "node");

                polygons.Add(ReadPolygon(polygon.Elements(), factory));
            }

            return factory.CreateMultiPolygon(polygons);
        }

        #endregion

        #region Private conversion utility methods

        /// <summary>
        /// Reads a coordinate from an XElement node.
        /// </summary>
        /// <param name="coordNode">The node to read the coordinate from.</param>
        /// <returns>The coordinate.</returns>
        private static Coordinate ReadCoord(XElement coordNode)
        {
            String Xcoord = coordNode.Elements().FirstOrDefault(e => e.Name.LocalName == "X").Value;

            if (Xcoord == null)
                throw new ArgumentException("The geometry has no valid coordinates.", "coordNode");

            String Ycoord = coordNode.Elements().FirstOrDefault(e => e.Name.LocalName == "Y").Value;

            if (Ycoord == null)
                throw new ArgumentException("The geometry has no valid coordinates.", "coordNode");

            return new Coordinate(Double.Parse(Xcoord, System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                                  Double.Parse(Ycoord, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
        }

        #endregion
    }
}
