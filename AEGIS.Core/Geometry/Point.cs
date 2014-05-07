/// <copyright file="Point.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a point geometry in spatial coordinate space.
    /// </summary>
    public class Point : Geometry, IPoint
    {
        #region Protected fields

        protected Coordinate _coordinate;

        #endregion

        #region IPoint properties

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        public virtual Double X 
        { 
            get { return _coordinate.X; }
            set 
            {
                if (_coordinate.X != value)
                {
                    _coordinate = new Coordinate(value, _coordinate.Y, _coordinate.Z);
                    OnGeometryChanged();
                }
            } 
        }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>The Y coordinate.</value>
        public virtual Double Y 
        { 
            get { return _coordinate.Y; }
            set 
            {
                if (_coordinate.Y != value)
                {
                    _coordinate = new Coordinate(_coordinate.X, value, _coordinate.Z);
                    OnGeometryChanged();
                }
            } 
        }

        /// <summary>
        /// Gets or sets the Z coordinate.
        /// </summary>
        /// <value>The Z coordinate.</value>
        public virtual Double Z 
        { 
            get { return _coordinate.Z; }
            set 
            {
                if (_coordinate.Z != value)
                {
                    _coordinate = new Coordinate(_coordinate.X, _coordinate.Y, value);
                    OnGeometryChanged();
                }
            } 
        }

        /// <summary>
        /// Gets or sets the coordinate associated with the point.
        /// </summary>
        /// <value>The coordinate associated with the point.</value>
        public Coordinate Coordinate 
        { 
            get { return _coordinate; } 
            set 
            {
                if (!_coordinate.Equals(value))
                {
                    _coordinate = value;
                    OnGeometryChanged();
                }
            } 
        }

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the inherent dimension of the point.
        /// </summary>
        /// <value><c>0</c>, which is the defined dimension of a point.</value>
        public override sealed Int32 Dimension { get { return 0; } }

        /// <summary>
        /// Gets the spatial dimension of the point.
        /// </summary>
        /// <value>The spatial dimension of the point. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        public override Int32 SpatialDimension { get { return _coordinate.Z != 0 ? 3 : 2; } }

        /// <summary>
        /// Gets the centroid of the point.
        /// </summary>
        /// <value>The centroid of the point.</value>
        public override sealed Coordinate Centroid { get { return _coordinate; } }

        /// <summary>
        /// Gets a value indicating whether the point is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty { get { return _coordinate.IsEmpty; } }

        /// <summary>
        /// Gets a value indicating whether the point is simple.
        /// </summary>
        /// <value><c>true</c>, as a point is always considered to be simple.</value>
        public override sealed Boolean IsSimple { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether the point is valid.
        /// </summary>
        /// <value><c>true</c> if the <see cref="Coordinate" /> associated with the point is valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid { get { return true; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Point" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public Point(Double x, Double y, Double z, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(referenceSystem, metadata)
        {
            _coordinate = new Coordinate(x, y, z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="factory">The factory of the point.</param>
        /// <param name="metadata">The metadata.</param>
        public Point(Double x, Double y, Double z, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
            _coordinate = new Coordinate(x, y, z);
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="Point" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="Point" /> instance.</returns>
        public override Object Clone()
        {
            return new Point(_coordinate.X, _coordinate.Y, _coordinate.Z, _factory, Metadata);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return Name + " " + _coordinate.ToString();
        }

        #endregion

        #region Protected Geometry methods

        /// <summary>
        /// Computes the minimal bounding envelope of the geometry.
        /// </summary>
        /// <returns>The minimum bounding box of the geometry.</returns>
        protected override Envelope ComputeEnvelope()
        {
            return new Envelope(_coordinate.X, _coordinate.X, _coordinate.Y, _coordinate.Y, _coordinate.Z, _coordinate.Z);
        }

        /// <summary>
        /// Computes the boundary of the geometry.
        /// </summary>
        /// <returns>The closure of the combinatorial boundary of the geometry.</returns>
        protected override IGeometry ComputeBoundary()
        {
            return null;
        }

        #endregion
    }
}
