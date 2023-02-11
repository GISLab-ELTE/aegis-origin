// <copyright file="RandomGridSubsamplingTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.LiDAR.Operations.Subsampling;
using NUnit.Framework;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.Subsampling
{
    [TestFixture]
    public class RandomGridSubsamplingTest
    {
        [Test]
        public void RandomGridTest()
        {
            var points = new List<LasPointBase>()
            {
                new LasPointFormat0(0, 0, 1),
                new LasPointFormat0(1, 1, -1),
                new LasPointFormat0(1, -1, 1),
                new LasPointFormat0(-1, 1, -1),
                new LasPointFormat0(-1, -1, 1)
            };

            var sampling = new RandomGridSubsampling(points, 2, System.Environment.ProcessorCount);
            var res = sampling.Execute();

            Assert.AreEqual(1, res.Count);

            sampling = new RandomGridSubsampling(points, 1, System.Environment.ProcessorCount);
            var res2 = sampling.Execute();

            Assert.True(points.Count <= res2.Count); //There can be multiplicities
        }
    }
}