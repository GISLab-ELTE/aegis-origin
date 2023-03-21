// <copyright file="OperationParameterCollectionAttributeTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Management;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.Operations.Management
{
    /// <summary>
    /// Test fixture for class <see cref="OperationParameterCollectionAttribute"/>.
    /// </summary>
    [TestFixture]
    public class OperationParameterCollectionAttributeTest
    {
        #region Test methods

        /// <summary>
        /// Test method for constructor.
        /// </summary>
        [TestCase]
        public void OperationParameterCollectionAttributeConstructorTest()
        {
            OperationParameterCollectionAttribute attribute = new OperationParameterCollectionAttribute();

            Assert.AreEqual(typeof(OperationParameter), attribute.Type);
        }

        #endregion
    }
}
