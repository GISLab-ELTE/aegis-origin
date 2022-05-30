/// <copyright file="MorphologicalDTM.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Líviusz Szalma</author>

using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DSMGeneration;
//using Emgu.CV;
//using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DTMGeneration
{
    //Commented out duo to a compatibility issue, that I could not resolve.

    /*/// <summary>
    /// Implements a method, which creates a digital terrain model from the input point cloud.
    /// This method will do morphological openings on a MinimumHeightMap.
    /// </summary>
    public class MorphologicalDTM : DTMGenerationMethod
    {
        readonly float minHeightMapSearchRadius;
        readonly double slopeTolerance;
        readonly double maxWindowRadius;
        Mat lastSurface;
        Mat thisSurface;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="octree">The octree which contains the points of the point cloud.</param>
        /// <param name="header"> Collection of data about the cloud.</param>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use. Not checked.</param>
        /// <param name="cellSize">The desired output cell size.</param>
        /// <param name="slopeTolerance">Determines the maximum differnce between two cells. Should be 0.05 - 0.5.</param>
        /// <param name="maxWindowSize">Determines the size of the biggest window.</param>
        public MorphologicalDTM(ref PointOctree<LasPointBase> octree, LasPublicHeader header, int threadCount, double cellSize, double slopeTolerance, int maxWindowSize)
            : base(ref octree, header, threadCount, cellSize)
        {
            minHeightMapSearchRadius = 1f;
            this.slopeTolerance = slopeTolerance; //0.05-0.5
            maxWindowRadius = maxWindowSize / cellSize; //1-40 meters

            lastSurface = new Mat(new Size(grid.GetLength(1), grid.GetLength(0)), Emgu.CV.CvEnum.DepthType.Cv32F, 1);
            thisSurface = new Mat(new Size(grid.GetLength(1), grid.GetLength(0)), Emgu.CV.CvEnum.DepthType.Cv32F, 1);
        }

        /// <summary>
        /// Runs the method.
        /// </summary>
        /// <returns></returns>
        public override Double[,] Execute()
        {
            MinimumHeightMap CMHM = new MinimumHeightMap(ref octree, header, threadCount, cellSize, minHeightMapSearchRadius);
            grid = CMHM.Execute();

            FillNoDataCells fnd = new FillNoDataCells(ref grid, FillNoDataMethod.FULL); fnd.Execute();

            var grid_copy = new float[grid.GetLength(0) * grid.GetLength(1)];

            for (int i = 0; i < grid.GetLength(0); ++i)
                for (int j = 0; j < grid.GetLength(1); ++j)
                    grid_copy[i * grid.GetLength(1) + j] = (float)grid[i, j];
            lastSurface.SetTo(grid_copy);

            Mat kernel;
            for (int i = 1; i <= maxWindowRadius; i += 2)
            {
                kernel = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Ellipse, new Size(i, i), new System.Drawing.Point(-1, -1));
                double elevationThreshold = slopeTolerance * i * cellSize;
                thisSurface = new Mat(new Size(grid.GetLength(0), grid.GetLength(1)), Emgu.CV.CvEnum.DepthType.Cv32F, 1);
                CvInvoke.MorphologyEx(lastSurface, thisSurface, Emgu.CV.CvEnum.MorphOp.Open, kernel, new System.Drawing.Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(255, 0, 255));
                FilterGrid(elevationThreshold);
                lastSurface = thisSurface;
            }

            return grid;
        }

        /// <summary>
        /// Filters the grid, and marks the filtered cells as nodata points
        /// </summary>
        /// <param name="elevationThreshold"></param>
        private void FilterGrid(double elevationThreshold)
        {
            var last = (float[,])lastSurface.GetData();
            var current = (float[,])thisSurface.GetData();
            for (int i = 0; i < lastSurface.Size.Height; i++)
            {
                for (int j = 0; j < thisSurface.Size.Width; j++)
                {
                    if (Math.Abs(last[i, j] - current[i, j]) > elevationThreshold)
                    {
                        grid[i, j] = double.NaN;
                    }
                }
            }
        }
    }*/
}
