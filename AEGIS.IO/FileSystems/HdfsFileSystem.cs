using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

            try
            {

                String urlPath = _urlRoot + path + "?op=MKDIRS&user.name=" + HdfsUsername;

                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.Method = "PUT";
                request.ContentType = "application/json; charset=utf-8";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) { }

                //WebClient webClient = new WebClient();
                //webClient.UploadString(urlPath, "PUT", "");
            }
            catch (WebException ex)
            {
                throw new WebException("Failed to create folder.",ex);
            }

        }

        /// <summary>
        /// Deletes the filesystem entry located at the specified path.
        /// </summary>
        /// <param name="path">The path of the entry to delete.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is null;path
        /// or
        /// The path is empty;path
        /// </exception>
        /// <exception cref="System.Net.WebException">Failed to delete path</exception>
        public override void Delete(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");

            //TODO: Mode recursive or not?
            String urlPath = _urlRoot + path + "?op=DELETE&recursive=true&user.name=" + HdfsUsername;

            try
            {
                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.Method = "DELETE";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) { }
            }
            catch(WebException ex)
            {
                throw new WebException("Failed to delete path",ex);
            }
        }

        /// <summary>
        /// Determines whether the specified path exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        ///   <c>true</c> if the path exists, otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The path is null;path
        /// or
        /// The path is empty;path
        /// </exception>
        public override Boolean Exists(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");

            String urlPath = _urlRoot + path + "?op=GETFILESTATUS&user.name=" + HdfsUsername;

            try
            {
                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                
                request.ContentType = "application/json; charset=utf-8";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    return (response.StatusCode == HttpStatusCode.OK);
                }

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

        /// <summary>
        /// Determines whether the specified path is an existing directory.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        ///   <c>true</c> if the path exists, and is a directory, otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The path is null;path
        /// or
        /// The path is empty;path
        /// </exception>
        public override Boolean IsDirectory(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");

            String urlPath = _urlRoot + path + "?op=GETFILESTATUS&user.name=" + HdfsUsername;

            try
            {
                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.ContentType = "application/json; charset=utf-8";


                using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    String responseString = ((new StreamReader(response.GetResponseStream())).ReadToEnd());

                    return responseString.Contains("\"type\":\"DIRECTORY\"");
                }
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

        /// <summary>
        /// Determines whether the specified path is an existing file.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        ///   <c>true</c> if the path exists, and is a file, otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The path is null;path
        /// or
        /// The path is empty;path
        /// </exception>
        public override Boolean IsFile(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");

            String urlPath = _urlRoot + path + "?op=GETFILESTATUS&user.name=" + HdfsUsername;

            try
            {
                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.ContentType = "application/json; charset=utf-8";

                using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    String responseString = ((new StreamReader(response.GetResponseStream())).ReadToEnd());

                    return responseString.Contains("\"type\":\"FILE\"");
                }

            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw;

                HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                if (statusCode == HttpStatusCode.NotFound)
                    return false;

                throw;
            }
        }

        /// <summary>
        /// Returns the full directory name for a specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>
        /// The full directory name for <paramref name="path" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// The path contains only white space, or contains one or more invalid characters.;path
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.;path
        /// </exception>
        public override String GetDirectory(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                return Path.GetDirectoryName(path);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
            }

        }

        /// <summary>
        /// Returns the file name and file extension for a specified path.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>
        /// The file name and file extension for <paramref name="path" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// The path contains only white space, or contains one or more invalid characters.;path
        /// </exception>
        public override String GetFileName(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                return Path.GetFileName(path);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }

        }

        /// <summary>
        /// Returns the file name without file extension for a specified path.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>
        /// The file name without file extension for <paramref name="path" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// The path contains only white space, or contains one or more invalid characters.;path
        /// </exception>
        public override String GetFileNameWithoutExtension(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
        }


        /// <summary>
        /// Returns the directories located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>
        /// An array containing the full paths to all directories.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// The specified path is a file.;path
        /// or
        /// The specified path is invalid.;path
        /// </exception>
        public override String[] GetDirectories(String path, String searchPattern, Boolean recursive)
        {
            return GetFileSystemEntries(path, searchPattern, recursive).
                Where(x => x.Split('.').Count() == 1).
                ToArray();
        }

        /// <summary>
        /// Returns the files located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>
        /// An array containing the full paths to all files.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// The specified path is a file.;path
        /// or
        /// The specified path is invalid.;path
        /// </exception>
        public override String[] GetFiles(String path, String searchPattern, Boolean recursive)
        {
            return GetFileSystemEntries(path, searchPattern, recursive).
                Where(x => x.Split('.').Count() > 1).
                ToArray();
        }

        /// <summary>
        /// Returns the file system entries located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>
        /// An array containing the full paths to all file system entries.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// The specified path is a file.;path
        /// or
        /// The specified path is invalid.;path
        /// </exception>
        public override String[] GetFileSystemEntries(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");
            if (path.Split('.').Count() != 1)
                throw new ArgumentException("The specified path is a file.", "path");

            List<String> entries = new List<String>();

            Queue<String> paths = new Queue<String>();
            paths.Enqueue(path);

            try
            {
                while (paths.Count > 0)
                {
                    String currentPath = paths.Dequeue();
                    String urlPath = _urlRoot + currentPath + "?op=LISTSTATUS&user.name=" + HdfsUsername;

                    HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                    request.ContentType = "application/json; charset=utf-8";

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        StringBuilder responseString = new StringBuilder((new StreamReader(response.GetResponseStream()).ReadToEnd()));

                        if (String.IsNullOrEmpty(responseString.ToString())) //Empty folder
                            continue;                        

                        responseString.Remove(0, 33);
                        responseString.Remove(responseString.Length - 4, 3);
                        String entryDescriptors = responseString.ToString().Trim();

                        String[] splittedDescriptors = entryDescriptors.Split('}').Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();

                        foreach(String descriptor in splittedDescriptors)
                        {
                            Int32 fileNameStartIndex = descriptor.IndexOf("\"pathSuffix\":\"");
                            if (fileNameStartIndex == -1)
                                continue;
                            fileNameStartIndex += 14;

                            String entryName = new String(descriptor.Substring(fileNameStartIndex).TakeWhile(x => x != '"').ToArray());
                            if (String.IsNullOrEmpty(entryName))
                                continue;

                            String entryFullName = currentPath + (currentPath[currentPath.Length - 1] == '/' ? "" : "/") + entryName;
                            
                            if(Regex.IsMatch(entryName, searchPattern)) //TODO: Search pattern is not regex
                                entries.Add(entryFullName);

                            if(recursive && (entryName.Split('.').Count() == 1) ) //If recursive and not file.
                                paths.Enqueue(entryFullName);
                        }

                    }

                }
                return entries.ToArray();
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw;

                HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                if (statusCode == HttpStatusCode.NotFound)
                    throw new ArgumentException("The specified path is invalid.", "path");

                throw;
            }
        }

        /// <summary>
        /// Returns the root information for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>
        /// A string containing the root information.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">The path is empty.;path</exception>
        public override String GetDirectoryRoot(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            return "/";
        }
        
        
        /// <summary>
        /// Returns the names of the logical drives of the file system (Not Supported).
        /// </summary>
        /// <exception cref="System.NotSupportedException">The operation is not supported in this context</exception>
        public override String[] GetLogicalDrives()
        {
            throw new System.NotSupportedException();
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




        public override String GetParent(String path)
        {
            throw new System.NotImplementedException();
        }





        #endregion

    }
}
