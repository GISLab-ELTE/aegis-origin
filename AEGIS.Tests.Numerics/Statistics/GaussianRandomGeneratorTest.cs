/// <copyright file="GaussianRandomGeneratorTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics.Statistics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Numerics.Statistics
{
    /// <summary>
    /// Test fixture for the <see cref="GaussianRandomGenerator" /> class.
    /// </summary>
    [TestFixture]
    public class GaussianRandomGeneratorTest
    {
        #region Private fields

        private GaussianRandomGenerator _generator;
        private Int32 _numberOfGeneratedNumbers;
        private Double[] _mean;
        private Double[] _stdDeviation;
        private Double[] _error;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _generator = new GaussianRandomGenerator();
            _numberOfGeneratedNumbers = 100000;
            _mean = new Double[] { 0, 1, 10, -10, 100, -100, 10000, -10000, 10, 10 };
            _stdDeviation = new Double[] { 1, 1, 5, 5, 10, 10, 100, 100, 0.1, 0.1 };
            _error = new Double[] { 0.01, 0.05, 0.05, 0.05, 0.1, 0.1, 1, 1, 0.005, 0.005 };
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="NextDouble" /> method.
        /// </summary>
        [Test]
        public void GaussianRandomGeneratorNextDoubleTest()
        {
            // numbers between 0 and 1
            List<Double> generatedNumbers = Enumerable.Range(0, _numberOfGeneratedNumbers).Select(number => _generator.NextDouble()).ToList();

            // range test
            for (Int32 i = 0; i < _numberOfGeneratedNumbers; i++)
            {
                Assert.GreaterOrEqual(generatedNumbers[i], 0);
                Assert.Less(generatedNumbers[i], 1);
            }

            // numbers with specified mean and std. variation
            for (Int32 i = 0; i < _mean.Length; i++)
            {
                generatedNumbers = Enumerable.Range(0, _numberOfGeneratedNumbers).Select(number => _generator.NextDouble(_mean[i], _stdDeviation[i])).ToList();

                // mean test
                Double mean = generatedNumbers.Sum() / _numberOfGeneratedNumbers;
                Assert.AreEqual(_mean[i], mean, _error[i]);

                // std. deviation test
                Double stdDeviation = Math.Sqrt(generatedNumbers.Sum(number => (number - mean) * (number - mean)) / _numberOfGeneratedNumbers);

                Assert.AreEqual(_stdDeviation[i], stdDeviation, _error[i]);
            }
        }

        #endregion
    }
}
