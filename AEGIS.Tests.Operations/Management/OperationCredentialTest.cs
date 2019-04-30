/// <copyright file="OperationCredentialTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Management;
using Moq;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Operations.Management
{
    /// <summary>
    /// Test fixture for class <see cref="OperationCredential"/>.
    /// </summary>
    [TestFixture]
    public class OperationCredentialTest
    {
        #region Test methods

        /// <summary>
        /// Test method for credential priority.
        /// </summary>
        [TestCase]
        public void OperationCredentialValidatePriorityTest()
        {
            OperationCredential credential = new OperationCredential();
            Assert.AreEqual(1, credential.Priority);
        }

        /// <summary>
        /// Test method for engine validation.
        /// </summary>
        [TestCase]
        public void OperationCredentialValidateEngineTest()
        {
            Mock<OperationsEngine> engineMock = new Mock<OperationsEngine>();

            OperationCredential credential = new OperationCredential();
            Assert.IsTrue(credential.ValidateEngine(engineMock.Object));
        }

        /// <summary>
        /// Test method for source validation.
        /// </summary>
        [TestCase]
        public void OperationCredentialValidateSourceTest()
        {
            OperationCredential credential = new OperationCredential();

            Assert.IsFalse(credential.ValidateSource(null));
            Assert.IsTrue(credential.ValidateSource(new Object()));
        }

        /// <summary>
        /// Test method for target validation.
        /// </summary>
        [TestCase]
        public void OperationCredentialValidateTargetTest()
        {
            Mock<OperationsEngine> engineMock = new Mock<OperationsEngine>();

            OperationCredential credential = new OperationCredential();

            Assert.IsFalse(credential.ValidateTarget(null));
            Assert.IsTrue(credential.ValidateTarget(new Object()));
            Assert.IsTrue(credential.ValidateTarget(new Object(), null));
        }

        /// <summary>
        /// Test method for target validation.
        /// </summary>
        [TestCase]
        public void OperationCredentialValidateParametersTest()
        {
            Mock<OperationsEngine> engineMock = new Mock<OperationsEngine>();

            OperationCredential credential = new OperationCredential();
            Assert.IsTrue(credential.ValidateParameters(null));
            Assert.IsTrue(credential.ValidateParameters(null, null));
            Assert.IsTrue(credential.ValidateParameters(null, null, null));
        }

        #endregion
    }
}
