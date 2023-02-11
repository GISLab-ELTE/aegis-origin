// <copyright file="MathExtension.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.LiDAR
{
    /// <summary>
    /// A class that describes extra mathematical functions.
    /// </summary>
    /// <author>Roland Krisztandl</author>
    public static class MathExtension
    {
    		/// <summary>
    		/// Calculates the standard deviation of <paramref name="data"/>.
    		/// </summary>
    		/// <returns>The standard deviation.</returns>
        public static double StandardDeviation(double[] data)
        {
            double result = 0;

            if (data.Length != 0)
            {
                double average = data.Average();
                double sum = data.Sum(d => (d - average) * (d - average));
                result = Math.Sqrt((sum) / data.Length);
            }

            return result;
        }

		/// <summary>
		/// Calculates the Square of the Euclidean distance between two points.
		/// </summary>
		/// <param name="a">First point.</param>
		/// <param name="b">Second point.</param>
		/// <returns>Square of the Euclidean distance.</returns>
		public static double DistanceSquared(IBasicPoint a, IBasicPoint b)
		{
			return Square(a.X - b.X) + Square(a.Y - b.Y) + Square(a.Z - b.Z);
		}

		/// <summary>
		/// Calculates the Euclidean distance between two points.
		/// </summary>
		/// <param name="a">First point.</param>
		/// <param name="b">Second point.</param>
		/// <returns>The Euclidean distance.</returns>
		public static double Distance(IBasicPoint a, IBasicPoint b)
		{
			return Math.Sqrt(DistanceSquared(a, b));
		}

		/// <summary>
		/// Calculates the Square of the Euclidean distance between two points in 2D, using only X and Y coordinates.
		/// </summary>
		/// <param name="a">First point.</param>
		/// <param name="b">Second point.</param>
		/// <returns>Square of the Euclidean distance in 2D, using only X and Y coordinates.</returns>
		public static double DistanceSquaredXY(IBasicPoint a, IBasicPoint b)
		{
			return Square(a.X - b.X) + Square(a.Y - b.Y);
		}

		/// <summary>
		/// Calculates the Euclidean distance between two points in 2D, using only X and Y coordinates.
		/// </summary>
		/// <param name="a">First point.</param>
		/// <param name="b">Second point.</param>
		/// <returns>The Euclidean distance in 2D, using only X and Y coordinates.</returns>
		public static double DistanceXY(IBasicPoint a, IBasicPoint b)
		{
			return Math.Sqrt(DistanceSquaredXY(a, b));
		}

		/// <summary>
		/// Calculates the square of <paramref name="a"/>.
		/// </summary>
		/// <returns>The square of <paramref name="a"/>.</returns>
		private static double Square(double a)
		{
			return a * a;
		}

	}
}
