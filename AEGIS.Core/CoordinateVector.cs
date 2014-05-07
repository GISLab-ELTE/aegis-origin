﻿/// <copyright file="CoordinateComparer.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a vector in 3 dimensional coordinate space.
    /// </summary>
    [Serializable]
    public struct CoordinateVector : IEquatable<Coordinate>, IEquatable<CoordinateVector>
    {
        #region Static instances

        /// <summary>
        /// Represents the null <see cref="CoordinateVector" /> value. This field is constant.
        /// </summary>
        public static readonly CoordinateVector NullVector = new CoordinateVector(0, 0, 0);

        #endregion

        #region Private fields

        private readonly Double _x;
        private readonly Double _y;
        private readonly Double _z;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        public Double X { get { return _x; } }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>The Y coordinate.</value>
        public Double Y { get { return _y; } }

        /// <summary>
        /// Gets or sets the Z coordinate.
        /// </summary>
        /// <value>The Z coordinate.</value>
        public Double Z { get { return _z; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CoordinateVector" /> is null.
        /// </summary>
        /// <value><c>true</c> if all coordinates are 0; otherwise, <c>false</c>.</value>
        public Boolean IsNull { get { return _x == 0 && _y == 0 && _z == 0; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CoordinateVector" /> is valid.
        /// </summary>
        /// <value><c>true</c> if all coordinates are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(_x) && !Double.IsNaN(_y) && !Double.IsNaN(_z); } }

        /// <summary>
        /// Gets the length of the <see cref="CoordinateVector" />.
        /// </summary>
        /// <value>The eucledian length of the vector.</value>
        public Double Length { get { return Math.Sqrt(Calculator.Pow(_x, 2) + Calculator.Pow(_y, 2) + Calculator.Pow(_z, 2)); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateVector" /> struct.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public CoordinateVector(Double x, Double y)
        {
            _x = x; _y = y; _z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateVector" /> struct.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public CoordinateVector(Double x, Double y, Double z) 
        {
            _x = x; _y = y; _z = z;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Normalizes the <see cref="CoordinateVector" /> instance.
        /// </summary>
        /// <returns>The normalized <see cref="CoordinateVector" /> with identical direction.</returns>
        public CoordinateVector Normalize()
        {
            Double length = Length;
            return new CoordinateVector(_x / length, _y / length, _z / length);
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="Coordinate" /> are equal.
        /// </summary>
        /// <param name="obj">Another <see cref="Coordinate" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Coordinate another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return _x == another.X && _y == another.Y && _z == another.Z;
        }

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="CoordinateVector" /> are equal.
        /// </summary>
        /// <param name="obj">Another <see cref="CoordinateVector" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(CoordinateVector another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return _x == another._x && _y == another._y && _z == another._z;
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
            if (IsNull)
                return "NULL";

            return "(" + _x + " " + _y + " " + _z + ")";
        }

        #endregion

        #region Operators

        /// <summary>
        /// Sums the specified <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="x">The first vector.</param>
        /// <param name="y">The second vector.</param>
        /// <returns>The sum of the <see cref="CoordinateVector" /> instances.</returns>
        public static CoordinateVector operator +(CoordinateVector x, CoordinateVector y)
        {
            return new CoordinateVector(x._x + y._x, x._y + y._y, x._z + y._z);
        }

        /// <summary>
        /// Extracts the specified <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="x">The first vector.</param>
        /// <param name="y">The second vector.</param>
        /// <returns>The extract of the <see cref="CoordinateVector" /> instances.</returns>
        public static CoordinateVector operator -(CoordinateVector x, CoordinateVector y)
        {
            return new CoordinateVector(x._x - y._x, x._y - y._y, x._z - y._z);
        }

        /// <summary>
        /// Multiplies the specified <see cref="CoordinateVector" /> instance with a <see cref="System.Double" /> instance.
        /// </summary>
        /// <param name="d">The double precision floating point number.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The scalar multiple of the <see cref=":System.Double" /> and <see cref="CoordinateVector" /> instances.</returns>
        public static CoordinateVector operator *(Double d, CoordinateVector vector)
        {
            return new CoordinateVector(d * vector._x, d * vector._y, d * vector._z);
        }

        /// <summary>
        /// Multiplies the specified <see cref="CoordinateVector" /> instance with a <see cref="System.Double" /> instance.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="d">The double precision floating point number.</param>
        /// <returns>The scalar multiple of the <see cref="System.Double" /> and <see cref="CoordinateVector" /> instances.</returns>
        public static CoordinateVector operator *(CoordinateVector vector, Double d)
        {
            return new CoordinateVector(d * vector._x, d * vector._y, d * vector._z);
        }

        /// <summary>
        /// Multiplies the specified <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="x">The first vector.</param>
        /// <param name="y">The second vector.</param>
        /// <returns>The scalar multiple of the <see cref="CoordinateVector" /> instances.</returns>
        public static Double operator *(CoordinateVector x, CoordinateVector y)
        {
            return x._x * y._x + x._y * y._y + x._z * y._z;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="CoordinateVector" /> instances are equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the two <see cref="CoordinateVector" /> instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(CoordinateVector x, CoordinateVector y)
        {
            return x._x == y.X && x._y == y._y && x._z == y._z;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="CoordinateVector" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the two <see cref="CoordinateVector" /> instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(CoordinateVector x, CoordinateVector y)
        {
            return x._x != y.X || x._y != y._y || x._z != y._z;
        }

        /// <summary>
        /// Converts the specified <see cref="Coordinate" /> instance to <see cref="CoordinateVector" />. 
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The <see cref="CoordinateVector" /> equivalent of the specified <see cref="Coordinate" /> instance.</returns>
        public static explicit operator CoordinateVector(Coordinate coordinate)
        {
            return new CoordinateVector(coordinate.X, coordinate.Y, coordinate.Z);
        }

        #endregion

        #region Public static query methods

        /// <summary>
        /// Determines whether the two <see cref="CoordinateVector" /> instances are parallel.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the two <see cref="CoordinateVector" /> instances are parallel; otherwise false.</returns>
        public static Boolean IsParalell(CoordinateVector x, CoordinateVector y)
        {
            return Math.Abs(x._x * y._y - x._y * y._x) <= Calculator.Tolerance && Math.Abs(x._x * y._z - x._z * y._z) <= Calculator.Tolerance;
        }

        /// <summary>
        /// Determines whether the two <see cref="CoordinateVector" /> instances are perpendicular.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the two <see cref="CoordinateVector" /> instances are perpendicular; otherwise false.</returns>
        public static Boolean IsPerpendicular(CoordinateVector x, CoordinateVector y)
        {
            return x._x * y._x + x._y * y._y + x._z * y._z <= Calculator.Tolerance;
        }

        /// <summary>
        /// Sums the specified <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="vectors">The vectors.</param>
        /// <returns>The sum of the <paramref name="vectors" />.</returns>
        public static CoordinateVector Sum(IEnumerable<CoordinateVector> vectors)
        {
            Double sumX = 0, sumY = 0, sumZ = 0;
            foreach (CoordinateVector vector in vectors)
            {
                sumX += vector._x;
                sumY += vector._y;
                sumZ += vector._z;
            }
            return new CoordinateVector(sumX, sumY, sumZ);
        }

        /// <summary>
        /// Sums the specified <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="vectors">The vectors.</param>
        /// <returns>The sum of the <paramref name="vectors" />.</returns>
        public static CoordinateVector Sum(params CoordinateVector[] vectors)
        {
            Double sumX = 0, sumY = 0, sumZ = 0;
            foreach (CoordinateVector vector in vectors)
            {
                sumX += vector._x;
                sumY += vector._y;
                sumZ += vector._z;
            }
            return new CoordinateVector(sumX, sumY, sumZ);
        }

        /// <summary>
        /// Computes the distance between two <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The distance between the two <see cref="CoordinateVector" /> instances.</returns>
        public static Double Distance(CoordinateVector x, CoordinateVector y)
        {
            return Math.Sqrt(Calculator.Pow(x._x - y._x, 2) + Calculator.Pow(x._y - y._y, 2) + Calculator.Pow(x._z - y._z, 2));
        }

        /// <summary>
        /// Computes the dot product of two <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The dot product of two <see cref="CoordinateVector" /> instances.</returns>
        public static Double DotProduct(CoordinateVector x, CoordinateVector y)
        {
            return x._x * y._x + x._y * y._y + x._z * y._z;
        }

        /// <summary>
        /// Computes the perp product of two <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The perp product of two <see cref="CoordinateVector" /> instances.</returns>
        public static Double PerpProduct(CoordinateVector x, CoordinateVector y)
        {
            return x._x * y._y - x._y * y._x;
        }

        /// <summary>
        /// Computes the cross product of two <see cref="CoordinateVector" /> instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The cross product of two <see cref="CoordinateVector" /> instances.</returns>
        public static CoordinateVector CrossProduct(CoordinateVector x, CoordinateVector y)
        {
            return new CoordinateVector(x._y * y._z - x._z * y._y, x._z * y._x - x._x * y._z, x._x * y._y - x._y * y._x);
        }

        #endregion
    }
}
