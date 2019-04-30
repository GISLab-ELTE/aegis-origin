/// <copyright file="SweepLineSegment.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Represents a Sweep line segment.
    /// </summary>
    public sealed class SweepLineSegment : IEquatable<SweepLineSegment>
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the edge associated with the segment.
        /// </summary>
        public Int32 Edge { get; set; }

        /// <summary>
        /// Gets or sets the left coordinate of the segment.
        /// </summary>
        public Coordinate LeftCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the right coordinate of the segment.
        /// </summary>
        public Coordinate RightCoordinate { get; set; }

        /// <summary>
        /// Gets the segment above this segment.
        /// </summary>
        public SweepLineSegment Above { get; set; }

        /// <summary>
        /// Gets the segment below this segment.
        /// </summary>
        public SweepLineSegment Below { get; set; }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified sweepline segment are equal.
        /// </summary>
        /// <param name="another">The sweepline segment to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(SweepLineSegment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Edge.Equals(other.Edge) && LeftCoordinate.Equals(other.LeftCoordinate) && RightCoordinate.Equals(other.RightCoordinate);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return (Equals(obj as SweepLineSegment));
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return Edge.GetHashCode() >> 4 ^ LeftCoordinate.GetHashCode() >> 2 ^ RightCoordinate.GetHashCode();
        }

        #endregion
    }
}
