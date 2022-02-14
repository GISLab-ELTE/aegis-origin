/// <copyright file="ClassificationCategoryChangeDetectionTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
using System.Linq;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.ChangeDetection;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.Operations.Spectral.ChangeDetection
{
    /// <summary>
    /// Test fixture for <see cref="ClassificationCategoryChangeDetectionTest" /> class.
    /// </summary>
    [TestFixture]
    public class ClassificationCategoryChangeDetectionTest
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
        /// Tests the <see cref="ClassificationAreaChangeDetection.Execute" /> method.
        /// </summary>
        [Test]
        public void ClassificationCategoryChangeDetectionExecuteTest()
        {
            // generic case

            Dictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _reference);

            ClassificationCategoryChangeDetection detection = new ClassificationCategoryChangeDetection(_source, parameters);
            detection.Execute();

            Double[,] result = detection.Result;

            Assert.AreEqual(4, result.GetLength(0));
            Assert.AreEqual(4, result.GetLength(1));

            Double[,] expected = new Double[4, 4]
            {
                { 0, 0, 0, 0 },
                { 0, 1, 2, 0 },
                { 0,-2, 3,-2 },
                { 0, 0, 2, 1 }
            };

            Assert.IsTrue(expected.OfType<Double>().SequenceEqual(result.OfType<Double>()));

            // source and reference match

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _source);

            detection = new ClassificationCategoryChangeDetection(_source, parameters);
            detection.Execute();

            result = detection.Result;

            Assert.AreEqual(4, result.GetLength(0));
            Assert.AreEqual(4, result.GetLength(1));

            expected = new Double[4, 4]
            {
                { 0, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 7, 0 },
                { 0, 0, 0, 1 }
            };

            Assert.IsTrue(expected.OfType<Double>().SequenceEqual(result.OfType<Double>()));
        }

        #endregion
    }
}
