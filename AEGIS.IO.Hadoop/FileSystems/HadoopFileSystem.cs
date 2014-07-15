using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.IO.FileSystems
{
    /// <summary>
    /// Represents a HDFS file system.
    /// </summary>
    public class HadoopFileSystem : FileSystem
    {
        #region Private types
        private enum FileSystemEntryType { File, Directory }
        #endregion

        #region FileSystem public properties

        /// <summary>
        /// Gets the scheme name for this file system.
        /// </summary>
        /// <value>The scheme name for this file system.</value>
        public override String UriScheme { get { return "HDFS://"; } }

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
        public override Char VolumeSeparator { get { return '/'; } }

        /// <summary>
        /// Gets a value indicating whether the file system is connected.
        /// </summary>
        /// <value><c>true</c> if operations can be executed on the file system; otherwise, <c>false</c>.</value>
        public override Boolean IsConnected
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether file streaming is supported by the file system.
        /// </summary>
        /// <value><c>true</c> if file streaming commands can be executed on the file system; otherwise, <c>false</c>.</value>
        public override Boolean IsStreamingSupported { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether content browsing is supported by the file system.
        /// </summary>
        /// <value><c>true</c> if the content of the file system can be listed; otherwise, <c>false</c>.</value>
        public override Boolean IsContentBrowsingSupported { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether content writing is supported by the file system.
        /// </summary>
        /// <value><c>true</c> if file creation, modification and removal operations are supported by the file system; otherwise, <c>false</c>.</value>
        public override Boolean IsContentWritingSupported { get { return true; } }

        #endregion

        #region Public properties
        /// <summary>
        /// Gets the HDFS NameNode hostname.
        /// </summary>
        public String HostName { get; private set; }

        /// <summary>
        /// Gets the port of the HDFS NameNode.
        /// </summary>
        public String PortNumber { get; private set; }

        /// <summary>
        /// Gets the HDFS username.
        /// </summary>
        public String HdfsUsername { get; private set; }
        #endregion

        #region Private fields
        private const String WebHdfsContextRoot = "/webhdfs/v1";

        private String _urlRoot;
        #endregion

        #region Public constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystem"/> class.
        /// </summary>
        /// <param name="hostName">The HDFS hostname.</param>
        /// <param name="hdfsUsername">The HDFS username.</param>
        /// <param name="portNumber">The HDFS port.</param>
        public HadoopFileSystem(String hostName, String portNumber, String hdfsUsername)
        {
            HostName = hostName;
            PortNumber = portNumber;
            HdfsUsername = hdfsUsername;

            _urlRoot = "http://" + HostName + ":" + PortNumber + WebHdfsContextRoot;
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
        /// or
        /// Failed to create folder on the specified path.;path
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to create folder</exception>
        public override void CreateDirectory(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");


            String urlPath = _urlRoot + path + "?op=MKDIRS&user.name=" + HdfsUsername;

            try
            {
                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.Method = "PUT";
                request.ContentType = "application/json; charset=utf-8";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) 
                {
                    String responseString = ((new StreamReader(response.GetResponseStream())).ReadToEnd());

                    if (!responseString.Contains("true"))
                        throw new ArgumentException("Failed to create folder on the specified path.", "path");
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw new ArgumentException("Failed to create folder on the specified path.", ex);

                switch(((HttpWebResponse)ex.Response).StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException("The caller does not have the required permission to create folder", ex);
                    default:
                        throw new ArgumentException("Failed to create folder on the specified path.", ex);
                }
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
        /// or
        /// Failed to delete entry on the specified path.;path
        /// or
        /// Failed to delete entry on the specified path.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to delete entry</exception>
        public override void Delete(String path)
        {
            if (path == null)
                throw new ArgumentException("The path is null", "path");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty", "path");

            String urlPath = _urlRoot + path + "?op=DELETE&recursive=true&user.name=" + HdfsUsername;

            try
            {
                HttpWebRequest request = WebRequest.Create(urlPath) as HttpWebRequest;
                request.Method = "DELETE";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) 
                {
                    String responseString = ((new StreamReader(response.GetResponseStream())).ReadToEnd());

                    if (!responseString.Contains("true"))
                        throw new ArgumentException("Failed to delete entry on the specified path.", "path");
                }
            }
            catch(WebException ex)
            {
                if (ex.Response == null)
                    throw new ArgumentException("Failed to delete entry on the specified path.", ex);

                switch (((HttpWebResponse)ex.Response).StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException("The caller does not have the required permission to delete entry", ex);
                    default:
                        throw new ArgumentException("Failed to delete entry on the specified path.", ex);
                }
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
        /// or
        /// An error occured during the request.
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
                    throw new ArgumentException("An error occured during the request.", ex);

                HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                if (statusCode == HttpStatusCode.NotFound)
                    return false;

                throw new ArgumentException("An error occured during the request.", ex);
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
        /// or
        /// An error occured during the request.
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
                    throw new ArgumentException("An error occured during the request.", ex);

                HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                if (statusCode == HttpStatusCode.NotFound)
                    return false;

                throw new ArgumentException("An error occured during the request.", ex);
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
        /// or
        /// An error occured during the request.
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
                    throw new ArgumentException("An error occured during the request.", ex);

                HttpStatusCode statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                if (statusCode == HttpStatusCode.NotFound)
                    return false;

                throw new ArgumentException("An error occured during the request.", ex);
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
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.;path
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
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
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
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.;path
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
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
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
        /// Failed to get the entries on the specified path.
        /// or
        /// Failed to get the entries on the specified path.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to get entries on the specified path.</exception>
        public override String[] GetDirectories(String path, String searchPattern, Boolean recursive)
        {
            return GetEntries(path, searchPattern, recursive)
                .Where(x => x.Value == FileSystemEntryType.Directory)
                .Select(x => x.Key)
                .ToArray();
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
        /// Failed to get the entries on the specified path.
        /// or
        /// Failed to get the entries on the specified path.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to get entries on the specified path.</exception>
        public override String[] GetFiles(String path, String searchPattern, Boolean recursive)
        {
            return GetEntries(path, searchPattern, recursive)
                .Where(x => x.Value == FileSystemEntryType.File)
                .Select(x => x.Key)
                .ToArray();
        }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// Failed to get the entries on the specified path.
        /// or
        /// Failed to get the entries on the specified path.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to get entries on the specified path.</exception>
        public override String[] GetFileSystemEntries(String path, String searchPattern, Boolean recursive)
        {
            return GetEntries(path, searchPattern, recursive).Keys.ToArray();
        }

        /// <summary>
        /// Gets entries from the filesystem on the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// Failed to get the entries on the specified path.
        /// or
        /// Failed to get the entries on the specified path.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission to get entries on the specified path.</exception>
        private IDictionary<String, FileSystemEntryType> GetEntries(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            Dictionary<String, FileSystemEntryType> entries = new Dictionary<String, FileSystemEntryType>();

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

                        if (responseString.ToString().Trim() == "{\"FileStatuses\":{\"FileStatus\":[]}}") //Empty folder
                            continue;

                        responseString.Remove(0, 33);
                        responseString.Remove(responseString.Length - 4, 3);



                        String entryDescriptors = responseString.ToString().Trim();

                        String[] splittedDescriptors = entryDescriptors.Split('}').Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();

                        foreach (String descriptor in splittedDescriptors)
                        {
                            //Get entry name
                            Int32 fileNameStartIndex = descriptor.IndexOf("pathSuffix\":\"");
                            if (fileNameStartIndex == -1)
                                continue;
                            fileNameStartIndex += 13;

                            String entryName = new String(descriptor.Substring(fileNameStartIndex).TakeWhile(x => x != '"').ToArray());
                            if (String.IsNullOrEmpty(entryName))
                                continue;

                            String entryFullName = currentPath + (currentPath[currentPath.Length - 1] == '/' ? "" : "/") + entryName;

                            //Get entry type
                            FileSystemEntryType entryType = descriptor.Contains("FILE") ? FileSystemEntryType.File : FileSystemEntryType.Directory;


                            if (Regex.IsMatch(entryName, WildcardToRegex(searchPattern)))
                                entries.Add(entryFullName, entryType);

                            if (recursive && entryType == FileSystemEntryType.Directory)
                                paths.Enqueue(entryFullName);
                        }

                    }

                }
                return entries;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw new ArgumentException("Failed to get the entries on the specified path.", ex);

                switch (((HttpWebResponse)ex.Response).StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException("The caller does not have the required permission to get entries on the specified path.", ex);
                    default:
                        throw new ArgumentException("Failed to get the entries on the specified path.", ex);
                }
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
        /// Returns the path to the root directories of the file system.
        /// </summary>
        /// <returns>The array containing the path to the root directories in the file system.</returns>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override String[] GetRootDirectories()
        {
            throw new NotSupportedException("The operation is not supported in this context.");
        }

        /// <summary>
        /// Returns the path of the parent directory for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>
        /// The string containing the full path to the parent directory.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">path;The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.;path
        /// or
        /// The specified path not exists
        /// </exception>
        public override String GetParent(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            String parent = path.Substring(0,path.LastIndexOf('/') + 1);

            if (!Exists(parent))
                throw new ArgumentException("The specified path does not exist.");

            return parent;
        }

        /// <summary>
        /// When overriden in a derived class, creates or overwrites a file on the specified path.
        /// </summary>
        /// <param name="path">The path of a file to create.</param>
        /// <param name="overwrite">A value that specifies whether the file should be overwritten in case it exists.</param>
        /// <returns>
        /// A stream that provides read/write access to the file specified in path.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override Stream CreateFile(String path, Boolean overwrite)
        {
            throw new NotSupportedException("The operation is not supported in this context.");
        }

        /// <summary>
        /// When overriden in a derived class, opens a stream on the specified path.
        /// </summary>
        /// <param name="path">The path of a file to open.</param>
        /// <param name="mode">A value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A value that specifies the operations that can be performed on the file.</param>
        /// <returns>
        /// A stream in the specified mode and path, with the specified access.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override Stream OpenFile(String path, FileMode mode, FileAccess access)
        {
            throw new NotSupportedException("The operation is not supported in this context.");
        }

        /// <summary>
        /// Moves a filesystem entry and its contents to a new location.
        /// </summary>
        /// <param name="sourcePath">The path of the file or directory to move.</param>
        /// <param name="destinationPath">The path to the new location for the entry.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is null.;localPath
        /// or
        /// The path is null.;remotePath
        /// or
        /// The path is empty;localPath
        /// or
        /// The path is empty;remotePath
        /// or
        /// The source is not a file;remotePath
        /// </exception>
        public override void Move(String sourcePath, String destinationPath)
        {
            Copy(sourcePath, destinationPath);
            Delete(sourcePath);
        }

        /// <summary>
        /// Copies an existing file to a new file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is null.;localPath
        /// or
        /// The path is null.;remotePath
        /// or
        /// The path is empty;localPath
        /// or
        /// The path is empty;remotePath
        /// or
        /// The source is not a file;remotePath
        /// </exception>
        public override void Copy(String sourcePath, String destinationPath)
        {
            if (sourcePath == null)
                throw new ArgumentException("The path is null.", "localPath");
            if (destinationPath == null)
                throw new ArgumentException("The path is null.", "remotePath");
            if (String.IsNullOrWhiteSpace(sourcePath))
                throw new ArgumentException("The path is empty", "localPath");
            if (String.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException("The path is empty", "remotePath");

            if (!IsFile(sourcePath))
                throw new ArgumentException("The source is not a file", "remotePath");

            String tempFilePath = Path.GetTempFileName();

            DownloadFile(sourcePath, tempFilePath);
            UploadFile(tempFilePath, destinationPath);


        }
        #endregion

        #region Public methods
        /// <summary>
        /// Uploads the file on the local path to the remote path.
        /// </summary>
        /// <param name="localPath">The local path.</param>
        /// <param name="remotePath">The remote path.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is null.;localPath
        /// or
        /// The path is null.;remotePath
        /// or
        /// The path is empty;localPath
        /// or
        /// The path is empty;remotePath
        /// </exception>
        public void UploadFile(String localPath, String remotePath)
        {
            if (localPath == null)
                throw new ArgumentException("The path is null.", "localPath");
            if (remotePath == null)
                throw new ArgumentException("The path is null.", "remotePath");
            if (String.IsNullOrWhiteSpace(localPath))
                throw new ArgumentException("The path is empty", "localPath");
            if (String.IsNullOrWhiteSpace(remotePath))
                throw new ArgumentException("The path is empty", "remotePath");

            String url = _urlRoot + remotePath + "?op=CREATE&user.name=" + HdfsUsername;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.UploadFile(url, "PUT", localPath);
                }
            }
            catch(WebException ex)
            {
                if (ex.Response == null)
                    throw new ArgumentException("Failed to upload file.", ex);
            }
        }

        /// <summary>
        /// Downloads the file from the remote path to the local path.
        /// </summary>
        /// <param name="remotePath">The remote path.</param>
        /// <param name="localPath">The local path.</param>
        /// <exception cref="System.ArgumentException">
        /// The path is null.;localPath
        /// or
        /// The path is null.;remotePath
        /// or
        /// The path is empty;localPath
        /// or
        /// The path is empty;remotePath
        /// </exception>
        public void DownloadFile(String remotePath, String localPath)
        {
            if (localPath == null)
                throw new ArgumentException("The path is null.", "localPath");
            if (remotePath == null)
                throw new ArgumentException("The path is null.", "remotePath");
            if (String.IsNullOrWhiteSpace(localPath))
                throw new ArgumentException("The path is empty", "localPath");
            if (String.IsNullOrWhiteSpace(remotePath))
                throw new ArgumentException("The path is empty", "remotePath");

            String url = _urlRoot + remotePath + "?op=OPEN&user.name=" + HdfsUsername;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, localPath);
                }
            }
            catch(WebException ex)
            {
                if (ex.Response == null)
                    throw new ArgumentException("Failed to upload file.", ex);
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Converts a pattern containing wildcard characters to regular expression.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>The regex corresponding to the wildcard pattern.</returns>
        private static String WildcardToRegex(String pattern)
        {
            return "^" + Regex.Escape(pattern).
                Replace("\\*", ".*").
                Replace("\\?", ".") + "$";
        }
        #endregion
    }
}
