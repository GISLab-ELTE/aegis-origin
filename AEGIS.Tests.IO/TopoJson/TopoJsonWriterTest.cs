/// <copyright file="TopoJsonWriterTest.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.IO.TopoJSON;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ELTE.AEGIS.Tests.IO.TopoJson
{
    [TestFixture]
    public class TopoJsonWriterTest
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
                Path.Combine(prefix, "tj0.topojson"),
                Path.Combine(prefix, "tj1.topojson"),
                Path.Combine(prefix, "tj2.topojson"),
            };
            _outputPath = Path.Combine(prefix, "result");
        }

        /// <summary>
        /// Test method for writing geometry to file in TopoJSON format.
        /// </summary>
        [Test]
        public void TopoJsonWriterWriteTest()
        {
            IList<IGeometry> geometries = null;

            foreach (string path in _inputFilePaths)
            {
                TopoJsonReader reader = new TopoJsonReader(path);
                geometries = reader.ReadToEnd();
                reader.Close();

                string outFileName = _outputPath + Path.GetFileName(path);

                TopoJsonWriter writer = new TopoJsonWriter(outFileName);
                writer.Write(geometries);
                writer.Close();
            }
        }

        /// <summary>
        /// Test method for checking written geometries.
        /// </summary>
        [Test]
        public void TopoJsonWriterWrittenContentTest()
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
                    factory.CreatePoint(50, 60),
                    factory.CreatePoint(55, 60),
                    factory.CreatePoint(71, 71)
                );

            List<IGeometry> geo = new List<IGeometry>() { p, lstr };

            string outFileName = _outputPath + ".topojson";

            TopoJsonWriter writer = new TopoJsonWriter(outFileName);
            writer.Write(mp as IGeometry);
            writer.Write(geo);
            writer.Close();

            TopoJsonReader reader = new TopoJsonReader(outFileName);
            IList<IGeometry> geometries = reader.ReadToEnd();
            reader.Close();

            GeometryComparer comp = new GeometryComparer();
            Assert.AreEqual(0, comp.Compare(geometries[0], mp));
            Assert.AreEqual(0, comp.Compare(geometries[1], p));
            Assert.AreEqual(0, comp.Compare(geometries[2], lstr));
        }
    }
}
