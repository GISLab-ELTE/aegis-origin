/// <copyright file="ShapefileWriterTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.IO.Shapefile
{
    [TestFixture]
    public class ShapefileWriterTest
    {
        private String[] _inputFilePaths;
        private String _outputPath;

        [SetUp]
        public void SetUp()
        {
            _inputFilePaths = new String[0];
            _outputPath = String.Empty;
        }

        [TestCase]
        public void TiffWriterWriteTest()
        {
            IList<IGeometry> geometries = null;

            foreach (String inFileName in _inputFilePaths)
            {
                ShapefileReader shpReader = new ShapefileReader(inFileName);
                geometries = shpReader.ReadToEnd();
                shpReader.Close();

                String outFileName = _outputPath + Path.GetFileName(inFileName);

                ShapefileWriter shpWriter = new ShapefileWriter(outFileName);
                shpWriter.Write(geometries);
                shpWriter.Close();
            }

        }
    }
}
