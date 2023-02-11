// <copyright file="DSMGenerationMethod.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Threading;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DSMGeneration
{
    /// <summary>
    /// Implements a method, which creates a digital surface model from the input point cloud.
    /// </summary>
    /// <author>Roland Krisztandl</author>
    public abstract class DSMGenerationMethod : DEMGenerationMethod
    {
        protected Double radius;

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
        protected DSMGenerationMethod(ref PointOctree<LasPointBase> octree, LasPublicHeader header, int threadCount, double cellSize, double radius)
            : base(ref octree, header, threadCount, cellSize)
        {
            this.radius = radius;
        }

        /// <summary>
        /// Calculates the value of the point in the <paramref name="center"/> based on the points in <paramref name="points"/>
        /// </summary>
        /// <returns>The calculated value.</returns>
        protected abstract Double CalculateValue(ref IEnumerable<LasPointBase> points, Coordinate center);

        /// <summary>
        /// Executes the chosen method, which creates a DEM.
        /// </summary>
        /// <returns>The DEM as a raster of elevations.</returns>
        public override double[,] Execute()
        {
            Thread[] threads = new Thread[threadCount];
            int length = grid.GetLength(0) / threadCount;
            int leftover = grid.GetLength(0) - length * threadCount;

            for (int i = 0; i < threadCount; ++i)
            {
                int index = i;
                if (index == threadCount - 1)
                {
                    threads[i] = new Thread(new ParameterizedThreadStart(delegate { this.Job(index * length, (index + 1) * length + leftover); }));
                }
                else
                {
                    threads[i] = new Thread(new ParameterizedThreadStart(delegate { this.Job(index * length, (index + 1) * length); }));
                }
                threads[i].Start();
            }

            for (int i = 0; i < threadCount; ++i)
                threads[i].Join();

            return grid;
        }

        /// <summary>
        /// The method is multithreaded. Each thread will call this function to iterate through the grid array.
        /// </summary>
        /// <param name="from">The staring index of the iteration.</param>
        /// <param name="to">The ending index of the iteration.</param>
        protected void Job(int from, int to)
        {
            float X, Y;
            Coordinate center;
            IEnumerable<LasPointBase> nearby;

            for (int i = from; i < to; ++i) // Y
            {
                Y = (float)(i * cellSize + octree.Envelope.MinY + cellSize / 2); //header.MinY
                for (int j = 0; j < grid.GetLength(1); ++j) // X
                {
                    X = (float)(j * cellSize + octree.Envelope.MinX + cellSize / 2); //header.MinX
                    center = new Coordinate(X, Y, 0);

                    Envelope envelope = new Envelope(
                        X - radius, X + radius,
                        Y - radius, Y + radius,
                        double.MinValue, double.MaxValue);
                    nearby = octree.Search(envelope);

                    if (nearby.Count() == 0)
                        grid[i, j] = double.NaN;
                    else
                        grid[i, j] = CalculateValue(ref nearby, center);
                }
            }
        }
    }
}
