/// <copyright file="SpectralPolygon.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a polygon contaning spectral data.
    /// </summary>
    public class SpectralPolygon : ISpectralPolygon
    {
        #region Private fields

        /// <summary>
        /// The spectral geometry factory. This field is read-only.
        /// </summary>
        private readonly ISpectralGeometryFactory _factory;

        /// <summary>
        /// The polygon. This field is read-only.
        /// </summary>
        private readonly IPolygon _polygon;

        /// <summary>
        /// The raster. This field is read-only.
        /// </summary>
        private readonly IRaster _raster;

        /// <summary>
        /// The imaging scene data. This field is read-only.
        /// </summary>
        private readonly ImagingScene _scene;

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the factory of the geometry.
        /// </summary>
        /// <value>The factory implementation the geometry was constructed by.</value>
        public IGeometryFactory Factory { get { return _polygon.Factory; } }

        /// <summary>
        /// Gets the general name of the geometry.
        /// </summary>
        /// <value>The general name of the specific geometry.</value>
        public String Name { get { return _polygon.Name; } }

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>The inherent dimension of the geometry.</value>
        public Int32 Dimension { get { return _polygon.Dimension; } }

        /// <summary>
        /// Gets the coordinate dimension of the geometry.
        /// </summary>
        /// <value>The coordinate dimension of the geometry. The coordinate dimension is equal to the dimension of the reference system, if provided.</value>
        public Int32 CoordinateDimension { get { return _polygon.CoordinateDimension; } }

        /// <summary>
        /// Gets the spatial dimension of the geometry.
        /// </summary>
        /// <value>The spatial dimension of the geometry. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        public Int32 SpatialDimension { get { return _polygon.SpatialDimension; } }

        /// <summary>
        /// Gets the model of the geometry.
        /// </summary>
        /// <value>The model of the geometry.</value>
        public GeometryModel GeometryModel { get { return _polygon.GeometryModel; } }

        /// <summary>
        /// Gets the reference system of the geometry.
        /// </summary>
        /// <value>The reference system of the geometry.</value>
        public IReferenceSystem ReferenceSystem { get { return _polygon.ReferenceSystem; } }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        public Envelope Envelope { get { return _polygon.Envelope; } }

        /// <summary>
        /// Gets the bounding <see cref="IGeometry" />.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public IGeometry Boundary { get { return _polygon.Boundary; } }

        /// <summary>
        /// Gets the centroid of the geometry.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        public Coordinate Centroid { get { return _polygon.Centroid; } }

        /// <summary>
        /// Determines whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return _polygon.IsEmpty; } }

        /// <summary>
        /// Determines whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public Boolean IsSimple { get { return _polygon.IsSimple; } }

        /// <summary>
        /// Determines whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return _polygon.IsValid; } }

        #endregion

        #region IMetadataProvider properties

        /// <summary>
        /// Gets the metadata collection.
        /// </summary>
        /// <value>The metadata collection.</value>
        public IMetadataCollection Metadata { get { return _polygon.Metadata; } }

        /// <summary>
        /// Gets or sets the metadata value for a specified key.
        /// </summary>
        /// <param name="key">The key of the metadata.</param>
        /// <returns>The metadata value with the <paramref name="key" /> if it exists; otherwise, <c>null</c>.</returns>
        public Object this[String key]
        {
            get
            {
                return _polygon[key];
            }
            set
            {
                _polygon[key] = value;
            }
        }

        #endregion

        #region ISurface properties

        /// <summary>
        /// Gets a value indicating whether the polygon is convex.
        /// </summary>
        /// <value><c>true</c> if the polygon is convex; otherwise, <c>false</c>.</value>
        public Boolean IsConvex { get { return _polygon.IsConvex; } }

        /// <summary>
        /// Gets a value indicating whether the polygon is divided.
        /// </summary>
        /// <value><c>true</c>, as a polygon is never divided.</value>
        public Boolean IsDivided { get { return _polygon.IsDivided; } }

        /// <summary>
        /// Gets a value indicating whether the polygon is whole.
        /// </summary>
        /// <value><c>true</c> if the polygon contains no holes; otherwise, <c>false</c>.</value>
        public Boolean IsWhole { get { return _polygon.IsWhole; } }

        /// <summary>
        /// Gets the area of the polygon.
        /// </summary>
        /// <value>The area of the surface.</value>
        public Double Area { get { return _polygon.Area; } }

        /// <summary>
        /// Gets the perimeter of the polygon.
        /// </summary>
        /// <value>The perimeter of the surface.</value>
        public Double Perimeter { get { return _polygon.Perimeter; } }

        #endregion

        #region IPolygon properties

        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The <see cref="ILinearRing" /> representing the shell of the polygon.</value>
        public ILinearRing Shell { get { return _polygon.Shell; } }

        /// <summary>
        /// Gets the number of holes of the polygon.
        /// </summary>
        /// <value>The number of holes in the <see cref="ILinearRing" />.</value>
        public Int32 HoleCount { get { return _polygon.HoleCount; } }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The <see cref="IList{ILinearRing}" /> containing the holes of the polygon.</value>
        public IList<ILinearRing> Holes { get { return _polygon.Holes; } }

        #endregion

        #region ISpectralGeometry properties

        /// <summary>
        /// Gets the raster associated with the geometry.
        /// </summary>
        /// <value>
        /// The raster associated with the geometry.
        /// </value>
        public IRaster Raster { get { return _raster; } }

        /// <summary>
        /// Gets the imaging scene data.
        /// </summary>
        /// <value>The imaging scene information of the spectral data.</value>
        public ImagingScene ImagingScene { get { return _scene; } }

        #endregion

        #region IGeometry events

        /// <summary>
        /// Occurs when the geometry is changed.
        /// </summary>
        public event EventHandler GeometryChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralPolygon" /> class.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="raster">The raster.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The raster is null.
        /// or
        /// The shell is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public SpectralPolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IReferenceSystem referenceSystem, IRaster raster, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");

            _factory = referenceSystem == null ? AEGIS.Factory.DefaultInstance<SpectralGeometryFactory>() : AEGIS.Factory.GetInstance<SpectralGeometryFactory>(referenceSystem);

            _raster = raster;
            _scene = scene;
            _polygon = _factory.GetFactory<IGeometryFactory>().CreatePolygon(shell, holes, metadata);

            _polygon.GeometryChanged += new EventHandler(Polygon_GeometryChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralPolygon" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <param name="raster">The raster.</param>
        /// <param name="scene">The imaging scene data.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The raster is null.
        /// or
        /// The shell is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public SpectralPolygon(ISpectralGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IRaster raster, ImagingScene scene, IDictionary<String, Object> metadata)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");

            _factory = factory ?? AEGIS.Factory.DefaultInstance<SpectralGeometryFactory>();

            _raster = raster;
            _scene = scene;
            _polygon = _factory.GetFactory<IGeometryFactory>().CreatePolygon(shell, holes, metadata);

            _polygon.GeometryChanged += new EventHandler(Polygon_GeometryChanged);
        }

        #endregion

        #region IPolygon methods

        /// <summary>
        /// Add a hole to the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.ArgumentNullException">hole;The hole is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the hole does not match the reference system of the polygon.;hole</exception>
        public void AddHole(ILinearRing hole)
        {
            _polygon.AddHole(hole);
        }

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">There are no holes in the polygon.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public ILinearRing GetHole(Int32 index)
        {
            return _polygon.GetHole(index);
        }

        /// <summary>
        /// Removes a hole from the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the polygon contains the <paramref name="hole" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.InvalidOperationException">There are no holes in the polygon.</exception>
        public virtual Boolean RemoveHole(ILinearRing hole)
        {
            return _polygon.RemoveHole(hole);
        }

        /// <summary>
        /// Removes the hole at the specified index of the polygon.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to remove.</param>
        public void RemoveHoleAt(Int32 index)
        {
            _polygon.RemoveHoleAt(index);
        }

        /// <summary>
        /// Removes all holes from the polygon.
        /// </summary>
        public void ClearHoles()
        {
            _polygon.ClearHoles();
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the spectral polygon instance.
        /// </summary>
        /// <returns>The deep copy of the spectral polygon instance.</returns>
        public Object Clone()
        {
            return new SpectralPolygon(_factory, _polygon.Shell, _polygon.Holes, _raster.Clone() as IRaster, _scene, _polygon.Metadata);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates of the shell, the holes and the properties of the raster.</returns>
        public override String ToString()
        {
            return _polygon.ToString() + " " + _raster.ToString();
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Called when the geometry is changed.
        /// </summary>
        protected virtual void OnGeometryChanged()
        {
            EventHandler geometryChanged = GeometryChanged;

            if (geometryChanged != null)
                geometryChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Handles the GeometryChanged event of the polygon.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Polygon_GeometryChanged(Object sender, EventArgs args)
        {
            OnGeometryChanged();
        }

        #endregion
    }
}