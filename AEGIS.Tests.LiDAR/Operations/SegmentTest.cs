// <copyright file="SegmentTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.LiDAR.Operations;
using NUnit.Framework;
using System.Linq;

namespace ELTE.AEGIS.Tests.LiDAR.Operations
{
    [TestFixture]
    public class SegmentTest
    {
        [Test]
        public void InsideTest()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(1, -1, 1, -1, 1, 6), 1);
            LasPointBase[] points = new LasPointBase[]
            {
                new LasPointFormat0(0, 0, 3),
                new LasPointFormat0(1, 1, 4),
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, 2),
                new LasPointFormat0(-1, -1, 6)
            };

            foreach(var point in points)
                octree.Add(point, point.Coordinate);

            Envelope envelope = new Envelope(1, -1, 1, 0, 4, 2);

            var seg = new Segment(ref octree, envelope, SegmentType.INSIDE);
            var res = seg.Execute();
            Assert.AreEqual(3, res.Count);
            var coords = res.Select(x => x.Point).ToHashSet();
            Assert.True(coords.Contains(points[0].Coordinate));
            Assert.True(coords.Contains(points[1].Coordinate));
            Assert.True(coords.Contains(points[3].Coordinate));
        }

        [Test]
        public void OutsideTest()
        {
            PointOctree<LasPointBase> octree = new PointOctree<LasPointBase>(new Envelope(1, -1, 1, -1, 1, 6), 1);
            LasPointBase[] points = new LasPointBase[]
            {
                new LasPointFormat0(0, 0, 3), 
                new LasPointFormat0(1, 1, 4), 
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, 2),
                new LasPointFormat0(-1, -1, 6)
            };

            foreach (var point in points)
                octree.Add(point, point.Coordinate);

            Envelope envelope = new Envelope(1, -1, 1, 0, 4, 2);

            var seg = new Segment(ref octree, envelope, SegmentType.OUTSIDE);
            var res = seg.Execute();
            Assert.AreEqual(2, res.Count);
            var coords = res.Select(x => x.Point).ToHashSet();
            Assert.True(coords.Contains(points[2].Coordinate));
            Assert.True(coords.Contains(points[4].Coordinate));
        }
    }
}