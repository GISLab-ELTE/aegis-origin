// <copyright file="MinimumHeightMap.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DSMGeneration
{
    /// <summary>
    /// Implemetation of a DSM gerenration operation, where each cell's height will be determined based on the lowest point in that area.
    /// </summary>
    /// <author>Roland Krisztandl</author>
    public class MinimumHeightMap : DSMGenerationMethod
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
        public MinimumHeightMap(ref PointOctree<LasPointBase> octree, LasPublicHeader header, int threadCount, double cellSize, double radius)
            : base(ref octree, header, threadCount, cellSize, radius) { }

        /// <summary>
        /// Calculates the value of the point in the <paramref name="center"/> based on the points in <paramref name="points"/>
        /// </summary>
        /// <returns>The calculated value.</returns>
        protected override Double CalculateValue(ref IEnumerable<LasPointBase> points, Coordinate center)
        {
            return points.Select(point => point.Z).Min();
        }
    }
}
