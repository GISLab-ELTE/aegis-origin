/// <copyright file="GeoTiffReaderTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.GeoTiff;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.Tests.IO.GeoTiff
{
    [TestFixture]
    public class GeoTiffReaderTest
    {
        private String[] _inputFilePaths;

        [SetUp]
        public void SetUp()
        {
            _inputFilePaths = new String[0];
        }

        [TestCase]
        public void TiffReaderOpenCloseTest()
        {
            foreach (String file in _inputFilePaths)
            {
                TiffReader tiffReader = new TiffReader(file);

                Assert.AreEqual(0, tiffReader.Parameters.Count);
                Assert.IsNotNull(tiffReader.BaseStream);
                Assert.IsNull(tiffReader.Factory);
                Assert.IsNotNull(tiffReader.Path);
                Assert.IsFalse(tiffReader.EndOfStream);

                tiffReader.Close();

                Assert.Throws<ObjectDisposedException>(() => tiffReader.Read());
                Assert.Throws<ObjectDisposedException>(() => { Stream value = tiffReader.BaseStream; });
                Assert.Throws<ObjectDisposedException>(() => { Boolean value = tiffReader.EndOfStream; });
            }
        }

        [TestCase]
        public void TiffReaderReadToEndTest()
        {
            foreach (String file in _inputFilePaths)
            {
                TiffReader tiffReader = new TiffReader(file);

                Assert.AreEqual(0, tiffReader.Parameters.Count);
                Assert.IsNotNull(tiffReader.BaseStream);
                Assert.IsNull(tiffReader.Factory);
                Assert.IsNotNull(tiffReader.Path);
                Assert.IsFalse(tiffReader.EndOfStream);

                IList<IGeometry> result = tiffReader.ReadToEnd();

                Assert.IsTrue(tiffReader.EndOfStream);
                Assert.AreEqual(0, tiffReader.ReadToEnd().Count);

                tiffReader.Close();
            }
        }
    }
}
