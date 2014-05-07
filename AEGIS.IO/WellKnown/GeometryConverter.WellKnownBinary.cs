/// <copyright file="GeometryConverter.WellKnownBinary.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.IO.WellKnown
{
    /// <summary>
    /// Represents a converter for Well-known Binary (WKB) representation.
    /// </summary>
    public static partial class GeometryConverter
    {
        #region Private types

        /// <summary>
        /// Defines Well-known Binary types.
        /// </summary>
        private enum WellKnownBinaryTypes
        {
            Geometry = 0,
            Point = 1,
            LineString = 2,
            Polygon = 3,
            MultiPoint = 4,
            MultiLineString = 5,
            MultiPolygon = 6,
            GeometryCollection = 7,
            CircularString = 8,
            CompoundCurve = 9,
            CurvewPolygon = 10,
            MultiCurve = 11,
            MultiSurface = 12,
            Curve = 13,
            Surface = 14,
            PolyhedralSurface = 15,
            TIN = 16,
            Triangle = 17,

            GeometryZ = 1000,
            PointZ = 1001,
            LineStringZ = 1002,
            PolygonZ = 1003,
            MultiPointZ = 1004,
            MultiLineStringZ = 1005,
            MultiPolygonZ = 1006,
            GeometryCollectionZ = 1007,
            CircularStringZ = 1008,
            CompoundCurveZ = 1009,
            CurvewPolygonZ = 1010,
            MultiCurveZ = 1011,
            MultiSurfaceZ = 1012,
            CurveZ = 1013,
            SurfaceZ = 1014,
            PolyhedralSurfaceZ = 1015,
            TINZ = 1016,
            TriangleZ = 1017,

            GeometryM = 2000,
            PointM = 2001,
            LineStringM = 2002,
            PolygonM = 2003,
            MultiPointM = 2004,
            MultiLineStringM = 2005,
            MultiPolygonM = 2006,
            GeometryCollectionM = 2007,
            CircularStringM = 2008,
            CompoundCurveM = 2009,
            CurvewPolygonM = 2010,
            MultiCurveM = 2011,
            MultiSurfaceM = 2012,
            CurveM = 2013,
            SurfaceM = 2014,
            PolyhedralSurfaceM = 2015,
            TINM = 2016,
            TriangleM = 2017,

            GeometryZM = 3000,
            PointZM = 3001,
            LineStringZM = 3002,
            PolygonZM = 3003,
            MultiPointZM = 3004,
            MultiLineStringZM = 3005,
            MultiPolygonZM = 3006,
            GeometryCollectionZM = 3007,
            CircularStringZM = 3008,
            CompoundCurveZM = 3009,
            CurvewPolygonZM = 3010,
            MultiCurveZM = 3011,
            MultiSurfaceZM = 3012,
            CurveZM = 3013,
            SurfaceZM = 3014,
            PolyhedralSurfaceZM = 3015,
            TINZM = 3016,
            TriangleZM = 3017
        }

        #endregion

        #region Public conversion methods from geometry to WKB

        /// <summary>
        /// Convert to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        public static Byte[] ToWellKnownBinary(this IGeometry geometry)
        {
            return ToWellKnownBinary(geometry, ByteOrder.LittleEndian, GeometryModel.Spatial3D);
        }

        /// <summary>
        /// Convert to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// Only 2D and 3D spatial conversion is supported for Well-Known Binary.;geometryModel
        /// or
        /// Conversion is not suppported with the specified geometry type.;geometry
        /// </exception>
        public static Byte[] ToWellKnownBinary(this IGeometry geometry, ByteOrder byteOrder, GeometryModel geometryModel)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            if (geometryModel != GeometryModel.Spatial2D && geometryModel != GeometryModel.Spatial3D)
                throw new ArgumentException("Only 2D and 3D spatial conversion is supported for Well-Known Binary.", "geometryModel");

            if (geometry is IPoint)
                return ComputeWellKnownBinary(geometry as IPoint, byteOrder, geometryModel);
            if (geometry is ILineString)
                return ComputeWellKnownBinary(geometry as ILineString, byteOrder, geometryModel);
            if (geometry is IPolygon)
                return ComputeWellKnownBinary(geometry as IPolygon, byteOrder, geometryModel);
            if (geometry is IMultiPoint)
                return ComputeWellKnownBinary(geometry as IMultiPoint, byteOrder, geometryModel);
            if (geometry is IMultiLineString)
                return ComputeWellKnownBinary(geometry as IMultiLineString, byteOrder, geometryModel);
            if (geometry is IMultiPolygon)
                return ComputeWellKnownBinary(geometry as IMultiPolygon, byteOrder, geometryModel);

            throw new ArgumentException("Conversion is not suppported with the specified geometry type.", "geometry");
        }

        #endregion

        #region Public conversion methods from WKB to geometry

        /// <summary>
        /// Convert Well-known Binary representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source byte array.</param>
        /// <param name="referenceSystem">The reference system of the geometry.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The source is empty.
        /// or
        /// The content of the source is invalid.
        /// </exception>
        public static IGeometry ToGeometry(Byte[] source, IReferenceSystem referenceSystem)
        {
            return ToGeometry(source, Factory.GetInstance<IGeometryFactory>(referenceSystem));
        }

        /// <summary>
        /// Convert Well-known Binary representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source byte array.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <param name="referenceSystem">The reference system of the geometry.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The source is empty.;source
        /// or
        /// The content of the source is invalid.;source
        /// </exception>
        public static IGeometry ToGeometry(Byte[] source, IGeometryFactory factory)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            if (source.Length == 0)
                throw new ArgumentException("The source is empty.", "source");

            if (factory == null)
                factory = Factory.DefaultInstance<IGeometryFactory>();

            try
            {
                ByteOrder byteOrder = (source[0] == 1) ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

                IGeometry resultGeometry = null;
                switch ((WellKnownBinaryTypes)EndianBitConverter.ToInt32(source, 1, byteOrder))
                {
                    case WellKnownBinaryTypes.Point:
                        resultGeometry = ComputePoint(source, byteOrder, GeometryModel.Spatial2D, factory);
                        break;
                    case WellKnownBinaryTypes.PointZ:
                        resultGeometry = ComputePoint(source, byteOrder, GeometryModel.Spatial3D, factory);
                        break;
                    case WellKnownBinaryTypes.LineString:
                        resultGeometry = ComputeLineString(source, byteOrder, GeometryModel.Spatial2D, factory);
                        break;
                    case WellKnownBinaryTypes.LineStringZ:
                        resultGeometry = ComputeLineString(source, byteOrder, GeometryModel.Spatial3D, factory);
                        break;
                    case WellKnownBinaryTypes.Polygon:
                        resultGeometry = ComputePolygon(source, byteOrder, GeometryModel.Spatial2D, factory);
                        break;
                    case WellKnownBinaryTypes.PolygonZ:
                        resultGeometry = ComputePolygon(source, byteOrder, GeometryModel.Spatial3D, factory);
                        break;
                    case WellKnownBinaryTypes.MultiPoint:
                        resultGeometry = ComputeMultiPoint(source, byteOrder, GeometryModel.Spatial2D, factory);
                        break;
                    case WellKnownBinaryTypes.MultiPointZ:
                        resultGeometry = ComputeMultiPoint(source, byteOrder, GeometryModel.Spatial3D, factory);
                        break;
                    case WellKnownBinaryTypes.MultiLineString:
                        resultGeometry = ComputeMultiLineString(source, byteOrder, GeometryModel.Spatial2D, factory);
                        break;
                    case WellKnownBinaryTypes.MultiLineStringZ:
                        resultGeometry = ComputeMultiLineString(source, byteOrder, GeometryModel.Spatial3D, factory);
                        break;
                    case WellKnownBinaryTypes.MultiPolygon:
                        resultGeometry = ComputeMultiPolygon(source, byteOrder, GeometryModel.Spatial2D, factory);
                        break;
                    case WellKnownBinaryTypes.MultiPolygonZ:
                        resultGeometry = ComputeMultiPolygon(source, byteOrder, GeometryModel.Spatial3D, factory);
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

        #region Private conversion methods from geometry to WKB

        /// <summary>
        /// Computes the Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ComputeWellKnownBinary(IPoint geometry, ByteOrder byteOrder, GeometryModel geometryModel)
        {
            Byte[] geometryBytes = new Byte[geometryModel == GeometryModel.Spatial3D ? 29 : 21];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (geometryModel == GeometryModel.Spatial3D) // típus
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.PointZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.Point, geometryBytes, 1, byteOrder);

            EndianBitConverter.CopyBytes(geometry.X, geometryBytes, 5, byteOrder);
            EndianBitConverter.CopyBytes(geometry.Y, geometryBytes, 13, byteOrder);

            if (geometryModel == GeometryModel.Spatial3D)
                EndianBitConverter.CopyBytes(geometry.Z, geometryBytes, 21, byteOrder);

            return geometryBytes;
        }
        /// <summary>
        /// Computes the Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ComputeWellKnownBinary(ILineString geometry, ByteOrder byteOrder, GeometryModel geometryModel)
        {
            Byte[] geometryBytes = new Byte[9 + ((geometryModel == GeometryModel.Spatial3D) ? 24 : 16) * geometry.Count];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (geometryModel == GeometryModel.Spatial3D)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.LineStringZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.LineString, geometryBytes, 1, byteOrder);

            Int32 byteIndex = 5;
            ComputeCoordinateList(geometryBytes, ref byteIndex, byteOrder, geometry.Coordinates, geometryModel);

            return geometryBytes;
        }
        /// <summary>
        /// Computes the Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ComputeWellKnownBinary(IPolygon geometry, ByteOrder byteOrder, GeometryModel geometryModel)
        {
            Byte[] geometryBytes = new Byte[9 + 4 * (geometry.HoleCount + 1) + ((geometryModel == GeometryModel.Spatial3D) ? 24 : 16) * (geometry.Shell.Count + geometry.Holes.Sum(hole => hole.Count))];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (geometryModel == GeometryModel.Spatial3D)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.PolygonZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.Polygon, geometryBytes, 1, byteOrder);

            EndianBitConverter.CopyBytes(geometry.HoleCount + 1, geometryBytes, 5, byteOrder); // the number of rings

            Int32 byteIndex = 9;
            ComputeCoordinateList(geometryBytes, ref byteIndex, byteOrder, geometry.Shell.Coordinates, geometryModel); // shell

            for (Int32 i = 0; i < geometry.Holes.Count; i++) // holes
            {
                ComputeCoordinateList(geometryBytes, ref byteIndex, byteOrder, geometry.Holes[i].Coordinates, geometryModel);
            }

            return geometryBytes;
        }
        /// <summary>
        /// Computes the Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ComputeWellKnownBinary(IMultiPoint geometry, ByteOrder byteOrder, GeometryModel geometryModel)
        {
            Byte[] geometryBytes = new Byte[9 + ((geometryModel == GeometryModel.Spatial3D) ? 24 : 16) * geometry.Count];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (geometryModel == GeometryModel.Spatial3D)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiPointZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiPoint, geometryBytes, 1, byteOrder);

            Int32 byteIndex = 5;
            EndianBitConverter.CopyBytes(geometry.Count, geometryBytes, byteIndex, byteOrder); // the number of points
            byteIndex += 4;

            for (Int32 i = 0; i < geometry.Count; i++)
            {
                EndianBitConverter.CopyBytes(geometry[i].X, geometryBytes, byteIndex, byteOrder);
                EndianBitConverter.CopyBytes(geometry[i].Y, geometryBytes, byteIndex + 8, byteOrder);

                if (geometryModel == GeometryModel.Spatial3D)
                {
                    EndianBitConverter.CopyBytes(geometry[i].Z, geometryBytes, byteIndex + 16, byteOrder);
                    byteIndex += 24;
                }
                else
                    byteIndex += 16;
            }

            return geometryBytes;
        }
        /// <summary>
        /// Computes the Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ComputeWellKnownBinary(IMultiLineString geometry, ByteOrder byteOrder, GeometryModel geometryModel)
        {
            Byte[] geometryBytes = new Byte[9 + 4 * geometry.Count + ((geometryModel == GeometryModel.Spatial3D) ? 24 : 16) * geometry.Sum(lineString => lineString.Count)];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (geometryModel == GeometryModel.Spatial3D)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiLineStringZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiLineString, geometryBytes, 1, byteOrder);

            EndianBitConverter.CopyBytes(geometry.Count, geometryBytes, 5, byteOrder); // the number of line strings
            Int32 byteIndex = 9;
            for (Int32 i = 0; i < geometry.Count; i++)
            {
                ComputeCoordinateList(geometryBytes, ref byteIndex, byteOrder, geometry[i].Coordinates, geometryModel);
            }

            return geometryBytes;
        }
        /// <summary>
        /// Computes the Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ComputeWellKnownBinary(IMultiPolygon geometry, ByteOrder byteOrder, GeometryModel geometryModel)
        {
            Byte[] geometryBytes = new Byte[9 + 4 * geometry.Count + 
                                            4 * geometry.Sum(polygon => polygon.HoleCount + 1) + 
                                            ((geometryModel == GeometryModel.Spatial3D) ? 24 : 16) * geometry.Sum(polygon => polygon.Shell.Count + polygon.Holes.Sum(hole => hole.Count))];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (geometryModel == GeometryModel.Spatial3D)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiPolygonZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiPolygon, geometryBytes, 1, byteOrder);

            EndianBitConverter.CopyBytes(geometry.Count, geometryBytes, 5, byteOrder); // the number of polygons
            Int32 byteIndex = 9;

            for (Int32 i = 0; i < geometry.Count; i++) // polygons
            {
                EndianBitConverter.CopyBytes(geometry[i].HoleCount + 1, geometryBytes, byteIndex, byteOrder); // the number of rings
                byteIndex += 4;

                ComputeCoordinateList(geometryBytes, ref byteIndex, byteOrder, geometry[i].Shell.Coordinates, geometryModel); // shell

                for (Int32 j = 0; j < geometry[i].Holes.Count; j++) // holes
                {
                    ComputeCoordinateList(geometryBytes, ref byteIndex, byteOrder, geometry[i].Holes[j].Coordinates, geometryModel);
                }
            }

            return geometryBytes;
        }

        #endregion

        #region Private conversion methods from WKB to geometry

        /// <summary>
        /// Computes the <see cref="IPoint" /> representation of the WKB.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IPoint" /> representation of the geometry.</returns>
        private static IPoint ComputePoint(Byte[] geometryBytes, ByteOrder byteOrder, GeometryModel geometryModel, IGeometryFactory factory)
        {
            return factory.CreatePoint(EndianBitConverter.ToDouble(geometryBytes, 5, byteOrder), 
                                       EndianBitConverter.ToDouble(geometryBytes, 13, byteOrder), 
                                       geometryModel == GeometryModel.Spatial3D ? EndianBitConverter.ToDouble(geometryBytes, 21, byteOrder) : 0);
        }
        /// <summary>
        /// Computes the <see cref="ILineString" /> representation of the WKB.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="ILineString" /> representation of the geometry.</returns>
        private static ILineString ComputeLineString(Byte[] geometryBytes, ByteOrder byteOrder, GeometryModel geometryModel, IGeometryFactory factory)
        {
            Int32 coordinateSize = (geometryModel == GeometryModel.Spatial3D) ? 24 : 16;
            Int32 coordinateCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);

            Coordinate[] coordinates = new Coordinate[coordinateCount];

            for (Int32 byteIndex = 9, coordinateIndex = 0; coordinateIndex < coordinateCount; byteIndex += coordinateSize, coordinateIndex++)
            {
                coordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                              EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                              geometryModel == GeometryModel.Spatial3D ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
            }

            return factory.CreateLineString(coordinates);
        }
        /// <summary>
        /// Computes the <see cref="IPolygon" /> representation of the WKB.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IPolygon" /> representation of the geometry.</returns>
        private static IPolygon ComputePolygon(Byte[] geometryBytes, ByteOrder byteOrder, GeometryModel geometryModel, IGeometryFactory factory)
        {
            Int32 coordinateSize = (geometryModel == GeometryModel.Spatial3D) ? 24 : 16;
            Int32 ringCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder); 
            Int32 shellCoordinateCount = EndianBitConverter.ToInt32(geometryBytes, 9, byteOrder);

            Coordinate[] shellCoordinates = new Coordinate[shellCoordinateCount];

            for (Int32 byteIndex = 13, coordinateIndex = 0; coordinateIndex < shellCoordinateCount; byteIndex += coordinateSize, coordinateIndex++)
                shellCoordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                   EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                   geometryModel == GeometryModel.Spatial3D ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);

            if (ringCount > 1)
            {
                Coordinate[][] holes = new Coordinate[ringCount - 1][];

                Int32 holeStartIndex = 13 + shellCoordinateCount * coordinateSize;

                for (Int32 i = 1; i < ringCount; i++)
                {
                    Int32 holeCoordianteCount = EndianBitConverter.ToInt32(geometryBytes, holeStartIndex, byteOrder);
                    holeStartIndex += 4;
                    Coordinate[] holeCoordinates = new Coordinate[holeCoordianteCount];

                    for (Int32 byteIndex = holeStartIndex, coordinateIndex = 0; coordinateIndex < holeCoordianteCount; byteIndex += coordinateSize, coordinateIndex++)
                        holeCoordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                          EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                          geometryModel == GeometryModel.Spatial3D ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);

                    holes[i - 1] = holeCoordinates;

                    holeStartIndex += holeCoordianteCount * coordinateSize;
                }

                return factory.CreatePolygon(shellCoordinates, holes);
            }
            else
                return factory.CreatePolygon(shellCoordinates);
        }
        /// <summary>
        /// Computes the <see cref="IMultiPoint" /> representation of the WKB.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiPoint" /> representation of the geometry.</returns>
        private static IMultiPoint ComputeMultiPoint(Byte[] geometryBytes, ByteOrder byteOrder, GeometryModel geometryModel, IGeometryFactory factory)
        {
            Int32 pointSize = (geometryModel == GeometryModel.Spatial3D) ? 24 : 16;
            Int32 pointCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);

            IPoint[] points = new IPoint[pointCount];

            for (Int32 byteIndex = 9, pointIndex = 0; pointIndex < pointCount; byteIndex += pointSize, pointIndex++)
                points[pointIndex] = factory.CreatePoint(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                         EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                         geometryModel == GeometryModel.Spatial3D ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);

            return factory.CreateMultiPoint(points);
        }
        /// <summary>
        /// Computes the <see cref="IMultiLineString" /> representation of the WKB.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiLineString" /> representation of the geometry.</returns>
        private static IMultiLineString ComputeMultiLineString(Byte[] geometryBytes, ByteOrder byteOrder, GeometryModel geometryModel, IGeometryFactory factory)
        {
            Int32 coordinateSize = (geometryModel == GeometryModel.Spatial3D) ? 24 : 16;
            Int32 lineStringCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);

            ILineString[] lineStrings = new ILineString[lineStringCount];

            Int32 lineStringStartIndex = 9;
            for (Int32 i = 0; i < lineStringCount; i++)
            {
                Int32 coordinateCount = EndianBitConverter.ToInt32(geometryBytes, lineStringStartIndex, byteOrder);
                lineStringStartIndex += 4;

                Coordinate[] coordinates = new Coordinate[coordinateCount];

                for (Int32 byteIndex = lineStringStartIndex, coordinateIndex = 0; coordinateIndex < coordinateCount; byteIndex += coordinateSize, coordinateIndex++)
                    coordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                  EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                  geometryModel == GeometryModel.Spatial3D ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);

                lineStrings[i] = factory.CreateLineString(coordinates);
            }

            return factory.CreateMultiLineString(lineStrings);
        }
        /// <summary>
        /// Computes the <see cref="IMultiPolygon" /> representation of the WKB.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <returns>The <see cref="IMultiPolygon" /> representation of the geometry.</returns>
        private static IMultiPolygon ComputeMultiPolygon(Byte[] geometryBytes, ByteOrder byteOrder, GeometryModel geometryModel, IGeometryFactory factory)
        {
            Int32 coordinateSize = (geometryModel == GeometryModel.Spatial3D) ? 24 : 16;
            Int32 polygonCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);

            IPolygon[] polygons = new Polygon[polygonCount];

            Int32 startIndex = 9;
            for (Int32 i = 0; i < polygonCount; i++)
            {
                Int32 ringCount = EndianBitConverter.ToInt32(geometryBytes, startIndex, byteOrder);
                Int32 shellCoordinateCount = EndianBitConverter.ToInt32(geometryBytes, startIndex + 4, byteOrder);

                startIndex += 8;

                Coordinate[] shellCoordinates = new Coordinate[shellCoordinateCount];

                for (Int32 byteIndex = startIndex, coordinateIndex = 0; coordinateIndex < shellCoordinateCount; byteIndex += coordinateSize, coordinateIndex++)
                    shellCoordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                       EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                       geometryModel == GeometryModel.Spatial3D ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
                startIndex += shellCoordinateCount * coordinateSize;

                if (ringCount > 1)
                {
                    Coordinate[][] holes = new Coordinate[ringCount - 1][];

                    for (Int32 j = 1; j < ringCount; j++)
                    {
                        Int32 holeCoordianteCount = EndianBitConverter.ToInt32(geometryBytes, startIndex, byteOrder);
                        startIndex += 4;

                        Coordinate[] holeCoordinates = new Coordinate[holeCoordianteCount];

                        for (Int32 byteIndex = startIndex, coordinateIndex = 0; coordinateIndex < holeCoordianteCount; byteIndex += coordinateSize, coordinateIndex++)
                            holeCoordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                              EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                              geometryModel == GeometryModel.Spatial3D ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);

                        holes[j - 1] = holeCoordinates;

                        startIndex += holeCoordianteCount * coordinateSize;
                    }

                    polygons[i] = factory.CreatePolygon(shellCoordinates, holes);
                }
                else
                    polygons[i] = factory.CreatePolygon(shellCoordinates);
            }

            return factory.CreateMultiPolygon(polygons);
        }

        #endregion

        #region Private conversion utility methods

        /// <summary>
        /// Computes the WKB representation of a coordinate list.
        /// </summary>
        /// <param name="byteArray">The byte-array.</param>
        /// <param name="byteIndex">The starting index.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="coordinateList">The coordinate list.</param>
        /// <param name="geometryModel">The geometry model of the conversion.</param>
        private static void ComputeCoordinateList(Byte[] byteArray, ref Int32 byteIndex, ByteOrder byteOrder, IList<Coordinate> coordinateList, GeometryModel geometryModel)
        {
            EndianBitConverter.CopyBytes(coordinateList.Count, byteArray, byteIndex, byteOrder); // the number of coordinates
            byteIndex += 4;

            for (Int32 i = 0; i < coordinateList.Count; i++)
            {
                EndianBitConverter.CopyBytes(coordinateList[i].X, byteArray, byteIndex, byteOrder);
                EndianBitConverter.CopyBytes(coordinateList[i].Y, byteArray, byteIndex + 8, byteOrder); 

                if (geometryModel == GeometryModel.Spatial3D)
                {
                    EndianBitConverter.CopyBytes(coordinateList[i].Z, byteArray, byteIndex + 16, byteOrder);
                    byteIndex += 24;
                }
                else
                    byteIndex += 16;
            }
        }

        #endregion
    }
}
