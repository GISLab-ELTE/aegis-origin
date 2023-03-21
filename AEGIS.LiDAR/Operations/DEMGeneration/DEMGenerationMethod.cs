// <copyright file="DEMGenerationMethod.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Threading.Tasks;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration
{
    /// <summary>
    /// Implements a method, which creates a digital elevation model from the input point cloud.
    /// </summary>
    /// <author>Roland Krisztandl</author>
    public abstract class DEMGenerationMethod
    {
        protected PointOctree<LasPointBase> octree;
        protected LasPublicHeader header;
        protected Int32 threadCount;
        protected Double cellSize;
        protected Double[,] grid;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="header">The header of the input point cloud.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        /// <param name="cellSize">Determines how big a cell would be in the DEM raster.</param>
        protected DEMGenerationMethod(ref PointOctree<LasPointBase> octree, LasPublicHeader header, int threadCount, double cellSize)
        {
            this.octree = octree;
            this.header = header;
            this.cellSize = cellSize;

            if (threadCount < 1)
                this.threadCount = 1;
            else if (threadCount > Environment.ProcessorCount)
                this.threadCount = Environment.ProcessorCount;
            else
                this.threadCount = threadCount;

            grid = CreateGrid();
        }

        /// <summary>
        /// Executes the chosen method, which creates a DEM.
        /// </summary>
        /// <returns>The DEM as a raster of elevations.</returns>
        public abstract Double[,] Execute();

        /// <summary>
        /// Executes the chosen method, which creates a DEM.
        /// </summary>
        /// <returns>The DEM as a raster of elevations.</returns>
        public async Task<Double[,]> ExecuteAsync() => await Task.Run(() => Execute());

        /// <summary>
        /// Creates an empty raster that covers the point cloud described by the header with the stored cellSize.>
        /// </summary>
        /// <returns>The empty raster.</returns>
        protected double[,] CreateGrid()
        {
            return CreateGrid(cellSize, header);
        }

        /// <summary>
        /// Creates an empty raster that covers the point cloud described by <paramref name="header"/> with the given cellSize of <paramref name="cellSize"/>
        /// </summary>
        /// <returns>The empty raster.</returns>
        public static double[,] CreateGrid(double cellSize, LasPublicHeader header)
        {
            double sizeX = (header.MaxX - header.MinX) / cellSize;
            double sizeY = (header.MaxY - header.MinY) / cellSize;
            int sizeXi = (int)Math.Ceiling(sizeX);
            int sizeYi = (int)Math.Ceiling(sizeY);

            return new double[sizeYi, sizeXi];
        }
    }
}
