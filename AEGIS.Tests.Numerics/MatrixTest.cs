// <copyright file="MatrixTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Numerics
{
    [TestFixture]
    public class MatrixTest
    {
        private Matrix[] _matrix;

        [SetUp]
        public void SetUp()
        {
            _matrix = new Matrix[]
            {
                MatrixFactory.CreateSquare(),
                MatrixFactory.CreateSquare(0),
                MatrixFactory.CreateSquare(0, 0, 0, 0),
                MatrixFactory.CreateSquare(1, 3, 5, 
                                           2, 4, 7, 
                                           1, 1, 0),
                MatrixFactory.CreateSquare(11, 9, 24, 2, 
                                           1, 5, 2, 6, 
                                           3, 17, 18, 1, 
                                           2, 5, 7, 1)
            };
        }

        [TestCase]
        public void MatrixIsNullTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                if (i <= 2)
                    Assert.IsTrue(Matrix.IsNull(_matrix[i]));
                else
                    Assert.IsFalse(Matrix.IsNull(_matrix[i])); 
            }
        }

        [TestCase]
        public void MatrixAreEqualTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                for (Int32 j = 0; j < _matrix.Length; j++)
                {
                    if (i == j)
                        Assert.IsTrue(Matrix.AreEqual(_matrix[i], _matrix[j]));
                    else
                        Assert.IsFalse(Matrix.AreEqual(_matrix[i], _matrix[j]));
                }
            }
        }


        [TestCase]
        public void MatrixTransponeTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                Matrix transponedMatrix = _matrix[i].Transpone();

                for (Int32 j = 0; j < transponedMatrix.NumberOfRows; j++)
                    for (Int32 k = 0; k < transponedMatrix.NumberOfColumns; k++)
                    {
                        Assert.AreEqual(transponedMatrix[j, k], _matrix[i][k, j]);
                    }
            }
        }

        [TestCase]
        public void MatrixUnaryNegationTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                Matrix negatedMatrix = -_matrix[i];

                for (Int32 j = 0; j < negatedMatrix.NumberOfRows; j++)
                    for (Int32 k = 0; k < negatedMatrix.NumberOfColumns; k++)
                    {
                        Assert.AreEqual(negatedMatrix[j, k], - _matrix[i][j, k]);
                    }
            }
        }

        [TestCase]
        public void MatrixSubtractionTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                Matrix extractMatrix = _matrix[i] - _matrix[i];
                Assert.IsTrue(Matrix.IsNull(extractMatrix));

                extractMatrix = new Matrix(_matrix[i].NumberOfRows, _matrix[i].NumberOfColumns) - _matrix[i];

                for (Int32 j = 0; j < extractMatrix.NumberOfRows; j++)
                    for (Int32 k = 0; k < extractMatrix.NumberOfColumns; k++)
                    {
                        Assert.AreEqual(extractMatrix[j, k], -_matrix[i][j, k]);
                    }
            }
        }

        [TestCase]
        public void MatrixMultiplyWithScalarTest()
        {
            Double d = Math.PI;
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                Matrix multipleMatrix = d * _matrix[i];

                for (Int32 j = 0; j < multipleMatrix.NumberOfRows; j++)
                    for (Int32 k = 0; k < multipleMatrix.NumberOfColumns; k++)
                    {
                        Assert.AreEqual(multipleMatrix[j, k], d * _matrix[i][j, k]);
                    }

                Matrix multipleMatrix2 = _matrix[i] * d;

                for (Int32 j = 0; j < multipleMatrix.NumberOfRows; j++)
                    for (Int32 k = 0; k < multipleMatrix.NumberOfColumns; k++)
                    {
                        Assert.AreEqual(multipleMatrix2[j, k], d * _matrix[i][j, k]);
                    }
            }
        }

        [TestCase]
        public void MatrixAdditionTest()
        {
            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                Matrix sumMatrix = _matrix[i] + _matrix[i];

                for (Int32 j = 0; j < sumMatrix.NumberOfRows; j++)
                    for (Int32 k = 0; k < sumMatrix.NumberOfColumns; k++)
                    {
                        Assert.AreEqual(2 * _matrix[i][j, k], sumMatrix[j, k]);
                    }
            }
        }

        [TestCase]
        public void MatrixMultiplyTest()
        {
            Matrix[] mulExpected = new Matrix[]
            {
                MatrixFactory.CreateSquare(),
                MatrixFactory.CreateSquare(0),
                MatrixFactory.CreateSquare(0, 0, 0, 0),
                MatrixFactory.CreateSquare(12, 20, 26,
                                           17, 29, 38,
                                           3, 7, 12),
                MatrixFactory.CreateSquare(206, 562, 728, 102,
                                           34, 98, 112, 40,
                                           106, 423, 437, 127,
                                           50, 167, 191, 42)
            };

            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                Matrix mulMatrix = _matrix[i] * _matrix[i];

                for (Int32 j = 0; j < mulMatrix.NumberOfRows; j++)
                    for (Int32 k = 0; k < mulMatrix.NumberOfColumns; k++)
                    {
                        Assert.AreEqual(mulExpected[i][j, k], mulMatrix[j, k]);
                    }
            }
        }
    }
}
