/// <copyright file="GeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a factory for producing <see cref="Geometry" /> instances.
    /// </summary>
    /// <remarks>
    /// This implementation of <see cref="IGeometryFactory" /> produces geometries in coordinate space.
    /// </remarks>
    public class GeometryFactory : Factory, IGeometryFactory
    {
        #region IGeometryFactory properties

        /// <summary>
        /// Gets the precision model used by the factory.
        /// </summary>
        /// <value>The precision model used by the factory.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the reference system used by the factory.
        /// </summary>
        /// <value>The reference system used by the factory.</value>
        public IReferenceSystem ReferenceSystem { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryFactory" /> class.
        /// </summary>
        public GeometryFactory()
            : this(PrecisionModel.Default, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryFactory" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        public GeometryFactory(IReferenceSystem referenceSystem)
            : this(PrecisionModel.Default, referenceSystem, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryFactory" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        public GeometryFactory(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
            : this(precisionModel, referenceSystem, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryFactory" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadataFactory">The metadata factory.</param>
        public GeometryFactory(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IMetadataFactory metadataFactory)
            : base(metadataFactory ?? new MetadataFactory())
        {
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            ReferenceSystem = referenceSystem;
        }

        #endregion

        #region Factory methods for points

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The point with the specified X, Y coordinates.</returns>
        public virtual IPoint CreatePoint(Double x, Double y)
        {
            return new Point(x, y, 0, this, null);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The point with the specified X, Y coordinates and metadata.</returns>
        public virtual IPoint CreatePoint(Double x, Double y, IDictionary<String, Object> metadata)
        {
            return new Point(x, y, 0, this, metadata);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>The point with the specified X, Y, Z coordinates.</returns>
        public virtual IPoint CreatePoint(Double x, Double y, Double z)
        {
            return new Point(x, y, z, this, null);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The point with the specified X, Y, Z coordinates and metadata.</returns>
        public virtual IPoint CreatePoint(Double x, Double y, Double z, IDictionary<String, Object> metadata)
        {
            return new Point(x, y, z, this, metadata);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <returns>The point with the specified coordinate.</returns>
        public virtual IPoint CreatePoint(Coordinate coordinate)
        {
            return new Point(coordinate.X, coordinate.Y, coordinate.Z, this, null);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The point with the specified coordinate and metadata.</returns>
        public virtual IPoint CreatePoint(Coordinate coordinate, IDictionary<String, Object> metadata)
        {
            return new Point(coordinate.X, coordinate.Y, coordinate.Z, this, metadata);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>A point that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other point is null.</exception>
        public virtual IPoint CreatePoint(IPoint other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other point is null.");

            return new Point(other.X, other.Y, other.Z, this, other.Metadata);
        }

        #endregion

        #region Factory methods for line strings

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <returns>An empty line string.</returns>
        public virtual ILineString CreateLineString()
        {
            return new LineString(this, null);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty line string containing the specified metadata.</returns>
        public virtual ILineString CreateLineString(IDictionary<String, Object> metadata)
        {
            return new LineString(this, metadata);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        public virtual ILineString CreateLineString(params Coordinate[] source)
        {
            return new LineString(source, this, null);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        public virtual ILineString CreateLineString(params IPoint[] source)
        {
            if (source == null)
                return new LineString(null, this, null);
            else
                return new LineString(source.Select(point => point.Coordinate), this, null);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        public virtual ILineString CreateLineString(IEnumerable<Coordinate> source)
        {
            return new LineString(source, this, null);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A line string containing the specified coordinates and metadata.</returns>
        public virtual ILineString CreateLineString(IEnumerable<Coordinate> source, IDictionary<String, Object> metadata)
        {
            return new LineString(source, this, metadata);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        public virtual ILineString CreateLineString(IEnumerable<IPoint> source)
        {
            if (source == null)
                return new LineString(this, null);
            else
                return new LineString(source.Select(point => point.Coordinate), this, null);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The points.</param>
        /// <returns>A line string containing the specified points and metadata.</returns>
        public virtual ILineString CreateLineString(IEnumerable<IPoint> source, IDictionary<String, Object> metadata)
        {
            if (source == null)
                return new LineString(this, metadata);
            else
                return new LineString(source.Select(point => point.Coordinate), this, metadata);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source line string.</param>
        /// <returns>A line string that matches <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other line string is null.</exception>
        public virtual ILineString CreateLineString(ILineString source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source line string is null.");

            return new LineString(source, this, source.Metadata);
        }

        #endregion

        #region Factory methods for lines

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The startint coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <returns>A line containing the specified coordinates.</returns>
        public virtual ILine CreateLine(Coordinate start, Coordinate end)
        {
            return new Line(start, end, this, null);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The startint coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A line containing the specified coordinates.</returns>
        public virtual ILine CreateLine(Coordinate start, Coordinate end, IDictionary<String, Object> metadata)
        {
            return new Line(start, end, this, metadata);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="end">The ending point.</param>
        /// <returns>A line containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The start point is null.
        /// or
        /// The end point is null.
        /// </exception>
        public virtual ILine CreateLine(IPoint start, IPoint end)
        {
            if (start == null)
                throw new ArgumentNullException("start", "The start point is null.");
            if (end == null)
                throw new ArgumentNullException("end", "The end point is null.");

            return new Line(start.Coordinate, end.Coordinate, this, null);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="end">The ending point.</param>
        /// <returns>A line containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The start point is null.
        /// or
        /// The end point is null.
        /// </exception>
        public virtual ILine CreateLine(IPoint start, IPoint end, IDictionary<String, Object> metadata)
        {
            if (start == null)
                throw new ArgumentNullException("start", "The start point is null.");
            if (end == null)
                throw new ArgumentNullException("end", "The end point is null.");

            return new Line(start.Coordinate, end.Coordinate, this, metadata);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <returns>A line that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other line is null.</exception>
        public virtual ILine CreateLine(ILine other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other line is null.");

            return new Line(other.StartCoordinate, other.EndCoordinate, this, other.Metadata);
        }

        #endregion

        #region Factory methods for linear rings

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.</exception>
        public virtual ILinearRing CreateLinearRing(params Coordinate[] source)
        {
            return new LinearRing(source, this, null);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.</exception>
        public virtual ILinearRing CreateLinearRing(params IPoint[] source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            return new LinearRing(source.Select(point => point.Coordinate), this, null);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.</exception>
        public virtual ILinearRing CreateLinearRing(IEnumerable<Coordinate> source)
        {
            return new LinearRing(source, this, null);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A linear ring containing the specified coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.</exception>
        public virtual ILinearRing CreateLinearRing(IEnumerable<Coordinate> source, IDictionary<String, Object> metadata)
        {
            return new LinearRing(source, this, metadata);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.</exception>
        public virtual ILinearRing CreateLinearRing(IEnumerable<IPoint> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            return new LinearRing(source.Select(point => point.Coordinate), this, null);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The points.</param>
        /// <returns>A linear ring containing the specified points and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.</exception>
        public virtual ILinearRing CreateLinearRing(IEnumerable<IPoint> source, IDictionary<String, Object> metadata)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            return new LinearRing(source.Select(point => point.Coordinate), this, metadata);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <returns>A linear ring that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other linear ring is null.</exception>
        public virtual ILinearRing CreateLinearRing(ILinearRing other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other linear ring is null.");

            return new LinearRing(other, this, other.Metadata);
        }

        #endregion

        #region Factory methods for polygons

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">public The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual IPolygon CreatePolygon(params Coordinate[] shell)
        {
            return new Polygon(shell, null, this, null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The points of the shell.</param>
        /// <returns>A polygon containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual IPolygon CreatePolygon(params IPoint[] shell)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return new Polygon(shell.Select(point => point.Coordinate), null, this, null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual IPolygon CreatePolygon(IEnumerable<Coordinate> shell)
        {
            return new Polygon(shell, null, this, null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            return new Polygon(shell, holes, this, null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata)
        {
            return new Polygon(shell, null, this, metadata);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata)
        {
            return new Polygon(shell, holes, this, metadata);
        }   

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other polygon is null.</exception>
        public virtual IPolygon CreatePolygon(IPolygon other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other polygon is null.");

            return new Polygon(other.Shell, other.Holes, this, other.Metadata);
        }

        #endregion

        #region Factory methods for triangles

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <returns>The triangle containing the specified coordinates.</returns>
        public virtual ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third)
        {
            return new Triangle(first, second, third, this, null);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The triangle containing the specified coordinates and metadata.</returns>
        public virtual ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third, IDictionary<String, Object> metadata)
        {
            return new Triangle(first, second, third, this, metadata);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <param name="third">The third point.</param>
        /// <returns>The triangle containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first point is null.
        /// or
        /// The second point is null.
        /// or
        /// The third point is null.
        /// </exception>
        public virtual ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first point is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second point is null.");
            if (third == null)
                throw new ArgumentNullException("third", "The third point is null.");

            return new Triangle(first.Coordinate, second.Coordinate, third.Coordinate, this, null);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <param name="third">The third point.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The triangle containing the specified points and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first point is null.
        /// or
        /// The second point is null.
        /// or
        /// The third point is null.
        /// </exception>
        public virtual ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third, IDictionary<String, Object> metadata)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first point is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second point is null.");
            if (third == null)
                throw new ArgumentNullException("third", "The third point is null.");

            return new Triangle(first.Coordinate, second.Coordinate, third.Coordinate, this, metadata);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <returns>A triangle that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other triangle is null.</exception>
        public virtual ITriangle CreateTriangle(ITriangle other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other polygon is null.");

            return new Triangle(other.Shell.Coordinates[0], other.Shell.Coordinates[1], other.Shell.Coordinates[2], this, other.Metadata);
        }

        #endregion

        #region Factory methods for geometry collections

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <returns>The empty geometry collection.</returns>
        public virtual IGeometryCollection<IGeometry> CreateGeometryCollection()
        {
            return new GeometryList<IGeometry>(this, null);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty geometry collection with the specified metadata.</returns>
        public virtual IGeometryCollection<IGeometry> CreateGeometryCollection(IDictionary<String, Object> metadata)
        {
            return new GeometryList<IGeometry>(this, metadata);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        public virtual IGeometryCollection<IGeometry> CreateGeometryCollection(IEnumerable<IGeometry> geometries)
        {
            return new GeometryList<IGeometry>(geometries, this, null);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The geometry collection containing the specified geometries and metadata.</returns>
        public virtual IGeometryCollection<IGeometry> CreateGeometryCollection(IEnumerable<IGeometry> geometries, IDictionary<String, Object> metadata)
        {
            return new GeometryList<IGeometry>(geometries, this, metadata);
        }
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public virtual IGeometryCollection<IGeometry> CreateGeometryCollection(IGeometryCollection<IGeometry> other)
        {
            return new GeometryList<IGeometry>(other, this, other.Metadata);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <returns>The empty geometry collection.</returns>
        public virtual IGeometryCollection<T> CreateGeometryCollection<T>() where T : IGeometry
        {
            return new GeometryList<T>(this, null);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty geometry collection with the specified metadata.</returns>
        public virtual IGeometryCollection<T> CreateGeometryCollection<T>(IDictionary<String, Object> metadata) where T : IGeometry
        {
            return new GeometryList<T>(this, metadata);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        public virtual IGeometryCollection<T> CreateGeometryCollection<T>(IEnumerable<T> geometries) where T : IGeometry
        {
            return new GeometryList<T>(geometries, this, null);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The geometry collection containing the specified geometries and metadata.</returns>
        public virtual IGeometryCollection<T> CreateGeometryCollection<T>(IEnumerable<T> geometries, IDictionary<String, Object> metadata) where T : IGeometry
        {
            return new GeometryList<T>(geometries, this, metadata);
        }
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public virtual IGeometryCollection<T> CreateGeometryCollection<T>(IGeometryCollection<T> other) where T : IGeometry
        {
            return new GeometryList<T>(other, this, other.Metadata);
        }

        #endregion

        #region Factory methods for multi points

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <returns>The empty multi point.</returns>
        public virtual IMultiPoint CreateMultiPoint()
        {
            return new MultiPoint(this, null);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty multi point with the specified metadata.</returns>
        public virtual IMultiPoint CreateMultiPoint(IDictionary<String, Object> metadata)
        {
            return new MultiPoint(this, metadata);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="coordinates">The source coordinates.</param>
        /// <returns>The multi point containing the specified coordinates.</returns>
        public virtual IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> coordinates)
        {
            return new MultiPoint(coordinates.Select(coordinate => CreatePoint(coordinate.X, coordinate.Y, coordinate.Z)), this, null);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="coordinates">The source coordinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi point containing the specified coordinates and metadata.</returns>
        public virtual IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> coordinates, IDictionary<String, Object> metadata)
        {
            return new MultiPoint(coordinates.Select(coordinate => CreatePoint(coordinate.X, coordinate.Y, coordinate.Z)), this, metadata);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source points.</param>
        /// <returns>The multi point containing the specified points.</returns>
        public virtual IMultiPoint CreateMultiPoint(IEnumerable<IPoint> points)
        {
            return new MultiPoint(points, this, null);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source points.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi point containing the specified points and metadata.</returns>
        public virtual IMultiPoint CreateMultiPoint(IEnumerable<IPoint> points, IDictionary<String, Object> metadata)
        {
            return new MultiPoint(points, this, metadata);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <returns>A multi point that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi point is null.</exception>
        public virtual IMultiPoint CreateMultiPoint(IMultiPoint other)
        {
            return new MultiPoint(other, this, other.Metadata);
        }

        #endregion

        #region Factory methods for multi line strings

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <returns>The empty multi line string.</returns>
        public virtual IMultiLineString CreateMultiLineString()
        {
            return new MultiLineString(this, null);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi line string with the specified metadata.</returns>
        public virtual IMultiLineString CreateMultiLineString(IDictionary<String, Object> metadata)
        {
            return new MultiLineString(this, metadata);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="lineStrings">The source line strings.</param>
        /// <returns>The multi line string containing the specified line strings.</returns>
        public virtual IMultiLineString CreateMultiLineString(IEnumerable<ILineString> lineStrings)
        {
            return new MultiLineString(lineStrings, this, null);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="lineStrings">The source line strings.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi line string containing the specified line strings and metadata.</returns>
        public virtual IMultiLineString CreateMultiLineString(IEnumerable<ILineString> lineStrings, IDictionary<String, Object> metadata)
        {
            return new MultiLineString(lineStrings, this, metadata);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <returns>A multi line string that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi line string is null.</exception>
        public virtual IMultiLineString CreateMultiLineString(IMultiLineString other)
        {
            return new MultiLineString(other, this, other.Metadata);
        }

        #endregion

        #region Factory methods for multi polygons

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <returns>The empty multi polygon.</returns>
        public virtual IMultiPolygon CreateMultiPolygon()
        {
            return new MultiPolygon(this, null);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty multi polygon with the specified metadata.</returns>
        public virtual IMultiPolygon CreateMultiPolygon(IDictionary<String, Object> metadata)
        {
            return new MultiPolygon(this, metadata);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="polygons">The source polygons.</param>
        /// <returns>The multi polygon containing the specified polygons.</returns>
        public virtual IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> polygons)
        {
            return new MultiPolygon(polygons, this, null);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="polygons">The source polygons.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi polygon containing the specified polygons and metadata.</returns>
        public virtual IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> polygons, IDictionary<String, Object> metadata)
        {
            return new MultiPolygon(polygons, this, metadata);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <returns>A multi polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi polygon is null.</exception>
        public virtual IMultiPolygon CreateMultiPolygon(IMultiPolygon other)
        {
            return new MultiPolygon(other, this, other.Metadata);
        }

        #endregion

        #region Factory methods for geometries

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="other">The other geometry.</param>
        /// <returns>The produced geometry matching <see cref="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        public IGeometry CreateGeometry(IGeometry other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other geometry is null.");

            if (other is IPoint)
                return CreatePoint(other as IPoint);
            if (other is ILine)
                return CreateLine(other as ILine);
            if (other is ILinearRing)
                return CreateLinearRing(other as ILinearRing);
            if (other is ILineString)
                return CreateLineString(other as ILineString);
            if (other is ITriangle)
                return CreateTriangle(other as ITriangle);
            if (other is IPolygon)
                return CreatePolygon(other as IPolygon);
            if (other is IMultiPoint)
                return CreateMultiPoint(other as IMultiPoint);
            if (other is IMultiLineString)
                return CreateMultiLineString(other as IMultiLineString);
            if (other is IMultiPolygon)
                return CreateMultiPolygon(other as IMultiPolygon);
            if (other is IGeometryCollection<IGeometry>)
                return CreateGeometryCollection(other as IGeometryCollection<IGeometry>);

            throw new ArgumentException("other", "The type of the other geometry is not supported.");
        }

        #endregion
    }
}
