// <copyright file="RevisionDescriptorTest.cs" company="Eötvös Loránd University (ELTE)">
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
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.Versioning
{
    /// <summary>
    /// Test fixture for the <see cref="RevisionDescriptor{TVersionKey}"/> class.
    /// </summary>
    /// <author>Máté Cserép</author>
    [TestFixture]
    public class RevisionDescriptorTest
    {
        /// <summary>
        /// Tests the properties of revision descriptors in various scenarios.
        /// </summary>
        [Test]
        public void RevisionDescriptorPropertyTest()
        {
            //
            // Test linear version history
            //

            var descriptors = new RevisionDescriptor<Int32>[5];
            var n = descriptors.Length;
            for (Int32 i = 0; i < n; ++i)
                descriptors[i] = new RevisionDescriptor<Int32>(i + 1, i > 0 ? descriptors[i - 1] : null);

            // Assert index: 0
            Assert.AreEqual(1, descriptors[0].Version);
            Assert.AreEqual(null, descriptors[0].PrecedingRevision);
            Assert.AreEqual(0, descriptors[0].PrecedingRevisions.Length);
            Assert.AreEqual(1, descriptors[0].SucceedingRevisions.Length);
            Assert.AreEqual(descriptors[1], descriptors[0].SucceedingRevisions[0]);
            Assert.AreEqual(0, descriptors[0].MergedRevisions.Length);
            Assert.AreEqual(descriptors[0], descriptors[0].OriginRevision);
            Assert.IsTrue(descriptors[0].IsOrigin);
            Assert.IsFalse(descriptors[0].IsHead);

            // Assert index: [1; n-2]
            for (Int32 i = 1; i < n - 1; ++i)
            {
                Assert.AreEqual(i + 1, descriptors[i].Version);
                Assert.AreEqual(descriptors[i - 1], descriptors[i].PrecedingRevision);
                Assert.AreEqual(1, descriptors[i].PrecedingRevisions.Length);
                Assert.AreEqual(descriptors[i].PrecedingRevision, descriptors[i].PrecedingRevisions[0]);
                Assert.AreEqual(1, descriptors[i].SucceedingRevisions.Length);
                Assert.AreEqual(descriptors[i + 1], descriptors[i].SucceedingRevisions[0]);
                Assert.AreEqual(0, descriptors[i].MergedRevisions.Length);
                Assert.AreEqual(descriptors[0], descriptors[i].OriginRevision);
                Assert.IsFalse(descriptors[i].IsOrigin);
                Assert.IsFalse(descriptors[i].IsHead);
            }

            // Assert index: n-1
            Assert.AreEqual(n, descriptors[n - 1].Version);
            Assert.AreEqual(descriptors[n - 2], descriptors[n - 1].PrecedingRevision);
            Assert.AreEqual(1, descriptors[n - 1].PrecedingRevisions.Length);
            Assert.AreEqual(descriptors[n - 1].PrecedingRevision, descriptors[n - 1].PrecedingRevisions[0]);
            Assert.AreEqual(0, descriptors[n - 1].SucceedingRevisions.Length);
            Assert.AreEqual(0, descriptors[n - 1].MergedRevisions.Length);
            Assert.AreEqual(descriptors[0], descriptors[n - 1].OriginRevision);
            Assert.IsFalse(descriptors[n - 1].IsOrigin);
            Assert.IsTrue(descriptors[n - 1].IsHead);


            //
            // Test non-linear version history
            //

            descriptors = new RevisionDescriptor<Int32>[8];
            descriptors[0] = new RevisionDescriptor<Int32>(1, null);
            descriptors[1] = new RevisionDescriptor<Int32>(2, descriptors[0]);
            descriptors[2] = new RevisionDescriptor<Int32>(3, descriptors[1]);
            descriptors[3] = new RevisionDescriptor<Int32>(4, descriptors[2]);
            descriptors[4] = new RevisionDescriptor<Int32>(5, descriptors[1]);
            descriptors[5] = new RevisionDescriptor<Int32>(6, descriptors[4]);
            descriptors[6] = new RevisionDescriptor<Int32>(7, descriptors[2]);
            descriptors[7] = new RevisionDescriptor<Int32>(8, descriptors[6]);

            // Assert: origin
            Assert.IsTrue(descriptors[0].IsOrigin);
            Assert.IsFalse(descriptors[1].IsOrigin);
            Assert.IsFalse(descriptors[2].IsOrigin);
            Assert.IsFalse(descriptors[3].IsOrigin);
            Assert.IsTrue(descriptors[4].IsOrigin);
            Assert.IsFalse(descriptors[5].IsOrigin);
            Assert.IsTrue(descriptors[6].IsOrigin);
            Assert.IsFalse(descriptors[7].IsOrigin);

            Assert.AreEqual(descriptors[0], descriptors[0].OriginRevision);
            Assert.AreEqual(descriptors[0], descriptors[1].OriginRevision);
            Assert.AreEqual(descriptors[0], descriptors[2].OriginRevision);
            Assert.AreEqual(descriptors[0], descriptors[3].OriginRevision);
            Assert.AreEqual(descriptors[4], descriptors[4].OriginRevision);
            Assert.AreEqual(descriptors[4], descriptors[5].OriginRevision);
            Assert.AreEqual(descriptors[6], descriptors[6].OriginRevision);
            Assert.AreEqual(descriptors[6], descriptors[7].OriginRevision);

            // Assert: head
            Assert.IsFalse(descriptors[0].IsHead);
            Assert.IsFalse(descriptors[1].IsHead);
            Assert.IsFalse(descriptors[2].IsHead);
            Assert.IsTrue(descriptors[3].IsHead);
            Assert.IsFalse(descriptors[4].IsHead);
            Assert.IsTrue(descriptors[5].IsHead);
            Assert.IsFalse(descriptors[6].IsHead);
            Assert.IsTrue(descriptors[7].IsHead);

        
            //
            // Test multiple version merging
            //

            descriptors = new RevisionDescriptor<Int32>[10];
            descriptors[0] = new RevisionDescriptor<Int32>(1, null);
            descriptors[1] = new RevisionDescriptor<Int32>(2, descriptors[0]);
            descriptors[2] = new RevisionDescriptor<Int32>(3, descriptors[1]);
            descriptors[3] = new RevisionDescriptor<Int32>(4, descriptors[2]);
            descriptors[4] = new RevisionDescriptor<Int32>(5, descriptors[1]);
            descriptors[5] = new RevisionDescriptor<Int32>(6, descriptors[4]);
            descriptors[6] = new RevisionDescriptor<Int32>(7, descriptors[2]);
            descriptors[7] = new RevisionDescriptor<Int32>(8, descriptors[6]);
            descriptors[8] = new RevisionDescriptor<Int32>(9, descriptors[1]);
            descriptors[9] = new RevisionDescriptor<Int32>(10, descriptors[5], new[] { descriptors[3], descriptors[6] });

            // Assertion
            Assert.AreEqual(descriptors[5], descriptors[9].PrecedingRevision);
            Assert.AreEqual(3, descriptors[9].PrecedingRevisions.Length);
            Assert.Contains(descriptors[3], descriptors[9].PrecedingRevisions);
            Assert.Contains(descriptors[5], descriptors[9].PrecedingRevisions);
            Assert.Contains(descriptors[6], descriptors[9].PrecedingRevisions);
            Assert.AreEqual(2, descriptors[9].MergedRevisions.Length);
            Assert.Contains(descriptors[3], descriptors[9].MergedRevisions);
            Assert.Contains(descriptors[6], descriptors[9].MergedRevisions);
            Assert.AreEqual(descriptors[4], descriptors[9].OriginRevision);
            Assert.AreEqual(2, descriptors[6].SucceedingRevisions.Length);
            Assert.Contains(descriptors[7], descriptors[6].SucceedingRevisions);
            Assert.Contains(descriptors[9], descriptors[6].SucceedingRevisions);
            Assert.Contains(descriptors[3], descriptors[9].MergedRevisions);
            Assert.Contains(descriptors[6], descriptors[9].MergedRevisions);
            Assert.IsFalse(descriptors[9].IsOrigin);
            Assert.IsTrue(descriptors[9].IsHead);
            Assert.IsTrue(descriptors[3].IsHead);
        }
    }
}