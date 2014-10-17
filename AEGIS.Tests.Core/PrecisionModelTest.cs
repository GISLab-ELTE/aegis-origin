/// <copyright file="PrecisionModel.cs" company="Eötvös Loránd University (ELTE)">
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


using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests
{
    /// <summary>
    /// Test fixture for the <see cref="PrecisionModel"/> class.
    /// </summary>
    [TestFixture]
    public class PrecisionModelTest
    {
        #region Private fields

        private PrecisionModel _defaultModel;
        private PrecisionModel _floatingModel;
        private PrecisionModel _floatingSingleModel;
        private PrecisionModel _fixedLargeModel;
        private PrecisionModel _fixedSmallModel;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _defaultModel = new PrecisionModel();
            _floatingModel = new PrecisionModel(PrecisionModelType.Floating);
            _floatingSingleModel = new PrecisionModel(PrecisionModelType.FloatingSingle);
            _fixedLargeModel = new PrecisionModel(1000);
            _fixedSmallModel = new PrecisionModel(0.001);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test method for the constructor.
        /// </summary>
        [Test]
        public void PrecisionModelPropertiesTest()
        {
            // no arguments

            Assert.AreEqual(PrecisionModelType.Floating, _defaultModel.ModelType);
            Assert.AreEqual(16, _defaultModel.MaximumSignificantDigits);
            Assert.AreEqual(0, _defaultModel.Scale);


            // floating model type

            Assert.AreEqual(PrecisionModelType.Floating, _floatingModel.ModelType);
            Assert.AreEqual(16, _floatingModel.MaximumSignificantDigits);
            Assert.AreEqual(0, _floatingModel.Scale);


            // floating single model type

            Assert.AreEqual(PrecisionModelType.FloatingSingle, _floatingSingleModel.ModelType);
            Assert.AreEqual(6, _floatingSingleModel.MaximumSignificantDigits);
            Assert.AreEqual(0, _floatingSingleModel.Scale);


            // fixed type with scale of 1000

            Assert.AreEqual(PrecisionModelType.Fixed, _fixedLargeModel.ModelType);
            Assert.AreEqual(4, _fixedLargeModel.MaximumSignificantDigits);
            Assert.AreEqual(1000, _fixedLargeModel.Scale);


            // fixed type with scale of 0.001

            Assert.AreEqual(PrecisionModelType.Fixed, _fixedSmallModel.ModelType);
            Assert.AreEqual(1, _fixedSmallModel.MaximumSignificantDigits);
            Assert.AreEqual(0.001, _fixedSmallModel.Scale);
        }

        [Test]
        public void PrecisionModelMakePreciseTest()
        {
            Double[] values = Enumerable.Range(1, 100).Select(value => Math.Pow((Double)value, value * ((value % 2 == 0) ? 1 : -1))).ToArray();


            // double values

            for (Int32 i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i], _defaultModel.MakePrecise(values[i]));
                Assert.AreEqual(values[i], _floatingModel.MakePrecise(values[i]));
                Assert.AreEqual((Single)values[i], _floatingSingleModel.MakePrecise(values[i]));
                Assert.AreEqual(Math.Round(values[i] / 1000) * 1000, _fixedLargeModel.MakePrecise(values[i]));
                Assert.AreEqual(Math.Round(values[i], 3), _fixedSmallModel.MakePrecise(values[i]));
            }
        }

        #endregion
    }
}
