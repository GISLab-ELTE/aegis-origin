/// <copyright file="MersenneTwisterRandomGenerator.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dóra Papp</author>

using System;

namespace ELTE.AEGIS.Numerics.Statistics
{
    /// <summary>
    /// Represents the Mersenne Twister pseudorandom number generator.
    /// </summary>
    public class MersenneTwisterRandomGenerator
    {
        #region Private constants

        /// <summary>
        /// The word size (in number of bits).
        /// </summary>
        private const Int16 w = 32;

        /// <summary>
        /// Degree of recurrence.
        /// </summary>
        private const Int16 n = 624;

        /// <summary>
        /// The middle word, an offset used in the recurrence relation defining the series x, 1 ≤ m < n.
        /// </summary>
        private const Int16 m = 397;

        /// <summary>
        /// The separation point of one word, or the number of bits of the lower bitmask, 0 ≤ r ≤ w - 1.
        /// </summary>
        private const Int16 r = 31;

        /// <summary>
        /// Coefficients of the rational normal form twist matrix.
        /// </summary>
        private const Int64 a = 0x9908B0DF16;

        /// <summary>
        /// Tempering bit shift 1.
        /// </summary>
        private const Int16 s = 7;

        /// <summary>
        /// Tempering bitmask 1.
        /// </summary>
        private const Int64 b = 0x9D2C5680;

        /// <summary>
        /// Tempering bit shift 2.
        /// </summary>
        private const Int16 t = 15;

        /// <summary>
        /// Tempering bitmask 2.
        /// </summary>
        private const Int64 c = 0xEFC60000;

        /// <summary>
        /// Tempering bit shift 3.
        /// </summary>
        private const Int16 u = 11;

        /// <summary>
        /// Tempering bitmask 3.
        /// </summary>
        private const Int64 d = 0xFFFFFFFF;

        /// <summary>
        /// Tempering bit shift 4.
        /// </summary>
        private const Int16 l = 18;

        /// <summary>
        /// Another parameter to the generator, though not part of the algorithm proper.
        /// </summary>
        private const Int32 f = 1812433253;

        /// <summary>
        /// Lower mask.
        /// </summary>
        private const UInt32 lower_mask = unchecked((1 << r) - 1);

        /// <summary>
        /// Upper mask.
        /// </summary>
        private const UInt32 upper_mask = ~lower_mask;

        #endregion

        #region Private fields

        /// <summary>
        /// Array to store the state of the generator.
        /// </summary>
        private UInt32[] _MT;

        /// <summary>
        /// The indey of the generated numbers.
        /// </summary>
        private Int16 _index;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwisterRandomGenerator" /> class.
        /// </summary>
        /// <param name="seed">The seed of the generator.</param>
        public MersenneTwisterRandomGenerator(UInt32 seed)
        {
            _MT = new UInt32[n];
            _index = n;
            _MT[0] = seed;
            for (UInt32 i = 1; i < n; ++i)
            {
                _MT[i] = (f * (_MT[i - 1] ^ (_MT[i - 1] >> (w - 2))) + i);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a random number.
        /// </summary>
        /// <returns>The generated number.</returns>
        public UInt32 Next()
        {
            if (_index == n)
            {
                Twist();
            }

            UInt32 y = _MT[_index];
            y = (UInt32)(y ^ ((y >> u) & d));
            y = (UInt32)(y ^ ((y << s) & b));
            y = (UInt32)(y ^ ((y << t) & c));
            y = y ^ (y >> l);

            ++_index;

            return y;
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>The generated number.</returns>
        public Double NextDouble()
        {
            return (Convert.ToDouble(Next()) / UInt32.MaxValue);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generates the next n values.
        /// </summary>
        private void Twist()
        {
            for (Int32 i = 0; i < n; ++i)
            {
                UInt32 x = (_MT[i] & upper_mask) + (_MT[(i + 1) % n] & lower_mask);
                UInt32 xA = x >> 1;
                if ((x % 2) != 0)
                    xA = (UInt32)(xA ^ a);
                _MT[i] = _MT[(i + m) % n] ^ xA;
            }

            _index = 0;
        }

        #endregion
    }
}
