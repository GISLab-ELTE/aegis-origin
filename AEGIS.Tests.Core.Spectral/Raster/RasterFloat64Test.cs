/// <copyright file="RasterFloat64Test.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Raster;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Collections.Spectral
{
    /// <summary>
    /// Test fixture for the <see cref="RasterFloat64"/> class.
    /// </summary>
    [TestFixture]
    public class RasterFloat64Test
    {
        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void RasterFloat64ConstructorTest()
        {
            // normal parameters

            for (Int32 numberOfBands = 0; numberOfBands < 10; numberOfBands++)
                for (Int32 numberOfRows = 0; numberOfRows < 1000; numberOfRows += 2500)
                    for (Int32 numberOfColumns = 0; numberOfColumns < 1000; numberOfColumns += 2050)
                    {
                        RasterFloat64 raster = new RasterFloat64(null, numberOfBands, numberOfRows, numberOfColumns, 32, null);

                        Assert.AreEqual(numberOfBands, raster.NumberOfBands);
                        Assert.AreEqual(numberOfRows, raster.NumberOfRows);
                        Assert.AreEqual(numberOfColumns, raster.NumberOfColumns);
                        Assert.AreEqual(32, raster.RadiometricResolution);
                        Assert.AreEqual(RasterFormat.Floating, raster.Format);
                        Assert.IsFalse(raster.IsMapped);
                        Assert.IsTrue(raster.IsReadable);
                        Assert.IsTrue(raster.IsWritable);
                        Assert.IsInstanceOf<RasterFactory>(raster.Factory);
                    }

            
            // argument out of range exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => { RasterFloat64 raster = new RasterFloat64(null, -1, 1, 1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { RasterFloat64 raster = new RasterFloat64(null, 1, -1, 1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { RasterFloat64 raster = new RasterFloat64(null, 1, 1, -1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { RasterFloat64 raster = new RasterFloat64(null, 1, 1, 1, -1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { RasterFloat64 raster = new RasterFloat64(null, 1, 1, 1, 65, null); });
        }

        /// <summary>
        /// Test case for value setting and getting.
        /// </summary>
        [Test]
        public void RasterFloat64ValueTest()
        {
            RasterFloat64 raster = new RasterFloat64(null, 3, 9, 27, 64, null);

            // single values

            for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                    {
                        raster.SetFloatValue(rowIndex, columnIndex, bandIndex, 1.0 / (bandIndex * rowIndex * columnIndex));

                        Assert.AreEqual(1.0 / (bandIndex * rowIndex * columnIndex), raster.GetFloatValue(rowIndex, columnIndex, bandIndex), 0);
                    }


            // multiple values

            for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                {
                    raster.SetFloatValues(rowIndex, columnIndex, new Double[] { 1.0 / rowIndex, 1.0 / columnIndex, 1.0 / (rowIndex * columnIndex) });

                    Assert.AreEqual(1.0 / rowIndex, raster.GetFloatValues(rowIndex, columnIndex)[0], 0);
                    Assert.AreEqual(1.0 / columnIndex, raster.GetFloatValues(rowIndex, columnIndex)[1], 0);
                    Assert.AreEqual(1.0 / (rowIndex * columnIndex), raster.GetFloatValues(rowIndex, columnIndex)[2], 0);
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

        #endregion
    }
}
