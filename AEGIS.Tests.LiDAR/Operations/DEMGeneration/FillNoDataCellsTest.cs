// <copyright file="FillNoDataCellsTest.cs" company="Eötvös Loránd University (ELTE)">
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

using NUnit.Framework;
using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Operations.DEMGeneration;
using System;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.DEMGeneration
{
    [TestFixture]
    public class FillNoDataCellsTest
    {
        [Test]
        public void NoneTest()
        {
            double[,] grid = new[,]
            {
                { 1, 2, 1 },
                { 3, double.NaN, 3},
                { 1, 2, double.NaN}
            };

            var FND = new FillNoDataCells(ref grid, FillNoDataMethod.NONE);
            FND.Execute();

            Assert.IsTrue(double.IsNaN(grid[1, 1]));
            Assert.IsTrue(double.IsNaN(grid[2, 2]));
        }

        [Test]
        public void PartTest()
        {
            double[,] grid = new[,]
            {
                { 1, 2, 1 },
                { 3, double.NaN, 3},
                { 1, 2, double.NaN}
            };

            var FND = new FillNoDataCells(ref grid, FillNoDataMethod.PART);
            FND.Execute();

            Assert.AreEqual(1.857, grid[1, 1], 0.01);
            Assert.AreEqual(2.285, grid[2, 2], 0.01);
        }

        [Test]
        public void FullTest()
        {
            double[,] grid = new[,]
            {
                { 1, 2, 1 },
                { 3, 4, 3},
                { 1, double.NaN, double.NaN}
            };

            var FND = new FillNoDataCells(ref grid, FillNoDataMethod.FULL, 1);
            FND.Execute();

            Assert.AreEqual(2.75, grid[2, 1]);
            Assert.AreEqual(3.25, grid[2, 2]);
        }
    }
}