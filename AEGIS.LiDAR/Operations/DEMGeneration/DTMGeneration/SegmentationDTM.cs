// <copyright file="SegmentationDTM.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DSMGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DTMGeneration
{
    /// <summary>
    /// Implements a method, which creates a digital terrain model from the input point cloud.
    /// This method will create and merge segments based on the elevation.
    /// </summary>
    /// <author>Roland Krisztandl</author>
    public class SegmentationDTM : DTMGenerationMethod
    {
        double[,] minHeightMap;
        bool[,] visited;
        int count;
        int visitedCount;

        readonly bool adaptivePercentage;
        readonly double elevationThreshold;
        readonly double radius;
        double percentage;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The octree which contains the points of the point cloud.</param>
        /// <param name="header"> Collection of data about the cloud.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use. Not checked.</param>
        /// <param name="cellSize">The desired output cell size.</param>
        /// <param name="useAdaptivePercentage">If true the <paramref name="percentage"/> will be calculated automaticly.</param>
        /// <param name="elevationThreshold">Determines how much is the maximum allowed distance between two cells when expanding a segment.</param>
        /// <param name="radius">
        /// Determines the search radius from the center of a grid point, from which the a value of a cell is calculated.
        /// If we want to use only the points that are in the cell, radius should be the half of <paramref name="cellSize"/>.
        /// </param>
        /// <param name="percentage">Determines how much of the terrain is ground (and not buildings).</param>
        public SegmentationDTM(ref PointOctree<LasPointBase> octree, LasPublicHeader header, int threadCount, double cellSize,
                               bool useAdaptivePercentage, double elevationThreshold, double radius, double percentage)
            : base(ref octree, header, threadCount, cellSize)
        {
            adaptivePercentage = useAdaptivePercentage;
            this.elevationThreshold = elevationThreshold;
            this.radius = radius;
            this.percentage = percentage > 1 ? 1 : percentage < 0 ? 0 : percentage;
        }

        /// <summary>
        /// A class that stores data on a segment.
        /// </summary>
        private class Segment
        {
            public Queue<Tuple<int, int>> points;
            public HashSet<Tuple<int, int>> borders;
            public double startX;
            public double startY;
            public int id;
            public int area;
        }

        /// <summary>
        /// Runs the method.
        /// </summary>
        public override Double[,] Execute()
        {
            MinimumHeightMap CMHM = new MinimumHeightMap(ref octree, header, threadCount, cellSize, radius);
            minHeightMap = CMHM.Execute();

            visited = new bool[minHeightMap.GetLength(0), minHeightMap.GetLength(1)];
            count = minHeightMap.GetLength(0) * minHeightMap.GetLength(1);
            for (int i = 0; i < grid.GetLength(0); ++i)
            {
                for (int j = 0; j < grid.GetLength(1); ++j)
                {
                    grid[i, j] = double.NaN;
                    if (double.IsNaN(minHeightMap[i, j])) --count;
                }
            }

            //Explore segments
            ExploreSegments(out List<Segment> segments, out double[,] segmentGrid);

            //merge small segments with bigger ones
            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i].area < 11)
                {
                    double maxZ = segments[i].points.Max(x => minHeightMap[x.Item1, x.Item2]);
                    if (segments[i].borders.Count == 0) continue;
                    double closestBorder = segments[i].borders.Max(x => minHeightMap[x.Item1, x.Item2] - maxZ);
                    var closestBorderCoords = segments[i].borders.Where(x => minHeightMap[x.Item1, x.Item2] - maxZ == closestBorder).First();
                    int newId = (int)segmentGrid[closestBorderCoords.Item1, closestBorderCoords.Item2];

                    if (newId == segments[i].id) continue;

                    foreach (var point in segments[i].points)
                    {
                        segmentGrid[point.Item1, point.Item2] = newId;
                        segments[newId].points.Enqueue(point);
                        segments[newId].area++;
                    }
                    segments[i] = null;
                }
            }
            segments.RemoveAll(x => x == null);

            //Calculate DTM based on segments and percentage
            if (adaptivePercentage)
                percentage = CalculatePercentage();

            visitedCount = 0;
            for (int i = 0; i < segments.Count && visitedCount < percentage * count; ++i)
            {
                visitedCount += segments[i].area;
                foreach (var point in segments[i].points)
                {
                    grid[point.Item1, point.Item2] = minHeightMap[point.Item1, point.Item2];
                }
            }

            return grid;
        }

        /// <summary>
        /// If minHeightMap[<paramref name="x"/>][<paramref name="y"/>] is valid, not visited and has noData value appends it to <paramref name="queue"/>.
        /// </summary>
        private void Add(int y, int x, ref Queue<Tuple<int, int>> queue)
        {
            if (ValidCoords(y, x) && !visited[y, x] && !Double.IsNaN(minHeightMap[y, x]))
            {
                visited[y, x] = true;
                Tuple<int, int> c = new Tuple<int, int>(y, x);
                queue.Enqueue(c);
                ++visitedCount;
            }
        }

        /// <summary>
        /// Checks weather <paramref name="x"/> and <paramref name="y"/> are valid indexes of minHeightMap[x][y].
        /// </summary>
        private bool ValidCoords(int y, int x)
        {
            return (y >= 0 && x >= 0 && y < minHeightMap.GetLength(0) && x < minHeightMap.GetLength(1));
        }

        /// <summary>
        /// Calculates how many percent is possibly ground.
        /// </summary>
        private double CalculatePercentage()
        {
            int count = 0, countBelowAvg = 0;
            double avg = 0;
            for (int i = 0; i < minHeightMap.GetLength(0); ++i)
            {
                for (int j = 0; j < minHeightMap.GetLength(1); ++j)
                {
                    if (!double.IsNaN(minHeightMap[i, j]))
                    {
                        ++count;
                        avg += minHeightMap[i, j];
                    }
                }
            }
            avg /= count;

            //count below avg
            for (int i = 0; i < minHeightMap.GetLength(0); ++i)
            {
                for (int j = 0; j < minHeightMap.GetLength(1); ++j)
                {
                    if (!double.IsNaN(minHeightMap[i, j]) && minHeightMap[i, j] < avg)
                    {
                        ++countBelowAvg;
                    }
                }
            }

            return countBelowAvg / (double)count - 0.01;
        }

        /// <summary>
        /// Explores the minHeightMap and creates segments based on elevation.
        /// </summary>
        /// <returns>With the list of segments.</returns>
        private void ExploreSegments(out List<Segment> segments, out double[,] segmentGrid)
        {
            int yStart = 0, xStart = 0;
            double min;
            visitedCount = 0;
            int segmentId = 0;

            segments = new List<Segment>();
            segmentGrid = CreateGrid(cellSize, header);

            //explore with region growing
            while (visitedCount != count)
            {
                Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();

                //find starting point
                min = double.MaxValue;
                for (int i = 0; i < minHeightMap.GetLength(0); ++i)
                {
                    for (int j = 0; j < minHeightMap.GetLength(1); ++j)
                    {
                        if (!visited[i, j] && minHeightMap[i, j] < min)
                        {
                            min = minHeightMap[i, j];
                            yStart = i;
                            xStart = j;
                        }
                    }
                }

                Add(yStart, xStart, ref queue);

                var segment = new Segment
                {
                    startX = xStart,
                    startY = yStart,
                    id = segmentId,
                    points = new Queue<Tuple<int, int>>(),
                    borders = new HashSet<Tuple<int, int>>()
                };

                //Explore level
                while (queue.Count != 0)
                {
                    var coord = queue.Dequeue();
                    int y = coord.Item1, x = coord.Item2;
                    segment.points.Enqueue(coord);
                    segmentGrid[y, x] = segmentId;

                    var coords = new List<Tuple<int, int>>(8) {
                        new Tuple<int, int>(y - 1, x),
                        new Tuple<int, int>(y + 1, x),
                        new Tuple<int, int>(y, x - 1),
                        new Tuple<int, int>(y, x + 1),
                        new Tuple<int, int>(y - 1, x - 1),
                        new Tuple<int, int>(y + 1, x + 1),
                        new Tuple<int, int>(y + 1, x - 1),
                        new Tuple<int, int>(y - 1, x + 1)
                    };

                    foreach (var c in coords)
                    {
                        if (!ValidCoords(c.Item1, c.Item2)) continue;

                        double kul = Math.Abs(minHeightMap[y, x] - minHeightMap[c.Item1, c.Item2]);
                        bool cond = kul <= elevationThreshold;
                        if (cond)
                        {
                            Add(c.Item1, c.Item2, ref queue);
                        }
                        else if (!double.IsNaN(minHeightMap[c.Item1, c.Item2])) segment.borders.Add(c);
                    }
                }
                segment.area = segment.points.Count;
                segmentId++;

                segments.Add(segment);
            }
        }

    }
}
