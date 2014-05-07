/// <copyright file="Rational.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics
{
    /// <summary>
    /// Represents a rational number.
    /// </summary>
    public struct Rational : IComparable<Rational>, IComparable, IEquatable<Rational>
    {
        #region Static instances

        /// <summary>
        /// Represents the zero rational value. This field is read-only.
        /// </summary>
        public static readonly Rational Zero = new Rational(0, 1);

        #endregion

        #region Private fields

        private readonly Int32 _numerator;
        private readonly Int32 _denominator;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the numerator.
        /// </summary>
        /// <value>A 32-bit signed integer representing the numerator of the rational number.</value>
        public Int32 Numerator
        {
            get { return _numerator; }
        }

        /// <summary>
        /// Gets the denominator.
        /// </summary>
        /// <value>A 32-bit signed integer representing the denominator of the rational number.</value>
        public Int32 Denominator
        {
            get { return _denominator; }
        }

        /// <summary>
        /// Gets the inverted value of the rational number.
        /// </summary>
        /// <value>The inverse rational number.</value>
        /// <exception cref="System.DivideByZeroException">The value of the rational number is 0.</exception>
        public Rational Inverse
        {
            get
            {
                if (_numerator == 0)
                {
                    throw new DivideByZeroException("The value of the rational number is 0.");
                }
                return new Rational(_denominator, _numerator);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Rational" /> struct.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <exception cref="System.ArgumentException">The denominator is 0.;denominator</exception>
        public Rational(Int32 numerator, Int32 denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("The denominator is 0.", "denominator");
            }

            if (numerator == 0)
            {
                _numerator = 0;
                _denominator = 1;
            }
            else
            {
                Int32 sign = ((numerator > 0 && denominator > 0) || (numerator < 0 && denominator < 0)) ? 1 : -1;
                _numerator = sign * Math.Abs(numerator);
                _denominator = Math.Abs(denominator);
                Int32 divisor = Calculator.GreatestCommonDivisor(_numerator, _denominator);
                _numerator /= divisor;
                _denominator /= divisor;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Rational" /> struct.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        public Rational(Int32 numerator) : this(numerator, 1) { }

        #endregion

        #region IComparable methods

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj" />. Zero This instance is equal to <paramref name="obj" />. Greater than zero This instance is greater than <paramref name="obj" />.</returns>
        public Int32 CompareTo(Rational other)
        {
            if (ReferenceEquals(other, this)) return 0;

            return (1.0 * _numerator / _denominator).CompareTo(1.0 * other._numerator / other._denominator);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="other" />. Zero This instance is equal to <paramref name="obj" />. Greater than zero This instance is greater than <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">obj;The object is null.</exception>
        /// <exception cref="System.ArgumentException">The object s not comparable with a rational number.;obj</exception>
        public Int32 CompareTo(Object obj)
        {
            if (ReferenceEquals(obj, null)) throw new ArgumentNullException("obj", "The object is null.");
            if (ReferenceEquals(obj, this)) return 0;

            if (obj is Rational)
                return CompareTo((Rational)obj);

            if (obj is IComparable)
            {
                return (obj as IComparable).CompareTo(1.0 * _numerator / _denominator);
            }

            throw new ArgumentException("The object s not comparable with a rational number.", "obj");
        }


        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other rational number are equal.
        /// </summary>
        /// <param name="another">Another rational number to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Rational other)
        {
            if (ReferenceEquals(other, this)) return true;

            return (1.0 * _numerator / _denominator).Equals(1.0 * other._numerator / other._denominator);
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

            if (obj is Rational)
                return Equals((Rational)obj);

            return obj.Equals((Single)_numerator / _denominator);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return (_numerator.GetHashCode() >> 2) ^ _denominator.GetHashCode() ^ 295200001;
            }
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the value and unit symbol of the instance.</returns>
        public override String ToString()
        {
            if (_numerator == 0)
            {
                return "0";
            }
            if (_denominator == 1)
            {
                return _numerator.ToString();
            }
            return _numerator + "/" + _denominator;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts the specified value to a rational number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rational equivalent of <paramref name="value" />.</returns>
        static public explicit operator Rational(Int32 value)
        {
            return new Rational(value);
        }

        /// <summary>
        /// Converts the specified value to a rational number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rational equivalent of <paramref name="value" />.</returns>
        static public explicit operator Rational(Single value)
        {
            return new Rational((Int32)(value * (1 << 20)), (1 << 20));
        }

        /// <summary>
        /// Converts the specified value to a rational number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rational equivalent of <paramref name="value" />.</returns>
        static public explicit operator Rational(Double value)
        {
            return new Rational((Int32)(value * (1 << 20)), (1 << 20));
        }

        /// <summary>
        /// Converts the specified value to a 32 bit signed integer.
        /// </summary>
        /// <param name="rational">The value.</param>
        /// <returns>The 32 bit signed integer equivalent of <paramref name="rational" />.</returns>
        static public explicit operator Int32(Rational value)
        {
            return value._numerator / value._denominator;
        }

        /// <summary>
        /// Converts the specified value to a 64 bit signed integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The 64 bit signed integer equivalent of <paramref name="value" />.</returns>
        static public explicit operator Int64(Rational value)
        {
            return value._numerator / value._denominator;
        }

        /// <summary>
        /// Converts the specified value to a single precision floating point number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The single precision floating point number equivalent of <paramref name="value" />.</returns>
        static public explicit operator Single(Rational value)
        {
            return (Single)value._numerator / value._denominator;
        }

        /// <summary>
        /// Converts the specified value to a double precision floating point number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The double precision floating point number equivalent of <paramref name="value" />.</returns>
        static public explicit operator Double(Rational value)
        {
            return (Double)value._numerator / value._denominator;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Rational" /> instances are equal.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Rational first, Rational second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Rational" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Rational first, Rational second)
        {
            return !first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is less than the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is less than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Rational first, Rational second)
        {
            return first.CompareTo(second) < 0;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is greater than the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is greater than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Rational first, Rational second)
        {
            return first.CompareTo(second) > 0;

        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is smaller or equal to the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is smaller or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Rational first, Rational second)
        {
            return first.CompareTo(second) <= 0;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is greater or equal to the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is greater or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Rational first, Rational second)
        {
            return first.CompareTo(second) >= 0;

        }

        /// <summary>
        /// Sums the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The sum of the two <see cref="Rational" /> instances.</returns>
        public static Rational operator +(Rational first, Rational second)
        {
            if (first.Equals(Rational.Zero)) { return second; }
            if (second.Equals(Rational.Zero)) { return first; }
            if (first.Denominator == second.Denominator)
            {
                return new Rational(first.Numerator + second.Numerator, first.Denominator);
            }
            return new Rational(first.Numerator * second.Denominator + second.Numerator * first.Denominator, first.Denominator * second.Denominator);
        }

        /// <summary>
        /// Sums the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="integer">The integer.</param>
        /// <returns>The sum of the rational and integer numbers.</returns>
        public static Rational operator +(Rational rational, Int32 integer)
        {
            if (rational.Equals(Rational.Zero)) { return new Rational(integer); }
            if (integer == 0) { return rational; }
            return new Rational(rational.Numerator + integer * rational.Denominator, rational.Denominator);
        }

        /// <summary>
        /// Sums the specified rational and integer numbers.
        /// </summary>
        /// <param name="integer">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The sum of the rational and integer numbers.</returns>
        public static Rational operator +(Int32 integer, Rational rational)
        {
            if (rational.Equals(Rational.Zero)) { return new Rational(integer); }
            if (integer == 0) { return rational; }
            return new Rational(rational.Numerator + integer * rational.Denominator, rational.Denominator);
        }

        /// <summary>
        /// Extracts the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The extract of the two <see cref="Rational" /> instances.</returns>
        public static Rational operator -(Rational first, Rational second)
        {
            if (first.Equals(Rational.Zero))
            {
                return -second;
            }

            if (second.Equals(Rational.Zero))
            {
                return first;
            }
            if (first.Denominator == second.Denominator)
            {
                return new Rational(first.Numerator - second.Numerator, first.Denominator);
            }
            return new Rational(first.Numerator * second.Denominator - second.Numerator * first.Denominator, first.Denominator * second.Denominator);
        }

        /// <summary>
        /// Extracts the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="integer">The integer.</param>
        /// <returns>The extract of the rational and integer numbers.</returns>
        public static Rational operator -(Rational rational, Int32 integer)
        {
            if (rational.Equals(Rational.Zero)) { return new Rational(integer); }
            if (integer == 0) { return rational; }
            return new Rational(rational.Numerator - integer * rational.Denominator, rational.Denominator);
        }

        /// <summary>
        /// Extracts the specified rational and integer numbers.
        /// </summary>
        /// <param name="integer">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The extract of the rational and integer numbers.</returns>
        public static Rational operator -(Int32 integer, Rational rational)
        {
            if (rational.Equals(Rational.Zero)) { return new Rational(integer); }
            if (integer == 0) { return rational; }
            return new Rational(rational.Numerator - integer * rational.Denominator, rational.Denominator);
        }

        /// <summary>
        /// Inverts the specified <see cref="Rational" />.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <returns>The inverted <see cref="Rational" />.</returns>
        public static Rational operator -(Rational rational)
        {
            return new Rational(-1 * rational.Numerator, rational.Denominator);
        }

        /// <summary>
        /// Multiplies the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The product of the specified <see cref="Rational" /> instances.</returns>
        public static Rational operator *(Rational first, Rational second)
        {
            if (first.Equals(Rational.Zero) || second.Equals(Rational.Zero))
            {
                return Rational.Zero;
            }
            return new Rational(first.Numerator * second.Numerator, first.Denominator * second.Denominator);
        }

        /// <summary>
        /// Multiplies the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="integer">The integer.</param>
        /// <returns>The product of the rational and integer numbers.</returns>
        public static Rational operator *(Rational rational, Int32 integer)
        {
            if (rational.Equals(Rational.Zero) || integer == 0)
            {
                return Rational.Zero;
            }
            return new Rational(rational.Numerator * integer, rational.Denominator);
        }

        /// <summary>
        /// Multiplies the specified rational and integer numbers.
        /// </summary>
        /// <param name="integer">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The product of the rational and integer numbers.</returns>
        public static Rational operator *(Int32 integer, Rational rational)
        {
            if (rational.Equals(Rational.Zero) || integer == 0)
            {
                return Rational.Zero;
            }
            return new Rational(rational.Numerator * integer, rational.Denominator);
        }

        /// <summary>
        /// Divides the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The quotient of the specified <see cref="Rational" /> instances.</returns>
        public static Rational operator /(Rational first, Rational second)
        {
            if (first.Equals(Rational.Zero))
            {
                return Rational.Zero;
            }
            if (second.Equals(Rational.Zero))
            {
                throw new DivideByZeroException();
            }
            return new Rational(first.Numerator * second.Denominator, first.Denominator * second.Numerator);
        }

        /// <summary>
        /// Divides the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="integer">The integer.</param>
        /// <returns>The quotient of the rational and integer numbers.</returns>
        public static Rational operator /(Rational rational, Int32 integer)
        {
            if (rational.Equals(Rational.Zero))
            {
                return Rational.Zero;
            }
            if (integer == 0)
            {
                throw new DivideByZeroException();
            }
            return new Rational(rational.Numerator, rational.Denominator * integer);
        }

        /// <summary>
        /// Divides the specified rational and integer numbers.
        /// </summary>
        /// <param name="integer">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The quotient of the rational and integer numbers.</returns>
        public static Rational operator /(Int32 integer, Rational rational)
        {
            if (integer == 0)
            {
                return Rational.Zero;
            }
            if (rational.Equals(Rational.Zero))
            {
                throw new DivideByZeroException();
            }
            return new Rational(rational.Denominator * integer, rational.Numerator);
        }

        #endregion
    }
}
