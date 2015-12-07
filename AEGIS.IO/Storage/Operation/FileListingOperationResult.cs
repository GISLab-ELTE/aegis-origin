/// <copyright file="FileListingOperationResult.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Marcell Lipp</author>

using System.Collections.Generic;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    /// Represents a file system operation result containing a file system entry list.
    /// </summary>
    public class FileListingOperationResult : FileSystemOperationResult
    {
        /// <summary>
        /// Gets the list of file system entries.
        /// </summary>
        /// <value>The list of file system entries.</value>
        public IList<FileSystemEntry> Entries { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileListingOperationResult" /> class.
        /// </summary>
        public FileListingOperationResult(IList<FileSystemEntry> entries) : base(FileSystemOperationResultCode.Completed)
        {
            Entries = entries;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileListingOperationResult" /> class.
        /// </summary>
        /// <param name="code">The result code of the operation.</param>
        /// <param name="entries">The list of file system entries.</param>
        public FileListingOperationResult(FileSystemOperationResultCode code, IList<FileSystemEntry> entries)
            : base(code)
        {
            Entries = entries;
        }
    }
}
