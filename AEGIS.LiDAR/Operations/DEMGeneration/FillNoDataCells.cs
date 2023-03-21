// <copyright file="FillNoDataCells.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELTE.AEGIS.LiDAR.Operations.DEMGeneration
{
    /// <summary>
    /// An enum to set the type of the filling.
    /// NONE: Do nothing
    /// PARTIAL: Filling small holes
    /// FULL: Fill every hole
    /// </summary>
    /// <author>Roland Krisztandl</author>
    public enum FillNoDataMethod { NONE, PART, FULL }

    /// <summary>
    /// Implements a method that fills the noData values on a digital elevation model.
    /// </summary>
    /// <author>Roland Krisztandl</author>
    public class FillNoDataCells
    {
        readonly double[,] grid;
        readonly bool[,] visited;
        readonly int maxHoleSize; //if partial, the maximum size of hole to fill
        readonly FillNoDataMethod fillNoDataMethod; //NONE: Do nothing,   PARTIAL: Filling small holes,   FULL: Fill every hole

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="grid">The grid to fill.</param>
        /// <param name="fillNoDataMethod">The type of the fill.</param>
        /// <param name="maxHoleSize">The maximum hole size to fill, if the filling is partial.</param>
        public FillNoDataCells(ref double[,] grid, FillNoDataMethod fillNoDataMethod, int maxHoleSize = 100)
        {
            this.grid = grid;
            this.fillNoDataMethod = fillNoDataMethod;
            this.maxHoleSize = maxHoleSize;
            visited = new bool[grid.GetLength(0), grid.GetLength(1)];
        }

        /// <summary>
        /// Runs the method.
        /// </summary>
        public void Execute()
        {
            if (fillNoDataMethod == FillNoDataMethod.NONE) return;

            for (int i = 0; i < grid.GetLength(0); ++i)
            {
                for (int j = 0; j < grid.GetLength(1); ++j)
                {
                    if (visited[i, j]) continue;
                    if (!Double.IsNaN(grid[i, j])) { visited[i, j] = true; continue; }

                    var list = Exploration(i, j);

                    //if PARTIAL then only fill small holes
                    if (list.Count <= maxHoleSize)
                    {
                        FillSmall(ref list);
                    }
                    else if (fillNoDataMethod == FillNoDataMethod.FULL)
                    {
                        FillBig(ref list);
                    }
                }
            }
        }

        /// <summary>
        /// Runs the method.
        /// </summary>
        public async void ExecuteAsync() => await Task.Run(() => Execute());

        /// <summary>
        /// Checks weather <paramref name="y"/> and <paramref name="x"/> are valid indexes of grid[y][x].
        /// </summary>
        private bool ValidCoords(int y, int x)
        {
            return (y >= 0 && x >= 0 && y < grid.GetLength(0) && x < grid.GetLength(1));
        }

        /// <summary>
        /// Collects the valid neighbors of grid[<paramref name="y"/>][<paramref name="x"/>].
        /// </summary>
        private List<double> ValidNeighbors(int y, int x)
        {
            List<double> neighbors = new List<double>();
            for (int ki = -1; ki < 2; ++ki)
                for (int kj = -1; kj < 2; ++kj)
                    if (ValidCoords(y + ki, x + kj) && !Double.IsNaN(grid[y + ki, x + kj]))
                        neighbors.Add(grid[y + ki, x + kj]);
            return neighbors;
        }

        /// <summary>
        /// Counts the valid neighbors of grid[<paramref name="y"/>][<paramref name="x"/>].
        /// </summary>
        private int CountNeighbors(int y, int x)
        {
            int count = 0;
            for (int ki = -1; ki < 2; ++ki)
                for (int kj = -1; kj < 2; ++kj)
                    if (ValidCoords(y + ki, x + kj) && !Double.IsNaN(grid[y + ki, x + kj]))
                        ++count;
            return count;
        }

        /// <summary>
        /// Sets the value in grid[<paramref name="y"/>][<paramref name="x"/>].
        /// </summary>
        private void SetValue(int y, int x)
        {
            var neighbors = ValidNeighbors(y, x);

            if (neighbors.Count > 0)
                grid[y, x] = neighbors.Average();
            else grid[y, x] = double.NaN;
        }

        /// <summary>
        /// If grid[<paramref name="y"/>][<paramref name="x"/>] is valid, not visited and has noData value appends it to <paramref name="list"/> and <paramref name="queue"/>.
        /// </summary>
        private void Add(int y, int x, ref Queue<Tuple<int, int, int>> queue, ref List<Tuple<int, int, int>> list)
        {
            if (ValidCoords(y, x) && !visited[y, x] && Double.IsNaN(grid[y, x]))
            {
                visited[y, x] = true;
                Tuple<int, int, int> c = new Tuple<int, int, int>(y, x, CountNeighbors(y, x));
                queue.Enqueue(c);
                list.Add(c);
            }
        }

        /// <summary>
        /// Iterative "flood and fill" agorthim, to find every NaN location within the same area.
        /// Start location is grid[<paramref name="i"/>][<paramref name="j"/>].
        /// </summary>
        /// <returns>The List of the coordinates of the NaN location within the same area.</returns>
        private List<Tuple<int, int, int>> Exploration(int i, int j)
        {
            //Item1: Y, Item2: X, Item3: How many non-NaN neighbours grid[X,Y] has
            Queue<Tuple<int, int, int>> queue = new Queue<Tuple<int, int, int>>();
            List<Tuple<int, int, int>> list = new List<Tuple<int, int, int>>();

            Add(i, j, ref queue, ref list);

            //iterative flood and fill
            while (queue.Count != 0)
            {
                var coord = queue.Dequeue();
                int y = coord.Item1, x = coord.Item2;

                Add(y + 1, x, ref queue, ref list);
                Add(y - 1, x, ref queue, ref list);
                Add(y, x + 1, ref queue, ref list);
                Add(y, x - 1, ref queue, ref list);
            }
            return list;
        }

        /// <summary>
        /// Fills the small hole given in the <paramref name="list"/>.
        /// </summary>
        private void FillSmall(ref List<Tuple<int, int, int>> list)
        {
            var listOrdered = list.OrderByDescending(t => t.Item3).ToArray();

            foreach (var coord in listOrdered)
            {
                SetValue(coord.Item1, coord.Item2);
            }
        }

        /// <summary>
        /// Fills the big hole given in the <paramref name="list"/>.
        /// </summary>
        private void FillBig(ref List<Tuple<int, int, int>> list)
        {
            Queue<Tuple<int, int, int>> queue = new Queue<Tuple<int, int, int>>(list.OrderByDescending(t => t.Item3));
            List<double> neighbors = new List<double>();
            Tuple<int, int, int> coord;
            int count;

            while (queue.Count != 0)
            {
                coord = queue.Dequeue();
                while (coord.Item3 > 0 && queue.Count != 0)
                {
                    SetValue(coord.Item1, coord.Item2);

                    coord = queue.Dequeue();
                    if (Double.IsNaN(grid[coord.Item1, coord.Item2]))
                        queue.Enqueue(coord);
                }

                list.Clear();
                foreach (var a in queue)
                {
                    count = CountNeighbors(a.Item1, a.Item2);
                    list.Add(new Tuple<int, int, int>(a.Item1, a.Item2, count));
                }

                queue = new Queue<Tuple<int, int, int>>(list.OrderByDescending(t => t.Item3));
            }
        }
    }
}
