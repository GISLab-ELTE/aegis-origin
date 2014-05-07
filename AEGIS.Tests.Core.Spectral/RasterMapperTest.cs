/// <copyright file="RasterMapperTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests
{
    [TestFixture]
    public class RasterMapperTest
    {
        [TestCase]
        public void RasterMapperFromTransformationTest()
        {
            RasterMapper mapper = RasterMapper.FromTransformation(300, 1000, 0, 10, 3, 1, RasterMapMode.ValueIsCoordinate);

            Assert.AreEqual(new Coordinate(300, 1000), mapper.TieCoordinate);
            Assert.AreEqual(RasterMapMode.ValueIsCoordinate, mapper.Mode);
            Assert.AreEqual(10, mapper.ColumnSize);
            Assert.AreEqual(3, mapper.RowSize);
            Assert.AreEqual(new CoordinateVector(10, 0), mapper.RowVector);
            Assert.AreEqual(new CoordinateVector(0, 3), mapper.ColumnVector);
        }


        [TestCase]
        public void RasterMapperMapCoordinateTest()
        {
            // test case 1: value is coordinate

            RasterMapper mapper = RasterMapper.FromTransformation(300, 1000, 0, 10, 3, 1, RasterMapMode.ValueIsCoordinate);

            for (Int32 i = -100; i < 100; i += 3)
                for (Int32 j = -100; j < 100; j += 3)
                {
                    Coordinate coordinate = mapper.MapCoordinate(i, j);

                    Assert.AreEqual(300 + i * 10, coordinate.X);
                    Assert.AreEqual(1000 + j * 3, coordinate.Y);
                }

            // test case 2: value is area

            mapper = RasterMapper.FromTransformation(300, 1000, 0, 10, 3, 1, RasterMapMode.ValueIsArea);

            for (Int32 i = -100; i < 100; i += 3)
                for (Int32 j = -100; j < 100; j += 3)
                {
                    Coordinate coordinate = mapper.MapCoordinate(i, j);

                    Assert.AreEqual(305 + i * 10, coordinate.X);
                    Assert.AreEqual(1001.5 + j * 3, coordinate.Y);
                }
        }

        [TestCase]
        public void RasterMapperMapRasterTest()
        {
            // test case 1: value is coordinate

            RasterMapper mapper = RasterMapper.FromTransformation(300, 1000, 0, 10, 3, 1, RasterMapMode.ValueIsCoordinate);

            for (Int32 i = -100; i < 100; i += 3)
                for (Int32 j = -100; j < 100; j += 3)
                {
                    Int32 rowIndex, columnIndex;
                    mapper.MapRaster(new Coordinate(300 + i * 10, 1000 + j * 3), out rowIndex, out columnIndex);

                    Assert.AreEqual(i, rowIndex);
                    Assert.AreEqual(j, columnIndex);
                }

            // test case 2: value is area

            mapper = RasterMapper.FromTransformation(300, 1000, 0, 10, 3, 1, RasterMapMode.ValueIsArea);

            for (Int32 i = -100; i < 100; i += 3)
                for (Int32 j = -100; j < 100; j += 3)
                {
                    Int32 rowIndex, columnIndex;
                    mapper.MapRaster(new Coordinate(305 + i * 10, 1001.5 + j * 3), out rowIndex, out columnIndex);

                    Assert.AreEqual(i, rowIndex);
                    Assert.AreEqual(j, columnIndex);
                }
        }
    }
}
