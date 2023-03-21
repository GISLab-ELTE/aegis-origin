// <copyright file="LUDecompositionTest.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Numerics.LinearAlgebra;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Numerics.LinearAlebra
{
    [TestFixture]
    public class LUDecompositionTest
    {
        private Matrix[] _matrix;
        private Matrix[] _lExpected;
        private Matrix[] _uExpected;
        private Matrix[] _pExpected;
        private Double[] _determinantExpected;
        private Matrix[] _inverseExpected;

        [SetUp]
        public void SetUp()
        {
            _matrix = new Matrix[]
            {
                MatrixFactory.CreateSquare(1, 3, 5, 
                                           2, 4, 7, 
                                           1, 1, 0),
                MatrixFactory.CreateSquare(11, 9, 24, 2, 
                                           1, 5, 2, 6, 
                                           3, 17, 18, 1, 
                                           2, 5, 7, 1)
            };

            _lExpected = new Matrix[]
            {
                MatrixFactory.CreateSquare(1, 0, 0, 
                                           0.5, 1, 0, 
                                           0.5, -1, 1),
                MatrixFactory.CreateSquare(1, 0, 0, 0, 
                                           0.272727272727273, 1, 0, 0, 
                                           0.0909090909090909, 0.28750, 1, 0, 
                                           0.181818181818182, 0.23125, 0.00359712230215807, 1)
            };

            _uExpected = new Matrix[]
            {
                MatrixFactory.CreateSquare(2, 4, 7, 
                                           0, 1, 1.5, 
                                           0, 0, -2),
                MatrixFactory.CreateSquare(11, 9, 24, 2, 
                                           0, 14.54545, 11.45455, 0.45455, 
                                           0, 0, -3.47500, 5.68750, 
                                           0, 0, 0, 0.51079)
            };

            _pExpected = new Matrix[]
            {
                MatrixFactory.CreateSquare(0, 1, 0, 
                                           1, 0, 0, 
                                           0, 0, 1),
                MatrixFactory.CreateSquare(1, 0, 0, 0, 
                                           0, 0, 1, 0, 
                                           0, 1, 0, 0, 
                                           0, 0, 0, 1)
            };

            _determinantExpected = new Double[] { 4, 284 };

            _inverseExpected = new Matrix[]
            {
                MatrixFactory.CreateSquare(-1.75, 1.25, 0.25,
                                           1.75, -1.25, 0.75,
                                           -0.5, 0.5, -0.5),
                MatrixFactory.CreateSquare(0.72, 0.46, 1.02, -5.23,
                                           0.29, 0.24, 0.60, -2.58,
                                           -0.38, -0.30, -0.65, 3.20,
                                           -0.23, 0.00, -0.45, 1.96)
            };
        }

        [TestCase]
        public void LUDecompositionComputeTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                LUDecomposition decomposition = new LUDecomposition(_matrix[i]);
                decomposition.Compute();

                for (Int32 j = 0; j < _matrix[i].NumberOfRows; j++)
                    for (Int32 k = 0; k < _matrix[i].NumberOfColumns; k++)
                    {
                        Assert.AreEqual(_lExpected[i][j, k], decomposition.L[j, k], 0.01);
                        Assert.AreEqual(_uExpected[i][j, k], decomposition.U[j, k], 0.01);
                        Assert.AreEqual(_pExpected[i][j, k], decomposition.P[j, k], 0.01);
                    }
            }
        }

        [TestCase]
        public void LUDecompositionDeterminantTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                Double determinant = LUDecomposition.ComputeDeterminant(_matrix[i]);

                Assert.AreEqual(_determinantExpected[i], determinant, 0.01);
            }
        }
    }
}
