/// <copyright file="BilinearResamplingAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Algorithms.Resampling;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Algorithms.Resampling
{
    /// <summary>
    /// Test fixture for the <see cref="BilinearResamplingAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class BilinearResamplingAlgorithmTest
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
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(10);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(8);
            _rasterMock.Setup(raster => raster.NumberOfBands).Returns(2);
            _rasterMock.Setup(raster => raster.RadiometricResolution).Returns(8);
            _rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
            _rasterMock.Setup(raster => raster.GetValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, UInt32[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (UInt32)(rowIndex * columnIndex * bandIndex + 256) % 256).ToArray()));
            _rasterMock.Setup(raster => raster.GetBoxedValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
            _rasterMock.Setup(raster => raster.GetBoxedValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, UInt32[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (UInt32)(rowIndex * columnIndex * bandIndex + 256) % 256).ToArray()));
            _rasterMock.Setup(raster => raster.GetFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => (rowIndex * columnIndex * bandIndex + 256) % 256));
            _rasterMock.Setup(raster => raster.GetFloatValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Double[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (Double)(rowIndex * columnIndex * bandIndex + 256) % 256).ToArray()));
            _rasterMock.Setup(raster => raster.GetBoxedFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => (rowIndex * columnIndex * bandIndex + 256) % 256));
            _rasterMock.Setup(raster => raster.GetBoxedFloatValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Double[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (Double)(rowIndex * columnIndex * bandIndex + 256) % 256).ToArray()));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for integer value computation.
        /// </summary>
        [Test]
        public void BilinearResamplingAlgorithmComputeTest()
        {
            BilinearResamplingAlgorithm algorithm = new BilinearResamplingAlgorithm(_rasterMock.Object);

            UInt32[] values = new UInt32[4];   

            for (Double rowIndex = 0; rowIndex < _rasterMock.Object.NumberOfRows; rowIndex += 0.1)
                for (Double columnIndex = 0; columnIndex < _rasterMock.Object.NumberOfColumns; columnIndex += 0.1)
                {
                    Int32 rowFloor = (Int32)Math.Floor(rowIndex);
                    Int32 columnFloor = (Int32)Math.Floor(columnIndex);

                    for (Int32 bandIndex = 0; bandIndex < _rasterMock.Object.NumberOfBands; bandIndex++)
                    {
                        values[0] = _rasterMock.Object.GetBoxedValue(rowFloor, columnFloor, bandIndex);
                        values[1] = _rasterMock.Object.GetBoxedValue(rowFloor, columnFloor + 1, bandIndex);
                        values[2] = _rasterMock.Object.GetBoxedValue(rowFloor + 1, columnFloor, bandIndex);
                        values[3] = _rasterMock.Object.GetBoxedValue(rowFloor + 1, columnFloor + 1, bandIndex);

                        UInt32 value = algorithm.Compute(rowIndex, columnIndex, bandIndex);

                        Assert.LessOrEqual(value, values.Max());
                        Assert.GreaterOrEqual(value, values.Min());
                    }
                }
        }

        /// <summary>
        /// Test case for floating value computation.
        /// </summary>
        [Test]
        public void BilinearResamplingAlgorithmComputeFloatTest()
        {
            BilinearResamplingAlgorithm algorithm = new BilinearResamplingAlgorithm(_rasterMock.Object);

            Double[] values = new Double[4];

            for (Double rowIndex = 0; rowIndex < _rasterMock.Object.NumberOfRows; rowIndex += 0.1)
                for (Double columnIndex = 0; columnIndex < _rasterMock.Object.NumberOfColumns; columnIndex += 0.1)
                {
                    Int32 rowFloor = (Int32)Math.Floor(rowIndex);
                    Int32 columnFloor = (Int32)Math.Floor(columnIndex);

                    for (Int32 bandIndex = 0; bandIndex < _rasterMock.Object.NumberOfBands; bandIndex++)
                    {
                        values[0] = _rasterMock.Object.GetBoxedFloatValue(rowFloor, columnFloor, bandIndex);
                        values[1] = _rasterMock.Object.GetBoxedFloatValue(rowFloor, columnFloor + 1, bandIndex);
                        values[2] = _rasterMock.Object.GetBoxedFloatValue(rowFloor + 1, columnFloor, bandIndex);
                        values[3] = _rasterMock.Object.GetBoxedFloatValue(rowFloor + 1, columnFloor + 1, bandIndex);

                        Double value = algorithm.ComputeFloat(rowIndex, columnIndex, bandIndex);

                        Assert.LessOrEqual(value, values.Max());
                        Assert.GreaterOrEqual(value, values.Min());
                    }
                }
        }

        #endregion
    }
}
