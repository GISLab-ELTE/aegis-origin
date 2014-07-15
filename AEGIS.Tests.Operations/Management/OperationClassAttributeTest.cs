/// <copyright file="OperationTypeAttributeTest.cs" company="Eötvös Loránd University (ELTE)">
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
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Operations.Management
{
    /// <summary>
    /// Test fixture for class <see cref="OperationTypeAttribute"/>.
    /// </summary>
    [TestFixture]
    public class OperationClassAttributeTest
    {
        #region Test methods

        /// <summary>
        /// Test method for constructor.
        /// </summary>
        [TestCase]
        public void OperationTypeAttributeConstructorTest()
        {
            OperationClassAttribute attribute;

            // test case 1: inproper parameters

            Assert.Throws<ArgumentNullException>(() => attribute = new OperationClassAttribute(null, null));
            Assert.Throws<ArgumentNullException>(() => attribute = new OperationClassAttribute("AEGIS::000000", null));
            Assert.Throws<ArgumentNullException>(() => attribute = new OperationClassAttribute("AEGIS::000000", "Test Method", null));
            Assert.Throws<ArgumentException>(() => attribute = new OperationClassAttribute("AEGIS::000000", "Test Method", "1.0.0", typeof(Object)));

            // test case 2: proper parameters without certificate

            attribute = new OperationClassAttribute("AEGIS::000000", "Test Method");

            Assert.AreEqual("AEGIS::000000", attribute.Identifier);
            Assert.AreEqual("Test Method", attribute.Name);
            Assert.IsNotNull(attribute.GetCertificate());
            Assert.IsInstanceOf<OperationCertificate>(attribute.GetCertificate());

            // test case 3: proper parameters with certificate

            attribute = new OperationClassAttribute("AEGIS::000000", "Test Method", "1.0.0", typeof(OperationCertificate));

            Assert.IsNotNull(attribute.GetCertificate());
            Assert.IsInstanceOf<OperationCertificate>(attribute.GetCertificate());
        }

        #endregion
    }
}
