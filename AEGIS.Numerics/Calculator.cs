/// <copyright file="Calculator.cs" company="Eötvös Loránd University (ELTE)">
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
/// <authors>Roberto Giachetta, Ákos Horváth, Dóra Papp</authors>

using System;

namespace ELTE.AEGIS.Numerics
{
    /// <summary>
    /// Defines basic mathematical calculations.
    /// </summary>
    public static class Calculator
    {
        #region Public constants

        /// <summary>
        /// Represents the general tolerance value for mathematical calculations. This field is constant.
        /// </summary>
        public const Double Tolerance = 1e-10;

        /// <summary>
        /// Represents the general limit for iterative operation. This field is constant.
        /// </summary>
        public const Double IterationLimit = 1000;

        #endregion

        #region Private static variables

        /// <summary>
        /// An array containing the factorial of the first 170 integers.
        /// </summary>
        private static Double[] _factorialCacheArray;

        /// <summary>
        /// A boolean indicating whether the Factorial method has been called at least once.
        /// </summary>
        private static Boolean _isFactorialComputed = false;

        #endregion

        #region General methods

        /// <summary>
        /// Return the fraction part of a single precision floating point value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The fraction part of a single precision floating point value.</returns>
        public static Single Fraction(Single value)
        {
            if (value < 0)
                return Convert.ToSingle(value + Math.Ceiling(value));

            return Convert.ToSingle(value - Math.Floor(value));
        }

        /// <summary>
        /// Return the fraction part of a double precision floating point value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The fraction part of a double precision floating point value.</returns>
        public static Double Fraction(Double value)
        { 
            if (value < 0)
                return value + Math.Ceiling(value);

            return value - Math.Floor(value);
        }

        #endregion

        #region Extrema computation methods

        /// <summary>
        /// Returns the absolute maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static SByte AbsMax(params SByte[] values)
        {
            if (values == null || values.Length == 0)
                return SByte.MaxValue;

            SByte max = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) > max) max = Math.Abs(values[i]);

            return max;
        }

        /// <summary>
        /// Returns the absolute maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Int16 AbsMax(params Int16[] values)
        {
            if (values == null || values.Length == 0)
                return Int16.MaxValue;

            Int16 max = Math.Abs(values[0]);
            for (Int32 i = 0; i < values.Length; i++)
                if (Math.Abs(values[i]) > max) max = Math.Abs(values[i]);

            return max;
        }

        /// <summary>
        /// Returns the absolute maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Int32 AbsMax(params Int32[] values)
        {
            if (values == null || values.Length == 0)
                return Int32.MaxValue;

            Int32 max = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) > max) max = Math.Abs(values[i]);

            return max;
        }

        /// <summary>
        /// Returns the absolute maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Int64 AbsMax(params Int64[] values)
        {
            if (values == null || values.Length == 0)
                return Int64.MaxValue;

            Int64 max = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) > max) max = Math.Abs(values[i]);

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Byte AbsMax(params Byte[] values)
        {
            if (values == null || values.Length == 0)
                return Byte.MaxValue;

            Byte max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static UInt16 AbsMax(params UInt16[] values)
        {
            if (values == null || values.Length == 0)
                return UInt16.MaxValue;

            UInt16 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static UInt32 AbsMax(params UInt32[] values)
        {
            if (values == null || values.Length == 0)
                return UInt32.MaxValue;

            UInt32 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static UInt64 AbsMax(params UInt64[] values)
        {
            if (values == null || values.Length == 0)
                return UInt64.MaxValue;

            UInt64 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the absolute maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Single AbsMax(params Single[] values)
        {
            if (values == null || values.Length == 0)
                return Single.PositiveInfinity;

            Single max = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) > max) max = Math.Abs(values[i]);

            return max;
        }

        /// <summary>
        /// Returns the absolute maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Double AbsMax(params Double[] values)
        {
            if (values == null || values.Length == 0)
                return Double.PositiveInfinity;

            Double max = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) > max) max = Math.Abs(values[i]);

            return max;
        }

        /// <summary>
        /// Returns the absolute maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Decimal AbsMax(params Decimal[] values)
        {
            if (values == null || values.Length == 0)
                return Decimal.MaxValue;

            Decimal max = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) > max) max = Math.Abs(values[i]);

            return max;
        }

        /// <summary>
        /// Returns the absolute minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static SByte AbsMin(params SByte[] values)
        {
            if (values == null || values.Length == 0)
                return SByte.MinValue;

            SByte min = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the absolute minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Int16 AbsMin(params Int16[] values)
        {
            if (values == null || values.Length == 0)
                return Int16.MinValue;

            Int16 min = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the absolute minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Int32 AbsMin(params Int32[] values)
        {
            if (values == null || values.Length == 0)
                return Int32.MinValue;

            Int32 min = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the absolute minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Int64 AbsMin(params Int64[] values)
        {
            if (values == null || values.Length == 0)
                return Int64.MinValue;

            Int64 min = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the absolute minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Single AbsMin(params Single[] values)
        {
            if (values == null || values.Length == 0)
                return Single.NegativeInfinity;

            Single min = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the absolute minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Double AbsMin(params Double[] values)
        {
            if (values == null || values.Length == 0)
                return Double.NegativeInfinity;

            Double min = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the absolute minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The absolute minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Decimal AbsMin(params Decimal[] values)
        {
            if (values == null || values.Length == 0)
                return Decimal.MinValue;

            Decimal min = Math.Abs(values[0]);
            for (Int32 i = 1; i < values.Length; i++)
                if (Math.Abs(values[i]) < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Byte AbsMin(params Byte[] values)
        {
            if (values == null || values.Length == 0)
                return Byte.MinValue;

            Byte min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static UInt16 AbsMin(params UInt16[] values)
        {
            if (values == null || values.Length == 0)
                return UInt16.MinValue;

            UInt16 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static UInt32 AbsMin(params UInt32[] values)
        {
            if (values == null || values.Length == 0)
                return UInt32.MinValue;

            UInt32 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static UInt64 AbsMin(params UInt64[] values)
        {
            if (values == null || values.Length == 0)
                return UInt64.MinValue;

            UInt64 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static SByte Max(params SByte[] values)
        {
            if (values == null || values.Length == 0)
                return SByte.MaxValue;

            SByte max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Int16 Max(params Int16[] values)
        {
            if (values == null || values.Length == 0)
                return Int16.MaxValue;

            Int16 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Int32 Max(params Int32[] values)
        {
            if (values == null || values.Length == 0)
                return Int32.MaxValue;

            Int32 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Int64 Max(params Int64[] values)
        {
            if (values == null || values.Length == 0)
                return Int64.MaxValue;

            Int64 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Byte Max(params Byte[] values)
        {
            if (values == null || values.Length == 0)
                return Byte.MaxValue;

            Byte max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static UInt16 Max(params UInt16[] values)
        {
            if (values == null || values.Length == 0)
                return UInt16.MaxValue;

            UInt16 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static UInt32 Max(params UInt32[] values)
        {
            if (values == null || values.Length == 0)
                return UInt32.MaxValue;

            UInt32 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static UInt64 Max(params UInt64[] values)
        {
            if (values == null || values.Length == 0)
                return UInt64.MaxValue;

            UInt64 max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Single Max(params Single[] values)
        {
            if (values == null || values.Length == 0)
                return Single.PositiveInfinity;

            Single max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Double Max(params Double[] values)
        {
            if (values == null || values.Length == 0)
                return Double.PositiveInfinity;

            Double max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the maximum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The maximum of the specified values, or the largest possible value of the type if none are specified.</returns>
        public static Decimal Max(params Decimal[] values)
        {
            if (values == null || values.Length == 0)
                return Decimal.MaxValue;

            Decimal max = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] > max) max = values[i];

            return max;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static SByte Min(params SByte[] values)
        {
            if (values == null || values.Length == 0)
                return SByte.MinValue;

            SByte min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Int16 Min(params Int16[] values)
        {
            if (values == null || values.Length == 0)
                return Int16.MinValue;

            Int16 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Int32 Min(params Int32[] values)
        {
            if (values == null || values.Length == 0)
                return Int32.MinValue;

            Int32 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Int64 Min(params Int64[] values)
        {
            if (values == null || values.Length == 0)
                return Int64.MinValue;

            Int64 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Byte Min(params Byte[] values)
        {
            if (values == null || values.Length == 0)
                return Byte.MinValue;

            Byte min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static UInt16 Min(params UInt16[] values)
        {
            if (values == null || values.Length == 0)
                return UInt16.MinValue;

            UInt16 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static UInt32 Min(params UInt32[] values)
        {
            if (values == null || values.Length == 0)
                return UInt32.MinValue;

            UInt32 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static UInt64 Min(params UInt64[] values)
        {
            if (values == null || values.Length == 0)
                return UInt64.MinValue;

            UInt64 min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Single Min(params Single[] values)
        {
            if (values == null || values.Length == 0)
                return Single.NegativeInfinity;

            Single min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Double Min(params Double[] values)
        {
            if (values == null || values.Length == 0)
                return Double.NegativeInfinity;

            Double min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        /// <summary>
        /// Returns the minimum of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The minimum of the specified values, or the smallest possible value of the type if none are specified.</returns>
        public static Decimal Min(params Decimal[] values)
        {
            if (values == null || values.Length == 0)
                return Decimal.MinValue;

            Decimal min = values[0];
            for (Int32 i = 1; i < values.Length; i++)
                if (values[i] < min) min = values[i];

            return min;
        }

        #endregion

        #region Factorial calculation methods

        /// <summary>
        /// Calculates the factorial of a specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The factorial of <param name="value">.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The value is less than 0.</exception>
        public static Double Factorial(Int32 value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value", "The value is less than 0.");

            if (value > 170)
                return Double.PositiveInfinity;

            if (_isFactorialComputed)
            {
                return _factorialCacheArray[value];
            }

            _factorialCacheArray = new Double[170];
            _factorialCacheArray[0] = 1;

            Double result = 1;
            for (int i = 1; i < 170; i++)
            {
                result = result * i;
                _factorialCacheArray[i] = result;
            }
            _isFactorialComputed = true;

            return _factorialCacheArray[value];
        }

        /// <summary>
        /// Calculates the Gamma function of a specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The Gamma function value.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The value is less than or equal to 0.</exception>
        public static Double Gamma(Double value)
        {

            if (value <= 0 && Math.Ceiling(value) == Math.Floor(value))
                throw new ArgumentOutOfRangeException("value", "The value is less than or equal to 0.");

            Double[] p = { 0.99999999999980993, 676.5203681218851, -1259.1392167224028,
                           771.32342877765313, -176.61502916214059, 12.507343278686905,
                           -0.13857109526572012, 9.9843695780195716e-6, 1.5056327351493116e-7 };
            const Int32 g = 7;
            const Int32 n = 9;
            if (value < 0.5)
                return Math.PI / (Math.Sin(Math.PI * value) * Gamma(1 - value));

            value = value - 1;

            Double a = p[0];
            for (Int32 i = 1; i < n; i++)
            {
                a += p[i] / (value + i);
            }

            return Math.Sqrt(2 * Math.PI) * Math.Pow(value + g + 0.5, value + 0.5) * Math.Exp(-(value + g + 0.5)) * a;
        }

        #endregion

        #region Summation methods

        /// <summary>
        /// Computes the sum of values in an interval.
        /// </summary>
        /// <param name="intervalStart">The start of the interval.</param>
        /// <param name="intervalEnd">The end of the interval.</param>
        /// <returns>The sum of values in the interval between <paramref name="intervalStart" /> and <paramref name="intervalEnd" />.</returns>
        public static Int32 Sum(Int32 intervalStart, Int32 intervalEnd)
        {
            Int32 sum = 0;
            for (Int32 i = intervalStart; i <= intervalEnd; i++)
                sum += i;
            return sum;
        }

        /// <summary>
        /// Computes the sum of values in an interval.
        /// </summary>
        /// <param name="intervalStart">The start of the interval.</param>
        /// <param name="intervalEnd">The end of the interval.</param>
        /// <returns>The sum of values in the interval between <paramref name="intervalStart" /> and <paramref name="intervalEnd" />.</returns>
        public static Int64 Sum(Int64 intervalStart, Int64 intervalEnd)
        {
            Int64 sum = 0;
            for (Int64 i = intervalStart; i <= intervalEnd; i++)
                sum += i;
            return sum;
        }

        /// <summary>
        /// Computes the sum of function values in an interval.
        /// </summary>
        /// <param name="intervalStart">The start of the interval.</param>
        /// <param name="intervalEnd">The end of the interval.</param>
        /// <param name="function">A function.</param>
        /// <returns>The sum of values produced by <paramref name="function" /> in the interval between <paramref name="intervalStart" /> and <paramref name="intervalEnd" />.</returns>
        public static Int32 Sum(Int32 intervalStart, Int32 intervalEnd, Func<Int32, Int32> function)
        {
            Int32 sum = 0;
            for (Int32 i = intervalStart; i <= intervalEnd; i++)
                sum += function(i);
            return sum;
        }

        /// <summary>
        /// Computes the sum of function values in an interval.
        /// </summary>
        /// <param name="intervalStart">The start of the interval.</param>
        /// <param name="intervalEnd">The end of the interval.</param>
        /// <param name="function">A function.</param>
        /// <returns>The sum of values produced by <paramref name="function" /> in the interval between <paramref name="intervalStart" /> and <paramref name="intervalEnd" />.</returns>
        public static Int64 Sum(Int32 intervalStart, Int32 intervalEnd, Func<Int32, Int64> function)
        {
            Int64 sum = 0;
            for (Int32 i = intervalStart; i <= intervalEnd; i++)
                sum += function(i);
            return sum;
        }

        /// <summary>
        /// Computes the sum of function values in an interval.
        /// </summary>
        /// <param name="intervalStart">The start of the interval.</param>
        /// <param name="intervalEnd">The end of the interval.</param>
        /// <param name="function">A function.</param>
        /// <returns>The sum of values produced by <paramref name="function" /> in the interval between <paramref name="intervalStart" /> and <paramref name="intervalEnd" />.</returns>
        public static Single Sum(Int32 intervalStart, Int32 intervalEnd, Func<Int32, Single> function)
        {
            Single sum = 0;
            for (Int32 i = intervalStart; i <= intervalEnd; i++)
                sum += function(i);
            return sum;
        }

        /// <summary>
        /// Computes the sum of function values in an interval.
        /// </summary>
        /// <param name="intervalStart">The start of the interval.</param>
        /// <param name="intervalEnd">The end of the interval.</param>
        /// <param name="function">A function.</param>
        /// <returns>The sum of values produced by <paramref name="function" /> in the interval between <paramref name="intervalStart" /> and <paramref name="intervalEnd" />.</returns>
        public static Double Sum(Int32 intervalStart, Int32 intervalEnd, Func<Int32, Double> function)
        {
            Double sum = 0;
            for (Int32 i = intervalStart; i <= intervalEnd; i++)
                sum += function(i);
            return sum;
        }

        /// <summary>
        /// Returns the square of a number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>The square of the number.</returns>
        public static Double Square(Double number)
        {
            return number * number;
        }

        /// <summary>
        /// Returns a number raised to the specified power.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="exponent">The exponent.</param>
        /// <returns>The number raised to the specified power.</returns>
        public static Int64 Pow(Int64 number, Int32 exponent)
        {
            Int64 result = 1;

            if (exponent >= 0)
            {
                while (exponent > 0)
                {
                    if (exponent % 2 == 1)
                        result *= number;
                    exponent >>= 1;
                    number *= number;
                }
            }
            else
            {
                exponent = -exponent;
                while (exponent > 0)
                {
                    if (exponent % 2 == 1)
                        result *= number;
                    exponent >>= 1;
                    number *= number;
                }
                result = 1 / result;
            }

            return result;
        }

        /// <summary>
        /// Returns a number raised to the specified power.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="exponent">The exponent.</param>
        /// <returns>The number raised to the specified power.</returns>
        public static Double Pow(Double number, UInt32 exponent)
        {
            Double result = 1.0;
            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                    result *= number;
                exponent >>= 1;
                number *= number;
            }

            return result;
        }

        /// <summary>
        /// Returns a number raised to the specified power.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="exponent">The exponent.</param>
        /// <returns>The number raised to the specified power.</returns>
        public static Double Pow(Double number, Int32 exponent)
        {
            Double result = 1.0;

            if (exponent >= 0)
            {
                while (exponent > 0)
                {
                    if (exponent % 2 == 1)
                        result *= number;
                    exponent >>= 1;
                    number *= number;
                }
            }
            else
            {
                exponent = -exponent;
                while (exponent > 0)
                {
                    if (exponent % 2 == 1)
                        result *= number;
                    exponent >>= 1;
                    number *= number;
                }
                result = 1 / result;
            }

            return result;
        }

        #endregion

        #region Algebra methods

        /// <summary>
        /// Computes the binomial coefficient indexed by two nonnegative integers.
        /// </summary>
        /// <param name="n">The first coefficient.</param>
        /// <param name="k">The second coefficient.</param>
        /// <returns>The binomial coefficient indexed by the two specified nonnegative integers</returns>
        /// <exception cref="ArgumentOutOfRangeException">The first argument is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The second argument is less than zero.</exception>
        public static Double Binomial(Int32 n, Int32 k)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException("n", "The first argument is less than zero.");
            if (k < 0)
                throw new ArgumentOutOfRangeException("k", "The second argument is less than zero.");

            Double result;

            if (k > n)
                result = 0;

            else
            {
                if ((n - k) < k)
                    k = n - k;

                result = 1;

                for (Int32 i = 1; i <= k; ++i)
                {
                    result = (result * (n + 1 - i)) / i;
                }
            }

            return result;
        }

        #endregion

        #region Trigonometry methods

        /// <summary>
        /// Returns the secant of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The secant of the value.</returns>
        public static Double Sec(Double value)
        {
            return 1 / Math.Cos(value);
        }

        /// <summary>
        /// Returns the cosecant of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The cosecant of the value.</returns>
        public static Double Csc(Double value)
        {
            return 1 / Math.Sin(value);
        }

        /// <summary>
        /// Returns the cotangent of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The cotangent of the value.</returns>
        public static Double Cot(Double value)
        {
            return 1 / Math.Tan(value);
        }

        /// <summary>
        /// Returns the inverse hyperbolic sine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The inverse hyperbolic sine of the value.</returns>
        public static Double Asinh(Double value)
        {
            return Math.Log(value + Math.Sqrt(value * value + 1));
        }

        /// <summary>
        /// Returns the inverse hyperbolic cosine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The inverse hyperbolic cosine of the value.</returns>
        public static Double Acosh(Double value)
        {
            return Math.Log(value + Math.Sqrt(value + 1) * Math.Sqrt(value - 1));
        }

        /// <summary>
        /// Returns the inverse hyperbolic tangent of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The inverse hyperbolic tangent of the value.</returns>
        public static Double Atanh(Double value)
        {
            return Math.Log((value + 1) / (1 - value)) / 2;
        }

        /// <summary>
        /// Returns the inverse hyperbolic cotangent of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The inverse hyperbolic cotangent of the value.</returns>
        public static Double ACotH(Double value)
        {
            return Math.Log((value + 1) / (value - 1)) / 2;
        }

        /// <summary>
        /// Returns the inverse hyperbolic secant of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The inverse hyperbolic secant of the value.</returns>
        public static Double Asech(Double value)
        {
            return Math.Log(1 / value + Math.Sqrt(1 / value + 1) * Math.Sqrt(1 / value - 1));
        }

        /// <summary>
        /// Returns the inverse hyperbolic cosecant of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The inverse hyperbolic cosecant of the value.</returns>
        public static Double Acsch(Double value)
        {
            return Math.Log(1 / value + Math.Sqrt(1 / value / value + 1));
        }

        /// <summary>
        /// Returns the squared sine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The squared sine of the value.</returns>
        public static Double Sin2(Double value)
        {
            value = Math.Sin(value);
            return value * value;
        }

        /// <summary>
        /// Returns the sine of a value raised to the third power.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The sine of the value raised to the third power.</returns>
        public static Double Sin3(Double value)
        {
            value = Math.Sin(value);
            return value * value * value;
        }

        /// <summary>
        /// Returns the sine of a value raised to the fourth power.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The sine of the value raised to the fourth power.</returns>
        public static Double Sin4(Double value)
        {
            value = Math.Sin(value);
            value = value * value;
            return value * value;
        }

        /// <summary>
        /// Returns the normalized cardinal sine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The normalized cardinal sine of the value.</returns>
        public static Double Sinc(Double value)
        {
            if (value == 0)
                return 1;

            return Math.Sin(Constants.PI * value) / (Constants.PI * value);
        }

        /// <summary>
        /// Returns the squared cosine of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The squared cosine of the value.</returns>
        public static Double Cos2(Double value)
        {
            value = Math.Cos(value);
            return value * value;
        }

        /// <summary>
        /// Returns the cosine of a value raised to the third power.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The cosine of the value raised to the third power.</returns>
        public static Double Cos3(Double value)
        {
            value = Math.Cos(value);
            return value * value * value;
        }

        /// <summary>
        /// Returns the cosine of a value raised to the fourth power.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The cosine of the value raised to the fourth power.</returns>
        public static Double Cos4(Double value)
        {
            value = Math.Cos(value);
            value = value * value;
            return value * value;
        }

        /// <summary>
        /// Returns the squared tangent of a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The squared tangent of the value.</returns>
        public static Double Tan2(Double value)
        {
            value = Math.Tan(value);
            return value * value;                 
        }

        /// <summary>
        /// Returns the tangent of a value raised to the third power.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The tangent of the value raised to the third power.</returns>
        public static Double Tan3(Double value)
        {
            value = Math.Tan(value);
            value = value * value;
            return value * value * value;
        }

        /// <summary>
        /// Returns the tangent of a value raised to the fourth power.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The tangent of the value raised to the fourth power.</returns>
        public static Double Tan4(Double value)
        {
            value = Math.Tan(value);
            return value * value;
        }

        #endregion

        #region Common value computation methods

        /// <summary>
        /// Computes the greatest common divisor of two numbers.
        /// </summary>
        /// <param name="x">The first number.</param>
        /// <param name="y">The second number.</param>
        /// <returns>The greatest common divisor of the two specified numbers.</returns>
        public static Int32 GreatestCommonDivisor(Int32 x, Int32 y)
        {
            Int32 result = 1;
            while (y != 0)
            {
                result = y;
                y = x % y;
                x = result;
            }
            return result;
        }

        /// <summary>
        /// Computes the greatest common divisor of two numbers.
        /// </summary>
        /// <param name="x">The first number.</param>
        /// <param name="y">The second number.</param>
        /// <returns>The greatest common divisor of the two specified numbers.</returns>
        public static Int64 GreatestCommonDivisor(Int64 x, Int64 y)
        {
            Int64 result = 1;
            while (y != 0)
            {
                result = y;
                y = x % y;
                x = result;
            }
            return result;
        }

        /// <summary>
        /// Computes the least common multiple of two numbers.
        /// </summary>
        /// <param name="x">The first number.</param>
        /// <param name="y">The second number.</param>
        /// <returns>The least common multiple of the two specified numbers.</returns>
        public static Int64 LeastCommonMultiple(Int64 x, Int64 y)
        {
            return x * y / GreatestCommonDivisor(x, y);
        }

        #endregion
    }
}
