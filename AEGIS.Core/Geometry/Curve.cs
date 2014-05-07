/// <copyright file="Curve.cs" company="Eötvös Loránd University (ELTE)">
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

using System.Linq;
using ELTE.AEGIS.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a curve geometry in spatial coordinate space.
    /// </summary>
    public abstract class Curve : Geometry, ICurve
    {
        #region Public types

        /// <summary>
        /// Enumerates the elements of a curve.
        /// </summary>
        public struct Enumerator : IEnumerator<Coordinate>, IEnumerator, IDisposable
        {
            #region Private fields

            private Curve _localCurve;
            private Int32 _localVersion;
            private Int32 _index;
            private Coordinate _current;

            #endregion

            #region IEnumerable properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            public Coordinate Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.-
            /// </value>
            Object IEnumerator.Current
            {
                get { return _current; }
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator" /> class.
            /// </summary>
            /// <param name="curve">The curve.</param>
            internal Enumerator(Curve curve)
            {
                _localCurve = curve;
                _localVersion = curve._version;

                _index = 0;
                _current = default(Coordinate);
            }

            #endregion

            #region IEnumerable methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (_localVersion != _localCurve._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_index >= _localCurve.Count)
                {
                    _index = _localCurve.Count + 1;
                    _current = default(Coordinate);
                    return false;
                }
                else
                {
                    _current = _localCurve._coordinates[_index];
                    _index++;
                    return true;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_localVersion != _localCurve._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _index = 0;
                _current = default(Coordinate);
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose() { }

            #endregion
        }

        #endregion
        
        #region Protected fields

        protected readonly List<Coordinate> _coordinates;
        protected Int32 _version;

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the inherent dimension of the curve.
        /// </summary>
        /// <value><c>1</c>, which is the defined dimension of a curve.</value>
        public override sealed Int32 Dimension { get { return 1; } }

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
                return !ShamosHoeyAlgorithm.Intersects(_coordinates);
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
        public virtual Boolean IsClosed { get { return _coordinates[0].Equals(_coordinates[_coordinates.Count - 1]); } }

        /// <summary>
        /// Gets a value indicating whether the curve is a ring.
        /// </summary>
        /// <value><c>true</c> if the curve is simple and closed; otherwise, <c>false</c>.</value>
        public Boolean IsRing { get { return IsClosed && IsSimple; } }

        /// <summary>
        /// Gets the number of coordinates in the curve.
        /// </summary>
        /// <value>The number of coordinates in the curve.</value>
        public Int32 Count { get { return _coordinates.Count; } }

        /// <summary>
        /// Gets the length of the curve.
        /// </summary>
        /// <value>The length of the curve.</value>
        public abstract Double Length { get; }

        /// <summary>
        /// Gets the list of coordinates in the curve.
        /// </summary>
        /// <value>The coordinates of the curve.</value>
        public IList<Coordinate> Coordinates { get { return _coordinates.AsReadOnly(); } }

        /// <summary>
        /// Gets the staring coordinate.
        /// </summary>
        /// <value>The first coordinate of the curve.</value>
        /// <exception cref="System.InvalidOperationException">The curve is empty.</exception>
        public Coordinate StartCoordinate 
        { 
            get 
            {
                if (_coordinates == null)
                    throw new InvalidOperationException("The curve is empty.");
                return _coordinates[0]; 
            } 
        }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The last coordinate of the curve.</value>
        /// <exception cref="System.InvalidOperationException">The curve is empty.</exception>
        public Coordinate EndCoordinate 
        { 
            get 
            {
                if (_coordinates == null)
                    throw new InvalidOperationException("The curve is empty.");
                return _coordinates[_coordinates.Count - 1]; 
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        protected Curve(IEnumerable<Coordinate> source, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(referenceSystem, metadata)
        {
            if (source == null)
                _coordinates = new List<Coordinate>();
            else
                _coordinates = new List<Coordinate>(source);

            _version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="factory">The factory of the curve.</param>
        /// <param name="metadata">The metadata.</param>
        protected Curve(IEnumerable<Coordinate> source, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
            if (source == null)
                _coordinates = new List<Coordinate>();
            else
                _coordinates = new List<Coordinate>(source);

            _version = 0;
        }

        #endregion

        #region ICurve methods

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

            _coordinates[index] = coordinate;
            OnGeometryChanged();
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the curve.
        /// </summary>
        /// <returns>An enumerator for the curve.</returns>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the curve.
        /// </summary>
        /// <returns>An enumerator for the curve.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Protected Geometry methods

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
