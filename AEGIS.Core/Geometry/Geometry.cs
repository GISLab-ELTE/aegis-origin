/// <copyright file="Geometry.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a general geometry in spatial coordinate space.
    /// </summary>
    /// <remarks>
    /// Geometry is the root class of the hierarchy.
    /// </remarks>
    public abstract class Geometry : IGeometry
    {
        #region Private fields

        private readonly IGeometryFactory _factory;
        private Envelope _envelope;
        private IGeometry _boundary;
        private IMetadataCollection _metadata;

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the factory of the geometry.
        /// </summary>
        /// <value>The factory implementation the geometry was constructed by.</value>
        public IGeometryFactory Factory { get { return _factory; } }

        /// <summary>
        /// Gets the precision model of the geometry.
        /// </summary>
        /// <value>The precision model of the geometry.</value>
        public PrecisionModel PrecisionModel { get { return _factory.PrecisionModel; } }

        /// <summary>
        /// Gets the general name of the geometry.
        /// </summary>
        /// <value>The general name of the specific geometry.</value>
        public virtual String Name { get { return GetType().Name; } }

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>The inherent dimension of the geometry.</value>
        public abstract Int32 Dimension { get; }

        /// <summary>
        /// Gets the coordinate dimension of the geometry.
        /// </summary>
        /// <value>The coordinate dimension of the geometry. The coordinate dimension is equal to the dimension of the reference system, if provided.</value>
        public virtual Int32 CoordinateDimension { get { return (ReferenceSystem != null) ? ReferenceSystem.Dimension : SpatialDimension; } }

        /// <summary>
        /// Gets the spatial dimension of the geometry.
        /// </summary>
        /// <value>The spatial dimension of the geometry. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        public virtual Int32 SpatialDimension { get { return (Envelope.Minimum.Z != 0 || Envelope.Maximum.Z != 0) ? 3 : 2; } }

        /// <summary>
        /// Gets the model of the geometry.
        /// </summary>
        /// <value>The model of the geometry.</value>
        public GeometryModel GeometryModel { get { return (CoordinateDimension == 3) ? GeometryModel.Spatial3D : GeometryModel.Spatial2D; } }

        /// <summary>
        /// Gets the reference system of the geometry.
        /// </summary>
        /// <value>The reference system of the geometry.</value>
        public IReferenceSystem ReferenceSystem { get { return _factory.ReferenceSystem; } }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        public Envelope Envelope { get { return _envelope ?? (_envelope = ComputeEnvelope()); } }

        /// <summary>
        /// Gets the bounding <see cref="IGeometry" />.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public IGeometry Boundary { get { return _boundary ?? (_boundary = ComputeBoundary()); } }

        /// <summary>
        /// Gets the centroid of the geometry.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        public abstract Coordinate Centroid { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public abstract Boolean IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public abstract Boolean IsSimple { get; }

        /// <summary>
        /// Determines whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public abstract Boolean IsValid { get; }

        #endregion

        #region IMetadataProvider properties

        /// <summary>
        /// Gets the metadata collection.
        /// </summary>
        /// <value>The metadata collection.</value>
        public IMetadataCollection Metadata { get { return _metadata; } }

        /// <summary>
        /// Gets or sets the metadata value for a specified key.
        /// </summary>
        /// <param name="key">The key of the metadata.</param>
        /// <returns>The metadata value with the <paramref name="key" /> if it exists; otherwise, <c>null</c>.</returns>
        public Object this[String key]
        {
            get
            {
                Object value = null;
                if (_metadata != null)
                    _metadata.TryGetValue(key, out value);
                return value;
            }
            set
            {
                if (_metadata == null)
                    _metadata = _factory.CreateMetadata();
                _metadata[key] = value;
            }
        }

        #endregion

        #region IGeometry events

        /// <summary>
        /// Occurs when the geometry is changed.
        /// </summary>
        public event EventHandler GeometryChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        protected Geometry(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
        {
            _factory = referenceSystem == null ? AEGIS.Factory.DefaultInstance<GeometryFactory>() : AEGIS.Factory.GetInstance<GeometryFactory>(referenceSystem);
            _metadata = _factory.CreateMetadata(metadata);

            _envelope = null;
            _boundary = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry" /> class.
        /// </summary>
        /// <param name="factory">The factory of the geometry.</param>
        /// <param name="metadata">The metadata.</param>
        protected Geometry(IGeometryFactory factory, IDictionary<String, Object> metadata)
        {
            _factory = factory ?? AEGIS.Factory.DefaultInstance<GeometryFactory>();
            _metadata = _factory.CreateMetadata(metadata);

            _envelope = null;
            _boundary = null;
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the geometry instance.
        /// </summary>
        /// <returns>The deep copy of the geometry instance.</returns>
        public abstract Object Clone();

        #endregion

        #region Protected methods

        /// <summary>
        /// Called when the geometry is changed.
        /// </summary>
        protected virtual void OnGeometryChanged()
        {
            EventHandler geometryChanged = GeometryChanged;

            _envelope = null;
            _boundary = null;

            if (geometryChanged != null)
                geometryChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Computes the minimal bounding envelope of the geometry.
        /// </summary>
        /// <returns>The minimum bounding box of the geometry.</returns>
        protected abstract Envelope ComputeEnvelope();

        /// <summary>
        /// Computes the boundary of the geometry.
        /// </summary>
        /// <returns>The closure of the combinatorial boundary of the geometry.</returns>
        protected abstract IGeometry ComputeBoundary();

        #endregion
    }
}
