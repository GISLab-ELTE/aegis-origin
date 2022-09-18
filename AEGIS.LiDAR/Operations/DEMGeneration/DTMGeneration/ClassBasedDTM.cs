/// <copyright file="ClassBasedDTM.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DSMGeneration;
using System;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DTMGeneration
{
    /// <summary>
    /// Implements a method, which creates a digital terrain model from the input point cloud.
    /// This method will create a DSM using only the ground points (Classification == 2).
    /// </summary>
    public class ClassBasedDTM : DTMGenerationMethod
    {
        readonly Double radius;

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
        public ClassBasedDTM(ref PointOctree<LasPointBase> octree, LasPublicHeader header, int threadCount, double cellSize, double radius)
            : base(ref octree, header, threadCount, cellSize)
        {
            this.radius = radius;
        }

        /// <summary>
        /// Executes the chosen method, which creates a DEM.
        /// </summary>
        /// <returns>The DEM as a raster of elevations.</returns>
        public override Double[,] Execute()
        {
            PointOctree<LasPointBase> groundPoints = new PointOctree<LasPointBase>(octree.Envelope, octree.MinNodeSize);
            foreach(var point in octree.GetAllWithCoords())
            {
                if (point.Obj.Classification == 2)
                    groundPoints.Add(point.Obj, point.Point);
            }

            InverseDistanceWeighting idw = new InverseDistanceWeighting(ref groundPoints, header, threadCount, cellSize, radius);
            return idw.Execute();
        }
    }
}
