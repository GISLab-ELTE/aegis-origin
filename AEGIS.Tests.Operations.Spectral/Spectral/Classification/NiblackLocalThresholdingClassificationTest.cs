/// <copyright file="NiblackLocalThresholdingClassificationTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gábor Balázs Butkay</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations.Spectral.Classification;
using ELTE.AEGIS.Raster;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Classification
{
    /// <summary>
    /// Test fixture for the <see cref="NiblackLocalThresholdingClassification" /> class.
    /// </summary>
    [TestFixture]
    public class NiblackLocalThresholdingClassificationTest
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
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)(rowIndex * columnIndex * bandIndex % 256)));
            _rasterMock.Setup(raster => raster.GetFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => rowIndex * columnIndex * bandIndex));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests operation execution.
        /// </summary>
        [Test]
        public void NiblackLocalThresholdingClassificationExecuteTest()
        {
            IGeometryFactory factory = new GeometryFactory();

            NiblackLocalThresholdingClassification operation = new NiblackLocalThresholdingClassification(factory.CreateSpectralPolygon(_rasterMock.Object), null);
            operation.Execute();

            ISpectralGeometry result = operation.Result;

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, result.Raster.NumberOfColumns);
            Assert.AreEqual(_rasterMock.Object.NumberOfBands, result.Raster.NumberOfBands);
            Assert.AreEqual(_rasterMock.Object.RadiometricResolution, result.Raster.RadiometricResolution);
            Assert.AreEqual(RasterFormat.Integer, result.Raster.Format);

            for (Int32 bandIndex = 0; bandIndex < result.Raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < result.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < result.Raster.NumberOfColumns; columnIndex++)
                    {
                        Assert.IsTrue(result.Raster.GetValue(rowIndex, columnIndex, bandIndex) == 0 ||
                            result.Raster.GetValue(rowIndex, columnIndex, bandIndex) == 255);
                    }
        }

        #endregion
    }
}
