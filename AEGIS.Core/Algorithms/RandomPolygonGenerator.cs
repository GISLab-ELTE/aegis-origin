/// <copyright file="RandomPolygonGenerator.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
/// <author>Orsolya Harazin</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type which performs creation of random polygons.
    /// </summary>
    public static class RandomPolygonGenerator
    {
        #region GetRandomPolygon computation

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateNumber">The number of the polygon coordinates.</param>
        /// <param name="minCoordinate">The lower boundary of the generated polygon.</param>
        /// <param name="maxCoordinate">The upper boundary of the generated polygon.</param>
        /// <returns>The generated polygon.</returns>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateNumber, Coordinate minCoordinate, Coordinate maxCoordinate)
        {
            return CreateRandomPolygon(coordinateNumber, minCoordinate, maxCoordinate, 0.1, PrecisionModel.Default);
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateNumber">The number of the polygon coordinates.</param>
        /// <param name="minCoordinate">The lower boundary of the generated polygon.</param>
        /// <param name="maxCoordinate">The upper boundary of the generated polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The generated polygon.</returns>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateNumber, Coordinate minCoordinate, Coordinate maxCoordinate, PrecisionModel precisionModel)
        {
            return CreateRandomPolygon(coordinateNumber, minCoordinate, maxCoordinate, 0.1, precisionModel);
        }
        
        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateNumber">The number of the polygon coordinates.</param>
        /// <param name="minCoordinate">The lower boundary of the generated polygon.</param>
        /// <param name="maxCoordinate">The upper boundary of the generated polygon.</param>
        /// <param name="convexityRatio">The convexity ratio.</param>
        /// <returns>The generated polygon.</returns>
        /// <remarks>
        /// Statistics for <paramref name="convexRation" /> with respect to chance of convex polygon: 0.1 => 1%, 0.2 => 3%, 0.3 => 6%, 0.4 => 10%, 0.5 => 15%, 0.6 => 25%, 0.7 => 39%, 0.8 => 53%, 0.9 => 72%, 1.0 => 100%
        /// </remarks>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateNumber, Coordinate minCoordinate, Coordinate maxCoordinate, Double convexityRatio)
        {
            return CreateRandomPolygon(coordinateNumber, minCoordinate, maxCoordinate, convexityRatio, PrecisionModel.Default);
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateNumber">The number of the polygon coordinates.</param>
        /// <param name="minCoordinate">The lower boundary of the generated polygon.</param>
        /// <param name="maxCoordinate">The upper boundary of the generated polygon.</param>
        /// <param name="convexityRatio">The convexity ratio.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The generated polygon.</returns>
        /// <remarks>
        /// Statistics for <paramref name="convexRation" /> with respect to chance of convex polygon: 0.1 => 1%, 0.2 => 3%, 0.3 => 6%, 0.4 => 10%, 0.5 => 15%, 0.6 => 25%, 0.7 => 39%, 0.8 => 53%, 0.9 => 72%, 1.0 => 100%
        /// </remarks>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateNumber, Coordinate minCoordinate, Coordinate maxCoordinate, Double convexityRatio, PrecisionModel precisionModel)
        {
            // source: http://csharphelper.com/blog/2012/08/generate-random-polygons-in-c/

            if (coordinateNumber < 3)
                throw new ArgumentOutOfRangeException("vertexNumber", "The number of vertices must be over 2.");
            if (minCoordinate.X > maxCoordinate.X || minCoordinate.Y > maxCoordinate.Y)
                throw new ArgumentOutOfRangeException("rectangleMinCoordinate", "The rectangle's minimum coordinate is greater than the maximum coordinate.");
            if (convexityRatio > 1.0 || convexityRatio < 0.0)
                throw new ArgumentOutOfRangeException("convexRatio", "The convexity ratio is less than 0 or greater than 1.");

            IList<Coordinate> shell = new List<Coordinate>(coordinateNumber + 1);

            Random rand = new Random();

            // random points
            Double[] points = new Double[coordinateNumber];
            Double minRadius = convexityRatio;
            Double maxRadius = 1.0;

            for (Int32 i = 0; i < coordinateNumber; i++)
            {
                points[i] = minRadius + rand.NextDouble() * (maxRadius - minRadius);
            }

            // random angle weights
            Double[] angleWeights = new Double[coordinateNumber];
            Double totalWeight = 0;

            for (Int32 i = 0; i < coordinateNumber; i++)
            {
                angleWeights[i] = rand.NextDouble();
                totalWeight += angleWeights[i];
            }

            // convert weights into radians
            Double[] angles = new Double[coordinateNumber];
            for (Int32 i = 0; i < coordinateNumber; i++)
            {
                angles[i] = angleWeights[i] * 2 * Math.PI / totalWeight;
            }

            // moving points according to angles
            Double halfWidth = (maxCoordinate.X - minCoordinate.X) / 2;
            Double halfHeight = (maxCoordinate.Y - minCoordinate.Y) / 2;
            Double midPointX = minCoordinate.X + halfWidth;
            Double midPointY = minCoordinate.Y + halfHeight;
            Double theta = 0;

            for (Int32 i = 0; i < coordinateNumber; i++)
            {
                shell.Add(precisionModel.MakePrecise(new Coordinate(midPointX + (halfWidth * points[i] * Math.Cos(theta)),
                                                                    midPointY + (halfHeight * points[i] * Math.Sin(theta)))));
                theta += angles[i];
            }
            shell.Add(shell[0]);

            return new BasicPolygon(shell);
        }

        #endregion
    }
}
