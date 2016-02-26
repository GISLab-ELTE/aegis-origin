/// <copyright file="HadoopFileSystemOperation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.IO.Storage.Authentication;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    /// Represents a Hadoop file system operation.
    /// </summary>
    public abstract class HadoopFileSystemOperation : IDisposable
    {
        #region Protected types

        /// <summary>
        /// Defines the possible HTTP request types.
        /// </summary>
        protected enum HttpRequestType 
        {
            /// <summary>
            /// PUT.
            /// </summary>
            Put,

            /// <summary>
            /// POST.
            /// </summary>
            Post,

            /// <summary>
            /// GET.
            /// </summary>
            Get,

            /// <summary>
            /// DELETE.
            /// </summary>
            Delete
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The HTTP content.
        /// </summary>
        private HttpContent _content;

        /// <summary>
        /// The HTTP client performing the operations.
        /// </summary>
        private HttpClient _client;

        /// <summary>
        /// A value indicating whether this instance is disposed.
        /// </summary>
        private Boolean _disposed;

        /// <summary>
        /// A value indicating whether the HTTP client should be disposed.
        /// </summary>
        private Boolean _disposeClient;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the timeout of the client.
        /// </summary>
        /// <value>The timeout applied to the operation.</value>
        public TimeSpan Timeout { get { return _client.Timeout; } set { _client.Timeout = value; } }

        /// <summary>
        /// Gets or sets the authentication used for the operation.
        /// </summary>
        /// <value>The Hadoop authentication.</value>
        public IHadoopFileSystemAuthentication Authentication { get; set; }

        /// <summary>
        /// Gets or sets the path of the operation.
        /// </summary>
        /// <value>The absolute path to the operation.</value>
        public String Path { get; private set; }

        #endregion

        #region Protected properties

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The HTTP type of the request used for execution.</value>
        protected abstract HttpRequestType RequestType { get; }

        /// <summary>
        /// Gets the request of the operation.
        /// </summary>
        /// <value>The request of the operation.</value>
        protected abstract String OperationRequest { get; }

        /// <summary>
        /// Gets the complete request of the operation.
        /// </summary>
        /// <value>The complete request including the path, operation and authentication.</value>
        protected String CompleteRequest
        {
            get
            {
                switch (Authentication.AutenticationType)
                {
                    case FileSystemAuthenticationType.UserCredentials:
                        return String.Format("{0}?{1}&{2}", Path, OperationRequest, Authentication.Request);
                    default:
                        return String.Format("{0}?{1}", Path, OperationRequest);
                }
            }
        }

        #endregion

        #region Constructors and destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation"/> class.
        /// </summary>
        protected HadoopFileSystemOperation()
        {
            _client = new HttpClient(new WebRequestHandler { AllowAutoRedirect = false }, true);
            _disposeClient = false;
            _disposed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        protected HadoopFileSystemOperation(String path, IHadoopFileSystemAuthentication authentication)
            : this()
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");
            if (authentication == null)
                throw new ArgumentNullException("authentication", "The authentication is null.");

            Path = path;
            Authentication = authentication;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation"/> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="content">The HTTP content.</param>
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        protected HadoopFileSystemOperation(HttpClient client, HttpContent content)
        {
            if (client == null)
                throw new ArgumentNullException("client", "The client is null.");

            _client = client;
            _content = content; 
            _disposeClient = false;
            _disposed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="content">The HTTP content.</param>
        /// <param name="path">The path of the operation.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The client is null.
        /// or
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        protected HadoopFileSystemOperation(HttpClient client, HttpContent content, String path, IHadoopFileSystemAuthentication authentication)
            : this(client, content)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");
            if (authentication == null)
                throw new ArgumentNullException("authentication", "The authentication is null.");

            Path = path;
            Authentication = authentication;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="HadoopFileSystemOperation"/> class.
        /// </summary>
        ~HadoopFileSystemOperation()
        {
            Dispose(false);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes the operation asynchronously.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">The remote address returned with an invalid response.</exception>
        /// <exception cref="HadoopRemoteException">The remote address returned with an exception.</exception>
        public async Task<HadoopFileSystemOperationResult> ExecuteAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            HttpResponseMessage message = null;

            // execute operation (without content)
            switch (RequestType)
            {
                case HttpRequestType.Put:
                    message = await _client.PutAsync(CompleteRequest, null);
                    break;
                case HttpRequestType.Post:
                    message = await _client.PostAsync(CompleteRequest, null);
                    break;
                case HttpRequestType.Get:
                    message = await _client.GetAsync(CompleteRequest, HttpCompletionOption.ResponseHeadersRead);
                    break;
                case HttpRequestType.Delete:
                    message = await _client.DeleteAsync(CompleteRequest);
                    break;
            }

            // some operations result in redirect to data node
            if (message.StatusCode == HttpStatusCode.TemporaryRedirect)
            {
                // execute the operation on the data node (with content)
                switch (RequestType)
                {
                    case HttpRequestType.Put:
                        message = await _client.PutAsync(message.Headers.Location, _content);
                        break;
                    case HttpRequestType.Post:
                        message = await _client.PutAsync(message.Headers.Location, _content);
                        break;
                    case HttpRequestType.Get:
                        message = await _client.GetAsync(message.Headers.Location, HttpCompletionOption.ResponseHeadersRead);
                        break;
                    case HttpRequestType.Delete:
                        message = await _client.DeleteAsync(message.Headers.Location);
                        break;
                }
            }

            // handle successful request
            if (message.IsSuccessStatusCode)
            {
                return await CreateResultAsync(message.Content);
            }

            // handle unsuccessful request
            switch (message.StatusCode)
            {
                case HttpStatusCode.BadRequest: // excepted error cases
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.InternalServerError:
                    throw await CreateRemoteExceptionAsync(message.Content);
                default: // unexpected error cases
                    throw new HttpRequestException("The remote address returned with an invalid response.");
            }
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Creates the result for the specified content asynchronously.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        /// <returns>The produced operation result.</returns>
        protected abstract Task<HadoopFileSystemOperationResult> CreateResultAsync(HttpContent content);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected virtual void Dispose(Boolean disposing)
        { 
            _disposed = true;

            if (disposing)
            {
                if (_disposeClient)
                    _client.Dispose();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates the remote exception for the specified content asynchronously.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        /// <returns>The produced remote exception.</returns>
        private async Task<HadoopRemoteException> CreateRemoteExceptionAsync(HttpContent content)
        {
            JObject contentObject = JObject.Parse(await content.ReadAsStringAsync()).Value<JObject>("RemoteException");

            return new HadoopRemoteException(contentObject.Value<String>("message"), contentObject.Value<String>("exception"), contentObject.Value<String>("javaClassName"));
        }

        #endregion
    }
}
