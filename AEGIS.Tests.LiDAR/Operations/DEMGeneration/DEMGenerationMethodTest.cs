// <copyright file="DEMGenerationMethodTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.LiDAR.Operations.DEMGeneration;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.LiDAR.Operations.DEMGeneration
{
    [TestFixture]
    public class DEMGenerationMethodTest
    {
        [Test]
        public void CreateGridTest()
        {
            LasPublicHeader header = new LasPublicHeader()
            {
                MaxX = 1,
                MinX = 0,
                MaxY = 1,
                MinY = 0
            };

            var grid = DEMGenerationMethod.CreateGrid(1, header);
            Assert.AreEqual(1, grid.GetLength(0));
            Assert.AreEqual(1, grid.GetLength(1));

            grid = DEMGenerationMethod.CreateGrid(0.5, header);
            Assert.AreEqual(2, grid.GetLength(0));
            Assert.AreEqual(2, grid.GetLength(1));
        }
    }
}