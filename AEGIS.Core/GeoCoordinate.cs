/// <copyright file="GeoCoordinate.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a geographic coordinate.
    /// </summary>
    [Serializable]
    public struct GeoCoordinate : IEquatable<GeoCoordinate>
    {
        #region Static instances

        /// <summary>
        /// Represents the empty <see cref="GeoCoordinate" /> value. This field is constant.
        /// </summary>
        public static readonly GeoCoordinate Empty = new GeoCoordinate(0, 0, 0);

        /// <summary>
        /// Represents the undefined <see cref="GeoCoordinate" /> value. This field is constant.
        /// </summary>
        public static readonly GeoCoordinate Undefined = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN);

        #endregion

        #region Private fields

        private readonly Angle _latitude;
        private readonly Angle _longitude;
        private readonly Length _height;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the geographic latitude.
        /// </summary>
        /// <value>The geographic latitude.</value>
        public Angle Latitude { get { return _latitude; } }

        /// <summary>
        /// Gets or sets the geographic longitude.
        /// </summary>
        /// <value>The geographic longitude.</value>
        public Angle Longitude { get { return _longitude; } }

        /// <summary>
        /// Gets or sets the geographic height.
        /// </summary>
        /// <value>The geographic height.</value>
        public Length Height { get { return _height; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="GeoCoordinate" /> is empty.
        /// </summary>
        /// <value><c>true</c> if all coordinates are <c>0</c>; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return _latitude.Equals(Angle.Zero) && _longitude.Equals(Angle.Zero) && _height.Equals(Length.Zero); } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="GeoCoordinate" /> is valid.
        /// </summary>
        /// <value><c>true</c> if all coordinates are numbers and within the globe; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(_longitude.Value) && !Double.IsNaN(_latitude.Value) && Math.Abs(_longitude.BaseValue) <= Constants.PI && Math.Abs(_latitude.BaseValue) <= Constants.PI / 2; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate" /> struct.
        /// </summary>
        /// <param name="latitude">The geographic latitude (in radians).</param>
        /// <param name="longitude">The geographic longitude (in radians).</param>
        public GeoCoordinate(Double latitude, Double longitude)
        {
            _latitude = new Angle(latitude, UnitsOfMeasurement.Radian);
            _longitude = new Angle(longitude, UnitsOfMeasurement.Radian);
            _height = Length.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate" /> struct.
        /// </summary>
        /// <param name="latitude">The geographic latitude (in radians).</param>
        /// <param name="longitude">The geographic longitude (in radians).</param>
        /// <param name="height">The height (in metre).</param>
        public GeoCoordinate(Double latitude, Double longitude, Double height)
        {
            _latitude = new Angle(latitude, UnitsOfMeasurement.Radian);
            _longitude = new Angle(longitude, UnitsOfMeasurement.Radian);
            _height = new Length(height, UnitsOfMeasurement.Metre);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate" /> struct.
        /// </summary>
        /// <param name="latitude">The geographic latitude.</param>
        /// <param name="longitude">The geographic longitude.</param>
        public GeoCoordinate(Angle latitude, Angle longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
            _height = Length.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate" /> struct.
        /// </summary>
        /// <param name="latitude">The geographic latitude.</param>
        /// <param name="longitude">The geographic longitude.</param>
        /// <param name="height">The height.</param>
        public GeoCoordinate(Angle latitude, Angle longitude, Length height)
        {
            _latitude = latitude;
            _longitude = longitude;
            _height = height;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Converts the coordinate to a valid globe value.
        /// </summary>
        /// <returns>The equivalent valid geographic coordinate.</returns>
        public GeoCoordinate ToGlobeValid()
        {
            return new GeoCoordinate(_latitude.BaseValue % Constants.PI / 2, _longitude.BaseValue % Constants.PI, _height.BaseValue);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="angularUnit">The angular unit of conversion.</param>
        /// <param name="lengthUnit">The length unit of conversion.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions in the specified units.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The angular unit of measurement is null.
        /// or
        /// The length unit of measurement is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The angular unit of measurement must be a angle quantity.
        /// or
        /// The length unit of measurement must be a length quantity.
        /// </exception>
        public String ToString(UnitOfMeasurement angularUnit, UnitOfMeasurement lengthUnit)
        {
            if (angularUnit == null)
                throw new ArgumentNullException("angularUnit", "The angular unit of measurement is null.");

            if (angularUnit.Type != UnitQuantityType.Angle)
                throw new ArgumentException("The angular unit of measurement must be a angle quantity.", "angularUnit");

            if (lengthUnit == null)
                throw new ArgumentNullException("lengthUnit", "The length unit of measurement is null.");

            if (lengthUnit.Type != UnitQuantityType.Length)
                throw new ArgumentException("The length unit of measurement must be a length quantity.", "lengthUnit");

            if (!IsValid)
                return "INVALID";

            if (IsEmpty)
                return "EMPTY";


            return "(" + _latitude.ToString(angularUnit) + ", " + _longitude.ToString(angularUnit) + ", " + _height.ToString(lengthUnit) + ")";
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="GeoCoordinate" /> are equal.
        /// </summary>
        /// <param name="obj">Another <see cref="GeoCoordinate" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(GeoCoordinate another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return _latitude.Equals(another._latitude) && _longitude.Equals(another._longitude) && _height.Equals(another._height);
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

            return (obj is GeoCoordinate && Equals((GeoCoordinate)obj));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (_latitude.GetHashCode() / 94541) ^ (_longitude.GetHashCode() / 5347) ^ _height.GetHashCode();
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

            return "(" + _latitude + ", " + _longitude + ", " + _height + ")";
        }        

        #endregion

        #region Operators

        /// <summary>
        /// Indicates whether the specified <see cref="GeoCoordinate" /> instances are equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two <see cref="GeoCoordinate" /> instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(GeoCoordinate x, GeoCoordinate y)
        {
            return x._latitude == y._latitude && x._longitude == y._longitude && x._height == y._height;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="GeoCoordinate" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two <see cref="GeoCoordinate" /> instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(GeoCoordinate x, GeoCoordinate y)
        {
            return x._latitude != y._latitude || x._longitude != y._longitude || x._height != y._height;
        }

        #endregion
    }
}
