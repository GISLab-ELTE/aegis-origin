// <copyright file="HistogramEqualizationTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.Common;
using ELTE.AEGIS.Raster;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Common
{
    /// <summary>
    /// Test fixture for the <see cref="HistogramEqualization" /> class.
    /// </summary>
    [TestFixture]
    public class HistogramEqualizationTest
    {
        #region Private fields

        /// <summary>
        /// The mock of the raster.
        /// </summary>
        private Mock<IRaster> _rasterMock;

        /// <summary>
        /// The histogram values for each band of the raster.
        /// </summary>
        private Int32[][] _histogramValues;

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
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)(rowIndex * columnIndex * bandIndex % 256)));
            _rasterMock.Setup(raster => raster.HistogramValues).Returns(ComputeHistogram(_rasterMock.Object));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests operation execution.
        /// </summary>
        [Test]
        public void HistogramEqualizationExecuteTest()
        {
            IGeometryFactory factory = new GeometryFactory();

            // execute for all bands

            HistogramEqualization operation = new HistogramEqualization(factory.CreateSpectralPolygon(_rasterMock.Object), null);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(_rasterMock.Object.NumberOfBands, operation.Result.Raster.NumberOfBands);
            Assert.AreEqual(_rasterMock.Object.RadiometricResolution, operation.Result.Raster.RadiometricResolution);
            Assert.AreEqual(_rasterMock.Object.Format, operation.Result.Raster.Format);

            for (Int32 bandIndex = 0; bandIndex < operation.Result.Raster.NumberOfBands; bandIndex++)
            {
                AssertResultForBand(_rasterMock.Object, bandIndex, operation.Result.Raster, bandIndex);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Asserts the results for the specified band.
        /// </summary>
        /// <param name="source">The source raster.</param>
        /// <param name="sourceBandIndex">The band index of the source raster.</param>
        /// <param name="result">The resulting raster.</param>
        /// <param name="resultBandIndex">The band index of the resulting raster.</param>
        private void AssertResultForBand(IRaster source, Int32 sourceBandIndex, IRaster result, Int32 resultBandIndex)
        {
            Assert.AreEqual(source.HistogramValues[sourceBandIndex].Sum(), result.HistogramValues[resultBandIndex].Sum());

            Assert.Greater(result.HistogramValues[resultBandIndex][0], 0);

            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    Assert.GreaterOrEqual(result.GetValue(rowIndex, columnIndex, resultBandIndex), 0);
                    Assert.LessOrEqual(result.GetValue(rowIndex, columnIndex, resultBandIndex), RasterAlgorithms.RadiometricResolutionMax(source.RadiometricResolution));
                }
        }

        /// <summary>
        /// Computes the histogram of the specified raster.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <returns>The histogram of the raster.</returns>
        private Int32[][] ComputeHistogram(IRaster raster)
        {
            if (_histogramValues == null)
            {
                _histogramValues = new Int32[raster.NumberOfBands][];

                for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                {
                    _histogramValues[bandIndex] = new Int32[1UL << raster.RadiometricResolution];

                    for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                        for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                        {
                            _histogramValues[bandIndex][raster.GetValue(rowIndex, columnIndex, bandIndex)]++;
                        }
                }
            }

            return _histogramValues;
        }

        #endregion
    }
}
