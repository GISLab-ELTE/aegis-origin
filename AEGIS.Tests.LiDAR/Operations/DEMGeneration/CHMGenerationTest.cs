using ELTE.AEGIS.LiDAR.Operations.DEMGeneration;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.DEMGeneration
{
    [TestFixture]
    public class CHMGenerationTest
    {
        [Test]
        public void CHMTest()
        {
            var DTM = new double[,] { { 1, 3 }, { Double.NaN, 1 } };
            var DSM = new double[,] { { 4, 5 }, { 6, Double.NaN } };

            var method = new CHMGeneration(1, DSM, DTM);
            var res = method.Execute();

            Assert.AreEqual(3, res[0, 0]);
            Assert.AreEqual(2, res[0, 1]);
            Assert.AreEqual(Double.NaN, res[1, 0]);
            Assert.AreEqual(Double.NaN, res[1, 1]);
        }

        [Test]
        public void CHMExceptionTest()
        {
            var DTM = new double[,] { { 1, 1 }, { 2, 2 } };
            var DSM = new double[,] { { 1, 1 }, { 2, 2 }, { 1, 1 } };

            Assert.Throws<IndexOutOfRangeException>(delegate
            {
                var method = new CHMGeneration(1, DSM, DTM);
            });
        }
    }
}