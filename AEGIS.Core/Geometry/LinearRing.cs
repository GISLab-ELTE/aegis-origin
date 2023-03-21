// <copyright file="LinearRing.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a linear ring geometry in spatial coordinate space.
    /// </summary>
    public class LinearRing : LineString, ILinearRing
    {
        #region IGeometry properties

        /// <summary>
        /// Determines whether the linear ring is valid.
        /// </summary>
        /// <value>
        /// <c>true</c> if the linear ring is simple and all coordinates of the linear ring are valid; otherwise, <c>false</c>.
        /// </value>
        public override Boolean IsValid { get { return base.IsValid && IsSimple; } }

        #endregion

        #region ICurve properties

        /// <summary>
        /// Gets a value indicating whether the linear ring is closed.
        /// </summary>
        /// <value><c>true</c>, as linear ring is always considered to be closed.</value>
        public override Boolean IsClosed { get { return true; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRing" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The source is empty.</exception>
        public LinearRing(IEnumerable<Coordinate> source, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(source, precisionModel, referenceSystem, metadata)
        {
            if (_coordinates.Count < 1)
                throw new ArgumentException("The source is empty.", "coordinates");

            if (!_coordinates[0].Equals(_coordinates[_coordinates.Count - 1]))
                _coordinates.Add(_coordinates[0]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRing" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="factory">The factory of the linear ring.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source is empty.
        /// or
        /// The specified factory is invalid.
        /// </exception>
        public LinearRing(IEnumerable<Coordinate> source, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(source, factory, metadata)
        {
            if (_coordinates.Count < 1)
                throw new ArgumentException("The source is empty.", "coordinates");

            if (!_coordinates[0].Equals(_coordinates[_coordinates.Count - 1]))
                _coordinates.Add(_coordinates[0]);
        }

        #endregion

        #region ILineString methods

        /// <summary>
        /// Sets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to set.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public override void SetCoordinate(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _coordinates.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of coordinates.");
            if (_coordinates[index].Equals(coordinate))
                return;

            if (index == 0 || index == _coordinates.Count - 1)
            {
                _coordinates[0] = PrecisionModel.MakePrecise(coordinate);
                _coordinates[_coordinates.Count - 1] = _coordinates[0];
            }
            else
            {
                _coordinates[index] = PrecisionModel.MakePrecise(coordinate);
            }

            OnGeometryChanged();
        }

        /// <summary>
        /// Adds a coordinate to the end of the linear ring.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        public override void Add(Coordinate coordinate)
        {
            base.Insert(_coordinates.Count - 1, coordinate);
        }

        /// <summary>
        /// Inserts a coordinate into the linear ring at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the coordinate should be inserted.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is greater than the number of coordinates.
        /// </exception>
        public override void Insert(Int32 index, Coordinate coordinate)
        {
            if (index == 0) // insertion of new starting coordinate
            {
                _coordinates[_coordinates.Count - 1] = PrecisionModel.MakePrecise(coordinate);
                base.Insert(0, coordinate);
            }
            else // insertion of new inner coordinate
            {
                base.Insert(index, PrecisionModel.MakePrecise(coordinate));
            }
        }

        /// <summary>
        /// Removes the first occurrence of the specified coordinate from the linear ring.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate was removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.InvalidOperationException">A linear ring must contain at least two coordinates.</exception>
        public override Boolean Remove(Coordinate coordinate)
        {
            if (_coordinates.Count == 2)
                throw new InvalidOperationException("A linear ring must contain at least two coordinates.");

            if (_coordinates[0].Equals(coordinate))
            {
                _coordinates[_coordinates.Count - 1] = _coordinates[1];
                base.RemoveAt(0);
                return true;
            }
            else
            {
                return base.Remove(coordinate);
            }
        }

        /// <summary>
        /// Removes the coordinate at the specified index from the linear ring.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to remove.</param>
        /// <exception cref="System.InvalidOperationException">A linear ring must contain at least two points.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public override void RemoveAt(Int32 index)
        {
            if (_coordinates.Count == 2)
                throw new InvalidOperationException("A linear ring must contain at least two coordinates.");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _coordinates.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of coordinates.");

            if (index == 0 || index == _coordinates.Count - 1)
            {
                _coordinates[_coordinates.Count - 1] = _coordinates[1];
                base.RemoveAt(0);
            }
            else
            {
                base.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes all inner coordinates from the linear ring.
        /// </summary>
        public override void Clear()
        {
            Coordinate coordinate = _coordinates[0];
            _coordinates.Clear();
            _coordinates[0] = _coordinates[1] = coordinate;

            OnGeometryChanged();
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the linear ring instance.
        /// </summary>
        /// <returns>The deep copy of the linear ring instance.</returns>
        public override Object Clone()
        {
            return new LinearRing(_coordinates, Factory, Metadata);
        }

        #endregion

        #region Protected Geometry methods

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
