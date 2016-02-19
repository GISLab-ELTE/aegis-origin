/// <copyright file="FtpCreateFileOperation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    /// Represents an FTP file creation operation.
    /// </summary>
    class FtpCreateFileOperation : FtpFileSystemOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpCreateFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        public FtpCreateFileOperation(Uri path) : base(path)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpCreateFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        /// <param name="authentication">The file system authentication.</param>
        public FtpCreateFileOperation(Uri path, IFileSystemAuthentication authentication) : base(path, authentication)
        {
        }

        /// <summary>
        /// Sets the properties of the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void SetupRequest(System.Net.FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.UploadFile;
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
            Stream responseStream = response.GetResponseStream();
            Byte[] fileContents = new Byte[0];
            
            await responseStream.WriteAsync(fileContents, 0, fileContents.Length);
            
            StreamOperationResult result = new StreamOperationResult(responseStream);

            return result;
        }
    }
}
