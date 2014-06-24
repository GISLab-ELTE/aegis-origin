/// <copyright file="AssemblyOperationProviderTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Operations.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ELTE.AEGIS.Tests.Operations.Providers
{
    /// <summary>
    /// Test fixture for class <see cref="AssemblyOperationProvider"/>.
    /// </summary>
    [TestFixture]
    public class AssemblyOperationProviderTest
    {
        #region Test methods

        /// <summary>
        /// Test method for constructor.
        /// </summary>
        [TestCase]
        public void AssemblyOperationProviderConstructorTest()
        {
            // test case 1: no assembly

            AssemblyOperationProvider provider;

            Assert.Throws<ArgumentNullException>(() => provider = new AssemblyOperationProvider(null));


            // test case 2: executing assembly

            provider = new AssemblyOperationProvider(Assembly.GetExecutingAssembly());
            
            Assert.AreEqual(Assembly.GetExecutingAssembly().Location, provider.Uri.LocalPath);
        }

        /// <summary>
        /// Test method for operation query.
        /// </summary>
        [TestCase]
        public void AssemblyOperationProviderQueryTest()
        {
            // test case 1: no operations

            AssemblyOperationProvider provider = new AssemblyOperationProvider(Assembly.GetAssembly(typeof(AssemblyOperationProvider)));

            Assert.AreEqual(0, provider.GetMethods().Count);
            Assert.AreEqual(0, provider.GetOperations().Count);

            Assert.AreEqual(0, provider.GetOperations(TestOperationMethods.TestMethodWithParameter).Count);

            Assert.Throws<ArgumentNullException>(() => provider.GetOperations(null));

            // test case 2: one operation

            provider = new AssemblyOperationProvider(Assembly.GetExecutingAssembly());

            Assert.AreEqual(2, provider.GetMethods().Count);
            Assert.AreEqual(1, provider.GetOperations().Count);
            Assert.AreEqual(TestOperationMethods.TestMethodWithoutParameter, provider.GetMethods()[0]);
            Assert.AreEqual(typeof(TestOperation), provider.GetOperations()[provider.GetMethods()[0]][0]);
            Assert.AreEqual(typeof(TestOperation), provider.GetOperations(provider.GetMethods()[0])[0]);
        }

        #endregion
    }
}
