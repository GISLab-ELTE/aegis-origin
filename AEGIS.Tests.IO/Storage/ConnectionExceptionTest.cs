/// <copyright file="ConnectionExceptionTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Storage;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.IO.Storage
{
    /// <summary>
    /// Test fixture for class <see cref="ConnectionException"/>.
    /// </summary>
    [TestFixture]
    public class ConnectionExceptionTest
    {
        /// <summary>
        /// Test method for the constructor.
        /// </summary>
        [TestCase]
        public void ConnectionExceptionConstructorTest()
        {
            // test case 1: no parameters
            
            ConnectionException exception = new ConnectionException();

            Assert.AreEqual(null, exception.Path);


            // test case 2: message parameter

            exception = new ConnectionException("Exception message.");

            Assert.AreEqual("Exception message.", exception.Message);
            Assert.AreEqual(null, exception.Path);

            // test case 3: message and path parameters

            exception = new ConnectionException("Exception message.", "path");

            Assert.AreEqual("Exception message.", exception.Message);
            Assert.AreEqual("path", exception.Path);


            // test case 4: message and innerexception parameters

            Exception innerException = new Exception();
            exception = new ConnectionException("Exception message.", innerException);

            Assert.AreEqual("Exception message.", exception.Message);
            Assert.AreEqual(null, exception.Path);
            Assert.AreEqual(innerException, exception.InnerException);


            // test case 5: message, path and innerexception parameters

            exception = new ConnectionException("Exception message.", "path", innerException);

            Assert.AreEqual("Exception message.", exception.Message);
            Assert.AreEqual("path", exception.Path);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
