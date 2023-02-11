// <copyright file="SegmentCollectionTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections.Segmentation;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Collections.Segmentation
{
    /// <summary>
    /// Test fixture for the <see cref="SegmentCollection" /> class.
    /// </summary>
    [TestFixture]
    public class SegmentCollectionTest
    {
        #region Private fields

        /// <summary>
        /// The mock of the raster service.
        /// </summary>
        private Mock<IRaster> _raster;

        /// <summary>
        /// The segment collection.
        /// </summary>
        private SegmentCollection _collection;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _raster = new Mock<IRaster>(MockBehavior.Strict);
            _raster.Setup(raster => raster.NumberOfBands).Returns(4);
            _raster.Setup(raster => raster.NumberOfColumns).Returns(20);
            _raster.Setup(raster => raster.NumberOfRows).Returns(15);
            _raster.Setup(raster => raster.GetValues(It.IsAny<Int32>(), It.IsAny<Int32>())).Returns(new UInt32[4]);
            _raster.Setup(raster => raster.GetFloatValues(It.IsAny<Int32>(), It.IsAny<Int32>())).Returns(new Double[4]);

            _collection = new SegmentCollection(_raster.Object);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void SegmentCollectionConstructorTest()
        {
            // create using raster

            SegmentCollection collection = new SegmentCollection(_raster.Object);
            Assert.AreEqual(_raster.Object.NumberOfColumns * _raster.Object.NumberOfRows, collection.Count);
            Assert.AreEqual(_raster.Object, collection.Raster);


            // create using copy constructor

            SegmentCollection cloneCollection = new SegmentCollection(collection);

            Assert.AreEqual(collection.Count, cloneCollection.Count);
            Assert.AreEqual(collection.Raster, cloneCollection.Raster);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => collection = new SegmentCollection((IRaster)null));
            Assert.Throws<ArgumentNullException>(() => collection = new SegmentCollection((SegmentCollection)null));
        }

        /// <summary>
        /// Test case for the <see cref="GetSegment" /> method.
        /// </summary>
        [Test]
        public void SegmentCollectionGetSegmentTest()
        {
            // test for all indices

            for (Int32 rowIndex = 0; rowIndex < _raster.Object.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < _raster.Object.NumberOfColumns; columnIndex++)
                {
                    Segment segment = _collection.GetSegment(rowIndex, columnIndex);

                    Assert.AreEqual(1, segment.Count);
                    Assert.AreEqual(_raster.Object.NumberOfBands, segment.NumberOfBands);
                    Assert.AreEqual(_raster.Object.NumberOfColumns * _raster.Object.NumberOfRows, _collection.Count);
                    Assert.IsTrue(_collection.Contains(segment));
                }


            // test repeating queries

            Random random = new Random(0);
            for (Int32 queryIndex = 0; queryIndex < 10; queryIndex++)
            {
                Int32 rowIndex = random.Next(0, _raster.Object.NumberOfRows);
                Int32 columnIndex = random.Next(0, _raster.Object.NumberOfColumns);
                Segment segment = _collection.GetSegment(rowIndex, columnIndex);

                Segment otherSegment = _collection.GetSegment(rowIndex, columnIndex);

                Assert.AreEqual(segment, otherSegment);

                for (Int32 otherQueryIndex = 0; otherQueryIndex < 10; otherQueryIndex++)
                    otherSegment = _collection.GetSegment(random.Next(0, _raster.Object.NumberOfRows), random.Next(0, _raster.Object.NumberOfColumns));

                otherSegment = _collection.GetSegment(rowIndex, columnIndex);

                Assert.AreEqual(segment, otherSegment);
            }

            // exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.GetSegment(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.GetSegment(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.GetSegment(_raster.Object.NumberOfRows, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.GetSegment(0, _raster.Object.NumberOfColumns));
        }

        /// <summary>
        /// Test case for the <see cref="GetSegments" /> method.
        /// </summary>
        [Test]
        public void SegmentCollectionGetSegmentsTest()
        {
            Segment[] segments = _collection.GetSegments().ToArray();

            Assert.AreEqual(_collection.Count, segments.Length);

            foreach (Segment segment in segments)
            {
                Assert.IsTrue(_collection.Contains(segment));
            }
        }

        /// <summary>
        /// Test case for the <see cref="MergeSegments" /> method.
        /// </summary>
        [Test]
        public void SegmentCollectionMergeSegmentsTest()
        {
            // merge using segments

            SegmentCollection collection = new SegmentCollection(_raster.Object);
            Segment segment = collection.GetSegment(0, 0);
            Int32 count = collection.Count;

            for (Int32 rowIndex = 0; rowIndex < _raster.Object.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < _raster.Object.NumberOfColumns; columnIndex++)
                {
                    Segment otherSegment = collection.GetSegment(rowIndex, columnIndex);
                    collection.MergeSegments(segment, otherSegment);

                    Assert.IsFalse(segment != otherSegment && collection.Contains(segment) && collection.Contains(otherSegment));

                    segment = collection.GetSegment(rowIndex, columnIndex);
                    
                    Assert.AreEqual(count, collection.Count);
                    count--;
                }

            Assert.AreEqual(_raster.Object.NumberOfColumns * _raster.Object.NumberOfRows, segment.Count);


            // merge using segment and indices

            collection = new SegmentCollection(_raster.Object);
            segment = collection.GetSegment(0, 0);
            count = collection.Count;

            for (Int32 rowIndex = 0; rowIndex < _raster.Object.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < _raster.Object.NumberOfColumns; columnIndex++)
                {
                    collection.MergeSegments(segment, rowIndex, columnIndex);

                    Segment otherSegment = collection.GetSegment(rowIndex, columnIndex);

                    Assert.AreEqual(segment, otherSegment);
                    Assert.AreEqual(count, collection.Count);
                    count--;
                }

            Assert.AreEqual(_raster.Object.NumberOfColumns * _raster.Object.NumberOfRows, segment.Count);


            // merge using indices

            collection = new SegmentCollection(_raster.Object);
            segment = collection.GetSegment(0, 0);
            count = collection.Count;

            for (Int32 rowIndex = 0; rowIndex < _raster.Object.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < _raster.Object.NumberOfColumns; columnIndex++)
                {
                    collection.MergeSegments(0, 0, rowIndex, columnIndex);

                    Segment otherSegment = collection.GetSegment(rowIndex, columnIndex);

                    Assert.AreEqual(segment, otherSegment);
                    Assert.AreEqual(count, collection.Count);
                    count--;
                }

            Assert.AreEqual(_raster.Object.NumberOfColumns * _raster.Object.NumberOfRows, segment.Count);
        }

        #endregion
    }
}
