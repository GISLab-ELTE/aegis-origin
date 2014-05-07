/// <copyright file="LocalFileSystem.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System;
using System.IO;

namespace ELTE.AEGIS.IO.FileSystems
{
    /// <summary>
    /// Represents the local file system.
    /// </summary>
    public class LocalFileSystem : FileSystem
    {
        #region FileSystem public properties

        /// <summary>
        /// Gets the scheme name for this file system.
        /// </summary>
        /// <value>The scheme name for this file system.</value>
        public override String UriScheme { get { return Uri.UriSchemeFile; } }

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

        #region FileSystem public methods

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
        /// or
        /// The path is invalid.
        /// or
        /// The path is invalid.
        /// or
        /// The path exceeds the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        public override void CreateDirectory(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
            catch (NotSupportedException)
            {
                throw new ArgumentException("The path is invalid.", "path");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("The path is invalid.", "path");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path exceeds the system-defined maximum length.", "path");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
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
        /// The path is empty.
        /// or
        /// A file already exists on the path.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        public override Stream CreateFile(String path, Boolean overwrite)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            if (!overwrite && File.Exists(path))
                throw new ArgumentException("A file already exists on the path.", "path");

            try
            {
                return File.Create(path);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }            
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
            }
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
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.;path
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        public override Stream OpenFile(String path, FileMode mode, FileAccess access)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");
            if (!File.Exists(path))
                throw new ArgumentException("The path is invalid.", "path");

            try
            {
                return File.Open(path, mode, access);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
            }
            catch (UnauthorizedAccessException)
            {
                if (mode == FileMode.Create || mode == FileMode.CreateNew)
                    throw new UnauthorizedAccessException("The file on path is hidden.");
                if (access == FileAccess.ReadWrite || access == FileAccess.Write)
                    throw new UnauthorizedAccessException("The file on path is read-only.");

                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
            }
        }

        /// <summary>
        /// Deletes the filesystem entry located at the specified path.
        /// </summary>
        /// <param name="path">The path of the entry to delete.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path does not exist.
        /// </exception>
        public override void Delete(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            if (File.Exists(path))
            {
                File.Delete(path);
                return;
            }
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return;
            }
            throw new ArgumentException("The path does not exist.", "path");
        }

        /// <summary>
        /// Moves a filesystem entry and its contents to a new location.
        /// </summary>
        /// <param name="sourcePath">The path of the file or directory to move.</param>
        /// <param name="destinationPath">The path to the new location for the entry.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source path is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source path is empty.;sourcePath
        /// or
        /// The destination path is empty.;destinationPath
        /// or
        /// The source and destination paths are equal.;destinationPath
        /// or
        /// The source or destination path contains only white space, or contains one or more invalid characters.;sourcePath
        /// or
        /// The source path is invalid.;sourcePath
        /// or
        /// The source or destination paths exceed the system-defined maximum length.;sourcePath
        /// or
        /// The destination path already exists.;destinationPath
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for either the source or the destination path.</exception>
        public override void Move(String sourcePath, String destinationPath)
        {
            if (sourcePath == null)
                throw new ArgumentNullException("sourcePath", "The source path is null.");
            if (destinationPath == null)
                throw new ArgumentNullException("destinationPath", "The destination path is null.");
            if (String.IsNullOrEmpty(sourcePath))
                throw new ArgumentException("The source path is empty.", "sourcePath");
            if (String.IsNullOrEmpty(destinationPath))
                throw new ArgumentException("The destination path is empty.", "destinationPath");

            if (sourcePath.Equals(destinationPath))
                throw new ArgumentException("The source and destination paths are equal.", "destinationPath");

            try
            {
                Directory.Move(sourcePath, destinationPath);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The source or destination path contains only white space, or contains one or more invalid characters.", "sourcePath");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("The source path is invalid.", "sourcePath");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The source or destination paths exceed the system-defined maximum length.", "sourcePath");
            }
            catch (IOException)
            {
                throw new ArgumentException("The destination path already exists.", "sourcePath");
            }  
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for either the source or the destination path.");
            }
        }

        /// <summary>
        /// Copies an existing file to a new file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source path is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source path is empty.;sourcePath
        /// or
        /// The destination path is empty.;destinationPath
        /// or
        /// The source and destination paths are equal.;destinationPath
        /// or
        /// The source or destination path contains only white space, or contains one or more invalid characters.;sourcePath
        /// or
        /// The source path is invalid.;sourcePath;sourcePath
        /// or
        /// The source or destination paths exceed the system-defined maximum length.;sourcePath
        /// or
        /// The destination path already exists.;destinationPath
        /// or
        /// The destination path is invalid.;destinationPath
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for either the source or the destination path.</exception>
        public override void Copy(String sourcePath, String destinationPath)
        {
            if (sourcePath == null)
                throw new ArgumentNullException("sourcePath", "The source path is null.");
            if (destinationPath == null)
                throw new ArgumentNullException("destinationPath", "The destination path is null.");
            if (String.IsNullOrEmpty(sourcePath))
                throw new ArgumentException("The source path is empty.", "sourcePath");
            if (String.IsNullOrEmpty(destinationPath))
                throw new ArgumentException("The destination path is empty.", "destinationPath");

            if (sourcePath.Equals(destinationPath))
                throw new ArgumentException("The source and destination paths are equal.", "destinationPath");

            try
            {
                File.Copy(sourcePath, destinationPath);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The source or destination path contains only white space, or contains one or more invalid characters.", "sourcePath");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("The source path is invalid.", "sourcePath");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The source or destination paths exceed the system-defined maximum length.", "sourcePath");
            }
            catch (IOException)
            {
                throw new ArgumentException("The destination path already exists.", "sourcePath");
            }
            catch (NotSupportedException)
            {
                throw new ArgumentException("The destination path is invalid.", "destinationPath");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for either the source or the destination path.");
            }
        }

        /// <summary>
        /// Determines whether the specified path exists.
        /// </summary>
        /// <param name="path">The path of a file or directory to check.</param>
        /// <returns><c>true</c> if the path exists, otherwise, <c>false</c>.</returns>
        public override Boolean Exists(String path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        /// <summary>
        /// Determines whether the specified path is an existing directory.
        /// </summary>
        /// <param name="path">The path of a directory to check.</param>
        /// <returns><c>true</c> if the path exists, and is a directory, otherwise, <c>false</c>.</returns>
        public override Boolean IsDirectory(String path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Determines whether the specified path is an existing file.
        /// </summary>
        /// <param name="path">The path of a file to check.</param>
        /// <returns><c>true</c> if the path exists, and is a file, otherwise, <c>false</c>.</returns>
        public override Boolean IsFile(String path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Returns the root information for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>A string containing the root information.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        public override String GetDirectoryRoot(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                return Directory.GetDirectoryRoot(path);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
            }
        }

        /// <summary>
        /// Returns the path of the parent directory for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>The string containing the full path to the parent directory.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// or
        /// The specified path was not found.
        /// or
        /// The directory specified by path is read-only.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        public override String GetParent(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                return Directory.GetParent(path).FullName;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("The specified path was not found.", "path");
            }
            catch (IOException)
            {
                throw new ArgumentException("The directory specified by path is read-only.", "path");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
            }
        }

        /// <summary>
        /// Returns the full directory name for a specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>The full directory name for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
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
        /// <returns>The file name and file extension for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
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
        /// <returns>The file name without file extension for <paramref name="path" />.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
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
        /// Returns the names of the logical drives of the file system.
        /// </summary>
        /// <returns>The array containing the logical drive names in the file system.</returns>
        /// <exception cref="System.IO.IOException">An I/O error occured.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        public override String[] GetLogicalDrives()
        {
            try
            {
                return Directory.GetLogicalDrives();
            }
            catch (IOException)
            {
                throw new IOException("An I/O error occured.");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
            }
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
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// or
        /// The specified path is invalid.
        /// or
        /// The path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        public override String[] GetDirectories(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                return Directory.GetDirectories(path, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("The specified path is invalid.", "path");
            }
            catch (IOException)
            {
                throw new ArgumentException("The path is a file.", "path");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
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
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// or
        /// The specified path is invalid.
        /// or
        /// The path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        public override String[] GetFiles(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                return Directory.GetFiles(path, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("The specified path is invalid.", "path");
            }
            catch (IOException)
            {
                throw new ArgumentException("The path is a file.", "path");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
            }
        }

        /// <summary>
        /// Returns the file system entries located on the specified path.
        /// </summary>
        /// <param name="path">The path of the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="recursive">A value that specifies whether subdirectories are included in the search.</param>
        /// <returns>An array containing the full paths to all file system entries.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// or
        /// The specified path is invalid.
        /// or
        /// The path is a file.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission for the path.</exception>
        public override String[] GetFileSystemEntries(String path, String searchPattern, Boolean recursive)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                return Directory.GetFileSystemEntries(path, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The path contains only white space, or contains one or more invalid characters.", "path");
            }
            catch (PathTooLongException)
            {
                throw new ArgumentException("The path, file name, or both exceed the system-defined maximum length.", "path");
            }
            catch (DirectoryNotFoundException)
            {
                throw new ArgumentException("The specified path is invalid.", "path");
            }
            catch (IOException)
            {
                throw new ArgumentException("The path is a file.", "path");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("The caller does not have the required permission for the path.");
            }
        }

        #endregion
    }
}
