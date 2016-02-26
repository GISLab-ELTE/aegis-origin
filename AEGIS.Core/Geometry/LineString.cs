/// <copyright file="LineString.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a line string geometry in spatial coordinate space.
    /// </summary>
    public class LineString : Curve, ILineString
    {
        #region Private fields

        /// <summary>
        /// The version of the line string.
        /// </summary>
        protected Int32 _version;

        #endregion

        #region Protected fields

        /// <summary>
        /// The list of coordinates.
        /// </summary>
        protected readonly List<Coordinate> _coordinates;

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the centroid of the line string.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        public override sealed Coordinate Centroid { get { return PrecisionModel.MakePrecise(LineAlgorithms.Centroid(_coordinates)); } }

        /// <summary>
        /// Gets a value indicating whether the curve is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public override sealed Boolean IsEmpty { get { return _coordinates.Count == 0; } }

        /// <summary>
        /// Gets a value indicating whether the curve is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple
        {
            get
            {
                return !ShamosHoeyAlgorithm.Intersects(_coordinates, PrecisionModel);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the curve is valid.
        /// </summary>
        /// <value><c>true</c> if all coordinates of the curve are valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid
        {
            get { return _coordinates.All(t => t.IsValid); }
        }

        #endregion

        #region ICurve properties 
        
        /// <summary>
        /// Gets a value indicating whether the curve is closed.
        /// </summary>
        /// <value><c>true</c> if the starting and ending coordinates are equal; otherwise, <c>false</c>.</value>
        public override Boolean IsClosed { get { return _coordinates.Count > 0 && _coordinates[0].Equals(_coordinates[_coordinates.Count - 1]); } }

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

        /// <summary>
        /// Gets the staring point.
        /// </summary>
        /// <value>The first point of the curve if the curve has at least one point; otherwise, <c>null</c>.</value>
        /// <exception cref="System.InvalidOperationException">The curve is empty.</exception>
        public override IPoint StartPoint
        {
            get
            {
                if (_coordinates.Count == 0)
                    return null;
                return Factory.CreatePoint(_coordinates[0]);
            }
        }

        /// <summary>
        /// Gets the ending point.
        /// </summary>
        /// <value>The last point of the curve if the curve has at least one point; otherwise, <c>null</c>.</value>
        /// <exception cref="System.InvalidOperationException">The curve is empty.</exception>
        public override IPoint EndPoint
        {
            get
            {
                if (_coordinates.Count == 0)
                    return null;
                return Factory.CreatePoint(_coordinates[_coordinates.Count - 1]);
            }
        }

        #endregion

        #region IBasicLineString properties

        /// <summary>
        /// Gets the number of coordinates in the line string.
        /// </summary>
        /// <value>The number of coordinates in the line string.</value>
        public Int32 Count { get { return _coordinates.Count; } }

        /// <summary>
        /// Gets the coordinates in the line string.
        /// </summary>
        /// <value>The read-only list of coordinates of the line string.</value>
        public IList<Coordinate> Coordinates { get { return _coordinates.AsReadOnly(); } }

        /// <summary>
        /// Gets the staring coordinate.
        /// </summary>
        /// <value>The first coordinate of the line string.</value>
        public Coordinate StartCoordinate
        {
            get
            {
                if (_coordinates.Count == 0)
                    return Coordinate.Undefined;
                return _coordinates[0];
            }
        }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The last coordinate of the curve.</value>
        public Coordinate EndCoordinate
        {
            get
            {
                if (_coordinates.Count == 0)
                    return Coordinate.Undefined;
                return _coordinates[_coordinates.Count - 1];
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="metadata">The metadata.</param>
        public LineString(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : this(null, precisionModel, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public LineString(IEnumerable<Coordinate> source, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(precisionModel, referenceSystem, metadata)
        {
            if (source == null)
                _coordinates = new List<Coordinate>();
            else
                _coordinates = new List<Coordinate>(source.Select(coordinate => PrecisionModel.MakePrecise(coordinate)));

            _version = 0;            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="factory">The factory of the line string.</param>
        /// <param name="metadata">The metadata.</param>
        public LineString(IGeometryFactory factory, IDictionary<String, Object> metadata)
            : this(null, factory, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="factory">The factory of the line string.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public LineString(IEnumerable<Coordinate> source, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
            if (source == null)
                _coordinates = new List<Coordinate>();
            else
                _coordinates = new List<Coordinate>(source.Select(coordinate => PrecisionModel.MakePrecise(coordinate)));

            _version = 0;
        }

        #endregion

        #region IBasicLineString methods

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
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to get.</param>
        /// <returns>The coordinate located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual Coordinate GetCoordinate(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _coordinates.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of coordinates.");

            return _coordinates[index];
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
        public virtual void SetCoordinate(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _coordinates.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of coordinates.");

            if (_coordinates[index].Equals(coordinate))
                return;

            _coordinates[index] = PrecisionModel.MakePrecise(coordinate);
            OnGeometryChanged();
        }

        /// <summary>
        /// Adds a coordinate to the end of the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        public virtual void Add(Coordinate coordinate)
        {
            _coordinates.Add(PrecisionModel.MakePrecise(coordinate));

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
        /// <exception cref="System.ArgumentException">The coordinate is not valid.</exception>
        public virtual void Insert(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index > _coordinates.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of coordinates.");

            _coordinates.Insert(index, PrecisionModel.MakePrecise(coordinate));

            OnGeometryChanged();
        }

        /// <summary>
        /// Removes the first occurrence of the specified coordinate from the line string.
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
            return new LineString(_coordinates, Factory, Metadata);
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator{Coordinate}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            foreach (Coordinate coordinate in _coordinates)
                yield return coordinate;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
            return Envelope.FromCoordinates(_coordinates);
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
                return Factory.CreateMultiPoint(
                    new IPoint[]
                    {
                        Factory.CreatePoint(_coordinates[0].X, _coordinates[0].Y, _coordinates[0].Z),
                        Factory.CreatePoint(_coordinates[_coordinates.Count - 1].X, _coordinates[_coordinates.Count - 1].Y, _coordinates[_coordinates.Count - 1].Z),
                    });
            }
        }

        /// <summary>
        /// Called when the geometry is changed.
        /// </summary>
        protected override void OnGeometryChanged()
        {
            _version++;
            base.OnGeometryChanged();
        }

        #endregion
    }
}
