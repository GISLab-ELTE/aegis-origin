/// <copyright file="Polygon.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a polygon geometry in spatial coordinate space.
    /// </summary>
    public class Polygon : Surface, IPolygon
    {
        #region Protected fields

        protected readonly ILinearRing _shell;
        protected readonly List<ILinearRing> _holes;

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the centroid of the polygon.
        /// </summary>
        /// <value>The centroid of the polygon.</value>
        public override Coordinate Centroid 
        { 
            get 
            {
                if (_holes.Count > 0)
                    return PrecisionModel.MakePrecise(PolygonCentroidAlgorithm.ComputeCentroid(_shell.Coordinates, _holes.Select(shell => shell.Coordinates)));
                else
                    return PrecisionModel.MakePrecise(PolygonCentroidAlgorithm.ComputeCentroid(_shell.Coordinates));
            } 
        }

        /// <summary>
        /// Gets a value indicating whether the polygon is empty.
        /// </summary>
        /// <value><c>true</c> if the polygon is considered to be empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty { get { return _shell.Count == 0 && (_holes == null || _holes.Count == 0); } }

        /// <summary>
        /// Gets a value indicating whether the polygon is simple.
        /// </summary>
        /// <value><c>true</c>, as a polygon is always considered to be simple.</value>
        public override Boolean IsSimple { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether the polygon is valid.
        /// </summary>
        /// <value><c>true</c> if the polygon is considered to be valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid { get { return PolygonAlgorithms.IsValid(this); } }

        #endregion

        #region ISurface properties

        /// <summary>
        /// Gets a value indicating whether the polygon is convex.
        /// </summary>
        /// <value><c>true</c> if the polygon is convex; otherwise, <c>false</c>.</value>
        public override Boolean IsConvex { get { return _holes.Count == 0 && PolygonAlgorithms.IsConvex(_shell.Coordinates); } }

        /// <summary>
        /// Gets a value indicating whether the polygon is divided.
        /// </summary>
        /// <value><c>true</c>, as a polygon is never divided.</value>
        public override sealed Boolean IsDivided { get { return false; } }

        /// <summary>
        /// Gets a value indicating whether the polygon is whole.
        /// </summary>
        /// <value><c>true</c> if the polygon contains no holes; otherwise, <c>false</c>.</value>
        public override Boolean IsWhole { get { return _holes.Count == 0; } }

        /// <summary>
        /// Gets the area of the polygon.
        /// </summary>
        /// <value>The area of the surface.</value>
        public override Double Area 
        { 
            get 
            {
                return PolygonAlgorithms.Area(_shell.Coordinates) + _holes.Sum(hole => -PolygonAlgorithms.Area(hole.Coordinates)); 
            } 
        }

        /// <summary>
        /// Gets the perimeter of the polygon.
        /// </summary>
        /// <value>The perimeter of the surface.</value>
        public override Double Perimeter 
        { 
            get 
            { 
                return _shell.Length + _holes.Sum(hole => hole.Length); 
            } 
        }

        #endregion

        #region IBasicPolygon properties (explicit)

        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The <see cref="IBasicLineString" /> representing the shell of the polygon.</value>
        IBasicLineString IBasicPolygon.Shell
        {
            get { return Shell; }
        }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The <see cref="IList{IBasicCurve}" /> containing the holes of the polygon.</value>
        IList<IBasicLineString> IBasicPolygon.Holes
        {
            get { return Holes.Cast<IBasicLineString>().ToList(); }
        }

        #endregion

        #region IPolygon properties

        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The <see cref="LinearRing" /> representing the shell of the polygon.</value>
        public ILinearRing Shell { get { return _shell; } }

        /// <summary>
        /// Gets the number of holes of the polygon.
        /// </summary>
        /// <value>The number of holes in the polygon.</value>
        public Int32 HoleCount { get { return _holes.Count; } }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The <see cref="IList{LinearRing}" /> containing the holes of the polygon.</value>
        public IList<ILinearRing> Holes { get { return _holes.AsReadOnly(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public Polygon(ILinearRing shell, IEnumerable<ILinearRing> holes, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(precisionModel, referenceSystem, metadata)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // initalize shell
            try
            {
                _shell = Factory.CreateLinearRing(shell); // create new shell instance
                _shell.GeometryChanged += new EventHandler(Shell_GeometryChanged); // add event handler
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The shell is empty.", "shell");
            }

            // initialize holes
            _holes = new List<ILinearRing>();
            if (holes != null)
            {
                foreach (ILinearRing hole in holes)
                {
                    if (hole == null)
                        continue;

                    try
                    {
                        _holes.Add(Factory.CreateLinearRing(hole)); // create new hole instance
                        _holes[_holes.Count - 1].GeometryChanged += new EventHandler(Hole_GeometryChanged); // add event handler
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public Polygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(precisionModel, referenceSystem, metadata)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
           
            // initalize shell
            try
            {
                _shell = Factory.CreateLinearRing(shell); // create new shell instance
                _shell.GeometryChanged += new EventHandler(Shell_GeometryChanged); // add event handler
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The shell is empty.", "shell");
            }

            // initialize holes
            _holes = new List<ILinearRing>();
            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> coordinates in holes)
                {
                    if (coordinates == null)
                        continue;

                    try
                    {
                        _holes.Add(Factory.CreateLinearRing(coordinates)); // create new hole instance
                        _holes[_holes.Count - 1].GeometryChanged += new EventHandler(Hole_GeometryChanged); // add event handler
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="factory">The factory of the polygon.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The specified factory is invalid.
        /// </exception>
        public Polygon(ILinearRing shell, IEnumerable<ILinearRing> holes, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // initalize shell
            try
            {
                _shell = Factory.CreateLinearRing(shell); // create new shell instance
                _shell.GeometryChanged += new EventHandler(Shell_GeometryChanged); // add event handler
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The shell is empty.", "shell");
            }

            // initialize holes
            _holes = new List<ILinearRing>();
            if (holes != null)
            {
                foreach (ILinearRing hole in holes)
                {
                    if (hole == null)
                        continue;

                    try
                    {
                        _holes.Add(Factory.CreateLinearRing(hole)); // create new hole instance
                        _holes[_holes.Count - 1].GeometryChanged += new EventHandler(Hole_GeometryChanged); // add event handler
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <param name="factory">The factory of the polygon.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell is empty.
        /// or
        /// The specified factory is invalid.
        /// </exception>
        public Polygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // initalize shell
            try
            {
                _shell = Factory.CreateLinearRing(shell); // create new shell instance
                _shell.GeometryChanged += new EventHandler(Shell_GeometryChanged); // add event handler
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The shell is empty.", "shell");
            }

            // initialize holes
            _holes = new List<ILinearRing>();
            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> coordinates in holes)
                {
                    if (coordinates == null)
                        continue;

                    try
                    {
                        _holes.Add(Factory.CreateLinearRing(coordinates)); // create new hole instance
                        _holes[_holes.Count - 1].GeometryChanged += new EventHandler(Hole_GeometryChanged); // add event handler
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        #endregion

        #region IBasicPolygon methods (explicit)

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        IBasicLineString IBasicPolygon.GetHole(Int32 index)
        {
            return GetHole(index);
        }

        #endregion

        #region IPolygon methods

        /// <summary>
        /// Add a hole to the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.ArgumentNullException">The hole is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the hole does not match the reference system of the polygon.</exception>
        public virtual void AddHole(ILinearRing hole)
        {
            if (hole == null)
                throw new ArgumentNullException("hole", "The hole is null.");
            if (ReferenceSystem != null && hole.ReferenceSystem != null && hole.ReferenceSystem.Equals(ReferenceSystem))
                throw new ArgumentException("The reference system of the hole does not match the reference system of the polygon.", "hole");

            _holes.Add(Factory.CreateLinearRing(hole.Coordinates));
            _holes[_holes.Count - 1].GeometryChanged += new EventHandler(Hole_GeometryChanged); // add event handler

            OnGeometryChanged();
        }

        /// <summary>
        /// Add a hole to the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.ArgumentNullException">The hole is null.</exception>
        public virtual void AddHole(IEnumerable<Coordinate> hole)
        {
            if (hole == null)
                throw new ArgumentNullException("hole", "The hole is null.");
            
            _holes.Add(Factory.CreateLinearRing(hole.Select(coordinate => PrecisionModel.MakePrecise(coordinate))));
            _holes[_holes.Count - 1].GeometryChanged += new EventHandler(Hole_GeometryChanged); // add event handler

            OnGeometryChanged();
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
        /// Index is equal to or greater than the number of holes.
        /// </exception>
        public virtual ILinearRing GetHole(Int32 index)
        {
            if (_holes.Count == 0)
                throw new InvalidOperationException("There are no holes in the polygon.");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _holes.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of holes.");

            return _holes[index];
        }

        /// <summary>
        /// Removes a hole from the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the polygon contains the <paramref name="hole" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The hole is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the hole does not match the reference system of the polygon.</exception>
        public virtual Boolean RemoveHole(ILinearRing hole)
        {
            if (hole == null)
                throw new ArgumentNullException("hole", "The hole is null.");
            if (ReferenceSystem != null && hole.ReferenceSystem != null && hole.ReferenceSystem.Equals(ReferenceSystem))
                throw new ArgumentException("The reference system of the hole does not match the reference system of the polygon.", "hole");

            if (_holes.Count == 0)
                return false;

            for (Int32 i = 0; i < _holes.Count; i++)
            {
                if (_holes[i].Equals(hole))
                {
                    _holes[i].GeometryChanged -= Hole_GeometryChanged; // remove event handler
                    _holes.RemoveAt(i);

                    OnGeometryChanged();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the hole at the specified index of the polygon.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is greater than or equal to the number of holes.
        /// </exception>
        public virtual void RemoveHoleAt(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _holes.Count)
                throw new ArgumentOutOfRangeException("index", "The index is greater than or equal to the number of holes.");

            _holes[index].GeometryChanged -= Hole_GeometryChanged; // remove event handler
            _holes.RemoveAt(index);

            OnGeometryChanged();
        }

        /// <summary>
        /// Removes all holes from the polygon.
        /// </summary>
        public virtual void ClearHoles()
        {
            _holes.ForEach(hole => hole.GeometryChanged -= Hole_GeometryChanged); // remove event handler
            _holes.Clear();

            OnGeometryChanged();
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the polygon instance.
        /// </summary>
        /// <returns>The deep copy of the polygon instance.</returns>
        public override Object Clone()
        {
            return new Polygon(_shell, _holes, Factory, Metadata);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="String" /> containing the coordinates of the shell and the holes.</returns>
        public override String ToString()
        {
            if (IsEmpty)
                return Name + " EMPTY";
            else
                return Name + " (" + _shell.Coordinates.Select(coordinate => coordinate.X + " " + coordinate.Y + " " + coordinate.Z).Aggregate((x, y) => x + ", " + y) + 
                    ((_holes != null) ? "; " + _holes.Select(interiorBoundary => interiorBoundary.Coordinates.Select(coordinate => coordinate.X + " " + coordinate.Y + " " + coordinate.Z).Aggregate((x, y) => x + ", " + y)).Aggregate((x, y) => x + "; " + y) : "") + ")";
        }

        #endregion

        #region Protected Geometry methods

        /// <summary>
        /// Computes the minimal bounding envelope of the geometry.
        /// </summary>
        /// <returns>The minimum bounding box of the geometry.</returns>
        protected override Envelope ComputeEnvelope()
        {
            return Envelope.FromCoordinates(_shell.Coordinates);
        }

        /// <summary>
        /// Computes the boundary of the geometry.
        /// </summary>
        /// <returns>The closure of the combinatorial boundary of the geometry.</returns>
        protected override IGeometry ComputeBoundary()
        {
            List<ILinearRing> boundary = new List<ILinearRing>() { Factory.CreateLinearRing(_shell.Coordinates) };
            boundary.AddRange(_holes.Select(hole => Factory.CreateLinearRing(hole.Coordinates)));

            return Factory.CreateMultiLineString(boundary);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Handles the GeometryChanged event of the shell.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Shell_GeometryChanged(Object sender, EventArgs e)
        {
            OnGeometryChanged();
        }

        /// <summary>
        /// Handles the GeometryChanged event of the hole.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Hole_GeometryChanged(Object sender, EventArgs e)
        {
            OnGeometryChanged();
        }

        #endregion
    }
}
