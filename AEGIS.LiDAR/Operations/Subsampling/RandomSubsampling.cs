/// <copyright file="RandomSubsampling.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.LiDAR.Operations.Subsampling
{
    /// <summary>
    /// Implements an operation, which generates a subsample from the input lidar point cloud.
    /// The points of the subsample are selected randomly.
    /// </summary>
    public class RandomSubsampling : SubsamplingMethod
    {
        int targetNumber;
        bool customSeed = false;
        int seed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="targetNumber">The number of points to select from the point cloud.</param>
        public RandomSubsampling(ref PointOctree<LasPointBase> octree, int targetNumber) : base(ref octree)
        {
            this.targetNumber = targetNumber;
            points = octree.GetAll();
            customSeed = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="points">The input array which contains the points of the point cloud.</param>
        /// <param name="targetNumber">The number of points to select from the point cloud.</param>
        public RandomSubsampling(List<LasPointBase> points, int targetNumber) : base(points)
        {
            this.targetNumber = targetNumber;
            customSeed = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="targetNumber">The number of points to select from the point cloud.</param>
        /// <param name="seed">The seed of the random generator.</param>
        public RandomSubsampling(ref PointOctree<LasPointBase> octree, int targetNumber, int seed) : this(ref octree, targetNumber)
        {
            customSeed = true;
            this.seed = seed;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="points">The input array which contains the points of the point cloud.</param>
        /// <param name="targetNumber">The number of points to select from the point cloud.</param>
        /// <param name="seed">The seed of the random generator.</param>
        public RandomSubsampling(List<LasPointBase> points, int targetNumber, int seed) : this(points, targetNumber)
        {
            customSeed = true;
            this.seed = seed;
        }

        /// <summary>
        /// Executes the chosen method.
        /// </summary>
        /// <returns>The points of the sumsample.</returns>
        public override List<LasPointBase> Execute()
        {
            if (targetNumber >= points.Count)
                return points;

            Random random;
            if (customSeed)
                random = new Random(seed);
            else
                random = new Random();

            return points.Select(v => new { v, i = random.NextDouble() })
                .OrderBy(x => x.i).Take(targetNumber)
                .Select(x => x.v).ToList();
        }
    }
}
