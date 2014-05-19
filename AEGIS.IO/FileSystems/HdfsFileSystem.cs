using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.FileSystems
{
    /// <summary>
    /// Represents a HDFS file system.
    /// </summary>
    public class HdfsFileSystem : FileSystem
    {

        #region FileSystem public properties
        /// <summary>
        /// Gets the scheme name for this file system.
        /// </summary>
        /// <value>The scheme name for this file system.</value>
        public override String UriScheme { get { return Uri.UriSchemeHttp; } }

        /// <summary>
        /// Gets the directory separator for this file system.
        /// </summary>
        /// <value>The directory separator for this file system.</value>
        public override Char DirectorySeparator { get { return Path.DirectorySeparatorChar; } }

        /// <summary>
        /// Gets the path separator for this file system.
        /// </summary>
        /// <value>The path separator for this file system.</value>
        public override Char PathSeparator { get { return Path.PathSeparator; } }

        /// <summary>
        /// Gets the volume separator for this file system.
        /// </summary>
        /// <value>The volume separator for this file system.</value>
        public override Char VolumeSeparator { get { return Path.VolumeSeparatorChar; } }
        #endregion

        private const String WebHdfsContextRoot = "/webhdfs/v1";

        /// <summary>
        /// Gets the HDFS NameNode hostname.
        /// </summary>
        public String NameNodeHost { get; private set; }

        /// <summary>
        /// Gets the port of the HDFS NameNode.
        /// </summary>
        public String NameNodePort { get; private set; }

        /// <summary>
        /// Gets the HDFS username.
        /// </summary>
        public String HdfsUsername { get; private set; }


        private String _urlRoot;

        #region Public constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HdfsFileSystem"/> class.
        /// </summary>
        /// <param name="nameNodeHost">The HDFS NameNode hostname.</param>
        /// <param name="hdfsUsername">The HDFS username.</param>
        /// <param name="nameNodePort">The HDFS NameNode port.</param>
        public HdfsFileSystem(String nameNodeHost, String hdfsUsername, String nameNodePort = "50070")
        {
            NameNodeHost = nameNodeHost;
            NameNodePort = nameNodePort;
            HdfsUsername = hdfsUsername;

            _urlRoot = "http://" + NameNodeHost + ":" + NameNodePort + WebHdfsContextRoot;

        }
        #endregion

        #region FileSystem public methods
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is null;path
        /// or
        /// The path is empty;path
        /// </exception>
        /// <exception cref="System.Net.WebException">Failed to create folder.</exception>
        public override void CreateDirectory(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");

            if(path[0] != '/') //TODO: Necessary?
                path = String.Concat('/', path);

            try
            {

                String urlPath = _urlRoot + path + "?op=MKDIRS&user.name=" + HdfsUsername;

                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.Method = "PUT";
                request.ContentType = "application/json; charset=utf-8";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                //WebClient webClient = new WebClient();
                //webClient.UploadString(urlPath, "PUT", "");
            }
            catch (WebException ex)
            {
                throw new WebException("Failed to create folder.",ex);
            }

        }

        public override void Delete(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");

            if (path[0] != '/') //TODO: Necessary?
                path = String.Concat('/', path);

            //TODO: Mode recursive or not?
            String urlPath = _urlRoot + path + "?op=DELETE&recursive=true&user.name=" + HdfsUsername;

            try
            {
                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.Method = "DELETE";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            }
            catch(WebException ex)
            {
                throw new WebException("Failed to delete path",ex);
            }
        }

        public override Boolean Exists(String path)
        {
            throw new System.NotImplementedException();
        }

        public override Boolean IsDirectory(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");

            if (path[0] != '/')
                path = String.Concat('/', path);

            String urlPath = _urlRoot + path + "?op=GETFILESTATUS&user.name=" + HdfsUsername;

            try
            {
                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.ContentType = "application/json; charset=utf-8";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;


                String responseString = ((new StreamReader(response.GetResponseStream())).ReadToEnd());

                return responseString.Contains("\"type\":\"DIRECTORY\"");

            }
            catch(WebException ex)
            {
                if (ex.Response == null)
                    throw;

                HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                if (statusCode == HttpStatusCode.NotFound)
                    return false;

                throw;
            }

        }

        public override Boolean IsFile(String path)
        {
            throw new System.NotImplementedException();
        }


        #endregion

        #region NotYetImplemented
        public override Stream CreateFile(string path, bool overwrite)
        {
            throw new System.NotImplementedException();
        }

        public override Stream OpenFile(string path, System.IO.FileMode mode, System.IO.FileAccess access)
        {
            throw new System.NotImplementedException();
        }

        public override void Move(string sourcePath, string destinationPath)
        {
 	        throw new System.NotImplementedException();
        }

        public override void Copy(string sourcePath, string destinationPath)
        {
 	        throw new System.NotImplementedException();
        }

       

        public override string GetDirectoryRoot(string path)
        {
 	        throw new System.NotImplementedException();
        }

        public override string GetParent(string path)
        {
 	        throw new System.NotImplementedException();
        }

        public override string GetDirectory(string path)
        {
 	        throw new System.NotImplementedException();
        }

        public override string GetFileName(string path)
        {
 	        throw new System.NotImplementedException();
        }

        public override string GetFileNameWithoutExtension(string path)
        {
 	        throw new System.NotImplementedException();
        }

        public override string[] GetLogicalDrives()
        {
 	        throw new System.NotImplementedException();
        }

        public override string[] GetDirectories(string path, string searchPattern, bool recursive)
        {
 	        throw new System.NotImplementedException();
        }

        public override string[] GetFiles(string path, string searchPattern, bool recursive)
        {
 	        throw new System.NotImplementedException();
        }

        public override string[] GetFileSystemEntries(string path, string searchPattern, bool recursive)
        {
 	        throw new System.NotImplementedException();
        }
        #endregion

    }
}
