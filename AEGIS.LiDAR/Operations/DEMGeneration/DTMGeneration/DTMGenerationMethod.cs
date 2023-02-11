// <copyright file="DTMGenerationMethod.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DTMGeneration
{
    /// <summary>
    /// Implements a method, which creates a digital terrain model from the input point cloud.
    /// </summary>
    /// <author>Roland Krisztandl</author>
    public abstract class DTMGenerationMethod : DEMGenerationMethod
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="header">The header of the input point cloud.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        /// <param name="cellSize">Determines how big a cell would be in the DEM raster.</param>
        protected DTMGenerationMethod(ref PointOctree<LasPointBase> octree, LasPublicHeader header, int threadCount, double cellSize)
            : base(ref octree, header, threadCount, cellSize) { }


    }
}
