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