/// <copyright file="ProxyRasterFactoryTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class ProxyRasterFactoryTest
    {
        private Mock<ISpectralEntity> _entity;
        private ProxyRasterFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _entity = new Mock<ISpectralEntity>(MockBehavior.Strict);
            _entity.Setup(entity => entity.IsReadable).Returns(true);
            _entity.Setup(entity => entity.IsWritable).Returns(true);
            _entity.Setup(entity => entity.NumberOfColumns).Returns(15);
            _entity.Setup(entity => entity.NumberOfRows).Returns(20);
            _entity.Setup(entity => entity.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 });
            _entity.Setup(entity => entity.Representation).Returns(RasterRepresentation.Integer);
            _entity.Setup(entity => entity.SpectralResolution).Returns(3);
            _entity.Setup(entity => entity.SupportedOrders).Returns(new SpectralDataOrder[] { SpectralDataOrder.RowColumnBand });

            _factory = new ProxyRasterFactory(_entity.Object);
        }


        [TestCase]
        public void ProxyRasterFactoryCreateRasterTest()
        {            
            // test case 1: without mapper

            IRaster raster = _factory.CreateRaster(null);

            Assert.IsInstanceOf<ProxyRaster>(raster);
            Assert.AreEqual(_factory, raster.Factory);
            Assert.AreEqual(_entity.Object.IsReadable, raster.IsReadable);
            Assert.AreEqual(_entity.Object.IsWritable, raster.IsWritable);
            Assert.IsFalse(raster.IsMapped);
            Assert.AreEqual(null, raster.Mapper);
            Assert.AreEqual(_entity.Object.NumberOfColumns, raster.NumberOfColumns);
            Assert.AreEqual(_entity.Object.NumberOfRows, raster.NumberOfRows);
            Assert.IsTrue(_entity.Object.RadiometricResolutions.SequenceEqual(raster.RadiometricResolutions));
            Assert.AreEqual(_entity.Object.Representation, raster.Representation);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.SpectralResolution);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.SpectralRanges.Count);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.Bands.Count);
            Assert.IsTrue(raster.SpectralRanges.All(range => range == null));

            // test case 2: with mapper

            RasterMapper mapper = RasterMapper.FromTransformation(1000, 1000, 0, 10, 10, 1, RasterMapMode.ValueIsCoordinate);

            raster = _factory.CreateRaster(mapper);

            Assert.IsInstanceOf<ProxyRaster>(raster);
            Assert.IsTrue(raster.IsMapped);
            Assert.AreEqual(mapper, raster.Mapper);

            // test case 3: with integer representation

            raster = _factory.CreateRaster(mapper, RasterRepresentation.Integer);

            Assert.IsInstanceOf<ProxyRaster>(raster);
            Assert.IsTrue(raster.IsMapped);
            Assert.AreEqual(mapper, raster.Mapper);
            Assert.AreEqual(RasterRepresentation.Integer, raster.Representation);

            // test case 4: with mapper and spectral ranges

            raster = _factory.CreateRaster(SpectralRanges.RGB, mapper);

            Assert.IsInstanceOf<ProxyRaster>(raster);
            Assert.IsTrue(raster.IsMapped);
            Assert.AreEqual(mapper, raster.Mapper);
            Assert.IsTrue(raster.SpectralRanges.SequenceEqual(SpectralRanges.RGB));

            // test case 5: argument exception

            Assert.Throws<ArgumentException>(() =>
            {
                _entity.Setup(entity => entity.Representation).Returns(RasterRepresentation.Floating);
                raster = _factory.CreateRaster(mapper, RasterRepresentation.Integer);
            });
        }

        [TestCase]
        public void ProxyRasterFactoryCreateFloatRasterTest()
        {
            _entity.Setup(entity => entity.Representation).Returns(RasterRepresentation.Floating);

            // test case 1: without mapper

            IRaster raster = _factory.CreateFloatRaster(null);

            Assert.IsInstanceOf<ProxyFloatRaster>(raster);
            Assert.AreEqual(_factory, raster.Factory);
            Assert.AreEqual(_entity.Object.IsReadable, raster.IsReadable);
            Assert.AreEqual(_entity.Object.IsWritable, raster.IsWritable);
            Assert.IsFalse(raster.IsMapped);
            Assert.AreEqual(null, raster.Mapper);
            Assert.AreEqual(_entity.Object.NumberOfColumns, raster.NumberOfColumns);
            Assert.AreEqual(_entity.Object.NumberOfRows, raster.NumberOfRows);
            Assert.IsTrue(_entity.Object.RadiometricResolutions.SequenceEqual(raster.RadiometricResolutions));
            Assert.AreEqual(_entity.Object.Representation, raster.Representation);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.SpectralResolution);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.SpectralRanges.Count);
            Assert.AreEqual(_entity.Object.SpectralResolution, raster.Bands.Count);
            Assert.IsTrue(raster.SpectralRanges.All(range => range == null));

            // test case 2: with mapper

            RasterMapper mapper = RasterMapper.FromTransformation(1000, 1000, 0, 10, 10, 1, RasterMapMode.ValueIsCoordinate);

            raster = _factory.CreateFloatRaster(mapper);

            Assert.IsInstanceOf<ProxyFloatRaster>(raster);
            Assert.IsTrue(raster.IsMapped);
            Assert.AreEqual(mapper, raster.Mapper);

            // test case 3: with mapper and spectral ranges

            raster = _factory.CreateFloatRaster(SpectralRanges.RGB, mapper);

            Assert.IsInstanceOf<ProxyFloatRaster>(raster);
            Assert.IsTrue(raster.IsMapped);
            Assert.AreEqual(mapper, raster.Mapper);
            Assert.IsTrue(raster.SpectralRanges.SequenceEqual(SpectralRanges.RGB));
        }
    }
}
