/// <copyright file="FtpFileSystemOperation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.IO.Storage.Authentication;
using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    /// Represents an FTP file system operation.
    /// </summary>
    public abstract class FtpFileSystemOperation
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the timeout of the client.
        /// </summary>
        /// <value>The timeout applied to the operation.</value>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets the authentication used for the operation.
        /// </summary>
        /// <value>The file system authentication.</value>
        public IFileSystemAuthentication Authentication { get; set; }

        /// <summary>
        /// Gets or sets the path of the operation.
        /// </summary>
        /// <value>The absolute path to the operation.</value>
        public Uri Path { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystemOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">The specified path scheme is not supported.</exception>
        protected FtpFileSystemOperation(Uri path)
            : this(path, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystemOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        /// <param name="authentication">The file system authentication.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">The specified path scheme is not supported.</exception>
        protected FtpFileSystemOperation(Uri path, IFileSystemAuthentication authentication)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (path.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The specified path scheme is not supported.", "path");

            Path = path;
            Authentication = authentication;
            Timeout = new TimeSpan(0,1,0);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        public FileSystemOperationResult Execute()
        {
            return ExecuteAsync().Result;
        }

        /// <summary>
        /// Executes the operation asynchronously.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        public async Task<FileSystemOperationResult> ExecuteAsync()
        {
            try
            {
                // create request
                FtpWebRequest request = WebRequest.Create(Path) as FtpWebRequest;
                request.Timeout = Convert.ToInt32(Timeout.TotalMilliseconds);
               
                // specify authentication
                if (Authentication is UsernamePasswordFileSystemAuthentication)
                {
                    request.Credentials = new NetworkCredential(((Authentication as UsernamePasswordFileSystemAuthentication).Username.Normalize()),
                                                                (Authentication as UsernamePasswordFileSystemAuthentication).Password.ToString().Normalize());
                }


                SetupRequest(request);

                using (FtpWebResponse response = (await request.GetResponseAsync()) as FtpWebResponse)
                {
                    switch (response.StatusCode)
                    {
                        case FtpStatusCode.CommandOK: // success
                        case FtpStatusCode.FileActionOK:
                        case FtpStatusCode.OpeningData:
                        case FtpStatusCode.DataAlreadyOpen:
                        case FtpStatusCode.PathnameCreated:
                            return await CreateResultAsync(response);
                        case FtpStatusCode.ActionNotTakenFileUnavailable: // path does not exist
                        case FtpStatusCode.ActionNotTakenFileUnavailableOrBusy:
                            throw new FileSystemOperationException(response.StatusDescription, Path, FileSystemOperationResultCode.UnavailablePath);
                        case FtpStatusCode.ActionNotTakenFilenameNotAllowed: // invalid format
                        case FtpStatusCode.ActionNotTakenInsufficientSpace:
                        case FtpStatusCode.ArgumentSyntaxError:
                        case FtpStatusCode.BadCommandSequence:
                        case FtpStatusCode.CommandSyntaxError:
                            throw new FileSystemOperationException(response.StatusDescription, Path, FileSystemOperationResultCode.InvalidPath);
                        case FtpStatusCode.AccountNeeded: // security error
                        case FtpStatusCode.NeedLoginAccount:
                        case FtpStatusCode.NotLoggedIn:
                            throw new FileSystemOperationException(response.StatusDescription, Path, FileSystemOperationResultCode.AccessDenied);
                        case FtpStatusCode.CommandExtraneous: // not supported
                        case FtpStatusCode.CommandNotImplemented:
                            throw new FileSystemOperationException(response.StatusDescription, Path, FileSystemOperationResultCode.UnsupportedOperation);
                        case FtpStatusCode.CantOpenData: // connection error
                        case FtpStatusCode.FileActionAborted:
                        case FtpStatusCode.ServiceNotAvailable:
                        case FtpStatusCode.ServiceTemporarilyNotAvailable:
                            throw new FileSystemOperationException(response.StatusDescription, Path, FileSystemOperationResultCode.ConnectionNotAvailable);
                        default:
                            throw new FileSystemOperationException(response.StatusDescription, Path, FileSystemOperationResultCode.Unknown);
                    }
                }
            }
            catch (FileSystemOperationException)
            {
                throw;
            }
            catch (WebException ex)
            {
                throw new FileSystemOperationException("Cannot connect to the FTP server.", Path, FileSystemOperationResultCode.ConnectionNotAvailable, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new FileSystemOperationException("The specified operation is not supported.", Path, FileSystemOperationResultCode.UnsupportedOperation, ex);
            }
            catch (SecurityException ex)
            {
                throw new FileSystemOperationException("Access denied to the specified path.", Path, FileSystemOperationResultCode.AccessDenied, ex);
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Sets the properties of the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected abstract void SetupRequest(FtpWebRequest request);

        /// <summary>
        /// Creates the result of the operation.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>The result of the operation.</returns>
        protected virtual async Task<FileSystemOperationResult> CreateResultAsync(FtpWebResponse response)
        {
            return await Task.Run(() => new FileSystemOperationResult(FileSystemOperationResultCode.Completed));
        }

        #endregion
    }
}
