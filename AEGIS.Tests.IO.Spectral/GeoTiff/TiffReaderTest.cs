/// <copyright file="TiffReaderTest.cs" company="Eötvös Loránd University (ELTE)">
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
    public class TiffReaderTest
    {
        private String[] _inputFilePaths;

        [SetUp]
        public void SetUp()
        {
            _inputFilePaths = new String[0];
        }

        [TestCase]
        public void GeoTiffReaderOpenCloseTest()
        {
            foreach (String file in _inputFilePaths)
            {
                GeoTiffReader geoTiffReader = new GeoTiffReader(file);

                Assert.AreEqual(0, geoTiffReader.Parameters.Count);
                Assert.IsNotNull(geoTiffReader.BaseStream);
                Assert.IsNotNull(geoTiffReader.Factory);
                Assert.IsNotNull(geoTiffReader.Path);
                Assert.IsFalse(geoTiffReader.EndOfStream);

                geoTiffReader.Close();

                Assert.Throws<ObjectDisposedException>(() => geoTiffReader.Read());
                Assert.Throws<ObjectDisposedException>(() => { Stream value = geoTiffReader.BaseStream; });
                Assert.Throws<ObjectDisposedException>(() => { Boolean value = geoTiffReader.EndOfStream; });
            }
        }

        [TestCase]
        public void GeoTiffReaderReadToEndTest()
        {
            foreach (String file in _inputFilePaths)
            {
                GeoTiffReader geoTiffReader = new GeoTiffReader(file);

                Assert.AreEqual(0, geoTiffReader.Parameters.Count);
                Assert.IsNotNull(geoTiffReader.BaseStream);
                Assert.IsNotNull(geoTiffReader.Factory);
                Assert.IsNotNull(geoTiffReader.Path);
                Assert.IsFalse(geoTiffReader.EndOfStream);

                IList<IGeometry> result = geoTiffReader.ReadToEnd();

                Assert.IsTrue(geoTiffReader.EndOfStream);
                Assert.AreEqual(0, geoTiffReader.ReadToEnd().Count);

                geoTiffReader.Close();
            }
        }
    }
}
