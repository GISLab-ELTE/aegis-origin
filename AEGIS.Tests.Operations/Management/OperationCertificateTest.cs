/// <copyright file="OperationCertificateTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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
    /// Test fixture for class <see cref="OperationCertificate"/>.
    /// </summary>
    [TestFixture]
    public class OperationCertificateTest
    {
        #region Test methods

        /// <summary>
        /// Test method for certificate priority.
        /// </summary>
        [TestCase]
        public void OperationCertificateValidatePriorityTest()
        {
            OperationCertificate certificate = new OperationCertificate();
            Assert.AreEqual(1, certificate.Priority);
        }

        /// <summary>
        /// Test method for engine validation.
        /// </summary>
        [TestCase]
        public void OperationCertificateValidateEngineTest()
        {
            Mock<OperationsEngine> engineMock = new Mock<OperationsEngine>();

            OperationCertificate certificate = new OperationCertificate();
            Assert.IsTrue(certificate.ValidateEngine(engineMock.Object));
        }

        /// <summary>
        /// Test method for source validation.
        /// </summary>
        [TestCase]
        public void OperationCertificateValidateSourceTest()
        {
            OperationCertificate certificate = new OperationCertificate();

            Assert.IsFalse(certificate.ValidateSource(null));
            Assert.IsTrue(certificate.ValidateSource(new Object()));
        }

        /// <summary>
        /// Test method for target validation.
        /// </summary>
        [TestCase]
        public void OperationCertificateValidateTargetTest()
        {
            Mock<OperationsEngine> engineMock = new Mock<OperationsEngine>();

            OperationCertificate certificate = new OperationCertificate();

            Assert.IsFalse(certificate.ValidateTarget(null));
            Assert.IsTrue(certificate.ValidateTarget(new Object()));
            Assert.IsTrue(certificate.ValidateTarget(new Object(), null));
        }

        /// <summary>
        /// Test method for target validation.
        /// </summary>
        [TestCase]
        public void OperationCertificateValidateParametersTest()
        {
            Mock<OperationsEngine> engineMock = new Mock<OperationsEngine>();

            OperationCertificate certificate = new OperationCertificate();
            Assert.IsTrue(certificate.ValidateParameters(null));
            Assert.IsTrue(certificate.ValidateParameters(null, null));
            Assert.IsTrue(certificate.ValidateParameters(null, null, null));
        }

        #endregion
    }
}
