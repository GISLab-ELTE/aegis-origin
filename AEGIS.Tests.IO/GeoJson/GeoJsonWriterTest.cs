// <copyright file="GeoJsonWriterTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.IO.GeoJSON;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ELTE.AEGIS.Tests.IO.GeoJson
{
    /// <summary>
    /// Test fixture for the <see cref="GeoJsonWriter" /> class.
    /// </summary>
    /// <author>Norbert Vass, Máté Cserép</author>
    [TestFixture]
    public class GeoJsonWriterTest
    {
        private string[] _inputFilePaths;
        private string _outputPath;

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
            _outputPath = Path.Combine(prefix, "result");
        }

        /// <summary>
        /// Test method for writing to file in GeoJson format.
        /// </summary>
        [Test]
        public void GeoJsonWriterWriteTest()
        {
            IList<IGeometry> geometries = null;

            byte i = 1;
            foreach (string path in _inputFilePaths)
            {
                GeoJsonReader reader = new GeoJsonReader(path);
                geometries = reader.ReadToEnd();
                reader.Close();

                string outFileName = _outputPath + i + ".geojson";

                GeoJsonWriter writer = new GeoJsonWriter(outFileName);
                writer.Write(geometries);
                writer.Close();

                i++;
            }
        }

        /// <summary>
        /// Test method for checking written geometries.
        /// </summary>
        [Test]
        public void GeoJsonWriterWrittenContentTest()
        {
            GeometryFactory factory = new GeometryFactory();
            IMultiPolygon mp = factory.CreateMultiPolygon
                (
                    new List<IPolygon>
                    {
                        factory.CreatePolygon(factory.CreatePoint(10,10),
                                                factory.CreatePoint(20,10),
                                                factory.CreatePoint(25,17),
                                                factory.CreatePoint(10,10)),

                        factory.CreatePolygon(factory.CreatePoint(50,30),
                                                factory.CreatePoint(40,20),
                                                factory.CreatePoint(20,10),
                                                factory.CreatePoint(25,17),
                                                factory.CreatePoint(30,30),
                                                factory.CreatePoint(50,30))
                    }
                );
            Assert.AreEqual(2, mp.Count);

            IMultiPoint p = factory.CreateMultiPoint(
                new IPoint[2]
                {
                    factory.CreatePoint(10,10),
                    factory.CreatePoint(23,23)
                });

            ILineString lstr = factory.CreateLineString(
                    factory.CreatePoint(50,60),
                    factory.CreatePoint(55,60),
                    factory.CreatePoint(71,71)
                );

            List<IGeometry> geo = new List<IGeometry>() { p, lstr };

            string outFileName = _outputPath + ".geojson";

            GeoJsonWriter writer = new GeoJsonWriter(outFileName);
            writer.Write(mp as IGeometry);
            writer.Write(geo);
            writer.Close();

            GeoJsonReader reader = new GeoJsonReader(outFileName);
            IList<IGeometry> geometries = reader.ReadToEnd();
            reader.Close();

            GeometryComparer comp = new GeometryComparer();
            Assert.AreEqual(0, comp.Compare(geometries[0], mp));
            Assert.AreEqual(0, comp.Compare(geometries[1], p));
            Assert.AreEqual(0, comp.Compare(geometries[2], lstr));
        }
    }
}
