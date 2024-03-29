﻿// <copyright file="TestUtils.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Operations
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Geometry;
    using ELTE.AEGIS.Raster;
    using Moq;

    public static class TestUtils
    {
        /// <summary>
        /// Creates a mocked spectral geometry.
        /// </summary>
        /// <param name="values">The raster values.</param>
        /// <returns>A spectral geometry with a mocked raster.</returns>
        public static ISpectralGeometry CreateRasterGeometryMock(Int32[,] values)
        {
            List<Coordinate> coordinates = new List<Coordinate>()
            {
                new Coordinate(0, 0),
                new Coordinate(0, values.GetLength(1) - 1),
                new Coordinate(values.GetLength(0) - 1, 0),
                new Coordinate(values.GetLength(0) - 1, values.GetLength(1) - 1)
            };

            Mock<IRaster> rasterMock = new Mock<IRaster>();
            rasterMock.Setup(raster => raster.NumberOfRows).Returns(values.GetLength(0));
            rasterMock.Setup(raster => raster.NumberOfColumns).Returns(values.GetLength(1));
            rasterMock.Setup(raster => raster.NumberOfBands).Returns(1);
            rasterMock.Setup(raster => raster.Factory).Returns(new RasterFactory());
            rasterMock.Setup(raster => raster.Coordinates).Returns(coordinates);
            rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), 0)).Returns<Int32, Int32, Int32>((row, column, band) => (UInt32)values[row, column]);

            return new GeometryFactory().CreateSpectralPolygon(rasterMock.Object);
        }

        /// <summary>
        /// Creates a mocked spectral geometry.
        /// </summary>
        /// <param name="values">The raster values.</param>
        /// <returns>A spectral geometry with a mocked raster.</returns>
        public static ISpectralGeometry CreateRasterGeometryMock(Int32[,,] values)
        {
            List<Coordinate> coordinates = new List<Coordinate>()
            {
                new Coordinate(0, 0),
                new Coordinate(0, values.GetLength(1) - 1),
                new Coordinate(values.GetLength(0) - 1, 0),
                new Coordinate(values.GetLength(0) - 1, values.GetLength(1) - 1)
            };

            Mock<IRaster> rasterMock = new Mock<IRaster>();
            rasterMock.Setup(raster => raster.NumberOfRows).Returns(values.GetLength(0));
            rasterMock.Setup(raster => raster.NumberOfColumns).Returns(values.GetLength(1));
            rasterMock.Setup(raster => raster.NumberOfBands).Returns(values.GetLength(2));
            rasterMock.Setup(raster => raster.Factory).Returns(new RasterFactory());
            rasterMock.Setup(raster => raster.Coordinates).Returns(coordinates);
            rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), 0)).Returns<Int32, Int32, Int32>((row, column, band) => (UInt32)values[row, column, band]);

            return new GeometryFactory().CreateSpectralPolygon(rasterMock.Object);
        }
    }
}
