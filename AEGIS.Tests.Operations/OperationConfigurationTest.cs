/// <copyright file="OperationConfigurationTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations
{
    /// <summary>
    /// Test fixture for class <see cref="OperationConfiguration"/>.
    /// </summary>
    [TestFixture]
    public class OperationConfigurationTest
    {
        #region Test methods

        /// <summary>
        /// Test method for the constructor.
        /// </summary>
        [TestCase]
        public void OperationConfigurationConstructorTest()
        {
            OperationConfiguration operation;
            Dictionary<OperationParameter, Object> parameters;
            
            // test case 1: null method
            
            Assert.Throws<ArgumentNullException>(() => operation = new OperationConfiguration(null, null));

            
            // test case 2: no parameters

            operation = new OperationConfiguration(TestOperationMethods.TestMethodWithoutParameter, null);

            Assert.AreEqual(TestOperationMethods.TestMethodWithoutParameter, operation.Method);
            Assert.AreEqual(null, operation.Parameters);

            // test case 3: only required parameter

            parameters = new Dictionary<OperationParameter, Object>();
            parameters[TestOperationParameters.TestRequiredParameter] = false;

            operation = new OperationConfiguration(TestOperationMethods.TestMethodWithParameter, parameters);

            Assert.AreEqual(TestOperationMethods.TestMethodWithParameter, operation.Method);
            Assert.AreEqual(1, operation.Parameters.Count);
            Assert.AreEqual(TestOperationParameters.TestRequiredParameter, operation.Parameters.Keys.First());
            Assert.AreEqual(false, operation.Parameters.Values.First());


            // test case 4: multiple parameters

            parameters = new Dictionary<OperationParameter, Object>();
            parameters[TestOperationParameters.TestRequiredParameter] = false;
            parameters[TestOperationParameters.TestOptionalParameter] = true;

            operation = new OperationConfiguration(TestOperationMethods.TestMethodWithParameter, parameters);

            Assert.AreEqual(TestOperationMethods.TestMethodWithParameter, operation.Method);
            Assert.AreEqual(2, operation.Parameters.Count);
            Assert.AreEqual(TestOperationParameters.TestRequiredParameter, operation.Parameters.Keys.First());
            Assert.AreEqual(TestOperationParameters.TestOptionalParameter, operation.Parameters.Keys.Last());
            

            // test case 5: bad parameter type

            parameters = new Dictionary<OperationParameter, Object>();
            parameters[TestOperationParameters.TestRequiredParameter] = TestOperationParameters.TestRequiredParameter;

            Assert.Throws<ArgumentException>(() => operation = new OperationConfiguration(TestOperationMethods.TestMethodWithParameter, parameters));


            // test case 6: bad parameter value

            parameters = new Dictionary<OperationParameter, Object>();
            parameters[TestOperationParameters.TestRequiredParameter] = false;
            parameters[TestOperationParameters.TestOptionalParameter] = false;

            Assert.Throws<ArgumentException>(() => operation = new OperationConfiguration(TestOperationMethods.TestMethodWithParameter, parameters));


            // test case 7: no parameters

            Assert.Throws<ArgumentNullException>(() => operation = new OperationConfiguration(TestOperationMethods.TestMethodWithParameter, null));


            // test case 7: missing parameter

            parameters = new Dictionary<OperationParameter, Object>();

            Assert.Throws<ArgumentException>(() => operation = new OperationConfiguration(TestOperationMethods.TestMethodWithParameter, parameters));
        }

        #endregion
    }
}
