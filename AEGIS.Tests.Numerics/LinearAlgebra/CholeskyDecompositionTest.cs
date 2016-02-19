/// <copyright file="CholeskyDecompositionTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Numerics.LinearAlgebra;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Numerics.LinearAlgebra
{
    [TestFixture]
    public class CholeskyDecompositionTest
    {
        private Matrix[] _matrix;
        private Matrix[] _lExpected;
        private Matrix[] _ltExpected;

        [SetUp]
        public void SetUp()
        {
            _matrix = new Matrix[]
            {
                MatrixFactory.CreateSquare(18, 22, 54, 42, 
                                           22, 70, 86, 62, 
                                           54, 86, 174, 134,
                                           42, 62, 134, 106)
            };

            _lExpected = new Matrix[]
            {
                MatrixFactory.CreateSquare(4.24264, 0.00000, 0.00000, 0.00000,
                                           5.18545, 6.56591, 0.00000, 0.00000, 
                                           12.72792, 3.04604, 1.64974, 0.00000,
                                           9.89949, 1.62455, 1.84971, 1.39262)
            };

            _ltExpected = new Matrix[]
            {
                MatrixFactory.CreateSquare(4.24264, 5.18545, 12.72792, 9.89949,
                                           0.00000, 6.56591, 3.04604, 1.62455, 
                                           0.00000, 0.00000, 1.64974, 1.84971,
                                           0.00000, 0.00000, 0.00000, 1.39262)
            };
        }

        [TestCase]
        public void CholeskyDecompositionComputeTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                CholeskyDecomposition decomposition = new CholeskyDecomposition(_matrix[i]);
                decomposition.Compute();

                for (Int32 j = 0; j < _matrix[i].NumberOfRows; j++)
                    for (Int32 k = 0; k < _matrix[i].NumberOfColumns; k++)
                    {
                        Assert.AreEqual(_lExpected[i][j, k], decomposition.L[j, k], 0.01);
                        Assert.AreEqual(_ltExpected[i][j, k], decomposition.LT[j, k], 0.01);
                    }
            }
        }
    }
}
