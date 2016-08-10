/// <copyright file="ClassificationAreaChangeDetectionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Operations.Spectral.Classification
{
    /// <summary>
    /// Test fixture for the <see cref="ClassificationAreaChangeDetection" /> class.
    /// </summary>
    [TestFixture]
    public class ClassificationAreaChangeDetectionTest
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
        public void ClassificationAreaChangeDetectionExecuteTest()
        {
            // generic case

            Dictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _reference);
            parameters.Add(SpectralOperationParameters.DifferentialAreaComputation, true);

            ClassificationAreaChangeDetection detection = new ClassificationAreaChangeDetection(_source, parameters);
            detection.Execute();

            Assert.AreEqual(4, detection.Result.Length);
            Assert.AreEqual(1d / 3, detection.Result[1]);
            Assert.AreEqual(7d / 3, detection.Result[2]);
            Assert.AreEqual(1d / 3, detection.Result[3]);

            // source and reference match

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.ClassificationReferenceGeometry, _source);
            parameters.Add(SpectralOperationParameters.DifferentialAreaComputation, true);

            detection = new ClassificationAreaChangeDetection(_source, parameters);
            detection.Execute();

            Assert.AreEqual(4, detection.Result.Length);
            for (Int32 index = 1; index < detection.Result.Length; index++)
                Assert.AreEqual(1, detection.Result[index]);
        }

        #endregion
    }
}
