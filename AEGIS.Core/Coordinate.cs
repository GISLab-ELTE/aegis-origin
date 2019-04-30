/// <copyright file="Coordinate.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a coordinate in 3 dimensional coordinate space.
    /// </summary>
    [Serializable]
    public struct Coordinate : IEquatable<Coordinate>, IEquatable<CoordinateVector>
    {
        #region Public static instances

        /// <summary>
        /// Represents the empty <see cref="Coordinate" /> value. This field is read-only.
        /// </summary>
        public static readonly Coordinate Empty = new Coordinate(0, 0, 0);

        /// <summary>
        /// Represents the undefined <see cref="Coordinate" /> value. This field is read-only.
        /// </summary>
        public static readonly Coordinate Undefined = new Coordinate(Double.NaN, Double.NaN, Double.NaN);

        #endregion

        #region Private fields

        /// <summary>
        /// The X component.
        /// </summary>
        private readonly Double _x;

        /// <summary>
        /// The Y component.
        /// </summary>
        private readonly Double _y;

        /// <summary>
        /// The Z component.
        /// </summary>
        private readonly Double _z;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the X component.
        /// </summary>
        /// <value>The X component.</value>
        public Double X { get { return _x; } }

        /// <summary>
        /// Gets the Y component.
        /// </summary>
        /// <value>The Y component.</value>
        public Double Y { get { return _y; } }

        /// <summary>
        /// Gets the Z component.
        /// </summary>
        /// <value>The Z component.</value>
        public Double Z { get { return _z; } }

        /// <summary>
        /// Gets a value indicating whether the coordinate is empty.
        /// </summary>
        /// <value><c>true</c> if all component are <c>0</c>; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return _z == 0 && _y == 0 && _x == 0; } }

        /// <summary>
        /// Gets a value indicating whether the coordinate is valid.
        /// </summary>
        /// <value><c>true</c> if all component are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(_x) && !Double.IsNaN(_y) && !Double.IsNaN(_z); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate" /> struct.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        public Coordinate(Double x, Double y)
        {
            _x = x; _y = y; _z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate" /> struct.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public Coordinate(Double x, Double y, Double z) 
        {
            _x = x; _y = y; _z = z;
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="Coordinate" /> are equal.
        /// </summary>
        /// <param name="another">Another <see cref="Coordinate" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Coordinate another)
        {
            if (ReferenceEquals(this, another)) return true;

            return _x == another._x && _y == another._y && _z == another._z;
        }

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="CoordinateVector" /> are equal.
        /// </summary>
        /// <param name="another">Another <see cref="CoordinateVector" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(CoordinateVector another)
        {
            if (ReferenceEquals(this, another)) return true;

            return _x == another.X && _y == another.Y && _z == another.Z;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return (obj is Coordinate && Equals((Coordinate)obj) || obj is CoordinateVector && Equals((CoordinateVector)obj));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (_x.GetHashCode() >> 4) ^ (_y.GetHashCode() >> 2) ^ _z.GetHashCode() ^ 57600017;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            if (!IsValid)
                return "INVALID";

            if (IsEmpty)
                return "EMPTY";
            
            return "(" + _x + " " + _y + " " + _z + ")";
        }

        #endregion

        #region Operators

        /// <summary>
        /// Sums the specified <see cref="Coordinate" /> instance with a <see cref="CoordinateVector" /> instance.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The sum of the <see cref="Coordinate" /> and <see cref="CoordinateVector" /> instances.</returns>
        public static Coordinate operator +(Coordinate coordinate, CoordinateVector vector)
        {
            return new Coordinate(coordinate.X + vector.X, coordinate.Y + vector.Y, coordinate.Z + vector.Z);
        }

        /// <summary>
        /// Extracts the specified <see cref="Coordinate" /> instance with a <see cref="CoordinateVector" /> instance.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The extract of the <see cref="Coordinate" /> and <see cref="CoordinateVector" /> instances.</returns>
        public static Coordinate operator -(Coordinate coordinate, CoordinateVector vector)
        {
            return new Coordinate(coordinate.X - vector.X, coordinate.Y - vector.Y, coordinate.Z - vector.Z);
        }

        /// <summary>
        /// Extracts the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The extract of the two <see cref="Coordinate" /> instances.</returns>
        public static CoordinateVector operator -(Coordinate first, Coordinate second)
        {
            return new CoordinateVector(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Coordinate" /> instances are equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two <see cref="Coordinate" /> instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Coordinate first, Coordinate second)
        {
            if (ReferenceEquals(first, second)) return true;

            return first._x == second.X && first._y == second._y && first._z == second._z;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Coordinate" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two <see cref="Coordinate" /> instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Coordinate first, Coordinate second)
        {
            return first._x != second.X || first._y != second._y || first._z != second._z;
        }

        /// <summary>
        /// Converts the specified <see cref="CoordinateVector" /> instance to <see cref="Coordinate" />. 
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The <see cref="Coordinate" /> equivalent of the specified <see cref="CoordinateVector" /> instance.</returns>
        public static explicit operator Coordinate(CoordinateVector vector)
        {
            return new Coordinate(vector.X, vector.Y, vector.Z);
        }

        #endregion

        #region Public static query methods

        /// <summary>
        /// Computes the distance between the two <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The distance between the two <see cref="Coordinate" /> instances.</returns>
        public static Double Distance(Coordinate first, Coordinate second)
        {
            if (!first.IsValid || !second.IsValid)
                return Double.NaN;

            if (first.Equals(second))
                return 0;

            return Math.Sqrt(Calculator.Pow(first.X - second.X, 2) + Calculator.Pow(first.Y - second.Y, 2) + Calculator.Pow(first.Z - second.Z, 2));
        }

        /// <summary>
        /// Computes the centroid of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The centroid of the specified <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">coordinates;The coordinate array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The count is negative.
        /// or
        /// The count is greater than the number of elements in the array.
        /// </exception>
        public static Coordinate Centroid(params Coordinate[] coordinates)
        {
            return Centroid(coordinates, coordinates.Length);
        }

        /// <summary>
        /// Computes the centroid of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The array of coordinates.</param>
        /// <param name="count">The number of coordinates taken from the array.</param>
        /// <returns>The centroid of the specified <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The count is negative.
        /// or
        /// The count is greater than the number of elements in the array.
        /// </exception>
        public static Coordinate Centroid(Coordinate[] coordinates, Int32 count)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The coordinate array is null.");
            if (count < 0)
                throw new ArgumentOutOfRangeException("The count is negative.");
            if (count > coordinates.Length)
                throw new ArgumentOutOfRangeException("The count is greater than the number of elements in the array.");

            if (count == 0)
                return Coordinate.Undefined;

            Double sumX = 0, sumY = 0, sumZ = 0;
            for (Int32 i = 0; i < count; i++)
            {
                sumX += coordinates[i].X;
                sumY += coordinates[i].Y;
                sumZ += coordinates[i].Z;
            }
            sumX /= count;
            sumY /= count;
            sumZ /= count;

            return new Coordinate(sumX, sumY, sumZ);
        }

        /// <summary>
        /// Computes the centroid of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The array of coordinates.</param>
        /// <param name="count">The number of coordinates taken from the array.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The centroid of the specified <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The count is negative.
        /// or
        /// The count is greater than the number of elements in the array.
        /// </exception>
        public static Coordinate Centroid(Coordinate[] coordinates, Int32 count, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            return precision.MakePrecise(Centroid(coordinates, count));
        }

        /// <summary>
        /// Computes the centroid of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The centroid of the specified <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate collection is null.</exception>
        public static Coordinate Centroid(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The coordinate collection is null.");

            Double sumX = 0, sumY = 0, sumZ = 0;
            Int32 count = 0;
            foreach (Coordinate coordinate in coordinates)
            {
                sumX += coordinate.X;
                sumY += coordinate.Y;
                sumZ += coordinate.Z;
                count++;
            }

            if (count == 0)
                return Coordinate.Undefined;

            sumX /= count;
            sumY /= count;
            sumZ /= count;

            return new Coordinate(sumX, sumY, sumZ);
        }

        /// <summary>
        /// Computes the centroid of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The centroid of the specified <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">coordinates;The coordinate collection is null.</exception>
        public static Coordinate Centroid(IEnumerable<Coordinate> coordinates, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            return precision.MakePrecise(Centroid(coordinates));
        }

        /// <summary>
        /// Computes the lower bound of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The lower bound of the specified <see cref="Coordinate" /> instances.</returns>
        public static Coordinate LowerBound(params Coordinate[] coordinates)
        {
            return new Coordinate(coordinates.Min(coordinate => coordinate.X), coordinates.Min(coordinate => coordinate.Y), coordinates.Min(coordinate => coordinate.Z));
        }

        /// <summary>
        /// Computes the lower bound of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The lower bound of the specified <see cref="Coordinate" /> instances.</returns>
        public static Coordinate LowerBound(IEnumerable<Coordinate> coordinates)
        {
            return new Coordinate(coordinates.Min(coordinate => coordinate.X), coordinates.Min(coordinate => coordinate.Y), coordinates.Min(coordinate => coordinate.Z));
        }

        /// <summary>
        /// Computes the lower bound of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The lower bound of the specified <see cref="Coordinate" /> instances.</returns>
        public static Coordinate LowerBound(IEnumerable<Coordinate> coordinates, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            return precision.MakePrecise(LowerBound(coordinates));
        }

        /// <summary>
        /// Computes the upper bound of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The upper bound of the specified <see cref="Coordinate" /> instances.</returns>
        public static Coordinate UpperBound(params Coordinate[] coordinates)
        {
            return new Coordinate(coordinates.Max(coordinate => coordinate.X), coordinates.Max(coordinate => coordinate.Y), coordinates.Max(coordinate => coordinate.Z));
        }

        /// <summary>
        /// Computes the upper bound of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The upper bound of the specified <see cref="Coordinate" /> instances.</returns>
        public static Coordinate UpperBound(IEnumerable<Coordinate> coordinates)
        {
            return new Coordinate(coordinates.Max(coordinate => coordinate.X), coordinates.Max(coordinate => coordinate.Y), coordinates.Max(coordinate => coordinate.Z));
        }

        /// <summary>
        /// Computes the upper bound of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The upper bound of the specified <see cref="Coordinate" /> instances.</returns>
        public static Coordinate UpperBound(IEnumerable<Coordinate> coordinates, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            return precision.MakePrecise(UpperBound(coordinates));
        }

        /// <summary>
        /// Computes the orientation of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="origin">The coordinate of origin.</param>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The orientation of the second <see cref="Coordinate" /> to the first with respect to origin.</returns>
        public static Orientation Orientation(Coordinate origin, Coordinate first, Coordinate second)
        {
            return Orientation(origin, first, second, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the orientation of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="origin">The coordinate of origin.</param>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The orientation of the second <see cref="Coordinate" /> to the first with respect to origin.</returns>
        public static Orientation Orientation(Coordinate origin, Coordinate first, Coordinate second, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;
            
            Double det = (first.X - origin.X) * (second.Y - origin.Y) - (first.Y - origin.Y) * (second.X - origin.X);

            if (Math.Abs(det) <= precision.Tolerance(origin, first, second))
                return AEGIS.Orientation.Collinear;

            if (det > 0)
                return AEGIS.Orientation.CounterClockwise;
            else
                return AEGIS.Orientation.Clockwise;
        }

        /// <summary>
        /// Computes the angle of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="origin">The coordinate of origin.</param>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The angle between first and second <see cref="Coordinate" /> with respect to origin.</returns>
        public static Double Angle(Coordinate origin, Coordinate first, Coordinate second)
        {
            Double distanceOriginFirst = Distance(origin, first);
            Double distanceOriginSecond = Distance(origin, second);
            Double distanceFirstSecond = Distance(first, second);

            return Math.Acos((distanceOriginFirst * distanceOriginFirst + distanceOriginSecond * distanceOriginSecond - distanceFirstSecond * distanceFirstSecond) / (2 * distanceOriginFirst * distanceOriginSecond));
        }

        #endregion
    }
}
