// <copyright file="MathExtensionTest.cs" company="Eötvös Loránd University (ELTE)">
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