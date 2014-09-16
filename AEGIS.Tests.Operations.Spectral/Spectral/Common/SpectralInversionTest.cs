/// <copyright file="SpectralInversionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.Common;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Common
{
    /// <summary>
    /// Test fixture for the <see cref="SpectralInversion"/> class.
    /// </summary>
    [TestFixture]
    public class SpectralInversionTest
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
            _rasterMock.Setup(raster => raster.Factory).Returns(Factory.DefaultInstance<IRasterFactory>());
            _rasterMock.Setup(raster => raster.IsReadable).Returns(true);
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(20);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(15);
            _rasterMock.Setup(raster => raster.NumberOfBands).Returns(3);
            _rasterMock.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 });
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
        /// Test case for operation execution.
        /// </summary>
        [Test]
        public void SpectralInversionExecuteTest()
        {
            // integer values

            SpectralInversion operation = new SpectralInversion(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), null);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(_rasterMock.Object.NumberOfBands, operation.Result.Raster.NumberOfBands);
            Assert.IsTrue(_rasterMock.Object.RadiometricResolutions.SequenceEqual(operation.Result.Raster.RadiometricResolutions));
            Assert.AreEqual(_rasterMock.Object.Format, operation.Result.Raster.Format);

            for (Int32 bandIndex = 0; bandIndex < operation.Result.Raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < operation.Result.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < operation.Result.Raster.NumberOfColumns; columnIndex++)
                    {
                        Assert.AreEqual(_rasterMock.Object.GetValue(rowIndex, columnIndex, bandIndex), 255 - operation.Result.Raster.GetValue(rowIndex, columnIndex, bandIndex));
                    }


            // integer values with specified band

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.BandIndex, 0);

            operation = new SpectralInversion(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), parameters);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(1, operation.Result.Raster.NumberOfBands);
            Assert.AreEqual(_rasterMock.Object.RadiometricResolutions[0], operation.Result.Raster.RadiometricResolutions[0]);
            Assert.AreEqual(_rasterMock.Object.Format, operation.Result.Raster.Format);

            for (Int32 rowIndex = 0; rowIndex < operation.Result.Raster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < operation.Result.Raster.NumberOfColumns; columnIndex++)
                {
                    Assert.AreEqual(_rasterMock.Object.GetValue(rowIndex, columnIndex, 0), 255 - operation.Result.Raster.GetValue(rowIndex, columnIndex, 0));
                }


            // floating point values

            _rasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Floating);

            operation = new SpectralInversion(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), null);
            operation.Execute();

            for (Int32 bandIndex = 0; bandIndex < operation.Result.Raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < operation.Result.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < operation.Result.Raster.NumberOfColumns; columnIndex++)
                    {
                        Assert.AreEqual(_rasterMock.Object.GetFloatValue(rowIndex, columnIndex, bandIndex), -operation.Result.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex));
                    }
        }

        #endregion
    }
}
