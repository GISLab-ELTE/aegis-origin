// <copyright file="InverseDistanceWeightingTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class InverseDistanceWeightingTest
    {
        [Test]
        public void IDWTest()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-1, 1, -1, 1, 1, 8), 1);
            octree.Add(new LasPointFormat0(0, 0, 3), new Coordinate(0, 0, 3));
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

            var idw = new InverseDistanceWeighting(ref octree, header, 1, 1, 1);
            var grid = idw.Execute();

            Assert.AreEqual(4.5, grid[0, 0]);
            Assert.AreEqual(2, grid[0, 1]);
            Assert.AreEqual(2.5, grid[1, 0]);
            Assert.AreEqual(3.5, grid[1, 1]);

            octree.Add(new LasPointFormat0(0.5, 0.5, 8), new Coordinate(0.5, 0.5, 8));

            idw = new InverseDistanceWeighting(ref octree, header, 1, 1, 0.5);
            grid = idw.Execute();

            Assert.AreEqual(4.5, grid[0, 0]);
            Assert.AreEqual(2, grid[0, 1]);
            Assert.AreEqual(2.5, grid[1, 0]);
            Assert.AreEqual(5.5, grid[1, 1]);
        }
    }
}