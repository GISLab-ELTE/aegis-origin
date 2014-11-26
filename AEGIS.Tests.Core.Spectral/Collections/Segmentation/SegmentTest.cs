/// <copyright file="SegmentTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Collections.Segmentation;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Collections.Segmentation
{
    /// <summary>
    /// Test fixture for the <see cref="Segment" /> class.
    /// </summary>
    [TestFixture]
    public class SegmentTest
    {
        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void SegmentConstructorTest()
        {
            // integer values

            UInt32[] intValues = Enumerable.Range(1, 4).Select(value => (UInt32)value).ToArray();

            Segment segment = new Segment(intValues);

            Assert.AreEqual(intValues.Length, segment.NumberOfBands);
            Assert.AreEqual(1, segment.Count);
            Assert.IsTrue(intValues.Select(value => (Double)value).SequenceEqual(segment.Mean));
            Assert.IsTrue(segment.Variance.All(value => value == 0));
            Assert.IsTrue(segment.Covariance.Cast<Double>().All(value => value == 0));


            // float values

            Double[] floatValues = Enumerable.Range(1, 4).Select(value => (Double)value).ToArray();

            segment = new Segment(floatValues);

            Assert.AreEqual(floatValues.Length, segment.NumberOfBands);
            Assert.AreEqual(1, segment.Count);
            Assert.IsTrue(floatValues.SequenceEqual(segment.Mean));
            Assert.IsTrue(segment.Variance.All(value => value == 0));
            Assert.IsTrue(segment.Covariance.Cast<Double>().All(value => value == 0));


            // no values

            segment = new Segment(4);

            Assert.AreEqual(floatValues.Length, segment.NumberOfBands);
            Assert.AreEqual(0, segment.Count);
            Assert.IsTrue(Enumerable.Repeat(0.0, 4).SequenceEqual(segment.Mean));
            Assert.IsTrue(segment.Variance.All(value => value == 0));
            Assert.IsTrue(segment.Covariance.Cast<Double>().All(value => value == 0));


            // exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => segment = new Segment(0));
            Assert.Throws<ArgumentNullException>(() => segment = new Segment((UInt32[])null));
            Assert.Throws<ArgumentNullException>(() => segment = new Segment((Double[])null));
            Assert.Throws<ArgumentException>(() => segment = new Segment(new UInt32[0]));
            Assert.Throws<ArgumentException>(() => segment = new Segment(new Double[0]));
        }

        #endregion
    }
}
