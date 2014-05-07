/// <copyright file="Envelope.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents an evelope in spatial coordinate space.
    /// </summary>
	[Serializable]
    public class Envelope : IEquatable<Envelope>
    {
        #region Static instances

        /// <summary>
        /// Represents the undefined <see cref="Envelope" /> value. This field is constant.
        /// </summary>
        public static readonly Envelope Undefined = new Envelope(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);

        /// <summary>
        /// Represents the infinite <see cref="Envelope" /> value. This field is constant.
        /// </summary>
        public static readonly Envelope Infinity = new Envelope(Double.NegativeInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.NegativeInfinity, Double.NegativeInfinity, Double.PositiveInfinity);

        #endregion

        #region Private fields

        private readonly Coordinate _minimum;
        private readonly Coordinate _maximum;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the minimum X value.
        /// </summary>
        /// <value>The minimum X value.</value>
        public Double MinX { get { return _minimum.X; } }

        /// <summary>
        /// Gets the minimum Y value.
        /// </summary>
        /// <value>The minimum Y value.</value>
        public Double MinY { get { return _minimum.Y; } }

        /// <summary>
        /// Gets the minimum Z value.
        /// </summary>
        /// <value>The minimum Z value.</value>
        public Double MinZ { get { return _minimum.Z; } }

        /// <summary>
        /// Gets the maximum X value.
        /// </summary>
        /// <value>The maximum X value.</value>
        public Double MaxX { get { return _maximum.X; } }

        /// <summary>
        /// Gets the maximum Y value.
        /// </summary>
        /// <value>The maximum Y value.</value>
        public Double MaxY { get { return _maximum.Y; } }

        /// <summary>
        /// Gets the maximum Z value.
        /// </summary>
        /// <value>The maximum Z value.</value>
        public Double MaxZ { get { return _maximum.Z; } }

        /// <summary>
        /// Gets the minimal coordinate in all dimensions.
        /// </summary>
        /// <value>The minimal coordinate in all dimensions.</value>
        public Coordinate Minimum { get { return _minimum; } }

        /// <summary>
        /// Gets the maximal coordinate in all dimensions.
        /// </summary>
        /// <value>The maximal coordinate in all dimensions.</value>
        public Coordinate Maximum { get { return _maximum; } }

        /// <summary>
        /// Gets the center coordinate in all dimensions.
        /// </summary>
        /// <value>The center coordinate in all dimensions.</value>
        public Coordinate Center { get { return new Coordinate((_minimum.X + _maximum.X) / 2, (_minimum.Y + _maximum.Y) / 2, (_minimum.Z + _maximum.Z) / 2); } }

        /// <summary>
        /// Indicates whether the extent of the instance is zero in all dimensions.
        /// </summary>
        /// <value><c>true</c> if the extent is zero in all dimensions; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return _minimum.Equals(_maximum); } }

        /// <summary>
        /// Indicates whether the instance has valid coordinates.
        /// </summary>
        /// <value><c>true</c> if all coordinates are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return _minimum.IsValid && _maximum.IsValid; } }

        /// <summary>
        /// Indicates whether the instance has zero extent in the Z dimension.
        /// </summary>
        /// <value><c>true</c> if the instance has zero extent in the Z dimension; otherwise, <c>false</c>.</value>
        public Boolean IsPlanar { get { return _minimum.Z == _maximum.Z; } }

        /// <summary>
        /// Gets the surface of the rectangle.
        /// </summary>
        /// <value>The surface of the rectangle. In case of planar rectangles, the surface equals the area.</value>
        public Double Surface
        {
            get
            {
                if (IsPlanar)
                    return (_maximum.X - _minimum.X) * (_maximum.Y - _minimum.Y);
                else
                    return 2 * (_maximum.X - _minimum.X) * (_maximum.Y - _minimum.Y) + 2 * (_maximum.X - _minimum.X) * (_maximum.Z - _minimum.Z) + 2 * (_maximum.Y - _minimum.Y) * (_maximum.Z - _minimum.Z);
            }
        }

        /// <summary>
        /// Gets the volume of the rectangle.
        /// </summary>
        /// <value>The volume of the rectangle. The volume is zero in case of planar rectangles.</value>
        public Double Volume { get { return (_maximum.X - _minimum.X) * (_maximum.Y - _minimum.Y) * (_maximum.Z - _minimum.Z); } }

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
            _maximum = new Coordinate(Math.Max(firstX, secondX), Math.Max(firstY, secondY), Math.Max(firstZ, secondZ));
            _minimum = new Coordinate(Math.Min(firstX, secondX), Math.Min(firstY, secondY), Math.Min(firstZ, secondZ));
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
            return _minimum.X <= coordinate.X && coordinate.X <= _maximum.X &&
                   _minimum.Y <= coordinate.Y && coordinate.Y <= _maximum.Y &&
                   _minimum.Z <= coordinate.Z && coordinate.Z <= _maximum.Z;
        }

        /// <summary>
        /// Determines whether the instance contains another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Envelope other)
        {
            return _minimum.X <= other._minimum.X && other._maximum.X <= _maximum.X &&
                   _minimum.Y <= other._minimum.Y && other._maximum.Y <= _maximum.Y &&
                   _minimum.Z <= other._minimum.Z && other._maximum.Z <= _maximum.Z;
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

            return (_minimum.Equals(another._minimum) && _maximum.Equals(another._maximum));
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

            return (obj is Envelope && _minimum == ((Envelope)obj)._minimum && _maximum == ((Envelope)obj)._maximum);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (_minimum.GetHashCode() >> 2) ^ _maximum.GetHashCode() ^ 190130741;
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
                return "EMPTY";

            return "(" + _minimum.X + " " + _minimum.Y + " " + _minimum.Z + ", " + 
                         _minimum.X + " " + _maximum.Y + " " + _minimum.Z + ", " +
                         _maximum.X + " " + _maximum.Y + " " + _minimum.Z + ", " +
                         _maximum.X + " " + _minimum.Y + " " + _minimum.Z + ", " +
                         _minimum.X + " " + _minimum.Y + " " + _maximum.Z + ", " +
                         _minimum.X + " " + _maximum.Y + " " + _maximum.Z + ", " +
                         _maximum.X + " " + _maximum.Y + " " + _maximum.Z + ", " +
                         _maximum.X + " " + _minimum.Y + " " + _maximum.Z + ")";
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
            return new Envelope(envelopes.Min(envelope => envelope.Minimum.X), envelopes.Max(envelope => envelope.Maximum.X),
                                envelopes.Min(envelope => envelope.Minimum.Y), envelopes.Max(envelope => envelope.Maximum.Y),
                                envelopes.Min(envelope => envelope.Minimum.Z), envelopes.Max(envelope => envelope.Maximum.Z));
        }

        #endregion

        #region Public static query methods

        /// <summary>
        /// Determines whether the envelope contains the specified coordinate.
        /// </summary>
        /// <param name="envelopeMinimum">The envelope minimum coordinate.</param>
        /// <param name="envelopeMaximum">The envelope maximum coordinate.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate envelopeMinimum, Coordinate envelopeMaximum, Coordinate coordinate)
        {
            return envelopeMinimum.X <= coordinate.X && coordinate.X <= envelopeMaximum.X && envelopeMinimum.Y <= coordinate.Y && coordinate.Y <= envelopeMaximum.Y && envelopeMinimum.Z <= coordinate.Z && coordinate.Z <= envelopeMaximum.Z;
        }

        /// <summary>
        /// Determines whether the first envelope contains the second envelope.
        /// </summary>
        /// <param name="firstEnvelopeMinimum">The first envelope minimum coordinate.</param>
        /// <param name="firstEnvelopeMaximum">The first envelope maximum coordinate.</param>
        /// <param name="secondEnvelopeMinimum">The second envelope minimum coordinate.</param>
        /// <param name="secondEnvelopeMaximum">The second envelope maximum coordinate.</param>
        /// <returns><c>true</c> if the envelope the first envelope contains the second envelope; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate firstEnvelopeMinimum, Coordinate firstEnvelopeMaximum, Coordinate secondEnvelopeMinimum, Coordinate secondEnvelopeMaximum)
        {
            return Contains(firstEnvelopeMinimum, firstEnvelopeMaximum, secondEnvelopeMinimum) && Contains(firstEnvelopeMinimum, firstEnvelopeMaximum, secondEnvelopeMaximum);
        }

        /// <summary>
        /// Determines whether the first envelope overlaps the second envelope.
        /// </summary>
        /// <param name="firstEnvelopeMinimum">The first envelope minimum coordinate.</param>
        /// <param name="firstEnvelopeMaximum">The first envelope maximum coordinate.</param>
        /// <param name="secondEnvelopeMinimum">The second envelope minimum coordinate.</param>
        /// <param name="secondEnvelopeMaximum">The second envelope maximum coordinate.</param>
        /// <returns><c>true</c> if the envelope the first envelope overlaps the second envelope; otherwise, <c>false</c>.</returns>
        public static Boolean Overlaps(Coordinate firstEnvelopeMinimum, Coordinate firstEnvelopeMaximum, Coordinate secondEnvelopeMinimum, Coordinate secondEnvelopeMaximum)
        {
            return !(firstEnvelopeMinimum.X > secondEnvelopeMaximum.X || firstEnvelopeMaximum.X < secondEnvelopeMinimum.X || firstEnvelopeMaximum.Y < secondEnvelopeMinimum.Y || firstEnvelopeMinimum.Y > secondEnvelopeMaximum.Y || firstEnvelopeMaximum.Z < secondEnvelopeMinimum.Z || firstEnvelopeMinimum.Z > secondEnvelopeMaximum.Z);
        }

        #endregion
    }
}
