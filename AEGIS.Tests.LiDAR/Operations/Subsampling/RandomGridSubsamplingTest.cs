using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Operations.Subsampling;
using NUnit.Framework;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.Subsampling
{
    [TestFixture]
    public class RandomGridSubsamplingTest
    {
        [Test]
        public void RandomGridTest()
        {
            var points = new List<LasPointBase>()
            {
                new LasPointFormat0(0, 0, 1),
                new LasPointFormat0(1, 1, -1),
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, -1),
                new LasPointFormat0(-1, -1, 1)
            };

            var sampling = new RandomGridSubsampling(points, 2, System.Environment.ProcessorCount);
            var res = sampling.Execute();

            Assert.AreEqual(1, res.Count);

            sampling = new RandomGridSubsampling(points, 1, System.Environment.ProcessorCount);
            var res2 = sampling.Execute();

            Assert.True(points.Count <= res2.Count); //There can be multiplicities
        }
    }
}