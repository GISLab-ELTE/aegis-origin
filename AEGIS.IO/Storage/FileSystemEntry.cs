// <copyright file="FileSystemEntry.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.IO.Storage
{
    /// <summary>
    /// Represents a file system entry.
    /// </summary>
    public class FileSystemEntry
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the path of the entry.
        /// </summary>
        /// <value>The full path of the entry.</value>

        public String Path { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        /// <value>The name of the entry.</value>
        public String Name { get; set; }

        /// <summary>
        /// Gets the full path of the parent directory.
        /// </summary>
        /// <value>The full path of the parent directory.</value>
        public String Parent { get { return Path.Substring(0, Path.LastIndexOf(Name)); } }

        /// <summary>
        /// Gets or sets the type of the entry.
        /// </summary>
        /// <value>The type of the entry.</value>
        public FileSystemEntryType Type { get; set; }

        /// <summary>
        /// Gets or sets the time of creation.
        /// </summary>
        /// <value>The time of creation. If the query is not supported by the file system, the minimum <see cref="DateTime" /> value is returned.</value>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the time of the last access.
        /// </summary>
        /// <value>The time of the last access. If the query is not supported by the file system, the minimum <see cref="DateTime" /> value is returned.</value>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// Gets or sets the time of the last modification.
        /// </summary>
        /// <value>The time of the last modification. If the query is not supported by the file system, the minimum <see cref="DateTime" /> value is returned.</value>
        public DateTime LastModificationTime { get; set; }

        /// <summary>
        /// Gets or sets the length of the file.
        /// </summary>
        /// <value>The length of the file in bytes. The value is <c>0</c> for directories.</value>
        public Int64 Length { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEntry" /> class.
        /// </summary>
        public FileSystemEntry() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEntry" /> class.
        /// </summary>
        /// <param name="path">The path of the entry.</param>
        /// <param name="name">The name of the entry.</param>
        /// <param name="entryType">The type of the entry.</param>
        /// <param name="creationTime">The time of creation.</param>
        /// <param name="lastAccessTime">The time of the last access.</param>
        /// <param name="lastModificationTime">The time of the last modification.</param>
        /// <param name="length">The length of the file.</param>
        public FileSystemEntry(String path, String name, FileSystemEntryType entryType, DateTime creationTime, DateTime lastAccessTime, DateTime lastModificationTime, Int64 length)
        {
            Path = path;
            Name = name;
            Type = entryType;
            CreationTime = creationTime;
            LastAccessTime = lastAccessTime;
            LastModificationTime = lastModificationTime;
            Length = length;
        }

        #endregion
    }
}
