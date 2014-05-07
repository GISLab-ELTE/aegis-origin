/// <copyright file="Instant.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Temporal
{
    /// <summary>
    /// Represents an instant on the timeline.
    /// </summary>
    /// <remarks>
    /// An instant is a zero-dimensional geometric primitive that represents position in time. It is equivalent to a point in space. 
    /// In practice, an instant is an interval whose duration is less than the resolution of the time scale.
    /// </remarks>
    public struct Instant : ITemporalPrimitive
    {
        #region Public static instances

        /// <summary>
        /// Represents the zero <see cref="Instant" />. This field is read-only.
        /// </summary>
        public static readonly Instant Zero = new Instant(0);

        /// <summary>
        /// Represents the smallest possible value of a <see cref="Instant" />. This field is read-only.
        /// </summary>
        public static readonly Instant MinValue = new Instant(Int64.MinValue);

        /// <summary>
        /// Represents the largest possible value of a <see cref="Instant" />. This field is read-only.
        /// </summary>
        public static readonly Instant MaxValue = new Instant(Int64.MaxValue);

        #endregion

        #region Private fields

        private readonly Int64 _ticks;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the number of ticks of the instant.
        /// </summary>
        /// <value>The number of ticks of the instant.</value>
        public Int64 Ticks { get { return _ticks; } }

        #endregion

        #region ITemporalPrimitive properties

        /// <summary>
        /// Gets the length of the <see cref="ITemporalPrimitive" />.
        /// </summary>
        /// <value>The length of the period.</value>
        public Duration Length { get { return Duration.Zero; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITemporalPrimitive" /> instance is valid.
        /// </summary>
        /// <value><c>true</c> if the order represents a valid temporal context; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return true; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Instant" /> struct.
        /// </summary>
        /// <param name="position">The ticks of the instant.</param>
        public Instant(Int64 ticks)
        {
            _ticks = ticks;
        }

        #endregion

        #region ITemporalPrimitive methods

        /// <summary>
        /// Computes the distance to the other <see cref="ITemporalPrimitive" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The distance to the other <see cref="ITemporalPrimitive" /> instance.</returns>
        /// <exception cref="System.ArgumentNullException">The other instance is null.</exception>
        /// <exception cref="System.ArgumentException">The other instance is not valid.</exception>
        public Duration Distance(ITemporalPrimitive other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other instance is null.");
            if (!other.IsValid)
                throw new ArgumentException("The other instance is not valid.", "other");

            if (ReferenceEquals(other, this))
                return Duration.Zero;

            if (other is Instant)
            {
                return new Duration(((Instant)other)._ticks - _ticks);
            }
            else if (other is Period)
            {
                return Distance((Period)other);
            }
            else
            {
                return Duration.MinValue;
            }
        }

        /// <summary>
        /// Determines the relative position compared to the other <see cref="ITemporalPrimitive" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The <see cref="RelativePosition" /> of this instance compared to the other.</returns>
        /// <exception cref="System.ArgumentNullException">The other instance is null.</exception>
        /// <exception cref="System.ArgumentException">The other instance is not valid.</exception>
        public RelativePosition RelativePosition(ITemporalPrimitive other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other instance is null.");
            if (!other.IsValid)
                throw new ArgumentException("The other instance is not valid.", "other");

            if (ReferenceEquals(other, this))
                return Temporal.RelativePosition.Equals;

            if (other is Instant)
            {
                return RelativePosition((Instant)other);
            }
            else if (other is Period)
            {
                return RelativePosition((Period)other);
            }
            else
            {
                return Temporal.RelativePosition.Undefined;
            }
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified <see cref="ITemporalPrimitive" /> are equal.
        /// </summary>
        /// <param name="another">Another <see cref="ITemporalPrimitive" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(ITemporalPrimitive another)
        {
            if (ReferenceEquals(another, null)) return false;
            if (ReferenceEquals(another, this)) return true;

            return ((another is Instant) && _ticks == ((Instant)another)._ticks) ||
                   ((another is Period) && _ticks == ((Period)another).Begin._ticks && _ticks == ((Period)another).End._ticks);
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
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;

            return ((obj is Instant) && _ticks == ((Instant)obj)._ticks) ||
                   ((obj is Period) && _ticks == ((Period)obj).Begin._ticks && _ticks == ((Period)obj).End._ticks);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return _ticks.GetHashCode() ^ 965800937;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> representation of this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> representation of this instance.</returns>
        public override String ToString()
        {
            return "TI[" + _ticks + "]";
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the distance to the other <see cref="Period" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The distance to the other <see cref="Period" /> instance.</returns>
        public Duration Distance(Period other)
        {
            if (_ticks == other.Begin.Ticks && _ticks == other.End.Ticks)
                return Duration.Zero;
            else if (_ticks < other.Begin._ticks)
                return new Duration(other.Begin._ticks - _ticks);
            else if (_ticks >= other.Begin._ticks && _ticks <= other.End._ticks)
                return Duration.Zero;
            else
                return new Duration(_ticks - other.End._ticks);
        }

        /// <summary>
        /// Determines the relative position compared to the other <see cref="Instant" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The <see cref="RelativePosition" /> of this instance compared to the other.</returns>
        private RelativePosition RelativePosition(Instant other)
        {
            if (_ticks < other._ticks)
                return Temporal.RelativePosition.Before;
            else if (_ticks > other._ticks)
                return Temporal.RelativePosition.After;
            else
                return Temporal.RelativePosition.Equals;
        }

        /// <summary>
        /// Determines the relative position compared to the other <see cref="Period" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The <see cref="RelativePosition" /> of this instance compared to the other.</returns>
        private RelativePosition RelativePosition(Period other)
        {
            if (_ticks == other.Begin.Ticks && _ticks == other.End.Ticks)
                return Temporal.RelativePosition.Equals;
            else if (_ticks < other.Begin._ticks)
                return Temporal.RelativePosition.Before;
            else if (_ticks == other.Begin._ticks)
                return Temporal.RelativePosition.Begins;
            else if (_ticks == other.End._ticks)
                return Temporal.RelativePosition.Ends;
            else if (_ticks > other.End._ticks)
                return Temporal.RelativePosition.After;
            else
                return Temporal.RelativePosition.During;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds a duration to an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>The sum of the instant and the duration.</returns>
        public static Instant operator +(Instant instant, Duration duration)
        {
            return new Instant(instant.Ticks + duration.Ticks);
        }

        /// <summary>
        /// Adds a duration to an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>The sum of the instant and the duration.</returns>
        public static Instant operator +(Instant instant, Int64 duration)
        {
            return new Instant(instant.Ticks + duration);
        }

        /// <summary>
        /// Extracts a duration from an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>The extract of the instant and the duration.</returns>
        public static Instant operator -(Instant instant, Duration duration)
        {
            return new Instant(instant.Ticks - duration.Ticks);
        }

        /// <summary>
        /// Extracts a duration from an instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>The extract of the instant and the duration.</returns>
        public static Instant operator -(Instant instant, Int64 duration)
        {
            return new Instant(instant.Ticks - duration);
        }

        /// <summary>
        /// Extracts two instants.
        /// </summary>
        /// <param name="firstInstant">The first instant.</param>
        /// <param name="secondInstant">The second instant.</param>
        /// <returns>The duration between the two instants.</returns>
        public static Duration operator -(Instant firstInstant, Instant secondInstant)
        {
            return new Duration(firstInstant._ticks - secondInstant._ticks);
        }

        /// <summary>
        /// Determines whether an instant is equal to another instant.
        /// </summary>
        /// <param name="firstInstant">The first instant.</param>
        /// <param name="secondInstant">The second instant.</param>
        /// <returns><c>true</c> if the first instant is equal to the second instant; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Instant firstInstant, Instant secondInstant)
        {
            return firstInstant._ticks == secondInstant._ticks;
        }

        /// <summary>
        /// Determines whether an instant is not equal to another instant.
        /// </summary>
        /// <param name="firstInstant">The first instant.</param>
        /// <param name="secondInstant">The second instant.</param>
        /// <returns><c>true</c> if the first instant is not equal to the second instant; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Instant firstInstant, Instant secondInstant)
        {
            return firstInstant._ticks != secondInstant._ticks;
        }

        /// <summary>
        /// Determines whether an instant is less than or equal to another instant.
        /// </summary>
        /// <param name="firstInstant">The first instant.</param>
        /// <param name="secondInstant">The second instant.</param>
        /// <returns><c>true</c> if the first instant is less than or equal to the second instant; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Instant firstInstant, Instant secondInstant)
        {
            return firstInstant._ticks <= secondInstant._ticks;
        }

        /// <summary>
        /// Determines whether an instant is geater than or equal to another instant.
        /// </summary>
        /// <param name="firstInstant">The first instant.</param>
        /// <param name="secondInstant">The second instant.</param>
        /// <returns><c>true</c> if the first instant is geater than or equal to the second instant; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Instant firstInstant, Instant secondInstant)
        {
            return firstInstant._ticks >= secondInstant._ticks;
        }

        /// <summary>
        /// Determines whether an instant is less than another instant.
        /// </summary>
        /// <param name="firstInstant">The first instant.</param>
        /// <param name="secondInstant">The second instant.</param>
        /// <returns><c>true</c> if the first instant is less than the second instant; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Instant firstInstant, Instant secondInstant)
        {
            return firstInstant._ticks < secondInstant._ticks;
        }

        /// <summary>
        /// Determines whether an instant is geater than another instant.
        /// </summary>
        /// <param name="firstInstant">The first instant.</param>
        /// <param name="secondInstant">The second instant.</param>
        /// <returns><c>true</c> if the first instant is geater than the second instant; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Instant firstInstant, Instant secondInstant)
        {
            return firstInstant._ticks > secondInstant._ticks;
        }

        #endregion

        #region Static conversion methods

        /// <summary>
        /// Converts the specified <see cref="Instant" /> instance to <see cref="System.Int64" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="System.Int64" /> representation of the the specified <see cref="Instant" /> instance.</returns>
        public static explicit operator Int64(Instant value)
        {
            return value._ticks;
        }

        /// <summary>
        /// Converts the specified <see cref="System.Int64" /> instance to <see cref="Instant" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="Instant" /> representation of the the specified <see cref="System.Int64" /> instance.</returns>
        public static explicit operator Instant(Int64 value)
        {
            return new Instant(value);
        }

        #endregion
    }
}
