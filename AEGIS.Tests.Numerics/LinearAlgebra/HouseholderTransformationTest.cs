/// <copyright file="HouseholderTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class HouseholderTransformationTest
    {
        [TestCase]
        public void HouseholderTridiagonalizeTest()
        {
            Matrix matrix = MatrixFactory.CreateSquare(4, 1, -2, 2,
                                                       1, 2, 0, 1,
                                                       -2, 0, 3, -2,
                                                       2, 1, -2, -1);

            Matrix expected = MatrixFactory.CreateSquare(4, -3, 0, 0,
                                                         -3, 10.0 / 3, -5.0 / 3, 0,
                                                         0, -5.0 / 3, -33.0 / 25, 68.0 / 75,
                                                         0, 0, 68.0 / 75, 149.0 / 75);

            Matrix tridiag = HouseholderTransformation.Tridiagonalize(matrix);

            for (Int32 j = 0; j < matrix.NumberOfRows; j++)
                for (Int32 k = 0; k < matrix.NumberOfColumns; k++)
                {
                    Assert.AreEqual(expected[j, k], tridiag[j, k], 0.01);
                }
        }
    }
}
