/// <copyright file="HadoopRenameOperation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
using System.Net.Http;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    ///  Represents a Hadoop file system operation for renaming.
    /// </summary>
    public class HadoopRenameOperation : HadoopFileSystemOperation
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        /// <value>The destination path.</value>
        public String Destination { get; set; }

        #endregion

        #region Protected HadoopFileSystemOperation properties

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The HTTP type of the request used for execution.</value>
        protected override HttpRequestType RequestType
        {
            get { return HttpRequestType.Put; }
        }

        /// <summary>
        /// Gets the request of the operation.
        /// </summary>
        /// <value>The request of the operation.</value>
        protected override String OperationRequest
        {
            get { return "op=RENAME&destination=" + Destination; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRenameOperation"/> class.
        /// </summary>
        public HadoopRenameOperation() {  }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRenameOperation"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <param name="recursive">A value indicating whether the deletion is recursive.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The destination path is empty.
        /// </exception>
        public HadoopRenameOperation(String path, IHadoopFileSystemAuthentication authentication, String destination)
            : base(path, authentication) 
        {
            if (destination == null)
                throw new ArgumentNullException("destination", "The destination path is null.");
            if (String.IsNullOrEmpty(destination))
                throw new ArgumentException("The destination path is empty.", "destination");

            Destination = destination; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRenameOperation"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        public HadoopRenameOperation(HttpClient client) : base(client, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRenameOperation"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <param name="recursive">A value indicating whether the deletion is recursive.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The client is null.
        /// or
        /// The path is null.
        /// or
        /// The authentication is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The destination path is empty.
        /// </exception>
        public HadoopRenameOperation(HttpClient client, String path, IHadoopFileSystemAuthentication authentication, String destination)
            : base(client, null, path, authentication)
        {
            if (destination == null)
                throw new ArgumentNullException("destination", "The destination path is null.");
            if (String.IsNullOrEmpty(destination))
                throw new ArgumentException("The destination path is empty.", "destination");

            Destination = destination;
        }

        #endregion

        #region Protected HadoopFileSystemOperation methods

        /// <summary>
        /// Creates the result for the specified content asynchronously.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        /// <returns>The produced operation result.</returns>
        protected async override Task<HadoopFileSystemOperationResult> CreateResultAsync(HttpContent content)
        {
            return new HadoopBooleanOperationResult
            {
                Request = CompleteRequest,
                Success = JObject.Parse(await content.ReadAsStringAsync()).Value<Boolean>("boolean")
            };
        }

        #endregion
    }
}
