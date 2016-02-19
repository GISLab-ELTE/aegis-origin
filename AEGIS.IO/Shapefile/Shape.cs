/// <copyright file="Shape.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.IO.Shapefile
{
    /// <summary>
    /// Represents a shape.
    /// </summary>
    class Shape
    {
        #region Private fields

        private readonly IGeometryFactory _factory;
        private readonly Int32 _id;
        private readonly Envelope _envelope;
        private readonly Coordinate[] _coordinates;
        private readonly ShapeType _type;
        private readonly Int32[] _parts;
        private readonly ShapePartType[] _partTypes;
        private readonly IDictionary<String, Object> _metadata;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the shape identifier.
        /// </summary>
        /// <value>A unique indentifier for the shape.</value>
        public Int32 Id { get { return _id; } }
        /// <summary>
        /// Gets the envelope of the shape.
        /// </summary>
        /// <value>The envelope of the shape.</value>
        public Envelope Envelope { get { return _envelope; } }
        /// <summary>
        /// Gets the type of the shape.
        /// </summary>
        /// <value>The type of the shape.</value>
        public ShapeType Type { get { return _type; } }
        /// <summary>
        /// Gets the number of coordinates in the shape.
        /// </summary>
        /// <value>The number of coordinates in the shape.</value>
        public Int32 CoordinateCount { get { return _coordinates == null ? 0 : _coordinates.Length; } }
        /// <summary>
        /// Gets the coordinates of the shape.
        /// </summary>
        /// <value>The read-only list containing the coordinates of the shape.</value>
        public IList<Coordinate> Coordinates { get { return _coordinates == null ? null : Array.AsReadOnly(_coordinates); } }
        /// <summary>
        /// Gets the number of parts in the shape.
        /// </summary>
        /// <value>The number of parts in the shape.</value>
        public Int32 PartCount { get { return _parts == null ? 1 : _parts.Length; } }
        /// <summary>
        /// Gets the starting indices of the shape parts.
        /// </summary>
        /// <value>The read-only list containing the starting indices of the shape parts.</value>
        public IList<Int32> Parts { get { return _parts == null ? null : Array.AsReadOnly(_parts); } }
        /// <summary>
        /// Gets the types of the shape parts.
        /// </summary>
        /// <value>The read-only list containing the types of the shape parts.</value>
        public IList<ShapePartType> PartTypes { get { return _partTypes == null ? null : Array.AsReadOnly(_partTypes); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="type">The type.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        public Shape(Int32 id, ShapeType type, Coordinate coordinate, IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            _id = id;
            _type = type;
            _coordinates = new Coordinate[] { coordinate };
            _envelope = Envelope.FromCoordinates(Coordinates);
            _factory = factory;
            _metadata = metadata;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Shape" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="type">The type.</param>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// There are no coordinates specified.
        /// or
        /// The factory is null.
        /// </exception>
        public Shape(Int32 id, ShapeType type, Coordinate[] coordinates, IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "There are no coordinates specified.");
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            _id = id;
            _type = type;
            _coordinates = coordinates;
            _envelope = Envelope.FromCoordinates(Coordinates);
            _factory = factory;
            _metadata = metadata;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Shape" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="type">The type.</param>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="parts">The parts.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// There are no coordinates specified.
        /// or
        /// There are no parts specified.
        /// or
        /// The factory is null.
        /// </exception>
        public Shape(Int32 id, ShapeType type, Coordinate[] coordinates, Int32[] parts, IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "There are no coordinates specified.");
            if (parts == null)
                throw new ArgumentNullException("parts", "There are no parts specified.");
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            _id = id;
            _type = type;
            _parts = parts;
            _coordinates = coordinates;
            _envelope = Envelope.FromCoordinates(Coordinates);
            _factory = factory;
            _metadata = metadata;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Shape" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="type">The type.</param>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="parts">The parts.</param>
        /// <param name="partTypes">The types of the parts.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// There are no coordinates specified.
        /// or
        /// There are no parts specified.
        /// or
        /// There are no part types specified.
        /// or
        /// The factory is null.
        /// </exception>
        public Shape(Int32 id, ShapeType type, Coordinate[] coordinates, Int32[] parts, ShapePartType[] partTypes, IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "There are no coordinates specified.");
            if (parts == null)
                throw new ArgumentNullException("parts", "There are no parts specified.");
            if (partTypes == null)
                throw new ArgumentNullException("partTypes", "There are no part types specified.");
            if (factory == null)
                throw new ArgumentNullException("factory", "The factory is null.");

            _id = id;
            _type = type;
            _parts = parts;
            _partTypes = partTypes;
            _coordinates = coordinates;
            _envelope = Envelope.FromCoordinates(Coordinates);
            _factory = factory;
            _metadata = metadata;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Shape" /> class.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">Geometry type is not supported by stream.</exception>
        public Shape(IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            _factory = geometry.Factory;

            if (geometry.Metadata != null)
            {
                _metadata = new Dictionary<String, Object>(geometry.Metadata);
            }

            if (geometry is IPoint)
            {
                _type = geometry.SpatialDimension == 3 ? ShapeType.PointZ : ShapeType.Point;
                _coordinates = new Coordinate[] { (geometry as IPoint).Coordinate };
            }
            else if (geometry is ILine)
            {
                _type = geometry.SpatialDimension == 3 ? ShapeType.PolyLineZ : ShapeType.PolyLine;
                _coordinates = new Coordinate[] { (geometry as ILine).StartCoordinate, (geometry as ILine).EndCoordinate };
                _parts = new Int32[] { 0 };
            }
            else if (geometry is ILineString)
            {
                _type = geometry.SpatialDimension == 3 ? ShapeType.PolyLineZ : ShapeType.PolyLine;
                _coordinates = (geometry as ILineString).Coordinates.ToArray();
                _parts = new Int32[] { 0 }; 
            }
            else if (geometry is IPolygon)
            {
                IPolygon sourcePolygon = geometry as IPolygon;

                _type = geometry.SpatialDimension == 3 ? ShapeType.PolygonZ : ShapeType.Polygon;

                List<Coordinate> coordinates = new List<Coordinate>(sourcePolygon.Shell.Coordinates);
                List<Int32> parts = new List<Int32>() { 0 };

                foreach (ILinearRing linearRing in sourcePolygon.Holes)
                {
                    parts.Add(coordinates.Count);
                    coordinates.AddRange(linearRing.Coordinates);
                }

                _coordinates = coordinates.ToArray();
                _parts = parts.ToArray();
            }
            else if (geometry is IMultiPoint)
            {
                _type = geometry.CoordinateDimension == 3 ? ShapeType.MultiPointZ : ShapeType.MultiPoint;
                _coordinates = (geometry as IMultiPoint).Select(point => point.Centroid).ToArray();
            }
            else
            {
                throw new ArgumentException(String.Format("Geometry type {0} is not supported by stream.", geometry.Name), "geometry");
            }

            _envelope = Envelope.FromCoordinates(Coordinates);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Transform the data to a geometry representation.
        /// </summary>
        /// <returns>The geometry representation of the data.</returns>
        /// <exception cref="System.NotSupportedException">Multipatch conversion is not supported.</exception>
        public IGeometry ToGeometry()
        {
            switch (Type)
            {
                case ShapeType.Point:
                case ShapeType.PointM:
                case ShapeType.PointZ:
                    return _factory.CreatePoint(Coordinates[0].X, Coordinates[0].Y, Coordinates[0].Z, _metadata);

                case ShapeType.PolyLine:
                case ShapeType.PolyLineM:
                case ShapeType.PolyLineZ:
                    if (PartCount > 1)
                    {
                        Coordinate[][] lines = new Coordinate[PartCount][];
                        for (Int32 i = 0; i < PartCount - 1; i++)
                        {
                            lines[i] = new Coordinate[_parts[i + 1] - _parts[i]];
                            Array.Copy(_coordinates, _parts[i], lines[i], 0, _parts[i + 1] - _parts[i]);
                        }
                        lines[_parts.Length - 1] = new Coordinate[_coordinates.Length - _parts[_parts.Length - 1]];
                        Array.Copy(_coordinates, Parts[_parts.Length - 1], lines[_parts.Length - 1], 0, _coordinates.Length - _parts[_parts.Length - 1]);

                        List<ILineString> lineStringList = new List<ILineString>();
                        foreach (Coordinate[] line in lines)
                        {
                            lineStringList.Add(_factory.CreateLineString(line));
                        }

                        return _factory.CreateMultiLineString(lineStringList, _metadata);
                    }
                    else
                    {
                        return _factory.CreateLineString(_coordinates, _metadata);
                    }

                case ShapeType.Polygon:
                case ShapeType.PolygonM:
                case ShapeType.PolygonZ:
                    if (PartCount > 1) // if there are more parts (interior rings or multiple polygons are added)
                    {
                        Coordinate[] partCoordinates = null;
                        List<IPolygon> polygons = new List<IPolygon>();

                        // all parts except final
                        for (Int32 i = 0; i < PartCount - 1; i++)
                        {
                            if (Parts[i + 1] == Parts[i]) // in case the part is empty (should never happen) 
                                continue;

                            partCoordinates = new Coordinate[Parts[i + 1] - Parts[i]];
                            Array.Copy(_coordinates, _parts[i], partCoordinates, 0, _parts[i + 1] - _parts[i]);

                            // check whether the current part is an outer or inner ring
                            switch (PolygonAlgorithms.Orientation(partCoordinates))
                            { 
                                case Orientation.Clockwise:
                                    // in case of outer ring a new polygon is created
                                    polygons.Add(_factory.CreatePolygon(partCoordinates, _metadata));
                                    break;
                                case Orientation.CounterClockwise:
                                    // inner rings are simply added to the last polygon
                                    Array.Reverse(partCoordinates);
                                    polygons[polygons.Count - 1].AddHole(_factory.CreateLinearRing(partCoordinates));
                                    break;
                            }

                        }

                        // final part
                        partCoordinates = new Coordinate[_coordinates.Length - _parts[_parts.Length - 1]];
                        Array.Copy(_coordinates, _parts[_parts.Length - 1], partCoordinates, 0, _coordinates.Length - _parts[_parts.Length - 1]);

                        // check whether the current part is an outer or inner ring
                        switch (PolygonAlgorithms.Orientation(partCoordinates))
                        {
                            case Orientation.Clockwise:
                                // in case of outer ring a new polygon is created
                                polygons.Add(_factory.CreatePolygon(partCoordinates, _metadata));
                                break;
                            case Orientation.CounterClockwise:
                                // inner rings are simply added to the last polygon
                                polygons[polygons.Count - 1].AddHole(_factory.CreateLinearRing(partCoordinates));
                                break;
                        }

                        // if there are more than one outer rings a multipolygon will be created
                        if (polygons.Count == 1)
                            return polygons[0];
                        else
                            return _factory.CreateMultiPolygon(polygons, _metadata);
                    }
                    else
                    {
                        return _factory.CreatePolygon(_coordinates, _metadata);
                    }
                case ShapeType.MultiPoint:
                case ShapeType.MultiPointM:
                case ShapeType.MultiPointZ:
                    return _factory.CreateMultiPoint(_coordinates, _metadata);
                case ShapeType.MultiPatch:
                    throw new NotSupportedException("Multipatch conversion is not supported.");
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts the shape into a binary record.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="number">The number of the shape used for indexing.</param>
        /// <returns>The byte array representing the <paramref name="shape" />.</returns>
        public Byte[] ToRecord(Int32 number)
        {
            Byte[] shapeBytes;
            switch (Type) // compute the size of the array according to the type
            { 
                case ShapeType.Point:
                    shapeBytes = new Byte[28];
                    break;
                case ShapeType.PointM:
                    shapeBytes = new Byte[36];
                    break;
                case ShapeType.PointZ:
                    shapeBytes = new Byte[44];
                    break;
                case ShapeType.MultiPoint:
                    shapeBytes = new Byte[48 + CoordinateCount * 16];
                    break;
                case ShapeType.MultiPointM:
                    shapeBytes = new Byte[64 + CoordinateCount * 24];
                    break;
                case ShapeType.MultiPointZ:
                    shapeBytes = new Byte[80 + CoordinateCount * 32];
                    break;
                case ShapeType.PolyLine:
                case ShapeType.Polygon:
                    shapeBytes = new Byte[52 + PartCount * 4 + CoordinateCount * 16];
                    break;
                case ShapeType.PolyLineM:
                case ShapeType.PolygonM:
                    shapeBytes = new Byte[68 + PartCount * 4 + CoordinateCount * 24];
                    break;
                default:
                    shapeBytes = new Byte[12];
                    break;
            }
            // compute header information of the record
            EndianBitConverter.CopyBytes(number, shapeBytes, 0, ByteOrder.BigEndian);
            EndianBitConverter.CopyBytes(shapeBytes.Length / 2 - 4, shapeBytes, 4, ByteOrder.BigEndian);
            EndianBitConverter.CopyBytes((Int32)_type, shapeBytes, 8, ByteOrder.LittleEndian);

            // compute envelope coordanates and shape coordinates
            switch (Type)
            {
                case ShapeType.Point:
                case ShapeType.PointM:
                    EndianBitConverter.GetBytes(Coordinates[0], 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 12);
                    break;
                case ShapeType.PointZ:
                    EndianBitConverter.GetBytes(Coordinates[0], 3, ByteOrder.LittleEndian).CopyTo(shapeBytes, 12);
                    break;
                case ShapeType.MultiPoint:
                case ShapeType.MultiPointM:
                    EndianBitConverter.GetBytes(Envelope.Minimum, 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 12);
                    EndianBitConverter.GetBytes(Envelope.Maximum, 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 28);
                    EndianBitConverter.GetBytes(CoordinateCount, ByteOrder.LittleEndian).CopyTo(shapeBytes, 44);
                    for(Int32 i = 0; i < CoordinateCount; i++)
                    {
                        EndianBitConverter.GetBytes(Coordinates[i], 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 48 + i * 16);
                    }
                    break;
                case ShapeType.MultiPointZ:
                    EndianBitConverter.GetBytes(Envelope.Minimum, 3, ByteOrder.LittleEndian).CopyTo(shapeBytes, 12);
                    EndianBitConverter.GetBytes(Envelope.Maximum, 3, ByteOrder.LittleEndian).CopyTo(shapeBytes, 28);
                    EndianBitConverter.GetBytes(CoordinateCount, ByteOrder.LittleEndian).CopyTo(shapeBytes, 44);
                    for (Int32 i = 0; i < CoordinateCount; i++)
                    {
                        EndianBitConverter.GetBytes(Coordinates[i], 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 48 + i * 16);
                    }
                    EndianBitConverter.GetBytes(Envelope.Minimum.Z, ByteOrder.LittleEndian).CopyTo(shapeBytes, 48 + CoordinateCount * 16);
                    EndianBitConverter.GetBytes(Envelope.Maximum.Z, ByteOrder.LittleEndian).CopyTo(shapeBytes, 56 + CoordinateCount * 16);
                    for (Int32 i = 0; i < CoordinateCount; i++)
                    {
                        EndianBitConverter.GetBytes(Coordinates[i].Z, ByteOrder.LittleEndian).CopyTo(shapeBytes, 64 + CoordinateCount * 16 + i * 8);
                    }                    
                    break;
                case ShapeType.PolyLine:
                case ShapeType.PolyLineM:
                case ShapeType.Polygon:
                case ShapeType.PolygonM:
                    EndianBitConverter.GetBytes(Envelope.Minimum, 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 12);
                    EndianBitConverter.GetBytes(Envelope.Maximum, 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 28);
                    EndianBitConverter.GetBytes(PartCount, ByteOrder.LittleEndian).CopyTo(shapeBytes, 44);
                    EndianBitConverter.GetBytes(CoordinateCount, ByteOrder.LittleEndian).CopyTo(shapeBytes, 48);
                    for (Int32 i = 0; i < PartCount; i++)
                    {
                        EndianBitConverter.GetBytes(Parts[i], ByteOrder.LittleEndian).CopyTo(shapeBytes, 52 + i * 16);
                    }
                    for(Int32 i = 0; i < CoordinateCount; i++)
                    {
                        EndianBitConverter.GetBytes(Coordinates[i], 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 52 + PartCount * 4 + i * 16);
                    }
                    break;
                case ShapeType.PolyLineZ:
                case ShapeType.PolygonZ:
                    EndianBitConverter.GetBytes(Envelope.Minimum, 3, ByteOrder.LittleEndian).CopyTo(shapeBytes, 12);
                    EndianBitConverter.GetBytes(Envelope.Maximum, 3, ByteOrder.LittleEndian).CopyTo(shapeBytes, 28);
                    EndianBitConverter.GetBytes(PartCount, ByteOrder.LittleEndian).CopyTo(shapeBytes, 44);
                    EndianBitConverter.GetBytes(CoordinateCount, ByteOrder.LittleEndian).CopyTo(shapeBytes, 48);
                    for (Int32 i = 0; i < PartCount; i++)
                    {
                        EndianBitConverter.GetBytes(Parts[i], ByteOrder.LittleEndian).CopyTo(shapeBytes, 52 + i * 16);
                    }
                    for(Int32 i = 0; i < CoordinateCount; i++)
                    {
                        EndianBitConverter.GetBytes(Coordinates[i], 2, ByteOrder.LittleEndian).CopyTo(shapeBytes, 52 + PartCount * 4 + i * 16);
                    }
                    EndianBitConverter.GetBytes(Envelope.Minimum.Z, ByteOrder.LittleEndian).CopyTo(shapeBytes, 52 + PartCount * 4 + CoordinateCount * 16);
                    EndianBitConverter.GetBytes(Envelope.Maximum.Z, ByteOrder.LittleEndian).CopyTo(shapeBytes, 60 + PartCount * 4 + CoordinateCount * 16);
                    for (Int32 i = 0; i < CoordinateCount; i++)
                    {
                        EndianBitConverter.GetBytes(Coordinates[i].Z, ByteOrder.LittleEndian).CopyTo(shapeBytes, 68 + PartCount * 4 + CoordinateCount * 16 + i * 8);
                    }
                    break;
            }

            return shapeBytes;
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Creates a shape from a record.
        /// </summary>
        /// <param name="recordNumber">The record number.</param>
        /// <param name="recordContents">The contents of the record.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The shape created from the record data.</returns>
        /// <exception cref="System.ArgumentNullException">recordContents;The record contents are not specified.</exception>
        /// <exception cref="System.ArgumentException">
        /// Record contents length is less than 4 bytes.;recordContents
        /// or
        /// Record content is invalid.;recordContents
        /// </exception>
        public static Shape FromRecord(Int32 recordNumber, Byte[] recordContents, IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            if (recordContents == null)
                throw new ArgumentNullException("recordContents", "The record contents are not specified.");

            if (recordContents.Length < 4)
                throw new ArgumentException("Record contents length is less than 4 bytes.", "recordContents");

            ShapeType shapeType = (ShapeType)EndianBitConverter.ToInt32(recordContents, 0);

            if (shapeType == ShapeType.Null)
                throw new ArgumentException("Record content is invalid.", "recordContents");

            try
            {
                Int32 coordinateCount, partCount;
                Int32[] parts = null;
                ShapePartType[] partTypes = null;
                Coordinate[] coordinates = null;
                switch (shapeType)
                {
                    case ShapeType.Point:
                    case ShapeType.PointM:
                        return new Shape(recordNumber, shapeType, EndianBitConverter.ToCoordinate(recordContents, 4, 2), factory, metadata);
                    case ShapeType.PointZ:
                        return new Shape(recordNumber, shapeType, EndianBitConverter.ToCoordinate(recordContents, 4, 3), factory, metadata);
                    case ShapeType.MultiPoint:
                    case ShapeType.MultiPointM:
                        coordinateCount = EndianBitConverter.ToInt32(recordContents, 36);
                        coordinates = new Coordinate[coordinateCount];
                        for (Int32 i = 0; i < coordinateCount; i++)
                            coordinates[i] = EndianBitConverter.ToCoordinate(recordContents, 40 + i * 16, 2);
                        return new Shape(recordNumber, shapeType, coordinates, factory, metadata);
                    case ShapeType.MultiPointZ:
                        coordinateCount = EndianBitConverter.ToInt32(recordContents, 36);
                        coordinates = new Coordinate[coordinateCount];
                        for (Int32 i = 0; i < coordinateCount; i++)
                            coordinates[i] = new Coordinate(EndianBitConverter.ToDouble(recordContents, 40 + i * 16), EndianBitConverter.ToDouble(recordContents, 48 + i * 16), EndianBitConverter.ToDouble(recordContents, 48 + coordinateCount * 24 + i * 8));
                        return new Shape(recordNumber, shapeType, coordinates, factory, metadata);
                    case ShapeType.PolyLine:
                    case ShapeType.PolyLineM:
                    case ShapeType.Polygon:
                    case ShapeType.PolygonM:
                        partCount = EndianBitConverter.ToInt32(recordContents, 36);
                        coordinateCount = EndianBitConverter.ToInt32(recordContents, 40);
                        parts = new Int32[partCount];
                        for (Int32 i = 0; i < partCount; i++)
                            parts[i] = EndianBitConverter.ToInt32(recordContents, 44 + 4 * i);
                        coordinates = new Coordinate[coordinateCount];
                        for (Int32 i = 0; i < coordinateCount; i++)
                            coordinates[i] = EndianBitConverter.ToCoordinate(recordContents, 44 + partCount * 4 + i * 16, 2);
                        return new Shape(recordNumber, shapeType, coordinates, parts, factory, metadata);
                    case ShapeType.PolyLineZ:
                    case ShapeType.PolygonZ:
                        partCount = EndianBitConverter.ToInt32(recordContents, 36);
                        coordinateCount = EndianBitConverter.ToInt32(recordContents, 40);
                        parts = new Int32[partCount];
                        for (Int32 i = 0; i < partCount; i++)
                            parts[i] = EndianBitConverter.ToInt32(recordContents, 44 + 4 * i);
                        coordinates = new Coordinate[coordinateCount];
                        for (Int32 i = 0; i < coordinateCount; i++)
                            coordinates[i] = new Coordinate(EndianBitConverter.ToDouble(recordContents, 44 + partCount * 4 + i * 16), EndianBitConverter.ToDouble(recordContents, 52 + partCount * 4 + i * 16), EndianBitConverter.ToDouble(recordContents, 60 + partCount * 4 + coordinateCount * 24 + i * 8));
                        return new Shape(recordNumber, shapeType, coordinates, parts, factory, metadata);
                    case ShapeType.MultiPatch:
                        partCount = EndianBitConverter.ToInt32(recordContents, 36);
                        coordinateCount = EndianBitConverter.ToInt32(recordContents, 40);
                        parts = new Int32[partCount];
                        for (Int32 i = 0; i < partCount; i++)
                            parts[i] = EndianBitConverter.ToInt32(recordContents, 44 + 4 * i);
                        partTypes = new ShapePartType[partCount];
                        for (Int32 i = 0; i < partCount; i++)
                            partTypes[i] = (ShapePartType)EndianBitConverter.ToInt32(recordContents, 44 + 4 * partCount + 4 * i);
                        coordinates = new Coordinate[coordinateCount];
                        for (Int32 i = 0; i < coordinateCount; i++)
                            coordinates[i] = new Coordinate(EndianBitConverter.ToDouble(recordContents, 44 + partCount * 8 + i * 16), EndianBitConverter.ToDouble(recordContents, 52 + partCount * 8 + i * 16), EndianBitConverter.ToDouble(recordContents, 60 + partCount * 8 + coordinateCount * 24 + i * 8));
                        return new Shape(recordNumber, shapeType, coordinates, parts, partTypes, factory, metadata);
                    default:
                        throw new ArgumentException();
                }
            }
            catch
            {
                throw new ArgumentException("Record content is invalid.", "recordContents");
            }
        }
        /// <summary>
        /// Creates a shape from a record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="factory">The factory used for geometry production.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The shape created from the record data.</returns>
        /// <exception cref="System.ArgumentNullException">record;The record is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// Record length is less tahn 12 bytes.;record
        /// or
        /// Record content is invalid.;record
        /// </exception>
        public static Shape FromRecord(Byte[] record, IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            if (record == null)
                throw new ArgumentNullException("record", "The record is null.");

            if (record.Length < 12)
                throw new ArgumentException("Record length is less tahn 12 bytes.", "record");

            Int32 recordNumber = EndianBitConverter.ToInt32(record, 0, ByteOrder.BigEndian);
            Int32 contentLength = EndianBitConverter.ToInt32(record, 4, ByteOrder.BigEndian);

            if (record.Length != (contentLength * 2) + 8)
                throw new ArgumentException("Record content is invalid.", "record");

            Byte[] recordContents = new Byte[contentLength * 2];
            Array.Copy(record, 8, recordContents, 0, contentLength * 2);

            return FromRecord(recordNumber, recordContents, factory, metadata);
        }

        #endregion
    }
}