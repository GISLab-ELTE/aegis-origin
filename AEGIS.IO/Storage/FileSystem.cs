/// <copyright file="FileSystem.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.IO.Storage.Authentication;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Storage
{
    /// <summary>
    /// Represents a file system.
    /// </summary>
    public abstract class FileSystem
    {
        #region Protected constant fields

        /// <summary>
        /// Exception message in case the path is null. This field is constant.
        /// </summary>
        protected const String MessagePathIsNull = "The path is null.";

        /// <summary>
        /// Exception message in case the path is empty, or consists only of whitespace characters. This field is constant.
        /// </summary>
        protected const String MessagePathIsEmpty = "The path is empty, or consists only of whitespace characters.";

        /// <summary>
        /// Exception message in case the path is a directory. This field is constant.
        /// </summary>
        protected const String MessagePathIsDirectory = "The path is a directory.";

        /// <summary>
        /// Exception message in case the specified path is a file. This field is constant.
        /// </summary>
        protected const String MessagePathIsFile = "The specified path is a file.";

        /// <summary>
        /// Exception message in case the path is in an invalid format. This field is constant.
        /// </summary>
        protected const String MessagePathInvalidFormat = "The path is in an invalid format.";

        /// <summary>
        /// Exception message in case the path exceeds the maximum length supported by the file system. This field is constant.
        /// </summary>
        protected const String MessagePathTooLong = "The path exceeds the maximum length supported by the file system.";

        /// <summary>
        /// Exception message in case the caller does not have the required permission for the path. This field is constant.
        /// </summary>
        protected const String MessagePathUnauthorized = "The caller does not have the required permission for the path.";

        /// <summary>
        /// Exception message in case the file system entry on the specified path is currently in use. This field is constant.
        /// </summary>
        protected const String MessagePathInUse = "The file system entry on the specified path is currently in use.";

        /// <summary>
        /// Exception message in case the path already exists. This field is constant.
        /// </summary>
        protected const String MessagePathExists = "The path already exists.";

        /// <summary>
        /// Exception message in case the path does not exist. This field is constant.
        /// </summary>
        protected const String MessagePathNotExists = "The path does not exist.";

        /// <summary>
        /// Exception message in case the source path is null. This field is constant.
        /// </summary>
        protected const String MessageSourcePathIsNull = "The source path is null.";
        
        /// <summary>
        /// Exception message in case the source path is empty, or consists only of whitespace characters. This field is constant.
        /// </summary>
        protected const String MessageSourcePathIsEmpty = "The source path is empty, or consists only of whitespace characters.";

        /// <summary>
        /// Exception message in case the source path does not exist. This field is constant.
        /// </summary>
        protected const String MessageSourcePathNotExists = "The source path does not exist.";

        /// <summary>
        /// Exception message in case the source path is in an invalid format. This field is constant.
        /// </summary>
        protected const String MessageSourcePathInvalidFormat = "The source path is in an invalid format.";

        /// <summary>
        /// Exception message in case the path exceeds the maximum length supported by the file system. This field is constant.
        /// </summary>
        protected const String MessageSourcePathTooLong = "The path exceeds the maximum length supported by the file system.";

        /// <summary>
        /// Exception message in case the file system entry on the specified path is currently in use. This field is constant.
        /// </summary>
        protected const String MessageSourcePathInUse = "The file system entry on the specified path is currently in use.";

        /// <summary>
        /// Exception message in case the source and destination paths are equal. This field is constant.
        /// </summary>
        protected const String MessageSourceDestinationPathEqual = "The source and destination paths are equal.";

        /// <summary>
        /// Exception message in case the source or destination paths exceed the system-defined maximum length. This field is constant.
        /// </summary>
        protected const String MessageSourceDestinationPathTooLong = "The path exceeds the maximum length supported by the file system.";

        /// <summary>
        /// Exception message in case the caller does not have the required permission for either the source or the destination path. This field is constant.
        /// </summary>
        protected const String MessageSourceDestinationPathUnauthorized = "The caller does not have the required permission for either the source or the destination path.";

        /// <summary>
        /// Exception message in case the destination path is null. This field is constant.
        /// </summary>
        protected const String MessageDestinationPathIsNull = "The destination path is null.";

        /// <summary>
        /// Exception message in case the destination path is empty, or consists only of whitespace characters. This field is constant.
        /// </summary>
        protected const String MessageDestinationPathIsEmpty = "The destination path is empty, or consists only of whitespace characters.";

        /// <summary>
        /// Exception message in case the destination path is in an invalid format. This field is constant.
        /// </summary>
        protected const String MessageDestinationPathInvalidFormat = "The destination path is in an invalid format.";

        /// <summary>
        /// Exception message in case the destination path already exists. This field is constant.
        /// </summary>
        protected const String MessageDestinationPathExists = "The destination path already exists.";

        /// <summary>
        /// Exception message in case no connection is available to the specified path. This field is constant.
        /// </summary>
        protected const String MessageNoConnectionToPath = "No connection is available to the specified path.";
        
        /// <summary>
        /// Exception message in case no connection is available to the file system. This field is constant.
        /// </summary>
        protected const String MessageNoConnectionToFileSystem = "No connection is available to the file system.";

        /// <summary>
        /// Exception message in case the file mode is invalid. This field is constant.
        /// </summary>
        protected const String MessageInvalidFileMode = "The file mode is invalid.";

        /// <summary>
        /// Exception message in case the file access is invalid. This field is constant.
        /// </summary>
        protected const String MessageInvalidFileAccess = "The file access is invalid.";

        /// <summary>
        /// Exception message in case the search pattern is an invalid format. This field is constant.
        /// </summary>
        protected const String MessageInvalidSearchPattern = "The search pattern is an invalid format.";

        /// <summary>
        /// Exception message in case the specified file mode and file access combination is invalid. This field is constant.
        /// </summary>
        protected const String MessageInvalidFileModeOrAccess = "The specified file mode and file access combination is invalid.";

        /// <summary>
        /// Exception message in case the file on path is read-only. This field is constant.
        /// </summary>
        protected const String MessagePathReadOnly = "The file on path is read-only.";

        /// <summary>
        /// Exception message in case the caller does not have the required permission. This field is constant.
        /// </summary>
        protected const String MessageUnauthorized = "The caller does not have the required permission.";

        /// <summary>
        /// Exception message in case the operation is not supported by the file system. This field is constant.
        /// </summary>
        protected const String MessageNotSupported = "The operation is not supported by the file system.";

        #endregion

        #region Public static methods

        /// <summary>
        /// Gets the local file system.
        /// </summary>
        /// <returns>The local file system.</returns>
        public static FileSystem GetLocalFileSystem() { return new LocalFileSystem();  }

        /// <summary>
        /// Gets the file system for the specified path.
        /// </summary>
        /// <param name="path">The path in a specific file system.</param>
        /// <returns>The file system of the specified path.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is invalid.
        /// </exception>
        public static FileSystem GetFileSystemForPath(String path) 
        { 
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty, or consists only of whitespace characters.", "path");

            Uri uri;
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri))
                throw new ArgumentException("The path is invalid.", "path");

            return GetFileSystemForPath(uri); 
        }

        /// <summary>
        /// Gets the file system for the specified path.
        /// </summary>
        /// <param name="path">The path in a specific file system.</param>
        /// <returns>The file system of the specified path.</returns>
        public static FileSystem GetFileSystemForPath(Uri path) { return new LocalFileSystem(); }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the scheme name for this file system.
        /// </summary>
        /// <value>The scheme name for this file system.</value>
        public abstract String UriScheme { get; }

        /// <summary>
        /// Gets the directory separator for this file system.
        /// </summary>
        /// <value>The directory separator for this file system.</value>
        public abstract Char DirectorySeparator { get; }

        /// <summary>
        /// Gets the path separator for this file system.
        /// </summary>
        /// <value>The path separator for this file system.</value>
        public abstract Char PathSeparator { get; }

        /// <summary>
        /// Gets the volume separator for this file system.
        /// </summary>
        /// <value>The volume separator for this file system.</value>
        public abstract Char VolumeSeparator { get; }

        /// <summary>
        /// Gets a value indicating whether the file system is connected.
        /// </summary>
        /// <value><c>true</c> if operations can be executed on the file system; otherwise, <c>false</c>.</value>
        public abstract Boolean IsConnected { get; }

        /// <summary>
        /// Gets a value indicating whether file streaming is supported by the file system.
        /// </summary>
        /// <value><c>true</c> if file streaming commands can be executed on the file system; otherwise, <c>false</c>.</value>
        public abstract Boolean IsStreamingSupported { get; }

        /// <summary>
        /// Gets a value indicating whether content browsing is supported by the file system.
        /// </summary>
        /// <value><c>true</c> if the content of the file system can be listed; otherwise, <c>false</c>.</value>
        public abstract Boolean IsContentBrowsingSupported { get; }

        /// <summary>
        /// Gets a value indicating whether content writing is supported by the file system.
        /// </summary>
        /// <value><c>true</c> if file creation, modification and removal operations are supported by the file system; otherwise, <c>false</c>.</value>
        public abstract Boolean IsContentWritingSupported { get; }

        /// <summary>
        /// Gets a value indicating whether the connection to the file system is secure.
        /// </summary>
        /// <value><c>true</c> if operations ans credentials are handled in a secure manner; otherwise, <c>false</c>.</value>
        public abstract Boolean IsSecureConnection { get; }

        /// <summary>
        /// Gets the authentication used by the file system.
        /// </summary>
        /// <value>The authentication used by the file system.</value>
        public IFileSystemAuthentication Authentication { get; private set; }

        /// <summary>
        /// Gets the location of the file system.
        /// </summary>
        /// <value>The URI of the file system location.</value>
        public Uri Location { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="location">The URI of the file system location.</param>
        protected FileSystem(Uri location, IFileSystemAuthentication authentication)
        {
            if (location == null)
                throw new ArgumentNullException("location", "The location is null.");

            Location = location;
            Authentication = authentication ?? new AnonymousFileSystemAuthentication();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The specified path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract void CreateDirectory(String path);

        /// <summary>
        /// Creates the directory asynchronously.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The specified path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public virtual async void CreateDirectoryAsync(String path)
        {
            await Task.Run(() => CreateDirectory(path));
        }

        /// <summary>
        /// Creates or overwrites a file on the specified path.
        /// </summary>
        /// <param name="path">The path of a file to create.</param>
        /// <returns>A stream that provides read/write access to the file specified in path.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path already exists.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public Stream CreateFile(String path)
        {            
            return CreateFile(path, true);
        }

        /// <summary>
        /// Creates or overwrites a file on the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of a file to create.</param>
        /// <returns>A stream that provides read/write access to the file specified in path.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path already exists.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async Task<Stream> CreateFileAsync(String path)
        {
            return await CreateFileAsync(path, true);
        }

        /// <summary>
        /// Creates or overwrites a file on the specified path.
        /// </summary>
        /// <param name="path">The path of a file to create.</param>
        /// <param name="overwrite">A value that specifies whether the file should be overwritten in case it exists.</param>
        /// <returns>A stream that provides read/write access to the file specified in path.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract Stream CreateFile(String path, Boolean overwrite);

        /// <summary>
        /// Creates or overwrites a file asynchronously on the specified path.
        /// </summary>
        /// <param name="path">The path of a file to create.</param>
        /// <param name="overwrite">A value that specifies whether the file should be overwritten in case it exists.</param>
        /// <returns>A stream that provides read/write access to the file specified in path.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public virtual async Task<Stream> CreateFileAsync(String path, Boolean overwrite)
        {
            return await Task.Run(() => CreateFile(path, overwrite));
        }

        /// <summary>
        /// Opens a stream on the specified path with read/write access.
        /// </summary>
        /// <param name="path">The path of a file to open.</param>
        /// <param name="mode">A value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <returns>A stream in the specified mode and path, with read/write access.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path does not exist.
        /// or
        /// The path is a directory.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The file mode is invalid.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public Stream OpenFile(String path, FileMode mode)
        {
            return OpenFile(path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite);
        }

        /// <summary>
        /// Opens a stream asynchronously on the specified path with read/write access.
        /// </summary>
        /// <param name="path">The path of a file to open.</param>
        /// <param name="mode">A value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <returns>A stream in the specified mode and path, with read/write access.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path does not exist.
        /// or
        /// The path is a directory.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The file mode is invalid.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async Task<Stream> OpenFileAsync(String path, FileMode mode)
        {
            return await OpenFileAsync(path, mode, FileAccess.ReadWrite);
        }

        /// <summary>
        /// Opens a stream on the specified path.
        /// </summary>
        /// <param name="path">The path of a file to open.</param>
        /// <param name="mode">A value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A value that specifies the operations that can be performed on the file.</param>
        /// <returns>A stream in the specified mode and path, with the specified access.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path does not exist.
        /// or
        /// The path is a directory.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The file mode is invalid.
        /// or
        /// The file access is invalid.
        /// or
        /// The specified file mode and file access combination is invalid.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract Stream OpenFile(String path, FileMode mode, FileAccess access);

        /// <summary>
        /// Opens a stream asynchronously on the specified path.
        /// </summary>
        /// <param name="path">The path of a file to open.</param>
        /// <param name="mode">A value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A value that specifies the operations that can be performed on the file.</param>
        /// <returns>A stream in the specified mode and path, with the specified access.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path does not exist.
        /// or
        /// The path is a directory.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The file mode is invalid.
        /// or
        /// The file access is invalid.
        /// or
        /// The specified file mode and file access combination is invalid.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public virtual async Task<Stream> OpenFileAsync(String path, FileMode mode, FileAccess access)
        {
            return await Task.Run(() => OpenFile(path, mode, access));
        }

        /// <summary>
        /// Deletes the filesystem entry located at the specified path.
        /// </summary>
        /// <param name="path">The path of the entry to delete.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The file system entry on the specified path is currently in use.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract void Delete(String path);

        /// <summary>
        /// Deletes the filesystem entry located at the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of the entry to delete.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The file system entry on the specified path is currently in use.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual void DeleteAsync(String path)
        {
            await Task.Run(() => Delete(path));
        }

        /// <summary>
        /// Moves a filesystem entry to a new location.
        /// </summary>
        /// <param name="sourcePath">The path of the file or directory to move.</param>
        /// <param name="destinationPath">The path to the new location for the entry.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source path is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source path is empty, or consists only of whitespace characters.
        /// or
        /// The destination path is empty, or consists only of whitespace characters.
        /// or
        /// The source and destination paths are equal.
        /// or
        /// The source path does not exist.
        /// or
        /// The destination path already exists.
        /// or
        /// The source path is in an invalid format.
        /// or
        /// The destination path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for either the source or the destination path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract void Move(String sourcePath, String destinationPath);

        /// <summary>
        /// Moves a filesystem entry asynchronously to a new location.
        /// </summary>
        /// <param name="sourcePath">The path of the file or directory to move.</param>
        /// <param name="destinationPath">The path to the new location for the entry.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source path is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source path is empty, or consists only of whitespace characters.
        /// or
        /// The destination path is empty, or consists only of whitespace characters.
        /// or
        /// The source and destination paths are equal.
        /// or
        /// The source path does not exist.
        /// or
        /// The destination path already exists.
        /// or
        /// The source path is in an invalid format.
        /// or
        /// The destination path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for either the source or the destination path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual void MoveAsync(String sourcePath, String destinationPath)
        {
            await Task.Run(() => Move(sourcePath, destinationPath));
        }

        /// <summary>
        /// Combines the specified paths.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The array of file system paths.</param>
        /// <returns>The combined path.</returns>
        public String Combine(params String[] path)
        {
            if (path == null || path.Length == 0)
                return null;

            StringBuilder pathBuilder = new StringBuilder();

            for (Int32 pathIndex = 1; pathIndex < path.Length; pathIndex++)
            {
                if (path[pathIndex] == null || path[pathIndex].Length == 0)
                    continue;

                if (pathBuilder.Length > 0 && !path[pathIndex - 1].EndsWith(DirectorySeparator.ToString()))
                    pathBuilder.Append(DirectorySeparator);

                pathBuilder.Append(path[pathIndex]);
            }

            return pathBuilder.ToString();
        }

        /// <summary>
        /// Copies an existing filesystem entry to a new location.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source path is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source path is empty, or consists only of whitespace characters.
        /// or
        /// The destination path is empty, or consists only of whitespace characters.
        /// or
        /// The source and destination paths are equal.
        /// or
        /// The source path does not exist.
        /// or
        /// The destination path already exists.
        /// or
        /// The source path is in an invalid format.
        /// or
        /// The destination path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for either the source or the destination path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract void Copy(String sourcePath, String destinationPath);

        /// <summary>
        /// Copies an existing filesystem entry asynchronously to a new location.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source path is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source path is empty, or consists only of whitespace characters.
        /// or
        /// The destination path is empty, or consists only of whitespace characters.
        /// or
        /// The source and destination paths are equal.
        /// or
        /// The source path does not exist.
        /// or
        /// The destination path already exists.
        /// or
        /// The source path is in an invalid format.
        /// or
        /// The destination path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for either the source or the destination path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual void CopyAsync(String sourcePath, String destinationPath)
        {
            await Task.Run(() => Copy(sourcePath, destinationPath));
        }

        /// <summary>
        /// Determines whether the specified path exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract Boolean Exists(String path);

        /// <summary>
        /// Asyncronously determines whether the specified path exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<Boolean> ExistsAsync(String path)
        {
            return await Task.Run(() => Exists(path));
        }

        /// <summary>
        /// Determines whether the specified path is an existing directory.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, and is a directory, otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract Boolean IsDirectory(String path);

        /// <summary>
        /// Asyncronously determines whether the specified path is an existing directory.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, and is a directory, otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<Boolean> IsDirectoryAsync(String path)
        {
            return await Task.Run(() => IsDirectory(path));
        }

        /// <summary>
        /// Determines whether the specified path is an existing file.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, and is a file, otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract Boolean IsFile(String path);

        /// <summary>
        /// Asyncronously determines whether the specified path is an existing file.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, and is a file, otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<Boolean> IsFileAsync(String path)
        {
            return await Task.Run(() => IsFile(path));
        }

        /// <summary>
        /// Returns the root information for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>A string containing the root information.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The source path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract String GetDirectoryRoot(String path);

        /// <summary>
        /// Returns the root information for the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>A string containing the root information.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The source path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String> GetDirectoryRootAsync(String path)
        {
            return await Task.Run(() => GetDirectoryRoot(path));
        }

        /// <summary>
        /// Returns the path of the parent directory for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>The string containing the full path to the parent directory.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract String GetParent(String path);

        /// <summary>
        /// Returns the path of the parent directory for the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>The string containing the full path to the parent directory.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String> GetParentAsync(String path)
        {
            return await Task.Run(() => GetParent(path));
        }

        /// <summary>
        /// Returns the full directory name for a specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>The full directory name for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract String GetDirectory(String path);

        /// <summary>
        /// Returns the full directory name for a specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>The full directory name for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String> GetDirectoryAsync(String path)
        {
            return await Task.Run(() => GetDirectory(path));
        }

        /// <summary>
        /// Returns the file name and file extension for a specified path.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>The file name and file extension for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract String GetFileName(String path);

        /// <summary>
        /// Returns the file name and file extension for a specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>The file name and file extension for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String> GetFileNameAsync(String path)
        {
            return await Task.Run(() => GetFileName(path));
        }

        /// <summary>
        /// Returns the file name without file extension for a specified path.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>The file name without file extension for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract String GetFileNameWithoutExtension(String path);

        /// <summary>
        /// Returns the file name without file extension for a specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>The file name without file extension for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String> GetFileNameWithoutExtensionAsync(String path)
        {
            return await Task.Run(() => GetFileNameWithoutExtension(path));
        }

        /// <summary>
        /// Returns the extension for a specified path.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>The extension for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract String GetExtension(String path);

        /// <summary>
        /// Returns the extension for a specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>The extension for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String> GetExtensionAsync(String path)
        {
            return await Task.Run(() => GetExtension(path));
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
        public abstract String[] GetRootDirectories();

        /// <summary>
        /// Returns the path to the root directories of the file system asynchronously.
        /// </summary>
        /// <returns>The array containing the path to the root directories in the file system.</returns>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String[]> GetRootDirectoriesAsync()
        {
            return await Task.Run(() => GetRootDirectories());
        }

        /// <summary>
        /// Returns the directories located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <returns>An array containing the full paths to all directories.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public String[] GetDirectories(String path)
        {
            return GetDirectories(path, "*", false);
        }

        /// <summary>
        /// Returns the directories located on the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <returns>An array containing the full paths to all directories.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async Task<String[]> GetDirectoriesAsync(String path)
        {
            return await GetDirectoriesAsync(path, "*", false);
        }

        /// <summary>
        /// Returns the directories located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>An array containing the full paths to all directories.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract String[] GetDirectories(String path, String searchPattern, Boolean recursive);

        /// <summary>
        /// Returns the directories located on the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>An array containing the full paths to all directories.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String[]> GetDirectoriesAsync(String path, String searchPattern, Boolean recursive)
        {
            return await Task.Run(() => GetDirectories(path));
        }

        /// <summary>
        /// Returns the files located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <returns>An array containing the full paths to all files.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public String[] GetFiles(String path)
        {
            return GetFiles(path, "*", false);
        }

        /// <summary>
        /// Returns the files located on the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <returns>An array containing the full paths to all files.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async Task<String[]> GetFilesAsync(String path)
        {
            return await GetFilesAsync(path, "*", false);
        }

        /// <summary>
        /// Returns the files located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>An array containing the full paths to all files.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract String[] GetFiles(String path, String searchPattern, Boolean recursive);

        /// <summary>
        /// Returns the files located on the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>An array containing the full paths to all files.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<String[]> GetFilesAsync(String path, String searchPattern, Boolean recursive)
        {
            return await Task.Run(() => GetFiles(path));
        }

        /// <summary>
        /// Returns the file system entries located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <returns>An array containing the file system entry informations.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public FileSystemEntry[] GetFileSystemEntries(String path)
        {
            return GetFileSystemEntries(path, "*", false);
        }

        /// <summary>
        /// Returns the file system entries located on the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <returns>An array containing the file system entry informations.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async Task<FileSystemEntry[]> GetFileSystemEntriesAsync(String path)
        {
            return await GetFileSystemEntriesAsync(path, "*", false);
        }

        /// <summary>
        /// Returns the file system entries located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>An array containing the file system entry informations.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public abstract FileSystemEntry[] GetFileSystemEntries(String path, String searchPattern, Boolean recursive);

        /// <summary>
        /// Returns the file system entries located on the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>An array containing the file system entry informations.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path exceeds the maximum length supported by the file system.
        /// or
        /// The path does not exist.
        /// or
        /// The specified path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async virtual Task<FileSystemEntry[]> GetFileSystemEntriesAsync(String path, String searchPattern, Boolean recursive)
        {
            return await Task.Run(() => GetFileSystemEntries(path));
        }

        #endregion
    }
}
