/// <copyright file="PrecisionModelTest.cs" company="Eötvös Loránd University (ELTE)">
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
        private PrecisionModel _fixedLargeModel1;
        private PrecisionModel _fixedLargeModel2;
        private PrecisionModel _fixedSmallModel1;
        private PrecisionModel _fixedSmallModel2;
        private Double[] _values;

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
            _fixedLargeModel1 = new PrecisionModel(1000);
            _fixedLargeModel2 = new PrecisionModel(1000000000000);
            _fixedSmallModel1 = new PrecisionModel(0.001);
            _fixedSmallModel2 = new PrecisionModel(0.000000000001);

            _values = Enumerable.Range(1, 28).Select(value => Math.Pow((Double)value / 2, (value / 2) * ((value % 2 == 0) ? 1 : -1))).ToArray();            
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the constructor.
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

            Assert.AreEqual(PrecisionModelType.Fixed, _fixedLargeModel1.ModelType);
            Assert.AreEqual(4, _fixedLargeModel1.MaximumSignificantDigits);
            Assert.AreEqual(1000, _fixedLargeModel1.Scale);

            // fixed type with scale of 1000000000000

            Assert.AreEqual(PrecisionModelType.Fixed, _fixedLargeModel2.ModelType);
            Assert.AreEqual(13, _fixedLargeModel2.MaximumSignificantDigits);
            Assert.AreEqual(1000000000000, _fixedLargeModel2.Scale);


            // fixed type with scale of 0.001

            Assert.AreEqual(PrecisionModelType.Fixed, _fixedSmallModel1.ModelType);
            Assert.AreEqual(1, _fixedSmallModel1.MaximumSignificantDigits);
            Assert.AreEqual(0.001, _fixedSmallModel1.Scale);

            // fixed type with scale of 0.000000000001

            Assert.AreEqual(PrecisionModelType.Fixed, _fixedSmallModel2.ModelType);
            Assert.AreEqual(1, _fixedSmallModel2.MaximumSignificantDigits);
            Assert.AreEqual(0.000000000001, _fixedSmallModel2.Scale);
        }

        /// <summary>
        /// Tests the <see cref="MakePrecise" /> method.
        /// </summary>
        [Test]
        public void PrecisionModelMakePreciseTest()
        {
            for (Int32 i = 0; i < _values.Length; i++)
            {
                Assert.AreEqual(_values[i], _defaultModel.MakePrecise(_values[i]));
                Assert.AreEqual(_values[i], _floatingModel.MakePrecise(_values[i]));
                Assert.AreEqual((Single)_values[i], _floatingSingleModel.MakePrecise(_values[i]));
                Assert.AreEqual(Math.Round(_values[i], 3), _fixedLargeModel1.MakePrecise(_values[i]));
                Assert.AreEqual(Math.Round(_values[i], 12), _fixedLargeModel2.MakePrecise(_values[i]));
                Assert.AreEqual(Math.Round(_values[i] / 1000) * 1000, _fixedSmallModel1.MakePrecise(_values[i]));
                Assert.AreEqual(Math.Round(_values[i] / 1000000000000) * 1000000000000, _fixedSmallModel2.MakePrecise(_values[i]));
            }
        }

        /// <summary>
        /// Tests the <see cref="Tolerance" /> method.
        /// </summary>
        [Test]
        public void PrecisionModelToleranceTest()
        {
            for (Int32 i = 0; i < _values.Length; i++)
            {
                Assert.IsTrue(_defaultModel.Tolerance(_values[i]) > _values[i] * Math.Pow(10, -16));
                Assert.IsTrue(_defaultModel.Tolerance(_values[i]) <= _values[i] * Math.Pow(10, -15));
                Assert.IsTrue(_floatingModel.Tolerance(_values[i]) > _values[i] * Math.Pow(10, -16));
                Assert.IsTrue(_floatingModel.Tolerance(_values[i]) <= _values[i] * Math.Pow(10, -15));
                Assert.IsTrue(_floatingSingleModel.Tolerance(_values[i]) > _values[i] * Math.Pow(10, -6));
                Assert.IsTrue(_floatingSingleModel.Tolerance(_values[i]) <= _values[i] * Math.Pow(10, -5));
                Assert.AreEqual(0.0005, _fixedLargeModel1.Tolerance(_values[i]));
                Assert.AreEqual(0.0000000000005, _fixedLargeModel2.Tolerance(_values[i]));
                Assert.AreEqual(500, _fixedSmallModel1.Tolerance(_values[i]));
                Assert.AreEqual(500000000000, _fixedSmallModel2.Tolerance(_values[i]));
            }
        }

        #endregion
    }
}
