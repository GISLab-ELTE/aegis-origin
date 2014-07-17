/// <copyright file="HadoopFileStreamingOperationResult.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.IO;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    ///  Represents a Hadoop file system operation result containing a file stream.
    /// </summary>
    public class HadoopFileStreamingOperationResult : HadoopFileSystemOperationResult
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the resulting file stream.
        /// </summary>
        /// <value>The resulting file stream.</value>
        public Stream FileStream { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileStreamingOperationResult"/> class.
        /// </summary>
        public HadoopFileStreamingOperationResult() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileStreamingOperationResult" /> class.
        /// </summary>
        /// <param name="request">The request of the operation.</param>
        /// <param name="fileStream">The resulting file stream.</param>
        public HadoopFileStreamingOperationResult(String request, Stream fileStream) : base(request) { FileStream = fileStream; }

        #endregion
    }
}
