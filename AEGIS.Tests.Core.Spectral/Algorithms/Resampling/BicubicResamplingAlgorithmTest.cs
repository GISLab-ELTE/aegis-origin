// <copyright file="BicubicResamplingAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.Tests.Algorithms.Resampling
{
    /// <summary>
    /// Test fixture for the <see cref="BicubicResamplingAlgorithm" /> class.
    /// </summary>
    /// <author>Gréta Bereczki</author>
    [TestFixture]
    public class BicubicResamplingAlgorithmTest
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
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(5);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(5);
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

        /// <summary>
        /// Test case for floating value computation.
        /// </summary>
        [Test]
        public void BicubicResamplingAlgorithmComputeFloatTest()
        {
            BicubicResamplingAlgorithm algorithm = new BicubicResamplingAlgorithm(_rasterMock.Object);

            for (Double rowIndex = 0; rowIndex < _rasterMock.Object.NumberOfRows; rowIndex += 0.1)
                for (Double columnIndex = 0; columnIndex < _rasterMock.Object.NumberOfColumns; columnIndex += 0.1)
                {
                    Int32 rowFloor = (Int32)Math.Floor(rowIndex);
                    Int32 columnFloor = (Int32)Math.Floor(columnIndex);

                    for (Int32 bandIndex = 0; bandIndex < _rasterMock.Object.NumberOfBands; bandIndex++)
                    {
                        Double minValue = Double.MaxValue, maxValue = Double.MinValue;

                        for (Int32 i = rowFloor - 4 + 1; i <= rowFloor + 4; i++)
                            for (Int32 j = columnFloor - 4 + 1; j <= columnFloor + 4; j++)
                            {
                                UInt32 value = _rasterMock.Object.GetBoxedValue(i, j, bandIndex);

                                minValue = Math.Min(minValue, value);
                                maxValue = Math.Max(maxValue, value);
                            }

                        Double strategyValue = algorithm.Compute(rowIndex, columnIndex, bandIndex);

                        Assert.GreaterOrEqual(strategyValue, minValue);
                        Assert.LessOrEqual(strategyValue, maxValue);
                    }
                }
        }
    }
}
