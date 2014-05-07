/// <copyright file="EsriRawImageReaderTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.RawImage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.Tests.IO.RawImage
{
    [TestFixture]
    public class EsriRawImageReaderTest
    {
        private String[] _inputFilePaths;

        [SetUp]
        public void SetUp()
        {
            _inputFilePaths = new String[0];
        }

        [TestCase]
        public void EsriRawImageReaderOpenCloseTest()
        {
            foreach (String file in _inputFilePaths)
            {
                EsriRawImageReader esriRawImageReader = new EsriRawImageReader(file);

                Assert.AreEqual(0, esriRawImageReader.Parameters.Count);
                Assert.IsNotNull(esriRawImageReader.BaseStream);
                Assert.IsNotNull(esriRawImageReader.Factory);
                Assert.IsNotNull(esriRawImageReader.Path);
                Assert.IsFalse(esriRawImageReader.EndOfStream);

                esriRawImageReader.Close();

                Assert.Throws<ObjectDisposedException>(() => esriRawImageReader.Read());
                Assert.Throws<ObjectDisposedException>(() => { Stream value = esriRawImageReader.BaseStream; });
                Assert.Throws<ObjectDisposedException>(() => { Boolean value = esriRawImageReader.EndOfStream; });
            }
        }

        [TestCase]
        public void EsriRawImageReaderReadToEndTest()
        {
            foreach (String file in _inputFilePaths)
            {
                EsriRawImageReader esriRawImageReader = new EsriRawImageReader(file);

                Assert.AreEqual(0, esriRawImageReader.Parameters.Count);
                Assert.IsNotNull(esriRawImageReader.BaseStream);
                Assert.IsNotNull(esriRawImageReader.Factory);
                Assert.IsNotNull(esriRawImageReader.Path);
                Assert.IsFalse(esriRawImageReader.EndOfStream);

                IList<IGeometry> result = esriRawImageReader.ReadToEnd();

                Assert.IsTrue(esriRawImageReader.EndOfStream);
                Assert.AreEqual(0, esriRawImageReader.ReadToEnd().Count);

                esriRawImageReader.Close();
            }
        }
    }
}
