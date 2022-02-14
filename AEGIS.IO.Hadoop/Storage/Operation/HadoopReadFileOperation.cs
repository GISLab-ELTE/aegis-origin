/// <copyright file="HadoopReadFileOperation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.IO.Storage.Authentication;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    ///  Represents a Hadoop file system operation for reading files.
    /// </summary>
    public class HadoopReadFileOperation : HadoopFileSystemOperation
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The zero-based byte offset at which the reading from the file begins.</value>
        public Int64 Offset { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The number of bytes read from the file. If the length is zero, the entire content of the file will be read.</value>
        public Int64 Length { get; set; }

        #endregion

        #region Protected HadoopFileSystemOperation properties

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The HTTP type of the request used for execution.</value>
        protected override HttpRequestType RequestType
        {
            get { return HttpRequestType.Get; }
        }

        /// <summary>
        /// Gets the request of the operation.
        /// </summary>
        /// <value>The request of the operation.</value>
        protected override String OperationRequest
        {
            get
            {
                StringBuilder requestBuilder = new StringBuilder("op=OPEN");

                if (Offset > 0)
                    requestBuilder.Append("&offset=" + Offset);

                if (Length > 0)
                    requestBuilder.Append("&length=" + Length);

                return requestBuilder.ToString();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation"/> class.
        /// </summary>
        public HadoopReadFileOperation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopReadFileOperation(String path, IHadoopFileSystemAuthentication authentication)
            : base(path, authentication)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <param name="offset">The zero based byte offset in the file.</param>
        /// <param name="length">The number of bytes to be read.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The offset is less than 0.
        /// or
        /// The length is less than 0.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopReadFileOperation(String path, IHadoopFileSystemAuthentication authentication, Int64 offset, Int64 length) : base(path, authentication) 
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "The offset is less than 0.");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "The length is less than 0.");

            Offset = offset;
            Length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        public HadoopReadFileOperation(HttpClient client) : base(client, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The client is null.
        /// or
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopReadFileOperation(HttpClient client, String path, IHadoopFileSystemAuthentication authentication)
            : base(client, null, path, authentication)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation"/> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <param name="offset">The zero based byte offset in the file.</param>
        /// <param name="length">The number of bytes to be read.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The offset is less than 0.
        /// or
        /// The length is less than 0.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The client is null.
        /// or
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopReadFileOperation(HttpClient client, String path, IHadoopFileSystemAuthentication authentication, Int64 offset, Int64 length) : base(client, null, path, authentication)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "The offset is less than 0.");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "The length is less than 0.");

            Offset = offset;
            Length = length;
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
            return new HadoopFileStreamingOperationResult
            {
                Request = CompleteRequest, 
                FileStream = await content.ReadAsStreamAsync()
            };
        }

        #endregion
    }
}
