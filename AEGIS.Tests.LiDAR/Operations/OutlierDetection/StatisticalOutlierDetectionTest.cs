// <copyright file="StatisticalOutlierDetectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.OutlierDetection
{
    [TestFixture]
    public class StatisticalOutlierDetectionTest
    {
        [Test]
        public void SORNeighborTest1()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-2, 2, -2, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(-1, 2, 0), new Coordinate(-1, 2, 0));
            octree.Add(new LasPointFormat0(-2, 1, 0), new Coordinate(-2, 1, 0));
            octree.Add(new LasPointFormat0(-2, -1, 0), new Coordinate(-2, -1, 0));
            octree.Add(new LasPointFormat0(-1, -2, 0), new Coordinate(-1, -2, 0));
            octree.Add(new LasPointFormat0(1, -2, 0), new Coordinate(1, -2, 0));
            octree.Add(new LasPointFormat0(2, -1, 0), new Coordinate(2, -1, 0));
            octree.Add(new LasPointFormat0(2, 1, 0), new Coordinate(2, 1, 0));
            octree.Add(new LasPointFormat0(1, 2, 0), new Coordinate(1, 2, 0));

            var sorTest = new StatisticalOutlierDetection(ref octree, 1, 8, 1);
            var outliers = sorTest.Execute().ToHashSet();

            Assert.AreEqual(8, outliers.Count);
        }

        [Test]
        public void SORNeighborTest2()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-2, 2, -2, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(-1, 2, 0), new Coordinate(-1, 2, 0));
            octree.Add(new LasPointFormat0(-2, 1, 0), new Coordinate(-2, 1, 0));
            octree.Add(new LasPointFormat0(-2, -1, 0), new Coordinate(-2, -1, 0));
            octree.Add(new LasPointFormat0(-1, -2, 0), new Coordinate(-1, -2, 0));
            octree.Add(new LasPointFormat0(1, -2, 0), new Coordinate(1, -2, 0));
            octree.Add(new LasPointFormat0(2, -1, 0), new Coordinate(2, -1, 0));
            octree.Add(new LasPointFormat0(2, 1, 0), new Coordinate(2, 1, 0));
            octree.Add(new LasPointFormat0(1, 2, 0), new Coordinate(1, 2, 0));

            var sorTest = new StatisticalOutlierDetection(ref octree, 1, 3, 1);
            var outliers = sorTest.Execute();

            Assert.AreEqual(1, outliers.Count);
        }

        [Test]
        public void SORSDFactorTest1()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-2, 4, -2, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(-1, 2, 0), new Coordinate(-1, 2, 0));
            octree.Add(new LasPointFormat0(-2, 1, 0), new Coordinate(-2, 1, 0));
            octree.Add(new LasPointFormat0(-2, -1, 0), new Coordinate(-2, -1, 0));
            octree.Add(new LasPointFormat0(-1, -2, 0), new Coordinate(-1, -2, 0));
            octree.Add(new LasPointFormat0(1, -2, 0), new Coordinate(1, -2, 0));
            octree.Add(new LasPointFormat0(2, -1, 0), new Coordinate(2, -1, 0));
            octree.Add(new LasPointFormat0(2, 1, 0), new Coordinate(2, 1, 0));
            octree.Add(new LasPointFormat0(1, 2, 0), new Coordinate(1, 2, 0));
            octree.Add(new LasPointFormat0(4, 0, 0), new Coordinate(4, 0, 0));

            var sorTest = new StatisticalOutlierDetection(ref octree, 1, 3, 1.8);
            var outliers = sorTest.Execute();

            Assert.AreEqual(2, outliers.Count);
        }

        [Test]
        public void SORSDFactorTest2()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(-2, 4, -2, 2, 0, 0), 1);
            octree.Add(new LasPointFormat0(0, 0, 0), new Coordinate(0, 0, 0));
            octree.Add(new LasPointFormat0(-1, 2, 0), new Coordinate(-1, 2, 0));
            octree.Add(new LasPointFormat0(-2, 1, 0), new Coordinate(-2, 1, 0));
            octree.Add(new LasPointFormat0(-2, -1, 0), new Coordinate(-2, -1, 0));
            octree.Add(new LasPointFormat0(-1, -2, 0), new Coordinate(-1, -2, 0));
            octree.Add(new LasPointFormat0(1, -2, 0), new Coordinate(1, -2, 0));
            octree.Add(new LasPointFormat0(2, -1, 0), new Coordinate(2, -1, 0));
            octree.Add(new LasPointFormat0(2, 1, 0), new Coordinate(2, 1, 0));
            octree.Add(new LasPointFormat0(1, 2, 0), new Coordinate(1, 2, 0));
            octree.Add(new LasPointFormat0(4, 0, 0), new Coordinate(4, 0, 0));

            var sorTest = new StatisticalOutlierDetection(ref octree, 1, 3, 2);
            var outliers = sorTest.Execute();

            Assert.AreEqual(1, outliers.Count);
        }
    }
}