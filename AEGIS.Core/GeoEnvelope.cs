/// <copyright file="GeoEnvelope.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using System.Linq;
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a geographic bounding envelope.
    /// </summary>
    [Serializable]
    public class GeoEnvelope : IEquatable<GeoEnvelope>
    {
        #region Static instances

        /// <summary>
        /// Represents the undefined <see cref="GeoEnvelope" /> value. This field is read-only.
        /// </summary>
        public static readonly GeoEnvelope Undefined = new GeoEnvelope(Angle.Undefined, Angle.Undefined, Angle.Undefined, Angle.Undefined, Length.Undefined, Length.Undefined);
        /// <summary>
        /// Represents the <see cref="GeoEnvelope" /> value for the globe. This field is read-only.
        /// </summary>
        public static readonly GeoEnvelope Globe = new GeoEnvelope(Angle.FromRadian(-Constants.PI / 2), Angle.FromRadian(Constants.PI / 2), Angle.FromRadian(-Constants.PI), Angle.FromRadian(Constants.PI), Length.NegativeInfinity, Length.PositiveInfinity);

        #endregion

        #region Private fields

        private readonly GeoCoordinate _minimum;
        private readonly GeoCoordinate _maximum;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the minimal coordinate.
        /// </summary>
        /// <value>The minimal coordinate.</value>
        public GeoCoordinate Minimum { get { return _minimum; } }

        /// <summary>
        /// Gets the maximal coordinate.
        /// </summary>
        /// <value>The maximal coordinate.</value>
        public GeoCoordinate Maximum { get { return _maximum; } }

        /// <summary>
        /// Indicates whether the extent of the instance is zero.
        /// </summary>
        /// <value><c>true</c> if the extent is zero; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return _minimum.Equals(_maximum); } }

        /// <summary>
        /// Indicates whether the instance has valid coordinates.
        /// </summary>
        /// <value><c>true</c> if all coordinates are numbers and within the globe; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return _minimum.IsValid && _maximum.IsValid; } }

        /// <summary>
        /// Indicates whether the instance has zero extent in the Z dimension.
        /// </summary>
        /// <value><c>true</c> if the instance has zero extent in the Z dimension; otherwise, <c>false</c>.</value>
        public Boolean IsPlanar { get { return _minimum.Height == _maximum.Height; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoEnvelope" /> struct.
        /// </summary>
        /// <param name="firstLatitude">The first latitude.</param>
        /// <param name="secondLatitude">The second latitude.</param>
        /// <param name="firstLongitude">The first longitude.</param>
        /// <param name="secondLongitude">The second longitude.</param>
        /// <param name="firstHeight">The first height.</param>
        /// <param name="secondHeight">The second height.</param>
        public GeoEnvelope(Angle firstLatitude, Angle secondLatitude, Angle firstLongitude, Angle secondLongitude, Length firstHeight, Length secondHeight)
        {
            _maximum = new GeoCoordinate(Angle.Min(firstLatitude, secondLatitude), Angle.Max(firstLongitude, secondLongitude), Length.Max(firstHeight, secondHeight));
            _minimum = new GeoCoordinate(Angle.Min(firstLatitude, secondLatitude), Angle.Min(firstLongitude, secondLongitude), Length.Min(firstHeight, secondHeight));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the envelope contains the specified geographic coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public Boolean Contains(GeoCoordinate coordinate)
        {
            return _minimum.Latitude <= coordinate.Latitude && coordinate.Latitude <= _maximum.Latitude &&
                   _minimum.Longitude <= coordinate.Longitude && coordinate.Longitude <= _maximum.Longitude &&
                   _minimum.Height <= coordinate.Height && coordinate.Height <= _maximum.Height;
        }

        /// <summary>
        /// Determines whether the instance contains another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="other" />; otherwise, <c>false</c>.</returns>
        public Boolean Contains(GeoEnvelope other)
        {
            return _minimum.Latitude <= other._minimum.Latitude && other._maximum.Latitude <= _maximum.Latitude &&
                   _minimum.Longitude <= other._minimum.Longitude && other._maximum.Longitude <= _maximum.Longitude &&
                   _minimum.Height <= other._minimum.Height && other._maximum.Height <= _maximum.Height;
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified <see cref="GeoEnvelope" /> are equal.
        /// </summary>
        /// <param name="another">The <see cref="GeoEnvelope" /> to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(GeoEnvelope another)
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

            return (obj is GeoEnvelope && _minimum == ((GeoEnvelope)obj)._minimum && _maximum == ((GeoEnvelope)obj)._maximum);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (_minimum.GetHashCode() >> 2) ^ _maximum.GetHashCode() ^ 190107161;
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

            return "(" + _minimum.Latitude + ", " + _minimum.Longitude + ", " + _minimum.Height + "; " +
                         _minimum.Latitude + ", " + _maximum.Longitude + ", " + _minimum.Height + "; " +
                         _maximum.Latitude + ", " + _maximum.Longitude + ", " + _minimum.Height + "; " +
                         _maximum.Latitude + ", " + _minimum.Longitude + ", " + _minimum.Height + "; " +
                         _minimum.Latitude + ", " + _minimum.Longitude + ", " + _maximum.Height + "; " +
                         _minimum.Latitude + ", " + _maximum.Longitude + ", " + _maximum.Height + "; " +
                         _maximum.Latitude + ", " + _maximum.Longitude + ", " + _maximum.Height + "; " +
                         _maximum.Latitude + ", " + _minimum.Longitude + ", " + _maximum.Height + ")";
        }

        #endregion
    }
}
