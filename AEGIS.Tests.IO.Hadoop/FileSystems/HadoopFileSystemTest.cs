using ELTE.AEGIS.IO.FileSystems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.Tests.IO.FileSystems
{
    [TestFixture]
    class HadoopFileSystemTest
    {
        HadoopFileSystem fileSystem;

        [SetUp]
        public void SetUp()
        {
            fileSystem = new HadoopFileSystem("gis.inf.elte.hu", "14000", "hduser");
        }


        [TestCase]
        public void HadoopFileSystemMethodsTest()
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

                if (File.Exists(@"D:\Data\testFile.txt"))
                {
                    fileSystem.UploadFile(@"D:\Data\testFile.txt", "/testing/testFile");
                    Assert.IsTrue(fileSystem.Exists("/testing"));
                    Assert.IsTrue(fileSystem.IsDirectory("/testing"));
                    Assert.IsFalse(fileSystem.IsFile("/testing"));
                    Assert.IsTrue(fileSystem.IsFile("/testing/testFile"));
                    Assert.IsFalse(fileSystem.IsDirectory("/testing/testFile"));

                    fileSystem.Copy("/testing/testFile", "/testing/testFile2");

                    fileSystem.CreateDirectory("/testing/testFolder");

                    fileSystem.Move("/testing/testFile", "/testing/testFolder/testFile");
                }
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
