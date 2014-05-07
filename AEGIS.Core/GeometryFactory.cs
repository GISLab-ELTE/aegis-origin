using System;
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a factory type for producing <see cref="T:ELTE.AEGIS.IGeometry" /> instances.
    /// </summary>
    public abstract class GeometryFactory
    {
        #region Singleton pattern

        private static GeometryFactory _instance;

        /// <summary>
        /// Gets or sets the current <see cref="T:ELTE.AEGIS.GeometryFactory" /> instance.
        /// </summary>
        /// <value>The current <see cref="T:ELTE.AEGIS.GeometryFactory" /> instance.</value>
        public static GeometryFactory Current
        {
            get { if (_instance == null) _instance = new ELTE.AEGIS.Geometry.CoordinateGeometryFactory(); return _instance; }
            set { _instance = value; }
        }
        /// <summary>
        /// Gets the default <see cref="T:ELTE.AEGIS.GeometryFactory"/> instance.
        /// </summary>
        /// <value>The default <see cref="T:ELTE.AEGIS.GeometryFactory"/> instance.</value>
        public static GeometryFactory Default
        {
            get { return new ELTE.AEGIS.Geometry.CoordinateGeometryFactory(); }
        }

        #endregion

        #region Private fields

        private IReferenceSystem _referenceSystem;
        private Boolean _copyReferenceSystem;
        private Boolean _copyMetadata;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the reference system used by the factory.
        /// </summary>
        /// <value>The reference system used by the factory.</value>
        public virtual IReferenceSystem ReferenceSystem { get { return _referenceSystem; } set { _referenceSystem = value; } }
        /// <summary>
        /// Gets or a sets a value indicating whether the factory should copy the reference system of the original geometry.
        /// </summary>
        /// <value><c>true</c> if the factory should copy the reference system of the original geometry; otherwise <c>false</c>.</value>
        public virtual Boolean CopyReferenceSystem { get { return _copyReferenceSystem; } set { _copyReferenceSystem = value; } }
        /// <summary>
        /// Gets or a sets a value indicating whether the factory should copy the metadata of the original geometry.
        /// </summary>
        /// <value><c>true</c> if the factory should copy the metadata of the original geometry; otherwise <c>false</c>.</value>
        public virtual Boolean CopyMetadata { get { return _copyMetadata; } set { _copyMetadata = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Geometry.GeometryFactory"/> class.
        /// </summary>
        protected GeometryFactory() 
        {
            _copyReferenceSystem = true;
            _copyMetadata = true;
        }

        #endregion

        #region Factory methods for points

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The point with the specified X, Y coordinates.</returns>
        public abstract IPoint CreatePoint(Double x, Double y);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The point with the specified X, Y coordinates in the specified reference system.</returns>
        public abstract IPoint CreatePoint(Double x, Double y, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The point with the specified X, Y coordinates and metadata.</returns>
        public abstract IPoint CreatePoint(Double x, Double y, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The point with the specified X, Y coordinates and metadata in the specified reference system.</returns>
        public abstract IPoint CreatePoint(Double x, Double y, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>The point with the specified X, Y, Z coordinates.</returns>
        public abstract IPoint CreatePoint(Double x, Double y, Double z);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The point with the specified X, Y, Z coordinates in the specified reference system.</returns>
        public abstract IPoint CreatePoint(Double x, Double y, Double z, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The point with the specified X, Y, Z coordinates and metadata.</returns>
        public abstract IPoint CreatePoint(Double x, Double y, Double z, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The point produced by the factory.</returns>
        public abstract IPoint CreatePoint(Double x, Double y, Double z, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <returns>The point with the specified coordinate.</returns>
        public abstract IPoint CreatePoint(Coordinate coordinate);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The point with the specified coordinate in the specified reference system.</returns>
        public abstract IPoint CreatePoint(Coordinate coordinate, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The point with the specified coordinate and metadata.</returns>
        public abstract IPoint CreatePoint(Coordinate coordinate, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The point with the specified coordinate and metadata in the specified reference system.</returns>
        public abstract IPoint CreatePoint(Coordinate coordinate, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>A point that matches <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other point is null.</exception>
        public abstract IPoint CreatePoint(IPoint other);

        #endregion

        #region Factory methods for line strings

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <returns>An empty line string.</returns>
        public abstract ILineString CreateLineString();
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>An empty line string in the specified reference system.</returns>
        public abstract ILineString CreateLineString(IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty line string containing the specified metadata.</returns>
        public abstract ILineString CreateLineString(IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An empty line string containing the specified metadata in the specified reference system.</returns>
        public abstract ILineString CreateLineString(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        public abstract ILineString CreateLineString(params Coordinate[] source);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        public abstract ILineString CreateLineString(params IPoint[] source);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        public abstract ILineString CreateLineString(IEnumerable<Coordinate> source);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A line string containing the specified coordinates in the specified reference system.</returns>
        public abstract ILineString CreateLineString(IEnumerable<Coordinate> source, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A line string containing the specified coordinates and metadata.</returns>
        public abstract ILineString CreateLineString(IEnumerable<Coordinate> source, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A line string containing the specified coordinates and metadata in the specified reference system.</returns>
        public abstract ILineString CreateLineString(IEnumerable<Coordinate> source, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        public abstract ILineString CreateLineString(IEnumerable<IPoint> source);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A line string containing the specified points in the specified reference system.</returns>
        public abstract ILineString CreateLineString(IEnumerable<IPoint> source, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The points.</param>
        /// <returns>A line string containing the specified points and metadata.</returns>
        public abstract ILineString CreateLineString(IEnumerable<IPoint> source, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The points.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A line string containing the specified points and metadata in the specified reference system.</returns>
        public abstract ILineString CreateLineString(IEnumerable<IPoint> source, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="other">The other line string.</param>
        /// <returns>A line string that matches <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other line string is null.</exception>
        public abstract ILineString CreateLineString(ILineString other);

        #endregion

        #region Factory methods for lines

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The startint coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <returns>A line containing the specified coordinates.</returns>
        public abstract ILine CreateLine(Coordinate start, Coordinate end);
        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The startint coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A line containing the specified coordinates in the specified reference system.</returns>
        public abstract ILine CreateLine(Coordinate start, Coordinate end, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The startint coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A line containing the specified coordinates.</returns>
        public abstract ILine CreateLine(Coordinate start, Coordinate end, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The startint coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A line containing the specified coordinates in the specified reference system.</returns>
        public abstract ILine CreateLine(Coordinate start, Coordinate end, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="end">The ending point.</param>
        /// <returns>A line containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// start;The start point is null.
        /// or
        /// end;The end point is null.
        /// </exception>
        public abstract ILine CreateLine(IPoint start, IPoint end);
        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="end">The ending point.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A line containing the specified points in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// start;The start point is null.
        /// or
        /// end;The end point is null.
        /// </exception>
        public abstract ILine CreateLine(IPoint start, IPoint end, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="end">The ending point.</param>
        /// <returns>A line containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// start;The start point is null.
        /// or
        /// end;The end point is null.
        /// </exception>
        public abstract ILine CreateLine(IPoint start, IPoint end, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="end">The ending point.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A line containing the specified points in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// start;The start point is null.
        /// or
        /// end;The end point is null.
        /// </exception>
        public abstract ILine CreateLine(IPoint start, IPoint end, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <returns>A line that matches <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other line is null.</exception>
        public abstract ILine CreateLine(ILine other);

        #endregion

        #region Factory methods for linear rings

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(params Coordinate[] source);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(params IPoint[] source);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(IEnumerable<Coordinate> source);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A linear ring containing the specified coordinates in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(IEnumerable<Coordinate> source, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A linear ring containing the specified coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(IEnumerable<Coordinate> source, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A linear ring containing the specified coordinates and metadata in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(IEnumerable<Coordinate> source, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(IEnumerable<IPoint> source);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A linear ring containing the specified points in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(IEnumerable<IPoint> source, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The points.</param>
        /// <returns>A linear ring containing the specified points and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(IEnumerable<IPoint> source, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The points.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A linear ring containing the specified points and metadata in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">source;The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.;coordinates</exception>
        public abstract ILinearRing CreateLinearRing(IEnumerable<IPoint> source, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <returns>A linear ring that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other linear ring is null.</exception>
        public abstract ILinearRing CreateLinearRing(ILinearRing other);

        #endregion

        #region Factory methods for polygons

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(params Coordinate[] shell);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The points of the shell.</param>
        /// <returns>A polygon containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(params IPoint[] shell);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(IEnumerable<Coordinate> shell);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A polygon containing the specified coordinates in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A polygon containing the specified coordinates in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified coordinates and metadata in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified coordinates and metadata in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">shell;The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other polygon is null.</exception>
        public abstract IPolygon CreatePolygon(IPolygon other);

        #endregion

        #region Factory methods for triangles

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <returns>The triangle containing the specified coordinates.</returns>
        public abstract ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third);
        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The triangle containing the specified coordinates in the specified reference system.</returns>
        public abstract ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The triangle containing the specified coordinates and metadata.</returns>
        public abstract ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The triangle containing the specified coordinates and metadata in the specified reference system.</returns>
        public abstract ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <param name="third">The third point.</param>
        /// <returns>The triangle containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// first;The first point is null.
        /// or
        /// second;The second point is null.
        /// or
        /// third;The third point is null.
        /// </exception>
        public abstract ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third);
        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <param name="third">The third point.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The triangle containing the specified points in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// first;The first point is null.
        /// or
        /// second;The second point is null.
        /// or
        /// third;The third point is null.
        /// </exception>
        public abstract ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <param name="third">The third point.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The triangle containing the specified points and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// first;The first point is null.
        /// or
        /// second;The second point is null.
        /// or
        /// third;The third point is null.
        /// </exception>
        public abstract ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <param name="third">The third point.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The triangle containing the specified points and metadata in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// first;The first point is null.
        /// or
        /// second;The second point is null.
        /// or
        /// third;The third point is null.
        /// </exception>
        public abstract ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <returns>A triangle that matches <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other triangle is null.</exception>
        public abstract ITriangle CreateTriangle(ITriangle other);

        #endregion

        #region Factory methods for raster geometries

        /// <summary>
        /// Creates a raster geometry to match a specified geometry.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="raster">The raster.</param>
        /// <returns>The raster geometry that matches <paramref name="source"/> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// source;The source is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        public abstract IRasterGeometry CreateRasterGeometry(IRaster raster, IGeometry source);

        #endregion

        #region Factory methods for raster polygons

        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <returns>A raster polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">raster;The raster is null.</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A raster polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">raster;The raster is null.</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A raster polygon containing the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">raster;The raster is null.</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A raster polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// shell;The shell is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IEnumerable<Coordinate> shell);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A polygon containing the specified raster data and coordinates in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// shell;The shell is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IEnumerable<Coordinate> shell, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified raster data and coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// shell;The shell is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A polygon containing the specified raster data and coordinates in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// shell;The shell is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// shell;The shell is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IEnumerable<Coordinate> shell, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// shell;The shell is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IEnumerable<Coordinate> shell, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// shell;The shell is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>A polygon containing the specified raster data, coordinates and metadata in the specified reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// shell;The shell is null.
        /// or
        /// raster;The raster is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.;shell</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="raster">The raster data.</param>
        /// <param name="source">The source polygon.</param>
        /// <returns>A polygon that matches <paramref name="source" /> and contains the specified raster data.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// raster;The raster is null.
        /// or
        /// source;The source polygon is null.
        /// </exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRaster raster, IPolygon source);
        /// <summary>
        /// Creates a raster polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A raster polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other raster polygon is null.</exception>
        public abstract IRasterPolygon CreateRasterPolygon(IRasterPolygon other);

        #endregion

        #region Factory methods for geometry collections

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <returns>The empty geometry collection.</returns>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection();
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The empty geometry collection in the specified reference system.</returns>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection(IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty geometry collection with the specified metadata.</returns>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection(IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty geometry collection with the specified metadata in the specified reference system.</returns>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection(IEnumerable<IGeometry> geometries);
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The geometry collection containing the specified geometries in the specified reference system.</returns>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection(IEnumerable<IGeometry> geometries, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The geometry collection containing the specified geometries and metadata.</returns>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection(IEnumerable<IGeometry> geometries, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The geometry collection containing the specified geometries and metadata in the specified reference system.</returns>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection(IEnumerable<IGeometry> geometries, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other geometry collection is null.</exception>
        public abstract IGeometryCollection<IGeometry> CreateGeometryCollection(IGeometryCollection<IGeometry> other);

        #endregion

        #region Factory methods for multi points

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <returns>The empty multi point.</returns>
        public abstract IMultiPoint CreateMultiPoint();
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The empty multi point in the specified reference system.</returns>
        public abstract IMultiPoint CreateMultiPoint(IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty multi point with the specified metadata.</returns>
        public abstract IMultiPoint CreateMultiPoint(IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty multi point with the specified metadata in the specified reference system.</returns>
        public abstract IMultiPoint CreateMultiPoint(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source coordinates.</param>
        /// <returns>The multi point containing the specified points.</returns>
        public abstract IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> points);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The multi point containing the specified points in the specified reference system.</returns>
        public abstract IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> points, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source coordinates.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi point containing the specified points and metadata.</returns>
        public abstract IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> points, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi point containing the specified points and metadata in the specified reference system.</returns>
        public abstract IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> points, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source points.</param>
        /// <returns>The multi point containing the specified points.</returns>
        public abstract IMultiPoint CreateMultiPoint(IEnumerable<IPoint> points);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source points.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The multi point containing the specified points in the specified reference system.</returns>
        public abstract IMultiPoint CreateMultiPoint(IEnumerable<IPoint> points, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source points.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi point containing the specified points and metadata.</returns>
        public abstract IMultiPoint CreateMultiPoint(IEnumerable<IPoint> points, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source points.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi point containing the specified points and metadata in the specified reference system.</returns>
        public abstract IMultiPoint CreateMultiPoint(IEnumerable<IPoint> points, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <returns>A multi point that matches <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other multi point is null.</exception>
        public abstract IMultiPoint CreateMultiPoint(IMultiPoint other);

        #endregion

        #region Factory methods for multi line strings

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <returns>The empty multi line string.</returns>
        public abstract IMultiLineString CreateMultiLineString();
        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The empty multi line string in the specified reference system.</returns>
        public abstract IMultiLineString CreateMultiLineString(IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty multi line string with the specified metadata.</returns>
        public abstract IMultiLineString CreateMultiLineString(IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty multi line string with the specified metadata in the specified reference system.</returns>
        public abstract IMultiLineString CreateMultiLineString(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="line strings">The source line strings.</param>
        /// <returns>The multi line string containing the specified line strings.</returns>
        public abstract IMultiLineString CreateMultiLineString(IEnumerable<ILineString> lineStrings);
        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="line strings">The source line strings.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The multi line string containing the specified line strings in the specified reference system.</returns>
        public abstract IMultiLineString CreateMultiLineString(IEnumerable<ILineString> lineStrings, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="line strings">The source line strings.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi line string containing the specified line strings and metadata.</returns>
        public abstract IMultiLineString CreateMultiLineString(IEnumerable<ILineString> lineStrings, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="line strings">The source line strings.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi line string containing the specified line strings and metadata in the specified reference system.</returns>
        public abstract IMultiLineString CreateMultiLineString(IEnumerable<ILineString> lineStrings, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <returns>A multi line string that matches <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other multi line string is null.</exception>
        public abstract IMultiLineString CreateMultiLineString(IMultiLineString other);

        #endregion

        #region Factory methods for multi polygons

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <returns>The empty multi polygon.</returns>
        public abstract IMultiPolygon CreateMultiPolygon();
        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The empty multi polygon in the specified reference system.</returns>
        public abstract IMultiPolygon CreateMultiPolygon(IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty multi polygon with the specified metadata.</returns>
        public abstract IMultiPolygon CreateMultiPolygon(IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The empty multi polygon with the specified metadata in the specified reference system.</returns>
        public abstract IMultiPolygon CreateMultiPolygon(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="polygons">The source polygons.</param>
        /// <returns>The multi polygon containing the specified polygons.</returns>
        public abstract IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> polygons);
        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="polygons">The source polygons.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The multi polygon containing the specified polygons in the specified reference system.</returns>
        public abstract IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> polygons, IReferenceSystem referenceSystem);
        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="polygons">The source polygons.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi polygon containing the specified polygons and metadata.</returns>
        public abstract IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> polygons, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="polygons">The source polygons.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The multi polygon containing the specified polygons and metadata in the specified reference system.</returns>
        public abstract IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> polygons, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata);
        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <returns>A multi polygon that matches <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">other;The other multi polygon is null.</exception>
        public abstract IMultiPolygon CreateMultiPolygon(IMultiPolygon other);

        #endregion
    }
}
