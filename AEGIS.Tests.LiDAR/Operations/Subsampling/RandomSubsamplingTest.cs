using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Operations.Subsampling;
using NUnit.Framework;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.Subsampling
{
    [TestFixture]
    public class RandomSubsamplingTest
    {
        [Test]
        public void EmptyTest()
        {
            var points = new List<LasPointBase>();

            var sampling = new RandomSubsampling(points, 0);
            var res = sampling.Execute();

            Assert.AreEqual(0, res.Count);
        }

        [Test]
        public void MoreTest()
        {
            var points = new List<LasPointBase>()
            {
                new LasPointFormat0(0, 0, 3),
                new LasPointFormat0(1, 1, 4),
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, 2),
                new LasPointFormat0(-1, -1, 6)
            };

            var sampling = new RandomSubsampling(points, 75);
            var res = sampling.Execute();

            Assert.AreEqual(points.Count, res.Count);
            Assert.True(points.Equals(res));
        }

        [Test]
        public void RandomNumTest()
        {
            var points = new List<LasPointBase>()
            {
                new LasPointFormat0(0, 0, 3),
                new LasPointFormat0(1, 1, 4),
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, 2),
                new LasPointFormat0(-1, -1, 6)
            };

            var sampling = new RandomSubsampling(points, 3);
            var res = sampling.Execute();

            Assert.AreEqual(3, res.Count);
        }

    }
}