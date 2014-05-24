
using ELTE.AEGIS.IO;
using ELTE.AEGIS.IO.RawImage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.Tests.IO.RawImage
{
    public class RawImageReaderTest
    {
        private String _inputFilePath;

        [SetUp]
        public void SetUp()
        {
            //_inputFilePath = "C:/Users/Greta/Downloads/RawData";
        }

        [TestCase]
        public void ReadTest()
        {
            //IDictionary<GeometryStreamParameter, Object> parameters = new IDictionary<GeometryStreamParameter, Object>();
            //RawImageReader rawImageReader = new RawImageReader(_inputFilePath, parameters);
            //Assert.IsTrue(rawImageReader.ReadValue(0, 0, 0)==5);
        }
    }
}

