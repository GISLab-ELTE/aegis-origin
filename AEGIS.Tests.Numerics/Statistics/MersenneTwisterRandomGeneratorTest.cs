/// <copyright file="MersenneTwisterRandomGeneratorTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dóra Papp</author>

using ELTE.AEGIS.Numerics.Statistics;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Numerics.Statistics
{
    /// <summary>
    /// Test fixture for the <see cref="MersenneTwisterRandomGenerator" /> class.
    /// </summary>
    [TestFixture]
    public class MersenneTwisterRandomGeneratorTest
    {
        #region Private fields

        private MersenneTwisterRandomGenerator _generator;
        private Int32 _numberOfGeneratedNumbers;
        private Int32 _numberOfIntervals;
        private Double _allowedError;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _generator = new MersenneTwisterRandomGenerator(5);
            _numberOfGeneratedNumbers = 1000000;
            _numberOfIntervals = 100;
            _allowedError = 0.03;
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the <see cref="NextDouble" /> method.
        /// </summary>
        [Test]
        public void MersenneTwisterRandomGeneratorNextDoubleTest()
        {
            Int64[] counter = new Int64[_numberOfIntervals];
            Int32 numbersPerInterval = _numberOfGeneratedNumbers / _numberOfIntervals;        
            Int32 lowerBound = (Int32)(numbersPerInterval * (1 - _allowedError));
            Int32 upperBound = (Int32)(numbersPerInterval * (1 + _allowedError));

            for (Int32 i = 0; i < _numberOfGeneratedNumbers; ++i)
            {
                Double actualGeneratedNumber = _generator.NextDouble();

                for (Int32 j = 1; j <= _numberOfIntervals; ++j)
                {
                    if (actualGeneratedNumber < (Double)j / _numberOfIntervals)
                    {
                        ++counter[j - 1];
                        break;
                    }                
                }
            }

            for (Int32 i = 0; i < _numberOfIntervals; ++i)
            {
                Assert.IsTrue(counter[i] >= lowerBound && counter[i] <= upperBound);
            }
        }

        #endregion
    }
}
