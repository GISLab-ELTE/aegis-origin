/// <copyright file="ClassificationAccuracyValidationTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Robeto Giachetta. Licensed under the
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
using ELTE.AEGIS.Numerics;
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
    /// Test fixture for the <see cref="ClassificationAccuracyValidation" /> class.
    /// </summary>
    [TestFixture]
    public class ClassificationAccuracyValidationTest
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
            _sourceRasterMock = new Mock<IRaster>(MockBehavior.Strict);
            _sourceRasterMock.Setup(raster => raster.Factory).Returns(new RasterFactory());
            _sourceRasterMock.Setup(raster => raster.IsReadable).Returns(true);
            _sourceRasterMock.Setup(raster => raster.NumberOfRows).Returns(4);
            _sourceRasterMock.Setup(raster => raster.NumberOfColumns).Returns(4);
            _sourceRasterMock.Setup(raster => raster.NumberOfBands).Returns(1);
            _sourceRasterMock.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { 8 });
            List<Coordinate> coordinates = new List<Coordinate>();
            coordinates.Add(new Coordinate(0, 0));
            coordinates.Add(new Coordinate(0, 4));
            coordinates.Add(new Coordinate(4, 0));
            coordinates.Add(new Coordinate(4, 4));
            _sourceRasterMock.Setup(raster => raster.Coordinates).Returns(coordinates);
            
            Matrix transformation = new Matrix(4, 4);
            transformation[0, 0] = 1;
            transformation[1, 1] = 1;
            transformation[3, 3] = 1;
            _sourceRasterMock.Setup(raster => raster.Mapper).Returns(new RasterMapper(RasterMapMode.ValueIsArea, transformation));

            _sourceRasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Integer);
            _sourceRasterMock.Setup(raster => raster.IsMapped).Returns(true);

            _sourceRasterMock.Setup(raster => raster.GetValue(It.IsInRange(0, 1, Range.Inclusive), It.IsInRange(0, 3, Range.Inclusive), 0)).Returns(2);
            _sourceRasterMock.Setup(raster => raster.GetValue(It.IsInRange(2, 3, Range.Inclusive), It.IsInRange(0, 3, Range.Inclusive), 0)).Returns(5);


            _referenceRasterMock = new Mock<IRaster>(MockBehavior.Strict);
            _referenceRasterMock.Setup(raster => raster.Factory).Returns(new RasterFactory());
            _referenceRasterMock.Setup(raster => raster.IsReadable).Returns(true);
            _referenceRasterMock.Setup(raster => raster.NumberOfRows).Returns(4);
            _referenceRasterMock.Setup(raster => raster.NumberOfColumns).Returns(4);
            _referenceRasterMock.Setup(raster => raster.NumberOfBands).Returns(1);
            _referenceRasterMock.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { 8 });
            _referenceRasterMock.Setup(raster => raster.Coordinates).Returns(coordinates);
            _referenceRasterMock.Setup(raster => raster.Mapper).Returns(new RasterMapper(RasterMapMode.ValueIsCoordinate, transformation));
            _referenceRasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Integer);
            _referenceRasterMock.Setup(raster => raster.IsMapped).Returns(true);

            _referenceRasterMock.Setup(raster => raster.GetValue(It.IsInRange(0, 1, Range.Inclusive), It.IsInRange(0, 3, Range.Inclusive), 0)).Returns(1);
            _referenceRasterMock.Setup(raster => raster.GetValue(It.IsInRange(2, 3, Range.Inclusive), It.IsInRange(0, 3, Range.Inclusive), 0)).Returns(5);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for image matching operation execution.
        /// </summary>
        [Test]
        public void ClassificationAccuracyValidationExecuteTest()
        {
            ISpectralGeometry validationGeometry = new GeometryFactory().CreateSpectralPolygon(_referenceRasterMock.Object);
            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationValidationGeometry, validationGeometry);

            ClassificationAccuracyValidation operation = new ClassificationAccuracyValidation(new GeometryFactory().CreateSpectralPolygon(_sourceRasterMock.Object), parameters);
            operation.Execute();

            Double result = operation.Result;
            Assert.AreEqual(0.5, result);
        }

        #endregion
    }
}
