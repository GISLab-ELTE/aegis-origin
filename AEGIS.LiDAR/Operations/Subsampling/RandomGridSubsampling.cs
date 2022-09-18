/// <copyright file="RandomGridSubsampling.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.LiDAR.Operations.Subsampling
{
    /// <summary>
    /// Implements an operation, which generates a subsample from the input lidar point cloud.
    /// A 3D voxel grid is generated and a point will be randomly chosen for each voxel. 
    /// </summary>
    public class RandomGridSubsampling : GridSubsampling
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="cellSize">Determines the size of a voxel.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        public RandomGridSubsampling(ref PointOctree<LasPointBase> octree, double cellSize, int threadCount) :
            base(ref octree, cellSize, threadCount) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="points">The input array which contains the points of the point cloud.</param>
        /// <param name="cellSize">Determines the size of a voxel.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        public RandomGridSubsampling(List<LasPointBase> points, double cellSize, int threadCount) :
            base(points, cellSize, threadCount) { }

        /// <summary>
        /// Implements a method which determines how a point will be selected for the voxel.
        /// </summary>
        /// <param name="points">The points located in the voxel.</param>
        /// <param name="center">The coordinates of the center of the voxel.</param>
        /// <returns>The selected point.</returns>
        protected override LasPointBase CalculatePoint(ref List<LasPointBase> points, LasPointBase center)
        {
            Random random = new Random();

            if (points.Count != 0)
                return points[random.Next(0, points.Count)];
            else
                return null;
        }
    }
}
