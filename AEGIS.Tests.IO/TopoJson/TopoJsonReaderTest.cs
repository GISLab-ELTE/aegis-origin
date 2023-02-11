// <copyright file="TopoJsonReaderTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.IO.TopoJSON;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ELTE.AEGIS.Tests.IO.TopoJson
{
    /// <summary>
    /// Test fixture for the <see cref="TopoJsonReader" /> class.
    /// </summary>
    /// <author>Norbert Vass, Máté Cserép</author>
    [TestFixture]
    public class TopoJsonReaderTest
    {
        private string[] _inputFilePaths;
        private string _invalidInputJsonFilePath;
        private string _invalidInputTopoJsonFilePath;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            string prefix = "file://" + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "_data");
            _inputFilePaths = new string[3]
            {
                Path.Combine(prefix, "tj0.topojson"),
                Path.Combine(prefix, "tj1.topojson"),
                Path.Combine(prefix, "tj2.topojson")
            };
            _invalidInputJsonFilePath = Path.Combine(prefix, "a.json");
            _invalidInputTopoJsonFilePath = Path.Combine(prefix, "b.json");
        }

        /// <summary>
        /// Test method for opening and closing TopoJSON files.
        /// </summary>
        [Test]
        public void TopoJsonReaderOpenCloseTest()
        {
            foreach (string path in _inputFilePaths)
            {
                TopoJsonReader reader = new TopoJsonReader(path);

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
        public void TopoJsonReaderHandleInvalidJsonTest()
        {
            TopoJsonReader reader = new TopoJsonReader(_invalidInputJsonFilePath);

            Assert.Throws<InvalidDataException>(() => reader.Read());

            reader.Close();

            reader = new TopoJsonReader(_invalidInputTopoJsonFilePath);

            Assert.Throws<InvalidDataException>(() => reader.Read());

            reader.Close();
        }

        /// <summary>
        /// Test method for reading all geometries from TopoJson file.
        /// </summary>
        [Test]
        public void TopoJsonReaderReadToEndTest()
        {
            foreach (string path in _inputFilePaths)
            {
                TopoJsonReader reader = new TopoJsonReader(path);

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
        public void TopoJsonReaderReadContentTest()
        {
            GeometryFactory factory = new GeometryFactory();

            //test 1st input file
            TopoJsonReader reader = new TopoJsonReader(_inputFilePaths[0]);

            IGeometry result = reader.Read();
            Assert.IsInstanceOf(typeof(IGeometryCollection<IGeometry>), result);

            IGeometryCollection<IGeometry> collection = result as IGeometryCollection<IGeometry>;

            Assert.AreEqual(2, collection.Count);
            Assert.IsInstanceOf(typeof(IPolygon), collection[0]);
            Assert.IsInstanceOf(typeof(IPolygon), collection[1]);

            IPolygon left_polygon = factory.CreatePolygon(new Coordinate[]
            {
                new Coordinate(1,2),
                new Coordinate(1,0),
                new Coordinate(0,0),
                new Coordinate(0,2),
                new Coordinate(1,2)
            });

            IPolygon right_polygon = factory.CreatePolygon(new Coordinate[]
            {
                new Coordinate(1,2),
                new Coordinate(2,2),
                new Coordinate(2,0),
                new Coordinate(1,0),
                new Coordinate(1,2)
            });

            GeometryComparer comp = new GeometryComparer();

            //System.Diagnostics.Debug.WriteLine((collection[0] as IPolygon).Shell.ToString());

            Assert.AreEqual(0, comp.Compare(collection[0], left_polygon));
            Assert.AreEqual(0, comp.Compare(collection[1], right_polygon));

            reader.Close();
        }
    }
}
