/// <copyright file="GeoVector.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a geographic vector.
    /// </summary>
    [Serializable]
    public struct GeoVector : IEquatable<GeoVector>
    {
        #region Static instances

        /// <summary>
        /// Represents the null <see cref="GeoVector" /> value. This field is constant.
        /// </summary>
        public static readonly GeoVector NullVector = new GeoVector(Angle.FromRadian(0), Length.FromMetre(0));

        #endregion

        #region Private fields

        private readonly Angle _azimuth;
        private readonly Length _length;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the azimuth of the vector.
        /// </summary>
        /// <value>The azimuth of a vector.</value>
        public Angle Azimuth { get { return _azimuth; } }

        /// <summary>
        /// Gets or sets the distance of the vector.
        /// </summary>
        /// <value>The distance of a vector.</value>
        public Length Distance { get { return _length; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="GeoVector" /> is null.
        /// </summary>
        /// <value><c>true</c> if the length of the vector are 0; otherwise, <c>false</c>.</value>
        public Boolean IsNull { get { return _length.Equals(Length.Zero); } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="GeoVector" /> is valid.
        /// </summary>
        /// <value><c>true</c> if the azimuth and length are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(_azimuth.Value) && !Double.IsNaN(_length.Value); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoVector" /> struct.
        /// </summary>
        /// <param name="azimuth">The azimuth.</param>
        /// <param name="length">The length.</param>
        public GeoVector(Angle azimuth, Length length)
        {
            if (length.Value < 0)
            {
                _azimuth = new Angle(-azimuth.Value % (2 * Constants.PI / azimuth.Unit.BaseMultiple), azimuth.Unit);
                _length = -length;
            }
            else
            {
                _azimuth = new Angle(azimuth.Value % (2 * Constants.PI / azimuth.Unit.BaseMultiple), azimuth.Unit);
                _length = length;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoVector" /> struct.
        /// </summary>
        /// <param name="azimuth">The azimuth (in radians).</param>
        /// <param name="length">The length (in metre).</param>
        public GeoVector(Double azimuth, Double length)
        {

            if (length < 0)
            {
                _azimuth = Angle.FromRadian(-azimuth % (2 * Constants.PI));
                _length = Length.FromMetre(-length);
            }
            else
            {
                _azimuth = Angle.FromRadian(azimuth % (2 * Constants.PI));
                _length = Length.FromMetre(length);
            }
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="GeoVector" /> are equal.
        /// </summary>
        /// <param name="obj">Another <see cref="GeoVector" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(GeoVector another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return _azimuth.Equals(another._azimuth) && _length.Equals(another._length);
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

            return (obj is GeoVector && Equals((GeoVector)obj));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (_azimuth.GetHashCode() >> 2) ^ _length.GetHashCode() ^ 57615137;
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

            return "(" + _azimuth + ", " + _length + ")";
        }

        #endregion

        #region Operators

        /// <summary>
        /// Indicates whether the specified <see cref="GeoVector" /> instances are equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(GeoVector first, GeoVector second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="GeoVector" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(GeoVector first, GeoVector second)
        {
            return !first.Equals(second);
        }

        #endregion
    }
}
