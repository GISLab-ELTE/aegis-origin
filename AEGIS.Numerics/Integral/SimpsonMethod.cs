/// <copyright file="SimpsonMethod.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics.Integral
{
    /// <summary>
    /// Represents a type for computing the integral of functions using Simpson's rule.
    /// </summary>
    public class SimpsonMethod
    {
        /// <summary>
        /// Computes the integral of a function in a closed interval.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="intervalStart">The begin of the interval.</param>
        /// <param name="intervalEnd">The end of the interval.</param>
        /// <returns>The integral of <paramref name="function" /> in the specified closed interval.</returns>
        /// <exception cref="System.ArgumentNullException">The function is null.</exception>
        public static Double Integrate(Func<Double, Double> function, Double intervalStart, Double intervalEnd)
        {
            return Integrate(function, intervalStart, intervalEnd, 2);
        }

        /// <summary>
        /// Computes the integral of a function in a closed interval.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="intervalStart">The begin of the interval.</param>
        /// <param name="intervalEnd">The end of the interval.</param>
        /// <param name="numberOfIntervals">The number of intervals.</param>
        /// <returns>The integral of <paramref name="function" /> in the specified closed interval.</returns>
        /// <exception cref="System.ArgumentNullException">The function is null.</exception>
        /// <exception cref="System.ArgumentException">The number of intervals must be positive even.</exception>
        public static Double Integrate(Func<Double, Double> function, Double intervalStart, Double intervalEnd, Int32 numberOfIntervals)
        {
            if (function == null)
                throw new ArgumentNullException("function", "The function is null.");

            if (numberOfIntervals < 2 || numberOfIntervals % 2 == 1)
                throw new ArgumentException("The number of intervals must be positive even.", "numberOfIntervals");

            // source: http://en.wikipedia.org/wiki/Simpson's_rule

            if (intervalStart == intervalEnd)
                return 0;

            Double intervalLength = (intervalEnd - intervalStart) / numberOfIntervals;

            Double value = function(intervalStart) + function(intervalEnd);
            for (Int32 i = 1; i <= numberOfIntervals; i += 2)
            {
                value += 4 * function(intervalStart + intervalLength * i);
            }
            for (Int32 i = 2; i <= numberOfIntervals - 1; i += 2)
            {
                value += 2 * function(intervalStart + intervalLength * i);
            }

            return value * intervalLength / 3;
        }

    }
}
