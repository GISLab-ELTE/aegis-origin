/// <copyright file="GeometryList.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a generic list of geometries in spatial coordinate space.
    /// </summary>
    /// <typeparam name="T">The type of geometries in the list.</typeparam>
    public class GeometryList<T> : Geometry, IGeometryCollection<T>, IList<T> where T : IGeometry
    {
        #region Public types

        /// <summary>
        /// Enumerates the elements of a geometry list.
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            #region Private fields

            /// <summary>
            /// The local reference to the geometry list.
            /// </summary>
            private readonly GeometryList<T> _localList;

            /// <summary>
            /// The version of the list during enumerator initialization.
            /// </summary>
            private readonly Int32 _localVersion;

            /// <summary>
            /// The current index of the enumerator.
            /// </summary>
            private Int32 _index;

            /// <summary>
            /// The current element.
            /// </summary>
            private T _current;

            #endregion

            #region IEnumerable properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            public T Current
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
            /// Initializes a new instance of the <see cref="GeometryList{T}.Enumerator" /> class.
            /// </summary>
            /// <param name="list">The list.</param>
            internal Enumerator(GeometryList<T> list)
            {
                _localList = list;
                _localVersion = list._version;

                _index = 0;
                _current = default(T);
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
                if (_localVersion != _localList._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_index >= _localList._size)
                {
                    _index = _localList._size + 1;
                    _current = default(T);
                    return false;
                }
                else
                {
                    _current = _localList._geometries[_index];
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
                if (_localVersion != _localList._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _index = 0;
                _current = default(T);
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

        #region Private fields

        /// <summary>
        /// The default capacity of the list.
        /// </summary>
        private const Int32 _defaultCapacity = 4;

        /// <summary>
        /// The size of the list.
        /// </summary>
        private Int32 _size;

        /// <summary>
        /// The version of the list.
        /// </summary>
        private Int32 _version;

        #endregion

        #region Protected fields

        /// <summary>
        /// The array of geometries.
        /// </summary>
        protected T[] _geometries;

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the general name of the geometry list.
        /// </summary>
        /// <value>The general name of the specific geometry.</value>
        public override String Name { get { return "GeometryCollection"; } }

        /// <summary>
        /// Gets the inherent dimension of the geometry list.
        /// </summary>
        /// <value>The maximum inherent dimension of all geometries within the collection.</value>
        public override Int32 Dimension { get { return (_size == 0) ? 0 : _geometries.Max(geometry => geometry.Dimension); } }

        /// <summary>
        /// Gets the centroid of the geometry list.
        /// </summary>
        /// <value>The centroid of the geometry list.</value>
        public override Coordinate Centroid
        {
            get 
            {
                if (_size == 0)
                    return Coordinate.Undefined;
                if (_size == 1)
                    return _geometries[0].Centroid;

                return PrecisionModel.MakePrecise(Coordinate.Centroid(_geometries.Select(x => x.Centroid))); 
            }
        }

        /// <summary>
        /// Determines whether the geometry list is empty.
        /// </summary>
        /// <value><c>true</c> if all geometries in the geometry list are empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty
        {
            get { return _size == 0 || _geometries.All(x => x.IsEmpty); }
        }

        /// <summary>
        /// Determines whether the geometry list is simple.
        /// </summary>
        /// <value><c>true</c> if all geometries in the geometry list are simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple
        {
            get { return _size == 0 || _geometries.All(geometry => geometry.IsSimple); }
        }

        /// <summary>
        /// Determines whether the geometry list is valid.
        /// </summary>
        /// <value><c>true</c> if all geometries in the geometry list are valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid
        {
            get { return _size == 0 || _geometries.All(geometry => geometry.IsValid); }
        }

        #endregion

        #region ICollection properties

        /// <summary>
        /// Gets the number of geometries contained in the geometry list.
        /// </summary>
        /// <value>
        /// The number of geometries contained in the geometry list.
        /// </value>
        public virtual Int32 Count { get { return _size; } }

        /// <summary>
        /// Gets a value indicating whether the geometry list is read-only.
        /// </summary>
        /// <value><c>true</c> if the geometry list is read-only; otherwise, <c>false</c>.</value>
        public virtual Boolean IsReadOnly { get { return false; } }

        #endregion

        #region IList properties

        /// <summary>
        /// Gets or sets the geometry at the specified index.
        /// </summary>
        /// <value>The geometry located at the specified <paramref name="index" />.</value>
        /// <param name="index">The zero-based index of the geometry to set.</param>
        /// <returns>The geometry located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">The value is null.</exception>
        public virtual T this[Int32 index]
        {
            get { return _geometries[index]; }
            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
                if (index >= _size)
                    throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of geometries.");
                if (value == null)
                    throw new InvalidOperationException("The value is null.");

                if (_geometries[index].Equals(value))
                    return;

                _geometries[index] = value;
                OnGeometryChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{T}" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public GeometryList(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(precisionModel, referenceSystem, metadata)
        {
            _geometries = new T[_defaultCapacity];
            _size = 0;
            _version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{T}" /> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the list can initially store.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public GeometryList(Int32 capacity, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(precisionModel, referenceSystem, metadata)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "The capacity is less than 0.");

            _geometries = new T[capacity];
            _size = 0;
            _version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{T}" /> class.
        /// </summary>
        /// <param name="source">The source of geometries.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public GeometryList(IEnumerable<T> source, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(precisionModel, referenceSystem, metadata)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Source is null.");

            // copy geometries
            _geometries = new T[source.Count()];
            _size = 0;
            using (IEnumerator<T> en = source.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    if (en.Current == null)
                        continue;

                    // check precision model and reference system
                    if (en.Current.ReferenceSystem != null && !en.Current.ReferenceSystem.Equals(ReferenceSystem) &&
                        en.Current.PrecisionModel.Equals(PrecisionModel))
                    {
                        _geometries[_size] = en.Current;
                    }
                    else
                    {
                        _geometries[_size] = (T) Factory.CreateGeometry(en.Current);
                    }
                    _size++;

                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{T}" /> class.
        /// </summary>
        /// <param name="factory">The factory of the list.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public GeometryList(IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
            _geometries = new T[_defaultCapacity];
            _size = 0;
            _version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{T}" /> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the list can initially store.</param>
        /// <param name="factory">The factory of the list.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public GeometryList(Int32 capacity, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "The capacity is less than 0.");

            _geometries = new T[capacity];
            _size = 0;
            _version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{T}" /> class.
        /// </summary>
        /// <param name="source">The source of geometries.</param>
        /// <param name="factory">The factory of the list.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The source is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public GeometryList(IEnumerable<T> source, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            // copy geometries
            _geometries = new T[source.Count()];
            _size = 0;
            using (IEnumerator<T> en = source.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    if (en.Current == null)
                        continue;

                    // check precision model and reference system
                    if (((en.Current.ReferenceSystem == null && ReferenceSystem == null) || en.Current.ReferenceSystem.Equals(ReferenceSystem))
                        && en.Current.PrecisionModel.Equals(PrecisionModel))
                    {
                        _geometries[_size] = en.Current;
                    }
                    else
                    {
                        _geometries[_size] = (T) Factory.CreateGeometry(en.Current);
                    }
                    _size++;
                }
            }
        }

        #endregion

        #region IList methods

        /// <summary>
        /// Determines the index of a specific geometry in the geometry list.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The zero-based index of geometry if found in the list; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public virtual Int32 IndexOf(T geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            return Array.IndexOf(_geometries, geometry);
        }

        /// <summary>
        /// Inserts a geometry to the geometry list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the <paramref name="geometry" /> should be inserted.</param>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the geometry does not match the reference system of the collection.</exception>
        public virtual void Insert(Int32 index, T geometry)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _size)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of geometries.");
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (ReferenceSystem != null && geometry.ReferenceSystem != null && geometry.ReferenceSystem.Equals(ReferenceSystem))
                throw new ArgumentException("The reference system of the geometry does not match the reference system of the collection.", "geometry");

            if (_size == _geometries.Length)
                EnsureCapacity(_size + 1);
            if (index < _size)
                Array.Copy(_geometries, index, _geometries, index + 1, _size - index);

            _geometries[index] = geometry;
            _size++;

            OnGeometryChanged();
        }

        /// <summary>
        /// Removes the geometry list geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        public virtual void RemoveAt(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _size)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of geometries.");

            _size--;

            if (index < _size)
                Array.Copy(_geometries, index + 1, _geometries, index, _size - index);

            _geometries[_size] = default(T);

            OnGeometryChanged();
        }

        #endregion

        #region ICollection methods

        /// <summary>
        /// Determines whether a geometry is in the geometry list.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if item is found in the geometry list; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public virtual Boolean Contains(T geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            return _geometries.Contains(geometry);
        }

        /// <summary>
        /// Adds a geometry to the end of the geometry list.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the geometry does not match the reference system of the collection.</exception>
        public virtual void Add(T geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (ReferenceSystem != null && geometry.ReferenceSystem != null && !geometry.ReferenceSystem.Equals(ReferenceSystem))
                throw new ArgumentException("The reference system of the geometry does not match the reference system of the collection.", "geometry");

            if (_size == _geometries.Length) EnsureCapacity(_size + 1);

            _geometries[_size] = geometry;
            _size++;

            OnGeometryChanged();
        }

        /// <summary>
        /// Removes the first occurrence of a specified geometry from the geometry list.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the <paramref name="geometry" /> is successfully removed; otherwise, <c>false</c>.</returns>
        public virtual Boolean Remove(T geometry)
        {
            if (geometry == null)
                return false;

            Int32 index = Array.IndexOf(_geometries, geometry);

            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all geometries from the geometry list.
        /// </summary>
        public virtual void Clear()
        {
            _geometries = new T[_defaultCapacity];
            OnGeometryChanged();
        }

        /// <summary>
        /// Copies the geometries of the geometry list to a <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements copied from geometry list.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The index is less than 0.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source geometry list is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination array.</exception>
        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", "The index is less than 0.");
            if (arrayIndex + _size > array.Length)
                throw new ArgumentException("The number of elements in the source GeometryList is greater than the available space from arrayIndex to the end of the destination array.", "array");

            _geometries.CopyTo(array, arrayIndex);
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the geometry list instance.
        /// </summary>
        /// <returns>
        /// The deep copy of the geometry list instance.
        /// </returns>
        public override Object Clone()
        {
            GeometryList<T> list = new GeometryList<T>(_size, Factory, Metadata);

            list._size = _size;
            for (Int32 i = 0; i < _size; i++)
            {
                list._geometries[i] = (T)_geometries[i].Clone();
            }

            return list;
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the geometry list.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{T}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the geometry list.
        /// </summary>
        /// <returns>A <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="String" /> containing the number of geometries in the collection.</returns>
        public override String ToString()
        {
            if (_size == 0)
                return Name + " EMPTY";

            return Name + " [" + _size + "]";
        }

        #endregion

        #region Protected Geometry methods

        /// <summary>
        /// Called when the geometry is changed.
        /// </summary>
        protected override void OnGeometryChanged()
        {
            base.OnGeometryChanged();
            _version++;
        }

        /// <summary>
        /// Computes the minimal bounding envelope of the geometry.
        /// </summary>
        /// <returns>The <see cref="Envelope" /> instance representing the minimum bounding box of the geometry.</returns>
        protected override Envelope ComputeEnvelope()
        {
            if (_size == 0)
                return Envelope.Undefined;
            else if (_size == 1)
                return _geometries[0].Envelope;
            else
                return Envelope.FromEnvelopes(_geometries.Select(geometry => geometry.Envelope));
        }

        /// <summary>
        /// Computes the boundary of the geometry.
        /// </summary>
        /// <returns>The <see cref="Geometry" /> instance representing the closure of the combinatorial boundary of the geometry.</returns>
        protected override IGeometry ComputeBoundary()
        {
            if (_size == 0)
                return null;

            if (_size == 1)
                return _geometries[0].Boundary as Geometry;

            List<IGeometry> boundaryList = new List<IGeometry>();
            for (Int32 i = 0; i < _size; i++)
            {
                if (_geometries[i] != null && !(_geometries[i] is IPoint))
                {
                    // check whether the boundary contains multiple parts (e.g. polygon)
                    IGeometry boundary = _geometries[i].Boundary;
                    if (boundary is IEnumerable<IGeometry>)
                    {
                        // only the parts should be added to the boundary (so that it is not recursive)
                        foreach (IGeometry geometry in boundary as IEnumerable<IGeometry>)
                            boundaryList.Add(geometry);
                    }
                    else
                    {
                        boundaryList.Add(boundary);
                    }
                }
                    
            }

            if (boundaryList.Count == 0)
                return null;
            if (boundaryList.Count == 1)
                return boundaryList[0];

            return Factory.CreateGeometryCollection(boundaryList);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Ensures the capacity of the linestring is at least the given minimum value.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        private void EnsureCapacity(Int32 min)
        {
            if (_geometries.Length < min)
            {
                Int32 newCapacity = _geometries.Length == 0 ? _defaultCapacity : _geometries.Length * 2;

                if (newCapacity < min)
                    newCapacity = min;

                if (newCapacity > _defaultCapacity)
                {
                    T[] newGeometries = new T[newCapacity];
                    if (_size > 0)
                    {
                        Array.Copy(_geometries, 0, newGeometries, 0, _size);
                    }
                    _geometries = newGeometries;
                }
                else
                {
                    _geometries = new T[_defaultCapacity];
                }
            }
        }

        #endregion
    }
}
