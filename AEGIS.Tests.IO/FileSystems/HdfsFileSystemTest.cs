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
            fileSystem = new HdfsFileSystem("192.168.56.101", "14000", "hduser");
        }


        [TestCase]
        public void HdfsFileSystemMethodsTest()
        {
            try
            {

                String[] entries = fileSystem.GetFileSystemEntries("/", "*", false);
                foreach (String entryPath in entries)
                    fileSystem.Delete(entryPath);

                fileSystem.CreateDirectory("/test");
                String str = fileSystem.GetParent("/test");
                

                fileSystem.UploadFile(@"D:\Data\testFile.txt","/testFile");
                Assert.IsTrue(fileSystem.Exists("/test"));
                Assert.IsTrue(fileSystem.IsDirectory("/test"));
                Assert.IsFalse(fileSystem.IsFile("/test"));
                Assert.IsTrue(fileSystem.IsFile("/testFile"));
                Assert.IsFalse(fileSystem.IsDirectory("/testFile"));

                fileSystem.Copy("/testFile", "/test/testFile");

                fileSystem.CreateDirectory("/test/testFolder");
                
                fileSystem.Move("/test/testFile", "/test/testFolder/testFile");
                
 



            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
