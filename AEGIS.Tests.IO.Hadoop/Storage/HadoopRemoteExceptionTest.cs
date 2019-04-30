/// <copyright file="HadoopRemoteExceptionTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Storage;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.IO.Storage
{
    /// <summary>
    /// Test fixture for class <see cref="HadoopRemoteException"/>.
    /// </summary>
    [TestFixture]
    public class HadoopRemoteExceptionTest
    {
        #region Test cases

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [TestCase]
        public void HadoopRemoteExceptionConstructorTest()
        {
            // test case 1: no parameters

            HadoopRemoteException exception = new HadoopRemoteException();

            Assert.AreEqual(exception.ExceptionName, null);
            Assert.AreEqual(exception.JavaClassName, null);

            // test case 2: message parameter

            exception = new HadoopRemoteException("Exception message.");

            Assert.AreEqual(exception.Message, "Exception message.");
            Assert.AreEqual(exception.ExceptionName, null);
            Assert.AreEqual(exception.JavaClassName, null);

            // test case 3: message, name and class parameters

            exception = new HadoopRemoteException("Exception message.", "Exception name.", "Class name.");

            Assert.AreEqual(exception.Message, "Exception message.");
            Assert.AreEqual(exception.ExceptionName, "Exception name.");
            Assert.AreEqual(exception.JavaClassName, "Class name.");
        }

        #endregion
    }
}
