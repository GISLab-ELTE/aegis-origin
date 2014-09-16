/// <copyright file="Raster8Test.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Raster;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Collections.Spectral
{
    /// <summary>
    /// Test fixture for the <see cref="Raster8"/> class.
    /// </summary>
    [TestFixture]
    public class Raster8Test
    {
        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void Raster8ConstructorTest()
        {
            // normal parameters

            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10000; j += 2000)
                    for (Int32 k = 0; k < 10000; k += 2000)
                    {
                        Raster8 raster = new Raster8(null, i, j, k, Enumerable.Repeat(8, i).ToArray(), null);

                        Assert.AreEqual(i, raster.NumberOfBands);
                        Assert.AreEqual(j, raster.NumberOfRows);
                        Assert.AreEqual(k, raster.NumberOfColumns);
                        Assert.AreEqual(RasterFormat.Integer, raster.Format);
                        Assert.IsFalse(raster.IsMapped);
                        Assert.IsTrue(raster.IsReadable);
                        Assert.IsTrue(raster.IsWritable);
                        Assert.IsInstanceOf<RasterFactory>(raster.Factory);
                        Assert.IsTrue(raster.HistogramValues.All(values => values.Sum() == raster.NumberOfColumns * raster.NumberOfRows && values[0] == raster.NumberOfColumns * raster.NumberOfRows));
                        Assert.IsTrue(raster.RadiometricResolutions.All(resolution => resolution == 8));
                    }


            // argument out of range exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, -1, 1, 1, null, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, -1, 1, null, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, -1, null, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, new Int32[] { -1 }, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, new Int32[] { 33 }, null); });


            // argument exceptions

            Assert.Throws<ArgumentException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, new Int32[] { 8, 8 }, null); });
        }

        /// <summary>
        /// Test case for value setting and getting.
        /// </summary>
        [Test]
        public void Raster8ValueTest()
        {
            Raster8 raster = new Raster8(null, 3, 9, 27, null, null);

            // single values

            for (Int32 i = 0; i < raster.NumberOfBands; i++)
                for (Int32 j = 0; j < raster.NumberOfRows; j++)
                    for (Int32 k = 0; k < raster.NumberOfColumns; k++)
                    {
                        raster.SetValue(j, k, i, (UInt32)(i * j * k));

                        Assert.AreEqual((i * j * k) % (Byte.MaxValue + 1), raster.GetValue(j, k, i), 0);
                    }


            // test case 2: multiple values

            for (Int32 j = 0; j < raster.NumberOfRows; j++)
                for (Int32 k = 0; k < raster.NumberOfColumns; k++)
                {
                    raster.SetValues(j, k, new UInt32[] { (UInt32)j, (UInt32)k, (UInt32)(j * k) });

                    Assert.AreEqual(j, raster.GetValues(j, k)[0], 0);
                    Assert.AreEqual(k, raster.GetValues(j, k)[1], 0);
                    Assert.AreEqual(j * k, raster.GetValues(j, k)[2], 0);
                }


            // test case 3: argument out of range exceptions

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
        public void Raster8HistogramTest()
        {
            Int32[] histogramValues = new Int32[Byte.MaxValue + 1];
            Random random = new Random();

            Raster8 raster = new Raster8(null, 1, 100, 100, null, null);
            for (Int32 j = 0; j < raster.NumberOfRows; j++)
                for (Int32 k = 0; k < raster.NumberOfColumns; k++)
                {
                    Byte value = (Byte)random.Next();
                    raster.SetValue(j, k, 0, value);

                    histogramValues[value]++;
                }

            for (Int32 i = 0; i < raster.GetHistogramValues(0).Count; i++)
            {
                Assert.AreEqual(histogramValues[i], raster.GetHistogramValues(0)[i]);
            }
        }

        #endregion
    }
}
