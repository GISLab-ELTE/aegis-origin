/// <copyright file="GeoJsonReaderTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Norbert Vass</author>
/// <author>Máté Cserép</author>

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.IO.GeoJSON;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ELTE.AEGIS.Tests.IO.GeoJson
{
    [TestFixture]
    public class GeoJsonReaderTest
    {
        private string[] _inputFilePaths;
        private string _invalidInputJsonFilePath;
        private string _unsupportedCrsJsonFilePath;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            string prefix = "file://" + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "_data");
            _inputFilePaths = new string[3]
            {
                Path.Combine(prefix, "1.geojson"),
                Path.Combine(prefix, "2.geojson"),
                Path.Combine(prefix, "3.geojson"),
            };

            _invalidInputJsonFilePath = Path.Combine(prefix, "a.json");
            _unsupportedCrsJsonFilePath = Path.Combine(prefix, "b.json");
        }

        /// <summary>
        /// Test method for opening and closing GeoJson files.
        /// </summary>
        [Test]
        public void GeoJsonReaderOpenCloseTest()
        {
            foreach (string path in _inputFilePaths)
            {
                GeoJsonReader reader = new GeoJsonReader(path);

                Assert.AreEqual(0, reader.Parameters.Count);
                Assert.IsNotNull(reader.BaseStream);                
                Assert.IsNotNull(reader.Path);
                Assert.IsFalse(reader.EndOfStream);

                reader.Close();

                Assert.Throws<ObjectDisposedException>(() => reader.Read());                
                Assert.Throws<ObjectDisposedException>(() => { bool value = reader.EndOfStream; });
            }
        }

        /// <summary>
        /// Test method for handling invalid json data.
        /// </summary>
        [Test]
        public void GeoJsonReaderHandleInvalidJsonTest()
        {
            GeoJsonReader reader = new GeoJsonReader(_invalidInputJsonFilePath);

            Assert.Throws<InvalidDataException>(() => reader.Read());

            reader.Close();
        }

        /// <summary>
        /// Test method for handling unsupported CRS data.
        /// </summary>
        [Test]
        public void GeoJsonReaderHandleUnsupportedCrsTest()
        {
            GeoJsonReader reader = new GeoJsonReader(_invalidInputJsonFilePath);

            Assert.Throws<InvalidDataException>(() => reader.Read());

            reader.Close();
        }

        /// <summary>
        /// Test method for reading all geometries from file.
        /// </summary>
        [Test]
        public void GeoJsonReaderReadToEndTest()
        {
            foreach (string path in _inputFilePaths)
            {
                GeoJsonReader reader = new GeoJsonReader(path);

                Assert.AreEqual(0, reader.Parameters.Count);
                Assert.IsNotNull(reader.BaseStream);
                Assert.IsNotNull(reader.Path);
                Assert.IsFalse(reader.EndOfStream);

                IList<IGeometry> result = reader.ReadToEnd();

                Assert.IsTrue(reader.EndOfStream);
                Assert.AreEqual(0, reader.ReadToEnd().Count);

                reader.Close();
            }            
        }

        /// <summary>
        /// Test method for reading and checking content of file.
        /// </summary>
        [Test]
        public void GeoJsonReaderReadContentTest()
        {
            //test 1st input file
            GeoJsonReader reader = new GeoJsonReader(_inputFilePaths[0]);

            IGeometry result = reader.Read();
            Assert.IsInstanceOf(typeof(IGeometryCollection<IGeometry>), result);

            IGeometryCollection<IGeometry> collection = result as IGeometryCollection<IGeometry>;

            Assert.IsTrue(collection[0] is IPoint);
            Assert.IsTrue((collection[0] as IPoint).Coordinate == new Coordinate(1, 1, 9));

            Assert.IsTrue(collection[1] is IMultiLineString);
            Assert.AreEqual((collection[1] as IMultiLineString).Count, 2);
            ILineString lstr = (collection[1] as IMultiLineString)[1];
            Assert.AreEqual(5, lstr.Coordinates.Count);
            Assert.AreEqual(lstr.Coordinates[0], new Coordinate(401, 28));
            Assert.AreEqual(lstr.Coordinates[1], new Coordinate(36, 12));
            Assert.AreEqual(lstr.Coordinates[2], new Coordinate(100, 37));
            Assert.AreEqual(lstr.Coordinates[3], new Coordinate(49, 46));
            Assert.AreEqual(lstr.Coordinates[4], new Coordinate(28, 34));

            Assert.IsTrue(collection[2] is IMultiPolygon);
            Assert.AreEqual(2, (collection[2] as IMultiPolygon).Count);

            Assert.IsTrue(reader.EndOfStream);
            Assert.AreEqual(0, reader.ReadToEnd().Count);

            reader.Close();
        }
    }
}
