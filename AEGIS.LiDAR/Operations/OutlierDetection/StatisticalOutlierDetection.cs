/// <copyright file="StatisticalOutlierDetection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roland Krisztandl and Líviusz Szalma</author>

using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.LiDAR.Operations.OutlierDetection
{
    /// <summary>
    /// Implements a statistical based outlier detection operation.
    /// 
    /// For each point, we compute the mean distance from it to its neighbors. By assuming that the resulted distribution is
    /// Gaussian with a mean and a standard deviation, all points whose mean distances are outside an interval defined by the
    /// global distances mean and standard deviation can be considered as outliers.
    /// </summary>
    public class StatisticalOutlierDetection : OutlierDetectionMethod
    {
        readonly Int32 neighbors;
        readonly Double radius;
        readonly Int32 iterationThreshold;
        readonly Double standardDeviationFactor;
        readonly Double[] means;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The input octree which contains the points of the point cloud.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        /// <param name="neighbors">Determines how many neighbours will be used.</param>
        /// <param name="standardDeviationFactor">Determines the difference between the distance and the mean from the Gaussian distribution.</param>
        public StatisticalOutlierDetection(ref PointOctree<LasPointBase> octree, int threadCount, int neighbors, double standardDeviationFactor)
            : base(ref octree, threadCount)
        {
            this.neighbors = neighbors + 1; //+1 because we need KNN, and it will find itself as well
            this.standardDeviationFactor = standardDeviationFactor;
            radius = 0.1d;
            iterationThreshold = 50;

            means = new double[points.Length];
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

            List<double> distances = new List<double>();
            double multiplier;
            int iterations = 1; //if the iterations reach iterThreshold then a new defMultiplier is set based on the modifiers in the previus iterations
            double defMultiplier = 1;
            double avgMultiplier = 0;
            LasPointBase point;
            int octreeCount = octree.NumberOfPoints;

            for (int i = from; i < to; ++i)
            {
                //Because we need KNN, we use a dynamic search with various radius. Every time we reach iterationThreshold we set a new multiplier
                point = points[i].Obj;
                if (iterations < iterationThreshold)
                {
                    ++iterations;
                    multiplier = defMultiplier;
                }
                else //if(iterations == iterThreshold)
                {
                    avgMultiplier /= iterationThreshold;
                    multiplier = avgMultiplier;
                    defMultiplier = avgMultiplier;
                    iterations = 1;
                    avgMultiplier = 0;
                }

                distances.Clear();

                Envelope envelope = new Envelope(
                    points[i].Point.X - radius * multiplier, points[i].Point.X + radius * multiplier,
                    points[i].Point.Y - radius * multiplier, points[i].Point.Y + radius * multiplier,
                    points[i].Point.Z - radius * multiplier, points[i].Point.Z + radius * multiplier);
                nearby = octree.Search(envelope).ToArray();

                //if we dont have enough points
                if (neighbors <= octreeCount)
                    nearby = octree.GetAll().ToArray();
                //repeats Search() until list has at least k elements
                else while (nearby.Length < neighbors)
                {
                    //if we dont have enough elements, we increase the multiplier
                    multiplier *= 1.5d;
                    envelope = new Envelope(
                        points[i].Point.X - radius * multiplier, points[i].Point.X + radius * multiplier,
                        points[i].Point.Y - radius * multiplier, points[i].Point.Y + radius * multiplier,
                        points[i].Point.Z - radius * multiplier, points[i].Point.Z + radius * multiplier);
                    nearby = octree.Search(envelope).ToArray();
                }
                //If we have to many elements, we decrease the multiplier
                if (nearby.Length > this.neighbors * this.neighbors)
                {
                    multiplier *= (1 / 1.5d);
                }

                avgMultiplier += multiplier;

                IEnumerator<LasPointBase> kClosestNeighbor = nearby.OrderBy(x => MathExtension.DistanceSquared(x, point)).Take(this.neighbors).GetEnumerator();
                kClosestNeighbor.MoveNext();
                while (kClosestNeighbor.MoveNext())
                {
                    distances.Add(MathExtension.Distance(point, kClosestNeighbor.Current));
                }
                means[i] = distances.Average();
            }
        }

        /// <summary>
        /// This method will use the data created by the Job method to detect outliers.
        /// </summary>
        /// <returns>The outlier points.</returns>
        protected override List<PointQuadTree<LasPointBase>.TreeObject> Collect()
        {
            List<PointQuadTree<LasPointBase>.TreeObject> list = new List<PointQuadTree<LasPointBase>.TreeObject>();

            double averageMean = means.Average();
            double threshold = standardDeviationFactor * MathExtension.StandardDeviation(means); //t * (standard deviation of means)

            for (int i = 0; i < points.Length; i++)
            {
                if (means[i] > averageMean + threshold * threshold)
                {
                    list.Add(points[i]);
                }
            }

            return list;
        }
    }
}
