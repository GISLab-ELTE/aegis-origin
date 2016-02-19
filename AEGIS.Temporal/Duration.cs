/// <copyright file="Duration.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.Temporal
{
    /// <summary>
    /// Represents a duration in time.
    /// </summary>
    public struct Duration : IEquatable<Duration>
    {
        #region Public static instances

        /// <summary>
        /// Represents a zero <see cref="Duration" />. This field is read-only.
        /// </summary>
        public static readonly Duration Zero = new Duration(0);

        /// <summary>
        /// Represents the minimum <see cref="Duration" />. This field is read-only.
        /// </summary>
        public static readonly Duration MinValue = new Duration(Int64.MinValue);

        /// <summary>
        /// Represents the maximum <see cref="Duration" />. This field is read-only.
        /// </summary>
        public static readonly Duration MaxValue = new Duration(Int64.MaxValue);

        #endregion

        #region Private fields

        private readonly Int64 _ticks;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the number of ticks that represent the value of the current <see cref="Duration" /> instance.
        /// </summary>
        /// <value>The number of ticks contained in this instance.</value>
        public Int64 Ticks { get { return _ticks; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Duration" /> struct.
        /// </summary>
        /// <param name="ticks">A time period expressed in ticks.</param>
        public Duration(Int64 ticks)
        {
            _ticks = ticks;
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="Duration" /> are equal.
        /// </summary>
        /// <param name="obj">Another <see cref="Duration" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Duration another)
        {
            return _ticks.Equals(another._ticks);
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

            return (obj is Duration && Equals((Duration)obj));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return _ticks.GetHashCode() ^ 965804401;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
          
            return "TD[" + _ticks + "]";
        }

        #endregion

        #region Operators

        /// <summary>
        /// Indicates whether the specified <see cref="Duration" /> instances are equal.
        /// </summary>
        /// <param name="first">The first duration.</param>
        /// <param name="second">The second duration.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Duration first, Duration second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Duration" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first duration.</param>
        /// <param name="second">The second duration.</param>
        /// <returns><c>true</c> if the instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Duration first, Duration second)
        {
            return !first.Equals(second);
        }

        #endregion
    }
}
