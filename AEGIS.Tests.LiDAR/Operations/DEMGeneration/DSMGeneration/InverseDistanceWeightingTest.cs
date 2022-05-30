using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DSMGeneration;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.DEMGeneration
{
    [TestFixture]
    public class InverseDistanceWeightingTest
    {
        [Test]
        public void IDWTest()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-1, 1, -1, 1, 1, 8), 1);
            octree.Add(new LasPointFormat0(0, 0, 3), new Coordinate(0, 0, 3));
            octree.Add(new LasPointFormat0(1, 1, 4), new Coordinate(1, 1, 4));
            octree.Add(new LasPointFormat0(1, -1, 1), new Coordinate(1, -1, 1));
            octree.Add(new LasPointFormat0(-1, 1, 2), new Coordinate(-1, 1, 2));
            octree.Add(new LasPointFormat0(-1, -1, 6), new Coordinate(-1, -1, 6));

            LasPublicHeader header = new LasPublicHeader()
            {
                MaxX = 1,
                MinX = -1,
                MaxY = 1,
                MinY = -1
            };

            var idw = new InverseDistanceWeighting(ref octree, header, 1, 1, 1);
            var grid = idw.Execute();

            Assert.AreEqual(4.5, grid[0, 0]);
            Assert.AreEqual(2, grid[0, 1]);
            Assert.AreEqual(2.5, grid[1, 0]);
            Assert.AreEqual(3.5, grid[1, 1]);

            octree.Add(new LasPointFormat0(0.5, 0.5, 8), new Coordinate(0.5, 0.5, 8));

            idw = new InverseDistanceWeighting(ref octree, header, 1, 1, 0.5);
            grid = idw.Execute();

            Assert.AreEqual(4.5, grid[0, 0]);
            Assert.AreEqual(2, grid[0, 1]);
            Assert.AreEqual(2.5, grid[1, 0]);
            Assert.AreEqual(5.5, grid[1, 1]);
        }
    }
}