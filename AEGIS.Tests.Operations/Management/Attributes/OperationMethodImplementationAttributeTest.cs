/// <copyright file="OperationMethodImplementationAttributeTest.cs" company="Eötvös Loránd University (ELTE)">
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
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.Operations.Management
{
    /// <summary>
    /// Test fixture for the <see cref="OperationMethodImplementationAttribute"/ > class.
    /// </summary>
    [TestFixture]
    public class OperationMethodImplementationAttributeTest
    {
        #region Test methods

        /// <summary>
        /// Test method for constructor.
        /// </summary>
        [TestCase]
        public void OperationTypeAttributeConstructorTest()
        {
            OperationMethodImplementationAttribute attribute;

            // test case 1: improper parameters

            Assert.Throws<ArgumentNullException>(() => attribute = new OperationMethodImplementationAttribute(null, null));
            Assert.Throws<ArgumentNullException>(() => attribute = new OperationMethodImplementationAttribute(IdentifiedObject.UserDefinedIdentifier, null));
            Assert.Throws<ArgumentNullException>(() => attribute = new OperationMethodImplementationAttribute(IdentifiedObject.UserDefinedIdentifier, "Test Method", null));
            Assert.Throws<ArgumentException>(() => attribute = new OperationMethodImplementationAttribute(IdentifiedObject.UserDefinedIdentifier, "Test Method", "1.0.0", typeof(Object)));

            // test case 2: proper parameters without credential

            attribute = new OperationMethodImplementationAttribute(IdentifiedObject.UserDefinedIdentifier, "Test Method");

            Assert.AreEqual(IdentifiedObject.UserDefinedIdentifier, attribute.Identifier);
            Assert.AreEqual("Test Method", attribute.Name);
            Assert.IsNotNull(attribute.GetCredential());
            Assert.IsInstanceOf<OperationCredential>(attribute.GetCredential());

            // test case 3: proper parameters with credential

            attribute = new OperationMethodImplementationAttribute(IdentifiedObject.UserDefinedIdentifier, "Test Method", "1.0.0", typeof(OperationCredential));

            Assert.IsNotNull(attribute.GetCredential());
            Assert.IsInstanceOf<OperationCredential>(attribute.GetCredential());
        }

        #endregion
    }
}
