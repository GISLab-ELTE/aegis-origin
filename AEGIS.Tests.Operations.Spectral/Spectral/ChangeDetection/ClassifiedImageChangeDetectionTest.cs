/// <copyright file="ClassifiedImageChangeDetectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamas Nagy</author>

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.ChangeDetection;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.Operations.Spectral.ChangeDetection
{
    /// <summary>
    /// Test fixture for the <see cref="ClassifiedImageChangeDetection"/> class.
    /// </summary>
    [TestFixture]
    public class ClassifiedImageChangeDetectionTest
    {
        #region Private fields

        /// <summary>
        /// The source geometry.
        /// </summary>
        private ISpectralGeometry _source;

        /// <summary>
        /// The reference geometry.
        /// </summary>
        private ISpectralGeometry _reference;

        #endregion

        #region Test setup

        /// <summary>
        /// The test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _source = TestUtils.CreateRasterGeometryMock(new[,] { { 1, 2, 2 }, { 2, 2, 2 }, { 2, 2, 3 } });
            _reference = TestUtils.CreateRasterGeometryMock(new[,] { { 1, 1, 2 }, { 1, 2, 3 }, { 2, 3, 3 } });
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="ClassifiedImageChangeDetection.Execute" /> method.
        /// </summary>
        [Test]
        public void ClassifiedImageChangeDetectionExecuteTest()
        {
            // loss

            Dictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _reference);
            parameters.Add(SpectralOperationParameters.LossDetection, true);

            ClassifiedImageChangeDetection detection = new ClassifiedImageChangeDetection(_source, parameters);
            detection.Execute();

            IRaster result = detection.Result.Raster;

            for (Int32 rowIndex = 0; rowIndex < 3; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < 3; columnIndex++)
                {
                    if ((rowIndex == 0 && columnIndex == 0) || (rowIndex == 2 && columnIndex == 2))
                        Assert.AreEqual(0, result.GetValue(rowIndex, columnIndex, 2));
                    else
                        Assert.AreEqual(2, result.GetValue(rowIndex, columnIndex, 2));
                }

            // difference with loss

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _reference);
            parameters.Add(SpectralOperationParameters.DifferentialChangeDetection, true);
            parameters.Add(SpectralOperationParameters.LossDetection, true);

            detection = new ClassifiedImageChangeDetection(_source, parameters);
            detection.Execute();

            result = detection.Result.Raster;

            // band 1
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    if ((rowIndex == 1 && columnIndex == 0) || (rowIndex == 0 && columnIndex == 1))
                        Assert.AreEqual(2, result.GetValue(rowIndex, columnIndex, 1));
                    else
                        Assert.AreEqual(0, result.GetValue(rowIndex, columnIndex, 1));
                }

            // band 2
            Assert.AreEqual(0, result.GetValue(0, 1, 2));
            Assert.AreEqual(0, result.GetValue(1, 0, 2));
            Assert.AreEqual(0, result.GetValue(2, 1, 2));
            Assert.AreEqual(0, result.GetValue(1, 2, 2));
            Assert.AreEqual(0, result.GetValue(0, 0, 2));
            Assert.AreEqual(0, result.GetValue(0, 2, 2));
            Assert.AreEqual(0, result.GetValue(1, 1, 2));
            Assert.AreEqual(0, result.GetValue(2, 0, 2));
            Assert.AreEqual(0, result.GetValue(2, 2, 2));

            // band 3
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    if ((rowIndex == 2 && columnIndex == 1) || (rowIndex == 1 && columnIndex == 2))
                        Assert.AreEqual(2, result.GetValue(rowIndex, columnIndex, 3));
                    else
                        Assert.AreEqual(0, result.GetValue(rowIndex, columnIndex, 3));
                }

            // specified category index

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _reference);
            parameters.Add(SpectralOperationParameters.DifferentialChangeDetection, true);
            parameters.Add(SpectralOperationParameters.LossDetection, true);
            parameters.Add(SpectralOperationParameters.ChangeDetectionCategoryIndex, 3);

            detection = new ClassifiedImageChangeDetection(_source, parameters);
            detection.Execute();

            result = detection.Result.Raster;

            Assert.AreEqual(1, result.NumberOfBands);

            for (Int32 i = 0; i < 3; i++)
                for (Int32 j = 0; j < 3; j++)
                {
                    if ((i == 2 && j == 1) || (i == 1 && j == 2))
                        Assert.AreEqual(2, result.GetValue(i, j, 0));
                    else
                        Assert.AreEqual(0, result.GetValue(i, j, 0));
                }

            // specified category indices

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _reference);
            parameters.Add(SpectralOperationParameters.DifferentialChangeDetection, true);
            parameters.Add(SpectralOperationParameters.LossDetection, true);
            parameters.Add(SpectralOperationParameters.ChangeDetectionCategoryIndices, new[] { 3, 1 });

            detection = new ClassifiedImageChangeDetection(_source, parameters);
            detection.Execute();

            result = detection.Result.Raster;

            Assert.AreEqual(2, result.NumberOfBands);

            //Category 3 -> Band 0:
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    if ((rowIndex == 2 && columnIndex == 1) || (rowIndex == 1 && columnIndex == 2))
                        Assert.AreEqual(2, result.GetValue(rowIndex, columnIndex, 0));
                    else
                        Assert.AreEqual(0, result.GetValue(rowIndex, columnIndex, 0));
                }

            //Category 1 -> Band 1:
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    if ((rowIndex == 1 && columnIndex == 0) || (rowIndex == 0 && columnIndex == 1))
                        Assert.AreEqual(2, result.GetValue(rowIndex, columnIndex, 1));
                    else
                        Assert.AreEqual(0, result.GetValue(rowIndex, columnIndex, 1));
                }


            // source and reference match

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _source);
            parameters.Add(SpectralOperationParameters.DifferentialChangeDetection, true);

            detection = new ClassifiedImageChangeDetection(_source, parameters);
            detection.Execute();

            result = detection.Result.Raster;

            Assert.AreEqual(3, result.NumberOfRows);
            Assert.AreEqual(3, result.NumberOfColumns);
            Assert.AreEqual(4, result.NumberOfBands);

            for (Int32 bandIndex = 0; bandIndex < result.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                        Assert.AreEqual(0, result.GetValue(rowIndex, columnIndex, bandIndex));
        }

        #endregion
    }
}
