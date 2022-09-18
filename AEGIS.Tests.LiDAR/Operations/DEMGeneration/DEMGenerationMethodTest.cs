using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Operations.DEMGeneration;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.DEMGeneration
{
    [TestFixture]
    public class DEMGenerationMethodTest
    {
        [Test]
        public void CreateGridTest()
        {
            LasPublicHeader header = new LasPublicHeader()
            {
                MaxX = 1,
                MinX = 0,
                MaxY = 1,
                MinY = 0
            };

            var grid = DEMGenerationMethod.CreateGrid(1, header);
            Assert.AreEqual(1, grid.GetLength(0));
            Assert.AreEqual(1, grid.GetLength(1));

            grid = DEMGenerationMethod.CreateGrid(0.5, header);
            Assert.AreEqual(2, grid.GetLength(0));
            Assert.AreEqual(2, grid.GetLength(1));
        }
    }
}