// <copyright file="MinimumHeightMapTest.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Indexes;
using ELTE.AEGIS.LiDAR.Operations.DEMGeneration.DSMGeneration;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.DEMGeneration
{
    [TestFixture]
    public class MinimumHeightMapTest
    {
        [Test]
        public void MinHMTest1()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-1, 1, -1, 1, 1, 6), 4);
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

            var method = new MinimumHeightMap(ref octree, header, 1, 1, 0.5);
            var grid = method.Execute();

            Assert.AreEqual(6, grid[0, 0]);
            Assert.AreEqual(1, grid[0, 1]);
            Assert.AreEqual(2, grid[1, 0]);
            Assert.AreEqual(4, grid[1, 1]);
        }

        [Test]
        public void MinHMTest2()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-1, 1, -1, 1, 1, 16), 4);
            octree.Add(new LasPointFormat0(1, 1, 4), new Coordinate(1, 1, 4));
            octree.Add(new LasPointFormat0(1, -1, 1), new Coordinate(1, -1, 1));
            octree.Add(new LasPointFormat0(-1, 1, 2), new Coordinate(-1, 1, 2));
            octree.Add(new LasPointFormat0(-1, -1, 6), new Coordinate(-1, -1, 6));
            octree.Add(new LasPointFormat0(1, 1, 14), new Coordinate(1, 1, 14));
            octree.Add(new LasPointFormat0(1, -1, 11), new Coordinate(1, -1, 11));
            octree.Add(new LasPointFormat0(-1, 1, 12), new Coordinate(-1, 1, 12));
            octree.Add(new LasPointFormat0(-1, -1, 16), new Coordinate(-1, -1, 16));

            LasPublicHeader header = new LasPublicHeader()
            {
                MaxX = 1,
                MinX = -1,
                MaxY = 1,
                MinY = -1
            };

            var method = new MinimumHeightMap(ref octree, header, 1, 1, 0.5);
            var grid = method.Execute();

            Assert.AreEqual(6, grid[0, 0]);
            Assert.AreEqual(1, grid[0, 1]);
            Assert.AreEqual(2, grid[1, 0]);
            Assert.AreEqual(4, grid[1, 1]);
        }
    }
}