// <copyright file="Angle.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents an angle measure.
    /// </summary>
    [Serializable]
    public struct Angle : IEquatable<Angle>, IComparable<Angle>
    {
        #region Public static instances

        /// <summary>
        /// Represents the zero <see cref="Angle" /> value. This field is read-only.
        /// </summary>
        public static readonly Angle Zero = Angle.FromRadian(0);

        /// <summary>
        /// Represents the unknown <see cref="Angle" /> value. This field is read-only.
        /// </summary>
        public static readonly Angle Undefined = Angle.FromRadian(Double.NaN);

        /// <summary>
        /// Represents the negative infinite <see cref="Angle" /> value. This field is read-only.
        /// </summary>
        public static readonly Angle NegativeInfinity = Angle.FromRadian(Double.PositiveInfinity);

        /// <summary>
        /// Represents the positive infinite <see cref="Angle" /> value. This field is read-only.
        /// </summary>
        public static readonly Angle PositiveInfinity = Angle.FromRadian(Double.NegativeInfinity);

        /// <summary>
        /// Represents the smallest positive <see cref="Angle" /> value that is greater than zero. This field is read-only.
        /// </summary>
        public static readonly Angle Epsilon = Angle.FromRadian(Double.Epsilon);

        /// <summary>
        /// Represents the <see cref="Angle" /> value of a half circle. This field is read-only.
        /// </summary>
        public static readonly Angle HalfCircle = Angle.FromDegree(180);

        /// <summary>
        /// Represents the <see cref="Angle" /> value of a circle. This field is read-only.
        /// </summary>
        public static readonly Angle Circle = Angle.FromDegree(360);

        #endregion

        #region Private fields

        private readonly Double _value;
        private readonly UnitOfMeasurement _unit;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the value of the angle.
        /// </summary>
        public Double Value { get { return _value; } }

        /// <summary>
        /// Gets the base value (converted to radian) of the angle.
        /// </summary>
        public Double BaseValue { get { return _value * _unit.BaseMultiple; } }

        /// <summary>
        /// Gets the unit of measurement of the angle.
        /// </summary>
        public UnitOfMeasurement Unit { get { return _unit; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <exception cref="System.ArgumentException">The unit is not an angular measure.</exception>
        public Angle(Double value, UnitOfMeasurement unit)
        {
            if (unit != null && unit.Type != UnitQuantityType.Angle)
                throw new ArgumentException("The unit is not an angular measure.", "unit");

            _value = value;
            _unit = unit ?? UnitsOfMeasurement.Radian;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the value using the specified <see cref="UnitOfMeasurement" />.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The value converted to the specified <see cref="UnitOfMeasurement" />.</returns>
        /// <exception cref="System.ArgumentNullException">The unit of measurement is null.</exception>
        /// <exception cref="System.ArgumentException">The unit is not an angular measure.</exception>
        public Double GetValue(UnitOfMeasurement unit)
        {
            if (unit == null)
                throw new ArgumentNullException("unit", "The unit of measurement is null.");
            if (unit.Type != UnitQuantityType.Angle)
                throw new ArgumentException("The unit is not an angular measure.", "unit");

            if (unit == _unit)
                return _value;

            return BaseValue / unit.BaseMultiple;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A <see cref="System.String" /> containing the value and unit symbol of the instance.</returns>
        /// <exception cref="System.ArgumentNullException">The unit of measurement is null.</exception>
        /// <exception cref="System.ArgumentException">The unit is not an angular measure.</exception>
        public String ToString(UnitOfMeasurement unit)
        {
            if (unit == null)
                throw new ArgumentNullException("unit", "The unit of measurement is null.");

            if (unit.Type != UnitQuantityType.Angle)
                throw new ArgumentException("The unit is not an angular measure.", "unit");

            if (Unit != unit)
            {
                return (BaseValue / unit.BaseMultiple).ToString() + unit.Symbol;
            }
            else
            {
                return Value.ToString() + Unit.Symbol;
            }
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="Angle" /> are equal.
        /// </summary>
        /// <param name="another">Another <see cref="Angle" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Angle another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return BaseValue.Equals(another.BaseValue);
        }

        #endregion

        #region IComparable methods

        /// <summary>
        /// Compares the current instance with another <see cref="Angle" />.
        /// </summary>
        /// <param name="other">An angle to compare with this angle.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has the following meanings: Value Meaning Less than zero
        /// This object is less than the other parameter.Zero This object is equal to
        /// other. Greater than zero This object is greater than other.
        /// </returns>
        public Int32 CompareTo(Angle other)
        {
            return BaseValue.CompareTo(other.BaseValue);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return (obj is Angle) && BaseValue.Equals(((Angle)obj).BaseValue);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return BaseValue.GetHashCode() ^ 625911703;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the value and unit symbol of the instance.</returns>
        public override String ToString()
        {
            return Value.ToString() + Unit.Symbol;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Sums the specified <see cref="Angle" /> instances.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns>The sum of the two <see cref="Angle" /> instances.</returns>
        public static Angle operator +(Angle first, Angle second)
        {
            return new Angle(first.BaseValue + second.BaseValue, UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Negates the specified <see cref="Angle" /> instance.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The negate of the <see cref="Angle" /> instance.</returns>
        public static Angle operator -(Angle angle)
        {
            return new Angle(-angle.Value, angle.Unit);
        }

        /// <summary>
        /// Extracts the specified <see cref="Angle" /> instances.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns>The extract of the two <see cref="Angle" /> instances.</returns>
        public static Angle operator -(Angle first, Angle second)
        {
            return new Angle(first.BaseValue - second.BaseValue, UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Multiplies the <see cref="System.Double" /> scalar with the specified <see cref="Angle" />.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="angle">The angle.</param>
        /// <returns>The scalar multiplication of the <see cref="Angle" />.</returns>
        public static Angle operator *(Double scalar, Angle angle)
        {
            return new Angle(scalar * angle.Value, angle.Unit);
        }

        /// <summary>
        /// Multiplies the specified <see cref="Angle" /> with the <see cref="System.Double" /> scalar.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The scalar multiplication of the <see cref="Angle" />.</returns>
        public static Angle operator *(Angle angle, Double scalar)
        {
            return new Angle(angle.Value * scalar, angle.Unit);
        }

        /// <summary>
        /// Divides the specified <see cref="Angle" /> with the <see cref="System.Double" /> scalar.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The scalar division of the <see cref="Angle" />.</returns>
        public static Angle operator /(Angle angle, Double scalar)
        {
            return new Angle(angle.Value / scalar, angle.Unit);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Angle" /> instances are equal.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the two <see cref="Angle" /> instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Angle first, Angle second)
        {
            return first.BaseValue == second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Angle" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if two <see cref="Angle" /> instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Angle first, Angle second)
        {
            return first.BaseValue != second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Angle" /> instance is less than the second.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the first <see cref="Angle" /> instance is less than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Angle first, Angle second)
        {
            return first.BaseValue < second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Angle" /> instance is greater than the second.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the first <see cref="Angle" /> instance is greater than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Angle first, Angle second)
        {
            return first.BaseValue > second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Angle" /> instance is smaller or equal to the second.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the first <see cref="Angle" /> instance is smaller or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Angle first, Angle second)
        {
            return first.BaseValue <= second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Angle" /> instance is greater or equal to the second.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the first <see cref="Angle" /> instance is greater or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Angle first, Angle second)
        {
            return first.BaseValue >= second.BaseValue;
        }

        /// <summary>
        /// Converts the specified <see cref="Angle" /> instance to a <see cref="System.Double" /> value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="System.Double" /> value of the specified <see cref="Angle" /> instance.</returns>
        public static explicit operator Double(Angle value)
        {
            return value.BaseValue;
        }

        /// <summary>
        /// Converts the specified <see cref="System.Double" /> instance to a <see cref="Angle" /> value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="Angle" /> value of the specified <see cref="System.Double" /> instance.</returns>
        public static explicit operator Angle(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Radian);
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in <see cref="UnitsOfMeasurement.ArcMinute" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.ArcMinute" />.</returns>
        public static Angle FromArcMinute(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.ArcMinute);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in <see cref="UnitsOfMeasurement.ArcSecond" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.ArcSecond" />.</returns>
        public static Angle FromArcSecond(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.ArcSecond);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in <see cref="UnitsOfMeasurement.CentesimalMinute" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.CentesimalMinute" />.</returns>
        public static Angle FromCentesimalMinute(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.CentesimalMinute);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in <see cref="UnitsOfMeasurement.CentesimalSecond" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.CentesimalSecond" />.</returns>
        public static Angle FromCentesimalSecond(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.CentesimalSecond);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in <see cref="UnitsOfMeasurement.Degree" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.Degree" />.</returns>
        public static Angle FromDegree(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Degree);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in degrees and minutes.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="arcMinute">The arc minutes.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.Degree" />.</returns>
        public static Angle FromDegree(Int32 degree, Double arcMinute)
        {
            if (degree < 0)
                return new Angle(degree - arcMinute / 60.0, UnitsOfMeasurement.Degree);
            else
                return new Angle(degree + arcMinute / 60.0, UnitsOfMeasurement.Degree);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in degrees, minutes and seconds.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="arcMinute">The arc minutes.</param>
        /// <param name="arcSecond">The arc seconds.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in degrees, minutes and seconds.</returns>
        public static Angle FromDegree(Int32 degree, Int32 arcMinute, Double arcSecond)
        {
            if (degree < 0)
                return new Angle(degree - arcMinute / 60.0 - arcSecond / 3600, UnitsOfMeasurement.Degree);
            else
                return new Angle(degree + arcMinute / 60.0 + arcSecond / 3600, UnitsOfMeasurement.Degree);
        }

        /// <summary>
        /// Creates an <see cref="Angle" /> from value specified in <see cref="UnitsOfMeasurement.Gon" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.Gon" />.</returns>
        public static Angle FromGon(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Gon);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in <see cref="UnitsOfMeasurement.Grad" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.Grad" />.</returns>
        public static Angle FromGrad(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Grad);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in <see cref="UnitsOfMeasurement.MicroRadian" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.MicroRadian" />.</returns>
        public static Angle FromMicroRadian(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.MicroRadian);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct from the value specified in <see cref="UnitsOfMeasurement.Radian" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Angle" /> instance with the value specified in <see cref="UnitsOfMeasurement.Radian" />.</returns>
        public static Angle FromRadian(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Radian);
        }

        #endregion

        #region Public static query methods

        /// <summary>
        /// Determines whether the specified <see cref="Angle" /> instance is valid.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns><c>true</c> if <paramref name="angle" /> is not an unknown value; otherwise, <c>false</c>.</returns>
        public static Boolean IsValid(Angle angle)
        {
            return !Double.IsNaN(angle._value);
        }

        /// <summary>
        /// Determines the maximum of the specified <see cref="Angle" /> instances.
        /// </summary>
        /// <param name="angles">The angle values.</param>
        /// <returns>The <see cref="Angle" /> instance representing the maximum of <paramref name="angles" />.</returns>
        /// <exception cref="System.ArgumentException">There are no angles specified.</exception>
        public static Angle Max(params Angle[] angles)
        {
            if (angles == null || angles.Length == 0)
                throw new ArgumentException("There are no angles specified.", "angles");

            return new Angle(angles.Max(angle => angle.BaseValue), UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Determines the minimum of the specified <see cref="Angle" /> instances.
        /// </summary>
        /// <param name="angles">The angle values.</param>
        /// <returns>The <see cref="Angle" /> instance representing the minimum of <paramref name="angles" />.</returns>
        /// <exception cref="System.ArgumentException">There are no angles specified.</exception>
        public static Angle Min(params Angle[] angles)
        {
            if (angles == null || angles.Length == 0)
                throw new ArgumentException("There are no angles specified.", "angles");

            return new Angle(angles.Min(angle => angle.BaseValue), UnitsOfMeasurement.Radian);
        }

        #endregion
    }
}
