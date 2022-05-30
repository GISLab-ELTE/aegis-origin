using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using ELTE.AEGIS.LiDAR.Operations;
using NUnit.Framework;
using System.Linq;

namespace ELTE.AEGIS.Tests.LiDAR.Operations
{
    [TestFixture]
    public class SegmentTest
    {
        [Test]
        public void InsideTest()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(1, -1, 1, -1, 1, 6), 1);
            LasPointBase[] points = new LasPointBase[]
            {
                new LasPointFormat0(0, 0, 3),
                new LasPointFormat0(1, 1, 4),
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, 2),
                new LasPointFormat0(-1, -1, 6)
            };

            foreach(var point in points)
                octree.Add(point, point.Coordinate);

            Envelope envelope = new Envelope(1, -1, 1, 0, 4, 2);

            var seg = new Segment(ref octree, envelope, SegmentType.INSIDE);
            var res = seg.Execute();
            Assert.AreEqual(3, res.Count);
            var coords = res.Select(x => x.Point).ToHashSet();
            Assert.True(coords.Contains(points[0].Coordinate));
            Assert.True(coords.Contains(points[1].Coordinate));
            Assert.True(coords.Contains(points[3].Coordinate));
        }

        [Test]
        public void OutsideTest()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(1, -1, 1, -1, 1, 6), 1);
            LasPointBase[] points = new LasPointBase[]
            {
                new LasPointFormat0(0, 0, 3), 
                new LasPointFormat0(1, 1, 4), 
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, 2),
                new LasPointFormat0(-1, -1, 6)
            };

            foreach (var point in points)
                octree.Add(point, point.Coordinate);

            Envelope envelope = new Envelope(1, -1, 1, 0, 4, 2);

            var seg = new Segment(ref octree, envelope, SegmentType.OUTSIDE);
            var res = seg.Execute();
            Assert.AreEqual(2, res.Count);
            var coords = res.Select(x => x.Point).ToHashSet();
            Assert.True(coords.Contains(points[2].Coordinate));
            Assert.True(coords.Contains(points[4].Coordinate));
        }
    }
}