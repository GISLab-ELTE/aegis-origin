/// <copyright file="Raster8Test.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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
    [TestFixture]
    public class Raster8Test
    {
        [TestCase]
        public void Raster8ConstructorTest()
        {
            // test case 1: normal parameters

            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10000; j += 2000)
                    for (Int32 k = 0; k < 10000; k += 2000)
                    {
                        Raster8 raster = new Raster8(null, i, j, k, Enumerable.Repeat(8, i).ToArray(), null, null);

                        Assert.AreEqual(i, raster.SpectralResolution);
                        Assert.AreEqual(j, raster.NumberOfRows);
                        Assert.AreEqual(k, raster.NumberOfColumns);
                        Assert.AreEqual(RasterRepresentation.Integer, raster.Representation);
                        Assert.IsFalse(raster.IsMapped);
                        Assert.IsTrue(raster.IsReadable);
                        Assert.IsTrue(raster.IsWritable);
                        Assert.IsInstanceOf<RasterFactory>(raster.Factory);
                        Assert.IsTrue(raster.HistogramValues.All(values => values.Sum() == raster.NumberOfColumns * raster.NumberOfRows && values[0] == raster.NumberOfColumns * raster.NumberOfRows));
                        Assert.IsTrue(raster.RadiometricResolutions.All(resolution => resolution == 8));
                    }


            // test case 2: argument out of range exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, -1, 1, 1, null, null, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, -1, 1, null, null, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, -1, null, null, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, new Int32[] { -1 }, null, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, new Int32[] { 33 }, null, null); });


            // test case 3: argument exceptions

            Assert.Throws<ArgumentException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, new Int32[] { 8, 8 }, null, null); });
            Assert.Throws<ArgumentException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, null, SpectralRanges.RGB, null); });
        }

        [TestCase]
        public void Raster8ValueTest()
        {
            Raster8 raster = new Raster8(null, 3, 9, 27, null, SpectralRanges.RGB, null);

            // test case 1: single values

            for (Int32 i = 0; i < raster.SpectralResolution; i++)
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
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, 0, raster.SpectralResolution));

            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(-1, 0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(raster.NumberOfRows, 0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, -1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, raster.NumberOfColumns, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, 0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, 0, raster.SpectralResolution, 0));
        }

        [TestCase]
        public void Raster8HistogramTest()
        {
            Int32[] histogramValues = new Int32[Byte.MaxValue + 1];
            Random random = new Random();

            Raster8 raster = new Raster8(null, 1, 100, 100, null, null, null);
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
    }
}
