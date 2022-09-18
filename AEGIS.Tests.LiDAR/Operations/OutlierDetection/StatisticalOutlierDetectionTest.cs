using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using ELTE.AEGIS.LiDAR.Operations.OutlierDetection;
using NUnit.Framework;
using System.Linq;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.OutlierDetection
{
    [TestFixture]
    public class StatisticalOutlierDetectionTest
    {
        [Test]
        public void SORNeighborTest1()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-2, 2, -2, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(-1, 2, 0), new Coordinate(-1, 2, 0));
            octree.Add(new LasPointFormat0(-2, 1, 0), new Coordinate(-2, 1, 0));
            octree.Add(new LasPointFormat0(-2, -1, 0), new Coordinate(-2, -1, 0));
            octree.Add(new LasPointFormat0(-1, -2, 0), new Coordinate(-1, -2, 0));
            octree.Add(new LasPointFormat0(1, -2, 0), new Coordinate(1, -2, 0));
            octree.Add(new LasPointFormat0(2, -1, 0), new Coordinate(2, -1, 0));
            octree.Add(new LasPointFormat0(2, 1, 0), new Coordinate(2, 1, 0));
            octree.Add(new LasPointFormat0(1, 2, 0), new Coordinate(1, 2, 0));

            var sorTest = new StatisticalOutlierDetection(ref octree, 1, 8, 1);
            var outliers = sorTest.Execute().ToHashSet();

            Assert.AreEqual(8, outliers.Count);
        }

        [Test]
        public void SORNeighborTest2()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-2, 2, -2, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(-1, 2, 0), new Coordinate(-1, 2, 0));
            octree.Add(new LasPointFormat0(-2, 1, 0), new Coordinate(-2, 1, 0));
            octree.Add(new LasPointFormat0(-2, -1, 0), new Coordinate(-2, -1, 0));
            octree.Add(new LasPointFormat0(-1, -2, 0), new Coordinate(-1, -2, 0));
            octree.Add(new LasPointFormat0(1, -2, 0), new Coordinate(1, -2, 0));
            octree.Add(new LasPointFormat0(2, -1, 0), new Coordinate(2, -1, 0));
            octree.Add(new LasPointFormat0(2, 1, 0), new Coordinate(2, 1, 0));
            octree.Add(new LasPointFormat0(1, 2, 0), new Coordinate(1, 2, 0));

            var sorTest = new StatisticalOutlierDetection(ref octree, 1, 3, 1);
            var outliers = sorTest.Execute();

            Assert.AreEqual(1, outliers.Count);
        }

        [Test]
        public void SORSDFactorTest1()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-2, 4, -2, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(-1, 2, 0), new Coordinate(-1, 2, 0));
            octree.Add(new LasPointFormat0(-2, 1, 0), new Coordinate(-2, 1, 0));
            octree.Add(new LasPointFormat0(-2, -1, 0), new Coordinate(-2, -1, 0));
            octree.Add(new LasPointFormat0(-1, -2, 0), new Coordinate(-1, -2, 0));
            octree.Add(new LasPointFormat0(1, -2, 0), new Coordinate(1, -2, 0));
            octree.Add(new LasPointFormat0(2, -1, 0), new Coordinate(2, -1, 0));
            octree.Add(new LasPointFormat0(2, 1, 0), new Coordinate(2, 1, 0));
            octree.Add(new LasPointFormat0(1, 2, 0), new Coordinate(1, 2, 0));
            octree.Add(new LasPointFormat0(4, 0, 0), new Coordinate(4, 0, 0));

            var sorTest = new StatisticalOutlierDetection(ref octree, 1, 3, 1.8);
            var outliers = sorTest.Execute();

            Assert.AreEqual(2, outliers.Count);
        }

        [Test]
        public void SORSDFactorTest2()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-2, 4, -2, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(-1, 2, 0), new Coordinate(-1, 2, 0));
            octree.Add(new LasPointFormat0(-2, 1, 0), new Coordinate(-2, 1, 0));
            octree.Add(new LasPointFormat0(-2, -1, 0), new Coordinate(-2, -1, 0));
            octree.Add(new LasPointFormat0(-1, -2, 0), new Coordinate(-1, -2, 0));
            octree.Add(new LasPointFormat0(1, -2, 0), new Coordinate(1, -2, 0));
            octree.Add(new LasPointFormat0(2, -1, 0), new Coordinate(2, -1, 0));
            octree.Add(new LasPointFormat0(2, 1, 0), new Coordinate(2, 1, 0));
            octree.Add(new LasPointFormat0(1, 2, 0), new Coordinate(1, 2, 0));
            octree.Add(new LasPointFormat0(4, 0, 0), new Coordinate(4, 0, 0));

            var sorTest = new StatisticalOutlierDetection(ref octree, 1, 3, 2);
            var outliers = sorTest.Execute();

            Assert.AreEqual(1, outliers.Count);
        }
    }
}