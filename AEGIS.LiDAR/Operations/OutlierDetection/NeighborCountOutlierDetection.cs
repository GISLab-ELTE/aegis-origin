/// <copyright file="NeighborCountOutlierDetection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.LiDAR.Operations.OutlierDetection
{
    /// <summary>
    /// Implements a simple method for outlier detection.
    /// 
    /// For each point we will check how many neighbours it has within a radius.
    /// If this is lower then a threshold, it can be considered an outlier.
    /// </summary>
    public class NeighborCountOutlierDetection : OutlierDetectionMethod
    {
        readonly Int32 neighbors;
        readonly Double radius;
        readonly Int32[] numberOfNeighbors;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        /// <param name="neighbors">Determines the threshold to use.</param>
        /// <param name="radius">Determines the radius of the search around the points.</param>
        public NeighborCountOutlierDetection(ref PointOctree<LasPointBase> octree, int threadCount, int neighbors, double radius)
            : base(ref octree, threadCount)
        {
            this.neighbors = neighbors;
            this.radius = radius;
            numberOfNeighbors = new int[points.Length];
        }

        /// <summary>
        /// The method is multithreaded. Each thread will call this function to iterate through the points array.
        /// This is a preprocessor, calculates various data for the outlier detection.
        /// </summary>
        /// <param name="from">The staring index of the iteration.</param>
        /// <param name="to">The ending index of the iteration.</param>
        protected override void Job(int from, int to)
        {
            LasPointBase[] nearby;

            for (int j = from; j < to; ++j)
            {
                Envelope envelope = new Envelope(
                    points[j].Point.X - radius, points[j].Point.X + radius,
                    points[j].Point.Y - radius, points[j].Point.Y + radius,
                    points[j].Point.Z - radius, points[j].Point.Z + radius);

                nearby = octree.Search(envelope).ToArray();
                numberOfNeighbors[j] = nearby.Length;
            }
        }

        /// <summary>
        /// This method will use the data created by the Job method to detect outliers.
        /// </summary>
        /// <returns>The outlier points.</returns>
        protected override List<PointQuadTree<LasPointBase>.TreeObject> Collect()
        {
            List<PointQuadTree<LasPointBase>.TreeObject> list = new List<PointQuadTree<LasPointBase>.TreeObject>();

            for (int i = 0; i < points.Length; ++i)
            {
                if(numberOfNeighbors[i] < neighbors)
                {
                    list.Add(points[i]);
                }
            }

            return list;
        }
    }
}
