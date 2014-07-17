/// <copyright file="HadoopBooleanOperationResultTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
    /// Test fixture for class <see cref="HadoopBooleanOperationResult"/>.
    /// </summary>
    [TestFixture]
    public class HadoopBooleanOperationResultTest
    {
        #region Test cases

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [TestCase]
        public void HadoopBooleanOperationResultConstructorTest()
        {
            // test case 1: no parameters
            
            HadoopBooleanOperationResult result = new HadoopBooleanOperationResult();

            Assert.AreEqual(null, result.Request);
            Assert.AreEqual(false, result.Success);

            // test case 2: request and success parameters

            result = new HadoopBooleanOperationResult("request", true);

            Assert.AreEqual("request", result.Request);
            Assert.AreEqual(true, result.Success);
        }

        #endregion
    }
}
