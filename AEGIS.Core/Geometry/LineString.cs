/// <copyright file="LineString.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Algorithms;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a line string geometry in spatial coordinate space.
    /// </summary>
    public class LineString : Curve, ILineString
    {
        #region IGeometry properties

        /// <summary>
        /// Gets the centroid of the line string.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        public override sealed Coordinate Centroid { get { return LineAlgorithms.Centroid(_coordinates); } }

        #endregion

        #region ICurve properties 

        /// <summary>
        /// Gets the length of the line string.
        /// </summary>
        /// <value>The length of the line string.</value>
        public override Double Length
        {
            get 
            {
                Double length = 0;
                for (Int32 i = 0; i < _coordinates.Count - 1; i++)
                    length += Coordinate.Distance(_coordinates[i], _coordinates[i + 1]);
                return length;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public LineString(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(null, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public LineString(IEnumerable<Coordinate> source, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(source, referenceSystem, metadata)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="factory">The factory of the line string.</param>
        /// <param name="metadata">The metadata.</param>
        public LineString(IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(null, factory, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="factory">The factory of the line string.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public LineString(IEnumerable<Coordinate> source, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(source, factory, metadata)
        {

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the line string contains the specified coordinate within its coordinates.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line string contains the specified coordinate within its coordinates; otherwise, <c>false</c>.</returns>
        public virtual Boolean Contains(Coordinate coordinate)
        {
            return _coordinates.Contains(coordinate);
        }

        /// <summary>
        /// Adds a coordinate to the end of the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        public virtual void Add(Coordinate coordinate)
        {
            _coordinates.Add(coordinate);

            OnGeometryChanged();
        }

        /// <summary>
        /// Inserts a coordinate into the line string at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the coordinate should be inserted.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        /// <exception cref="System.ArgumentException">The coordinate is not valid.;coordinate</exception>
        public virtual void Insert(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index > _coordinates.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of coordinates.");

            _coordinates.Insert(index, coordinate);

            OnGeometryChanged();
        }
        /// <summary>
        /// Removes the first occurence of the specified coordinate from the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate was removed; otherwise, <c>false</c>.</returns>
        public virtual Boolean Remove(Coordinate coordinate)
        {
            return _coordinates.Remove(coordinate);
        }

        /// <summary>
        /// Removes the coordinate at the specified index from the line string.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual void RemoveAt(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _coordinates.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of coordinates.");

            _coordinates.RemoveAt(index);

            OnGeometryChanged();
        }

        /// <summary>
        /// Removes all coordinates from the line string.
        /// </summary>
        public virtual void Clear()
        {
            _coordinates.Clear();
            OnGeometryChanged();
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the current geometry instance.
        /// </summary>
        /// <returns>The deep copy of the current instance.</returns>
        public override Object Clone()
        {
            return new LineString(_coordinates, _factory, Metadata);
        }

        #endregion 

        #region Object methods

        /// <summary>
        /// Returns the <see cref="String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            if (IsEmpty)
                return Name + " EMPTY";
            else
                return Name + " (" + _coordinates.Select(coordinate => coordinate.X + " " + coordinate.Y + " " + coordinate.Z).Aggregate((x, y) => x + ", " + y) + ")";
        }

        #endregion

        #region Protected Geometry methods

        /// <summary>
        /// Computes the minimal bounding envelope of the geometry.
        /// </summary>
        /// <returns>The minimum bounding box of the geometry.</returns>
        protected override Envelope ComputeEnvelope()
        {
            return AEGIS.Envelope.FromCoordinates(_coordinates);
        }

        /// <summary>
        /// Computes the boundary of the geometry.
        /// </summary>
        /// <returns>The closure of the combinatorial boundary of the geometry.</returns>
        protected override IGeometry ComputeBoundary()
        {
            if (IsClosed)
                return null;
            else
            {
                return _factory.CreateMultiPoint(
                    new IPoint[]
                    {
                        _factory.CreatePoint(_coordinates[0].X, _coordinates[0].Y, _coordinates[0].Z),
                        _factory.CreatePoint(_coordinates[_coordinates.Count - 1].X, _coordinates[_coordinates.Count - 1].Y, _coordinates[_coordinates.Count - 1].Z),
                    });
            }
        }

        #endregion
    }
}
