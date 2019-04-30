/// <copyright file="Raster32Test.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Collections;
using ELTE.AEGIS.Raster;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Collections.Spectral
{
    /// <summary>
    /// Test fixture for the <see cref="Raster32"/> class.
    /// </summary>
    [TestFixture]
    public class Raster32Test
    {
        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void Raster32ConstructorTest()
        {
            // normal parameters

            for (Int32 numberOfBands = 0; numberOfBands < 10; numberOfBands++)
                for (Int32 numberOfRows = 0; numberOfRows < 1000; numberOfRows += 250)
                    for (Int32 numberOfColumns = 0; numberOfColumns < 1000; numberOfColumns += 250)
                    {
                        Raster32 raster = new Raster32(null, numberOfBands, numberOfRows, numberOfColumns, 8, null);

                        Assert.AreEqual(numberOfBands, raster.NumberOfBands);
                        Assert.AreEqual(numberOfRows, raster.NumberOfRows);
                        Assert.AreEqual(numberOfColumns, raster.NumberOfColumns);
                        Assert.AreEqual(8, raster.RadiometricResolution);
                        Assert.AreEqual(RasterFormat.Integer, raster.Format);
                        Assert.IsFalse(raster.IsMapped);
                        Assert.IsTrue(raster.IsReadable);
                        Assert.IsTrue(raster.IsWritable);
                        Assert.IsInstanceOf<RasterFactory>(raster.Factory);
                    }


            // argument out of range exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster32 raster = new Raster32(null, -1, 1, 1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster32 raster = new Raster32(null, 1, -1, 1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster32 raster = new Raster32(null, 1, 1, -1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster32 raster = new Raster32(null, 1, 1, 1, -1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster32 raster = new Raster32(null, 1, 1, 1, 65, null); });
        }

        /// <summary>
        /// Test case for value setting and getting.
        /// </summary>
        [Test]
        public void Raster32ValueTest()
        {
            Raster32 raster = new Raster32(null, 3, 9, 27, 32, null);

            // single values

            for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                    for (Int32 k = 0; k < raster.NumberOfColumns; k++)
                    {
                        raster.SetValue(rowIndex, k, bandIndex, (UInt32)(bandIndex * rowIndex * k));

                        Assert.AreEqual(bandIndex * rowIndex * k, raster.GetValue(rowIndex, k, bandIndex), 0);
                    }


            // multiple values

            for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                for (Int32 k = 0; k < raster.NumberOfColumns; k++)
                {
                    raster.SetValues(rowIndex, k, new UInt32[] { (UInt32)rowIndex, (UInt32)k, (UInt32)(rowIndex * k) });

                    Assert.AreEqual(rowIndex, raster.GetValues(rowIndex, k)[0], 0);
                    Assert.AreEqual(k, raster.GetValues(rowIndex, k)[1], 0);
                    Assert.AreEqual(rowIndex * k, raster.GetValues(rowIndex, k)[2], 0);
                }


            // argument out of range exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(-1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(raster.NumberOfRows, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, raster.NumberOfColumns, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, 0, raster.NumberOfBands));

            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(-1, 0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(raster.NumberOfRows, 0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, -1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, raster.NumberOfColumns, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, 0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, 0, raster.NumberOfBands, 0));
        }

        /// <summary>
        /// Test case for the histogram.
        /// </summary>
        [Test]
        public void Raster32HistogramTest()
        {
            SparseArray<Int32> histogramValues = new SparseArray<Int32>(UInt32.MaxValue + 1L);
            Random random = new Random();

            Raster32 raster = new Raster32(null, 1, 100, 100, 32, null);
            for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                {
                    UInt32 value = (UInt32)random.Next();
                    raster.SetValue(rowIndex, columnIndex, 0, value);

                    histogramValues[value]++;
                }

            for (Int32 valueIndex = 0; valueIndex < raster.GetHistogramValues(0).Count; valueIndex++)
            {
                Assert.AreEqual(histogramValues[valueIndex], raster.GetHistogramValues(0)[valueIndex]);
            }
        }

        #endregion
    }
}
