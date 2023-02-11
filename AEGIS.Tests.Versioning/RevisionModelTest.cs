// <copyright file="RevisionModelTest.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using ELTE.AEGIS.Versioning;
using ELTE.AEGIS.Versioning.Keys;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.Versioning
{
    /// <summary>
    /// Test fixture for the <see cref="RevisionModel{TVersionKey}"/> class.
    /// </summary>
    /// <author>Máté Cserép</author>
    [TestFixture]
    public class RevisionModelTest
    {
        /// <summary>
        /// Tests the properties and getter methods of the <see cref="RevisionModel{TVersionKey}"/> in various scenarios.
        /// </summary>
        [Test]
        public void RevisionModelPropertyTest()
        {
            //
            // Test linear version model
            //

            IKeyFactory<Int32> factory = new SequentialKeyFactory(1);
            RevisionModel<Int32> model = new RevisionModel<Int32>(factory);
            Assert.AreEqual(factory.LastKey, model.InitRevision.Version);
            Assert.AreEqual(factory.LastKey, model.HeadRevision.Version);
            Assert.AreEqual(factory.LastKey, model.MainRevision.Version);

            for (Int32 i = 2; i <= 10; ++i)
                model.CreateRevision(factory.LastKey);

            Assert.AreEqual(factory.FirstKey, model.InitRevision.Version);
            Assert.AreEqual(factory.LastKey, model.HeadRevision.Version);
            Assert.AreEqual(factory.LastKey, model.MainRevision.Version);
            Assert.AreEqual(5, model.GetRevision(6).PrecedingRevision.Version);
            Assert.AreEqual(10, model.GetHeadRevision(5).Version);
            Assert.Throws(typeof (KeyNotFoundException), () => model.CreateRevision(factory.CreateKey()));


            //
            // Test non-linear version model
            //

            factory = new SequentialKeyFactory(1);
            model = new RevisionModel<Int32>(factory);
            model.CreateRevision(1); // 2
            model.CreateRevision(2); // 3
            model.CreateRevision(3); // 4
            model.CreateRevision(2); // 5
            model.CreateRevision(5); // 6
            model.CreateRevision(4); // 7
            model.CreateRevision(1); // 8

            // Assert: properties
            Assert.AreEqual(factory.FirstKey, model.InitRevision.Version);
            Assert.AreEqual(factory.LastKey, model.HeadRevision.Version);
            Assert.AreEqual(7, model.MainRevision.Version);

            // Assert: head revisions
            Assert.AreEqual(7, model.GetHeadRevision(1).Version);
            Assert.AreEqual(7, model.GetHeadRevision(2).Version);
            Assert.AreEqual(7, model.GetHeadRevision(3).Version);
            Assert.AreEqual(7, model.GetHeadRevision(4).Version);
            Assert.AreEqual(6, model.GetHeadRevision(5).Version);
            Assert.AreEqual(6, model.GetHeadRevision(6).Version);
            Assert.AreEqual(7, model.GetHeadRevision(7).Version);
            Assert.AreEqual(8, model.GetHeadRevision(8).Version);


            //
            // Test version model with merge
            //

            factory = new SequentialKeyFactory(1);
            model = new RevisionModel<Int32>(factory);
            model.CreateRevision(1);                 // 2
            model.CreateRevision(2);                 // 3
            model.CreateRevision(3);                 // 4
            model.CreateRevision(2);                 // 5
            model.CreateRevision(5);                 // 6
            model.CreateRevision(4);                 // 7
            model.CreateRevision(1);                 // 8
            model.CreateRevision(8);                 // 9
            model.CreateRevision(5, new[] { 7, 9 }); // 10

            // Assert: properties
            Assert.AreEqual(factory.FirstKey, model.InitRevision.Version);
            Assert.AreEqual(factory.LastKey, model.HeadRevision.Version);
            Assert.AreEqual(7, model.MainRevision.Version);

            // Assert: head revisions
            Assert.AreEqual(7, model.GetHeadRevision(1).Version);
            Assert.AreEqual(7, model.GetHeadRevision(2).Version);
            Assert.AreEqual(7, model.GetHeadRevision(3).Version);
            Assert.AreEqual(7, model.GetHeadRevision(4).Version);
            Assert.AreEqual(6, model.GetHeadRevision(5).Version);
            Assert.AreEqual(6, model.GetHeadRevision(6).Version);
            Assert.AreEqual(7, model.GetHeadRevision(7).Version);
            Assert.AreEqual(9, model.GetHeadRevision(8).Version);
            Assert.AreEqual(9, model.GetHeadRevision(9).Version);
            Assert.AreEqual(10, model.GetHeadRevision(10).Version);
        }
    }
}
