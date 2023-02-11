// <copyright file="KeyFactoryTest.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using ELTE.AEGIS.Versioning;
using ELTE.AEGIS.Versioning.Keys;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.Versioning
{
    /// <summary>
    /// Test fixture for the <see cref="IKeyFactory{TKey}"/> interface and its implementations.
    /// </summary>
    /// <author>Máté Cserép</author>
    [TestFixture]
    public class KeyFactoryTest
    {
        #region Test methods

        /// <summary>
        /// Test method for the <see cref="SequentialKeyFactory"/> class.
        /// </summary>
        [Test]
        public void SequentialKeyFactoryTest()
        {
            IKeyFactory<Int32> factory = new SequentialKeyFactory(1);
            for (Int32 i = 1; i <= 10; ++i)
            {
                Assert.AreEqual(i, factory.LastKey);
                Int32 previousKey = factory.LastKey;
                factory.CreateKey();
                Assert.Greater(factory.LastKey, previousKey);
            }
            Assert.AreEqual(1, factory.FirstKey);
        }

        /// <summary>
        /// Test method for the <see cref="GuidKeyFactory"/> class.
        /// </summary>
        [Test]
        public void GuidKeyFactoryTest()
        {
            IKeyFactory<Guid> factory = new GuidKeyFactory();
            Guid firstKey = factory.FirstKey;

            for (Int32 i = 0; i < 10; ++i)
            {
                Guid previousKey = factory.LastKey;
                factory.CreateKey();
                Assert.AreNotEqual(previousKey, factory.LastKey);
            }
            Assert.AreEqual(firstKey, factory.FirstKey);
        }

        #endregion
    }
}
