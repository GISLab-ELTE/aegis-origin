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
            fileSystem = new HdfsFileSystem("192.168.56.101", "hduser");
        }


        [TestCase]
        public void HdfsFileSystemMethodsTest()
        {
            try
            {
                fileSystem.CreateDirectory("/hduser5");
                Assert.IsTrue(fileSystem.Exists("/hduser5"));
                Assert.IsTrue(fileSystem.IsDirectory("/hduser5"));
                Assert.IsFalse(fileSystem.IsFile("/hduser5"));
                fileSystem.Delete("/hduser5");

                Assert.IsTrue(fileSystem.Exists("/user/hduser/test.txt"));
                Assert.IsFalse(fileSystem.IsDirectory("/user/hduser/test.txt"));
                Assert.IsTrue(fileSystem.IsFile("/user/hduser/test.txt"));
                Assert.IsFalse(fileSystem.Exists("/ALÁNYKINEKNEVEÖT"));

                Assert.IsTrue(fileSystem.Exists("/user/hduser/test.txt"));
                Assert.IsTrue(fileSystem.Exists("/user/hduser/test"));
                Assert.IsFalse(fileSystem.Exists("/user/hduserASD"));

                String[] directories = fileSystem.GetDirectories("/", ".*", true);
                String[] files = fileSystem.GetFiles("/", ".*", true);

                String[] s = fileSystem.GetFileSystemEntries("/",".*",true);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
