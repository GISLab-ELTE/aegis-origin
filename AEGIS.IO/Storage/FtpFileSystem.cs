// <copyright file="FtpFileSystem.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.IO.Storage.Authentication;
using ELTE.AEGIS.IO.Storage.Operation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.IO.Storage
{
    /// <summary>
    /// Represents FTP file system
    /// </summary>
    /// <author>Marcell Lipp</author>
    public class FtpFileSystem : FileSystem
    {
        #region Public properties

        /// <summary>
        /// Gets the scheme name for this file system.
        /// </summary>
        /// <value>The scheme name for this file system.</value>
        public override String UriScheme { get { return Uri.UriSchemeFtp; } }

        /// <summary>
        /// Gets the directory separator for this file system.
        /// </summary>
        /// <value>The directory separator for this file system.</value>
        public override Char DirectorySeparator { get { return '/'; } }

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
                try
                {
                    return IsDirectory("/");
                }
                catch
                {
                    return false;
                }
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

        /// <summary>
        /// Gets a value indicating whether the connection to the file system is secure.
        /// </summary>
        /// <value><c>true</c> if operations ans credentials are handled in a secure manner; otherwise, <c>false</c>.</value>
        public override Boolean IsSecureConnection { get { return false; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystem" /> class.
        /// </summary>
        /// <param name="location">The URI of the file system location.</param>
        public FtpFileSystem(Uri location)
            : base(location, new AnonymousFileSystemAuthentication())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystem" /> class.
        /// </summary>
        /// <param name="location">The URI of the file system location.</param>
        /// <param name="username">The user name.</param>
        /// <param name="password">The password.</param>
        public FtpFileSystem(Uri location, String username, String password)
            : base(location, new UsernamePasswordFileSystemAuthentication(username, password))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystem" /> class.
        /// </summary>
        /// <param name="location">The URI of the file system location.</param>
        /// <param name="username">The user name.</param>
        /// <param name="password">The password.</param>
        public FtpFileSystem(Uri location, String username, SecureString password)
            : base(location, new UsernamePasswordFileSystemAuthentication(username, password))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystem"/> class.
        /// </summary>
        /// <param name="location">The URI of the file system location.</param>
        /// <param name="authentication"></param>
        public FtpFileSystem(Uri location, IFileSystemAuthentication authentication) : base(location, authentication)
        { }

        #endregion

        #region Public FileSystem methods

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override void CreateDirectory(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            FtpCreateDirectoryOperation operation;
            try
            {
                operation = new FtpCreateDirectoryOperation(CreateUri(path), Authentication);
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, path);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

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
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task CreateDirectoryAsync(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            FtpCreateDirectoryOperation operation;
            try
            {
                operation = new FtpCreateDirectoryOperation(CreateUri(path));
                await operation.ExecuteAsync();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, path);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
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
        /// or
        /// The path already exists.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override Stream CreateFile(String path, Boolean overwrite)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            if (!overwrite && Exists(path))
                throw new ArgumentException(MessagePathExists, "path");

            FtpCreateFileOperation operation = new FtpCreateFileOperation(CreateUri(path));

            try
            {
                return (operation.Execute() as StreamOperationResult).Stream;
            }
            catch (SecurityException ex)
            {
                throw new UnauthorizedAccessException(MessagePathUnauthorized, ex);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(MessageNoConnectionToFileSystem, ex);
            }
        }

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
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override async Task<Stream> CreateFileAsync(String path, Boolean overwrite)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            if (!overwrite && Exists(path))
                throw new ArgumentException(MessagePathExists, "path");

            FtpCreateFileOperation operation = new FtpCreateFileOperation(CreateUri(path));

            try
            {
                return (await operation.ExecuteAsync() as StreamOperationResult).Stream;
            }
            catch (SecurityException ex)
            {
                throw new UnauthorizedAccessException(MessagePathUnauthorized, ex);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(MessageNoConnectionToFileSystem, ex);
            }
        }

        /// <summary>
        /// Opens a stream on the specified path with read/write access.
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
        /// The file mode is invalid.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override Stream OpenFile(String path, FileMode mode, FileAccess access)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            FtpOpenFileOperation operation = new FtpOpenFileOperation(CreateUri(path));

            try
            {
                return (operation.Execute() as StreamOperationResult).Stream;
            }
            catch (SecurityException ex)
            {
                throw new UnauthorizedAccessException(MessagePathUnauthorized, ex);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(MessageNoConnectionToFileSystem, ex);
            }
        }

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
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override async Task<Stream> OpenFileAsync(String path, FileMode mode, FileAccess access)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            FtpOpenFileOperation operation = new FtpOpenFileOperation(CreateUri(path));

            try
            {
                StreamOperationResult result = await operation.ExecuteAsync() as StreamOperationResult;
                return result.Stream;
            }
            catch (SecurityException ex)
            {
                throw new UnauthorizedAccessException(MessagePathUnauthorized, ex);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(MessageNoConnectionToFileSystem, ex);
            }
        }

        /// <summary>
        /// Deletes the file system entry located at the specified path.
        /// </summary>
        /// <param name="path">The path of the entry to delete.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override void Delete(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            if (!Exists(path))
                throw new ArgumentException(MessagePathNotExists, "path");

            FtpDeleteOperation operation;
            try
            {
                operation = new FtpDeleteOperation(CreateUri(path));
                operation.Execute();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, path);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

        /// <summary>
        /// Deletes the file system entry located at the specified path.
        /// </summary>
        /// <param name="path">The path of the entry to delete.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The path does not exist.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task DeleteAsync(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            if (!Exists(path))
                throw new ArgumentException(MessagePathNotExists, "path");

            FtpDeleteOperation operation;
            try
            {
                operation = new FtpDeleteOperation(CreateUri(path));
                await operation.ExecuteAsync();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, path);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

        /// <summary>
        /// Moves a file system entry and its contents to a new location.
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
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for either the source or the destination path.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override void Move(String sourcePath, String destinationPath)
        {
            if (sourcePath == null)
                throw new ArgumentNullException("sourcePath", MessageSourcePathIsNull);
            if (destinationPath == null)
                throw new ArgumentNullException("destinationPath", MessageDestinationPathIsNull);
            if (String.IsNullOrWhiteSpace(sourcePath))
                throw new ArgumentException(MessageSourcePathIsEmpty, "sourcePath");
            if (String.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException(MessageDestinationPathIsEmpty, "destinationPath");

            if (sourcePath.Equals(destinationPath))
                throw new ArgumentException(MessageSourceDestinationPathEqual, "destinationPath");

            if (Exists(destinationPath))
                throw new ArgumentException(MessageDestinationPathExists, "destinationPath");

            FtpMoveOperation operation;
            try
            {
                operation = new FtpMoveOperation(CreateUri(sourcePath), CreateUri(destinationPath));
                operation.Execute();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, sourcePath);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

        /// <summary>
        /// Moves a file system entry and its contents to a new location.
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
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for either the source or the destination path.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task MoveAsync(String sourcePath, String destinationPath)
        {
            if (sourcePath == null)
                throw new ArgumentNullException("sourcePath", MessageSourcePathIsNull);
            if (destinationPath == null)
                throw new ArgumentNullException("destinationPath", MessageDestinationPathIsNull);
            if (String.IsNullOrWhiteSpace(sourcePath))
                throw new ArgumentException(MessageSourcePathIsEmpty, "sourcePath");
            if (String.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException(MessageDestinationPathIsEmpty, "destinationPath");

            if (sourcePath.Equals(destinationPath))
                throw new ArgumentException(MessageSourceDestinationPathEqual, "destinationPath");

            if (Exists(destinationPath))
                throw new ArgumentException(MessageDestinationPathExists, "destinationPath");

            FtpMoveOperation operation;
            try
            {
                operation = new FtpMoveOperation(CreateUri(sourcePath), CreateUri(destinationPath));
                await operation.ExecuteAsync();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, sourcePath);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

        /// <summary>
        /// Copies an existing file system entry to a new location.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        public override void Copy(String sourcePath, String destinationPath)
        {
            throw new NotSupportedException(MessageNotSupported);
        }

        /// <summary>
        /// Determines whether the specified path exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, otherwise, <c>false</c>.</returns>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override Boolean Exists(String path)
        {
            if (path == null)
                return false;
            if (String.IsNullOrWhiteSpace(path))
                return false;

            // the root path always exists
            if (path.Equals(VolumeSeparator.ToString()))
                return true;

            try
            {
                String parentUri = GetParent(path);
                return GetFileSystemEntries(parentUri).Count(a => a.Path == path) > 0;
            }
            catch (SecurityException ex)
            {
                throw new UnauthorizedAccessException(MessagePathUnauthorized, ex);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(MessageNoConnectionToFileSystem, ex);
            }
        }

        /// <summary>
        /// Asynchronously determines whether the specified path exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, otherwise, <c>false</c>.</returns>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task<Boolean> ExistsAsync(String path)
        {
            if (path == null)
                return false;
            if (String.IsNullOrWhiteSpace(path))
                return false;

            // the root path always exists
            if (path.Equals(VolumeSeparator.ToString()))
                return true;

            try
            {
                String parentUri = await GetParentAsync(path);
                return (await GetFileSystemEntriesAsync(parentUri)).Any(a => a.Path.Equals(path));
            }
            catch (SecurityException ex)
            {
                throw new UnauthorizedAccessException(MessagePathUnauthorized, ex);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(MessageNoConnectionToFileSystem, ex);
            }
        }

        /// <summary>
        /// Determines whether the specified path is an existing directory.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, and is a directory, otherwise, <c>false</c>.</returns>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override Boolean IsDirectory(String path)
        {
            if (path == null)
                return false;
            if (String.IsNullOrWhiteSpace(path))
                return false;

            return GetDirectories(GetParent(path)).Contains(path.TrimEnd(DirectorySeparator));
        }

        /// <summary>
        /// Asynchronously determines whether the specified path is an existing directory.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, and is a directory, otherwise, <c>false</c>.</returns>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task<Boolean> IsDirectoryAsync(String path)
        {
            if (path == null)
                return false;
            if (String.IsNullOrWhiteSpace(path))
                return false;

            return (await GetDirectoriesAsync(await GetParentAsync(path))).Contains(path.TrimEnd(DirectorySeparator));
        }

        /// <summary>
        /// Determines whether the specified path is an existing file.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        ///   <c>true</c> if the path exists, and is a file, otherwise <c>false</c>.
        /// </returns>
        public override Boolean IsFile(String path)
        {
            return GetFiles(GetParent(path)).Contains(path);
        }

        /// <summary>
        /// Asynchronously determines whether the specified path is an existing file.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the path exists, and is a file, otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the file system.</exception>
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task<Boolean> IsFileAsync(String path)
        {
            return (await GetFilesAsync(await GetParentAsync(path))).Contains(path);
        }

        /// <summary>
        /// Returns the root information for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>A string containing the root information.</returns>
        public override String GetDirectoryRoot(String path)
        {
            return VolumeSeparator.ToString();
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
        public override String GetParent(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            if (!path.Contains(DirectorySeparator))
                throw new ArgumentException(MessagePathInvalidFormat);

            return path.Substring(0, path.LastIndexOf(DirectorySeparator) - 1);
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
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public override String GetDirectory(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            if (!path.Contains(DirectorySeparator))
                throw new ArgumentException(MessagePathInvalidFormat);

            if (IsDirectory(path))
                return path;

            return GetParent(path);
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
        public override String GetFileName(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            if (!path.Contains(DirectorySeparator))
                throw new ArgumentException(MessagePathInvalidFormat);

            if (IsDirectory(path))
                return String.Empty;

            return path.Substring(path.LastIndexOf(DirectorySeparator) + 1);
        }

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
        public async override Task<String> GetFileNameAsync(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            if (!path.Contains(DirectorySeparator))
                throw new ArgumentException(MessagePathInvalidFormat);

            if (await IsDirectoryAsync(path))
                return String.Empty;

            return path.Substring(path.LastIndexOf(DirectorySeparator) + 1);
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
        public override String GetFileNameWithoutExtension(String path)
        {
            return GetFileName(path).Substring(0, GetFileName(path).LastIndexOf('.'));
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
        /// </exception>
        public override String GetExtension(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            try
            {
                return Path.GetExtension(path);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(MessagePathInvalidFormat, "path", ex);
            }
        }

        /// <summary>
        /// Returns the names of the root directories of the file system.
        /// </summary>
        /// <returns>The array containing the root directories in the file system.</returns>
        public override String[] GetRootDirectories()
        {
            return new String[] { DirectorySeparator.ToString() };
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
        /// The path does not exist.
        /// or
        /// The path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="ConnectionException">No connection is available to the file system.</exception>
        public override String[] GetDirectories(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            // check whether the path is a file
            if (IsFile(path))
                throw new ArgumentException(MessagePathIsFile, "path");

            FtpFileListingOperation operation = new FtpFileListingOperation(CreateUri(path));

            try
            {
                FileListingOperationResult result = operation.Execute() as FileListingOperationResult;

                // if the query is not recursive, return the results
                if (!recursive)
                    return result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path).ToArray();

                // if the query is recursive, the directories are accumulated in a queue
                List<String> fileList = new List<String>();
                Queue<String> directoryQueue = new Queue<String>();

                fileList.AddRange(result.Entries.Where(entry => entry.Type == FileSystemEntryType.File && IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path));

                foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                    directoryQueue.Enqueue(entry.Path);

                // while there are directories to list
                while (directoryQueue.Count > 0)
                {
                    operation = new FtpFileListingOperation(CreateUri(directoryQueue.Dequeue()));
                    result = operation.Execute() as FileListingOperationResult;

                    fileList.AddRange(result.Entries.Where(entry => entry.Type == FileSystemEntryType.File && IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path));

                    foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                        directoryQueue.Enqueue(entry.Path);
                }

                return fileList.ToArray();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, path);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

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
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task<String[]> GetDirectoriesAsync(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            // check whether the path is a file
            if (await IsFileAsync(path))
                throw new ArgumentException(MessagePathIsFile, "path");

            FtpFileListingOperation operation = new FtpFileListingOperation(CreateUri(path));

            try
            {
                FileListingOperationResult result = (await operation.ExecuteAsync()) as FileListingOperationResult;

                // if the query is not recursive, return the results
                if (!recursive)
                    return result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path).ToArray();

                // if the query is recursive, the directories are accumulated in a queue
                List<String> fileList = new List<String>();
                Queue<String> directoryQueue = new Queue<String>();

                fileList.AddRange(result.Entries.Where(entry => entry.Type == FileSystemEntryType.File && IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path));

                foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                    directoryQueue.Enqueue(entry.Path);

                // while there are directories to list
                while (directoryQueue.Count > 0)
                {
                    operation = new FtpFileListingOperation(CreateUri(directoryQueue.Dequeue()));
                    result = (await operation.ExecuteAsync()) as FileListingOperationResult;

                    fileList.AddRange(result.Entries.Where(entry => entry.Type == FileSystemEntryType.File && IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path));

                    foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                        directoryQueue.Enqueue(entry.Path);
                }

                return fileList.ToArray();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, path);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
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
        /// The path does not exist.
        /// or
        /// The path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="ConnectionException">No connection is available to the file system.</exception>
        public override String[] GetFiles(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            // check whether the path is a file
            if (IsFile(path))
                throw new ArgumentException(MessagePathIsFile, "path");

            FtpFileListingOperation operation = new FtpFileListingOperation(CreateUri(path));

            try
            {
                FileListingOperationResult result = operation.Execute() as FileListingOperationResult;

                // if the query is not recursive, return the results
                if (!recursive)
                    return result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path).ToArray();

                // if the query is recursive, the directories are accumulated in a queue
                List<String> fileList = new List<String>();
                Queue<String> directoryQueue = new Queue<String>();

                fileList.AddRange(result.Entries.Where(entry => entry.Type == FileSystemEntryType.File && IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path));

                foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                    directoryQueue.Enqueue(entry.Path);

                // while there are directories to list
                while (directoryQueue.Count > 0)
                {
                    operation = new FtpFileListingOperation(CreateUri(directoryQueue.Dequeue()));
                    result = operation.Execute() as FileListingOperationResult;

                    fileList.AddRange(result.Entries.Where(entry => entry.Type == FileSystemEntryType.File && IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path));

                    foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                        directoryQueue.Enqueue(entry.Path);
                }

                return fileList.ToArray();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, path);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

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
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task<String[]> GetFilesAsync(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            // check whether the path is a file
            if (await IsFileAsync(path))
                throw new ArgumentException(MessagePathIsFile, "path");

            FtpFileListingOperation operation = new FtpFileListingOperation(CreateUri(path));

            try
            {
                FileListingOperationResult result = (await operation.ExecuteAsync()) as FileListingOperationResult;

                // if the query is not recursive, return the results
                if (!recursive)
                    return result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path).ToArray();

                // if the query is recursive, the directories are accumulated in a queue
                List<String> fileList = new List<String>();
                Queue<String> directoryQueue = new Queue<String>();

                fileList.AddRange(result.Entries.Where(entry => entry.Type == FileSystemEntryType.File && IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path));

                foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                    directoryQueue.Enqueue(entry.Path);

                // while there are directories to list
                while (directoryQueue.Count > 0)
                {
                    operation = new FtpFileListingOperation(CreateUri(directoryQueue.Dequeue()));
                    result = (await operation.ExecuteAsync()) as FileListingOperationResult;

                    fileList.AddRange(result.Entries.Where(entry => entry.Type == FileSystemEntryType.File && IsMatch(entry.Path, searchPattern)).Select(entry => entry.Path));

                    foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                        directoryQueue.Enqueue(entry.Path);
                }

                return fileList.ToArray();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    case FileSystemOperationResultCode.ConnectionNotAvailable:
                        throw new ConnectionException(MessageNoConnectionToPath, path);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
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
        /// The path does not exist.
        /// or
        /// The path is a file.
        /// or
        /// The search pattern is an invalid format.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        /// <exception cref="ConnectionException">No connection is available to the file system.</exception>
        public override FileSystemEntry[] GetFileSystemEntries(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            // check whether the path is a file
            if (IsFile(path))
                throw new ArgumentException(MessagePathIsFile, "path");

            FtpFileListingOperation operation = new FtpFileListingOperation(CreateUri(path));

            try
            {
                FileListingOperationResult result = operation.Execute() as FileListingOperationResult;

                // if the query is not recursive, return the results
                if (!recursive)
                    return result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)).ToArray();

                // if the query is recursive, the directories are accumulated in a queue
                List<FileSystemEntry> entryList = new List<FileSystemEntry>();
                Queue<String> directoryQueue = new Queue<String>();

                entryList.AddRange(result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)));

                foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                    directoryQueue.Enqueue(entry.Path);

                // while there are directories to list
                while (directoryQueue.Count > 0)
                {
                    operation = new FtpFileListingOperation(CreateUri(directoryQueue.Dequeue()));
                    result = operation.Execute() as FileListingOperationResult;

                    entryList.AddRange(result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)));

                    foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                        directoryQueue.Enqueue(entry.Path);
                }

                return entryList.ToArray();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

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
        /// <exception cref="ConnectionException">
        /// No connection is available to the specified path.
        /// or
        /// No connection is available to the file system.
        /// </exception>
        public async override Task<FileSystemEntry[]> GetFileSystemEntriesAsync(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", MessagePathIsNull);
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException(MessagePathIsEmpty, "path");

            // check whether the path is a file
            if (IsFile(path))
                throw new ArgumentException(MessagePathIsFile, "path");

            FtpFileListingOperation operation = new FtpFileListingOperation(CreateUri(path));

            try
            {
                FileListingOperationResult result = (await operation.ExecuteAsync()) as FileListingOperationResult;

                // if the query is not recursive, return the results
                if (!recursive)
                    return result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)).ToArray();

                // if the query is recursive, the directories are accumulated in a queue
                List<FileSystemEntry> entryList = new List<FileSystemEntry>();
                Queue<String> directoryQueue = new Queue<String>();

                entryList.AddRange(result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)));

                foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                    directoryQueue.Enqueue(entry.Path);

                // while there are directories to list
                while (directoryQueue.Count > 0)
                {
                    operation = new FtpFileListingOperation(CreateUri(directoryQueue.Dequeue()));
                    result = (await operation.ExecuteAsync()) as FileListingOperationResult;

                    entryList.AddRange(result.Entries.Where(entry => IsMatch(entry.Path, searchPattern)));

                    foreach (FileSystemEntry entry in result.Entries.Where(entry => entry.Type == FileSystemEntryType.Directory))
                        directoryQueue.Enqueue(entry.Path);
                }

                return entryList.ToArray();
            }
            catch (FileSystemOperationException ex)
            {
                switch (ex.Code)
                {
                    case FileSystemOperationResultCode.InvalidPath:
                        throw new ArgumentException(MessagePathInvalidFormat, "path");
                    case FileSystemOperationResultCode.UnavailablePath:
                        throw new ArgumentException(MessagePathNotExists, "path");
                    case FileSystemOperationResultCode.UnsupportedOperation:
                        throw new NotSupportedException(MessageNotSupported);
                    case FileSystemOperationResultCode.AccessDenied:
                        throw new UnauthorizedAccessException(MessageUnauthorized, ex);
                    default:
                        throw new ConnectionException(MessageNoConnectionToFileSystem);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates the URI from the specified local path.
        /// </summary>
        /// <param name="path">The local path.</param>
        /// <returns>The URI.</returns>
        private Uri CreateUri(String path)
        {
            // remove directory separator
            return new Uri(Location + path.TrimEnd(DirectorySeparator));
        }

        /// <summary>
        /// Determines whether the specified name matches the pattern.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="pattern">The search pattern.</param>
        /// <returns><c>true</c> if the specified path matches the pattern; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The search pattern is an invalid format.</exception>
        private static Boolean IsMatch(String path, String pattern)
        {
            if (String.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException(MessageInvalidSearchPattern, "searchPattern");

            pattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";

            try
            {
                return Regex.IsMatch(path, pattern, RegexOptions.CultureInvariant);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(MessageInvalidSearchPattern, "searchPattern", ex);
            }
        }

        #endregion
    }
}
