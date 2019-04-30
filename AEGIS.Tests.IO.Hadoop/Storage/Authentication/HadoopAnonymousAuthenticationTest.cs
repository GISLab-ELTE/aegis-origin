/// <copyright file="HadoopAnonymousAuthenticationTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.IO.Storage.Authentication;
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests.IO.Storage.Authentication
{
    /// <summary>
    /// Test fixture for class <see cref="HadoopAnonymousAuthentication"/>.
    /// </summary>
    [TestFixture]
    public class HadoopAnonymousAuthenticationTest
    {
        #region Test cases

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [TestCase]
        public void HadoopAnonymousAuthenticationConstructorTest()
        {
            HadoopAnonymousAuthentication authentication = new HadoopAnonymousAuthentication();

            Assert.AreEqual(String.Empty, authentication.Request);
            Assert.AreEqual(FileSystemAuthenticationType.Anonymous, authentication.AutenticationType);
        }

        #endregion
    }
}
