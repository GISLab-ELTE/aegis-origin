using ELTE.AEGIS.IO.FileSystems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            fileSystem = new HdfsFileSystem("gis.inf.elte.hu", "14000", "hduser");
        }


        [TestCase]
        public void HdfsFileSystemMethodsTest()
        {
            try
            {
                fileSystem.Exists("/");
            }
            catch(ArgumentException ex)
            {
                if (((WebException)ex.InnerException).Status == WebExceptionStatus.ConnectFailure)
                    return;
            }

            try
            {

                if (fileSystem.Exists("/testing"))
                    fileSystem.Delete("/testing");

                String[] entries = fileSystem.GetFileSystemEntries("/", "*", false);

                fileSystem.CreateDirectory("/testing");
                String str = fileSystem.GetParent("/testing");

                fileSystem.UploadFile(@"D:\Data\testFile.txt","/testing/testFile");
                Assert.IsTrue(fileSystem.Exists("/testing"));
                Assert.IsTrue(fileSystem.IsDirectory("/testing"));
                Assert.IsFalse(fileSystem.IsFile("/testing"));
                Assert.IsTrue(fileSystem.IsFile("/testing/testFile"));
                Assert.IsFalse(fileSystem.IsDirectory("/testing/testFile"));

                fileSystem.Copy("/testing/testFile", "/testing/testFile2");

                fileSystem.CreateDirectory("/testing/testFolder");

                fileSystem.Move("/testing/testFile", "/testing/testFolder/testFile");

            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
