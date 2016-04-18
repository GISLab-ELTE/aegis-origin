/// <copyright file="GaborFilterOperationTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
using ELTE.AEGIS.Operations.Spectral.Filtering;
using ELTE.AEGIS.Raster;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Filtering
{
    /// <summary>
    /// Test fixture for the <see cref="GaborFilterOperation" /> class.
    /// </summary>
    [TestFixture]
    public class GaborFilterOperationTest
    {
        #region Private fields

        /// <summary>
        /// The mock of the raster.
        /// </summary>
        private Mock<IRaster> _rasterMock;

        /// <summary>
        /// The radiometric resolution of the mock object.
        /// </summary>
        private Int32 _radiometricResolution;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _radiometricResolution = 8;
            _rasterMock = new Mock<IRaster>(MockBehavior.Strict);
            _rasterMock.Setup(raster => raster.Factory).Returns(new RasterFactory());
            _rasterMock.Setup(raster => raster.IsReadable).Returns(true);
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(20);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(15);
            _rasterMock.Setup(raster => raster.NumberOfBands).Returns(3);
            _rasterMock.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { _radiometricResolution, _radiometricResolution, _radiometricResolution });
            _rasterMock.Setup(raster => raster.Coordinates).Returns(Enumerable.Repeat(Coordinate.Empty, 4).ToArray());
            _rasterMock.Setup(raster => raster.Mapper).Returns<RasterMapper>(null);
            _rasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Integer);
            _rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
            _rasterMock.Setup(raster => raster.GetNearestValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
            _rasterMock.Setup(raster => raster.GetFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => (rowIndex * columnIndex * bandIndex + 256) % 256));
            _rasterMock.Setup(raster => raster.GetNearestFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => (rowIndex * columnIndex * bandIndex + 256) % 256));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests operation execution.
        /// </summary>
        [Test]
        public void GaborFilterOperationExecuteTest()
        {
            IGeometryFactory factory = new GeometryFactory();

            // integer values with default parameters

            GaborFilterOperation operation = new GaborFilterOperation(factory.CreateSpectralPolygon(_rasterMock.Object), null);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(_rasterMock.Object.NumberOfBands, operation.Result.Raster.NumberOfBands);
            Assert.IsTrue(_rasterMock.Object.RadiometricResolutions.SequenceEqual(operation.Result.Raster.RadiometricResolutions));
            Assert.AreEqual(_rasterMock.Object.Format, operation.Result.Raster.Format);

            for (Int32 bandIndex = 0; bandIndex < operation.Result.Raster.NumberOfBands; bandIndex++)
            {
                for (Int32 rowIndex = 0; rowIndex < operation.Result.Raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < operation.Result.Raster.NumberOfColumns; columnIndex++)
                    {
                        Assert.IsTrue(0 <= operation.Result.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex));
                        Assert.IsTrue(Math.Pow(2,_radiometricResolution) >= operation.Result.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex));
                    }
                }
            }                     
        }

        #endregion        
    }
}
