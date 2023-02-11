// <copyright file="NeighborCountOutlierDetectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.LiDAR.Operations.OutlierDetection;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.OutlierDetection
{
    [TestFixture]
    public class NeighborCountOutlierDetectionTest
    {
        [Test]
        public void NCTest1()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-1, 2, -1, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(1, 1, 0), new Coordinate(1, 1, 0));
            octree.Add(new LasPointFormat0(1, -1, 0), new Coordinate(1, -1, 0));
            octree.Add(new LasPointFormat0(-1, 1, 0), new Coordinate(-1, 1, 0));
            octree.Add(new LasPointFormat0(-1, -1, 0), new Coordinate(-1, -1, 0));
            octree.Add(new LasPointFormat0(2, 2, 0), new Coordinate(2, 2, 0));

            var ncTest = new NeighborCountOutlierDetection(ref octree, 1, 4, 3);
            var outliers = ncTest.Execute();

            Assert.AreEqual(0, outliers.Count);
        }

        [Test]
        public void NCTest2()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-1, 2, -1, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(1, 1, 0), new Coordinate(1, 1, 0));
            octree.Add(new LasPointFormat0(1, -1, 0), new Coordinate(1, -1, 0));
            octree.Add(new LasPointFormat0(-1, 1, 0), new Coordinate(-1, 1, 0));
            octree.Add(new LasPointFormat0(-1, -1, 0), new Coordinate(-1, -1, 0));
            octree.Add(new LasPointFormat0(2, 2, 0), new Coordinate(2, 2, 0));

            var ncTest = new NeighborCountOutlierDetection(ref octree, 1, 4, 1.5);
            var outliers = ncTest.Execute();

            Assert.AreEqual(5, outliers.Count);
        }

        [Test]
        public void NCTest3()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-1, 2, -1, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(1, 1, 0), new Coordinate(1, 1, 0));
            octree.Add(new LasPointFormat0(1, -1, 0), new Coordinate(1, -1, 0));
            octree.Add(new LasPointFormat0(-1, 1, 0), new Coordinate(-1, 1, 0));
            octree.Add(new LasPointFormat0(-1, -1, 0), new Coordinate(-1, -1, 0));
            octree.Add(new LasPointFormat0(2, 2, 0), new Coordinate(2, 2, 0));

            var ncTest = new NeighborCountOutlierDetection(ref octree, 1, 3, 1.5);
            var outliers = ncTest.Execute();

            Assert.AreEqual(4, outliers.Count);
        }
    }
}