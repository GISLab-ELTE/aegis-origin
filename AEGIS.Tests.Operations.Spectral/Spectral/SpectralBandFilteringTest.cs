/// <copyright file="SpectralBandFilteringTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Raster;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral
{
    /// <summary>
    /// Test fixture for the <see cref="ExponentialTransformation" /> class.
    /// </summary>
    [TestFixture]
    public class SpectralBandFilteringTest
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
            _rasterMock.Setup(raster => raster.Dimensions).Returns(new RasterDimensions(3, 20, 15, 8));
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(20);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(15);
            _rasterMock.Setup(raster => raster.NumberOfBands).Returns(3);
            _rasterMock.Setup(raster => raster.RadiometricResolution).Returns(8);
            _rasterMock.Setup(raster => raster.Coordinates).Returns(Enumerable.Repeat(Coordinate.Empty, 4).ToArray());
            _rasterMock.Setup(raster => raster.Mapper).Returns<RasterMapper>(null);
            _rasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Integer);
            _rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests operation execution.
        /// </summary>
        [Test]
        public void SpectralBandFilteringExecuteTest()
        {
            ISpectralGeometry source = new GeometryFactory().CreateSpectralPolygon(_rasterMock.Object);

            Dictionary<OperationParameter, Object> parameters;
            SpectralBandFiltering operation;
            IRaster result;

            // individual band indices
            for (Int32 bandIndex = 0; bandIndex < _rasterMock.Object.NumberOfBands; bandIndex++)
            {
                parameters = new Dictionary<OperationParameter, Object>() { { SpectralOperationParameters.BandIndex, bandIndex } };
                operation = new SpectralBandFiltering(source, parameters);

                // without execution
                result = operation.Result.Raster;

                Assert.AreEqual(_rasterMock.Object.NumberOfRows, result.NumberOfRows);
                Assert.AreEqual(_rasterMock.Object.NumberOfColumns, result.NumberOfColumns);
                Assert.AreEqual(1, result.NumberOfBands);
                Assert.AreEqual(_rasterMock.Object.RadiometricResolution, result.RadiometricResolution);
                Assert.AreEqual(RasterFormat.Integer, result.Format);

                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                    {
                        Assert.AreEqual(_rasterMock.Object.GetValue(rowIndex, columnIndex, bandIndex), result.GetValue(rowIndex, columnIndex, 0));
                    }


                // with execution
                operation.Execute();
                result = operation.Result.Raster;
                
                Assert.AreEqual(1, result.NumberOfBands);

                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                    {
                        Assert.AreEqual(_rasterMock.Object.GetValue(rowIndex, columnIndex, bandIndex), result.GetValue(rowIndex, columnIndex, 0));
                    }
            }


            // multiple band indices

            parameters = new Dictionary<OperationParameter, Object>() { { SpectralOperationParameters.BandIndices, new Int32[] { 0, 1 } } };
            operation = new SpectralBandFiltering(source, parameters);

            // without execution
            result = operation.Result.Raster;
            
            Assert.AreEqual(2, result.NumberOfBands);

            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    Assert.AreEqual(_rasterMock.Object.GetValue(rowIndex, columnIndex, 0), result.GetValue(rowIndex, columnIndex, 0));
                    Assert.AreEqual(_rasterMock.Object.GetValue(rowIndex, columnIndex, 1), result.GetValue(rowIndex, columnIndex, 1));
                }



            // errors
            Assert.Throws<ArgumentException>(() =>
            {
                parameters = new Dictionary<OperationParameter, Object>() { { SpectralOperationParameters.BandIndex, -1 } };
                operation = new SpectralBandFiltering(source, parameters);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                parameters = new Dictionary<OperationParameter, Object>() { { SpectralOperationParameters.BandIndex, _rasterMock.Object.NumberOfBands } };
                operation = new SpectralBandFiltering(source, parameters);
            });
            
            Assert.Throws<ArgumentException>(() =>
            {
                parameters = new Dictionary<OperationParameter, Object>() { { SpectralOperationParameters.BandIndices, new Int32[] { } } };
                operation = new SpectralBandFiltering(source, parameters);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                parameters = new Dictionary<OperationParameter, Object>() { { SpectralOperationParameters.BandIndices, new Int32[] { -1, 0, 1 } } };
                operation = new SpectralBandFiltering(source, parameters);
            });
        }

        #endregion
    }
}
