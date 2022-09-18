using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Operations.Subsampling;
using NUnit.Framework;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.Subsampling
{
    [TestFixture]
    public class GridSubsamplingTest
    {
        [Test]
        public void GridTest()
        {
            var points = new List<LasPointBase>()
            {
                new LasPointFormat0(0, 0, 1),
                new LasPointFormat0(1, 1, -1),
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, -1),
                new LasPointFormat0(-1, -1, 1)
            };

            var sampling = new GridSubsampling(points, 2, System.Environment.ProcessorCount);
            var res = sampling.Execute();

            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(points[0], res[0]);
        }
    }
}