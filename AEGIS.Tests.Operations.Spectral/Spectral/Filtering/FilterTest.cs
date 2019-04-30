/// <copyright file="FilterTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Operations.Spectral.Filtering;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Filtering
{
    /// <summary>
    /// Test fixture for the <see cref="Filter"/> class.
    /// </summary>
    [TestFixture]
    public class FilterTest
    {
        #region Test methods

        /// <summary>
        /// Test method for the constructor.
        /// </summary>
        [TestCase]
        public void FilterConstructorTest()
        {
            Filter filter = null;

            // test case 1: no kernel
            Assert.Throws<ArgumentNullException>(() => { filter = new Filter(null); });

            // test case 2: arbitrary size kernel
            Assert.Throws<ArgumentException>(() => { filter = new Filter(new Matrix(1, 2)); });

            // test case 3: even size kernel
            Assert.Throws<ArgumentException>(() => { filter = new Filter(new Matrix(2, 2)); });

            // test case 4: normal kernel
            Matrix matrix = new Matrix(3, 3);
            filter = new Filter(matrix);

            Assert.AreEqual(filter.Radius, 1);
            Assert.AreEqual(filter.Factor, 1);
            Assert.AreEqual(filter.Offset, 0);
            Assert.AreEqual(filter.Kernel, matrix);

            // test case 5: normal kernel with factor and offset
            matrix = new Matrix(7, 7);
            filter = new Filter(matrix, 100, 10);
            Assert.AreEqual(filter.Radius, 3);
            Assert.AreEqual(filter.Factor, 100);
            Assert.AreEqual(filter.Offset, 10);
            Assert.AreEqual(filter.Kernel, matrix);
        }

        #endregion
    }
}
