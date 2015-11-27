/// <copyright file="ProxyRasterTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Raster;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Raster
{
    /// <summary>
    /// Test fixture for the <see cref="ProxyRaster"/> class.
    /// </summary>
    [TestFixture]
    public class ProxyRasterTest
    {
        #region Private fields

        /// <summary>
        /// The mock of the raster factory.
        /// </summary>
        private Mock<IRasterFactory> _factory;

        /// <summary>
        /// The mock of the raster service.
        /// </summary>
        private Mock<IRasterService> _service;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IRasterService>(MockBehavior.Strict);
            _service.Setup(entity => entity.IsReadable).Returns(true);
            _service.Setup(entity => entity.IsWritable).Returns(true);
            _service.Setup(entity => entity.NumberOfColumns).Returns(15);
            _service.Setup(entity => entity.NumberOfRows).Returns(20);
            _service.Setup(entity => entity.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 });
            _service.Setup(entity => entity.Format).Returns(RasterFormat.Integer);
            _service.Setup(entity => entity.NumberOfBands).Returns(3);
            _service.Setup(entity => entity.SupportedOrders).Returns(new RasterDataOrder[] { RasterDataOrder.RowColumnBand });

            _service.Setup(entity => entity.ReadValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>())).Throws<ArgumentOutOfRangeException>();

            for (Int32 i = 0; i < 3; i++)
                _service.Setup(entity => entity.ReadValue(It.IsInRange(0, 19, Range.Inclusive),
                                                            It.IsInRange(0, 14, Range.Inclusive),
                                                            i)).Returns((UInt32)i);

            _service.Setup(entity => entity.ReadValueSequence(It.IsAny<Int32>(), It.IsAny<Int32>())).Throws<ArgumentOutOfRangeException>();
            _service.Setup(entity => entity.ReadValueSequence(It.IsInRange(0, 19, Range.Inclusive),
                                                                It.IsInRange(0, 14, Range.Inclusive),
                                                                0, 3)).Returns(new UInt32[] { 0, 1, 2 });
            _service.Setup(entity => entity.WriteValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<UInt32>())).Throws<ArgumentOutOfRangeException>();
            _service.Setup(entity => entity.WriteValueSequence(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<UInt32[]>())).Throws<ArgumentOutOfRangeException>();

            _factory = new Mock<IRasterFactory>(MockBehavior.Loose);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void ProxyRasterConstructorTest()
        {
            // successful construction without spectral ranges and mapping
            ProxyRaster raster = new ProxyRaster(_factory.Object, _service.Object, null);

            Assert.IsFalse(raster.IsMapped);
            Assert.IsNull(raster.Mapper);
            Assert.IsTrue(raster.Coordinates.All(coordinate => !coordinate.IsValid));

            Assert.AreEqual(_service.Object.IsReadable, raster.IsReadable);
            Assert.AreEqual(_service.Object.IsWritable, raster.IsWritable);
            Assert.AreEqual(_service.Object.NumberOfColumns, raster.NumberOfColumns);
            Assert.AreEqual(_service.Object.NumberOfRows, raster.NumberOfRows);
            Assert.IsTrue(_service.Object.RadiometricResolutions.SequenceEqual(raster.RadiometricResolutions));
            Assert.AreEqual(_service.Object.Format, raster.Format);
            Assert.AreEqual(_service.Object.NumberOfBands, raster.NumberOfBands);
            Assert.AreEqual(_service.Object.NumberOfBands, raster.Bands.Count);
            Assert.AreEqual(_factory.Object, raster.Factory);

            // successful construction with mapping

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 1000, 1000, 0, 10, 10, 1);

            raster = new ProxyRaster(_factory.Object, _service.Object, mapper);
            Assert.IsTrue(raster.IsMapped);
            Assert.AreEqual(mapper, raster.Mapper);
            Assert.AreEqual(new Coordinate(1000, 1200), raster.Coordinates[0]);
            Assert.AreEqual(new Coordinate(1150, 1200), raster.Coordinates[1]);
            Assert.AreEqual(new Coordinate(1150, 1000), raster.Coordinates[2]);
            Assert.AreEqual(new Coordinate(1000, 1000), raster.Coordinates[3]);

            // argument null exception

            Assert.Throws<ArgumentNullException>(() => { raster = new ProxyRaster(_factory.Object, null, null); });
        }

        /// <summary>
        /// Test case for the <see cref="GetValue"/> method.
        /// </summary>
        [Test]
        public void ProxyRasterGetValueTest()
        {
            // index based

            ProxyRaster raster = new ProxyRaster(_factory.Object, _service.Object, null);

            for (Int32 i = -1; i <= raster.NumberOfBands; i++)
                for (Int32 j = -1; j <= raster.NumberOfRows + 5; j += 5)
                    for (Int32 k = -1; k <= raster.NumberOfColumns + 5; k += 5)
                    {
                        try
                        {
                            Assert.AreEqual(_service.Object.ReadValue(j, k, i), raster.GetValue(j, k, i));
                            Assert.AreEqual(_service.Object.ReadValue(j, k, i), raster[j, k, i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Assert.IsTrue(i < 0 || i >= raster.NumberOfBands ||
                                          j < 0 || j >= raster.NumberOfRows ||
                                          k < 0 || k >= raster.NumberOfColumns);
                        }
                    }

            // coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, 1000, 1000, 0, 10, 10, 1);

            raster = new ProxyRaster(_factory.Object, _service.Object, mapper);

            for (Int32 x = 1000; x < 1000 + 10 * _service.Object.NumberOfColumns; x += 100)
                for (Int32 y = 1000; y < 1000 + 10 * _service.Object.NumberOfRows; y += 100)
                {
                    Assert.AreEqual(_service.Object.ReadValue((x - 1000) / 10, (y - 1000) / 10, 0), raster.GetValue(new Coordinate(x, y), 0));
                    Assert.AreEqual(_service.Object.ReadValue((x - 1000) / 10, (y - 1000) / 10, 0), raster[new Coordinate(x, y), 0]);
                }
        }

        /// <summary>
        /// Test case for the <see cref="GetValues"/> method.
        /// </summary>
        [Test]
        public void ProxyRasterGetValuesTest()
        {
            // index based

            ProxyRaster raster = new ProxyRaster(_factory.Object, _service.Object, null);

            for (Int32 j = 0; j < raster.NumberOfRows; j += 5)
                for (Int32 k = 0; k < raster.NumberOfColumns; k += 5)
                {
                    Assert.IsTrue(_service.Object.ReadValueSequence(j, k, 0, 3).SequenceEqual(raster.GetValues(j, k)));
                    Assert.IsTrue(_service.Object.ReadValueSequence(j, k, 0, 3).SequenceEqual(raster[j, k]));
                }

            // coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, 1000, 1000, 0, 10, 10, 1);

            raster = new ProxyRaster(_factory.Object, _service.Object, mapper);

            for (Int32 x = 1000; x < 1000 + 10 * _service.Object.NumberOfColumns; x += 100)
                for (Int32 y = 1000; y < 1000 + 10 * _service.Object.NumberOfRows; y += 100)
                {
                    Assert.IsTrue(_service.Object.ReadValueSequence((x - 1000) / 10, (y - 1000) / 10, 0, 3).SequenceEqual(raster.GetValues(new Coordinate(x, y))));
                    Assert.IsTrue(_service.Object.ReadValueSequence((x - 1000) / 10, (y - 1000) / 10, 0, 3).SequenceEqual(raster[new Coordinate(x, y)]));
                }
        }

        /// <summary>
        /// Test case for the <see cref="GetNearestValue"/> method.
        /// </summary>
        [Test]
        public void ProxyRasterNearestValueTest()
        {
            // test case 1: index based

            ProxyRaster raster = new ProxyRaster(_factory.Object, _service.Object, null);

            for (Int32 i = 0; i < raster.NumberOfBands; i++)
                for (Int32 j = -1; j <= raster.NumberOfRows + 5; j += 5)
                    for (Int32 k = -1; k <= raster.NumberOfColumns + 5; k += 5)
                    {
                        Assert.AreEqual(_service.Object.ReadValue(Math.Max(0, Math.Min(j, _service.Object.NumberOfRows - 1)),
                                                                  Math.Max(0, Math.Min(j, _service.Object.NumberOfColumns - 1)), i),
                                        raster.GetNearestValue(j, k, i));
                    }

            // test case 2: coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, 1000, 1000, 0, 10, 10, 1);

            raster = new ProxyRaster(_factory.Object, _service.Object, mapper);

            for (Int32 i = 0; i < raster.NumberOfBands; i++)
                for (Int32 x = 1000; x < 1000 + 10 * _service.Object.NumberOfColumns; x += 100)
                    for (Int32 y = 1000; y < 1000 + 10 * _service.Object.NumberOfRows; y += 100)
                    {
                        Assert.AreEqual(_service.Object.ReadValue(Math.Max(0, Math.Min((x - 1000) / 10, _service.Object.NumberOfRows - 1)),
                                                                  Math.Max(0, Math.Min((y - 1000) / 10, _service.Object.NumberOfColumns - 1)), i),
                                        raster.GetNearestValue(new Coordinate(x, y), i));
                    }
        }

        /// <summary>
        /// Test case for the <see cref="SetValue"/> method.
        /// </summary>
        [Test]
        public void ProxyRasterSetValueTest()
        {
            for (Int32 i = 0; i < 3; i++)
                _service.Setup(entity => entity.WriteValue(It.IsInRange(0, 19, Range.Inclusive),
                                                             It.IsInRange(0, 14, Range.Inclusive),
                                                             i, It.IsAny<UInt32>()))
                                                 .Callback(new Action<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex, value) => Assert.AreEqual(bandIndex, value, 0)));

            // test case 1: index based

            ProxyRaster raster = new ProxyRaster(_factory.Object, _service.Object, null);

            for (Int32 i = 0; i < raster.NumberOfBands; i++)
                for (Int32 j = 0; j < raster.NumberOfRows; j += 5)
                    for (Int32 k = 0; k < raster.NumberOfColumns; k += 5)
                    {
                        raster.SetValue(j, k, i, (UInt32)i);
                        raster[j, k, i] = (UInt32)i;
                    }

            // test case 2: coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, 1000, 1000, 0, 10, 10, 1);

            raster = new ProxyRaster(_factory.Object, _service.Object, mapper);

            for (Int32 i = 0; i < raster.NumberOfBands; i++)
                for (Int32 x = 1000; x < 1000 + 10 * _service.Object.NumberOfColumns; x += 100)
                    for (Int32 y = 1000; y < 1000 + 10 * _service.Object.NumberOfRows; y += 100)
                    {
                        raster.SetValue(new Coordinate(x, y), i, (UInt32)i);
                        raster[new Coordinate(x, y), i] = (UInt32)i;
                    }
        }

        /// <summary>
        /// Test case for the <see cref="SetValues"/> method.
        /// </summary>
        [Test]
        public void ProxyRasterSetValuesTest()
        {
            _service.Setup(entity => entity.WriteValueSequence(It.IsInRange(0, 19, Range.Inclusive),
                                                                 It.IsInRange(0, 14, Range.Inclusive),
                                                                 It.IsInRange(0, 2, Range.Inclusive), It.IsAny<UInt32[]>()))
                                             .Callback(new Action<Int32, Int32, Int32, UInt32[]>((rowIndex, columnIndex, bandIndex, values) => Assert.IsTrue(values.SequenceEqual(new UInt32[] { 0, 1, 2 }))));

            // test case 1: index based

            ProxyRaster raster = new ProxyRaster(_factory.Object, _service.Object, null);

            for (Int32 j = 0; j < raster.NumberOfRows; j += 5)
                for (Int32 k = 0; k < raster.NumberOfColumns; k += 5)
                {
                    raster.SetValues(j, k, new UInt32[] { 0, 1, 2 });
                }

            // test case 2: coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, 1000, 1000, 0, 10, 10, 1);

            raster = new ProxyRaster(_factory.Object, _service.Object, mapper);

            for (Int32 x = 1000; x < 1000 + 10 * _service.Object.NumberOfColumns; x += 100)
                for (Int32 y = 1000; y < 1000 + 10 * _service.Object.NumberOfRows; y += 100)
                {
                    raster.SetValues(new Coordinate(x, y), new UInt32[] { 0, 1, 2 });
                    raster[new Coordinate(x, y)] = new UInt32[] { 0, 1, 2 };
                }
        }

        #endregion
    }
}
