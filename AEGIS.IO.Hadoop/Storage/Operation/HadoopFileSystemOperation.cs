/// <copyright file="HadoopFileSystemOperation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
            /// PUSH.
            /// </summary>
            Push,

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
        /// <value>The HDFS authentication.</value>
        public IHadoopFileSystemAuthentication Authentication { get; set; }

        /// <summary>
        /// Gets or sets the path of the operation.
        /// </summary>
        /// <value>The absolute path to the operation.</value>
        public String Path { get; set; }

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

        #endregion

        #region Constructors and destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation"/> class.
        /// </summary>
        protected HadoopFileSystemOperation()
        {
            _client = new HttpClient(new WebRequestHandler { AllowAutoRedirect = true }, true);
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
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        protected HadoopFileSystemOperation(HttpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client", "The client is null.");

            _client = client;
            _disposeClient = false;
            _disposed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
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
        protected HadoopFileSystemOperation(HttpClient client, String path, IHadoopFileSystemAuthentication authentication)
            : this(client)
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
        /// Executes the operation asyncronously.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="HadoopRemoteException">The remote address returned with an exception.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">The remote address returned with an invalid response.</exception>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public async Task<HadoopFileSystemOperationResult> ExecuteAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            HttpResponseMessage message = null;

            // execute operation

            String requestUri = String.Format("{0}?{1}&{2}", Path, OperationRequest, Authentication.Request);

            switch (RequestType)
            {
                case HttpRequestType.Put:
                    message = await _client.PutAsync(requestUri, null);
                    break;
                case HttpRequestType.Get:
                    message = await _client.GetAsync(requestUri);
                    break;
                case HttpRequestType.Delete:
                    message = await _client.DeleteAsync(requestUri);
                    break;
            }

            // parse the message
            JObject contentObject = JObject.Parse(await message.Content.ReadAsStringAsync());

            message.Dispose();

            // handle successful request
            if (message.IsSuccessStatusCode)
            {
                return CreateResult(contentObject);
            }

            // handle unsuccessful request
            switch (message.StatusCode)
            {
                case HttpStatusCode.BadRequest: // excepted error cases
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.InternalServerError:
                    throw CreateRemoteException(contentObject);
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
        /// Creates the result for the specified JSON object.
        /// </summary>
        /// <param name="obj">The content JSON object.</param>
        /// <returns>The produced operation result.</returns>
        protected abstract HadoopFileSystemOperationResult CreateResult(JObject obj);

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
        /// Creates the remote exeption for the specified JSON object.
        /// </summary>
        /// <param name="obj">The content JSON object.</param>
        /// <returns>The produced remote exception.</returns>
        private HadoopRemoteException CreateRemoteException(JObject obj)
        {
            return new HadoopRemoteException(obj.Value<String>("message"), obj.Value<String>("exception"), obj.Value<String>("javaClassName"));
        }

        #endregion
    }
}
