/// <copyright file="ProxyFloatRasterTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Raster.Proxy;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Raster.Proxy
{
    [TestFixture]
    public class ProxyFloatRasterTest
    {
        private Mock<IRasterFactory> _factory;
        private Mock<ISpectralEntity> _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new Mock<ISpectralEntity>(MockBehavior.Strict);
            _entity.Setup(entity => entity.IsReadable).Returns(true);
            _entity.Setup(entity => entity.IsWritable).Returns(true);
            _entity.Setup(entity => entity.NumberOfColumns).Returns(15);
            _entity.Setup(entity => entity.NumberOfRows).Returns(20);
            _entity.Setup(entity => entity.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 });
            _entity.Setup(entity => entity.Representation).Returns(RasterRepresentation.Floating);
            _entity.Setup(entity => entity.SpectralResolution).Returns(3);
            _entity.Setup(entity => entity.SupportedOrders).Returns(new SpectralDataOrder[] { SpectralDataOrder.RowColumnBand });

            _entity.Setup(entity => entity.ReadFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>())).Throws<ArgumentOutOfRangeException>();

            for (Int32 i = 0; i < 3; i++)
                _entity.Setup(entity => entity.ReadFloatValue(It.IsInRange(0, 19, Range.Inclusive),
                                                                 It.IsInRange(0, 14, Range.Inclusive),
                                                                 i)).Returns((Double)i);

            _entity.Setup(entity => entity.ReadFloatValueSequence(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>())).Throws<ArgumentOutOfRangeException>();
            _entity.Setup(entity => entity.ReadFloatValueSequence(It.IsInRange(0, 19, Range.Inclusive),
                                                                     It.IsInRange(0, 14, Range.Inclusive),
                                                                     0, 3)).Returns(new Double[] { 0, 1, 2 });
            _entity.Setup(entity => entity.WriteValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Double>())).Throws<ArgumentOutOfRangeException>();
            _entity.Setup(entity => entity.WriteValueSequence(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Double[]>())).Throws<ArgumentOutOfRangeException>();

            _factory = new Mock<IRasterFactory>(MockBehavior.Loose);
        }

        [TestCase]
        public void ProxyFloatRasterConstructorTest()
        {
            // test case 1: successful construction without spectral ranges and mapping
            ProxyFloatRaster raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, null);

            Assert.IsFalse(raster.IsMapped);
            Assert.IsNull(raster.Mapper);
            Assert.IsTrue(raster.Coordinates.All(coordinate => coordinate.Equals(Coordinate.Empty)));

            Assert.AreEqual(_entity.Object.IsReadable, raster.IsReadable);
            Assert.AreEqual(_entity.Object.IsWritable, raster.IsWritable);
            Assert.AreEqual(_entity.Object.NumberOfColumns, raster.NumberOfColumns);
            Assert.AreEqual(_entity.Object.NumberOfRows, raster.NumberOfRows);
            Assert.IsTrue(_entity.Object.RadiometricResolutions.SequenceEqual(raster.RadiometricResolutions));
            Assert.AreEqual(_entity.Object.Representation, raster.Representation);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.SpectralResolution);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.SpectralRanges.Count);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.Bands.Count);
            Assert.IsTrue(raster.SpectralRanges.All(range => range == null));
            Assert.AreEqual(_factory.Object, raster.Factory);

            // test case 2: sucessful construction with spectral ranges

            raster = new ProxyFloatRaster(_factory.Object, _entity.Object, SpectralRanges.RGB, null);

            Assert.AreEqual(_entity.Object.SpectralResolution, raster.SpectralRanges.Count);
            Assert.IsTrue(raster.SpectralRanges.SequenceEqual(SpectralRanges.RGB));

            // test case 3: sucessful construction with mapping

            RasterMapper mapper = RasterMapper.FromTransformation(1000, 1000, 0, 10, 10, 1, RasterMapMode.ValueIsCoordinate);

            raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, mapper);
            Assert.IsTrue(raster.IsMapped);
            Assert.AreEqual(mapper, raster.Mapper);
            Assert.AreEqual(new Coordinate(1000, 1000), raster.Coordinates[0]);
            Assert.AreEqual(new Coordinate(1200, 1150), raster.Coordinates[1]);
            Assert.AreEqual(new Coordinate(1200, 1000), raster.Coordinates[2]);
            Assert.AreEqual(new Coordinate(1000, 1150), raster.Coordinates[3]);

            // test case 4: argument null exception

            Assert.Throws<ArgumentNullException>(() => { raster = new ProxyFloatRaster(_factory.Object, null, null, null); });


            // test case 5: argument exception

            Assert.Throws<ArgumentException>(() => { raster = new ProxyFloatRaster(_factory.Object, _entity.Object, new SpectralRange[] { SpectralRanges.Blue }, null); });
            Assert.Throws<ArgumentException>(() => 
            {
                _entity.Setup(entity => entity.Representation).Returns(RasterRepresentation.Integer);
                raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, null); 
            });        
        }

        [TestCase]
        public void ProxyFloatRasterGetValueTest()
        {
            // test case 1: index based

            ProxyFloatRaster raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, null);

            for (Int32 i = -1; i <= raster.SpectralResolution; i++)
                for (Int32 j = -1; j <= raster.NumberOfRows + 5; j += 5)
                    for (Int32 k = -1; k <= raster.NumberOfColumns + 5; k += 5)
                    {
                        try
                        {
                            Assert.AreEqual(_entity.Object.ReadFloatValue(j, k, i), raster.GetValue(j, k, i));
                            Assert.AreEqual(_entity.Object.ReadFloatValue(j, k, i), raster[j, k, i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Assert.IsTrue(i < 0 || i >= raster.SpectralResolution ||
                                          j < 0 || j >= raster.NumberOfRows ||
                                          k < 0 || k >= raster.NumberOfColumns);
                        }
                    }

            // test case 2: coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(1000, 1000, 0, 10, 10, 1, RasterMapMode.ValueIsCoordinate);

            raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, mapper);

            for (Int32 x = 1000; x < 1000 + 10 * _entity.Object.NumberOfColumns; x += 100)
                for (Int32 y = 1000; y < 1000 + 10 * _entity.Object.NumberOfRows; y += 100)
                {
                    Assert.AreEqual(_entity.Object.ReadFloatValue((x - 1000) / 10, (y - 1000) / 10, 0), raster.GetValue(new Coordinate(x, y), 0));
                    Assert.AreEqual(_entity.Object.ReadFloatValue((x - 1000) / 10, (y - 1000) / 10, 0), raster[new Coordinate(x, y), 0]);
                }
        }

        [TestCase]
        public void ProxyFloatRasterGetValuesTest()
        {
            // test case 1: index based

            ProxyFloatRaster raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, null);

            for (Int32 j = 0; j < raster.NumberOfRows; j += 5)
                for (Int32 k = 0; k < raster.NumberOfColumns; k += 5)
                {
                    Assert.IsTrue(_entity.Object.ReadFloatValueSequence(j, k, 0, 3).SequenceEqual(raster.GetValues(j, k)));
                    Assert.IsTrue(_entity.Object.ReadFloatValueSequence(j, k, 0, 3).SequenceEqual(raster[j, k]));
                }

            // test case 2: coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(1000, 1000, 0, 10, 10, 1, RasterMapMode.ValueIsCoordinate);

            raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, mapper);

            for (Int32 x = 1000; x < 1000 + 10 * _entity.Object.NumberOfColumns; x += 100)
                for (Int32 y = 1000; y < 1000 + 10 * _entity.Object.NumberOfRows; y += 100)
                {
                    Assert.IsTrue(_entity.Object.ReadFloatValueSequence((x - 1000) / 10, (y - 1000) / 10, 0, 3).SequenceEqual(raster.GetValues(new Coordinate(x, y))));
                    Assert.IsTrue(_entity.Object.ReadFloatValueSequence((x - 1000) / 10, (y - 1000) / 10, 0, 3).SequenceEqual(raster[new Coordinate(x, y)]));
                }
        }

        [TestCase]
        public void ProxyFloatRasterNearestValueTest()
        {
            // test case 1: index based

            ProxyFloatRaster raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, null);

            for (Int32 i = 0; i < raster.SpectralResolution; i++)
                for (Int32 j = -1; j <= raster.NumberOfRows + 5; j += 5)
                    for (Int32 k = -1; k <= raster.NumberOfColumns + 5; k += 5)
                    {
                        Assert.AreEqual(_entity.Object.ReadFloatValue(Math.Max(0, Math.Min(j, _entity.Object.NumberOfRows - 1)),
                                                                       Math.Max(0, Math.Min(j, _entity.Object.NumberOfColumns - 1)), i),
                                        raster.GetNearestValue(j, k, i));
                    }

            // test case 2: coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(1000, 1000, 0, 10, 10, 1, RasterMapMode.ValueIsCoordinate);

            raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, mapper);

            for (Int32 i = 0; i < raster.SpectralResolution; i++)
                for (Int32 x = 1000; x < 1000 + 10 * _entity.Object.NumberOfColumns; x += 100)
                    for (Int32 y = 1000; y < 1000 + 10 * _entity.Object.NumberOfRows; y += 100)
                    {
                        Assert.AreEqual(_entity.Object.ReadFloatValue(Math.Max(0, Math.Min((x - 1000) / 10, _entity.Object.NumberOfRows - 1)),
                                                                       Math.Max(0, Math.Min((y - 1000) / 10, _entity.Object.NumberOfColumns - 1)), i),
                                        raster.GetNearestValue(new Coordinate(x, y), i));
                    }
        }

        [TestCase]
        public void ProxyFloatRasterSetValueTest()
        {
            for (Int32 i = 0; i < 3; i++)
                _entity.Setup(entity => entity.WriteValue(It.IsInRange(0, 19, Range.Inclusive),
                                                             It.IsInRange(0, 14, Range.Inclusive),
                                                             i, It.IsAny<Double>()))
                                                 .Callback(new Action<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex, value) => Assert.AreEqual(bandIndex, value, 0)));

            // test case 1: index based

            ProxyFloatRaster raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, null);

            for (Int32 i = 0; i < raster.SpectralResolution; i++)
                for (Int32 j = 0; j < raster.NumberOfRows; j += 5)
                    for (Int32 k = 0; k < raster.NumberOfColumns; k += 5)
                    {
                        raster.SetValue(j, k, i, (Double)i);
                        raster[j, k, i] = (Double)i;
                    }

            // test case 2: coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(1000, 1000, 0, 10, 10, 1, RasterMapMode.ValueIsCoordinate);

            raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, mapper);

            for (Int32 i = 0; i < raster.SpectralResolution; i++)
                for (Int32 x = 1000; x < 1000 + 10 * _entity.Object.NumberOfColumns; x += 100)
                    for (Int32 y = 1000; y < 1000 + 10 * _entity.Object.NumberOfRows; y += 100)
                    {
                        raster.SetValue(new Coordinate(x, y), i, (Double)i);
                        raster[new Coordinate(x, y), i] = (Double)i;
                    }
        }

        [TestCase]
        public void ProxyFloatRasterSetValuesTest()
        {
            _entity.Setup(entity => entity.WriteValueSequence(It.IsInRange(0, 19, Range.Inclusive),
                                                                 It.IsInRange(0, 14, Range.Inclusive),
                                                                 It.IsInRange(0, 2, Range.Inclusive), It.IsAny<Double[]>()))
                                             .Callback(new Action<Int32, Int32, Int32, Double[]>((rowIndex, columnIndex, bandIndex, values) => Assert.IsTrue(values.SequenceEqual(new Double[] { 0, 1, 2 }))));


            // test case 1: index based

            ProxyFloatRaster raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, null);

            for (Int32 j = 0; j < raster.NumberOfRows; j += 5)
                for (Int32 k = 0; k < raster.NumberOfColumns; k += 5)
                {
                    raster.SetValues(j, k, new Double[] { 0, 1, 2 });
                    raster[j, k] = new Double[] { 0, 1, 2 };
                }

            // test case 2: coordinate based

            RasterMapper mapper = RasterMapper.FromTransformation(1000, 1000, 0, 10, 10, 1, RasterMapMode.ValueIsCoordinate);

            raster = new ProxyFloatRaster(_factory.Object, _entity.Object, null, mapper);

            for (Int32 x = 1000; x < 1000 + 10 * _entity.Object.NumberOfColumns; x += 100)
                for (Int32 y = 1000; y < 1000 + 10 * _entity.Object.NumberOfRows; y += 100)
                {
                    raster.SetValues(new Coordinate(x, y), new Double[] { 0, 1, 2 });
                    raster[new Coordinate(x, y)] = new Double[] { 0, 1, 2 };
                }
        }
    }
}
