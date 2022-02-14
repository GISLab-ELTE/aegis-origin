/// <copyright file="HadoopFileListingOperationResultTest.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.IO.Storage.Operation;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.IO.Storage.Operation
{
    /// <summary>
    /// Test fixture for class <see cref="HadoopFileListingOperationResult"/>.
    /// </summary>
    [TestFixture]
    public class HadoopFileListingOperationResultTest
    {
        #region Test cases

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [TestCase]
        public void HadoopFileListingOperationResultConstructorTest()
        {
            // test case 1: no parameters

            HadoopFileListingOperationResult result = new HadoopFileListingOperationResult();

            Assert.AreEqual(null, result.Request);
            Assert.AreEqual(0, result.StatusList.Count);

            // test case 2: request and list parameters

            result = new HadoopFileListingOperationResult("request", new HadoopFileStatusOperationResult[1]);

            Assert.AreEqual("request", result.Request);
            Assert.AreEqual(1, result.StatusList.Count);
        }

        #endregion
    }
}
