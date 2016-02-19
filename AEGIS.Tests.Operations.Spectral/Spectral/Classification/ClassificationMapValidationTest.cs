/// <copyright file="ClassificationMapClassificationTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gréta Bereczki</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.Classification;
using ELTE.AEGIS.Raster;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Classification
{
    /// <summary>
    /// Test fixture for the <see cref="ClassificationMapClassification"/> class.
    /// </summary>
    [TestFixture]
    public class ClassificationMapValidationTest
    {
        #region Private fields

        /// <summary>
        /// The mock of the source raster.
        /// </summary>
        private Mock<IRaster> _sourceRasterMock;

        /// <summary>
        /// The mock of the reference raster.
        /// </summary>
        private Mock<IRaster> _referenceRasterMock;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            List<Coordinate> coordinates = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(0, 4),
                new Coordinate(4, 0),
                new Coordinate(4, 4)
            };

            RasterMapper mapper = RasterMapper.FromCoordinates(RasterMapMode.ValueIsArea,
                new RasterCoordinate(0, 0, 0, 0),
                new RasterCoordinate(4, 0, 0, 4),
                new RasterCoordinate(4, 4, 4, 4),
                new RasterCoordinate(0, 4, 4, 0));

            _sourceRasterMock = new Mock<IRaster>(MockBehavior.Strict);
            _sourceRasterMock.Setup(raster => raster.Factory).Returns(new RasterFactory());
            _sourceRasterMock.Setup(raster => raster.Coordinates).Returns(coordinates);
            _sourceRasterMock.Setup(raster => raster.IsReadable).Returns(true);
            _sourceRasterMock.Setup(raster => raster.NumberOfRows).Returns(4);
            _sourceRasterMock.Setup(raster => raster.NumberOfColumns).Returns(4);
            _sourceRasterMock.Setup(raster => raster.NumberOfBands).Returns(3);
            _sourceRasterMock.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 }); 
            _sourceRasterMock.Setup(raster => raster.Mapper).Returns(mapper);
            _sourceRasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Integer);
            _sourceRasterMock.Setup(raster => raster.IsMapped).Returns(true);

            _sourceRasterMock.Setup(raster => raster.GetValue(It.IsInRange(0, 4, Range.Inclusive), It.IsInRange(0, 4, Range.Inclusive), It.IsInRange(0, 3, Range.Inclusive))).Returns(4);


            _referenceRasterMock = new Mock<IRaster>(MockBehavior.Strict);
            _referenceRasterMock.Setup(raster => raster.Factory).Returns(new RasterFactory());
            _referenceRasterMock.Setup(raster => raster.Coordinates).Returns(coordinates);
            _referenceRasterMock.Setup(raster => raster.IsReadable).Returns(true);
            _referenceRasterMock.Setup(raster => raster.NumberOfRows).Returns(4);
            _referenceRasterMock.Setup(raster => raster.NumberOfColumns).Returns(4);
            _referenceRasterMock.Setup(raster => raster.NumberOfBands).Returns(3);
            _referenceRasterMock.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 });
            _referenceRasterMock.Setup(raster => raster.Mapper).Returns(mapper);
            _referenceRasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Integer);
            _referenceRasterMock.Setup(raster => raster.IsMapped).Returns(true);

            for (Int32 i = 0; i < 4; i++)
                for (Int32 j = 0; j < 4; j++)
                {
                    if (i == 0 && j == 0)
                        _referenceRasterMock.Setup(raster => raster.GetValue(0, 0, It.IsInRange(0, 3, Range.Inclusive))).Returns(10);
                    else
                        _referenceRasterMock.Setup(raster => raster.GetValue(i, j, It.IsInRange(0, 3, Range.Inclusive))).Returns(4);
                }
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for image matching operation execution.
        /// </summary>
        [TestCase]
        public void ImageMatchingClassificationExecuteTest()
        {
            ISpectralGeometry validationGeometry = new GeometryFactory().CreateSpectralPolygon(_referenceRasterMock.Object);
            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationValidationGeometry, validationGeometry);

            ClassificationMapValidation operation = new ClassificationMapValidation(new GeometryFactory().CreateSpectralPolygon(_sourceRasterMock.Object), parameters);
            operation.Execute();

            ISpectralGeometry result = operation.Result;

            Assert.AreEqual(_sourceRasterMock.Object.NumberOfRows, result.Raster.NumberOfRows);
            Assert.AreEqual(_sourceRasterMock.Object.NumberOfColumns, result.Raster.NumberOfColumns);
            Assert.AreEqual(_sourceRasterMock.Object.NumberOfBands, result.Raster.NumberOfBands);
            Assert.AreEqual(RasterFormat.Integer, result.Raster.Format);

            for (Int32 i = 0; i < 4; i++)
                for (Int32 j = 0; j < 4; j++)
                {
                    if (i ==0 && j == 0)
                        Assert.AreEqual(0, result.Raster.GetValue(i, j, 0));
                    else
                        Assert.AreEqual(1, result.Raster.GetValue(i, j, 0));
                }
        }

        #endregion

    }
}
