/// <copyright file="HadoopFileSystemTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.IO.Storage;
using ELTE.AEGIS.IO.Storage.Authentication;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;

namespace ELTE.AEGIS.Tests.IO.Storage
{
    /// <summary>
    /// Test fixture for class <see cref="HadoopFileSystem"/>.
    /// </summary>
    [TestFixture]
    [Ignore("There is no longer a Hadoop server available on gis.inf.elte.hu.")]
    public class HadoopFileSystemTest
    {
        #region Private fields

        /// <summary>
        /// The hostname.
        /// </summary>
        private String _hostname;
        
        /// <summary>
        /// The port number.
        /// </summary>
        private Int32 _portNumber;

        /// <summary>
        /// The authentication.
        /// </summary>
        private IHadoopFileSystemAuthentication _authentication;

        /// <summary>
        /// The path of the working directory.
        /// </summary>
        private String _directoryPath;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _hostname = "gis.inf.elte.hu";
            _portNumber = 14000;
            _authentication = new HadoopUsernameAuthentication("hduser");
            _directoryPath = "/UnitTest" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Test tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            // remove the test directory
            if (fileSystem.IsConnected && fileSystem.Exists(_directoryPath))
                fileSystem.Delete(_directoryPath);
        }

        #endregion

        #region Test cases

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemConstructorTest()
        {
            HadoopFileSystem fileSystem;
            Uri location = new Uri(String.Format("http://{0}:{1}", _hostname, _portNumber));
           
            // hostname and port parameters

            fileSystem = new HadoopFileSystem(_hostname, _portNumber);

            Assert.IsFalse(fileSystem.IsConnected);
            Assert.AreEqual('/', fileSystem.DirectorySeparator);
            Assert.AreEqual('/', fileSystem.VolumeSeparator);
            Assert.AreEqual(';', fileSystem.PathSeparator);
            Assert.AreEqual('/', fileSystem.VolumeSeparator);
            Assert.AreEqual("hdfs", fileSystem.UriScheme);
            Assert.IsTrue(fileSystem.IsContentBrowsingSupported);
            Assert.IsTrue(fileSystem.IsContentWritingSupported);
            Assert.IsTrue(fileSystem.IsStreamingSupported);
            Assert.AreEqual(location, fileSystem.Location);
            Assert.AreEqual(FileSystemAuthenticationType.Anonymous, fileSystem.Authentication.AutenticationType);


            // host, port and authentication parameters

            fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            Assert.AreEqual(_authentication, fileSystem.Authentication);
            
            // exceptions
            
            Assert.Throws<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(null, 1));
            Assert.Throws<ArgumentException>(() => fileSystem = new HadoopFileSystem(String.Empty, 1));
            Assert.Throws<ArgumentException>(() => fileSystem = new HadoopFileSystem(":", 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => fileSystem = new HadoopFileSystem(_hostname, 0));

            Assert.Throws<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(null));
            Assert.Throws<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(null, _authentication));
            Assert.Throws<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(location, (IHadoopFileSystemAuthentication)null));
            Assert.Throws<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(location, _authentication, (HttpClient)null));
            Assert.Throws<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(location, (HttpClient)null));            
        }

        /// <summary>
        /// Test case for the <see cref="CreateDirectory"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemCreateDirectoryTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();


            // new directory

            Assert.IsFalse(fileSystem.Exists(_directoryPath));

            fileSystem.CreateDirectory(_directoryPath);

            Assert.IsTrue(fileSystem.Exists(_directoryPath));
            Assert.IsTrue(fileSystem.IsDirectory(_directoryPath));

            fileSystem.CreateDirectory(_directoryPath + "/InternalDirectory");

            Assert.IsTrue(fileSystem.Exists(_directoryPath + "/InternalDirectory"));
            Assert.IsTrue(fileSystem.IsDirectory(_directoryPath + "/InternalDirectory"));


            // existing directory

            fileSystem.CreateDirectory("/");
            fileSystem.CreateDirectory(_directoryPath);

            Assert.IsTrue(fileSystem.Exists(_directoryPath));

            fileSystem.Delete(_directoryPath);


            // multiple new directories

            fileSystem.CreateDirectory(_directoryPath + "/InternalDirectory");

            Assert.IsTrue(fileSystem.Exists(_directoryPath));
            Assert.IsTrue(fileSystem.Exists(_directoryPath + "/InternalDirectory"));

            fileSystem.Delete(_directoryPath);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => fileSystem.CreateDirectory(null));
            Assert.Throws<ArgumentException>(() => fileSystem.CreateDirectory(String.Empty));
            Assert.Throws<ArgumentException>(() => fileSystem.CreateDirectory("InvalidPath"));
        }

        /// <summary>
        /// Test case for the <see cref="OpenFile"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemOpenFileTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();
            if (!fileSystem.Exists(_directoryPath + "TestFile.txt"))
                Assert.Inconclusive();


            // read existing file

            Stream fileStream = fileSystem.OpenFile(_directoryPath + "TestFile.txt", FileMode.Open, FileAccess.Read);

            Assert.IsNotNull(fileSystem);

            StreamReader reader = new StreamReader(fileStream);

            Assert.IsTrue(reader.ReadToEnd().Length > 0);
        }

        /// <summary>
        /// Test case for the <see cref="Delete"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemDeleteTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();


            // existing path

            fileSystem.CreateDirectory(_directoryPath);

            Assert.IsTrue(fileSystem.Exists(_directoryPath));
            Assert.IsTrue(fileSystem.IsDirectory(_directoryPath));

            fileSystem.Delete(_directoryPath);

            Assert.IsFalse(fileSystem.Exists(_directoryPath));
            Assert.IsFalse(fileSystem.IsDirectory(_directoryPath));


            // exceptions

            Assert.Throws<ArgumentNullException>(() => fileSystem.Delete(null));
            Assert.Throws<ArgumentException>(() => fileSystem.Delete(String.Empty));
            Assert.Throws<ArgumentException>(() => fileSystem.Delete("InvalidPath"));
            Assert.Throws<ArgumentException>(() => fileSystem.Delete("/NotExistingPath"));
        }

        /// <summary>
        /// Test case for the <see cref="GetRootDirectories"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemGetDirectoryRootTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);


            // valid parameters 

            Assert.AreEqual("/", fileSystem.GetDirectoryRoot("/"));
            Assert.AreEqual("/", fileSystem.GetDirectoryRoot(_directoryPath));


            // exceptions

            Assert.Throws<ArgumentNullException>(() => fileSystem.GetDirectoryRoot(null));
            Assert.Throws<ArgumentException>(() => fileSystem.GetDirectoryRoot(String.Empty));
        }

        /// <summary>
        /// Test case for the <see cref="GetParent"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemGetParentTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();


            // valid parameters

            fileSystem.CreateDirectory(_directoryPath);
            fileSystem.CreateDirectory(_directoryPath + "/InternalDirectory");

            Assert.AreEqual(null, fileSystem.GetParent("/"));
            Assert.AreEqual("/", fileSystem.GetParent(_directoryPath));
            Assert.AreEqual("/", fileSystem.GetParent(_directoryPath + "/"));
            Assert.AreEqual(_directoryPath, fileSystem.GetParent(_directoryPath + "/InternalDirectory"));
            Assert.AreEqual(_directoryPath, fileSystem.GetParent(_directoryPath + "/InternalDirectory/"));

            fileSystem.Delete(_directoryPath);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => fileSystem.GetParent(null));
            Assert.Throws<ArgumentException>(() => fileSystem.GetParent(String.Empty));
            Assert.Throws<ArgumentException>(() => fileSystem.GetParent(":"));
            Assert.Throws<ArgumentException>(() => fileSystem.GetParent("/NotExistingPath/"));            
        }

        /// <summary>
        /// Test case for the <see cref="GetDirectory"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemGetDirectoryTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();


            // valid parameters

            fileSystem.CreateDirectory(_directoryPath);
            fileSystem.CreateDirectory(_directoryPath + "/InternalDirectory");

            Assert.AreEqual("/", fileSystem.GetDirectory("/"));
            Assert.AreEqual(_directoryPath, fileSystem.GetDirectory(_directoryPath));
            Assert.AreEqual(_directoryPath, fileSystem.GetDirectory(_directoryPath + "/"));
            Assert.AreEqual(_directoryPath + "/InternalDirectory", fileSystem.GetDirectory(_directoryPath + "/InternalDirectory"));

            fileSystem.Delete(_directoryPath);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => fileSystem.GetDirectory(null));
            Assert.Throws<ArgumentException>(() => fileSystem.GetDirectory(String.Empty));
            Assert.Throws<ArgumentException>(() => fileSystem.GetDirectory(":"));
            Assert.Throws<ArgumentException>(() => fileSystem.GetDirectory("/NotExistingPath/"));
        }

        /// <summary>
        /// Test case for the <see cref="GetRootDirectories"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemGetRootDirectoriesTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            Assert.AreEqual(1, fileSystem.GetRootDirectories().Length);
            Assert.AreEqual("/", fileSystem.GetRootDirectories()[0]);
        }

        /// <summary>
        /// Test case for the <see cref="GetDirectories"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemGetDirectoriesTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();


            // empty directory

            fileSystem.CreateDirectory(_directoryPath);

            Assert.AreEqual(0, fileSystem.GetDirectories(_directoryPath).Length);


            // only directories

            fileSystem.CreateDirectory(_directoryPath + "/InnerDirectory1");
            fileSystem.CreateDirectory(_directoryPath + "/InnerDirectory2");

            String[] directories = fileSystem.GetDirectories(_directoryPath);

            Assert.AreEqual(2, directories.Length);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1", directories[0]);
            Assert.AreEqual(_directoryPath + "/InnerDirectory2", directories[1]);


            // with search pattern

            directories = fileSystem.GetDirectories(_directoryPath, "*1", false);

            Assert.AreEqual(1, directories.Length);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1", directories[0]);


            // recursive

            fileSystem.CreateDirectory(_directoryPath + "/InnerDirectory1/DeepInnerDirectory1");
            fileSystem.CreateDirectory(_directoryPath + "/InnerDirectory1/DeepInnerDirectory2");

            directories = fileSystem.GetDirectories(_directoryPath, "*", true);

            
            Assert.AreEqual(4, directories.Length);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1", directories[0]);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1/DeepInnerDirectory1", directories[1]);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1/DeepInnerDirectory2", directories[2]);
            Assert.AreEqual(_directoryPath + "/InnerDirectory2", directories[3]);


            fileSystem.Delete(_directoryPath);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => fileSystem.GetDirectories(null));
            Assert.Throws<ArgumentException>(() => fileSystem.GetDirectories(String.Empty));
            Assert.Throws<ArgumentException>(() => fileSystem.GetDirectories(":"));
            Assert.Throws<ArgumentException>(() => fileSystem.GetDirectories("/NotExistingPath/"));
        }

        /// <summary>
        /// Test case for the <see cref="GetFileSystemEntries"/> method.
        /// </summary>
        [TestCase]
        public void HadoopFileSystemGetFileSystemEntriesTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(_hostname, _portNumber, _authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();


            // empty directory

            fileSystem.CreateDirectory(_directoryPath);

            Assert.AreEqual(0, fileSystem.GetFileSystemEntries(_directoryPath).Length);


            // only directories

            fileSystem.CreateDirectory(_directoryPath + "/InnerDirectory1");
            fileSystem.CreateDirectory(_directoryPath + "/InnerDirectory2");

            FileSystemEntry[] directories = fileSystem.GetFileSystemEntries(_directoryPath);

            Assert.AreEqual(2, directories.Length);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1", directories[0].Path);
            Assert.AreEqual(_directoryPath + "/InnerDirectory2", directories[1].Path);


            // with search pattern

            directories = fileSystem.GetFileSystemEntries(_directoryPath, "*1", false);

            Assert.AreEqual(1, directories.Length);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1", directories[0].Path);


            // recursive

            fileSystem.CreateDirectory(_directoryPath + "/InnerDirectory1/DeepInnerDirectory1");
            fileSystem.CreateDirectory(_directoryPath + "/InnerDirectory1/DeepInnerDirectory2");

            directories = fileSystem.GetFileSystemEntries(_directoryPath, "*", true);

            Assert.AreEqual(4, directories.Length);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1", directories[0].Path);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1/DeepInnerDirectory1", directories[1].Path);
            Assert.AreEqual(_directoryPath + "/InnerDirectory1/DeepInnerDirectory2", directories[2].Path);
            Assert.AreEqual(_directoryPath + "/InnerDirectory2", directories[3].Path);


            fileSystem.Delete(_directoryPath);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => fileSystem.GetFileSystemEntries(null));
            Assert.Throws<ArgumentException>(() => fileSystem.GetFileSystemEntries(String.Empty));
            Assert.Throws<ArgumentException>(() => fileSystem.GetFileSystemEntries(":"));
            Assert.Throws<ArgumentException>(() => fileSystem.GetFileSystemEntries("/NotExistingPath/"));

        }
        
        #endregion
    }
}
