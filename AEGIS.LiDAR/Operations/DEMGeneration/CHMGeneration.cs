/// <copyright file="CHMGeneration.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Threading;
using System.Threading.Tasks;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration
{
    /// <summary>
    /// Implements a method, which creates a canopy height model from an input DSM and DTM.
    /// </summary>
    public class CHMGeneration
    {
        readonly Double[,] DTM;
        Double[,] DSM;
        Int32 threadCount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="threadCount">The method is multithreaded. With this paramater you can specify how many threads to use.</param>
        /// <param name="DSM">The input DSM.</param>
        /// <param name="DTM">The input DTM.</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public CHMGeneration(Int32 threadCount, Double[,] DSM, Double[,] DTM)
        {
            /*if (DSM == null || DTM == null)
                throw new ArgumentNullException("DSM or DTM is null!");*/
            if (DSM.GetLength(0) != DTM.GetLength(0) || DSM.GetLength(1) != DTM.GetLength(1))
                throw new IndexOutOfRangeException("DSM and DTM must be the same size!");

            this.DSM = (double[,])DSM.Clone();
            this.DTM = DTM;
            this.threadCount = threadCount;
        }

        /// <summary>
        /// Executes the chosen method, which creates a DEM.
        /// </summary>
        /// <returns>The DEM as a raster of elevations.</returns>
        public double[,] Execute()
        {
            Thread[] threads = new Thread[threadCount];
            int length = DSM.GetLength(0) / threadCount;
            int leftover = DSM.GetLength(0) - length * threadCount;

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

            return DSM;
        }

        /// <summary>
        /// Executes the chosen method, which creates a DEM.
        /// </summary>
        /// <returns>The DEM as a raster of elevations.</returns>
        public async Task<Double[,]> ExecuteAsync() => await Task.Run(() => Execute());

        /// <summary>
        /// The method is multithreaded. Each thread will call this function to iterate through the grid array.
        /// </summary>
        /// <param name="from">The staring index of the iteration.</param>
        /// <param name="to">The ending index of the iteration.</param>
        protected void Job(int from, int to)
        {
            for (int i = from; i < to; ++i)
            {
                for (int j = 0; j < DSM.GetLength(1); ++j)
                {
                    if (Double.IsNaN(DSM[i, j]) || Double.IsNaN(DTM[i, j]))
                        DSM[i, j] = Double.NaN;
                    else
                        DSM[i, j] -= DTM[i, j];
                }
            }
        }

    }
}
