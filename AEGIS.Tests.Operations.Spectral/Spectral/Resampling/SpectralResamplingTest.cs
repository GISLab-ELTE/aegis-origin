// <copyright file="SpectralResamplingTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms.Resampling;
using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.Resampling;
using ELTE.AEGIS.Raster;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Resampling
{
    /// <summary>
    /// Test fixture for the <see cref="SpectralResampling" /> class.
    /// </summary>
    [TestFixture]
    public class SpectralResamplingTest
    {
        #region Private fields

        /// <summary>
        /// The mock of the raster.
        /// </summary>
        private Mock<IRaster> _rasterMock;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _rasterMock = new Mock<IRaster>(MockBehavior.Strict);
            _rasterMock.Setup(raster => raster.Factory).Returns(new RasterFactory());
            _rasterMock.Setup(raster => raster.IsReadable).Returns(true);
            _rasterMock.Setup(raster => raster.IsMapped).Returns(false);
            _rasterMock.Setup(raster => raster.Dimensions).Returns(new RasterDimensions(3, 200, 150, 8));
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(200);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(150);
            _rasterMock.Setup(raster => raster.NumberOfBands).Returns(3);
            _rasterMock.Setup(raster => raster.RadiometricResolution).Returns(8);
            _rasterMock.Setup(raster => raster.Coordinates).Returns(Enumerable.Repeat(Coordinate.Empty, 4).ToArray());
            _rasterMock.Setup(raster => raster.Mapper).Returns<RasterMapper>(null);
            _rasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Integer);
            _rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (Byte)(rowIndex * columnIndex * bandIndex)));
            _rasterMock.Setup(raster => raster.GetFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => rowIndex * columnIndex * bandIndex));
            _rasterMock.Setup(raster => raster.GetNearestValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (Byte)(rowIndex * columnIndex * bandIndex)));
            _rasterMock.Setup(raster => raster.GetNearestValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, UInt32[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (UInt32)(Byte)(rowIndex * columnIndex * bandIndex)).ToArray()));
            _rasterMock.Setup(raster => raster.GetNearestFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => rowIndex * columnIndex * bandIndex + 256));
            _rasterMock.Setup(raster => raster.GetNearestFloatValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Double[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (Double)(rowIndex * columnIndex * bandIndex)).ToArray()));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for operation execution.
        /// </summary>
        [Test]
        public void SpectralResamplingExecuteTest()
        {
            IGeometryFactory factory = new GeometryFactory();

            // integer values

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.RasterResamplingAlgorithmType, typeof(NearestNeighborResamplingAlgorithm));
            parameters.Add(SpectralOperationParameters.NumberOfRows, 20);
            parameters.Add(SpectralOperationParameters.NumberOfColumns, 10);

            SpectralResampling operation = new SpectralResampling(factory.CreateSpectralPolygon(_rasterMock.Object), parameters);
            operation.Execute();


            ISpectralGeometry result = operation.Result;

            Assert.AreEqual(20, result.Raster.NumberOfRows);
            Assert.AreEqual(10, result.Raster.NumberOfColumns);
            Assert.AreEqual(_rasterMock.Object.NumberOfBands, result.Raster.NumberOfBands);
            Assert.AreEqual(_rasterMock.Object.RadiometricResolution, result.Raster.RadiometricResolution);
            Assert.AreEqual(RasterFormat.Integer, result.Raster.Format);

            NearestNeighborResamplingAlgorithm strategy = new NearestNeighborResamplingAlgorithm(_rasterMock.Object);

            for (Int32 bandIndex = 0; bandIndex < result.Raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < result.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < result.Raster.NumberOfColumns; columnIndex++)
                    {
                        Int32 sourceRowIndex = (Int32)(Math.Floor((rowIndex + 0.5) * _rasterMock.Object.NumberOfRows / 20));
                        Int32 sourceColumnIndex = (Int32)(Math.Floor((columnIndex + 0.5) * _rasterMock.Object.NumberOfColumns / 10));

                        Assert.AreEqual(_rasterMock.Object.GetValue(sourceRowIndex, sourceColumnIndex, bandIndex), result.Raster.GetValue(rowIndex, columnIndex, bandIndex));
                    }


            // floating point values

            _rasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Floating);

            operation = new SpectralResampling(factory.CreateSpectralPolygon(_rasterMock.Object), parameters);
            operation.Execute();

            result = operation.Result;

            for (Int32 bandIndex = 0; bandIndex < result.Raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < result.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < result.Raster.NumberOfColumns; columnIndex++)
                    {
                        Int32 sourceRowIndex = (Int32)(Math.Floor((rowIndex + 0.5) * _rasterMock.Object.NumberOfRows / 20));
                        Int32 sourceColumnIndex = (Int32)(Math.Floor((columnIndex + 0.5) * _rasterMock.Object.NumberOfColumns / 10));

                        Assert.AreEqual(_rasterMock.Object.GetFloatValue(sourceRowIndex, sourceColumnIndex, bandIndex), result.Raster.GetValue(rowIndex, columnIndex, bandIndex));
                    }
        }

        #endregion
    }
}
