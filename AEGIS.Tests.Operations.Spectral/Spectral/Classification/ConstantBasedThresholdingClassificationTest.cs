/// <copyright file="ConstantBasedThresholdingClassificationTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.Classification;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Classification
{
    [TestFixture]
    public class ConstantBasedThresholdingClassificationTest
    {
        private Mock<IRaster> _raster;
        private Mock<IFloatRaster> _floatRaster;

        [SetUp]
        public void Initialize()
        {
            _raster = new Mock<IRaster>(MockBehavior.Strict);
            _raster.Setup(raster => raster.Factory).Returns(Factory.DefaultInstance<IRasterFactory>());
            _raster.Setup(raster => raster.IsReadable).Returns(true);
            _raster.Setup(raster => raster.NumberOfRows).Returns(20);
            _raster.Setup(raster => raster.NumberOfColumns).Returns(15);
            _raster.Setup(raster => raster.SpectralResolution).Returns(3);
            _raster.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 });
            _raster.Setup(raster => raster.Coordinates).Returns(Enumerable.Repeat(Coordinate.Empty, 4).ToArray());
            _raster.Setup(raster => raster.SpectralRanges).Returns(SpectralRanges.RGB);
            _raster.Setup(raster => raster.Mapper).Returns<RasterMapper>(null);
            _raster.Setup(raster => raster.Representation).Returns(RasterRepresentation.Integer);
            _raster.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                          .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)(rowIndex * columnIndex * bandIndex % 256)));

            _floatRaster = new Mock<IFloatRaster>(MockBehavior.Strict);
            _floatRaster.Setup(raster => raster.Factory).Returns(Factory.DefaultInstance<IRasterFactory>());
            _floatRaster.Setup(raster => raster.IsReadable).Returns(true);
            _floatRaster.Setup(raster => raster.NumberOfRows).Returns(20);
            _floatRaster.Setup(raster => raster.NumberOfColumns).Returns(15);
            _floatRaster.Setup(raster => raster.SpectralResolution).Returns(3);
            _floatRaster.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 });
            _floatRaster.Setup(raster => raster.Coordinates).Returns(Enumerable.Repeat(Coordinate.Empty, 4).ToArray());
            _floatRaster.Setup(raster => raster.SpectralRanges).Returns(SpectralRanges.RGB);
            _floatRaster.Setup(raster => raster.Mapper).Returns<RasterMapper>(null);
            _floatRaster.Setup(raster => raster.Representation).Returns(RasterRepresentation.Floating);
            _floatRaster.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                               .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => rowIndex * columnIndex * bandIndex));
        }

        [TestCase]
        public void ConstantBasedThresholdingClassificationExecuteTest()
        {
            // test case 1: integer raster

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.LowerThresholdBoundary, 100);
            parameters.Add(SpectralOperationParameters.UpperThresholdBoundary, 150);

            ConstantBasedThresholdingClassification operation = new ConstantBasedThresholdingClassification(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_raster.Object), parameters);
            operation.Execute();

            ISpectralGeometry result = operation.Result;

            Assert.AreEqual(_raster.Object.NumberOfRows, result.Raster.NumberOfRows);
            Assert.AreEqual(_raster.Object.NumberOfColumns, result.Raster.NumberOfColumns);
            Assert.AreEqual(_raster.Object.SpectralResolution, result.Raster.SpectralResolution);
            Assert.IsTrue(_raster.Object.RadiometricResolutions.SequenceEqual(result.Raster.RadiometricResolutions));
            Assert.AreEqual(RasterRepresentation.Integer, result.Raster.Representation);

            for (Int32 i = 0; i < result.Raster.SpectralResolution; i++)
                for (Int32 j = 0; j < result.Raster.NumberOfRows; j += 5)
                    for (Int32 k = 0; k < result.Raster.NumberOfColumns; k += 5)
                    {
                        Assert.AreEqual(_raster.Object.GetValue(j, k, i) >= 100 && _raster.Object.GetValue(j, k, i) <= 150 ? 255 : 0, result.Raster.GetValue(j, k, i), 0);
                    }

            // test case 2: floating raster

            operation = new ConstantBasedThresholdingClassification(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_floatRaster.Object), parameters);
            operation.Execute();

            result = operation.Result;

            Assert.AreEqual(_floatRaster.Object.NumberOfRows, result.Raster.NumberOfRows);
            Assert.AreEqual(_floatRaster.Object.NumberOfColumns, result.Raster.NumberOfColumns);
            Assert.AreEqual(_floatRaster.Object.SpectralResolution, result.Raster.SpectralResolution);
            Assert.IsTrue(_floatRaster.Object.RadiometricResolutions.SequenceEqual(result.Raster.RadiometricResolutions));
            Assert.AreEqual(RasterRepresentation.Integer, result.Raster.Representation);

            for (Int32 i = 0; i < result.Raster.SpectralResolution; i++)
                for (Int32 j = 0; j < result.Raster.NumberOfRows; j += 5)
                    for (Int32 k = 0; k < result.Raster.NumberOfColumns; k += 5)
                    {
                        Assert.AreEqual(_floatRaster.Object.GetValue(j, k, i) >= 100 && _floatRaster.Object.GetValue(j, k, i) <= 150 ? 255 : 0, result.Raster.GetValue(j, k, i), 0);
                    }
        }

        [TestCase]
        public void ConstantBasedThresholdingClassificationExecuteForBandTest()
        {
            // test case 1: integer raster

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.LowerThresholdBoundary, 100);
            parameters.Add(SpectralOperationParameters.UpperThresholdBoundary, 150);
            parameters.Add(SpectralOperationParameters.BandIndex, 0);

            ConstantBasedThresholdingClassification operation = new ConstantBasedThresholdingClassification(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_raster.Object), parameters);
            operation.Execute();

            ISpectralGeometry result = operation.Result;

            Assert.AreEqual(_raster.Object.NumberOfRows, result.Raster.NumberOfRows);
            Assert.AreEqual(_raster.Object.NumberOfColumns, result.Raster.NumberOfColumns);
            Assert.AreEqual(1, result.Raster.SpectralResolution);
            Assert.AreEqual(_raster.Object.RadiometricResolutions[0], result.Raster.RadiometricResolutions[0]);
            Assert.AreEqual(RasterRepresentation.Integer, result.Raster.Representation);

            for (Int32 j = 0; j < result.Raster.NumberOfRows; j += 5)
                for (Int32 k = 0; k < result.Raster.NumberOfColumns; k += 5)
                {
                    Assert.AreEqual(_raster.Object.GetValue(j, k, 0) >= 100 && _raster.Object.GetValue(j, k, 0) <= 150 ? 255 : 0, result.Raster.GetValue(j, k, 0), 0);
                }

            // test case 2: floating raster

            operation = new ConstantBasedThresholdingClassification(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_floatRaster.Object), parameters);
            operation.Execute();

            result = operation.Result;

            Assert.AreEqual(_floatRaster.Object.NumberOfRows, result.Raster.NumberOfRows);
            Assert.AreEqual(_floatRaster.Object.NumberOfColumns, result.Raster.NumberOfColumns);
            Assert.AreEqual(1, result.Raster.SpectralResolution);
            Assert.AreEqual(_raster.Object.RadiometricResolutions[0], result.Raster.RadiometricResolutions[0]);
            Assert.AreEqual(RasterRepresentation.Integer, result.Raster.Representation);

            for (Int32 j = 0; j < result.Raster.NumberOfRows; j += 5)
                for (Int32 k = 0; k < result.Raster.NumberOfColumns; k += 5)
                {
                    Assert.AreEqual(_floatRaster.Object.GetValue(j, k, 0) >= 100 && _floatRaster.Object.GetValue(j, k, 0) <= 150 ? 255 : 0, result.Raster.GetValue(j, k, 0), 0);
                }
        }
    }
}
