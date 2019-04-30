/// <copyright file="QRDecompositionTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Numerics.LinearAlgebra;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Numerics.LinearAlgebra
{
    [TestFixture]
    public class QRDecompositionTest
    {
        [TestCase]
        public void QRDecompositionComputeTest()
        {
            Matrix matrix = MatrixFactory.CreateSquare(12, -51, 4,
                                                       6, 167, -68,
                                                       -4, 24, -41);

            Matrix qExpected = MatrixFactory.CreateSquare(6.0 / 7, 69.0 / 175, -58.0 / 175,
                                                          3.0 / 7, -158.0 / 175, 6.0 / 175,
                                                          -2.0 / 7, -6.0 / 35, -33.0 / 35);

            Matrix rExpected = MatrixFactory.CreateSquare(14, 21, -14,
                                                          0, -175, 70,
                                                          0, 0, 35);

            QRDecomposition decomposition = new QRDecomposition(matrix);
            decomposition.Compute();

            for (Int32 j = 0; j < matrix.NumberOfRows; j++)
                for (Int32 k = 0; k < matrix.NumberOfColumns; k++)
                {
                    Assert.AreEqual(qExpected[j, k], decomposition.Q[j, k], 0.001);
                    Assert.AreEqual(rExpected[j, k], decomposition.R[j, k], 0.001);
                }
        }
    }
}
