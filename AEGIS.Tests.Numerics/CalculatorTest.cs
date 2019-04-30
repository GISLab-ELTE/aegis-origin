/// <copyright file="CalculatorTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
/// <author>Ákos Horváth, Dóra Papp</author>

using ELTE.AEGIS.Numerics;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Numerics
{
    /// <summary>
    /// Test fixture for the <see cref="Calculator" /> class.
    /// </summary>
    [TestFixture]
    public class CalculatorTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="AbsMax" /> method.
        /// </summary>
        [Test]
        public void CalculatorAbsMaxTest()
        {
            Assert.AreEqual(Calculator.AbsMax(-57.678, 18.45, 13.01, 0, 200, -245), 245);
            Assert.AreEqual(Calculator.AbsMax(-57.678, 57.856, 11.111, 0, 20, -0, 0), 57.856);
            Assert.AreEqual(Calculator.AbsMax(Double.NaN), Double.NaN);
            Assert.AreEqual(Calculator.AbsMax(Double.NegativeInfinity, 0, 57, 4), Double.PositiveInfinity);
            Assert.AreEqual(Calculator.AbsMax(Double.NegativeInfinity, 0, 57, 4, Double.PositiveInfinity), Double.PositiveInfinity);
            Assert.AreEqual(Calculator.AbsMax((Double[])null), Double.PositiveInfinity);
        }

        /// <summary>
        /// Tests the <see cref="Factorial" /> method.
        /// </summary>
        [Test]
        public void CalculatorFactorialTest()
        {
            Assert.AreEqual(Calculator.Factorial(0), 1);
            Assert.AreEqual(Calculator.Factorial(1), 1);
            Assert.AreEqual(Calculator.Factorial(2), 2);
            Assert.AreEqual(Calculator.Factorial(3), 6);
            Assert.AreEqual(Calculator.Factorial(5), 120);
            Assert.AreEqual(Calculator.Factorial(10), 3628800);
            Assert.AreEqual(Calculator.Factorial(20), 2432902008176640000);
            Assert.AreEqual(Calculator.Factorial(171), Double.PositiveInfinity);
            Assert.Throws<ArgumentOutOfRangeException>(() => Calculator.Factorial(-5));

        }

        /// <summary>
        /// Tests the <see cref="Gamma" /> method.
        /// </summary>
        [Test]
        public void CalcaultorGammaTest()
        {
            Assert.AreEqual(Calculator.Gamma(5.6), 61.5539150062892670342628016328, 0.0000000000001);
            Assert.AreEqual(Calculator.Gamma(4), 6, 0.00000000000001);
            Assert.AreEqual(Calculator.Gamma(17.432786), 70794796680408.184132649758153230595573203898806, 0.1);
            Assert.AreEqual(Calculator.Gamma(17.4), 64524993678768.475176512444528832088723820561940);
            Assert.AreEqual(Calculator.Gamma(17.2), 36698964629326.666366399049040308611484289683410, 0.1);
            Assert.AreEqual(Calculator.Gamma(17.4327), 70777572425326.219973716843329090056968590230874, 1);
            Assert.AreEqual(Calculator.Gamma(7.84932), 3725.5363220501936325763777399300694165140295715708192, 0.00000000001);
            Assert.AreEqual(Calculator.Gamma(1.79342), 0.9296536910011532941878683112978521231640028664002138, 0.000000000000001);
            Assert.AreEqual(Calculator.Gamma(0.78), 1.1874709053741035003665987584542707907235290398108421, 0.000000000000001);
            Assert.AreEqual(Calculator.Gamma(21.5), 11082798113786903841.710590978006853895159991111, 100000);
            Assert.AreEqual(Calculator.Gamma(21.4378), 9171594978637877217.1951336804437033178076357804, 10000);
            Assert.AreEqual(Calculator.Gamma(0.1234), 7.6363851985070193846874769204366944252526236697467540, 0.00000000000001);
            Assert.AreEqual(Calculator.Gamma(-8.64), -0.00002137352460747696486247515808363540929072012897, 0.0000000000000000001);

            Assert.Throws<ArgumentOutOfRangeException>(() => Calculator.Gamma(-5));
        }

        /// <summary>
        /// Tests the <see cref="Binomial" /> method.
        /// </summary>
        [Test]
        public void BinomialTest()
        {
            Assert.AreEqual(Calculator.Binomial(18, 13), 8568);
            Assert.AreEqual(Calculator.Binomial(60, 30), 118264581564861424);
            Assert.AreEqual(Calculator.Binomial(0, 0), 1);
            Assert.AreEqual(Calculator.Binomial(5, 0), 1);
            Assert.AreEqual(Calculator.Binomial(0, 5), 0);
            Assert.AreEqual(Calculator.Binomial(30, 30), 1);
            Assert.AreEqual(Calculator.Binomial(30, 31), 0);

            Assert.Throws<ArgumentOutOfRangeException>(() => Calculator.Binomial(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Calculator.Binomial(1, -1));
        }
        #endregion
    }
}

