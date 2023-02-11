// <copyright file="CHMGenerationTest.cs" company="Eötvös Loránd University (ELTE)">
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