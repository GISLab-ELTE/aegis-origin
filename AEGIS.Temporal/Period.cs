/// <copyright file="Period.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.Temporal
{
    /// <summary>
    /// Represents a period on the timeline.
    /// </summary>
    /// <remarks>
    /// The period is a one-dimensional geometric primitive that represents extent in time. The period is equivalent to a curve in space. 
    /// Like a curve, it is an open interval bounded by beginning and end points (instants), and has length (duration). 
    /// Its location in time is described by the temporal positions of the instants at which it begins and ends; its duration equals the temporal distance between those two temporal positions.
    /// </remarks>
    public struct Period : ITemporalPrimitive
    {
        #region Public static instances

        /// <summary>
        /// Represents the infinite period. This field is constant.
        /// </summary>
        public static readonly Period Infinity = new Period(Instant.MinValue, Instant.MaxValue);

        #endregion

        #region Private fields

        private readonly Instant _begin;
        private readonly Instant _end;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the beginning of the <see cref="Period" />.
        /// </summary>
        /// <value>The beginning of the period.</value>
        public Instant Begin { get { return _begin; } }

        /// <summary>
        /// Gets the end of the <see cref="Period" />.
        /// </summary>
        /// <value>The end of the period.</value>
        public Instant End { get { return _end; } }

        #endregion

        #region ITemporalPrimitive properties

        /// <summary>
        /// Gets the length of the <see cref="ITemporalPrimitive" />.
        /// </summary>
        /// <value>The length of the period.</value>
        public Duration Length
        {
            get { return new Duration(_end.Ticks - _begin.Ticks); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITemporalPrimitive" /> instance is valid.
        /// </summary>
        /// <value><c>true</c> if the order represents a valid temporal context; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return _begin.Ticks <= _end.Ticks; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Period" /> struct.
        /// </summary>
        /// <param name="begin">The beginning of the period.</param>
        /// <param name="end">The end of the period.</param>
        public Period(Instant begin, Instant end)
        {
            if (begin <= end)
            {
                _begin = begin;
                _end = end;
            }
            else
            {
                _begin = end;
                _end = begin;
            }
        }

        #endregion

        #region ITemporalPrimitive methods

        /// <summary>
        /// Computes the distance to the other <see cref="ITemporalPrimitive" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>
        /// The <see cref="Duration" /> representing the distance to the other <see cref="ITemporalPrimitive" /> instance.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">The instance is not valid.</exception>
        /// <exception cref="System.ArgumentNullException">The other instance is null.</exception>
        /// <exception cref="System.ArgumentException">The other instance is not valid.</exception>
        public Duration Distance(ITemporalPrimitive other)
        {
            if (!IsValid)
                throw new InvalidOperationException("The instance is not valid.");
            if (other == null)
                throw new ArgumentNullException("other", "The other instance is null.");
            if (!other.IsValid)
                throw new ArgumentException("The other instance is not valid.", "other");

            if (ReferenceEquals(other, this))
                return Duration.Zero;

            if (other is Instant)
            {
                return Distance((Instant)other);
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
        /// <exception cref="System.InvalidOperationException">The instance is not valid.</exception>
        /// <exception cref="System.ArgumentNullException">The other instance is null.</exception>
        /// <exception cref="System.ArgumentException">The other instance is not valid.</exception>
        public RelativePosition RelativePosition(ITemporalPrimitive other)
        {
            if (!IsValid)
                throw new InvalidOperationException("The instance is not valid.");
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

            return ((another is Instant) && _begin.Ticks == ((Instant)another).Ticks && _end.Ticks == ((Instant)another).Ticks) ||
                   ((another is Period) && _begin.Ticks == ((Period)another)._begin.Ticks && _end.Ticks == ((Period)another)._end.Ticks);
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

            return ((obj is Instant) && _begin.Ticks == ((Instant)obj).Ticks && _end.Ticks == ((Instant)obj).Ticks) ||
                   ((obj is Period) && _begin.Ticks == ((Period)obj)._begin.Ticks && _end.Ticks == ((Period)obj)._end.Ticks);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (_begin.GetHashCode() >> 2) ^ _end.GetHashCode() >> 2 ^ 965831401;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> representation of this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> representation of this instance.</returns>
        public override String ToString()
        {
            return "TP[" + _begin.Ticks + ":" + _end.Ticks + "]";
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the distance to the other <see cref="Instant" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The distance to the other <see cref="Instant" /> instance.</returns>
        public Duration Distance(Instant other)
        {
            if (other.Ticks < _begin.Ticks)
                return new Duration(_begin.Ticks - other.Ticks);
            else if (other.Ticks > _end.Ticks)
                return new Duration(other.Ticks - _end.Ticks);
            else
                return Duration.Zero;
        }

        /// <summary>
        /// Computes the distance to the other <see cref="Period" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The distance to the other <see cref="Period" /> instance.</returns>
        public Duration Distance(Period other)
        {
            if (other._end.Ticks < _begin.Ticks)
                return new Duration(_begin.Ticks - other._end.Ticks);
            else if (other._begin.Ticks < _end.Ticks)
                return new Duration(other._begin.Ticks - _end.Ticks);
            else
                return Duration.Zero;
        }

        /// <summary>
        /// Determines the relative position compared to the other <see cref="Instant" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The <see cref="RelativePosition" /> of this instance compared to the other.</returns>
        private RelativePosition RelativePosition(Instant other)
        {
            if (other.Ticks < _begin.Ticks)
                return Temporal.RelativePosition.Before;
            else if (other.Ticks == _begin.Ticks)
                return Temporal.RelativePosition.BegunBy;
            else if (other.Ticks == _end.Ticks)
                return Temporal.RelativePosition.EndedBy;
            else if (other.Ticks > _end.Ticks)
                return Temporal.RelativePosition.After;
            else
                return Temporal.RelativePosition.Contains;
        }

        /// <summary>
        /// Determines the relative position compared to the other <see cref="Period" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The <see cref="RelativePosition" /> of this instance compared to the other.</returns>
        private RelativePosition RelativePosition(Period other)
        {
            if (_end.Ticks < other._begin.Ticks)
                return Temporal.RelativePosition.Before;
            else if (_begin.Ticks < other._end.Ticks)
                return Temporal.RelativePosition.After;
            else if (_end.Ticks == other._begin.Ticks)
                return Temporal.RelativePosition.Meets;
            else if (_begin.Ticks == other._end.Ticks)
                return Temporal.RelativePosition.MetBy;
            else if (_begin.Ticks < other._begin.Ticks)
            {
                if (_end.Ticks > other._end.Ticks)
                    return Temporal.RelativePosition.Contains;
                else if (_end.Ticks == other._end.Ticks)
                    return Temporal.RelativePosition.EndedBy;
                else
                    return Temporal.RelativePosition.Overlaps;
            }
            else if (_begin.Ticks == other._begin.Ticks)
            {
                if (_end.Ticks < other._end.Ticks)
                    return Temporal.RelativePosition.Begins;
                else if (other._end.Ticks == _end.Ticks)
                    return Temporal.RelativePosition.Equals;
                else
                    return Temporal.RelativePosition.BegunBy;
            }
            else if (_begin.Ticks > other._begin.Ticks)
            {
                if (_end.Ticks < other._end.Ticks)
                    return Temporal.RelativePosition.During;
                else if (_end.Ticks == other._end.Ticks)
                    return Temporal.RelativePosition.Ends;
                else
                    return Temporal.RelativePosition.OverlappedBy;
            }
            else // execution should never come here
                return Temporal.RelativePosition.Undefined;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Indicates whether the specified <see cref="Period" /> instances are equal.
        /// </summary>
        /// <param name="first">The first period.</param>
        /// <param name="second">The second period.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Period first, Period second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Period" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first period.</param>
        /// <param name="second">The second period.</param>
        /// <returns><c>true</c> if the instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Period first, Period second)
        {
            return !first.Equals(second);
        }

        #endregion
    }
}
