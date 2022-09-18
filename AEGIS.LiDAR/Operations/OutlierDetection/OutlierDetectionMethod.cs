/// <copyright file="OutlierDetectionMethod.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Threading;
using System.Threading.Tasks;

namespace ELTE.AEGIS.LiDAR.Operations.OutlierDetection
{
    /// <summary>
    /// Implements an operation, which detects outliers from the input lidar point cloud.
    /// </summary>
    public abstract class OutlierDetectionMethod
    {
        /// <summary>
        /// The octree wich contains the points of the point cloud.
        /// </summary>
        protected PointOctree<LasPointBase> octree;
        /// <summary>
        /// The points of the point cloud in an array form. Acquired from the octree.
        /// </summary>
        protected PointQuadTree<LasPointBase>.TreeObject[] points;
        /// <summary>
        /// Number of threads to use.
        /// </summary>
        protected Int32 threadCount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        protected OutlierDetectionMethod(ref PointOctree<LasPointBase> octree, int threadCount)
        {
            this.octree = octree;
            points = octree.GetAllWithCoords().ToArray();

            if (threadCount < 1)
                this.threadCount = 1;
            else if (threadCount > Environment.ProcessorCount)
                this.threadCount = Environment.ProcessorCount;
            else
                this.threadCount = threadCount;
        }

        /// <summary>
        /// Executes the chosen method, which detects outlier points in the point cloud.
        /// </summary>
        /// <returns>The outlier points.</returns>
        public List<PointQuadTree<LasPointBase>.TreeObject> Execute()
        {
            Thread[] threads = new Thread[threadCount];
            int length = octree.NumberOfPoints / threadCount;
            int leftover = octree.NumberOfPoints - length * threadCount;

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

            return Collect();
        }

        /// <summary>
        /// Executes the chosen method, which detects outlier points in the point cloud.
        /// </summary>
        /// <returns>The outlier points.</returns>
        public async Task<List<PointQuadTree<LasPointBase>.TreeObject>> ExecuteAsync() => await Task.Run(() => Execute());

        /// <summary>
        /// The method is multithreaded. Each thread will call this function to iterate through the points array.
        /// This is a preprocessor, calculates various data for the outlier detection.
        /// </summary>
        /// <param name="from">The staring index of the iteration.</param>
        /// <param name="to">The ending index of the iteration.</param>
        protected abstract void Job(int from, int to);

        /// <summary>
        /// This method will use the data created by the Job method to detect outliers.
        /// </summary>
        /// <returns>The outlier points.</returns>
        protected abstract List<PointQuadTree<LasPointBase>.TreeObject> Collect();
    }
}
