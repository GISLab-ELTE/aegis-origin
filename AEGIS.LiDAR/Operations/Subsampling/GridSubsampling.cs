/// <copyright file="GridSubsampling.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Threading;

namespace ELTE.AEGIS.LiDAR.Operations.Subsampling
{
    /// <summary>
    /// Implements an operation, which generates a subsample from the input lidar point cloud.
    /// A 3D voxel grid is generated and for each voxel a point will be assinged (if any). Always the closest point will be chosen. 
    /// </summary>
    public class GridSubsampling : SubsamplingMethod
    {
        double cellSize;
        int threadCount;
        Envelope envelope;

        LasPointBase[,,] voxel;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="cellSize">Determines the size of a voxel.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        public GridSubsampling(ref PointOctree<LasPointBase> octree, double cellSize, int threadCount) : base(ref octree)
        {
            this.cellSize = cellSize;
            points = octree.GetAll();

            if (threadCount < 1)
                this.threadCount = 1;
            else if (threadCount > Environment.ProcessorCount)
                this.threadCount = Environment.ProcessorCount;
            else
                this.threadCount = threadCount;

            envelope = octree.Envelope;
            Initalize();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="points">The input array which contains the points of the point cloud.</param>
        /// <param name="cellSize">Determines the size of a voxel.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        public GridSubsampling(List<LasPointBase> points, double cellSize, int threadCount) : base(points)
        {
            this.cellSize = cellSize;

            if (threadCount < 1)
                this.threadCount = 1;
            else if (threadCount > Environment.ProcessorCount)
                this.threadCount = Environment.ProcessorCount;
            else
                this.threadCount = threadCount;

            //build octree
            Envelope envelope = Envelope.FromCoordinates(points.Select(x => x.Coordinate));
            octree = new PointOctree<LasPointBase>(envelope, 1);

            foreach (var point in points)
                octree.Add(point, point.Coordinate);

            this.envelope = envelope;
            Initalize();
        }

        private void Initalize()
        {
            double sizeX = (envelope.MaxX - envelope.MinX) / cellSize;
            double sizeY = (envelope.MaxY - envelope.MinY) / cellSize;
            double sizeZ = (envelope.MaxZ - envelope.MinZ) / cellSize;
            int sizeXi = (int)Math.Ceiling(sizeX);
            int sizeYi = (int)Math.Ceiling(sizeY);
            int sizeZi = (int)Math.Ceiling(sizeZ);

            voxel = new LasPointBase[sizeXi, sizeYi, sizeZi];
        }

        /// <summary>
        /// The method is multithreaded. Each thread will call this function to iterate through the voxel array.
        /// </summary>
        /// <param name="from">The staring index of the iteration.</param>
        /// <param name="to">The ending index of the iteration.</param>
        protected void Job(int from, int to)
        {
            double cSHalf = cellSize / 2;
            double X, Y, Z;

            for (int x = from; x < to; ++x)
            {
                X = (x * cellSize + envelope.MinX + cSHalf);
                for (int y = 0; y < voxel.GetLength(1); ++y)
                {
                    Y = (y * cellSize + envelope.MinY + cSHalf);
                    for (int z = 0; z < voxel.GetLength(2); ++z)
                    {
                        Z = (z * cellSize + envelope.MinZ + cSHalf);

                        Envelope e = new Envelope(
                            X - cSHalf, X + cSHalf,
                            Y - cSHalf, Y + cSHalf,
                            Z - cSHalf, Z + cSHalf);

                        var list = octree.Search(e);

                        voxel[x, y, z] = CalculatePoint(ref list, new LasPointFormat0(x, y, z));
                    }
                }
            }
        }

        /// <summary>
        /// Implements a method which determines how a point will be selected for the voxel.
        /// </summary>
        /// <param name="points">The points located in the voxel.</param>
        /// <param name="center">The coordinates of the center of the voxel.</param>
        /// <returns>The selected point.</returns>
        protected virtual LasPointBase CalculatePoint(ref List<LasPointBase> points, LasPointBase center)
        {
            if (points.Count != 0)
                return points.OrderBy(p => MathExtension.DistanceSquared(p, center)).First();
            else
                return null;
        }

        /// <summary>
        /// Executes the chosen method.
        /// </summary>
        /// <returns>The points of the sumsample.</returns>
        public override List<LasPointBase> Execute()
        {
            Thread[] threads = new Thread[threadCount];

            int length = voxel.GetLength(0) / threadCount;
            int leftover = voxel.GetLength(0) - length * threadCount;

            for (int i = 0; i < threadCount; ++i)
            {
                int index = i;
                if (index == threadCount - 1)
                {
                    threads[index] = new Thread(new ParameterizedThreadStart(delegate { this.Job(index * length, (index + 1) * length + leftover); }));
                }
                else
                {
                    threads[index] = new Thread(new ParameterizedThreadStart(delegate { this.Job(index * length, (index + 1) * length); }));
                }
                threads[index].Start();
            }

            for (int i = 0; i < threadCount; ++i)
                threads[i].Join();


            List<LasPointBase> sample = new List<LasPointBase>(voxel.Length);
            foreach(LasPointBase p in voxel)
            {
                if (p != null)
                    sample.Add(p);
            }
            sample.TrimExcess();
            return sample;
        }

    }
}
