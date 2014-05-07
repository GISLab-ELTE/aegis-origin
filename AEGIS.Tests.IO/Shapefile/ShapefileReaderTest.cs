/// <copyright file="ShapefileReaderTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Shapefile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.Tests.IO
{
    [TestFixture]
    public class ShapefileReaderTest
    {
        private String[] _inputFilePaths;

        [SetUp]
        public void SetUp()
        {
            _inputFilePaths = new String[0];
        }

        [TestCase]
        public void ShapefileReaderOpenCloseTest()
        {
            foreach (String file in _inputFilePaths)
            {
                ShapefileReader shp = new ShapefileReader(file);

                Assert.AreEqual(0, shp.Parameters.Count);
                Assert.IsNotNull(shp.BaseStream);
                Assert.IsNotNull(shp.Factory);
                Assert.IsNotNull(shp.Path);
                Assert.IsFalse(shp.EndOfStream);

                shp.Close();

                Assert.Throws<ObjectDisposedException>(() => shp.Read());
                Assert.Throws<ObjectDisposedException>(() => { Stream value = shp.BaseStream; });
                Assert.Throws<ObjectDisposedException>(() => { Boolean value = shp.EndOfStream; });
            }
        }

        [TestCase]
        public void ShapefileReaderReadToEndTest()
        {
            foreach (String file in _inputFilePaths)
            {
                ShapefileReader shp = new ShapefileReader(file);

                Assert.AreEqual(0, shp.Parameters.Count);
                Assert.IsNotNull(shp.BaseStream);
                Assert.IsNotNull(shp.Factory);
                Assert.IsNotNull(shp.Path);
                Assert.IsFalse(shp.EndOfStream);

                IList<IGeometry> result = shp.ReadToEnd();

                Assert.IsTrue(shp.EndOfStream);
                Assert.AreEqual(0, shp.ReadToEnd().Count);

                shp.Close();
            }
        }
    }
}
