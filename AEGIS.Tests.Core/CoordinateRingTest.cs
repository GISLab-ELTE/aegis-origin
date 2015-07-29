using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests
{
    [TestFixture]
    public class CoordinateRingTest
    {
        [Test]
        public void CoordinateRingEqualityTest()
        {
            // test case: no duplicate coordinate
            List<Coordinate> origin = new List<Coordinate>()
            {
                new Coordinate(10, 10),
                new Coordinate(20, 10),
                new Coordinate(30, 15),
                new Coordinate(25, 20),
                new Coordinate(15, 20),
                new Coordinate(10, 15),
                new Coordinate(10, 10),
            };

            List<Coordinate>[] variants = new List<Coordinate>[origin.Count];
            for(Int32 i = 0; i < variants.Length; ++i)
                variants[i] = origin.Skip(i).Concat(origin.Take(i)).ToList();

            CoordinateRing originRing = new CoordinateRing(origin);
            for (Int32 i = 0; i < variants.Length; ++i)
            {
                Assert.IsTrue(originRing.Equals(new CoordinateRing(variants[i])));
            }


            // test case: duplication at minimum coordinate
            origin = new List<Coordinate>()
            {
                new Coordinate(10, 0),
                new Coordinate(10, 5),
                new Coordinate(0,  0),
                new Coordinate(5,  15),
                new Coordinate(0,  15),
                new Coordinate(0,  0),
            };

            variants = new List<Coordinate>[origin.Count];
            for (Int32 i = 0; i < variants.Length; ++i)
                variants[i] = origin.Skip(i).Concat(origin.Take(i)).ToList();

            originRing = new CoordinateRing(origin);
            for (Int32 i = 0; i < variants.Length; ++i)
            {
                Assert.IsTrue(originRing.Equals(new CoordinateRing(variants[i])));
            }
        }
    }
}
