/// <copyright file="FtpFileListingOperation.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    /// Represents an FTP file system operation returning a file system entry list.
    /// </summary>
    public class FtpFileListingOperation : FtpFileSystemOperation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileListingOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">The specified path scheme is not supported.</exception>
        public FtpFileListingOperation(Uri path)
            : base(path)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileListingOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        /// <param name="authentication">The file system authentication.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">The specified path scheme is not supported.</exception>
        public FtpFileListingOperation(Uri path, IFileSystemAuthentication authentication)
            : base(path, authentication)
        { }

        #endregion

        #region Protected FtpFileSystemOperation methods

        /// <summary>
        /// Sets the properties of the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void SetupRequest(FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        }

        /// <summary>
        /// Creates the result of the operation.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        protected override async Task<FileSystemOperationResult> CreateResultAsync(FtpWebResponse response)
        {
            List<FileSystemEntry> entryList = new List<FileSystemEntry>();

            Stream responseStream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream))
            {
                while (!reader.EndOfStream)
                {
                    String line = await reader.ReadLineAsync();
                    String[] lineContent = line.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);

                    entryList.Add(new FileSystemEntry
                    {
                        Path = Path.LocalPath + '/' + lineContent[3],
                        Name = lineContent[3],
                        Type = lineContent[2].Equals("<DIR>") ? FileSystemEntryType.Directory : FileSystemEntryType.File,
                        CreationTime = DateTime.MinValue,
                        LastAccessTime = DateTime.MinValue,
                        LastModificationTime = DateTime.Parse(lineContent[0] + ' ' + lineContent[1]),
                        Length = lineContent[2].Equals("<DIR>") ? 0 : Int64.Parse(lineContent[2])
                    });
                }
            }

            return new FileListingOperationResult(FileSystemOperationResultCode.Completed, entryList);
        }

        #endregion
    }
}
