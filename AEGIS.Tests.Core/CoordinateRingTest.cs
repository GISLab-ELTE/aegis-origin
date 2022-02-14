/// <copyright file="CoorindateRingTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Máté Cserép</author>

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
            };
            List<Coordinate> originReversed = new List<Coordinate>(origin);
            originReversed.Reverse();

            List<Coordinate>[] variants = new List<Coordinate>[origin.Count];
            for(Int32 i = 0; i < variants.Length; ++i)
                variants[i] = origin.Skip(i).Concat(origin.Take(i)).ToList();

            CoordinateRing originRing = new CoordinateRing(origin);
            for (Int32 i = 0; i < variants.Length; ++i)
            {
                Assert.IsTrue(originRing.Equals(new CoordinateRing(variants[i])));
            }

            CoordinateRing originRingReversed = new CoordinateRing(originReversed);
            Assert.IsTrue(originRing.Equals(originRingReversed));


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
            originReversed = new List<Coordinate>(origin);
            originReversed.Reverse();

            variants = new List<Coordinate>[origin.Count];
            for (Int32 i = 0; i < variants.Length; ++i)
                variants[i] = origin.Skip(i).Concat(origin.Take(i)).ToList();

            originRing = new CoordinateRing(origin);
            for (Int32 i = 0; i < variants.Length; ++i)
            {
                Assert.IsTrue(originRing.Equals(new CoordinateRing(variants[i])));
            }

            originRingReversed = new CoordinateRing(originReversed);
            Assert.IsTrue(originRing.Equals(originRingReversed));
        }
    }
}
