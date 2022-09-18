/// <copyright file="InverseDistanceWeighting.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Roland Krisztandl</author>

using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DSMGeneration
{
    /// <summary>
    /// Implemetation of a DSM gerenration operation, which will use Inverse Distance Weighting to calculate the height in each cell.
    /// </summary>
    public class InverseDistanceWeighting : DSMGenerationMethod
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="header">The header of the input point cloud.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        /// <param name="cellSize">Determines how big a cell would be in the DEM raster.</param>
        /// <param name="radius">
        /// Determines the search radius from the center of a grid point, from which the a value of a cell is calculated.
        /// If we want to use only the points that are in the cell, radius should be the half of <paramref name="cellSize"/>.
        /// </param>
        public InverseDistanceWeighting(ref PointOctree<LasPointBase> octree, LasPublicHeader header, int threadCount, double cellSize, double radius)
            : base(ref octree, header, threadCount, cellSize, radius) { }

        /// <summary>
        /// Calculates the value of the point in the <paramref name="center"/> based on the points in <paramref name="points"/>
        /// </summary>
        /// <returns>The calculated value.</returns>
        protected override Double CalculateValue(ref IEnumerable<LasPointBase> points, Coordinate center)
        {
            double value = 0, distances = 0, weight, distance;

            LasPointBase centerPoint = new LasPointFormat0(center.X, center.Y, center.Z);

            foreach (var point in points)
            {
                distance = MathExtension.DistanceSquaredXY(point, centerPoint);
                if (distance == 0)
                {
                    weight = 1;
                }
                else
                {
                    weight = 1 / distance;
                    distances += weight;
                }
                value += (point.Z * weight);
            }
            if (distances == 0)
                return value;

            value /= distances;

            return value;
        }
    }
}
