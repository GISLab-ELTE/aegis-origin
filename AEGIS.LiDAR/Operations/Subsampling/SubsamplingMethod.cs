/// <copyright file="SubsamplingMethod.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELTE.AEGIS.LiDAR.Operations.Subsampling
{
    /// <summary>
    /// Implements an operation, which generates a subsample from the input lidar point cloud.
    /// </summary>
    public abstract class SubsamplingMethod
    {
        protected PointOctree<LasPointBase> octree;
        protected List<LasPointBase> points;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        protected SubsamplingMethod(ref PointOctree<LasPointBase> octree)
        {
            this.octree = octree;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="points">The input array which contains the points of the point cloud.</param>
        protected SubsamplingMethod(List<LasPointBase> points)
        {
            this.points = points;
        }

        /// <summary>
        /// Executes the chosen method.
        /// </summary>
        /// <returns>The points of the sumsample.</returns>
        public abstract List<LasPointBase> Execute();

        /// <summary>
        /// Executes the chosen method.
        /// </summary>
        /// <returns>The points of the sumsample.</returns>
        public async Task<List<LasPointBase>> ExecuteAsync() => await Task.Run(() => Execute());
    }
}
