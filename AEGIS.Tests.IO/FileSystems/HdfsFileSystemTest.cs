using ELTE.AEGIS.IO.FileSystems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.Tests.IO.FileSystems
{
    [TestFixture]
    class HdfsFileSystemTest
    {
        HdfsFileSystem fileSystem;

        [SetUp]
        public void SetUp()
        {
            fileSystem = new HdfsFileSystem("192.168.56.1001", "hduser");
        }


        [TestCase]
        public void HdfsFileSystemMethodsTest()
        {
            try
            {
                //fileSystem.CreateDirectory("/hduser2");
                //fileSystem.Delete("/hduser2");

                fileSystem.IsDirectory("/user/hduser/test.txt");
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
