/// <copyright file="BoxFilterTranformationTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.Filtering;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Filtering
{
    /// <summary>
    /// Test fixture for the <see cref="BoxFilterTranformation"/> class.
    /// </summary>
    [TestFixture]
    public class BoxFilterTranformationTest
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
            _rasterMock.Setup(raster => raster.Representation).Returns(RasterRepresentation.Integer);
            _rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
            _rasterMock.Setup(raster => raster.GetNearestValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
            _rasterMock.Setup(raster => raster.GetFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => rowIndex * columnIndex * bandIndex));
            _rasterMock.Setup(raster => raster.GetNearestFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => rowIndex * columnIndex * bandIndex));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test method for operation execution.
        /// </summary>
        [Test]
        public void BoxFilterTransformationExecuteTest()
        {
            // integer values with default parameters

            BoxFilterTransformation operation = new BoxFilterTransformation(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), null);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(_rasterMock.Object.NumberOfBands, operation.Result.Raster.NumberOfBands);
            Assert.IsTrue(_rasterMock.Object.RadiometricResolutions.SequenceEqual(operation.Result.Raster.RadiometricResolutions));
            Assert.AreEqual(_rasterMock.Object.Representation, operation.Result.Raster.Representation);

            for (Int32 bandIndex = 0; bandIndex < operation.Result.Raster.NumberOfBands; bandIndex++)
            {
                AssertResultForBand(_rasterMock.Object, bandIndex, operation.Result.Raster, bandIndex, (Int32)SpectralOperationParameters.FilterRadius.DefaultValue);
            }


            // integer values with specified radius

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.FilterRadius, 3);

            operation = new BoxFilterTransformation(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), parameters);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(_rasterMock.Object.NumberOfBands, operation.Result.Raster.NumberOfBands);
            Assert.IsTrue(_rasterMock.Object.RadiometricResolutions.SequenceEqual(operation.Result.Raster.RadiometricResolutions));
            Assert.AreEqual(_rasterMock.Object.Representation, operation.Result.Raster.Representation);

            for (Int32 bandIndex = 0; bandIndex < operation.Result.Raster.NumberOfBands; bandIndex++)
            {
                AssertResultForBand(_rasterMock.Object, bandIndex, operation.Result.Raster, bandIndex, 3);
            }


            // integer values with specified band

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.BandIndex, 1);

            operation = new BoxFilterTransformation(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), parameters);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(1, operation.Result.Raster.NumberOfBands);
            Assert.AreEqual(_rasterMock.Object.RadiometricResolutions[1], operation.Result.Raster.RadiometricResolutions[0]);
            Assert.AreEqual(_rasterMock.Object.Representation, operation.Result.Raster.Representation);

            AssertResultForBand(_rasterMock.Object, 1, operation.Result.Raster, 0, (Int32)SpectralOperationParameters.FilterRadius.DefaultValue);


            // floating values with default parameters

            _rasterMock.Setup(raster => raster.Representation).Returns(RasterRepresentation.Floating);

            operation = new BoxFilterTransformation(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), null);
            operation.Execute();

            for (Int32 bandIndex = 0; bandIndex < operation.Result.Raster.NumberOfBands; bandIndex++)
            {
                AssertResultForBand(_rasterMock.Object, bandIndex, operation.Result.Raster, bandIndex, (Int32)SpectralOperationParameters.FilterRadius.DefaultValue);
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
        /// <param name="filterRadius">The radius of the filter.</param>
        private void AssertResultForBand(IRaster source, Int32 sourceBandIndex, IRaster result, Int32 resultBandIndex, Int32 filterRadius)
        {            
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    Double filteredValue = 0;

                    switch (result.Representation)
                    {
                        case RasterRepresentation.Integer:
                            for (Int32 filterRowIndex = -filterRadius; filterRowIndex <= filterRadius; filterRowIndex++)
                                for (Int32 filterColumnIndex = -filterRadius; filterColumnIndex <= filterRadius; filterColumnIndex++)
                                    filteredValue += source.GetNearestValue(rowIndex + filterRowIndex, columnIndex + filterColumnIndex, sourceBandIndex);

                            filteredValue /= Calculator.Square(2 * filterRadius + 1);

                            Assert.AreEqual((UInt32)filteredValue, result.GetValue(rowIndex, columnIndex, resultBandIndex));
                            break;

                        case RasterRepresentation.Floating:
                            for (Int32 filterRowIndex = -filterRadius; filterRowIndex <= filterRadius; filterRowIndex++)
                                for (Int32 filterColumnIndex = -filterRadius; filterColumnIndex <= filterRadius; filterColumnIndex++)
                                    filteredValue += source.GetNearestFloatValue(rowIndex + filterRowIndex, columnIndex + filterColumnIndex, sourceBandIndex);

                            filteredValue /= Calculator.Square(2 * filterRadius + 1);

                            Assert.AreEqual((UInt32)filteredValue, result.GetFloatValue(rowIndex, columnIndex, resultBandIndex));
                            break;
                    }
                }
        }

        #endregion
    }
}
