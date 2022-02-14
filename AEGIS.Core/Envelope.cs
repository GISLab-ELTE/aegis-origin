/// <copyright file="Envelope.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents an envelope in spatial coordinate space.
    /// </summary>
	[Serializable]
    public class Envelope : IEquatable<Envelope>
    {
        #region Public static instances

        /// <summary>
        /// Represents the undefined <see cref="Envelope" /> value. This field is constant.
        /// </summary>
        public static readonly Envelope Undefined = new Envelope(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);

        /// <summary>
        /// Represents the infinite <see cref="Envelope" /> value. This field is constant.
        /// </summary>
        public static readonly Envelope Infinity = new Envelope(Double.NegativeInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.NegativeInfinity, Double.NegativeInfinity, Double.PositiveInfinity);

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the minimum X value.
        /// </summary>
        /// <value>The minimum X value.</value>
        public Double MinX { get { return Minimum.X; } }

        /// <summary>
        /// Gets the minimum Y value.
        /// </summary>
        /// <value>The minimum Y value.</value>
        public Double MinY { get { return Minimum.Y; } }

        /// <summary>
        /// Gets the minimum Z value.
        /// </summary>
        /// <value>The minimum Z value.</value>
        public Double MinZ { get { return Minimum.Z; } }

        /// <summary>
        /// Gets the maximum X value.
        /// </summary>
        /// <value>The maximum X value.</value>
        public Double MaxX { get { return Maximum.X; } }

        /// <summary>
        /// Gets the maximum Y value.
        /// </summary>
        /// <value>The maximum Y value.</value>
        public Double MaxY { get { return Maximum.Y; } }

        /// <summary>
        /// Gets the maximum Z value.
        /// </summary>
        /// <value>The maximum Z value.</value>
        public Double MaxZ { get { return Maximum.Z; } }

        /// <summary>
        /// Gets the minimal coordinate in all dimensions.
        /// </summary>
        /// <value>The minimal coordinate in all dimensions.</value>
        public Coordinate Minimum { get; private set; }

        /// <summary>
        /// Gets the maximal coordinate in all dimensions.
        /// </summary>
        /// <value>The maximal coordinate in all dimensions.</value>
        public Coordinate Maximum { get; private set; }

        /// <summary>
        /// Gets the center coordinate in all dimensions.
        /// </summary>
        /// <value>The center coordinate in all dimensions.</value>
        public Coordinate Center { get { return new Coordinate((Minimum.X + Maximum.X) / 2, (Minimum.Y + Maximum.Y) / 2, (Minimum.Z + Maximum.Z) / 2); } }

        /// <summary>
        /// Indicates whether the extent of the instance is zero in all dimensions.
        /// </summary>
        /// <value><c>true</c> if the extent is zero in all dimensions; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return Minimum.Equals(Maximum); } }

        /// <summary>
        /// Indicates whether the instance has valid coordinates.
        /// </summary>
        /// <value><c>true</c> if all coordinates are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return Minimum.IsValid && Maximum.IsValid; } }

        /// <summary>
        /// Indicates whether the instance has zero extent in the Z dimension.
        /// </summary>
        /// <value><c>true</c> if the instance has zero extent in the Z dimension; otherwise, <c>false</c>.</value>
        public Boolean IsPlanar { get { return Minimum.Z == Maximum.Z; } }

        /// <summary>
        /// Gets the surface of the rectangle.
        /// </summary>
        /// <value>The surface of the rectangle. In case of planar rectangles, the surface equals the area.</value>
        public Double Surface
        {
            get
            {
                if (IsPlanar)
                    return (Maximum.X - Minimum.X) * (Maximum.Y - Minimum.Y);
                else
                    return 2 * (Maximum.X - Minimum.X) * (Maximum.Y - Minimum.Y) + 2 * (Maximum.X - Minimum.X) * (Maximum.Z - Minimum.Z) + 2 * (Maximum.Y - Minimum.Y) * (Maximum.Z - Minimum.Z);
            }
        }

        /// <summary>
        /// Gets the volume of the rectangle.
        /// </summary>
        /// <value>The volume of the rectangle. The volume is zero in case of planar rectangles.</value>
        public Double Volume { get { return (Maximum.X - Minimum.X) * (Maximum.Y - Minimum.Y) * (Maximum.Z - Minimum.Z); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class.
        /// </summary>
        /// <param name="firstX">The first X coordinate.</param>
        /// <param name="secondX">The second X coordinate.</param>
        /// <param name="firstY">The first Y coordinate.</param>
        /// <param name="secondY">The second Y coordinate.</param>
        /// <param name="firstZ">The first Z coordinate.</param>
        /// <param name="secondZ">The second Z coordinate.</param>
        public Envelope(Double firstX, Double secondX, Double firstY, Double secondY, Double firstZ, Double secondZ)
        {
            Maximum = new Coordinate(Math.Max(firstX, secondX), Math.Max(firstY, secondY), Math.Max(firstZ, secondZ));
            Minimum = new Coordinate(Math.Min(firstX, secondX), Math.Min(firstY, secondY), Math.Min(firstZ, secondZ));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the envelope contains the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Coordinate coordinate)
        {
            return Minimum.X <= coordinate.X && coordinate.X <= Maximum.X &&
                   Minimum.Y <= coordinate.Y && coordinate.Y <= Maximum.Y &&
                   Minimum.Z <= coordinate.Z && coordinate.Z <= Maximum.Z;
        }

        /// <summary>
        /// Determines whether the instance contains another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Envelope other)
        {
            if (other == null)
                return false;

            return Minimum.X <= other.Minimum.X && other.Maximum.X <= Maximum.X &&
                   Minimum.Y <= other.Minimum.Y && other.Maximum.Y <= Maximum.Y &&
                   Minimum.Z <= other.Minimum.Z && other.Maximum.Z <= Maximum.Z;
        }

        /// <summary>
        /// Determines whether the instance crosses another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope crosses <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Crosses(Envelope other)
        {
            if (other == null)
                return false;

            return !Disjoint(other) && !Equals(other);
        }

        /// <summary>
        /// Determines whether the instance is disjoint from another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope is disjoint from <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Disjoint(Envelope other)
        {
            if (other == null)
                return false;

            return Maximum.X < other.Minimum.X || Minimum.X > other.Maximum.X ||
                   Maximum.Y < other.Minimum.Y || Minimum.Y > other.Maximum.Y ||
                   Maximum.Z < other.Minimum.Z || Minimum.Z > other.Maximum.Z;  
        }

        /// <summary>
        /// Determines whether the instance intersects another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope intersects <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Intersects(Envelope other)
        {
            if (other == null)
                return false;

            return !Disjoint(other);
        }

        /// <summary>
        /// Determines whether the instance overlaps another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope overlaps <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Overlaps(Envelope other)
        {
            if (other == null)
                return false;

            return !Disjoint(other) && !Equals(other);
        }

        /// <summary>
        /// Determines whether the instance touches another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope touches <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Touches(Envelope other)
        {
            if (other == null)
                return false;

            return !Disjoint(other) && (Minimum.X == other.Maximum.X || Maximum.X == other.Minimum.X ||
                                        Minimum.Y == other.Maximum.Y || Maximum.Y == other.Minimum.Y ||
                                        Minimum.Z == other.Maximum.Z || Maximum.Z == other.Minimum.Z);
        }

        /// <summary>
        /// Determines whether the instance is within another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope is within <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Within(Envelope other)
        {
            if (other == null)
                return false;

            return other.Contains(this);
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified envelope are equal.
        /// </summary>
        /// <param name="another">The envelope to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Envelope another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return (Minimum.Equals(another.Minimum) && Maximum.Equals(another.Maximum));
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return (obj is Envelope && Minimum == ((Envelope)obj).Minimum && Maximum == ((Envelope)obj).Maximum);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (Minimum.GetHashCode() >> 2) ^ Maximum.GetHashCode() ^ 190130741;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>The <see cref="System.String" /> containing the coordinates of the instance.</returns>
        public override String ToString()
        {
            if (!IsValid)
                return "INVALID";

            if (IsEmpty)
                return String.Format("EMPTY ({0} {1} {2})", Minimum.X, Minimum.Y, Minimum.Z);

            return String.Format("({0} {1} {2}, {3} {4} {5})", Minimum.X, Minimum.Y, Minimum.Z, Maximum.X, Maximum.Y, Maximum.Z);
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class based on coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The <see cref="Envelope" /> created from the coordinates.</returns>
        public static Envelope FromCoordinates(params Coordinate[] coordinates)
        {
            if (coordinates == null || coordinates.Length == 0)
                return Envelope.Undefined;

            return new Envelope(coordinates.Min(coordinate => coordinate.X), coordinates.Max(coordinate => coordinate.X),
                                coordinates.Min(coordinate => coordinate.Y), coordinates.Max(coordinate => coordinate.Y),
                                coordinates.Min(coordinate => coordinate.Z), coordinates.Max(coordinate => coordinate.Z));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class based on coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The <see cref="Envelope" /> created from the coordinates.</returns>
        public static Envelope FromCoordinates(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null || !coordinates.Any())
                return Envelope.Undefined;

            return new Envelope(coordinates.Min(coordinate => coordinate.X), coordinates.Max(coordinate => coordinate.X),
                                coordinates.Min(coordinate => coordinate.Y), coordinates.Max(coordinate => coordinate.Y),
                                coordinates.Min(coordinate => coordinate.Z), coordinates.Max(coordinate => coordinate.Z));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class based on other instances.
        /// </summary>
        /// <param name="envelopes">The envelopes.</param>
        /// <returns>The <see cref="Envelope" /> created from the envelopes.</returns>
        public static Envelope FromEnvelopes(params Envelope[] envelopes)
        {
            if (envelopes == null || envelopes.Length == 0)
                return Envelope.Undefined;

            return new Envelope(envelopes.Min(envelope => envelope.Minimum.X), envelopes.Max(envelope => envelope.Maximum.X),
                                envelopes.Min(envelope => envelope.Minimum.Y), envelopes.Max(envelope => envelope.Maximum.Y),
                                envelopes.Min(envelope => envelope.Minimum.Z), envelopes.Max(envelope => envelope.Maximum.Z));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class based on other instances.
        /// </summary>
        /// <param name="envelopes">The envelopes.</param>
        /// <returns>The <see cref="Envelope" /> created from the envelopes.</returns>
        public static Envelope FromEnvelopes(IEnumerable<Envelope> envelopes)
        {
            if (envelopes == null || !envelopes.Any())
                return Envelope.Undefined;

            return new Envelope(envelopes.Min(envelope => envelope.Minimum.X), envelopes.Max(envelope => envelope.Maximum.X),
                                envelopes.Min(envelope => envelope.Minimum.Y), envelopes.Max(envelope => envelope.Maximum.Y),
                                envelopes.Min(envelope => envelope.Minimum.Z), envelopes.Max(envelope => envelope.Maximum.Z));
        }

        #endregion

        #region Public static query methods

        /// <summary>
        /// Determines whether the envelope contains the specified coordinate.
        /// </summary>
        /// <param name="first">The first coordinate of the envelope.</param>
        /// <param name="second">The second coordinate of the envelope.</param>
        /// <param name="coordinate">The examined coordinate.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first"/> and <paramref name="second"/> contains <paramref name="coordinate"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate first, Coordinate second, Coordinate coordinate)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Contains(coordinate);
        }

        /// <summary>
        /// Determines whether the first envelope contains the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the defined by <paramref name="first"/> and <paramref name="second"/> contains the envelope defined by <paramref name="third"/> and <paramref name="fourth"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Contains(new Envelope(third.X, fourth.X, third.Y, fourth.Y, third.Z, fourth.Z));
        }

        /// <summary>
        /// Determines whether the first envelope crosses the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the defined by <paramref name="first"/> and <paramref name="second"/> crosses the envelope defined by <paramref name="third"/> and <paramref name="fourth"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Crosses(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Crosses(new Envelope(third.X, fourth.X, third.Y, fourth.Y, third.Z, fourth.Z));
        }

        /// <summary>
        /// Determines whether the first envelope is disjoint from the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the defined by <paramref name="first"/> and <paramref name="second"/> is disjoint from the envelope defined by <paramref name="third"/> and <paramref name="fourth"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Disjoint(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Disjoint(new Envelope(third.X, fourth.X, third.Y, fourth.Y, third.Z, fourth.Z));
        }

        /// <summary>
        /// Determines whether the first envelope is equal to the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the defined by <paramref name="first"/> and <paramref name="second"/> is equal to the envelope defined by <paramref name="third"/> and <paramref name="fourth"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Equals(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Equals(new Envelope(third.X, fourth.X, third.Y, fourth.Y, third.Z, fourth.Z));
        }

        /// <summary>
        /// Determines whether the first envelope intersects the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the defined by <paramref name="first"/> and <paramref name="second"/> intersects the envelope defined by <paramref name="third"/> and <paramref name="fourth"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Intersects(new Envelope(third.X, fourth.X, third.Y, fourth.Y, third.Z, fourth.Z));
        }

        /// <summary>
        /// Determines whether the first envelope overlaps the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the defined by <paramref name="first"/> and <paramref name="second"/> overlaps the envelope defined by <paramref name="third"/> and <paramref name="fourth"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Overlaps(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Overlaps(new Envelope(third.X, fourth.X, third.Y, fourth.Y, third.Z, fourth.Z));
        }

        /// <summary>
        /// Determines whether the first envelope touches the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the defined by <paramref name="first"/> and <paramref name="second"/> touches the envelope defined by <paramref name="third"/> and <paramref name="fourth"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Touches(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Touches(new Envelope(third.X, fourth.X, third.Y, fourth.Y, third.Z, fourth.Z));
        }

        /// <summary>
        /// Determines whether the first envelope is within the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the defined by <paramref name="first"/> and <paramref name="second"/> is within the envelope defined by <paramref name="third"/> and <paramref name="fourth"/>; otherwise, <c>false</c>.</returns>
        public static Boolean Within(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            Envelope envelope = new Envelope(first.X, second.X, first.Y, second.Y, first.Z, second.Z);

            return envelope.Within(new Envelope(third.X, fourth.X, third.Y, fourth.Y, third.Z, fourth.Z));
        }

        #endregion
    }
}
