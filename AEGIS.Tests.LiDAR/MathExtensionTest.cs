using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.LiDAR
{
    [TestFixture]
    public class MathExtensionTest
    {
        [Test]
        public void StandardDeviationTest()
        {
            double[] data = new double[] { 1, 2, 3, 4, 5, 6, 7 };
            Assert.AreEqual(2, MathExtension.StandardDeviation(data));
        }

        [Test]
        public void DistanceTest()
        {
            LasPointFormat0 a = new LasPointFormat0(1, 2, 3);
            LasPointFormat0 b = new LasPointFormat0(2, 3, 4);

            Assert.AreEqual(3, MathExtension.DistanceSquared(a, b));
            Assert.AreEqual(Math.Sqrt(3), MathExtension.Distance(a, b));
        }

        [Test]
        public void DistanceXYTest()
        {
            LasPointFormat0 a = new LasPointFormat0(1, 2, 3);
            LasPointFormat0 b = new LasPointFormat0(2, 3, 4);

            Assert.AreEqual(2, MathExtension.DistanceSquaredXY(a, b));
            Assert.AreEqual(Math.Sqrt(2), MathExtension.DistanceXY(a, b));
        }
    }
}