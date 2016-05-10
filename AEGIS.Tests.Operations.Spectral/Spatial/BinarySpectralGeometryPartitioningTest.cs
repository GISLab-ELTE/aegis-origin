/// <copyright file="BinarySpectralGeometryPartitioningTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spatial.Partitioning;
using ELTE.AEGIS.Raster;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spatial
{
    /// <summary>
    /// Test fixture for the <see cref="BinarySpectralGeometryPartitioning"/> class.
    /// </summary>
    [TestFixture]
    public class BinarySpectralGeometryPartitioningTest
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
            _rasterMock.Setup(raster => raster.IsMapped).Returns(false);
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(160);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(200);
            _rasterMock.Setup(raster => raster.NumberOfBands).Returns(1);
            _rasterMock.Setup(raster => raster.RadiometricResolution).Returns(8);
            _rasterMock.Setup(raster => raster.Coordinates).Returns(Enumerable.Repeat(Coordinate.Empty, 4).ToArray());
            _rasterMock.Setup(raster => raster.Mapper).Returns<RasterMapper>(null);
            _rasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Floating);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests operation execution.
        /// </summary>
        [Test]
        public void BinarySpectralGeometryPartitioningExecuteTest()
        {
            IGeometryFactory factory = new GeometryFactory();

            Dictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(CommonOperationParameters.BufferValueCount, 20);
            parameters.Add(CommonOperationParameters.NumberOfParts, 16);

            BinarySpectralGeometryPartitioning operation = new BinarySpectralGeometryPartitioning(factory.CreateSpectralPolygon(_rasterMock.Object), parameters);
            operation.Execute();

            IGeometryCollection<IGeometry> result = operation.Result;

            Assert.AreEqual(16, result.Count);
            Assert.IsTrue(result.All(geometry => geometry is ISpectralGeometry));

            foreach (ISpectralGeometry geometry in result)
            {
                Assert.LessOrEqual(geometry.Raster.NumberOfRows, 100);
                Assert.LessOrEqual(geometry.Raster.NumberOfColumns, 120);
            }
        }

        #endregion
    }
}
